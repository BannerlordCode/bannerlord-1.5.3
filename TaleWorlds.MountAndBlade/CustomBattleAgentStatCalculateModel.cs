using System;
using MBHelpers;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001FA RID: 506
	public class CustomBattleAgentStatCalculateModel : AgentStatCalculateModel
	{
		// Token: 0x06001DEE RID: 7662 RVA: 0x000652A7 File Offset: 0x000634A7
		public override float GetDifficultyModifier()
		{
			return 1f;
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000652AE File Offset: 0x000634AE
		public override bool CanAgentRideMount(Agent agent, Agent targetMount)
		{
			return agent.CheckSkillForMounting(targetMount);
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x000652B8 File Offset: 0x000634B8
		public override void InitializeAgentStats(Agent agent, Equipment spawnEquipment, AgentDrivenProperties agentDrivenProperties, AgentBuildData agentBuildData)
		{
			agentDrivenProperties.ArmorEncumbrance = spawnEquipment.GetTotalWeightOfArmor(agent.IsHuman);
			if (agent.IsHuman)
			{
				agentDrivenProperties.ArmorHead = spawnEquipment.GetHeadArmorSum();
				agentDrivenProperties.ArmorTorso = spawnEquipment.GetHumanBodyArmorSum();
				agentDrivenProperties.ArmorLegs = spawnEquipment.GetLegArmorSum();
				agentDrivenProperties.ArmorArms = spawnEquipment.GetArmArmorSum();
			}
			else
			{
				agentDrivenProperties.AiSpeciesIndex = (int)spawnEquipment[EquipmentIndex.ArmorItemEndSlot].Item.Id.InternalValue;
				agentDrivenProperties.AttributeRiding = 0.8f + ((spawnEquipment[EquipmentIndex.HorseHarness].Item != null) ? 0.2f : 0f);
				float num = 0f;
				for (int i = 1; i < 12; i++)
				{
					if (spawnEquipment[i].Item != null)
					{
						num += (float)spawnEquipment[i].GetModifiedMountBodyArmor();
					}
				}
				agentDrivenProperties.ArmorTorso = num;
				ItemObject item = spawnEquipment[EquipmentIndex.ArmorItemEndSlot].Item;
				if (item != null)
				{
					HorseComponent horseComponent = item.HorseComponent;
					EquipmentElement equipmentElement = spawnEquipment[EquipmentIndex.ArmorItemEndSlot];
					EquipmentElement equipmentElement2 = spawnEquipment[EquipmentIndex.HorseHarness];
					agentDrivenProperties.MountChargeDamage = (float)equipmentElement.GetModifiedMountCharge(in equipmentElement2) * 0.01f;
					agentDrivenProperties.MountDifficulty = (float)equipmentElement.Item.Difficulty;
				}
			}
			agentDrivenProperties.OffhandWeaponDefendSpeedMultiplier = 1f;
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x00065408 File Offset: 0x00063608
		public override void UpdateAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			if (agent.IsHuman)
			{
				this.UpdateHumanStats(agent, agentDrivenProperties);
				return;
			}
			this.UpdateHorseStats(agent, agentDrivenProperties);
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x00065424 File Offset: 0x00063624
		public override float GetWeaponDamageMultiplier(Agent agent, WeaponComponentData weapon)
		{
			float num = 1f;
			SkillObject skillObject = ((weapon != null) ? weapon.RelevantSkill : null);
			if (skillObject != null)
			{
				int effectiveSkill = MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveSkill(agent, skillObject);
				if (skillObject == DefaultSkills.OneHanded)
				{
					num += (float)effectiveSkill * 0.0015f;
				}
				else if (skillObject == DefaultSkills.TwoHanded)
				{
					num += (float)effectiveSkill * 0.0016f;
				}
				else if (skillObject == DefaultSkills.Polearm)
				{
					num += (float)effectiveSkill * 0.0007f;
				}
				else if (skillObject == DefaultSkills.Bow)
				{
					num += (float)effectiveSkill * 0.0011f;
				}
				else if (skillObject == DefaultSkills.Throwing)
				{
					num += (float)effectiveSkill * 0.0006f;
				}
			}
			return Math.Max(0f, num);
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x000654CB File Offset: 0x000636CB
		public override float GetEquipmentStealthBonus(Agent agent)
		{
			return 0f;
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x000654D4 File Offset: 0x000636D4
		public override float GetSneakAttackMultiplier(Agent agent, WeaponComponentData weapon)
		{
			BasicCharacterObject character = agent.Character;
			float num = 1f;
			if (weapon != null && character != null)
			{
				int skillValue = character.GetSkillValue(DefaultSkills.Roguery);
				num += 0.5f + (float)skillValue * 0.002f;
				if (weapon.WeaponClass == WeaponClass.Dagger)
				{
					num *= 3f;
				}
				else if (weapon.WeaponClass == WeaponClass.ThrowingKnife)
				{
					num *= 2f;
				}
			}
			return Math.Max(0f, num);
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x00065544 File Offset: 0x00063744
		public override float GetKnockBackResistance(Agent agent)
		{
			if (agent.IsHuman)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Athletics);
				float num = 0.15f + (float)effectiveSkill * 0.001f;
				return Math.Max(0f, num);
			}
			return float.MaxValue;
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x00065588 File Offset: 0x00063788
		public override float GetKnockDownResistance(Agent agent, StrikeType strikeType = StrikeType.Invalid)
		{
			if (agent.IsHuman)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Athletics);
				float num = 0.4f + (float)effectiveSkill * 0.001f;
				if (agent.HasMount)
				{
					num += 0.1f;
				}
				else if (strikeType == StrikeType.Thrust)
				{
					num += 0.15f;
				}
				return Math.Max(0f, num);
			}
			return float.MaxValue;
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000655E8 File Offset: 0x000637E8
		public override float GetDismountResistance(Agent agent)
		{
			if (agent.IsHuman)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Riding);
				float num = 0.4f + (float)effectiveSkill * 0.001f;
				return Math.Max(0f, num);
			}
			return float.MaxValue;
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0006562C File Offset: 0x0006382C
		public override float GetWeaponInaccuracy(Agent agent, WeaponComponentData weapon, int weaponSkill)
		{
			if (weapon == null || !weapon.IsRangedWeapon || weapon.RelevantSkill != DefaultSkills.Throwing || weapon.WeaponClass != WeaponClass.Sling)
			{
				return base.GetWeaponInaccuracy(agent, weapon, weaponSkill);
			}
			float num = MathF.Max(0.2f, 1f - 0.007f * (float)weaponSkill);
			return (100f - (float)weapon.Accuracy) * 0.001f * num;
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x00065699 File Offset: 0x00063899
		public override float GetBreatheHoldMaxDuration(Agent agent, float baseBreatheHoldMaxDuration)
		{
			return baseBreatheHoldMaxDuration;
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0006569C File Offset: 0x0006389C
		private int GetSkillValueForItem(Agent agent, ItemObject primaryItem)
		{
			return this.GetEffectiveSkill(agent, (primaryItem != null) ? primaryItem.RelevantSkill : DefaultSkills.Athletics);
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x000656B8 File Offset: 0x000638B8
		private void UpdateHumanStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			BasicCharacterObject character = agent.Character;
			MissionEquipment equipment = agent.Equipment;
			float num = equipment.GetTotalWeightOfWeapons();
			int weight = agent.Monster.Weight;
			float num2 = agentDrivenProperties.ArmorEncumbrance + num;
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			EquipmentIndex offhandWieldedItemIndex = agent.GetOffhandWieldedItemIndex();
			if (primaryWieldedItemIndex != EquipmentIndex.None)
			{
				ItemObject item = equipment[primaryWieldedItemIndex].Item;
				WeaponComponent weaponComponent = item.WeaponComponent;
				if (weaponComponent != null)
				{
					float realWeaponLength = weaponComponent.PrimaryWeapon.GetRealWeaponLength();
					num += 1.5f * item.Weight * MathF.Sqrt(realWeaponLength);
				}
			}
			if (offhandWieldedItemIndex != EquipmentIndex.None)
			{
				ItemObject item2 = equipment[offhandWieldedItemIndex].Item;
				num += 1.5f * item2.Weight;
			}
			agentDrivenProperties.WeaponExternalAccelerationAccuracyPenalty = 0f;
			agentDrivenProperties.WeaponsEncumbrance = num;
			WeaponComponentData weaponComponentData = ((primaryWieldedItemIndex != EquipmentIndex.None) ? equipment[primaryWieldedItemIndex].CurrentUsageItem : null);
			ItemObject itemObject = ((primaryWieldedItemIndex != EquipmentIndex.None) ? equipment[primaryWieldedItemIndex].Item : null);
			WeaponComponentData weaponComponentData2 = ((offhandWieldedItemIndex != EquipmentIndex.None) ? equipment[offhandWieldedItemIndex].CurrentUsageItem : null);
			agentDrivenProperties.SwingSpeedMultiplier = 0.93f + 0.0007f * (float)this.GetSkillValueForItem(agent, itemObject);
			agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = agentDrivenProperties.SwingSpeedMultiplier;
			agentDrivenProperties.HandlingMultiplier = 1f;
			agentDrivenProperties.ShieldBashStunDurationMultiplier = 1f;
			agentDrivenProperties.KickStunDurationMultiplier = 1f;
			agentDrivenProperties.ReloadSpeed = 0.93f + 0.0007f * (float)this.GetSkillValueForItem(agent, itemObject);
			agentDrivenProperties.MissileSpeedMultiplier = 1f;
			agentDrivenProperties.ReloadMovementPenaltyFactor = 1f;
			agentDrivenProperties.DamageMultiplierBonus = 0f;
			base.SetAllWeaponInaccuracy(agent, agentDrivenProperties, (int)primaryWieldedItemIndex, weaponComponentData);
			int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Athletics);
			int effectiveSkill2 = this.GetEffectiveSkill(agent, DefaultSkills.Riding);
			if (weaponComponentData != null)
			{
				WeaponComponentData weaponComponentData3 = weaponComponentData;
				int effectiveSkillForWeapon = this.GetEffectiveSkillForWeapon(agent, weaponComponentData3);
				if (weaponComponentData3.IsRangedWeapon)
				{
					int thrustSpeed = weaponComponentData3.ThrustSpeed;
					if (!agent.HasMount)
					{
						float num3 = MathF.Max(0f, 1f - (float)effectiveSkillForWeapon / 500f);
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = 0.125f * num3;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = 0.1f * num3;
					}
					else
					{
						float num4 = MathF.Max(0f, (1f - (float)effectiveSkillForWeapon / 500f) * (1f - (float)effectiveSkill2 / 1800f));
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = 0.025f * num4;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = 0.12f * num4;
					}
					agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = MathF.Max(0f, agentDrivenProperties.WeaponMaxMovementAccuracyPenalty);
					agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = MathF.Max(0f, agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty);
					if (weaponComponentData3.RelevantSkill == DefaultSkills.Bow)
					{
						float num5 = ((float)thrustSpeed - 45f) / 90f;
						num5 = MBMath.ClampFloat(num5, 0f, 1f);
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 6f;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 4.5f / MBMath.Lerp(0.75f, 2f, num5, 1E-05f);
					}
					else if (weaponComponentData3.RelevantSkill == DefaultSkills.Throwing)
					{
						if (weaponComponentData3.WeaponClass == WeaponClass.Sling)
						{
							float num6 = ((float)thrustSpeed - 30f) / 90f;
							num6 = MBMath.ClampFloat(num6, 0f, 1f);
							agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 5f;
							agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 2.4f * MBMath.Lerp(2.4f, 1.2f, num6, 1E-05f);
						}
						else
						{
							float num7 = ((float)thrustSpeed - 91f) / 14f;
							num7 = MBMath.ClampFloat(num7, 0f, 1f);
							agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 0.5f;
							agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 1.5f * MBMath.Lerp(1.5f, 0.8f, num7, 1E-05f);
						}
					}
					else if (weaponComponentData3.RelevantSkill == DefaultSkills.Crossbow)
					{
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 2.5f;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 1.2f;
					}
					switch (weaponComponentData3.WeaponClass)
					{
					case WeaponClass.Bow:
					{
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.3f + (95.75f - (float)thrustSpeed) * 0.005f;
						float num8 = ((float)thrustSpeed - 45f) / 90f;
						num8 = MBMath.ClampFloat(num8, 0f, 1f);
						agentDrivenProperties.WeaponUnsteadyBeginTime = 0.6f + (float)effectiveSkillForWeapon * 0.01f * MBMath.Lerp(2f, 4f, num8, 1E-05f);
						if (agent.IsAIControlled)
						{
							agentDrivenProperties.WeaponUnsteadyBeginTime *= 4f;
						}
						agentDrivenProperties.WeaponUnsteadyEndTime = 2f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.1f;
						goto IL_060C;
					}
					case WeaponClass.Sling:
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 2.6f + (89f - (float)thrustSpeed) * 0.12f;
						agentDrivenProperties.WeaponUnsteadyBeginTime = 3f + (float)effectiveSkillForWeapon * 0.064f;
						agentDrivenProperties.WeaponUnsteadyEndTime = 22f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.2f;
						goto IL_060C;
					case WeaponClass.ThrowingAxe:
					case WeaponClass.ThrowingKnife:
					case WeaponClass.Javelin:
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.2f + (89f - (float)thrustSpeed) * 0.009f;
						agentDrivenProperties.WeaponUnsteadyBeginTime = 2.5f + (float)effectiveSkillForWeapon * 0.01f;
						agentDrivenProperties.WeaponUnsteadyEndTime = 10f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.025f;
						goto IL_060C;
					}
					agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.1f;
					agentDrivenProperties.WeaponUnsteadyBeginTime = 0f;
					agentDrivenProperties.WeaponUnsteadyEndTime = 0f;
					agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.1f;
				}
				else if (weaponComponentData3.WeaponFlags.HasAllFlags(WeaponFlags.WideGrip))
				{
					agentDrivenProperties.WeaponUnsteadyBeginTime = 1f + (float)effectiveSkillForWeapon * 0.005f;
					agentDrivenProperties.WeaponUnsteadyEndTime = 3f + (float)effectiveSkillForWeapon * 0.01f;
				}
			}
			IL_060C:
			agentDrivenProperties.TopSpeedReachDuration = 2f / MathF.Max((200f + (float)effectiveSkill) / 300f * ((float)weight / ((float)weight + num2)), 0.3f);
			float num9 = 0.7f + 0.00070000015f * (float)effectiveSkill;
			float num10 = MathF.Max(0.2f * (1f - (float)effectiveSkill * 0.001f), 0f) * num2 / (float)weight;
			float num11 = MBMath.ClampFloat(num9 - num10, 0f, 0.91f);
			agentDrivenProperties.MaxSpeedMultiplier = this.GetEnvironmentSpeedFactor(agent) * num11;
			float managedParameter = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalCombatSpeedMinMultiplier);
			float managedParameter2 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalCombatSpeedMaxMultiplier);
			float num12 = MathF.Min(num2 / (float)weight, 1f);
			agentDrivenProperties.CombatMaxSpeedMultiplier = MathF.Min(MBMath.Lerp(managedParameter2, managedParameter, num12, 1E-05f), 1f);
			int effectiveSkill3 = this.GetEffectiveSkill(agent, DefaultSkills.Roguery);
			agentDrivenProperties.CrouchedSpeedMultiplier = 1f + (float)effectiveSkill3 * 0.001f;
			agentDrivenProperties.AttributeShieldMissileCollisionBodySizeAdder = 0.3f;
			Agent mountAgent = agent.MountAgent;
			float num13 = ((mountAgent != null) ? mountAgent.GetAgentDrivenPropertyValue(DrivenProperty.AttributeRiding) : 1f);
			agentDrivenProperties.AttributeRiding = (float)effectiveSkill2 * num13;
			agentDrivenProperties.AttributeHorseArchery = MissionGameModels.Current.StrikeMagnitudeModel.CalculateHorseArcheryFactor(character);
			agentDrivenProperties.BipedalRangedReadySpeedMultiplier = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRangedReadySpeedMultiplier);
			agentDrivenProperties.BipedalRangedReloadSpeedMultiplier = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRangedReloadSpeedMultiplier);
			if (agent.Character != null)
			{
				if (agent.HasMount && weaponComponentData != null)
				{
					this.SetMountedWeaponPenaltiesOnAgent(agent, agentDrivenProperties, weaponComponentData);
				}
				this.SetBannerEffectsOnAgent(agent, agentDrivenProperties, weaponComponentData);
			}
			base.SetAiRelatedProperties(agent, agentDrivenProperties, weaponComponentData, weaponComponentData2);
			float num14 = 1f;
			if (!agent.Mission.Scene.IsAtmosphereIndoor)
			{
				float rainDensity = agent.Mission.Scene.GetRainDensity();
				float fog = agent.Mission.Scene.GetFog();
				if (rainDensity > 0f || fog > 0f)
				{
					num14 += MathF.Min(0.3f, rainDensity + fog);
				}
				if (!agent.Mission.Scene.IsDayTime)
				{
					num14 += 0.1f;
				}
			}
			agentDrivenProperties.AiShooterError *= num14;
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x00065EF4 File Offset: 0x000640F4
		private void UpdateHorseStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			Equipment spawnEquipment = agent.SpawnEquipment;
			EquipmentElement equipmentElement = spawnEquipment[EquipmentIndex.ArmorItemEndSlot];
			EquipmentElement equipmentElement2 = spawnEquipment[EquipmentIndex.HorseHarness];
			ItemObject item = equipmentElement.Item;
			float num = (float)(equipmentElement.GetModifiedMountSpeed(in equipmentElement2) + 1);
			int modifiedMountManeuver = equipmentElement.GetModifiedMountManeuver(in equipmentElement2);
			int num2 = 0;
			float environmentSpeedFactor = this.GetEnvironmentSpeedFactor(agent);
			if (agent.RiderAgent != null)
			{
				num2 = this.GetEffectiveSkill(agent.RiderAgent, DefaultSkills.Riding);
				FactoredNumber factoredNumber = new FactoredNumber(num);
				FactoredNumber factoredNumber2 = new FactoredNumber((float)modifiedMountManeuver);
				factoredNumber.AddFactor((float)num2 * 0.001f);
				factoredNumber2.AddFactor((float)num2 * 0.0004f);
				Formation formation = agent.RiderAgent.Formation;
				BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(formation);
				if (activeBanner != null)
				{
					BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMountMovementSpeed, activeBanner, ref factoredNumber);
				}
				agentDrivenProperties.MountManeuver = factoredNumber2.ResultNumber;
				agentDrivenProperties.MountSpeed = environmentSpeedFactor * 0.22f * (1f + factoredNumber.ResultNumber);
			}
			else
			{
				agentDrivenProperties.MountManeuver = (float)modifiedMountManeuver;
				agentDrivenProperties.MountSpeed = environmentSpeedFactor * 0.22f * (1f + num);
			}
			float num3 = equipmentElement.Weight / 2f + (equipmentElement2.IsEmpty ? 0f : equipmentElement2.Weight);
			agentDrivenProperties.MountDashAccelerationMultiplier = ((num3 > 200f) ? ((num3 < 300f) ? (1f - (num3 - 200f) / 111f) : 0.1f) : 1f);
			agentDrivenProperties.TopSpeedReachDuration = Game.Current.BasicModels.RidingModel.CalculateAcceleration(in equipmentElement, in equipmentElement2, num2);
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x00066090 File Offset: 0x00064290
		private void SetBannerEffectsOnAgent(Agent agent, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedWeaponComponent)
		{
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(agent.Formation);
			if (activeBanner != null)
			{
				bool flag = equippedWeaponComponent != null && equippedWeaponComponent.IsRangedWeapon;
				FactoredNumber factoredNumber = new FactoredNumber(agentDrivenProperties.MaxSpeedMultiplier);
				FactoredNumber factoredNumber2 = new FactoredNumber(agentDrivenProperties.WeaponInaccuracy);
				if (flag && equippedWeaponComponent != null)
				{
					BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedRangedAccuracyPenalty, activeBanner, ref factoredNumber2);
				}
				BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedTroopMovementSpeed, activeBanner, ref factoredNumber);
				agentDrivenProperties.MaxSpeedMultiplier = factoredNumber.ResultNumber;
				agentDrivenProperties.WeaponInaccuracy = factoredNumber2.ResultNumber;
			}
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x00066118 File Offset: 0x00064318
		private void SetMountedWeaponPenaltiesOnAgent(Agent agent, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedWeaponComponent)
		{
			int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Riding);
			float num = 0.3f - (float)effectiveSkill * 0.003f;
			if (num > 0f)
			{
				float num2 = agentDrivenProperties.SwingSpeedMultiplier * (1f - num);
				float num3 = agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier * (1f - num);
				float num4 = agentDrivenProperties.ReloadSpeed * (1f - num);
				float num5 = agentDrivenProperties.WeaponBestAccuracyWaitTime * (1f + num);
				agentDrivenProperties.SwingSpeedMultiplier = Math.Max(0f, num2);
				agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = Math.Max(0f, num3);
				agentDrivenProperties.ReloadSpeed = Math.Max(0f, num4);
				agentDrivenProperties.WeaponBestAccuracyWaitTime = Math.Max(0f, num5);
			}
			float num6 = 15f - (float)effectiveSkill * 0.15f;
			if (num6 > 0f)
			{
				float num7 = agentDrivenProperties.WeaponInaccuracy * (1f + num6);
				agentDrivenProperties.WeaponInaccuracy = Math.Max(0f, num7);
			}
		}
	}
}
