using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B2 RID: 178
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class KickPlayerFromPartyMessage : Message
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00004323 File Offset: 0x00002523
		// (set) Token: 0x06000333 RID: 819 RVA: 0x0000432B File Offset: 0x0000252B
		[JsonProperty]
		public PlayerId KickedPlayerId { get; private set; }

		// Token: 0x06000334 RID: 820 RVA: 0x00004334 File Offset: 0x00002534
		public KickPlayerFromPartyMessage()
		{
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000433C File Offset: 0x0000253C
		public KickPlayerFromPartyMessage(PlayerId kickedPlayerId)
		{
			this.KickedPlayerId = kickedPlayerId;
		}
	}
}
