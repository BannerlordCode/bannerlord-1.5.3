using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003E RID: 62
	[Serializable]
	public class GetPlayerClanInfoResult : FunctionResult
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00002E86 File Offset: 0x00001086
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00002E8E File Offset: 0x0000108E
		[JsonProperty]
		public ClanInfo ClanInfo { get; private set; }

		// Token: 0x06000141 RID: 321 RVA: 0x00002E97 File Offset: 0x00001097
		public GetPlayerClanInfoResult()
		{
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00002E9F File Offset: 0x0000109F
		public GetPlayerClanInfoResult(ClanInfo clanInfo)
		{
			this.ClanInfo = clanInfo;
		}
	}
}
