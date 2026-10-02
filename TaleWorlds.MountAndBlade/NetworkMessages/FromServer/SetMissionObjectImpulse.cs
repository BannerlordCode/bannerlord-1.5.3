using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B2 RID: 178
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectImpulse : GameNetworkMessage
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0000C7FC File Offset: 0x0000A9FC
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x0000C804 File Offset: 0x0000AA04
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0000C80D File Offset: 0x0000AA0D
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x0000C815 File Offset: 0x0000AA15
		public Vec3 Position { get; private set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0000C81E File Offset: 0x0000AA1E
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x0000C826 File Offset: 0x0000AA26
		public Vec3 Impulse { get; private set; }

		// Token: 0x0600072B RID: 1835 RVA: 0x0000C82F File Offset: 0x0000AA2F
		public SetMissionObjectImpulse(MissionObjectId missionObjectId, Vec3 position, Vec3 impulse)
		{
			this.MissionObjectId = missionObjectId;
			this.Position = position;
			this.Impulse = impulse;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0000C84C File Offset: 0x0000AA4C
		public SetMissionObjectImpulse()
		{
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0000C854 File Offset: 0x0000AA54
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.LocalPositionCompressionInfo, ref flag);
			this.Impulse = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.ImpulseCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0000C895 File Offset: 0x0000AA95
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionBasic.LocalPositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Impulse, CompressionBasic.ImpulseCompressionInfo);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0000C8C2 File Offset: 0x0000AAC2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0000C8CA File Offset: 0x0000AACA
		protected override string OnGetLogFormat()
		{
			return "Set impulse on MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
