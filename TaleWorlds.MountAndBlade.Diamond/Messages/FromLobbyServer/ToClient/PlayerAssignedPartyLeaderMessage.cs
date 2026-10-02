using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000057 RID: 87
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PlayerAssignedPartyLeaderMessage : Message
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000340C File Offset: 0x0000160C
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00003414 File Offset: 0x00001614
		[JsonProperty]
		public PlayerId PartyLeaderId { get; private set; }

		// Token: 0x060001C5 RID: 453 RVA: 0x0000341D File Offset: 0x0000161D
		public PlayerAssignedPartyLeaderMessage()
		{
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00003425 File Offset: 0x00001625
		public PlayerAssignedPartyLeaderMessage(PlayerId partyLeaderId)
		{
			this.PartyLeaderId = partyLeaderId;
		}
	}
}
