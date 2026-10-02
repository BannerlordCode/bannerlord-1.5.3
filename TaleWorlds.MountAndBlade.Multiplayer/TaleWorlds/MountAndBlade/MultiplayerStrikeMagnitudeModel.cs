using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000005 RID: 5
	public class MultiplayerStrikeMagnitudeModel : StrikeMagnitudeCalculationModel
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020C4 File Offset: 0x000002C4
		public override float CalculateStrikeMagnitudeForMissile(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float missileSpeed)
		{
			AttackCollisionData attackCollisionData = collisionData;
			float missileTotalDamage = attackCollisionData.MissileTotalDamage;
			attackCollisionData = collisionData;
			float missileStartingBaseSpeed = attackCollisionData.MissileStartingBaseSpeed;
			float num = missileSpeed;
			float num2 = missileSpeed - missileStartingBaseSpeed;
			if (num2 > 0f)
			{
				MPPerkObject.MPCombatPerkHandler combatPerkHandler = MPPerkObject.GetCombatPerkHandler(attackInformation.AttackerAgent, attackInformation.VictimAgent);
				if (combatPerkHandler != null)
				{
					float num3 = 1f;
					MPPerkObject.MPCombatPerkHandler mpcombatPerkHandler = combatPerkHandler;
					MissionWeapon missionWeapon = weapon;
					WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
					attackCollisionData = collisionData;
					float num4 = MathF.Max(MathF.Sqrt(num3 + mpcombatPerkHandler.GetSpeedBonusEffectiveness(currentUsageItem, (DamageTypes)attackCollisionData.DamageType)) - 1f, 0f);
					num += num2 * num4;
				}
			}
			num /= missileStartingBaseSpeed;
			return num * num * missileTotalDamage;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000216C File Offset: 0x0000036C
		public override float CalculateStrikeMagnitudeForSwing(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float swingSpeed, float impactPoint, float extraLinearSpeed)
		{
			float num = extraLinearSpeed;
			MissionWeapon missionWeapon;
			if (extraLinearSpeed > 0f)
			{
				MPPerkObject.MPCombatPerkHandler combatPerkHandler = MPPerkObject.GetCombatPerkHandler(attackInformation.AttackerAgent, attackInformation.VictimAgent);
				if (combatPerkHandler != null)
				{
					float num2 = 1f;
					MPPerkObject.MPCombatPerkHandler mpcombatPerkHandler = combatPerkHandler;
					missionWeapon = weapon;
					WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
					AttackCollisionData attackCollisionData = collisionData;
					float num3 = MathF.Max(MathF.Sqrt(num2 + mpcombatPerkHandler.GetSpeedBonusEffectiveness(currentUsageItem, (DamageTypes)attackCollisionData.DamageType)) - 1f, 0f);
					num += num * num3;
				}
			}
			missionWeapon = weapon;
			WeaponComponentData currentUsageItem2 = missionWeapon.CurrentUsageItem;
			missionWeapon = weapon;
			return CombatStatCalculator.CalculateStrikeMagnitudeForSwing(swingSpeed, impactPoint, missionWeapon.Item.Weight, currentUsageItem2.GetRealWeaponLength(), currentUsageItem2.TotalInertia, currentUsageItem2.CenterOfMass, num);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002220 File Offset: 0x00000420
		public override float CalculateStrikeMagnitudeForUnarmedAttack(in AttackInformation attackInformation, in AttackCollisionData collisionData, float progressEffect, float momentumRemaining)
		{
			return momentumRemaining * progressEffect * ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.FistFightDamageMultiplier);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002234 File Offset: 0x00000434
		public override float CalculateStrikeMagnitudeForThrust(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float thrustWeaponSpeed, float extraLinearSpeed, bool isThrown = false)
		{
			float num = extraLinearSpeed;
			MissionWeapon missionWeapon;
			if (extraLinearSpeed > 0f)
			{
				MPPerkObject.MPCombatPerkHandler combatPerkHandler = MPPerkObject.GetCombatPerkHandler(attackInformation.AttackerAgent, attackInformation.VictimAgent);
				if (combatPerkHandler != null)
				{
					float num2 = 1f;
					MPPerkObject.MPCombatPerkHandler mpcombatPerkHandler = combatPerkHandler;
					missionWeapon = weapon;
					WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
					AttackCollisionData attackCollisionData = collisionData;
					float num3 = MathF.Max(MathF.Sqrt(num2 + mpcombatPerkHandler.GetSpeedBonusEffectiveness(currentUsageItem, (DamageTypes)attackCollisionData.DamageType)) - 1f, 0f);
					num += num * num3;
				}
				if (attackInformation.AttackerAgent.MountAgent != null)
				{
					AttackCollisionData attackCollisionData = collisionData;
					if (attackCollisionData.StrikeType == 1)
					{
						num *= 0.75f;
					}
				}
			}
			missionWeapon = weapon;
			return CombatStatCalculator.CalculateStrikeMagnitudeForThrust(thrustWeaponSpeed, missionWeapon.Item.Weight, num, isThrown);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022F0 File Offset: 0x000004F0
		public override float ComputeRawDamage(DamageTypes damageType, float magnitude, float armorEffectiveness, float absorbedDamageRatio)
		{
			float bluntDamageFactorByDamageType = this.GetBluntDamageFactorByDamageType(damageType);
			float num = 100f / (100f + armorEffectiveness);
			float num2 = magnitude * num;
			float num3 = bluntDamageFactorByDamageType * num2;
			if (damageType != DamageTypes.Blunt)
			{
				float num4;
				if (damageType != DamageTypes.Cut)
				{
					if (damageType != DamageTypes.Pierce)
					{
						Debug.FailedAssert("Given damage type is invalid.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\ComponentInterfaces\\MultiplayerStrikeMagnitudeModel.cs", "ComputeRawDamage", 112);
						return 0f;
					}
					num4 = MathF.Max(0f, magnitude * (45f / (45f + armorEffectiveness)));
				}
				else
				{
					num4 = MathF.Max(0f, magnitude * (1f - 0.6f * armorEffectiveness / (20f + 0.4f * armorEffectiveness)));
				}
				num3 += (1f - bluntDamageFactorByDamageType) * num4;
			}
			return num3 * absorbedDamageRatio;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023A0 File Offset: 0x000005A0
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
				num = 1f;
				break;
			}
			return num;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023DE File Offset: 0x000005DE
		public override float CalculateHorseArcheryFactor(BasicCharacterObject characterObject)
		{
			return 100f;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000023E8 File Offset: 0x000005E8
		public override float CalculateBaseBlowMagnitudeForPassiveUsage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float extraLinearSpeed)
		{
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			return CombatStatCalculator.CalculateBaseBlowMagnitudeForPassiveUsage(attackerWeapon.Item.Weight, extraLinearSpeed);
		}
	}
}
