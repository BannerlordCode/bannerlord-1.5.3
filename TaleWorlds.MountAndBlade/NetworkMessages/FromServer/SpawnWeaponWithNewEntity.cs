using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C9 RID: 201
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SpawnWeaponWithNewEntity : GameNetworkMessage
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x0000DE8A File Offset: 0x0000C08A
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x0000DE92 File Offset: 0x0000C092
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0000DE9B File Offset: 0x0000C09B
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x0000DEA3 File Offset: 0x0000C0A3
		public Mission.WeaponSpawnFlags WeaponSpawnFlags { get; private set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x0000DEB4 File Offset: 0x0000C0B4
		public int ForcedIndex { get; private set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0000DEBD File Offset: 0x0000C0BD
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0000DEC5 File Offset: 0x0000C0C5
		public MatrixFrame Frame { get; private set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0000DECE File Offset: 0x0000C0CE
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0000DED6 File Offset: 0x0000C0D6
		public MissionObjectId ParentMissionObjectId { get; private set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0000DEDF File Offset: 0x0000C0DF
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0000DEE7 File Offset: 0x0000C0E7
		public bool IsVisible { get; private set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0000DEF0 File Offset: 0x0000C0F0
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x0000DEF8 File Offset: 0x0000C0F8
		public bool HasLifeTime { get; private set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x0000DF01 File Offset: 0x0000C101
		// (set) Token: 0x06000838 RID: 2104 RVA: 0x0000DF09 File Offset: 0x0000C109
		public bool SpawnedOnACorpse { get; private set; }

		// Token: 0x06000839 RID: 2105 RVA: 0x0000DF14 File Offset: 0x0000C114
		public SpawnWeaponWithNewEntity(MissionWeapon weapon, Mission.WeaponSpawnFlags weaponSpawnFlags, int forcedIndex, MatrixFrame frame, MissionObjectId parentMissionObjectId, bool isVisible, bool hasLifeTime, bool spawnedOnACorpse)
		{
			this.Weapon = weapon;
			this.WeaponSpawnFlags = weaponSpawnFlags;
			this.ForcedIndex = forcedIndex;
			this.Frame = frame;
			this.ParentMissionObjectId = parentMissionObjectId;
			this.IsVisible = isVisible;
			this.HasLifeTime = hasLifeTime;
			this.SpawnedOnACorpse = spawnedOnACorpse;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0000DF64 File Offset: 0x0000C164
		public SpawnWeaponWithNewEntity()
		{
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0000DF6C File Offset: 0x0000C16C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Weapon = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			this.Frame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref flag);
			this.WeaponSpawnFlags = (Mission.WeaponSpawnFlags)GameNetworkMessage.ReadUintFromPacket(CompressionMission.SpawnedItemWeaponSpawnFlagCompressionInfo, ref flag);
			this.ForcedIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			this.ParentMissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.IsVisible = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.HasLifeTime = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.SpawnedOnACorpse = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0000DFF4 File Offset: 0x0000C1F4
		protected override void OnWrite()
		{
			ModuleNetworkData.WriteWeaponReferenceToPacket(this.Weapon);
			GameNetworkMessage.WriteMatrixFrameToPacket(this.Frame);
			GameNetworkMessage.WriteUintToPacket((uint)this.WeaponSpawnFlags, CompressionMission.SpawnedItemWeaponSpawnFlagCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ForcedIndex, CompressionBasic.MissionObjectIDCompressionInfo);
			GameNetworkMessage.WriteMissionObjectIdToPacket((this.ParentMissionObjectId.Id >= 0) ? this.ParentMissionObjectId : MissionObjectId.Invalid);
			GameNetworkMessage.WriteBoolToPacket(this.IsVisible);
			GameNetworkMessage.WriteBoolToPacket(this.HasLifeTime);
			GameNetworkMessage.WriteBoolToPacket(this.SpawnedOnACorpse);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0000E078 File Offset: 0x0000C278
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0000E07C File Offset: 0x0000C27C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Spawn Weapon with name: ",
				this.Weapon.Item.Name,
				", and with ID: ",
				this.ForcedIndex
			});
		}
	}
}
