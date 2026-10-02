using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public class GetPlayerCountInQueueResult : FunctionResult
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00002EAE File Offset: 0x000010AE
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00002EB6 File Offset: 0x000010B6
		[JsonProperty]
		public MatchmakingQueueStats MatchmakingQueueStats { get; private set; }

		// Token: 0x06000145 RID: 325 RVA: 0x00002EBF File Offset: 0x000010BF
		public GetPlayerCountInQueueResult()
		{
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00002EC7 File Offset: 0x000010C7
		public GetPlayerCountInQueueResult(MatchmakingQueueStats matchmakingQueueStats)
		{
			this.MatchmakingQueueStats = matchmakingQueueStats;
		}
	}
}
