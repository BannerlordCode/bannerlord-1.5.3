using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000176 RID: 374
	public class MatchHistoryData : MultiplayerLocalData
	{
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x00010F1F File Offset: 0x0000F11F
		// (set) Token: 0x06000A7E RID: 2686 RVA: 0x00010F27 File Offset: 0x0000F127
		public string MatchId { get; set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x00010F30 File Offset: 0x0000F130
		// (set) Token: 0x06000A80 RID: 2688 RVA: 0x00010F38 File Offset: 0x0000F138
		public string MatchType { get; set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x00010F41 File Offset: 0x0000F141
		// (set) Token: 0x06000A82 RID: 2690 RVA: 0x00010F49 File Offset: 0x0000F149
		public string GameType { get; set; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x00010F52 File Offset: 0x0000F152
		// (set) Token: 0x06000A84 RID: 2692 RVA: 0x00010F5A File Offset: 0x0000F15A
		public string Map { get; set; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x00010F63 File Offset: 0x0000F163
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x00010F6B File Offset: 0x0000F16B
		public DateTime MatchDate { get; set; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00010F74 File Offset: 0x0000F174
		// (set) Token: 0x06000A88 RID: 2696 RVA: 0x00010F7C File Offset: 0x0000F17C
		public int WinnerTeam { get; set; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x00010F85 File Offset: 0x0000F185
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x00010F8D File Offset: 0x0000F18D
		public string Faction1 { get; set; }

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x00010F96 File Offset: 0x0000F196
		// (set) Token: 0x06000A8C RID: 2700 RVA: 0x00010F9E File Offset: 0x0000F19E
		public string Faction2 { get; set; }

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x00010FA7 File Offset: 0x0000F1A7
		// (set) Token: 0x06000A8E RID: 2702 RVA: 0x00010FAF File Offset: 0x0000F1AF
		public int DefenderScore { get; set; }

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x00010FB8 File Offset: 0x0000F1B8
		// (set) Token: 0x06000A90 RID: 2704 RVA: 0x00010FC0 File Offset: 0x0000F1C0
		public int AttackerScore { get; set; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x00010FC9 File Offset: 0x0000F1C9
		// (set) Token: 0x06000A92 RID: 2706 RVA: 0x00010FD1 File Offset: 0x0000F1D1
		public List<PlayerInfo> Players { get; set; }

		// Token: 0x06000A93 RID: 2707 RVA: 0x00010FDA File Offset: 0x0000F1DA
		public MatchHistoryData()
		{
			this.Players = new List<PlayerInfo>();
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00010FF0 File Offset: 0x0000F1F0
		public override bool HasSameContentWith(MultiplayerLocalData other)
		{
			MatchHistoryData matchHistoryData;
			if ((matchHistoryData = other as MatchHistoryData) == null)
			{
				return false;
			}
			bool flag = this.MatchId == matchHistoryData.MatchId && this.MatchType == matchHistoryData.MatchType && this.GameType == matchHistoryData.GameType && this.Map == matchHistoryData.Map && this.MatchDate == matchHistoryData.MatchDate && this.WinnerTeam == matchHistoryData.WinnerTeam && this.Faction1 == matchHistoryData.Faction1 && this.Faction2 == matchHistoryData.Faction2 && this.DefenderScore == matchHistoryData.DefenderScore && this.AttackerScore == matchHistoryData.AttackerScore;
			if (!flag)
			{
				return false;
			}
			if (this.Players != null || matchHistoryData.Players != null)
			{
				List<PlayerInfo> players = this.Players;
				int? num = ((players != null) ? new int?(players.Count) : null);
				List<PlayerInfo> players2 = matchHistoryData.Players;
				int? num2 = ((players2 != null) ? new int?(players2.Count) : null);
				if (!((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null))))
				{
					return flag;
				}
			}
			for (int i = 0; i < this.Players.Count; i++)
			{
				PlayerInfo playerInfo = this.Players[i];
				PlayerInfo playerInfo2 = matchHistoryData.Players[i];
				if (!playerInfo.HasSameContentWith(playerInfo2))
				{
					return false;
				}
			}
			return flag;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00011184 File Offset: 0x0000F384
		private PlayerInfo TryGetPlayer(string id)
		{
			foreach (PlayerInfo playerInfo in this.Players)
			{
				if (playerInfo.PlayerId == id)
				{
					return playerInfo;
				}
			}
			return null;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x000111E8 File Offset: 0x0000F3E8
		public void AddOrUpdatePlayer(string id, string username, int forcedIndex, int teamNo)
		{
			PlayerInfo playerInfo = this.TryGetPlayer(id);
			if (playerInfo == null)
			{
				this.Players.Add(new PlayerInfo
				{
					PlayerId = id,
					Username = username,
					ForcedIndex = forcedIndex,
					TeamNo = teamNo
				});
				return;
			}
			playerInfo.TeamNo = teamNo;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00011238 File Offset: 0x0000F438
		public bool TryUpdatePlayerStats(string id, int kill, int death, int assist)
		{
			PlayerInfo playerInfo = this.TryGetPlayer(id);
			if (playerInfo != null)
			{
				playerInfo.Kill = kill;
				playerInfo.Death = death;
				playerInfo.Assist = assist;
				return true;
			}
			return false;
		}
	}
}
