using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AF RID: 175
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectFrameOverTime : GameNetworkMessage
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0000C2F9 File Offset: 0x0000A4F9
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x0000C301 File Offset: 0x0000A501
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0000C30A File Offset: 0x0000A50A
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x0000C312 File Offset: 0x0000A512
		public MatrixFrame Frame { get; private set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0000C31B File Offset: 0x0000A51B
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0000C323 File Offset: 0x0000A523
		public float Duration { get; private set; }

		// Token: 0x06000709 RID: 1801 RVA: 0x0000C32C File Offset: 0x0000A52C
		public SetMissionObjectFrameOverTime(MissionObjectId missionObjectId, ref MatrixFrame frame, float duration)
		{
			this.MissionObjectId = missionObjectId;
			this.Frame = frame;
			this.Duration = duration;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0000C34E File Offset: 0x0000A54E
		public SetMissionObjectFrameOverTime()
		{
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0000C358 File Offset: 0x0000A558
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Frame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref flag);
			this.Duration = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagCapturePointDurationCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0000C394 File Offset: 0x0000A594
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteMatrixFrameToPacket(this.Frame);
			GameNetworkMessage.WriteFloatToPacket(this.Duration, CompressionMission.FlagCapturePointDurationCompressionInfo);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0000C3BC File Offset: 0x0000A5BC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0000C3C4 File Offset: 0x0000A5C4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Move-to-frame on MissionObject with ID: ", this.MissionObjectId, " over a period of ", this.Duration, " seconds." });
		}
	}
}
