using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004D RID: 77
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class EquipEquipmentToPeer : GameNetworkMessage
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00005236 File Offset: 0x00003436
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000523E File Offset: 0x0000343E
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00005247 File Offset: 0x00003447
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0000524F File Offset: 0x0000344F
		public Equipment Equipment { get; private set; }

		// Token: 0x0600028F RID: 655 RVA: 0x00005258 File Offset: 0x00003458
		public EquipEquipmentToPeer(NetworkCommunicator peer, Equipment equipment)
		{
			this.Peer = peer;
			this.Equipment = new Equipment();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				this.Equipment[equipmentIndex] = equipment.GetEquipmentFromSlot(equipmentIndex);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000529D File Offset: 0x0000349D
		public EquipEquipmentToPeer()
		{
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000052A8 File Offset: 0x000034A8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			if (flag)
			{
				this.Equipment = new Equipment();
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
				{
					if (flag)
					{
						this.Equipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, ModuleNetworkData.ReadItemReferenceFromPacket(MBObjectManager.Instance, ref flag));
					}
				}
			}
			return flag;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000052FC File Offset: 0x000034FC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				ModuleNetworkData.WriteItemReferenceToPacket(this.Equipment.GetEquipmentFromSlot(equipmentIndex));
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00005332 File Offset: 0x00003532
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Equipment;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00005337 File Offset: 0x00003537
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Equip equipment to peer: ",
				this.Peer.UserName,
				" with peer-index:",
				this.Peer.Index
			});
		}
	}
}
