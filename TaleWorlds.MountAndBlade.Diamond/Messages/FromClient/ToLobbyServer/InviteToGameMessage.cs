using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000AF RID: 175
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class InviteToGameMessage : Message
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00004293 File Offset: 0x00002493
		// (set) Token: 0x06000325 RID: 805 RVA: 0x0000429B File Offset: 0x0000249B
		[JsonProperty]
		public PlayerId InvitedPlayerId { get; private set; }

		// Token: 0x06000326 RID: 806 RVA: 0x000042A4 File Offset: 0x000024A4
		public InviteToGameMessage()
		{
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000042AC File Offset: 0x000024AC
		public InviteToGameMessage(PlayerId invitedPlayerId)
		{
			this.InvitedPlayerId = invitedPlayerId;
		}
	}
}
