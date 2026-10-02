using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BE RID: 190
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RemoveFriendMessage : Message
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000374 RID: 884 RVA: 0x000045F8 File Offset: 0x000027F8
		// (set) Token: 0x06000375 RID: 885 RVA: 0x00004600 File Offset: 0x00002800
		[JsonProperty]
		public PlayerId FriendId { get; private set; }

		// Token: 0x06000376 RID: 886 RVA: 0x00004609 File Offset: 0x00002809
		public RemoveFriendMessage()
		{
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00004611 File Offset: 0x00002811
		public RemoveFriendMessage(PlayerId friendId)
		{
			this.FriendId = friendId;
		}
	}
}
