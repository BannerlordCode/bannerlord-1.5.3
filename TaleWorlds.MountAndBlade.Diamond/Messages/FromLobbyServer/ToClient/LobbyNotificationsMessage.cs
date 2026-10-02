using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000051 RID: 81
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class LobbyNotificationsMessage : Message
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00003364 File Offset: 0x00001564
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x0000336C File Offset: 0x0000156C
		[JsonProperty]
		public LobbyNotification[] Notifications { get; private set; }

		// Token: 0x060001B4 RID: 436 RVA: 0x00003375 File Offset: 0x00001575
		public LobbyNotificationsMessage()
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000337D File Offset: 0x0000157D
		public LobbyNotificationsMessage(LobbyNotification[] notifications)
		{
			this.Notifications = notifications;
		}
	}
}
