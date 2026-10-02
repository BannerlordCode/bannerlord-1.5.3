using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CD RID: 205
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SynchronizeAgentSpawnEquipment : GameNetworkMessage
	{
		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x0000E3FD File Offset: 0x0000C5FD
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x0000E405 File Offset: 0x0000C605
		public int AgentIndex { get; private set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0000E40E File Offset: 0x0000C60E
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x0000E416 File Offset: 0x0000C616
		public Equipment SpawnEquipment { get; private set; }

		// Token: 0x06000867 RID: 2151 RVA: 0x0000E420 File Offset: 0x0000C620
		public SynchronizeAgentSpawnEquipment(int agentIndex, Equipment spawnEquipment)
		{
			this.AgentIndex = agentIndex;
			this.SpawnEquipment = new Equipment();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				this.SpawnEquipment[equipmentIndex] = spawnEquipment.GetEquipmentFromSlot(equipmentIndex);
			}
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0000E465 File Offset: 0x0000C665
		public SynchronizeAgentSpawnEquipment()
		{
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0000E470 File Offset: 0x0000C670
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SpawnEquipment = new Equipment();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				this.SpawnEquipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, ModuleNetworkData.ReadItemReferenceFromPacket(MBObjectManager.Instance, ref flag));
			}
			return flag;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0000E4C0 File Offset: 0x0000C6C0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				ModuleNetworkData.WriteItemReferenceToPacket(this.SpawnEquipment.GetEquipmentFromSlot(equipmentIndex));
			}
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0000E4F6 File Offset: 0x0000C6F6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0000E4FE File Offset: 0x0000C6FE
		protected override string OnGetLogFormat()
		{
			return "Equipment synchronized for agent-index: " + this.AgentIndex;
		}
	}
}
