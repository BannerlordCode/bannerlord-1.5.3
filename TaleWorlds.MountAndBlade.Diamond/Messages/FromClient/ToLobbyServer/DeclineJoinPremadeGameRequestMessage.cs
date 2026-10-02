using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200008C RID: 140
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class DeclineJoinPremadeGameRequestMessage : Message
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00003E18 File Offset: 0x00002018
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x00003E20 File Offset: 0x00002020
		[JsonProperty]
		public Guid PartyId { get; private set; }

		// Token: 0x060002B6 RID: 694 RVA: 0x00003E29 File Offset: 0x00002029
		public DeclineJoinPremadeGameRequestMessage()
		{
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00003E31 File Offset: 0x00002031
		public DeclineJoinPremadeGameRequestMessage(Guid partyId)
		{
			this.PartyId = partyId;
		}
	}
}
