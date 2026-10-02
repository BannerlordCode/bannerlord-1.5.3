using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Matchmaking
{
	// Token: 0x020000AD RID: 173
	public class MultiplayerLobbyMatchmakingRegionConnectionQualityTextWidget : TextWidget
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x0001A4A1 File Offset: 0x000186A1
		public MultiplayerLobbyMatchmakingRegionConnectionQualityTextWidget(UIContext context)
			: base(context)
		{
			base.AddState("PoorQuality");
			base.AddState("AverageQuality");
			base.AddState("GoodQuality");
			this.ConnectionQualityLevelUpdated();
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001A4D4 File Offset: 0x000186D4
		private void ConnectionQualityLevelUpdated()
		{
			switch (this.ConnectionQualityLevel)
			{
			case 0:
				this.SetState("PoorQuality");
				return;
			case 1:
				this.SetState("AverageQuality");
				return;
			case 2:
				this.SetState("GoodQuality");
				return;
			default:
				this.SetState("Default");
				return;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x0001A52B File Offset: 0x0001872B
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x0001A533 File Offset: 0x00018733
		[Editor(false)]
		public int ConnectionQualityLevel
		{
			get
			{
				return this._connectionQualityLevel;
			}
			set
			{
				if (this._connectionQualityLevel != value)
				{
					this._connectionQualityLevel = value;
					base.OnPropertyChanged(value, "ConnectionQualityLevel");
					this.ConnectionQualityLevelUpdated();
				}
			}
		}

		// Token: 0x04000427 RID: 1063
		private int _connectionQualityLevel;
	}
}
