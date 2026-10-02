using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C8 RID: 200
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SpawnWeaponAsDropFromAgent : GameNetworkMessage
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0000DC6E File Offset: 0x0000BE6E
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x0000DC76 File Offset: 0x0000BE76
		public int AgentIndex { get; private set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x0000DC7F File Offset: 0x0000BE7F
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x0000DC87 File Offset: 0x0000BE87
		public EquipmentIndex EquipmentIndex { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0000DC90 File Offset: 0x0000BE90
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x0000DC98 File Offset: 0x0000BE98
		public Vec3 Velocity { get; private set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0000DCA1 File Offset: 0x0000BEA1
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0000DCA9 File Offset: 0x0000BEA9
		public Vec3 AngularVelocity { get; private set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0000DCB2 File Offset: 0x0000BEB2
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x0000DCBA File Offset: 0x0000BEBA
		public Mission.WeaponSpawnFlags WeaponSpawnFlags { get; private set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0000DCC3 File Offset: 0x0000BEC3
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x0000DCCB File Offset: 0x0000BECB
		public int ForcedIndex { get; private set; }

		// Token: 0x06000823 RID: 2083 RVA: 0x0000DCD4 File Offset: 0x0000BED4
		public SpawnWeaponAsDropFromAgent(int agentIndex, EquipmentIndex equipmentIndex, Vec3 velocity, Vec3 angularVelocity, Mission.WeaponSpawnFlags weaponSpawnFlags, int forcedIndex)
		{
			this.AgentIndex = agentIndex;
			this.EquipmentIndex = equipmentIndex;
			this.Velocity = velocity;
			this.AngularVelocity = angularVelocity;
			this.WeaponSpawnFlags = weaponSpawnFlags;
			this.ForcedIndex = forcedIndex;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0000DD09 File Offset: 0x0000BF09
		public SpawnWeaponAsDropFromAgent()
		{
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0000DD14 File Offset: 0x0000BF14
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.EquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.WeaponSpawnFlags = (Mission.WeaponSpawnFlags)GameNetworkMessage.ReadUintFromPacket(CompressionMission.SpawnedItemWeaponSpawnFlagCompressionInfo, ref flag);
			if (this.WeaponSpawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics))
			{
				this.Velocity = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.SpawnedItemVelocityCompressionInfo, ref flag);
				this.AngularVelocity = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.SpawnedItemAngularVelocityCompressionInfo, ref flag);
			}
			else
			{
				this.Velocity = Vec3.Zero;
				this.AngularVelocity = Vec3.Zero;
			}
			this.ForcedIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0000DDB4 File Offset: 0x0000BFB4
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.EquipmentIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteUintToPacket((uint)this.WeaponSpawnFlags, CompressionMission.SpawnedItemWeaponSpawnFlagCompressionInfo);
			if (this.WeaponSpawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics))
			{
				GameNetworkMessage.WriteVec3ToPacket(this.Velocity, CompressionMission.SpawnedItemVelocityCompressionInfo);
				GameNetworkMessage.WriteVec3ToPacket(this.AngularVelocity, CompressionMission.SpawnedItemAngularVelocityCompressionInfo);
			}
			GameNetworkMessage.WriteIntToPacket(this.ForcedIndex, CompressionBasic.MissionObjectIDCompressionInfo);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0000DE2A File Offset: 0x0000C02A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0000DE30 File Offset: 0x0000C030
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Spawn Weapon from agent with agent-index: ", this.AgentIndex, " from equipment index: ", this.EquipmentIndex, ", and with ID: ", this.ForcedIndex });
		}
	}
}
