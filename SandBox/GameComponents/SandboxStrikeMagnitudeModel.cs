using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace SandBox.GameComponents
{
	// Token: 0x020000CB RID: 203
	public class SandboxStrikeMagnitudeModel : StrikeMagnitudeCalculationModel
	{
		// Token: 0x06000849 RID: 2121 RVA: 0x0003B049 File Offset: 0x00039249
		public override float CalculateHorseArcheryFactor(BasicCharacterObject characterObject)
		{
			return 100f;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0003B050 File Offset: 0x00039250
		public override float CalculateStrikeMagnitudeForMissile(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float missileSpeed)
		{
			BasicCharacterObject attackerAgentCharacter = attackInformation.AttackerAgentCharacter;
			BattleEnvironment attackerBattleEnvironment = attackInformation.AttackerBattleEnvironment;
			MissionWeapon missionWeapon = weapon;
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			AttackCollisionData attackCollisionData = collisionData;
			float missileTotalDamage = attackCollisionData.MissileTotalDamage;
			attackCollisionData = collisionData;
			float missileStartingBaseSpeed = attackCollisionData.MissileStartingBaseSpeed;
			float num = missileSpeed;
			float num2 = missileSpeed - missileStartingBaseSpeed;
			if (num2 > 0f)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
				CharacterObject characterObject = attackerAgentCharacter as CharacterObject;
				if (characterObject != null && characterObject.IsHero)
				{
					WeaponClass ammoClass = currentUsageItem.AmmoClass;
					if (ammoClass == WeaponClass.Sling || ammoClass == WeaponClass.Stone || ammoClass == WeaponClass.ThrowingAxe || ammoClass == WeaponClass.ThrowingKnife || ammoClass == WeaponClass.Javelin)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.RunningThrow, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					}
				}
				num += num2 * explainedNumber.ResultNumber;
			}
			num /= missileStartingBaseSpeed;
			return num * num * missileTotalDamage;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0003B12C File Offset: 0x0003932C
		public override float CalculateBaseBlowMagnitudeForPassiveUsage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float extraLinearSpeed)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(extraLinearSpeed, false, null);
			CharacterObject characterObject = attackInformation.AttackerAgentCharacter as CharacterObject;
			CharacterObject characterObject2 = attackInformation.AttackerCaptainCharacter as CharacterObject;
			bool doesAttackerHaveMountAgent = attackInformation.DoesAttackerHaveMountAgent;
			MissionWeapon missionWeapon = attackInformation.AttackerWeapon;
			SkillObject relevantSkill = missionWeapon.CurrentUsageItem.RelevantSkill;
			BattleEnvironment attackerBattleEnvironment = attackInformation.AttackerBattleEnvironment;
			if (doesAttackerHaveMountAgent)
			{
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.NomadicTraditions, attackerBattleEnvironment, characterObject2, ref explainedNumber);
			}
			else
			{
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.SurgingBlow, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.SurgingBlow, attackerBattleEnvironment, characterObject2, ref explainedNumber);
			}
			if (relevantSkill == DefaultSkills.Polearm)
			{
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Lancer, attackerBattleEnvironment, characterObject2, ref explainedNumber);
				if (doesAttackerHaveMountAgent)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Lancer, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.UnstoppableForce, attackerBattleEnvironment, characterObject2, ref explainedNumber);
				}
			}
			missionWeapon = attackInformation.AttackerWeapon;
			return CombatStatCalculator.CalculateBaseBlowMagnitudeForPassiveUsage(missionWeapon.Item.Weight, explainedNumber.ResultNumber);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0003B214 File Offset: 0x00039414
		public override float CalculateStrikeMagnitudeForSwing(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float swingSpeed, float impactPointAsPercent, float extraLinearSpeed)
		{
			BasicCharacterObject attackerAgentCharacter = attackInformation.AttackerAgentCharacter;
			BattleEnvironment attackerBattleEnvironment = attackInformation.AttackerBattleEnvironment;
			BasicCharacterObject attackerCaptainCharacter = attackInformation.AttackerCaptainCharacter;
			bool doesAttackerHaveMountAgent = attackInformation.DoesAttackerHaveMountAgent;
			MissionWeapon missionWeapon = weapon;
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			CharacterObject characterObject = attackerAgentCharacter as CharacterObject;
			ExplainedNumber explainedNumber = new ExplainedNumber(extraLinearSpeed, false, null);
			if (characterObject != null && extraLinearSpeed > 0f)
			{
				SkillObject relevantSkill = currentUsageItem.RelevantSkill;
				CharacterObject characterObject2 = attackerCaptainCharacter as CharacterObject;
				if (doesAttackerHaveMountAgent)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.NomadicTraditions, attackerBattleEnvironment, characterObject2, ref explainedNumber);
				}
				else
				{
					if (relevantSkill == DefaultSkills.TwoHanded)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.RecklessCharge, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					}
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.DashAndSlash, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.SurgingBlow, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.SurgingBlow, attackerBattleEnvironment, characterObject2, ref explainedNumber);
				}
				if (relevantSkill == DefaultSkills.Polearm)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Lancer, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					if (doesAttackerHaveMountAgent)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Lancer, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.UnstoppableForce, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					}
				}
			}
			missionWeapon = weapon;
			ItemObject item = missionWeapon.Item;
			float num = CombatStatCalculator.CalculateStrikeMagnitudeForSwing(swingSpeed, impactPointAsPercent, item.Weight, currentUsageItem.GetRealWeaponLength(), currentUsageItem.TotalInertia, currentUsageItem.CenterOfMass, explainedNumber.ResultNumber);
			if (item.IsCraftedByPlayer)
			{
				ExplainedNumber explainedNumber2 = new ExplainedNumber(num, false, null);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crafting.SharpenedEdge, attackerBattleEnvironment, characterObject, true, ref explainedNumber2);
				num = explainedNumber2.ResultNumber;
			}
			return num;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0003B38B File Offset: 0x0003958B
		public override float CalculateStrikeMagnitudeForUnarmedAttack(in AttackInformation attackInformation, in AttackCollisionData collisionData, float progressEffect, float momentumRemaining)
		{
			return momentumRemaining * progressEffect * TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.Core.ManagedParametersEnum.FistFightDamageMultiplier) * 2f;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0003B3A4 File Offset: 0x000395A4
		public override float CalculateStrikeMagnitudeForThrust(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float thrustWeaponSpeed, float extraLinearSpeed, bool isThrown = false)
		{
			BasicCharacterObject attackerAgentCharacter = attackInformation.AttackerAgentCharacter;
			BattleEnvironment attackerBattleEnvironment = attackInformation.AttackerBattleEnvironment;
			BasicCharacterObject attackerCaptainCharacter = attackInformation.AttackerCaptainCharacter;
			bool doesAttackerHaveMountAgent = attackInformation.DoesAttackerHaveMountAgent;
			MissionWeapon missionWeapon = weapon;
			ItemObject item = missionWeapon.Item;
			float weight = item.Weight;
			missionWeapon = weapon;
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			CharacterObject characterObject = attackerAgentCharacter as CharacterObject;
			ExplainedNumber explainedNumber = new ExplainedNumber(extraLinearSpeed, false, null);
			if (characterObject != null && extraLinearSpeed > 0f)
			{
				SkillObject relevantSkill = currentUsageItem.RelevantSkill;
				CharacterObject characterObject2 = attackerCaptainCharacter as CharacterObject;
				if (doesAttackerHaveMountAgent)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.NomadicTraditions, attackerBattleEnvironment, characterObject2, ref explainedNumber);
				}
				else
				{
					if (relevantSkill == DefaultSkills.TwoHanded)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.RecklessCharge, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					}
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.DashAndSlash, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.SurgingBlow, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.SurgingBlow, attackerBattleEnvironment, characterObject2, ref explainedNumber);
				}
				if (relevantSkill == DefaultSkills.Polearm)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Lancer, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					if (doesAttackerHaveMountAgent)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Lancer, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.UnstoppableForce, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					}
				}
			}
			float num = CombatStatCalculator.CalculateStrikeMagnitudeForThrust(thrustWeaponSpeed, weight, explainedNumber.ResultNumber, isThrown);
			if (item.IsCraftedByPlayer)
			{
				ExplainedNumber explainedNumber2 = new ExplainedNumber(num, false, null);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crafting.SharpenedTip, attackerBattleEnvironment, characterObject, true, ref explainedNumber2);
				num = explainedNumber2.ResultNumber;
			}
			return num;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0003B50C File Offset: 0x0003970C
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
				Debug.FailedAssert("Given damage type is invalid.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\GameComponents\\SandboxStrikeMagnitudeModel.cs", "ComputeRawDamage", 267);
				return 0f;
			}
			num3 += (1f - bluntDamageFactorByDamageType) * num4;
			return num3 * absorbedDamageRatio;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0003B5C4 File Offset: 0x000397C4
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

		// Token: 0x06000851 RID: 2129 RVA: 0x0003B604 File Offset: 0x00039804
		public override float CalculateAdjustedArmorForBlow(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseArmor, BasicCharacterObject attackerCharacter, BasicCharacterObject attackerCaptainCharacter, BasicCharacterObject victimCharacter, BasicCharacterObject victimCaptainCharacter, WeaponComponentData weaponComponent)
		{
			bool flag = false;
			float num = baseArmor;
			CharacterObject characterObject = attackerCharacter as CharacterObject;
			CharacterObject characterObject2 = attackerCaptainCharacter as CharacterObject;
			BattleEnvironment attackerBattleEnvironment = attackInformation.AttackerBattleEnvironment;
			if (attackerCharacter == characterObject2)
			{
				characterObject2 = null;
			}
			if (num > 0f && characterObject != null)
			{
				if (weaponComponent != null)
				{
					float num2;
					if (weaponComponent.RelevantSkill == DefaultSkills.Crossbow && characterObject.GetPerkValue(DefaultPerks.Crossbow.Piercer, attackInformation.AttackerBattleEnvironment, true, out num2) && baseArmor < num2)
					{
						flag = true;
					}
					else if (weaponComponent.WeaponClass == WeaponClass.SlingStone)
					{
						AttackCollisionData attackCollisionData = collisionData;
						float num3;
						if (attackCollisionData.VictimHitBodyPart == BoneBodyPartType.Head && characterObject.GetPerkValue(DefaultPerks.Throwing.SlingingCompetitions, attackInformation.AttackerBattleEnvironment, true, out num3))
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					num = 0f;
				}
				else
				{
					ExplainedNumber explainedNumber = new ExplainedNumber(baseArmor, false, null);
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.Vandal, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					if (weaponComponent != null)
					{
						if (weaponComponent.RelevantSkill == DefaultSkills.OneHanded)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.ChinkInTheArmor, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
						}
						else if (weaponComponent.RelevantSkill == DefaultSkills.Bow)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.Bodkin, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							if (characterObject2 != null)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.Bodkin, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							}
						}
						else if (weaponComponent.RelevantSkill == DefaultSkills.Crossbow)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Puncture, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							if (characterObject2 != null)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.Puncture, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							}
						}
						else if (weaponComponent.RelevantSkill == DefaultSkills.Throwing)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.WeakSpot, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							if (characterObject2 != null)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.WeakSpot, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							}
						}
					}
					float num4 = explainedNumber.ResultNumber - baseArmor;
					num = MathF.Max(0f, baseArmor - num4);
					if (weaponComponent != null)
					{
						if (weaponComponent.RelevantSkill == DefaultSkills.Bow)
						{
							num *= 1f - attackInformation.AttackerAgent.AgentDrivenProperties.ArmorPenetrationMultiplierBow;
						}
						else if (weaponComponent.RelevantSkill == DefaultSkills.Crossbow)
						{
							num *= 1f - attackInformation.AttackerAgent.AgentDrivenProperties.ArmorPenetrationMultiplierCrossbow;
						}
					}
				}
			}
			return num;
		}
	}
}
