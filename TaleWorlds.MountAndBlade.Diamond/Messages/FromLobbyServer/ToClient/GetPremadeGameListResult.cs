using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000042 RID: 66
	[Serializable]
	public class GetPremadeGameListResult : FunctionResult
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00002F26 File Offset: 0x00001126
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00002F2E File Offset: 0x0000112E
		[JsonProperty]
		public PremadeGameList GameList { get; private set; }

		// Token: 0x06000151 RID: 337 RVA: 0x00002F37 File Offset: 0x00001137
		public GetPremadeGameListResult()
		{
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00002F3F File Offset: 0x0000113F
		public GetPremadeGameListResult(PremadeGameList gameList)
		{
			this.GameList = gameList;
		}
	}
}
