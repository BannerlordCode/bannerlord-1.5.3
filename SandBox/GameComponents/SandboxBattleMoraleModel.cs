using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace SandBox.GameComponents
{
	// Token: 0x020000C8 RID: 200
	public class SandboxBattleMoraleModel : BattleMoraleModel
	{
		// Token: 0x06000832 RID: 2098 RVA: 0x0003A050 File Offset: 0x00038250
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public override ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentIncapacitated(Agent affectedAgent, AgentState affectedAgentState, Agent affectorAgent, in KillingBlow killingBlow)
		{
			Team team = affectedAgent.Team;
			float num = this.CalculateCasualtiesFactor((team != null) ? team.Side : BattleSideEnum.None);
			ValueTuple<ExplainedNumber, ExplainedNumber> valueTuple = SandboxBattleMoraleModel.CalculateMaxMoraleChangeDueToAgentIncapacitatedExplained(affectedAgent, affectedAgentState, affectorAgent, in killingBlow, num);
			ExplainedNumber item = valueTuple.Item1;
			ExplainedNumber item2 = valueTuple.Item2;
			return new ValueTuple<float, float>(MathF.Max(item.ResultNumber, 0f), MathF.Max(item2.ResultNumber, 0f));
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0003A0B8 File Offset: 0x000382B8
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public override ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentPanicked(Agent agent)
		{
			float battleImportance = agent.GetBattleImportance();
			Team team = agent.Team;
			BattleSideEnum battleSideEnum = ((team != null) ? team.Side : BattleSideEnum.None);
			float num = this.CalculateCasualtiesFactor(battleSideEnum);
			float num2 = battleImportance * 2f;
			float num3 = battleImportance * num * 1.1f;
			if (((agent != null) ? agent.Character : null) is CharacterObject)
			{
				BattleEnvironment currentBattleEnvironment = agent.CurrentBattleEnvironment;
				ExplainedNumber explainedNumber = new ExplainedNumber(num3, false, null);
				Formation formation = agent.Formation;
				Agent agent2 = ((formation != null) ? formation.Captain : null);
				CharacterObject characterObject = ((agent2 != null) ? agent2.Character : null) as CharacterObject;
				BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(formation);
				if (characterObject != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.StandardBearer, currentBattleEnvironment, characterObject, ref explainedNumber);
				}
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
				MobileParty mobileParty = ((partyBase != null) ? partyBase.MobileParty : null);
				Hero hero = ((mobileParty != null) ? mobileParty.EffectiveQuartermaster : null);
				if (hero != null)
				{
					PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Steward.PriceOfLoyalty, currentBattleEnvironment, hero.CharacterObject, DefaultSkills.Steward, true, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
				}
				if (activeBanner != null)
				{
					BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedMoraleShock, activeBanner, ref explainedNumber);
				}
				num3 = explainedNumber.ResultNumber;
			}
			return new ValueTuple<float, float>(MathF.Max(num3, 0f), MathF.Max(num2, 0f));
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0003A210 File Offset: 0x00038410
		public override float GetEffectiveInitialMorale(Agent agent, float baseMorale)
		{
			return SandboxBattleMoraleModel.GetEffectiveInitialMoraleExplained(agent, baseMorale).ResultNumber;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0003A22C File Offset: 0x0003842C
		public override float CalculateMoraleChangeToCharacter(Agent agent, float maxMoraleChange)
		{
			return maxMoraleChange / MathF.Max(1f, agent.Character.GetMoraleResistance());
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0003A248 File Offset: 0x00038448
		public override bool CanPanicDueToMorale(Agent agent)
		{
			bool flag = true;
			if (agent.IsHuman)
			{
				CharacterObject characterObject = agent.Character as CharacterObject;
				IAgentOriginBase origin = agent.Origin;
				PartyBase partyBase = (PartyBase)((origin != null) ? origin.BattleCombatant : null);
				Hero hero = ((partyBase != null) ? partyBase.LeaderHero : null);
				float num;
				if (characterObject != null && hero != null && hero.GetPerkValue(DefaultPerks.Leadership.LoyaltyAndHonor, true, out num, agent.CurrentBattleEnvironment) && characterObject.Tier >= (int)num)
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0003A2B8 File Offset: 0x000384B8
		public override float CalculateCasualtiesFactor(BattleSideEnum battleSide)
		{
			float num = 1f;
			if (Mission.Current != null && battleSide != BattleSideEnum.None)
			{
				float removedAgentRatioForSide = Mission.Current.GetRemovedAgentRatioForSide(battleSide);
				num += removedAgentRatioForSide * 2f;
				num = MathF.Max(0f, num);
			}
			return num;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0003A2FC File Offset: 0x000384FC
		public override float GetAverageMorale(Formation formation)
		{
			float num = 0f;
			int num2 = 0;
			if (formation != null)
			{
				using (List<IFormationUnit>.Enumerator enumerator = formation.Arrangement.GetAllUnits().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent;
						if ((agent = enumerator.Current as Agent) != null && agent.IsHuman && agent.IsAIControlled)
						{
							num2++;
							num += agent.GetMorale();
						}
					}
				}
			}
			if (num2 > 0)
			{
				return MBMath.ClampFloat(num / (float)num2, 0f, 100f);
			}
			return 0f;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0003A39C File Offset: 0x0003859C
		public override float CalculateMoraleChangeOnShipSunk(IShipOrigin shipOrigin)
		{
			return 0f;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0003A3A3 File Offset: 0x000385A3
		public override float CalculateMoraleOnRamming(Agent agent, IShipOrigin rammingShip, IShipOrigin rammedShip)
		{
			return agent.GetMorale();
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0003A3AC File Offset: 0x000385AC
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public static ValueTuple<ExplainedNumber, ExplainedNumber> CalculateMaxMoraleChangeDueToAgentIncapacitatedExplained(Agent affectedAgent, AgentState affectedAgentState, Agent affectorAgent, in KillingBlow killingBlow, float casualtiesFactor)
		{
			CharacterObject characterObject = ((affectorAgent != null) ? affectorAgent.Character : null) as CharacterObject;
			BattleEnvironment battleEnvironment = ((affectorAgent != null) ? affectorAgent.CurrentBattleEnvironment : BattleEnvironment.None);
			bool flag = ((affectedAgent != null) ? affectedAgent.Character : null) is CharacterObject;
			BattleEnvironment battleEnvironment2 = ((affectedAgent != null) ? affectedAgent.CurrentBattleEnvironment : BattleEnvironment.None);
			SkillObject relevantSkillFromWeaponClass = WeaponComponentData.GetRelevantSkillFromWeaponClass((WeaponClass)killingBlow.WeaponClass);
			bool flag2 = relevantSkillFromWeaponClass == DefaultSkills.OneHanded || relevantSkillFromWeaponClass == DefaultSkills.TwoHanded || relevantSkillFromWeaponClass == DefaultSkills.Polearm;
			bool flag3 = relevantSkillFromWeaponClass == DefaultSkills.Bow || relevantSkillFromWeaponClass == DefaultSkills.Crossbow || relevantSkillFromWeaponClass == DefaultSkills.Throwing;
			bool flag4 = killingBlow.WeaponRecordWeaponFlags.HasAnyFlag(WeaponFlags.AffectsArea | WeaponFlags.AffectsAreaBig | WeaponFlags.MultiplePenetration);
			float battleImportance = affectedAgent.GetBattleImportance();
			float num = 0.75f;
			if (flag4)
			{
				num = 0.25f;
				if (killingBlow.WeaponRecordWeaponFlags.HasAllFlags(WeaponFlags.Burning | WeaponFlags.MultiplePenetration))
				{
					num += num * 0.25f;
				}
			}
			else if (flag3)
			{
				num = 0.5f;
			}
			num = Math.Max(0f, num);
			ExplainedNumber explainedNumber = new ExplainedNumber(battleImportance * 3f * num, false, null);
			ExplainedNumber explainedNumber2 = new ExplainedNumber(battleImportance * 4f * num * casualtiesFactor, false, null);
			if (characterObject != null)
			{
				object obj;
				if (affectorAgent == null)
				{
					obj = null;
				}
				else
				{
					Formation formation = affectorAgent.Formation;
					obj = ((formation != null) ? formation.Captain : null);
				}
				object obj2 = obj;
				CharacterObject characterObject2 = ((obj2 != null) ? obj2.Character : null) as CharacterObject;
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Leadership.MakeADifference, battleEnvironment, characterObject, true, ref explainedNumber);
				if (flag2)
				{
					if (relevantSkillFromWeaponClass == DefaultSkills.TwoHanded)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.Hope, battleEnvironment, characterObject, true, ref explainedNumber);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.Terror, battleEnvironment, characterObject, true, ref explainedNumber2);
					}
					if (affectorAgent != null && affectorAgent.HasMount)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.ThunderousCharge, battleEnvironment, characterObject, true, ref explainedNumber2);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.ThunderousCharge, battleEnvironment, characterObject2, ref explainedNumber2);
					}
				}
				else if (flag3)
				{
					if (relevantSkillFromWeaponClass == DefaultSkills.Crossbow)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.Terror, battleEnvironment, characterObject2, ref explainedNumber2);
					}
					if (affectorAgent != null && affectorAgent.HasMount)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.AnnoyingBuzz, battleEnvironment, characterObject, true, ref explainedNumber2);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.AnnoyingBuzz, battleEnvironment, characterObject2, ref explainedNumber2);
					}
				}
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Leadership.HeroicLeader, battleEnvironment, characterObject2, ref explainedNumber2);
			}
			if (flag)
			{
				object obj3;
				if (affectedAgent == null)
				{
					obj3 = null;
				}
				else
				{
					IAgentOriginBase origin = affectedAgent.Origin;
					obj3 = ((origin != null) ? origin.BattleCombatant : null);
				}
				PartyBase partyBase = obj3 as PartyBase;
				MobileParty mobileParty = ((partyBase != null) ? partyBase.MobileParty : null);
				Hero hero = null;
				if (affectedAgentState == AgentState.Unconscious && mobileParty != null && mobileParty.HasPerk(DefaultPerks.Medicine.HealthAdvise, out hero, true))
				{
					explainedNumber2 = default(ExplainedNumber);
				}
				else
				{
					Formation formation2 = affectedAgent.Formation;
					Agent agent = ((formation2 != null) ? formation2.Captain : null);
					CharacterObject characterObject3;
					if ((characterObject3 = ((agent != null) ? agent.Character : null) as CharacterObject) != null)
					{
						ArrangementOrder arrangementOrder = affectedAgent.Formation.ArrangementOrder;
						if (arrangementOrder == ArrangementOrder.ArrangementOrderShieldWall || arrangementOrder == ArrangementOrder.ArrangementOrderSquare || arrangementOrder == ArrangementOrder.ArrangementOrderSkein || arrangementOrder == ArrangementOrder.ArrangementOrderColumn)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.TightFormations, battleEnvironment2, characterObject3, ref explainedNumber2);
						}
						if (arrangementOrder == ArrangementOrder.ArrangementOrderLine || arrangementOrder == ArrangementOrder.ArrangementOrderLoose || arrangementOrder == ArrangementOrder.ArrangementOrderCircle || arrangementOrder == ArrangementOrder.ArrangementOrderScatter)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.LooseFormations, battleEnvironment2, characterObject3, ref explainedNumber2);
						}
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.StandardBearer, battleEnvironment2, characterObject3, ref explainedNumber2);
					}
					Hero hero2 = ((mobileParty != null) ? mobileParty.EffectiveQuartermaster : null);
					if (hero2 != null)
					{
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Steward.PriceOfLoyalty, battleEnvironment2, hero2.CharacterObject, DefaultSkills.Steward, true, ref explainedNumber2, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
					}
				}
			}
			Formation formation3 = affectedAgent.Formation;
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(formation3);
			if (activeBanner != null)
			{
				BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedMoraleShock, activeBanner, ref explainedNumber2);
			}
			Formation formation4 = affectorAgent.Formation;
			BannerComponent activeBanner2 = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(formation4);
			if (activeBanner2 != null && affectorAgent.Character.DefaultFormationClass == FormationClass.Infantry && flag2)
			{
				BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMoraleShockByMeleeTroops, activeBanner2, ref explainedNumber);
			}
			return new ValueTuple<ExplainedNumber, ExplainedNumber>(explainedNumber2, explainedNumber);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0003A7A9 File Offset: 0x000389A9
		public override float CalculateMoraleOnShipsConnected(Agent agent, IShipOrigin ownerShip, IShipOrigin targetShip)
		{
			return agent.GetMorale();
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0003A7B4 File Offset: 0x000389B4
		public static ExplainedNumber GetEffectiveInitialMoraleExplained(Agent agent, float baseMorale)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(baseMorale, false, null);
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
			MobileParty mobileParty = ((partyBase != null && partyBase.IsMobile) ? partyBase.MobileParty : null);
			CharacterObject characterObject = ((agent != null) ? agent.Character : null) as CharacterObject;
			if (mobileParty != null && characterObject != null)
			{
				BattleEnvironment currentBattleEnvironment = agent.CurrentBattleEnvironment;
				Army army = mobileParty.Army;
				CharacterObject characterObject2;
				if (army == null)
				{
					characterObject2 = null;
				}
				else
				{
					MobileParty leaderParty = army.LeaderParty;
					if (leaderParty == null)
					{
						characterObject2 = null;
					}
					else
					{
						Hero leaderHero = leaderParty.LeaderHero;
						characterObject2 = ((leaderHero != null) ? leaderHero.CharacterObject : null);
					}
				}
				CharacterObject characterObject3 = characterObject2;
				Hero leaderHero2 = mobileParty.LeaderHero;
				CharacterObject characterObject4 = ((leaderHero2 != null) ? leaderHero2.CharacterObject : null);
				CharacterObject characterObject5 = ((characterObject3 != characterObject) ? characterObject3 : null);
				characterObject4 = ((characterObject4 != characterObject) ? characterObject4 : null);
				if (characterObject4 != null)
				{
					if (partyBase.Side == BattleSideEnum.Attacker)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.FerventAttacker, currentBattleEnvironment, mobileParty, true, ref explainedNumber);
					}
					else if (partyBase.Side == BattleSideEnum.Defender)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.StoutDefender, currentBattleEnvironment, mobileParty, true, ref explainedNumber);
					}
					if (characterObject4.Culture == characterObject.Culture)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.GreatLeader, currentBattleEnvironment, mobileParty, false, ref explainedNumber);
					}
					if (characterObject4.GetPerkValue(DefaultPerks.Leadership.WePledgeOurSwords))
					{
						int num = MathF.Min(partyBase.GetNumberOfHealthyMenOfTier(6), 10);
						explainedNumber.Add((float)num, null, null);
					}
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.LastHit, currentBattleEnvironment, mobileParty, false, ref explainedNumber);
					PartyBase partyBase2;
					if (partyBase == null)
					{
						partyBase2 = null;
					}
					else
					{
						MapEventSide mapEventSide = partyBase.MapEventSide;
						partyBase2 = ((mapEventSide != null) ? mapEventSide.LeaderParty : null);
					}
					PartyBase partyBase3 = partyBase2;
					if (partyBase3 != null && partyBase != partyBase3)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Riding.ReliefForce, currentBattleEnvironment, mobileParty, true, ref explainedNumber);
					}
					if (partyBase.MapEvent != null)
					{
						float num2;
						float num3;
						partyBase.MapEvent.GetStrengthsRelativeToParty(partyBase.Side, out num2, out num3);
						if (num2 < num3)
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.StandUnited, currentBattleEnvironment, mobileParty, true, ref explainedNumber);
						}
						if (partyBase.MapEvent.IsSiegeAssault || partyBase.MapEvent.IsSiegeOutside)
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.UpliftingSpirit, currentBattleEnvironment, mobileParty, true, ref explainedNumber);
						}
						bool flag = false;
						foreach (PartyBase partyBase4 in partyBase.MapEvent.InvolvedParties)
						{
							if (partyBase4.Side != partyBase.Side && partyBase4.MapFaction != null && partyBase4.Culture.IsBandit)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Patrols, currentBattleEnvironment, mobileParty, true, ref explainedNumber);
						}
					}
					PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.LeadByExample, currentBattleEnvironment, mobileParty, false, ref explainedNumber);
				}
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.GreatLeader, currentBattleEnvironment, mobileParty, true, ref explainedNumber);
				Army army2 = mobileParty.Army;
				Hero hero;
				if (army2 == null)
				{
					hero = null;
				}
				else
				{
					MobileParty leaderParty2 = army2.LeaderParty;
					hero = ((leaderParty2 != null) ? leaderParty2.LeaderHero : null);
				}
				Hero hero2 = hero ?? mobileParty.LeaderHero;
				if (hero2 != null && hero2.CharacterObject != characterObject)
				{
					TraitEffectHelper.ApplyTraitEffect(hero2, DefaultPersonalityTraitEffects.CalculatingCombatMoraleEffect, ref explainedNumber);
				}
				if (characterObject.IsRanged)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.RenownedArcher, currentBattleEnvironment, partyBase.MobileParty, true, ref explainedNumber);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Crossbow.Marksmen, currentBattleEnvironment, partyBase.MobileParty, false, ref explainedNumber);
				}
				if (mobileParty.IsDisorganized && (mobileParty.MapEvent == null || mobileParty.SiegeEvent == null || mobileParty.MapEventSide.MissionSide != BattleSideEnum.Attacker) && (characterObject4 == null || !characterObject4.GetPerkValue(DefaultPerks.Tactics.Improviser)))
				{
					explainedNumber.AddFactor(-0.2f, null);
				}
			}
			return explainedNumber;
		}
	}
}
