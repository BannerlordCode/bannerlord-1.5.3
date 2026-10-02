using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C2 RID: 194
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetWeaponAmmoData : GameNetworkMessage
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0000D4E9 File Offset: 0x0000B6E9
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x0000D4F1 File Offset: 0x0000B6F1
		public int AgentIndex { get; private set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0000D4FA File Offset: 0x0000B6FA
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x0000D502 File Offset: 0x0000B702
		public EquipmentIndex WeaponEquipmentIndex { get; private set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x0000D50B File Offset: 0x0000B70B
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x0000D513 File Offset: 0x0000B713
		public EquipmentIndex AmmoEquipmentIndex { get; private set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x0000D51C File Offset: 0x0000B71C
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x0000D524 File Offset: 0x0000B724
		public short Ammo { get; private set; }

		// Token: 0x060007CF RID: 1999 RVA: 0x0000D52D File Offset: 0x0000B72D
		public SetWeaponAmmoData(int agentIndex, EquipmentIndex weaponEquipmentIndex, EquipmentIndex ammoEquipmentIndex, short ammo)
		{
			this.AgentIndex = agentIndex;
			this.WeaponEquipmentIndex = weaponEquipmentIndex;
			this.AmmoEquipmentIndex = ammoEquipmentIndex;
			this.Ammo = ammo;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0000D552 File Offset: 0x0000B752
		public SetWeaponAmmoData()
		{
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0000D55C File Offset: 0x0000B75C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.WeaponEquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.AmmoEquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WieldSlotCompressionInfo, ref flag);
			this.Ammo = (short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemDataCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0000D5B0 File Offset: 0x0000B7B0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.WeaponEquipmentIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.AmmoEquipmentIndex, CompressionMission.WieldSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.Ammo, CompressionMission.ItemDataCompressionInfo);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0000D5ED File Offset: 0x0000B7ED
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0000D5F4 File Offset: 0x0000B7F4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set ammo: ", this.Ammo, " for weapon with EquipmentIndex: ", this.WeaponEquipmentIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
