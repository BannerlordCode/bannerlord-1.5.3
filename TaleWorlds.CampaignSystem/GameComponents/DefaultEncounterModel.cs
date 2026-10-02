using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011B RID: 283
	public class DefaultEncounterModel : EncounterModel
	{
		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x0600186A RID: 6250 RVA: 0x0007599C File Offset: 0x00073B9C
		public override float NeededMaximumLandDistanceForEncounteringMobileParty
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x0600186B RID: 6251 RVA: 0x000759A3 File Offset: 0x00073BA3
		public override float NeededMaximumNavalDistanceForEncounteringMobileParty
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600186C RID: 6252 RVA: 0x000759AA File Offset: 0x00073BAA
		public override float MaximumAllowedLandDistanceForEncounteringMobilePartyInArmy
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x0600186D RID: 6253 RVA: 0x000759B1 File Offset: 0x00073BB1
		public override float MaximumAllowedNavalDistanceForEncounteringMobilePartyInArmy
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x0600186E RID: 6254 RVA: 0x000759B8 File Offset: 0x00073BB8
		public override float NeededMaximumDistanceForEncounteringTown
		{
			get
			{
				return 0.05f;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x0600186F RID: 6255 RVA: 0x000759BF File Offset: 0x00073BBF
		public override float NeededMaximumDistanceForEncounteringBlockade
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001870 RID: 6256 RVA: 0x000759C6 File Offset: 0x00073BC6
		public override float NeededMaximumDistanceForEncounteringVillage
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001871 RID: 6257 RVA: 0x000759CD File Offset: 0x00073BCD
		public override float GetEncounterJoiningRadius
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001872 RID: 6258 RVA: 0x000759D4 File Offset: 0x00073BD4
		public override float PlayerParleyDistance
		{
			get
			{
				return MobileParty.MainParty.SeeingRange;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001873 RID: 6259 RVA: 0x000759E0 File Offset: 0x00073BE0
		public override float GetSettlementBeingNearFieldBattleRadius
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001874 RID: 6260 RVA: 0x000759E7 File Offset: 0x00073BE7
		public override int MinimumNumberOfMenForAttackingVillageViaScene
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x000759EA File Offset: 0x00073BEA
		public override bool IsEncounterExemptFromHostileActions(PartyBase side1, PartyBase side2)
		{
			return side1 == null || side2 == null || (side1.IsMobile && side1.MobileParty.AvoidHostileActions) || (side2.IsMobile && side2.MobileParty.AvoidHostileActions);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x00075A20 File Offset: 0x00073C20
		public override Hero GetLeaderOfSiegeEvent(SiegeEvent siegeEvent, BattleSideEnum side)
		{
			IEnumerable<PartyBase> involvedPartiesForEventType = siegeEvent.GetSiegeEventSide(side).GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege);
			if (involvedPartiesForEventType.Count<PartyBase>() == 1)
			{
				return involvedPartiesForEventType.ElementAt<PartyBase>(0).LeaderHero;
			}
			IFaction faction = ((side == BattleSideEnum.Attacker) ? siegeEvent.BesiegerCamp.MapFaction : siegeEvent.BesiegedSettlement.MapFaction);
			return this.GetLeaderOfEventInternal(involvedPartiesForEventType, faction);
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00075A78 File Offset: 0x00073C78
		public override bool CanMainHeroDoParleyWithParty(PartyBase partyBase, out TextObject explanation)
		{
			bool flag = true;
			explanation = null;
			if (MapEvent.PlayerMapEvent == null && Settlement.CurrentSettlement == null && MobileParty.MainParty.IsActive && !Hero.MainHero.IsPrisoner && partyBase.MapFaction != null && (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty) && partyBase.MapFaction.IsAtWarWith(Clan.PlayerClan.MapFaction))
			{
				if (partyBase.MapFaction.IsRebelClan)
				{
					explanation = new TextObject("{=6LG4BDZZ}You can't start parley with Rebels.", null);
					flag = false;
				}
				else
				{
					if (partyBase.IsMobile)
					{
						return false;
					}
					if (partyBase.IsSettlement && partyBase.Settlement.IsFortification && !partyBase.Settlement.IsUnderSiege && partyBase.Settlement.IsInspected)
					{
						Settlement settlement = partyBase.Settlement;
						float num;
						bool flag2 = Campaign.Current.Models.MapDistanceModel.GetDistance(MobileParty.MainParty, settlement, MobileParty.MainParty.IsCurrentlyAtSea && settlement.HasPort, MobileParty.MainParty.NavigationCapability, out num) < Campaign.Current.Models.EncounterModel.PlayerParleyDistance;
						bool flag3;
						if (!Campaign.Current.Models.SettlementAccessModel.IsRequestMeetingOptionAvailable(settlement, out flag3, out explanation) || flag3)
						{
							flag = false;
						}
						else if (!flag2)
						{
							explanation = new TextObject("{=Y8JPgz1c}You are too far away from {SETTLEMENT} to start parley.", null);
							explanation.SetTextVariable("SETTLEMENT", partyBase.Settlement.Name);
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00075C1C File Offset: 0x00073E1C
		public override Hero GetLeaderOfMapEvent(MapEvent mapEvent, BattleSideEnum side)
		{
			IFaction faction = ((side == BattleSideEnum.Attacker) ? mapEvent.AttackerSide.LeaderParty.MapFaction : mapEvent.DefenderSide.LeaderParty.MapFaction);
			return this.GetLeaderOfEventInternal(mapEvent.GetMapEventSide(side).Parties.Select<MapEventParty, PartyBase>((MapEventParty x) => x.Party), faction);
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00075C87 File Offset: 0x00073E87
		private bool IsArmyLeader(Hero hero)
		{
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			return ((partyBelongedTo != null) ? partyBelongedTo.Army : null) != null && hero.PartyBelongedTo.Army.LeaderParty == hero.PartyBelongedTo;
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00075CB7 File Offset: 0x00073EB7
		private int GetLeadingScore(Hero hero)
		{
			if (!hero.IsKingdomLeader && !this.IsArmyLeader(hero))
			{
				return this.GetCharacterSergeantScore(hero);
			}
			return (int)hero.PartyBelongedTo.GetTotalLandStrengthWithFollowers(true);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00075CE0 File Offset: 0x00073EE0
		private Hero GetLeaderOfEventInternal(IEnumerable<PartyBase> allPartiesThatBelongToASide, IFaction eventFaction)
		{
			Hero hero = null;
			int num = 0;
			foreach (PartyBase partyBase in allPartiesThatBelongToASide)
			{
				Hero leaderHero = partyBase.LeaderHero;
				if (leaderHero != null)
				{
					int leadingScore = this.GetLeadingScore(leaderHero);
					if (hero == null)
					{
						hero = leaderHero;
						num = leadingScore;
					}
					bool flag = leaderHero.MapFaction == eventFaction;
					bool isKingdomLeader = leaderHero.IsKingdomLeader;
					bool flag2 = this.IsArmyLeader(leaderHero);
					bool flag3 = hero.MapFaction == eventFaction;
					bool isKingdomLeader2 = hero.IsKingdomLeader;
					bool flag4 = this.IsArmyLeader(hero);
					if (!flag3 && flag)
					{
						hero = leaderHero;
						num = leadingScore;
					}
					else if (flag == flag3)
					{
						if (isKingdomLeader)
						{
							if (!isKingdomLeader2 || leadingScore > num)
							{
								hero = leaderHero;
								num = leadingScore;
							}
						}
						else if (flag2)
						{
							if ((!isKingdomLeader2 && !flag4) || (flag4 && !isKingdomLeader2 && leadingScore > num))
							{
								hero = leaderHero;
								num = leadingScore;
							}
						}
						else if (!isKingdomLeader2 && !flag4 && leadingScore > num)
						{
							hero = leaderHero;
							num = leadingScore;
						}
					}
				}
			}
			return hero;
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00075DE0 File Offset: 0x00073FE0
		public override int GetCharacterSergeantScore(Hero hero)
		{
			int num = 0;
			Clan clan = hero.Clan;
			if (clan != null)
			{
				num += clan.Tier * ((hero == clan.Leader) ? 100 : 20);
				if (clan.Kingdom != null && clan.Kingdom.Leader == hero)
				{
					num += 2000;
				}
			}
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			if (partyBelongedTo != null)
			{
				if (partyBelongedTo.Army != null && partyBelongedTo.Army.LeaderParty == partyBelongedTo)
				{
					num += partyBelongedTo.Army.Parties.Count * 200;
				}
				num += partyBelongedTo.MemberRoster.TotalManCount - partyBelongedTo.MemberRoster.TotalWounded;
			}
			return num;
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00075E84 File Offset: 0x00074084
		public override IEnumerable<PartyBase> GetDefenderPartiesOfSettlement(Settlement settlement, MapEvent.BattleTypes mapEventType)
		{
			if (settlement.IsFortification)
			{
				return settlement.Town.GetDefenderParties(mapEventType);
			}
			if (settlement.IsVillage)
			{
				return settlement.Village.GetDefenderParties(mapEventType);
			}
			if (settlement.IsHideout)
			{
				return settlement.Hideout.GetDefenderParties(mapEventType);
			}
			return null;
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00075ED4 File Offset: 0x000740D4
		public override PartyBase GetNextDefenderPartyOfSettlement(Settlement settlement, ref int partyIndex, MapEvent.BattleTypes mapEventType)
		{
			if (settlement.IsFortification)
			{
				return settlement.Town.GetNextDefenderParty(ref partyIndex, mapEventType);
			}
			if (settlement.IsVillage)
			{
				return settlement.Village.GetNextDefenderParty(ref partyIndex, mapEventType);
			}
			if (settlement.IsHideout)
			{
				return settlement.Hideout.GetNextDefenderParty(ref partyIndex, mapEventType);
			}
			return null;
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x00075F24 File Offset: 0x00074124
		public override MapEventComponent CreateMapEventComponentForEncounter(PartyBase attackerParty, PartyBase defenderParty, MapEvent.BattleTypes battleType)
		{
			MapEventComponent mapEventComponent = null;
			switch (battleType)
			{
			case MapEvent.BattleTypes.FieldBattle:
				mapEventComponent = FieldBattleEventComponent.CreateFieldBattleEvent(attackerParty, defenderParty);
				break;
			case MapEvent.BattleTypes.Raid:
				mapEventComponent = RaidEventComponent.CreateRaidEvent(attackerParty, defenderParty);
				break;
			case MapEvent.BattleTypes.Siege:
				mapEventComponent = SiegeAssaultEventComponent.CreateSiegeAssaultMapEvent(attackerParty, defenderParty);
				break;
			case MapEvent.BattleTypes.Hideout:
				mapEventComponent = HideoutEventComponent.CreateHideoutEvent(attackerParty, defenderParty, false);
				break;
			case MapEvent.BattleTypes.SallyOut:
				mapEventComponent = SiegeSallyOutEventComponent.CreateSiegeSallyOutEvent(attackerParty, defenderParty);
				break;
			case MapEvent.BattleTypes.SiegeOutside:
				mapEventComponent = SiegeOutsideEventComponent.CreateSiegeOutsideMapEvent(attackerParty, defenderParty);
				break;
			case MapEvent.BattleTypes.BlockadeBattle:
				mapEventComponent = BlockadeBattleEventComponent.CreateBlockadeBattleMapEvent(attackerParty, defenderParty, false);
				break;
			case MapEvent.BattleTypes.BlockadeSallyOutBattle:
				mapEventComponent = BlockadeBattleEventComponent.CreateBlockadeBattleMapEvent(attackerParty, defenderParty, true);
				break;
			}
			return mapEventComponent;
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00075FB8 File Offset: 0x000741B8
		public override float GetSurrenderChance(MobileParty defenderParty, MobileParty attackerParty)
		{
			float num = defenderParty.Party.CalculateCurrentStrength();
			float num2 = attackerParty.Party.CalculateCurrentStrength();
			if (num.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return 1f;
			}
			if (num2.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return 0f;
			}
			if (num >= num2)
			{
				return 0f;
			}
			float num3 = 0f;
			float num4 = 0f;
			if (defenderParty.IsVillager)
			{
				num3 = 0.23f;
				num4 = -13f;
			}
			else if (defenderParty.IsCaravan)
			{
				num3 = 0.3f;
				num4 = -10f;
			}
			else if (defenderParty.IsBandit)
			{
				if (defenderParty.IsCurrentlyAtSea)
				{
					num3 = 0.2f;
				}
				else if (defenderParty.ActualClan.StringId == "deserters")
				{
					num3 = 0.005f;
				}
				else
				{
					num3 = 0.1f;
				}
				num4 = -15f;
			}
			else
			{
				Debug.FailedAssert("Unable to calculate threshold and exponentialScalingFactor!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultEncounterModel.cs", "GetSurrenderChance", 350);
			}
			float num5 = num / num2;
			float num6 = num4 * (num5 - num3);
			float num7 = 1f - 1f / (1f + (float)Math.Exp((double)num6));
			if (!MobileParty.MainParty.IsCurrentlyAtSea && Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.Scarface))
			{
				num7 = MathF.Min(1f, num7 * (1f + DefaultPerks.Roguery.Scarface.PrimaryBonus));
			}
			return num7;
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00076118 File Offset: 0x00074318
		public override ExplainedNumber GetBribeChance(MobileParty defenderParty, MobileParty attackerParty)
		{
			float num = defenderParty.Party.CalculateCurrentStrength();
			float num2 = attackerParty.Party.CalculateCurrentStrength();
			if (num.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return new ExplainedNumber(1f, false, null);
			}
			if (num2.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return new ExplainedNumber(0f, false, null);
			}
			if (num >= num2)
			{
				return new ExplainedNumber(0f, false, null);
			}
			float num3 = 0f;
			float num4 = 0f;
			if (defenderParty.IsVillager)
			{
				num3 = 0.3f;
				num4 = -10f;
			}
			else if (defenderParty.IsCaravan)
			{
				num3 = 0.52f;
				num4 = -10f;
			}
			else if (defenderParty.IsBandit)
			{
				num3 = 0.2f;
				num4 = -15f;
			}
			else
			{
				Debug.FailedAssert("Unable to calculate threshold and exponentialScalingFactor!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultEncounterModel.cs", "GetBribeChance", 405);
			}
			float num5 = num / num2;
			float num6 = num4 * (num5 - num3);
			ExplainedNumber explainedNumber = new ExplainedNumber(1f - 1f / (1f + (float)Math.Exp((double)num6)), false, null);
			explainedNumber.LimitMax(1f, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.Scarface, BattleEnvironment.Any, Hero.MainHero.CharacterObject, true, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0007624C File Offset: 0x0007444C
		public override float GetMapEventSideRunAwayChance(MapEventSide mapEventSide)
		{
			float num = 0f;
			if (mapEventSide.MapEvent.EventType != MapEvent.BattleTypes.Siege && mapEventSide.MapEvent.EventType != MapEvent.BattleTypes.SallyOut && mapEventSide.MapEvent.EventType != MapEvent.BattleTypes.SiegeOutside && mapEventSide.MapEvent.EventType != MapEvent.BattleTypes.Raid && mapEventSide != MobileParty.MainParty.MapEventSide)
			{
				num = this.GetRunAwayChanceInternal(mapEventSide);
			}
			return num;
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x000762B0 File Offset: 0x000744B0
		private float GetRunAwayChanceInternal(MapEventSide mapEventSide)
		{
			MapEvent mapEvent = mapEventSide.MapEvent;
			float num = 0f;
			if (mapEvent.UpdateCount >= 8 && mapEventSide.LeaderParty.IsMobile && mapEventSide.GetSideMorale() <= 20f)
			{
				for (int i = 0; i < 4; i++)
				{
					BattleSideEnum battleSideEnum = mapEvent.WonRounds[mapEvent.WonRounds.Count - 1 - i];
					if (battleSideEnum == mapEventSide.MissionSide || battleSideEnum == BattleSideEnum.None)
					{
						return 0f;
					}
				}
				num = 0.2f;
				Hero leaderHero = mapEventSide.LeaderParty.LeaderHero;
				int num2 = ((leaderHero != null) ? leaderHero.GetTraitLevel(DefaultTraits.Valor) : 0);
				num -= (float)num2 * 0.05f;
			}
			return num;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0007635C File Offset: 0x0007455C
		public override void FindNonAttachedNpcPartiesWhoWillJoinPlayerEncounter(List<MobileParty> partiesToJoinPlayerSide, List<MobileParty> partiesToJoinEnemySide)
		{
			CampaignVec2 campaignVec = MobileParty.MainParty.Position;
			float num = Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius;
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				num = Campaign.Current.Models.MobilePartyAIModel.SettlementDefendingWaitingPositionRadius * 1.25f;
			}
			if (PlayerEncounter.Battle != null)
			{
				campaignVec = PlayerEncounter.Battle.Position;
				if (PlayerEncounter.Battle.IsSallyOut)
				{
					campaignVec = ((PlayerSiege.PlayerSiegeEvent != null) ? PlayerSiege.PlayerSiegeEvent : PlayerEncounter.EncounterSettlement.SiegeEvent).BesiegerCamp.LeaderParty.Position;
				}
				else if (PlayerEncounter.Battle.IsBlockade || PlayerEncounter.Battle.IsBlockadeSallyOut)
				{
					Settlement besiegedSettlement = PlayerSiege.BesiegedSettlement;
					campaignVec = ((besiegedSettlement != null) ? besiegedSettlement.PortPosition : PlayerEncounter.Battle.MapEventSettlement.PortPosition);
					num = Campaign.Current.Models.EncounterModel.NeededMaximumDistanceForEncounteringBlockade * 3f;
				}
			}
			LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(campaignVec.ToVec2(), num);
			MobileParty nearbyParty = MobileParty.FindNextLocatable(ref locatableSearchData);
			List<MobileParty> list = new List<MobileParty>();
			List<MobileParty> list2 = new List<MobileParty>();
			Func<MobileParty, bool> <>9__4;
			Func<MobileParty, bool> <>9__5;
			while (nearbyParty != null)
			{
				bool flag = (PlayerEncounter.Battle != null && (PlayerEncounter.Battle.IsBlockade || PlayerEncounter.Battle.IsBlockadeSallyOut)) || MobileParty.MainParty.IsCurrentlyAtSea;
				if (nearbyParty != MobileParty.MainParty && nearbyParty.MapEvent == null && !nearbyParty.IsInRaftState && nearbyParty.SiegeEvent == null && nearbyParty.CurrentSettlement == null && nearbyParty.AttachedTo == null)
				{
					if (nearbyParty.IsCurrentlyAtSea != flag)
					{
						MapEvent battle = PlayerEncounter.Battle;
						bool flag2;
						if (battle == null)
						{
							flag2 = false;
						}
						else
						{
							Settlement mapEventSettlement = battle.MapEventSettlement;
							bool? flag3 = ((mapEventSettlement != null) ? new bool?(mapEventSettlement.IsVillage) : null);
							bool flag4 = true;
							flag2 = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
						}
						if (!flag2)
						{
							goto IL_0388;
						}
					}
					if (nearbyParty.IsLordParty || nearbyParty.IsBandit || nearbyParty.IsPatrolParty || nearbyParty.ShouldJoinPlayerBattles)
					{
						if (PlayerEncounter.Battle != null)
						{
							bool flag5 = PlayerEncounter.Battle.CanPartyJoinBattle(nearbyParty.Party, PlayerEncounter.Battle.PlayerSide);
							bool flag6 = PlayerEncounter.Battle.CanPartyJoinBattle(nearbyParty.Party, PlayerEncounter.Battle.PlayerSide.GetOppositeSide());
							if (flag5)
							{
								list.Add(nearbyParty);
							}
							if (flag6)
							{
								list2.Add(nearbyParty);
							}
						}
						else
						{
							if (!nearbyParty.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction) && nearbyParty.MapFaction.IsAtWarWith(PlayerEncounter.EncounteredParty.MapFaction))
							{
								IEnumerable<MobileParty> enumerable = list2;
								Func<MobileParty, bool> func;
								if ((func = <>9__4) == null)
								{
									func = (<>9__4 = (MobileParty x) => x.MapFaction.IsAtWarWith(nearbyParty.MapFaction));
								}
								if (enumerable.All<MobileParty>(func))
								{
									list.Add(nearbyParty);
								}
							}
							if (nearbyParty.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction) && !nearbyParty.MapFaction.IsAtWarWith(PlayerEncounter.EncounteredParty.MapFaction))
							{
								IEnumerable<MobileParty> enumerable2 = list;
								Func<MobileParty, bool> func2;
								if ((func2 = <>9__5) == null)
								{
									func2 = (<>9__5 = (MobileParty x) => x.MapFaction.IsAtWarWith(nearbyParty.MapFaction));
								}
								if (enumerable2.All<MobileParty>(func2))
								{
									list2.Add(nearbyParty);
								}
							}
						}
					}
				}
				IL_0388:
				nearbyParty = MobileParty.FindNextLocatable(ref locatableSearchData);
			}
			if (!list2.AnyQ<MobileParty>((MobileParty t) => t.ShouldBeIgnored))
			{
				if (!partiesToJoinEnemySide.AnyQ<MobileParty>((MobileParty t) => t.ShouldBeIgnored))
				{
					goto IL_040C;
				}
			}
			Debug.Print("Ally parties wont join player encounter since there is an ignored party in enemy side", 0, Debug.DebugColor.White, 17592186044416UL);
			list.Clear();
			IL_040C:
			if (!list.AnyQ<MobileParty>((MobileParty t) => t.ShouldBeIgnored))
			{
				if (!partiesToJoinPlayerSide.AnyQ<MobileParty>((MobileParty t) => t != MobileParty.MainParty && t.ShouldBeIgnored))
				{
					goto IL_0478;
				}
			}
			Debug.Print("Enemy parties wont join player encounter since there is an ignored party in ally side", 0, Debug.DebugColor.White, 17592186044416UL);
			list2.Clear();
			IL_0478:
			partiesToJoinPlayerSide.AddRange(list.Except<MobileParty>(partiesToJoinPlayerSide));
			partiesToJoinEnemySide.AddRange(list2.Except<MobileParty>(partiesToJoinEnemySide));
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00076800 File Offset: 0x00074A00
		public override bool CanPlayerForceBanditsToJoin(out TextObject explanation)
		{
			bool perkValue = Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.PartnersInCrime);
			explanation = (perkValue ? null : new TextObject("{=MaetSSa1}You need '{PERK}' perk to make this party join you.", null));
			TextObject textObject = explanation;
			if (textObject != null)
			{
				textObject.SetTextVariable("PERK", DefaultPerks.Roguery.PartnersInCrime.Name);
			}
			return perkValue;
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x00076850 File Offset: 0x00074A50
		public override bool IsPartyUnderPlayerCommand(PartyBase party)
		{
			if (party == PartyBase.MainParty)
			{
				return true;
			}
			if (party.Side != PartyBase.MainParty.Side)
			{
				return false;
			}
			bool flag = party.Owner == Hero.MainHero;
			IFaction mapFaction = party.MapFaction;
			bool flag2 = ((mapFaction != null) ? mapFaction.Leader : null) == Hero.MainHero;
			bool flag3 = party.MobileParty != null && party.MobileParty.DefaultBehavior == AiBehavior.EscortParty && party.MobileParty.TargetParty == MobileParty.MainParty;
			bool flag4 = party.MobileParty != null && party.MobileParty.Army != null && party.MobileParty.Army.LeaderParty == MobileParty.MainParty;
			Settlement mapEventSettlement = party.MapEvent.MapEventSettlement;
			bool flag5 = mapEventSettlement != null && mapEventSettlement.OwnerClan.Leader == Hero.MainHero;
			return flag || flag2 || flag3 || flag4 || flag5;
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00076930 File Offset: 0x00074B30
		public override MBReadOnlyList<MobileParty> GetPartiesToTeleportOnMapEventFinalize(MapEvent mapEvent)
		{
			MBReadOnlyList<MapEventParty> mbreadOnlyList;
			if (mapEvent.IsPlayerMapEvent)
			{
				mbreadOnlyList = mapEvent.GetMapEventSide(mapEvent.PlayerSide.GetOppositeSide()).Parties;
			}
			else
			{
				mbreadOnlyList = mapEvent.GetMapEventSide(mapEvent.DefeatedSide).Parties;
			}
			MBList<MobileParty> mblist = new MBList<MobileParty>();
			foreach (MapEventParty mapEventParty in mbreadOnlyList)
			{
				if (mapEventParty.Party.IsMobile && mapEventParty.Party.MobileParty.IsActive && mapEventParty.Party.NumberOfHealthyMembers > 0 && !mapEventParty.Party.MobileParty.IsGarrison && (mapEventParty.Party.MobileParty.Army == null || mapEventParty.Party.MobileParty.Army.LeaderParty == mapEventParty.Party.MobileParty || mapEventParty.Party.MobileParty.AttachedTo == null))
				{
					mblist.Add(mapEventParty.Party.MobileParty);
				}
			}
			return mblist;
		}
	}
}
