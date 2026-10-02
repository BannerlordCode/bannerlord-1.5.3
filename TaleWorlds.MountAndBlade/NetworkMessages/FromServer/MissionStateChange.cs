using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000059 RID: 89
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MissionStateChange : GameNetworkMessage
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00006199 File Offset: 0x00004399
		// (set) Token: 0x0600031E RID: 798 RVA: 0x000061A1 File Offset: 0x000043A1
		public MissionLobbyComponent.MultiplayerGameState CurrentState { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000061AA File Offset: 0x000043AA
		// (set) Token: 0x06000320 RID: 800 RVA: 0x000061B2 File Offset: 0x000043B2
		public float StateStartTimeInSeconds { get; private set; }

		// Token: 0x06000321 RID: 801 RVA: 0x000061BB File Offset: 0x000043BB
		public MissionStateChange(MissionLobbyComponent.MultiplayerGameState currentState, long stateStartTimeInTicks)
		{
			this.CurrentState = currentState;
			this.StateStartTimeInSeconds = (float)stateStartTimeInTicks / 10000000f;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000061D8 File Offset: 0x000043D8
		public MissionStateChange()
		{
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000061E0 File Offset: 0x000043E0
		protected override bool OnRead()
		{
			bool flag = true;
			this.CurrentState = (MissionLobbyComponent.MultiplayerGameState)GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.MissionCurrentStateCompressionInfo, ref flag);
			if (this.CurrentState != MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				this.StateStartTimeInSeconds = GameNetworkMessage.ReadFloatFromPacket(CompressionMatchmaker.MissionTimeCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000621C File Offset: 0x0000441C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.CurrentState, CompressionMatchmaker.MissionCurrentStateCompressionInfo);
			if (this.CurrentState != MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				GameNetworkMessage.WriteFloatToPacket(this.StateStartTimeInSeconds, CompressionMatchmaker.MissionTimeCompressionInfo);
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00006246 File Offset: 0x00004446
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000624E File Offset: 0x0000444E
		protected override string OnGetLogFormat()
		{
			return "Mission State has changed to: " + this.CurrentState;
		}
	}
}
