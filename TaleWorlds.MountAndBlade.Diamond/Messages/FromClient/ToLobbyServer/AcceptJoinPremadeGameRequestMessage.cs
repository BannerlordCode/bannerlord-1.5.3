using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000073 RID: 115
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AcceptJoinPremadeGameRequestMessage : Message
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00003995 File Offset: 0x00001B95
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000399D File Offset: 0x00001B9D
		[JsonProperty]
		public Guid PartyId { get; private set; }

		// Token: 0x06000249 RID: 585 RVA: 0x000039A6 File Offset: 0x00001BA6
		public AcceptJoinPremadeGameRequestMessage()
		{
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000039AE File Offset: 0x00001BAE
		public AcceptJoinPremadeGameRequestMessage(Guid partyId)
		{
			this.PartyId = partyId;
		}
	}
}
