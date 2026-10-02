using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000091 RID: 145
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class HandleMissileCollisionReaction : GameNetworkMessage
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x0000A2C6 File Offset: 0x000084C6
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x0000A2CE File Offset: 0x000084CE
		public int MissileIndex { get; private set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0000A2D7 File Offset: 0x000084D7
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x0000A2DF File Offset: 0x000084DF
		public Mission.MissileCollisionReaction CollisionReaction { get; private set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0000A2E8 File Offset: 0x000084E8
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x0000A2F0 File Offset: 0x000084F0
		public MatrixFrame AttachLocalFrame { get; private set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0000A2F9 File Offset: 0x000084F9
		// (set) Token: 0x060005A5 RID: 1445 RVA: 0x0000A301 File Offset: 0x00008501
		public bool IsAttachedFrameLocal { get; private set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x0000A30A File Offset: 0x0000850A
		// (set) Token: 0x060005A7 RID: 1447 RVA: 0x0000A312 File Offset: 0x00008512
		public int AttackerAgentIndex { get; private set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0000A31B File Offset: 0x0000851B
		// (set) Token: 0x060005A9 RID: 1449 RVA: 0x0000A323 File Offset: 0x00008523
		public int AttachedAgentIndex { get; private set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x0000A32C File Offset: 0x0000852C
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x0000A334 File Offset: 0x00008534
		public bool AttachedToShield { get; private set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0000A33D File Offset: 0x0000853D
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x0000A345 File Offset: 0x00008545
		public sbyte AttachedBoneIndex { get; private set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0000A34E File Offset: 0x0000854E
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x0000A356 File Offset: 0x00008556
		public MissionObjectId AttachedMissionObjectId { get; private set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0000A35F File Offset: 0x0000855F
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0000A367 File Offset: 0x00008567
		public Vec3 BounceBackVelocity { get; private set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0000A370 File Offset: 0x00008570
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x0000A378 File Offset: 0x00008578
		public Vec3 BounceBackAngularVelocity { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0000A381 File Offset: 0x00008581
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x0000A389 File Offset: 0x00008589
		public int ForcedSpawnIndex { get; private set; }

		// Token: 0x060005B6 RID: 1462 RVA: 0x0000A394 File Offset: 0x00008594
		public HandleMissileCollisionReaction(int missileIndex, Mission.MissileCollisionReaction collisionReaction, MatrixFrame attachLocalFrame, bool isAttachedFrameLocal, int attackerAgentIndex, int attachedAgentIndex, bool attachedToShield, sbyte attachedBoneIndex, MissionObjectId attachedMissionObjectId, Vec3 bounceBackVelocity, Vec3 bounceBackAngularVelocity, int forcedSpawnIndex)
		{
			this.MissileIndex = missileIndex;
			this.CollisionReaction = collisionReaction;
			this.AttachLocalFrame = attachLocalFrame;
			this.IsAttachedFrameLocal = isAttachedFrameLocal;
			this.AttackerAgentIndex = attackerAgentIndex;
			this.AttachedAgentIndex = attachedAgentIndex;
			this.AttachedToShield = attachedToShield;
			this.AttachedBoneIndex = attachedBoneIndex;
			this.AttachedMissionObjectId = attachedMissionObjectId;
			this.BounceBackVelocity = bounceBackVelocity;
			this.BounceBackAngularVelocity = bounceBackAngularVelocity;
			this.ForcedSpawnIndex = forcedSpawnIndex;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0000A404 File Offset: 0x00008604
		public HandleMissileCollisionReaction()
		{
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0000A40C File Offset: 0x0000860C
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissileIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissileCompressionInfo, ref flag);
			this.CollisionReaction = (Mission.MissileCollisionReaction)GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissileCollisionReactionCompressionInfo, ref flag);
			this.AttackerAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.AttachedAgentIndex = -1;
			this.AttachedToShield = false;
			this.AttachedBoneIndex = -1;
			this.AttachedMissionObjectId = MissionObjectId.Invalid;
			if (this.CollisionReaction == Mission.MissileCollisionReaction.Stick || this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				if (GameNetworkMessage.ReadBoolFromPacket(ref flag))
				{
					this.AttachedAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
					this.AttachedToShield = GameNetworkMessage.ReadBoolFromPacket(ref flag);
					if (!this.AttachedToShield)
					{
						this.AttachedBoneIndex = (sbyte)GameNetworkMessage.ReadIntFromPacket(CompressionMission.BoneIndexCompressionInfo, ref flag);
					}
				}
				else
				{
					this.AttachedMissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
				}
			}
			if (this.CollisionReaction != Mission.MissileCollisionReaction.BecomeInvisible && this.CollisionReaction != Mission.MissileCollisionReaction.PassThrough)
			{
				this.IsAttachedFrameLocal = GameNetworkMessage.ReadBoolFromPacket(ref flag);
				if (this.IsAttachedFrameLocal)
				{
					this.AttachLocalFrame = GameNetworkMessage.ReadNonUniformTransformFromPacket(CompressionBasic.BigRangeLowResLocalPositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo, ref flag);
				}
				else
				{
					this.AttachLocalFrame = GameNetworkMessage.ReadNonUniformTransformFromPacket(CompressionBasic.PositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo, ref flag);
				}
			}
			else
			{
				this.AttachLocalFrame = MatrixFrame.Identity;
			}
			if (this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				this.BounceBackVelocity = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.SpawnedItemVelocityCompressionInfo, ref flag);
				this.BounceBackAngularVelocity = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.SpawnedItemAngularVelocityCompressionInfo, ref flag);
			}
			else
			{
				this.BounceBackVelocity = Vec3.Zero;
				this.BounceBackAngularVelocity = Vec3.Zero;
			}
			if (this.CollisionReaction == Mission.MissileCollisionReaction.Stick || this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				this.ForcedSpawnIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0000A598 File Offset: 0x00008798
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.MissileIndex, CompressionMission.MissileCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.CollisionReaction, CompressionMission.MissileCollisionReactionCompressionInfo);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AttackerAgentIndex);
			if (this.CollisionReaction == Mission.MissileCollisionReaction.Stick || this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				bool flag = this.AttachedAgentIndex >= 0;
				GameNetworkMessage.WriteBoolToPacket(flag);
				if (flag)
				{
					GameNetworkMessage.WriteAgentIndexToPacket(this.AttachedAgentIndex);
					GameNetworkMessage.WriteBoolToPacket(this.AttachedToShield);
					if (!this.AttachedToShield)
					{
						GameNetworkMessage.WriteIntToPacket((int)this.AttachedBoneIndex, CompressionMission.BoneIndexCompressionInfo);
					}
				}
				else
				{
					GameNetworkMessage.WriteMissionObjectIdToPacket((this.AttachedMissionObjectId.Id >= 0) ? this.AttachedMissionObjectId : MissionObjectId.Invalid);
				}
			}
			if (this.CollisionReaction != Mission.MissileCollisionReaction.BecomeInvisible && this.CollisionReaction != Mission.MissileCollisionReaction.PassThrough)
			{
				GameNetworkMessage.WriteBoolToPacket(this.IsAttachedFrameLocal);
				if (this.IsAttachedFrameLocal)
				{
					GameNetworkMessage.WriteNonUniformTransformToPacket(this.AttachLocalFrame, CompressionBasic.BigRangeLowResLocalPositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo);
				}
				else
				{
					GameNetworkMessage.WriteNonUniformTransformToPacket(this.AttachLocalFrame, CompressionBasic.PositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo);
				}
			}
			if (this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				GameNetworkMessage.WriteVec3ToPacket(this.BounceBackVelocity, CompressionMission.SpawnedItemVelocityCompressionInfo);
				GameNetworkMessage.WriteVec3ToPacket(this.BounceBackAngularVelocity, CompressionMission.SpawnedItemAngularVelocityCompressionInfo);
			}
			if (this.CollisionReaction == Mission.MissileCollisionReaction.Stick || this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				GameNetworkMessage.WriteIntToPacket(this.ForcedSpawnIndex, CompressionBasic.MissionObjectIDCompressionInfo);
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0000A6E0 File Offset: 0x000088E0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0000A6E4 File Offset: 0x000088E4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Handle Missile Collision with index: ",
				this.MissileIndex,
				" collision reaction: ",
				this.CollisionReaction,
				" AttackerAgent index: ",
				this.AttackerAgentIndex,
				" AttachedAgent index: ",
				this.AttachedAgentIndex,
				" AttachedToShield: ",
				this.AttachedToShield.ToString(),
				" AttachedBoneIndex: ",
				this.AttachedBoneIndex,
				" AttachedMissionObject id: ",
				(this.AttachedMissionObjectId != MissionObjectId.Invalid) ? this.AttachedMissionObjectId.Id.ToString() : "-1",
				" ForcedSpawnIndex: ",
				this.ForcedSpawnIndex
			});
		}
	}
}
