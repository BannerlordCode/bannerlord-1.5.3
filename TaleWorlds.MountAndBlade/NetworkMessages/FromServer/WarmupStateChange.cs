using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D5 RID: 213
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class WarmupStateChange : GameNetworkMessage
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0000EC78 File Offset: 0x0000CE78
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0000EC80 File Offset: 0x0000CE80
		public MultiplayerWarmupComponent.WarmupStates WarmupState { get; private set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0000EC89 File Offset: 0x0000CE89
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x0000EC91 File Offset: 0x0000CE91
		public float StateStartTimeInSeconds { get; private set; }

		// Token: 0x060008C1 RID: 2241 RVA: 0x0000EC9A File Offset: 0x0000CE9A
		public WarmupStateChange(MultiplayerWarmupComponent.WarmupStates warmupState, long stateStartTimeInTicks)
		{
			this.WarmupState = warmupState;
			this.StateStartTimeInSeconds = (float)stateStartTimeInTicks / 10000000f;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0000ECB7 File Offset: 0x0000CEB7
		public WarmupStateChange()
		{
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0000ECBF File Offset: 0x0000CEBF
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.WarmupState, CompressionMission.MissionRoundStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.StateStartTimeInSeconds, CompressionMatchmaker.MissionTimeCompressionInfo);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0000ECE4 File Offset: 0x0000CEE4
		protected override bool OnRead()
		{
			bool flag = true;
			this.WarmupState = (MultiplayerWarmupComponent.WarmupStates)GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissionRoundStateCompressionInfo, ref flag);
			this.StateStartTimeInSeconds = GameNetworkMessage.ReadFloatFromPacket(CompressionMatchmaker.MissionTimeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0000ED18 File Offset: 0x0000CF18
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0000ED20 File Offset: 0x0000CF20
		protected override string OnGetLogFormat()
		{
			return "Warmup state set to " + this.WarmupState;
		}
	}
}
