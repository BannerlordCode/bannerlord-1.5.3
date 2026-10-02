using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame
{
	// Token: 0x02000047 RID: 71
	public class MPHostGameVM : ViewModel
	{
		// Token: 0x06000682 RID: 1666 RVA: 0x000153B8 File Offset: 0x000135B8
		public MPHostGameVM(LobbyState lobbyState, MPCustomGameVM.CustomGameMode customGameMode)
		{
			this._lobbyState = lobbyState;
			this._customGameMode = customGameMode;
			this.HostGameOptions = new MPHostGameOptionsVM(false, this._customGameMode);
			this.RefreshValues();
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000153E6 File Offset: 0x000135E6
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CreateText = new TextObject("{=aRzlp5XH}CREATE", null).ToString();
			this.HostGameOptions.RefreshValues();
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001540F File Offset: 0x0001360F
		public void ExecuteStart()
		{
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
			{
				this._lobbyState.HostGame();
				return;
			}
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				this._lobbyState.CreatePremadeGame();
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x00015439 File Offset: 0x00013639
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x00015441 File Offset: 0x00013641
		[DataSourceProperty]
		public MPHostGameOptionsVM HostGameOptions
		{
			get
			{
				return this._hostGameOptions;
			}
			set
			{
				if (value != this._hostGameOptions)
				{
					this._hostGameOptions = value;
					base.OnPropertyChangedWithValue<MPHostGameOptionsVM>(value, "HostGameOptions");
				}
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001545F File Offset: 0x0001365F
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x00015467 File Offset: 0x00013667
		[DataSourceProperty]
		public string CreateText
		{
			get
			{
				return this._createText;
			}
			set
			{
				if (value != this._createText)
				{
					this._createText = value;
					base.OnPropertyChangedWithValue<string>(value, "CreateText");
				}
			}
		}

		// Token: 0x04000313 RID: 787
		private LobbyState _lobbyState;

		// Token: 0x04000314 RID: 788
		private MPCustomGameVM.CustomGameMode _customGameMode;

		// Token: 0x04000315 RID: 789
		private MPHostGameOptionsVM _hostGameOptions;

		// Token: 0x04000316 RID: 790
		private string _createText;
	}
}
