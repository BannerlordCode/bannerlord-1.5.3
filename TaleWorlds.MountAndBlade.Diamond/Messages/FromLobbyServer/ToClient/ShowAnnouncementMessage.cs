using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000068 RID: 104
	[Serializable]
	public class ShowAnnouncementMessage : Message
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000219 RID: 537 RVA: 0x000037B8 File Offset: 0x000019B8
		// (set) Token: 0x0600021A RID: 538 RVA: 0x000037C0 File Offset: 0x000019C0
		[JsonProperty]
		public Announcement Announcement { get; private set; }

		// Token: 0x0600021B RID: 539 RVA: 0x000037C9 File Offset: 0x000019C9
		public ShowAnnouncementMessage()
		{
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000037D1 File Offset: 0x000019D1
		public ShowAnnouncementMessage(Announcement announcement)
		{
			this.Announcement = announcement;
		}
	}
}
