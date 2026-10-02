using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009F RID: 159
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentIsPlayer : GameNetworkMessage
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0000B459 File Offset: 0x00009659
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x0000B461 File Offset: 0x00009661
		public int AgentIndex { get; private set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0000B46A File Offset: 0x0000966A
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x0000B472 File Offset: 0x00009672
		public bool IsPlayer { get; private set; }

		// Token: 0x0600065A RID: 1626 RVA: 0x0000B47B File Offset: 0x0000967B
		public SetAgentIsPlayer(int agentIndex, bool isPlayer)
		{
			this.AgentIndex = agentIndex;
			this.IsPlayer = isPlayer;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0000B491 File Offset: 0x00009691
		public SetAgentIsPlayer()
		{
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0000B49C File Offset: 0x0000969C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.IsPlayer = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0000B4C6 File Offset: 0x000096C6
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteBoolToPacket(this.IsPlayer);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0000B4DE File Offset: 0x000096DE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0000B4E6 File Offset: 0x000096E6
		protected override string OnGetLogFormat()
		{
			return "Set Controller is player on Agent with agent-index: " + this.AgentIndex + (this.IsPlayer ? " - TRUE." : " - FALSE.");
		}
	}
}
