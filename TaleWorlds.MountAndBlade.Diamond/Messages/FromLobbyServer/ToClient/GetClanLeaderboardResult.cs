using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public class GetClanLeaderboardResult : FunctionResult
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00002D96 File Offset: 0x00000F96
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00002D9E File Offset: 0x00000F9E
		[JsonProperty]
		public ClanLeaderboardInfo ClanLeaderboardInfo { get; private set; }

		// Token: 0x06000129 RID: 297 RVA: 0x00002DA7 File Offset: 0x00000FA7
		public GetClanLeaderboardResult()
		{
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00002DAF File Offset: 0x00000FAF
		public GetClanLeaderboardResult(ClanLeaderboardInfo info)
		{
			this.ClanLeaderboardInfo = info;
		}
	}
}
