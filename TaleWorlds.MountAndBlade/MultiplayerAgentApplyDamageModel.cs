using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000207 RID: 519
	public class MultiplayerAgentApplyDamageModel : AgentApplyDamageModel
	{
		// Token: 0x06001E36 RID: 7734 RVA: 0x0006740E File Offset: 0x0006560E
		public override bool IsDamageIgnored(in AttackInformation attackInformation, in AttackCollisionData collisionData)
		{
			return false;
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x00067411 File Offset: 0x00065611
		public override float ApplyDamageAmplifications(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			return baseDamage;
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x00067414 File Offset: 0x00065614
		public override float ApplyDamageScaling(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			return baseDamage;
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x00067417 File Offset: 0x00065617
		public override float ApplyDamageReductions(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			return baseDamage;
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x0006741C File Offset: 0x0006561C
		public override float ApplyGeneralDamageModifiers(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			float num = baseDamage;
			Agent attackerAgent = attackInformation.AttackerAgent;
			Agent victimAgent = attackInformation.VictimAgent;
			MPPerkObject.MPCombatPerkHandler combatPerkHandler = MPPerkObject.GetCombatPerkHandler(attackerAgent, victimAgent);
			if (combatPerkHandler != null)
			{
				AttackCollisionData attackCollisionData = collisionData;
				if (attackCollisionData.AttackBlockedWithShield)
				{
					float num2 = 1f;
					MPPerkObject.MPCombatPerkHandler mpcombatPerkHandler = combatPerkHandler;
					attackCollisionData = collisionData;
					float num3 = num2 + mpcombatPerkHandler.GetShieldDamage(attackCollisionData.CorrectSideShieldBlock);
					MPPerkObject.MPCombatPerkHandler mpcombatPerkHandler2 = combatPerkHandler;
					attackCollisionData = collisionData;
					float num4 = num3 + mpcombatPerkHandler2.GetShieldDamageTaken(attackCollisionData.CorrectSideShieldBlock);
					num = MathF.Max(0f, num * num4);
				}
				bool flag = MissionCombatMechanicsHelper.IsCollisionBoneDifferentThanWeaponAttachBone(in collisionData, attackInformation.WeaponAttachBoneIndex);
				MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
				bool flag2;
				if (!attackerWeapon.IsEmpty && !flag)
				{
					attackCollisionData = collisionData;
					if (!attackCollisionData.IsAlternativeAttack)
					{
						attackCollisionData = collisionData;
						if (!attackCollisionData.IsFallDamage)
						{
							attackCollisionData = collisionData;
							flag2 = attackCollisionData.IsHorseCharge;
							goto IL_00C3;
						}
					}
				}
				flag2 = true;
				IL_00C3:
				DamageTypes damageTypes;
				if (!flag2)
				{
					attackCollisionData = collisionData;
					damageTypes = (DamageTypes)attackCollisionData.DamageType;
				}
				else
				{
					damageTypes = DamageTypes.Blunt;
				}
				DamageTypes damageTypes2 = damageTypes;
				float num5 = 0f;
				float num6 = 1f;
				MPPerkObject.MPCombatPerkHandler mpcombatPerkHandler3 = combatPerkHandler;
				WeaponComponentData currentUsageItem = attackerWeapon.CurrentUsageItem;
				DamageTypes damageTypes3 = damageTypes2;
				attackCollisionData = collisionData;
				float num7 = MathF.Max(num5, num6 + mpcombatPerkHandler3.GetDamage(currentUsageItem, damageTypes3, attackCollisionData.IsAlternativeAttack) + combatPerkHandler.GetDamageTaken(attackerWeapon.CurrentUsageItem, damageTypes2));
				if (attackInformation.IsHeadShot && attackerWeapon.CurrentUsageItem != null && (attackerWeapon.CurrentUsageItem.IsConsumable || attackerWeapon.CurrentUsageItem.IsRangedWeapon))
				{
					num7 += combatPerkHandler.GetRangedHeadShotDamage();
				}
				num *= num7;
			}
			return num;
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x00067580 File Offset: 0x00065780
		public override void DecideMissileWeaponFlags(Agent attackerAgent, in MissionWeapon missileWeapon, ref WeaponFlags missileWeaponFlags)
		{
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x00067584 File Offset: 0x00065784
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

		// Token: 0x06001E3D RID: 7741 RVA: 0x00067604 File Offset: 0x00065804
		public override bool CanWeaponDealSneakAttack(in AttackInformation attackInformation, WeaponComponentData weapon)
		{
			return false;
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x00067608 File Offset: 0x00065808
		public override bool CanWeaponDismount(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			return MBMath.IsBetween((int)blow.VictimBodyPart, 0, 6) && ((!attackerAgent.HasMount && blow.StrikeType == StrikeType.Swing && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanHook)) || (blow.StrikeType == StrikeType.Thrust && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanDismount)));
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x00067671 File Offset: 0x00065871
		public override void CalculateDefendedBlowStunMultipliers(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, WeaponComponentData attackerWeapon, WeaponComponentData defenderWeapon, ref float attackerStunPeriod, ref float defenderStunPeriod)
		{
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x00067674 File Offset: 0x00065874
		public override bool CanWeaponKnockback(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			AttackCollisionData attackCollisionData = collisionData;
			return MBMath.IsBetween((int)attackCollisionData.VictimHitBodyPart, 0, 6) && !attackerWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown) && (attackerWeapon.IsConsumable || (blow.BlowFlag & BlowFlags.CrushThrough) != BlowFlags.None || (blow.StrikeType == StrikeType.Thrust && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.WideGrip)));
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x000676E4 File Offset: 0x000658E4
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

		// Token: 0x06001E42 RID: 7746 RVA: 0x0006777A File Offset: 0x0006597A
		public override float GetDismountPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			return 0f;
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x00067781 File Offset: 0x00065981
		public override float GetKnockBackPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			return 0f;
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x00067788 File Offset: 0x00065988
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

		// Token: 0x06001E45 RID: 7749 RVA: 0x000677FF File Offset: 0x000659FF
		public override float GetHorseChargePenetration()
		{
			return 0.4f;
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x00067808 File Offset: 0x00065A08
		public override float CalculateStaggerThresholdDamage(Agent defenderAgent, in Blow blow)
		{
			MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(defenderAgent);
			float? num = ((perkHandler != null) ? new float?(perkHandler.GetDamageInterruptionThreshold()) : null);
			if (num != null && num.Value > 0f)
			{
				return num.Value;
			}
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

		// Token: 0x06001E47 RID: 7751 RVA: 0x0006787D File Offset: 0x00065A7D
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

		// Token: 0x06001E48 RID: 7752 RVA: 0x000678BB File Offset: 0x00065ABB
		public override float CalculatePassiveAttackDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			return baseDamage;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000678BE File Offset: 0x00065ABE
		public override MeleeCollisionReaction DecidePassiveAttackCollisionReaction(Agent attacker, Agent defender, bool isFatalHit)
		{
			return MeleeCollisionReaction.Bounced;
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000678C4 File Offset: 0x00065AC4
		public override float CalculateShieldDamage(in AttackInformation attackInformation, float baseDamage)
		{
			baseDamage *= 1.25f;
			MissionMultiplayerFlagDomination missionBehavior = Mission.Current.GetMissionBehavior<MissionMultiplayerFlagDomination>();
			if (missionBehavior != null && missionBehavior.GetMissionType() == MultiplayerGameType.Captain)
			{
				return baseDamage * 0.75f;
			}
			return baseDamage;
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000678FA File Offset: 0x00065AFA
		public override float CalculateSailFireDamage(Agent attackerAgent, IShipOrigin shipOrigin, float baseDamage, bool damageFromShipMachine)
		{
			return 0f;
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x00067901 File Offset: 0x00065B01
		public override float CalculateHullFireDamage(float baseFireDamage, IShipOrigin shipOrigin)
		{
			return 0f;
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x00067908 File Offset: 0x00065B08
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

		// Token: 0x06001E4E RID: 7758 RVA: 0x00067A22 File Offset: 0x00065C22
		public override bool CanWeaponIgnoreFriendlyFireChecks(WeaponComponentData weapon)
		{
			return weapon != null && weapon.IsConsumable && weapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanPenetrateShield) && weapon.WeaponFlags.HasAnyFlag(WeaponFlags.MultiplePenetration);
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x00067A58 File Offset: 0x00065C58
		public override bool DecideAgentShrugOffBlow(Agent victimAgent, in AttackCollisionData collisionData, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentShrugOffBlow(victimAgent, in collisionData, in blow);
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x00067A62 File Offset: 0x00065C62
		public override bool DecideAgentDismountedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentDismountedByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x00067A70 File Offset: 0x00065C70
		public override bool DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentKnockedBackByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x00067A7E File Offset: 0x00065C7E
		public override bool DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentKnockedDownByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x00067A8C File Offset: 0x00065C8C
		public override bool DecideMountRearedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideMountRearedByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x00067A9C File Offset: 0x00065C9C
		public override void DecideWeaponCollisionReaction(in Blow registeredBlow, in AttackCollisionData collisionData, Agent attacker, Agent defender, in MissionWeapon attackerWeapon, bool isFatalHit, bool isShruggedOff, float momentumRemaining, out MeleeCollisionReaction colReaction)
		{
			MissionCombatMechanicsHelper.DecideWeaponCollisionReaction(in registeredBlow, in collisionData, attacker, defender, in attackerWeapon, isFatalHit, isShruggedOff, momentumRemaining, out colReaction);
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x00067ABD File Offset: 0x00065CBD
		public override bool ShouldMissilePassThroughAfterShieldBreak(Agent attackerAgent, WeaponComponentData attackerWeapon)
		{
			return false;
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x00067AC0 File Offset: 0x00065CC0
		public override float CalculateRemainingMomentum(float originalMomentum, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough)
		{
			return base.CalculateDefaultRemainingMomentum(originalMomentum, in b, in collisionData, attacker, victim, in attackerWeapon, isCrushThrough);
		}
	}
}
