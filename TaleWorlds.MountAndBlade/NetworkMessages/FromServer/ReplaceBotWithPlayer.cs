using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009C RID: 156
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ReplaceBotWithPlayer : GameNetworkMessage
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0000B04B File Offset: 0x0000924B
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x0000B053 File Offset: 0x00009253
		public int BotAgentIndex { get; private set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0000B05C File Offset: 0x0000925C
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x0000B064 File Offset: 0x00009264
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x0000B06D File Offset: 0x0000926D
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x0000B075 File Offset: 0x00009275
		public int Health { get; private set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0000B07E File Offset: 0x0000927E
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x0000B086 File Offset: 0x00009286
		public int MountHealth { get; private set; }

		// Token: 0x06000632 RID: 1586 RVA: 0x0000B08F File Offset: 0x0000928F
		public ReplaceBotWithPlayer(NetworkCommunicator peer, int botAgentIndex, float botAgentHealth, float botAgentMountHealth = -1f)
		{
			this.Peer = peer;
			this.BotAgentIndex = botAgentIndex;
			this.Health = MathF.Ceiling(botAgentHealth);
			this.MountHealth = MathF.Ceiling(botAgentMountHealth);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0000B0BE File Offset: 0x000092BE
		public ReplaceBotWithPlayer()
		{
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0000B0C8 File Offset: 0x000092C8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.BotAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Health = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentHealthCompressionInfo, ref flag);
			this.MountHealth = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentHealthCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0000B117 File Offset: 0x00009317
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteAgentIndexToPacket(this.BotAgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.Health, CompressionMission.AgentHealthCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.MountHealth, CompressionMission.AgentHealthCompressionInfo);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0000B14F File Offset: 0x0000934F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0000B157 File Offset: 0x00009357
		protected override string OnGetLogFormat()
		{
			return "Replace a bot with a player";
		}
	}
}
