using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BF RID: 191
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetStonePileAmmo : GameNetworkMessage
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0000D261 File Offset: 0x0000B461
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x0000D269 File Offset: 0x0000B469
		public MissionObjectId StonePileId { get; private set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0000D272 File Offset: 0x0000B472
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x0000D27A File Offset: 0x0000B47A
		public int AmmoCount { get; private set; }

		// Token: 0x060007AD RID: 1965 RVA: 0x0000D283 File Offset: 0x0000B483
		public SetStonePileAmmo(MissionObjectId stonePileId, int ammoCount)
		{
			this.StonePileId = stonePileId;
			this.AmmoCount = ammoCount;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0000D299 File Offset: 0x0000B499
		public SetStonePileAmmo()
		{
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0000D2A4 File Offset: 0x0000B4A4
		protected override bool OnRead()
		{
			bool flag = true;
			this.StonePileId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.AmmoCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0000D2D3 File Offset: 0x0000B4D3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.StonePileId);
			GameNetworkMessage.WriteIntToPacket(this.AmmoCount, CompressionMission.RangedSiegeWeaponAmmoCompressionInfo);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0000D2F0 File Offset: 0x0000B4F0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0000D2F8 File Offset: 0x0000B4F8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set ammo left to: ", this.AmmoCount, " on StonePile with ID: ", this.StonePileId });
		}
	}
}
