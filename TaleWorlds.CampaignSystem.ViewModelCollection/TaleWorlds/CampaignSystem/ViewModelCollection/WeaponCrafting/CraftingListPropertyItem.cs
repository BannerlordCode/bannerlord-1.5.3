using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x02000100 RID: 256
	public class CraftingListPropertyItem : ViewModel
	{
		// Token: 0x060016C7 RID: 5831 RVA: 0x00058CEC File Offset: 0x00056EEC
		public CraftingListPropertyItem(TextObject description, float maxValue, float value, float targetValue, CraftingTemplate.CraftingStatTypes propertyType, bool isAlternativeUsageProperty = false)
		{
			this.Description = description;
			this.PropertyMaxValue = maxValue;
			this.PropertyValue = value;
			this.TargetValue = targetValue;
			this.IsAlternativeUsageProperty = isAlternativeUsageProperty;
			this.Type = propertyType;
			this.RefreshValues();
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x00058D40 File Offset: 0x00056F40
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.HasValidTarget = this.TargetValue > float.Epsilon;
			this.HasValidValue = this.PropertyValue > float.Epsilon;
			TextObject description = this.Description;
			this.PropertyLbl = ((description != null) ? description.ToString() : null);
			this.IsExceedingBeneficial = this.CheckIfExceedingIsBeneficial();
			this.SeparatorText = new TextObject("{=dB6cFDmz}/", null).ToString();
			this.PropertyValueText = CampaignUIHelper.GetFormattedItemPropertyText(this.PropertyValue, this.GetIsTypeRequireInteger(this.Type));
			if (this.HasValidTarget)
			{
				this.TargetValueText = CampaignUIHelper.GetFormattedItemPropertyText(this.TargetValue, this.GetIsTypeRequireInteger(this.Type));
			}
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00058DF5 File Offset: 0x00056FF5
		private bool CheckIfExceedingIsBeneficial()
		{
			return this.Type > CraftingTemplate.CraftingStatTypes.Weight;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00058E00 File Offset: 0x00057000
		private bool GetIsTypeRequireInteger(CraftingTemplate.CraftingStatTypes type)
		{
			return type == CraftingTemplate.CraftingStatTypes.StackAmount;
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x060016CB RID: 5835 RVA: 0x00058E07 File Offset: 0x00057007
		// (set) Token: 0x060016CC RID: 5836 RVA: 0x00058E0F File Offset: 0x0005700F
		[DataSourceProperty]
		public bool IsValidForUsage
		{
			get
			{
				return this._showStats;
			}
			set
			{
				if (value != this._showStats)
				{
					this._showStats = value;
					base.OnPropertyChangedWithValue(value, "IsValidForUsage");
				}
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x060016CD RID: 5837 RVA: 0x00058E2D File Offset: 0x0005702D
		// (set) Token: 0x060016CE RID: 5838 RVA: 0x00058E35 File Offset: 0x00057035
		[DataSourceProperty]
		public bool IsExceedingBeneficial
		{
			get
			{
				return this._isExceedingBeneficial;
			}
			set
			{
				if (value != this._isExceedingBeneficial)
				{
					this._isExceedingBeneficial = value;
					base.OnPropertyChangedWithValue(value, "IsExceedingBeneficial");
				}
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060016CF RID: 5839 RVA: 0x00058E53 File Offset: 0x00057053
		// (set) Token: 0x060016D0 RID: 5840 RVA: 0x00058E5B File Offset: 0x0005705B
		[DataSourceProperty]
		public bool HasValidTarget
		{
			get
			{
				return this._hasValidTarget;
			}
			set
			{
				if (value != this._hasValidTarget)
				{
					this._hasValidTarget = value;
					base.OnPropertyChangedWithValue(value, "HasValidTarget");
				}
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060016D1 RID: 5841 RVA: 0x00058E79 File Offset: 0x00057079
		// (set) Token: 0x060016D2 RID: 5842 RVA: 0x00058E81 File Offset: 0x00057081
		[DataSourceProperty]
		public bool HasValidValue
		{
			get
			{
				return this._hasValidValue;
			}
			set
			{
				if (value != this._hasValidValue)
				{
					this._hasValidValue = value;
					base.OnPropertyChangedWithValue(value, "HasValidValue");
				}
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x060016D3 RID: 5843 RVA: 0x00058E9F File Offset: 0x0005709F
		// (set) Token: 0x060016D4 RID: 5844 RVA: 0x00058EA7 File Offset: 0x000570A7
		[DataSourceProperty]
		public float TargetValue
		{
			get
			{
				return this._targetValue;
			}
			set
			{
				if (value != this._targetValue)
				{
					this._targetValue = value;
					base.OnPropertyChangedWithValue(value, "TargetValue");
				}
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x060016D5 RID: 5845 RVA: 0x00058EC5 File Offset: 0x000570C5
		// (set) Token: 0x060016D6 RID: 5846 RVA: 0x00058ECD File Offset: 0x000570CD
		[DataSourceProperty]
		public string TargetValueText
		{
			get
			{
				return this._targetValueText;
			}
			set
			{
				if (value != this._targetValueText)
				{
					this._targetValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "TargetValueText");
				}
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x060016D7 RID: 5847 RVA: 0x00058EF0 File Offset: 0x000570F0
		// (set) Token: 0x060016D8 RID: 5848 RVA: 0x00058EF8 File Offset: 0x000570F8
		[DataSourceProperty]
		public bool IsAlternativeUsageProperty
		{
			get
			{
				return this._isAlternativeUsageProperty;
			}
			set
			{
				if (this._isAlternativeUsageProperty != value)
				{
					this._isAlternativeUsageProperty = value;
					base.OnPropertyChangedWithValue(value, "IsAlternativeUsageProperty");
				}
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060016D9 RID: 5849 RVA: 0x00058F16 File Offset: 0x00057116
		// (set) Token: 0x060016DA RID: 5850 RVA: 0x00058F1E File Offset: 0x0005711E
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

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060016DB RID: 5851 RVA: 0x00058F41 File Offset: 0x00057141
		// (set) Token: 0x060016DC RID: 5852 RVA: 0x00058F49 File Offset: 0x00057149
		[DataSourceProperty]
		public float PropertyValue
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
					base.OnPropertyChangedWithValue(value, "PropertyValue");
				}
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x00058F6F File Offset: 0x0005716F
		// (set) Token: 0x060016DE RID: 5854 RVA: 0x00058F77 File Offset: 0x00057177
		[DataSourceProperty]
		public float PropertyMaxValue
		{
			get
			{
				return this._propertyMaxValue;
			}
			set
			{
				if (value != this._propertyMaxValue)
				{
					this._propertyMaxValue = value;
					base.OnPropertyChangedWithValue(value, "PropertyMaxValue");
				}
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x00058F95 File Offset: 0x00057195
		// (set) Token: 0x060016E0 RID: 5856 RVA: 0x00058F9D File Offset: 0x0005719D
		[DataSourceProperty]
		public string PropertyValueText
		{
			get
			{
				return this._propertyValueText;
			}
			set
			{
				if (this._propertyValueText != value)
				{
					this._propertyValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "PropertyValueText");
				}
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x00058FC0 File Offset: 0x000571C0
		// (set) Token: 0x060016E2 RID: 5858 RVA: 0x00058FC8 File Offset: 0x000571C8
		[DataSourceProperty]
		public string SeparatorText
		{
			get
			{
				return this._separatorText;
			}
			set
			{
				if (value != this._separatorText)
				{
					this._separatorText = value;
					base.OnPropertyChangedWithValue<string>(value, "SeparatorText");
				}
			}
		}

		// Token: 0x04000A5C RID: 2652
		public readonly TextObject Description;

		// Token: 0x04000A5D RID: 2653
		public readonly CraftingTemplate.CraftingStatTypes Type;

		// Token: 0x04000A5E RID: 2654
		private bool _showStats;

		// Token: 0x04000A5F RID: 2655
		private bool _isExceedingBeneficial;

		// Token: 0x04000A60 RID: 2656
		private bool _hasValidTarget;

		// Token: 0x04000A61 RID: 2657
		private bool _hasValidValue;

		// Token: 0x04000A62 RID: 2658
		private float _targetValue;

		// Token: 0x04000A63 RID: 2659
		private string _targetValueText;

		// Token: 0x04000A64 RID: 2660
		private string _propertyLbl;

		// Token: 0x04000A65 RID: 2661
		private float _propertyValue;

		// Token: 0x04000A66 RID: 2662
		private float _propertyMaxValue = -1f;

		// Token: 0x04000A67 RID: 2663
		private string _propertyValueText;

		// Token: 0x04000A68 RID: 2664
		public bool _isAlternativeUsageProperty;

		// Token: 0x04000A69 RID: 2665
		private string _separatorText;
	}
}
