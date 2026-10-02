using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002E RID: 46
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class DropWeapon : GameNetworkMessage
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00003D6D File Offset: 0x00001F6D
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00003D75 File Offset: 0x00001F75
		public bool IsDefendPressed { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00003D7E File Offset: 0x00001F7E
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00003D86 File Offset: 0x00001F86
		public EquipmentIndex ForcedSlotIndexToDropWeaponFrom { get; private set; }

		// Token: 0x06000171 RID: 369 RVA: 0x00003D8F File Offset: 0x00001F8F
		public DropWeapon(bool isDefendPressed, EquipmentIndex forcedSlotIndexToDropWeaponFrom)
		{
			this.IsDefendPressed = isDefendPressed;
			this.ForcedSlotIndexToDropWeaponFrom = forcedSlotIndexToDropWeaponFrom;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00003DA5 File Offset: 0x00001FA5
		public DropWeapon()
		{
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00003DB0 File Offset: 0x00001FB0
		protected override bool OnRead()
		{
			bool flag = true;
			this.IsDefendPressed = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.ForcedSlotIndexToDropWeaponFrom = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WieldSlotCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00003DDF File Offset: 0x00001FDF
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.IsDefendPressed);
			GameNetworkMessage.WriteIntToPacket((int)this.ForcedSlotIndexToDropWeaponFrom, CompressionMission.WieldSlotCompressionInfo);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00003DFC File Offset: 0x00001FFC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00003E00 File Offset: 0x00002000
		protected override string OnGetLogFormat()
		{
			bool flag = this.ForcedSlotIndexToDropWeaponFrom != EquipmentIndex.None;
			return "Dropping " + ((!flag) ? "equipped" : "") + " weapon" + (flag ? (" " + (int)this.ForcedSlotIndexToDropWeaponFrom) : "");
		}
	}
}
