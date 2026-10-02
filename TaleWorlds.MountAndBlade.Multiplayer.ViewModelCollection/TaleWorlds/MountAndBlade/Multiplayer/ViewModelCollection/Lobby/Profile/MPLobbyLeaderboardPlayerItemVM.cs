using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000039 RID: 57
	public class MPLobbyLeaderboardPlayerItemVM : MPLobbyPlayerBaseVM
	{
		// Token: 0x0600054B RID: 1355 RVA: 0x00012354 File Offset: 0x00010554
		public MPLobbyLeaderboardPlayerItemVM(int rank, PlayerLeaderboardData playerLeaderboardData, Action<MPLobbyLeaderboardPlayerItemVM> onActivatePlayerActions)
			: base(playerLeaderboardData.PlayerId, playerLeaderboardData.Name, null, null)
		{
			this.Rank = rank;
			base.Rating = playerLeaderboardData.Rating;
			base.RatingID = playerLeaderboardData.RankId;
			this._onActivatePlayerActions = onActivatePlayerActions;
			this.RefreshValues();
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x000123A1 File Offset: 0x000105A1
		private void ExecuteActivatePlayerActions()
		{
			Action<MPLobbyLeaderboardPlayerItemVM> onActivatePlayerActions = this._onActivatePlayerActions;
			if (onActivatePlayerActions == null)
			{
				return;
			}
			onActivatePlayerActions(this);
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x000123B4 File Offset: 0x000105B4
		// (set) Token: 0x0600054E RID: 1358 RVA: 0x000123BC File Offset: 0x000105BC
		[DataSourceProperty]
		public int Rank
		{
			get
			{
				return this._rank;
			}
			set
			{
				if (value != this._rank)
				{
					this._rank = value;
					base.OnPropertyChangedWithValue(value, "Rank");
				}
			}
		}

		// Token: 0x04000287 RID: 647
		public readonly MatchHistoryData MatchOfThePlayer;

		// Token: 0x04000288 RID: 648
		private readonly Action<MPLobbyLeaderboardPlayerItemVM> _onActivatePlayerActions;

		// Token: 0x04000289 RID: 649
		private int _rank;
	}
}
