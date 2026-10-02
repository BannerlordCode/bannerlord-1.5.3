using System;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x0200003F RID: 63
	public class MPLobbyRecentGamePlayerItemVM : MPLobbyPlayerBaseVM
	{
		// Token: 0x06000609 RID: 1545 RVA: 0x00013F60 File Offset: 0x00012160
		public MPLobbyRecentGamePlayerItemVM(PlayerId playerId, MatchHistoryData matchOfThePlayer, Action<MPLobbyRecentGamePlayerItemVM> onActivatePlayerActions)
			: base(playerId, "", null, null)
		{
			this.MatchOfThePlayer = matchOfThePlayer;
			this._onActivatePlayerActions = onActivatePlayerActions;
			PlayerInfo playerInfo = this.MatchOfThePlayer.Players.FirstOrDefault<PlayerInfo>((PlayerInfo p) => p.PlayerId == playerId.ToString());
			if (playerInfo != null)
			{
				this.KillCount = playerInfo.Kill;
				this.DeathCount = playerInfo.Death;
				this.AssistCount = playerInfo.Assist;
			}
			this.RefreshValues();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00013FE5 File Offset: 0x000121E5
		private void ExecuteActivatePlayerActions()
		{
			Action<MPLobbyRecentGamePlayerItemVM> onActivatePlayerActions = this._onActivatePlayerActions;
			if (onActivatePlayerActions == null)
			{
				return;
			}
			onActivatePlayerActions(this);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00013FF8 File Offset: 0x000121F8
		public override void RefreshValues()
		{
			base.RefreshValues();
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00014000 File Offset: 0x00012200
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x00014008 File Offset: 0x00012208
		[DataSourceProperty]
		public int KillCount
		{
			get
			{
				return this._killCount;
			}
			set
			{
				if (value != this._killCount)
				{
					this._killCount = value;
					base.OnPropertyChangedWithValue(value, "KillCount");
				}
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x00014026 File Offset: 0x00012226
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x0001402E File Offset: 0x0001222E
		[DataSourceProperty]
		public int DeathCount
		{
			get
			{
				return this._deathCount;
			}
			set
			{
				if (value != this._deathCount)
				{
					this._deathCount = value;
					base.OnPropertyChangedWithValue(value, "DeathCount");
				}
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001404C File Offset: 0x0001224C
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x00014054 File Offset: 0x00012254
		[DataSourceProperty]
		public int AssistCount
		{
			get
			{
				return this._assistCount;
			}
			set
			{
				if (value != this._assistCount)
				{
					this._assistCount = value;
					base.OnPropertyChangedWithValue(value, "AssistCount");
				}
			}
		}

		// Token: 0x040002DB RID: 731
		public readonly MatchHistoryData MatchOfThePlayer;

		// Token: 0x040002DC RID: 732
		private readonly Action<MPLobbyRecentGamePlayerItemVM> _onActivatePlayerActions;

		// Token: 0x040002DD RID: 733
		private int _killCount;

		// Token: 0x040002DE RID: 734
		private int _deathCount;

		// Token: 0x040002DF RID: 735
		private int _assistCount;
	}
}
