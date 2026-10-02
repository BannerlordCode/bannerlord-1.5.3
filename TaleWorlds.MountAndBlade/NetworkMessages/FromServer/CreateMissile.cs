using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000089 RID: 137
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateMissile : GameNetworkMessage
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x000099D1 File Offset: 0x00007BD1
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x000099D9 File Offset: 0x00007BD9
		public int MissileIndex { get; private set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x000099E2 File Offset: 0x00007BE2
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x000099EA File Offset: 0x00007BEA
		public int AgentIndex { get; private set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x000099F3 File Offset: 0x00007BF3
		// (set) Token: 0x06000546 RID: 1350 RVA: 0x000099FB File Offset: 0x00007BFB
		public EquipmentIndex WeaponIndex { get; private set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x00009A04 File Offset: 0x00007C04
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x00009A0C File Offset: 0x00007C0C
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00009A15 File Offset: 0x00007C15
		// (set) Token: 0x0600054A RID: 1354 RVA: 0x00009A1D File Offset: 0x00007C1D
		public Vec3 Position { get; private set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x00009A26 File Offset: 0x00007C26
		// (set) Token: 0x0600054C RID: 1356 RVA: 0x00009A2E File Offset: 0x00007C2E
		public Vec3 Direction { get; private set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x00009A37 File Offset: 0x00007C37
		// (set) Token: 0x0600054E RID: 1358 RVA: 0x00009A3F File Offset: 0x00007C3F
		public float Speed { get; private set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x00009A48 File Offset: 0x00007C48
		// (set) Token: 0x06000550 RID: 1360 RVA: 0x00009A50 File Offset: 0x00007C50
		public Mat3 Orientation { get; private set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x00009A59 File Offset: 0x00007C59
		// (set) Token: 0x06000552 RID: 1362 RVA: 0x00009A61 File Offset: 0x00007C61
		public bool HasRigidBody { get; private set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x00009A6A File Offset: 0x00007C6A
		// (set) Token: 0x06000554 RID: 1364 RVA: 0x00009A72 File Offset: 0x00007C72
		public MissionObjectId MissionObjectToIgnoreId { get; private set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00009A7B File Offset: 0x00007C7B
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x00009A83 File Offset: 0x00007C83
		public bool IsPrimaryWeaponShot { get; private set; }

		// Token: 0x06000557 RID: 1367 RVA: 0x00009A8C File Offset: 0x00007C8C
		public CreateMissile(int missileIndex, int agentIndex, EquipmentIndex weaponIndex, MissionWeapon weapon, Vec3 position, Vec3 direction, float speed, Mat3 orientation, bool hasRigidBody, MissionObjectId missionObjectToIgnoreId, bool isPrimaryWeaponShot)
		{
			this.MissileIndex = missileIndex;
			this.AgentIndex = agentIndex;
			this.WeaponIndex = weaponIndex;
			this.Weapon = weapon;
			this.Position = position;
			this.Direction = direction;
			this.Speed = speed;
			this.Orientation = orientation;
			this.HasRigidBody = hasRigidBody;
			this.MissionObjectToIgnoreId = missionObjectToIgnoreId;
			this.IsPrimaryWeaponShot = isPrimaryWeaponShot;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00009AF4 File Offset: 0x00007CF4
		public CreateMissile()
		{
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00009AFC File Offset: 0x00007CFC
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissileIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissileCompressionInfo, ref flag);
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.WeaponIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WieldSlotCompressionInfo, ref flag);
			if (this.WeaponIndex == EquipmentIndex.None)
			{
				this.Weapon = ModuleNetworkData.ReadMissileWeaponReferenceFromPacket(Game.Current.ObjectManager, ref flag);
			}
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.Direction = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			this.Speed = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.MissileSpeedCompressionInfo, ref flag);
			this.HasRigidBody = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			if (this.HasRigidBody)
			{
				this.Orientation = GameNetworkMessage.ReadRotationMatrixFromPacket(ref flag);
				this.MissionObjectToIgnoreId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			}
			else
			{
				Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
				this.Orientation = new Mat3(in Vec3.Side, in vec, in Vec3.Up);
				this.Orientation.Orthonormalize();
				this.MissionObjectToIgnoreId = MissionObjectId.Invalid;
			}
			this.IsPrimaryWeaponShot = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00009C10 File Offset: 0x00007E10
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.MissileIndex, CompressionMission.MissileCompressionInfo);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.WeaponIndex, CompressionMission.WieldSlotCompressionInfo);
			if (this.WeaponIndex == EquipmentIndex.None)
			{
				ModuleNetworkData.WriteMissileWeaponReferenceToPacket(this.Weapon);
			}
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Direction, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Speed, CompressionMission.MissileSpeedCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.HasRigidBody);
			if (this.HasRigidBody)
			{
				GameNetworkMessage.WriteRotationMatrixToPacket(this.Orientation);
				GameNetworkMessage.WriteMissionObjectIdToPacket((this.MissionObjectToIgnoreId.Id >= 0) ? this.MissionObjectToIgnoreId : MissionObjectId.Invalid);
			}
			else
			{
				GameNetworkMessage.WriteVec3ToPacket(this.Orientation.f, CompressionBasic.UnitVectorCompressionInfo);
			}
			GameNetworkMessage.WriteBoolToPacket(this.IsPrimaryWeaponShot);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00009CEC File Offset: 0x00007EEC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00009CF4 File Offset: 0x00007EF4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Create a missile with index: ", this.MissileIndex, " on agent with agent-index: ", this.AgentIndex });
		}
	}
}
