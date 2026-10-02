using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000044 RID: 68
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class GetRankedLeaderboardCountMessageResult : FunctionResult
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00002F76 File Offset: 0x00001176
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00002F7E File Offset: 0x0000117E
		[JsonProperty]
		public int Count { get; private set; }

		// Token: 0x06000159 RID: 345 RVA: 0x00002F87 File Offset: 0x00001187
		public GetRankedLeaderboardCountMessageResult()
		{
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00002F8F File Offset: 0x0000118F
		public GetRankedLeaderboardCountMessageResult(int count)
		{
			this.Count = count;
		}
	}
}
