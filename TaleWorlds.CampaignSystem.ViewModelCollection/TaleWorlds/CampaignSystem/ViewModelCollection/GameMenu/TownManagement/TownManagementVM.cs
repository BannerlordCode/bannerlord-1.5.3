using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000B2 RID: 178
	public class TownManagementVM : ViewModel
	{
		// Token: 0x060010A0 RID: 4256 RVA: 0x00043BF0 File Offset: 0x00041DF0
		public TownManagementVM()
		{
			this._settlement = Settlement.CurrentSettlement;
			Settlement settlement = this._settlement;
			if (((settlement != null) ? settlement.Town : null) == null)
			{
				Debug.FailedAssert("Town management initialized with null settlement and/or town!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\TownManagement\\TownManagementVM.cs", ".ctor", 27);
				Debug.Print("Town management initialized with null settlement and/or town!", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			this.ProjectSelection = new SettlementProjectSelectionVM(this._settlement, new Action(this.OnChangeInBuildingQueue));
			this.GovernorSelection = new SettlementGovernorSelectionVM(this._settlement, new Action<Hero>(this.OnGovernorSelectionDone));
			this.ReserveControl = new TownManagementReserveControlVM(this._settlement, new Action(this.OnReserveUpdated));
			this.MiddleFirstTextList = new MBBindingList<TownManagementDescriptionItemVM>();
			this.MiddleSecondTextList = new MBBindingList<TownManagementDescriptionItemVM>();
			this.Shops = new MBBindingList<TownManagementShopItemVM>();
			this.Villages = new MBBindingList<TownManagementVillageItemVM>();
			this.Show = false;
			this.IsTown = this._settlement.IsTown;
			this.IsThereCurrentProject = this._settlement.Town.CurrentBuilding != null;
			this.CurrentGovernor = new HeroVM(this._settlement.Town.Governor ?? CampaignUIHelper.GetTeleportingGovernor(this._settlement, Campaign.Current.GetCampaignBehavior<ITeleportationCampaignBehavior>()), true);
			if (this.CurrentGovernor.Hero != null)
			{
				this.CurrentGovernorTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetHeroGovernorEffectsTooltip(this.CurrentGovernor.Hero, this._settlement));
			}
			else
			{
				this.CurrentGovernorTooltip = new BasicTooltipViewModel(() => this.GetAssignGovernorTooltip());
			}
			this.UpdateGovernorSelectionProperties();
			this.RefreshCurrentDevelopment();
			this.RefreshTownManagementStats();
			foreach (Workshop workshop in this._settlement.Town.Workshops)
			{
				WorkshopType workshopType = workshop.WorkshopType;
				if (workshopType != null && !workshopType.IsHidden)
				{
					this.Shops.Add(new TownManagementShopItemVM(workshop));
				}
			}
			foreach (Village village in this._settlement.BoundVillages)
			{
				this.Villages.Add(new TownManagementVillageItemVM(village));
			}
			this.ConsumptionTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetSettlementConsumptionTooltip(this._settlement));
			this.RefreshValues();
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00043E48 File Offset: 0x00042048
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CurrentProjectText = new TextObject("{=qBq70qDq}Current Project", null).ToString();
			this.CompletionText = new TextObject("{=Rkh2k1OA}Completion:", null).ToString();
			this.ManageText = new TextObject("{=XseYJYka}Manage", null).ToString();
			this.DoneText = new TextObject("{=WiNRdfsm}Done", null).ToString();
			this.WallsText = new TextObject("{=LsZEdD2z}Walls", null).ToString();
			this.VillagesText = GameTexts.FindText("str_bound_village", null).ToString();
			this.ShopsInSettlementText = GameTexts.FindText("str_shops_in_settlement", null).ToString();
			this.GovernorText = GameTexts.FindText("str_sort_by_governor_label", null).ToString();
			this.MiddleFirstTextList.ApplyActionOnAllItems(delegate(TownManagementDescriptionItemVM x)
			{
				x.RefreshValues();
			});
			this.MiddleSecondTextList.ApplyActionOnAllItems(delegate(TownManagementDescriptionItemVM x)
			{
				x.RefreshValues();
			});
			this.ProjectSelection.RefreshValues();
			this.GovernorSelection.RefreshValues();
			this.ReserveControl.RefreshValues();
			this.Shops.ApplyActionOnAllItems(delegate(TownManagementShopItemVM x)
			{
				x.RefreshValues();
			});
			this.Villages.ApplyActionOnAllItems(delegate(TownManagementVillageItemVM x)
			{
				x.RefreshValues();
			});
			this.CurrentGovernor.RefreshValues();
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00043FE0 File Offset: 0x000421E0
		private void RefreshTownManagementStats()
		{
			this.MiddleFirstTextList.Clear();
			this.MiddleSecondTextList.Clear();
			ExplainedNumber taxExplanation = Campaign.Current.Models.SettlementTaxModel.CalculateTownTax(this._settlement.Town, true);
			int taxValue = MathF.Round(taxExplanation.ResultNumber);
			BasicTooltipViewModel basicTooltipViewModel = new BasicTooltipViewModel(() => CampaignUIHelper.GetTooltipForAccumulatingPropertyWithResult(GameTexts.FindText("str_town_management_population_tax", null).ToString(), (float)taxValue, ref taxExplanation));
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_town_management_population_tax", null));
			this.MiddleFirstTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_LEFT_colon", null), taxValue, 0, TownManagementDescriptionItemVM.DescriptionType.Gold, basicTooltipViewModel));
			BasicTooltipViewModel basicTooltipViewModel2 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownDailyProductionTooltip(this._settlement.Town));
			this.MiddleFirstTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_daily_production", null), MathF.Round(Campaign.Current.Models.BuildingConstructionModel.CalculateDailyConstructionPower(this._settlement.Town, false).ResultNumber), 0, TownManagementDescriptionItemVM.DescriptionType.Production, basicTooltipViewModel2));
			BasicTooltipViewModel basicTooltipViewModel3 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownProsperityTooltip(this._settlement.Town));
			this.MiddleFirstTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_prosperity", null), MathF.Round(this._settlement.Town.Prosperity), MathF.Round(Campaign.Current.Models.SettlementProsperityModel.CalculateProsperityChange(this._settlement.Town, false).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Prosperity, basicTooltipViewModel3));
			BasicTooltipViewModel basicTooltipViewModel4 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownFoodTooltip(this._settlement.Town));
			this.MiddleFirstTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_food", null), MathF.Round(this._settlement.Town.FoodStocks), MathF.Round(Campaign.Current.Models.SettlementFoodModel.CalculateTownFoodStocksChange(this._settlement.Town, true, false).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Food, basicTooltipViewModel4));
			BasicTooltipViewModel basicTooltipViewModel5 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownLoyaltyTooltip(this._settlement.Town));
			this.MiddleSecondTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_loyalty", null), MathF.Round(this._settlement.Town.Loyalty), MathF.Round(Campaign.Current.Models.SettlementLoyaltyModel.CalculateLoyaltyChange(this._settlement.Town, false).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Loyalty, basicTooltipViewModel5));
			BasicTooltipViewModel basicTooltipViewModel6 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownSecurityTooltip(this._settlement.Town));
			this.MiddleSecondTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_security", null), MathF.Round(this._settlement.Town.Security), MathF.Round(Campaign.Current.Models.SettlementSecurityModel.CalculateSecurityChange(this._settlement.Town, false).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Security, basicTooltipViewModel6));
			BasicTooltipViewModel basicTooltipViewModel7 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownMilitiaTooltip(this._settlement.Town));
			this.MiddleSecondTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_militia", null), MathF.Round(this._settlement.Militia), MathF.Round(Campaign.Current.Models.SettlementMilitiaModel.CalculateMilitiaChange(this._settlement, false).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Militia, basicTooltipViewModel7));
			BasicTooltipViewModel basicTooltipViewModel8 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownGarrisonTooltip(this._settlement.Town));
			Collection<TownManagementDescriptionItemVM> middleSecondTextList = this.MiddleSecondTextList;
			TextObject textObject = GameTexts.FindText("str_town_management_garrison", null);
			MobileParty garrisonParty = this._settlement.Town.GarrisonParty;
			middleSecondTextList.Add(new TownManagementDescriptionItemVM(textObject, (garrisonParty != null) ? garrisonParty.Party.NumberOfAllMembers : 0, MathF.Round(SettlementHelper.GetGarrisonChangeExplainedNumber(this._settlement.Town).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Garrison, basicTooltipViewModel8));
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0004439E File Offset: 0x0004259E
		private void OnChangeInBuildingQueue()
		{
			this.OnProjectSelectionDone();
			this.RefreshTownManagementStats();
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x000443AC File Offset: 0x000425AC
		private void RefreshCurrentDevelopment()
		{
			if (this._settlement.Town.CurrentBuilding != null)
			{
				this.IsCurrentProjectDaily = this._settlement.Town.CurrentBuilding.BuildingType.IsDailyProject;
				if (!this.IsCurrentProjectDaily)
				{
					this.CurrentProjectProgress = (int)(BuildingHelper.GetProgressOfBuilding(this.ProjectSelection.CurrentSelectedProject.Building, this._settlement.Town) * 100f);
					this.ProjectSelection.CurrentSelectedProject.RefreshProductionText();
				}
			}
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00044430 File Offset: 0x00042630
		private void OnProjectSelectionDone()
		{
			List<Building> localDevelopmentList = this.ProjectSelection.LocalDevelopmentList;
			Building building = this.ProjectSelection.CurrentDailyDefault.Building;
			if (localDevelopmentList != null)
			{
				BuildingHelper.ChangeCurrentBuildingQueue(localDevelopmentList, this._settlement.Town);
			}
			if (building != this._settlement.Town.Buildings.FirstOrDefault<Building>((Building k) => k.IsCurrentlyDefault) && building != null)
			{
				BuildingHelper.ChangeDefaultBuilding(building, this._settlement.Town);
			}
			this.RefreshCurrentDevelopment();
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x000444C0 File Offset: 0x000426C0
		private void OnGovernorSelectionDone(Hero selectedGovernor)
		{
			if (selectedGovernor != this.CurrentGovernor.Hero)
			{
				this.CurrentGovernor = new HeroVM(selectedGovernor, true);
				if (this.CurrentGovernor.Hero != null)
				{
					ChangeGovernorAction.Apply(this._settlement.Town, this.CurrentGovernor.Hero);
					this.CurrentGovernorTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetHeroGovernorEffectsTooltip(selectedGovernor, this._settlement));
				}
				else
				{
					ChangeGovernorAction.RemoveGovernorOfIfExists(this._settlement.Town);
					this.CurrentGovernorTooltip = new BasicTooltipViewModel(() => this.GetAssignGovernorTooltip());
				}
			}
			this.UpdateGovernorSelectionProperties();
			this.RefreshTownManagementStats();
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0004457C File Offset: 0x0004277C
		private void UpdateGovernorSelectionProperties()
		{
			this.HasGovernor = this.CurrentGovernor.Hero != null;
			TextObject textObject;
			this.IsGovernorSelectionEnabled = this.GetCanChangeGovernor(out textObject);
			this.GovernorSelectionDisabledHint = new HintViewModel(textObject, null);
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x000445B8 File Offset: 0x000427B8
		private bool GetCanChangeGovernor(out TextObject disabledReason)
		{
			HeroVM currentGovernor = this.CurrentGovernor;
			bool flag;
			if (currentGovernor == null)
			{
				flag = false;
			}
			else
			{
				Hero hero = currentGovernor.Hero;
				bool? flag2 = ((hero != null) ? new bool?(hero.IsTraveling) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				disabledReason = new TextObject("{=qbqimqMb}{GOVERNOR.NAME} is on the way to be the new governor of {SETTLEMENT_NAME}", null);
				if (this.CurrentGovernor.Hero.CharacterObject != null)
				{
					StringHelpers.SetCharacterProperties("GOVERNOR", this.CurrentGovernor.Hero.CharacterObject, disabledReason, false);
				}
				TextObject textObject = disabledReason;
				string text = "SETTLEMENT_NAME";
				TextObject name = this._settlement.Name;
				textObject.SetTextVariable(text, ((name != null) ? name.ToString() : null) ?? string.Empty);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0004467B File Offset: 0x0004287B
		private void OnReserveUpdated()
		{
			this.RefreshCurrentDevelopment();
			this.RefreshTownManagementStats();
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00044689 File Offset: 0x00042889
		public void ExecuteDone()
		{
			this.OnProjectSelectionDone();
			this.Show = false;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00044698 File Offset: 0x00042898
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.DoneInputKey.OnFinalize();
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x000446AB File Offset: 0x000428AB
		private string GetAssignGovernorTooltip()
		{
			return GameTexts.FindText("str_clan_assign_governor", null).ToString();
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x000446BD File Offset: 0x000428BD
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x000446CC File Offset: 0x000428CC
		// (set) Token: 0x060010AF RID: 4271 RVA: 0x000446D4 File Offset: 0x000428D4
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x000446F2 File Offset: 0x000428F2
		// (set) Token: 0x060010B1 RID: 4273 RVA: 0x000446FA File Offset: 0x000428FA
		[DataSourceProperty]
		public string CompletionText
		{
			get
			{
				return this._completionText;
			}
			set
			{
				if (value != this._completionText)
				{
					this._completionText = value;
					base.OnPropertyChangedWithValue<string>(value, "CompletionText");
				}
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x0004471D File Offset: 0x0004291D
		// (set) Token: 0x060010B3 RID: 4275 RVA: 0x00044725 File Offset: 0x00042925
		[DataSourceProperty]
		public string GovernorText
		{
			get
			{
				return this._governorText;
			}
			set
			{
				if (value != this._governorText)
				{
					this._governorText = value;
					base.OnPropertyChangedWithValue<string>(value, "GovernorText");
				}
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x00044748 File Offset: 0x00042948
		// (set) Token: 0x060010B5 RID: 4277 RVA: 0x00044750 File Offset: 0x00042950
		[DataSourceProperty]
		public string ManageText
		{
			get
			{
				return this._manageText;
			}
			set
			{
				if (value != this._manageText)
				{
					this._manageText = value;
					base.OnPropertyChangedWithValue<string>(value, "ManageText");
				}
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x00044773 File Offset: 0x00042973
		// (set) Token: 0x060010B7 RID: 4279 RVA: 0x0004477B File Offset: 0x0004297B
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x0004479E File Offset: 0x0004299E
		// (set) Token: 0x060010B9 RID: 4281 RVA: 0x000447A6 File Offset: 0x000429A6
		[DataSourceProperty]
		public string WallsText
		{
			get
			{
				return this._wallsText;
			}
			set
			{
				if (value != this._wallsText)
				{
					this._wallsText = value;
					base.OnPropertyChangedWithValue<string>(value, "WallsText");
				}
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x000447C9 File Offset: 0x000429C9
		// (set) Token: 0x060010BB RID: 4283 RVA: 0x000447D1 File Offset: 0x000429D1
		[DataSourceProperty]
		public string CurrentProjectText
		{
			get
			{
				return this._currentProjectText;
			}
			set
			{
				if (value != this._currentProjectText)
				{
					this._currentProjectText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentProjectText");
				}
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x000447F4 File Offset: 0x000429F4
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x000447FC File Offset: 0x000429FC
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0004481F File Offset: 0x00042A1F
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x00044827 File Offset: 0x00042A27
		[DataSourceProperty]
		public bool HasGovernor
		{
			get
			{
				return this._hasGovernor;
			}
			set
			{
				if (value != this._hasGovernor)
				{
					this._hasGovernor = value;
					base.OnPropertyChangedWithValue(value, "HasGovernor");
				}
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x00044845 File Offset: 0x00042A45
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x0004484D File Offset: 0x00042A4D
		[DataSourceProperty]
		public bool IsGovernorSelectionEnabled
		{
			get
			{
				return this._isGovernorSelectionEnabled;
			}
			set
			{
				if (value != this._isGovernorSelectionEnabled)
				{
					this._isGovernorSelectionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsGovernorSelectionEnabled");
				}
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x0004486B File Offset: 0x00042A6B
		// (set) Token: 0x060010C3 RID: 4291 RVA: 0x00044873 File Offset: 0x00042A73
		[DataSourceProperty]
		public bool IsTown
		{
			get
			{
				return this._isTown;
			}
			set
			{
				if (value != this._isTown)
				{
					this._isTown = value;
					base.OnPropertyChangedWithValue(value, "IsTown");
				}
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x00044891 File Offset: 0x00042A91
		// (set) Token: 0x060010C5 RID: 4293 RVA: 0x00044899 File Offset: 0x00042A99
		[DataSourceProperty]
		public bool Show
		{
			get
			{
				return this._show;
			}
			set
			{
				if (value != this._show)
				{
					this._show = value;
					base.OnPropertyChangedWithValue(value, "Show");
				}
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x000448B7 File Offset: 0x00042AB7
		// (set) Token: 0x060010C7 RID: 4295 RVA: 0x000448BF File Offset: 0x00042ABF
		[DataSourceProperty]
		public bool IsThereCurrentProject
		{
			get
			{
				return this._isThereCurrentProject;
			}
			set
			{
				if (value != this._isThereCurrentProject)
				{
					this._isThereCurrentProject = value;
					base.OnPropertyChangedWithValue(value, "IsThereCurrentProject");
				}
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x000448DD File Offset: 0x00042ADD
		// (set) Token: 0x060010C9 RID: 4297 RVA: 0x000448E5 File Offset: 0x00042AE5
		[DataSourceProperty]
		public bool IsSelectingGovernor
		{
			get
			{
				return this._isSelectingGovernor;
			}
			set
			{
				if (value != this._isSelectingGovernor)
				{
					this._isSelectingGovernor = value;
					base.OnPropertyChangedWithValue(value, "IsSelectingGovernor");
				}
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x00044903 File Offset: 0x00042B03
		// (set) Token: 0x060010CB RID: 4299 RVA: 0x0004490B File Offset: 0x00042B0B
		[DataSourceProperty]
		public MBBindingList<TownManagementDescriptionItemVM> MiddleFirstTextList
		{
			get
			{
				return this._middleLeftTextList;
			}
			set
			{
				if (value != this._middleLeftTextList)
				{
					this._middleLeftTextList = value;
					base.OnPropertyChanged("MiddleLeftTextList");
				}
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x00044928 File Offset: 0x00042B28
		// (set) Token: 0x060010CD RID: 4301 RVA: 0x00044930 File Offset: 0x00042B30
		[DataSourceProperty]
		public MBBindingList<TownManagementDescriptionItemVM> MiddleSecondTextList
		{
			get
			{
				return this._middleRightTextList;
			}
			set
			{
				if (value != this._middleRightTextList)
				{
					this._middleRightTextList = value;
					base.OnPropertyChanged("MiddleRightTextList");
				}
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x0004494D File Offset: 0x00042B4D
		// (set) Token: 0x060010CF RID: 4303 RVA: 0x00044955 File Offset: 0x00042B55
		[DataSourceProperty]
		public MBBindingList<TownManagementShopItemVM> Shops
		{
			get
			{
				return this._shops;
			}
			set
			{
				if (value != this._shops)
				{
					this._shops = value;
					base.OnPropertyChangedWithValue<MBBindingList<TownManagementShopItemVM>>(value, "Shops");
				}
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x00044973 File Offset: 0x00042B73
		// (set) Token: 0x060010D1 RID: 4305 RVA: 0x0004497B File Offset: 0x00042B7B
		[DataSourceProperty]
		public MBBindingList<TownManagementVillageItemVM> Villages
		{
			get
			{
				return this._villages;
			}
			set
			{
				if (value != this._villages)
				{
					this._villages = value;
					base.OnPropertyChangedWithValue<MBBindingList<TownManagementVillageItemVM>>(value, "Villages");
				}
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x00044999 File Offset: 0x00042B99
		// (set) Token: 0x060010D3 RID: 4307 RVA: 0x000449A1 File Offset: 0x00042BA1
		[DataSourceProperty]
		public HintViewModel GovernorSelectionDisabledHint
		{
			get
			{
				return this._governorSelectionDisabledHint;
			}
			set
			{
				if (value != this._governorSelectionDisabledHint)
				{
					this._governorSelectionDisabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GovernorSelectionDisabledHint");
				}
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060010D4 RID: 4308 RVA: 0x000449BF File Offset: 0x00042BBF
		// (set) Token: 0x060010D5 RID: 4309 RVA: 0x000449C7 File Offset: 0x00042BC7
		[DataSourceProperty]
		public string VillagesText
		{
			get
			{
				return this._villagesText;
			}
			set
			{
				if (value != this._villagesText)
				{
					this._villagesText = value;
					base.OnPropertyChangedWithValue<string>(value, "VillagesText");
				}
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x000449EA File Offset: 0x00042BEA
		// (set) Token: 0x060010D7 RID: 4311 RVA: 0x000449F2 File Offset: 0x00042BF2
		[DataSourceProperty]
		public string ShopsInSettlementText
		{
			get
			{
				return this._shopsInSettlementText;
			}
			set
			{
				if (value != this._shopsInSettlementText)
				{
					this._shopsInSettlementText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShopsInSettlementText");
				}
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x00044A15 File Offset: 0x00042C15
		// (set) Token: 0x060010D9 RID: 4313 RVA: 0x00044A1D File Offset: 0x00042C1D
		[DataSourceProperty]
		public bool IsCurrentProjectDaily
		{
			get
			{
				return this._isCurrentProjectDaily;
			}
			set
			{
				if (value != this._isCurrentProjectDaily)
				{
					this._isCurrentProjectDaily = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentProjectDaily");
				}
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x00044A3B File Offset: 0x00042C3B
		// (set) Token: 0x060010DB RID: 4315 RVA: 0x00044A43 File Offset: 0x00042C43
		[DataSourceProperty]
		public int CurrentProjectProgress
		{
			get
			{
				return this._currentProjectProgress;
			}
			set
			{
				if (value != this._currentProjectProgress)
				{
					this._currentProjectProgress = value;
					base.OnPropertyChangedWithValue(value, "CurrentProjectProgress");
				}
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060010DC RID: 4316 RVA: 0x00044A61 File Offset: 0x00042C61
		// (set) Token: 0x060010DD RID: 4317 RVA: 0x00044A69 File Offset: 0x00042C69
		[DataSourceProperty]
		public SettlementProjectSelectionVM ProjectSelection
		{
			get
			{
				return this._projectSelection;
			}
			set
			{
				if (value != this._projectSelection)
				{
					this._projectSelection = value;
					base.OnPropertyChangedWithValue<SettlementProjectSelectionVM>(value, "ProjectSelection");
				}
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x00044A87 File Offset: 0x00042C87
		// (set) Token: 0x060010DF RID: 4319 RVA: 0x00044A8F File Offset: 0x00042C8F
		[DataSourceProperty]
		public SettlementGovernorSelectionVM GovernorSelection
		{
			get
			{
				return this._governorSelection;
			}
			set
			{
				if (value != this._governorSelection)
				{
					this._governorSelection = value;
					base.OnPropertyChangedWithValue<SettlementGovernorSelectionVM>(value, "GovernorSelection");
				}
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x00044AAD File Offset: 0x00042CAD
		// (set) Token: 0x060010E1 RID: 4321 RVA: 0x00044AB5 File Offset: 0x00042CB5
		[DataSourceProperty]
		public TownManagementReserveControlVM ReserveControl
		{
			get
			{
				return this._reserveControl;
			}
			set
			{
				if (value != this._reserveControl)
				{
					this._reserveControl = value;
					base.OnPropertyChangedWithValue<TownManagementReserveControlVM>(value, "ReserveControl");
				}
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x00044AD3 File Offset: 0x00042CD3
		// (set) Token: 0x060010E3 RID: 4323 RVA: 0x00044ADB File Offset: 0x00042CDB
		[DataSourceProperty]
		public BasicTooltipViewModel CurrentGovernorTooltip
		{
			get
			{
				return this._currentGovernorTooltip;
			}
			set
			{
				if (value != this._currentGovernorTooltip)
				{
					this._currentGovernorTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CurrentGovernorTooltip");
				}
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x00044AF9 File Offset: 0x00042CF9
		// (set) Token: 0x060010E5 RID: 4325 RVA: 0x00044B01 File Offset: 0x00042D01
		[DataSourceProperty]
		public HeroVM CurrentGovernor
		{
			get
			{
				return this._currentGovernor;
			}
			set
			{
				if (value != this._currentGovernor)
				{
					this._currentGovernor = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "CurrentGovernor");
				}
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x060010E6 RID: 4326 RVA: 0x00044B1F File Offset: 0x00042D1F
		// (set) Token: 0x060010E7 RID: 4327 RVA: 0x00044B27 File Offset: 0x00042D27
		[DataSourceProperty]
		public BasicTooltipViewModel ConsumptionTooltip
		{
			get
			{
				return this._consumptionTooltip;
			}
			set
			{
				if (value != this._consumptionTooltip)
				{
					this._consumptionTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ConsumptionTooltip");
				}
			}
		}

		// Token: 0x0400078E RID: 1934
		private readonly Settlement _settlement;

		// Token: 0x0400078F RID: 1935
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000790 RID: 1936
		private bool _isThereCurrentProject;

		// Token: 0x04000791 RID: 1937
		private bool _isSelectingGovernor;

		// Token: 0x04000792 RID: 1938
		private SettlementProjectSelectionVM _projectSelection;

		// Token: 0x04000793 RID: 1939
		private SettlementGovernorSelectionVM _governorSelection;

		// Token: 0x04000794 RID: 1940
		private TownManagementReserveControlVM _reserveControl;

		// Token: 0x04000795 RID: 1941
		private MBBindingList<TownManagementDescriptionItemVM> _middleLeftTextList;

		// Token: 0x04000796 RID: 1942
		private MBBindingList<TownManagementDescriptionItemVM> _middleRightTextList;

		// Token: 0x04000797 RID: 1943
		private MBBindingList<TownManagementShopItemVM> _shops;

		// Token: 0x04000798 RID: 1944
		private MBBindingList<TownManagementVillageItemVM> _villages;

		// Token: 0x04000799 RID: 1945
		private HintViewModel _governorSelectionDisabledHint;

		// Token: 0x0400079A RID: 1946
		private bool _show;

		// Token: 0x0400079B RID: 1947
		private bool _isTown;

		// Token: 0x0400079C RID: 1948
		private bool _hasGovernor;

		// Token: 0x0400079D RID: 1949
		private bool _isGovernorSelectionEnabled;

		// Token: 0x0400079E RID: 1950
		private string _titleText;

		// Token: 0x0400079F RID: 1951
		private bool _isCurrentProjectDaily;

		// Token: 0x040007A0 RID: 1952
		private int _currentProjectProgress;

		// Token: 0x040007A1 RID: 1953
		private string _currentProjectText;

		// Token: 0x040007A2 RID: 1954
		private HeroVM _currentGovernor;

		// Token: 0x040007A3 RID: 1955
		private BasicTooltipViewModel _currentGovernorTooltip;

		// Token: 0x040007A4 RID: 1956
		private string _manageText;

		// Token: 0x040007A5 RID: 1957
		private string _doneText;

		// Token: 0x040007A6 RID: 1958
		private string _wallsText;

		// Token: 0x040007A7 RID: 1959
		private string _completionText;

		// Token: 0x040007A8 RID: 1960
		private string _villagesText;

		// Token: 0x040007A9 RID: 1961
		private string _shopsInSettlementText;

		// Token: 0x040007AA RID: 1962
		private BasicTooltipViewModel _consumptionTooltip;

		// Token: 0x040007AB RID: 1963
		private string _governorText;
	}
}
