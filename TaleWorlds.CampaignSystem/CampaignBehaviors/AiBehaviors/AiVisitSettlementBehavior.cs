using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x02000496 RID: 1174
	public class AiVisitSettlementBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004B5F RID: 19295 RVA: 0x0017E9AC File Offset: 0x0017CBAC
		private static float GetMaximumDistanceAsDays(MobileParty.NavigationType navigationType)
		{
			return Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(navigationType) * 4f / (Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay);
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x0017E9D1 File Offset: 0x0017CBD1
		private float MaximumMeaningfulDistanceAsDays(MobileParty.NavigationType navigationType)
		{
			return AiVisitSettlementBehavior.GetMaximumDistanceAsDays(navigationType) * 0.7f;
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06004B61 RID: 19297 RVA: 0x0017E9DF File Offset: 0x0017CBDF
		private static float SearchForNeutralSettlementRadiusAsDays
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06004B62 RID: 19298 RVA: 0x0017E9E6 File Offset: 0x0017CBE6
		private float NumberOfHoursAtDay
		{
			get
			{
				return (float)Campaign.Current.Models.CampaignTimeModel.HoursInDay;
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06004B63 RID: 19299 RVA: 0x0017E9FD File Offset: 0x0017CBFD
		private float IdealTimePeriodForVisitingOwnedSettlement
		{
			get
			{
				return (float)Campaign.Current.Models.CampaignTimeModel.HoursInDay * 15f;
			}
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x0017EA1C File Offset: 0x0017CC1C
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x0017EA9C File Offset: 0x0017CC9C
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this._disbandPartyCampaignBehavior = Campaign.Current.GetCampaignBehavior<IDisbandPartyCampaignBehavior>();
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x0017EAAE File Offset: 0x0017CCAE
		private void OnNewGameCreated(CampaignGameStarter obj)
		{
			this.RefreshTheTargetingSettlementDictionary();
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x0017EAB6 File Offset: 0x0017CCB6
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.RefreshTheTargetingSettlementDictionary();
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x0017EABE File Offset: 0x0017CCBE
		private void OnHourlyTick()
		{
			this.RefreshTheTargetingSettlementDictionary();
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x0017EAC8 File Offset: 0x0017CCC8
		private void RefreshTheTargetingSettlementDictionary()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsFortification || settlement.IsVillage)
				{
					this._numberOfAlliedMobilePartiesTargetingSettlement[settlement] = 0;
				}
			}
			foreach (MobileParty mobileParty in MobileParty.AllLordParties)
			{
				if ((mobileParty.Army == null || mobileParty.AttachedTo == null || mobileParty.Army.LeaderParty == mobileParty) && mobileParty.TargetSettlement != null && mobileParty.CurrentSettlement != mobileParty.TargetSettlement && mobileParty.TargetSettlement.MapFaction == mobileParty.MapFaction)
				{
					Army army = mobileParty.Army;
					int num = ((army != null) ? army.LeaderPartyAndAttachedPartiesCount : 1);
					Dictionary<Settlement, int> numberOfAlliedMobilePartiesTargetingSettlement = this._numberOfAlliedMobilePartiesTargetingSettlement;
					Settlement targetSettlement = mobileParty.TargetSettlement;
					numberOfAlliedMobilePartiesTargetingSettlement[targetSettlement] += num;
				}
			}
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x0017EBF0 File Offset: 0x0017CDF0
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x0017EBF4 File Offset: 0x0017CDF4
		private void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			Settlement currentSettlement = mobileParty.CurrentSettlement;
			if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) != null)
			{
				return;
			}
			Settlement currentSettlementOfMobilePartyForAICalculation = MobilePartyHelper.GetCurrentSettlementOfMobilePartyForAICalculation(mobileParty);
			if (mobileParty.IsBandit)
			{
				this.CalculateVisitHideoutScoresForBanditParty(mobileParty, currentSettlementOfMobilePartyForAICalculation, p);
				return;
			}
			IFaction mapFaction = mobileParty.MapFaction;
			if (mobileParty.IsMilitia || mobileParty.IsCaravan || mobileParty.IsPatrolParty || mobileParty.IsVillager || (!mapFaction.IsMinorFaction && !mapFaction.IsKingdomFaction && (mobileParty.LeaderHero == null || !mobileParty.LeaderHero.IsLord)))
			{
				return;
			}
			if (mobileParty.Army == null || mobileParty.AttachedTo == null || mobileParty.Army.LeaderParty == mobileParty)
			{
				Hero leaderHero = mobileParty.LeaderHero;
				ValueTuple<float, float, int, int> valueTuple = this.CalculatePartyParameters(mobileParty);
				float item = valueTuple.Item1;
				float item2 = valueTuple.Item2;
				int item3 = valueTuple.Item3;
				int item4 = valueTuple.Item4;
				float num = item2 / Math.Min(1f, Math.Max(0.1f, item));
				float num2 = ((num >= 1f) ? 0.33f : ((MathF.Max(1f, MathF.Min(2f, num)) - 0.5f) / 1.5f));
				float num3 = mobileParty.Food;
				float num4 = -mobileParty.FoodChange;
				int num5 = mobileParty.PartyTradeGold;
				if (mobileParty.Army != null && mobileParty == mobileParty.Army.LeaderParty)
				{
					foreach (MobileParty mobileParty2 in mobileParty.Army.LeaderParty.AttachedParties)
					{
						num3 += mobileParty2.Food;
						num4 += -mobileParty2.FoodChange;
						num5 += mobileParty2.PartyTradeGold;
					}
				}
				float num6 = 1f;
				if (leaderHero != null && mobileParty.IsLordParty)
				{
					num6 = this.CalculateSellItemScore(mobileParty);
				}
				int num7 = mobileParty.Party.PrisonerSizeLimit;
				if (mobileParty.Army != null)
				{
					foreach (MobileParty mobileParty3 in mobileParty.Army.LeaderParty.AttachedParties)
					{
						num7 += mobileParty3.Party.PrisonerSizeLimit;
					}
				}
				this._settlementsNavigationData.Clear();
				AiVisitSettlementBehavior.FillSettlementsToVisitWithDistancesAsDays(mobileParty, this._settlementsNavigationData);
				float num8 = PartyBaseHelper.FindPartySizeNormalLimit(mobileParty);
				float num9 = 2000f;
				float num10 = 2000f;
				if (leaderHero != null)
				{
					num9 = HeroHelper.StartRecruitingMoneyLimitForClanLeader(leaderHero);
					num10 = HeroHelper.StartRecruitingMoneyLimit(leaderHero);
				}
				float num11 = 0.2f;
				float num12 = 1f;
				this._settlementsNavigationData.Sort();
				foreach (AiVisitSettlementBehavior.SettlementNavigationData settlementNavigationData in this._settlementsNavigationData)
				{
					Settlement settlement = settlementNavigationData.Settlement;
					MobileParty.NavigationType bestNavigationType = settlementNavigationData.BestNavigationType;
					float distance = settlementNavigationData.Distance;
					bool isFromPort = settlementNavigationData.IsFromPort;
					bool isTargetingPortBetter = settlementNavigationData.IsTargetingPortBetter;
					float num13 = 1.6f;
					if (mobileParty.IsDisbanding)
					{
						goto IL_02DE;
					}
					IDisbandPartyCampaignBehavior disbandPartyCampaignBehavior = this._disbandPartyCampaignBehavior;
					if (disbandPartyCampaignBehavior != null && disbandPartyCampaignBehavior.IsPartyWaitingForDisband(mobileParty))
					{
						goto IL_02DE;
					}
					if (leaderHero == null)
					{
						bool flag;
						float num14 = this.CalculateMergeScoreForLeaderlessParty(mobileParty, settlement, distance, out flag);
						if (flag)
						{
							this.AddBehaviorTupleWithScore(p, settlement, num14, bestNavigationType, isFromPort, isTargetingPortBetter);
						}
					}
					else
					{
						if (distance >= this.MaximumMeaningfulDistanceAsDays(bestNavigationType))
						{
							this.AddBehaviorTupleWithScore(p, settlement, 0.025f, bestNavigationType, isFromPort, isTargetingPortBetter);
							continue;
						}
						float num15 = MathF.Max(num11, distance);
						float num16 = 1f;
						if (distance > num11)
						{
							num16 = num12 / (num12 - num11 + distance);
						}
						float num17 = num16;
						if (item < 0.6f)
						{
							num17 = MathF.Pow(num16, MathF.Pow(0.6f / MathF.Max(0.15f, item), 0.3f));
						}
						float num18 = 1f;
						float num19 = (float)item3 / (float)item4;
						bool flag2 = mobileParty.Army != null && mobileParty.AttachedTo == null && mobileParty.Army.LeaderParty != mobileParty;
						if (settlement.IsFortification && num19 > 0.2f)
						{
							num18 = MBMath.Map(num19 - 0.2f, 0f, 0.8f, 1f, 5f);
							if (flag2 || mobileParty.MapEvent != null || mobileParty.SiegeEvent != null)
							{
								num18 *= 0.6f;
							}
						}
						float num20 = 1f;
						if (mobileParty.DefaultBehavior == AiBehavior.GoToSettlement && ((settlement == currentSettlementOfMobilePartyForAICalculation && currentSettlementOfMobilePartyForAICalculation.IsFortification) || (currentSettlementOfMobilePartyForAICalculation == null && settlement == mobileParty.TargetSettlement)))
						{
							num20 = 1.2f;
						}
						else if (currentSettlementOfMobilePartyForAICalculation == null && settlement == mobileParty.LastVisitedSettlement)
						{
							num20 = 0.8f;
						}
						float num21 = ((num19 > 0.2f) ? 1f : 0.16f);
						float num22 = Math.Max(0f, num3) / num4;
						if (num4 > 0f && (mobileParty.BesiegedSettlement == null || num22 <= 1f) && num5 > 100 && (settlement.IsTown || (settlement.IsVillage && mobileParty.Army == null)))
						{
							float neededFoodsInDaysThresholdForSiege = Campaign.Current.Models.MobilePartyAIModel.NeededFoodsInDaysThresholdForSiege;
							if (num22 < neededFoodsInDaysThresholdForSiege)
							{
								float num23 = (float)((int)(num4 * ((num22 < 1f && settlement.IsVillage) ? Campaign.Current.Models.PartyFoodBuyingModel.MinimumDaysFoodToLastWhileBuyingFoodFromVillage : Campaign.Current.Models.PartyFoodBuyingModel.MinimumDaysFoodToLastWhileBuyingFoodFromTown)) + 1);
								float num24 = neededFoodsInDaysThresholdForSiege * 0.5f;
								float num25 = num24 - Math.Min(num24, Math.Max(0f, num22 - 1f));
								float num26 = num23 + 20f * (float)(settlement.IsTown ? 2 : 1) * ((num15 > num12) ? 1f : (num15 / num12));
								int num27 = (int)((float)(num5 - 100) / Campaign.Current.Models.PartyFoodBuyingModel.LowCostFoodPriceAverage);
								num21 += num25 * num25 * 0.093f * ((num22 < num24) ? (15f + 0.5f * (num24 - num22)) : 1f) * Math.Min(num26, (float)Math.Min(num27, settlement.ItemRoster.TotalFood)) / num26;
							}
						}
						float num28 = 0f;
						int num29 = 0;
						float num30 = 1f;
						if (!settlement.IsCastle && item < 1f && mobileParty.GetAvailableWageBudget() > 0)
						{
							int num31 = this._numberOfAlliedMobilePartiesTargetingSettlement[settlement];
							if (currentSettlementOfMobilePartyForAICalculation == settlement)
							{
								int num32 = num29;
								Army army = mobileParty.Army;
								num29 = num32 - ((army != null) ? army.LeaderPartyAndAttachedPartiesCount : 1);
								if (num29 < 0)
								{
									num29 = 0;
								}
							}
							if (mobileParty.TargetSettlement == settlement || (mobileParty.Army != null && mobileParty.Army.LeaderParty.TargetSettlement == settlement))
							{
								int num33 = num31;
								Army army2 = mobileParty.Army;
								num31 = num33 - ((army2 != null) ? army2.LeaderPartyAndAttachedPartiesCount : 1);
								if (num31 < 0)
								{
									num31 = 0;
								}
							}
							if (mobileParty.Army != null)
							{
								num31 += mobileParty.Army.LeaderPartyAndAttachedPartiesCount;
							}
							if (!mobileParty.Party.IsStarving && (float)mobileParty.PartyTradeGold > num10 && (leaderHero.Clan.Leader == leaderHero || (float)leaderHero.Clan.Gold > num9) && num8 > mobileParty.PartySizeRatio)
							{
								ValueTuple<int, float> approximateVolunteersCanBeRecruitedDataFromSettlement = this.GetApproximateVolunteersCanBeRecruitedDataFromSettlement(leaderHero, settlement);
								num28 = (float)approximateVolunteersCanBeRecruitedDataFromSettlement.Item1;
								if (num28 > 0f)
								{
									float item5 = approximateVolunteersCanBeRecruitedDataFromSettlement.Item2;
									num28 = Math.Min(num28, (float)MathF.Floor((float)mobileParty.GetAvailableWageBudget() / item5));
								}
							}
							float num34 = num28 * num16 / MathF.Sqrt((float)(1 + num29 + num31));
							float num35 = ((num34 < 1f) ? num34 : ((float)Math.Pow((double)num34, (double)num2)));
							num30 = Math.Max(Math.Min(1f, num21), Math.Max((mapFaction == settlement.MapFaction) ? 0.25f : 0.16f, num * Math.Max(1f, Math.Min(2f, num)) * num35 * (1f - 0.9f * num19) * (1f - 0.9f * num19)));
						}
						num13 *= num30 * num18 * num21 * num17;
						if (num13 >= 8f)
						{
							this.AddBehaviorTupleWithScore(p, settlement, num13, bestNavigationType, isFromPort, isTargetingPortBetter);
							break;
						}
						float num36 = 1f;
						if (num28 > 0f && !flag2)
						{
							num36 = 1f + ((mobileParty.DefaultBehavior == AiBehavior.GoToSettlement && settlement != currentSettlementOfMobilePartyForAICalculation && num15 < num11) ? (0.1f * MathF.Min(5f, num28) - 0.1f * MathF.Min(5f, num28) * (num15 / num11) * (num15 / num11)) : 0f);
						}
						float num37 = ((settlement.IsCastle && !flag2 && num21 < 1f) ? 1.4f : 1f);
						num13 *= (settlement.IsTown ? num6 : 1f) * num36 * num37;
						if (num13 >= 8f)
						{
							this.AddBehaviorTupleWithScore(p, settlement, num13, bestNavigationType, isFromPort, isTargetingPortBetter);
							break;
						}
						int num38 = mobileParty.PrisonRoster.TotalRegulars;
						if (mobileParty.PrisonRoster.TotalHeroes > 0)
						{
							foreach (TroopRosterElement troopRosterElement in mobileParty.PrisonRoster.GetTroopRoster())
							{
								if (troopRosterElement.Character.IsHero && troopRosterElement.Character.HeroObject.Clan.IsAtWarWith(settlement.MapFaction))
								{
									num38 += 6;
								}
							}
						}
						float num39 = 1f;
						float num40 = 1f;
						if (mobileParty.Army != null && mobileParty.Army.LeaderParty.AttachedParties.Contains(mobileParty))
						{
							if (mobileParty.Army.LeaderParty != mobileParty)
							{
								num39 = ((float)mobileParty.Army.CohesionThresholdForDispersion - mobileParty.Army.Cohesion) / (float)mobileParty.Army.CohesionThresholdForDispersion;
							}
							num40 = ((MobileParty.MainParty != null && mobileParty.Army == MobileParty.MainParty.Army) ? 0.6f : 0.8f);
							foreach (MobileParty mobileParty4 in mobileParty.Army.LeaderParty.AttachedParties)
							{
								num38 += mobileParty4.PrisonRoster.TotalRegulars;
								if (mobileParty4.PrisonRoster.TotalHeroes > 0)
								{
									foreach (TroopRosterElement troopRosterElement2 in mobileParty4.PrisonRoster.GetTroopRoster())
									{
										if (troopRosterElement2.Character.IsHero && troopRosterElement2.Character.HeroObject.Clan.IsAtWarWith(settlement.MapFaction))
										{
											num38 += 6;
										}
									}
								}
							}
						}
						float num41 = (settlement.IsFortification ? (1f + 2f * (float)(num38 / num7)) : 1f);
						float num42 = ((mobileParty.DesiredAiNavigationType == bestNavigationType) ? 1.5f : 1f);
						float num43 = 1f;
						float num44 = 1f;
						float num45 = 1f;
						float num46 = 1f;
						float num47 = 1f;
						if (num21 <= 0.5f)
						{
							ValueTuple<float, float, float, float> valueTuple2 = this.CalculateBeingSettlementOwnerScores(mobileParty, settlement, currentSettlementOfMobilePartyForAICalculation, -1f, num16, item);
							num43 = valueTuple2.Item1;
							num44 = valueTuple2.Item2;
							num45 = valueTuple2.Item3;
							num46 = valueTuple2.Item4;
						}
						float num48 = 1f;
						if (settlement.HasPort && mobileParty.Ships.Any<Ship>())
						{
							float num49 = mobileParty.Ships.AverageQ<Ship>((Ship x) => x.HitPoints / x.MaxHitPoints);
							if (num49 < 0.8f)
							{
								if (num49 > 0.6f)
								{
									num48 = 1.5f;
								}
								else if (num49 > 0.4f)
								{
									num48 = 1.75f;
								}
								else
								{
									num48 = 3f;
								}
							}
						}
						num13 *= num47 * num20 * num39 * num41 * num40 * num43 * num45 * num44 * num46 * num42 * num48;
					}
					IL_0BFB:
					if (num13 > 0.025f)
					{
						this.AddBehaviorTupleWithScore(p, settlement, num13, bestNavigationType, isFromPort, isTargetingPortBetter);
						continue;
					}
					continue;
					IL_02DE:
					float num50 = this.CalculateMergeScoreForDisbandingParty(mobileParty, settlement, distance);
					this.AddBehaviorTupleWithScore(p, settlement, num50, bestNavigationType, isFromPort, isTargetingPortBetter);
					goto IL_0BFB;
				}
			}
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x0017F8C8 File Offset: 0x0017DAC8
		private ValueTuple<int, float> GetApproximateVolunteersCanBeRecruitedDataFromSettlement(Hero hero, Settlement settlement)
		{
			int num = 4;
			if (hero.MapFaction != settlement.MapFaction)
			{
				num = 2;
			}
			int num2 = 0;
			int num3 = 0;
			foreach (Hero hero2 in settlement.Notables)
			{
				if (hero2.IsAlive)
				{
					for (int i = 0; i < num; i++)
					{
						if (hero2.VolunteerTypes[i] != null)
						{
							num2++;
							num3 += Campaign.Current.Models.PartyWageModel.GetCharacterWage(hero2.VolunteerTypes[i]);
						}
					}
				}
			}
			if (num2 > 0)
			{
				num3 /= num2;
			}
			return new ValueTuple<int, float>(num2, (float)num3);
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x0017F988 File Offset: 0x0017DB88
		private float CalculateSellItemScore(MobileParty mobileParty)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < mobileParty.ItemRoster.Count; i++)
			{
				ItemRosterElement itemRosterElement = mobileParty.ItemRoster[i];
				if (itemRosterElement.EquipmentElement.Item.IsMountable)
				{
					num2 += (float)(itemRosterElement.Amount * itemRosterElement.EquipmentElement.Item.Value);
				}
				else if (!itemRosterElement.EquipmentElement.Item.IsFood)
				{
					num += (float)(itemRosterElement.Amount * itemRosterElement.EquipmentElement.Item.Value);
				}
			}
			float num3 = ((num2 > (float)mobileParty.PartyTradeGold * 0.1f) ? MathF.Min(3f, MathF.Pow((num2 + 1000f) / ((float)mobileParty.PartyTradeGold * 0.1f + 1000f), 0.33f)) : 1f);
			float num4 = 1f + MathF.Min(3f, MathF.Pow(num / (((float)mobileParty.MemberRoster.TotalManCount + 5f) * 100f), 0.33f));
			float num5 = num3 * num4;
			if (mobileParty.Army != null)
			{
				num5 = MathF.Sqrt(num5);
			}
			return num5;
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x0017FAD0 File Offset: 0x0017DCD0
		private ValueTuple<float, float, int, int> CalculatePartyParameters(MobileParty mobileParty)
		{
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			float num6;
			if (mobileParty.Army != null && (mobileParty.AttachedTo != null || mobileParty.Army.LeaderParty == mobileParty))
			{
				float num4 = 0f;
				foreach (MobileParty mobileParty2 in mobileParty.AttachedParties)
				{
					float partySizeRatio = mobileParty2.PartySizeRatio;
					num4 += partySizeRatio;
					num2 += mobileParty2.MemberRoster.TotalWounded;
					num3 += mobileParty2.MemberRoster.TotalManCount;
					float num5 = PartyBaseHelper.FindPartySizeNormalLimit(mobileParty2);
					num += num5;
				}
				num6 = num4 / (float)mobileParty.Army.Parties.Count;
				num /= (float)mobileParty.Army.Parties.Count;
			}
			else
			{
				num6 = mobileParty.PartySizeRatio;
				num2 += mobileParty.MemberRoster.TotalWounded;
				num3 += mobileParty.MemberRoster.TotalManCount;
				num += PartyBaseHelper.FindPartySizeNormalLimit(mobileParty);
			}
			return new ValueTuple<float, float, int, int>(num6, num, num2, num3);
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x0017FBF0 File Offset: 0x0017DDF0
		private void CalculateVisitHideoutScoresForBanditParty(MobileParty mobileParty, Settlement currentSettlement, PartyThinkParams p)
		{
			if (!mobileParty.MapFaction.Culture.CanHaveSettlement)
			{
				return;
			}
			if (currentSettlement != null && currentSettlement.IsHideout)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < mobileParty.ItemRoster.Count; i++)
			{
				ItemRosterElement itemRosterElement = mobileParty.ItemRoster[i];
				num += itemRosterElement.Amount * itemRosterElement.EquipmentElement.Item.Value;
			}
			float num2 = 1f + 4f * Math.Min((float)num, 1000f) / 1000f;
			int num3 = 0;
			MBReadOnlyList<Hideout> allHideouts = Campaign.Current.AllHideouts;
			foreach (Hideout hideout in allHideouts)
			{
				if (hideout.Settlement.Culture == mobileParty.Party.Culture && hideout.IsInfested)
				{
					num3++;
				}
			}
			float num4 = 1f + 4f * (float)Math.Sqrt((double)(mobileParty.PrisonRoster.TotalManCount / mobileParty.Party.PrisonerSizeLimit));
			int numberOfMinimumBanditPartiesInAHideoutToInfestIt = Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;
			int numberOfMaximumBanditPartiesInEachHideout = Campaign.Current.Models.BanditDensityModel.NumberOfMaximumBanditPartiesInEachHideout;
			int numberOfMaximumHideoutsAtEachBanditFaction = Campaign.Current.Models.BanditDensityModel.NumberOfMaximumHideoutsAtEachBanditFaction;
			foreach (Hideout hideout2 in allHideouts)
			{
				Settlement settlement = hideout2.Settlement;
				if (settlement.Party.MapEvent == null && settlement.Culture == mobileParty.Party.Culture)
				{
					bool flag = false;
					MobileParty.NavigationType navigationType;
					float num5;
					bool flag2;
					AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, settlement, flag, out navigationType, out num5, out flag2);
					if (navigationType != MobileParty.NavigationType.None)
					{
						float averageDistanceBetweenClosestTwoTownsWithNavigationType = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(navigationType);
						float num6 = averageDistanceBetweenClosestTwoTownsWithNavigationType * 6f / (Campaign.Current.EstimatedAverageBanditPartySpeed * (float)CampaignTime.HoursInDay);
						num5 = Math.Max(averageDistanceBetweenClosestTwoTownsWithNavigationType * 0.15f, num5);
						float num7 = num5 / (Campaign.Current.EstimatedAverageBanditPartySpeed * (float)CampaignTime.HoursInDay);
						float num8 = num6 / (num6 + num7);
						int num9 = 0;
						foreach (MobileParty mobileParty2 in settlement.Parties)
						{
							if (mobileParty2.IsBandit && !mobileParty2.IsBanditBossParty)
							{
								num9++;
							}
						}
						float num11;
						if (num9 < numberOfMinimumBanditPartiesInAHideoutToInfestIt)
						{
							float num10 = (float)(numberOfMaximumHideoutsAtEachBanditFaction - num3) / (float)numberOfMaximumHideoutsAtEachBanditFaction;
							num11 = ((num3 < numberOfMaximumHideoutsAtEachBanditFaction) ? (0.25f + 0.75f * num10) : 0f);
						}
						else
						{
							num11 = Math.Max(0f, 1f * (1f - (float)(Math.Min(numberOfMaximumBanditPartiesInEachHideout, num9) - numberOfMinimumBanditPartiesInAHideoutToInfestIt) / (float)(numberOfMaximumBanditPartiesInEachHideout - numberOfMinimumBanditPartiesInAHideoutToInfestIt)));
						}
						float num12 = ((mobileParty.DefaultBehavior == AiBehavior.GoToSettlement && mobileParty.TargetSettlement == settlement) ? 1f : (MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat));
						float num13 = num8 * num11 * num2 * num12 * num4;
						if (num13 > 0f)
						{
							this.AddBehaviorTupleWithScore(p, hideout2.Settlement, num13, navigationType, false, false);
						}
					}
				}
			}
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x0017FF94 File Offset: 0x0017E194
		private ValueTuple<float, float, float, float> CalculateBeingSettlementOwnerScores(MobileParty mobileParty, Settlement settlement, Settlement currentSettlement, float idealGarrisonStrengthPerWalledCenter, float distanceScorePure, float averagePartySizeRatioToMaximumSize)
		{
			float num = 1f;
			float num2 = 1f;
			float num3 = 1f;
			float num4 = 1f;
			Hero leaderHero = mobileParty.LeaderHero;
			IFaction mapFaction = mobileParty.MapFaction;
			if (currentSettlement != settlement && (mobileParty.Army == null || mobileParty.Army.LeaderParty != mobileParty))
			{
				if (settlement.OwnerClan.Leader == leaderHero)
				{
					float currentTime = Campaign.CurrentTime;
					float lastVisitTimeOfOwner = settlement.LastVisitTimeOfOwner;
					float num5 = ((currentTime - lastVisitTimeOfOwner > this.NumberOfHoursAtDay) ? (currentTime - lastVisitTimeOfOwner) : ((this.NumberOfHoursAtDay - (currentTime - lastVisitTimeOfOwner)) * (this.IdealTimePeriodForVisitingOwnedSettlement / this.NumberOfHoursAtDay))) / this.IdealTimePeriodForVisitingOwnedSettlement;
					num += num5;
				}
				if (MBRandom.RandomFloatWithSeed((uint)mobileParty.RandomValue, (uint)CampaignTime.Now.ToDays) < 0.5f && settlement.IsFortification && leaderHero.Clan != Clan.PlayerClan && (settlement.OwnerClan.Leader == leaderHero || settlement.OwnerClan == leaderHero.Clan))
				{
					if (idealGarrisonStrengthPerWalledCenter.ApproximatelyEqualsTo(-1f, 1E-05f))
					{
						idealGarrisonStrengthPerWalledCenter = FactionHelper.FindIdealGarrisonStrengthPerWalledCenter(mapFaction as Kingdom, null);
					}
					int num6 = Campaign.Current.Models.SettlementGarrisonModel.FindNumberOfTroopsToTakeFromGarrison(mobileParty, settlement, idealGarrisonStrengthPerWalledCenter);
					if (num6 > 0)
					{
						num2 = 1f + MathF.Pow((float)num6, 0.67f);
						if (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty)
						{
							num2 = 1f + (num2 - 1f) / MathF.Sqrt((float)mobileParty.Army.Parties.Count);
						}
					}
				}
			}
			if (settlement == leaderHero.HomeSettlement && mobileParty.Army == null && !settlement.IsVillage)
			{
				float num7 = (leaderHero.HomeSettlement.IsCastle ? 1.5f : 1f);
				if (currentSettlement == settlement)
				{
					num3 += 3000f * num7 / (250f + leaderHero.PassedTimeAtHomeSettlement * leaderHero.PassedTimeAtHomeSettlement);
				}
				else
				{
					num3 += 1000f * num7 / (250f + leaderHero.PassedTimeAtHomeSettlement * leaderHero.PassedTimeAtHomeSettlement);
				}
			}
			if (settlement != currentSettlement)
			{
				float num8 = 1f;
				if (mobileParty.LastVisitedSettlement == settlement)
				{
					num8 = 0.25f;
				}
				if (settlement.IsFortification && settlement.MapFaction == mapFaction && settlement.OwnerClan != Clan.PlayerClan)
				{
					float num9 = ((settlement.Town.GarrisonParty != null) ? settlement.Town.GarrisonParty.Party.EstimatedStrength : 0f);
					float num10 = FactionHelper.OwnerClanEconomyEffectOnGarrisonSizeConstant(settlement.OwnerClan);
					float num11 = FactionHelper.SettlementProsperityEffectOnGarrisonSizeConstant(settlement.Town);
					float num12 = FactionHelper.SettlementFoodPotentialEffectOnGarrisonSizeConstant(settlement);
					if (idealGarrisonStrengthPerWalledCenter == -1f)
					{
						idealGarrisonStrengthPerWalledCenter = FactionHelper.FindIdealGarrisonStrengthPerWalledCenter(mapFaction as Kingdom, null);
					}
					float num13 = idealGarrisonStrengthPerWalledCenter;
					if (settlement.Town.GarrisonParty != null && settlement.Town.GarrisonParty.HasLimitedWage())
					{
						num13 = (float)settlement.Town.GarrisonParty.PaymentLimit / Campaign.Current.AverageWage;
					}
					else
					{
						if (mobileParty.Army != null)
						{
							num13 *= 0.75f;
						}
						num13 *= num10 * num11 * num12;
					}
					float num14 = num13;
					if (num9 < num14)
					{
						float num15 = ((settlement.OwnerClan == leaderHero.Clan) ? 149f : 99f);
						if (settlement.OwnerClan == Clan.PlayerClan)
						{
							num15 *= 0.5f;
						}
						float num16 = 1f - num9 / num14;
						num4 = 1f + num15 * distanceScorePure * distanceScorePure * (averagePartySizeRatioToMaximumSize - 0.5f) * num16 * num16 * num16 * num8;
					}
				}
			}
			return new ValueTuple<float, float, float, float>(num, num2, num3, num4);
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x00180338 File Offset: 0x0017E538
		private float CalculateMergeScoreForDisbandingParty(MobileParty disbandParty, Settlement settlement, float distanceAsDays)
		{
			float num = Campaign.MapDiagonal / (disbandParty._lastCalculatedSpeed * (float)CampaignTime.HoursInDay);
			float num2 = MathF.Pow(3.5f - 0.95f * (Math.Min(num, distanceAsDays) / num), 3f);
			Hero owner = disbandParty.Party.Owner;
			float num3;
			if (((owner != null) ? owner.Clan : null) != settlement.OwnerClan)
			{
				Hero owner2 = disbandParty.Party.Owner;
				num3 = ((((owner2 != null) ? owner2.MapFaction : null) == settlement.MapFaction) ? 0.35f : 0.025f);
			}
			else
			{
				num3 = 1f;
			}
			float num4 = num3;
			float num5 = ((disbandParty.DefaultBehavior == AiBehavior.GoToSettlement && disbandParty.TargetSettlement == settlement) ? 1f : 0.3f);
			float num6 = (settlement.IsFortification ? 3f : 1f);
			float num7 = num2 * num4 * num5 * num6;
			if (num7 < 0.025f)
			{
				num7 = 0.035f;
			}
			return num7;
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x00180418 File Offset: 0x0017E618
		private float CalculateMergeScoreForLeaderlessParty(MobileParty leaderlessParty, Settlement settlement, float distanceAsDays, out bool canMerge)
		{
			if (settlement.IsVillage)
			{
				canMerge = false;
				return -1f;
			}
			float num = Campaign.MapDiagonal / (leaderlessParty._lastCalculatedSpeed * (float)CampaignTime.HoursInDay);
			float num2 = MathF.Pow(3.5f - 0.95f * (Math.Min(num, distanceAsDays) / num), 3f);
			float num3;
			if (leaderlessParty.ActualClan != settlement.OwnerClan)
			{
				Clan actualClan = leaderlessParty.ActualClan;
				num3 = ((((actualClan != null) ? actualClan.MapFaction : null) == settlement.MapFaction) ? 0.35f : 0f);
			}
			else
			{
				num3 = 2f;
			}
			float num4 = num3;
			float num5 = ((leaderlessParty.DefaultBehavior == AiBehavior.GoToSettlement && leaderlessParty.TargetSettlement == settlement) ? 1f : 0.3f);
			float num6 = (settlement.IsFortification ? 3f : 0.5f);
			canMerge = true;
			return num2 * num4 * num5 * num6;
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x001804E4 File Offset: 0x0017E6E4
		private static void FillSettlementsToVisitWithDistancesAsDays(MobileParty mobileParty, List<AiVisitSettlementBehavior.SettlementNavigationData> listToFill)
		{
			float num = AiVisitSettlementBehavior.SearchForNeutralSettlementRadiusAsDays * Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay;
			if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.MapFaction.IsKingdomFaction)
			{
				List<Settlement> settlements = mobileParty.MapFaction.Settlements;
				float num2 = 0f;
				foreach (Settlement settlement in settlements)
				{
					if (AiVisitSettlementBehavior.IsSettlementSuitableForVisitingCondition(mobileParty, settlement))
					{
						MobileParty.NavigationType navigationType;
						float num3;
						bool flag;
						bool flag2;
						AiVisitSettlementBehavior.GetBestNavigationDataForVisitingSettlement(mobileParty, settlement, out navigationType, out num3, out flag, out flag2);
						if (navigationType != MobileParty.NavigationType.None && num3 < AiVisitSettlementBehavior.GetMaximumDistanceAsDays(navigationType))
						{
							num2 += num3;
							listToFill.Add(new AiVisitSettlementBehavior.SettlementNavigationData(num3, settlement.GetHashCode(), settlement, navigationType, flag, flag2));
						}
					}
				}
				num2 /= (float)listToFill.Count;
				if (num2 > AiVisitSettlementBehavior.GetMaximumDistanceAsDays(mobileParty.NavigationCapability) * 0.7f && (mobileParty.Army == null || mobileParty.Army.LeaderParty == mobileParty))
				{
					LocatableSearchData<Settlement> locatableSearchData = Settlement.StartFindingLocatablesAroundPosition(mobileParty.Position.ToVec2(), num);
					for (Settlement settlement2 = Settlement.FindNextLocatable(ref locatableSearchData); settlement2 != null; settlement2 = Settlement.FindNextLocatable(ref locatableSearchData))
					{
						if (!settlement2.IsCastle && settlement2.MapFaction != mobileParty.MapFaction && AiVisitSettlementBehavior.IsSettlementSuitableForVisitingCondition(mobileParty, settlement2))
						{
							MobileParty.NavigationType navigationType2;
							float num4;
							bool flag3;
							bool flag4;
							AiVisitSettlementBehavior.GetBestNavigationDataForVisitingSettlement(mobileParty, settlement2, out navigationType2, out num4, out flag3, out flag4);
							if (navigationType2 != MobileParty.NavigationType.None && num4 < AiVisitSettlementBehavior.GetMaximumDistanceAsDays(navigationType2))
							{
								listToFill.Add(new AiVisitSettlementBehavior.SettlementNavigationData(num4, settlement2.GetHashCode(), settlement2, navigationType2, flag3, flag4));
							}
						}
					}
				}
			}
			else
			{
				LocatableSearchData<Settlement> locatableSearchData2 = Settlement.StartFindingLocatablesAroundPosition(mobileParty.Position.ToVec2(), num * 1.6f);
				for (Settlement settlement3 = Settlement.FindNextLocatable(ref locatableSearchData2); settlement3 != null; settlement3 = Settlement.FindNextLocatable(ref locatableSearchData2))
				{
					if (AiVisitSettlementBehavior.IsSettlementSuitableForVisitingCondition(mobileParty, settlement3))
					{
						MobileParty.NavigationType navigationType3;
						float num5;
						bool flag5;
						bool flag6;
						AiVisitSettlementBehavior.GetBestNavigationDataForVisitingSettlement(mobileParty, settlement3, out navigationType3, out num5, out flag5, out flag6);
						if (navigationType3 != MobileParty.NavigationType.None && num5 < AiVisitSettlementBehavior.GetMaximumDistanceAsDays(navigationType3))
						{
							listToFill.Add(new AiVisitSettlementBehavior.SettlementNavigationData(num5, settlement3.GetHashCode(), settlement3, navigationType3, flag5, flag6));
						}
					}
				}
			}
			if (!listToFill.AnyQ<AiVisitSettlementBehavior.SettlementNavigationData>())
			{
				Settlement factionMidSettlement = mobileParty.MapFaction.FactionMidSettlement;
				if (factionMidSettlement != null)
				{
					if (factionMidSettlement.IsFortification)
					{
						using (List<Village>.Enumerator enumerator2 = factionMidSettlement.BoundVillages.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Village village = enumerator2.Current;
								if (AiVisitSettlementBehavior.IsSettlementSuitableForVisitingCondition(mobileParty, village.Settlement))
								{
									MobileParty.NavigationType navigationType4;
									float num6;
									bool flag7;
									bool flag8;
									AiVisitSettlementBehavior.GetBestNavigationDataForVisitingSettlement(mobileParty, village.Settlement, out navigationType4, out num6, out flag7, out flag8);
									if (navigationType4 != MobileParty.NavigationType.None)
									{
										listToFill.Add(new AiVisitSettlementBehavior.SettlementNavigationData(num6, village.GetHashCode(), village.Settlement, navigationType4, flag7, flag8));
									}
								}
							}
							return;
						}
					}
					if (AiVisitSettlementBehavior.IsSettlementSuitableForVisitingCondition(mobileParty, factionMidSettlement))
					{
						MobileParty.NavigationType navigationType5;
						float num7;
						bool flag9;
						bool flag10;
						AiVisitSettlementBehavior.GetBestNavigationDataForVisitingSettlement(mobileParty, factionMidSettlement, out navigationType5, out num7, out flag9, out flag10);
						if (navigationType5 != MobileParty.NavigationType.None)
						{
							listToFill.Add(new AiVisitSettlementBehavior.SettlementNavigationData(num7, factionMidSettlement.GetHashCode(), factionMidSettlement, navigationType5, flag9, flag10));
						}
					}
				}
			}
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x001807E4 File Offset: 0x0017E9E4
		private static void GetBestNavigationDataForVisitingSettlement(MobileParty mobileParty, Settlement settlement, out MobileParty.NavigationType bestNavigationType, out float distanceAsDays, out bool isFromPort, out bool isTargetingPortBetter)
		{
			bestNavigationType = MobileParty.NavigationType.None;
			float num = float.MaxValue;
			bool flag = false;
			isTargetingPortBetter = false;
			isFromPort = false;
			if (!settlement.HasPort || settlement.SiegeEvent == null || settlement.SiegeEvent.IsBlockadeActive || !mobileParty.HasNavalNavigationCapability)
			{
				AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, settlement, false, out bestNavigationType, out num, out flag);
			}
			if (mobileParty.HasNavalNavigationCapability && settlement.HasPort && settlement.IsFortification)
			{
				MobileParty.NavigationType navigationType;
				float num2;
				bool flag2;
				AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, settlement, true, out navigationType, out num2, out flag2);
				if (num2 < num)
				{
					bestNavigationType = navigationType;
					num = num2;
					isFromPort = flag2;
					isTargetingPortBetter = true;
				}
				else
				{
					isFromPort = flag;
					isTargetingPortBetter = false;
				}
			}
			distanceAsDays = num / (Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay);
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x00180890 File Offset: 0x0017EA90
		private void AddBehaviorTupleWithScore(PartyThinkParams p, Settlement settlement, float visitingNearbySettlementScore, MobileParty.NavigationType navigationType, bool isFromPort, bool isTargetingPortBetter)
		{
			AIBehaviorData aibehaviorData = new AIBehaviorData(settlement, AiBehavior.GoToSettlement, navigationType, false, isFromPort, isTargetingPortBetter);
			float num;
			if (p.TryGetBehaviorScore(in aibehaviorData, out num))
			{
				p.SetBehaviorScore(in aibehaviorData, num + visitingNearbySettlementScore);
				return;
			}
			ValueTuple<AIBehaviorData, float> valueTuple = new ValueTuple<AIBehaviorData, float>(aibehaviorData, visitingNearbySettlementScore);
			p.AddBehaviorScore(in valueTuple);
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x001808D8 File Offset: 0x0017EAD8
		private static bool IsSettlementSuitableForVisitingCondition(MobileParty mobileParty, Settlement settlement)
		{
			return settlement.Party.MapEvent == null && (settlement.Party.SiegeEvent == null || (!settlement.Party.SiegeEvent.IsBlockadeActive && mobileParty.HasNavalNavigationCapability)) && (!mobileParty.Party.Owner.MapFaction.IsAtWarWith(settlement.MapFaction) || ((mobileParty.Party.Owner.MapFaction.IsMinorFaction || mobileParty.MapFaction.Settlements.Count == 0) && settlement.IsVillage)) && (settlement.IsVillage || settlement.IsFortification) && (!settlement.IsVillage || settlement.Village.VillageState == Village.VillageStates.Normal);
		}

		// Token: 0x040014F4 RID: 5364
		public const float GoodEnoughScore = 8f;

		// Token: 0x040014F5 RID: 5365
		public const float MeaningfulScoreThreshold = 0.025f;

		// Token: 0x040014F6 RID: 5366
		public const float BaseVisitScore = 1.6f;

		// Token: 0x040014F7 RID: 5367
		private const float DefaultMoneyLimitForRecruiting = 2000f;

		// Token: 0x040014F8 RID: 5368
		private readonly List<AiVisitSettlementBehavior.SettlementNavigationData> _settlementsNavigationData = new List<AiVisitSettlementBehavior.SettlementNavigationData>();

		// Token: 0x040014F9 RID: 5369
		private readonly Dictionary<Settlement, int> _numberOfAlliedMobilePartiesTargetingSettlement = new Dictionary<Settlement, int>();

		// Token: 0x040014FA RID: 5370
		private IDisbandPartyCampaignBehavior _disbandPartyCampaignBehavior;

		// Token: 0x020008CA RID: 2250
		private readonly struct SettlementNavigationData : IComparable<AiVisitSettlementBehavior.SettlementNavigationData>
		{
			// Token: 0x06006C72 RID: 27762 RVA: 0x001DC15E File Offset: 0x001DA35E
			public SettlementNavigationData(float distance, int settlementIdentifier, Settlement settlement, MobileParty.NavigationType bestNavigationType, bool isFromPort, bool isTargetingPortBetter)
			{
				this.Distance = distance;
				this.SettlementIdentifier = settlementIdentifier;
				this.Settlement = settlement;
				this.BestNavigationType = bestNavigationType;
				this.IsFromPort = isFromPort;
				this.IsTargetingPortBetter = isTargetingPortBetter;
			}

			// Token: 0x06006C73 RID: 27763 RVA: 0x001DC190 File Offset: 0x001DA390
			public int CompareTo(AiVisitSettlementBehavior.SettlementNavigationData otherSettlementNavigationData)
			{
				int num = this.Distance.CompareTo(otherSettlementNavigationData.Distance);
				if (num == 0)
				{
					num = this.SettlementIdentifier.CompareTo(otherSettlementNavigationData.SettlementIdentifier);
				}
				return num;
			}

			// Token: 0x04002626 RID: 9766
			public readonly float Distance;

			// Token: 0x04002627 RID: 9767
			public readonly int SettlementIdentifier;

			// Token: 0x04002628 RID: 9768
			public readonly Settlement Settlement;

			// Token: 0x04002629 RID: 9769
			public readonly MobileParty.NavigationType BestNavigationType;

			// Token: 0x0400262A RID: 9770
			public readonly bool IsFromPort;

			// Token: 0x0400262B RID: 9771
			public readonly bool IsTargetingPortBetter;
		}
	}
}
