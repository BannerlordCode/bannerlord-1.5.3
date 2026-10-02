using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000041 RID: 65
	[Serializable]
	public class GetPlayerStatsMessageResult : FunctionResult
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00002EFE File Offset: 0x000010FE
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00002F06 File Offset: 0x00001106
		[JsonProperty]
		public PlayerStatsBase[] PlayerStats { get; private set; }

		// Token: 0x0600014D RID: 333 RVA: 0x00002F0F File Offset: 0x0000110F
		public GetPlayerStatsMessageResult()
		{
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00002F17 File Offset: 0x00001117
		public GetPlayerStatsMessageResult(PlayerStatsBase[] playerStats)
		{
			this.PlayerStats = playerStats;
		}
	}
}
