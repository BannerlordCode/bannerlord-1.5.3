using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection
{
	// Token: 0x0200000B RID: 11
	public class PhotoModeValueOptionVM : ViewModel
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00003EBC File Offset: 0x000020BC
		public PhotoModeValueOptionVM(TextObject valueNameTextObj, float min, float max, float currentValue, Action<float> onChange)
		{
			this.MinValue = min;
			this.MaxValue = max;
			this._valueNameTextObj = valueNameTextObj;
			this._onChange = onChange;
			this._currentValue = currentValue;
			this.CurrentValueText = currentValue.ToString("0.0");
			this.RefreshValues();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003F0C File Offset: 0x0000210C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ValueName = this._valueNameTextObj.ToString();
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003F25 File Offset: 0x00002125
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00003F2D File Offset: 0x0000212D
		[DataSourceProperty]
		public float MinValue
		{
			get
			{
				return this._minValue;
			}
			set
			{
				if (value != this._minValue)
				{
					this._minValue = value;
					base.OnPropertyChangedWithValue(value, "MinValue");
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003F4B File Offset: 0x0000214B
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003F53 File Offset: 0x00002153
		[DataSourceProperty]
		public float MaxValue
		{
			get
			{
				return this._maxValue;
			}
			set
			{
				if (value != this._maxValue)
				{
					this._maxValue = value;
					base.OnPropertyChangedWithValue(value, "MaxValue");
				}
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00003F71 File Offset: 0x00002171
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00003F7C File Offset: 0x0000217C
		[DataSourceProperty]
		public float CurrentValue
		{
			get
			{
				return this._currentValue;
			}
			set
			{
				if (value != this._currentValue)
				{
					this._currentValue = value;
					base.OnPropertyChangedWithValue(value, "CurrentValue");
					Action<float> onChange = this._onChange;
					if (onChange != null)
					{
						onChange(value);
					}
					this.CurrentValueText = value.ToString("0.0");
				}
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00003FC9 File Offset: 0x000021C9
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00003FD1 File Offset: 0x000021D1
		[DataSourceProperty]
		public string CurrentValueText
		{
			get
			{
				return this._currentValueText;
			}
			set
			{
				if (value != this._currentValueText)
				{
					this._currentValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentValueText");
				}
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00003FF4 File Offset: 0x000021F4
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00003FFC File Offset: 0x000021FC
		[DataSourceProperty]
		public string ValueName
		{
			get
			{
				return this._valueName;
			}
			set
			{
				if (value != this._valueName)
				{
					this._valueName = value;
					base.OnPropertyChangedWithValue<string>(value, "ValueName");
				}
			}
		}

		// Token: 0x04000049 RID: 73
		private readonly Action<float> _onChange;

		// Token: 0x0400004A RID: 74
		private readonly TextObject _valueNameTextObj;

		// Token: 0x0400004B RID: 75
		private float _minValue;

		// Token: 0x0400004C RID: 76
		private float _maxValue;

		// Token: 0x0400004D RID: 77
		private float _currentValue;

		// Token: 0x0400004E RID: 78
		private string _currentValueText;

		// Token: 0x0400004F RID: 79
		private string _valueName;
	}
}
