using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A2 RID: 162
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentPrefabComponentVisibility : GameNetworkMessage
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x0000B6BC File Offset: 0x000098BC
		// (set) Token: 0x06000675 RID: 1653 RVA: 0x0000B6C4 File Offset: 0x000098C4
		public int AgentIndex { get; private set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0000B6CD File Offset: 0x000098CD
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x0000B6D5 File Offset: 0x000098D5
		public int ComponentIndex { get; private set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x0000B6DE File Offset: 0x000098DE
		// (set) Token: 0x06000679 RID: 1657 RVA: 0x0000B6E6 File Offset: 0x000098E6
		public bool Visibility { get; private set; }

		// Token: 0x0600067A RID: 1658 RVA: 0x0000B6EF File Offset: 0x000098EF
		public SetAgentPrefabComponentVisibility(int agentIndex, int componentIndex, bool visibility)
		{
			this.AgentIndex = agentIndex;
			this.ComponentIndex = componentIndex;
			this.Visibility = visibility;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0000B70C File Offset: 0x0000990C
		public SetAgentPrefabComponentVisibility()
		{
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0000B714 File Offset: 0x00009914
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.ComponentIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentPrefabComponentIndexCompressionInfo, ref flag);
			this.Visibility = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0000B750 File Offset: 0x00009950
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.ComponentIndex, CompressionMission.AgentPrefabComponentIndexCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.Visibility);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0000B778 File Offset: 0x00009978
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0000B780 File Offset: 0x00009980
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set Component with index: ",
				this.ComponentIndex,
				" to be ",
				this.Visibility ? "visible" : "invisible",
				" on Agent with agent-index: ",
				this.AgentIndex
			});
		}
	}
}
