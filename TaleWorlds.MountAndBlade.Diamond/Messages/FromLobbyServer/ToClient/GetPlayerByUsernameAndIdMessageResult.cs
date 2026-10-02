using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public class GetPlayerByUsernameAndIdMessageResult : FunctionResult
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00002E5E File Offset: 0x0000105E
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00002E66 File Offset: 0x00001066
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x0600013D RID: 317 RVA: 0x00002E6F File Offset: 0x0000106F
		public GetPlayerByUsernameAndIdMessageResult()
		{
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00002E77 File Offset: 0x00001077
		public GetPlayerByUsernameAndIdMessageResult(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
