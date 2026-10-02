using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000045 RID: 69
	[Serializable]
	public class GetRankedLeaderboardMessageResult : FunctionResult
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00002F9E File Offset: 0x0000119E
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00002FA6 File Offset: 0x000011A6
		[JsonProperty]
		public PlayerLeaderboardData[] LeaderboardPlayers { get; private set; }

		// Token: 0x0600015D RID: 349 RVA: 0x00002FAF File Offset: 0x000011AF
		public GetRankedLeaderboardMessageResult()
		{
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00002FB7 File Offset: 0x000011B7
		public GetRankedLeaderboardMessageResult(PlayerLeaderboardData[] leaderboardPlayers)
		{
			this.LeaderboardPlayers = leaderboardPlayers;
		}
	}
}
