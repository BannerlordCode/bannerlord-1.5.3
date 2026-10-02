using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B2 RID: 178
	public class MultiplayerAdminPanelNumericOptionVM : MultiplayerAdminPanelOptionBaseVM
	{
		// Token: 0x06001102 RID: 4354 RVA: 0x000359B0 File Offset: 0x00033BB0
		public MultiplayerAdminPanelNumericOptionVM(IAdminPanelNumericOption option)
			: base(option)
		{
			this._option = option;
			this._minValue = this._option.GetMinimumValue();
			this._maxValue = this._option.GetMaximumValue();
			this.IntValue = this._option.GetValue();
			this.IsNumericOption = true;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00035A05 File Offset: 0x00033C05
		public override void UpdateValues()
		{
			base.UpdateValues();
			this.IntValue = this._option.GetValue();
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00035A20 File Offset: 0x00033C20
		private int GetClampedInt(int value)
		{
			if (this._minValue != null)
			{
				value = MathF.Max(value, this._minValue.Value);
			}
			if (this._maxValue != null)
			{
				value = MathF.Min(value, this._maxValue.Value);
			}
			return value;
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x00035A6E File Offset: 0x00033C6E
		// (set) Token: 0x06001106 RID: 4358 RVA: 0x00035A76 File Offset: 0x00033C76
		[DataSourceProperty]
		public bool IsNumericOption
		{
			get
			{
				return this._isNumericOption;
			}
			set
			{
				if (value != this._isNumericOption)
				{
					this._isNumericOption = value;
					base.OnPropertyChangedWithValue(value, "IsNumericOption");
				}
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x00035A94 File Offset: 0x00033C94
		// (set) Token: 0x06001108 RID: 4360 RVA: 0x00035A9C File Offset: 0x00033C9C
		[DataSourceProperty]
		public int IntValue
		{
			get
			{
				return this._intValue;
			}
			set
			{
				if (value != this._intValue)
				{
					value = this.GetClampedInt(value);
					this._intValue = value;
					base.OnPropertyChangedWithValue(value, "IntValue");
					this._option.SetValue(value);
				}
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x00035ACF File Offset: 0x00033CCF
		// (set) Token: 0x0600110A RID: 4362 RVA: 0x00035AF0 File Offset: 0x00033CF0
		public int MinValueInt
		{
			get
			{
				if (this._minValue == null)
				{
					return int.MinValue;
				}
				return this._minValue.Value;
			}
			set
			{
				int? minValue = this._minValue;
				if (!((value == minValue.GetValueOrDefault()) & (minValue != null)))
				{
					this._minValue = new int?(value);
					base.OnPropertyChangedWithValue(value, "MinValueInt");
				}
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x00035B30 File Offset: 0x00033D30
		// (set) Token: 0x0600110C RID: 4364 RVA: 0x00035B50 File Offset: 0x00033D50
		public int MaxValueInt
		{
			get
			{
				if (this._maxValue == null)
				{
					return int.MaxValue;
				}
				return this._maxValue.Value;
			}
			set
			{
				int? maxValue = this._maxValue;
				if (!((value == maxValue.GetValueOrDefault()) & (maxValue != null)))
				{
					this._maxValue = new int?(value);
					base.OnPropertyChangedWithValue(value, "MaxValueInt");
				}
			}
		}

		// Token: 0x040007FE RID: 2046
		private int? _minValue;

		// Token: 0x040007FF RID: 2047
		private int? _maxValue;

		// Token: 0x04000800 RID: 2048
		private new readonly IAdminPanelNumericOption _option;

		// Token: 0x04000801 RID: 2049
		private bool _isNumericOption;

		// Token: 0x04000802 RID: 2050
		private int _intValue;
	}
}
