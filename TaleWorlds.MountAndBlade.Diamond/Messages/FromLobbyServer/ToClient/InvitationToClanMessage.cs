using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000048 RID: 72
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class InvitationToClanMessage : Message
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000030A8 File Offset: 0x000012A8
		// (set) Token: 0x06000174 RID: 372 RVA: 0x000030B0 File Offset: 0x000012B0
		[JsonProperty]
		public PlayerId InviterId { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000030B9 File Offset: 0x000012B9
		// (set) Token: 0x06000176 RID: 374 RVA: 0x000030C1 File Offset: 0x000012C1
		[JsonProperty]
		public string ClanName { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000030CA File Offset: 0x000012CA
		// (set) Token: 0x06000178 RID: 376 RVA: 0x000030D2 File Offset: 0x000012D2
		[JsonProperty]
		public string ClanTag { get; private set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000030DB File Offset: 0x000012DB
		// (set) Token: 0x0600017A RID: 378 RVA: 0x000030E3 File Offset: 0x000012E3
		[JsonProperty]
		public int ClanPlayerCount { get; private set; }

		// Token: 0x0600017B RID: 379 RVA: 0x000030EC File Offset: 0x000012EC
		public InvitationToClanMessage()
		{
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000030F4 File Offset: 0x000012F4
		public InvitationToClanMessage(PlayerId inviterId, string clanName, string clanTag, int clanPlayerCount)
		{
			this.InviterId = inviterId;
			this.ClanName = clanName;
			this.ClanTag = clanTag;
			this.ClanPlayerCount = clanPlayerCount;
		}
	}
}
