using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000400 RID: 1024
	public class CraftingCampaignBehavior : CampaignBehaviorBase, ICraftingCampaignBehavior, ICampaignBehavior, INonReadyObjectHandler
	{
		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06004007 RID: 16391 RVA: 0x001172C7 File Offset: 0x001154C7
		public IReadOnlyDictionary<Town, CraftingCampaignBehavior.CraftingOrderSlots> CraftingOrders
		{
			get
			{
				return this._craftingOrders;
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06004008 RID: 16392 RVA: 0x001172D0 File Offset: 0x001154D0
		public IReadOnlyCollection<WeaponDesign> CraftingHistory
		{
			get
			{
				MBList<WeaponDesign> mblist = new MBList<WeaponDesign>();
				foreach (ItemObject itemObject in this._cratingItemsHistory)
				{
					WeaponDesign weaponDesign = itemObject.WeaponDesign;
					mblist.Add(new WeaponDesign(weaponDesign.Template, weaponDesign.WeaponName, weaponDesign.UsedPieces, weaponDesign.HashedCode));
				}
				return mblist;
			}
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x0011734C File Offset: 0x0011554C
		private string GetNextCraftedItemId()
		{
			string text = string.Format("crafted_item_{0}", this._craftedItemCount);
			this._craftedItemCount++;
			return text;
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x00117371 File Offset: 0x00115571
		private string GetNextTownOrderId()
		{
			string text = string.Format("town_order_{0}", this._townOrderCount);
			this._townOrderCount++;
			return text;
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x00117398 File Offset: 0x00115598
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Hero>("_activeCraftingHero", ref this._activeCraftingHero);
			dataStore.SyncData<Dictionary<ItemObject, CraftingCampaignBehavior.CraftedItemInitializationData>>("_craftedItemDictionary", ref this._craftedItemDictionary);
			dataStore.SyncData<Dictionary<Hero, CraftingCampaignBehavior.HeroCraftingRecord>>("_heroCraftingRecordsNew", ref this._heroCraftingRecords);
			dataStore.SyncData<Dictionary<Town, CraftingCampaignBehavior.CraftingOrderSlots>>("_craftingOrders", ref this._craftingOrders);
			dataStore.SyncData<List<ItemObject>>("_cratingItemsHistory", ref this._cratingItemsHistory);
			dataStore.SyncData<Dictionary<CraftingTemplate, List<CraftingPiece>>>("_openedPartsDictionary", ref this._openedPartsDictionary);
			dataStore.SyncData<Dictionary<CraftingTemplate, float>>("_openNewPartXpDictionary", ref this._openNewPartXpDictionary);
			dataStore.SyncData<int>("_townOrderCount", ref this._townOrderCount);
			dataStore.SyncData<int>("_craftedItemCount", ref this._craftedItemCount);
			if (dataStore.IsLoading && MBSaveLoad.IsUpdatingGameVersion)
			{
				if (MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("e1.8.0", 0))
				{
					List<CraftingPiece> list = new List<CraftingPiece>();
					dataStore.SyncData<List<CraftingPiece>>("_openedParts", ref list);
					if (list != null)
					{
						this._openedPartsDictionary = new Dictionary<CraftingTemplate, List<CraftingPiece>>();
						foreach (CraftingTemplate craftingTemplate in CraftingTemplate.All)
						{
							this._openedPartsDictionary.Add(craftingTemplate, new List<CraftingPiece>());
							foreach (CraftingPiece craftingPiece in list)
							{
								if (craftingTemplate.Pieces.Contains(craftingPiece) && !this._openedPartsDictionary[craftingTemplate].Contains(craftingPiece))
								{
									this._openedPartsDictionary[craftingTemplate].Add(craftingPiece);
								}
							}
						}
					}
				}
				if (MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.3.2", 0))
				{
					List<ItemObject> list2 = new List<ItemObject>();
					for (int i = 0; i < this._craftedItemDictionary.Count; i++)
					{
						KeyValuePair<ItemObject, CraftingCampaignBehavior.CraftedItemInitializationData> keyValuePair = this._craftedItemDictionary.ElementAt<KeyValuePair<ItemObject, CraftingCampaignBehavior.CraftedItemInitializationData>>(i);
						if (keyValuePair.Value.CraftedData.Template.IsReady)
						{
							bool flag = true;
							foreach (PieceData pieceData in keyValuePair.Value.CraftedData.Template.BuildOrders)
							{
								bool flag2 = false;
								foreach (WeaponDesignElement weaponDesignElement in keyValuePair.Value.CraftedData.UsedPieces)
								{
									if (pieceData.PieceType == weaponDesignElement.CraftingPiece.PieceType && weaponDesignElement.CraftingPiece.IsValid)
									{
										flag2 = true;
									}
								}
								if (!flag2)
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								string nextCraftedItemId = this.GetNextCraftedItemId();
								keyValuePair.Key.StringId = nextCraftedItemId;
								WeaponDesignElement[] array = new WeaponDesignElement[keyValuePair.Value.CraftedData.UsedPieces.Length];
								for (int l = 0; l < keyValuePair.Value.CraftedData.UsedPieces.Length; l++)
								{
									array[l] = keyValuePair.Value.CraftedData.UsedPieces[l].GetCopy();
								}
								WeaponDesign weaponDesign = new WeaponDesign(keyValuePair.Value.CraftedData.Template, keyValuePair.Value.CraftedData.WeaponName, array, nextCraftedItemId);
								this._craftedItemDictionary[keyValuePair.Key] = new CraftingCampaignBehavior.CraftedItemInitializationData(weaponDesign, keyValuePair.Value.ItemName, keyValuePair.Value.Culture);
							}
							else
							{
								list2.Add(keyValuePair.Key);
							}
						}
						else
						{
							list2.Add(keyValuePair.Key);
						}
					}
					foreach (ItemObject itemObject in list2)
					{
						this._craftedItemDictionary.Remove(itemObject);
					}
					List<WeaponDesign> list3 = new List<WeaponDesign>();
					dataStore.SyncData<List<WeaponDesign>>("_craftingHistory", ref list3);
					foreach (WeaponDesign weaponDesign2 in list3)
					{
						ItemObject itemObject2 = null;
						foreach (KeyValuePair<ItemObject, CraftingCampaignBehavior.CraftedItemInitializationData> keyValuePair2 in this._craftedItemDictionary)
						{
							WeaponDesign craftedData = keyValuePair2.Value.CraftedData;
							if (!this._cratingItemsHistory.Contains(keyValuePair2.Key) && weaponDesign2.Template == craftedData.Template)
							{
								bool flag3 = true;
								int num = 0;
								while (num < weaponDesign2.UsedPieces.Length && flag3)
								{
									WeaponDesignElement weaponDesignElement2 = weaponDesign2.UsedPieces[num];
									string text;
									if (weaponDesignElement2 == null)
									{
										text = null;
									}
									else
									{
										CraftingPiece craftingPiece2 = weaponDesignElement2.CraftingPiece;
										text = ((craftingPiece2 != null) ? craftingPiece2.StringId : null);
									}
									WeaponDesignElement weaponDesignElement3 = craftedData.UsedPieces[num];
									string text2;
									if (weaponDesignElement3 == null)
									{
										text2 = null;
									}
									else
									{
										CraftingPiece craftingPiece3 = weaponDesignElement3.CraftingPiece;
										text2 = ((craftingPiece3 != null) ? craftingPiece3.StringId : null);
									}
									if (text != text2)
									{
										flag3 = false;
									}
									num++;
								}
								if (flag3)
								{
									itemObject2 = keyValuePair2.Key;
									break;
								}
							}
						}
						if (itemObject2 != null)
						{
							this._cratingItemsHistory.Add(itemObject2);
						}
					}
					for (int m = 0; m < this._craftingOrders.Count; m++)
					{
						KeyValuePair<Town, CraftingCampaignBehavior.CraftingOrderSlots> keyValuePair3 = this._craftingOrders.ElementAt<KeyValuePair<Town, CraftingCampaignBehavior.CraftingOrderSlots>>(m);
						for (int n = 0; n < keyValuePair3.Value.Slots.Count<CraftingOrder>(); n++)
						{
							string nextTownOrderId = this.GetNextTownOrderId();
							CraftingOrder craftingOrder = keyValuePair3.Value.Slots[n];
							if (craftingOrder != null)
							{
								WeaponDesign weaponDesignTemplate = craftingOrder.WeaponDesignTemplate;
								WeaponDesign weaponDesign3 = new WeaponDesign(weaponDesignTemplate.Template, weaponDesignTemplate.WeaponName, weaponDesignTemplate.UsedPieces, nextTownOrderId);
								CraftingTemplate templateFromId = CraftingTemplate.GetTemplateFromId(weaponDesignTemplate.Template.StringId);
								CraftingOrder craftingOrder2 = new CraftingOrder(craftingOrder.OrderOwner, (float)craftingOrder.DifficultyLevel, weaponDesign3, templateFromId, craftingOrder.DifficultyLevel, nextTownOrderId);
								keyValuePair3.Value.Slots[n] = craftingOrder2;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x00117A14 File Offset: 0x00115C14
		void INonReadyObjectHandler.OnBeforeNonReadyObjectsDeleted()
		{
			if (this._craftedItemDictionary.Count > 0)
			{
				this.InitializeCraftedItemData();
			}
			foreach (KeyValuePair<Town, CraftingCampaignBehavior.CraftingOrderSlots> keyValuePair in this.CraftingOrders)
			{
				foreach (CraftingOrder craftingOrder in keyValuePair.Value.Slots)
				{
					if (craftingOrder != null && !craftingOrder.IsPreCraftedWeaponDesignValid())
					{
						keyValuePair.Value.RemoveTownOrder(craftingOrder);
					}
					else if (craftingOrder != null)
					{
						craftingOrder.InitializeCraftingOrderOnLoad();
					}
				}
				List<CraftingOrder> list = new List<CraftingOrder>();
				foreach (CraftingOrder craftingOrder2 in keyValuePair.Value.CustomOrders)
				{
					if (!craftingOrder2.IsPreCraftedWeaponDesignValid())
					{
						list.Add(craftingOrder2);
					}
					else
					{
						craftingOrder2.InitializeCraftingOrderOnLoad();
					}
				}
				foreach (CraftingOrder craftingOrder3 in list)
				{
					keyValuePair.Value.RemoveCustomOrder(craftingOrder3);
				}
			}
			for (int j = this._cratingItemsHistory.Count - 1; j >= 0; j--)
			{
				ItemObject itemObject = this._cratingItemsHistory[j];
				if (itemObject == DefaultItems.Trash || itemObject == null)
				{
					this._cratingItemsHistory.RemoveAt(j);
				}
			}
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x00117BB0 File Offset: 0x00115DB0
		private void InitializeCraftedItemData()
		{
			for (int i = 0; i < this._craftedItemDictionary.Count; i++)
			{
				KeyValuePair<ItemObject, CraftingCampaignBehavior.CraftedItemInitializationData> keyValuePair = this._craftedItemDictionary.ElementAt<KeyValuePair<ItemObject, CraftingCampaignBehavior.CraftedItemInitializationData>>(i);
				ItemObject key = keyValuePair.Key;
				WeaponDesignElement[] array = new WeaponDesignElement[keyValuePair.Value.CraftedData.UsedPieces.Length];
				for (int j = 0; j < keyValuePair.Value.CraftedData.UsedPieces.Length; j++)
				{
					array[j] = keyValuePair.Value.CraftedData.UsedPieces[j].GetCopy();
				}
				WeaponDesign weaponDesign = new WeaponDesign(keyValuePair.Value.CraftedData.Template, keyValuePair.Value.CraftedData.WeaponName, array, key.StringId);
				this._craftedItemDictionary[key] = new CraftingCampaignBehavior.CraftedItemInitializationData(weaponDesign, keyValuePair.Value.ItemName, keyValuePair.Value.Culture);
			}
			foreach (KeyValuePair<ItemObject, CraftingCampaignBehavior.CraftedItemInitializationData> keyValuePair2 in this._craftedItemDictionary)
			{
				ItemObject itemObject = Crafting.InitializePreCraftedWeaponOnLoad(keyValuePair2.Key, keyValuePair2.Value.CraftedData, keyValuePair2.Value.ItemName, keyValuePair2.Value.Culture);
				if (itemObject == DefaultItems.Trash || itemObject == null)
				{
					if (MBObjectManager.Instance.GetObject(keyValuePair2.Key.Id) != null)
					{
						MBObjectManager.Instance.UnregisterObject(keyValuePair2.Key);
					}
				}
				else
				{
					ItemObject.InitAsPlayerCraftedItem(ref itemObject);
					itemObject.IsReady = true;
				}
			}
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x00117D64 File Offset: 0x00115F64
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnNewItemCraftedEvent.AddNonSerializedListener(this, new Action<ItemObject, ItemModifier, bool>(this.OnNewItemCrafted));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x00117E2C File Offset: 0x0011602C
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			this.InitializeLists();
			MBList<Hero> mblist = new MBList<Hero>();
			foreach (Town town in Town.AllTowns)
			{
				Settlement settlement = town.Settlement;
				mblist.AddRange(settlement.HeroesWithoutParty);
				foreach (MobileParty mobileParty in settlement.Parties)
				{
					if (mobileParty.LeaderHero != null && !mobileParty.IsMainParty)
					{
						mblist.Add(mobileParty.LeaderHero);
					}
				}
				if (mblist.Count > 0)
				{
					for (int i = 0; i < 6; i++)
					{
						if (this.CraftingOrders[settlement.Town].GetAvailableSlot() > -1)
						{
							this.CreateTownOrder(mblist.GetRandomElement<Hero>(), i);
						}
					}
				}
				mblist.Clear();
			}
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x00117F3C File Offset: 0x0011613C
		private void DailyTickSettlement(Settlement settlement)
		{
			if (settlement.IsTown && this.CraftingOrders[settlement.Town].IsThereAvailableSlot())
			{
				List<Hero> list = new List<Hero>(settlement.HeroesWithoutParty);
				foreach (MobileParty mobileParty in settlement.Parties)
				{
					if (mobileParty.LeaderHero != null && !mobileParty.IsMainParty)
					{
						list.Add(mobileParty.LeaderHero);
					}
				}
				foreach (Hero hero in list)
				{
					if (hero != Hero.MainHero && MBRandom.RandomFloat <= 0.05f)
					{
						int availableSlot = this.CraftingOrders[settlement.Town].GetAvailableSlot();
						if (availableSlot <= -1)
						{
							break;
						}
						this.CreateTownOrder(hero, availableSlot);
					}
				}
			}
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x0011804C File Offset: 0x0011624C
		private void DailyTick()
		{
			foreach (KeyValuePair<Town, CraftingCampaignBehavior.CraftingOrderSlots> keyValuePair in this.CraftingOrders)
			{
				foreach (CraftingOrder craftingOrder in keyValuePair.Value.Slots)
				{
					if (craftingOrder != null && MBRandom.RandomFloat <= 0.05f)
					{
						this.ReplaceCraftingOrder(keyValuePair.Key, craftingOrder);
					}
				}
			}
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x001180D4 File Offset: 0x001162D4
		private void HourlyTick()
		{
			foreach (KeyValuePair<Hero, CraftingCampaignBehavior.HeroCraftingRecord> keyValuePair in this._heroCraftingRecords)
			{
				if (keyValuePair.Key.CurrentSettlement != null)
				{
					int maxHeroCraftingStamina = this.GetMaxHeroCraftingStamina(keyValuePair.Key);
					if (keyValuePair.Value.CraftingStamina < maxHeroCraftingStamina)
					{
						keyValuePair.Value.CraftingStamina = MathF.Min(maxHeroCraftingStamina, keyValuePair.Value.CraftingStamina + CraftingCampaignBehavior.GetStaminaHourlyRecoveryRate(keyValuePair.Key));
					}
				}
			}
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x00118178 File Offset: 0x00116378
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			this.RemoveOrdersOfHeroWithoutCompletionIfExists(victim);
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x00118184 File Offset: 0x00116384
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeLists();
			foreach (KeyValuePair<Town, CraftingCampaignBehavior.CraftingOrderSlots> keyValuePair in this._craftingOrders)
			{
				for (int i = 0; i < 6; i++)
				{
					CraftingOrder craftingOrder = keyValuePair.Value.Slots[i];
					if (craftingOrder != null && (craftingOrder.PreCraftedWeaponDesignItem == DefaultItems.Trash || craftingOrder.PreCraftedWeaponDesignItem == null || !craftingOrder.PreCraftedWeaponDesignItem.IsReady))
					{
						this.CancelOrder(keyValuePair.Key, craftingOrder);
					}
				}
			}
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x00118224 File Offset: 0x00116424
		private static int GetStaminaHourlyRecoveryRate(Hero hero)
		{
			int num = 5 + MathF.Round((float)hero.GetSkillValue(DefaultSkills.Crafting) * 0.025f);
			if (hero.GetPerkValue(DefaultPerks.Athletics.Stamina))
			{
				num += MathF.Round((float)num * DefaultPerks.Athletics.Stamina.PrimaryBonus);
			}
			return num;
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x00118270 File Offset: 0x00116470
		private void OnNewItemCrafted(ItemObject itemObject, ItemModifier overriddenItemModifier, bool isCraftingOrderItem)
		{
			if (!this._craftedItemDictionary.ContainsKey(itemObject))
			{
				CultureObject @object = MBObjectManager.Instance.GetObject<CultureObject>(itemObject.Culture.StringId);
				CraftingCampaignBehavior.CraftedItemInitializationData craftedItemInitializationData = new CraftingCampaignBehavior.CraftedItemInitializationData(itemObject.WeaponDesign, itemObject.Name, @object);
				this._craftedItemDictionary.Add(itemObject, craftedItemInitializationData);
			}
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x001182C4 File Offset: 0x001164C4
		private void AddResearchPoints(CraftingTemplate craftingTemplate, int researchPoints)
		{
			Dictionary<CraftingTemplate, float> dictionary = this._openNewPartXpDictionary;
			CraftingTemplate craftingTemplate2 = craftingTemplate;
			dictionary[craftingTemplate2] += (float)researchPoints;
			int count = craftingTemplate.Pieces.Count;
			int num = craftingTemplate.Pieces.Count<CraftingPiece>((CraftingPiece x) => this.IsOpened(x, craftingTemplate));
			float num2 = Campaign.Current.Models.SmithingModel.ResearchPointsNeedForNewPart(count, num);
			do
			{
				if (this._openNewPartXpDictionary[craftingTemplate] > num2)
				{
					dictionary = this._openNewPartXpDictionary;
					craftingTemplate2 = craftingTemplate;
					dictionary[craftingTemplate2] -= num2;
					if (this.OpenNewPart(craftingTemplate))
					{
						num++;
					}
				}
				num2 = Campaign.Current.Models.SmithingModel.ResearchPointsNeedForNewPart(count, craftingTemplate.Pieces.Count<CraftingPiece>((CraftingPiece x) => this.IsOpened(x, craftingTemplate)));
			}
			while (this._openNewPartXpDictionary[craftingTemplate] > num2 && num < count);
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x001183E8 File Offset: 0x001165E8
		private bool OpenNewPart(CraftingTemplate craftingTemplate)
		{
			int num = int.MaxValue;
			MBList<CraftingPiece> mblist = new MBList<CraftingPiece>();
			foreach (CraftingPiece craftingPiece in craftingTemplate.Pieces)
			{
				int pieceTier = craftingPiece.PieceTier;
				if (num >= pieceTier && !craftingPiece.IsHiddenOnDesigner && !this.IsOpened(craftingPiece, craftingTemplate))
				{
					if (num > craftingPiece.PieceTier)
					{
						mblist.Clear();
						num = pieceTier;
					}
					mblist.Add(craftingPiece);
				}
			}
			if (mblist.Count > 0)
			{
				CraftingPiece randomElement = mblist.GetRandomElement<CraftingPiece>();
				this.OpenPart(randomElement, craftingTemplate, true);
				return true;
			}
			return false;
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x00118498 File Offset: 0x00116698
		private void OpenPart(CraftingPiece selectedPiece, CraftingTemplate craftingTemplate, bool showNotification = true)
		{
			this._openedPartsDictionary[craftingTemplate].Add(selectedPiece);
			CampaignEventDispatcher.Instance.CraftingPartUnlocked(selectedPiece);
			if (showNotification)
			{
				TextObject textObject = new TextObject("{=p9F90bc0}New Smithing Part Unlocked: {PART_NAME} for {WEAPON_TYPE}.", null);
				textObject.SetTextVariable("PART_NAME", selectedPiece.Name);
				textObject.SetTextVariable("WEAPON_TYPE", craftingTemplate.TemplateName);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x00118501 File Offset: 0x00116701
		public bool IsOpened(CraftingPiece craftingPiece, CraftingTemplate craftingTemplate)
		{
			return craftingPiece.IsGivenByDefault || this._openedPartsDictionary[craftingTemplate].Contains(craftingPiece);
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x0011851F File Offset: 0x0011671F
		public int GetCraftingDifficulty(WeaponDesign weaponDesign)
		{
			return Campaign.Current.Models.SmithingModel.CalculateWeaponDesignDifficulty(weaponDesign);
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x00118538 File Offset: 0x00116738
		private void InitializeLists()
		{
			if (this._craftingOrders.IsEmpty<KeyValuePair<Town, CraftingCampaignBehavior.CraftingOrderSlots>>())
			{
				foreach (Town town in Campaign.Current.AllTowns)
				{
					this._craftingOrders.Add(town, new CraftingCampaignBehavior.CraftingOrderSlots());
				}
			}
			foreach (KeyValuePair<CraftingTemplate, List<CraftingPiece>> keyValuePair in this._openedPartsDictionary.ToList<KeyValuePair<CraftingTemplate, List<CraftingPiece>>>())
			{
				if (!CraftingTemplate.All.Contains(keyValuePair.Key))
				{
					this._openedPartsDictionary.Remove(keyValuePair.Key);
				}
			}
			foreach (KeyValuePair<CraftingTemplate, float> keyValuePair2 in this._openNewPartXpDictionary.ToList<KeyValuePair<CraftingTemplate, float>>())
			{
				if (!CraftingTemplate.All.Contains(keyValuePair2.Key))
				{
					this._openNewPartXpDictionary.Remove(keyValuePair2.Key);
				}
			}
			foreach (CraftingTemplate craftingTemplate in CraftingTemplate.All)
			{
				if (!this._openNewPartXpDictionary.ContainsKey(craftingTemplate))
				{
					this._openNewPartXpDictionary.Add(craftingTemplate, 0f);
				}
				if (!this._openedPartsDictionary.ContainsKey(craftingTemplate))
				{
					this._openedPartsDictionary.Add(craftingTemplate, new List<CraftingPiece>());
				}
				foreach (CraftingPiece craftingPiece in this._openedPartsDictionary[craftingTemplate].ToList<CraftingPiece>())
				{
					if (!craftingTemplate.Pieces.Contains(craftingPiece))
					{
						this._openedPartsDictionary[craftingTemplate].Remove(craftingPiece);
					}
				}
			}
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x00118768 File Offset: 0x00116968
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x00118774 File Offset: 0x00116974
		private void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("blacksmith_begin", "start", "blacksmith_player", "{=gYByVHQy}Good day, {?PLAYER.GENDER}madam{?}sir{\\?}. How may I help you?", new ConversationSentence.OnConditionDelegate(this.conversation_blacksmith_begin_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("blacksmith_craft_items", "blacksmith_player", "player_blacksmith_after_craft", "{=VXKGD0ta}I want to use your forge.", () => Campaign.Current.IsCraftingEnabled, new ConversationSentence.OnConsequenceDelegate(this.conversation_blacksmith_craft_items_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("blacksmith_leave", "blacksmith_player", "close_window", "{=iW9iKbb8}Nothing.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("blacksmith_player_after_craft_anything_else", "player_blacksmith_after_craft", "blacksmith_player_1", "{=IvY187PJ}No matter. Anything else?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("blacksmith_craft_items_1", "blacksmith_player_1", "player_blacksmith_after_craft", "{=hrn1Cdwo}There is something else I need you to make.", () => Campaign.Current.IsCraftingEnabled, new ConversationSentence.OnConsequenceDelegate(this.conversation_blacksmith_craft_items_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("blacksmith_leave_1", "blacksmith_player_1", "close_window", "{=iW9iKbb8}Nothing.", null, null, 100, null, null);
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x001188A2 File Offset: 0x00116AA2
		private bool conversation_blacksmith_begin_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Blacksmith;
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x001188B2 File Offset: 0x00116AB2
		private void conversation_blacksmith_craft_items_on_consequence()
		{
			CraftingHelper.OpenCrafting(CraftingTemplate.All[0], null);
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x001188C5 File Offset: 0x00116AC5
		public int GetHeroCraftingStamina(Hero hero)
		{
			return this.GetRecordForCompanion(hero).CraftingStamina;
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x001188D4 File Offset: 0x00116AD4
		private CraftingCampaignBehavior.HeroCraftingRecord GetRecordForCompanion(Hero hero)
		{
			CraftingCampaignBehavior.HeroCraftingRecord heroCraftingRecord;
			if (!this._heroCraftingRecords.TryGetValue(hero, out heroCraftingRecord))
			{
				heroCraftingRecord = new CraftingCampaignBehavior.HeroCraftingRecord(this.GetMaxHeroCraftingStamina(hero));
				this._heroCraftingRecords[hero] = heroCraftingRecord;
			}
			return heroCraftingRecord;
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x0011890C File Offset: 0x00116B0C
		public void SetHeroCraftingStamina(Hero hero, int value)
		{
			this.GetRecordForCompanion(hero).CraftingStamina = MathF.Max(0, value);
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x00118924 File Offset: 0x00116B24
		public void SetCraftedWeaponName(ItemObject craftedWeaponItem, TextObject name)
		{
			CraftingCampaignBehavior.CraftedItemInitializationData craftedItemInitializationData;
			if (this._craftedItemDictionary.TryGetValue(craftedWeaponItem, out craftedItemInitializationData))
			{
				this._craftedItemDictionary[craftedWeaponItem] = new CraftingCampaignBehavior.CraftedItemInitializationData(craftedItemInitializationData.CraftedData, name, craftedItemInitializationData.Culture);
			}
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x0011895F File Offset: 0x00116B5F
		public int GetMaxHeroCraftingStamina(Hero hero)
		{
			return 100 + MathF.Round((float)hero.GetSkillValue(DefaultSkills.Crafting) * 0.5f);
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x0011897C File Offset: 0x00116B7C
		public void DoRefinement(Hero hero, Crafting.RefiningFormula refineFormula)
		{
			ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
			if (refineFormula.Input1Count > 0)
			{
				ItemObject craftingMaterialItem = Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem(refineFormula.Input1);
				itemRoster.AddToCounts(craftingMaterialItem, -refineFormula.Input1Count);
			}
			if (refineFormula.Input2Count > 0)
			{
				ItemObject craftingMaterialItem2 = Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem(refineFormula.Input2);
				itemRoster.AddToCounts(craftingMaterialItem2, -refineFormula.Input2Count);
			}
			if (refineFormula.OutputCount > 0)
			{
				ItemObject craftingMaterialItem3 = Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem(refineFormula.Output);
				itemRoster.AddToCounts(craftingMaterialItem3, refineFormula.OutputCount);
			}
			if (refineFormula.Output2Count > 0)
			{
				ItemObject craftingMaterialItem4 = Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem(refineFormula.Output2);
				itemRoster.AddToCounts(craftingMaterialItem4, refineFormula.Output2Count);
			}
			hero.AddSkillXp(DefaultSkills.Crafting, (float)Campaign.Current.Models.SmithingModel.GetSkillXpForRefining(ref refineFormula));
			int energyCostForRefining = Campaign.Current.Models.SmithingModel.GetEnergyCostForRefining(ref refineFormula, hero);
			this.SetHeroCraftingStamina(hero, this.GetHeroCraftingStamina(hero) - energyCostForRefining);
			CampaignEventDispatcher.Instance.OnItemsRefined(hero, refineFormula);
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x00118AB8 File Offset: 0x00116CB8
		public void DoSmelting(Hero currentCraftingHero, EquipmentElement equipmentElement)
		{
			ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
			ItemObject item = equipmentElement.Item;
			int[] smeltingOutputForItem = Campaign.Current.Models.SmithingModel.GetSmeltingOutputForItem(item);
			for (int i = 8; i >= 0; i--)
			{
				if (smeltingOutputForItem[i] != 0)
				{
					itemRoster.AddToCounts(Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem((CraftingMaterials)i), smeltingOutputForItem[i]);
				}
			}
			itemRoster.AddToCounts(equipmentElement, -1);
			currentCraftingHero.AddSkillXp(DefaultSkills.Crafting, (float)Campaign.Current.Models.SmithingModel.GetSkillXpForSmelting(item));
			int energyCostForSmelting = Campaign.Current.Models.SmithingModel.GetEnergyCostForSmelting(item, currentCraftingHero);
			this.SetHeroCraftingStamina(currentCraftingHero, this.GetHeroCraftingStamina(currentCraftingHero) - energyCostForSmelting);
			this.AddResearchPoints(item.WeaponDesign.Template, Campaign.Current.Models.SmithingModel.GetPartResearchGainForSmeltingItem(item, currentCraftingHero));
			CampaignEventDispatcher.Instance.OnEquipmentSmeltedByHero(currentCraftingHero, equipmentElement);
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x00118BAC File Offset: 0x00116DAC
		public ItemObject CreateCraftedWeaponInFreeBuildMode(Hero hero, WeaponDesign weaponDesign, ItemModifier weaponModifier = null)
		{
			ItemObject itemObject = this.CreateCraftedWeaponInternal(true, hero, weaponDesign, weaponModifier);
			int skillXpForSmithingInFreeBuildMode = Campaign.Current.Models.SmithingModel.GetSkillXpForSmithingInFreeBuildMode(itemObject);
			hero.AddSkillXp(DefaultSkills.Crafting, (float)skillXpForSmithingInFreeBuildMode);
			this.AddItemToHistory(itemObject);
			return itemObject;
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x00118BF0 File Offset: 0x00116DF0
		public ItemObject CreateCraftedWeaponInCraftingOrderMode(Hero crafterHero, CraftingOrder craftingOrder, WeaponDesign weaponDesign)
		{
			ItemObject itemObject = this.CreateCraftedWeaponInternal(false, crafterHero, weaponDesign, null);
			float num = craftingOrder.GetOrderExperience(itemObject, this._currentItemModifier) + (float)Campaign.Current.Models.SmithingModel.GetSkillXpForSmithingInCraftingOrderMode(itemObject);
			crafterHero.AddSkillXp(DefaultSkills.Crafting, num);
			return itemObject;
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00118C3C File Offset: 0x00116E3C
		private ItemObject CreateCraftedWeaponInternal(bool isFreeMode, Hero crafterHero, WeaponDesign weaponDesign, ItemModifier weaponModifier = null)
		{
			string nextCraftedItemId = this.GetNextCraftedItemId();
			if (isFreeMode)
			{
				weaponDesign = new WeaponDesign(weaponDesign.Template, weaponDesign.WeaponName, weaponDesign.UsedPieces, nextCraftedItemId);
			}
			CraftingCampaignBehavior.SpendMaterials(weaponDesign);
			ItemObject currentCraftedItemObject = (GameStateManager.Current.ActiveState as CraftingState).CraftingLogic.GetCurrentCraftedItemObject(true, nextCraftedItemId);
			ItemObject.InitAsPlayerCraftedItem(ref currentCraftedItemObject);
			MBObjectManager.Instance.RegisterObject<ItemObject>(currentCraftedItemObject);
			if (isFreeMode)
			{
				if (weaponModifier == null)
				{
					PartyBase.MainParty.ItemRoster.AddToCounts(currentCraftedItemObject, 1);
				}
				else
				{
					EquipmentElement equipmentElement = new EquipmentElement(currentCraftedItemObject, weaponModifier, null, false);
					PartyBase.MainParty.ItemRoster.AddToCounts(equipmentElement, 1);
				}
			}
			CampaignEventDispatcher.Instance.OnNewItemCrafted(currentCraftedItemObject, weaponModifier, !isFreeMode);
			int energyCostForSmithing = Campaign.Current.Models.SmithingModel.GetEnergyCostForSmithing(currentCraftedItemObject, crafterHero);
			this.SetHeroCraftingStamina(crafterHero, this.GetHeroCraftingStamina(crafterHero) - energyCostForSmithing);
			this.AddResearchPoints(weaponDesign.Template, Campaign.Current.Models.SmithingModel.GetPartResearchGainForSmithingItem(currentCraftedItemObject, crafterHero, isFreeMode));
			return currentCraftedItemObject;
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x00118D38 File Offset: 0x00116F38
		private static void SpendMaterials(WeaponDesign weaponDesign)
		{
			ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
			int[] smithingCostsForWeaponDesign = Campaign.Current.Models.SmithingModel.GetSmithingCostsForWeaponDesign(weaponDesign);
			for (int i = 8; i >= 0; i--)
			{
				if (smithingCostsForWeaponDesign[i] != 0)
				{
					itemRoster.AddToCounts(Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem((CraftingMaterials)i), smithingCostsForWeaponDesign[i]);
				}
			}
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x00118D96 File Offset: 0x00116F96
		private void AddItemToHistory(ItemObject craftedObject)
		{
			while (this._cratingItemsHistory.Count >= 10)
			{
				this._cratingItemsHistory.RemoveAt(0);
			}
			this._cratingItemsHistory.Add(craftedObject);
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x00118DC1 File Offset: 0x00116FC1
		public Hero GetActiveCraftingHero()
		{
			return this._activeCraftingHero;
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x00118DC9 File Offset: 0x00116FC9
		public void SetActiveCraftingHero(Hero hero)
		{
			this._activeCraftingHero = hero;
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x00118DD4 File Offset: 0x00116FD4
		public void CreateTownOrder(Hero orderOwner, int orderSlot)
		{
			if (orderOwner.CurrentSettlement == null || !orderOwner.CurrentSettlement.IsTown)
			{
				Debug.Print(string.Concat(new string[]
				{
					"Order owner: ",
					orderOwner.StringId,
					" Settlement",
					(orderOwner.CurrentSettlement == null) ? "null" : orderOwner.CurrentSettlement.StringId,
					" Order owner party: ",
					(orderOwner.PartyBelongedTo == null) ? "null" : orderOwner.PartyBelongedTo.StringId
				}), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			float townOrderDifficulty = CraftingCampaignBehavior.GetTownOrderDifficulty(orderOwner.CurrentSettlement.Town, orderSlot);
			int num = (int)townOrderDifficulty / 50;
			CraftingTemplate randomElement = CraftingTemplate.All.GetRandomElement<CraftingTemplate>();
			string nextTownOrderId = this.GetNextTownOrderId();
			WeaponDesign weaponDesign = new WeaponDesign(randomElement, TextObject.GetEmpty(), this.GetWeaponPieces(randomElement, num), nextTownOrderId);
			this._craftingOrders[orderOwner.CurrentSettlement.Town].AddTownOrder(new CraftingOrder(orderOwner, townOrderDifficulty, weaponDesign, randomElement, orderSlot, nextTownOrderId));
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x00118ED4 File Offset: 0x001170D4
		private static float GetTownOrderDifficulty(Town town, int orderSlot)
		{
			int num = 0;
			switch (orderSlot)
			{
			case 0:
				num = MBRandom.RandomInt(40, 80);
				break;
			case 1:
				num = MBRandom.RandomInt(80, 120);
				break;
			case 2:
				num = MBRandom.RandomInt(120, 160);
				break;
			case 3:
				num = MBRandom.RandomInt(160, 200);
				break;
			case 4:
				num = MBRandom.RandomInt(200, 241);
				break;
			case 5:
				num = Hero.MainHero.GetSkillValue(DefaultSkills.Crafting);
				break;
			}
			return (float)num + town.Prosperity / 500f;
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x00118F70 File Offset: 0x00117170
		public CraftingOrder CreateCustomOrderForHero(Hero orderOwner, float orderDifficulty = -1f, WeaponDesign weaponDesign = null, CraftingTemplate craftingTemplate = null)
		{
			string nextTownOrderId = this.GetNextTownOrderId();
			if (orderDifficulty < 0f)
			{
				orderDifficulty = CraftingCampaignBehavior.GetRandomOrderDifficulty(orderOwner.CurrentSettlement.Town);
			}
			if (craftingTemplate == null)
			{
				craftingTemplate = CraftingTemplate.All.GetRandomElement<CraftingTemplate>();
			}
			if (weaponDesign == null)
			{
				int num = (int)orderDifficulty / 40;
				weaponDesign = new WeaponDesign(craftingTemplate, TextObject.GetEmpty(), this.GetWeaponPieces(craftingTemplate, num), nextTownOrderId);
			}
			CraftingOrder craftingOrder = new CraftingOrder(orderOwner, orderDifficulty, weaponDesign, craftingTemplate, -1, nextTownOrderId);
			this._craftingOrders[orderOwner.CurrentSettlement.Town].AddCustomOrder(craftingOrder);
			return craftingOrder;
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x00119000 File Offset: 0x00117200
		private static float GetRandomOrderDifficulty(Town town)
		{
			int num = MBRandom.RandomInt(0, 6);
			int num2 = 0;
			switch (num)
			{
			case 0:
				num2 = MBRandom.RandomInt(40, 80);
				break;
			case 1:
				num2 = MBRandom.RandomInt(80, 120);
				break;
			case 2:
				num2 = MBRandom.RandomInt(120, 160);
				break;
			case 3:
				num2 = MBRandom.RandomInt(160, 200);
				break;
			case 4:
				num2 = MBRandom.RandomInt(200, 241);
				break;
			case 5:
				num2 = Hero.MainHero.GetSkillValue(DefaultSkills.Crafting);
				break;
			}
			return (float)num2 + town.Prosperity / 500f;
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x001190A4 File Offset: 0x001172A4
		private WeaponDesignElement[] GetWeaponPieces(CraftingTemplate craftingTemplate, int pieceTier)
		{
			WeaponDesignElement[] array = new WeaponDesignElement[4];
			List<WeaponDesignElement>[] array2 = new List<WeaponDesignElement>[4];
			foreach (CraftingPiece craftingPiece in craftingTemplate.Pieces)
			{
				bool flag = false;
				foreach (PieceData pieceData in craftingTemplate.BuildOrders)
				{
					if (pieceData.PieceType == craftingPiece.PieceType)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					int pieceType = (int)craftingPiece.PieceType;
					if (array2[pieceType] == null)
					{
						array2[pieceType] = new List<WeaponDesignElement>();
					}
					array2[pieceType].Add(WeaponDesignElement.CreateUsablePiece(craftingPiece, 100));
				}
			}
			Func<WeaponDesignElement, bool> <>9__0;
			for (int j = 0; j < array.Length; j++)
			{
				if (array2[j] != null)
				{
					WeaponDesignElement[] array3 = array;
					int num = j;
					List<WeaponDesignElement> list = array2[j];
					Func<WeaponDesignElement, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (WeaponDesignElement p) => !p.CraftingPiece.IsHiddenOnDesigner && p.CraftingPiece.PieceTier == pieceTier);
					}
					WeaponDesignElement weaponDesignElement;
					if ((weaponDesignElement = list.FirstOrDefaultQ<WeaponDesignElement>(func)) == null)
					{
						weaponDesignElement = array2[j].FirstOrDefaultQ<WeaponDesignElement>((WeaponDesignElement p) => !p.CraftingPiece.IsHiddenOnDesigner && p.CraftingPiece.PieceTier == 1);
					}
					WeaponDesignElement weaponDesignElement2;
					if ((weaponDesignElement2 = weaponDesignElement) == null)
					{
						weaponDesignElement2 = array2[j].First<WeaponDesignElement>((WeaponDesignElement p) => !p.CraftingPiece.IsHiddenOnDesigner);
					}
					array3[num] = weaponDesignElement2;
				}
				else
				{
					array[j] = WeaponDesignElement.GetInvalidPieceForType((CraftingPiece.PieceTypes)j);
				}
			}
			return array;
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x00119230 File Offset: 0x00117430
		private void ReplaceCraftingOrder(Town town, CraftingOrder order)
		{
			MBList<Hero> mblist = new MBList<Hero>();
			Settlement settlement = town.Settlement;
			mblist.AddRange(settlement.HeroesWithoutParty);
			foreach (MobileParty mobileParty in settlement.Parties)
			{
				if (mobileParty.LeaderHero != null && !mobileParty.IsMainParty)
				{
					mblist.Add(mobileParty.LeaderHero);
				}
			}
			int difficultyLevel = order.DifficultyLevel;
			this._craftingOrders[town].RemoveTownOrder(order);
			if (mblist.Count > 0)
			{
				this.CreateTownOrder(mblist.GetRandomElement<Hero>(), difficultyLevel);
			}
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x001192E4 File Offset: 0x001174E4
		public void GetOrderResult(CraftingOrder craftingOrder, ItemObject craftedItem, out bool isSucceed, out TextObject orderRemark, out TextObject orderResult, out int finalReward)
		{
			finalReward = this.CalculateOrderPriceDifference(craftingOrder, craftedItem);
			float num;
			float num2;
			bool flag;
			bool flag2;
			craftingOrder.CheckForBonusesAndPenalties(craftedItem, this._currentItemModifier, out num, out num2, out flag, out flag2);
			isSucceed = num >= num2 && flag && flag2;
			int num3 = finalReward - craftingOrder.BaseGoldReward;
			orderRemark = TextObject.GetEmpty();
			if (isSucceed)
			{
				orderResult = new TextObject("{=Nn49hU2W}The client is satisfied.", null);
				if (num3 == 0)
				{
					orderRemark = new TextObject("{=FWHvvZFq}\"This is exactly what I wanted. Here is your money, you've earned it.\"", null);
					return;
				}
				if ((float)num3 > 0f)
				{
					orderRemark = new TextObject("{=raCa7QXj}\"This is even better than what I have imagined. Here is your money, and I'm putting a little extra for your effort.\"", null);
					return;
				}
			}
			else
			{
				orderResult = new TextObject("{=bC2jevlu}The client is displeased.", null);
				if (finalReward <= 0)
				{
					orderRemark = new TextObject("{=NZynd8vT}\"This weapon is worthless. I'm not giving you a dime!\"", null);
					return;
				}
				if (finalReward < craftingOrder.BaseGoldReward)
				{
					TextObject textObject;
					if (!flag || !flag2)
					{
						textObject = new TextObject("{=WyuIksRB}\"This weapon does not have the damage type I wanted. I'm cutting {AMOUNT}{GOLD_ICON} from the price.\"", null);
					}
					else
					{
						textObject = new TextObject("{=wU76OPxM}\"This is worse than what I've asked for. I'm cutting {AMOUNT}{GOLD_ICON} from the price.\"", null);
					}
					textObject.SetTextVariable("AMOUNT", MathF.Abs(num3));
					orderRemark = textObject;
				}
			}
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x001193DC File Offset: 0x001175DC
		private int CalculateOrderPriceDifference(CraftingOrder craftingOrder, ItemObject craftedItem)
		{
			float num;
			float num2;
			bool flag;
			bool flag2;
			craftingOrder.CheckForBonusesAndPenalties(craftedItem, this._currentItemModifier, out num, out num2, out flag, out flag2);
			float num3 = (float)craftingOrder.BaseGoldReward;
			if (!num.ApproximatelyEqualsTo(0f, 1E-05f) && !num2.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				if (num < num2 || !flag || !flag2)
				{
					float num4 = (float)Campaign.Current.Models.TradeItemPriceFactorModel.GetTheoreticalMaxItemMarketValue(craftedItem) / (float)Campaign.Current.Models.TradeItemPriceFactorModel.GetTheoreticalMaxItemMarketValue(craftingOrder.PreCraftedWeaponDesignItem);
					num3 = (float)craftingOrder.BaseGoldReward * 0.5f * MathF.Min(1f, num4);
					if (num3 > (float)craftingOrder.BaseGoldReward)
					{
						num3 = (float)craftingOrder.BaseGoldReward * 0.5f;
					}
				}
				else if (num > num2)
				{
					num3 = (float)craftingOrder.BaseGoldReward * (1f + (num - num2) / num2 * 0.1f);
				}
			}
			return (int)num3;
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x001194CC File Offset: 0x001176CC
		public void CompleteOrder(Town town, CraftingOrder craftingOrder, ItemObject craftedItem, Hero completerHero)
		{
			int num = this.CalculateOrderPriceDifference(craftingOrder, craftedItem);
			bool flag;
			TextObject textObject;
			TextObject textObject2;
			int num2;
			this.GetOrderResult(craftingOrder, craftedItem, out flag, out textObject, out textObject2, out num2);
			GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, num, false);
			if (this._craftingOrders[town].CustomOrders.Contains(craftingOrder))
			{
				this._craftingOrders[town].RemoveCustomOrder(craftingOrder);
			}
			else
			{
				if (craftingOrder.IsLordOrder)
				{
					this.ChangeCraftedOrderWithTheNoblesWeaponIfItIsBetter(craftedItem, craftingOrder);
					if (craftingOrder.OrderOwner.PartyBelongedTo != null)
					{
						this.GiveTroopToNobleAtWeaponTier((int)craftedItem.Tier, craftingOrder.OrderOwner);
					}
					if (flag && completerHero.GetPerkValue(DefaultPerks.Crafting.SteelMaker3))
					{
						int num3 = (int)DefaultPerks.Crafting.SteelMaker3.SecondaryBonus;
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(completerHero, craftingOrder.OrderOwner, num3, true);
					}
				}
				else
				{
					craftingOrder.OrderOwner.AddPower((float)(craftedItem.Tier + 1));
					if (flag && completerHero.GetPerkValue(DefaultPerks.Crafting.ExperiencedSmith))
					{
						int num4 = (int)DefaultPerks.Crafting.ExperiencedSmith.SecondaryBonus;
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(completerHero, craftingOrder.OrderOwner, num4, true);
					}
				}
				this._craftingOrders[town].RemoveTownOrder(craftingOrder);
			}
			CampaignEventDispatcher.Instance.OnCraftingOrderCompleted(town, craftingOrder, craftedItem, completerHero);
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x001195F1 File Offset: 0x001177F1
		public ItemModifier GetCurrentItemModifier()
		{
			return this._currentItemModifier;
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x001195F9 File Offset: 0x001177F9
		public void SetCurrentItemModifier(ItemModifier modifier)
		{
			this._currentItemModifier = modifier;
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x00119604 File Offset: 0x00117804
		private void RemoveOrdersOfHeroWithoutCompletionIfExists(Hero hero)
		{
			foreach (KeyValuePair<Town, CraftingCampaignBehavior.CraftingOrderSlots> keyValuePair in this._craftingOrders)
			{
				for (int i = 0; i < 6; i++)
				{
					if (keyValuePair.Value.Slots[i] != null && keyValuePair.Value.Slots[i].OrderOwner == hero)
					{
						keyValuePair.Value.RemoveTownOrder(keyValuePair.Value.Slots[i]);
					}
				}
			}
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x0011969C File Offset: 0x0011789C
		public void CancelCustomOrder(Town town, CraftingOrder craftingOrder)
		{
			if (this._craftingOrders[town].CustomOrders.Contains(craftingOrder))
			{
				this._craftingOrders[town].RemoveCustomOrder(craftingOrder);
				return;
			}
			Debug.FailedAssert("Trying to cancel a custom order that doesn't exist.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CraftingCampaignBehavior.cs", "CancelCustomOrder", 1422);
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x001196EE File Offset: 0x001178EE
		private void CancelOrder(Town town, CraftingOrder craftingOrder)
		{
			this._craftingOrders[town].RemoveTownOrder(craftingOrder);
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x00119704 File Offset: 0x00117904
		private void ChangeCraftedOrderWithTheNoblesWeaponIfItIsBetter(ItemObject craftedItem, CraftingOrder craftingOrder)
		{
			Equipment battleEquipment = craftingOrder.OrderOwner.BattleEquipment;
			for (int i = 0; i < 12; i++)
			{
				if (!battleEquipment[i].IsEmpty)
				{
					WeaponClass weaponClass = craftedItem.PrimaryWeapon.WeaponClass;
					WeaponComponentData primaryWeapon = battleEquipment[i].Item.PrimaryWeapon;
					WeaponClass? weaponClass2 = ((primaryWeapon != null) ? new WeaponClass?(primaryWeapon.WeaponClass) : null);
					if ((weaponClass == weaponClass2.GetValueOrDefault()) & (weaponClass2 != null))
					{
						ItemObject item = battleEquipment[i].Item;
						int thrustSpeed = item.PrimaryWeapon.ThrustSpeed;
						int thrustSpeed2 = craftedItem.PrimaryWeapon.ThrustSpeed;
						int swingSpeed = item.PrimaryWeapon.SwingSpeed;
						int swingSpeed2 = craftedItem.PrimaryWeapon.SwingSpeed;
						int missileSpeed = item.PrimaryWeapon.MissileSpeed;
						int missileSpeed2 = craftedItem.PrimaryWeapon.MissileSpeed;
						float weaponBalance = item.PrimaryWeapon.WeaponBalance;
						float weaponBalance2 = craftedItem.PrimaryWeapon.WeaponBalance;
						int thrustDamage = item.PrimaryWeapon.ThrustDamage;
						int thrustDamage2 = craftedItem.PrimaryWeapon.ThrustDamage;
						DamageTypes thrustDamageType = item.PrimaryWeapon.ThrustDamageType;
						DamageTypes thrustDamageType2 = craftedItem.PrimaryWeapon.ThrustDamageType;
						int swingDamage = item.PrimaryWeapon.SwingDamage;
						int swingDamage2 = craftedItem.PrimaryWeapon.SwingDamage;
						DamageTypes swingDamageType = item.PrimaryWeapon.SwingDamageType;
						DamageTypes swingDamageType2 = craftedItem.PrimaryWeapon.SwingDamageType;
						int accuracy = item.PrimaryWeapon.Accuracy;
						int accuracy2 = craftedItem.PrimaryWeapon.Accuracy;
						float weight = item.Weight;
						float weight2 = craftedItem.Weight;
						if (thrustSpeed2 > thrustSpeed && swingSpeed2 > swingSpeed && missileSpeed2 > missileSpeed && weaponBalance2 > weaponBalance && thrustDamage2 > thrustDamage && thrustDamageType == thrustDamageType2 && swingDamage2 > swingDamage && swingDamageType2 == swingDamageType && accuracy2 > accuracy && weight2 < weight)
						{
							battleEquipment[i] = new EquipmentElement(craftedItem, null, null, false);
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x001198EC File Offset: 0x00117AEC
		private void GiveTroopToNobleAtWeaponTier(int tier, Hero noble)
		{
			CharacterObject characterObject = noble.Culture.BasicTroop;
			for (int i = 0; i < tier; i++)
			{
				if (characterObject.UpgradeTargets.Length != 0)
				{
					characterObject = characterObject.UpgradeTargets.GetRandomElement<CharacterObject>();
				}
			}
			noble.PartyBelongedTo.AddElementToMemberRoster(characterObject, 1, false);
		}

		// Token: 0x04001386 RID: 4998
		private const float CraftingOrderReplaceChance = 0.05f;

		// Token: 0x04001387 RID: 4999
		private const float CreateCraftingOrderChance = 0.05f;

		// Token: 0x04001388 RID: 5000
		private const int TownCraftingOrderCount = 6;

		// Token: 0x04001389 RID: 5001
		private const int DefaultCraftingOrderPieceTier = 1;

		// Token: 0x0400138A RID: 5002
		private const int CraftingOrderTroopBonusAmount = 1;

		// Token: 0x0400138B RID: 5003
		private const int MinOrderDifficulty = 40;

		// Token: 0x0400138C RID: 5004
		private const int MaxOrderDifficulty = 240;

		// Token: 0x0400138D RID: 5005
		private const int MaxCraftingHistoryDesigns = 10;

		// Token: 0x0400138E RID: 5006
		private const int BaseHeroCraftingStamina = 100;

		// Token: 0x0400138F RID: 5007
		private Hero _activeCraftingHero;

		// Token: 0x04001390 RID: 5008
		private ItemModifier _currentItemModifier;

		// Token: 0x04001391 RID: 5009
		private Dictionary<CraftingTemplate, List<CraftingPiece>> _openedPartsDictionary = new Dictionary<CraftingTemplate, List<CraftingPiece>>();

		// Token: 0x04001392 RID: 5010
		private Dictionary<CraftingTemplate, float> _openNewPartXpDictionary = new Dictionary<CraftingTemplate, float>();

		// Token: 0x04001393 RID: 5011
		private Dictionary<ItemObject, CraftingCampaignBehavior.CraftedItemInitializationData> _craftedItemDictionary = new Dictionary<ItemObject, CraftingCampaignBehavior.CraftedItemInitializationData>();

		// Token: 0x04001394 RID: 5012
		private Dictionary<Hero, CraftingCampaignBehavior.HeroCraftingRecord> _heroCraftingRecords = new Dictionary<Hero, CraftingCampaignBehavior.HeroCraftingRecord>();

		// Token: 0x04001395 RID: 5013
		private Dictionary<Town, CraftingCampaignBehavior.CraftingOrderSlots> _craftingOrders = new Dictionary<Town, CraftingCampaignBehavior.CraftingOrderSlots>();

		// Token: 0x04001396 RID: 5014
		private List<ItemObject> _cratingItemsHistory = new List<ItemObject>();

		// Token: 0x04001397 RID: 5015
		private int _townOrderCount;

		// Token: 0x04001398 RID: 5016
		private int _craftedItemCount;

		// Token: 0x02000823 RID: 2083
		public class CraftingCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x0600671F RID: 26399 RVA: 0x001D181D File Offset: 0x001CFA1D
			public CraftingCampaignBehaviorTypeDefiner()
				: base(150000)
			{
			}

			// Token: 0x06006720 RID: 26400 RVA: 0x001D182A File Offset: 0x001CFA2A
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(CraftingCampaignBehavior.CraftedItemInitializationData), 10, null);
				base.AddClassDefinition(typeof(CraftingCampaignBehavior.HeroCraftingRecord), 20, null);
				base.AddClassDefinition(typeof(CraftingCampaignBehavior.CraftingOrderSlots), 30, null);
			}

			// Token: 0x06006721 RID: 26401 RVA: 0x001D1865 File Offset: 0x001CFA65
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<ItemObject, CraftingCampaignBehavior.CraftedItemInitializationData>));
				base.ConstructContainerDefinition(typeof(Dictionary<Hero, CraftingCampaignBehavior.HeroCraftingRecord>));
				base.ConstructContainerDefinition(typeof(Dictionary<Town, CraftingCampaignBehavior.CraftingOrderSlots>));
			}
		}

		// Token: 0x02000824 RID: 2084
		internal class CraftedItemInitializationData
		{
			// Token: 0x06006722 RID: 26402 RVA: 0x001D1897 File Offset: 0x001CFA97
			public CraftedItemInitializationData(WeaponDesign craftedData, TextObject itemName, CultureObject culture)
			{
				this.CraftedData = craftedData;
				this.ItemName = itemName;
				this.Culture = culture;
			}

			// Token: 0x06006723 RID: 26403 RVA: 0x001D18B4 File Offset: 0x001CFAB4
			internal static void AutoGeneratedStaticCollectObjectsCraftedItemInitializationData(object o, List<object> collectedObjects)
			{
				((CraftingCampaignBehavior.CraftedItemInitializationData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006724 RID: 26404 RVA: 0x001D18C2 File Offset: 0x001CFAC2
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.CraftedData);
				collectedObjects.Add(this.ItemName);
				collectedObjects.Add(this.Culture);
			}

			// Token: 0x06006725 RID: 26405 RVA: 0x001D18E8 File Offset: 0x001CFAE8
			internal static object AutoGeneratedGetMemberValueCraftedData(object o)
			{
				return ((CraftingCampaignBehavior.CraftedItemInitializationData)o).CraftedData;
			}

			// Token: 0x06006726 RID: 26406 RVA: 0x001D18F5 File Offset: 0x001CFAF5
			internal static object AutoGeneratedGetMemberValueItemName(object o)
			{
				return ((CraftingCampaignBehavior.CraftedItemInitializationData)o).ItemName;
			}

			// Token: 0x06006727 RID: 26407 RVA: 0x001D1902 File Offset: 0x001CFB02
			internal static object AutoGeneratedGetMemberValueCulture(object o)
			{
				return ((CraftingCampaignBehavior.CraftedItemInitializationData)o).Culture;
			}

			// Token: 0x0400213C RID: 8508
			[SaveableField(10)]
			public readonly WeaponDesign CraftedData;

			// Token: 0x0400213D RID: 8509
			[SaveableField(20)]
			public readonly TextObject ItemName;

			// Token: 0x0400213E RID: 8510
			[SaveableField(30)]
			public readonly CultureObject Culture;
		}

		// Token: 0x02000825 RID: 2085
		internal class HeroCraftingRecord
		{
			// Token: 0x06006728 RID: 26408 RVA: 0x001D190F File Offset: 0x001CFB0F
			public HeroCraftingRecord(int maxStamina)
			{
				this.CraftingStamina = maxStamina;
			}

			// Token: 0x06006729 RID: 26409 RVA: 0x001D191E File Offset: 0x001CFB1E
			internal static void AutoGeneratedStaticCollectObjectsHeroCraftingRecord(object o, List<object> collectedObjects)
			{
				((CraftingCampaignBehavior.HeroCraftingRecord)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600672A RID: 26410 RVA: 0x001D192C File Offset: 0x001CFB2C
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
			}

			// Token: 0x0600672B RID: 26411 RVA: 0x001D192E File Offset: 0x001CFB2E
			internal static object AutoGeneratedGetMemberValueCraftingStamina(object o)
			{
				return ((CraftingCampaignBehavior.HeroCraftingRecord)o).CraftingStamina;
			}

			// Token: 0x0400213F RID: 8511
			[SaveableField(10)]
			public int CraftingStamina;
		}

		// Token: 0x02000826 RID: 2086
		public class CraftingOrderSlots
		{
			// Token: 0x170015A3 RID: 5539
			// (get) Token: 0x0600672C RID: 26412 RVA: 0x001D1940 File Offset: 0x001CFB40
			public MBReadOnlyList<CraftingOrder> CustomOrders
			{
				get
				{
					return this._customOrders;
				}
			}

			// Token: 0x0600672D RID: 26413 RVA: 0x001D1948 File Offset: 0x001CFB48
			public CraftingOrderSlots()
			{
				this.Slots = new CraftingOrder[6];
				for (int i = 0; i < 6; i++)
				{
					this.Slots[i] = null;
				}
				this._customOrders = new MBList<CraftingOrder>();
			}

			// Token: 0x0600672E RID: 26414 RVA: 0x001D1987 File Offset: 0x001CFB87
			[LoadInitializationCallback]
			private void OnLoad()
			{
				if (this._customOrders == null)
				{
					this._customOrders = new MBList<CraftingOrder>();
				}
			}

			// Token: 0x0600672F RID: 26415 RVA: 0x001D199C File Offset: 0x001CFB9C
			public bool IsThereAvailableSlot()
			{
				for (int i = 0; i < 6; i++)
				{
					if (this.Slots[i] == null)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06006730 RID: 26416 RVA: 0x001D19C4 File Offset: 0x001CFBC4
			public int GetAvailableSlot()
			{
				for (int i = 0; i < 6; i++)
				{
					if (this.Slots[i] == null)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06006731 RID: 26417 RVA: 0x001D19EA File Offset: 0x001CFBEA
			internal void AddTownOrder(CraftingOrder craftingOrder)
			{
				this.Slots[craftingOrder.DifficultyLevel] = craftingOrder;
			}

			// Token: 0x06006732 RID: 26418 RVA: 0x001D19FA File Offset: 0x001CFBFA
			internal void RemoveTownOrder(CraftingOrder craftingOrder)
			{
				this.Slots[craftingOrder.DifficultyLevel] = null;
			}

			// Token: 0x06006733 RID: 26419 RVA: 0x001D1A0A File Offset: 0x001CFC0A
			internal void AddCustomOrder(CraftingOrder order)
			{
				this._customOrders.Add(order);
			}

			// Token: 0x06006734 RID: 26420 RVA: 0x001D1A18 File Offset: 0x001CFC18
			internal void RemoveCustomOrder(CraftingOrder order)
			{
				this._customOrders.Remove(order);
			}

			// Token: 0x06006735 RID: 26421 RVA: 0x001D1A27 File Offset: 0x001CFC27
			internal static void AutoGeneratedStaticCollectObjectsCraftingOrderSlots(object o, List<object> collectedObjects)
			{
				((CraftingCampaignBehavior.CraftingOrderSlots)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006736 RID: 26422 RVA: 0x001D1A35 File Offset: 0x001CFC35
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Slots);
				collectedObjects.Add(this._customOrders);
			}

			// Token: 0x06006737 RID: 26423 RVA: 0x001D1A4F File Offset: 0x001CFC4F
			internal static object AutoGeneratedGetMemberValueSlots(object o)
			{
				return ((CraftingCampaignBehavior.CraftingOrderSlots)o).Slots;
			}

			// Token: 0x06006738 RID: 26424 RVA: 0x001D1A5C File Offset: 0x001CFC5C
			internal static object AutoGeneratedGetMemberValue_customOrders(object o)
			{
				return ((CraftingCampaignBehavior.CraftingOrderSlots)o)._customOrders;
			}

			// Token: 0x04002140 RID: 8512
			private const int SlotCount = 6;

			// Token: 0x04002141 RID: 8513
			[SaveableField(10)]
			public CraftingOrder[] Slots;

			// Token: 0x04002142 RID: 8514
			[SaveableField(30)]
			private MBList<CraftingOrder> _customOrders;
		}
	}
}
