using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C3 RID: 195
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetWeaponNetworkData : GameNetworkMessage
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0000D64E File Offset: 0x0000B84E
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x0000D656 File Offset: 0x0000B856
		public int AgentIndex { get; private set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0000D65F File Offset: 0x0000B85F
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x0000D667 File Offset: 0x0000B867
		public EquipmentIndex WeaponEquipmentIndex { get; private set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0000D670 File Offset: 0x0000B870
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x0000D678 File Offset: 0x0000B878
		public short DataValue { get; private set; }

		// Token: 0x060007DB RID: 2011 RVA: 0x0000D681 File Offset: 0x0000B881
		public SetWeaponNetworkData(int agent, EquipmentIndex weaponEquipmentIndex, short dataValue)
		{
			this.AgentIndex = agent;
			this.WeaponEquipmentIndex = weaponEquipmentIndex;
			this.DataValue = dataValue;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0000D69E File Offset: 0x0000B89E
		public SetWeaponNetworkData()
		{
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.WeaponEquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.DataValue = (short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemDataCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0000D6EA File Offset: 0x0000B8EA
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.WeaponEquipmentIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.DataValue, CompressionMission.ItemDataCompressionInfo);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0000D717 File Offset: 0x0000B917
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0000D71C File Offset: 0x0000B91C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Network data: ", this.DataValue, " for weapon with EquipmentIndex: ", this.WeaponEquipmentIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
