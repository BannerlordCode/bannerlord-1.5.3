using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B5 RID: 181
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PromotePlayerToPartyLeaderMessage : Message
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000439B File Offset: 0x0000259B
		// (set) Token: 0x0600033F RID: 831 RVA: 0x000043A3 File Offset: 0x000025A3
		[JsonProperty]
		public PlayerId PromotedPlayerId { get; private set; }

		// Token: 0x06000340 RID: 832 RVA: 0x000043AC File Offset: 0x000025AC
		public PromotePlayerToPartyLeaderMessage()
		{
		}

		// Token: 0x06000341 RID: 833 RVA: 0x000043B4 File Offset: 0x000025B4
		public PromotePlayerToPartyLeaderMessage(PlayerId promotedPlayerId)
		{
			this.PromotedPlayerId = promotedPlayerId;
		}
	}
}
