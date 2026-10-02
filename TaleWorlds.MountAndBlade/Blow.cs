using System;
using System.Runtime.InteropServices;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001ED RID: 493
	[EngineStruct("Blow", false, null)]
	public struct Blow
	{
		// Token: 0x06001CE7 RID: 7399 RVA: 0x00062571 File Offset: 0x00060771
		public Blow(int ownerId)
		{
			this = default(Blow);
			this.OwnerId = ownerId;
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x00062581 File Offset: 0x00060781
		public bool IsMissile
		{
			get
			{
				return this.WeaponRecord.IsMissile;
			}
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x0006258E File Offset: 0x0006078E
		public bool IsBlowCrit(int maxHitPointsOfVictim)
		{
			return (float)this.InflictedDamage > (float)maxHitPointsOfVictim * 0.5f;
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x000625A1 File Offset: 0x000607A1
		public bool IsBlowLow(int maxHitPointsOfVictim)
		{
			return (float)this.InflictedDamage <= (float)maxHitPointsOfVictim * 0.1f;
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000625B7 File Offset: 0x000607B7
		public bool IsHeadShot()
		{
			return this.VictimBodyPart == BoneBodyPartType.Head;
		}

		// Token: 0x040009AD RID: 2477
		public BlowWeaponRecord WeaponRecord;

		// Token: 0x040009AE RID: 2478
		public Vec3 GlobalPosition;

		// Token: 0x040009AF RID: 2479
		public Vec3 Direction;

		// Token: 0x040009B0 RID: 2480
		public Vec3 SwingDirection;

		// Token: 0x040009B1 RID: 2481
		public int InflictedDamage;

		// Token: 0x040009B2 RID: 2482
		public int SelfInflictedDamage;

		// Token: 0x040009B3 RID: 2483
		public float BaseMagnitude;

		// Token: 0x040009B4 RID: 2484
		public float DefenderStunPeriod;

		// Token: 0x040009B5 RID: 2485
		public float AttackerStunPeriod;

		// Token: 0x040009B6 RID: 2486
		public float AbsorbedByArmor;

		// Token: 0x040009B7 RID: 2487
		public float MovementSpeedDamageModifier;

		// Token: 0x040009B8 RID: 2488
		public StrikeType StrikeType;

		// Token: 0x040009B9 RID: 2489
		public AgentAttackType AttackType;

		// Token: 0x040009BA RID: 2490
		[CustomEngineStructMemberData("blow_flags")]
		public BlowFlags BlowFlag;

		// Token: 0x040009BB RID: 2491
		public int OwnerId;

		// Token: 0x040009BC RID: 2492
		public sbyte BoneIndex;

		// Token: 0x040009BD RID: 2493
		public BoneBodyPartType VictimBodyPart;

		// Token: 0x040009BE RID: 2494
		public DamageTypes DamageType;

		// Token: 0x040009BF RID: 2495
		[MarshalAs(UnmanagedType.U1)]
		public bool NoIgnore;

		// Token: 0x040009C0 RID: 2496
		[MarshalAs(UnmanagedType.U1)]
		public bool DamageCalculated;

		// Token: 0x040009C1 RID: 2497
		[MarshalAs(UnmanagedType.U1)]
		public bool IsFallDamage;

		// Token: 0x040009C2 RID: 2498
		public float DamagedPercentage;
	}
}
