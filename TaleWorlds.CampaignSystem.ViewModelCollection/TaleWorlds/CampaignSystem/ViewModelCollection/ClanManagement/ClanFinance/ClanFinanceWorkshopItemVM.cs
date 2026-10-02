using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance
{
	// Token: 0x0200013B RID: 315
	public class ClanFinanceWorkshopItemVM : ClanFinanceIncomeItemBaseVM
	{
		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06001DE9 RID: 7657 RVA: 0x0006BFA3 File Offset: 0x0006A1A3
		// (set) Token: 0x06001DEA RID: 7658 RVA: 0x0006BFAB File Offset: 0x0006A1AB
		public Workshop Workshop { get; private set; }

		// Token: 0x06001DEB RID: 7659 RVA: 0x0006BFB4 File Offset: 0x0006A1B4
		public ClanFinanceWorkshopItemVM(Workshop workshop, Action<ClanFinanceWorkshopItemVM> onSelection, Action onRefresh, Action<ClanCardSelectionInfo> openCardSelectionPopup)
			: base(null, onRefresh)
		{
			this._workshopWarehouseBehavior = Campaign.Current.GetCampaignBehavior<IWorkshopWarehouseCampaignBehavior>();
			this.Workshop = workshop;
			this._openCardSelectionPopup = openCardSelectionPopup;
			this._workshopModel = Campaign.Current.Models.WorkshopModel;
			base.IncomeTypeAsEnum = IncomeTypes.Workshop;
			this._onSelection = new Action<ClanFinanceIncomeItemBaseVM>(this.tempOnSelection);
			this._onSelectionT = onSelection;
			SettlementComponent settlementComponent = this.Workshop.Settlement.SettlementComponent;
			base.ImageName = ((settlementComponent != null) ? settlementComponent.WaitMeshName : "");
			this.ManageWorkshopHint = new HintViewModel(new TextObject("{=LxWVtDF0}Manage Workshop", null), null);
			this.UseWarehouseAsInputHint = new HintViewModel(new TextObject("{=a4oqWgUi}If there are no raw materials in the warehouse, the workshop will buy raw materials from the market until the warehouse is restocked", null), null);
			this.StoreOutputPercentageHint = new HintViewModel(new TextObject("{=NVUi4bB9}When the warehouse is full, the workshop will sell the products to the town market", null), null);
			this.InputWarehouseCountsTooltip = new BasicTooltipViewModel();
			this.OutputWarehouseCountsTooltip = new BasicTooltipViewModel();
			this.ReceiveInputFromWarehouse = this._workshopWarehouseBehavior.IsGettingInputsFromWarehouse(workshop);
			this.WarehousePercentageSelector = new SelectorVM<WorkshopPercentageSelectorItemVM>(0, new Action<SelectorVM<WorkshopPercentageSelectorItemVM>>(this.OnStoreOutputInWarehousePercentageUpdated));
			this.RefreshStoragePercentages();
			float currentPercentage = this._workshopWarehouseBehavior.GetStockProductionInWarehouseRatio(workshop);
			WorkshopPercentageSelectorItemVM workshopPercentageSelectorItemVM = this.WarehousePercentageSelector.ItemList.FirstOrDefault<WorkshopPercentageSelectorItemVM>((WorkshopPercentageSelectorItemVM x) => x.Percentage.ApproximatelyEqualsTo(currentPercentage, 0.1f));
			this.WarehousePercentageSelector.SelectedIndex = ((workshopPercentageSelectorItemVM != null) ? this.WarehousePercentageSelector.ItemList.IndexOf(workshopPercentageSelectorItemVM) : 0);
			this.RefreshValues();
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x0006C181 File Offset: 0x0006A381
		private void tempOnSelection(ClanFinanceIncomeItemBaseVM temp)
		{
			this._onSelectionT(this);
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x0006C190 File Offset: 0x0006A390
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.Name = this.Workshop.WorkshopType.Name.ToString();
			this.WorkshopTypeId = this.Workshop.WorkshopType.StringId;
			base.Location = this.Workshop.Settlement.Name.ToString();
			base.Income = (int)((float)this.Workshop.ProfitMade * (1f / Campaign.Current.Models.ClanFinanceModel.RevenueSmoothenFraction()));
			base.IncomeValueText = base.DetermineIncomeText(base.Income);
			this.InputsText = GameTexts.FindText("str_clan_workshop_inputs", null).ToString();
			this.OutputsText = GameTexts.FindText("str_clan_workshop_outputs", null).ToString();
			this.StoreOutputPercentageText = new TextObject("{=y6qCNFQj}Store Outputs in the Warehouse", null).ToString();
			this.UseWarehouseAsInputText = new TextObject("{=88WPmTKH}Get Input from the Warehouse", null).ToString();
			this.WarehouseCapacityText = new TextObject("{=X6eG4Q5V}Warehouse Capacity", null).ToString();
			float warehouseItemRosterWeight = this._workshopWarehouseBehavior.GetWarehouseItemRosterWeight(this.Workshop.Settlement);
			int warehouseCapacity = Campaign.Current.Models.WorkshopModel.WarehouseCapacity;
			this.WarehouseCapacityValue = GameTexts.FindText("str_LEFT_over_RIGHT", null).SetTextVariable("LEFT", warehouseItemRosterWeight, 2).SetTextVariable("RIGHT", warehouseCapacity)
				.ToString();
			this.WarehouseInputAmount = this._workshopWarehouseBehavior.GetInputCount(this.Workshop);
			this.WarehouseOutputAmount = this._workshopWarehouseBehavior.GetOutputCount(this.Workshop);
			this._inputDetails = this._workshopWarehouseBehavior.GetInputDailyChange(this.Workshop);
			this._outputDetails = this._workshopWarehouseBehavior.GetOutputDailyChange(this.Workshop);
			this.InputWarehouseCountsTooltip.SetToolipCallback(() => this.GetWarehouseInputOutputTooltip(true));
			this.OutputWarehouseCountsTooltip.SetToolipCallback(() => this.GetWarehouseInputOutputTooltip(false));
			base.ItemProperties.Clear();
			this.PopulateStatsList();
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x0006C394 File Offset: 0x0006A594
		private List<TooltipProperty> GetWarehouseInputOutputTooltip(bool isInput)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			ExplainedNumber explainedNumber = (isInput ? this._inputDetails : this._outputDetails);
			if (!explainedNumber.ResultNumber.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				list.Add(new TooltipProperty(new TextObject("{=Y9egTJg0}Daily Change", null).ToString(), "", 1, false, TooltipProperty.TooltipPropertyFlags.Title));
				list.Add(new TooltipProperty("", "", 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator));
				foreach (ValueTuple<string, float> valueTuple in explainedNumber.GetLines())
				{
					string text = GameTexts.FindText("str_clan_workshop_material_daily_Change", null).SetTextVariable("CHANGE", MathF.Abs(valueTuple.Item2).ToString("F1")).SetTextVariable("IS_POSITIVE", (valueTuple.Item2 > 0f) ? 1 : 0)
						.ToString();
					list.Add(new TooltipProperty(valueTuple.Item1, text, 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x0006C4C0 File Offset: 0x0006A6C0
		private void RefreshStoragePercentages()
		{
			this.WarehousePercentageSelector.ItemList.Clear();
			TextObject textObject = GameTexts.FindText("str_NUMBER_percent", null);
			textObject.SetTextVariable("NUMBER", 0);
			this.WarehousePercentageSelector.AddItem(new WorkshopPercentageSelectorItemVM(textObject.ToString(), 0f));
			textObject.SetTextVariable("NUMBER", 25);
			this.WarehousePercentageSelector.AddItem(new WorkshopPercentageSelectorItemVM(textObject.ToString(), 0.25f));
			textObject.SetTextVariable("NUMBER", 50);
			this.WarehousePercentageSelector.AddItem(new WorkshopPercentageSelectorItemVM(textObject.ToString(), 0.5f));
			textObject.SetTextVariable("NUMBER", 75);
			this.WarehousePercentageSelector.AddItem(new WorkshopPercentageSelectorItemVM(textObject.ToString(), 0.75f));
			textObject.SetTextVariable("NUMBER", 100);
			this.WarehousePercentageSelector.AddItem(new WorkshopPercentageSelectorItemVM(textObject.ToString(), 1f));
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x0006C5B5 File Offset: 0x0006A7B5
		public void ExecuteToggleWarehouseUsage()
		{
			this.ReceiveInputFromWarehouse = !this.ReceiveInputFromWarehouse;
			this._workshopWarehouseBehavior.SetIsGettingInputsFromWarehouse(this.Workshop, this.ReceiveInputFromWarehouse);
			base.ItemProperties.Clear();
			this.PopulateStatsList();
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0006C5F0 File Offset: 0x0006A7F0
		protected override void PopulateStatsList()
		{
			ValueTuple<TextObject, bool, BasicTooltipViewModel> workshopStatus = this.GetWorkshopStatus(this.Workshop);
			if (!TextObject.IsNullOrEmpty(workshopStatus.Item1))
			{
				base.ItemProperties.Add(new SelectableItemPropertyVM(new TextObject("{=DXczLzml}Status", null).ToString(), workshopStatus.Item1.ToString(), workshopStatus.Item2, workshopStatus.Item3));
			}
			SelectableItemPropertyVM currentCapitalProperty = this.GetCurrentCapitalProperty();
			base.ItemProperties.Add(currentCapitalProperty);
			base.ItemProperties.Add(new SelectableItemPropertyVM(new TextObject("{=CaRbMaZY}Daily Wage", null).ToString(), this.Workshop.Expense.ToString(), false, null));
			TextObject textObject;
			TextObject textObject2;
			ClanFinanceWorkshopItemVM.GetWorkshopTypeProductionTexts(this.Workshop.WorkshopType, out textObject, out textObject2);
			this.InputProducts = textObject.ToString();
			this.OutputProducts = textObject2.ToString();
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x0006C6C4 File Offset: 0x0006A8C4
		private SelectableItemPropertyVM GetCurrentCapitalProperty()
		{
			string text3 = new TextObject("{=Ra17aK4e}Current Capital", null).ToString();
			string text2 = this.Workshop.Capital.ToString();
			bool flag = false;
			BasicTooltipViewModel basicTooltipViewModel;
			if (this.Workshop.Capital < this._workshopModel.CapitalLowLimit)
			{
				flag = true;
				basicTooltipViewModel = new BasicTooltipViewModel(() => new TextObject("{=Qu5clctb}The workshop is losing money. The expenses are being paid from your treasury because the workshop's capital is below {LOWER_THRESHOLD} denars", null).SetTextVariable("LOWER_THRESHOLD", this._workshopModel.CapitalLowLimit).ToString());
			}
			else
			{
				TextObject text = new TextObject("{=dEMUqz2Y}This workshop will send 20% of its profits above {INITIAL_CAPITAL} capital to your treasury", null);
				text.SetTextVariable("INITIAL_CAPITAL", Campaign.Current.Models.WorkshopModel.InitialCapital);
				basicTooltipViewModel = new BasicTooltipViewModel(() => text.ToString());
			}
			return new SelectableItemPropertyVM(text3, text2, flag, basicTooltipViewModel);
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0006C77C File Offset: 0x0006A97C
		[return: TupleElementNames(new string[] { "Status", "IsWarning", "Hint" })]
		private ValueTuple<TextObject, bool, BasicTooltipViewModel> GetWorkshopStatus(Workshop workshop)
		{
			bool flag = false;
			BasicTooltipViewModel basicTooltipViewModel = null;
			TextObject textObject;
			if (workshop.LastRunCampaignTime.ElapsedDaysUntilNow >= 1f)
			{
				textObject = this._haltedText;
				flag = true;
				TextObject tooltipText = TextObject.GetEmpty();
				if (workshop.Settlement.Town.InRebelliousState)
				{
					tooltipText = this._townRebellionText;
				}
				else if (!this._workshopWarehouseBehavior.IsRawMaterialsSufficientInTownMarket(workshop))
				{
					tooltipText = this._noRawMaterialsText;
				}
				else if (this.WarehousePercentageSelector.SelectedItem.Percentage < 1f)
				{
					tooltipText = this._noProfitText;
				}
				int num = (int)workshop.LastRunCampaignTime.ElapsedDaysUntilNow;
				tooltipText.SetTextVariable("DAY", num);
				tooltipText.SetTextVariable("PLURAL_DAYS", (num == 1) ? "0" : "1");
				basicTooltipViewModel = new BasicTooltipViewModel(() => tooltipText.ToString());
			}
			else
			{
				textObject = this._runningText;
			}
			return new ValueTuple<TextObject, bool, BasicTooltipViewModel>(textObject, flag, basicTooltipViewModel);
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x0006C890 File Offset: 0x0006AA90
		private static void GetWorkshopTypeProductionTexts(WorkshopType workshopType, out TextObject inputsText, out TextObject outputsText)
		{
			CampaignUIHelper.ProductInputOutputEqualityComparer productInputOutputEqualityComparer = new CampaignUIHelper.ProductInputOutputEqualityComparer();
			IEnumerable<TextObject> enumerable = from x in workshopType.Productions.SelectMany<WorkshopType.Production, ValueTuple<ItemCategory, int>>((WorkshopType.Production p) => p.Inputs).Distinct<ValueTuple<ItemCategory, int>>(productInputOutputEqualityComparer)
				select x.Item1.GetName();
			IEnumerable<TextObject> enumerable2 = from x in workshopType.Productions.SelectMany<WorkshopType.Production, ValueTuple<ItemCategory, int>>((WorkshopType.Production p) => p.Outputs).Distinct<ValueTuple<ItemCategory, int>>(productInputOutputEqualityComparer)
				select x.Item1.GetName();
			inputsText = CampaignUIHelper.GetCommaSeparatedText(null, enumerable);
			outputsText = CampaignUIHelper.GetCommaSeparatedText(null, enumerable2);
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x0006C95F File Offset: 0x0006AB5F
		public void ExecuteBeginWorkshopHint()
		{
			if (this.Workshop.WorkshopType != null)
			{
				InformationManager.ShowTooltip(typeof(Workshop), new object[] { this.Workshop });
			}
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x0006C98C File Offset: 0x0006AB8C
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x0006C994 File Offset: 0x0006AB94
		public void OnStoreOutputInWarehousePercentageUpdated(SelectorVM<WorkshopPercentageSelectorItemVM> selector)
		{
			if (selector.SelectedIndex != -1)
			{
				this._workshopWarehouseBehavior.SetStockProductionInWarehouseRatio(this.Workshop, selector.SelectedItem.Percentage);
				this._inputDetails = this._workshopWarehouseBehavior.GetInputDailyChange(this.Workshop);
				this._outputDetails = this._workshopWarehouseBehavior.GetOutputDailyChange(this.Workshop);
			}
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0006C9F4 File Offset: 0x0006ABF4
		public void ExecuteManageWorkshop()
		{
			TextObject textObject = new TextObject("{=LxWVtDF0}Manage Workshop", null);
			ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(textObject, this.GetManageWorkshopItems(), new Action<List<object>, Action>(this.OnManageWorkshopDone), false, 1, 0);
			Action<ClanCardSelectionInfo> openCardSelectionPopup = this._openCardSelectionPopup;
			if (openCardSelectionPopup == null)
			{
				return;
			}
			openCardSelectionPopup(clanCardSelectionInfo);
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0006CA3B File Offset: 0x0006AC3B
		private IEnumerable<ClanCardSelectionItemInfo> GetManageWorkshopItems()
		{
			int costForNotable = this._workshopModel.GetCostForNotable(this.Workshop);
			TextObject textObject = new TextObject("{=ysireFjT}Sell This Workshop for {GOLD_AMOUNT}{GOLD_ICON}", null);
			textObject.SetTextVariable("GOLD_AMOUNT", costForNotable);
			textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			yield return new ClanCardSelectionItemInfo(textObject, false, null, ClanCardSelectionItemPropertyInfo.CreateActionGoldChangeText(costForNotable), false);
			foreach (WorkshopType workshopType in WorkshopType.All)
			{
				if (this.Workshop.WorkshopType != workshopType && !workshopType.IsHidden)
				{
					TextObject name = workshopType.Name;
					int convertProductionCost = this._workshopModel.GetConvertProductionCost(workshopType);
					TextObject textObject2 = new TextObject("{=av51ur2M}You need at least {REQUIRED_AMOUNT} denars to change the production type of this workshop.", null);
					textObject2.SetTextVariable("REQUIRED_AMOUNT", convertProductionCost);
					bool flag = convertProductionCost <= Hero.MainHero.Gold;
					yield return new ClanCardSelectionItemInfo(workshopType, name, null, CardSelectionItemSpriteType.Workshop, workshopType.StringId, null, this.GetWorkshopItemProperties(workshopType), !flag, textObject2, ClanCardSelectionItemPropertyInfo.CreateActionGoldChangeText(-convertProductionCost), false);
				}
			}
			List<WorkshopType>.Enumerator enumerator = default(List<WorkshopType>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0006CA4B File Offset: 0x0006AC4B
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetWorkshopItemProperties(WorkshopType workshopType)
		{
			Workshop workshop = this.Workshop;
			int? num;
			if (workshop == null)
			{
				num = null;
			}
			else
			{
				Settlement settlement = workshop.Settlement;
				if (settlement == null)
				{
					num = null;
				}
				else
				{
					Town town = settlement.Town;
					if (town == null)
					{
						num = null;
					}
					else
					{
						Workshop[] workshops = town.Workshops;
						num = ((workshops != null) ? new int?(workshops.Count<Workshop>((Workshop x) => ((x != null) ? x.WorkshopType : null) == workshopType)) : null);
					}
				}
			}
			int num2 = num ?? 0;
			TextObject textObject = ((num2 == 0) ? new TextObject("{=gu5xmV0E}No other {WORKSHOP_NAME} in this town.", null) : new TextObject("{=lhIpaGt9}There {?(COUNT > 1)}are{?}is{\\?} {COUNT} more {?(COUNT > 1)}{PLURAL(WORKSHOP_NAME)}{?}{WORKSHOP_NAME}{\\?} in this town.", null));
			textObject.SetTextVariable("WORKSHOP_NAME", workshopType.Name);
			textObject.SetTextVariable("COUNT", num2);
			TextObject inputsText;
			TextObject outputsText;
			ClanFinanceWorkshopItemVM.GetWorkshopTypeProductionTexts(workshopType, out inputsText, out outputsText);
			yield return new ClanCardSelectionItemPropertyInfo(textObject);
			yield return new ClanCardSelectionItemPropertyInfo(ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(new TextObject("{=XCz81XYm}Inputs", null), inputsText));
			yield return new ClanCardSelectionItemPropertyInfo(ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(new TextObject("{=ErnykQEH}Outputs", null), outputsText));
			yield break;
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x0006CA64 File Offset: 0x0006AC64
		private void OnManageWorkshopDone(List<object> selectedItems, Action closePopup)
		{
			if (closePopup != null)
			{
				closePopup();
			}
			if (selectedItems.Count == 1)
			{
				WorkshopType workshopType = (WorkshopType)selectedItems[0];
				if (workshopType == null)
				{
					if (this.Workshop.Settlement.Town.Workshops.Count<Workshop>((Workshop x) => x.Owner == Hero.MainHero) == 1)
					{
						bool flag = Hero.MainHero.CurrentSettlement == this.Workshop.Settlement;
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=HiJTlBgF}Sell Workshop", null).ToString(), flag ? new TextObject("{=s06mScpJ}If you have goods in the warehouse, they will be transferred to your party. Are you sure?", null).ToString() : new TextObject("{=yuxBDKgM}If you have goods in the warehouse, they will be lost! Are you sure?", null).ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), new Action(this.ExecuteSellWorkshop), null, "", 0f, null, null, null), false, false);
					}
					else
					{
						this.ExecuteSellWorkshop();
					}
				}
				else
				{
					ChangeProductionTypeOfWorkshopAction.Apply(this.Workshop, workshopType, false);
				}
				Action onRefresh = this._onRefresh;
				if (onRefresh == null)
				{
					return;
				}
				onRefresh();
			}
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0006CB98 File Offset: 0x0006AD98
		private void ExecuteSellWorkshop()
		{
			Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(this.Workshop);
			ChangeOwnerOfWorkshopAction.ApplyByPlayerSelling(this.Workshop, notableOwnerForWorkshop, this.Workshop.WorkshopType);
			Action onRefresh = this._onRefresh;
			if (onRefresh == null)
			{
				return;
			}
			onRefresh();
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06001DFD RID: 7677 RVA: 0x0006CBE7 File Offset: 0x0006ADE7
		// (set) Token: 0x06001DFE RID: 7678 RVA: 0x0006CBEF File Offset: 0x0006ADEF
		[DataSourceProperty]
		public HintViewModel UseWarehouseAsInputHint
		{
			get
			{
				return this._useWarehouseAsInputHint;
			}
			set
			{
				if (value != this._useWarehouseAsInputHint)
				{
					this._useWarehouseAsInputHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UseWarehouseAsInputHint");
				}
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06001DFF RID: 7679 RVA: 0x0006CC0D File Offset: 0x0006AE0D
		// (set) Token: 0x06001E00 RID: 7680 RVA: 0x0006CC15 File Offset: 0x0006AE15
		[DataSourceProperty]
		public HintViewModel StoreOutputPercentageHint
		{
			get
			{
				return this._storeOutputPercentageHint;
			}
			set
			{
				if (value != this._storeOutputPercentageHint)
				{
					this._storeOutputPercentageHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "StoreOutputPercentageHint");
				}
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06001E01 RID: 7681 RVA: 0x0006CC33 File Offset: 0x0006AE33
		// (set) Token: 0x06001E02 RID: 7682 RVA: 0x0006CC3B File Offset: 0x0006AE3B
		[DataSourceProperty]
		public HintViewModel ManageWorkshopHint
		{
			get
			{
				return this._manageWorkshopHint;
			}
			set
			{
				if (value != this._manageWorkshopHint)
				{
					this._manageWorkshopHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ManageWorkshopHint");
				}
			}
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06001E03 RID: 7683 RVA: 0x0006CC59 File Offset: 0x0006AE59
		// (set) Token: 0x06001E04 RID: 7684 RVA: 0x0006CC61 File Offset: 0x0006AE61
		[DataSourceProperty]
		public BasicTooltipViewModel InputWarehouseCountsTooltip
		{
			get
			{
				return this._inputWarehouseCountsTooltip;
			}
			set
			{
				if (value != this._inputWarehouseCountsTooltip)
				{
					this._inputWarehouseCountsTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "InputWarehouseCountsTooltip");
				}
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06001E05 RID: 7685 RVA: 0x0006CC7F File Offset: 0x0006AE7F
		// (set) Token: 0x06001E06 RID: 7686 RVA: 0x0006CC87 File Offset: 0x0006AE87
		[DataSourceProperty]
		public BasicTooltipViewModel OutputWarehouseCountsTooltip
		{
			get
			{
				return this._outputWarehouseCountsTooltip;
			}
			set
			{
				if (value != this._outputWarehouseCountsTooltip)
				{
					this._outputWarehouseCountsTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "OutputWarehouseCountsTooltip");
				}
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06001E07 RID: 7687 RVA: 0x0006CCA5 File Offset: 0x0006AEA5
		// (set) Token: 0x06001E08 RID: 7688 RVA: 0x0006CCAD File Offset: 0x0006AEAD
		public string WorkshopTypeId
		{
			get
			{
				return this._workshopTypeId;
			}
			set
			{
				if (value != this._workshopTypeId)
				{
					this._workshopTypeId = value;
					base.OnPropertyChangedWithValue<string>(value, "WorkshopTypeId");
				}
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06001E09 RID: 7689 RVA: 0x0006CCD0 File Offset: 0x0006AED0
		// (set) Token: 0x06001E0A RID: 7690 RVA: 0x0006CCD8 File Offset: 0x0006AED8
		public string InputsText
		{
			get
			{
				return this._inputsText;
			}
			set
			{
				if (value != this._inputsText)
				{
					this._inputsText = value;
					base.OnPropertyChangedWithValue<string>(value, "InputsText");
				}
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06001E0B RID: 7691 RVA: 0x0006CCFB File Offset: 0x0006AEFB
		// (set) Token: 0x06001E0C RID: 7692 RVA: 0x0006CD03 File Offset: 0x0006AF03
		public string OutputsText
		{
			get
			{
				return this._outputsText;
			}
			set
			{
				if (value != this._outputsText)
				{
					this._outputsText = value;
					base.OnPropertyChangedWithValue<string>(value, "OutputsText");
				}
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06001E0D RID: 7693 RVA: 0x0006CD26 File Offset: 0x0006AF26
		// (set) Token: 0x06001E0E RID: 7694 RVA: 0x0006CD2E File Offset: 0x0006AF2E
		public string InputProducts
		{
			get
			{
				return this._inputProducts;
			}
			set
			{
				if (value != this._inputProducts)
				{
					this._inputProducts = value;
					base.OnPropertyChangedWithValue<string>(value, "InputProducts");
				}
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06001E0F RID: 7695 RVA: 0x0006CD51 File Offset: 0x0006AF51
		// (set) Token: 0x06001E10 RID: 7696 RVA: 0x0006CD59 File Offset: 0x0006AF59
		public string OutputProducts
		{
			get
			{
				return this._outputProducts;
			}
			set
			{
				if (value != this._outputProducts)
				{
					this._outputProducts = value;
					base.OnPropertyChangedWithValue<string>(value, "OutputProducts");
				}
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06001E11 RID: 7697 RVA: 0x0006CD7C File Offset: 0x0006AF7C
		// (set) Token: 0x06001E12 RID: 7698 RVA: 0x0006CD84 File Offset: 0x0006AF84
		public string UseWarehouseAsInputText
		{
			get
			{
				return this._useWarehouseAsInputText;
			}
			set
			{
				if (value != this._useWarehouseAsInputText)
				{
					this._useWarehouseAsInputText = value;
					base.OnPropertyChangedWithValue<string>(value, "UseWarehouseAsInputText");
				}
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06001E13 RID: 7699 RVA: 0x0006CDA7 File Offset: 0x0006AFA7
		// (set) Token: 0x06001E14 RID: 7700 RVA: 0x0006CDAF File Offset: 0x0006AFAF
		public string StoreOutputPercentageText
		{
			get
			{
				return this._storeOutputPercentageText;
			}
			set
			{
				if (value != this._storeOutputPercentageText)
				{
					this._storeOutputPercentageText = value;
					base.OnPropertyChangedWithValue<string>(value, "StoreOutputPercentageText");
				}
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06001E15 RID: 7701 RVA: 0x0006CDD2 File Offset: 0x0006AFD2
		// (set) Token: 0x06001E16 RID: 7702 RVA: 0x0006CDDA File Offset: 0x0006AFDA
		public string WarehouseCapacityText
		{
			get
			{
				return this._warehouseCapacityText;
			}
			set
			{
				if (value != this._warehouseCapacityText)
				{
					this._warehouseCapacityText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarehouseCapacityText");
				}
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x0006CDFD File Offset: 0x0006AFFD
		// (set) Token: 0x06001E18 RID: 7704 RVA: 0x0006CE05 File Offset: 0x0006B005
		public string WarehouseCapacityValue
		{
			get
			{
				return this._warehouseCapacityValue;
			}
			set
			{
				if (value != this._warehouseCapacityValue)
				{
					this._warehouseCapacityValue = value;
					base.OnPropertyChangedWithValue<string>(value, "WarehouseCapacityValue");
				}
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06001E19 RID: 7705 RVA: 0x0006CE28 File Offset: 0x0006B028
		// (set) Token: 0x06001E1A RID: 7706 RVA: 0x0006CE30 File Offset: 0x0006B030
		public bool ReceiveInputFromWarehouse
		{
			get
			{
				return this._receiveInputFromWarehouse;
			}
			set
			{
				if (value != this._receiveInputFromWarehouse)
				{
					this._receiveInputFromWarehouse = value;
					base.OnPropertyChangedWithValue(value, "ReceiveInputFromWarehouse");
				}
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06001E1B RID: 7707 RVA: 0x0006CE4E File Offset: 0x0006B04E
		// (set) Token: 0x06001E1C RID: 7708 RVA: 0x0006CE56 File Offset: 0x0006B056
		public int WarehouseInputAmount
		{
			get
			{
				return this._warehouseInputAmount;
			}
			set
			{
				if (value != this._warehouseInputAmount)
				{
					this._warehouseInputAmount = value;
					base.OnPropertyChangedWithValue(value, "WarehouseInputAmount");
				}
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06001E1D RID: 7709 RVA: 0x0006CE74 File Offset: 0x0006B074
		// (set) Token: 0x06001E1E RID: 7710 RVA: 0x0006CE7C File Offset: 0x0006B07C
		public int WarehouseOutputAmount
		{
			get
			{
				return this._warehouseOutputAmount;
			}
			set
			{
				if (value != this._warehouseOutputAmount)
				{
					this._warehouseOutputAmount = value;
					base.OnPropertyChangedWithValue(value, "WarehouseOutputAmount");
				}
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06001E1F RID: 7711 RVA: 0x0006CE9A File Offset: 0x0006B09A
		// (set) Token: 0x06001E20 RID: 7712 RVA: 0x0006CEA2 File Offset: 0x0006B0A2
		public SelectorVM<WorkshopPercentageSelectorItemVM> WarehousePercentageSelector
		{
			get
			{
				return this._warehousePercentageSelector;
			}
			set
			{
				if (value != this._warehousePercentageSelector)
				{
					this._warehousePercentageSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<WorkshopPercentageSelectorItemVM>>(value, "WarehousePercentageSelector");
				}
			}
		}

		// Token: 0x04000DAC RID: 3500
		private readonly TextObject _runningText = new TextObject("{=iuKvbKJ7}Running", null);

		// Token: 0x04000DAD RID: 3501
		private readonly TextObject _haltedText = new TextObject("{=zgnEagTJ}Halted", null);

		// Token: 0x04000DAE RID: 3502
		private readonly TextObject _noRawMaterialsText = new TextObject("{=JRKC4ed4}This workshop has not been producing for {DAY} {?PLURAL_DAYS}days{?}day{\\?} due to lack of raw materials in the town market.", null);

		// Token: 0x04000DAF RID: 3503
		private readonly TextObject _noProfitText = new TextObject("{=no0chrAH}This workshop has not been running for {DAY} {?PLURAL_DAYS}days{?}day{\\?} because the production has not been profitable", null);

		// Token: 0x04000DB0 RID: 3504
		private readonly TextObject _townRebellionText = new TextObject("{=pDAuV918}This workshop has not been producing for {DAY} {?PLURAL_DAYS}days{?}day{\\?} due to rebel activity in the town.", null);

		// Token: 0x04000DB1 RID: 3505
		private readonly IWorkshopWarehouseCampaignBehavior _workshopWarehouseBehavior;

		// Token: 0x04000DB2 RID: 3506
		private readonly WorkshopModel _workshopModel;

		// Token: 0x04000DB3 RID: 3507
		private readonly Action<ClanCardSelectionInfo> _openCardSelectionPopup;

		// Token: 0x04000DB4 RID: 3508
		private readonly Action<ClanFinanceWorkshopItemVM> _onSelectionT;

		// Token: 0x04000DB5 RID: 3509
		private ExplainedNumber _inputDetails;

		// Token: 0x04000DB6 RID: 3510
		private ExplainedNumber _outputDetails;

		// Token: 0x04000DB7 RID: 3511
		private HintViewModel _useWarehouseAsInputHint;

		// Token: 0x04000DB8 RID: 3512
		private HintViewModel _storeOutputPercentageHint;

		// Token: 0x04000DB9 RID: 3513
		private HintViewModel _manageWorkshopHint;

		// Token: 0x04000DBA RID: 3514
		private BasicTooltipViewModel _inputWarehouseCountsTooltip;

		// Token: 0x04000DBB RID: 3515
		private BasicTooltipViewModel _outputWarehouseCountsTooltip;

		// Token: 0x04000DBC RID: 3516
		private string _workshopTypeId;

		// Token: 0x04000DBD RID: 3517
		private string _inputsText;

		// Token: 0x04000DBE RID: 3518
		private string _outputsText;

		// Token: 0x04000DBF RID: 3519
		private string _inputProducts;

		// Token: 0x04000DC0 RID: 3520
		private string _outputProducts;

		// Token: 0x04000DC1 RID: 3521
		private string _useWarehouseAsInputText;

		// Token: 0x04000DC2 RID: 3522
		private string _storeOutputPercentageText;

		// Token: 0x04000DC3 RID: 3523
		private string _warehouseCapacityText;

		// Token: 0x04000DC4 RID: 3524
		private string _warehouseCapacityValue;

		// Token: 0x04000DC5 RID: 3525
		private bool _receiveInputFromWarehouse;

		// Token: 0x04000DC6 RID: 3526
		private int _warehouseInputAmount;

		// Token: 0x04000DC7 RID: 3527
		private int _warehouseOutputAmount;

		// Token: 0x04000DC8 RID: 3528
		private SelectorVM<WorkshopPercentageSelectorItemVM> _warehousePercentageSelector;
	}
}
