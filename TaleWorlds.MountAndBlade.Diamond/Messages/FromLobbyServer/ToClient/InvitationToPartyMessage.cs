using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000049 RID: 73
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class InvitationToPartyMessage : Message
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00003119 File Offset: 0x00001319
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00003121 File Offset: 0x00001321
		[JsonProperty]
		public string InviterPlayerName { get; private set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000312A File Offset: 0x0000132A
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00003132 File Offset: 0x00001332
		[JsonProperty]
		public PlayerId InviterPlayerId { get; private set; }

		// Token: 0x06000181 RID: 385 RVA: 0x0000313B File Offset: 0x0000133B
		public InvitationToPartyMessage()
		{
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00003143 File Offset: 0x00001343
		public InvitationToPartyMessage(string inviterPlayerName, PlayerId inviterPlayerId)
		{
			this.InviterPlayerName = inviterPlayerName;
			this.InviterPlayerId = inviterPlayerId;
		}
	}
}
