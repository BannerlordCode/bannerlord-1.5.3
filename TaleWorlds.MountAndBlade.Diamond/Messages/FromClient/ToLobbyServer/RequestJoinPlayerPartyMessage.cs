using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C2 RID: 194
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RequestJoinPlayerPartyMessage : Message
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000470A File Offset: 0x0000290A
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00004712 File Offset: 0x00002912
		[JsonProperty]
		public PlayerId TargetPlayer { get; private set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000471B File Offset: 0x0000291B
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00004723 File Offset: 0x00002923
		[JsonProperty]
		public bool InviteRequest { get; private set; }

		// Token: 0x06000391 RID: 913 RVA: 0x0000472C File Offset: 0x0000292C
		public RequestJoinPlayerPartyMessage()
		{
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00004734 File Offset: 0x00002934
		public RequestJoinPlayerPartyMessage(PlayerId targetPlayer, bool inviteRequest)
		{
			this.TargetPlayer = targetPlayer;
			this.InviteRequest = inviteRequest;
		}
	}
}
