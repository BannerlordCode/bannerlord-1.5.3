using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace SandBox.GameComponents
{
	// Token: 0x020000C2 RID: 194
	public class SandboxAgentApplyDamageModel : AgentApplyDamageModel
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x0003517C File Offset: 0x0003337C
		public override bool IsDamageIgnored(in AttackInformation attackInformation, in AttackCollisionData collisionData)
		{
			CharacterObject characterObject = (attackInformation.IsVictimAgentMount ? attackInformation.VictimRiderAgentCharacter : attackInformation.VictimAgentCharacter) as CharacterObject;
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem = attackerWeapon.CurrentUsageItem;
			bool flag = false;
			if (currentUsageItem != null && currentUsageItem.IsConsumable)
			{
				AttackCollisionData attackCollisionData = collisionData;
				float num;
				if (attackCollisionData.CollidedWithShieldOnBack && characterObject != null && characterObject.GetPerkValue(DefaultPerks.Crossbow.Pavise, attackInformation.VictimBattleEnvironment, true, out num))
				{
					flag = MBRandom.RandomFloat <= num;
				}
			}
			return flag;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000351FC File Offset: 0x000333FC
		public override float ApplyDamageAmplifications(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			Formation attackerFormation = attackInformation.AttackerFormation;
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(attackerFormation);
			Agent agent = (attackInformation.IsAttackerAgentMount ? attackInformation.AttackerAgent.RiderAgent : attackInformation.AttackerAgent);
			BattleEnvironment attackerBattleEnvironment = attackInformation.AttackerBattleEnvironment;
			CharacterObject characterObject = (attackInformation.IsAttackerAgentMount ? attackInformation.AttackerRiderAgentCharacter : attackInformation.AttackerAgentCharacter) as CharacterObject;
			CharacterObject characterObject2 = attackInformation.AttackerCaptainCharacter as CharacterObject;
			bool flag = attackInformation.IsAttackerAgentHuman && !attackInformation.DoesAttackerHaveMountAgent;
			bool flag2 = attackInformation.DoesAttackerHaveMountAgent || attackInformation.DoesAttackerHaveRiderAgent;
			CharacterObject characterObject3 = (attackInformation.IsVictimAgentMount ? attackInformation.VictimRiderAgentCharacter : attackInformation.VictimAgentCharacter) as CharacterObject;
			bool flag3 = attackInformation.IsVictimAgentHuman && !attackInformation.DoesVictimHaveMountAgent;
			bool flag4 = attackInformation.DoesVictimHaveMountAgent || attackInformation.DoesVictimHaveRiderAgent;
			Formation victimFormation = attackInformation.VictimFormation;
			BannerComponent activeBanner2 = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(victimFormation);
			AttackCollisionData attackCollisionData = collisionData;
			bool flag5;
			if (!attackCollisionData.AttackBlockedWithShield)
			{
				attackCollisionData = collisionData;
				flag5 = attackCollisionData.CollidedWithShieldOnBack;
			}
			else
			{
				flag5 = true;
			}
			bool flag6 = flag5;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseDamage, false, null);
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem = attackerWeapon.CurrentUsageItem;
			if (characterObject != null)
			{
				if (currentUsageItem != null)
				{
					if (currentUsageItem.IsMeleeWeapon)
					{
						if (currentUsageItem.RelevantSkill == DefaultSkills.OneHanded)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.DeadlyPurpose, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							if (flag2)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Cavalry, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							MissionWeapon offHandItem = attackInformation.OffHandItem;
							if (offHandItem.IsEmpty)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Duelist, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							if (currentUsageItem.WeaponClass == WeaponClass.Mace || currentUsageItem.WeaponClass == WeaponClass.OneHandedAxe)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.ToBeBlunt, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							if (flag6)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Prestige, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Roguery.Carver, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.OneHanded.WayOfTheSword, attackerBattleEnvironment, characterObject, DefaultSkills.OneHanded, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
						}
						else if (currentUsageItem.RelevantSkill == DefaultSkills.TwoHanded)
						{
							if (flag6)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.WoodChopper, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.WoodChopper, attackerBattleEnvironment, characterObject2, ref explainedNumber);
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.ShieldBreaker, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.ShieldBreaker, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							}
							if (currentUsageItem.WeaponClass == WeaponClass.TwoHandedAxe || currentUsageItem.WeaponClass == WeaponClass.TwoHandedMace)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.HeadBasher, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							if (attackInformation.IsVictimAgentMount)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.BeastSlayer, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.BeastSlayer, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							}
							if (attackInformation.AttackerHitPointRate < 0.5f)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.Berserker, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							else if (attackInformation.AttackerHitPointRate > 0.9f)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.Confidence, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.BladeMaster, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Roguery.DashAndSlash, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.TwoHanded.WayOfTheGreatAxe, attackerBattleEnvironment, characterObject, DefaultSkills.TwoHanded, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
						}
						else if (currentUsageItem.RelevantSkill == DefaultSkills.Polearm)
						{
							if (flag2)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Cavalry, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							else
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Pikeman, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							attackCollisionData = collisionData;
							if (attackCollisionData.StrikeType == 1)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.CleanThrust, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.SharpenTheTip, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							if (attackInformation.IsVictimAgentMount)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.SteedKiller, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								if (flag)
								{
									PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.SteedKiller, attackerBattleEnvironment, characterObject2, ref explainedNumber);
								}
							}
							if (attackInformation.IsHeadShot)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Guards, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Phalanx, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Polearm.WayOfTheSpear, attackerBattleEnvironment, characterObject, DefaultSkills.Polearm, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
						}
						else if (currentUsageItem.IsShield)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Basher, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
						}
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.Powerful, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.Powerful, attackerBattleEnvironment, characterObject2, ref explainedNumber);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Engineering.ImprovedTools, attackerBattleEnvironment, characterObject2, ref explainedNumber);
						if (attackerWeapon.Item != null && attackerWeapon.Item.ItemType == ItemObject.ItemTypeEnum.Thrown)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.FlexibleFighter, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
						}
						if (flag2)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.MountedWarrior, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.MountedWarrior, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.Cavalry, attackerBattleEnvironment, characterObject2, ref explainedNumber);
						}
						else
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.DeadlyPurpose, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							attackCollisionData = collisionData;
							if (attackCollisionData.StrikeType == 1)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.SharpenTheTip, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							}
						}
						if (activeBanner != null)
						{
							BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMeleeDamage, activeBanner, ref explainedNumber);
							if (attackInformation.DoesVictimHaveMountAgent)
							{
								BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMeleeDamageAgainstMountedTroops, activeBanner, ref explainedNumber);
							}
						}
					}
					else if (currentUsageItem.IsConsumable)
					{
						if (currentUsageItem.RelevantSkill == DefaultSkills.Bow)
						{
							attackCollisionData = collisionData;
							if (attackCollisionData.CollisionBoneIndex != -1)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.BowControl, attackerBattleEnvironment, characterObject2, ref explainedNumber);
								if (attackInformation.IsHeadShot)
								{
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.DeadAim, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								}
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.StrongBows, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								if (characterObject.Tier >= 3)
								{
									PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.StrongBows, attackerBattleEnvironment, characterObject2, ref explainedNumber);
								}
								if (attackInformation.IsVictimAgentMount)
								{
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.HunterClan, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								}
								PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Bow.Deadshot, attackerBattleEnvironment, characterObject, DefaultSkills.Bow, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus);
								goto IL_0836;
							}
						}
						if (currentUsageItem.RelevantSkill == DefaultSkills.Crossbow)
						{
							attackCollisionData = collisionData;
							if (attackCollisionData.CollisionBoneIndex != -1)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Engineering.TorsionEngines, attackerBattleEnvironment, characterObject, false, ref explainedNumber);
								if (attackInformation.IsVictimAgentMount)
								{
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Unhorser, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
									PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.Unhorser, attackerBattleEnvironment, characterObject2, ref explainedNumber);
								}
								if (attackInformation.IsHeadShot)
								{
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Sheriff, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								}
								if (flag3)
								{
									PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.Sheriff, attackerBattleEnvironment, characterObject2, ref explainedNumber);
								}
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.HammerBolts, attackerBattleEnvironment, characterObject2, ref explainedNumber);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Engineering.DreadfulSieger, attackerBattleEnvironment, characterObject2, ref explainedNumber);
								PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Crossbow.MightyPull, attackerBattleEnvironment, characterObject, DefaultSkills.Crossbow, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus);
								goto IL_0836;
							}
						}
						if (currentUsageItem.RelevantSkill == DefaultSkills.Throwing)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.StrongArms, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							if (flag6)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.ShieldBreaker, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.ShieldBreaker, attackerBattleEnvironment, characterObject2, ref explainedNumber);
								if (currentUsageItem.WeaponClass == WeaponClass.ThrowingAxe)
								{
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Splinters, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								}
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.Splinters, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							}
							if (attackInformation.IsVictimAgentMount)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Hunter, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.Hunter, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							}
							if (flag2)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.MountedSkirmisher, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							}
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.Impale, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							if (flag4)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.KnockOff, attackerBattleEnvironment, characterObject2, ref explainedNumber);
							}
							if (attackInformation.VictimAgentHealth <= attackInformation.VictimAgentMaxHealth * 0.5f)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.LastHit, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							if (attackInformation.IsHeadShot)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.HeadHunter, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							}
							PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Throwing.UnstoppableForce, attackerBattleEnvironment, characterObject, DefaultSkills.Throwing, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus);
						}
						IL_0836:
						if (flag2)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.HorseArcher, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.HorseArcher, attackerBattleEnvironment, characterObject2, ref explainedNumber);
						}
						if (activeBanner != null)
						{
							BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedRangedDamage, activeBanner, ref explainedNumber);
						}
					}
					if (attackerWeapon.Item != null && attackerWeapon.Item.IsCivilian)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.Carver, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					}
				}
				attackCollisionData = collisionData;
				if (attackCollisionData.IsHorseCharge)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.FullSpeed, attackerBattleEnvironment, characterObject, true, ref explainedNumber);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.FullSpeed, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					int effectiveSkill = MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveSkill(agent, DefaultSkills.Riding);
					PerkHelper.AddEpicPerkBonusForCharacterWithSkill(DefaultPerks.Riding.TheWayOfTheSaddle, attackInformation.AttackerBattleEnvironment, characterObject, effectiveSkill, true, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
					if (activeBanner != null)
					{
						BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedChargeDamage, activeBanner, ref explainedNumber);
					}
					if (activeBanner2 != null)
					{
						BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedChargeDamage, activeBanner2, ref explainedNumber);
					}
				}
				if (attackerFormation != null)
				{
					MovementOrder.MovementOrderEnum orderEnum = attackerFormation.GetReadonlyMovementOrderReference().OrderEnum;
					if (orderEnum == MovementOrder.MovementOrderEnum.Charge || orderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget)
					{
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
						PartyBase partyBase = obj as PartyBase;
						MobileParty mobileParty = ((partyBase != null && partyBase.IsMobile) ? partyBase.MobileParty : null);
						Hero hero;
						if (mobileParty == null)
						{
							hero = null;
						}
						else
						{
							Army army = mobileParty.Army;
							if (army == null)
							{
								hero = null;
							}
							else
							{
								MobileParty leaderParty = army.LeaderParty;
								hero = ((leaderParty != null) ? leaderParty.LeaderHero : null);
							}
						}
						Hero hero2 = hero ?? ((mobileParty != null) ? mobileParty.LeaderHero : null);
						if (hero2 != null && hero2.CharacterObject != characterObject)
						{
							TraitEffectHelper.ApplyTraitEffect(hero2, DefaultPersonalityTraitEffects.CalculatingChargeDamageEffect, ref explainedNumber);
						}
					}
				}
				if (flag)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.HeadBasher, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.RecklessCharge, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Pikeman, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					if (flag4)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Braced, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					}
				}
				if (flag2)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Cavalry, attackerBattleEnvironment, characterObject2, ref explainedNumber);
				}
				if (currentUsageItem == null)
				{
					attackCollisionData = collisionData;
					if (attackCollisionData.IsAlternativeAttack && characterObject.GetPerkValue(DefaultPerks.Athletics.StrongLegs))
					{
						float num = 1f;
						explainedNumber.AddFactor(num, null);
					}
				}
				if (flag6)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Engineering.WallBreaker, attackerBattleEnvironment, characterObject2, ref explainedNumber);
				}
				attackCollisionData = collisionData;
				if (attackCollisionData.EntityExists)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.Vandal, attackerBattleEnvironment, characterObject2, ref explainedNumber);
				}
				if (characterObject3 != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.Coaching, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					if (characterObject3.Culture.IsBandit)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.LawKeeper, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					}
					if (flag2 && flag3)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.Gensdarmes, attackerBattleEnvironment, characterObject2, ref explainedNumber);
					}
				}
				if (characterObject.Culture.IsBandit)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Roguery.PartnersInCrime, attackerBattleEnvironment, characterObject2, ref explainedNumber);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00035D18 File Offset: 0x00033F18
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

		// Token: 0x060007EC RID: 2028 RVA: 0x00035D60 File Offset: 0x00033F60
		public override float ApplyDamageReductions(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			Agent agent = (attackInformation.IsAttackerAgentMount ? attackInformation.AttackerAgent.RiderAgent : attackInformation.AttackerAgent);
			bool isAttackerAgentMount = attackInformation.IsAttackerAgentMount;
			CharacterObject characterObject = (attackInformation.IsVictimAgentMount ? attackInformation.VictimRiderAgentCharacter : attackInformation.VictimAgentCharacter) as CharacterObject;
			BattleEnvironment victimBattleEnvironment = attackInformation.VictimBattleEnvironment;
			CharacterObject characterObject2 = attackInformation.VictimCaptainCharacter as CharacterObject;
			bool flag = attackInformation.IsVictimAgentHuman && !attackInformation.DoesVictimHaveMountAgent;
			Formation victimFormation = attackInformation.VictimFormation;
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(victimFormation);
			MissionWeapon victimMainHandWeapon = attackInformation.VictimMainHandWeapon;
			WeaponComponentData currentUsageItem = victimMainHandWeapon.CurrentUsageItem;
			AttackCollisionData attackCollisionData = collisionData;
			bool flag2;
			if (!attackCollisionData.AttackBlockedWithShield)
			{
				attackCollisionData = collisionData;
				flag2 = attackCollisionData.CollidedWithShieldOnBack;
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseDamage, false, null);
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem2 = attackerWeapon.CurrentUsageItem;
			if (attackInformation.DoesAttackerHaveMountAgent && (currentUsageItem2 == null || currentUsageItem2.RelevantSkill != DefaultSkills.Crossbow))
			{
				int effectiveSkill = MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveSkill(agent, DefaultSkills.Riding);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.MountedWeaponDamagePenalty, ref explainedNumber, effectiveSkill);
			}
			if (characterObject != null)
			{
				if (currentUsageItem2 != null)
				{
					if (currentUsageItem2.IsConsumable)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.SkirmishPhaseMaster, victimBattleEnvironment, characterObject, true, ref explainedNumber);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.Skirmisher, victimBattleEnvironment, characterObject2, ref explainedNumber);
						if (characterObject.IsRanged)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.SkirmishPhaseMaster, victimBattleEnvironment, characterObject2, ref explainedNumber);
						}
						if (currentUsageItem != null)
						{
							if (currentUsageItem.WeaponClass == WeaponClass.Crossbow)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.CounterFire, victimBattleEnvironment, characterObject, true, ref explainedNumber);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.CounterFire, victimBattleEnvironment, characterObject2, ref explainedNumber);
							}
							else if (currentUsageItem.RelevantSkill == DefaultSkills.Throwing)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Skirmisher, victimBattleEnvironment, characterObject, true, ref explainedNumber);
							}
						}
						if (activeBanner != null)
						{
							BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedRangedAttackDamage, activeBanner, ref explainedNumber);
						}
					}
					else if (currentUsageItem2.IsMeleeWeapon)
					{
						if (characterObject2 != null)
						{
							Formation victimFormation2 = attackInformation.VictimFormation;
							if (victimFormation2 != null && victimFormation2.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.Basher, victimBattleEnvironment, characterObject2, ref explainedNumber);
							}
						}
						if (activeBanner != null)
						{
							BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedMeleeAttackDamage, activeBanner, ref explainedNumber);
						}
					}
				}
				if (flag3)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.SteelCoreShields, victimBattleEnvironment, characterObject, true, ref explainedNumber);
					if (flag)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.SteelCoreShields, victimBattleEnvironment, characterObject2, ref explainedNumber);
					}
					attackCollisionData = collisionData;
					if (attackCollisionData.AttackBlockedWithShield)
					{
						attackCollisionData = collisionData;
						if (!attackCollisionData.CorrectSideShieldBlock)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.ShieldWall, victimBattleEnvironment, characterObject, true, ref explainedNumber);
						}
					}
				}
				attackCollisionData = collisionData;
				if (attackCollisionData.IsHorseCharge)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.SureFooted, victimBattleEnvironment, characterObject, true, ref explainedNumber);
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.Braced, victimBattleEnvironment, characterObject, true, ref explainedNumber);
					if (characterObject2 != null)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.SureFooted, victimBattleEnvironment, characterObject2, ref explainedNumber);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.Braced, victimBattleEnvironment, characterObject2, ref explainedNumber);
					}
				}
				attackCollisionData = collisionData;
				if (attackCollisionData.IsFallDamage)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.StrongLegs, victimBattleEnvironment, characterObject, true, ref explainedNumber);
				}
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.EliteReserves, victimBattleEnvironment, characterObject2, ref explainedNumber);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00036064 File Offset: 0x00034264
		public override float ApplyGeneralDamageModifiers(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			bool isAttackerAgentMount = attackInformation.IsAttackerAgentMount;
			bool isVictimAgentMount = attackInformation.IsVictimAgentMount;
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem = attackerWeapon.CurrentUsageItem;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseDamage, false, null);
			if (currentUsageItem != null)
			{
				if (currentUsageItem.RelevantSkill == DefaultSkills.Throwing)
				{
					explainedNumber = new ExplainedNumber(explainedNumber.ResultNumber * (1f + attackInformation.AttackerAgent.AgentDrivenProperties.ThrowingWeaponDamageMultiplierBonus), false, null);
				}
				else if (currentUsageItem.IsMeleeWeapon)
				{
					explainedNumber = new ExplainedNumber(explainedNumber.ResultNumber * (1f + attackInformation.AttackerAgent.AgentDrivenProperties.MeleeWeaponDamageMultiplierBonus), false, null);
				}
			}
			Agent attackerAgent = attackInformation.AttackerAgent;
			if (attackerAgent != null)
			{
				explainedNumber = new ExplainedNumber(explainedNumber.ResultNumber * (1f + attackerAgent.AgentDrivenProperties.DamageMultiplierBonus), false, null);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00036134 File Offset: 0x00034334
		public override bool DecideCrushedThrough(Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
		{
			EquipmentIndex equipmentIndex = attackerAgent.GetOffhandWieldedItemIndex();
			if (equipmentIndex == EquipmentIndex.None)
			{
				equipmentIndex = attackerAgent.GetPrimaryWieldedItemIndex();
			}
			if (((equipmentIndex != EquipmentIndex.None) ? attackerAgent.Equipment[equipmentIndex].CurrentUsageItem : null) == null || isPassiveUsage || strikeType != StrikeType.Swing || attackDirection != Agent.UsageDirection.AttackUp)
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

		// Token: 0x060007EF RID: 2031 RVA: 0x000361A0 File Offset: 0x000343A0
		public override void DecideMissileWeaponFlags(Agent attackerAgent, in MissionWeapon missileWeapon, ref WeaponFlags missileWeaponFlags)
		{
			CharacterObject characterObject = ((attackerAgent != null) ? attackerAgent.Character : null) as CharacterObject;
			if (characterObject != null)
			{
				MissionWeapon missionWeapon = missileWeapon;
				if (missionWeapon.CurrentUsageItem.WeaponClass == WeaponClass.Javelin && characterObject.GetPerkValue(DefaultPerks.Throwing.Impale))
				{
					missileWeaponFlags |= WeaponFlags.CanPenetrateShield;
				}
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000361F1 File Offset: 0x000343F1
		public override bool CanWeaponIgnoreFriendlyFireChecks(WeaponComponentData weapon)
		{
			return weapon != null && weapon.IsConsumable && weapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanPenetrateShield) && weapon.WeaponFlags.HasAnyFlag(WeaponFlags.MultiplePenetration);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00036228 File Offset: 0x00034428
		public override bool CanWeaponDealSneakAttack(in AttackInformation attackInformation, WeaponComponentData weapon)
		{
			if (weapon != null && (weapon.IsMeleeWeapon || weapon.WeaponClass == WeaponClass.ThrowingKnife) && attackInformation.IsVictimAgentHuman && !attackInformation.IsVictimPlayer)
			{
				if ((attackInformation.VictimAgentAIStateFlags & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.None && attackInformation.VictimAgentFlags.HasAnyFlag(AgentFlag.CanGetAlarmed))
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

		// Token: 0x060007F2 RID: 2034 RVA: 0x000362C8 File Offset: 0x000344C8
		public override bool CanWeaponDismount(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			if (!MBMath.IsBetween((int)blow.VictimBodyPart, 0, 6))
			{
				return false;
			}
			if (!attackerAgent.HasMount && blow.StrikeType == StrikeType.Swing && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanHook))
			{
				return true;
			}
			if (blow.StrikeType == StrikeType.Thrust && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanDismount))
			{
				return true;
			}
			CharacterObject characterObject;
			if ((characterObject = attackerAgent.Character as CharacterObject) != null)
			{
				bool flag = attackerWeapon.RelevantSkill == DefaultSkills.Crossbow && attackerWeapon.IsConsumable && characterObject.GetPerkValue(DefaultPerks.Crossbow.HammerBolts);
				bool flag2 = attackerWeapon.RelevantSkill == DefaultSkills.Throwing && attackerWeapon.IsConsumable && characterObject.GetPerkValue(DefaultPerks.Throwing.KnockOff);
				return flag || flag2;
			}
			return false;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00036390 File Offset: 0x00034590
		public override void CalculateDefendedBlowStunMultipliers(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, WeaponComponentData attackerWeapon, WeaponComponentData defenderWeapon, ref float attackerStunPeriod, ref float defenderStunPeriod)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			ExplainedNumber explainedNumber2 = new ExplainedNumber(1f, false, null);
			CharacterObject characterObject;
			if ((characterObject = attackerAgent.Character as CharacterObject) != null && (collisionResult == CombatCollisionResult.Blocked || collisionResult == CombatCollisionResult.Parried))
			{
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.MightyBlow, attackerAgent.CurrentBattleEnvironment, characterObject, true, ref explainedNumber);
			}
			attackerStunPeriod *= MathF.Max(0f, explainedNumber.ResultNumber);
			defenderStunPeriod *= MathF.Max(0f, explainedNumber2.ResultNumber);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00036418 File Offset: 0x00034618
		public override bool CanWeaponKnockback(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			AttackCollisionData attackCollisionData = collisionData;
			return MBMath.IsBetween((int)attackCollisionData.VictimHitBodyPart, 0, 6) && !attackerWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown) && (attackerWeapon.IsConsumable || (blow.BlowFlag & BlowFlags.CrushThrough) != BlowFlags.None || (blow.StrikeType == StrikeType.Thrust && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.WideGrip)));
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00036488 File Offset: 0x00034688
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

		// Token: 0x060007F6 RID: 2038 RVA: 0x00036520 File Offset: 0x00034720
		public override float GetDismountPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			if (blow.StrikeType == StrikeType.Swing && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanHook))
			{
				explainedNumber.Add(0.25f, null, null);
			}
			BattleEnvironment currentBattleEnvironment = attackerAgent.CurrentBattleEnvironment;
			CharacterObject characterObject;
			if (attackerWeapon != null && (characterObject = attackerAgent.Character as CharacterObject) != null)
			{
				if (attackerWeapon.RelevantSkill == DefaultSkills.Polearm)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Braced, currentBattleEnvironment, characterObject, true, ref explainedNumber);
				}
				else if (attackerWeapon.RelevantSkill == DefaultSkills.Crossbow && attackerWeapon.IsConsumable)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.HammerBolts, currentBattleEnvironment, characterObject, true, ref explainedNumber);
				}
				else if (attackerWeapon.RelevantSkill == DefaultSkills.Throwing && attackerWeapon.IsConsumable)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.KnockOff, currentBattleEnvironment, characterObject, true, ref explainedNumber);
				}
			}
			return MathF.Max(0f, explainedNumber.ResultNumber);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00036600 File Offset: 0x00034800
		public override float GetKnockBackPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			CharacterObject characterObject;
			if (attackerWeapon != null && attackerWeapon.RelevantSkill == DefaultSkills.Polearm && (characterObject = ((attackerAgent != null) ? attackerAgent.Character : null) as CharacterObject) != null && blow.StrikeType == StrikeType.Thrust)
			{
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.KeepAtBay, attackerAgent.CurrentBattleEnvironment, characterObject, true, ref explainedNumber);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00036668 File Offset: 0x00034868
		public override float GetKnockDownPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			BattleEnvironment currentBattleEnvironment = attackerAgent.CurrentBattleEnvironment;
			if (attackerWeapon.WeaponClass == WeaponClass.Boulder || attackerWeapon.WeaponClass == WeaponClass.BallistaBoulder)
			{
				explainedNumber.Add(0.25f, null, null);
			}
			else if (attackerWeapon.IsMeleeWeapon)
			{
				CharacterObject characterObject = ((attackerAgent != null) ? attackerAgent.Character : null) as CharacterObject;
				AttackCollisionData attackCollisionData;
				if (blow.StrikeType == StrikeType.Swing)
				{
					attackCollisionData = collisionData;
					if (attackCollisionData.VictimHitBodyPart == BoneBodyPartType.Legs)
					{
						explainedNumber.Add(0.1f, null, null);
					}
					if (characterObject != null && attackerWeapon.RelevantSkill == DefaultSkills.TwoHanded)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.ShowOfStrength, currentBattleEnvironment, characterObject, true, ref explainedNumber);
					}
				}
				attackCollisionData = collisionData;
				if (attackCollisionData.VictimHitBodyPart == BoneBodyPartType.Head)
				{
					explainedNumber.Add(0.15f, null, null);
				}
				if (characterObject != null && attackerWeapon.RelevantSkill == DefaultSkills.Polearm)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.HardKnock, currentBattleEnvironment, characterObject, true, ref explainedNumber);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0003675C File Offset: 0x0003495C
		public override float GetHorseChargePenetration()
		{
			return 0.4f;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00036764 File Offset: 0x00034964
		public override float CalculateStaggerThresholdDamage(Agent defenderAgent, in Blow blow)
		{
			float num = 1f;
			CharacterObject characterObject = defenderAgent.Character as CharacterObject;
			BattleEnvironment currentBattleEnvironment = defenderAgent.CurrentBattleEnvironment;
			Formation formation = defenderAgent.Formation;
			Agent agent = ((formation != null) ? formation.Captain : null);
			CharacterObject characterObject2 = ((agent != null) ? agent.Character : null) as CharacterObject;
			if (characterObject != null)
			{
				if (characterObject2 == characterObject)
				{
					characterObject2 = null;
				}
				ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
				if (defenderAgent.HasMount)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.DauntlessSteed, currentBattleEnvironment, characterObject, true, ref explainedNumber);
				}
				else
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.Spartan, currentBattleEnvironment, characterObject, true, ref explainedNumber);
				}
				WeaponComponentData currentUsageItem = defenderAgent.WieldedWeapon.CurrentUsageItem;
				if (currentUsageItem != null && currentUsageItem.WeaponClass == WeaponClass.Crossbow && defenderAgent.WieldedWeapon.IsReloading)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.DeftHands, currentBattleEnvironment, characterObject, true, ref explainedNumber);
					if (characterObject2 != null)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.DeftHands, currentBattleEnvironment, characterObject2, ref explainedNumber);
					}
				}
				num = explainedNumber.ResultNumber;
			}
			TaleWorlds.Core.ManagedParametersEnum managedParametersEnum;
			if (blow.DamageType == DamageTypes.Cut)
			{
				managedParametersEnum = TaleWorlds.Core.ManagedParametersEnum.DamageInterruptAttackThresholdCut;
			}
			else if (blow.DamageType == DamageTypes.Pierce)
			{
				managedParametersEnum = TaleWorlds.Core.ManagedParametersEnum.DamageInterruptAttackThresholdPierce;
			}
			else
			{
				managedParametersEnum = TaleWorlds.Core.ManagedParametersEnum.DamageInterruptAttackThresholdBlunt;
			}
			return TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(managedParametersEnum) * num;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0003687D File Offset: 0x00034A7D
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

		// Token: 0x060007FC RID: 2044 RVA: 0x000368BC File Offset: 0x00034ABC
		public override float CalculatePassiveAttackDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(baseDamage, false, null);
			CharacterObject characterObject = attackInformation.AttackerAgentCharacter as CharacterObject;
			if (characterObject != null)
			{
				AttackCollisionData attackCollisionData = collisionData;
				if (attackCollisionData.AttackBlockedWithShield)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.UnstoppableForce, attackInformation.AttackerBattleEnvironment, characterObject, true, ref explainedNumber);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00036910 File Offset: 0x00034B10
		public override MeleeCollisionReaction DecidePassiveAttackCollisionReaction(Agent attacker, Agent defender, bool isFatalHit)
		{
			MeleeCollisionReaction meleeCollisionReaction = MeleeCollisionReaction.Bounced;
			if (isFatalHit && attacker.HasMount)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(0.05f, false, null);
				CharacterObject characterObject;
				if ((characterObject = attacker.Character as CharacterObject) != null)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Skewer, attacker.CurrentBattleEnvironment, characterObject, true, ref explainedNumber);
				}
				float resultNumber = explainedNumber.ResultNumber;
				if (MBRandom.RandomFloat < resultNumber)
				{
					meleeCollisionReaction = MeleeCollisionReaction.SlicedThrough;
				}
			}
			return meleeCollisionReaction;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00036970 File Offset: 0x00034B70
		public override float CalculateShieldDamage(in AttackInformation attackInformation, float baseDamage)
		{
			Formation victimFormation = attackInformation.VictimFormation;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseDamage, false, null);
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(victimFormation);
			if (activeBanner != null)
			{
				BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedShieldDamage, activeBanner, ref explainedNumber);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000369B6 File Offset: 0x00034BB6
		public override float CalculateSailFireDamage(Agent attackerAgent, IShipOrigin shipOrigin, float baseDamage, bool damageFromShipMachine)
		{
			return baseDamage;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x000369BC File Offset: 0x00034BBC
		public override float CalculateHullFireDamage(float baseFireDamage, IShipOrigin shipOrigin)
		{
			return new ExplainedNumber(baseFireDamage, false, null).ResultNumber;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000369DC File Offset: 0x00034BDC
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
				num = (isHuman ? 1f : 0.8f);
				break;
			case BoneBodyPartType.Legs:
				num = 0.8f;
				break;
			}
			return num;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00036AF5 File Offset: 0x00034CF5
		public override bool DecideAgentShrugOffBlow(Agent victimAgent, in AttackCollisionData collisionData, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentShrugOffBlow(victimAgent, in collisionData, in blow);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00036AFF File Offset: 0x00034CFF
		public override bool DecideAgentDismountedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentDismountedByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00036B0D File Offset: 0x00034D0D
		public override bool DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentKnockedBackByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00036B1B File Offset: 0x00034D1B
		public override bool DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentKnockedDownByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00036B29 File Offset: 0x00034D29
		public override bool DecideMountRearedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideMountRearedByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00036B38 File Offset: 0x00034D38
		public override void DecideWeaponCollisionReaction(in Blow registeredBlow, in AttackCollisionData collisionData, Agent attacker, Agent defender, in MissionWeapon attackerWeapon, bool isFatalHit, bool isShruggedOff, float momentumRemaining, out MeleeCollisionReaction colReaction)
		{
			MissionCombatMechanicsHelper.DecideWeaponCollisionReaction(in registeredBlow, in collisionData, attacker, defender, in attackerWeapon, isFatalHit, isShruggedOff, momentumRemaining, out colReaction);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00036B59 File Offset: 0x00034D59
		public override bool ShouldMissilePassThroughAfterShieldBreak(Agent attackerAgent, WeaponComponentData attackerWeapon)
		{
			return false;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00036B5C File Offset: 0x00034D5C
		public override float CalculateRemainingMomentum(float originalMomentum, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough)
		{
			return base.CalculateDefaultRemainingMomentum(originalMomentum, in b, in collisionData, attacker, victim, in attackerWeapon, isCrushThrough);
		}

		// Token: 0x0400042F RID: 1071
		private const float SallyOutSiegeEngineDamageMultiplier = 4.5f;
	}
}
