using System;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x0200006A RID: 106
	public class BooleanOptionDataVM : GenericOptionDataVM
	{
		// Token: 0x06000835 RID: 2101 RVA: 0x0001C27C File Offset: 0x0001A47C
		public BooleanOptionDataVM(OptionsVM optionsVM, IBooleanOptionData option, TextObject name, TextObject description)
			: base(optionsVM, option, name, description, OptionsVM.OptionsDataType.BooleanOption)
		{
			this._booleanOptionData = option;
			this._initialValue = option.GetValue(false).Equals(1f);
			this.OptionValueAsBoolean = this._initialValue;
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x0001C2C2 File Offset: 0x0001A4C2
		// (set) Token: 0x06000837 RID: 2103 RVA: 0x0001C2CA File Offset: 0x0001A4CA
		[DataSourceProperty]
		public bool OptionValueAsBoolean
		{
			get
			{
				return this._optionValue;
			}
			set
			{
				if (value != this._optionValue)
				{
					this._optionValue = value;
					base.OnPropertyChangedWithValue(value, "OptionValueAsBoolean");
					this.UpdateValue();
				}
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001C2F0 File Offset: 0x0001A4F0
		public override void UpdateValue()
		{
			this.Option.SetValue((float)(this.OptionValueAsBoolean ? 1 : 0));
			this.Option.Commit();
			this._optionsVM.SetConfig(this.Option, (float)(this.OptionValueAsBoolean ? 1 : 0));
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001C33E File Offset: 0x0001A53E
		public override void Cancel()
		{
			this.OptionValueAsBoolean = this._initialValue;
			this.UpdateValue();
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001C352 File Offset: 0x0001A552
		public override void SetValue(float value)
		{
			this.OptionValueAsBoolean = (int)value == 1;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001C35F File Offset: 0x0001A55F
		public override void ResetData()
		{
			this.OptionValueAsBoolean = (int)this.Option.GetDefaultValue() == 1;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001C376 File Offset: 0x0001A576
		public override bool IsChanged()
		{
			return this._initialValue != this.OptionValueAsBoolean;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001C389 File Offset: 0x0001A589
		public override void ApplyValue()
		{
			if (this._initialValue != this.OptionValueAsBoolean)
			{
				this._initialValue = this.OptionValueAsBoolean;
			}
		}

		// Token: 0x040003B9 RID: 953
		private bool _initialValue;

		// Token: 0x040003BA RID: 954
		private readonly IBooleanOptionData _booleanOptionData;

		// Token: 0x040003BB RID: 955
		private bool _optionValue;
	}
}
