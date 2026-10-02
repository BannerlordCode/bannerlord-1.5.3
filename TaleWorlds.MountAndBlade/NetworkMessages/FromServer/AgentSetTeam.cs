using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000078 RID: 120
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AgentSetTeam : GameNetworkMessage
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00007F65 File Offset: 0x00006165
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x00007F6D File Offset: 0x0000616D
		public int AgentIndex { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00007F76 File Offset: 0x00006176
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00007F7E File Offset: 0x0000617E
		public int TeamIndex { get; private set; }

		// Token: 0x0600044A RID: 1098 RVA: 0x00007F87 File Offset: 0x00006187
		public AgentSetTeam(int agentIndex, int teamIndex)
		{
			this.AgentIndex = agentIndex;
			this.TeamIndex = teamIndex;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00007F9D File Offset: 0x0000619D
		public AgentSetTeam()
		{
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00007FA8 File Offset: 0x000061A8
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.TeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00007FD2 File Offset: 0x000061D2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteTeamIndexToPacket(this.TeamIndex);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00007FEA File Offset: 0x000061EA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00007FF2 File Offset: 0x000061F2
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Assign agent with agent-index: ", this.AgentIndex, " to team: ", this.TeamIndex });
		}
	}
}
