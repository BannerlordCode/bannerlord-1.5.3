using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000091 RID: 145
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class EditClanAnnouncementMessage : Message
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00003E98 File Offset: 0x00002098
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x00003EA0 File Offset: 0x000020A0
		[JsonProperty]
		public int AnnouncementId { get; private set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x00003EA9 File Offset: 0x000020A9
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x00003EB1 File Offset: 0x000020B1
		[JsonProperty]
		public string Text { get; private set; }

		// Token: 0x060002C5 RID: 709 RVA: 0x00003EBA File Offset: 0x000020BA
		public EditClanAnnouncementMessage()
		{
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00003EC2 File Offset: 0x000020C2
		public EditClanAnnouncementMessage(int announcementId, string text)
		{
			this.AnnouncementId = announcementId;
			this.Text = text;
		}
	}
}
