using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010E RID: 270
	public class DefaultCombatSimulationModel : CombatSimulationModel
	{
		// Token: 0x060017CD RID: 6093 RVA: 0x00070474 File Offset: 0x0006E674
		public override ExplainedNumber SimulateHit(CharacterObject strikerTroop, CharacterObject struckTroop, PartyBase strikerParty, PartyBase struckParty, float strikerAdvantage, MapEvent battle, BattleEnvironment battleEnvironment, float strikerSideMorale, float struckSideMorale)
		{
			float troopPower = Campaign.Current.Models.MilitaryPowerModel.GetTroopPower(strikerTroop, strikerParty.Side, strikerParty.MapEvent.SimulationContext, strikerParty.MapEventSide.LeaderSimulationModifier);
			float troopPower2 = Campaign.Current.Models.MilitaryPowerModel.GetTroopPower(struckTroop, struckParty.Side, struckParty.MapEvent.SimulationContext, struckParty.MapEventSide.LeaderSimulationModifier);
			int num = (int)((0.5f + 0.5f * MBRandom.RandomFloat) * (40f * MathF.Pow(troopPower / troopPower2, 0.7f) * strikerAdvantage));
			ExplainedNumber explainedNumber = new ExplainedNumber((float)num, false, null);
			if (strikerParty.IsMobile && struckParty.IsMobile)
			{
				DefaultCombatSimulationModel.CalculateSimulationDamagePerkEffects(strikerTroop, struckTroop, strikerParty.MobileParty, struckParty.MobileParty, battle, battleEnvironment, ref explainedNumber);
			}
			DefaultCombatSimulationModel.CalculateSimulationMoraleEffects(strikerSideMorale, struckSideMorale, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00070554 File Offset: 0x0006E754
		public override ExplainedNumber SimulateHit(Ship strikerShip, Ship struckShip, PartyBase strikerParty, PartyBase struckParty, SiegeEngineType siegeEngine, float strikerAdvantage, MapEvent battle, out int troopCasualties)
		{
			troopCasualties = 0;
			return new ExplainedNumber(0f, false, null);
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00070568 File Offset: 0x0006E768
		private static void CalculateSimulationMoraleEffects(float strikerMorale, float struckMorale, ref ExplainedNumber effectiveDamage)
		{
			float num = MathF.Min(strikerMorale - 50f, 0f);
			float num2 = MathF.Max(struckMorale - 50f, 0f);
			effectiveDamage.AddFactor((num - num2) * 0.005f, null);
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x000705AC File Offset: 0x0006E7AC
		private static void CalculateSimulationDamagePerkEffects(CharacterObject strikerTroop, CharacterObject struckTroop, MobileParty strikerParty, MobileParty struckParty, MapEvent battle, BattleEnvironment battleEnvironment, ref ExplainedNumber effectiveDamage)
		{
			if (strikerTroop.IsInfantry && struckTroop.IsMounted)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.TightFormations, battleEnvironment, strikerParty, true, ref effectiveDamage);
			}
			if (struckTroop.IsInfantry && strikerTroop.IsRanged)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.LooseFormations, battleEnvironment, struckParty, true, ref effectiveDamage);
			}
			TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(strikerParty.CurrentNavigationFace);
			if (faceTerrainType == TerrainType.Snow || faceTerrainType == TerrainType.Forest)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.ExtendedSkirmish, battleEnvironment, strikerParty, true, ref effectiveDamage);
			}
			if (faceTerrainType == TerrainType.Plain || faceTerrainType == TerrainType.Steppe || faceTerrainType == TerrainType.Desert)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.DecisiveBattle, battleEnvironment, strikerParty, true, ref effectiveDamage);
			}
			if (!strikerParty.IsBandit && struckParty.IsBandit)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.LawKeeper, battleEnvironment, strikerParty, true, ref effectiveDamage);
			}
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.Coaching, battleEnvironment, strikerParty, true, ref effectiveDamage);
			if (struckTroop.Tier >= 3)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.EliteReserves, battleEnvironment, struckParty, true, ref effectiveDamage);
			}
			if (strikerParty.MemberRoster.TotalHealthyCount > struckParty.MemberRoster.TotalHealthyCount)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.Encirclement, battleEnvironment, strikerParty, true, ref effectiveDamage);
			}
			if (strikerParty.MemberRoster.TotalHealthyCount < struckParty.MemberRoster.TotalHealthyCount)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.Counteroffensive, battleEnvironment, strikerParty, false, ref effectiveDamage);
			}
			bool flag = false;
			using (List<MapEventParty>.Enumerator enumerator = battle.PartiesOnSide(BattleSideEnum.Defender).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Party == struckParty.Party)
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = !flag;
			bool flag3 = flag2;
			if (battle.IsSiegeAssault && flag2)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.Besieged, battleEnvironment, strikerParty, true, ref effectiveDamage);
			}
			if (flag)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Vanguard, battleEnvironment, strikerParty, true, ref effectiveDamage);
			}
			if ((battle.IsSiegeOutside || battle.IsSallyOut) && flag3)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Rearguard, battleEnvironment, strikerParty, false, ref effectiveDamage);
			}
			if (battle.IsSallyOut && flag)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Vanguard, battleEnvironment, strikerParty, false, ref effectiveDamage);
			}
			if (battle.IsFieldBattle && flag2)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.Counteroffensive, battleEnvironment, strikerParty, true, ref effectiveDamage);
			}
			if (strikerParty.Army != null && strikerParty.LeaderHero != null && strikerParty.Army.LeaderParty == strikerParty)
			{
				PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Tactics.TacticalMastery, battleEnvironment, strikerParty.LeaderHero.CharacterObject, DefaultSkills.Tactics, true, ref effectiveDamage, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus);
			}
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00070824 File Offset: 0x0006EA24
		public override float GetMaximumSiegeEquipmentProgress(Settlement settlement)
		{
			float num = 0f;
			if (settlement.SiegeEvent != null && settlement.IsFortification)
			{
				foreach (SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress in settlement.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.AllSiegeEngines())
				{
					if (!siegeEngineConstructionProgress.IsConstructed && siegeEngineConstructionProgress.Progress > num)
					{
						num = siegeEngineConstructionProgress.Progress;
					}
				}
			}
			return num;
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x000708AC File Offset: 0x0006EAAC
		public override int GetNumberOfEquipmentsBuilt(Settlement settlement)
		{
			if (settlement.SiegeEvent != null && settlement.IsFortification)
			{
				bool flag = false;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				foreach (SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress in settlement.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.AllSiegeEngines())
				{
					if (siegeEngineConstructionProgress.IsConstructed)
					{
						if (siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.Ram)
						{
							flag = true;
						}
						else if (siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.SiegeTower)
						{
							num++;
						}
						else if (siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.Trebuchet || siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.Onager || siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.Ballista)
						{
							num2++;
						}
						else if (siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.FireOnager || siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.FireBallista)
						{
							num3++;
						}
					}
				}
				return (flag ? 1 : 0) + num + num2 + num3;
			}
			return 0;
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000709BC File Offset: 0x0006EBBC
		public override float GetSettlementAdvantage(Settlement settlement)
		{
			if (settlement.SiegeEvent != null && settlement.IsFortification)
			{
				int wallLevel = settlement.Town.GetWallLevel();
				bool flag = false;
				bool flag2 = false;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				foreach (SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress in settlement.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.AllSiegeEngines())
				{
					if (siegeEngineConstructionProgress.IsConstructed)
					{
						if (siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.Ram || siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.ImprovedRam)
						{
							if (siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.ImprovedRam)
							{
								flag2 = true;
							}
							flag = true;
						}
						else if (siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.SiegeTower)
						{
							num++;
						}
						else if (siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.Trebuchet || siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.Onager || siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.Ballista)
						{
							num2++;
						}
						else if (siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.FireOnager || siegeEngineConstructionProgress.SiegeEngine == DefaultSiegeEngineTypes.FireBallista)
						{
							num3++;
						}
					}
				}
				float num4 = 4f + (float)(wallLevel - 1);
				if (settlement.SettlementTotalWallHitPoints < 1E-05f)
				{
					num4 *= 0.25f;
				}
				float num5 = 1f + num4;
				float num6 = 1f + ((flag | (num > 0)) ? 0.25f : 0f) + (flag2 ? 0.24f : (flag ? 0.16f : 0f)) + ((num > 1) ? 0.24f : ((num == 1) ? 0.16f : 0f)) + (float)num2 * 0.08f + (float)num3 * 0.12f;
				float num7 = num5 / num6;
				ExplainedNumber explainedNumber = new ExplainedNumber(num7, false, null);
				ISiegeEventSide siegeEventSide = settlement.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker);
				DefaultCombatSimulationModel.CalculateSettlementAdvantagePerkEffects(settlement, ref explainedNumber, siegeEventSide);
				return explainedNumber.ResultNumber;
			}
			if (settlement.IsVillage)
			{
				return 1.25f;
			}
			return 1f;
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00070BC4 File Offset: 0x0006EDC4
		private static void CalculateSettlementAdvantagePerkEffects(Settlement settlement, ref ExplainedNumber effectiveAdvantage, ISiegeEventSide opposingSide)
		{
			foreach (PartyBase partyBase in opposingSide.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege))
			{
				if (PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.OnTheMarch, BattleEnvironment.Any, partyBase.MobileParty, true, ref effectiveAdvantage))
				{
					break;
				}
			}
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Tactics.OnTheMarch, settlement.Town, false, ref effectiveAdvantage);
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00070C38 File Offset: 0x0006EE38
		[return: TupleElementNames(new string[] { "defenderRounds", "attackerRounds" })]
		public override ValueTuple<int, int> GetSimulationTicksForBattleRound(MapEvent mapEvent)
		{
			MapEvent.BattleTypes eventType = mapEvent.EventType;
			Settlement mapEventSettlement = mapEvent.MapEventSettlement;
			int num = 0;
			int num2 = 0;
			int numRemainingSimulationTroops = mapEvent.DefenderSide.NumRemainingSimulationTroops;
			int numRemainingSimulationTroops2 = mapEvent.AttackerSide.NumRemainingSimulationTroops;
			if (!mapEvent.IsInvulnerable)
			{
				if (eventType == MapEvent.BattleTypes.Siege && mapEventSettlement.CurrentSiegeState != Settlement.SiegeState.InTheLordsHall && ((mapEventSettlement.IsTown && numRemainingSimulationTroops > 100) || (mapEventSettlement.IsCastle && numRemainingSimulationTroops > 30)))
				{
					float num3 = this.GetSettlementAdvantage(mapEventSettlement) * 0.7f;
					num2 = MathF.Round(1.5f + MathF.Pow((float)numRemainingSimulationTroops, 0.3f)) * 2;
					num = MathF.Round(0.5f + MathF.Max(1f + MathF.Pow((float)numRemainingSimulationTroops, 0.3f) * num3, (float)((numRemainingSimulationTroops + 1) / (numRemainingSimulationTroops2 + 1)))) * 2;
				}
				else if (numRemainingSimulationTroops <= 10)
				{
					num = Math.Max(MathF.Round(MathF.Min((float)numRemainingSimulationTroops2 * 3f, (float)numRemainingSimulationTroops * 0.3f)), 1);
					num2 = Math.Max(MathF.Round(MathF.Min((float)numRemainingSimulationTroops * 3f, (float)numRemainingSimulationTroops2 * 0.3f)), 1);
				}
				else
				{
					num = MathF.Round(MathF.Min((float)numRemainingSimulationTroops2 * 2f, MathF.Pow((float)numRemainingSimulationTroops, 0.6f)));
					num2 = MathF.Round(MathF.Min((float)numRemainingSimulationTroops * 2f, MathF.Pow((float)numRemainingSimulationTroops2, 0.6f)));
				}
				if (mapEvent.RetreatingSide != BattleSideEnum.None)
				{
					if (mapEvent.RetreatingSide == BattleSideEnum.Attacker)
					{
						num2 = 0;
					}
					else
					{
						num = 0;
					}
				}
			}
			return new ValueTuple<int, int>(num, num2);
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00070DBC File Offset: 0x0006EFBC
		public override void GetBattleAdvantage(MapEvent mapEvent, out ExplainedNumber defenderAdvantage, out ExplainedNumber attackerAdvantage)
		{
			defenderAdvantage = DefaultCombatSimulationModel.GetPartyBattleAdvantage(mapEvent, mapEvent.DefenderSide.LeaderParty, mapEvent.AttackerSide.LeaderParty);
			attackerAdvantage = DefaultCombatSimulationModel.GetPartyBattleAdvantage(mapEvent, mapEvent.AttackerSide.LeaderParty, mapEvent.DefenderSide.LeaderParty);
			if (mapEvent.EventType == MapEvent.BattleTypes.Siege)
			{
				attackerAdvantage.AddFactor(-0.1f, null);
			}
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00070E24 File Offset: 0x0006F024
		private static ExplainedNumber GetPartyBattleAdvantage(MapEvent mapEvent, PartyBase party, PartyBase opposingParty)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			if (party.LeaderHero != null)
			{
				if (!mapEvent.IsNavalMapEvent)
				{
					SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.TacticsAdvantage, party.LeaderHero.CharacterObject, ref explainedNumber);
				}
				if (party.IsMobile && opposingParty.Culture.IsBandit)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Patrols, party.MobileParty, false, ref explainedNumber);
				}
			}
			Hero hero = null;
			if (party.IsMobile && opposingParty.IsMobile && opposingParty.LeaderHero != null && party.MobileParty.HasPerk(DefaultPerks.Tactics.PreBattleManeuvers, out hero, true))
			{
				int num = hero.GetSkillValue(DefaultSkills.Tactics) - opposingParty.LeaderHero.GetSkillValue(DefaultSkills.Tactics);
				if (num > 0)
				{
					float num2 = (float)num * 0.01f;
					explainedNumber.Add(num2, null, null);
				}
			}
			return explainedNumber;
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00070EF3 File Offset: 0x0006F0F3
		public override float GetShipSiegeEngineHitChance(Ship ship, SiegeEngineType siegeEngineType, BattleSideEnum battleSide)
		{
			return 0f;
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00070EFA File Offset: 0x0006F0FA
		public override int GetPursuitRoundCount(MapEvent mapEvent)
		{
			return 4;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x00070EFD File Offset: 0x0006F0FD
		public override float GetBluntDamageChance(CharacterObject strikerTroop, CharacterObject strikedTroop, PartyBase strikerParty, PartyBase strikedParty, MapEvent battle)
		{
			if (battle.IsPlayerMapEvent)
			{
				return 0.3f;
			}
			return 0.1f;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00070F13 File Offset: 0x0006F113
		public override CampaignTime GetSimulationTickInterval(MapEvent mapEvent)
		{
			if (mapEvent.IsSiegeAssault)
			{
				return CampaignTime.Minutes(60L);
			}
			return CampaignTime.Minutes(30L);
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00070F2E File Offset: 0x0006F12E
		public override int GetParticipatingTroopCount(MapEventSide side)
		{
			return side.HealthyTroopCountAtMapEventStart;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00070F36 File Offset: 0x0006F136
		public override float GetShipCombatImportance(Ship ship)
		{
			return 0f;
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00070F3D File Offset: 0x0006F13D
		public override float GetShipCombatScore(Ship ship)
		{
			return 0f;
		}
	}
}
