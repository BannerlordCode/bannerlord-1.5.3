using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B8 RID: 184
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetRangedSiegeWeaponAmmo : GameNetworkMessage
	{
		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x0000CD65 File Offset: 0x0000AF65
		// (set) Token: 0x06000768 RID: 1896 RVA: 0x0000CD6D File Offset: 0x0000AF6D
		public MissionObjectId RangedSiegeWeaponId { get; private set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0000CD76 File Offset: 0x0000AF76
		// (set) Token: 0x0600076A RID: 1898 RVA: 0x0000CD7E File Offset: 0x0000AF7E
		public int AmmoCount { get; private set; }

		// Token: 0x0600076B RID: 1899 RVA: 0x0000CD87 File Offset: 0x0000AF87
		public SetRangedSiegeWeaponAmmo(MissionObjectId rangedSiegeWeaponId, int ammoCount)
		{
			this.RangedSiegeWeaponId = rangedSiegeWeaponId;
			this.AmmoCount = ammoCount;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0000CD9D File Offset: 0x0000AF9D
		public SetRangedSiegeWeaponAmmo()
		{
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
		protected override bool OnRead()
		{
			bool flag = true;
			this.RangedSiegeWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.AmmoCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0000CDD7 File Offset: 0x0000AFD7
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.RangedSiegeWeaponId);
			GameNetworkMessage.WriteIntToPacket(this.AmmoCount, CompressionMission.RangedSiegeWeaponAmmoCompressionInfo);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0000CDF4 File Offset: 0x0000AFF4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0000CDFC File Offset: 0x0000AFFC
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set ammo left to: ", this.AmmoCount, " on RangedSiegeWeapon with ID: ", this.RangedSiegeWeaponId });
		}
	}
}
