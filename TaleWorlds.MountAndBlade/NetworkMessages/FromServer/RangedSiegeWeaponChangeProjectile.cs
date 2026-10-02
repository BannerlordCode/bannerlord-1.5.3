using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000096 RID: 150
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RangedSiegeWeaponChangeProjectile : GameNetworkMessage
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x0000ABD6 File Offset: 0x00008DD6
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x0000ABDE File Offset: 0x00008DDE
		public MissionObjectId RangedSiegeWeaponId { get; private set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0000ABE7 File Offset: 0x00008DE7
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x0000ABEF File Offset: 0x00008DEF
		public int Index { get; private set; }

		// Token: 0x060005F4 RID: 1524 RVA: 0x0000ABF8 File Offset: 0x00008DF8
		public RangedSiegeWeaponChangeProjectile(MissionObjectId rangedSiegeWeaponId, int index)
		{
			this.RangedSiegeWeaponId = rangedSiegeWeaponId;
			this.Index = index;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0000AC0E File Offset: 0x00008E0E
		public RangedSiegeWeaponChangeProjectile()
		{
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0000AC18 File Offset: 0x00008E18
		protected override bool OnRead()
		{
			bool flag = true;
			this.RangedSiegeWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Index = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0000AC47 File Offset: 0x00008E47
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.RangedSiegeWeaponId);
			GameNetworkMessage.WriteIntToPacket(this.Index, CompressionMission.RangedSiegeWeaponAmmoIndexCompressionInfo);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0000AC64 File Offset: 0x00008E64
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0000AC6C File Offset: 0x00008E6C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Changed Projectile Type Index to: ", this.Index, " on RangedSiegeWeapon with ID: ", this.RangedSiegeWeaponId });
		}
	}
}
