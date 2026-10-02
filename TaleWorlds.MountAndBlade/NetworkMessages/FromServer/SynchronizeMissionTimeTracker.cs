using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006A RID: 106
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SynchronizeMissionTimeTracker : GameNetworkMessage
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003AD RID: 941 RVA: 0x000070C0 File Offset: 0x000052C0
		// (set) Token: 0x060003AE RID: 942 RVA: 0x000070C8 File Offset: 0x000052C8
		public float CurrentTime { get; private set; }

		// Token: 0x060003AF RID: 943 RVA: 0x000070D1 File Offset: 0x000052D1
		public SynchronizeMissionTimeTracker(float currentTime)
		{
			this.CurrentTime = currentTime;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x000070E0 File Offset: 0x000052E0
		public SynchronizeMissionTimeTracker()
		{
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000070E8 File Offset: 0x000052E8
		protected override bool OnRead()
		{
			bool flag = true;
			this.CurrentTime = GameNetworkMessage.ReadFloatFromPacket(CompressionMatchmaker.MissionTimeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000710A File Offset: 0x0000530A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteFloatToPacket(this.CurrentTime, CompressionMatchmaker.MissionTimeCompressionInfo);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000711C File Offset: 0x0000531C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00007124 File Offset: 0x00005324
		protected override string OnGetLogFormat()
		{
			return this.CurrentTime + " seconds have elapsed since the start of the mission.";
		}
	}
}
