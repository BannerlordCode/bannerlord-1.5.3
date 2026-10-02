using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AE RID: 174
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectFrame : GameNetworkMessage
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0000C252 File Offset: 0x0000A452
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x0000C25A File Offset: 0x0000A45A
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0000C263 File Offset: 0x0000A463
		// (set) Token: 0x060006FC RID: 1788 RVA: 0x0000C26B File Offset: 0x0000A46B
		public MatrixFrame Frame { get; private set; }

		// Token: 0x060006FD RID: 1789 RVA: 0x0000C274 File Offset: 0x0000A474
		public SetMissionObjectFrame(MissionObjectId missionObjectId, ref MatrixFrame frame)
		{
			this.MissionObjectId = missionObjectId;
			this.Frame = frame;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0000C28F File Offset: 0x0000A48F
		public SetMissionObjectFrame()
		{
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0000C298 File Offset: 0x0000A498
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Frame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0000C2C2 File Offset: 0x0000A4C2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteMatrixFrameToPacket(this.Frame);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0000C2DA File Offset: 0x0000A4DA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0000C2E2 File Offset: 0x0000A4E2
		protected override string OnGetLogFormat()
		{
			return "Set Frame on MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
