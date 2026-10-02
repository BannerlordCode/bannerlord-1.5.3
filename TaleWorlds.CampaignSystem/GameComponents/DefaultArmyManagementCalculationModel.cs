using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F7 RID: 247
	public class DefaultArmyManagementCalculationModel : ArmyManagementCalculationModel
	{
		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x000694DC File Offset: 0x000676DC
		public override float AIMobilePartySizeRatioToCallToArmy
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060016C6 RID: 5830 RVA: 0x000694E3 File Offset: 0x000676E3
		public override float PlayerMobilePartySizeRatioToCallToArmy
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x060016C7 RID: 5831 RVA: 0x000694EA File Offset: 0x000676EA
		public override float MinimumNeededFoodInDaysToCallToArmy
		{
			get
			{
				return 15f;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x060016C8 RID: 5832 RVA: 0x000694F1 File Offset: 0x000676F1
		public override float MaximumDistanceToCallToArmy
		{
			get
			{
				return Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All) * 8f;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x00069504 File Offset: 0x00067704
		public override int InfluenceValuePerGold
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060016CA RID: 5834 RVA: 0x00069508 File Offset: 0x00067708
		public override int AverageCallToArmyCost
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060016CB RID: 5835 RVA: 0x0006950C File Offset: 0x0006770C
		public override int CohesionThresholdForDispersion
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060016CC RID: 5836 RVA: 0x00069510 File Offset: 0x00067710
		public override float MaximumWaitTime
		{
			get
			{
				return (float)CampaignTime.HoursInDay * 3f;
			}
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00069520 File Offset: 0x00067720
		public override float DailyBeingAtArmyInfluenceAward(MobileParty armyMemberParty)
		{
			float num = (armyMemberParty.Party.EstimatedStrength + 20f) / 200f;
			if (PartyBaseHelper.HasFeat(armyMemberParty.Party, DefaultCulturalFeats.EmpireArmyInfluenceFeat))
			{
				num += num * DefaultCulturalFeats.EmpireArmyInfluenceFeat.EffectBonus;
			}
			return num;
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00069568 File Offset: 0x00067768
		public override int CalculatePartyInfluenceCost(MobileParty armyLeaderParty, MobileParty party)
		{
			if (armyLeaderParty.LeaderHero != null && party.LeaderHero != null && armyLeaderParty.LeaderHero.Clan == party.LeaderHero.Clan)
			{
				return 0;
			}
			float num = (float)armyLeaderParty.LeaderHero.GetRelation(party.LeaderHero);
			float partySizeScore = this.GetPartySizeScore(party);
			float num2 = (float)MathF.Round(party.Party.EstimatedStrength);
			float num3 = (armyLeaderParty.IsMainParty ? Campaign.Current.Models.ArmyManagementCalculationModel.PlayerMobilePartySizeRatioToCallToArmy : Campaign.Current.Models.ArmyManagementCalculationModel.AIMobilePartySizeRatioToCallToArmy);
			float num4 = ((num < 0f) ? (1f + MathF.Sqrt(MathF.Abs(MathF.Max(-100f, num))) / 10f) : (1f - MathF.Sqrt(MathF.Abs(MathF.Min(100f, num))) / 20f));
			float num5 = 0.5f + MathF.Min(1000f, num2) / 100f;
			float num6 = 0.5f + 1f * (1f - (partySizeScore - num3) / (1f - num3));
			float num7;
			float distanceBetweenMobilePartyToMobileParty = DistanceHelper.GetDistanceBetweenMobilePartyToMobileParty(party, armyLeaderParty, party.NavigationCapability, out num7);
			float num8 = 1f + 1f * MathF.Pow(MathF.Min(Campaign.MapDiagonal * 10f, MathF.Max(1f, distanceBetweenMobilePartyToMobileParty)) / Campaign.MapDiagonal, 0.67f);
			float num9 = ((party.LeaderHero != null) ? party.LeaderHero.RandomFloat(0.75f, 1.25f) : 1f);
			float num10 = 1f;
			float num11 = 1f;
			float num12 = 1f;
			Hero leaderHero = armyLeaderParty.LeaderHero;
			if (((leaderHero != null) ? leaderHero.Clan.Kingdom : null) != null)
			{
				if (armyLeaderParty.LeaderHero.Clan.Tier >= 5 && armyLeaderParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Marshals))
				{
					num10 -= 0.1f;
				}
				if (armyLeaderParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.RoyalCommissions))
				{
					if (armyLeaderParty.LeaderHero == armyLeaderParty.LeaderHero.Clan.Kingdom.Leader)
					{
						num10 -= 0.3f;
					}
					else
					{
						num10 += 0.1f;
					}
				}
				if (party.LeaderHero != null)
				{
					if (armyLeaderParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.LordsPrivyCouncil) && party.LeaderHero.Clan.Tier <= 4)
					{
						num10 += 0.2f;
					}
					if (armyLeaderParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Senate) && party.LeaderHero.Clan.Tier <= 2)
					{
						num10 += 0.1f;
					}
				}
				if (armyLeaderParty.LeaderHero.GetPerkValue(DefaultPerks.Leadership.InspiringLeader))
				{
					num11 += DefaultPerks.Leadership.InspiringLeader.PrimaryBonus;
				}
				if (armyLeaderParty.LeaderHero.GetPerkValue(DefaultPerks.Tactics.CallToArms))
				{
					num11 += DefaultPerks.Tactics.CallToArms.SecondaryBonus;
				}
			}
			if (PartyBaseHelper.HasFeat(armyLeaderParty.Party, DefaultCulturalFeats.VlandianArmyInfluenceFeat))
			{
				num12 += DefaultCulturalFeats.VlandianArmyInfluenceFeat.EffectBonus;
			}
			if (PartyBaseHelper.HasFeat(armyLeaderParty.Party, DefaultCulturalFeats.SturgianArmyInfluenceCostFeat))
			{
				num12 += DefaultCulturalFeats.SturgianArmyInfluenceCostFeat.EffectBonus;
			}
			return (int)(0.65f * num4 * num5 * num9 * num8 * num6 * num10 * num11 * num12 * (float)this.AverageCallToArmyCost);
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x000698E8 File Offset: 0x00067AE8
		public override bool CanLordCreateArmy(MobileParty mobileParty, out MBList<MobileParty> possibleArmyMembers)
		{
			possibleArmyMembers = new MBList<MobileParty>();
			Kingdom kingdom = mobileParty.MapFaction as Kingdom;
			if (!mobileParty.IsCurrentlyAtSea && mobileParty.LeaderHero.Clan.Influence > 100f && !mobileParty.LeaderHero.Clan.IsUnderMercenaryService && (float)mobileParty.GetNumDaysForFoodToLast() > Campaign.Current.Models.MobilePartyAIModel.NeededFoodsInDaysThresholdForSiege)
			{
				if (kingdom.FactionsAtWarWith.AnyQ<IFaction>((IFaction x) => x.Fiefs.Any<Town>()) && mobileParty.PartySizeRatio > Campaign.Current.Models.ArmyManagementCalculationModel.AIMobilePartySizeRatioToCallToArmy && (mobileParty.LeaderHero.Clan.Leader == mobileParty.LeaderHero || (mobileParty.LeaderHero.Clan.Leader.PartyBelongedTo == null && mobileParty.LeaderHero.Clan.WarPartyComponents != null && mobileParty.LeaderHero.Clan.WarPartyComponents.FirstOrDefault<WarPartyComponent>() == mobileParty.WarPartyComponent)))
				{
					this.GetInfluenceBudgetWhileCreatingArmy(mobileParty);
					List<ValueTuple<MobileParty, float, int>> list = new List<ValueTuple<MobileParty, float, int>>();
					foreach (WarPartyComponent warPartyComponent in mobileParty.MapFaction.WarPartyComponents)
					{
						MobileParty mobileParty2 = warPartyComponent.MobileParty;
						Hero leaderHero = mobileParty2.LeaderHero;
						if (mobileParty2.IsLordParty && mobileParty2.Army == null && mobileParty2 != mobileParty && leaderHero != null && !mobileParty2.IsMainParty && leaderHero != leaderHero.MapFaction.Leader && mobileParty2.LeaderHero.CanJoinArmy && !mobileParty2.Ai.DoNotMakeNewDecisions)
						{
							Settlement currentSettlement = mobileParty2.CurrentSettlement;
							if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) == null && !mobileParty2.IsDisbanding && (float)mobileParty2.GetNumDaysForFoodToLast() > Campaign.Current.Models.ArmyManagementCalculationModel.MinimumNeededFoodInDaysToCallToArmy && mobileParty2.PartySizeRatio > Campaign.Current.Models.ArmyManagementCalculationModel.AIMobilePartySizeRatioToCallToArmy && leaderHero.CanLeadParty() && !mobileParty2.IsInRaftState && mobileParty2.MapEvent == null && mobileParty2.BesiegedSettlement == null)
							{
								IDisbandPartyCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IDisbandPartyCampaignBehavior>();
								if (campaignBehavior == null || !campaignBehavior.IsPartyWaitingForDisband(mobileParty2))
								{
									float maximumDistanceToCallToArmy = Campaign.Current.Models.ArmyManagementCalculationModel.MaximumDistanceToCallToArmy;
									float num;
									if (DistanceHelper.GetDistanceBetweenMobilePartyToMobileParty(mobileParty2, mobileParty, mobileParty2.NavigationCapability, out num) < maximumDistanceToCallToArmy)
									{
										bool flag = false;
										using (List<ValueTuple<MobileParty, float, int>>.Enumerator enumerator2 = list.GetEnumerator())
										{
											while (enumerator2.MoveNext())
											{
												if (enumerator2.Current.Item1 == mobileParty2)
												{
													flag = true;
													break;
												}
											}
										}
										if (!flag)
										{
											int num2 = Campaign.Current.Models.ArmyManagementCalculationModel.CalculatePartyInfluenceCost(mobileParty, mobileParty2);
											float estimatedStrength = mobileParty2.Party.EstimatedStrength;
											float num3 = 1f - (float)mobileParty2.Party.MemberRoster.TotalWounded / (float)mobileParty2.Party.MemberRoster.TotalManCount;
											float num4 = estimatedStrength / ((float)num2 + 0.1f) * num3;
											list.Add(new ValueTuple<MobileParty, float, int>(mobileParty2, num4, num2));
										}
									}
								}
							}
						}
					}
					list = list.OrderByQ<ValueTuple<MobileParty, float, int>, float>((ValueTuple<MobileParty, float, int> x) => x.Item2).ToListQ<ValueTuple<MobileParty, float, int>>();
					int count = kingdom.WarPartyComponents.Count;
					int num5 = kingdom.Armies.SumQ<Army>((Army x) => x.Parties.Count);
					int num6 = MathF.Ceiling((float)count * 0.7f - (float)num5);
					if (num6 > 0)
					{
						if (num6 < list.Count)
						{
							list.RemoveRange(num6, list.Count - num6);
						}
						possibleArmyMembers = list.SelectQ<ValueTuple<MobileParty, float, int>, MobileParty>((ValueTuple<MobileParty, float, int> x) => x.Item1).ToMBList<MobileParty>();
						if (possibleArmyMembers.AnyQ<MobileParty>())
						{
							if (kingdom.Settlements.Count == 0)
							{
								return true;
							}
							float num7 = mobileParty.Party.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.Siege);
							foreach (MobileParty mobileParty3 in possibleArmyMembers)
							{
								num7 += mobileParty3.Party.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.Siege);
							}
							if (num7 < 1000f)
							{
								possibleArmyMembers.Clear();
								return false;
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00069DF8 File Offset: 0x00067FF8
		public override int CalculateTotalInfluenceCost(Army army, float percentage)
		{
			int num = this.CalculateTotalInfluenceCostInternal(army, percentage);
			if (army != MobileParty.MainParty.Army)
			{
				num = (int)((float)num * 0.25f);
			}
			return num;
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00069E28 File Offset: 0x00068028
		private int CalculateTotalInfluenceCostInternal(Army army, float percentage)
		{
			int num = 0;
			foreach (MobileParty mobileParty in army.Parties.Where<MobileParty>((MobileParty p) => !p.IsMainParty))
			{
				num += this.CalculatePartyInfluenceCost(army.LeaderParty, mobileParty);
			}
			ExplainedNumber explainedNumber = new ExplainedNumber((float)num, false, null);
			if (army.LeaderParty.MapFaction.IsKingdomFaction && ((Kingdom)army.LeaderParty.MapFaction).ActivePolicies.Contains(DefaultPolicies.RoyalCommissions))
			{
				explainedNumber.AddFactor(-0.3f, DefaultPolicies.RoyalCommissions.Name);
			}
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.Encirclement, army.LeaderParty, false, ref explainedNumber);
			return MathF.Ceiling(explainedNumber.ResultNumber * percentage / 100f);
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00069F20 File Offset: 0x00068120
		public override float GetPartySizeScore(MobileParty party)
		{
			return MathF.Min(1f, party.PartySizeRatio);
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00069F34 File Offset: 0x00068134
		public override ExplainedNumber CalculateDailyCohesionChange(Army army, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(-2f, includeDescriptions, null);
			this.CalculateCohesionChangeInternal(army, ref explainedNumber);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.HordeLeader, army.LeaderParty, false, ref explainedNumber);
			SiegeEvent siegeEvent = army.LeaderParty.SiegeEvent;
			if (siegeEvent != null && siegeEvent.BesiegerCamp.IsBesiegerSideParty(army.LeaderParty))
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.CampBuilding, army.LeaderParty, true, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00069FA4 File Offset: 0x000681A4
		private void CalculateCohesionChangeInternal(Army army, ref ExplainedNumber cohesionChange)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (MobileParty mobileParty in army.LeaderParty.AttachedParties)
			{
				if (mobileParty.Party.IsStarving)
				{
					num++;
				}
				if (mobileParty.Morale <= 25f)
				{
					num2++;
				}
				if (mobileParty.Party.NumberOfHealthyMembers <= 10)
				{
					num3++;
				}
				num4++;
			}
			float num5 = (float)(-(float)num4);
			float num6 = -((float)(num + 1) / 2f);
			float num7 = -((float)(num2 + 1) / 2f);
			float num8 = -((float)(num3 + 1) / 2f);
			if (army.LeaderParty != MobileParty.MainParty)
			{
				num5 *= 0.25f;
				num6 *= 0.25f;
				num7 *= 0.25f;
				num8 *= 0.25f;
			}
			cohesionChange.Add(num5, this._numberOfPartiesText, null);
			cohesionChange.Add(num6, this._numberOfStarvingPartiesText, null);
			cohesionChange.Add(num7, this._numberOfLowMoralePartiesText, null);
			cohesionChange.Add(num8, this._numberOfLessMemberPartiesText, null);
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x0006A0D4 File Offset: 0x000682D4
		public override int CalculateNewCohesion(Army army, PartyBase newParty, int calculatedCohesion, int sign)
		{
			if (army == null)
			{
				return calculatedCohesion;
			}
			sign = MathF.Sign(sign);
			int num = ((sign == 1) ? (army.Parties.Count - 1) : army.Parties.Count);
			int num2 = (calculatedCohesion * num + 100 * sign) / (num + sign);
			if (num2 > 100)
			{
				return 100;
			}
			if (num2 >= 0)
			{
				return num2;
			}
			return 0;
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x0006A12D File Offset: 0x0006832D
		public override int GetCohesionBoostInfluenceCost(Army army, int percentageToBoost = 100)
		{
			return this.CalculateTotalInfluenceCostInternal(army, (float)percentageToBoost);
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x0006A138 File Offset: 0x00068338
		private float GetInfluenceBudgetWhileCreatingArmy(MobileParty mobileParty)
		{
			return mobileParty.LeaderHero.Clan.Influence * 0.7f;
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x0006A150 File Offset: 0x00068350
		public override int GetPartyRelation(Hero hero)
		{
			if (hero == null)
			{
				return -101;
			}
			if (hero == Hero.MainHero)
			{
				return 101;
			}
			return Hero.MainHero.GetRelation(hero);
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x0006A170 File Offset: 0x00068370
		public override bool CanPlayerCreateArmy(out TextObject disabledReason)
		{
			if (Clan.PlayerClan.Kingdom == null)
			{
				disabledReason = new TextObject("{=XSQ0Y9gy}You need to be a part of a kingdom to create an army.", null);
				return false;
			}
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				disabledReason = new TextObject("{=aRhQzJca}Mercenaries cannot create or manage armies.", null);
				return false;
			}
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				disabledReason = new TextObject("{=NAA4pajB}You need to leave your current army to create a new one.", null);
				return false;
			}
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				disabledReason = GameTexts.FindText("str_cannot_gather_army_at_sea", null);
				return false;
			}
			if (Hero.MainHero.IsPrisoner)
			{
				disabledReason = GameTexts.FindText("str_action_disabled_reason_prisoner", null);
				return false;
			}
			if (MobileParty.MainParty.IsInRaftState)
			{
				disabledReason = GameTexts.FindText("str_action_disabled_reason_raft_state", null);
				return false;
			}
			if (MobileParty.MainParty.IsInFerryState)
			{
				disabledReason = GameTexts.FindText("str_action_disabled_reason_ferry_state", null);
				return false;
			}
			if (CampaignMission.Current != null)
			{
				disabledReason = new TextObject("{=FdzsOvDq}This action is disabled while in a mission", null);
				return false;
			}
			if (PlayerEncounter.Current != null)
			{
				if (PlayerEncounter.EncounterSettlement == null)
				{
					disabledReason = GameTexts.FindText("str_action_disabled_reason_encounter", null);
					return false;
				}
				Village village = PlayerEncounter.EncounterSettlement.Village;
				if (village != null && village.VillageState == Village.VillageStates.BeingRaided)
				{
					MapEvent mapEvent = MobileParty.MainParty.MapEvent;
					if (mapEvent != null && mapEvent.IsRaid)
					{
						disabledReason = GameTexts.FindText("str_action_disabled_reason_raid", null);
						return false;
					}
				}
				if (PlayerEncounter.EncounterSettlement.IsUnderSiege)
				{
					disabledReason = GameTexts.FindText("str_action_disabled_reason_siege", null);
					return false;
				}
			}
			else
			{
				if (PlayerSiege.PlayerSiegeEvent != null)
				{
					disabledReason = GameTexts.FindText("str_action_disabled_reason_siege", null);
					return false;
				}
				if (MobileParty.MainParty.MapEvent != null)
				{
					disabledReason = new TextObject("{=MIylzRc5}You can't perform this action while you are in a map event.", null);
					return false;
				}
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x0006A31C File Offset: 0x0006851C
		public override bool CheckPartyEligibility(MobileParty party, out TextObject explanation)
		{
			bool flag = true;
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				flag = false;
				explanation = GameTexts.FindText("str_action_disabled_reason_siege", null);
			}
			else if (party == null)
			{
				flag = false;
				explanation = new TextObject("{=f6vTzVar}Does not have a mobile party.", null);
			}
			else
			{
				Hero leaderHero = party.LeaderHero;
				IFaction mapFaction = Hero.MainHero.MapFaction;
				if (leaderHero == ((mapFaction != null) ? mapFaction.Leader : null))
				{
					flag = false;
					explanation = new TextObject("{=ipLqVv1f}You cannot invite the ruler's party to your army.", null);
				}
				else
				{
					if (party.Army != null)
					{
						Army army = party.Army;
						MobileParty partyBelongedTo = Hero.MainHero.PartyBelongedTo;
						if (army != ((partyBelongedTo != null) ? partyBelongedTo.Army : null))
						{
							flag = false;
							explanation = new TextObject("{=aROohsat}Already in another army.", null);
							return flag;
						}
					}
					if (party.Army != null)
					{
						Army army2 = party.Army;
						MobileParty partyBelongedTo2 = Hero.MainHero.PartyBelongedTo;
						if (army2 == ((partyBelongedTo2 != null) ? partyBelongedTo2.Army : null))
						{
							flag = false;
							explanation = new TextObject("{=Vq8yavES}Already in army.", null);
							return flag;
						}
					}
					if (party.MapEvent != null || party.SiegeEvent != null || (party.CurrentSettlement != null && party.CurrentSettlement.IsUnderSiege))
					{
						flag = false;
						explanation = new TextObject("{=pkbUiKFJ}Currently fighting an enemy.", null);
					}
					else if (this.GetPartySizeScore(party) <= Campaign.Current.Models.ArmyManagementCalculationModel.PlayerMobilePartySizeRatioToCallToArmy)
					{
						flag = false;
						explanation = new TextObject("{=SVJlOYCB}Party has less men than 40% of it's party size limit.", null);
					}
					else
					{
						if (!party.IsDisbanding)
						{
							IDisbandPartyCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IDisbandPartyCampaignBehavior>();
							if (campaignBehavior == null || !campaignBehavior.IsPartyWaitingForDisband(party))
							{
								if (MobileParty.MainParty.IsCurrentlyAtSea)
								{
									flag = false;
									explanation = ((!party.HasNavalNavigationCapability) ? new TextObject("{=nqq84Dzq}Party cannot reach your army since it has no ships.", null) : new TextObject("{=gFixGQsr}You cannot call a party to your army while your party is at sea.", null));
									return flag;
								}
								if (party.IsInRaftState)
								{
									flag = false;
									explanation = new TextObject("{=TbXDmh3t}This party is lost at sea.", null);
									return flag;
								}
								float num;
								if (DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(party, MobileParty.MainParty, party.NavigationCapability, out num) > Campaign.Current.Models.ArmyManagementCalculationModel.MaximumDistanceToCallToArmy)
								{
									flag = false;
									explanation = new TextObject("{=UINgZDN5}You can not call a party that is far away.", null);
									return flag;
								}
								explanation = null;
								return flag;
							}
						}
						flag = false;
						explanation = new TextObject("{=tFGM0yav}This party is disbanding.", null);
					}
				}
			}
			return flag;
		}

		// Token: 0x0400079D RID: 1949
		private const float MinimumInfluenceNeededToCreateArmy = 100f;

		// Token: 0x0400079E RID: 1950
		private readonly TextObject _numberOfPartiesText = GameTexts.FindText("str_number_of_parties", null);

		// Token: 0x0400079F RID: 1951
		private readonly TextObject _numberOfStarvingPartiesText = GameTexts.FindText("str_number_of_starving_parties", null);

		// Token: 0x040007A0 RID: 1952
		private readonly TextObject _numberOfLowMoralePartiesText = GameTexts.FindText("str_number_of_low_morale_parties", null);

		// Token: 0x040007A1 RID: 1953
		private readonly TextObject _numberOfLessMemberPartiesText = GameTexts.FindText("str_number_of_less_member_parties", null);
	}
}
