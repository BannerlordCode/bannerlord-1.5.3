using System;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x02000071 RID: 113
	public class NumericOptionDataVM : GenericOptionDataVM
	{
		// Token: 0x060008D1 RID: 2257 RVA: 0x0001D658 File Offset: 0x0001B858
		public NumericOptionDataVM(OptionsVM optionsVM, INumericOptionData option, TextObject name, TextObject description)
			: base(optionsVM, option, name, description, OptionsVM.OptionsDataType.NumericOption)
		{
			this._numericOptionData = option;
			this._initialValue = this._numericOptionData.GetValue(false);
			this.Min = this._numericOptionData.GetMinValue();
			this.Max = this._numericOptionData.GetMaxValue();
			this.IsDiscrete = this._numericOptionData.GetIsDiscrete();
			this.DiscreteIncrementInterval = this._numericOptionData.GetDiscreteIncrementInterval();
			this.UpdateContinuously = this._numericOptionData.GetShouldUpdateContinuously();
			this.OptionValue = this._initialValue;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0001D6EC File Offset: 0x0001B8EC
		private string GetValueAsString()
		{
			string text = (this.IsDiscrete ? ((int)this._optionValue).ToString() : this._optionValue.ToString("F"));
			if (this._numericOptionData.IsNative() || this._numericOptionData.IsAction())
			{
				return text;
			}
			ManagedOptions.ManagedOptionsType managedOptionsType = (ManagedOptions.ManagedOptionsType)this._numericOptionData.GetOptionType();
			if (managedOptionsType != ManagedOptions.ManagedOptionsType.AutoSaveInterval)
			{
				return text;
			}
			if ((int)this.Min < (int)this._optionValue)
			{
				return text;
			}
			return new TextObject("{=1JlzQIXE}Disabled", null).ToString();
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0001D779 File Offset: 0x0001B979
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0001D781 File Offset: 0x0001B981
		[DataSourceProperty]
		public int DiscreteIncrementInterval
		{
			get
			{
				return this._discreteIncrementInterval;
			}
			set
			{
				if (value != this._discreteIncrementInterval)
				{
					this._discreteIncrementInterval = value;
					base.OnPropertyChangedWithValue(value, "DiscreteIncrementInterval");
				}
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0001D79F File Offset: 0x0001B99F
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0001D7A7 File Offset: 0x0001B9A7
		[DataSourceProperty]
		public float Min
		{
			get
			{
				return this._min;
			}
			set
			{
				if (value != this._min)
				{
					this._min = value;
					base.OnPropertyChangedWithValue(value, "Min");
				}
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0001D7C5 File Offset: 0x0001B9C5
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x0001D7CD File Offset: 0x0001B9CD
		[DataSourceProperty]
		public float Max
		{
			get
			{
				return this._max;
			}
			set
			{
				if (value != this._max)
				{
					this._max = value;
					base.OnPropertyChangedWithValue(value, "Max");
				}
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0001D7EB File Offset: 0x0001B9EB
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x0001D7F3 File Offset: 0x0001B9F3
		[DataSourceProperty]
		public float OptionValue
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
					base.OnPropertyChangedWithValue(value, "OptionValue");
					base.OnPropertyChanged("OptionValueAsString");
					this.UpdateValue();
				}
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0001D822 File Offset: 0x0001BA22
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x0001D82A File Offset: 0x0001BA2A
		[DataSourceProperty]
		public bool IsDiscrete
		{
			get
			{
				return this._isDiscrete;
			}
			set
			{
				if (value != this._isDiscrete)
				{
					this._isDiscrete = value;
					base.OnPropertyChangedWithValue(value, "IsDiscrete");
				}
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0001D848 File Offset: 0x0001BA48
		// (set) Token: 0x060008DE RID: 2270 RVA: 0x0001D850 File Offset: 0x0001BA50
		[DataSourceProperty]
		public bool UpdateContinuously
		{
			get
			{
				return this._updateContinuously;
			}
			set
			{
				if (value != this._updateContinuously)
				{
					this._updateContinuously = value;
					base.OnPropertyChangedWithValue(value, "UpdateContinuously");
				}
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x0001D86E File Offset: 0x0001BA6E
		[DataSourceProperty]
		public string OptionValueAsString
		{
			get
			{
				return this.GetValueAsString();
			}
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0001D876 File Offset: 0x0001BA76
		public override void UpdateValue()
		{
			this.Option.SetValue(this.OptionValue);
			this.Option.Commit();
			this._optionsVM.SetConfig(this.Option, this.OptionValue);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0001D8AB File Offset: 0x0001BAAB
		public override void Cancel()
		{
			this.OptionValue = this._initialValue;
			this.UpdateValue();
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001D8BF File Offset: 0x0001BABF
		public override void SetValue(float value)
		{
			this.OptionValue = value;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001D8C8 File Offset: 0x0001BAC8
		public override void ResetData()
		{
			this.OptionValue = this.Option.GetDefaultValue();
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001D8DB File Offset: 0x0001BADB
		public override bool IsChanged()
		{
			return this._initialValue != this.OptionValue;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001D8EE File Offset: 0x0001BAEE
		public override void ApplyValue()
		{
			if (this._initialValue != this.OptionValue)
			{
				this._initialValue = this.OptionValue;
			}
		}

		// Token: 0x040003F8 RID: 1016
		private float _initialValue;

		// Token: 0x040003F9 RID: 1017
		private INumericOptionData _numericOptionData;

		// Token: 0x040003FA RID: 1018
		private int _discreteIncrementInterval;

		// Token: 0x040003FB RID: 1019
		private float _min;

		// Token: 0x040003FC RID: 1020
		private float _max;

		// Token: 0x040003FD RID: 1021
		private float _optionValue;

		// Token: 0x040003FE RID: 1022
		private bool _isDiscrete;

		// Token: 0x040003FF RID: 1023
		private bool _updateContinuously;
	}
}
