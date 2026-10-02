using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C8 RID: 200
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateNotificationsMessage : Message
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00004872 File Offset: 0x00002A72
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x0000487A File Offset: 0x00002A7A
		[JsonProperty]
		public int[] SeenNotificationIds { get; private set; }

		// Token: 0x060003B1 RID: 945 RVA: 0x00004883 File Offset: 0x00002A83
		public UpdateNotificationsMessage()
		{
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000488B File Offset: 0x00002A8B
		public UpdateNotificationsMessage(int[] seenNotificationIds)
		{
			this.SeenNotificationIds = seenNotificationIds;
		}
	}
}
