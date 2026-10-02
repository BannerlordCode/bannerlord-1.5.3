using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B6 RID: 182
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PromoteToClanLeaderMessage : Message
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000342 RID: 834 RVA: 0x000043C3 File Offset: 0x000025C3
		// (set) Token: 0x06000343 RID: 835 RVA: 0x000043CB File Offset: 0x000025CB
		[JsonProperty]
		public PlayerId PromotedPlayerId { get; private set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000344 RID: 836 RVA: 0x000043D4 File Offset: 0x000025D4
		// (set) Token: 0x06000345 RID: 837 RVA: 0x000043DC File Offset: 0x000025DC
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x06000346 RID: 838 RVA: 0x000043E5 File Offset: 0x000025E5
		public PromoteToClanLeaderMessage()
		{
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000043ED File Offset: 0x000025ED
		public PromoteToClanLeaderMessage(PlayerId promotedPlayerId, bool dontUseNameForUnknownPlayer)
		{
			this.PromotedPlayerId = promotedPlayerId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
