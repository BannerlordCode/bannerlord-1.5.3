using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000112 RID: 274
	public class WeaponDesignResultPropertyItemVM : ViewModel
	{
		// Token: 0x06001821 RID: 6177 RVA: 0x0005C434 File Offset: 0x0005A634
		public WeaponDesignResultPropertyItemVM(TextObject description, float value, float changeAmount, bool showFloatingPoint)
		{
			this._description = description;
			this.InitialValue = value;
			this.ChangeAmount = changeAmount;
			this.ShowFloatingPoint = showFloatingPoint;
			this.IsOrderResult = false;
			this.OrderRequirementTooltip = new HintViewModel();
			this.CraftedValueTooltip = new HintViewModel();
			this.BonusPenaltyTooltip = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0005C494 File Offset: 0x0005A694
		public WeaponDesignResultPropertyItemVM(TextObject description, float craftedValue, float requiredValue, float changeAmount, bool showFloatingPoint, bool isExceedingBeneficial, bool showTooltip = true)
		{
			this._showTooltip = showTooltip;
			this._description = description;
			this.TargetValue = requiredValue;
			this.InitialValue = craftedValue;
			this.ChangeAmount = changeAmount;
			this._isExceedingBeneficial = isExceedingBeneficial;
			this.IsOrderResult = true;
			this.ShowFloatingPoint = showFloatingPoint;
			this.OrderRequirementTooltip = new HintViewModel();
			this.CraftedValueTooltip = new HintViewModel();
			this.BonusPenaltyTooltip = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0005C50C File Offset: 0x0005A70C
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject description = this._description;
			this.PropertyLbl = ((description != null) ? description.ToString() : null);
			TextObject textObject = GameTexts.FindText("str_STR_in_parentheses", null);
			textObject.SetTextVariable("STR", CampaignUIHelper.GetFormattedItemPropertyText(this.TargetValue, this.ShowFloatingPoint));
			this.RequiredValueText = ((this.TargetValue == 0f) ? string.Empty : textObject.ToString());
			this.HasBenefit = (this._isExceedingBeneficial ? (this.InitialValue + this.ChangeAmount >= this.TargetValue) : (this.InitialValue + this.ChangeAmount <= this.TargetValue));
			this.OrderRequirementTooltip.HintText = (this._showTooltip ? GameTexts.FindText("str_crafting_order_requirement_tooltip", null) : TextObject.GetEmpty());
			this.CraftedValueTooltip.HintText = (this._showTooltip ? GameTexts.FindText("str_crafting_crafted_value_tooltip", null) : TextObject.GetEmpty());
			this.BonusPenaltyTooltip.HintText = (this._showTooltip ? GameTexts.FindText("str_crafting_bonus_penalty_tooltip", null) : TextObject.GetEmpty());
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06001824 RID: 6180 RVA: 0x0005C62F File Offset: 0x0005A82F
		// (set) Token: 0x06001825 RID: 6181 RVA: 0x0005C637 File Offset: 0x0005A837
		[DataSourceProperty]
		public string PropertyLbl
		{
			get
			{
				return this._propertyLbl;
			}
			set
			{
				if (value != this._propertyLbl)
				{
					this._propertyLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "PropertyLbl");
				}
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06001826 RID: 6182 RVA: 0x0005C65A File Offset: 0x0005A85A
		// (set) Token: 0x06001827 RID: 6183 RVA: 0x0005C662 File Offset: 0x0005A862
		[DataSourceProperty]
		public float InitialValue
		{
			get
			{
				return this._propertyValue;
			}
			set
			{
				if (value == 0f || value != this._propertyValue)
				{
					this._propertyValue = value;
					base.OnPropertyChangedWithValue(value, "InitialValue");
				}
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x0005C688 File Offset: 0x0005A888
		// (set) Token: 0x06001829 RID: 6185 RVA: 0x0005C690 File Offset: 0x0005A890
		[DataSourceProperty]
		public float TargetValue
		{
			get
			{
				return this._requiredValue;
			}
			set
			{
				if (value != this._requiredValue)
				{
					this._requiredValue = value;
					base.OnPropertyChangedWithValue(value, "TargetValue");
				}
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x0600182A RID: 6186 RVA: 0x0005C6AE File Offset: 0x0005A8AE
		// (set) Token: 0x0600182B RID: 6187 RVA: 0x0005C6B6 File Offset: 0x0005A8B6
		[DataSourceProperty]
		public string RequiredValueText
		{
			get
			{
				return this._requiredValueText;
			}
			set
			{
				if (value != this._requiredValueText)
				{
					this._requiredValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "RequiredValueText");
				}
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x0600182C RID: 6188 RVA: 0x0005C6D9 File Offset: 0x0005A8D9
		// (set) Token: 0x0600182D RID: 6189 RVA: 0x0005C6E1 File Offset: 0x0005A8E1
		[DataSourceProperty]
		public float ChangeAmount
		{
			get
			{
				return this._changeAmount;
			}
			set
			{
				if (this._changeAmount != value)
				{
					this._changeAmount = value;
					base.OnPropertyChangedWithValue(value, "ChangeAmount");
				}
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x0600182E RID: 6190 RVA: 0x0005C6FF File Offset: 0x0005A8FF
		// (set) Token: 0x0600182F RID: 6191 RVA: 0x0005C707 File Offset: 0x0005A907
		[DataSourceProperty]
		public bool ShowFloatingPoint
		{
			get
			{
				return this._showFloatingPoint;
			}
			set
			{
				if (this._showFloatingPoint != value)
				{
					this._showFloatingPoint = value;
					base.OnPropertyChangedWithValue(value, "ShowFloatingPoint");
				}
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06001830 RID: 6192 RVA: 0x0005C725 File Offset: 0x0005A925
		// (set) Token: 0x06001831 RID: 6193 RVA: 0x0005C72D File Offset: 0x0005A92D
		[DataSourceProperty]
		public bool IsOrderResult
		{
			get
			{
				return this._isOrderResult;
			}
			set
			{
				if (value != this._isOrderResult)
				{
					this._isOrderResult = value;
					base.OnPropertyChangedWithValue(value, "IsOrderResult");
				}
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06001832 RID: 6194 RVA: 0x0005C74B File Offset: 0x0005A94B
		// (set) Token: 0x06001833 RID: 6195 RVA: 0x0005C753 File Offset: 0x0005A953
		[DataSourceProperty]
		public bool HasBenefit
		{
			get
			{
				return this._hasBenefit;
			}
			set
			{
				if (value != this._hasBenefit)
				{
					this._hasBenefit = value;
					base.OnPropertyChangedWithValue(value, "HasBenefit");
				}
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06001834 RID: 6196 RVA: 0x0005C771 File Offset: 0x0005A971
		// (set) Token: 0x06001835 RID: 6197 RVA: 0x0005C779 File Offset: 0x0005A979
		[DataSourceProperty]
		public HintViewModel OrderRequirementTooltip
		{
			get
			{
				return this._orderRequirementTooltip;
			}
			set
			{
				if (value != this._orderRequirementTooltip)
				{
					this._orderRequirementTooltip = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "OrderRequirementTooltip");
				}
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06001836 RID: 6198 RVA: 0x0005C797 File Offset: 0x0005A997
		// (set) Token: 0x06001837 RID: 6199 RVA: 0x0005C79F File Offset: 0x0005A99F
		[DataSourceProperty]
		public HintViewModel CraftedValueTooltip
		{
			get
			{
				return this._craftedValueTooltip;
			}
			set
			{
				if (value != this._craftedValueTooltip)
				{
					this._craftedValueTooltip = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CraftedValueTooltip");
				}
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06001838 RID: 6200 RVA: 0x0005C7BD File Offset: 0x0005A9BD
		// (set) Token: 0x06001839 RID: 6201 RVA: 0x0005C7C5 File Offset: 0x0005A9C5
		[DataSourceProperty]
		public HintViewModel BonusPenaltyTooltip
		{
			get
			{
				return this._bonusPenaltyTooltip;
			}
			set
			{
				if (value != this._bonusPenaltyTooltip)
				{
					this._bonusPenaltyTooltip = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BonusPenaltyTooltip");
				}
			}
		}

		// Token: 0x04000B05 RID: 2821
		private readonly TextObject _description;

		// Token: 0x04000B06 RID: 2822
		private bool _isExceedingBeneficial;

		// Token: 0x04000B07 RID: 2823
		private bool _showTooltip;

		// Token: 0x04000B08 RID: 2824
		private string _propertyLbl;

		// Token: 0x04000B09 RID: 2825
		private float _propertyValue;

		// Token: 0x04000B0A RID: 2826
		private float _requiredValue;

		// Token: 0x04000B0B RID: 2827
		private string _requiredValueText;

		// Token: 0x04000B0C RID: 2828
		private float _changeAmount;

		// Token: 0x04000B0D RID: 2829
		private bool _showFloatingPoint;

		// Token: 0x04000B0E RID: 2830
		private bool _isOrderResult;

		// Token: 0x04000B0F RID: 2831
		private bool _hasBenefit;

		// Token: 0x04000B10 RID: 2832
		private HintViewModel _orderRequirementTooltip;

		// Token: 0x04000B11 RID: 2833
		private HintViewModel _craftedValueTooltip;

		// Token: 0x04000B12 RID: 2834
		private HintViewModel _bonusPenaltyTooltip;
	}
}
