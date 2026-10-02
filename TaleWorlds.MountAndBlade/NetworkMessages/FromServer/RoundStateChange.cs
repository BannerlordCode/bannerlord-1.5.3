using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000071 RID: 113
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RoundStateChange : GameNetworkMessage
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000777B File Offset: 0x0000597B
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x00007783 File Offset: 0x00005983
		public MultiplayerRoundState RoundState { get; private set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0000778C File Offset: 0x0000598C
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x00007794 File Offset: 0x00005994
		public float StateStartTimeInSeconds { get; private set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000779D File Offset: 0x0000599D
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x000077A5 File Offset: 0x000059A5
		public int RemainingTimeOnPreviousState { get; private set; }

		// Token: 0x060003F4 RID: 1012 RVA: 0x000077AE File Offset: 0x000059AE
		public RoundStateChange(MultiplayerRoundState roundState, long stateStartTimeInTicks, int remainingTimeOnPreviousState)
		{
			this.RoundState = roundState;
			this.StateStartTimeInSeconds = (float)stateStartTimeInTicks / 10000000f;
			this.RemainingTimeOnPreviousState = remainingTimeOnPreviousState;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000077D2 File Offset: 0x000059D2
		public RoundStateChange()
		{
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000077DC File Offset: 0x000059DC
		protected override bool OnRead()
		{
			bool flag = true;
			this.RoundState = (MultiplayerRoundState)GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissionRoundStateCompressionInfo, ref flag);
			this.StateStartTimeInSeconds = GameNetworkMessage.ReadFloatFromPacket(CompressionMatchmaker.MissionTimeCompressionInfo, ref flag);
			this.RemainingTimeOnPreviousState = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RoundTimeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00007822 File Offset: 0x00005A22
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.RoundState, CompressionMission.MissionRoundStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.StateStartTimeInSeconds, CompressionMatchmaker.MissionTimeCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.RemainingTimeOnPreviousState, CompressionMission.RoundTimeCompressionInfo);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00007854 File Offset: 0x00005A54
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000785C File Offset: 0x00005A5C
		protected override string OnGetLogFormat()
		{
			return "Changing round state to: " + this.RoundState;
		}
	}
}
