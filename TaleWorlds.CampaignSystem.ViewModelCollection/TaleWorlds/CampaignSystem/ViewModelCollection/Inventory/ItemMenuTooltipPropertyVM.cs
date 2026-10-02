using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000096 RID: 150
	public class ItemMenuTooltipPropertyVM : TooltipProperty
	{
		// Token: 0x06000C9C RID: 3228 RVA: 0x0003387A File Offset: 0x00031A7A
		public ItemMenuTooltipPropertyVM()
		{
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00033882 File Offset: 0x00031A82
		public ItemMenuTooltipPropertyVM(string definition, string value, int textHeight, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null, string modifierBonusText = null, bool isModifierBeneficial = false)
			: base(definition, value, textHeight, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
			this.ModifierBonusText = modifierBonusText;
			this.HasModifierBonus = !string.IsNullOrEmpty(modifierBonusText);
			this.IsModifierBeneficial = isModifierBeneficial;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x000338B8 File Offset: 0x00031AB8
		public ItemMenuTooltipPropertyVM(string definition, Func<string> _valueFunc, int textHeight, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null)
			: base(definition, _valueFunc, textHeight, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x000338CE File Offset: 0x00031ACE
		public ItemMenuTooltipPropertyVM(Func<string> _definitionFunc, Func<string> _valueFunc, int textHeight, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null)
			: base(_definitionFunc, _valueFunc, textHeight, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x000338E4 File Offset: 0x00031AE4
		public ItemMenuTooltipPropertyVM(Func<string> _definitionFunc, Func<string> _valueFunc, object[] valueArgs, int textHeight, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null)
			: base(_definitionFunc, _valueFunc, valueArgs, textHeight, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x000338FC File Offset: 0x00031AFC
		public ItemMenuTooltipPropertyVM(string definition, string value, int textHeight, Color color, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None, string modifierBonusText = null, bool isModifierBeneficial = false)
			: base(definition, value, textHeight, color, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
			this.ModifierBonusText = modifierBonusText;
			this.HasModifierBonus = !string.IsNullOrEmpty(modifierBonusText);
			this.IsModifierBeneficial = isModifierBeneficial;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00033934 File Offset: 0x00031B34
		public ItemMenuTooltipPropertyVM(string definition, Func<string> _valueFunc, int textHeight, Color color, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null)
			: base(definition, _valueFunc, textHeight, color, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0003394C File Offset: 0x00031B4C
		public ItemMenuTooltipPropertyVM(Func<string> _definitionFunc, Func<string> _valueFunc, int textHeight, Color color, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null)
			: base(_definitionFunc, _valueFunc, textHeight, color, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00033964 File Offset: 0x00031B64
		public ItemMenuTooltipPropertyVM(TooltipProperty property, HintViewModel propertyHint = null)
			: base(property)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x00033974 File Offset: 0x00031B74
		// (set) Token: 0x06000CA6 RID: 3238 RVA: 0x0003397C File Offset: 0x00031B7C
		[DataSourceProperty]
		public HintViewModel PropertyHint
		{
			get
			{
				return this._propertyHint;
			}
			set
			{
				if (value != this._propertyHint)
				{
					this._propertyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PropertyHint");
				}
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0003399A File Offset: 0x00031B9A
		// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x000339A2 File Offset: 0x00031BA2
		[DataSourceProperty]
		public bool HasModifierBonus
		{
			get
			{
				return this._hasModifierBonus;
			}
			set
			{
				if (value != this._hasModifierBonus)
				{
					this._hasModifierBonus = value;
					base.OnPropertyChangedWithValue(value, "HasModifierBonus");
				}
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x000339C0 File Offset: 0x00031BC0
		// (set) Token: 0x06000CAA RID: 3242 RVA: 0x000339C8 File Offset: 0x00031BC8
		[DataSourceProperty]
		public bool IsModifierBeneficial
		{
			get
			{
				return this._isModifierBeneficial;
			}
			set
			{
				if (value != this._isModifierBeneficial)
				{
					this._isModifierBeneficial = value;
					base.OnPropertyChangedWithValue(value, "IsModifierBeneficial");
				}
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x000339E6 File Offset: 0x00031BE6
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x000339EE File Offset: 0x00031BEE
		[DataSourceProperty]
		public string ModifierBonusText
		{
			get
			{
				return this._modifierBonusText;
			}
			set
			{
				if (value != this._modifierBonusText)
				{
					this._modifierBonusText = value;
					base.OnPropertyChangedWithValue<string>(value, "ModifierBonusText");
				}
			}
		}

		// Token: 0x0400059B RID: 1435
		private HintViewModel _propertyHint;

		// Token: 0x0400059C RID: 1436
		private bool _hasModifierBonus;

		// Token: 0x0400059D RID: 1437
		private bool _isModifierBeneficial;

		// Token: 0x0400059E RID: 1438
		private string _modifierBonusText;
	}
}
