using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public class GetClanHomeInfoResult : FunctionResult
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00002D6E File Offset: 0x00000F6E
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00002D76 File Offset: 0x00000F76
		[JsonProperty]
		public ClanHomeInfo ClanHomeInfo { get; private set; }

		// Token: 0x06000125 RID: 293 RVA: 0x00002D7F File Offset: 0x00000F7F
		public GetClanHomeInfoResult()
		{
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00002D87 File Offset: 0x00000F87
		public GetClanHomeInfoResult(ClanHomeInfo clanHomeInfo)
		{
			this.ClanHomeInfo = clanHomeInfo;
		}
	}
}
