using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000128 RID: 296
	public class ClanFinanceExpenseItemVM : ViewModel
	{
		// Token: 0x06001AAC RID: 6828 RVA: 0x00064B40 File Offset: 0x00062D40
		public ClanFinanceExpenseItemVM(MobileParty mobileParty)
		{
			this._mobileParty = mobileParty;
			this.CurrentWageTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetPartyWageTooltip(mobileParty));
			this.MinWage = 100;
			this.MaxWage = 2000;
			this.CurrentWage = this._mobileParty.TotalWage;
			this.CurrentWageValueText = this.CurrentWage.ToString();
			this.IsUnlimitedWage = !this._mobileParty.HasLimitedWage();
			this.CurrentWageLimit = ((this._mobileParty.PaymentLimit == Campaign.Current.Models.PartyWageModel.MaxWagePaymentLimit) ? 2000 : this._mobileParty.PaymentLimit);
			this.IsEnabled = true;
			this.RefreshValues();
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00064C18 File Offset: 0x00062E18
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CurrentWageText = new TextObject("{=pnFgwLYG}Current Wage", null).ToString();
			this.CurrentWageLimitText = new TextObject("{=sWWxrafa}Current Limit", null).ToString();
			this.TitleText = new TextObject("{=qdoJOH0j}Party Wage", null).ToString();
			this.UnlimitedWageText = new TextObject("{=lC5xsoSh}Unlimited", null).ToString();
			this.WageLimitHint = new HintViewModel(new TextObject("{=w0slxNAl}If limit is lower than current wage, party will not recruit troops until wage is reduced to the limit. If limit is higher than current wage, party will keep recruiting.", null), null);
			this.UpdateCurrentWageLimitText();
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x00064CA0 File Offset: 0x00062EA0
		private void OnCurrentWageLimitUpdated(int newValue)
		{
			if (!this.IsUnlimitedWage)
			{
				this._mobileParty.SetWagePaymentLimit(newValue);
			}
			this.UpdateCurrentWageLimitText();
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x00064CBC File Offset: 0x00062EBC
		private void OnUnlimitedWageToggled(bool newValue)
		{
			this.CurrentWageLimit = 2000;
			if (newValue)
			{
				this._mobileParty.SetWagePaymentLimit(Campaign.Current.Models.PartyWageModel.MaxWagePaymentLimit);
			}
			else
			{
				this._mobileParty.SetWagePaymentLimit(2000);
			}
			this.UpdateCurrentWageLimitText();
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x00064D10 File Offset: 0x00062F10
		private void UpdateCurrentWageLimitText()
		{
			this.CurrentWageLimitValueText = (this.IsUnlimitedWage ? new TextObject("{=lC5xsoSh}Unlimited", null).ToString() : this.CurrentWageLimit.ToString());
			this.CurrentWageOverLimitText = new TextObject("{=!}{CURRENT}/{LIMIT}", null).SetTextVariable("CURRENT", this.CurrentWage).SetTextVariable("LIMIT", this.CurrentWageLimitValueText).ToString();
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x00064D81 File Offset: 0x00062F81
		// (set) Token: 0x06001AB2 RID: 6834 RVA: 0x00064D89 File Offset: 0x00062F89
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06001AB3 RID: 6835 RVA: 0x00064DA7 File Offset: 0x00062FA7
		// (set) Token: 0x06001AB4 RID: 6836 RVA: 0x00064DAF File Offset: 0x00062FAF
		[DataSourceProperty]
		public HintViewModel WageLimitHint
		{
			get
			{
				return this._wageLimitHint;
			}
			set
			{
				if (value != this._wageLimitHint)
				{
					this._wageLimitHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "WageLimitHint");
				}
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x00064DCD File Offset: 0x00062FCD
		// (set) Token: 0x06001AB6 RID: 6838 RVA: 0x00064DD5 File Offset: 0x00062FD5
		[DataSourceProperty]
		public BasicTooltipViewModel CurrentWageTooltip
		{
			get
			{
				return this._currentWageTooltip;
			}
			set
			{
				if (value != this._currentWageTooltip)
				{
					this._currentWageTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CurrentWageTooltip");
				}
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06001AB7 RID: 6839 RVA: 0x00064DF3 File Offset: 0x00062FF3
		// (set) Token: 0x06001AB8 RID: 6840 RVA: 0x00064DFB File Offset: 0x00062FFB
		[DataSourceProperty]
		public string CurrentWageText
		{
			get
			{
				return this._currentWageText;
			}
			set
			{
				if (value != this._currentWageText)
				{
					this._currentWageText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWageText");
				}
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06001AB9 RID: 6841 RVA: 0x00064E1E File Offset: 0x0006301E
		// (set) Token: 0x06001ABA RID: 6842 RVA: 0x00064E26 File Offset: 0x00063026
		[DataSourceProperty]
		public string CurrentWageLimitText
		{
			get
			{
				return this._currentWageLimitText;
			}
			set
			{
				if (value != this._currentWageLimitText)
				{
					this._currentWageLimitText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWageLimitText");
				}
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06001ABB RID: 6843 RVA: 0x00064E49 File Offset: 0x00063049
		// (set) Token: 0x06001ABC RID: 6844 RVA: 0x00064E51 File Offset: 0x00063051
		[DataSourceProperty]
		public string CurrentWageValueText
		{
			get
			{
				return this._currentWageValueText;
			}
			set
			{
				if (value != this._currentWageValueText)
				{
					this._currentWageValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWageValueText");
				}
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x00064E74 File Offset: 0x00063074
		// (set) Token: 0x06001ABE RID: 6846 RVA: 0x00064E7C File Offset: 0x0006307C
		[DataSourceProperty]
		public string CurrentWageLimitValueText
		{
			get
			{
				return this._currentWageLimitValueText;
			}
			set
			{
				if (value != this._currentWageLimitValueText)
				{
					this._currentWageLimitValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWageLimitValueText");
				}
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x00064E9F File Offset: 0x0006309F
		// (set) Token: 0x06001AC0 RID: 6848 RVA: 0x00064EA7 File Offset: 0x000630A7
		[DataSourceProperty]
		public string CurrentWageOverLimitText
		{
			get
			{
				return this._currentWageOverLimitText;
			}
			set
			{
				if (value != this._currentWageOverLimitText)
				{
					this._currentWageOverLimitText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWageOverLimitText");
				}
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x00064ECA File Offset: 0x000630CA
		// (set) Token: 0x06001AC2 RID: 6850 RVA: 0x00064ED2 File Offset: 0x000630D2
		[DataSourceProperty]
		public string UnlimitedWageText
		{
			get
			{
				return this._unlimitedWageText;
			}
			set
			{
				if (value != this._unlimitedWageText)
				{
					this._unlimitedWageText = value;
					base.OnPropertyChangedWithValue<string>(value, "UnlimitedWageText");
				}
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x00064EF5 File Offset: 0x000630F5
		// (set) Token: 0x06001AC4 RID: 6852 RVA: 0x00064EFD File Offset: 0x000630FD
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

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x00064F20 File Offset: 0x00063120
		// (set) Token: 0x06001AC6 RID: 6854 RVA: 0x00064F28 File Offset: 0x00063128
		[DataSourceProperty]
		public int CurrentWage
		{
			get
			{
				return this._currentWage;
			}
			set
			{
				if (value != this._currentWage)
				{
					this._currentWage = value;
					base.OnPropertyChangedWithValue(value, "CurrentWage");
				}
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x00064F46 File Offset: 0x00063146
		// (set) Token: 0x06001AC8 RID: 6856 RVA: 0x00064F4E File Offset: 0x0006314E
		[DataSourceProperty]
		public int CurrentWageLimit
		{
			get
			{
				return this._currentWageLimit;
			}
			set
			{
				if (value != this._currentWageLimit)
				{
					this._currentWageLimit = value;
					base.OnPropertyChangedWithValue(value, "CurrentWageLimit");
					this.OnCurrentWageLimitUpdated(value);
				}
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06001AC9 RID: 6857 RVA: 0x00064F73 File Offset: 0x00063173
		// (set) Token: 0x06001ACA RID: 6858 RVA: 0x00064F7B File Offset: 0x0006317B
		[DataSourceProperty]
		public int MinWage
		{
			get
			{
				return this._minWage;
			}
			set
			{
				if (value != this._minWage)
				{
					this._minWage = value;
					base.OnPropertyChangedWithValue(value, "MinWage");
				}
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06001ACB RID: 6859 RVA: 0x00064F99 File Offset: 0x00063199
		// (set) Token: 0x06001ACC RID: 6860 RVA: 0x00064FA1 File Offset: 0x000631A1
		[DataSourceProperty]
		public int MaxWage
		{
			get
			{
				return this._maxWage;
			}
			set
			{
				if (value != this._maxWage)
				{
					this._maxWage = value;
					base.OnPropertyChangedWithValue(value, "MaxWage");
				}
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x00064FBF File Offset: 0x000631BF
		// (set) Token: 0x06001ACE RID: 6862 RVA: 0x00064FC7 File Offset: 0x000631C7
		[DataSourceProperty]
		public bool IsUnlimitedWage
		{
			get
			{
				return this._isUnlimitedWage;
			}
			set
			{
				if (value != this._isUnlimitedWage)
				{
					this._isUnlimitedWage = value;
					base.OnPropertyChangedWithValue(value, "IsUnlimitedWage");
					this.OnUnlimitedWageToggled(value);
				}
			}
		}

		// Token: 0x04000C55 RID: 3157
		private const int UIWageSliderMaxLimit = 2000;

		// Token: 0x04000C56 RID: 3158
		private const int UIWageSliderMinLimit = 100;

		// Token: 0x04000C57 RID: 3159
		private readonly MobileParty _mobileParty;

		// Token: 0x04000C58 RID: 3160
		private bool _isEnabled;

		// Token: 0x04000C59 RID: 3161
		private int _minWage;

		// Token: 0x04000C5A RID: 3162
		private int _maxWage;

		// Token: 0x04000C5B RID: 3163
		private int _currentWage;

		// Token: 0x04000C5C RID: 3164
		private int _currentWageLimit;

		// Token: 0x04000C5D RID: 3165
		private string _currentWageText;

		// Token: 0x04000C5E RID: 3166
		private string _currentWageLimitText;

		// Token: 0x04000C5F RID: 3167
		private string _currentWageValueText;

		// Token: 0x04000C60 RID: 3168
		private string _currentWageLimitValueText;

		// Token: 0x04000C61 RID: 3169
		private string _currentWageOverLimitText;

		// Token: 0x04000C62 RID: 3170
		private string _unlimitedWageText;

		// Token: 0x04000C63 RID: 3171
		private string _titleText;

		// Token: 0x04000C64 RID: 3172
		private bool _isUnlimitedWage;

		// Token: 0x04000C65 RID: 3173
		private HintViewModel _wageLimitHint;

		// Token: 0x04000C66 RID: 3174
		private BasicTooltipViewModel _currentWageTooltip;
	}
}
