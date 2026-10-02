using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CB RID: 203
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class StopPhysicsAndSetFrameOfMissionObject : GameNetworkMessage
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x0000E22E File Offset: 0x0000C42E
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x0000E236 File Offset: 0x0000C436
		public MissionObjectId ObjectId { get; private set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x0000E23F File Offset: 0x0000C43F
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x0000E247 File Offset: 0x0000C447
		public MissionObjectId ParentId { get; private set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x0000E250 File Offset: 0x0000C450
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x0000E258 File Offset: 0x0000C458
		public MatrixFrame Frame { get; private set; }

		// Token: 0x06000853 RID: 2131 RVA: 0x0000E261 File Offset: 0x0000C461
		public StopPhysicsAndSetFrameOfMissionObject(MissionObjectId objectId, MissionObjectId parentId, MatrixFrame frame)
		{
			this.ObjectId = objectId;
			this.ParentId = parentId;
			this.Frame = frame;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0000E27E File Offset: 0x0000C47E
		public StopPhysicsAndSetFrameOfMissionObject()
		{
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0000E288 File Offset: 0x0000C488
		protected override bool OnRead()
		{
			bool flag = true;
			this.ObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.ParentId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Frame = GameNetworkMessage.ReadNonUniformTransformFromPacket(CompressionBasic.PositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0000E2CC File Offset: 0x0000C4CC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.ObjectId);
			GameNetworkMessage.WriteMissionObjectIdToPacket((this.ParentId.Id >= 0) ? this.ParentId : MissionObjectId.Invalid);
			GameNetworkMessage.WriteNonUniformTransformToPacket(this.Frame, CompressionBasic.PositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0000E319 File Offset: 0x0000C519
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0000E321 File Offset: 0x0000C521
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Stop physics and set frame of MissionObject with ID: ", this.ObjectId, " Parent Index: ", this.ParentId });
		}
	}
}
