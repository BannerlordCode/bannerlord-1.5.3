using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B1 RID: 177
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectGlobalFrameOverTime : GameNetworkMessage
	{
		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0000C5CC File Offset: 0x0000A7CC
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x0000C5D4 File Offset: 0x0000A7D4
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x0000C5DD File Offset: 0x0000A7DD
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x0000C5E5 File Offset: 0x0000A7E5
		public MatrixFrame Frame { get; private set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x0000C5EE File Offset: 0x0000A7EE
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x0000C5F6 File Offset: 0x0000A7F6
		public float Duration { get; private set; }

		// Token: 0x0600071F RID: 1823 RVA: 0x0000C5FF File Offset: 0x0000A7FF
		public SetMissionObjectGlobalFrameOverTime(MissionObjectId missionObjectId, ref MatrixFrame frame, float duration)
		{
			this.MissionObjectId = missionObjectId;
			this.Frame = frame;
			this.Duration = duration;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0000C621 File Offset: 0x0000A821
		public SetMissionObjectGlobalFrameOverTime()
		{
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0000C62C File Offset: 0x0000A82C
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
			this.Duration = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagCapturePointDurationCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0000C6D0 File Offset: 0x0000A8D0
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
			GameNetworkMessage.WriteFloatToPacket(this.Duration, CompressionMission.FlagCapturePointDurationCompressionInfo);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0000C7A5 File Offset: 0x0000A9A5
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0000C7B0 File Offset: 0x0000A9B0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Move-to-global-frame on MissionObject with ID: ", this.MissionObjectId, " over a period of ", this.Duration, " seconds." });
		}
	}
}
