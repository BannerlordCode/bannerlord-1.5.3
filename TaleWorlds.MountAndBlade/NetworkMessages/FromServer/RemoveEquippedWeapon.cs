using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000099 RID: 153
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemoveEquippedWeapon : GameNetworkMessage
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x0000ADEA File Offset: 0x00008FEA
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x0000ADF2 File Offset: 0x00008FF2
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0000ADFB File Offset: 0x00008FFB
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x0000AE03 File Offset: 0x00009003
		public int AgentIndex { get; private set; }

		// Token: 0x06000610 RID: 1552 RVA: 0x0000AE0C File Offset: 0x0000900C
		public RemoveEquippedWeapon(int agentIndex, EquipmentIndex slot)
		{
			this.AgentIndex = agentIndex;
			this.SlotIndex = slot;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0000AE22 File Offset: 0x00009022
		public RemoveEquippedWeapon()
		{
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0000AE2A File Offset: 0x0000902A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.SlotIndex, CompressionMission.ItemSlotCompressionInfo);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0000AE48 File Offset: 0x00009048
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SlotIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0000AE77 File Offset: 0x00009077
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0000AE7F File Offset: 0x0000907F
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Remove equipped weapon from SlotIndex: ", this.SlotIndex, " on agent with agent-index: ", this.AgentIndex });
		}
	}
}
