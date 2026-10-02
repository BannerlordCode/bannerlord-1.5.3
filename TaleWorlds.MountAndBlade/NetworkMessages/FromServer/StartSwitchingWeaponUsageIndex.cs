using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CA RID: 202
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class StartSwitchingWeaponUsageIndex : GameNetworkMessage
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0000E0C8 File Offset: 0x0000C2C8
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x0000E0D0 File Offset: 0x0000C2D0
		public int AgentIndex { get; private set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0000E0D9 File Offset: 0x0000C2D9
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x0000E0E1 File Offset: 0x0000C2E1
		public EquipmentIndex EquipmentIndex { get; private set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0000E0EA File Offset: 0x0000C2EA
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x0000E0F2 File Offset: 0x0000C2F2
		public int UsageIndex { get; private set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x0000E0FB File Offset: 0x0000C2FB
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x0000E103 File Offset: 0x0000C303
		public Agent.UsageDirection CurrentMovementFlagUsageDirection { get; private set; }

		// Token: 0x06000847 RID: 2119 RVA: 0x0000E10C File Offset: 0x0000C30C
		public StartSwitchingWeaponUsageIndex(int agentIndex, EquipmentIndex equipmentIndex, int usageIndex, Agent.UsageDirection currentMovementFlagUsageDirection)
		{
			this.AgentIndex = agentIndex;
			this.EquipmentIndex = equipmentIndex;
			this.UsageIndex = usageIndex;
			this.CurrentMovementFlagUsageDirection = currentMovementFlagUsageDirection;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0000E131 File Offset: 0x0000C331
		public StartSwitchingWeaponUsageIndex()
		{
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0000E13C File Offset: 0x0000C33C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.EquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.UsageIndex = (int)((short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponUsageIndexCompressionInfo, ref flag));
			this.CurrentMovementFlagUsageDirection = (Agent.UsageDirection)GameNetworkMessage.ReadIntFromPacket(CompressionMission.UsageDirectionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0000E190 File Offset: 0x0000C390
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.EquipmentIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.UsageIndex, CompressionMission.WeaponUsageIndexCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.CurrentMovementFlagUsageDirection, CompressionMission.UsageDirectionCompressionInfo);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0000E1CD File Offset: 0x0000C3CD
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0000E1D4 File Offset: 0x0000C3D4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "StartSwitchingWeaponUsageIndex: ", this.UsageIndex, " for weapon with EquipmentIndex: ", this.EquipmentIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
