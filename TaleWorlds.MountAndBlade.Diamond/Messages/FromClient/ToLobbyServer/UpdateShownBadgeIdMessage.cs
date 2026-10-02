using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C9 RID: 201
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateShownBadgeIdMessage : Message
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000489A File Offset: 0x00002A9A
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x000048A2 File Offset: 0x00002AA2
		[JsonProperty]
		public string ShownBadgeId { get; private set; }

		// Token: 0x060003B5 RID: 949 RVA: 0x000048AB File Offset: 0x00002AAB
		public UpdateShownBadgeIdMessage()
		{
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000048B3 File Offset: 0x00002AB3
		public UpdateShownBadgeIdMessage(string shownBadgeId)
		{
			this.ShownBadgeId = shownBadgeId;
		}
	}
}
