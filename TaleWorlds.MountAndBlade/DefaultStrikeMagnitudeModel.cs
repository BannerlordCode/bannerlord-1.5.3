using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000206 RID: 518
	public class DefaultStrikeMagnitudeModel : StrikeMagnitudeCalculationModel
	{
		// Token: 0x06001E2D RID: 7725 RVA: 0x00067224 File Offset: 0x00065424
		public override float CalculateStrikeMagnitudeForMissile(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float missileSpeed)
		{
			AttackCollisionData attackCollisionData = collisionData;
			float missileTotalDamage = attackCollisionData.MissileTotalDamage;
			attackCollisionData = collisionData;
			float missileStartingBaseSpeed = attackCollisionData.MissileStartingBaseSpeed;
			float num = missileSpeed / missileStartingBaseSpeed;
			return num * num * missileTotalDamage;
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x00067258 File Offset: 0x00065458
		public override float CalculateStrikeMagnitudeForSwing(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float swingSpeed, float impactPointAsPercent, float extraLinearSpeed)
		{
			MissionWeapon missionWeapon = weapon;
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			missionWeapon = weapon;
			return CombatStatCalculator.CalculateStrikeMagnitudeForSwing(swingSpeed, impactPointAsPercent, missionWeapon.Item.Weight, currentUsageItem.GetRealWeaponLength(), currentUsageItem.TotalInertia, currentUsageItem.CenterOfMass, extraLinearSpeed);
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x000672A4 File Offset: 0x000654A4
		public override float CalculateStrikeMagnitudeForUnarmedAttack(in AttackInformation attackInformation, in AttackCollisionData collisionData, float progressEffect, float momentumRemaining)
		{
			return momentumRemaining * progressEffect * ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.FistFightDamageMultiplier);
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x000672B8 File Offset: 0x000654B8
		public override float CalculateStrikeMagnitudeForThrust(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float thrustWeaponSpeed, float extraLinearSpeed, bool isThrown = false)
		{
			MissionWeapon missionWeapon = weapon;
			return CombatStatCalculator.CalculateStrikeMagnitudeForThrust(thrustWeaponSpeed, missionWeapon.Item.Weight, extraLinearSpeed, isThrown);
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x000672E4 File Offset: 0x000654E4
		public override float ComputeRawDamage(DamageTypes damageType, float magnitude, float armorEffectiveness, float absorbedDamageRatio)
		{
			float bluntDamageFactorByDamageType = this.GetBluntDamageFactorByDamageType(damageType);
			float num = 50f / (50f + armorEffectiveness);
			float num2 = magnitude * num;
			float num3 = bluntDamageFactorByDamageType * num2;
			float num4;
			switch (damageType)
			{
			case DamageTypes.Cut:
				num4 = MathF.Max(0f, num2 - armorEffectiveness * 0.5f);
				break;
			case DamageTypes.Pierce:
				num4 = MathF.Max(0f, num2 - armorEffectiveness * 0.33f);
				break;
			case DamageTypes.Blunt:
				num4 = MathF.Max(0f, num2 - armorEffectiveness * 0.2f);
				break;
			default:
				Debug.FailedAssert("Given damage type is invalid.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\ComponentInterfaces\\DefaultStrikeMagnitudeModel.cs", "ComputeRawDamage", 70);
				return 0f;
			}
			num3 += (1f - bluntDamageFactorByDamageType) * num4;
			return num3 * absorbedDamageRatio;
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x00067398 File Offset: 0x00065598
		public override float GetBluntDamageFactorByDamageType(DamageTypes damageType)
		{
			float num = 0f;
			switch (damageType)
			{
			case DamageTypes.Cut:
				num = 0.1f;
				break;
			case DamageTypes.Pierce:
				num = 0.25f;
				break;
			case DamageTypes.Blunt:
				num = 0.6f;
				break;
			}
			return num;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000673D6 File Offset: 0x000655D6
		public override float CalculateHorseArcheryFactor(BasicCharacterObject characterObject)
		{
			return 100f;
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000673E0 File Offset: 0x000655E0
		public override float CalculateBaseBlowMagnitudeForPassiveUsage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float extraLinearSpeed)
		{
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			return CombatStatCalculator.CalculateBaseBlowMagnitudeForPassiveUsage(attackerWeapon.Item.Weight, extraLinearSpeed);
		}
	}
}
