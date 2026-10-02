using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions
{
	// Token: 0x0200004A RID: 74
	public class InputHostGameOptionDataVM : GenericHostGameOptionDataVM
	{
		// Token: 0x0600069A RID: 1690 RVA: 0x000155FD File Offset: 0x000137FD
		public InputHostGameOptionDataVM(MultiplayerOptions.OptionType optionType, int preferredIndex)
			: base(OptionsVM.OptionsDataType.InputOption, optionType, preferredIndex)
		{
			this.RefreshData();
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00015610 File Offset: 0x00013810
		public override void RefreshData()
		{
			string strValue = base.OptionType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this.Text = strValue;
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x00015631 File Offset: 0x00013831
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x00015639 File Offset: 0x00013839
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
					base.OptionType.SetValue(value, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
			}
		}

		// Token: 0x0400031E RID: 798
		private string _text;
	}
}
