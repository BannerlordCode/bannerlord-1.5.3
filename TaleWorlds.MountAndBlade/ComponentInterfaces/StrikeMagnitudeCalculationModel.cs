using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003FE RID: 1022
	public abstract class StrikeMagnitudeCalculationModel : MBGameModel<StrikeMagnitudeCalculationModel>
	{
		// Token: 0x0600382A RID: 14378
		public abstract float CalculateStrikeMagnitudeForMissile(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float missileSpeed);

		// Token: 0x0600382B RID: 14379
		public abstract float CalculateStrikeMagnitudeForSwing(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float swingSpeed, float impactPointAsPercent, float extraLinearSpeed);

		// Token: 0x0600382C RID: 14380
		public abstract float CalculateStrikeMagnitudeForThrust(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float thrustSpeed, float extraLinearSpeed, bool isThrown = false);

		// Token: 0x0600382D RID: 14381
		public abstract float CalculateBaseBlowMagnitudeForPassiveUsage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float extraLinearSpeed);

		// Token: 0x0600382E RID: 14382
		public abstract float ComputeRawDamage(DamageTypes damageType, float magnitude, float armorEffectiveness, float absorbedDamageRatio);

		// Token: 0x0600382F RID: 14383
		public abstract float CalculateStrikeMagnitudeForUnarmedAttack(in AttackInformation attackInformation, in AttackCollisionData collisionData, float progressEffect, float momentumRemaining);

		// Token: 0x06003830 RID: 14384
		public abstract float GetBluntDamageFactorByDamageType(DamageTypes damageType);

		// Token: 0x06003831 RID: 14385
		public abstract float CalculateHorseArcheryFactor(BasicCharacterObject characterObject);

		// Token: 0x06003832 RID: 14386 RVA: 0x000E932A File Offset: 0x000E752A
		public virtual float CalculateAdjustedArmorForBlow(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseArmor, BasicCharacterObject attackerCharacter, BasicCharacterObject attackerCaptainCharacter, BasicCharacterObject victimCharacter, BasicCharacterObject victimCaptainCharacter, WeaponComponentData weaponComponent)
		{
			return baseArmor;
		}
	}
}
