using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D6 RID: 214
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class WeaponUsageIndexChangeMessage : GameNetworkMessage
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0000ED37 File Offset: 0x0000CF37
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x0000ED3F File Offset: 0x0000CF3F
		public int AgentIndex { get; private set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0000ED48 File Offset: 0x0000CF48
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x0000ED50 File Offset: 0x0000CF50
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x0000ED59 File Offset: 0x0000CF59
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x0000ED61 File Offset: 0x0000CF61
		public int UsageIndex { get; private set; }

		// Token: 0x060008CD RID: 2253 RVA: 0x0000ED6A File Offset: 0x0000CF6A
		public WeaponUsageIndexChangeMessage()
		{
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0000ED72 File Offset: 0x0000CF72
		public WeaponUsageIndexChangeMessage(int agentIndex, EquipmentIndex slotIndex, int usageIndex)
		{
			this.AgentIndex = agentIndex;
			this.SlotIndex = slotIndex;
			this.UsageIndex = usageIndex;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0000ED90 File Offset: 0x0000CF90
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SlotIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.UsageIndex = (int)((short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponUsageIndexCompressionInfo, ref flag));
			return flag;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0000EDD2 File Offset: 0x0000CFD2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.SlotIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.UsageIndex, CompressionMission.WeaponUsageIndexCompressionInfo);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0000EDFF File Offset: 0x0000CFFF
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0000EE08 File Offset: 0x0000D008
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Weapon Usage Index: ", this.UsageIndex, " for weapon with EquipmentIndex: ", this.SlotIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
