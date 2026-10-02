using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B0 RID: 176
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectGlobalFrame : GameNetworkMessage
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0000C410 File Offset: 0x0000A610
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0000C418 File Offset: 0x0000A618
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0000C421 File Offset: 0x0000A621
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0000C429 File Offset: 0x0000A629
		public MatrixFrame Frame { get; private set; }

		// Token: 0x06000713 RID: 1811 RVA: 0x0000C432 File Offset: 0x0000A632
		public SetMissionObjectGlobalFrame(MissionObjectId missionObjectId, ref MatrixFrame frame)
		{
			this.MissionObjectId = missionObjectId;
			this.Frame = frame;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0000C44D File Offset: 0x0000A64D
		public SetMissionObjectGlobalFrame()
		{
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0000C458 File Offset: 0x0000A658
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			Vec3 vec2 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			Vec3 vec3 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			Vec3 vec4 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.ScaleCompressionInfo, ref flag);
			Vec3 vec5 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			if (flag)
			{
				Mat3 mat = new Mat3(in vec, in vec2, in vec3);
				this.Frame = new MatrixFrame(in mat, in vec5);
				this.Frame.Scale(in vec4);
			}
			return flag;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0000C4E8 File Offset: 0x0000A6E8
		protected override void OnWrite()
		{
			Vec3 scaleVector = this.Frame.rotation.GetScaleVector();
			MatrixFrame frame = this.Frame;
			Vec3 vec = new Vec3(1f / scaleVector.x, 1f / scaleVector.y, 1f / scaleVector.z, -1f);
			frame.Scale(in vec);
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteVec3ToPacket(frame.rotation.f, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(frame.rotation.s, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(frame.rotation.u, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(scaleVector, CompressionBasic.ScaleCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(frame.origin, CompressionBasic.PositionCompressionInfo);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0000C5AD File Offset: 0x0000A7AD
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0000C5B5 File Offset: 0x0000A7B5
		protected override string OnGetLogFormat()
		{
			return "Set Global Frame on MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
