using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D4 RID: 212
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class UseObject : GameNetworkMessage
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0000EB57 File Offset: 0x0000CD57
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x0000EB5F File Offset: 0x0000CD5F
		public int AgentIndex { get; private set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0000EB68 File Offset: 0x0000CD68
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x0000EB70 File Offset: 0x0000CD70
		public MissionObjectId UsableGameObjectId { get; private set; }

		// Token: 0x060008B7 RID: 2231 RVA: 0x0000EB79 File Offset: 0x0000CD79
		public UseObject(int agentIndex, MissionObjectId usableGameObjectId)
		{
			this.AgentIndex = agentIndex;
			this.UsableGameObjectId = usableGameObjectId;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0000EB8F File Offset: 0x0000CD8F
		public UseObject()
		{
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0000EB98 File Offset: 0x0000CD98
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.UsableGameObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0000EBC2 File Offset: 0x0000CDC2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteMissionObjectIdToPacket((this.UsableGameObjectId.Id >= 0) ? this.UsableGameObjectId : MissionObjectId.Invalid);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0000EBEF File Offset: 0x0000CDEF
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents | MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0000EBF8 File Offset: 0x0000CDF8
		protected override string OnGetLogFormat()
		{
			string text = "Use UsableMissionObject with ID: ";
			if (this.UsableGameObjectId != MissionObjectId.Invalid)
			{
				text += this.UsableGameObjectId;
			}
			else
			{
				text += "null";
			}
			text += " by Agent with name: ";
			if (this.AgentIndex >= 0)
			{
				text = text + "agent-index: " + this.AgentIndex;
			}
			else
			{
				text += "null";
			}
			return text;
		}
	}
}
