using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000208 RID: 520
	public class MultiplayerAgentStatCalculateModel : AgentStatCalculateModel
	{
		// Token: 0x06001E58 RID: 7768 RVA: 0x00067ADB File Offset: 0x00065CDB
		public override float GetDifficultyModifier()
		{
			return 1f;
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x00067AE2 File Offset: 0x00065CE2
		public override bool CanAgentRideMount(Agent agent, Agent targetMount)
		{
			return agent.CheckSkillForMounting(targetMount);
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x00067AEB File Offset: 0x00065CEB
		public override void InitializeAgentStats(Agent agent, Equipment spawnEquipment, AgentDrivenProperties agentDrivenProperties, AgentBuildData agentBuildData)
		{
			agentDrivenProperties.ArmorEncumbrance = spawnEquipment.GetTotalWeightOfArmor(agent.IsHuman);
			if (!agent.IsHuman)
			{
				MultiplayerAgentStatCalculateModel.InitializeHorseAgentStats(agent, spawnEquipment, agentDrivenProperties);
			}
			else
			{
				agentDrivenProperties = this.InitializeHumanAgentStats(agent, agentDrivenProperties, agentBuildData);
			}
			agentDrivenProperties.OffhandWeaponDefendSpeedMultiplier = 1f;
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x00067B28 File Offset: 0x00065D28
		private AgentDrivenProperties InitializeHumanAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties, AgentBuildData agentBuildData)
		{
			MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(agent.Character);
			if (mpheroClassForCharacter != null)
			{
				this.FillAgentStatsFromData(ref agentDrivenProperties, agent, mpheroClassForCharacter, (agentBuildData != null) ? agentBuildData.AgentMissionPeer : null, (agentBuildData != null) ? agentBuildData.OwningAgentMissionPeer : null);
				agentDrivenProperties.SetStat(DrivenProperty.UseRealisticBlocking, MultiplayerOptions.OptionType.UseRealisticBlocking.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) ? 1f : 0f);
			}
			if (mpheroClassForCharacter != null)
			{
				agent.BaseHealthLimit = (float)mpheroClassForCharacter.Health;
			}
			else
			{
				agent.BaseHealthLimit = 100f;
			}
			agent.HealthLimit = agent.BaseHealthLimit;
			agent.Health = agent.HealthLimit;
			return agentDrivenProperties;
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x00067BBC File Offset: 0x00065DBC
		private static void InitializeHorseAgentStats(Agent agent, Equipment spawnEquipment, AgentDrivenProperties agentDrivenProperties)
		{
			agentDrivenProperties.AiSpeciesIndex = agent.Monster.FamilyType;
			float num = 0.8f;
			EquipmentElement equipmentElement = spawnEquipment[EquipmentIndex.HorseHarness];
			agentDrivenProperties.AttributeRiding = num + ((equipmentElement.Item != null) ? 0.2f : 0f);
			float num2 = 0f;
			for (int i = 1; i < 12; i++)
			{
				equipmentElement = spawnEquipment[i];
				if (equipmentElement.Item != null)
				{
					float num3 = num2;
					equipmentElement = spawnEquipment[i];
					num2 = num3 + (float)equipmentElement.GetModifiedMountBodyArmor();
				}
			}
			agentDrivenProperties.ArmorTorso = num2;
			equipmentElement = spawnEquipment[EquipmentIndex.ArmorItemEndSlot];
			HorseComponent horseComponent = equipmentElement.Item.HorseComponent;
			EquipmentElement equipmentElement2 = spawnEquipment[EquipmentIndex.ArmorItemEndSlot];
			equipmentElement = spawnEquipment[EquipmentIndex.HorseHarness];
			agentDrivenProperties.MountChargeDamage = (float)equipmentElement2.GetModifiedMountCharge(in equipmentElement) * 0.01f;
			agentDrivenProperties.MountDifficulty = (float)equipmentElement2.Item.Difficulty;
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x00067C93 File Offset: 0x00065E93
		public override float GetWeaponDamageMultiplier(Agent agent, WeaponComponentData weapon)
		{
			return 1f;
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x00067C9A File Offset: 0x00065E9A
		public override float GetEquipmentStealthBonus(Agent agent)
		{
			return 0f;
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x00067CA1 File Offset: 0x00065EA1
		public override float GetSneakAttackMultiplier(Agent agent, WeaponComponentData weapon)
		{
			return 1f;
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x00067CA8 File Offset: 0x00065EA8
		public override float GetKnockBackResistance(Agent agent)
		{
			return agent.Character.KnockbackResistance;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x00067CB8 File Offset: 0x00065EB8
		public override float GetKnockDownResistance(Agent agent, StrikeType strikeType = StrikeType.Invalid)
		{
			float num = agent.Character.KnockdownResistance;
			if (agent.HasMount)
			{
				num += 0.1f;
			}
			else if (strikeType == StrikeType.Thrust)
			{
				num += 0.25f;
			}
			return num;
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x00067CF0 File Offset: 0x00065EF0
		public override float GetDismountResistance(Agent agent)
		{
			return agent.Character.DismountResistance;
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x00067D00 File Offset: 0x00065F00
		public override float GetWeaponInaccuracy(Agent agent, WeaponComponentData weapon, int weaponSkill)
		{
			float num = 0f;
			if (weapon != null && weapon.IsRangedWeapon)
			{
				float num2;
				if (weapon.RelevantSkill == DefaultSkills.Throwing && weapon.WeaponClass == WeaponClass.Sling)
				{
					num2 = MathF.Max(1f - 0.007f * (float)weaponSkill, 0.2f);
				}
				else
				{
					num2 = 1f - 0.002f * (float)weaponSkill;
					if (weapon.WeaponClass == WeaponClass.ThrowingAxe)
					{
						num2 *= 2f;
					}
				}
				num = (100f - (float)weapon.Accuracy) * num2 * 0.001f;
			}
			else if (weapon != null && weapon.WeaponFlags.HasAllFlags(WeaponFlags.WideGrip))
			{
				num = 1f - (float)weaponSkill * 0.01f;
			}
			return MathF.Max(0f, num);
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x00067DBC File Offset: 0x00065FBC
		public override float GetBreatheHoldMaxDuration(Agent agent, float baseBreatheHoldMaxDuration)
		{
			return baseBreatheHoldMaxDuration;
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x00067DBF File Offset: 0x00065FBF
		public override void UpdateAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			if (agent.IsHuman)
			{
				this.UpdateHumanAgentStats(agent, agentDrivenProperties);
				return;
			}
			if (agent.IsMount)
			{
				this.UpdateMountAgentStats(agent, agentDrivenProperties);
			}
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x00067DE4 File Offset: 0x00065FE4
		private void UpdateMountAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(agent.RiderAgent);
			EquipmentElement equipmentElement = agent.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot];
			EquipmentElement equipmentElement2 = agent.SpawnEquipment[EquipmentIndex.HorseHarness];
			agentDrivenProperties.MountManeuver = (float)equipmentElement.GetModifiedMountManeuver(in equipmentElement2) * (1f + ((perkHandler != null) ? perkHandler.GetMountManeuver() : 0f));
			agentDrivenProperties.MountSpeed = (float)(equipmentElement.GetModifiedMountSpeed(in equipmentElement2) + 1) * 0.22f * (1f + ((perkHandler != null) ? perkHandler.GetMountSpeed() : 0f));
			Agent riderAgent = agent.RiderAgent;
			int num = ((riderAgent != null) ? riderAgent.Character.GetSkillValue(DefaultSkills.Riding) : 100);
			agentDrivenProperties.TopSpeedReachDuration = Game.Current.BasicModels.RidingModel.CalculateAcceleration(in equipmentElement, in equipmentElement2, num);
			agentDrivenProperties.MountSpeed *= 1f + (float)num * 0.0032f;
			agentDrivenProperties.MountManeuver *= 1f + (float)num * 0.0035f;
			float num2 = equipmentElement.Weight / 2f + (equipmentElement2.IsEmpty ? 0f : equipmentElement2.Weight);
			agentDrivenProperties.MountDashAccelerationMultiplier = ((num2 > 200f) ? ((num2 < 300f) ? (1f - (num2 - 200f) / 111f) : 0.1f) : 1f);
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x00067F44 File Offset: 0x00066144
		public override int GetEffectiveSkillForWeapon(Agent agent, WeaponComponentData weapon)
		{
			int num = base.GetEffectiveSkillForWeapon(agent, weapon);
			if (num > 0 && weapon.IsRangedWeapon)
			{
				MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(agent);
				if (perkHandler != null)
				{
					num = MathF.Ceiling((float)num * (perkHandler.GetRangedAccuracy() + 1f));
				}
			}
			return num;
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x00067F88 File Offset: 0x00066188
		private void UpdateHumanAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(agent);
			BasicCharacterObject character = agent.Character;
			MissionEquipment equipment = agent.Equipment;
			float num = equipment.GetTotalWeightOfWeapons();
			num *= 1f + ((perkHandler != null) ? perkHandler.GetEncumbrance(true) : 0f);
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			EquipmentIndex offhandWieldedItemIndex = agent.GetOffhandWieldedItemIndex();
			if (primaryWieldedItemIndex != EquipmentIndex.None)
			{
				ItemObject item = equipment[primaryWieldedItemIndex].Item;
				WeaponComponent weaponComponent = item.WeaponComponent;
				if (weaponComponent != null)
				{
					float realWeaponLength = weaponComponent.PrimaryWeapon.GetRealWeaponLength();
					float num2 = ((weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.Bow) ? 4f : 1.5f) * item.Weight * MathF.Sqrt(realWeaponLength);
					num2 *= 1f + ((perkHandler != null) ? perkHandler.GetEncumbrance(false) : 0f);
					num += num2;
				}
			}
			if (offhandWieldedItemIndex != EquipmentIndex.None)
			{
				ItemObject item2 = equipment[offhandWieldedItemIndex].Item;
				float num3 = 1.5f * item2.Weight;
				num3 *= 1f + ((perkHandler != null) ? perkHandler.GetEncumbrance(false) : 0f);
				num += num3;
			}
			agentDrivenProperties.WeaponExternalAccelerationAccuracyPenalty = 0f;
			agentDrivenProperties.WeaponsEncumbrance = num;
			EquipmentIndex primaryWieldedItemIndex2 = agent.GetPrimaryWieldedItemIndex();
			WeaponComponentData weaponComponentData = ((primaryWieldedItemIndex2 != EquipmentIndex.None) ? equipment[primaryWieldedItemIndex2].CurrentUsageItem : null);
			ItemObject itemObject = ((primaryWieldedItemIndex2 != EquipmentIndex.None) ? equipment[primaryWieldedItemIndex2].Item : null);
			EquipmentIndex offhandWieldedItemIndex2 = agent.GetOffhandWieldedItemIndex();
			WeaponComponentData weaponComponentData2 = ((offhandWieldedItemIndex2 != EquipmentIndex.None) ? equipment[offhandWieldedItemIndex2].CurrentUsageItem : null);
			agentDrivenProperties.SwingSpeedMultiplier = 0.93f + 0.0007f * (float)this.GetSkillValueForItem(character, itemObject);
			agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = agentDrivenProperties.SwingSpeedMultiplier;
			agentDrivenProperties.HandlingMultiplier = 1f;
			agentDrivenProperties.ShieldBashStunDurationMultiplier = 1f;
			agentDrivenProperties.KickStunDurationMultiplier = 1f;
			agentDrivenProperties.ReloadSpeed = 0.93f + 0.0007f * (float)this.GetSkillValueForItem(character, itemObject);
			agentDrivenProperties.MissileSpeedMultiplier = 1f;
			agentDrivenProperties.ReloadMovementPenaltyFactor = 1f;
			agentDrivenProperties.DamageMultiplierBonus = 0f;
			base.SetAllWeaponInaccuracy(agent, agentDrivenProperties, (int)primaryWieldedItemIndex2, weaponComponentData);
			MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(agent.Character);
			float num4 = (mpheroClassForCharacter.IsTroopCharacter(agent.Character) ? mpheroClassForCharacter.TroopMovementSpeedMultiplier : mpheroClassForCharacter.HeroMovementSpeedMultiplier);
			agentDrivenProperties.MaxSpeedMultiplier = 1.05f * (num4 * (100f / (100f + num)));
			int skillValue = character.GetSkillValue(DefaultSkills.Riding);
			bool flag = false;
			bool flag2 = false;
			if (weaponComponentData != null)
			{
				WeaponComponentData weaponComponentData3 = weaponComponentData;
				int effectiveSkillForWeapon = this.GetEffectiveSkillForWeapon(agent, weaponComponentData3);
				if (perkHandler != null)
				{
					agentDrivenProperties.MissileSpeedMultiplier *= perkHandler.GetThrowingWeaponSpeed(weaponComponentData) + 1f;
				}
				if (weaponComponentData3.IsRangedWeapon)
				{
					int thrustSpeed = weaponComponentData3.ThrustSpeed;
					if (!agent.HasMount)
					{
						float num5 = MathF.Max(0f, 1f - (float)effectiveSkillForWeapon / 500f);
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = 0.125f * num5;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = 0.1f * num5;
					}
					else
					{
						float num6 = MathF.Max(0f, (1f - (float)effectiveSkillForWeapon / 500f) * (1f - (float)skillValue / 1800f));
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = 0.025f * num6;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = 0.06f * num6;
					}
					agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = MathF.Max(0f, agentDrivenProperties.WeaponMaxMovementAccuracyPenalty);
					agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = MathF.Max(0f, agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty);
					if (weaponComponentData3.RelevantSkill == DefaultSkills.Bow)
					{
						float num7 = ((float)thrustSpeed - 60f) / 75f;
						num7 = MBMath.ClampFloat(num7, 0f, 1f);
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 6f;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 4.5f / MBMath.Lerp(0.75f, 2f, num7, 1E-05f);
					}
					else if (weaponComponentData3.RelevantSkill == DefaultSkills.Throwing)
					{
						if (weaponComponentData3.WeaponClass == WeaponClass.Sling)
						{
							float num8 = ((float)thrustSpeed - 30f) / 90f;
							num8 = MBMath.ClampFloat(num8, 0f, 1f);
							agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 5f;
							agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 2.4f * MBMath.Lerp(2.4f, 1.2f, num8, 1E-05f);
						}
						else
						{
							float num9 = ((float)thrustSpeed - 85f) / 17f;
							num9 = MBMath.ClampFloat(num9, 0f, 1f);
							agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 0.5f;
							agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 1.5f * MBMath.Lerp(1.5f, 0.8f, num9, 1E-05f);
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
						flag = true;
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.3f + (95.75f - (float)thrustSpeed) * 0.005f;
						float num10 = ((float)thrustSpeed - 60f) / 75f;
						num10 = MBMath.ClampFloat(num10, 0f, 1f);
						agentDrivenProperties.WeaponUnsteadyBeginTime = 0.1f + (float)effectiveSkillForWeapon * 0.01f * MBMath.Lerp(1f, 2f, num10, 1E-05f);
						if (agent.IsAIControlled)
						{
							agentDrivenProperties.WeaponUnsteadyBeginTime *= 4f;
						}
						agentDrivenProperties.WeaponUnsteadyEndTime = 2f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.1f;
						goto IL_06FE;
					}
					case WeaponClass.Sling:
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 2.6f + (89f - (float)thrustSpeed) * 0.12f;
						agentDrivenProperties.WeaponUnsteadyBeginTime = 3f + (float)effectiveSkillForWeapon * 0.064f;
						agentDrivenProperties.WeaponUnsteadyEndTime = 22f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.2f;
						goto IL_06FE;
					case WeaponClass.ThrowingAxe:
					case WeaponClass.ThrowingKnife:
					case WeaponClass.Javelin:
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.2f + (89f - (float)thrustSpeed) * 0.009f;
						agentDrivenProperties.WeaponUnsteadyBeginTime = 2.5f + (float)effectiveSkillForWeapon * 0.01f;
						agentDrivenProperties.WeaponUnsteadyEndTime = 10f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.025f;
						if (weaponComponentData3.WeaponClass == WeaponClass.ThrowingAxe)
						{
							agentDrivenProperties.WeaponInaccuracy *= 6.6f;
							goto IL_06FE;
						}
						goto IL_06FE;
					}
					agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.1f;
					agentDrivenProperties.WeaponUnsteadyBeginTime = 0f;
					agentDrivenProperties.WeaponUnsteadyEndTime = 0f;
					agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.1f;
				}
				else if (weaponComponentData3.WeaponFlags.HasAllFlags(WeaponFlags.WideGrip))
				{
					flag2 = true;
					agentDrivenProperties.WeaponUnsteadyBeginTime = 1f + (float)effectiveSkillForWeapon * 0.005f;
					agentDrivenProperties.WeaponUnsteadyEndTime = 3f + (float)effectiveSkillForWeapon * 0.01f;
				}
			}
			IL_06FE:
			agentDrivenProperties.AttributeShieldMissileCollisionBodySizeAdder = 0.3f;
			Agent mountAgent = agent.MountAgent;
			float num11 = ((mountAgent != null) ? mountAgent.GetAgentDrivenPropertyValue(DrivenProperty.AttributeRiding) : 1f);
			agentDrivenProperties.AttributeRiding = (float)skillValue * num11;
			agentDrivenProperties.AttributeHorseArchery = MissionGameModels.Current.StrikeMagnitudeModel.CalculateHorseArcheryFactor(character);
			agentDrivenProperties.BipedalRangedReadySpeedMultiplier = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRangedReadySpeedMultiplier);
			agentDrivenProperties.BipedalRangedReloadSpeedMultiplier = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRangedReloadSpeedMultiplier);
			if (perkHandler != null)
			{
				for (int i = 64; i < 98; i++)
				{
					DrivenProperty drivenProperty = (DrivenProperty)i;
					if (((drivenProperty != DrivenProperty.WeaponUnsteadyBeginTime && drivenProperty != DrivenProperty.WeaponUnsteadyEndTime) || flag || flag2) && (drivenProperty != DrivenProperty.WeaponRotationalAccuracyPenaltyInRadians || flag))
					{
						float stat = agentDrivenProperties.GetStat(drivenProperty);
						agentDrivenProperties.SetStat(drivenProperty, stat + perkHandler.GetDrivenPropertyBonus(drivenProperty, stat));
					}
				}
			}
			if (agent.Character != null && agent.HasMount && weaponComponentData != null)
			{
				this.SetMountedWeaponPenaltiesOnAgent(agent, agentDrivenProperties, weaponComponentData);
			}
			base.SetAiRelatedProperties(agent, agentDrivenProperties, weaponComponentData, weaponComponentData2);
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x00068788 File Offset: 0x00066988
		private void FillAgentStatsFromData(ref AgentDrivenProperties agentDrivenProperties, Agent agent, MultiplayerClassDivisions.MPHeroClass heroClass, MissionPeer missionPeer, MissionPeer owningMissionPeer)
		{
			MissionPeer missionPeer2 = missionPeer ?? owningMissionPeer;
			if (missionPeer2 != null)
			{
				MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(missionPeer2);
				bool flag = missionPeer != null;
				for (int i = 0; i < 64; i++)
				{
					DrivenProperty drivenProperty = (DrivenProperty)i;
					float stat = agentDrivenProperties.GetStat(drivenProperty);
					if (drivenProperty == DrivenProperty.ArmorHead || drivenProperty == DrivenProperty.ArmorTorso || drivenProperty == DrivenProperty.ArmorLegs || drivenProperty == DrivenProperty.ArmorArms)
					{
						agentDrivenProperties.SetStat(drivenProperty, stat + (float)heroClass.ArmorValue + onSpawnPerkHandler.GetDrivenPropertyBonusOnSpawn(flag, drivenProperty, stat));
					}
					else
					{
						agentDrivenProperties.SetStat(drivenProperty, stat + onSpawnPerkHandler.GetDrivenPropertyBonusOnSpawn(flag, drivenProperty, stat));
					}
				}
			}
			float num = (heroClass.IsTroopCharacter(agent.Character) ? heroClass.TroopTopSpeedReachDuration : heroClass.HeroTopSpeedReachDuration);
			agentDrivenProperties.TopSpeedReachDuration = num;
			float managedParameter = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalCombatSpeedMinMultiplier);
			float managedParameter2 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalCombatSpeedMaxMultiplier);
			float num2 = (heroClass.IsTroopCharacter(agent.Character) ? heroClass.TroopCombatMovementSpeedMultiplier : heroClass.HeroCombatMovementSpeedMultiplier);
			agentDrivenProperties.CombatMaxSpeedMultiplier = managedParameter + (managedParameter2 - managedParameter) * num2;
			agentDrivenProperties.CrouchedSpeedMultiplier = 1f;
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0006889D File Offset: 0x00066A9D
		private int GetSkillValueForItem(BasicCharacterObject characterObject, ItemObject primaryItem)
		{
			return characterObject.GetSkillValue((primaryItem != null) ? primaryItem.RelevantSkill : DefaultSkills.Athletics);
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x000688B8 File Offset: 0x00066AB8
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

		// Token: 0x06001E6C RID: 7788 RVA: 0x000689AC File Offset: 0x00066BAC
		public static float CalculateMaximumSpeedMultiplier(Agent agent)
		{
			MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(agent.Character);
			if (!mpheroClassForCharacter.IsTroopCharacter(agent.Character))
			{
				return mpheroClassForCharacter.HeroMovementSpeedMultiplier;
			}
			return mpheroClassForCharacter.TroopMovementSpeedMultiplier;
		}
	}
}
