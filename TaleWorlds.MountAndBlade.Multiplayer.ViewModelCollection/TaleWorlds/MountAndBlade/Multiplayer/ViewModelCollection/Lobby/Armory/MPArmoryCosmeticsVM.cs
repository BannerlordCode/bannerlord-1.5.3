using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticCategory;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x02000079 RID: 121
	public class MPArmoryCosmeticsVM : ViewModel
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000BFF RID: 3071 RVA: 0x00023E10 File Offset: 0x00022010
		// (remove) Token: 0x06000C00 RID: 3072 RVA: 0x00023E44 File Offset: 0x00022044
		public static event Action<MPArmoryCosmeticItemBaseVM> OnCosmeticPreview;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000C01 RID: 3073 RVA: 0x00023E78 File Offset: 0x00022078
		// (remove) Token: 0x06000C02 RID: 3074 RVA: 0x00023EAC File Offset: 0x000220AC
		public static event Action<MPArmoryCosmeticItemBaseVM> OnRemoveCosmeticFromPreview;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000C03 RID: 3075 RVA: 0x00023EE0 File Offset: 0x000220E0
		// (remove) Token: 0x06000C04 RID: 3076 RVA: 0x00023F14 File Offset: 0x00022114
		public static event Action<List<EquipmentElement>> OnEquipmentRefreshed;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000C05 RID: 3077 RVA: 0x00023F48 File Offset: 0x00022148
		// (remove) Token: 0x06000C06 RID: 3078 RVA: 0x00023F7C File Offset: 0x0002217C
		public static event Action OnTauntAssignmentRefresh;

		// Token: 0x06000C07 RID: 3079 RVA: 0x00023FB0 File Offset: 0x000221B0
		public MPArmoryCosmeticsVM(Func<List<IReadOnlyPerkObject>> getSelectedPerks)
		{
			this._getSelectedPerks = getSelectedPerks;
			this._usedCosmetics = new Dictionary<string, List<string>>();
			this._ownedCosmetics = new List<string>();
			this._clothingCategoriesLookup = new Dictionary<MPArmoryCosmeticsVM.ClothingCategory, MPArmoryClothingCosmeticCategoryVM>();
			this._tauntCategoriesLookup = new Dictionary<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM>();
			this._cosmeticItemsLookup = new Dictionary<string, MPArmoryCosmeticItemBaseVM>();
			this.AvailableCategories = new MBBindingList<MPArmoryCosmeticCategoryBaseVM>();
			this.SortCategories = new SelectorVM<SelectorItemVM>(0, new Action<SelectorVM<SelectorItemVM>>(this.OnSortCategoryUpdated));
			this.SortOrders = new SelectorVM<SelectorItemVM>(0, new Action<SelectorVM<SelectorItemVM>>(this.OnSortOrderUpdated));
			this.TauntSlots = new MBBindingList<MPArmoryCosmeticTauntSlotVM>();
			this.InitializeCosmeticItemComparers();
			this.InitializeAllCosmetics();
			this.InitializeCallbacks();
			this.IsLoading = true;
			this.SortCategories.AddItem(new SelectorItemVM(new TextObject("{=J2wEawTl}Category", null)));
			this.SortCategories.AddItem(new SelectorItemVM(new TextObject("{=ebUrBmHK}Price", null)));
			this.SortCategories.AddItem(new SelectorItemVM(new TextObject("{=bD8nTS86}Rarity", null)));
			this.SortCategories.AddItem(new SelectorItemVM(new TextObject("{=PDdh1sBj}Name", null)));
			this.SortCategories.SelectedIndex = 0;
			this.SortOrders.AddItem(new SelectorItemVM(new TextObject("{=mOmFzU78}Ascending", null)));
			this.SortOrders.AddItem(new SelectorItemVM(new TextObject("{=FgFUsncP}Descending", null)));
			this.SortOrders.SelectedIndex = 0;
			this.RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType.Clothing);
			this.RefreshValues();
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00024128 File Offset: 0x00022328
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SortCategories.RefreshValues();
			this.SortOrders.RefreshValues();
			this.CosmeticInfoErrorText = new TextObject("{=ehkVpzpa}Unable to get cosmetic information", null).ToString();
			this.AllCategoriesHint = new HintViewModel(new TextObject("{=yfa7tpbK}All", null), null);
			this.BodyCategoryHint = new HintViewModel(GameTexts.FindText("str_inventory_type_13", null), null);
			this.HeadCategoryHint = new HintViewModel(GameTexts.FindText("str_inventory_type_12", null), null);
			this.ShoulderCategoryHint = new HintViewModel(GameTexts.FindText("str_inventory_type_22", null), null);
			this.HandCategoryHint = new HintViewModel(GameTexts.FindText("str_inventory_type_15", null), null);
			this.LegCategoryHint = new HintViewModel(GameTexts.FindText("str_inventory_type_14", null), null);
			this.ResetPreviewHint = new HintViewModel(new TextObject("{=imUnCFgZ}Reset preview", null), null);
			this._allCosmetics.ForEach(delegate(MPArmoryCosmeticItemBaseVM c)
			{
				c.RefreshValues();
			});
			this.AvailableCategories.ApplyActionOnAllItems(delegate(MPArmoryCosmeticCategoryBaseVM c)
			{
				c.RefreshValues();
			});
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0002425C File Offset: 0x0002245C
		private void InitializeCallbacks()
		{
			MPArmoryClothingCosmeticCategoryVM.OnSelected += this.OnClothingCosmeticCategorySelected;
			MPArmoryTauntCosmeticCategoryVM.OnSelected += this.OnTauntCosmeticCategorySelected;
			MPArmoryCosmeticItemBaseVM.OnPreviewed += this.EquipItemOnHeroPreview;
			MPArmoryCosmeticItemBaseVM.OnEquipped += this.OnCosmeticEquipRequested;
			MPArmoryCosmeticTauntSlotVM.OnFocusChanged += this.OnTauntSlotFocusChanged;
			MPArmoryCosmeticTauntSlotVM.OnSelected += this.OnTauntSlotSelected;
			MPArmoryCosmeticTauntSlotVM.OnPreview += this.OnTauntSlotPreview;
			MPArmoryCosmeticTauntSlotVM.OnTauntEquipped += this.OnTauntItemEquipped;
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000242F4 File Offset: 0x000224F4
		private void FinalizeCallbacks()
		{
			MPArmoryClothingCosmeticCategoryVM.OnSelected -= this.OnClothingCosmeticCategorySelected;
			MPArmoryTauntCosmeticCategoryVM.OnSelected -= this.OnTauntCosmeticCategorySelected;
			MPArmoryCosmeticItemBaseVM.OnPreviewed -= this.EquipItemOnHeroPreview;
			MPArmoryCosmeticItemBaseVM.OnEquipped -= this.OnCosmeticEquipRequested;
			MPArmoryCosmeticTauntSlotVM.OnFocusChanged -= this.OnTauntSlotFocusChanged;
			MPArmoryCosmeticTauntSlotVM.OnSelected -= this.OnTauntSlotSelected;
			MPArmoryCosmeticTauntSlotVM.OnPreview -= this.OnTauntSlotPreview;
			MPArmoryCosmeticTauntSlotVM.OnTauntEquipped -= this.OnTauntItemEquipped;
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00024389 File Offset: 0x00022589
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.FinalizeCallbacks();
			this.AvailableCategories.ApplyActionOnAllItems(delegate(MPArmoryCosmeticCategoryBaseVM c)
			{
				c.OnFinalize();
			});
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x000243C4 File Offset: 0x000225C4
		public async void OnTick(float dt)
		{
			if (NetworkMain.GameClient == null)
			{
				this._isNetworkCosmeticsDirty = false;
				this._isLocalCosmeticsDirty = false;
			}
			if (!this._isSendingCosmeticData && !this._isRetrievingCosmeticData)
			{
				if (this._isNetworkCosmeticsDirty)
				{
					this.RefreshCosmeticInfoFromNetworkAux();
					this._isNetworkCosmeticsDirty = false;
				}
				if (this._isLocalCosmeticsDirty)
				{
					await this.UpdateUsedCosmeticsAux();
					this._isLocalCosmeticsDirty = false;
				}
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00024400 File Offset: 0x00022600
		private void InitializeCosmeticItemComparers()
		{
			this._itemComparers = new List<MPArmoryCosmeticsVM.CosmeticItemComparer>
			{
				new MPArmoryCosmeticsVM.CosmeticItemCategoryComparer(),
				new MPArmoryCosmeticsVM.CosmeticItemCostComparer(),
				new MPArmoryCosmeticsVM.CosmeticItemRarityComparer(),
				new MPArmoryCosmeticsVM.CosmeticItemNameComparer()
			};
			this._currentItemComparer = this._itemComparers[0];
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x00024458 File Offset: 0x00022658
		private void InitializeAllCosmetics()
		{
			this._tauntCategoriesLookup.Clear();
			this._tauntCategoriesLookup.Add(MPArmoryCosmeticsVM.TauntCategoryFlag.All, new MPArmoryTauntCosmeticCategoryVM(MPArmoryCosmeticsVM.TauntCategoryFlag.All));
			foreach (object obj in Enum.GetValues(typeof(MPArmoryCosmeticsVM.TauntCategoryFlag)))
			{
				MPArmoryCosmeticsVM.TauntCategoryFlag tauntCategoryFlag = (MPArmoryCosmeticsVM.TauntCategoryFlag)obj;
				if (tauntCategoryFlag > MPArmoryCosmeticsVM.TauntCategoryFlag.None && tauntCategoryFlag < MPArmoryCosmeticsVM.TauntCategoryFlag.All)
				{
					this._tauntCategoriesLookup.Add(tauntCategoryFlag, new MPArmoryTauntCosmeticCategoryVM(tauntCategoryFlag));
				}
			}
			this._clothingCategoriesLookup.Clear();
			for (MPArmoryCosmeticsVM.ClothingCategory clothingCategory = MPArmoryCosmeticsVM.ClothingCategory.ClothingCategoriesBegin; clothingCategory < MPArmoryCosmeticsVM.ClothingCategory.ClothingCategoriesEnd; clothingCategory++)
			{
				this._clothingCategoriesLookup.Add(clothingCategory, new MPArmoryClothingCosmeticCategoryVM(clothingCategory));
			}
			this._allCosmetics = new List<MPArmoryCosmeticItemBaseVM>();
			CosmeticsManager.Initialize(ModuleHelper.GetModuleFullPath("Native") + "ModuleData");
			List<CosmeticElement> list = CosmeticsManager.CosmeticElementsList.ToList<CosmeticElement>();
			for (int i = 0; i < list.Count; i++)
			{
				ClothingCosmeticElement clothingCosmeticElement;
				TauntCosmeticElement tauntCosmeticElement;
				if (list[i].Type == CosmeticsManager.CosmeticType.Clothing && (clothingCosmeticElement = list[i] as ClothingCosmeticElement) != null)
				{
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM = new MPArmoryCosmeticClothingItemVM(clothingCosmeticElement, clothingCosmeticElement.Id);
					mparmoryCosmeticClothingItemVM.IsUnlocked = clothingCosmeticElement.IsFree;
					mparmoryCosmeticClothingItemVM.IsSelectable = true;
					this._allCosmetics.Add(mparmoryCosmeticClothingItemVM);
					this._cosmeticItemsLookup.Add(clothingCosmeticElement.Id, mparmoryCosmeticClothingItemVM);
					this._clothingCategoriesLookup[mparmoryCosmeticClothingItemVM.ClothingCategory].AvailableCosmetics.Add(mparmoryCosmeticClothingItemVM);
					this._clothingCategoriesLookup[MPArmoryCosmeticsVM.ClothingCategory.ClothingCategoriesBegin].AvailableCosmetics.Add(mparmoryCosmeticClothingItemVM);
				}
				else if (list[i].Type == CosmeticsManager.CosmeticType.Taunt && (tauntCosmeticElement = list[i] as TauntCosmeticElement) != null)
				{
					MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM = new MPArmoryCosmeticTauntItemVM(tauntCosmeticElement.Id, tauntCosmeticElement, tauntCosmeticElement.Id);
					mparmoryCosmeticTauntItemVM.IsUnlocked = tauntCosmeticElement.IsFree;
					mparmoryCosmeticTauntItemVM.IsSelectable = true;
					this._allCosmetics.Add(mparmoryCosmeticTauntItemVM);
					this._cosmeticItemsLookup.Add(tauntCosmeticElement.Id, mparmoryCosmeticTauntItemVM);
					foreach (object obj2 in Enum.GetValues(typeof(MPArmoryCosmeticsVM.TauntCategoryFlag)))
					{
						MPArmoryCosmeticsVM.TauntCategoryFlag tauntCategoryFlag2 = (MPArmoryCosmeticsVM.TauntCategoryFlag)obj2;
						if (tauntCategoryFlag2 > MPArmoryCosmeticsVM.TauntCategoryFlag.None && tauntCategoryFlag2 <= MPArmoryCosmeticsVM.TauntCategoryFlag.All && (mparmoryCosmeticTauntItemVM.TauntCategory & tauntCategoryFlag2) != MPArmoryCosmeticsVM.TauntCategoryFlag.None)
						{
							this._tauntCategoriesLookup[tauntCategoryFlag2].AvailableCosmetics.Add(mparmoryCosmeticTauntItemVM);
						}
					}
				}
			}
			for (int j = 0; j < TauntCosmeticElement.MaxNumberOfTaunts; j++)
			{
				this.TauntSlots.Add(new MPArmoryCosmeticTauntSlotVM(j));
			}
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00024728 File Offset: 0x00022928
		private void OnClothingCosmeticCategorySelected(MPArmoryClothingCosmeticCategoryVM selectedCosmetic)
		{
			this.FilterClothingsByCategory(selectedCosmetic);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00024731 File Offset: 0x00022931
		private void OnTauntCosmeticCategorySelected(MPArmoryTauntCosmeticCategoryVM selectedCosmetic)
		{
			this.FilterTauntsByCategory(selectedCosmetic);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0002473C File Offset: 0x0002293C
		public void RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType type)
		{
			this._currentCosmeticType = type;
			this.AvailableCategories.Clear();
			if (type == CosmeticsManager.CosmeticType.Clothing)
			{
				using (Dictionary<MPArmoryCosmeticsVM.ClothingCategory, MPArmoryClothingCosmeticCategoryVM>.Enumerator enumerator = this._clothingCategoriesLookup.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<MPArmoryCosmeticsVM.ClothingCategory, MPArmoryClothingCosmeticCategoryVM> keyValuePair = enumerator.Current;
						this.AvailableCategories.Add(keyValuePair.Value);
					}
					goto IL_009B;
				}
			}
			if (type == CosmeticsManager.CosmeticType.Taunt)
			{
				foreach (KeyValuePair<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM> keyValuePair2 in this._tauntCategoriesLookup)
				{
					this.AvailableCategories.Add(keyValuePair2.Value);
				}
			}
			IL_009B:
			if (this.AvailableCategories.Count > 0)
			{
				if (type == CosmeticsManager.CosmeticType.Clothing && this._currentClothingCategory != MPArmoryCosmeticsVM.ClothingCategory.Invalid)
				{
					this.FilterClothingsByCategory(this._clothingCategoriesLookup[this._currentClothingCategory]);
					return;
				}
				if (type == CosmeticsManager.CosmeticType.Taunt)
				{
					MPArmoryCosmeticsVM.TauntCategoryFlag tauntCategoryFlag = ((this._currentTauntCategory != MPArmoryCosmeticsVM.TauntCategoryFlag.None) ? this._currentTauntCategory : MPArmoryCosmeticsVM.TauntCategoryFlag.All);
					this.FilterTauntsByCategory(this._tauntCategoriesLookup[tauntCategoryFlag]);
				}
			}
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00024860 File Offset: 0x00022A60
		public void RefreshPlayerData(PlayerData playerData)
		{
			this.Loot = playerData.Gold;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0002486E File Offset: 0x00022A6E
		public void RefreshCosmeticInfoFromNetwork()
		{
			this._isNetworkCosmeticsDirty = true;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00024878 File Offset: 0x00022A78
		private void RefreshCosmeticInfoFromNetworkAux()
		{
			this._isRetrievingCosmeticData = true;
			if (NetworkMain.GameClient.PlayerData == null)
			{
				this._isRetrievingCosmeticData = false;
				return;
			}
			this.IsLoading = true;
			this.HasCosmeticInfoReceived = true;
			this.IsLoading = false;
			LobbyClient gameClient = NetworkMain.GameClient;
			string text;
			if (gameClient == null)
			{
				text = null;
			}
			else
			{
				PlayerData playerData = gameClient.PlayerData;
				text = ((playerData != null) ? playerData.UserId.ToString() : null);
			}
			string text2 = text;
			LobbyClient gameClient2 = NetworkMain.GameClient;
			IReadOnlyDictionary<string, List<string>> readOnlyDictionary = ((gameClient2 != null) ? gameClient2.UsedCosmetics : null);
			LobbyClient gameClient3 = NetworkMain.GameClient;
			List<string> list;
			if (gameClient3 == null)
			{
				list = null;
			}
			else
			{
				IReadOnlyList<string> ownedCosmetics = gameClient3.OwnedCosmetics;
				list = ((ownedCosmetics != null) ? ownedCosmetics.ToList<string>() : null);
			}
			List<string> list2 = list;
			if (text2 == null || readOnlyDictionary == null || list2 == null)
			{
				this._isRetrievingCosmeticData = false;
				return;
			}
			this._ownedCosmetics = list2;
			MBReadOnlyList<TauntIndexData> tauntIndicesForPlayer = MultiplayerLocalDataManager.Instance.TauntSlotData.GetTauntIndicesForPlayer(text2);
			this.RefreshTaunts(text2, tauntIndicesForPlayer);
			this._usedCosmetics = new Dictionary<string, List<string>>();
			foreach (KeyValuePair<string, List<string>> keyValuePair in readOnlyDictionary)
			{
				this._usedCosmetics.Add(keyValuePair.Key, new List<string>());
				foreach (string text3 in readOnlyDictionary[keyValuePair.Key])
				{
					this._usedCosmetics[keyValuePair.Key].Add(text3);
				}
			}
			this.RefreshSelectedClass(this._selectedClass, this._getSelectedPerks());
			this._isRetrievingCosmeticData = false;
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00024A14 File Offset: 0x00022C14
		private async Task<bool> UpdateUsedCosmeticsAux()
		{
			this._isSendingCosmeticData = true;
			IReadOnlyDictionary<string, List<string>> usedCosmetics = NetworkMain.GameClient.UsedCosmetics;
			Dictionary<string, List<ValueTuple<string, bool>>> dictionary = new Dictionary<string, List<ValueTuple<string, bool>>>();
			foreach (string text in this._usedCosmetics.Keys)
			{
				dictionary.Add(text, new List<ValueTuple<string, bool>>());
			}
			foreach (KeyValuePair<string, List<string>> keyValuePair in usedCosmetics)
			{
				foreach (string text2 in keyValuePair.Value)
				{
					if (!this._usedCosmetics[keyValuePair.Key].Contains(text2))
					{
						dictionary[keyValuePair.Key].Add(new ValueTuple<string, bool>(text2, false));
					}
				}
			}
			foreach (KeyValuePair<string, List<string>> keyValuePair2 in this._usedCosmetics)
			{
				if (!usedCosmetics.ContainsKey(keyValuePair2.Key))
				{
					using (List<string>.Enumerator enumerator3 = keyValuePair2.Value.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							string text3 = enumerator3.Current;
							dictionary[keyValuePair2.Key].Add(new ValueTuple<string, bool>(text3, true));
						}
						continue;
					}
				}
				foreach (string text4 in keyValuePair2.Value)
				{
					if (!usedCosmetics[keyValuePair2.Key].Contains(text4))
					{
						dictionary[keyValuePair2.Key].Add(new ValueTuple<string, bool>(text4, true));
					}
				}
			}
			foreach (KeyValuePair<string, List<ValueTuple<string, bool>>> keyValuePair3 in dictionary)
			{
				List<ItemObject.ItemTypeEnum> list = new List<ItemObject.ItemTypeEnum>();
				foreach (ValueTuple<string, bool> valueTuple in keyValuePair3.Value)
				{
					string item = valueTuple.Item1;
					MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM;
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
					if (valueTuple.Item2 && this._cosmeticItemsLookup.TryGetValue(item, out mparmoryCosmeticItemBaseVM) && (mparmoryCosmeticClothingItemVM = mparmoryCosmeticItemBaseVM as MPArmoryCosmeticClothingItemVM) != null)
					{
						ItemObject.ItemTypeEnum itemType = mparmoryCosmeticClothingItemVM.EquipmentElement.Item.ItemType;
						list.Add(itemType);
					}
				}
			}
			List<TauntIndexData> list2 = new List<TauntIndexData>();
			for (int i = 0; i < this.TauntSlots.Count; i++)
			{
				MPArmoryCosmeticTauntItemVM assignedTauntItem = this.TauntSlots[i].AssignedTauntItem;
				if (assignedTauntItem != null)
				{
					TauntIndexData tauntIndexData = new TauntIndexData(assignedTauntItem.TauntID, i);
					list2.Add(tauntIndexData);
				}
			}
			bool flag = false;
			LobbyClient gameClient = NetworkMain.GameClient;
			string text5;
			if (gameClient == null)
			{
				text5 = null;
			}
			else
			{
				PlayerData playerData = gameClient.PlayerData;
				text5 = ((playerData != null) ? playerData.UserId.ToString() : null);
			}
			string text6 = text5;
			if (text6 != null)
			{
				MultiplayerLocalDataManager.Instance.TauntSlotData.SetTauntIndicesForPlayer(text6, list2);
				flag = await NetworkMain.GameClient.UpdateUsedCosmeticItems(dictionary);
			}
			this._isSendingCosmeticData = false;
			return flag;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00024A5C File Offset: 0x00022C5C
		public void RefreshSelectedClass(MultiplayerClassDivisions.MPHeroClass selectedClass, List<IReadOnlyPerkObject> selectedPerks)
		{
			this._selectedClass = selectedClass;
			if (this._selectedClass == null)
			{
				return;
			}
			this._selectedClassDefaultEquipment = this._selectedClass.HeroCharacter.Equipment.Clone(false);
			if (selectedPerks != null)
			{
				MPArmoryVM.ApplyPerkEffectsToEquipment(ref this._selectedClassDefaultEquipment, selectedPerks);
			}
			this._selectedTroopID = this._selectedClass.StringId;
			MPArmoryCosmeticCategoryBaseVM activeCategory = this.ActiveCategory;
			if (activeCategory != null)
			{
				activeCategory.Sort(this._currentItemComparer);
			}
			if (this._ownedCosmetics != null)
			{
				using (List<string>.Enumerator enumerator = this._ownedCosmetics.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string ownedCosmeticID = enumerator.Current;
						MPArmoryCosmeticCategoryBaseVM activeCategory2 = this.ActiveCategory;
						MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM = ((activeCategory2 != null) ? activeCategory2.AvailableCosmetics.FirstOrDefault<MPArmoryCosmeticItemBaseVM>((MPArmoryCosmeticItemBaseVM c) => c.CosmeticID == ownedCosmeticID) : null);
						if (mparmoryCosmeticItemBaseVM != null)
						{
							mparmoryCosmeticItemBaseVM.IsUnlocked = true;
						}
					}
				}
			}
			this.RefreshFilters();
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00024B54 File Offset: 0x00022D54
		private void EquipItemOnHeroPreview(MPArmoryCosmeticItemBaseVM itemVM)
		{
			if (itemVM == null)
			{
				Debug.FailedAssert("Previewing null item", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Armory\\MPArmoryCosmeticsVM.cs", "EquipItemOnHeroPreview", 531);
				return;
			}
			Action<MPArmoryCosmeticItemBaseVM> onCosmeticPreview = MPArmoryCosmeticsVM.OnCosmeticPreview;
			if (onCosmeticPreview == null)
			{
				return;
			}
			onCosmeticPreview(itemVM);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00024B83 File Offset: 0x00022D83
		private void OnCosmeticEquipRequested(MPArmoryCosmeticItemBaseVM cosmeticItemVM)
		{
			if (cosmeticItemVM.CosmeticType == CosmeticsManager.CosmeticType.Clothing)
			{
				this.OnItemEquipRequested((MPArmoryCosmeticClothingItemVM)cosmeticItemVM);
				return;
			}
			if (cosmeticItemVM.CosmeticType == CosmeticsManager.CosmeticType.Taunt)
			{
				this.OnTauntEquipRequested((MPArmoryCosmeticTauntItemVM)cosmeticItemVM);
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00024BB0 File Offset: 0x00022DB0
		private void OnItemEquipRequested(MPArmoryCosmeticClothingItemVM itemVM)
		{
			if (itemVM.IsUsed && !itemVM.Cosmetic.IsFree && this.ActiveCategory != null && this.ActiveCategory.CosmeticType == CosmeticsManager.CosmeticType.Clothing && itemVM.ClothingCosmeticElement.ReplaceItemsId.Count > 0 && this._selectedClassDefaultEquipment != null)
			{
				for (int i = 0; i < itemVM.ClothingCosmeticElement.ReplaceItemsId.Count; i++)
				{
					string replacedItemId = itemVM.ClothingCosmeticElement.ReplaceItemsId[i];
					Func<MPArmoryCosmeticItemBaseVM, bool> <>9__1;
					for (EquipmentIndex equipmentIndex = EquipmentIndex.NumAllWeaponSlots; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
					{
						ItemObject item = this._selectedClassDefaultEquipment[equipmentIndex].Item;
						if (((item != null) ? item.StringId : null) == replacedItemId)
						{
							IEnumerable<MPArmoryCosmeticItemBaseVM> availableCosmetics = this.ActiveCategory.AvailableCosmetics;
							Func<MPArmoryCosmeticItemBaseVM, bool> func;
							if ((func = <>9__1) == null)
							{
								func = (<>9__1 = (MPArmoryCosmeticItemBaseVM c) => c.Cosmetic.Id == replacedItemId);
							}
							MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM = availableCosmetics.FirstOrDefault<MPArmoryCosmeticItemBaseVM>(func) as MPArmoryCosmeticClothingItemVM;
							if (mparmoryCosmeticClothingItemVM != null)
							{
								this.OnClothingItemEquipped(mparmoryCosmeticClothingItemVM, true);
								this._isLocalCosmeticsDirty = true;
								return;
							}
						}
					}
				}
			}
			if (itemVM.ClothingCosmeticElement.ReplaceItemless.Any<Tuple<string, string>>((Tuple<string, string> r) => r.Item1 == this._selectedClass.StringId))
			{
				if (itemVM.IsUsed)
				{
					itemVM.IsUsed = false;
					Dictionary<string, List<string>> usedCosmetics = this._usedCosmetics;
					if (usedCosmetics != null)
					{
						usedCosmetics[this._selectedTroopID].Remove(itemVM.CosmeticID);
					}
					Action<MPArmoryCosmeticItemBaseVM> onRemoveCosmeticFromPreview = MPArmoryCosmeticsVM.OnRemoveCosmeticFromPreview;
					if (onRemoveCosmeticFromPreview != null)
					{
						onRemoveCosmeticFromPreview(itemVM);
					}
				}
				else
				{
					itemVM.ActionText = itemVM.UnequipText;
					this.OnClothingItemEquipped(itemVM, true);
				}
			}
			else
			{
				this.OnClothingItemEquipped(itemVM, true);
			}
			this._isLocalCosmeticsDirty = true;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00024D64 File Offset: 0x00022F64
		private void OnClothingItemEquipped(MPArmoryCosmeticClothingItemVM itemVM, bool forceRemove = true)
		{
			this.EquipItemOnHeroPreview(itemVM);
			if (!this._usedCosmetics.ContainsKey(this._selectedTroopID))
			{
				this._usedCosmetics.Add(this._selectedTroopID, new List<string>());
			}
			if (itemVM.CosmeticID != string.Empty && !this._usedCosmetics[this._selectedTroopID].Contains(itemVM.CosmeticID))
			{
				this._usedCosmetics[this._selectedTroopID].Add(itemVM.CosmeticID);
			}
			foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM in this.ActiveCategory.AvailableCosmetics)
			{
				if (((MPArmoryCosmeticClothingItemVM)mparmoryCosmeticItemBaseVM).EquipmentElement.Item.ItemType == itemVM.EquipmentElement.Item.ItemType)
				{
					mparmoryCosmeticItemBaseVM.IsUsed = false;
					if (itemVM.Cosmetic.Id != mparmoryCosmeticItemBaseVM.Cosmetic.Id && forceRemove)
					{
						List<string> list = this._usedCosmetics[this._selectedTroopID];
						if (list != null)
						{
							list.Remove(mparmoryCosmeticItemBaseVM.CosmeticID);
						}
					}
				}
			}
			itemVM.IsUsed = true;
			if (this.ActiveCategory != null)
			{
				this.UpdateKeyBindingsForCategory(this.ActiveCategory);
			}
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00024EC0 File Offset: 0x000230C0
		public void ClearTauntSelections()
		{
			if (this.SelectedTauntItem == null && this.SelectedTauntSlot == null)
			{
				return;
			}
			this.OnTauntEquipRequested(null);
			this.OnTauntSlotSelected(null);
			foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM in this.TauntSlots)
			{
				mparmoryCosmeticTauntSlotVM.IsAcceptingTaunts = false;
				mparmoryCosmeticTauntSlotVM.IsFocused = false;
			}
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00024F34 File Offset: 0x00023134
		private void OnTauntEquipRequested(MPArmoryCosmeticTauntItemVM tauntItem)
		{
			if (this.SelectedTauntItem != null)
			{
				if (this.SelectedTauntItem == tauntItem)
				{
					this.ClearTauntSelections();
					return;
				}
				this.SelectedTauntItem.IsSelected = false;
			}
			this.SelectedTauntItem = tauntItem;
			if (this.SelectedTauntItem != null)
			{
				MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM = null;
				for (int i = 0; i < this.TauntSlots.Count; i++)
				{
					MPArmoryCosmeticTauntItemVM assignedTauntItem = this.TauntSlots[i].AssignedTauntItem;
					if (((assignedTauntItem != null) ? assignedTauntItem.CosmeticID : null) == tauntItem.CosmeticID)
					{
						mparmoryCosmeticTauntSlotVM = this.TauntSlots[i];
						break;
					}
				}
				if (mparmoryCosmeticTauntSlotVM != null)
				{
					this.SelectedTauntItem = null;
					mparmoryCosmeticTauntSlotVM.AssignTauntItem(null, false);
					this.ClearTauntSelections();
					this._isLocalCosmeticsDirty = true;
					return;
				}
				this.SelectedTauntItem.IsSelected = true;
				this.SelectedTauntItem.ActionText = this.SelectedTauntItem.CancelEquipText;
				using (IEnumerator<MPArmoryCosmeticItemBaseVM> enumerator = this.ActiveCategory.AvailableCosmetics.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM = enumerator.Current;
						mparmoryCosmeticItemBaseVM.IsSelectable = mparmoryCosmeticItemBaseVM == this.SelectedTauntItem;
					}
					goto IL_0137;
				}
			}
			foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM2 in this.ActiveCategory.AvailableCosmetics)
			{
				mparmoryCosmeticItemBaseVM2.IsSelectable = true;
			}
			IL_0137:
			foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM2 in this.TauntSlots)
			{
				mparmoryCosmeticTauntSlotVM2.IsAcceptingTaunts = mparmoryCosmeticTauntSlotVM2.AssignedTauntItem != tauntItem;
			}
			Action onTauntAssignmentRefresh = MPArmoryCosmeticsVM.OnTauntAssignmentRefresh;
			if (onTauntAssignmentRefresh == null)
			{
				return;
			}
			onTauntAssignmentRefresh();
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x000250E8 File Offset: 0x000232E8
		private void OnTauntSlotFocusChanged(MPArmoryCosmeticTauntSlotVM changedSlot, bool isFocused)
		{
			foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM in this.TauntSlots)
			{
				mparmoryCosmeticTauntSlotVM.IsFocused = isFocused && changedSlot == mparmoryCosmeticTauntSlotVM;
				if (mparmoryCosmeticTauntSlotVM.IsAcceptingTaunts)
				{
					mparmoryCosmeticTauntSlotVM.EmptySlotKeyVisual.SetForcedVisibility(new bool?(false));
					mparmoryCosmeticTauntSlotVM.SelectKeyVisual.SetForcedVisibility(null);
				}
				else if (mparmoryCosmeticTauntSlotVM.AssignedTauntItem != null)
				{
					mparmoryCosmeticTauntSlotVM.EmptySlotKeyVisual.SetForcedVisibility(null);
					mparmoryCosmeticTauntSlotVM.SelectKeyVisual.SetForcedVisibility(new bool?(false));
				}
				else
				{
					mparmoryCosmeticTauntSlotVM.EmptySlotKeyVisual.SetForcedVisibility(new bool?(false));
				}
				bool? flag = ((!mparmoryCosmeticTauntSlotVM.IsAcceptingTaunts && mparmoryCosmeticTauntSlotVM.AssignedTauntItem != null) ? null : new bool?(false));
				bool? flag2 = ((mparmoryCosmeticTauntSlotVM.AssignedTauntItem != null || mparmoryCosmeticTauntSlotVM.IsAcceptingTaunts) ? null : new bool?(false));
				InputKeyItemVM emptySlotKeyVisual = mparmoryCosmeticTauntSlotVM.EmptySlotKeyVisual;
				if (emptySlotKeyVisual != null)
				{
					emptySlotKeyVisual.SetForcedVisibility(flag);
				}
				InputKeyItemVM selectKeyVisual = mparmoryCosmeticTauntSlotVM.SelectKeyVisual;
				if (selectKeyVisual != null)
				{
					selectKeyVisual.SetForcedVisibility(flag2);
				}
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0002522C File Offset: 0x0002342C
		private void OnTauntSlotPreview(MPArmoryCosmeticTauntSlotVM previewSlot)
		{
			if (previewSlot != null)
			{
				MPArmoryCosmeticTauntItemVM assignedTauntItem = previewSlot.AssignedTauntItem;
				if (assignedTauntItem == null)
				{
					return;
				}
				assignedTauntItem.ExecutePreview();
			}
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00025244 File Offset: 0x00023444
		private void OnTauntSlotSelected(MPArmoryCosmeticTauntSlotVM selectedSlot)
		{
			if (this.SelectedTauntSlot == null && this.SelectedTauntItem == null && selectedSlot != null && selectedSlot.IsEmpty)
			{
				return;
			}
			MPArmoryCosmeticTauntSlotVM selectedTauntSlot = this.SelectedTauntSlot;
			this.SelectedTauntSlot = selectedSlot;
			if (selectedTauntSlot != null)
			{
				selectedTauntSlot.IsSelected = false;
			}
			if (this.SelectedTauntSlot != null)
			{
				this.SelectedTauntSlot.IsSelected = true;
			}
			if (((selectedSlot != null) ? selectedSlot.AssignedTauntItem : null) != null)
			{
				bool flag = false;
				for (int i = 0; i < this.ActiveCategory.AvailableCosmetics.Count; i++)
				{
					if (this.ActiveCategory.AvailableCosmetics[i] == selectedSlot.AssignedTauntItem)
					{
						flag = true;
						break;
					}
				}
				MPArmoryTauntCosmeticCategoryVM mparmoryTauntCosmeticCategoryVM;
				if (!flag && this._tauntCategoriesLookup.TryGetValue(MPArmoryCosmeticsVM.TauntCategoryFlag.All, out mparmoryTauntCosmeticCategoryVM))
				{
					this.FilterTauntsByCategory(mparmoryTauntCosmeticCategoryVM);
				}
			}
			foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM in this.ActiveCategory.AvailableCosmetics)
			{
				mparmoryCosmeticItemBaseVM.IsSelectable = selectedSlot == null || mparmoryCosmeticItemBaseVM == ((selectedSlot != null) ? selectedSlot.AssignedTauntItem : null);
			}
			if (this.SelectedTauntItem == null)
			{
				MPArmoryCosmeticTauntSlotVM selectedTauntSlot2 = this.SelectedTauntSlot;
				if (selectedTauntSlot2 != null && !selectedTauntSlot2.IsEmpty)
				{
					foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM in this.TauntSlots)
					{
						mparmoryCosmeticTauntSlotVM.IsAcceptingTaunts = mparmoryCosmeticTauntSlotVM != selectedSlot;
					}
				}
			}
			if (this.SelectedTauntSlot != null)
			{
				bool flag2 = false;
				if (this.SelectedTauntItem != null && this.SelectedTauntSlot.AssignedTauntItem != this.SelectedTauntItem)
				{
					MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM2 = null;
					for (int j = 0; j < this.TauntSlots.Count; j++)
					{
						if (this.TauntSlots[j].AssignedTauntItem == this.SelectedTauntItem)
						{
							mparmoryCosmeticTauntSlotVM2 = this.TauntSlots[j];
							break;
						}
					}
					if (mparmoryCosmeticTauntSlotVM2 != null)
					{
						MPArmoryCosmeticTauntItemVM assignedTauntItem = this.SelectedTauntSlot.AssignedTauntItem;
						MPArmoryCosmeticTauntItemVM assignedTauntItem2 = mparmoryCosmeticTauntSlotVM2.AssignedTauntItem;
						this.SelectedTauntSlot.AssignTauntItem(assignedTauntItem2, true);
						mparmoryCosmeticTauntSlotVM2.AssignTauntItem(assignedTauntItem, true);
					}
					else
					{
						this.SelectedTauntSlot.AssignTauntItem(this.SelectedTauntItem, false);
					}
					flag2 = true;
					this.ClearTauntSelections();
				}
				else if (selectedTauntSlot != null && !selectedTauntSlot.IsEmpty && this.SelectedTauntSlot != selectedTauntSlot)
				{
					MPArmoryCosmeticTauntItemVM assignedTauntItem3 = selectedTauntSlot.AssignedTauntItem;
					MPArmoryCosmeticTauntItemVM assignedTauntItem4 = this.SelectedTauntSlot.AssignedTauntItem;
					this.SelectedTauntSlot.AssignTauntItem(assignedTauntItem3, true);
					selectedTauntSlot.AssignTauntItem(assignedTauntItem4, true);
					flag2 = true;
					this.ClearTauntSelections();
				}
				if (flag2)
				{
					this._isLocalCosmeticsDirty = true;
				}
			}
			Action onTauntAssignmentRefresh = MPArmoryCosmeticsVM.OnTauntAssignmentRefresh;
			if (onTauntAssignmentRefresh == null)
			{
				return;
			}
			onTauntAssignmentRefresh();
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x000254EC File Offset: 0x000236EC
		private void OnTauntItemEquipped(MPArmoryCosmeticTauntSlotVM equippedSlot, MPArmoryCosmeticTauntItemVM previousTauntItem, bool isSwapping)
		{
			for (int i = 0; i < this.TauntSlots.Count; i++)
			{
				MPArmoryCosmeticTauntItemVM assignedTauntItem = this.TauntSlots[i].AssignedTauntItem;
				if (assignedTauntItem != null && !assignedTauntItem.IsUnlocked)
				{
					Debug.FailedAssert("Assigned a taunt without ownership: " + assignedTauntItem.TauntID, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Armory\\MPArmoryCosmeticsVM.cs", "OnTauntItemEquipped", 874);
				}
			}
			this._isLocalCosmeticsDirty = true;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00025557 File Offset: 0x00023757
		public void OnItemObtained(string cosmeticID, int finalLoot)
		{
			this._ownedCosmetics.Add(cosmeticID);
			this.RefreshCosmeticInfoFromNetwork();
			this.Loot = finalLoot;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00025574 File Offset: 0x00023774
		private void OnSortCategoryUpdated(SelectorVM<SelectorItemVM> selector)
		{
			if (this.SortCategories.SelectedIndex == -1)
			{
				this.SortCategories.SelectedIndex = 0;
			}
			this._currentItemComparer = this._itemComparers[selector.SelectedIndex];
			MPArmoryCosmeticCategoryBaseVM activeCategory = this.ActiveCategory;
			if (activeCategory == null)
			{
				return;
			}
			activeCategory.Sort(this._currentItemComparer);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000255C8 File Offset: 0x000237C8
		private void OnSortOrderUpdated(SelectorVM<SelectorItemVM> selector)
		{
			if (this.SortOrders.SelectedIndex == -1)
			{
				this.SortOrders.SelectedIndex = 0;
			}
			foreach (MPArmoryCosmeticsVM.CosmeticItemComparer cosmeticItemComparer in this._itemComparers)
			{
				cosmeticItemComparer.SetSortMode(selector.SelectedIndex == 0);
			}
			MPArmoryCosmeticCategoryBaseVM activeCategory = this.ActiveCategory;
			if (activeCategory == null)
			{
				return;
			}
			activeCategory.Sort(this._currentItemComparer);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00025654 File Offset: 0x00023854
		private void RefreshFilters()
		{
			MPArmoryClothingCosmeticCategoryVM mparmoryClothingCosmeticCategoryVM;
			if (this._currentCosmeticType == CosmeticsManager.CosmeticType.Clothing && this._clothingCategoriesLookup.TryGetValue(this._currentClothingCategory, out mparmoryClothingCosmeticCategoryVM))
			{
				this.FilterClothingsByCategory(mparmoryClothingCosmeticCategoryVM);
				return;
			}
			MPArmoryTauntCosmeticCategoryVM mparmoryTauntCosmeticCategoryVM;
			if (this._currentCosmeticType == CosmeticsManager.CosmeticType.Taunt && this._tauntCategoriesLookup.TryGetValue(this._currentTauntCategory, out mparmoryTauntCosmeticCategoryVM))
			{
				this.FilterTauntsByCategory(mparmoryTauntCosmeticCategoryVM);
			}
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x000256AC File Offset: 0x000238AC
		private void FilterClothingsByCategory(MPArmoryClothingCosmeticCategoryVM clothingCategory)
		{
			if (this._currentCosmeticType != CosmeticsManager.CosmeticType.Clothing)
			{
				this.RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType.Clothing);
				return;
			}
			if (clothingCategory == null)
			{
				Debug.FailedAssert("Trying to filter by null clothing category", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Armory\\MPArmoryCosmeticsVM.cs", "FilterClothingsByCategory", 937);
				return;
			}
			this._currentClothingCategory = clothingCategory.ClothingCategory;
			foreach (KeyValuePair<MPArmoryCosmeticsVM.ClothingCategory, MPArmoryClothingCosmeticCategoryVM> keyValuePair in this._clothingCategoriesLookup)
			{
				keyValuePair.Value.IsSelected = false;
			}
			clothingCategory.SetDefaultEquipments(this._selectedClassDefaultEquipment);
			this.ActiveCategory = clothingCategory;
			if (this._selectedClass != null)
			{
				foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM in this._allCosmetics)
				{
					if (mparmoryCosmeticItemBaseVM.CosmeticType == CosmeticsManager.CosmeticType.Clothing)
					{
						clothingCategory.ReplaceCosmeticWithDefaultItem((MPArmoryCosmeticClothingItemVM)mparmoryCosmeticItemBaseVM, clothingCategory.ClothingCategory, this._selectedClass, this._ownedCosmetics);
					}
				}
			}
			this.ActiveCategory.Sort(this._currentItemComparer);
			this.RefreshEquipment();
			if (this.ActiveCategory != null)
			{
				this.ActiveCategory.IsSelected = true;
				this.UpdateKeyBindingsForCategory(this.ActiveCategory);
			}
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x000257F4 File Offset: 0x000239F4
		private void FilterTauntsByCategory(MPArmoryTauntCosmeticCategoryVM tauntCategory)
		{
			if (this._currentCosmeticType != CosmeticsManager.CosmeticType.Taunt)
			{
				this.RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType.Taunt);
			}
			this._currentTauntCategory = tauntCategory.TauntCategory;
			foreach (KeyValuePair<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM> keyValuePair in this._tauntCategoriesLookup)
			{
				keyValuePair.Value.IsSelected = false;
			}
			this.ActiveCategory = tauntCategory;
			if (this.ActiveCategory != null)
			{
				this.ActiveCategory.IsSelected = true;
				this.UpdateKeyBindingsForCategory(this.ActiveCategory);
			}
			this.ActiveCategory.Sort(this._currentItemComparer);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x000258A4 File Offset: 0x00023AA4
		private void RefreshEquipment()
		{
			Dictionary<EquipmentIndex, bool> dictionary = new Dictionary<EquipmentIndex, bool>();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.NumAllWeaponSlots; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
			{
				dictionary.Add(equipmentIndex, false);
			}
			List<EquipmentElement> list = new List<EquipmentElement>();
			using (IEnumerator<MPArmoryCosmeticItemBaseVM> enumerator = this.ActiveCategory.AvailableCosmetics.Where<MPArmoryCosmeticItemBaseVM>((MPArmoryCosmeticItemBaseVM c) => c.Cosmetic.Rarity == CosmeticsManager.CosmeticRarity.Default).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
					if ((mparmoryCosmeticClothingItemVM = enumerator.Current as MPArmoryCosmeticClothingItemVM) != null)
					{
						this.OnClothingItemEquipped(mparmoryCosmeticClothingItemVM, false);
						dictionary[mparmoryCosmeticClothingItemVM.EquipmentElement.Item.GetCosmeticEquipmentIndex()] = true;
						list.Add(mparmoryCosmeticClothingItemVM.EquipmentElement);
					}
				}
			}
			if (!string.IsNullOrEmpty(this._selectedTroopID))
			{
				Dictionary<string, List<string>> usedCosmetics = this._usedCosmetics;
				if (usedCosmetics != null && usedCosmetics.ContainsKey(this._selectedTroopID))
				{
					Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
					foreach (string text in this._usedCosmetics.Keys)
					{
						List<string> list2 = new List<string>();
						foreach (string text2 in this._usedCosmetics[text])
						{
							list2.Add(text2);
						}
						dictionary2.Add(text, list2);
					}
					using (List<string>.Enumerator enumerator3 = dictionary2[this._selectedTroopID].GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							string cosmeticID = enumerator3.Current;
							MPArmoryCosmeticClothingItemVM cosmeticItem = (MPArmoryCosmeticClothingItemVM)this._allCosmetics.First<MPArmoryCosmeticItemBaseVM>((MPArmoryCosmeticItemBaseVM c) => c.CosmeticID == cosmeticID);
							if (cosmeticItem != null)
							{
								EquipmentIndex cosmeticEquipmentIndex = cosmeticItem.EquipmentElement.Item.GetCosmeticEquipmentIndex();
								if (!(cosmeticItem.Cosmetic as ClothingCosmeticElement).ReplaceItemless.IsEmpty<Tuple<string, string>>() || !this._selectedClassDefaultEquipment[cosmeticEquipmentIndex].IsEmpty)
								{
									EquipmentElement equipmentElement = list.FirstOrDefault<EquipmentElement>((EquipmentElement i) => i.Item.GetCosmeticEquipmentIndex() == cosmeticItem.EquipmentElement.Item.GetCosmeticEquipmentIndex());
									if (!equipmentElement.IsEmpty)
									{
										list.Remove(equipmentElement);
										list.Add(cosmeticItem.EquipmentElement);
									}
									this.OnClothingItemEquipped(cosmeticItem, true);
									dictionary[cosmeticEquipmentIndex] = true;
								}
							}
						}
					}
				}
			}
			foreach (EquipmentIndex equipmentIndex2 in dictionary.Keys)
			{
				if (!dictionary[equipmentIndex2])
				{
					MPArmoryClothingCosmeticCategoryVM mparmoryClothingCosmeticCategoryVM = (MPArmoryClothingCosmeticCategoryVM)this.ActiveCategory;
					if (mparmoryClothingCosmeticCategoryVM != null)
					{
						mparmoryClothingCosmeticCategoryVM.OnEquipmentRefreshed(equipmentIndex2);
					}
				}
			}
			Action<List<EquipmentElement>> onEquipmentRefreshed = MPArmoryCosmeticsVM.OnEquipmentRefreshed;
			if (onEquipmentRefreshed == null)
			{
				return;
			}
			onEquipmentRefreshed(list);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00025C20 File Offset: 0x00023E20
		private void RefreshTaunts(string playerId, MBReadOnlyList<TauntIndexData> registeredTaunts)
		{
			List<TauntIndexData> list = ((registeredTaunts != null) ? registeredTaunts.ToList<TauntIndexData>() : null);
			if (list == null)
			{
				list = new List<TauntIndexData>();
				foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM in (from c in this._tauntCategoriesLookup.SelectMany<KeyValuePair<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM>, MPArmoryCosmeticItemBaseVM>((KeyValuePair<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM> c) => c.Value.AvailableCosmetics)
					where c.Cosmetic.IsFree
					select c).Distinct<MPArmoryCosmeticItemBaseVM>())
				{
					TauntIndexData tauntIndexData = new TauntIndexData(mparmoryCosmeticItemBaseVM.CosmeticID, list.Count);
					list.Add(tauntIndexData);
				}
			}
			foreach (KeyValuePair<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM> keyValuePair in this._tauntCategoriesLookup)
			{
				foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM2 in keyValuePair.Value.AvailableCosmetics)
				{
					mparmoryCosmeticItemBaseVM2.IsUnlocked = mparmoryCosmeticItemBaseVM2.Cosmetic.IsFree || this._ownedCosmetics.Contains(mparmoryCosmeticItemBaseVM2.CosmeticID);
				}
			}
			for (int i = 0; i < this.TauntSlots.Count; i++)
			{
				this.TauntSlots[i].AssignTauntItem(null, false);
			}
			for (int j = 0; j < list.Count; j++)
			{
				string tauntId = list[j].TauntId;
				int tauntIndex = list[j].TauntIndex;
				MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM3;
				MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM;
				if (this._cosmeticItemsLookup.TryGetValue(tauntId, out mparmoryCosmeticItemBaseVM3) && (mparmoryCosmeticTauntItemVM = mparmoryCosmeticItemBaseVM3 as MPArmoryCosmeticTauntItemVM) != null)
				{
					if (!mparmoryCosmeticTauntItemVM.IsUnlocked)
					{
						Debug.FailedAssert("Trying to add non-owned cosmetic to taunt slot: " + mparmoryCosmeticTauntItemVM.TauntID, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Armory\\MPArmoryCosmeticsVM.cs", "RefreshTaunts", 1115);
					}
					else if (tauntIndex >= 0 && tauntIndex < this.TauntSlots.Count)
					{
						this.TauntSlots[tauntIndex].AssignTauntItem(mparmoryCosmeticTauntItemVM, false);
					}
				}
			}
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00025E70 File Offset: 0x00024070
		private void UpdateTauntAssignmentState()
		{
			this.IsTauntAssignmentActive = this.SelectedTauntItem != null || this.SelectedTauntSlot != null;
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00025E8C File Offset: 0x0002408C
		private void ExecuteRefreshCosmeticInfo()
		{
			this.RefreshCosmeticInfoFromNetwork();
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00025E94 File Offset: 0x00024094
		private void ExecuteResetPreview()
		{
			this.RefreshSelectedClass(this._selectedClass, this._getSelectedPerks());
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00025EB0 File Offset: 0x000240B0
		public void RefreshKeyBindings(HotKey actionKey, HotKey previewKey)
		{
			this.ActionInputKey = InputKeyItemVM.CreateFromHotKey(actionKey, false);
			this.PreviewInputKey = InputKeyItemVM.CreateFromHotKey(previewKey, false);
			for (int i = 0; i < this.AvailableCategories.Count; i++)
			{
				this.UpdateKeyBindingsForCategory(this.AvailableCategories[i]);
			}
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00025F00 File Offset: 0x00024100
		private void UpdateKeyBindingsForCategory(MPArmoryCosmeticCategoryBaseVM categoryVM)
		{
			if (this.ActionInputKey != null && this.PreviewInputKey != null)
			{
				for (int i = 0; i < categoryVM.AvailableCosmetics.Count; i++)
				{
					categoryVM.AvailableCosmetics[i].RefreshKeyBindings(this.ActionInputKey.HotKey, this.PreviewInputKey.HotKey);
				}
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00025F5A File Offset: 0x0002415A
		// (set) Token: 0x06000C2F RID: 3119 RVA: 0x00025F62 File Offset: 0x00024162
		[DataSourceProperty]
		public InputKeyItemVM ActionInputKey
		{
			get
			{
				return this._actionInputKey;
			}
			set
			{
				if (value != this._actionInputKey)
				{
					this._actionInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ActionInputKey");
				}
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00025F80 File Offset: 0x00024180
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x00025F88 File Offset: 0x00024188
		[DataSourceProperty]
		public InputKeyItemVM PreviewInputKey
		{
			get
			{
				return this._previewInputKey;
			}
			set
			{
				if (value != this._previewInputKey)
				{
					this._previewInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviewInputKey");
				}
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00025FA6 File Offset: 0x000241A6
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x00025FAE File Offset: 0x000241AE
		[DataSourceProperty]
		public int Loot
		{
			get
			{
				return this._loot;
			}
			set
			{
				if (value != this._loot)
				{
					this._loot = value;
					base.OnPropertyChangedWithValue(value, "Loot");
				}
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00025FCC File Offset: 0x000241CC
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x00025FD4 File Offset: 0x000241D4
		[DataSourceProperty]
		public bool IsLoading
		{
			get
			{
				return this._isLoading;
			}
			set
			{
				if (value != this._isLoading)
				{
					this._isLoading = value;
					base.OnPropertyChangedWithValue(value, "IsLoading");
				}
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00025FF2 File Offset: 0x000241F2
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x00025FFA File Offset: 0x000241FA
		[DataSourceProperty]
		public bool HasCosmeticInfoReceived
		{
			get
			{
				return this._hasCosmeticInfoReceived;
			}
			set
			{
				if (value != this._hasCosmeticInfoReceived)
				{
					this._hasCosmeticInfoReceived = value;
					base.OnPropertyChangedWithValue(value, "HasCosmeticInfoReceived");
				}
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x00026018 File Offset: 0x00024218
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x00026020 File Offset: 0x00024220
		[DataSourceProperty]
		public bool IsManagingTaunts
		{
			get
			{
				return this._isManagingTaunts;
			}
			set
			{
				if (value != this._isManagingTaunts)
				{
					this._isManagingTaunts = value;
					base.OnPropertyChangedWithValue(value, "IsManagingTaunts");
				}
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0002603E File Offset: 0x0002423E
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x00026046 File Offset: 0x00024246
		[DataSourceProperty]
		public bool IsTauntAssignmentActive
		{
			get
			{
				return this._isTauntAssignmentActive;
			}
			set
			{
				if (value != this._isTauntAssignmentActive)
				{
					this._isTauntAssignmentActive = value;
					base.OnPropertyChangedWithValue(value, "IsTauntAssignmentActive");
				}
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00026064 File Offset: 0x00024264
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x0002606C File Offset: 0x0002426C
		[DataSourceProperty]
		public string CosmeticInfoErrorText
		{
			get
			{
				return this._cosmeticInfoErrorText;
			}
			set
			{
				if (value != this._cosmeticInfoErrorText)
				{
					this._cosmeticInfoErrorText = value;
					base.OnPropertyChangedWithValue<string>(value, "CosmeticInfoErrorText");
				}
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0002608F File Offset: 0x0002428F
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x00026097 File Offset: 0x00024297
		[DataSourceProperty]
		public HintViewModel AllCategoriesHint
		{
			get
			{
				return this._allCategoriesHint;
			}
			set
			{
				if (value != this._allCategoriesHint)
				{
					this._allCategoriesHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AllCategoriesHint");
				}
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x000260B5 File Offset: 0x000242B5
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x000260BD File Offset: 0x000242BD
		[DataSourceProperty]
		public HintViewModel BodyCategoryHint
		{
			get
			{
				return this._bodyCategoryHint;
			}
			set
			{
				if (value != this._bodyCategoryHint)
				{
					this._bodyCategoryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BodyCategoryHint");
				}
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x000260DB File Offset: 0x000242DB
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x000260E3 File Offset: 0x000242E3
		[DataSourceProperty]
		public HintViewModel HeadCategoryHint
		{
			get
			{
				return this._headCategoryHint;
			}
			set
			{
				if (value != this._headCategoryHint)
				{
					this._headCategoryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HeadCategoryHint");
				}
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x00026101 File Offset: 0x00024301
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x00026109 File Offset: 0x00024309
		[DataSourceProperty]
		public HintViewModel ShoulderCategoryHint
		{
			get
			{
				return this._shoulderCategoryHint;
			}
			set
			{
				if (value != this._shoulderCategoryHint)
				{
					this._shoulderCategoryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ShoulderCategoryHint");
				}
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x00026127 File Offset: 0x00024327
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x0002612F File Offset: 0x0002432F
		[DataSourceProperty]
		public HintViewModel HandCategoryHint
		{
			get
			{
				return this._handCategoryHint;
			}
			set
			{
				if (value != this._handCategoryHint)
				{
					this._handCategoryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HandCategoryHint");
				}
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0002614D File Offset: 0x0002434D
		// (set) Token: 0x06000C49 RID: 3145 RVA: 0x00026155 File Offset: 0x00024355
		[DataSourceProperty]
		public HintViewModel LegCategoryHint
		{
			get
			{
				return this._legCategoryHint;
			}
			set
			{
				if (value != this._legCategoryHint)
				{
					this._legCategoryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LegCategoryHint");
				}
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x00026173 File Offset: 0x00024373
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x0002617B File Offset: 0x0002437B
		[DataSourceProperty]
		public HintViewModel ResetPreviewHint
		{
			get
			{
				return this._resetPreviewHint;
			}
			set
			{
				if (value != this._resetPreviewHint)
				{
					this._resetPreviewHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResetPreviewHint");
				}
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x00026199 File Offset: 0x00024399
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x000261A1 File Offset: 0x000243A1
		[DataSourceProperty]
		public MPArmoryCosmeticCategoryBaseVM ActiveCategory
		{
			get
			{
				return this._activeCategory;
			}
			set
			{
				if (value != this._activeCategory)
				{
					this._activeCategory = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticCategoryBaseVM>(value, "ActiveCategory");
				}
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x000261BF File Offset: 0x000243BF
		// (set) Token: 0x06000C4F RID: 3151 RVA: 0x000261C7 File Offset: 0x000243C7
		[DataSourceProperty]
		public MPArmoryCosmeticTauntSlotVM SelectedTauntSlot
		{
			get
			{
				return this._selectedTauntSlot;
			}
			set
			{
				if (value != this._selectedTauntSlot)
				{
					this._selectedTauntSlot = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticTauntSlotVM>(value, "SelectedTauntSlot");
					this.UpdateTauntAssignmentState();
				}
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x000261EB File Offset: 0x000243EB
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x000261F3 File Offset: 0x000243F3
		[DataSourceProperty]
		public MPArmoryCosmeticTauntItemVM SelectedTauntItem
		{
			get
			{
				return this._selectedTauntItem;
			}
			set
			{
				if (value != this._selectedTauntItem)
				{
					this._selectedTauntItem = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticTauntItemVM>(value, "SelectedTauntItem");
					this.UpdateTauntAssignmentState();
				}
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x00026217 File Offset: 0x00024417
		// (set) Token: 0x06000C53 RID: 3155 RVA: 0x0002621F File Offset: 0x0002441F
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> SortCategories
		{
			get
			{
				return this._sortCategories;
			}
			set
			{
				if (value != this._sortCategories)
				{
					this._sortCategories = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "SortCategories");
				}
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x0002623D File Offset: 0x0002443D
		// (set) Token: 0x06000C55 RID: 3157 RVA: 0x00026245 File Offset: 0x00024445
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> SortOrders
		{
			get
			{
				return this._sortOrders;
			}
			set
			{
				if (value != this._sortOrders)
				{
					this._sortOrders = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "SortOrders");
				}
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x00026263 File Offset: 0x00024463
		// (set) Token: 0x06000C57 RID: 3159 RVA: 0x0002626B File Offset: 0x0002446B
		[DataSourceProperty]
		public MBBindingList<MPArmoryCosmeticTauntSlotVM> TauntSlots
		{
			get
			{
				return this._tauntSlots;
			}
			set
			{
				if (value != this._tauntSlots)
				{
					this._tauntSlots = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPArmoryCosmeticTauntSlotVM>>(value, "TauntSlots");
				}
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00026289 File Offset: 0x00024489
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x00026291 File Offset: 0x00024491
		[DataSourceProperty]
		public MBBindingList<MPArmoryCosmeticCategoryBaseVM> AvailableCategories
		{
			get
			{
				return this._availableCategories;
			}
			set
			{
				if (value != this._availableCategories)
				{
					this._availableCategories = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPArmoryCosmeticCategoryBaseVM>>(value, "AvailableCategories");
				}
			}
		}

		// Token: 0x04000572 RID: 1394
		private readonly Func<List<IReadOnlyPerkObject>> _getSelectedPerks;

		// Token: 0x04000573 RID: 1395
		private List<MPArmoryCosmeticItemBaseVM> _allCosmetics;

		// Token: 0x04000574 RID: 1396
		private List<string> _ownedCosmetics;

		// Token: 0x04000575 RID: 1397
		private Dictionary<string, List<string>> _usedCosmetics;

		// Token: 0x04000576 RID: 1398
		private Equipment _selectedClassDefaultEquipment;

		// Token: 0x04000577 RID: 1399
		private MPArmoryCosmeticsVM.CosmeticItemComparer _currentItemComparer;

		// Token: 0x04000578 RID: 1400
		private List<MPArmoryCosmeticsVM.CosmeticItemComparer> _itemComparers;

		// Token: 0x04000579 RID: 1401
		private Dictionary<MPArmoryCosmeticsVM.ClothingCategory, MPArmoryClothingCosmeticCategoryVM> _clothingCategoriesLookup;

		// Token: 0x0400057A RID: 1402
		private Dictionary<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM> _tauntCategoriesLookup;

		// Token: 0x0400057B RID: 1403
		private Dictionary<string, MPArmoryCosmeticItemBaseVM> _cosmeticItemsLookup;

		// Token: 0x0400057C RID: 1404
		private MultiplayerClassDivisions.MPHeroClass _selectedClass;

		// Token: 0x0400057D RID: 1405
		private string _selectedTroopID;

		// Token: 0x0400057E RID: 1406
		private bool _isLocalCosmeticsDirty;

		// Token: 0x0400057F RID: 1407
		private bool _isNetworkCosmeticsDirty;

		// Token: 0x04000580 RID: 1408
		private bool _isSendingCosmeticData;

		// Token: 0x04000581 RID: 1409
		private bool _isRetrievingCosmeticData;

		// Token: 0x04000582 RID: 1410
		private CosmeticsManager.CosmeticType _currentCosmeticType;

		// Token: 0x04000583 RID: 1411
		private MPArmoryCosmeticsVM.ClothingCategory _currentClothingCategory;

		// Token: 0x04000584 RID: 1412
		private MPArmoryCosmeticsVM.TauntCategoryFlag _currentTauntCategory;

		// Token: 0x04000585 RID: 1413
		private InputKeyItemVM _actionInputKey;

		// Token: 0x04000586 RID: 1414
		private InputKeyItemVM _previewInputKey;

		// Token: 0x04000587 RID: 1415
		private int _loot;

		// Token: 0x04000588 RID: 1416
		private bool _isLoading;

		// Token: 0x04000589 RID: 1417
		private bool _hasCosmeticInfoReceived;

		// Token: 0x0400058A RID: 1418
		private bool _isManagingTaunts;

		// Token: 0x0400058B RID: 1419
		private bool _isTauntAssignmentActive;

		// Token: 0x0400058C RID: 1420
		private string _cosmeticInfoErrorText;

		// Token: 0x0400058D RID: 1421
		private HintViewModel _allCategoriesHint;

		// Token: 0x0400058E RID: 1422
		private HintViewModel _bodyCategoryHint;

		// Token: 0x0400058F RID: 1423
		private HintViewModel _headCategoryHint;

		// Token: 0x04000590 RID: 1424
		private HintViewModel _shoulderCategoryHint;

		// Token: 0x04000591 RID: 1425
		private HintViewModel _handCategoryHint;

		// Token: 0x04000592 RID: 1426
		private HintViewModel _legCategoryHint;

		// Token: 0x04000593 RID: 1427
		private HintViewModel _resetPreviewHint;

		// Token: 0x04000594 RID: 1428
		private MPArmoryCosmeticCategoryBaseVM _activeCategory;

		// Token: 0x04000595 RID: 1429
		private MPArmoryCosmeticTauntSlotVM _selectedTauntSlot;

		// Token: 0x04000596 RID: 1430
		private MPArmoryCosmeticTauntItemVM _selectedTauntItem;

		// Token: 0x04000597 RID: 1431
		private SelectorVM<SelectorItemVM> _sortCategories;

		// Token: 0x04000598 RID: 1432
		private SelectorVM<SelectorItemVM> _sortOrders;

		// Token: 0x04000599 RID: 1433
		private MBBindingList<MPArmoryCosmeticTauntSlotVM> _tauntSlots;

		// Token: 0x0400059A RID: 1434
		private MBBindingList<MPArmoryCosmeticCategoryBaseVM> _availableCategories;

		// Token: 0x0200016C RID: 364
		public enum ClothingCategory
		{
			// Token: 0x04000A58 RID: 2648
			Invalid = -1,
			// Token: 0x04000A59 RID: 2649
			ClothingCategoriesBegin,
			// Token: 0x04000A5A RID: 2650
			All = 0,
			// Token: 0x04000A5B RID: 2651
			HeadArmor,
			// Token: 0x04000A5C RID: 2652
			Cape,
			// Token: 0x04000A5D RID: 2653
			BodyArmor,
			// Token: 0x04000A5E RID: 2654
			HandArmor,
			// Token: 0x04000A5F RID: 2655
			LegArmor,
			// Token: 0x04000A60 RID: 2656
			ClothingCategoriesEnd
		}

		// Token: 0x0200016D RID: 365
		[Flags]
		public enum TauntCategoryFlag
		{
			// Token: 0x04000A62 RID: 2658
			None = 0,
			// Token: 0x04000A63 RID: 2659
			UsableWithMount = 1,
			// Token: 0x04000A64 RID: 2660
			UsableWithOneHanded = 2,
			// Token: 0x04000A65 RID: 2661
			UsableWithTwoHanded = 4,
			// Token: 0x04000A66 RID: 2662
			UsableWithBow = 8,
			// Token: 0x04000A67 RID: 2663
			UsableWithCrossbow = 16,
			// Token: 0x04000A68 RID: 2664
			UsableWithShield = 32,
			// Token: 0x04000A69 RID: 2665
			All = 63
		}

		// Token: 0x0200016E RID: 366
		public abstract class CosmeticItemComparer : IComparer<MPArmoryCosmeticItemBaseVM>
		{
			// Token: 0x170005D1 RID: 1489
			// (get) Token: 0x06001329 RID: 4905 RVA: 0x0003D81E File Offset: 0x0003BA1E
			protected int _sortMultiplier
			{
				get
				{
					if (!this._isAscending)
					{
						return -1;
					}
					return 1;
				}
			}

			// Token: 0x0600132A RID: 4906 RVA: 0x0003D82B File Offset: 0x0003BA2B
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x0600132B RID: 4907
			public abstract int Compare(MPArmoryCosmeticItemBaseVM x, MPArmoryCosmeticItemBaseVM y);

			// Token: 0x04000A6A RID: 2666
			private bool _isAscending;
		}

		// Token: 0x0200016F RID: 367
		private class CosmeticItemNameComparer : MPArmoryCosmeticsVM.CosmeticItemComparer
		{
			// Token: 0x0600132D RID: 4909 RVA: 0x0003D83C File Offset: 0x0003BA3C
			public override int Compare(MPArmoryCosmeticItemBaseVM x, MPArmoryCosmeticItemBaseVM y)
			{
				return x.Name.CompareTo(y.Name) * base._sortMultiplier;
			}
		}

		// Token: 0x02000170 RID: 368
		private class CosmeticItemCostComparer : MPArmoryCosmeticsVM.CosmeticItemComparer
		{
			// Token: 0x0600132F RID: 4911 RVA: 0x0003D860 File Offset: 0x0003BA60
			public override int Compare(MPArmoryCosmeticItemBaseVM x, MPArmoryCosmeticItemBaseVM y)
			{
				int num = x.Cost.CompareTo(y.Cost);
				if (num == 0)
				{
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM2;
					MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM;
					MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM2;
					if ((mparmoryCosmeticClothingItemVM = x as MPArmoryCosmeticClothingItemVM) != null && (mparmoryCosmeticClothingItemVM2 = y as MPArmoryCosmeticClothingItemVM) != null)
					{
						num = mparmoryCosmeticClothingItemVM.EquipmentElement.Item.ItemType.CompareTo(mparmoryCosmeticClothingItemVM2.EquipmentElement.Item.ItemType);
					}
					else if ((mparmoryCosmeticTauntItemVM = x as MPArmoryCosmeticTauntItemVM) != null && (mparmoryCosmeticTauntItemVM2 = y as MPArmoryCosmeticTauntItemVM) != null)
					{
						num = mparmoryCosmeticTauntItemVM.Name.CompareTo(mparmoryCosmeticTauntItemVM2.Name);
					}
				}
				return num * base._sortMultiplier;
			}
		}

		// Token: 0x02000171 RID: 369
		private class CosmeticItemRarityComparer : MPArmoryCosmeticsVM.CosmeticItemComparer
		{
			// Token: 0x06001331 RID: 4913 RVA: 0x0003D914 File Offset: 0x0003BB14
			public override int Compare(MPArmoryCosmeticItemBaseVM x, MPArmoryCosmeticItemBaseVM y)
			{
				int num = x.Cosmetic.Rarity.CompareTo(y.Cosmetic.Rarity);
				if (num == 0)
				{
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM2;
					MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM;
					MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM2;
					if ((mparmoryCosmeticClothingItemVM = x as MPArmoryCosmeticClothingItemVM) != null && (mparmoryCosmeticClothingItemVM2 = y as MPArmoryCosmeticClothingItemVM) != null)
					{
						num = mparmoryCosmeticClothingItemVM.EquipmentElement.Item.ItemType.CompareTo(mparmoryCosmeticClothingItemVM2.EquipmentElement.Item.ItemType);
					}
					else if ((mparmoryCosmeticTauntItemVM = x as MPArmoryCosmeticTauntItemVM) != null && (mparmoryCosmeticTauntItemVM2 = y as MPArmoryCosmeticTauntItemVM) != null)
					{
						num = mparmoryCosmeticTauntItemVM.Name.CompareTo(mparmoryCosmeticTauntItemVM2.Name);
					}
				}
				return num * base._sortMultiplier;
			}
		}

		// Token: 0x02000172 RID: 370
		private class CosmeticItemCategoryComparer : MPArmoryCosmeticsVM.CosmeticItemComparer
		{
			// Token: 0x06001333 RID: 4915 RVA: 0x0003D9D8 File Offset: 0x0003BBD8
			public override int Compare(MPArmoryCosmeticItemBaseVM x, MPArmoryCosmeticItemBaseVM y)
			{
				MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
				MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM2;
				if ((mparmoryCosmeticClothingItemVM = x as MPArmoryCosmeticClothingItemVM) != null && (mparmoryCosmeticClothingItemVM2 = y as MPArmoryCosmeticClothingItemVM) != null)
				{
					return mparmoryCosmeticClothingItemVM.EquipmentElement.Item.ItemType.CompareTo(mparmoryCosmeticClothingItemVM2.EquipmentElement.Item.ItemType) * base._sortMultiplier;
				}
				return 0;
			}
		}
	}
}
