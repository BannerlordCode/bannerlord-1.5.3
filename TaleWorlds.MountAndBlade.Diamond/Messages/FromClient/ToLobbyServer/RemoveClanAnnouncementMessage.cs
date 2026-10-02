using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BC RID: 188
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RemoveClanAnnouncementMessage : Message
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000045A8 File Offset: 0x000027A8
		// (set) Token: 0x0600036D RID: 877 RVA: 0x000045B0 File Offset: 0x000027B0
		[JsonProperty]
		public int AnnouncementId { get; private set; }

		// Token: 0x0600036E RID: 878 RVA: 0x000045B9 File Offset: 0x000027B9
		public RemoveClanAnnouncementMessage()
		{
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000045C1 File Offset: 0x000027C1
		public RemoveClanAnnouncementMessage(int announcementId)
		{
			this.AnnouncementId = announcementId;
		}
	}
}
