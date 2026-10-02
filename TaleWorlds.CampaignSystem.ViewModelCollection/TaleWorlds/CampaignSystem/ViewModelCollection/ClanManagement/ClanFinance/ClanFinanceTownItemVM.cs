using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance
{
	// Token: 0x0200013A RID: 314
	public class ClanFinanceTownItemVM : ClanFinanceIncomeItemBaseVM
	{
		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x0006BCFE File Offset: 0x00069EFE
		// (set) Token: 0x06001DD9 RID: 7641 RVA: 0x0006BD06 File Offset: 0x00069F06
		public Settlement Settlement { get; private set; }

		// Token: 0x06001DDA RID: 7642 RVA: 0x0006BD10 File Offset: 0x00069F10
		public ClanFinanceTownItemVM(Settlement settlement, TaxType taxType, Action<ClanFinanceIncomeItemBaseVM> onSelection, Action onRefresh)
			: base(onSelection, onRefresh)
		{
			base.IncomeTypeAsEnum = IncomeTypes.Settlement;
			this.Settlement = settlement;
			MBTextManager.SetTextVariable("SETTLEMENT_NAME", settlement.Name.ToString(), false);
			base.Name = ((taxType == TaxType.ProsperityTax) ? GameTexts.FindText("str_prosperity_tax", null).ToString() : GameTexts.FindText("str_trade_tax", null).ToString());
			this.IsUnderSiege = settlement.IsUnderSiege;
			this.IsUnderSiegeHint = new HintViewModel(new TextObject("{=!}PLACEHOLDER | THIS SETTLEMENT IS UNDER SIEGE", null), null);
			this.IsUnderRebellion = settlement.IsUnderRebellionAttack();
			this.IsUnderRebellionHint = new HintViewModel(new TextObject("{=!}PLACEHOLDER | THIS SETTLEMENT IS UNDER REBELLION", null), null);
			if (taxType == TaxType.ProsperityTax && settlement.Town != null)
			{
				float resultNumber = Campaign.Current.Models.SettlementTaxModel.CalculateTownTax(settlement.Town, false).ResultNumber;
				base.Income = (this.IsUnderRebellion ? 0 : ((int)resultNumber));
			}
			else if (taxType == TaxType.TradeTax)
			{
				if (settlement.Town != null)
				{
					base.Income = (int)((float)settlement.Town.TradeTaxAccumulated / Campaign.Current.Models.ClanFinanceModel.RevenueSmoothenFraction());
				}
				else if (settlement.Village != null)
				{
					base.Income = ((settlement.Village.VillageState == Village.VillageStates.Looted || settlement.Village.VillageState == Village.VillageStates.BeingRaided) ? 0 : ((int)((float)settlement.Village.TradeTaxAccumulated / Campaign.Current.Models.ClanFinanceModel.RevenueSmoothenFraction())));
				}
			}
			base.IncomeValueText = base.DetermineIncomeText(base.Income);
			this.HasGovernor = settlement.IsTown && settlement.Town.Governor != null;
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x0006BEBB File Offset: 0x0006A0BB
		protected override void PopulateActionList()
		{
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0006BEBD File Offset: 0x0006A0BD
		protected override void PopulateStatsList()
		{
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06001DDD RID: 7645 RVA: 0x0006BEBF File Offset: 0x0006A0BF
		// (set) Token: 0x06001DDE RID: 7646 RVA: 0x0006BEC7 File Offset: 0x0006A0C7
		[DataSourceProperty]
		public bool IsUnderSiege
		{
			get
			{
				return this._isUnderSiege;
			}
			set
			{
				if (value != this._isUnderSiege)
				{
					this._isUnderSiege = value;
					base.OnPropertyChangedWithValue(value, "IsUnderSiege");
				}
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x0006BEE5 File Offset: 0x0006A0E5
		// (set) Token: 0x06001DE0 RID: 7648 RVA: 0x0006BEED File Offset: 0x0006A0ED
		[DataSourceProperty]
		public bool IsUnderRebellion
		{
			get
			{
				return this._isUnderRebellion;
			}
			set
			{
				if (value != this._isUnderRebellion)
				{
					this._isUnderRebellion = value;
					base.OnPropertyChangedWithValue(value, "IsUnderRebellion");
				}
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x0006BF0B File Offset: 0x0006A10B
		// (set) Token: 0x06001DE2 RID: 7650 RVA: 0x0006BF13 File Offset: 0x0006A113
		[DataSourceProperty]
		public HintViewModel IsUnderSiegeHint
		{
			get
			{
				return this._isUnderSiegeHint;
			}
			set
			{
				if (value != this._isUnderSiegeHint)
				{
					this._isUnderSiegeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "IsUnderSiegeHint");
				}
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x0006BF31 File Offset: 0x0006A131
		// (set) Token: 0x06001DE4 RID: 7652 RVA: 0x0006BF39 File Offset: 0x0006A139
		[DataSourceProperty]
		public HintViewModel IsUnderRebellionHint
		{
			get
			{
				return this._isUnderRebellionHint;
			}
			set
			{
				if (value != this._isUnderRebellionHint)
				{
					this._isUnderRebellionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "IsUnderRebellionHint");
				}
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06001DE5 RID: 7653 RVA: 0x0006BF57 File Offset: 0x0006A157
		// (set) Token: 0x06001DE6 RID: 7654 RVA: 0x0006BF5F File Offset: 0x0006A15F
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

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x0006BF7D File Offset: 0x0006A17D
		// (set) Token: 0x06001DE8 RID: 7656 RVA: 0x0006BF85 File Offset: 0x0006A185
		[DataSourceProperty]
		public HintViewModel GovernorHint
		{
			get
			{
				return this._governorHint;
			}
			set
			{
				if (value != this._governorHint)
				{
					this._governorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GovernorHint");
				}
			}
		}

		// Token: 0x04000DA5 RID: 3493
		private bool _isUnderSiege;

		// Token: 0x04000DA6 RID: 3494
		private bool _isUnderRebellion;

		// Token: 0x04000DA7 RID: 3495
		private HintViewModel _isUnderSiegeHint;

		// Token: 0x04000DA8 RID: 3496
		private HintViewModel _isUnderRebellionHint;

		// Token: 0x04000DA9 RID: 3497
		private HintViewModel _governorHint;

		// Token: 0x04000DAA RID: 3498
		private bool _hasGovernor;
	}
}
