using System;
using MBHelpers;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F9 RID: 505
	public class CustomAgentApplyDamageModel : AgentApplyDamageModel
	{
		// Token: 0x06001DCC RID: 7628 RVA: 0x00064A7C File Offset: 0x00062C7C
		public override bool IsDamageIgnored(in AttackInformation attackInformation, in AttackCollisionData collisionData)
		{
			return false;
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x00064A80 File Offset: 0x00062C80
		public override float ApplyDamageAmplifications(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			bool flag = (attackInformation.IsAttackerAgentMount ? attackInformation.AttackerRiderAgentCharacter : attackInformation.AttackerAgentCharacter) != null;
			Formation attackerFormation = attackInformation.AttackerFormation;
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(attackerFormation);
			bool isVictimAgentMount = attackInformation.IsVictimAgentMount;
			Formation victimFormation = attackInformation.VictimFormation;
			BannerComponent activeBanner2 = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(victimFormation);
			FactoredNumber factoredNumber = new FactoredNumber(baseDamage);
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem = attackerWeapon.CurrentUsageItem;
			if (flag)
			{
				if (currentUsageItem != null)
				{
					if (currentUsageItem.IsMeleeWeapon)
					{
						if (activeBanner != null)
						{
							BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMeleeDamage, activeBanner, ref factoredNumber);
							if (attackInformation.DoesVictimHaveMountAgent)
							{
								BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMeleeDamageAgainstMountedTroops, activeBanner, ref factoredNumber);
							}
						}
					}
					else if (currentUsageItem.IsConsumable && activeBanner != null)
					{
						BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedRangedDamage, activeBanner, ref factoredNumber);
					}
				}
				AttackCollisionData attackCollisionData = collisionData;
				if (attackCollisionData.IsHorseCharge)
				{
					if (activeBanner != null)
					{
						BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedChargeDamage, activeBanner, ref factoredNumber);
					}
					if (activeBanner2 != null)
					{
						BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedChargeDamage, activeBanner2, ref factoredNumber);
					}
				}
			}
			return factoredNumber.ResultNumber;
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x00064B7C File Offset: 0x00062D7C
		public override float ApplyDamageScaling(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			float num = 1f;
			if (Mission.Current.IsSallyOutBattle)
			{
				DestructableComponent hitObjectDestructibleComponent = attackInformation.HitObjectDestructibleComponent;
				if (hitObjectDestructibleComponent != null && hitObjectDestructibleComponent.GameEntity.GetFirstScriptOfType<SiegeWeapon>() != null)
				{
					num *= 4.5f;
				}
			}
			return baseDamage * num;
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x00064BC4 File Offset: 0x00062DC4
		public override float ApplyDamageReductions(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			bool flag = (attackInformation.IsVictimAgentMount ? attackInformation.VictimRiderAgentCharacter : attackInformation.VictimAgentCharacter) != null;
			Formation victimFormation = attackInformation.VictimFormation;
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(victimFormation);
			Agent agent = (attackInformation.IsAttackerAgentMount ? attackInformation.AttackerAgent.RiderAgent : attackInformation.AttackerAgent);
			FactoredNumber factoredNumber = new FactoredNumber(baseDamage);
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem = attackerWeapon.CurrentUsageItem;
			if (flag && currentUsageItem != null)
			{
				if (currentUsageItem.IsConsumable)
				{
					if (activeBanner != null)
					{
						BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedRangedAttackDamage, activeBanner, ref factoredNumber);
					}
					if (Mission.Current.IsNavalBattle && agent != null && agent.IsAIControlled && (currentUsageItem.WeaponClass == WeaponClass.Bolt || currentUsageItem.WeaponClass == WeaponClass.Arrow))
					{
						factoredNumber.AddFactor(-0.15f);
					}
				}
				else if (currentUsageItem.IsMeleeWeapon && activeBanner != null)
				{
					BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedMeleeAttackDamage, activeBanner, ref factoredNumber);
				}
			}
			return factoredNumber.ResultNumber;
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x00064CAE File Offset: 0x00062EAE
		public override float ApplyGeneralDamageModifiers(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			return baseDamage;
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x00064CB1 File Offset: 0x00062EB1
		public override void DecideMissileWeaponFlags(Agent attackerAgent, in MissionWeapon missileWeapon, ref WeaponFlags missileWeaponFlags)
		{
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x00064CB4 File Offset: 0x00062EB4
		public override bool DecideCrushedThrough(Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
		{
			EquipmentIndex equipmentIndex = attackerAgent.GetOffhandWieldedItemIndex();
			if (equipmentIndex == EquipmentIndex.None)
			{
				equipmentIndex = attackerAgent.GetPrimaryWieldedItemIndex();
			}
			WeaponComponentData weaponComponentData = ((equipmentIndex != EquipmentIndex.None) ? attackerAgent.Equipment[equipmentIndex].CurrentUsageItem : null);
			if (weaponComponentData == null || isPassiveUsage || !weaponComponentData.WeaponFlags.HasAnyFlag(WeaponFlags.CanCrushThrough) || strikeType != StrikeType.Swing || attackDirection != Agent.UsageDirection.AttackUp)
			{
				return false;
			}
			float num = 58f;
			if (defendItem != null && defendItem.IsShield)
			{
				num *= 1.2f;
			}
			return totalAttackEnergy > num;
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x00064D34 File Offset: 0x00062F34
		public override bool CanWeaponDealSneakAttack(in AttackInformation attackInformation, WeaponComponentData weapon)
		{
			if (weapon != null && (weapon.IsMeleeWeapon || weapon.WeaponClass == WeaponClass.ThrowingKnife) && attackInformation.IsVictimAgentHuman && !attackInformation.IsVictimPlayer)
			{
				if ((attackInformation.VictimAgentAIStateFlags & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.None)
				{
					return true;
				}
				if (!attackInformation.VictimAgentAIStateFlags.HasAllFlags(Agent.AIStateFlag.Alarmed) && !attackInformation.IsAttackerAgentNull && Vec2.DotProduct((attackInformation.AttackerAgentPosition - attackInformation.VictimAgentPosition).AsVec2.Normalized(), attackInformation.VictimAgentMovementDirection) < 0.174f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x00064DC0 File Offset: 0x00062FC0
		public override bool CanWeaponDismount(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			return MBMath.IsBetween((int)blow.VictimBodyPart, 0, 6) && ((!attackerAgent.HasMount && blow.StrikeType == StrikeType.Swing && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanHook)) || (blow.StrikeType == StrikeType.Thrust && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanDismount)));
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x00064E29 File Offset: 0x00063029
		public override void CalculateDefendedBlowStunMultipliers(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, WeaponComponentData attackerWeapon, WeaponComponentData defenderWeapon, ref float attackerStunPeriod, ref float defenderStunPeriod)
		{
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x00064E2C File Offset: 0x0006302C
		public override bool CanWeaponKnockback(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			AttackCollisionData attackCollisionData = collisionData;
			return MBMath.IsBetween((int)attackCollisionData.VictimHitBodyPart, 0, 6) && !attackerWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown) && (attackerWeapon.IsConsumable || (blow.BlowFlag & BlowFlags.CrushThrough) != BlowFlags.None || (blow.StrikeType == StrikeType.Thrust && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.WideGrip)));
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x00064E9C File Offset: 0x0006309C
		public override bool CanWeaponKnockDown(Agent attackerAgent, Agent victimAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			if (attackerWeapon.WeaponClass == WeaponClass.Boulder || attackerWeapon.WeaponClass == WeaponClass.BallistaBoulder)
			{
				return true;
			}
			AttackCollisionData attackCollisionData = collisionData;
			BoneBodyPartType victimHitBodyPart = attackCollisionData.VictimHitBodyPart;
			bool flag = MBMath.IsBetween((int)victimHitBodyPart, 0, 6);
			if (!victimAgent.HasMount && victimHitBodyPart == BoneBodyPartType.Legs)
			{
				flag = true;
			}
			return flag && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown) && ((attackerWeapon.IsPolearm && blow.StrikeType == StrikeType.Thrust) || (attackerWeapon.IsMeleeWeapon && blow.StrikeType == StrikeType.Swing && MissionCombatMechanicsHelper.DecideSweetSpotCollision(in collisionData)));
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x00064F34 File Offset: 0x00063134
		public override float GetDismountPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			float num = 0f;
			if (blow.StrikeType == StrikeType.Swing && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanHook))
			{
				num += 0.25f;
			}
			return num;
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x00064F70 File Offset: 0x00063170
		public override float GetKnockBackPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			return 0f;
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x00064F78 File Offset: 0x00063178
		public override float GetKnockDownPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			float num = 0f;
			if (attackerWeapon.WeaponClass == WeaponClass.Boulder || attackerWeapon.WeaponClass == WeaponClass.BallistaBoulder)
			{
				num += 0.25f;
			}
			else if (attackerWeapon.IsMeleeWeapon)
			{
				AttackCollisionData attackCollisionData2 = attackCollisionData;
				if (attackCollisionData2.VictimHitBodyPart == BoneBodyPartType.Legs && blow.StrikeType == StrikeType.Swing)
				{
					num += 0.1f;
				}
				else
				{
					attackCollisionData2 = attackCollisionData;
					if (attackCollisionData2.VictimHitBodyPart == BoneBodyPartType.Head)
					{
						num += 0.15f;
					}
				}
			}
			return num;
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x00064FEF File Offset: 0x000631EF
		public override float GetHorseChargePenetration()
		{
			return 0.4f;
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x00064FF8 File Offset: 0x000631F8
		public override float CalculateStaggerThresholdDamage(Agent defenderAgent, in Blow blow)
		{
			ManagedParametersEnum managedParametersEnum;
			if (blow.DamageType == DamageTypes.Cut)
			{
				managedParametersEnum = ManagedParametersEnum.DamageInterruptAttackThresholdCut;
			}
			else if (blow.DamageType == DamageTypes.Pierce)
			{
				managedParametersEnum = ManagedParametersEnum.DamageInterruptAttackThresholdPierce;
			}
			else
			{
				managedParametersEnum = ManagedParametersEnum.DamageInterruptAttackThresholdBlunt;
			}
			return ManagedParameters.Instance.GetManagedParameter(managedParametersEnum);
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0006502E File Offset: 0x0006322E
		public override float CalculateAlternativeAttackDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, WeaponComponentData weapon)
		{
			if (weapon == null)
			{
				return 2f;
			}
			if (weapon.WeaponClass == WeaponClass.LargeShield)
			{
				return 2f;
			}
			if (weapon.WeaponClass == WeaponClass.SmallShield)
			{
				return 1f;
			}
			if (weapon.IsTwoHanded)
			{
				return 2f;
			}
			return 1f;
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x0006506C File Offset: 0x0006326C
		public override float CalculatePassiveAttackDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			return baseDamage;
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x0006506F File Offset: 0x0006326F
		public override MeleeCollisionReaction DecidePassiveAttackCollisionReaction(Agent attacker, Agent defender, bool isFatalHit)
		{
			return MeleeCollisionReaction.Bounced;
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x00065074 File Offset: 0x00063274
		public override float CalculateShieldDamage(in AttackInformation attackInformation, float baseDamage)
		{
			baseDamage *= 1.25f;
			FactoredNumber factoredNumber = new FactoredNumber(baseDamage);
			Formation victimFormation = attackInformation.VictimFormation;
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(victimFormation);
			if (activeBanner != null)
			{
				BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedShieldDamage, activeBanner, ref factoredNumber);
			}
			return Math.Max(0f, factoredNumber.ResultNumber);
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x000650CB File Offset: 0x000632CB
		public override float CalculateSailFireDamage(Agent attackerAgent, IShipOrigin shipOrigin, float baseDamage, bool damageFromShipMachine)
		{
			return baseDamage;
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x000650CE File Offset: 0x000632CE
		public override float CalculateHullFireDamage(float baseFireDamage, IShipOrigin shipOrigin)
		{
			return baseFireDamage;
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000650D4 File Offset: 0x000632D4
		public override float GetDamageMultiplierForBodyPart(BoneBodyPartType bodyPart, DamageTypes type, bool isHuman, bool isMissile)
		{
			float num = 1f;
			switch (bodyPart)
			{
			case BoneBodyPartType.None:
				num = 1f;
				break;
			case BoneBodyPartType.Head:
				switch (type)
				{
				case DamageTypes.Invalid:
					num = 1.5f;
					break;
				case DamageTypes.Cut:
					num = 1.2f;
					break;
				case DamageTypes.Pierce:
					if (isHuman)
					{
						num = (isMissile ? 2f : 1.25f);
					}
					else
					{
						num = 1.2f;
					}
					break;
				case DamageTypes.Blunt:
					num = 1.2f;
					break;
				}
				break;
			case BoneBodyPartType.Neck:
				switch (type)
				{
				case DamageTypes.Invalid:
					num = 1.5f;
					break;
				case DamageTypes.Cut:
					num = 1.2f;
					break;
				case DamageTypes.Pierce:
					if (isHuman)
					{
						num = (isMissile ? 2f : 1.25f);
					}
					else
					{
						num = 1.2f;
					}
					break;
				case DamageTypes.Blunt:
					num = 1.2f;
					break;
				}
				break;
			case BoneBodyPartType.Chest:
			case BoneBodyPartType.Abdomen:
			case BoneBodyPartType.ShoulderLeft:
			case BoneBodyPartType.ShoulderRight:
			case BoneBodyPartType.ArmLeft:
			case BoneBodyPartType.ArmRight:
				if (isHuman)
				{
					num = 1f;
				}
				else
				{
					num = 0.8f;
				}
				break;
			case BoneBodyPartType.Legs:
				num = 0.8f;
				break;
			}
			return num;
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x000651EE File Offset: 0x000633EE
		public override bool CanWeaponIgnoreFriendlyFireChecks(WeaponComponentData weapon)
		{
			return weapon != null && weapon.IsConsumable && weapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanPenetrateShield) && weapon.WeaponFlags.HasAnyFlag(WeaponFlags.MultiplePenetration);
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x00065224 File Offset: 0x00063424
		public override bool DecideAgentShrugOffBlow(Agent victimAgent, in AttackCollisionData collisionData, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentShrugOffBlow(victimAgent, in collisionData, in blow);
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x0006522E File Offset: 0x0006342E
		public override bool DecideAgentDismountedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentDismountedByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x0006523C File Offset: 0x0006343C
		public override bool DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentKnockedBackByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x0006524A File Offset: 0x0006344A
		public override bool DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentKnockedDownByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x00065258 File Offset: 0x00063458
		public override bool DecideMountRearedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideMountRearedByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x00065268 File Offset: 0x00063468
		public override void DecideWeaponCollisionReaction(in Blow registeredBlow, in AttackCollisionData collisionData, Agent attacker, Agent defender, in MissionWeapon attackerWeapon, bool isFatalHit, bool isShruggedOff, float momentumRemaining, out MeleeCollisionReaction colReaction)
		{
			MissionCombatMechanicsHelper.DecideWeaponCollisionReaction(in registeredBlow, in collisionData, attacker, defender, in attackerWeapon, isFatalHit, isShruggedOff, momentumRemaining, out colReaction);
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x00065289 File Offset: 0x00063489
		public override bool ShouldMissilePassThroughAfterShieldBreak(Agent attackerAgent, WeaponComponentData attackerWeapon)
		{
			return false;
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x0006528C File Offset: 0x0006348C
		public override float CalculateRemainingMomentum(float originalMomentum, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough)
		{
			return base.CalculateDefaultRemainingMomentum(originalMomentum, in b, in collisionData, attacker, victim, in attackerWeapon, isCrushThrough);
		}

		// Token: 0x04000A5C RID: 2652
		private const float SallyOutSiegeEngineDamageMultiplier = 4.5f;
	}
}
