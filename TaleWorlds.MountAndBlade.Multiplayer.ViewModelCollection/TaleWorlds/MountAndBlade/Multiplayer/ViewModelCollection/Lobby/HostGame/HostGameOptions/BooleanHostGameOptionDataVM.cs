using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions
{
	// Token: 0x02000048 RID: 72
	public class BooleanHostGameOptionDataVM : GenericHostGameOptionDataVM
	{
		// Token: 0x06000689 RID: 1673 RVA: 0x0001548A File Offset: 0x0001368A
		public BooleanHostGameOptionDataVM(MultiplayerOptions.OptionType optionType, int preferredIndex)
			: base(OptionsVM.OptionsDataType.BooleanOption, optionType, preferredIndex)
		{
			this.RefreshData();
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001549B File Offset: 0x0001369B
		public override void RefreshData()
		{
			this.IsSelected = base.OptionType.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x000154AF File Offset: 0x000136AF
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x000154B7 File Offset: 0x000136B7
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
					base.OptionType.SetValue(value, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
			}
		}

		// Token: 0x04000317 RID: 791
		private bool _isSelected;
	}
}
