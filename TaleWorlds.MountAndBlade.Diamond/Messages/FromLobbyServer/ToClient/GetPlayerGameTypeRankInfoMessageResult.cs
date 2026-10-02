using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Ranked;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public class GetPlayerGameTypeRankInfoMessageResult : FunctionResult
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00002ED6 File Offset: 0x000010D6
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00002EDE File Offset: 0x000010DE
		[JsonProperty]
		public GameTypeRankInfo[] GameTypeRankInfo { get; private set; }

		// Token: 0x06000149 RID: 329 RVA: 0x00002EE7 File Offset: 0x000010E7
		public GetPlayerGameTypeRankInfoMessageResult()
		{
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00002EEF File Offset: 0x000010EF
		public GetPlayerGameTypeRankInfoMessageResult(GameTypeRankInfo[] gameTypeRankInfo)
		{
			this.GameTypeRankInfo = gameTypeRankInfo;
		}
	}
}
