using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F8 RID: 504
	public abstract class AgentStatCalculateModel : MBGameModel<AgentStatCalculateModel>
	{
		// Token: 0x06001DAB RID: 7595
		public abstract void InitializeAgentStats(Agent agent, Equipment spawnEquipment, AgentDrivenProperties agentDrivenProperties, AgentBuildData agentBuildData);

		// Token: 0x06001DAC RID: 7596 RVA: 0x00064262 File Offset: 0x00062462
		public virtual void InitializeMissionEquipment(Agent agent)
		{
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x00064264 File Offset: 0x00062464
		public virtual void InitializeAgentStatsAfterDeploymentFinished(Agent agent)
		{
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x00064266 File Offset: 0x00062466
		public virtual void InitializeMissionEquipmentAfterDeploymentFinished(Agent agent)
		{
		}

		// Token: 0x06001DAF RID: 7599
		public abstract void UpdateAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties);

		// Token: 0x06001DB0 RID: 7600
		public abstract float GetDifficultyModifier();

		// Token: 0x06001DB1 RID: 7601
		public abstract bool CanAgentRideMount(Agent agent, Agent targetMount);

		// Token: 0x06001DB2 RID: 7602 RVA: 0x00064268 File Offset: 0x00062468
		public virtual bool HasHeavyArmor(Agent agent)
		{
			return agent.GetBaseArmorEffectivenessForBodyPart(BoneBodyPartType.Chest) >= 24f;
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x0006427B File Offset: 0x0006247B
		public virtual float GetEffectiveArmorEncumbrance(Agent agent, Equipment equipment)
		{
			return equipment.GetTotalWeightOfArmor(agent.IsHuman);
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x00064289 File Offset: 0x00062489
		public virtual float GetEffectiveMaxHealth(Agent agent)
		{
			return agent.BaseHealthLimit;
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x00064294 File Offset: 0x00062494
		public virtual float GetEnvironmentSpeedFactor(Agent agent)
		{
			Scene scene = agent.Mission.Scene;
			float num = 1f;
			if (!scene.IsAtmosphereIndoor)
			{
				if (scene.GetRainDensity() > 0f)
				{
					num *= 0.9f;
				}
				if (!agent.IsHuman && !scene.IsDayTime)
				{
					num *= 0.9f;
				}
			}
			return num;
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x000642E9 File Offset: 0x000624E9
		public float CalculateAIAttackOnDecideMaxValue()
		{
			if (this.GetDifficultyModifier() <= 0.5f)
			{
				return 0.16f;
			}
			return 0.48f;
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x00064304 File Offset: 0x00062504
		public virtual float GetWeaponInaccuracy(Agent agent, WeaponComponentData weapon, int weaponSkill)
		{
			float num = 0f;
			if (weapon.IsRangedWeapon)
			{
				if (weapon.WeaponClass == WeaponClass.Sling)
				{
					num = (100f - (float)weapon.Accuracy) * (1f - 0.003f * (float)weaponSkill) * 0.001f;
				}
				else
				{
					num = (100f - (float)weapon.Accuracy) * (1f - 0.002f * (float)weaponSkill) * 0.001f;
				}
			}
			else if (weapon.WeaponFlags.HasAllFlags(WeaponFlags.WideGrip))
			{
				num = 1f - (float)weaponSkill * 0.01f;
			}
			return MathF.Max(num, 0f);
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0006439D File Offset: 0x0006259D
		public virtual float GetDetachmentCostMultiplierOfAgent(Agent agent, IDetachment detachment)
		{
			if (agent.Banner != null)
			{
				return 10f;
			}
			return 1f;
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x000643B2 File Offset: 0x000625B2
		public virtual float GetInteractionDistance(Agent agent)
		{
			return 1.5f;
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x000643B9 File Offset: 0x000625B9
		public virtual float GetMaxCameraZoom(Agent agent)
		{
			return 1f;
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x000643C0 File Offset: 0x000625C0
		public virtual int GetEffectiveSkill(Agent agent, SkillObject skill)
		{
			return agent.Character.GetSkillValue(skill);
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x000643CE File Offset: 0x000625CE
		public virtual int GetEffectiveSkillForWeapon(Agent agent, WeaponComponentData weapon)
		{
			return this.GetEffectiveSkill(agent, weapon.RelevantSkill);
		}

		// Token: 0x06001DBD RID: 7613
		public abstract float GetWeaponDamageMultiplier(Agent agent, WeaponComponentData weapon);

		// Token: 0x06001DBE RID: 7614
		public abstract float GetEquipmentStealthBonus(Agent agent);

		// Token: 0x06001DBF RID: 7615
		public abstract float GetSneakAttackMultiplier(Agent agent, WeaponComponentData weapon);

		// Token: 0x06001DC0 RID: 7616
		public abstract float GetKnockBackResistance(Agent agent);

		// Token: 0x06001DC1 RID: 7617
		public abstract float GetKnockDownResistance(Agent agent, StrikeType strikeType = StrikeType.Invalid);

		// Token: 0x06001DC2 RID: 7618
		public abstract float GetDismountResistance(Agent agent);

		// Token: 0x06001DC3 RID: 7619
		public abstract float GetBreatheHoldMaxDuration(Agent agent, float baseBreatheHoldMaxDuration);

		// Token: 0x06001DC4 RID: 7620 RVA: 0x000643DD File Offset: 0x000625DD
		public virtual string GetMissionDebugInfoForAgent(Agent agent)
		{
			return "Debug info not supported in this model";
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x000643E4 File Offset: 0x000625E4
		public void ResetAILevelMultiplier()
		{
			this._AILevelMultiplier = 1f;
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x000643F1 File Offset: 0x000625F1
		public void SetAILevelMultiplier(float multiplier)
		{
			this._AILevelMultiplier = multiplier;
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x000643FC File Offset: 0x000625FC
		protected int GetMeleeSkill(Agent agent, WeaponComponentData equippedItem, WeaponComponentData secondaryItem)
		{
			SkillObject skillObject = DefaultSkills.Athletics;
			if (equippedItem != null)
			{
				SkillObject relevantSkill = equippedItem.RelevantSkill;
				if (relevantSkill == DefaultSkills.OneHanded || relevantSkill == DefaultSkills.Polearm)
				{
					skillObject = relevantSkill;
				}
				else if (relevantSkill == DefaultSkills.TwoHanded)
				{
					skillObject = ((secondaryItem == null) ? DefaultSkills.TwoHanded : DefaultSkills.OneHanded);
				}
				else
				{
					skillObject = DefaultSkills.OneHanded;
				}
			}
			return this.GetEffectiveSkill(agent, skillObject);
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00064458 File Offset: 0x00062658
		protected float CalculateAILevel(Agent agent, int relevantSkillLevel)
		{
			float difficultyModifier = this.GetDifficultyModifier();
			return MBMath.ClampFloat((float)relevantSkillLevel / 300f * ((difficultyModifier <= 0f) ? 0.1f : ((difficultyModifier <= 0.5f) ? 0.32f : 0.96f)), 0f, 1f);
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x000644A8 File Offset: 0x000626A8
		protected void SetAiRelatedProperties(Agent agent, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedItem, WeaponComponentData secondaryItem)
		{
			int meleeSkill = this.GetMeleeSkill(agent, equippedItem, secondaryItem);
			SkillObject skillObject = ((equippedItem == null) ? DefaultSkills.Athletics : equippedItem.RelevantSkill);
			int effectiveSkill = this.GetEffectiveSkill(agent, skillObject);
			agentDrivenProperties.AiShooterErrorWoRangeUpdate = 0f;
			float num = this.CalculateAILevel(agent, meleeSkill) * this._AILevelMultiplier;
			float num2 = this.CalculateAILevel(agent, effectiveSkill) * this._AILevelMultiplier;
			float num3 = num + agent.Defensiveness;
			float difficultyModifier = this.GetDifficultyModifier();
			agentDrivenProperties.AiRangedHorsebackMissileRange = 0.3f + 0.4f * num2;
			agentDrivenProperties.AiFacingMissileWatch = -0.96f + num * 0.06f;
			agentDrivenProperties.AiFlyingMissileCheckRadius = 8f - 6f * num;
			agentDrivenProperties.AiShootFreq = 0.3f + 0.7f * num2;
			agentDrivenProperties.AiWaitBeforeShootFactor = (agent.PropertyModifiers.resetAiWaitBeforeShootFactor ? 0f : (1f - 0.5f * num2));
			agentDrivenProperties.AIBlockOnDecideAbility = MBMath.Lerp(0.5f, 0.99f, MBMath.ClampFloat(MathF.Pow(num, 0.5f), 0f, 1f), 1E-05f);
			agentDrivenProperties.AIParryOnDecideAbility = MBMath.Lerp(0.5f, 0.95f, MBMath.ClampFloat(num, 0f, 1f), 1E-05f);
			agentDrivenProperties.AiTryChamberAttackOnDecide = (num - 0.15f) * 0.1f;
			agentDrivenProperties.AIAttackOnParryChance = 0.08f - 0.02f * agent.Defensiveness;
			agentDrivenProperties.AiAttackOnParryTiming = -0.2f + 0.3f * num;
			agentDrivenProperties.AIDecideOnAttackChance = 0.5f * agent.Defensiveness;
			agentDrivenProperties.AIParryOnAttackAbility = MBMath.ClampFloat(num, 0f, 1f);
			agentDrivenProperties.AiKick = -0.1f + ((num > 0.4f) ? 0.4f : num);
			agentDrivenProperties.AiAttackCalculationMaxTimeFactor = num;
			agentDrivenProperties.AiDecideOnAttackWhenReceiveHitTiming = -0.25f * (1f - num);
			agentDrivenProperties.AiDecideOnAttackContinueAction = -0.5f * (1f - num);
			agentDrivenProperties.AiDecideOnAttackingContinue = 0.1f * num;
			agentDrivenProperties.AIParryOnAttackingContinueAbility = MBMath.Lerp(0.5f, 0.95f, MBMath.ClampFloat(num, 0f, 1f), 1E-05f);
			agentDrivenProperties.AIDecideOnRealizeEnemyBlockingAttackAbility = MBMath.ClampFloat(MathF.Pow(num, 2.5f) - 0.1f, 0f, 1f);
			agentDrivenProperties.AIRealizeBlockingFromIncorrectSideAbility = MBMath.ClampFloat(MathF.Pow(num, 2.5f) - 0.01f, 0f, 1f);
			agentDrivenProperties.AiAttackingShieldDefenseChance = 0.2f + 0.3f * num;
			agentDrivenProperties.AiAttackingShieldDefenseTimer = -0.3f + 0.3f * num;
			agentDrivenProperties.AiRandomizedDefendDirectionChance = 1f - MathF.Pow(num, 3f);
			agentDrivenProperties.AiShooterError = 0.008f;
			agentDrivenProperties.AISetNoAttackTimerAfterBeingHitAbility = MBMath.Lerp(0.33f, 1f, num, 1E-05f);
			agentDrivenProperties.AISetNoAttackTimerAfterBeingParriedAbility = MBMath.Lerp(0.2f, 1f, num * num, 1E-05f);
			agentDrivenProperties.AISetNoDefendTimerAfterHittingAbility = MBMath.Lerp(0.1f, 0.99f, num * num, 1E-05f);
			agentDrivenProperties.AISetNoDefendTimerAfterParryingAbility = MBMath.Lerp(0.15f, 1f, num * num, 1E-05f);
			agentDrivenProperties.AIEstimateStunDurationPrecision = 1f - MBMath.Lerp(0.2f, 1f, num, 1E-05f);
			agentDrivenProperties.AIHoldingReadyMaxDuration = MBMath.Lerp(0.25f, 0f, MathF.Min(1f, num * 2f), 1E-05f);
			agentDrivenProperties.AIHoldingReadyVariationPercentage = num;
			agentDrivenProperties.AiRaiseShieldDelayTimeBase = -0.75f + 0.5f * num;
			agentDrivenProperties.AiUseShieldAgainstEnemyMissileProbability = 0.1f + num * 0.6f + num3 * 0.2f;
			agentDrivenProperties.AiCheckApplyMovementInterval = (2f - difficultyModifier) * (0.05f + 0.005f * (1.1f - num));
			agentDrivenProperties.AiCheckCalculateMovementInterval = ((agent.HasMount || agent.IsMount) ? 0.25f : ((2f - difficultyModifier) * 0.25f));
			agentDrivenProperties.AiCheckDecideSimpleBehaviorInterval = (2f - difficultyModifier) * (agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanWieldWeapon) ? 1.5f : 0.2f);
			agentDrivenProperties.AiCheckDoSimpleBehaviorInterval = 2f - difficultyModifier;
			agentDrivenProperties.AiMovementDelayFactor = 4f / (3f + num2);
			agentDrivenProperties.AiParryDecisionChangeValue = 0.05f + 0.7f * num;
			agentDrivenProperties.AiDefendWithShieldDecisionChanceValue = MathF.Min(2f, 0.5f + num + 0.6f * num3);
			agentDrivenProperties.AiMoveEnemySideTimeValue = -2.5f + 0.5f * num;
			agentDrivenProperties.AiMinimumDistanceToContinueFactor = 2f + 0.3f * (3f - num);
			agentDrivenProperties.AiChargeHorsebackTargetDistFactor = 1.5f * (3f - num);
			agentDrivenProperties.AiWaitBeforeShootFactor = (agent.PropertyModifiers.resetAiWaitBeforeShootFactor ? 0f : (1f - 0.5f * num2));
			float num4 = 1f - num2;
			agentDrivenProperties.AiRangerLeadErrorMin = -num4 * 0.35f;
			agentDrivenProperties.AiRangerLeadErrorMax = num4 * 0.2f;
			agentDrivenProperties.AiRangerVerticalErrorMultiplier = num4 * 0.1f;
			agentDrivenProperties.AiRangerHorizontalErrorMultiplier = num4 * 0.034906585f;
			agentDrivenProperties.AIAttackOnDecideChance = MathF.Clamp(0.1f * this.CalculateAIAttackOnDecideMaxValue() * (3f - agent.Defensiveness), 0.05f, 1f);
			agentDrivenProperties.SetStat(DrivenProperty.UseRealisticBlocking, (agent.Controller != AgentControllerType.Player) ? 1f : 0f);
			agentDrivenProperties.AiWeaponFavorMultiplierMelee = 1f;
			agentDrivenProperties.AiWeaponFavorMultiplierRanged = 1f;
			agentDrivenProperties.AiWeaponFavorMultiplierPolearm = 1f;
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x00064A3F File Offset: 0x00062C3F
		protected void SetAllWeaponInaccuracy(Agent agent, AgentDrivenProperties agentDrivenProperties, int equippedIndex, WeaponComponentData equippedWeaponComponent)
		{
			if (equippedWeaponComponent != null)
			{
				agentDrivenProperties.WeaponInaccuracy = this.GetWeaponInaccuracy(agent, equippedWeaponComponent, this.GetEffectiveSkillForWeapon(agent, equippedWeaponComponent));
				return;
			}
			agentDrivenProperties.WeaponInaccuracy = 0f;
		}

		// Token: 0x04000A5A RID: 2650
		protected const float MaxHorizontalErrorRadian = 0.034906585f;

		// Token: 0x04000A5B RID: 2651
		private float _AILevelMultiplier = 1f;
	}
}
