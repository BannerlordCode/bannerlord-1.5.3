using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B4 RID: 180
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectVertexAnimationProgress : GameNetworkMessage
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0000CA03 File Offset: 0x0000AC03
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x0000CA0B File Offset: 0x0000AC0B
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0000CA14 File Offset: 0x0000AC14
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x0000CA1C File Offset: 0x0000AC1C
		public float Progress { get; private set; }

		// Token: 0x06000743 RID: 1859 RVA: 0x0000CA25 File Offset: 0x0000AC25
		public SetMissionObjectVertexAnimationProgress(MissionObjectId missionObjectId, float progress)
		{
			this.MissionObjectId = missionObjectId;
			this.Progress = progress;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0000CA3B File Offset: 0x0000AC3B
		public SetMissionObjectVertexAnimationProgress()
		{
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0000CA44 File Offset: 0x0000AC44
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Progress = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationProgressCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0000CA73 File Offset: 0x0000AC73
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteFloatToPacket(this.Progress, CompressionBasic.AnimationProgressCompressionInfo);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0000CA90 File Offset: 0x0000AC90
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0000CA98 File Offset: 0x0000AC98
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set progress of Vertex Animation on MissionObject with ID: ", this.MissionObjectId, " to: ", this.Progress });
		}
	}
}
