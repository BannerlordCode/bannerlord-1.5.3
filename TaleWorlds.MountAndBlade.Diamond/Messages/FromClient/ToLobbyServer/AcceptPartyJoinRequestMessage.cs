using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000075 RID: 117
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AcceptPartyJoinRequestMessage : Message
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000039C5 File Offset: 0x00001BC5
		// (set) Token: 0x0600024D RID: 589 RVA: 0x000039CD File Offset: 0x00001BCD
		[JsonProperty]
		public PlayerId RequesterPlayerId { get; private set; }

		// Token: 0x0600024E RID: 590 RVA: 0x000039D6 File Offset: 0x00001BD6
		public AcceptPartyJoinRequestMessage()
		{
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000039DE File Offset: 0x00001BDE
		public AcceptPartyJoinRequestMessage(PlayerId requesterPlayerId)
		{
			this.RequesterPlayerId = requesterPlayerId;
		}
	}
}
