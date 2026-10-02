using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C5 RID: 197
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetWieldedItemIndex : GameNetworkMessage
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0000D89E File Offset: 0x0000BA9E
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0000D8A6 File Offset: 0x0000BAA6
		public int AgentIndex { get; private set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0000D8AF File Offset: 0x0000BAAF
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0000D8B7 File Offset: 0x0000BAB7
		public bool IsLeftHand { get; private set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0000D8C0 File Offset: 0x0000BAC0
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x0000D8C8 File Offset: 0x0000BAC8
		public bool IsWieldedInstantly { get; private set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0000D8D1 File Offset: 0x0000BAD1
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x0000D8D9 File Offset: 0x0000BAD9
		public bool IsWieldedOnSpawn { get; private set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0000D8E2 File Offset: 0x0000BAE2
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x0000D8EA File Offset: 0x0000BAEA
		public EquipmentIndex WieldedItemIndex { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0000D8F3 File Offset: 0x0000BAF3
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x0000D8FB File Offset: 0x0000BAFB
		public int MainHandCurrentUsageIndex { get; private set; }

		// Token: 0x060007F9 RID: 2041 RVA: 0x0000D904 File Offset: 0x0000BB04
		public SetWieldedItemIndex(int agentIndex, bool isLeftHand, bool isWieldedInstantly, bool isWieldedOnSpawn, EquipmentIndex wieldedItemIndex, int mainHandCurUsageIndex)
		{
			this.AgentIndex = agentIndex;
			this.IsLeftHand = isLeftHand;
			this.IsWieldedInstantly = isWieldedInstantly;
			this.IsWieldedOnSpawn = isWieldedOnSpawn;
			this.WieldedItemIndex = wieldedItemIndex;
			this.MainHandCurrentUsageIndex = mainHandCurUsageIndex;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0000D939 File Offset: 0x0000BB39
		public SetWieldedItemIndex()
		{
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0000D944 File Offset: 0x0000BB44
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.IsLeftHand = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsWieldedInstantly = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsWieldedOnSpawn = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.WieldedItemIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WieldSlotCompressionInfo, ref flag);
			this.MainHandCurrentUsageIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponUsageIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0000D9AC File Offset: 0x0000BBAC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteBoolToPacket(this.IsLeftHand);
			GameNetworkMessage.WriteBoolToPacket(this.IsWieldedInstantly);
			GameNetworkMessage.WriteBoolToPacket(this.IsWieldedOnSpawn);
			GameNetworkMessage.WriteIntToPacket((int)this.WieldedItemIndex, CompressionMission.WieldSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.MainHandCurrentUsageIndex, CompressionMission.WeaponUsageIndexCompressionInfo);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0000DA05 File Offset: 0x0000BC05
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0000DA0A File Offset: 0x0000BC0A
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Wielded Item Index to: ", this.WieldedItemIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
