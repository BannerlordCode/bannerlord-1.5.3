using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000061 RID: 97
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class RecentPlayerStatusesMessage : Message
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000365F File Offset: 0x0000185F
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x00003667 File Offset: 0x00001867
		[JsonProperty]
		public FriendInfo[] Friends { get; private set; }

		// Token: 0x060001FA RID: 506 RVA: 0x00003670 File Offset: 0x00001870
		public RecentPlayerStatusesMessage()
		{
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00003678 File Offset: 0x00001878
		public RecentPlayerStatusesMessage(FriendInfo[] friends)
		{
			this.Friends = friends;
		}
	}
}
