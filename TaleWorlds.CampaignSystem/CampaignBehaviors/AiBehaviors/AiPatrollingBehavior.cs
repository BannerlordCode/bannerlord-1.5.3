using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x02000495 RID: 1173
	public class AiPatrollingBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004B4F RID: 19279 RVA: 0x0017E14C File Offset: 0x0017C34C
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnShipDestroyedEvent.AddNonSerializedListener(this, new Action<PartyBase, Ship, DestroyShipAction.ShipDestroyDetail>(this.OnShipDestroyed));
			CampaignEvents.OnBlockadeActivatedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnBlockadeActivated));
			CampaignEvents.OnShipOwnerChangedEvent.AddNonSerializedListener(this, new Action<Ship, PartyBase, ChangeShipOwnerAction.ShipOwnerChangeDetail>(this.OnShipOwnerChanged));
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x0017E1CC File Offset: 0x0017C3CC
		private void OnBlockadeActivated(SiegeEvent siegeEvent)
		{
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				if (mobileParty.DefaultBehavior == AiBehavior.GoToSettlement && mobileParty.TargetSettlement == siegeEvent.BesiegedSettlement && mobileParty.CurrentSettlement != siegeEvent.BesiegedSettlement)
				{
					mobileParty.SetMoveModeHold();
				}
			}
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x0017E244 File Offset: 0x0017C444
		private void OnShipOwnerChanged(Ship ship, PartyBase oldOwner, ChangeShipOwnerAction.ShipOwnerChangeDetail changeDetail)
		{
			this.CheckPartyIfNeeded(oldOwner);
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x0017E24D File Offset: 0x0017C44D
		private void OnShipDestroyed(PartyBase owner, Ship ship, DestroyShipAction.ShipDestroyDetail detail)
		{
			this.CheckPartyIfNeeded(owner);
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x0017E258 File Offset: 0x0017C458
		private void CheckPartyIfNeeded(PartyBase party)
		{
			if (party != null && party.IsMobile && party.MobileParty.IsLordParty && party.MobileParty.DefaultBehavior == AiBehavior.PatrolAroundPoint && !party.MobileParty.TargetPosition.IsOnLand && !party.MobileParty.HasNavalNavigationCapability)
			{
				party.MobileParty.SetMoveModeHold();
			}
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x0017E2B6 File Offset: 0x0017C4B6
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this._disbandPartyCampaignBehavior = Campaign.Current.GetCampaignBehavior<IDisbandPartyCampaignBehavior>();
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x0017E2C8 File Offset: 0x0017C4C8
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x0017E2CC File Offset: 0x0017C4CC
		private void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			if (mobileParty.IsMilitia || mobileParty.IsCaravan || mobileParty.IsVillager || mobileParty.IsBandit || mobileParty.IsPatrolParty || mobileParty.IsDisbanding || (!mobileParty.MapFaction.IsMinorFaction && !mobileParty.MapFaction.IsKingdomFaction && !mobileParty.MapFaction.Leader.IsLord))
			{
				return;
			}
			if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsUnderSiege)
			{
				return;
			}
			if (mobileParty.Army != null)
			{
				return;
			}
			if (mobileParty.GetNumDaysForFoodToLast() <= 6)
			{
				return;
			}
			Settlement currentSettlement = mobileParty.CurrentSettlement;
			if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) != null)
			{
				return;
			}
			float num4;
			if (mobileParty.Army != null)
			{
				float num = 0f;
				foreach (MobileParty mobileParty2 in mobileParty.Army.Parties)
				{
					float num2 = PartyBaseHelper.FindPartySizeNormalLimit(mobileParty2);
					float num3 = mobileParty2.PartySizeRatio / num2;
					num += num3;
				}
				num4 = num / (float)mobileParty.Army.Parties.Count;
			}
			else
			{
				float num5 = PartyBaseHelper.FindPartySizeNormalLimit(mobileParty);
				num4 = mobileParty.PartySizeRatio / num5;
			}
			float num6 = MathF.Sqrt(MathF.Min(1f, num4));
			if (!mobileParty.IsDisbanding)
			{
				IDisbandPartyCampaignBehavior disbandPartyCampaignBehavior = this._disbandPartyCampaignBehavior;
				if (disbandPartyCampaignBehavior == null || !disbandPartyCampaignBehavior.IsPartyWaitingForDisband(mobileParty))
				{
					goto IL_0157;
				}
			}
			num6 *= 0.25f;
			IL_0157:
			this.CalculateDefensivePatrollingScores(mobileParty, p, num6);
			this.CalculateOffensiveNavalPatrollingScores(mobileParty, p, num6);
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x0017E454 File Offset: 0x0017C654
		private void CalculateOffensiveNavalPatrollingScores(MobileParty mobileParty, PartyThinkParams p, float scoreAdjustment)
		{
			if (mobileParty.HasNavalNavigationCapability && mobileParty.MapFaction.IsKingdomFaction && mobileParty.MapFaction.Leader != mobileParty.LeaderHero)
			{
				foreach (IFaction faction in mobileParty.MapFaction.FactionsAtWarWith)
				{
					if (faction.IsMapFaction)
					{
						foreach (Settlement settlement in faction.Settlements)
						{
							if (settlement.HasPort)
							{
								float num;
								this.GetDistanceScoreForOffensiveNavalPatrolling(settlement, mobileParty, out num);
								if (num > 0.5f)
								{
									this.CalculateOffensiveNavalPatrollingScoreForSettlement(settlement, p, num);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x0017E540 File Offset: 0x0017C740
		private void CalculateDefensivePatrollingScores(MobileParty mobileParty, PartyThinkParams p, float scoreAdjustment)
		{
			if (mobileParty.Party.MapFaction.Settlements.Count > 0)
			{
				float num;
				SettlementHelper.FindFurthestFortificationToSettlement(mobileParty.MapFaction.Fiefs, MobileParty.NavigationType.Default, mobileParty.MapFaction.FactionMidSettlement, out num);
				using (List<Settlement>.Enumerator enumerator = mobileParty.Party.MapFaction.Settlements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Settlement settlement = enumerator.Current;
						if (settlement.IsTown || settlement.IsVillage)
						{
							float maxValue = float.MaxValue;
							if (settlement.HasPort && mobileParty.HasNavalNavigationCapability && (!mobileParty.MapFaction.IsKingdomFaction || mobileParty.MapFaction.Leader != mobileParty.LeaderHero))
							{
								this.GetDistanceScoreForDefensiveNavalPatrolling(settlement, mobileParty, out maxValue);
								if (maxValue > 0.2f)
								{
									this.CalculateDefensivePatrollingScoreForSettlement(settlement, p, maxValue, true);
								}
							}
							this.GetDistanceScoreForLandPatrolling(settlement, mobileParty, num, out maxValue);
							if (maxValue > 0.2f)
							{
								this.CalculateDefensivePatrollingScoreForSettlement(settlement, p, maxValue, false);
							}
						}
					}
					return;
				}
			}
			float num2 = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(mobileParty.NavigationCapability) * 4f / (Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay) * Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay;
			int num3 = -1;
			do
			{
				num3 = SettlementHelper.FindNextSettlementAroundMobileParty(mobileParty, mobileParty.NavigationCapability, num2, num3, (Settlement x) => x.IsTown);
				if (num3 >= 0)
				{
					Settlement settlement2 = Settlement.All[num3];
					float averageDistanceBetweenClosestTwoTownsWithNavigationType = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Default);
					float num4 = Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty.HomeSettlement, settlement2, false, false, MobileParty.NavigationType.Default);
					if (num4 < averageDistanceBetweenClosestTwoTownsWithNavigationType)
					{
						num4 = averageDistanceBetweenClosestTwoTownsWithNavigationType;
					}
					float num5 = averageDistanceBetweenClosestTwoTownsWithNavigationType * 5f / num4;
					this.CalculateDefensivePatrollingScoreForSettlement(settlement2, p, scoreAdjustment * num5, false);
				}
			}
			while (num3 >= 0);
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x0017E738 File Offset: 0x0017C938
		private void GetDistanceScoreForDefensiveNavalPatrolling(Settlement targetSettlement, MobileParty mobileParty, out float bestDistanceScore)
		{
			bestDistanceScore = 0f;
			MobileParty.NavigationType navigationType;
			float num;
			bool flag;
			AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, targetSettlement, true, out navigationType, out num, out flag);
			if (navigationType != MobileParty.NavigationType.None)
			{
				float averageDistanceBetweenClosestTwoTownsWithNavigationType = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Naval);
				if (num > averageDistanceBetweenClosestTwoTownsWithNavigationType)
				{
					bestDistanceScore = -1f;
					return;
				}
				bestDistanceScore = MBMath.Map(1f - num / averageDistanceBetweenClosestTwoTownsWithNavigationType, 0f, 1f, 0.2f, 1f);
			}
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x0017E79C File Offset: 0x0017C99C
		private void GetDistanceScoreForOffensiveNavalPatrolling(Settlement targetSettlement, MobileParty mobileParty, out float bestDistanceScore)
		{
			bestDistanceScore = 0f;
			MobileParty.NavigationType navigationType;
			float num;
			bool flag;
			AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, targetSettlement, true, out navigationType, out num, out flag);
			if (navigationType != MobileParty.NavigationType.None)
			{
				float num2 = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Naval) * 3f;
				if (num > num2)
				{
					bestDistanceScore = -1f;
					return;
				}
				bestDistanceScore = MBMath.Map(1f - num / num2, 0f, 1f, 0.5f, 1.5f);
			}
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x0017E804 File Offset: 0x0017CA04
		private void GetDistanceScoreForLandPatrolling(Settlement targetSettlement, MobileParty mobileParty, float distanceToFurthestAllySettlementToFactionMidSettlement, out float bestDistanceScore)
		{
			float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty.MapFaction.FactionMidSettlement, targetSettlement, false, false, mobileParty.NavigationCapability);
			float num;
			if (distanceToFurthestAllySettlementToFactionMidSettlement == 0f)
			{
				num = 0.5f;
			}
			else
			{
				num = distance / distanceToFurthestAllySettlementToFactionMidSettlement;
			}
			float num2 = MBMath.Map(num, 0f, 1f, 0.2f, 0.8f);
			if (mobileParty.PartySizeRatio >= num2)
			{
				bestDistanceScore = MBMath.Map(0.8f - (mobileParty.PartySizeRatio - num2), 0f, 0.8f, 0.2f, 1f);
				return;
			}
			bestDistanceScore = 0f;
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x0017E8AC File Offset: 0x0017CAAC
		private void CalculateDefensivePatrollingScoreForSettlement(Settlement settlement, PartyThinkParams p, float scoreAdjustment, bool isNavalPatrolling)
		{
			MobileParty mobilePartyOf = p.MobilePartyOf;
			MobileParty.NavigationType navigationType;
			float num;
			bool flag;
			AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobilePartyOf, settlement, isNavalPatrolling, out navigationType, out num, out flag);
			if (navigationType != MobileParty.NavigationType.None)
			{
				AIBehaviorData aibehaviorData = new AIBehaviorData(settlement, AiBehavior.PatrolAroundPoint, navigationType, false, flag, isNavalPatrolling);
				float num2 = Campaign.Current.Models.TargetScoreCalculatingModel.CalculateDefensivePatrollingScoreForSettlement(settlement, isNavalPatrolling, mobilePartyOf);
				num2 *= scoreAdjustment;
				if (num2 > 0f)
				{
					if (!mobilePartyOf.IsCurrentlyAtSea)
					{
					}
					ValueTuple<AIBehaviorData, float> valueTuple = new ValueTuple<AIBehaviorData, float>(aibehaviorData, 1.44f + num2);
					p.AddBehaviorScore(in valueTuple);
				}
			}
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x0017E930 File Offset: 0x0017CB30
		private void CalculateOffensiveNavalPatrollingScoreForSettlement(Settlement settlement, PartyThinkParams p, float scoreAdjustment)
		{
			MobileParty mobilePartyOf = p.MobilePartyOf;
			MobileParty.NavigationType navigationType;
			float num;
			bool flag;
			AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobilePartyOf, settlement, true, out navigationType, out num, out flag);
			if (navigationType != MobileParty.NavigationType.None)
			{
				AIBehaviorData aibehaviorData = new AIBehaviorData(settlement, AiBehavior.PatrolAroundPoint, navigationType, false, flag, true);
				float num2 = Campaign.Current.Models.TargetScoreCalculatingModel.CalculateOffensivePatrollingScoreForSettlement(settlement, true, mobilePartyOf);
				num2 *= scoreAdjustment;
				if (num2 > 0f)
				{
					ValueTuple<AIBehaviorData, float> valueTuple = new ValueTuple<AIBehaviorData, float>(aibehaviorData, 1.44f + num2);
					p.AddBehaviorScore(in valueTuple);
				}
			}
		}

		// Token: 0x040014EE RID: 5358
		private const float BasePatrolScore = 1.44f;

		// Token: 0x040014EF RID: 5359
		private const float MinimumDefensivePatrolDistanceScore = 0.2f;

		// Token: 0x040014F0 RID: 5360
		private const float MaximumDefensivePatrolDistanceScore = 1f;

		// Token: 0x040014F1 RID: 5361
		private const float MinimumOffensivePatrolDistanceScore = 0.5f;

		// Token: 0x040014F2 RID: 5362
		private const float MaximumOffensivePatrolDistanceScore = 1.5f;

		// Token: 0x040014F3 RID: 5363
		private IDisbandPartyCampaignBehavior _disbandPartyCampaignBehavior;
	}
}
