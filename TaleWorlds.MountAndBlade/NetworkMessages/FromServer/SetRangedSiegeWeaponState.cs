using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B7 RID: 183
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetRangedSiegeWeaponState : GameNetworkMessage
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0000CC97 File Offset: 0x0000AE97
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x0000CC9F File Offset: 0x0000AE9F
		public MissionObjectId RangedSiegeWeaponId { get; private set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x0000CCA8 File Offset: 0x0000AEA8
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x0000CCB0 File Offset: 0x0000AEB0
		public RangedSiegeWeapon.WeaponState State { get; private set; }

		// Token: 0x06000761 RID: 1889 RVA: 0x0000CCB9 File Offset: 0x0000AEB9
		public SetRangedSiegeWeaponState(MissionObjectId rangedSiegeWeaponId, RangedSiegeWeapon.WeaponState state)
		{
			this.RangedSiegeWeaponId = rangedSiegeWeaponId;
			this.State = state;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0000CCCF File Offset: 0x0000AECF
		public SetRangedSiegeWeaponState()
		{
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0000CCD8 File Offset: 0x0000AED8
		protected override bool OnRead()
		{
			bool flag = true;
			this.RangedSiegeWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.State = (RangedSiegeWeapon.WeaponState)GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponStateCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0000CD07 File Offset: 0x0000AF07
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.RangedSiegeWeaponId);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.RangedSiegeWeaponStateCompressionInfo);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0000CD24 File Offset: 0x0000AF24
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0000CD2C File Offset: 0x0000AF2C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set RangedSiegeWeapon State to: ", this.State, " on RangedSiegeWeapon with ID: ", this.RangedSiegeWeaponId });
		}
	}
}
