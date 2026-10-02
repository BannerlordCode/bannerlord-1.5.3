using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004A RID: 74
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class InvitedPlayerOnlineMessage : Message
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00003159 File Offset: 0x00001359
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00003161 File Offset: 0x00001361
		[JsonProperty]
		public PlayerId PlayerId { get; set; }

		// Token: 0x06000185 RID: 389 RVA: 0x0000316A File Offset: 0x0000136A
		public InvitedPlayerOnlineMessage()
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00003172 File Offset: 0x00001372
		public InvitedPlayerOnlineMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
