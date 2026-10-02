using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public class ClientQuitFromCustomGameMessage : Message
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00002A80 File Offset: 0x00000C80
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00002A88 File Offset: 0x00000C88
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060000DC RID: 220 RVA: 0x00002A91 File Offset: 0x00000C91
		public ClientQuitFromCustomGameMessage()
		{
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00002A99 File Offset: 0x00000C99
		public ClientQuitFromCustomGameMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
