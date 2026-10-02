using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000AE RID: 174
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class InviteToClanMessage : Message
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600031E RID: 798 RVA: 0x00004253 File Offset: 0x00002453
		// (set) Token: 0x0600031F RID: 799 RVA: 0x0000425B File Offset: 0x0000245B
		[JsonProperty]
		public PlayerId InvitedPlayerId { get; private set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000320 RID: 800 RVA: 0x00004264 File Offset: 0x00002464
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0000426C File Offset: 0x0000246C
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x06000322 RID: 802 RVA: 0x00004275 File Offset: 0x00002475
		public InviteToClanMessage()
		{
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000427D File Offset: 0x0000247D
		public InviteToClanMessage(PlayerId invitedPlayerId, bool dontUseNameForUnknownPlayer)
		{
			this.InvitedPlayerId = invitedPlayerId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
