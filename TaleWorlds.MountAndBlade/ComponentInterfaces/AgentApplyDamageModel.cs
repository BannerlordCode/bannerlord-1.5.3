using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000401 RID: 1025
	public abstract class AgentApplyDamageModel : MBGameModel<AgentApplyDamageModel>
	{
		// Token: 0x0600383C RID: 14396 RVA: 0x000E9348 File Offset: 0x000E7548
		public float CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			AgentApplyDamageModel agentApplyDamageModel = MissionGameModels.Current.AgentApplyDamageModel;
			if (agentApplyDamageModel.IsDamageIgnored(in attackInformation, in collisionData))
			{
				return 0f;
			}
			float num = agentApplyDamageModel.ApplyDamageAmplifications(in attackInformation, in collisionData, baseDamage);
			num = agentApplyDamageModel.ApplyDamageScaling(in attackInformation, in collisionData, num);
			num = agentApplyDamageModel.ApplyDamageReductions(in attackInformation, in collisionData, num);
			num = agentApplyDamageModel.ApplyGeneralDamageModifiers(in attackInformation, in collisionData, num);
			return MathF.Max(0f, num);
		}

		// Token: 0x0600383D RID: 14397
		public abstract bool IsDamageIgnored(in AttackInformation attackInformation, in AttackCollisionData collisionData);

		// Token: 0x0600383E RID: 14398
		public abstract float ApplyDamageAmplifications(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage);

		// Token: 0x0600383F RID: 14399
		public abstract float ApplyDamageScaling(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage);

		// Token: 0x06003840 RID: 14400
		public abstract float ApplyDamageReductions(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage);

		// Token: 0x06003841 RID: 14401
		public abstract float ApplyGeneralDamageModifiers(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage);

		// Token: 0x06003842 RID: 14402
		public abstract void DecideMissileWeaponFlags(Agent attackerAgent, in MissionWeapon missileWeapon, ref WeaponFlags missileWeaponFlags);

		// Token: 0x06003843 RID: 14403
		public abstract void CalculateDefendedBlowStunMultipliers(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, WeaponComponentData attackerWeapon, WeaponComponentData defenderWeapon, ref float attackerStunPeriod, ref float defenderStunPeriod);

		// Token: 0x06003844 RID: 14404
		public abstract float CalculateStaggerThresholdDamage(Agent defenderAgent, in Blow blow);

		// Token: 0x06003845 RID: 14405
		public abstract float CalculateAlternativeAttackDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, WeaponComponentData weapon);

		// Token: 0x06003846 RID: 14406
		public abstract float CalculatePassiveAttackDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage);

		// Token: 0x06003847 RID: 14407
		public abstract MeleeCollisionReaction DecidePassiveAttackCollisionReaction(Agent attacker, Agent defender, bool isFatalHit);

		// Token: 0x06003848 RID: 14408
		public abstract void DecideWeaponCollisionReaction(in Blow registeredBlow, in AttackCollisionData collisionData, Agent attacker, Agent defender, in MissionWeapon attackerWeapon, bool isFatalHit, bool isShruggedOff, float momentumRemaining, out MeleeCollisionReaction colReaction);

		// Token: 0x06003849 RID: 14409
		public abstract float CalculateShieldDamage(in AttackInformation attackInformation, float baseDamage);

		// Token: 0x0600384A RID: 14410
		public abstract float CalculateSailFireDamage(Agent attackerAgent, IShipOrigin shipOrigin, float baseDamage, bool damageFromShipMachine);

		// Token: 0x0600384B RID: 14411
		public abstract float CalculateHullFireDamage(float baseFireDamage, IShipOrigin shipOrigin);

		// Token: 0x0600384C RID: 14412
		public abstract float GetDamageMultiplierForBodyPart(BoneBodyPartType bodyPart, DamageTypes type, bool isHuman, bool isMissile);

		// Token: 0x0600384D RID: 14413
		public abstract bool CanWeaponIgnoreFriendlyFireChecks(WeaponComponentData weapon);

		// Token: 0x0600384E RID: 14414
		public abstract bool CanWeaponDealSneakAttack(in AttackInformation attackInformation, WeaponComponentData weapon);

		// Token: 0x0600384F RID: 14415
		public abstract bool CanWeaponDismount(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x06003850 RID: 14416
		public abstract bool CanWeaponKnockback(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x06003851 RID: 14417
		public abstract bool CanWeaponKnockDown(Agent attackerAgent, Agent victimAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x06003852 RID: 14418
		public abstract bool DecideCrushedThrough(Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsageHit);

		// Token: 0x06003853 RID: 14419
		public abstract float CalculateRemainingMomentum(float originalMomentum, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough);

		// Token: 0x06003854 RID: 14420 RVA: 0x000E93A4 File Offset: 0x000E75A4
		protected float CalculateDefaultRemainingMomentum(float originalMomentum, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough)
		{
			float num = 0f;
			if (isCrushThrough)
			{
				num = originalMomentum * 0.3f;
			}
			else if (b.InflictedDamage > 0)
			{
				AttackCollisionData attackCollisionData = collisionData;
				if (!attackCollisionData.AttackBlockedWithShield)
				{
					attackCollisionData = collisionData;
					if (!attackCollisionData.CollidedWithShieldOnBack)
					{
						attackCollisionData = collisionData;
						if (attackCollisionData.IsColliderAgent)
						{
							attackCollisionData = collisionData;
							if (!attackCollisionData.IsHorseCharge)
							{
								if (attacker != null && attacker.IsDoingPassiveAttack)
								{
									num = originalMomentum * 0.5f;
								}
								else if (!MissionCombatMechanicsHelper.HitWithAnotherBone(in collisionData, attacker, in attackerWeapon))
								{
									MissionWeapon missionWeapon = attackerWeapon;
									if (!missionWeapon.IsEmpty && b.StrikeType != StrikeType.Thrust)
									{
										missionWeapon = attackerWeapon;
										if (!missionWeapon.IsEmpty)
										{
											missionWeapon = attackerWeapon;
											if (missionWeapon.CurrentUsageItem.CanHitMultipleTargets)
											{
												num = originalMomentum * (1f - b.AbsorbedByArmor / (float)b.InflictedDamage);
												num *= 0.5f;
												if (num < 0.25f)
												{
													num = 0f;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06003855 RID: 14421
		public abstract bool DecideAgentShrugOffBlow(Agent victimAgent, in AttackCollisionData collisionData, in Blow blow);

		// Token: 0x06003856 RID: 14422
		public abstract bool DecideAgentDismountedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow);

		// Token: 0x06003857 RID: 14423
		public abstract bool DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow);

		// Token: 0x06003858 RID: 14424
		public abstract bool DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow);

		// Token: 0x06003859 RID: 14425
		public abstract bool DecideMountRearedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow);

		// Token: 0x0600385A RID: 14426
		public abstract bool ShouldMissilePassThroughAfterShieldBreak(Agent attackerAgent, WeaponComponentData attackerWeapon);

		// Token: 0x0600385B RID: 14427
		public abstract float GetDismountPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x0600385C RID: 14428
		public abstract float GetKnockBackPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x0600385D RID: 14429
		public abstract float GetKnockDownPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x0600385E RID: 14430
		public abstract float GetHorseChargePenetration();
	}
}
