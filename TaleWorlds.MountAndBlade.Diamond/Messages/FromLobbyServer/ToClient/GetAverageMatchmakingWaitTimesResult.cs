using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	public class GetAverageMatchmakingWaitTimesResult : FunctionResult
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00002D1E File Offset: 0x00000F1E
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00002D26 File Offset: 0x00000F26
		[JsonProperty]
		public MatchmakingWaitTimeStats MatchmakingWaitTimeStats { get; private set; }

		// Token: 0x0600011D RID: 285 RVA: 0x00002D2F File Offset: 0x00000F2F
		public GetAverageMatchmakingWaitTimesResult()
		{
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00002D37 File Offset: 0x00000F37
		public GetAverageMatchmakingWaitTimesResult(MatchmakingWaitTimeStats matchmakingWaitTimeStats)
		{
			this.MatchmakingWaitTimeStats = matchmakingWaitTimeStats;
		}
	}
}
