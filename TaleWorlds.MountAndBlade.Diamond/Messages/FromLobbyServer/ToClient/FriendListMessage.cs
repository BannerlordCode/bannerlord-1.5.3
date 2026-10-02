using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000031 RID: 49
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class FriendListMessage : Message
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00002C78 File Offset: 0x00000E78
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00002C80 File Offset: 0x00000E80
		[JsonProperty]
		public FriendInfo[] Friends { get; private set; }

		// Token: 0x0600010D RID: 269 RVA: 0x00002C89 File Offset: 0x00000E89
		public FriendListMessage()
		{
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00002C91 File Offset: 0x00000E91
		public FriendListMessage(FriendInfo[] friends)
		{
			this.Friends = friends;
		}
	}
}
