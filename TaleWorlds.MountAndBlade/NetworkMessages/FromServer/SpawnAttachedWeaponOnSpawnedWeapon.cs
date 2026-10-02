using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C7 RID: 199
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SpawnAttachedWeaponOnSpawnedWeapon : GameNetworkMessage
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x0000DB47 File Offset: 0x0000BD47
		// (set) Token: 0x0600080C RID: 2060 RVA: 0x0000DB4F File Offset: 0x0000BD4F
		public MissionObjectId SpawnedWeaponId { get; private set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0000DB58 File Offset: 0x0000BD58
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x0000DB60 File Offset: 0x0000BD60
		public int AttachmentIndex { get; private set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0000DB69 File Offset: 0x0000BD69
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x0000DB71 File Offset: 0x0000BD71
		public int ForcedIndex { get; private set; }

		// Token: 0x06000811 RID: 2065 RVA: 0x0000DB7A File Offset: 0x0000BD7A
		public SpawnAttachedWeaponOnSpawnedWeapon(MissionObjectId spawnedWeaponId, int attachmentIndex, int forcedIndex)
		{
			this.SpawnedWeaponId = spawnedWeaponId;
			this.AttachmentIndex = attachmentIndex;
			this.ForcedIndex = forcedIndex;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0000DB97 File Offset: 0x0000BD97
		public SpawnAttachedWeaponOnSpawnedWeapon()
		{
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0000DBA0 File Offset: 0x0000BDA0
		protected override bool OnRead()
		{
			bool flag = true;
			this.SpawnedWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.AttachmentIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponAttachmentIndexCompressionInfo, ref flag);
			this.ForcedIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0000DBE1 File Offset: 0x0000BDE1
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SpawnedWeaponId);
			GameNetworkMessage.WriteIntToPacket(this.AttachmentIndex, CompressionMission.WeaponAttachmentIndexCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ForcedIndex, CompressionBasic.MissionObjectIDCompressionInfo);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0000DC0E File Offset: 0x0000BE0E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0000DC14 File Offset: 0x0000BE14
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "SpawnAttachedWeaponOnSpawnedWeapon with Spawned Weapon ID: ", this.SpawnedWeaponId, " AttachmentIndex: ", this.AttachmentIndex, " Attached Weapon ID: ", this.ForcedIndex });
		}
	}
}
