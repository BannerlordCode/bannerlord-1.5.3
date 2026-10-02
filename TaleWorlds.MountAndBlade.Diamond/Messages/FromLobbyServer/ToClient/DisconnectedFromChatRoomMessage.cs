using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace TaleWorlds.MountAndBlade.Diamond.Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000170 RID: 368
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class DisconnectedFromChatRoomMessage : Message
	{
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0001076D File Offset: 0x0000E96D
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x00010775 File Offset: 0x0000E975
		[JsonProperty]
		public Guid RoomId { get; private set; }

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0001077E File Offset: 0x0000E97E
		// (set) Token: 0x06000A48 RID: 2632 RVA: 0x00010786 File Offset: 0x0000E986
		[JsonProperty]
		public string RoomName { get; private set; }

		// Token: 0x06000A49 RID: 2633 RVA: 0x0001078F File Offset: 0x0000E98F
		public DisconnectedFromChatRoomMessage()
		{
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00010797 File Offset: 0x0000E997
		public DisconnectedFromChatRoomMessage(Guid roomId, string roomName)
		{
			this.RoomId = roomId;
			this.RoomName = roomName;
		}
	}
}
