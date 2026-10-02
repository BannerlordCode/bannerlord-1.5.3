using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200008E RID: 142
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class DeclinePartyJoinRequestMessage : Message
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x00003E48 File Offset: 0x00002048
		// (set) Token: 0x060002BA RID: 698 RVA: 0x00003E50 File Offset: 0x00002050
		[JsonProperty]
		public PlayerId RequesterPlayerId { get; private set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00003E59 File Offset: 0x00002059
		// (set) Token: 0x060002BC RID: 700 RVA: 0x00003E61 File Offset: 0x00002061
		[JsonProperty]
		public PartyJoinDeclineReason Reason { get; private set; }

		// Token: 0x060002BD RID: 701 RVA: 0x00003E6A File Offset: 0x0000206A
		public DeclinePartyJoinRequestMessage()
		{
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00003E72 File Offset: 0x00002072
		public DeclinePartyJoinRequestMessage(PlayerId requesterPlayerId, PartyJoinDeclineReason reason)
		{
			this.RequesterPlayerId = requesterPlayerId;
			this.Reason = reason;
		}
	}
}
