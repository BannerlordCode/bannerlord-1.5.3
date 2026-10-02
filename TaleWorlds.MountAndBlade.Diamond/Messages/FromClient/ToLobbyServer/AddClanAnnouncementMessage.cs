using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000076 RID: 118
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AddClanAnnouncementMessage : Message
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000250 RID: 592 RVA: 0x000039ED File Offset: 0x00001BED
		// (set) Token: 0x06000251 RID: 593 RVA: 0x000039F5 File Offset: 0x00001BF5
		[JsonProperty]
		public string Announcement { get; private set; }

		// Token: 0x06000252 RID: 594 RVA: 0x000039FE File Offset: 0x00001BFE
		public AddClanAnnouncementMessage()
		{
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00003A06 File Offset: 0x00001C06
		public AddClanAnnouncementMessage(string announcement)
		{
			this.Announcement = announcement;
		}
	}
}
