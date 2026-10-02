using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B0 RID: 176
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class InviteToPartyMessage : Message
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000328 RID: 808 RVA: 0x000042BB File Offset: 0x000024BB
		// (set) Token: 0x06000329 RID: 809 RVA: 0x000042C3 File Offset: 0x000024C3
		[JsonProperty]
		public PlayerId InvitedPlayerId { get; private set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600032A RID: 810 RVA: 0x000042CC File Offset: 0x000024CC
		// (set) Token: 0x0600032B RID: 811 RVA: 0x000042D4 File Offset: 0x000024D4
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x0600032C RID: 812 RVA: 0x000042DD File Offset: 0x000024DD
		public InviteToPartyMessage()
		{
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000042E5 File Offset: 0x000024E5
		public InviteToPartyMessage(PlayerId invitedPlayerId, bool dontUseNameForUnknownPlayer)
		{
			this.InvitedPlayerId = invitedPlayerId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
