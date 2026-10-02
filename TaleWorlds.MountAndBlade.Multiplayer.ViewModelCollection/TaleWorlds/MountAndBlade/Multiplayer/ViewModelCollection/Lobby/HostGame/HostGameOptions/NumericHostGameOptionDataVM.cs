using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions
{
	// Token: 0x0200004C RID: 76
	public class NumericHostGameOptionDataVM : GenericHostGameOptionDataVM
	{
		// Token: 0x060006A4 RID: 1700 RVA: 0x00015922 File Offset: 0x00013B22
		public NumericHostGameOptionDataVM(MultiplayerOptions.OptionType optionType, int preferredIndex)
			: base(OptionsVM.OptionsDataType.NumericOption, optionType, preferredIndex)
		{
			this.RefreshData();
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00015934 File Offset: 0x00013B34
		public override void RefreshData()
		{
			MultiplayerOptionsProperty optionProperty = base.OptionType.GetOptionProperty();
			this.Min = optionProperty.BoundsMin;
			this.Max = optionProperty.BoundsMax;
			this.Value = base.OptionType.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00015977 File Offset: 0x00013B77
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x0001597F File Offset: 0x00013B7F
		[DataSourceProperty]
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue(value, "Value");
					base.OnPropertyChanged("ValueAsString");
					base.OptionType.SetValue(value, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x000159B5 File Offset: 0x00013BB5
		[DataSourceProperty]
		public string ValueAsString
		{
			get
			{
				return this._value.ToString();
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x000159C2 File Offset: 0x00013BC2
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x000159CA File Offset: 0x00013BCA
		[DataSourceProperty]
		public int Min
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

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x000159E8 File Offset: 0x00013BE8
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x000159F0 File Offset: 0x00013BF0
		[DataSourceProperty]
		public int Max
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

		// Token: 0x04000321 RID: 801
		private int _value;

		// Token: 0x04000322 RID: 802
		private int _min;

		// Token: 0x04000323 RID: 803
		private int _max;
	}
}
