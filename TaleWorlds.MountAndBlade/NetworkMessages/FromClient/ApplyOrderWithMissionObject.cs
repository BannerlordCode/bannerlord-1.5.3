using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000026 RID: 38
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithMissionObject : GameNetworkMessage
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00003962 File Offset: 0x00001B62
		// (set) Token: 0x0600012E RID: 302 RVA: 0x0000396A File Offset: 0x00001B6A
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x0600012F RID: 303 RVA: 0x00003973 File Offset: 0x00001B73
		public ApplyOrderWithMissionObject(MissionObjectId missionObjectId)
		{
			this.MissionObjectId = missionObjectId;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00003982 File Offset: 0x00001B82
		public ApplyOrderWithMissionObject()
		{
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000398C File Offset: 0x00001B8C
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000039A9 File Offset: 0x00001BA9
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000039B6 File Offset: 0x00001BB6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000039BE File Offset: 0x00001BBE
		protected override string OnGetLogFormat()
		{
			return "Apply order to MissionObject with ID: " + this.MissionObjectId + " and with name ";
		}
	}
}
