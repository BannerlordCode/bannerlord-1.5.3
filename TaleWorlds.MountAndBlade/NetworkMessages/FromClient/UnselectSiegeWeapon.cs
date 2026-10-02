using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200003C RID: 60
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class UnselectSiegeWeapon : GameNetworkMessage
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000043B8 File Offset: 0x000025B8
		// (set) Token: 0x060001DC RID: 476 RVA: 0x000043C0 File Offset: 0x000025C0
		public MissionObjectId SiegeWeaponId { get; private set; }

		// Token: 0x060001DD RID: 477 RVA: 0x000043C9 File Offset: 0x000025C9
		public UnselectSiegeWeapon(MissionObjectId siegeWeaponId)
		{
			this.SiegeWeaponId = siegeWeaponId;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000043D8 File Offset: 0x000025D8
		public UnselectSiegeWeapon()
		{
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000043E0 File Offset: 0x000025E0
		protected override bool OnRead()
		{
			bool flag = true;
			this.SiegeWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000043FD File Offset: 0x000025FD
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SiegeWeaponId);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000440A File Offset: 0x0000260A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00004412 File Offset: 0x00002612
		protected override string OnGetLogFormat()
		{
			return "Deselect SiegeWeapon with ID: " + this.SiegeWeaponId;
		}
	}
}
