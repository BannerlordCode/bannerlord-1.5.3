using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000077 RID: 119
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AddFriendByUsernameAndIdMessage : Message
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00003A15 File Offset: 0x00001C15
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00003A1D File Offset: 0x00001C1D
		[JsonProperty]
		public string Username { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00003A26 File Offset: 0x00001C26
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00003A2E File Offset: 0x00001C2E
		[JsonProperty]
		public int UserId { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00003A37 File Offset: 0x00001C37
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00003A3F File Offset: 0x00001C3F
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x0600025A RID: 602 RVA: 0x00003A48 File Offset: 0x00001C48
		public AddFriendByUsernameAndIdMessage()
		{
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00003A50 File Offset: 0x00001C50
		public AddFriendByUsernameAndIdMessage(string username, int userId, bool dontUseNameForUnknownPlayer)
		{
			this.Username = username;
			this.UserId = userId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
