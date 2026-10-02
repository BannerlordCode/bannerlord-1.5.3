using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.GameComponents
{
	// Token: 0x020000C4 RID: 196
	public class SandboxAgentStatCalculateModel : AgentStatCalculateModel
	{
		// Token: 0x0600080D RID: 2061 RVA: 0x00036C68 File Offset: 0x00034E68
		public override float GetDifficultyModifier()
		{
			Campaign campaign = Campaign.Current;
			float? num;
			if (campaign == null)
			{
				num = null;
			}
			else
			{
				GameModels models = campaign.Models;
				if (models == null)
				{
					num = null;
				}
				else
				{
					DifficultyModel difficultyModel = models.DifficultyModel;
					num = ((difficultyModel != null) ? new float?(difficultyModel.GetCombatAIDifficultyMultiplier()) : null);
				}
			}
			float? num2 = num;
			if (num2 == null)
			{
				return 1f;
			}
			return num2.GetValueOrDefault();
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00036CD2 File Offset: 0x00034ED2
		public override bool CanAgentRideMount(Agent agent, Agent targetMount)
		{
			return agent.CheckSkillForMounting(targetMount);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00036CDC File Offset: 0x00034EDC
		public override void InitializeAgentStats(Agent agent, Equipment spawnEquipment, AgentDrivenProperties agentDrivenProperties, AgentBuildData agentBuildData)
		{
			BattleEnvironment currentBattleEnvironment = agent.CurrentBattleEnvironment;
			agentDrivenProperties.ArmorEncumbrance = this.GetEffectiveArmorEncumbrance(agent, spawnEquipment);
			if (agent.IsHero)
			{
				CharacterObject characterObject = agent.Character as CharacterObject;
				AgentFlag agentFlag = agent.GetAgentFlags();
				float num;
				if (characterObject.GetPerkValue(DefaultPerks.Bow.HorseMaster, currentBattleEnvironment, true, out num))
				{
					agentFlag |= AgentFlag.CanUseAllBowsMounted;
				}
				if (characterObject.GetPerkValue(DefaultPerks.Crossbow.MountedCrossbowman, currentBattleEnvironment, true, out num))
				{
					agentFlag |= AgentFlag.CanReloadAllXBowsMounted;
				}
				if (characterObject.GetPerkValue(DefaultPerks.TwoHanded.ProjectileDeflection, currentBattleEnvironment, true, out num))
				{
					agentFlag |= AgentFlag.CanDeflectArrowsWith2HSword;
				}
				agent.SetAgentFlags(agentFlag);
			}
			else
			{
				agent.HealthLimit = this.GetEffectiveMaxHealth(agent);
				agent.Health = agent.HealthLimit;
			}
			agentDrivenProperties.OffhandWeaponDefendSpeedMultiplier = 1f;
			MissionGameModels.Current.AgentStatCalculateModel.UpdateAgentStats(agent, agentDrivenProperties);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00036DA0 File Offset: 0x00034FA0
		public override void InitializeMissionEquipment(Agent agent)
		{
			if (agent.IsHuman)
			{
				CharacterObject characterObject = agent.Character as CharacterObject;
				if (characterObject != null)
				{
					BattleEnvironment currentBattleEnvironment = agent.CurrentBattleEnvironment;
					object obj;
					if (agent == null)
					{
						obj = null;
					}
					else
					{
						IAgentOriginBase origin = agent.Origin;
						obj = ((origin != null) ? origin.BattleCombatant : null);
					}
					PartyBase partyBase = (PartyBase)obj;
					MapEvent mapEvent = ((partyBase != null) ? partyBase.MapEvent : null);
					MobileParty mobileParty = ((partyBase != null && partyBase.IsMobile) ? partyBase.MobileParty : null);
					CharacterObject characterObject2 = PartyBaseHelper.GetVisualPartyLeader(partyBase);
					if (characterObject2 == characterObject)
					{
						characterObject2 = null;
					}
					MissionEquipment equipment = agent.Equipment;
					for (int i = 0; i < 5; i++)
					{
						EquipmentIndex equipmentIndex = (EquipmentIndex)i;
						MissionWeapon missionWeapon = equipment[equipmentIndex];
						if (!missionWeapon.IsEmpty)
						{
							WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
							if (currentUsageItem != null)
							{
								if (currentUsageItem.IsConsumable && currentUsageItem.RelevantSkill != null)
								{
									ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
									if (currentUsageItem.RelevantSkill == DefaultSkills.Bow)
									{
										PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.DeepQuivers, currentBattleEnvironment, characterObject, true, ref explainedNumber);
										float num;
										if (characterObject2 != null && characterObject2.GetPerkValue(DefaultPerks.Bow.DeepQuivers, currentBattleEnvironment, false, out num))
										{
											explainedNumber.Add(DefaultPerks.Bow.DeepQuivers.SecondaryBonus, null, null);
										}
									}
									else if (currentUsageItem.RelevantSkill == DefaultSkills.Crossbow)
									{
										PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Fletcher, currentBattleEnvironment, characterObject, true, ref explainedNumber);
										float num;
										if (characterObject2 != null && characterObject2.GetPerkValue(DefaultPerks.Crossbow.Fletcher, currentBattleEnvironment, true, out num))
										{
											explainedNumber.Add(DefaultPerks.Crossbow.Fletcher.SecondaryBonus, null, null);
										}
									}
									else if (currentUsageItem.RelevantSkill == DefaultSkills.Throwing)
									{
										PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.WellPrepared, currentBattleEnvironment, characterObject, true, ref explainedNumber);
										PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Resourceful, currentBattleEnvironment, characterObject, true, ref explainedNumber);
										if (agent.HasMount)
										{
											PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Saddlebags, currentBattleEnvironment, characterObject, true, ref explainedNumber);
										}
										PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.WellPrepared, currentBattleEnvironment, mobileParty, false, ref explainedNumber);
									}
									int num2 = MathF.Round(explainedNumber.ResultNumber);
									ExplainedNumber explainedNumber2 = new ExplainedNumber((float)((int)missionWeapon.Amount + num2), false, null);
									if (mobileParty != null && mapEvent != null && mapEvent.AttackerSide == partyBase.MapEventSide && mapEvent.EventType == MapEvent.BattleTypes.Siege)
									{
										PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.MilitaryPlanner, currentBattleEnvironment, mobileParty, true, ref explainedNumber2);
									}
									int num3 = MathF.Round(explainedNumber2.ResultNumber);
									if (num3 != (int)missionWeapon.Amount)
									{
										equipment.SetAmountOfSlot(equipmentIndex, (short)num3, true);
									}
								}
								else if (currentUsageItem.IsShield)
								{
									ExplainedNumber explainedNumber3 = new ExplainedNumber((float)missionWeapon.HitPoints, false, null);
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Engineering.Scaffolds, currentBattleEnvironment, characterObject, false, ref explainedNumber3);
									int num4 = MathF.Round(explainedNumber3.ResultNumber);
									if (num4 != (int)missionWeapon.HitPoints)
									{
										equipment.SetHitPointsOfSlot(equipmentIndex, (short)num4, true);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0003704C File Offset: 0x0003524C
		public override void UpdateAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			if (agent.IsHuman)
			{
				this.UpdateHumanStats(agent, agentDrivenProperties);
				return;
			}
			this.UpdateHorseStats(agent, agentDrivenProperties);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00037068 File Offset: 0x00035268
		public override int GetEffectiveSkill(Agent agent, SkillObject skill)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)base.GetEffectiveSkill(agent, skill), false, null);
			CharacterObject characterObject = agent.Character as CharacterObject;
			Formation formation = agent.Formation;
			IAgentOriginBase origin = agent.Origin;
			PartyBase partyBase = (PartyBase)((origin != null) ? origin.BattleCombatant : null);
			MobileParty mobileParty = ((partyBase != null && partyBase.IsMobile) ? partyBase.MobileParty : null);
			BattleEnvironment currentBattleEnvironment = agent.CurrentBattleEnvironment;
			Agent agent2 = ((formation != null) ? formation.Captain : null);
			CharacterObject characterObject2 = ((agent2 != null) ? agent2.Character : null) as CharacterObject;
			if (characterObject2 == characterObject)
			{
				characterObject2 = null;
			}
			if (characterObject2 != null)
			{
				bool flag = skill == DefaultSkills.Bow || skill == DefaultSkills.Crossbow || skill == DefaultSkills.Throwing;
				bool flag2 = skill == DefaultSkills.OneHanded || skill == DefaultSkills.TwoHanded || skill == DefaultSkills.Polearm;
				if ((characterObject.IsInfantry && flag) || (characterObject.IsRanged && flag2))
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.FlexibleFighter, currentBattleEnvironment, characterObject2, ref explainedNumber);
				}
			}
			if (skill == DefaultSkills.Bow)
			{
				if (characterObject2 != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.DeadAim, currentBattleEnvironment, characterObject2, ref explainedNumber);
					if (characterObject.HasMount())
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.HorseMaster, currentBattleEnvironment, characterObject2, ref explainedNumber);
					}
				}
			}
			else if (skill == DefaultSkills.Throwing)
			{
				if (characterObject2 != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.StrongArms, currentBattleEnvironment, characterObject2, ref explainedNumber);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.RunningThrow, currentBattleEnvironment, characterObject2, ref explainedNumber);
				}
			}
			else if (skill == DefaultSkills.Crossbow && characterObject2 != null)
			{
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.DonkeysSwiftness, currentBattleEnvironment, characterObject2, ref explainedNumber);
			}
			if (mobileParty != null && characterObject.Occupation == Occupation.Bandit)
			{
				if (skill.Attributes.Any<CharacterAttribute>((CharacterAttribute attribute) => attribute == DefaultCharacterAttributes.Vigor || attribute == DefaultCharacterAttributes.Control))
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.OneOfTheFamily, agent.CurrentBattleEnvironment, mobileParty, true, ref explainedNumber);
				}
			}
			if (characterObject.HasMount())
			{
				if (skill == DefaultSkills.Riding && characterObject2 != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.NimbleSteed, currentBattleEnvironment, characterObject2, ref explainedNumber);
				}
			}
			else
			{
				if (mobileParty != null && formation != null)
				{
					bool flag3 = skill == DefaultSkills.OneHanded || skill == DefaultSkills.TwoHanded || skill == DefaultSkills.Polearm;
					bool flag4 = formation.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall;
					if (flag3 && flag4)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Polearm.Phalanx, currentBattleEnvironment, mobileParty, true, ref explainedNumber);
					}
				}
				if (characterObject2 != null)
				{
					if (skill == DefaultSkills.OneHanded)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.WrappedHandles, currentBattleEnvironment, characterObject2, ref explainedNumber);
					}
					else if (skill == DefaultSkills.TwoHanded)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.StrongGrip, currentBattleEnvironment, characterObject2, ref explainedNumber);
					}
					else if (skill == DefaultSkills.Polearm)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.CleanThrust, currentBattleEnvironment, characterObject2, ref explainedNumber);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.CounterWeight, currentBattleEnvironment, characterObject2, ref explainedNumber);
					}
				}
			}
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00037318 File Offset: 0x00035518
		public override float GetWeaponDamageMultiplier(Agent agent, WeaponComponentData weapon)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			SkillObject skillObject = ((weapon != null) ? weapon.RelevantSkill : null);
			if (agent.Character is CharacterObject && skillObject != null)
			{
				if (skillObject == DefaultSkills.OneHanded)
				{
					int effectiveSkill = this.GetEffectiveSkill(agent, skillObject);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.OneHandedDamage, ref explainedNumber, effectiveSkill);
				}
				else if (skillObject == DefaultSkills.TwoHanded)
				{
					int effectiveSkill2 = this.GetEffectiveSkill(agent, skillObject);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.TwoHandedDamage, ref explainedNumber, effectiveSkill2);
				}
				else if (skillObject == DefaultSkills.Polearm)
				{
					int effectiveSkill3 = this.GetEffectiveSkill(agent, skillObject);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.PolearmDamage, ref explainedNumber, effectiveSkill3);
				}
				else if (skillObject == DefaultSkills.Bow)
				{
					int effectiveSkill4 = this.GetEffectiveSkill(agent, skillObject);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.BowDamage, ref explainedNumber, effectiveSkill4);
				}
				else if (skillObject == DefaultSkills.Throwing)
				{
					int effectiveSkill5 = this.GetEffectiveSkill(agent, skillObject);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.ThrowingDamage, ref explainedNumber, effectiveSkill5);
				}
			}
			return Math.Max(0f, explainedNumber.ResultNumber);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0003740E File Offset: 0x0003560E
		private float GetArmorStealthBonus(EquipmentElement armorElement, int maxBodyPartBonus)
		{
			if (!armorElement.IsEmpty && armorElement.Item != null && armorElement.Item.HasArmorComponent)
			{
				return (float)armorElement.GetModifiedStealthFactor();
			}
			return 0f;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00037440 File Offset: 0x00035640
		public override float GetEquipmentStealthBonus(Agent agent)
		{
			Equipment spawnEquipment = agent.SpawnEquipment;
			return 0f + this.GetArmorStealthBonus(spawnEquipment[EquipmentIndex.NumAllWeaponSlots], 25) + this.GetArmorStealthBonus(spawnEquipment[EquipmentIndex.Cape], 15) + this.GetArmorStealthBonus(spawnEquipment[EquipmentIndex.Body], 30) + this.GetArmorStealthBonus(spawnEquipment[EquipmentIndex.Gloves], 10) + this.GetArmorStealthBonus(spawnEquipment[EquipmentIndex.Leg], 20);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x000374AC File Offset: 0x000356AC
		public override float GetSneakAttackMultiplier(Agent agent, WeaponComponentData weapon)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			if (weapon != null && agent.Character is CharacterObject)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Roguery);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.SneakDamage, ref explainedNumber, effectiveSkill);
				if ((weapon != null && weapon.WeaponClass == WeaponClass.Dagger) || (weapon != null && weapon.WeaponClass == WeaponClass.ThrowingKnife))
				{
					explainedNumber.AddFactor(2f, null);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00037520 File Offset: 0x00035720
		public override float GetKnockBackResistance(Agent agent)
		{
			if (agent.IsHuman)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Athletics);
				return DefaultSkillEffects.KnockBackResistance.GetSkillEffectValue(effectiveSkill);
			}
			return float.MaxValue;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00037554 File Offset: 0x00035754
		public override float GetKnockDownResistance(Agent agent, StrikeType strikeType = StrikeType.Invalid)
		{
			if (agent.IsHuman)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Athletics);
				float num = DefaultSkillEffects.KnockDownResistance.GetSkillEffectValue(effectiveSkill);
				if (agent.HasMount)
				{
					num += 0.1f;
				}
				else if (strikeType == StrikeType.Thrust)
				{
					num += 0.15f;
				}
				return num;
			}
			return float.MaxValue;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x000375A8 File Offset: 0x000357A8
		public override float GetDismountResistance(Agent agent)
		{
			if (agent.IsHuman)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Riding);
				return DefaultSkillEffects.DismountResistance.GetSkillEffectValue(effectiveSkill);
			}
			return float.MaxValue;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x000375DB File Offset: 0x000357DB
		public override float GetBreatheHoldMaxDuration(Agent agent, float baseBreatheHoldMaxDuration)
		{
			return baseBreatheHoldMaxDuration;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x000375E0 File Offset: 0x000357E0
		public override float GetWeaponInaccuracy(Agent agent, WeaponComponentData weapon, int weaponSkill)
		{
			CharacterObject characterObject = agent.Character as CharacterObject;
			Formation formation = agent.Formation;
			Agent agent2 = ((formation != null) ? formation.Captain : null);
			CharacterObject characterObject2 = ((agent2 != null) ? agent2.Character : null) as CharacterObject;
			if (characterObject == characterObject2)
			{
				characterObject2 = null;
			}
			float num = 0f;
			if (weapon.IsRangedWeapon)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
				if (characterObject != null)
				{
					if (weapon.RelevantSkill == DefaultSkills.Bow)
					{
						SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.BowAccuracy, ref explainedNumber, weaponSkill);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.QuickAdjustments, agent.CurrentBattleEnvironment, characterObject2, ref explainedNumber);
					}
					else if (weapon.RelevantSkill == DefaultSkills.Crossbow)
					{
						SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.CrossbowAccuracy, ref explainedNumber, weaponSkill);
					}
					else if (weapon.RelevantSkill == DefaultSkills.Throwing)
					{
						if (weapon.WeaponClass == WeaponClass.Sling)
						{
							explainedNumber = new ExplainedNumber(MathF.Max(1f - 0.007f * (float)weaponSkill, 0.2f), false, null);
						}
						else
						{
							SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.ThrowingAccuracy, ref explainedNumber, weaponSkill);
						}
					}
				}
				num = (100f - (float)weapon.Accuracy) * explainedNumber.ResultNumber * 0.001f;
			}
			else if (weapon.WeaponFlags.HasAllFlags(WeaponFlags.WideGrip))
			{
				num = 1f - (float)weaponSkill * 0.01f;
			}
			return MathF.Max(num, 0f);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00037724 File Offset: 0x00035924
		public override float GetInteractionDistance(Agent agent)
		{
			CharacterObject characterObject;
			float num;
			if (agent.HasMount && (characterObject = agent.Character as CharacterObject) != null && characterObject.GetPerkValue(DefaultPerks.Throwing.LongReach, agent.CurrentBattleEnvironment, true, out num))
			{
				return 3f;
			}
			return base.GetInteractionDistance(agent);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0003776C File Offset: 0x0003596C
		public override float GetMaxCameraZoom(Agent agent)
		{
			CharacterObject characterObject = agent.Character as CharacterObject;
			BattleEnvironment currentBattleEnvironment = agent.CurrentBattleEnvironment;
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			if (characterObject != null)
			{
				MissionEquipment equipment = agent.Equipment;
				EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
				WeaponComponentData weaponComponentData = ((primaryWieldedItemIndex != EquipmentIndex.None) ? equipment[primaryWieldedItemIndex].CurrentUsageItem : null);
				if (weaponComponentData != null)
				{
					if (weaponComponentData.RelevantSkill == DefaultSkills.Bow)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.EagleEye, currentBattleEnvironment, characterObject, true, ref explainedNumber);
					}
					else if (weaponComponentData.RelevantSkill == DefaultSkills.Crossbow)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.LongShots, currentBattleEnvironment, characterObject, true, ref explainedNumber);
					}
					else if (weaponComponentData.RelevantSkill == DefaultSkills.Throwing)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Focus, currentBattleEnvironment, characterObject, true, ref explainedNumber);
					}
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00037834 File Offset: 0x00035A34
		public List<PerkObject> GetPerksOfAgent(CharacterObject agentCharacter, SkillObject skill = null, bool filterPartyRole = false, PartyRole partyRole = PartyRole.Personal)
		{
			List<PerkObject> list = new List<PerkObject>();
			if (agentCharacter != null)
			{
				foreach (PerkObject perkObject in PerkObject.All)
				{
					if (agentCharacter.GetPerkValue(perkObject) && (skill == null || skill == perkObject.Skill))
					{
						if (filterPartyRole)
						{
							if (perkObject.PrimaryRole == partyRole || perkObject.SecondaryRole == partyRole)
							{
								list.Add(perkObject);
							}
						}
						else
						{
							list.Add(perkObject);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x000378C8 File Offset: 0x00035AC8
		public override string GetMissionDebugInfoForAgent(Agent agent)
		{
			string text = "";
			text += "Base: Initial stats modified only by skills\n";
			text += "Effective (Eff): Stats that are modified by perks & mission effects\n\n";
			string text2 = "{0,-20}";
			text = string.Concat(new string[]
			{
				text,
				string.Format(text2, "Name"),
				": ",
				agent.Name,
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				string.Format(text2, "Age"),
				": ",
				(int)agent.Age,
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				string.Format(text2, "Health"),
				": ",
				agent.Health,
				"\n"
			});
			int num = (agent.IsHuman ? agent.Character.MaxHitPoints() : agent.Monster.HitPoints);
			text = string.Concat(new object[]
			{
				text,
				string.Format(text2, "Max.Health"),
				": ",
				num,
				"(Base)\n"
			});
			text = string.Concat(new object[]
			{
				text,
				string.Format(text2, ""),
				"  ",
				MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveMaxHealth(agent),
				"(Eff)\n"
			});
			text = string.Concat(new string[]
			{
				text,
				string.Format(text2, "Team"),
				": ",
				(agent.Team != null) ? (agent.Team.IsAttacker ? "Attacker" : "Defender") : "N/A",
				"\n"
			});
			if (agent.IsHuman)
			{
				string text3 = text2 + ": {1,4:G}, {2,4:G}";
				text += "-------------------------------------\n";
				text = text + string.Format(text2 + ": {1,4}, {2,4}", "Skills", "Base", "Eff") + "\n";
				text += "-------------------------------------\n";
				foreach (SkillObject skillObject in Skills.All)
				{
					int skillValue = agent.Character.GetSkillValue(skillObject);
					int effectiveSkill = MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveSkill(agent, skillObject);
					string text4 = string.Format(text3, skillObject.Name, skillValue, effectiveSkill);
					text = text + text4 + "\n";
				}
				text += "-------------------------------------\n";
				CharacterObject characterObject = agent.Character as CharacterObject;
				string debugPerkInfoForAgent = this.GetDebugPerkInfoForAgent(characterObject, false, PartyRole.Personal);
				if (debugPerkInfoForAgent.Length > 0)
				{
					text = text + string.Format(text2 + ": ", "Perks") + "\n";
					text += "-------------------------------------\n";
					text += debugPerkInfoForAgent;
					text += "-------------------------------------\n";
				}
				Formation formation = agent.Formation;
				object obj;
				if (formation == null)
				{
					obj = null;
				}
				else
				{
					Agent captain = formation.Captain;
					obj = ((captain != null) ? captain.Character : null);
				}
				CharacterObject characterObject2 = obj as CharacterObject;
				string debugPerkInfoForAgent2 = this.GetDebugPerkInfoForAgent(characterObject2, true, PartyRole.Captain);
				if (debugPerkInfoForAgent2.Length > 0)
				{
					text = string.Concat(new object[]
					{
						text,
						string.Format(text2 + ": ", "Captain Perks"),
						characterObject2.Name,
						"\n"
					});
					text += "-------------------------------------\n";
					text += debugPerkInfoForAgent2;
					text += "-------------------------------------\n";
				}
				IAgentOriginBase origin = agent.Origin;
				PartyBase partyBase = (PartyBase)((origin != null) ? origin.BattleCombatant : null);
				PartyBase partyBase2;
				if (partyBase == null)
				{
					partyBase2 = null;
				}
				else
				{
					MobileParty mobileParty = partyBase.MobileParty;
					partyBase2 = ((mobileParty != null) ? mobileParty.Party : null);
				}
				CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(partyBase2);
				string debugPerkInfoForAgent3 = this.GetDebugPerkInfoForAgent(visualPartyLeader, true, PartyRole.PartyLeader);
				if (debugPerkInfoForAgent3.Length > 0)
				{
					text = string.Concat(new object[]
					{
						text,
						string.Format(text2 + ": ", "Party Leader Perks"),
						visualPartyLeader.Name,
						"\n"
					});
					text += "-------------------------------------\n";
					text += debugPerkInfoForAgent3;
					text += "-------------------------------------\n";
				}
			}
			return text;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00037D3C File Offset: 0x00035F3C
		public override float GetEffectiveArmorEncumbrance(Agent agent, Equipment equipment)
		{
			float totalWeightOfArmor = equipment.GetTotalWeightOfArmor(agent.IsHuman);
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			CharacterObject characterObject;
			if ((characterObject = agent.Character as CharacterObject) != null)
			{
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.FormFittingArmor, agent.CurrentBattleEnvironment, characterObject, true, ref explainedNumber);
			}
			return MathF.Max(0f, totalWeightOfArmor * explainedNumber.ResultNumber);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00037D9C File Offset: 0x00035F9C
		public override float GetEffectiveMaxHealth(Agent agent)
		{
			if (agent.IsHero)
			{
				return (float)agent.Character.MaxHitPoints();
			}
			float baseHealthLimit = agent.BaseHealthLimit;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseHealthLimit, false, null);
			if (agent.IsHuman)
			{
				CharacterObject characterObject = agent.Character as CharacterObject;
				BattleEnvironment currentBattleEnvironment = agent.CurrentBattleEnvironment;
				IAgentOriginBase agentOriginBase = ((agent != null) ? agent.Origin : null);
				PartyBase partyBase = (PartyBase)((agentOriginBase != null) ? agentOriginBase.BattleCombatant : null);
				MobileParty mobileParty = ((partyBase != null) ? partyBase.MobileParty : null);
				CharacterObject characterObject2;
				if (mobileParty == null)
				{
					characterObject2 = null;
				}
				else
				{
					Hero leaderHero = mobileParty.LeaderHero;
					characterObject2 = ((leaderHero != null) ? leaderHero.CharacterObject : null);
				}
				CharacterObject characterObject3 = characterObject2;
				if (characterObject != null && characterObject3 != null)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.TwoHanded.ThickHides, currentBattleEnvironment, mobileParty, false, ref explainedNumber);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Polearm.HardyFrontline, currentBattleEnvironment, mobileParty, true, ref explainedNumber);
					if (characterObject.IsRanged)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Crossbow.PickedShots, currentBattleEnvironment, mobileParty, false, ref explainedNumber);
					}
					if (!agent.HasMount)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.WellBuilt, currentBattleEnvironment, mobileParty, false, ref explainedNumber);
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Polearm.HardKnock, currentBattleEnvironment, mobileParty, false, ref explainedNumber);
						if (characterObject.IsInfantry)
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.UnwaveringDefense, currentBattleEnvironment, mobileParty, false, ref explainedNumber);
						}
					}
					PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Medicine.MinisterOfHealth, agent.CurrentBattleEnvironment, characterObject3, DefaultSkills.Medicine, true, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
				}
			}
			else
			{
				Agent riderAgent = agent.RiderAgent;
				if (riderAgent != null)
				{
					BattleEnvironment currentBattleEnvironment2 = riderAgent.CurrentBattleEnvironment;
					CharacterObject characterObject4 = ((riderAgent != null) ? riderAgent.Character : null) as CharacterObject;
					object obj;
					if (riderAgent == null)
					{
						obj = null;
					}
					else
					{
						IAgentOriginBase origin = riderAgent.Origin;
						obj = ((origin != null) ? origin.BattleCombatant : null);
					}
					PartyBase partyBase2 = (PartyBase)obj;
					MobileParty mobileParty2 = ((partyBase2 != null) ? partyBase2.MobileParty : null);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.Sledges, currentBattleEnvironment2, mobileParty2, false, ref explainedNumber);
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.Veterinary, currentBattleEnvironment2, characterObject4, true, ref explainedNumber);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Riding.Veterinary, currentBattleEnvironment2, mobileParty2, false, ref explainedNumber);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00037F8C File Offset: 0x0003618C
		public override float GetEnvironmentSpeedFactor(Agent agent)
		{
			Scene scene = agent.Mission.Scene;
			float num = 1f;
			if (!agent.Mission.Scene.IsAtmosphereIndoor)
			{
				if (agent.Mission.Scene.GetRainDensity() > 0f)
				{
					num *= 0.9f;
				}
				if (!agent.IsHuman && CampaignTime.Now.IsNightTime)
				{
					num *= 0.9f;
				}
			}
			return num;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00037FFC File Offset: 0x000361FC
		private string GetDebugPerkInfoForAgent(CharacterObject agentCharacter, bool filterPartyRole = false, PartyRole partyRole = PartyRole.Personal)
		{
			string text = "";
			string text2 = "{0,-18}";
			if (this.GetPerksOfAgent(agentCharacter, null, filterPartyRole, partyRole).Count > 0)
			{
				foreach (SkillObject skillObject in Skills.All)
				{
					List<PerkObject> perksOfAgent = this.GetPerksOfAgent(agentCharacter, skillObject, filterPartyRole, partyRole);
					if (perksOfAgent != null && perksOfAgent.Count > 0)
					{
						string text3 = string.Format(text2, skillObject.Name) + ": ";
						int num = 5;
						int num2 = 0;
						foreach (PerkObject perkObject in perksOfAgent)
						{
							string text4 = perkObject.Name.ToString();
							if (num2 == num)
							{
								text3 = text3 + "\n" + string.Format(text2, "") + "  ";
								num2 = 0;
							}
							text3 = text3 + text4 + ", ";
							num2++;
						}
						text3 = text3.Remove(text3.LastIndexOf(","));
						text = text + text3 + "\n";
					}
				}
			}
			return text;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00038150 File Offset: 0x00036350
		private void UpdateHumanStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			Equipment spawnEquipment = agent.SpawnEquipment;
			agentDrivenProperties.ArmorHead = spawnEquipment.GetHeadArmorSum();
			agentDrivenProperties.ArmorTorso = spawnEquipment.GetHumanBodyArmorSum();
			agentDrivenProperties.ArmorLegs = spawnEquipment.GetLegArmorSum();
			agentDrivenProperties.ArmorArms = spawnEquipment.GetArmArmorSum();
			BasicCharacterObject character = agent.Character;
			BattleEnvironment currentBattleEnvironment = agent.CurrentBattleEnvironment;
			CharacterObject characterObject = character as CharacterObject;
			MissionEquipment equipment = agent.Equipment;
			float num = equipment.GetTotalWeightOfWeapons();
			float effectiveArmorEncumbrance = this.GetEffectiveArmorEncumbrance(agent, spawnEquipment);
			int weight = agent.Monster.Weight;
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			EquipmentIndex offhandWieldedItemIndex = agent.GetOffhandWieldedItemIndex();
			if (primaryWieldedItemIndex != EquipmentIndex.None)
			{
				ItemObject item = equipment[primaryWieldedItemIndex].Item;
				WeaponComponent weaponComponent = item.WeaponComponent;
				if (weaponComponent != null)
				{
					ItemObject.ItemTypeEnum itemType = weaponComponent.GetItemType();
					bool flag = false;
					if (characterObject != null)
					{
						float num2;
						bool flag2 = itemType == ItemObject.ItemTypeEnum.Bow && characterObject.GetPerkValue(DefaultPerks.Bow.RangersSwiftness, currentBattleEnvironment, true, out num2);
						bool flag3 = itemType == ItemObject.ItemTypeEnum.Crossbow && characterObject.GetPerkValue(DefaultPerks.Crossbow.LooseAndMove, currentBattleEnvironment, true, out num2);
						flag = flag2 || flag3;
					}
					if (!flag)
					{
						float realWeaponLength = weaponComponent.PrimaryWeapon.GetRealWeaponLength();
						num += 4f * MathF.Sqrt(realWeaponLength) * item.Weight;
					}
				}
			}
			if (offhandWieldedItemIndex != EquipmentIndex.None)
			{
				ItemObject item2 = equipment[offhandWieldedItemIndex].Item;
				WeaponComponentData primaryWeapon = item2.PrimaryWeapon;
				float num2;
				if (primaryWeapon != null && primaryWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanBlockRanged) && (characterObject == null || !characterObject.GetPerkValue(DefaultPerks.OneHanded.ShieldBearer, currentBattleEnvironment, true, out num2)))
				{
					num += 1.5f * item2.Weight;
				}
			}
			agentDrivenProperties.WeaponExternalAccelerationAccuracyPenalty = 0f;
			agentDrivenProperties.WeaponsEncumbrance = num;
			agentDrivenProperties.ArmorEncumbrance = effectiveArmorEncumbrance;
			float num3 = effectiveArmorEncumbrance + num;
			EquipmentIndex primaryWieldedItemIndex2 = agent.GetPrimaryWieldedItemIndex();
			WeaponComponentData weaponComponentData = ((primaryWieldedItemIndex2 != EquipmentIndex.None) ? equipment[primaryWieldedItemIndex2].CurrentUsageItem : null);
			EquipmentIndex offhandWieldedItemIndex2 = agent.GetOffhandWieldedItemIndex();
			WeaponComponentData weaponComponentData2 = ((offhandWieldedItemIndex2 != EquipmentIndex.None) ? equipment[offhandWieldedItemIndex2].CurrentUsageItem : null);
			agentDrivenProperties.SwingSpeedMultiplier = 0.93f;
			agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = 0.93f;
			agentDrivenProperties.HandlingMultiplier = 1f;
			agentDrivenProperties.ShieldBashStunDurationMultiplier = 1f;
			agentDrivenProperties.KickStunDurationMultiplier = 1f;
			agentDrivenProperties.ReloadSpeed = 0.93f;
			agentDrivenProperties.MissileSpeedMultiplier = 1f;
			agentDrivenProperties.ReloadMovementPenaltyFactor = 1f;
			agentDrivenProperties.DamageMultiplierBonus = 0f;
			base.SetAllWeaponInaccuracy(agent, agentDrivenProperties, (int)primaryWieldedItemIndex2, weaponComponentData);
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
						float num4 = MathF.Max(0f, 1f - (float)effectiveSkillForWeapon / 500f);
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = MathF.Max(0f, 0.125f * num4);
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = MathF.Max(0f, 0.1f * num4);
					}
					else
					{
						float num5 = MathF.Max(0f, (1f - (float)effectiveSkillForWeapon / 500f) * (1f - (float)effectiveSkill2 / 1800f));
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = MathF.Max(0f, 0.025f * num5);
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = MathF.Max(0f, 0.12f * num5);
					}
					if (weaponComponentData3.RelevantSkill == DefaultSkills.Bow)
					{
						float num6 = ((float)thrustSpeed - 45f) / 90f;
						num6 = MBMath.ClampFloat(num6, 0f, 1f);
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 6f;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 4.5f / MBMath.Lerp(0.75f, 2f, num6, 1E-05f);
					}
					else if (weaponComponentData3.RelevantSkill == DefaultSkills.Throwing)
					{
						if (weaponComponentData3.WeaponClass == WeaponClass.Sling)
						{
							float num7 = ((float)thrustSpeed - 30f) / 90f;
							num7 = MBMath.ClampFloat(num7, 0f, 1f);
							agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 5f;
							agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 2.4f * MBMath.Lerp(2.4f, 1.2f, num7, 1E-05f);
						}
						else
						{
							float num8 = ((float)thrustSpeed - 91f) / 14f;
							num8 = MBMath.ClampFloat(num8, 0f, 1f);
							agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 0.5f;
							agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 1.5f * MBMath.Lerp(1.5f, 0.8f, num8, 1E-05f);
						}
					}
					else if (weaponComponentData3.RelevantSkill == DefaultSkills.Crossbow)
					{
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 2.5f;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 1.2f;
					}
					if (weaponComponentData3.WeaponClass == WeaponClass.Bow)
					{
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.3f + (95.75f - (float)thrustSpeed) * 0.005f;
						float num9 = ((float)thrustSpeed - 45f) / 90f;
						num9 = MBMath.ClampFloat(num9, 0f, 1f);
						agentDrivenProperties.WeaponUnsteadyBeginTime = 0.6f + (float)effectiveSkillForWeapon * 0.01f * MBMath.Lerp(2f, 4f, num9, 1E-05f);
						if (agent.IsAIControlled)
						{
							agentDrivenProperties.WeaponUnsteadyBeginTime *= 4f;
						}
						agentDrivenProperties.WeaponUnsteadyEndTime = 2f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.1f;
					}
					else if (weaponComponentData3.WeaponClass == WeaponClass.Javelin || weaponComponentData3.WeaponClass == WeaponClass.ThrowingAxe || weaponComponentData3.WeaponClass == WeaponClass.ThrowingKnife)
					{
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.2f + (89f - (float)thrustSpeed) * 0.009f;
						agentDrivenProperties.WeaponUnsteadyBeginTime = 2.5f + (float)effectiveSkillForWeapon * 0.01f;
						agentDrivenProperties.WeaponUnsteadyEndTime = 10f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.025f;
					}
					else if (weaponComponentData3.WeaponClass == WeaponClass.Sling)
					{
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 2.6f + (89f - (float)thrustSpeed) * 0.12f;
						agentDrivenProperties.WeaponUnsteadyBeginTime = 3f + (float)effectiveSkillForWeapon * 0.064f;
						agentDrivenProperties.WeaponUnsteadyEndTime = 22f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.2f;
					}
					else
					{
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.1f;
						agentDrivenProperties.WeaponUnsteadyBeginTime = 0f;
						agentDrivenProperties.WeaponUnsteadyEndTime = 0f;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.1f;
					}
				}
				else if (weaponComponentData3.WeaponFlags.HasAllFlags(WeaponFlags.WideGrip))
				{
					agentDrivenProperties.WeaponUnsteadyBeginTime = 1f + (float)effectiveSkillForWeapon * 0.005f;
					agentDrivenProperties.WeaponUnsteadyEndTime = 3f + (float)effectiveSkillForWeapon * 0.01f;
				}
			}
			agentDrivenProperties.TopSpeedReachDuration = 2.5f + MathF.Max(5f - (1f + (float)effectiveSkill * 0.01f), 1f) / 3.5f - MathF.Min((float)weight / ((float)weight + num3), 0.8f);
			ExplainedNumber explainedNumber = new ExplainedNumber(0.7f, false, null);
			ExplainedNumber explainedNumber2 = new ExplainedNumber(0.7f, false, null);
			SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.AthleticsSpeedFactor, ref explainedNumber, effectiveSkill);
			SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.AthleticsSpeedFactor, ref explainedNumber2, 300);
			ExplainedNumber explainedNumber3 = new ExplainedNumber(0.2f, false, null);
			explainedNumber3.LimitMin(0f);
			SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.AthleticsWeightFactor, ref explainedNumber3, effectiveSkill);
			float num10 = explainedNumber3.ResultNumber * num3 / (float)weight;
			float num11 = MBMath.ClampFloat(explainedNumber.ResultNumber - num10, 0f, explainedNumber2.ResultNumber);
			agentDrivenProperties.MaxSpeedMultiplier = this.GetEnvironmentSpeedFactor(agent) * num11;
			float managedParameter = TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.Core.ManagedParametersEnum.BipedalCombatSpeedMinMultiplier);
			float managedParameter2 = TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.Core.ManagedParametersEnum.BipedalCombatSpeedMaxMultiplier);
			float num12 = MathF.Min(num3 / (float)weight, 1f);
			agentDrivenProperties.CombatMaxSpeedMultiplier = MathF.Min(MBMath.Lerp(managedParameter2, managedParameter, num12, 1E-05f), 1f);
			agentDrivenProperties.CrouchedSpeedMultiplier = 1f;
			agentDrivenProperties.AttributeShieldMissileCollisionBodySizeAdder = 0.3f;
			Agent mountAgent = agent.MountAgent;
			float num13 = ((mountAgent != null) ? mountAgent.GetAgentDrivenPropertyValue(DrivenProperty.AttributeRiding) : 1f);
			agentDrivenProperties.AttributeRiding = (float)effectiveSkill2 * num13;
			agentDrivenProperties.AttributeHorseArchery = MissionGameModels.Current.StrikeMagnitudeModel.CalculateHorseArcheryFactor(character);
			agentDrivenProperties.BipedalRangedReadySpeedMultiplier = TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.Core.ManagedParametersEnum.BipedalRangedReadySpeedMultiplier);
			agentDrivenProperties.BipedalRangedReloadSpeedMultiplier = TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.Core.ManagedParametersEnum.BipedalRangedReloadSpeedMultiplier);
			if (characterObject != null)
			{
				if (weaponComponentData != null)
				{
					this.SetWeaponSkillEffectsOnAgent(agent, characterObject, agentDrivenProperties, weaponComponentData);
					if (agent.HasMount)
					{
						this.SetMountedPenaltiesOnAgent(agent, agentDrivenProperties, weaponComponentData);
					}
				}
				this.SetPerkAndBannerEffectsOnAgent(agent, characterObject, agentDrivenProperties, weaponComponentData);
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
				if (CampaignTime.Now.IsNightTime)
				{
					num14 += 0.1f;
				}
			}
			agentDrivenProperties.AiShooterError *= num14;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00038A8C File Offset: 0x00036C8C
		private void UpdateHorseStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			Equipment spawnEquipment = agent.SpawnEquipment;
			EquipmentElement equipmentElement = spawnEquipment[EquipmentIndex.ArmorItemEndSlot];
			ItemObject item = equipmentElement.Item;
			EquipmentElement equipmentElement2 = spawnEquipment[EquipmentIndex.HorseHarness];
			agentDrivenProperties.AiSpeciesIndex = (int)item.Id.InternalValue;
			agentDrivenProperties.AttributeRiding = 0.8f + ((equipmentElement2.Item != null) ? 0.2f : 0f);
			float num = 0f;
			for (int i = 1; i < 12; i++)
			{
				if (spawnEquipment[i].Item != null)
				{
					num += (float)spawnEquipment[i].GetModifiedMountBodyArmor();
				}
			}
			agentDrivenProperties.ArmorTorso = num;
			int modifiedMountManeuver = equipmentElement.GetModifiedMountManeuver(in equipmentElement2);
			int num2 = equipmentElement.GetModifiedMountSpeed(in equipmentElement2) + 1;
			int num3 = 0;
			float environmentSpeedFactor = this.GetEnvironmentSpeedFactor(agent);
			bool flag = Campaign.Current.Models.MapWeatherModel.GetWeatherEffectOnTerrainForPosition(MobileParty.MainParty.Position.ToVec2()) == MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			Agent riderAgent = agent.RiderAgent;
			if (riderAgent != null)
			{
				CharacterObject characterObject = riderAgent.Character as CharacterObject;
				BattleEnvironment currentBattleEnvironment = riderAgent.CurrentBattleEnvironment;
				Formation formation = riderAgent.Formation;
				Agent agent2 = ((formation != null) ? formation.Captain : null);
				if (agent2 == riderAgent)
				{
					agent2 = null;
				}
				CharacterObject characterObject2 = ((agent2 != null) ? agent2.Character : null) as CharacterObject;
				BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(formation);
				ExplainedNumber explainedNumber = new ExplainedNumber((float)modifiedMountManeuver, false, null);
				ExplainedNumber explainedNumber2 = new ExplainedNumber((float)num2, false, null);
				num3 = this.GetEffectiveSkill(agent.RiderAgent, DefaultSkills.Riding);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.HorseManeuver, ref explainedNumber, num3);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.HorseSpeed, ref explainedNumber2, num3);
				if (activeBanner != null)
				{
					BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMountMovementSpeed, activeBanner, ref explainedNumber2);
				}
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.NimbleSteed, currentBattleEnvironment, characterObject, true, ref explainedNumber);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.SweepingWind, currentBattleEnvironment, characterObject, true, ref explainedNumber2);
				ExplainedNumber explainedNumber3 = new ExplainedNumber(agentDrivenProperties.ArmorTorso, false, null);
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.ToughSteed, currentBattleEnvironment, characterObject2, ref explainedNumber3);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.ToughSteed, currentBattleEnvironment, characterObject, true, ref explainedNumber3);
				PerkHelper.AddEpicPerkBonusForCharacterWithSkill(DefaultPerks.Riding.TheWayOfTheSaddle, riderAgent.CurrentBattleEnvironment, characterObject, num3, true, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
				if (equipmentElement2.Item == null)
				{
					explainedNumber.AddFactor(-0.1f, null);
					explainedNumber2.AddFactor(-0.1f, null);
				}
				if (flag)
				{
					explainedNumber2.AddFactor(-0.25f, null);
				}
				agentDrivenProperties.ArmorTorso = explainedNumber3.ResultNumber;
				agentDrivenProperties.MountManeuver = explainedNumber.ResultNumber;
				agentDrivenProperties.MountSpeed = environmentSpeedFactor * 0.22f * (1f + explainedNumber2.ResultNumber);
			}
			else
			{
				agentDrivenProperties.MountManeuver = (float)modifiedMountManeuver;
				agentDrivenProperties.MountSpeed = environmentSpeedFactor * 0.22f * (float)(1 + num2);
			}
			float num4 = equipmentElement.Weight / 2f + (equipmentElement2.IsEmpty ? 0f : equipmentElement2.Weight);
			agentDrivenProperties.MountDashAccelerationMultiplier = ((num4 > 200f) ? ((num4 < 300f) ? (1f - (num4 - 200f) / 111f) : 0.1f) : 1f);
			if (flag)
			{
				agentDrivenProperties.MountDashAccelerationMultiplier *= 0.75f;
			}
			agentDrivenProperties.TopSpeedReachDuration = Game.Current.BasicModels.RidingModel.CalculateAcceleration(in equipmentElement, in equipmentElement2, num3);
			agentDrivenProperties.MountChargeDamage = (float)equipmentElement.GetModifiedMountCharge(in equipmentElement2) * 0.004f;
			agentDrivenProperties.MountDifficulty = (float)equipmentElement.Item.Difficulty;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00038E20 File Offset: 0x00037020
		private void SetPerkAndBannerEffectsOnAgent(Agent agent, CharacterObject agentCharacter, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedWeaponComponent)
		{
			BattleEnvironment currentBattleEnvironment = agent.CurrentBattleEnvironment;
			Formation formation = agent.Formation;
			Agent agent2 = ((formation != null) ? formation.Captain : null);
			CharacterObject characterObject = ((agent2 != null) ? agent2.Character : null) as CharacterObject;
			if (agent2 == agent)
			{
				characterObject = null;
			}
			ItemObject itemObject = null;
			EquipmentIndex offhandWieldedItemIndex = agent.GetOffhandWieldedItemIndex();
			if (offhandWieldedItemIndex != EquipmentIndex.None)
			{
				itemObject = agent.Equipment[offhandWieldedItemIndex].Item;
			}
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(agent.Formation);
			bool flag = equippedWeaponComponent != null && equippedWeaponComponent.IsRangedWeapon;
			bool flag2 = equippedWeaponComponent != null && equippedWeaponComponent.IsMeleeWeapon;
			bool flag3 = itemObject != null && itemObject.PrimaryWeapon.IsShield;
			ExplainedNumber explainedNumber = new ExplainedNumber(agentDrivenProperties.CombatMaxSpeedMultiplier, false, null);
			ExplainedNumber explainedNumber2 = new ExplainedNumber(agentDrivenProperties.MaxSpeedMultiplier, false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.FleetOfFoot, currentBattleEnvironment, agentCharacter, true, ref explainedNumber);
			ExplainedNumber explainedNumber3 = new ExplainedNumber(agentDrivenProperties.KickStunDurationMultiplier, false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.DirtyFighting, currentBattleEnvironment, agentCharacter, true, ref explainedNumber3);
			agentDrivenProperties.KickStunDurationMultiplier = explainedNumber3.ResultNumber;
			if (equippedWeaponComponent != null)
			{
				ExplainedNumber explainedNumber4 = new ExplainedNumber(agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier, false, null);
				if (flag2)
				{
					ExplainedNumber explainedNumber5 = new ExplainedNumber(agentDrivenProperties.SwingSpeedMultiplier, false, null);
					ExplainedNumber explainedNumber6 = new ExplainedNumber(agentDrivenProperties.HandlingMultiplier, false, null);
					if (!agent.HasMount)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.Fury, currentBattleEnvironment, agentCharacter, true, ref explainedNumber6);
						if (characterObject != null)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.Fury, currentBattleEnvironment, characterObject, ref explainedNumber6);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.OnTheEdge, currentBattleEnvironment, characterObject, ref explainedNumber5);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.BladeMaster, currentBattleEnvironment, characterObject, ref explainedNumber5);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.SwiftSwing, currentBattleEnvironment, characterObject, ref explainedNumber5);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.BladeMaster, currentBattleEnvironment, characterObject, ref explainedNumber4);
						}
					}
					if (equippedWeaponComponent.RelevantSkill == DefaultSkills.OneHanded)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.SwiftStrike, currentBattleEnvironment, agentCharacter, true, ref explainedNumber5);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.OneHanded.WayOfTheSword, currentBattleEnvironment, agentCharacter, DefaultSkills.OneHanded, true, ref explainedNumber5, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.OneHanded.WayOfTheSword, currentBattleEnvironment, agentCharacter, DefaultSkills.OneHanded, true, ref explainedNumber4, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.WrappedHandles, currentBattleEnvironment, agentCharacter, true, ref explainedNumber6);
					}
					else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.TwoHanded)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.OnTheEdge, currentBattleEnvironment, agentCharacter, true, ref explainedNumber5);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.TwoHanded.WayOfTheGreatAxe, currentBattleEnvironment, agentCharacter, DefaultSkills.TwoHanded, true, ref explainedNumber5, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.TwoHanded.WayOfTheGreatAxe, currentBattleEnvironment, agentCharacter, DefaultSkills.TwoHanded, true, ref explainedNumber4, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.StrongGrip, currentBattleEnvironment, agentCharacter, true, ref explainedNumber6);
					}
					else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Polearm)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Footwork, currentBattleEnvironment, agentCharacter, true, ref explainedNumber);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.SwiftSwing, currentBattleEnvironment, agentCharacter, true, ref explainedNumber5);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Polearm.WayOfTheSpear, currentBattleEnvironment, agentCharacter, DefaultSkills.Polearm, true, ref explainedNumber5, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Polearm.WayOfTheSpear, currentBattleEnvironment, agentCharacter, DefaultSkills.Polearm, true, ref explainedNumber4, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
						if (equippedWeaponComponent.SwingDamageType != DamageTypes.Invalid)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.CounterWeight, currentBattleEnvironment, agentCharacter, true, ref explainedNumber6);
						}
					}
					agentDrivenProperties.SwingSpeedMultiplier = explainedNumber5.ResultNumber;
					agentDrivenProperties.HandlingMultiplier = explainedNumber6.ResultNumber;
				}
				if (flag)
				{
					ExplainedNumber explainedNumber7 = new ExplainedNumber(agentDrivenProperties.WeaponInaccuracy, false, null);
					ExplainedNumber explainedNumber8 = new ExplainedNumber(agentDrivenProperties.WeaponMaxMovementAccuracyPenalty, false, null);
					ExplainedNumber explainedNumber9 = new ExplainedNumber(agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty, false, null);
					ExplainedNumber explainedNumber10 = new ExplainedNumber(agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians, false, null);
					ExplainedNumber explainedNumber11 = new ExplainedNumber(agentDrivenProperties.WeaponUnsteadyBeginTime, false, null);
					ExplainedNumber explainedNumber12 = new ExplainedNumber(agentDrivenProperties.WeaponUnsteadyEndTime, false, null);
					ExplainedNumber explainedNumber13 = new ExplainedNumber(agentDrivenProperties.ReloadMovementPenaltyFactor, false, null);
					ExplainedNumber explainedNumber14 = new ExplainedNumber(agentDrivenProperties.ReloadSpeed, false, null);
					ExplainedNumber explainedNumber15 = new ExplainedNumber(agentDrivenProperties.MissileSpeedMultiplier, false, null);
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.NockingPoint, currentBattleEnvironment, agentCharacter, true, ref explainedNumber13);
					if (characterObject != null)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.LooseAndMove, currentBattleEnvironment, characterObject, ref explainedNumber2);
					}
					if (activeBanner != null)
					{
						BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedRangedAccuracyPenalty, activeBanner, ref explainedNumber7);
					}
					if (agent.HasMount)
					{
						if (agentCharacter.GetPerkValue(DefaultPerks.Riding.Sagittarius))
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.Sagittarius, currentBattleEnvironment, agentCharacter, true, ref explainedNumber8);
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.Sagittarius, currentBattleEnvironment, agentCharacter, true, ref explainedNumber9);
						}
						if (characterObject != null && characterObject.GetPerkValue(DefaultPerks.Riding.Sagittarius))
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.Sagittarius, currentBattleEnvironment, characterObject, ref explainedNumber8);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.Sagittarius, currentBattleEnvironment, characterObject, ref explainedNumber9);
						}
						if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Bow && agentCharacter.GetPerkValue(DefaultPerks.Bow.MountedArchery))
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.MountedArchery, currentBattleEnvironment, agentCharacter, true, ref explainedNumber8);
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.MountedArchery, currentBattleEnvironment, agentCharacter, true, ref explainedNumber9);
						}
						if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Throwing && agentCharacter.GetPerkValue(DefaultPerks.Throwing.MountedSkirmisher))
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.MountedSkirmisher, currentBattleEnvironment, agentCharacter, true, ref explainedNumber8);
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.MountedSkirmisher, currentBattleEnvironment, agentCharacter, true, ref explainedNumber9);
						}
					}
					bool flag4 = false;
					if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Bow)
					{
						flag4 = true;
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.BowControl, currentBattleEnvironment, agentCharacter, true, ref explainedNumber8);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.RapidFire, currentBattleEnvironment, agentCharacter, true, ref explainedNumber14);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.QuickAdjustments, currentBattleEnvironment, agentCharacter, true, ref explainedNumber10);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.Discipline, currentBattleEnvironment, agentCharacter, true, ref explainedNumber11);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.Discipline, currentBattleEnvironment, agentCharacter, true, ref explainedNumber12);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.QuickDraw, currentBattleEnvironment, agentCharacter, true, ref explainedNumber4);
						if (characterObject != null)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.RapidFire, currentBattleEnvironment, characterObject, ref explainedNumber14);
							if (!agent.HasMount)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.NockingPoint, currentBattleEnvironment, characterObject, ref explainedNumber2);
							}
						}
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Bow.Deadshot, currentBattleEnvironment, agentCharacter, DefaultSkills.Bow, true, ref explainedNumber14, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus);
					}
					else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Crossbow)
					{
						flag4 = true;
						if (agent.HasMount)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Steady, currentBattleEnvironment, agentCharacter, true, ref explainedNumber8);
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Steady, currentBattleEnvironment, agentCharacter, true, ref explainedNumber10);
						}
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.WindWinder, currentBattleEnvironment, agentCharacter, true, ref explainedNumber14);
						if (characterObject != null)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.WindWinder, currentBattleEnvironment, characterObject, ref explainedNumber14);
						}
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.DonkeysSwiftness, currentBattleEnvironment, agentCharacter, true, ref explainedNumber8);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Marksmen, currentBattleEnvironment, agentCharacter, true, ref explainedNumber4);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Crossbow.MightyPull, currentBattleEnvironment, agentCharacter, DefaultSkills.Crossbow, true, ref explainedNumber14, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus);
					}
					else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Throwing)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.QuickDraw, currentBattleEnvironment, agentCharacter, true, ref explainedNumber14);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.PerfectTechnique, currentBattleEnvironment, agentCharacter, true, ref explainedNumber15);
						if (characterObject != null)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.QuickDraw, currentBattleEnvironment, characterObject, ref explainedNumber14);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.PerfectTechnique, currentBattleEnvironment, characterObject, ref explainedNumber15);
						}
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Throwing.UnstoppableForce, currentBattleEnvironment, agentCharacter, DefaultSkills.Throwing, true, ref explainedNumber15, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus);
					}
					if (flag4 && Campaign.Current.Models.MapWeatherModel.GetWeatherEffectOnTerrainForPosition(MobileParty.MainParty.Position.ToVec2()) == MapWeatherModel.WeatherEventEffectOnTerrain.Wet)
					{
						explainedNumber15.AddFactor(-0.2f, null);
					}
					agentDrivenProperties.ReloadMovementPenaltyFactor = explainedNumber13.ResultNumber;
					agentDrivenProperties.ReloadSpeed = explainedNumber14.ResultNumber;
					agentDrivenProperties.MissileSpeedMultiplier = explainedNumber15.ResultNumber;
					agentDrivenProperties.WeaponInaccuracy = explainedNumber7.ResultNumber;
					agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = explainedNumber8.ResultNumber;
					agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = explainedNumber9.ResultNumber;
					agentDrivenProperties.WeaponUnsteadyBeginTime = explainedNumber11.ResultNumber;
					agentDrivenProperties.WeaponUnsteadyEndTime = explainedNumber12.ResultNumber;
					agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = explainedNumber10.ResultNumber;
				}
				agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = explainedNumber4.ResultNumber;
			}
			if (flag3)
			{
				ExplainedNumber explainedNumber16 = new ExplainedNumber(agentDrivenProperties.AttributeShieldMissileCollisionBodySizeAdder, false, null);
				if (characterObject != null)
				{
					Formation formation2 = agent.Formation;
					if (formation2 != null && formation2.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.ShieldWall, currentBattleEnvironment, characterObject, ref explainedNumber16);
					}
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.ArrowCatcher, currentBattleEnvironment, characterObject, ref explainedNumber16);
				}
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.ArrowCatcher, currentBattleEnvironment, agentCharacter, true, ref explainedNumber16);
				agentDrivenProperties.AttributeShieldMissileCollisionBodySizeAdder = explainedNumber16.ResultNumber;
				ExplainedNumber explainedNumber17 = new ExplainedNumber(agentDrivenProperties.ShieldBashStunDurationMultiplier, false, null);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Basher, currentBattleEnvironment, agentCharacter, true, ref explainedNumber17);
				agentDrivenProperties.ShieldBashStunDurationMultiplier = explainedNumber17.ResultNumber;
			}
			else
			{
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.MorningExercise, currentBattleEnvironment, agentCharacter, true, ref explainedNumber2);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.SelfMedication, currentBattleEnvironment, agentCharacter, false, ref explainedNumber2);
				if (!flag3 && !flag)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.Sprint, currentBattleEnvironment, agentCharacter, true, ref explainedNumber2);
				}
				if (equippedWeaponComponent == null && itemObject == null)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.FleetFooted, currentBattleEnvironment, agentCharacter, true, ref explainedNumber2);
				}
				if (characterObject != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.MorningExercise, currentBattleEnvironment, characterObject, ref explainedNumber2);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.ShieldBearer, currentBattleEnvironment, characterObject, ref explainedNumber2);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.FleetOfFoot, currentBattleEnvironment, characterObject, ref explainedNumber2);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.RecklessCharge, currentBattleEnvironment, characterObject, ref explainedNumber2);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Footwork, currentBattleEnvironment, characterObject, ref explainedNumber2);
					if (agentCharacter.Tier >= 3)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.FormFittingArmor, currentBattleEnvironment, characterObject, ref explainedNumber2);
					}
					if (agentCharacter.IsInfantry)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.Sprint, currentBattleEnvironment, characterObject, ref explainedNumber2);
					}
				}
			}
			float num = 0f;
			float num2 = 0f;
			bool flag5 = false;
			if (characterObject != null)
			{
				float num3;
				if (agent.HasMount && characterObject.GetPerkValue(DefaultPerks.Riding.DauntlessSteed, currentBattleEnvironment, false, out num3))
				{
					num += DefaultPerks.Riding.DauntlessSteed.SecondaryBonus;
					flag5 = true;
				}
				else if (!agent.HasMount && characterObject.GetPerkValue(DefaultPerks.Athletics.IgnorePain, currentBattleEnvironment, false, out num3))
				{
					num += DefaultPerks.Athletics.IgnorePain.SecondaryBonus;
					flag5 = true;
				}
				if (characterObject.GetPerkValue(DefaultPerks.Engineering.Metallurgy, currentBattleEnvironment, false, out num3))
				{
					num += DefaultPerks.Engineering.Metallurgy.SecondaryBonus;
					flag5 = true;
				}
			}
			float num4;
			if (!agent.HasMount && agentCharacter.GetPerkValue(DefaultPerks.Athletics.IgnorePain, currentBattleEnvironment, true, out num4))
			{
				num2 += num4;
				flag5 = true;
			}
			if (flag5)
			{
				float num5 = 1f + num2;
				agentDrivenProperties.ArmorHead = MathF.Max(0f, (agentDrivenProperties.ArmorHead + num) * num5);
				agentDrivenProperties.ArmorTorso = MathF.Max(0f, (agentDrivenProperties.ArmorTorso + num) * num5);
				agentDrivenProperties.ArmorArms = MathF.Max(0f, (agentDrivenProperties.ArmorArms + num) * num5);
				agentDrivenProperties.ArmorLegs = MathF.Max(0f, (agentDrivenProperties.ArmorLegs + num) * num5);
			}
			if (Mission.Current != null && Mission.Current.HasValidTerrainType)
			{
				TerrainType terrainType = Mission.Current.TerrainType;
				if (terrainType == TerrainType.Snow || terrainType == TerrainType.Forest)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.ExtendedSkirmish, currentBattleEnvironment, characterObject, ref explainedNumber2);
				}
				else if (terrainType == TerrainType.Plain || terrainType == TerrainType.Steppe || terrainType == TerrainType.Desert)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.DecisiveBattle, currentBattleEnvironment, characterObject, ref explainedNumber2);
				}
			}
			if (agentCharacter.Tier >= 3 && agentCharacter.IsInfantry)
			{
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.FormFittingArmor, currentBattleEnvironment, characterObject, ref explainedNumber2);
			}
			if (agent.Formation != null && agent.Formation.CountOfUnits <= 15)
			{
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.SmallUnitTactics, currentBattleEnvironment, characterObject, ref explainedNumber2);
			}
			if (activeBanner != null)
			{
				BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedTroopMovementSpeed, activeBanner, ref explainedNumber2);
			}
			agentDrivenProperties.MaxSpeedMultiplier = explainedNumber2.ResultNumber;
			agentDrivenProperties.CombatMaxSpeedMultiplier = explainedNumber.ResultNumber;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0003993C File Offset: 0x00037B3C
		private void SetWeaponSkillEffectsOnAgent(Agent agent, CharacterObject agentCharacter, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedWeaponComponent)
		{
			if (equippedWeaponComponent != null)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, equippedWeaponComponent.RelevantSkill);
				ExplainedNumber explainedNumber = new ExplainedNumber(agentDrivenProperties.SwingSpeedMultiplier, false, null);
				ExplainedNumber explainedNumber2 = new ExplainedNumber(agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier, false, null);
				ExplainedNumber explainedNumber3 = new ExplainedNumber(agentDrivenProperties.ReloadSpeed, false, null);
				if (equippedWeaponComponent.RelevantSkill == DefaultSkills.OneHanded)
				{
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.OneHandedSpeed, ref explainedNumber, effectiveSkill);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.OneHandedSpeed, ref explainedNumber2, effectiveSkill);
				}
				else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.TwoHanded)
				{
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.TwoHandedSpeed, ref explainedNumber, effectiveSkill);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.TwoHandedSpeed, ref explainedNumber2, effectiveSkill);
				}
				else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Polearm)
				{
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.PolearmSpeed, ref explainedNumber, effectiveSkill);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.PolearmSpeed, ref explainedNumber2, effectiveSkill);
				}
				else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Crossbow)
				{
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.CrossbowReloadSpeed, ref explainedNumber3, effectiveSkill);
				}
				else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Throwing)
				{
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.ThrowingSpeed, ref explainedNumber2, effectiveSkill);
				}
				agentDrivenProperties.SwingSpeedMultiplier = explainedNumber.ResultNumber;
				agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = explainedNumber2.ResultNumber;
				agentDrivenProperties.ReloadSpeed = explainedNumber3.ResultNumber;
			}
			int effectiveSkill2 = this.GetEffectiveSkill(agent, DefaultSkills.Roguery);
			ExplainedNumber explainedNumber4 = new ExplainedNumber(1f, false, null);
			SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.CrouchedSpeed, ref explainedNumber4, effectiveSkill2);
			agentDrivenProperties.CrouchedSpeedMultiplier = explainedNumber4.ResultNumber;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00039AA4 File Offset: 0x00037CA4
		private void SetMountedPenaltiesOnAgent(Agent agent, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedWeaponComponent)
		{
			int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Riding);
			float skillEffectValue = DefaultSkillEffects.MountedWeaponSpeedPenalty.GetSkillEffectValue(effectiveSkill);
			if (skillEffectValue < 0f)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(agentDrivenProperties.WeaponBestAccuracyWaitTime, false, null);
				ExplainedNumber explainedNumber2 = new ExplainedNumber(agentDrivenProperties.SwingSpeedMultiplier, false, null);
				ExplainedNumber explainedNumber3 = new ExplainedNumber(agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier, false, null);
				ExplainedNumber explainedNumber4 = new ExplainedNumber(agentDrivenProperties.ReloadSpeed, false, null);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.MountedWeaponSpeedPenalty, ref explainedNumber2, effectiveSkill);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.MountedWeaponSpeedPenalty, ref explainedNumber3, effectiveSkill);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.MountedWeaponSpeedPenalty, ref explainedNumber4, effectiveSkill);
				explainedNumber.AddFactor(-1f * skillEffectValue, null);
				agentDrivenProperties.SwingSpeedMultiplier = Math.Max(0f, explainedNumber2.ResultNumber);
				agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = Math.Max(0f, explainedNumber3.ResultNumber);
				agentDrivenProperties.ReloadSpeed = Math.Max(0f, explainedNumber4.ResultNumber);
				agentDrivenProperties.WeaponBestAccuracyWaitTime = Math.Max(0f, explainedNumber.ResultNumber);
			}
			float num = 5f - (float)effectiveSkill * 0.05f;
			if (num > 0f)
			{
				ExplainedNumber explainedNumber5 = new ExplainedNumber(agentDrivenProperties.WeaponInaccuracy, false, null);
				explainedNumber5.AddFactor(num, null);
				agentDrivenProperties.WeaponInaccuracy = Math.Max(0f, explainedNumber5.ResultNumber);
			}
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00039BE9 File Offset: 0x00037DE9
		public static float CalculateMaximumSpeedMultiplier(int athletics, float baseWeight, float totalEncumbrance)
		{
			return MathF.Min((200f + (float)athletics) / 300f * (baseWeight * 2f / (baseWeight * 2f + totalEncumbrance)), 1f);
		}
	}
}
