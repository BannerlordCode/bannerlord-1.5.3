using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000095 RID: 149
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MakeAgentDead : GameNetworkMessage
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0000AAB3 File Offset: 0x00008CB3
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x0000AABB File Offset: 0x00008CBB
		public int AgentIndex { get; private set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0000AAC4 File Offset: 0x00008CC4
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x0000AACC File Offset: 0x00008CCC
		public bool IsKilled { get; private set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0000AAD5 File Offset: 0x00008CD5
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x0000AADD File Offset: 0x00008CDD
		public ActionIndexCache ActionCodeIndex { get; private set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0000AAE6 File Offset: 0x00008CE6
		// (set) Token: 0x060005E9 RID: 1513 RVA: 0x0000AAEE File Offset: 0x00008CEE
		public int CorpsesToFadeIndex { get; private set; }

		// Token: 0x060005EA RID: 1514 RVA: 0x0000AAF7 File Offset: 0x00008CF7
		public MakeAgentDead(int agentIndex, bool isKilled, ActionIndexCache actionCodeIndex, int corpsesToFadeIndex = -1)
		{
			this.AgentIndex = agentIndex;
			this.IsKilled = isKilled;
			this.ActionCodeIndex = actionCodeIndex;
			this.CorpsesToFadeIndex = corpsesToFadeIndex;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0000AB1C File Offset: 0x00008D1C
		public MakeAgentDead()
		{
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0000AB24 File Offset: 0x00008D24
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.IsKilled = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.ActionCodeIndex = new ActionIndexCache(GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ActionCodeCompressionInfo, ref flag));
			this.CorpsesToFadeIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0000AB74 File Offset: 0x00008D74
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteBoolToPacket(this.IsKilled);
			GameNetworkMessage.WriteIntToPacket(this.ActionCodeIndex.Index, CompressionBasic.ActionCodeCompressionInfo);
			GameNetworkMessage.WriteAgentIndexToPacket(this.CorpsesToFadeIndex);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0000ABBA File Offset: 0x00008DBA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0000ABBF File Offset: 0x00008DBF
		protected override string OnGetLogFormat()
		{
			return "Make Agent Dead on Agent with agent-index: " + this.AgentIndex;
		}
	}
}
