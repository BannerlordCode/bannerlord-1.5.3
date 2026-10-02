using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B4 RID: 180
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PlatformPlayerJoinedToPlayerSessionMessage : Message
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00004373 File Offset: 0x00002573
		// (set) Token: 0x0600033B RID: 827 RVA: 0x0000437B File Offset: 0x0000257B
		[JsonProperty]
		public PlayerId InviterPlayerId { get; private set; }

		// Token: 0x0600033C RID: 828 RVA: 0x00004384 File Offset: 0x00002584
		public PlatformPlayerJoinedToPlayerSessionMessage()
		{
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000438C File Offset: 0x0000258C
		public PlatformPlayerJoinedToPlayerSessionMessage(PlayerId inviterPlayerId)
		{
			this.InviterPlayerId = inviterPlayerId;
		}
	}
}
