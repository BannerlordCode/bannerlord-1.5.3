using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200019A RID: 410
	[EngineStruct("Attack_collision_data", false, null)]
	public struct AttackCollisionData
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001577 RID: 5495 RVA: 0x000506D0 File Offset: 0x0004E8D0
		public bool AttackBlockedWithShield
		{
			get
			{
				return this._attackBlockedWithShield;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x000506D8 File Offset: 0x0004E8D8
		public bool CorrectSideShieldBlock
		{
			get
			{
				return this._correctSideShieldBlock;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001579 RID: 5497 RVA: 0x000506E0 File Offset: 0x0004E8E0
		public bool IsAlternativeAttack
		{
			get
			{
				return this._isAlternativeAttack;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x000506E8 File Offset: 0x0004E8E8
		public bool IsColliderAgent
		{
			get
			{
				return this._isColliderAgent;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x000506F0 File Offset: 0x0004E8F0
		public bool CollidedWithShieldOnBack
		{
			get
			{
				return this._collidedWithShieldOnBack;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x0600157C RID: 5500 RVA: 0x000506F8 File Offset: 0x0004E8F8
		public bool IsMissile
		{
			get
			{
				return this._isMissile;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x0600157D RID: 5501 RVA: 0x00050700 File Offset: 0x0004E900
		public bool MissileBlockedWithWeapon
		{
			get
			{
				return this._missileBlockedWithWeapon;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x0600157E RID: 5502 RVA: 0x00050708 File Offset: 0x0004E908
		public bool MissileHasPhysics
		{
			get
			{
				return this._missileHasPhysics;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x0600157F RID: 5503 RVA: 0x00050710 File Offset: 0x0004E910
		public bool EntityExists
		{
			get
			{
				return this._entityExists;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x00050718 File Offset: 0x0004E918
		public bool ThrustTipHit
		{
			get
			{
				return this._thrustTipHit;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001581 RID: 5505 RVA: 0x00050720 File Offset: 0x0004E920
		public bool MissileGoneUnderWater
		{
			get
			{
				return this._missileGoneUnderWater;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x00050728 File Offset: 0x0004E928
		public bool MissileGoneOutOfBorder
		{
			get
			{
				return this._missileGoneOutOfBorder;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001583 RID: 5507 RVA: 0x00050730 File Offset: 0x0004E930
		public bool CollidedWithLastBoneSegment
		{
			get
			{
				return this._collidedWithLastBoneSegment;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x00050738 File Offset: 0x0004E938
		public bool IsHorseCharge
		{
			get
			{
				return this.ChargeVelocity > 0f;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x00050747 File Offset: 0x0004E947
		public bool IsFallDamage
		{
			get
			{
				return this.FallSpeed > 0f;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x00050756 File Offset: 0x0004E956
		public CombatCollisionResult CollisionResult
		{
			get
			{
				return (CombatCollisionResult)this._collisionResult;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001587 RID: 5511 RVA: 0x0005075E File Offset: 0x0004E95E
		public int AffectorWeaponSlotOrMissileIndex { get; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x00050766 File Offset: 0x0004E966
		public int StrikeType { get; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001589 RID: 5513 RVA: 0x0005076E File Offset: 0x0004E96E
		public int DamageType { get; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x00050776 File Offset: 0x0004E976
		// (set) Token: 0x0600158B RID: 5515 RVA: 0x0005077E File Offset: 0x0004E97E
		public sbyte CollisionBoneIndex { get; private set; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x00050787 File Offset: 0x0004E987
		public BoneBodyPartType VictimHitBodyPart { get; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600158D RID: 5517 RVA: 0x0005078F File Offset: 0x0004E98F
		// (set) Token: 0x0600158E RID: 5518 RVA: 0x00050797 File Offset: 0x0004E997
		public sbyte AttackBoneIndex { get; private set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x0600158F RID: 5519 RVA: 0x000507A0 File Offset: 0x0004E9A0
		public Agent.UsageDirection AttackDirection { get; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x000507A8 File Offset: 0x0004E9A8
		// (set) Token: 0x06001591 RID: 5521 RVA: 0x000507B0 File Offset: 0x0004E9B0
		public int PhysicsMaterialIndex { get; private set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001592 RID: 5522 RVA: 0x000507B9 File Offset: 0x0004E9B9
		// (set) Token: 0x06001593 RID: 5523 RVA: 0x000507C1 File Offset: 0x0004E9C1
		public CombatHitResultFlags CollisionHitResultFlags { get; private set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001594 RID: 5524 RVA: 0x000507CA File Offset: 0x0004E9CA
		public float AttackProgress { get; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001595 RID: 5525 RVA: 0x000507D2 File Offset: 0x0004E9D2
		public float CollisionDistanceOnWeapon { get; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x000507DA File Offset: 0x0004E9DA
		// (set) Token: 0x06001597 RID: 5527 RVA: 0x000507E2 File Offset: 0x0004E9E2
		public float AttackerStunPeriod { get; set; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001598 RID: 5528 RVA: 0x000507EB File Offset: 0x0004E9EB
		// (set) Token: 0x06001599 RID: 5529 RVA: 0x000507F3 File Offset: 0x0004E9F3
		public float DefenderStunPeriod { get; set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x0600159A RID: 5530 RVA: 0x000507FC File Offset: 0x0004E9FC
		public float MissileTotalDamage { get; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600159B RID: 5531 RVA: 0x00050804 File Offset: 0x0004EA04
		public float MissileStartingBaseSpeed { get; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600159C RID: 5532 RVA: 0x0005080C File Offset: 0x0004EA0C
		public float ChargeVelocity { get; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x0600159D RID: 5533 RVA: 0x00050814 File Offset: 0x0004EA14
		// (set) Token: 0x0600159E RID: 5534 RVA: 0x0005081C File Offset: 0x0004EA1C
		public float FallSpeed { get; private set; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x0600159F RID: 5535 RVA: 0x00050825 File Offset: 0x0004EA25
		public Vec3 WeaponRotUp { get; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060015A0 RID: 5536 RVA: 0x0005082D File Offset: 0x0004EA2D
		public Vec3 WeaponBlowDir
		{
			get
			{
				return this._weaponBlowDir;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060015A1 RID: 5537 RVA: 0x00050835 File Offset: 0x0004EA35
		// (set) Token: 0x060015A2 RID: 5538 RVA: 0x0005083D File Offset: 0x0004EA3D
		public Vec3 CollisionGlobalPosition { get; private set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060015A3 RID: 5539 RVA: 0x00050846 File Offset: 0x0004EA46
		public Vec3 MissileVelocity { get; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x0005084E File Offset: 0x0004EA4E
		public Vec3 MissileStartingPosition { get; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x00050856 File Offset: 0x0004EA56
		public Vec3 VictimAgentCurVelocity { get; }

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x0005085E File Offset: 0x0004EA5E
		public Vec3 CollisionGlobalNormal { get; }

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x00050866 File Offset: 0x0004EA66
		public Vec3 LastBoneSegmentRotUp { get; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x0005086E File Offset: 0x0004EA6E
		public Vec3 LastBoneSegmentSwingDir { get; }

		// Token: 0x060015A9 RID: 5545 RVA: 0x00050876 File Offset: 0x0004EA76
		public void SetCollisionBoneIndexForAreaDamage(sbyte boneIndex)
		{
			this.CollisionBoneIndex = boneIndex;
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0005087F File Offset: 0x0004EA7F
		public void UpdateCollisionPositionAndBoneForReflect(int inflictedDamage, Vec3 position, sbyte boneIndex)
		{
			this.InflictedDamage = inflictedDamage;
			this.CollisionGlobalPosition = position;
			this.AttackBoneIndex = boneIndex;
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00050898 File Offset: 0x0004EA98
		private AttackCollisionData(bool attackBlockedWithShield, bool correctSideShieldBlock, bool isAlternativeAttack, bool isColliderAgent, bool collidedWithShieldOnBack, bool isMissile, bool missileBlockedWithWeapon, bool missileHasPhysics, bool entityExists, bool thrustTipHit, bool missileGoneUnderWater, bool missileGoneOutOfBorder, bool collidedWithLastBoneSegment, CombatCollisionResult collisionResult, int affectorWeaponSlotOrMissileIndex, int StrikeType, int DamageType, sbyte CollisionBoneIndex, BoneBodyPartType VictimHitBodyPart, sbyte AttackBoneIndex, Agent.UsageDirection AttackDirection, int PhysicsMaterialIndex, CombatHitResultFlags CollisionHitResultFlags, float AttackProgress, float CollisionDistanceOnWeapon, float AttackerStunPeriod, float DefenderStunPeriod, float MissileTotalDamage, float MissileStartingBaseSpeed, float ChargeVelocity, float FallSpeed, Vec3 WeaponRotUp, Vec3 weaponBlowDir, Vec3 CollisionGlobalPosition, Vec3 MissileVelocity, Vec3 MissileStartingPosition, Vec3 VictimAgentCurVelocity, Vec3 GroundNormal, Vec3 LastBoneSegmentRotUp, Vec3 LastBoneSegmentSwingDir)
		{
			this._attackBlockedWithShield = attackBlockedWithShield;
			this._correctSideShieldBlock = correctSideShieldBlock;
			this._isAlternativeAttack = isAlternativeAttack;
			this._isColliderAgent = isColliderAgent;
			this._collidedWithShieldOnBack = collidedWithShieldOnBack;
			this._isMissile = isMissile;
			this._missileBlockedWithWeapon = missileBlockedWithWeapon;
			this._missileHasPhysics = missileHasPhysics;
			this._entityExists = entityExists;
			this._thrustTipHit = thrustTipHit;
			this._missileGoneUnderWater = missileGoneUnderWater;
			this._missileGoneOutOfBorder = missileGoneOutOfBorder;
			this._collidedWithLastBoneSegment = collidedWithLastBoneSegment;
			this._collisionResult = (int)collisionResult;
			this.AffectorWeaponSlotOrMissileIndex = affectorWeaponSlotOrMissileIndex;
			this.StrikeType = StrikeType;
			this.DamageType = DamageType;
			this.CollisionBoneIndex = CollisionBoneIndex;
			this.VictimHitBodyPart = VictimHitBodyPart;
			this.AttackBoneIndex = AttackBoneIndex;
			this.AttackDirection = AttackDirection;
			this.PhysicsMaterialIndex = PhysicsMaterialIndex;
			this.CollisionHitResultFlags = CollisionHitResultFlags;
			this.AttackProgress = AttackProgress;
			this.CollisionDistanceOnWeapon = CollisionDistanceOnWeapon;
			this.AttackerStunPeriod = AttackerStunPeriod;
			this.DefenderStunPeriod = DefenderStunPeriod;
			this.MissileTotalDamage = MissileTotalDamage;
			this.MissileStartingBaseSpeed = MissileStartingBaseSpeed;
			this.ChargeVelocity = ChargeVelocity;
			this.FallSpeed = FallSpeed;
			this.WeaponRotUp = WeaponRotUp;
			this._weaponBlowDir = weaponBlowDir;
			this.CollisionGlobalPosition = CollisionGlobalPosition;
			this.MissileVelocity = MissileVelocity;
			this.MissileStartingPosition = MissileStartingPosition;
			this.VictimAgentCurVelocity = VictimAgentCurVelocity;
			this.CollisionGlobalNormal = GroundNormal;
			this.LastBoneSegmentRotUp = LastBoneSegmentRotUp;
			this.LastBoneSegmentSwingDir = LastBoneSegmentSwingDir;
			this.BaseMagnitude = 0f;
			this.MovementSpeedDamageModifier = 0f;
			this.AbsorbedByArmor = 0;
			this.InflictedDamage = 0;
			this.SelfInflictedDamage = 0;
			this.IsShieldBroken = false;
			this.IsSneakAttack = false;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x00050A1C File Offset: 0x0004EC1C
		public static AttackCollisionData GetAttackCollisionDataForDebugPurpose(bool _attackBlockedWithShield, bool _correctSideShieldBlock, bool _isAlternativeAttack, bool _isColliderAgent, bool _collidedWithShieldOnBack, bool _isMissile, bool _isMissileBlockedWithWeapon, bool _missileHasPhysics, bool _entityExists, bool _thrustTipHit, bool _missileGoneUnderWater, bool _missileGoneOutOfBorder, CombatCollisionResult collisionResult, int affectorWeaponSlotOrMissileIndex, int StrikeType, int DamageType, sbyte CollisionBoneIndex, BoneBodyPartType VictimHitBodyPart, sbyte AttackBoneIndex, Agent.UsageDirection AttackDirection, int PhysicsMaterialIndex, CombatHitResultFlags CollisionHitResultFlags, float AttackProgress, float CollisionDistanceOnWeapon, float AttackerStunPeriod, float DefenderStunPeriod, float MissileTotalDamage, float MissileInitialSpeed, float ChargeVelocity, float FallSpeed, Vec3 WeaponRotUp, Vec3 _weaponBlowDir, Vec3 CollisionGlobalPosition, Vec3 MissileVelocity, Vec3 MissileStartingPosition, Vec3 VictimAgentCurVelocity, Vec3 GroundNormal)
		{
			return new AttackCollisionData(_attackBlockedWithShield, _correctSideShieldBlock, _isAlternativeAttack, _isColliderAgent, _collidedWithShieldOnBack, _isMissile, _isMissileBlockedWithWeapon, _missileHasPhysics, _entityExists, _thrustTipHit, _missileGoneUnderWater, _missileGoneOutOfBorder, false, collisionResult, affectorWeaponSlotOrMissileIndex, StrikeType, DamageType, CollisionBoneIndex, VictimHitBodyPart, AttackBoneIndex, AttackDirection, PhysicsMaterialIndex, CollisionHitResultFlags, AttackProgress, CollisionDistanceOnWeapon, AttackerStunPeriod, DefenderStunPeriod, MissileTotalDamage, MissileInitialSpeed, ChargeVelocity, FallSpeed, WeaponRotUp, _weaponBlowDir, CollisionGlobalPosition, MissileVelocity, MissileStartingPosition, VictimAgentCurVelocity, GroundNormal, Vec3.Zero, Vec3.Zero);
		}

		// Token: 0x04000634 RID: 1588
		[MarshalAs(UnmanagedType.U1)]
		private bool _attackBlockedWithShield;

		// Token: 0x04000635 RID: 1589
		[MarshalAs(UnmanagedType.U1)]
		private bool _correctSideShieldBlock;

		// Token: 0x04000636 RID: 1590
		[MarshalAs(UnmanagedType.U1)]
		private bool _isAlternativeAttack;

		// Token: 0x04000637 RID: 1591
		[MarshalAs(UnmanagedType.U1)]
		private bool _isColliderAgent;

		// Token: 0x04000638 RID: 1592
		[MarshalAs(UnmanagedType.U1)]
		private bool _collidedWithShieldOnBack;

		// Token: 0x04000639 RID: 1593
		[MarshalAs(UnmanagedType.U1)]
		private bool _isMissile;

		// Token: 0x0400063A RID: 1594
		[MarshalAs(UnmanagedType.U1)]
		private bool _missileBlockedWithWeapon;

		// Token: 0x0400063B RID: 1595
		[MarshalAs(UnmanagedType.U1)]
		private bool _missileHasPhysics;

		// Token: 0x0400063C RID: 1596
		[MarshalAs(UnmanagedType.U1)]
		private bool _entityExists;

		// Token: 0x0400063D RID: 1597
		[MarshalAs(UnmanagedType.U1)]
		private bool _thrustTipHit;

		// Token: 0x0400063E RID: 1598
		[MarshalAs(UnmanagedType.U1)]
		private bool _missileGoneUnderWater;

		// Token: 0x0400063F RID: 1599
		[MarshalAs(UnmanagedType.U1)]
		private bool _missileGoneOutOfBorder;

		// Token: 0x04000640 RID: 1600
		[MarshalAs(UnmanagedType.U1)]
		private bool _collidedWithLastBoneSegment;

		// Token: 0x04000641 RID: 1601
		private int _collisionResult;

		// Token: 0x04000654 RID: 1620
		private Vec3 _weaponBlowDir;

		// Token: 0x0400065C RID: 1628
		[CustomEngineStructMemberData(true)]
		public float BaseMagnitude;

		// Token: 0x0400065D RID: 1629
		[CustomEngineStructMemberData(true)]
		public float MovementSpeedDamageModifier;

		// Token: 0x0400065E RID: 1630
		[CustomEngineStructMemberData(true)]
		public int AbsorbedByArmor;

		// Token: 0x0400065F RID: 1631
		[CustomEngineStructMemberData(true)]
		public int InflictedDamage;

		// Token: 0x04000660 RID: 1632
		[CustomEngineStructMemberData(true)]
		public int SelfInflictedDamage;

		// Token: 0x04000661 RID: 1633
		[CustomEngineStructMemberData(true)]
		[MarshalAs(UnmanagedType.U1)]
		public bool IsShieldBroken;

		// Token: 0x04000662 RID: 1634
		[CustomEngineStructMemberData(true)]
		[MarshalAs(UnmanagedType.U1)]
		public bool IsSneakAttack;
	}
}
