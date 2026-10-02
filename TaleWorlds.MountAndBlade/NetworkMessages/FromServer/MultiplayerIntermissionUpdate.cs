using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005E RID: 94
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionUpdate : GameNetworkMessage
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600034B RID: 843 RVA: 0x000064D0 File Offset: 0x000046D0
		// (set) Token: 0x0600034C RID: 844 RVA: 0x000064D8 File Offset: 0x000046D8
		public MultiplayerIntermissionState IntermissionState { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600034D RID: 845 RVA: 0x000064E1 File Offset: 0x000046E1
		// (set) Token: 0x0600034E RID: 846 RVA: 0x000064E9 File Offset: 0x000046E9
		public float IntermissionTimer { get; private set; }

		// Token: 0x0600034F RID: 847 RVA: 0x000064F2 File Offset: 0x000046F2
		public MultiplayerIntermissionUpdate()
		{
		}

		// Token: 0x06000350 RID: 848 RVA: 0x000064FA File Offset: 0x000046FA
		public MultiplayerIntermissionUpdate(MultiplayerIntermissionState intermissionState, float intermissionTimer)
		{
			this.IntermissionState = intermissionState;
			this.IntermissionTimer = intermissionTimer;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00006510 File Offset: 0x00004710
		protected override bool OnRead()
		{
			bool flag = true;
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionStateCompressionInfo, ref flag);
			this.IntermissionState = (MultiplayerIntermissionState)num;
			this.IntermissionTimer = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.IntermissionTimerCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00006546 File Offset: 0x00004746
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.IntermissionState, CompressionBasic.IntermissionStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.IntermissionTimer, CompressionBasic.IntermissionTimerCompressionInfo);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00006568 File Offset: 0x00004768
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00006570 File Offset: 0x00004770
		protected override string OnGetLogFormat()
		{
			return "Receiving runtime intermission state.";
		}
	}
}
