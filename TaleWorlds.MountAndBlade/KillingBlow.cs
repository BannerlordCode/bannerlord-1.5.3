using System;
using System.Runtime.InteropServices;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000256 RID: 598
	[EngineStruct("Killing_blow", false, null)]
	public struct KillingBlow
	{
		// Token: 0x0600222F RID: 8751 RVA: 0x00078054 File Offset: 0x00076254
		public KillingBlow(Blow b, Vec3 ragdollImpulsePoint, Vec3 ragdollImpulseAmount, int deathAction, int weaponItemKind, Agent.KillInfo overrideKillInfo = Agent.KillInfo.Invalid)
		{
			this.RagdollImpulseLocalPoint = ragdollImpulsePoint;
			this.RagdollImpulseAmount = ragdollImpulseAmount;
			this.DeathAction = deathAction;
			this.OverrideKillInfo = overrideKillInfo;
			this.DamageType = b.DamageType;
			this.AttackType = b.AttackType;
			this.OwnerId = b.OwnerId;
			this.VictimBodyPart = b.VictimBodyPart;
			this.WeaponClass = (int)b.WeaponRecord.WeaponClass;
			this.BlowPosition = b.GlobalPosition;
			this.WeaponRecordWeaponFlags = b.WeaponRecord.WeaponFlags;
			this.WeaponItemKind = weaponItemKind;
			this.InflictedDamage = b.InflictedDamage;
			this.IsMissile = b.IsMissile;
			this.IsValid = true;
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x00078105 File Offset: 0x00076305
		public bool IsHeadShot()
		{
			return this.VictimBodyPart == BoneBodyPartType.Head;
		}

		// Token: 0x04000D20 RID: 3360
		public Vec3 RagdollImpulseLocalPoint;

		// Token: 0x04000D21 RID: 3361
		public Vec3 RagdollImpulseAmount;

		// Token: 0x04000D22 RID: 3362
		public int DeathAction;

		// Token: 0x04000D23 RID: 3363
		public DamageTypes DamageType;

		// Token: 0x04000D24 RID: 3364
		public AgentAttackType AttackType;

		// Token: 0x04000D25 RID: 3365
		public int OwnerId;

		// Token: 0x04000D26 RID: 3366
		public BoneBodyPartType VictimBodyPart;

		// Token: 0x04000D27 RID: 3367
		public int WeaponClass;

		// Token: 0x04000D28 RID: 3368
		public Agent.KillInfo OverrideKillInfo;

		// Token: 0x04000D29 RID: 3369
		public Vec3 BlowPosition;

		// Token: 0x04000D2A RID: 3370
		public WeaponFlags WeaponRecordWeaponFlags;

		// Token: 0x04000D2B RID: 3371
		public int WeaponItemKind;

		// Token: 0x04000D2C RID: 3372
		public int InflictedDamage;

		// Token: 0x04000D2D RID: 3373
		[MarshalAs(UnmanagedType.U1)]
		public bool IsMissile;

		// Token: 0x04000D2E RID: 3374
		[MarshalAs(UnmanagedType.U1)]
		public bool IsValid;
	}
}
