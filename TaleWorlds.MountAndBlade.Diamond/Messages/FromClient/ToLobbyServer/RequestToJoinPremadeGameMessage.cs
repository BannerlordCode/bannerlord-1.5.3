using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C3 RID: 195
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RequestToJoinPremadeGameMessage : Message
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000474A File Offset: 0x0000294A
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00004752 File Offset: 0x00002952
		[JsonProperty]
		public Guid GameId { get; private set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0000475B File Offset: 0x0000295B
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00004763 File Offset: 0x00002963
		[JsonProperty]
		public string Password { get; private set; }

		// Token: 0x06000397 RID: 919 RVA: 0x0000476C File Offset: 0x0000296C
		public RequestToJoinPremadeGameMessage()
		{
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00004774 File Offset: 0x00002974
		public RequestToJoinPremadeGameMessage(Guid gameId, string password)
		{
			this.GameId = gameId;
			this.Password = password;
		}
	}
}
