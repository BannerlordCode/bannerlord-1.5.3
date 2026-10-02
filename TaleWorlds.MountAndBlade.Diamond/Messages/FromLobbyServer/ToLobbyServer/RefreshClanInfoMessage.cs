using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToLobbyServer
{
	// Token: 0x02000070 RID: 112
	[MessageDescription("LobbyServer", "LobbyServer", true)]
	[Serializable]
	public class RefreshClanInfoMessage : Message
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000395D File Offset: 0x00001B5D
		// (set) Token: 0x06000242 RID: 578 RVA: 0x00003965 File Offset: 0x00001B65
		[JsonProperty]
		public Guid ClanId { get; private set; }

		// Token: 0x06000243 RID: 579 RVA: 0x0000396E File Offset: 0x00001B6E
		public RefreshClanInfoMessage()
		{
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00003976 File Offset: 0x00001B76
		public RefreshClanInfoMessage(Guid clanId)
		{
			this.ClanId = clanId;
		}
	}
}
