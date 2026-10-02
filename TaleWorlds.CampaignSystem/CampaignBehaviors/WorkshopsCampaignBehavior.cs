using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000476 RID: 1142
	public class WorkshopsCampaignBehavior : CampaignBehaviorBase, IWorkshopWarehouseCampaignBehavior
	{
		// Token: 0x06004A6E RID: 19054 RVA: 0x00176D20 File Offset: 0x00174F20
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
			CampaignEvents.OnAfterSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterSessionLaunched));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.DailyTickTownEvent.AddNonSerializedListener(this, new Action<Town>(this.DailyTickTown));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.WorkshopOwnerChangedEvent.AddNonSerializedListener(this, new Action<Workshop, Hero>(this.OnWorkshopOwnerChanged));
			CampaignEvents.WorkshopTypeChangedEvent.AddNonSerializedListener(this, new Action<Workshop>(this.OnWorkshopTypeChanged));
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x00176E13 File Offset: 0x00175013
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<KeyValuePair<Settlement, ItemRoster>[]>("_warehouseRosterPerSettlement", ref this._warehouseRosterPerSettlement);
			dataStore.SyncData<WorkshopsCampaignBehavior.WorkshopData[]>("_workshopData", ref this._workshopData);
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x00176E39 File Offset: 0x00175039
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int i)
		{
			if (i >= 10)
			{
				if (i == 10)
				{
					this.InitializeBehaviorData();
					this.FillItemsInAllCategories();
					this.InitializeWorkshops();
					this.BuildWorkshopsAtGameStart();
				}
				if (i % 20 == 0)
				{
					this.RunTownShopsAtGameStart();
				}
			}
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x00176E6C File Offset: 0x0017506C
		private void InitializeBehaviorData()
		{
			if (this._workshopData == null)
			{
				this._workshopData = new WorkshopsCampaignBehavior.WorkshopData[Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave];
			}
			if (this._warehouseRosterPerSettlement == null)
			{
				this._warehouseRosterPerSettlement = new KeyValuePair<Settlement, ItemRoster>[Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave];
			}
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x00176EC8 File Offset: 0x001750C8
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeBehaviorData();
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.0", 0))
			{
				foreach (Workshop workshop in Hero.MainHero.OwnedWorkshops)
				{
					this.AddNewWorkshopData(workshop);
					this.AddNewWarehouseDataIfNeeded(workshop.Settlement);
				}
			}
			if (MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.2.9.35637", 0)))
			{
				for (int i = 0; i < this._workshopData.Length; i++)
				{
					if (this._workshopData[i] != null && this._workshopData[i].Workshop.Owner != Hero.MainHero)
					{
						this._workshopData[i] = null;
					}
				}
			}
			this.EnsureBehaviorDataSize();
			this.FillItemsInAllCategories();
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.3.0", 0))
			{
				this.RemoveDeadOwnersFromWorkshops();
			}
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x00176FD8 File Offset: 0x001751D8
		private void RemoveDeadOwnersFromWorkshops()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsTown)
				{
					foreach (Workshop workshop in settlement.Town.Workshops)
					{
						if (workshop.Owner.IsDead)
						{
							Debug.FailedAssert("Workshop owner is dead", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\WorkshopsCampaignBehavior.cs", "RemoveDeadOwnersFromWorkshops", 176);
							Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(workshop);
							ChangeOwnerOfWorkshopAction.ApplyByDeath(workshop, notableOwnerForWorkshop);
						}
					}
				}
			}
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x00177094 File Offset: 0x00175294
		private void EnsureBehaviorDataSize()
		{
			if (this._workshopData.Length < Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave)
			{
				WorkshopsCampaignBehavior.WorkshopData[] array = new WorkshopsCampaignBehavior.WorkshopData[Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave];
				for (int i = 0; i < Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave; i++)
				{
					if (i < this._workshopData.Length)
					{
						array[i] = this._workshopData[i];
					}
					else
					{
						array[i] = null;
					}
				}
				this._workshopData = array;
			}
			if (this._warehouseRosterPerSettlement.Length < Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave)
			{
				KeyValuePair<Settlement, ItemRoster>[] array2 = new KeyValuePair<Settlement, ItemRoster>[Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave];
				for (int j = 0; j < Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave; j++)
				{
					if (j < this._warehouseRosterPerSettlement.Length && this._warehouseRosterPerSettlement[j].Key != null && this._warehouseRosterPerSettlement[j].Value != null)
					{
						array2[j] = this._warehouseRosterPerSettlement[j];
					}
				}
				this._warehouseRosterPerSettlement = array2;
			}
		}

		// Token: 0x06004A75 RID: 19061 RVA: 0x001771C0 File Offset: 0x001753C0
		private void OnWorkshopTypeChanged(Workshop workshop)
		{
			if (workshop.Owner == Hero.MainHero)
			{
				this.RemoveWorkshopData(workshop);
				this.AddNewWorkshopData(workshop);
			}
		}

		// Token: 0x06004A76 RID: 19062 RVA: 0x001771E0 File Offset: 0x001753E0
		private void OnWorkshopOwnerChanged(Workshop workshop, Hero oldOwner)
		{
			Hero owner = workshop.Owner;
			if (owner == Hero.MainHero)
			{
				this.AddNewWarehouseDataIfNeeded(workshop.Settlement);
				this.AddNewWorkshopData(workshop);
				return;
			}
			if (oldOwner == Hero.MainHero && Clan.PlayerClan.Leader != owner)
			{
				if (Hero.MainHero.OwnedWorkshops.All<Workshop>((Workshop x) => x.Settlement != workshop.Settlement))
				{
					if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement == workshop.Settlement)
					{
						this.TransferWarehouseToPlayerParty(Settlement.CurrentSettlement);
					}
					this.RemoveWarehouseData(workshop.Settlement);
				}
				this.RemoveWorkshopData(workshop);
			}
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x001772A0 File Offset: 0x001754A0
		private void DailyTickTown(Town town)
		{
			foreach (Workshop workshop in town.Workshops)
			{
				if (!town.InRebelliousState)
				{
					this.RunTownWorkshop(town, workshop);
				}
				this.HandleDailyExpense(workshop);
			}
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x001772E0 File Offset: 0x001754E0
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if (!victim.IsHumanPlayerCharacter)
			{
				foreach (Workshop workshop in victim.OwnedWorkshops.ToList<Workshop>())
				{
					Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(workshop);
					ChangeOwnerOfWorkshopAction.ApplyByDeath(workshop, notableOwnerForWorkshop);
				}
			}
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x00177358 File Offset: 0x00175558
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			this.TransferPlayerWorkshopsIfNeeded();
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x00177360 File Offset: 0x00175560
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			this.TransferPlayerWorkshopsIfNeeded();
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x00177368 File Offset: 0x00175568
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newSettlementOwner, Hero oldSettlementOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement.IsTown)
			{
				foreach (Workshop workshop in settlement.Town.Workshops)
				{
					if (workshop.Owner != null && workshop.Owner.MapFaction.IsAtWarWith(newSettlementOwner.MapFaction) && workshop.Owner.GetPerkValue(DefaultPerks.Trade.RapidDevelopment))
					{
						GiveGoldAction.ApplyBetweenCharacters(null, workshop.Owner, MathF.Round(DefaultPerks.Trade.RapidDevelopment.PrimaryBonus), false);
					}
				}
				this.TransferPlayerWorkshopsIfNeeded();
			}
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x001773EF File Offset: 0x001755EF
		private void OnAfterSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeGameMenus(campaignGameStarter);
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x001773F8 File Offset: 0x001755F8
		protected void InitializeGameMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenuOption("town", "manage_warehouse", "{=LK4kNZkb}Enter the warehouse", new GameMenuOption.OnConditionDelegate(this.warehouse_manage_on_condition), new GameMenuOption.OnConsequenceDelegate(this.warehouse_manage_on_consequence), false, 7, false, null);
			campaignGameStarter.AddPlayerLine("workshop_worker_manage_warehouse", "player_options", "warehouse", "{=mBnoWa8R}I would like to access the Warehouse.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_worker_manage_warehouse_answer", "warehouse", "player_options", "{=Y4LhmAdi}Sure, boss. Go ahead.", null, new ConversationSentence.OnConsequenceDelegate(this.warehouse_manage_on_consequence), 100, null);
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x00177484 File Offset: 0x00175684
		private void warehouse_manage_on_consequence()
		{
			InventoryLogic.CapacityData capacityData = new InventoryLogic.CapacityData(new Func<int>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityDelegate|21_0), new Func<TextObject>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityExceededWarningDelegate|21_1), new Func<TextObject>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityExceededHintDelegate|21_2), false);
			InventoryScreenHelper.OpenScreenAsWarehouse(this.GetWarehouseRoster(Settlement.CurrentSettlement), capacityData);
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x001774E8 File Offset: 0x001756E8
		private void warehouse_manage_on_consequence(MenuCallbackArgs args)
		{
			InventoryLogic.CapacityData capacityData = new InventoryLogic.CapacityData(new Func<int>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityDelegate|22_0), new Func<TextObject>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityExceededWarningDelegate|22_1), new Func<TextObject>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityExceededHintDelegate|22_2), false);
			InventoryScreenHelper.OpenScreenAsWarehouse(this.GetWarehouseRoster(Settlement.CurrentSettlement), capacityData);
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x0017753D File Offset: 0x0017573D
		private bool warehouse_manage_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Warehouse;
			return this.GetWarehouseRoster(Settlement.CurrentSettlement) != null;
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x00177558 File Offset: 0x00175758
		bool IWorkshopWarehouseCampaignBehavior.IsGettingInputsFromWarehouse(Workshop workshop)
		{
			WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop = this.GetDataOfWorkshop(workshop);
			return dataOfWorkshop != null && dataOfWorkshop.IsGettingInputsFromWarehouse;
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x00177578 File Offset: 0x00175778
		void IWorkshopWarehouseCampaignBehavior.SetIsGettingInputsFromWarehouse(Workshop workshop, bool isActive)
		{
			WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop = this.GetDataOfWorkshop(workshop);
			if (dataOfWorkshop != null)
			{
				dataOfWorkshop.IsGettingInputsFromWarehouse = isActive;
			}
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x00177598 File Offset: 0x00175798
		float IWorkshopWarehouseCampaignBehavior.GetStockProductionInWarehouseRatio(Workshop workshop)
		{
			WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop = this.GetDataOfWorkshop(workshop);
			if (dataOfWorkshop != null)
			{
				return dataOfWorkshop.StockProductionInWarehouseRatio;
			}
			return 0f;
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x001775BC File Offset: 0x001757BC
		void IWorkshopWarehouseCampaignBehavior.SetStockProductionInWarehouseRatio(Workshop workshop, float ratio)
		{
			WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop = this.GetDataOfWorkshop(workshop);
			if (dataOfWorkshop != null)
			{
				dataOfWorkshop.StockProductionInWarehouseRatio = ratio;
			}
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x001775DC File Offset: 0x001757DC
		int IWorkshopWarehouseCampaignBehavior.GetInputCount(Workshop workshop)
		{
			ItemRoster warehouseRoster = this.GetWarehouseRoster(workshop.Settlement);
			int num = 0;
			List<ItemCategory> list = new List<ItemCategory>();
			foreach (WorkshopType.Production production in workshop.WorkshopType.Productions)
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
				{
					ItemCategory item = valueTuple.Item1;
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			foreach (ItemCategory itemCategory in list)
			{
				foreach (ItemRosterElement itemRosterElement in warehouseRoster)
				{
					if (itemRosterElement.EquipmentElement.Item.ItemCategory == itemCategory)
					{
						num += itemRosterElement.Amount;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x0017772C File Offset: 0x0017592C
		ExplainedNumber IWorkshopWarehouseCampaignBehavior.GetInputDailyChange(Workshop workshop)
		{
			ItemRoster warehouseRoster = this.GetWarehouseRoster(workshop.Settlement);
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, true, null);
			Dictionary<ItemCategory, float> dictionary = new Dictionary<ItemCategory, float>();
			foreach (WorkshopType.Production production in workshop.WorkshopType.Productions)
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
				{
					float num = Campaign.Current.Models.WorkshopModel.GetEffectiveConversionSpeedOfProduction(workshop, production.ConversionSpeed, false).ResultNumber * (float)valueTuple.Item2;
					ItemCategory item = valueTuple.Item1;
					float num2;
					if (!dictionary.TryGetValue(item, out num2))
					{
						dictionary.Add(item, num);
					}
					else
					{
						dictionary[item] = num2 + num;
					}
				}
			}
			foreach (KeyValuePair<ItemCategory, float> keyValuePair in dictionary)
			{
				int num3 = 0;
				foreach (ItemRosterElement itemRosterElement in warehouseRoster)
				{
					if (itemRosterElement.EquipmentElement.Item.ItemCategory == keyValuePair.Key)
					{
						num3 += itemRosterElement.Amount;
					}
				}
				if (num3 > 0)
				{
					TextObject textObject = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null);
					textObject.SetTextVariable("RANK", keyValuePair.Key.GetName());
					textObject.SetTextVariable("NUMBER", num3);
					explainedNumber.Add(-keyValuePair.Value, textObject, null);
				}
			}
			return explainedNumber;
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x00177930 File Offset: 0x00175B30
		int IWorkshopWarehouseCampaignBehavior.GetOutputCount(Workshop workshop)
		{
			ItemRoster warehouseRoster = this.GetWarehouseRoster(workshop.Settlement);
			int num = 0;
			List<ItemCategory> list = new List<ItemCategory>();
			foreach (WorkshopType.Production production in workshop.WorkshopType.Productions)
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Outputs)
				{
					ItemCategory item = valueTuple.Item1;
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			foreach (ItemCategory itemCategory in list)
			{
				foreach (ItemRosterElement itemRosterElement in warehouseRoster)
				{
					if (itemRosterElement.EquipmentElement.Item.ItemCategory == itemCategory)
					{
						num += itemRosterElement.Amount;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x00177A80 File Offset: 0x00175C80
		ExplainedNumber IWorkshopWarehouseCampaignBehavior.GetOutputDailyChange(Workshop workshop)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, true, null);
			ItemRoster warehouseRoster = this.GetWarehouseRoster(workshop.Settlement);
			Dictionary<ItemCategory, float> dictionary = new Dictionary<ItemCategory, float>();
			foreach (WorkshopType.Production production in workshop.WorkshopType.Productions)
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Outputs)
				{
					ItemCategory item = valueTuple.Item1;
					if (item.IsTradeGood)
					{
						int item2 = valueTuple.Item2;
						float num = Campaign.Current.Models.WorkshopModel.GetEffectiveConversionSpeedOfProduction(workshop, production.ConversionSpeed, false).ResultNumber * (float)item2 * this.GetDataOfWorkshop(workshop).StockProductionInWarehouseRatio;
						float num2;
						if (!dictionary.TryGetValue(item, out num2))
						{
							dictionary.Add(item, num);
						}
						else
						{
							dictionary[item] = num2 + num;
						}
					}
				}
			}
			foreach (KeyValuePair<ItemCategory, float> keyValuePair in dictionary)
			{
				int num3 = 0;
				foreach (ItemRosterElement itemRosterElement in warehouseRoster)
				{
					if (itemRosterElement.EquipmentElement.Item.ItemCategory == keyValuePair.Key)
					{
						num3 += itemRosterElement.Amount;
					}
				}
				TextObject textObject = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null);
				textObject.SetTextVariable("RANK", keyValuePair.Key.GetName());
				textObject.SetTextVariable("NUMBER", num3);
				explainedNumber.Add(keyValuePair.Value, textObject, null);
			}
			return explainedNumber;
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x00177CA0 File Offset: 0x00175EA0
		bool IWorkshopWarehouseCampaignBehavior.IsRawMaterialsSufficientInTownMarket(Workshop workshop)
		{
			for (int i = 0; i < workshop.WorkshopType.Productions.Count; i++)
			{
				WorkshopType.Production production = workshop.WorkshopType.Productions[i];
				int num;
				if (this.DetermineItemRosterHasSufficientInputs(production, workshop.Settlement.Town.Owner.ItemRoster, workshop.Settlement.Town, out num))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x00177D08 File Offset: 0x00175F08
		public float GetWarehouseItemRosterWeight(Settlement settlement)
		{
			ItemRoster warehouseRoster = this.GetWarehouseRoster(settlement);
			float num = 0f;
			foreach (ItemRosterElement itemRosterElement in warehouseRoster)
			{
				num += itemRosterElement.GetRosterElementWeight();
			}
			return num;
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x00177D60 File Offset: 0x00175F60
		private bool TickOneProductionCycleForPlayerWorkshop(WorkshopType.Production production, Workshop workshop, bool effectCapital)
		{
			bool flag = false;
			int num = 0;
			Town town = workshop.Settlement.Town;
			WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop = this.GetDataOfWorkshop(workshop);
			bool flag2 = dataOfWorkshop.IsGettingInputsFromWarehouse;
			if (flag2)
			{
				flag = this.DetermineItemRosterHasSufficientInputs(production, this.GetWarehouseRoster(workshop.Settlement), town, out num);
				if (flag)
				{
					num = 0;
				}
				else
				{
					flag2 = false;
				}
			}
			if (!flag)
			{
				flag = this.DetermineItemRosterHasSufficientInputs(production, town.Owner.ItemRoster, town, out num);
			}
			if (flag)
			{
				int num2;
				List<EquipmentElement> itemsToProduce = this.GetItemsToProduce(production, workshop, out num2);
				float num3 = dataOfWorkshop.StockProductionInWarehouseRatio;
				bool flag3 = num3.ApproximatelyEqualsTo(1f, 1E-05f);
				if (this.CanPlayerWorkshopProduceThisCycle(production, workshop, num, num2, effectCapital, flag3))
				{
					Dictionary<ItemObject, int> dictionary = new Dictionary<ItemObject, int>();
					foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
					{
						if (flag2)
						{
							this.ConsumeInputFromWarehouse(valueTuple.Item1, valueTuple.Item2, workshop);
						}
						else
						{
							this.ConsumeInputFromTownMarket(valueTuple.Item1, valueTuple.Item2, town, workshop, effectCapital);
						}
					}
					foreach (EquipmentElement equipmentElement in itemsToProduce)
					{
						WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop2 = this.GetDataOfWorkshop(workshop);
						if (equipmentElement.Item.IsTradeGood && this.CanItemFitInWarehouse(workshop.Settlement, equipmentElement))
						{
							this.AddOutputProgressForWarehouse(workshop, num3);
							if (dataOfWorkshop2.ProductionProgressForWarehouse >= 1f)
							{
								this.ProduceAnOutputToWarehouse(equipmentElement, workshop);
								this.AddOutputProgressForWarehouse(workshop, -1f);
								if (dictionary.ContainsKey(equipmentElement.Item))
								{
									Dictionary<ItemObject, int> dictionary2 = dictionary;
									ItemObject itemObject = equipmentElement.Item;
									int num4 = dictionary2[itemObject];
									dictionary2[itemObject] = num4 + 1;
								}
								else
								{
									dictionary.Add(equipmentElement.Item, 1);
								}
							}
						}
						else
						{
							num3 = 0f;
						}
						this.AddOutputProgressForTown(workshop, 1f - num3);
						if (dataOfWorkshop2.ProductionProgressForTown >= 1f)
						{
							this.ProduceAnOutputToTown(equipmentElement, workshop, effectCapital);
							SkillLevelingManager.OnProductionProducedToWarehouse(equipmentElement);
							this.AddOutputProgressForTown(workshop, -1f);
							if (dictionary.ContainsKey(equipmentElement.Item))
							{
								Dictionary<ItemObject, int> dictionary3 = dictionary;
								ItemObject itemObject = equipmentElement.Item;
								int num4 = dictionary3[itemObject];
								dictionary3[itemObject] = num4 + 1;
							}
							else
							{
								dictionary.Add(equipmentElement.Item, 1);
							}
						}
					}
					foreach (KeyValuePair<ItemObject, int> keyValuePair in dictionary)
					{
						ItemObject key = keyValuePair.Key;
						int value = keyValuePair.Value;
						CampaignEventDispatcher.Instance.OnItemProduced(key, workshop.Settlement, value);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x00178064 File Offset: 0x00176264
		private void ProduceAnOutputToWarehouse(EquipmentElement outputItem, Workshop workshop)
		{
			this.GetWarehouseRoster(workshop.Settlement).AddToCounts(outputItem, 1);
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x0017807C File Offset: 0x0017627C
		private void ConsumeInputFromWarehouse(ItemCategory productionInput, int inputCount, Workshop workshop)
		{
			ItemRoster warehouseRoster = this.GetWarehouseRoster(workshop.Settlement);
			int num = inputCount;
			for (int i = 0; i < warehouseRoster.Count; i++)
			{
				if (num == 0)
				{
					return;
				}
				ItemObject itemAtIndex = warehouseRoster.GetItemAtIndex(i);
				if (itemAtIndex.ItemCategory == productionInput)
				{
					int elementNumber = warehouseRoster.GetElementNumber(i);
					int num2 = MathF.Min(num, elementNumber);
					num -= num2;
					warehouseRoster.AddToCounts(itemAtIndex, -inputCount);
					CampaignEventDispatcher.Instance.OnItemConsumed(itemAtIndex, workshop.Settlement, inputCount);
				}
			}
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x001780F4 File Offset: 0x001762F4
		private bool CanPlayerWorkshopProduceThisCycle(WorkshopType.Production production, Workshop workshop, int inputMaterialCost, int outputIncome, bool effectCapital, bool allOutputsWillBeSentToWarehouse)
		{
			float num = (workshop.WorkshopType.IsHidden ? ((float)inputMaterialCost) : ((float)inputMaterialCost + 200f / production.ConversionSpeed));
			if (Campaign.Current.GameStarted && (float)outputIncome <= num)
			{
				return false;
			}
			if (workshop.Capital < inputMaterialCost)
			{
				return false;
			}
			if (effectCapital)
			{
				bool flag = workshop.Settlement.Town.Gold >= outputIncome;
				bool flag2 = !this.IsWarehouseAtLimit(workshop.Settlement);
				if (!flag && (!allOutputsWillBeSentToWarehouse || flag2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x0017817C File Offset: 0x0017637C
		private void HandlePlayerWorkshopExpense(Workshop shop)
		{
			int expense = shop.Expense;
			if (shop.Capital > Campaign.Current.Models.WorkshopModel.CapitalLowLimit)
			{
				shop.ChangeGold(-expense);
				return;
			}
			if (shop.Owner.Gold >= expense)
			{
				shop.Owner.Gold -= expense;
				return;
			}
			if (shop.Capital >= expense)
			{
				shop.ChangeGold(-expense);
				return;
			}
			this.ChangeWorkshopOwnerByBankruptcy(shop);
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x001781F0 File Offset: 0x001763F0
		private bool TickOneProductionCycleForNotableWorkshop(WorkshopType.Production production, Workshop workshop, bool effectCapital)
		{
			Town town = workshop.Settlement.Town;
			int num = 0;
			if (!this.DetermineItemRosterHasSufficientInputs(production, town.Owner.ItemRoster, town, out num))
			{
				return false;
			}
			int num2;
			List<EquipmentElement> itemsToProduce = this.GetItemsToProduce(production, workshop, out num2);
			if (this.CanNotableWorkshopProduceThisCycle(production, workshop, num, num2, effectCapital))
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
				{
					this.ConsumeInputFromTownMarket(valueTuple.Item1, valueTuple.Item2, town, workshop, effectCapital);
				}
				foreach (EquipmentElement equipmentElement in itemsToProduce)
				{
					this.ProduceAnOutputToTown(equipmentElement, workshop, effectCapital);
					CampaignEventDispatcher.Instance.OnItemProduced(equipmentElement.Item, workshop.Settlement, 1);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x001782F8 File Offset: 0x001764F8
		private bool CanNotableWorkshopProduceThisCycle(WorkshopType.Production production, Workshop workshop, int inputMaterialCost, int outputIncome, bool effectCapital)
		{
			float num = (workshop.WorkshopType.IsHidden ? ((float)inputMaterialCost) : ((float)inputMaterialCost + 200f / production.ConversionSpeed));
			return (!Campaign.Current.GameStarted || (float)outputIncome > num) && (workshop.Settlement.Town.Gold >= outputIncome || !effectCapital) && workshop.Capital >= inputMaterialCost;
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x00178364 File Offset: 0x00176564
		private void HandleNotableWorkshopExpense(Workshop shop)
		{
			int expense = shop.Expense;
			if (shop.Capital >= expense)
			{
				shop.ChangeGold(-expense);
				return;
			}
			this.ChangeWorkshopOwnerByBankruptcy(shop);
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x00178394 File Offset: 0x00176594
		private WorkshopsCampaignBehavior.WorkshopData GetDataOfWorkshop(Workshop workshop)
		{
			for (int i = 0; i < this._workshopData.Length; i++)
			{
				WorkshopsCampaignBehavior.WorkshopData workshopData = this._workshopData[i];
				if (workshopData != null && workshopData.Workshop == workshop)
				{
					return workshopData;
				}
			}
			return null;
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x001783CC File Offset: 0x001765CC
		private List<EquipmentElement> GetItemsToProduce(WorkshopType.Production production, Workshop workshop, out int income)
		{
			List<EquipmentElement> list = new List<EquipmentElement>();
			income = 0;
			for (int i = 0; i < production.Outputs.Count; i++)
			{
				int item = production.Outputs[i].Item2;
				for (int j = 0; j < item; j++)
				{
					EquipmentElement randomItem = this.GetRandomItem(production.Outputs[i].Item1, workshop.Settlement.Town);
					if (!randomItem.IsEmpty)
					{
						list.Add(randomItem);
						income += workshop.Settlement.Town.GetItemPrice(randomItem, null, true);
					}
					else
					{
						Debug.FailedAssert("Workshop produces empty items", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\WorkshopsCampaignBehavior.cs", "GetItemsToProduce", 916);
					}
				}
			}
			return list;
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x0017848C File Offset: 0x0017668C
		private void ProduceAnOutputToTown(EquipmentElement outputItem, Workshop workshop, bool effectCapital)
		{
			Town town = workshop.Settlement.Town;
			int itemPrice = town.GetItemPrice(outputItem, null, false);
			town.Owner.ItemRoster.AddToCounts(outputItem, 1);
			if (Campaign.Current.GameStarted && effectCapital)
			{
				int num = MathF.Min(1000, itemPrice);
				workshop.ChangeGold(num);
				town.ChangeGold(-num);
			}
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x001784EC File Offset: 0x001766EC
		private void ConsumeInputFromTownMarket(ItemCategory productionInput, int productionInputCount, Town town, Workshop workshop, bool effectCapital)
		{
			ItemRoster itemRoster = town.Owner.ItemRoster;
			int num = itemRoster.FindIndex((ItemObject x) => x.ItemCategory == productionInput);
			if (num >= 0)
			{
				ItemObject itemAtIndex = itemRoster.GetItemAtIndex(num);
				if (Campaign.Current.GameStarted && effectCapital)
				{
					int itemPrice = town.GetItemPrice(itemAtIndex, null, false);
					workshop.ChangeGold(-itemPrice);
					town.ChangeGold(itemPrice);
				}
				itemRoster.AddToCounts(itemAtIndex, -productionInputCount);
				CampaignEventDispatcher.Instance.OnItemConsumed(itemAtIndex, town.Owner.Settlement, productionInputCount);
			}
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x0017857E File Offset: 0x0017677E
		private bool IsItemPreferredForTown(ItemObject item, Town townComponent)
		{
			return item.Culture == null || item.Culture.StringId == "neutral_culture" || item.Culture == townComponent.Culture;
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x001785B0 File Offset: 0x001767B0
		private bool DetermineItemRosterHasSufficientInputs(WorkshopType.Production production, ItemRoster itemRoster, Town town, out int inputMaterialCost)
		{
			List<ValueTuple<ItemCategory, int>> inputs = production.Inputs;
			inputMaterialCost = 0;
			foreach (ValueTuple<ItemCategory, int> valueTuple in inputs)
			{
				ItemCategory item = valueTuple.Item1;
				int num = valueTuple.Item2;
				for (int i = 0; i < itemRoster.Count; i++)
				{
					ItemObject itemAtIndex = itemRoster.GetItemAtIndex(i);
					if (itemAtIndex.ItemCategory == item)
					{
						int elementNumber = itemRoster.GetElementNumber(i);
						int num2 = MathF.Min(num, elementNumber);
						num -= num2;
						inputMaterialCost += town.GetItemPrice(itemAtIndex, null, false) * num2;
					}
				}
				if (num > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x0017866C File Offset: 0x0017686C
		private void AddOutputProgressForWarehouse(Workshop workshop, float progressToAdd)
		{
			this.GetDataOfWorkshop(workshop).ProductionProgressForWarehouse += progressToAdd;
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x00178682 File Offset: 0x00176882
		private void AddOutputProgressForTown(Workshop workshop, float progressToAdd)
		{
			this.GetDataOfWorkshop(workshop).ProductionProgressForTown += progressToAdd;
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x00178698 File Offset: 0x00176898
		private bool CanItemFitInWarehouse(Settlement settlement, EquipmentElement equipmentElement)
		{
			return this.GetWarehouseItemRosterWeight(settlement) + equipmentElement.Weight <= (float)Campaign.Current.Models.WorkshopModel.WarehouseCapacity;
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x001786C3 File Offset: 0x001768C3
		private bool IsWarehouseAtLimit(Settlement settlement)
		{
			return this.GetWarehouseItemRosterWeight(settlement) >= (float)Campaign.Current.Models.WorkshopModel.WarehouseCapacity;
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x001786E8 File Offset: 0x001768E8
		private void AddNewWorkshopData(Workshop workshop)
		{
			for (int i = 0; i < this._workshopData.Length; i++)
			{
				if (this._workshopData[i] == null)
				{
					this._workshopData[i] = new WorkshopsCampaignBehavior.WorkshopData(workshop);
					return;
				}
			}
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x00178724 File Offset: 0x00176924
		private void RemoveWorkshopData(Workshop workshop)
		{
			for (int i = 0; i < this._workshopData.Length; i++)
			{
				WorkshopsCampaignBehavior.WorkshopData workshopData = this._workshopData[i];
				if (workshopData != null && workshopData.Workshop == workshop)
				{
					this._workshopData[i] = null;
					return;
				}
			}
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x00178764 File Offset: 0x00176964
		private ItemRoster GetWarehouseRoster(Settlement settlement)
		{
			foreach (KeyValuePair<Settlement, ItemRoster> keyValuePair in this._warehouseRosterPerSettlement)
			{
				if (keyValuePair.Key == settlement)
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x001787A4 File Offset: 0x001769A4
		private void FillItemsInAllCategories()
		{
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (this.IsProducable(itemObject))
				{
					ItemCategory itemCategory = itemObject.ItemCategory;
					if (itemCategory != null)
					{
						List<ItemObject> list;
						if (!this._itemsInCategory.TryGetValue(itemCategory, out list))
						{
							list = new List<ItemObject>();
							this._itemsInCategory[itemCategory] = list;
						}
						list.Add(itemObject);
					}
				}
			}
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x00178838 File Offset: 0x00176A38
		private bool IsProducable(ItemObject item)
		{
			return !item.MultiplayerItem && !item.NotMerchandise && !item.IsCraftedByPlayer;
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x00178858 File Offset: 0x00176A58
		private void RemoveWarehouseData(Settlement settlement)
		{
			for (int i = 0; i < this._warehouseRosterPerSettlement.Length; i++)
			{
				if (this._warehouseRosterPerSettlement[i].Key == settlement)
				{
					this._warehouseRosterPerSettlement[i] = new KeyValuePair<Settlement, ItemRoster>(null, null);
					return;
				}
			}
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x001788A0 File Offset: 0x00176AA0
		private void AddNewWarehouseDataIfNeeded(Settlement settlement)
		{
			bool flag = false;
			foreach (KeyValuePair<Settlement, ItemRoster> keyValuePair in this._warehouseRosterPerSettlement)
			{
				if (keyValuePair.Key == settlement)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < this._warehouseRosterPerSettlement.Length; j++)
				{
					KeyValuePair<Settlement, ItemRoster> keyValuePair2 = this._warehouseRosterPerSettlement[j];
					if (keyValuePair2.Value == null)
					{
						this._warehouseRosterPerSettlement[j] = new KeyValuePair<Settlement, ItemRoster>(settlement, new ItemRoster());
						return;
					}
				}
			}
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x00178928 File Offset: 0x00176B28
		private EquipmentElement GetRandomItem(ItemCategory itemGroupBase, Town townComponent)
		{
			EquipmentElement randomItemAux = this.GetRandomItemAux(itemGroupBase, townComponent);
			if (randomItemAux.Item != null)
			{
				return randomItemAux;
			}
			return this.GetRandomItemAux(itemGroupBase, null);
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x00178954 File Offset: 0x00176B54
		private EquipmentElement GetRandomItemAux(ItemCategory itemGroupBase, Town townComponent = null)
		{
			ItemObject itemObject = null;
			ItemModifier itemModifier = null;
			List<ValueTuple<ItemObject, float>> list = new List<ValueTuple<ItemObject, float>>();
			List<ItemObject> list2;
			if (this._itemsInCategory.TryGetValue(itemGroupBase, out list2))
			{
				foreach (ItemObject itemObject2 in list2)
				{
					if ((townComponent == null || this.IsItemPreferredForTown(itemObject2, townComponent)) && itemObject2.ItemCategory == itemGroupBase)
					{
						float num = 1f / ((float)MathF.Max(100, itemObject2.Value) + 100f);
						list.Add(new ValueTuple<ItemObject, float>(itemObject2, num));
					}
				}
				itemObject = MBRandom.ChooseWeighted<ItemObject>(list);
				ItemModifierGroup itemModifierGroup;
				if (itemObject == null)
				{
					itemModifierGroup = null;
				}
				else
				{
					ItemComponent itemComponent = itemObject.ItemComponent;
					itemModifierGroup = ((itemComponent != null) ? itemComponent.ItemModifierGroup : null);
				}
				ItemModifierGroup itemModifierGroup2 = itemModifierGroup;
				if (itemModifierGroup2 != null)
				{
					itemModifier = itemModifierGroup2.GetRandomItemModifierProductionScoreBased();
				}
			}
			return new EquipmentElement(itemObject, itemModifier, null, false);
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x00178A34 File Offset: 0x00176C34
		private void TransferPlayerWorkshopsIfNeeded()
		{
			int count = Hero.MainHero.OwnedWorkshops.Count;
			List<Workshop> list = Hero.MainHero.OwnedWorkshops.ToList<Workshop>();
			for (int i = 0; i < count; i++)
			{
				Workshop workshop = list[i];
				if (workshop.Settlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(workshop);
					if (notableOwnerForWorkshop != null)
					{
						WorkshopType workshopType = this.DecideBestWorkshopType(workshop.Settlement, false, workshop.WorkshopType);
						ChangeOwnerOfWorkshopAction.ApplyByWar(workshop, notableOwnerForWorkshop, workshopType);
					}
				}
			}
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x00178ACC File Offset: 0x00176CCC
		private void ChangeWorkshopOwnerByBankruptcy(Workshop workshop)
		{
			int costForNotable = Campaign.Current.Models.WorkshopModel.GetCostForNotable(workshop);
			Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(workshop);
			WorkshopType workshopType = this.DecideBestWorkshopType(workshop.Settlement, false, workshop.WorkshopType);
			ChangeOwnerOfWorkshopAction.ApplyByBankruptcy(workshop, notableOwnerForWorkshop, workshopType, costForNotable);
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x00178B22 File Offset: 0x00176D22
		private void HandleDailyExpense(Workshop shop)
		{
			if (!shop.WorkshopType.IsHidden)
			{
				if (shop.Owner != Hero.MainHero)
				{
					this.HandleNotableWorkshopExpense(shop);
					return;
				}
				this.HandlePlayerWorkshopExpense(shop);
			}
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x00178B50 File Offset: 0x00176D50
		private float FindTotalInputDensityScore(Settlement settlement, WorkshopType workshopType, IDictionary<ItemCategory, float> productionDict, bool atGameStart)
		{
			float num = 0f;
			for (int i = 0; i < settlement.Town.Workshops.Length; i++)
			{
				Workshop workshop = settlement.Town.Workshops[i];
				if (workshop.WorkshopType != null && !workshop.WorkshopType.IsHidden)
				{
					if (workshop.WorkshopType == workshopType)
					{
						num += 1f;
					}
					else
					{
						ValueTuple<float, float> inputOutputSimilarityForWorkshopTypes = this.GetInputOutputSimilarityForWorkshopTypes(workshopType, workshop.WorkshopType);
						float item = inputOutputSimilarityForWorkshopTypes.Item1;
						float item2 = inputOutputSimilarityForWorkshopTypes.Item2;
						num += item;
						num += item2;
					}
				}
			}
			float num2 = 0.01f;
			float num3 = 0f;
			foreach (WorkshopType.Production production in workshopType.Productions)
			{
				bool flag = false;
				using (List<ValueTuple<ItemCategory, int>>.Enumerator enumerator2 = production.Outputs.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Item1.IsTradeGood)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
					{
						ItemCategory item3 = valueTuple.Item1;
						int item4 = valueTuple.Item2;
						float num4;
						if (productionDict.TryGetValue(item3, out num4))
						{
							num2 += num4 / (production.ConversionSpeed * (float)item4);
						}
						if (!atGameStart)
						{
							float priceFactor = settlement.Town.MarketData.GetPriceFactor(item3);
							num3 += Math.Max(0f, 1f - priceFactor);
						}
					}
				}
			}
			float num5 = 1f + num * 6f;
			num2 *= (float)workshopType.Frequency * (1f / (float)Math.Pow((double)num5, 3.0));
			num2 += num3;
			num2 = MathF.Pow(num2, 0.6f);
			return num2;
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x00178D64 File Offset: 0x00176F64
		private ValueTuple<float, float> GetInputOutputSimilarityForWorkshopTypes(WorkshopType workshop, WorkshopType otherWorkshop)
		{
			List<ItemCategory> list = new List<ItemCategory>();
			List<ItemCategory> list2 = new List<ItemCategory>();
			ValueTuple<Dictionary<ItemCategory, float>, Dictionary<ItemCategory, float>> inputOutputProductionForWorkshop = this.GetInputOutputProductionForWorkshop(workshop, ref list2, ref list);
			Dictionary<ItemCategory, float> item = inputOutputProductionForWorkshop.Item1;
			Dictionary<ItemCategory, float> item2 = inputOutputProductionForWorkshop.Item2;
			ValueTuple<Dictionary<ItemCategory, float>, Dictionary<ItemCategory, float>> inputOutputProductionForWorkshop2 = this.GetInputOutputProductionForWorkshop(otherWorkshop, ref list2, ref list);
			Dictionary<ItemCategory, float> item3 = inputOutputProductionForWorkshop2.Item1;
			Dictionary<ItemCategory, float> item4 = inputOutputProductionForWorkshop2.Item2;
			float num = item.SumQ<KeyValuePair<ItemCategory, float>>((KeyValuePair<ItemCategory, float> x) => x.Value);
			float num2 = item3.SumQ<KeyValuePair<ItemCategory, float>>((KeyValuePair<ItemCategory, float> x) => x.Value);
			float num3 = 0f;
			foreach (ItemCategory itemCategory in list2)
			{
				if (item.ContainsKey(itemCategory) && item3.ContainsKey(itemCategory))
				{
					float num4 = item[itemCategory] / num;
					float num5 = item3[itemCategory] / num2;
					num3 += Math.Min(num4, num5);
				}
			}
			float num6 = item2.SumQ<KeyValuePair<ItemCategory, float>>((KeyValuePair<ItemCategory, float> x) => x.Value);
			float num7 = item4.SumQ<KeyValuePair<ItemCategory, float>>((KeyValuePair<ItemCategory, float> x) => x.Value);
			float num8 = 0f;
			foreach (ItemCategory itemCategory2 in list)
			{
				if (item2.ContainsKey(itemCategory2) && item4.ContainsKey(itemCategory2))
				{
					float num9 = item2[itemCategory2] / num6;
					float num10 = item4[itemCategory2] / num7;
					num8 += Math.Min(num9, num10);
				}
			}
			return new ValueTuple<float, float>(num3, num8);
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x00178F54 File Offset: 0x00177154
		private ValueTuple<Dictionary<ItemCategory, float>, Dictionary<ItemCategory, float>> GetInputOutputProductionForWorkshop(WorkshopType workshop, ref List<ItemCategory> allInputItems, ref List<ItemCategory> allOutputItems)
		{
			Dictionary<ItemCategory, float> dictionary = new Dictionary<ItemCategory, float>();
			Dictionary<ItemCategory, float> dictionary2 = new Dictionary<ItemCategory, float>();
			foreach (WorkshopType.Production production in workshop.Productions)
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
				{
					if (valueTuple.Item1.IsTradeGood)
					{
						if (dictionary.ContainsKey(valueTuple.Item1))
						{
							Dictionary<ItemCategory, float> dictionary3 = dictionary;
							ItemCategory itemCategory = valueTuple.Item1;
							dictionary3[itemCategory] += (float)valueTuple.Item2 * production.ConversionSpeed;
						}
						else
						{
							dictionary.Add(valueTuple.Item1, (float)valueTuple.Item2 * production.ConversionSpeed);
						}
						if (!allInputItems.Contains(valueTuple.Item1))
						{
							allInputItems.Add(valueTuple.Item1);
						}
					}
				}
				foreach (ValueTuple<ItemCategory, int> valueTuple2 in production.Outputs)
				{
					if (valueTuple2.Item1.IsTradeGood)
					{
						if (dictionary2.ContainsKey(valueTuple2.Item1))
						{
							Dictionary<ItemCategory, float> dictionary3 = dictionary2;
							ItemCategory itemCategory = valueTuple2.Item1;
							dictionary3[itemCategory] += (float)valueTuple2.Item2 * production.ConversionSpeed;
						}
						else
						{
							dictionary2.Add(valueTuple2.Item1, (float)valueTuple2.Item2 * production.ConversionSpeed);
						}
						if (!allOutputItems.Contains(valueTuple2.Item1))
						{
							allOutputItems.Add(valueTuple2.Item1);
						}
					}
				}
			}
			return new ValueTuple<Dictionary<ItemCategory, float>, Dictionary<ItemCategory, float>>(dictionary, dictionary2);
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x00179178 File Offset: 0x00177378
		private void BuildWorkshopForHeroAtGameStart(Hero ownerHero)
		{
			Settlement bornSettlement = ownerHero.BornSettlement;
			WorkshopType workshopType = this.DecideBestWorkshopType(bornSettlement, true, null);
			if (workshopType != null)
			{
				int num = -1;
				for (int i = 0; i < bornSettlement.Town.Workshops.Length; i++)
				{
					if (bornSettlement.Town.Workshops[i].WorkshopType == null)
					{
						num = i;
						break;
					}
				}
				if (num >= 0)
				{
					InitializeWorkshopAction.ApplyByNewGame(bornSettlement.Town.Workshops[num], ownerHero, workshopType);
				}
			}
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x001791EC File Offset: 0x001773EC
		private WorkshopType DecideBestWorkshopType(Settlement currentSettlement, bool atGameStart, WorkshopType workshopToExclude = null)
		{
			IDictionary<ItemCategory, float> dictionary = new Dictionary<ItemCategory, float>();
			foreach (Village village in Village.All.Where<Village>((Village x) => x.TradeBound == currentSettlement))
			{
				foreach (ValueTuple<ItemObject, float> valueTuple in village.VillageType.Productions)
				{
					ItemCategory itemCategory = valueTuple.Item1.ItemCategory;
					if (itemCategory != DefaultItemCategories.Grain || village.VillageType.PrimaryProduction == DefaultItems.Grain)
					{
						float item = valueTuple.Item2;
						if (itemCategory == DefaultItemCategories.Cow)
						{
							itemCategory = DefaultItemCategories.Hides;
						}
						if (itemCategory == DefaultItemCategories.Sheep)
						{
							itemCategory = DefaultItemCategories.Wool;
						}
						float num;
						if (dictionary.TryGetValue(itemCategory, out num))
						{
							dictionary[itemCategory] = num + item;
						}
						else
						{
							dictionary.Add(itemCategory, item);
						}
					}
				}
			}
			Dictionary<WorkshopType, float> dictionary2 = new Dictionary<WorkshopType, float>();
			float num2 = 0f;
			foreach (WorkshopType workshopType in WorkshopType.All)
			{
				if (!workshopType.IsHidden && (workshopToExclude == null || workshopToExclude != workshopType))
				{
					float num3 = this.FindTotalInputDensityScore(currentSettlement, workshopType, dictionary, atGameStart);
					dictionary2.Add(workshopType, num3);
					num2 += num3;
				}
			}
			float num4 = num2 * MBRandom.RandomFloat;
			WorkshopType workshopType2 = null;
			foreach (WorkshopType workshopType3 in WorkshopType.All)
			{
				if (!workshopType3.IsHidden && (workshopToExclude == null || workshopToExclude != workshopType3))
				{
					num4 -= dictionary2[workshopType3];
					if (num4 < 0f)
					{
						workshopType2 = workshopType3;
						break;
					}
				}
			}
			if (workshopType2 == null)
			{
				workshopType2 = WorkshopType.All[MBRandom.RandomInt(1, WorkshopType.All.Count)];
			}
			return workshopType2;
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x00179434 File Offset: 0x00177634
		private void InitializeWorkshops()
		{
			foreach (Town town in Town.AllTowns)
			{
				town.InitializeWorkshops(Campaign.Current.Models.WorkshopModel.DefaultWorkshopCountInSettlement);
			}
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x00179498 File Offset: 0x00177698
		private void BuildWorkshopsAtGameStart()
		{
			foreach (Town town in Town.AllTowns)
			{
				this.BuildArtisanWorkshop(town);
				for (int i = 1; i < town.Workshops.Length; i++)
				{
					Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(town.Workshops[i]);
					this.BuildWorkshopForHeroAtGameStart(notableOwnerForWorkshop);
				}
			}
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x00179524 File Offset: 0x00177724
		private void BuildArtisanWorkshop(Town town)
		{
			Hero hero = town.Settlement.Notables.FirstOrDefault<Hero>((Hero x) => x.IsArtisan);
			if (hero == null)
			{
				hero = town.Settlement.Notables.FirstOrDefault<Hero>();
			}
			if (hero != null)
			{
				WorkshopType workshopType = WorkshopType.Find("artisans");
				town.Workshops[0].InitializeWorkshop(hero, workshopType);
			}
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x00179594 File Offset: 0x00177794
		private void RunTownShopsAtGameStart()
		{
			foreach (Town town in Town.AllTowns)
			{
				foreach (Workshop workshop in town.Workshops)
				{
					this.RunTownWorkshop(town, workshop);
				}
			}
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x00179604 File Offset: 0x00177804
		private void RunTownWorkshop(Town townComponent, Workshop workshop)
		{
			WorkshopType workshopType = workshop.WorkshopType;
			bool flag = false;
			for (int i = 0; i < workshopType.Productions.Count; i++)
			{
				float num = workshop.GetProductionProgress(i);
				if (num > 1f)
				{
					num = 1f;
				}
				num += Campaign.Current.Models.WorkshopModel.GetEffectiveConversionSpeedOfProduction(workshop, workshopType.Productions[i].ConversionSpeed, false).ResultNumber;
				if (num >= 1f)
				{
					bool flag2 = true;
					while (flag2 && num >= 1f)
					{
						WorkshopType.Production production = workshopType.Productions[i];
						bool flag3;
						if (!production.Inputs.Any<ValueTuple<ItemCategory, int>>((ValueTuple<ItemCategory, int> x) => !x.Item1.IsTradeGood))
						{
							flag3 = !production.Outputs.Any<ValueTuple<ItemCategory, int>>((ValueTuple<ItemCategory, int> x) => !x.Item1.IsTradeGood);
						}
						else
						{
							flag3 = false;
						}
						bool flag4 = flag3;
						flag2 = ((workshop.Owner == Hero.MainHero) ? this.TickOneProductionCycleForPlayerWorkshop(production, workshop, flag4) : this.TickOneProductionCycleForNotableWorkshop(production, workshop, flag4));
						if (flag2 && flag4)
						{
							flag = true;
						}
						num -= 1f;
					}
				}
				workshop.SetProgress(i, num);
			}
			if (flag)
			{
				workshop.UpdateLastRunTime();
			}
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x0017975C File Offset: 0x0017795C
		public void TransferWarehouseToPlayerParty(Settlement settlement)
		{
			foreach (ItemRosterElement itemRosterElement in this.GetWarehouseRoster(settlement))
			{
				MobileParty.MainParty.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, itemRosterElement.Amount);
			}
			this.RemoveWarehouseData(settlement);
		}

		// Token: 0x040014D9 RID: 5337
		private KeyValuePair<Settlement, ItemRoster>[] _warehouseRosterPerSettlement;

		// Token: 0x040014DA RID: 5338
		private WorkshopsCampaignBehavior.WorkshopData[] _workshopData;

		// Token: 0x040014DB RID: 5339
		private readonly Dictionary<ItemCategory, List<ItemObject>> _itemsInCategory = new Dictionary<ItemCategory, List<ItemObject>>();

		// Token: 0x020008BE RID: 2238
		public class WorkshopsCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06006C3D RID: 27709 RVA: 0x001DBD44 File Offset: 0x001D9F44
			public WorkshopsCampaignBehaviorTypeDefiner()
				: base(155828)
			{
			}

			// Token: 0x06006C3E RID: 27710 RVA: 0x001DBD51 File Offset: 0x001D9F51
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(WorkshopsCampaignBehavior.WorkshopData), 10, null);
			}

			// Token: 0x06006C3F RID: 27711 RVA: 0x001DBD66 File Offset: 0x001D9F66
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<Workshop, WorkshopsCampaignBehavior.WorkshopData>));
				base.ConstructContainerDefinition(typeof(WorkshopsCampaignBehavior.WorkshopData[]));
			}
		}

		// Token: 0x020008BF RID: 2239
		internal class WorkshopData
		{
			// Token: 0x06006C40 RID: 27712 RVA: 0x001DBD88 File Offset: 0x001D9F88
			public WorkshopData(Workshop workshop)
			{
				this.Workshop = workshop;
			}

			// Token: 0x06006C41 RID: 27713 RVA: 0x001DBD97 File Offset: 0x001D9F97
			public override string ToString()
			{
				return this.Workshop.WorkshopType.ToString() + " in " + this.Workshop.Settlement.GetName();
			}

			// Token: 0x06006C42 RID: 27714 RVA: 0x001DBDC3 File Offset: 0x001D9FC3
			internal static void AutoGeneratedStaticCollectObjectsWorkshopData(object o, List<object> collectedObjects)
			{
				((WorkshopsCampaignBehavior.WorkshopData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006C43 RID: 27715 RVA: 0x001DBDD1 File Offset: 0x001D9FD1
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Workshop);
			}

			// Token: 0x06006C44 RID: 27716 RVA: 0x001DBDDF File Offset: 0x001D9FDF
			internal static object AutoGeneratedGetMemberValueWorkshop(object o)
			{
				return ((WorkshopsCampaignBehavior.WorkshopData)o).Workshop;
			}

			// Token: 0x06006C45 RID: 27717 RVA: 0x001DBDEC File Offset: 0x001D9FEC
			internal static object AutoGeneratedGetMemberValueIsGettingInputsFromWarehouse(object o)
			{
				return ((WorkshopsCampaignBehavior.WorkshopData)o).IsGettingInputsFromWarehouse;
			}

			// Token: 0x06006C46 RID: 27718 RVA: 0x001DBDFE File Offset: 0x001D9FFE
			internal static object AutoGeneratedGetMemberValueProductionProgressForWarehouse(object o)
			{
				return ((WorkshopsCampaignBehavior.WorkshopData)o).ProductionProgressForWarehouse;
			}

			// Token: 0x06006C47 RID: 27719 RVA: 0x001DBE10 File Offset: 0x001DA010
			internal static object AutoGeneratedGetMemberValueProductionProgressForTown(object o)
			{
				return ((WorkshopsCampaignBehavior.WorkshopData)o).ProductionProgressForTown;
			}

			// Token: 0x06006C48 RID: 27720 RVA: 0x001DBE22 File Offset: 0x001DA022
			internal static object AutoGeneratedGetMemberValueStockProductionInWarehouseRatio(object o)
			{
				return ((WorkshopsCampaignBehavior.WorkshopData)o).StockProductionInWarehouseRatio;
			}

			// Token: 0x04002606 RID: 9734
			[SaveableField(1)]
			public Workshop Workshop;

			// Token: 0x04002607 RID: 9735
			[SaveableField(2)]
			public bool IsGettingInputsFromWarehouse;

			// Token: 0x04002608 RID: 9736
			[SaveableField(3)]
			public float ProductionProgressForWarehouse;

			// Token: 0x04002609 RID: 9737
			[SaveableField(4)]
			public float ProductionProgressForTown;

			// Token: 0x0400260A RID: 9738
			[SaveableField(5)]
			public float StockProductionInWarehouseRatio;
		}
	}
}
