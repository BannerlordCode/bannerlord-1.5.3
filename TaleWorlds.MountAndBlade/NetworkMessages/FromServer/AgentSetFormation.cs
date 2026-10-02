using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000077 RID: 119
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AgentSetFormation : GameNetworkMessage
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00007E97 File Offset: 0x00006097
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x00007E9F File Offset: 0x0000609F
		public int AgentIndex { get; private set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00007EA8 File Offset: 0x000060A8
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x00007EB0 File Offset: 0x000060B0
		public int FormationIndex { get; private set; }

		// Token: 0x06000440 RID: 1088 RVA: 0x00007EB9 File Offset: 0x000060B9
		public AgentSetFormation(int agentIndex, int formationIndex)
		{
			this.AgentIndex = agentIndex;
			this.FormationIndex = formationIndex;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00007ECF File Offset: 0x000060CF
		public AgentSetFormation()
		{
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00007ED8 File Offset: 0x000060D8
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00007F07 File Offset: 0x00006107
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00007F24 File Offset: 0x00006124
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00007F2C File Offset: 0x0000612C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Assign agent with agent-index: ", this.AgentIndex, " to formation with index: ", this.FormationIndex });
		}
	}
}
