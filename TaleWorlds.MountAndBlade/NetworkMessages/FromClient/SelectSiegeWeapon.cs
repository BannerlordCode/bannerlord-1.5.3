using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000036 RID: 54
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SelectSiegeWeapon : GameNetworkMessage
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00004050 File Offset: 0x00002250
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00004058 File Offset: 0x00002258
		public MissionObjectId SiegeWeaponId { get; private set; }

		// Token: 0x060001A7 RID: 423 RVA: 0x00004061 File Offset: 0x00002261
		public SelectSiegeWeapon(MissionObjectId siegeWeaponId)
		{
			this.SiegeWeaponId = siegeWeaponId;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00004070 File Offset: 0x00002270
		public SelectSiegeWeapon()
		{
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00004078 File Offset: 0x00002278
		protected override bool OnRead()
		{
			bool flag = true;
			this.SiegeWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00004095 File Offset: 0x00002295
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SiegeWeaponId);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000040A2 File Offset: 0x000022A2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000040AA File Offset: 0x000022AA
		protected override string OnGetLogFormat()
		{
			return "Select SiegeWeapon with ID: " + this.SiegeWeaponId;
		}
	}
}
