using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C4 RID: 196
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetWeaponReloadPhase : GameNetworkMessage
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0000D776 File Offset: 0x0000B976
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x0000D77E File Offset: 0x0000B97E
		public int AgentIndex { get; private set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0000D787 File Offset: 0x0000B987
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x0000D78F File Offset: 0x0000B98F
		public EquipmentIndex EquipmentIndex { get; private set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0000D798 File Offset: 0x0000B998
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x0000D7A0 File Offset: 0x0000B9A0
		public short ReloadPhase { get; private set; }

		// Token: 0x060007E7 RID: 2023 RVA: 0x0000D7A9 File Offset: 0x0000B9A9
		public SetWeaponReloadPhase(int agentIndex, EquipmentIndex equipmentIndex, short reloadPhase)
		{
			this.AgentIndex = agentIndex;
			this.EquipmentIndex = equipmentIndex;
			this.ReloadPhase = reloadPhase;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0000D7C6 File Offset: 0x0000B9C6
		public SetWeaponReloadPhase()
		{
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0000D7D0 File Offset: 0x0000B9D0
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.EquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.ReloadPhase = (short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponReloadPhaseCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0000D812 File Offset: 0x0000BA12
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.EquipmentIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.ReloadPhase, CompressionMission.WeaponReloadPhaseCompressionInfo);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0000D83F File Offset: 0x0000BA3F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0000D844 File Offset: 0x0000BA44
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Reload Phase: ", this.ReloadPhase, " for weapon with EquipmentIndex: ", this.EquipmentIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
