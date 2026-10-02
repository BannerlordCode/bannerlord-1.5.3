using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000078 RID: 120
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AddFriendMessage : Message
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00003A6D File Offset: 0x00001C6D
		// (set) Token: 0x0600025D RID: 605 RVA: 0x00003A75 File Offset: 0x00001C75
		[JsonProperty]
		public PlayerId FriendId { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00003A7E File Offset: 0x00001C7E
		// (set) Token: 0x0600025F RID: 607 RVA: 0x00003A86 File Offset: 0x00001C86
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x06000260 RID: 608 RVA: 0x00003A8F File Offset: 0x00001C8F
		public AddFriendMessage()
		{
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00003A97 File Offset: 0x00001C97
		public AddFriendMessage(PlayerId friendId, bool dontUseNameForUnknownPlayer)
		{
			this.FriendId = friendId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
