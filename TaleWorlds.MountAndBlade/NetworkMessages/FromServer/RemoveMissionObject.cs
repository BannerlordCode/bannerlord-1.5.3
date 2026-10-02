using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009A RID: 154
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemoveMissionObject : GameNetworkMessage
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0000AEB8 File Offset: 0x000090B8
		// (set) Token: 0x06000617 RID: 1559 RVA: 0x0000AEC0 File Offset: 0x000090C0
		public MissionObjectId ObjectId { get; private set; }

		// Token: 0x06000618 RID: 1560 RVA: 0x0000AEC9 File Offset: 0x000090C9
		public RemoveMissionObject(MissionObjectId objectId)
		{
			this.ObjectId = objectId;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0000AED8 File Offset: 0x000090D8
		public RemoveMissionObject()
		{
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0000AEE0 File Offset: 0x000090E0
		protected override bool OnRead()
		{
			bool flag = true;
			this.ObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0000AEFD File Offset: 0x000090FD
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.ObjectId);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0000AF0A File Offset: 0x0000910A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0000AF12 File Offset: 0x00009112
		protected override string OnGetLogFormat()
		{
			return "Remove MissionObject with ID: " + this.ObjectId;
		}
	}
}
