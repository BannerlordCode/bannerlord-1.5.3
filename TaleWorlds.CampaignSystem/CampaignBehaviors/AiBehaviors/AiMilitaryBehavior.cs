using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x02000492 RID: 1170
	public class AiMilitaryBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004B2E RID: 19246 RVA: 0x0017BEF8 File Offset: 0x0017A0F8
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x0017BF78 File Offset: 0x0017A178
		private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			if (mapEvent.MapEventSettlement != null && mapEvent.MapEventSettlement.HasPort)
			{
				if (mapEvent.MapEventSettlement.IsFortification && mapEvent.MapEventSettlement.SiegeEvent != null && mapEvent.MapEventSettlement.SiegeEvent.IsBlockadeActive)
				{
					bool isNavalMapEvent = mapEvent.IsNavalMapEvent;
					using (List<MobileParty>.Enumerator enumerator = MobileParty.AllLordParties.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MobileParty mobileParty = enumerator.Current;
							bool flag = mobileParty.DefaultBehavior == AiBehavior.DefendSettlement && mobileParty.TargetSettlement == mapEvent.MapEventSettlement;
							if (((mobileParty.ShortTermBehavior == AiBehavior.EngageParty && mobileParty.ShortTermTargetParty.SiegeEvent != null && mobileParty.ShortTermTargetParty.MapFaction.IsAtWarWith(mobileParty.MapFaction)) || flag) && isNavalMapEvent != mobileParty.IsTargetingPort)
							{
								mobileParty.SetMoveModeHold();
							}
						}
						return;
					}
				}
				if (mapEvent.MapEventSettlement.IsVillage)
				{
					foreach (MobileParty mobileParty2 in MobileParty.AllLordParties)
					{
						if (mobileParty2.CurrentSettlement != mapEvent.MapEventSettlement && mobileParty2.DefaultBehavior == AiBehavior.GoToSettlement && mobileParty2.TargetSettlement == mapEvent.MapEventSettlement)
						{
							mobileParty2.SetMoveModeHold();
						}
					}
				}
			}
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x0017C0F0 File Offset: 0x0017A2F0
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				if (mobileParty.DefaultBehavior == AiBehavior.GoToSettlement && mobileParty.TargetSettlement == siegeEvent.BesiegedSettlement && mobileParty.CurrentSettlement != siegeEvent.BesiegedSettlement && !mobileParty.IsTargetingPort)
				{
					mobileParty.SetMoveModeHold();
				}
			}
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x0017C170 File Offset: 0x0017A370
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.RetreatingSide != BattleSideEnum.None)
			{
				MapEventSide mapEventSide = mapEvent.GetMapEventSide(mapEvent.RetreatingSide.GetOppositeSide());
				using (List<MapEventParty>.Enumerator enumerator = mapEvent.GetMapEventSide(mapEvent.RetreatingSide).Parties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MapEventParty mapEventParty = enumerator.Current;
						MobileParty mobileParty = mapEventParty.Party.MobileParty;
						if (mobileParty != null && mobileParty.AttachedTo == null)
						{
							mobileParty.TeleportPartyToOutSideOfEncounterRadius();
							CampaignVec2 campaignVec;
							mobileParty.Ai.CalculateFleePosition(out campaignVec, mapEventSide.LeaderParty.MobileParty, ((mapEventSide.LeaderParty.MobileParty != null) ? mapEventSide.LeaderParty.MobileParty.Position.ToVec2() : mapEvent.Position.ToVec2()) - mobileParty.Position.ToVec2());
							mobileParty.SetMoveGoToPoint(campaignVec, mobileParty.IsCurrentlyAtSea ? MobileParty.NavigationType.Naval : MobileParty.NavigationType.Default);
						}
					}
					return;
				}
			}
			MobileParty mobileParty2 = mapEvent.AttackerSide.LeaderParty.MobileParty;
			bool flag = mapEvent.IsRaid && mapEvent.BattleState == BattleState.AttackerVictory && !mapEvent.MapEventSettlement.SettlementHitPoints.ApproximatelyEqualsTo(0f, 1E-05f);
			Settlement mapEventSettlement = mapEvent.MapEventSettlement;
			if (mobileParty2 != MobileParty.MainParty && flag)
			{
				mobileParty2.SetMoveRaidSettlement(mapEventSettlement, mobileParty2.NavigationCapability, mobileParty2.IsCurrentlyAtSea);
				mobileParty2.RecalculateShortTermBehavior();
			}
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x0017C300 File Offset: 0x0017A500
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this._disbandPartyCampaignBehavior = Campaign.Current.GetCampaignBehavior<IDisbandPartyCampaignBehavior>();
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x0017C312 File Offset: 0x0017A512
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x0017C314 File Offset: 0x0017A514
		public void FindBestTargetAndItsValueForFaction(Army.ArmyTypes missionType, PartyThinkParams p, float ourStrength)
		{
			MobileParty mobilePartyOf = p.MobilePartyOf;
			IFaction mapFaction = mobilePartyOf.MapFaction;
			if (mobilePartyOf.Army != null && mobilePartyOf.Army.LeaderParty != mobilePartyOf)
			{
				return;
			}
			float num = 1f;
			if (mobilePartyOf.Army != null && mobilePartyOf.Army.Cohesion < 40f)
			{
				num *= mobilePartyOf.Army.Cohesion / 40f;
			}
			if (num > 0.25f)
			{
				float partySizeScore = this.GetPartySizeScore(mobilePartyOf, missionType);
				AiBehavior aiBehavior = AiBehavior.Hold;
				switch (missionType)
				{
				case Army.ArmyTypes.Besieger:
					aiBehavior = AiBehavior.BesiegeSettlement;
					break;
				case Army.ArmyTypes.Raider:
					aiBehavior = AiBehavior.RaidSettlement;
					break;
				case Army.ArmyTypes.Defender:
					aiBehavior = AiBehavior.DefendSettlement;
					break;
				}
				float foodScoreForActionType = this.GetFoodScoreForActionType(p, missionType);
				if (foodScoreForActionType > 0f)
				{
					if (missionType == Army.ArmyTypes.Defender)
					{
						this.CalculateMilitaryBehaviorForFactionSettlements(mapFaction, p, missionType, aiBehavior, ourStrength, partySizeScore, num, foodScoreForActionType);
						return;
					}
					if (missionType != Army.ArmyTypes.Raider || (mobilePartyOf.Army == null && !p.WillGatherAnArmy))
					{
						for (int i = 0; i < mapFaction.FactionsAtWarWith.Count; i++)
						{
							IFaction faction = mapFaction.FactionsAtWarWith[i];
							if (faction.Leader != null && faction.IsMapFaction)
							{
								this.CalculateMilitaryBehaviorForFactionSettlements(faction, p, missionType, aiBehavior, ourStrength, partySizeScore, num, foodScoreForActionType);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x0017C43C File Offset: 0x0017A63C
		private float GetFoodScoreForActionType(PartyThinkParams p, Army.ArmyTypes type)
		{
			float num = ((type == Army.ArmyTypes.Raider) ? Campaign.Current.Models.MobilePartyAIModel.NeededFoodsInDaysThresholdForRaid : Campaign.Current.Models.MobilePartyAIModel.NeededFoodsInDaysThresholdForSiege);
			MobileParty mobilePartyOf = p.MobilePartyOf;
			int num2 = mobilePartyOf.GetNumDaysForFoodToLast();
			if (p.WillGatherAnArmy)
			{
				foreach (MobileParty mobileParty in p.PossibleArmyMembersUponArmyCreation)
				{
					num2 += mobileParty.GetNumDaysForFoodToLast();
				}
				num2 /= p.PossibleArmyMembersUponArmyCreation.Count + 1;
			}
			else if (mobilePartyOf.Army != null && mobilePartyOf == mobilePartyOf.Army.LeaderParty)
			{
				foreach (MobileParty mobileParty2 in mobilePartyOf.Army.LeaderParty.AttachedParties)
				{
					num2 += mobileParty2.GetNumDaysForFoodToLast();
				}
				num2 /= mobilePartyOf.Army.LeaderParty.AttachedParties.Count + 1;
			}
			if ((p.WillGatherAnArmy || type == Army.ArmyTypes.Raider) && num > (float)num2)
			{
				return 0f;
			}
			if ((float)num2 >= num)
			{
				return 1f;
			}
			return 0.1f + 0.9f * ((float)num2 / num);
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x0017C59C File Offset: 0x0017A79C
		private float GetPartySizeScore(MobileParty mobileParty, Army.ArmyTypes missionType)
		{
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
			float num6 = MathF.Max(1f, MathF.Min((float)mobileParty.MapFaction.Fiefs.Count<Town>((Town x) => x.IsTown) / 5f, 2.5f));
			if (missionType == Army.ArmyTypes.Defender)
			{
				num6 *= 0.5f;
			}
			else if (missionType == Army.ArmyTypes.Raider)
			{
				num6 *= 0.75f;
			}
			return MathF.Min(1f, MathF.Pow(num4, num6));
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x0017C6B8 File Offset: 0x0017A8B8
		private void CalculateMilitaryBehaviorForFactionSettlements(IFaction faction, PartyThinkParams p, Army.ArmyTypes missionType, AiBehavior aiBehavior, float ourStrength, float partySizeScore, float cohesionScore, float foodScore)
		{
			MobileParty mobilePartyOf = p.MobilePartyOf;
			for (int i = 0; i < faction.Settlements.Count; i++)
			{
				Settlement settlement = faction.Settlements[i];
				if (this.CheckIfSettlementIsSuitableForMilitaryAction(settlement, mobilePartyOf, missionType, p.WillGatherAnArmy))
				{
					this.CalculateMilitaryBehaviorForSettlement(settlement, missionType, aiBehavior, p, ourStrength, partySizeScore, cohesionScore, foodScore);
				}
			}
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x0017C714 File Offset: 0x0017A914
		private bool CheckIfSettlementIsSuitableForMilitaryAction(Settlement settlement, MobileParty mobileParty, Army.ArmyTypes missionType, bool isCalculatingForNewArmyCreation)
		{
			return (!MobileParty.MainParty.ShouldBeIgnored || mobileParty.IsMainParty || mobileParty.AttachedParties.Contains(MobileParty.MainParty) || ((settlement.Party.MapEvent == null || settlement.Party.MapEvent != MapEvent.PlayerMapEvent) && (settlement.SiegeEvent == null || !settlement.SiegeEvent.IsPlayerSiegeEvent))) && (settlement.LastAttackerParty != MobileParty.MainParty || mobileParty.Ai.DoNotAttackMainPartyUntil.IsPast) && ((mobileParty.Army == null && !isCalculatingForNewArmyCreation) || missionType != Army.ArmyTypes.Defender || !settlement.IsVillage);
		}

		// Token: 0x06004B39 RID: 19257 RVA: 0x0017C7C0 File Offset: 0x0017A9C0
		private void CalculateDistanceScoreForBesieging(Settlement targetSettlement, MobileParty mobileParty, out MobileParty.NavigationType bestNavigationType, out float bestDistanceScore, out bool isFromPort, out bool isTargetingPort)
		{
			this._checkedNeighbors.Clear();
			float num = 0.01f;
			float num2 = 0.0001f;
			IFaction mapFaction = mobileParty.MapFaction;
			MBReadOnlyList<Settlement> neighborFortifications = targetSettlement.Town.GetNeighborFortifications(MobileParty.NavigationType.All);
			foreach (Settlement settlement in neighborFortifications)
			{
				this._checkedNeighbors.Add(settlement);
				if (settlement.MapFaction != targetSettlement.MapFaction)
				{
					num += 1f;
					if (settlement.MapFaction == mapFaction)
					{
						num2 += 1f;
					}
				}
			}
			float num3 = 0.01f;
			float num4 = 0.0001f;
			foreach (Settlement settlement2 in neighborFortifications)
			{
				foreach (Settlement settlement3 in settlement2.Town.GetNeighborFortifications(MobileParty.NavigationType.All))
				{
					if (settlement3 != targetSettlement && !this._checkedNeighbors.Contains(settlement3))
					{
						this._checkedNeighbors.Add(settlement3);
						if (settlement3.MapFaction != targetSettlement.MapFaction)
						{
							num3 += 1f;
							if (settlement3.MapFaction == mapFaction)
							{
								num4 += 1f;
							}
						}
					}
				}
			}
			bestDistanceScore = 0f + num2 / num * 1f + num4 / num3 * 0.25f;
			if (bestDistanceScore < 0.1f)
			{
				bestDistanceScore = 0f;
			}
			isTargetingPort = false;
			float num5;
			AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, targetSettlement, isTargetingPort, out bestNavigationType, out num5, out isFromPort);
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x0017C98C File Offset: 0x0017AB8C
		private void GetDistanceScoreForRaiding(Settlement targetSettlement, MobileParty mobileParty, out MobileParty.NavigationType bestNavigationType, out float bestDistanceScore, out bool isFromPort, out bool isTargetingPort)
		{
			float num;
			AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, targetSettlement, false, out bestNavigationType, out num, out isFromPort);
			isTargetingPort = false;
			if (mobileParty.HasNavalNavigationCapability && targetSettlement.HasPort)
			{
				MobileParty.NavigationType navigationType;
				float num2;
				bool flag;
				AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, targetSettlement, true, out navigationType, out num2, out flag);
				if (num2 < num)
				{
					bestNavigationType = navigationType;
					num = num2;
					isFromPort = flag;
					isTargetingPort = true;
				}
			}
			float num3 = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(mobileParty.NavigationCapability) * 5f;
			if (num > num3)
			{
				bestNavigationType = MobileParty.NavigationType.None;
				num = float.MaxValue;
				isTargetingPort = false;
				bestDistanceScore = 0f;
				return;
			}
			bestDistanceScore = MBMath.Map(0.75f - num / num3, 0f, 1f, 0.1f, 1f);
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x0017CA34 File Offset: 0x0017AC34
		private void GetDistanceScoreForDefending(Settlement targetSettlement, MobileParty mobileParty, out MobileParty.NavigationType bestNavigationType, out float bestDistanceScore, out bool isFromPort, out bool isTargetingPort)
		{
			isTargetingPort = false;
			bool flag = targetSettlement.HasPort && mobileParty.HasNavalNavigationCapability;
			bool flag2 = false;
			if (flag)
			{
				if (targetSettlement.IsFortification)
				{
					flag2 = targetSettlement.SiegeEvent != null && (!targetSettlement.SiegeEvent.IsBlockadeActive || (targetSettlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent != null && (targetSettlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsBlockade || targetSettlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsBlockadeSallyOut)));
				}
				else if (targetSettlement.IsVillage)
				{
					flag2 = targetSettlement.HasPort;
				}
			}
			if (flag2)
			{
				isTargetingPort = true;
			}
			float num;
			AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, targetSettlement, isTargetingPort, out bestNavigationType, out num, out isFromPort);
			if (!flag2 && flag)
			{
				AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, targetSettlement, true, out bestNavigationType, out num, out isFromPort);
			}
			float num2 = ((bestNavigationType == MobileParty.NavigationType.Naval) ? Campaign.Current.EstimatedAverageLordPartyNavalSpeed : Campaign.Current.EstimatedAverageLordPartySpeed);
			if (bestNavigationType == MobileParty.NavigationType.All)
			{
				num2 = (Campaign.Current.EstimatedAverageLordPartyNavalSpeed + Campaign.Current.EstimatedAverageLordPartySpeed) * 0.5f;
			}
			float num3 = num / (num2 * (float)CampaignTime.HoursInDay);
			float num4 = 4.01f;
			if (targetSettlement.IsVillage)
			{
				MapEvent mapEvent = targetSettlement.Party.MapEvent;
				RaidEventComponent raidEventComponent;
				if (mapEvent != null && (raidEventComponent = mapEvent.Component as RaidEventComponent) != null && raidEventComponent.RaidDamage > 0f)
				{
					float num5 = raidEventComponent.RaidDamage / mapEvent.BattleStartTime.ElapsedDaysUntilNow;
					num4 = targetSettlement.SettlementHitPoints / num5 * 0.25f;
				}
			}
			else if (targetSettlement.IsFortification && targetSettlement.Party.SiegeEvent != null)
			{
				num4 = 8.02f;
			}
			if (num3 >= num4)
			{
				bestNavigationType = MobileParty.NavigationType.None;
				bestDistanceScore = 0f;
				isTargetingPort = false;
			}
			else if (targetSettlement.Party.MapEventSide == null && targetSettlement.SiegeEvent != null && mobileParty.NavigationCapability == MobileParty.NavigationType.All)
			{
				bool flag3 = false;
				bool flag4 = mobileParty.DefaultBehavior == AiBehavior.DefendSettlement && mobileParty.ShortTermTargetParty != null && !mobileParty.ShortTermTargetParty.MapFaction.IsAtWarWith(mobileParty.MapFaction);
				if (flag4)
				{
					flag3 = !mobileParty.ShortTermTargetParty.IsCurrentlyAtSea;
					bestNavigationType = MobileParty.NavigationType.All;
				}
				else
				{
					bool flag5;
					MobileParty mobileParty2;
					MobileParty mobileParty3;
					mobileParty.Ai.GetNearbyPartyDataWhileDefendingSettlement(targetSettlement, out flag4, out flag3, out flag5, out mobileParty2, out mobileParty3);
				}
				isTargetingPort = !flag3 && targetSettlement.HasPort;
			}
			else if (targetSettlement.IsVillage && targetSettlement.Party.MapEvent != null && targetSettlement.Party.MapEvent.IsRaid)
			{
				bool flag5;
				MobileParty mobileParty2;
				MobileParty mobileParty3;
				bool flag6;
				bool flag7;
				mobileParty.Ai.GetNearbyPartyDataWhileDefendingSettlement(targetSettlement, out flag6, out flag7, out flag5, out mobileParty3, out mobileParty2);
				isTargetingPort = !flag7 && targetSettlement.HasPort;
			}
			bestDistanceScore = MBMath.Map(1f - num3 / (num4 + 0.01f), 0f, 1f, 0.1f, 1.25f);
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x0017CD10 File Offset: 0x0017AF10
		private void CalculateMilitaryBehaviorForSettlement(Settlement settlement, Army.ArmyTypes missionType, AiBehavior aiBehavior, PartyThinkParams p, float ourStrength, float partySizeScore, float cohesionScore, float foodScore)
		{
			if ((missionType == Army.ArmyTypes.Defender && settlement.LastAttackerParty != null && settlement.LastAttackerParty.IsActive) || (missionType == Army.ArmyTypes.Raider && settlement.IsVillage && settlement.Village.VillageState != Village.VillageStates.Looted) || (missionType == Army.ArmyTypes.Besieger && settlement.IsFortification && (settlement.SiegeEvent == null || settlement.SiegeEvent.BesiegerCamp.MapFaction == p.MobilePartyOf.MapFaction)))
			{
				MobileParty mobilePartyOf = p.MobilePartyOf;
				if ((missionType == Army.ArmyTypes.Raider && (settlement.Village.VillageState != Village.VillageStates.Normal || settlement.Party.MapEvent != null) && (mobilePartyOf.MapEvent == null || mobilePartyOf.MapEvent.MapEventSettlement != settlement)) || (missionType == Army.ArmyTypes.Besieger && (settlement.Party.MapEvent != null || settlement.SiegeEvent != null) && (settlement.SiegeEvent == null || settlement.SiegeEvent.BesiegerCamp.MapFaction != mobilePartyOf.MapFaction) && (mobilePartyOf.MapEvent == null || mobilePartyOf.MapEvent.MapEventSettlement != settlement)) || (missionType == Army.ArmyTypes.Defender && (settlement.LastAttackerParty == null || !settlement.LastAttackerParty.IsActive || !settlement.LastAttackerParty.MapFaction.IsAtWarWith(mobilePartyOf.MapFaction))))
				{
					return;
				}
				if (mobilePartyOf.Army == null && missionType == Army.ArmyTypes.Besieger && ((settlement.Party.MapEvent != null && settlement.Party.MapEvent.AttackerSide.LeaderParty != mobilePartyOf.Party) || (settlement.Party.SiegeEvent != null && mobilePartyOf.BesiegedSettlement != settlement)))
				{
					return;
				}
				MobileParty.NavigationType navigationType = MobileParty.NavigationType.None;
				float maxValue = float.MaxValue;
				bool flag = false;
				bool flag2 = false;
				switch (missionType)
				{
				case Army.ArmyTypes.Besieger:
					this.CalculateDistanceScoreForBesieging(settlement, mobilePartyOf, out navigationType, out maxValue, out flag2, out flag);
					break;
				case Army.ArmyTypes.Raider:
					this.GetDistanceScoreForRaiding(settlement, mobilePartyOf, out navigationType, out maxValue, out flag2, out flag);
					break;
				case Army.ArmyTypes.Defender:
					this.GetDistanceScoreForDefending(settlement, mobilePartyOf, out navigationType, out maxValue, out flag2, out flag);
					break;
				}
				if (maxValue > 0f)
				{
					if (mobilePartyOf.SiegeEvent != null && mobilePartyOf.BesiegerCamp != null && mobilePartyOf.SiegeEvent.BesiegedSettlement == settlement)
					{
						ourStrength = mobilePartyOf.BesiegerCamp.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege).Sum<PartyBase>((PartyBase x) => x.EstimatedStrength);
					}
					float num = Campaign.Current.Models.TargetScoreCalculatingModel.GetTargetScoreForFaction(settlement, missionType, mobilePartyOf, ourStrength);
					num *= maxValue * cohesionScore * partySizeScore * foodScore;
					if (!mobilePartyOf.IsDisbanding)
					{
						IDisbandPartyCampaignBehavior disbandPartyCampaignBehavior = this._disbandPartyCampaignBehavior;
						if (disbandPartyCampaignBehavior == null || !disbandPartyCampaignBehavior.IsPartyWaitingForDisband(mobilePartyOf))
						{
							goto IL_0284;
						}
					}
					num *= 0.25f;
					IL_0284:
					if (navigationType != MobileParty.NavigationType.None)
					{
						AIBehaviorData aibehaviorData = new AIBehaviorData(settlement, aiBehavior, navigationType, p.WillGatherAnArmy, flag2, flag);
						ValueTuple<AIBehaviorData, float> valueTuple = new ValueTuple<AIBehaviorData, float>(aibehaviorData, num);
						p.AddBehaviorScore(in valueTuple);
					}
				}
			}
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x0017CFCC File Offset: 0x0017B1CC
		private void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			if (mobileParty.IsMilitia || mobileParty.IsCaravan || mobileParty.IsVillager || mobileParty.IsBandit || mobileParty.IsPatrolParty || mobileParty.IsDisbanding || mobileParty.LeaderHero == null || (mobileParty.MapFaction != Clan.PlayerClan.MapFaction && !mobileParty.MapFaction.IsKingdomFaction))
			{
				return;
			}
			Settlement currentSettlement = mobileParty.CurrentSettlement;
			if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) != null)
			{
				return;
			}
			if (mobileParty.Army != null)
			{
				mobileParty.Ai.SetInitiative(0.33f, 0.33f, 24f);
				if (mobileParty.Army.LeaderParty == mobileParty && mobileParty.Army.LeaderParty.Army.IsWaitingForArmyMembers())
				{
					mobileParty.Ai.SetInitiative(0.33f, 1f, 24f);
					p.DoNotChangeBehavior = true;
				}
				else if (mobileParty.Army.LeaderParty.DefaultBehavior == AiBehavior.PatrolAroundPoint)
				{
					mobileParty.Ai.SetInitiative(1f, 1f, 24f);
				}
				else if (mobileParty.Army.LeaderParty.DefaultBehavior == AiBehavior.DefendSettlement && mobileParty.Army.LeaderParty == mobileParty && mobileParty.Army.AiBehaviorObject != null && mobileParty.Army.AiBehaviorObject is Settlement && ((Settlement)mobileParty.Army.AiBehaviorObject).Position.DistanceSquared(mobileParty.Position) < Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(mobileParty.NavigationCapability) * 1.53f)
				{
					mobileParty.Ai.SetInitiative(1f, 1f, 24f);
				}
				if (mobileParty.Army.LeaderParty != mobileParty)
				{
					return;
				}
			}
			else if (mobileParty.DefaultBehavior == AiBehavior.DefendSettlement)
			{
				mobileParty.Ai.SetInitiative(0.33f, 1f, 2f);
			}
			float totalLandStrengthWithFollowers = mobileParty.GetTotalLandStrengthWithFollowers(true);
			p.Initialization();
			bool flag = false;
			float num = totalLandStrengthWithFollowers;
			MBList<MobileParty> mblist;
			if (mobileParty.LeaderHero != null && mobileParty.Army == null && mobileParty.LeaderHero.Clan != null && mobileParty.MapFaction is Kingdom && mobileParty.MapEvent == null && Campaign.Current.Models.ArmyManagementCalculationModel.CanLordCreateArmy(mobileParty, out mblist))
			{
				p.SetArmyMembers(mblist);
				foreach (MobileParty mobileParty2 in mblist)
				{
					num += mobileParty2.Party.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.Siege);
				}
				flag = true;
			}
			int i = 0;
			while (i < 4)
			{
				Army.ArmyTypes armyTypes = (Army.ArmyTypes)i;
				if (armyTypes != Army.ArmyTypes.Raider)
				{
					goto IL_02A9;
				}
				Hero leaderHero = mobileParty.LeaderHero;
				if (leaderHero == null || leaderHero.CanRaid)
				{
					goto IL_02A9;
				}
				IL_02D2:
				i++;
				continue;
				IL_02A9:
				if (flag && armyTypes == Army.ArmyTypes.Besieger)
				{
					p.WillGatherAnArmy = true;
					this.FindBestTargetAndItsValueForFaction(armyTypes, p, num);
				}
				p.WillGatherAnArmy = false;
				this.FindBestTargetAndItsValueForFaction(armyTypes, p, totalLandStrengthWithFollowers);
				goto IL_02D2;
			}
		}

		// Token: 0x040014E6 RID: 5350
		private const float MeaningfulCohesionThresholdForArmy = 40f;

		// Token: 0x040014E7 RID: 5351
		private const float MinimumCohesionScoreThreshold = 0.25f;

		// Token: 0x040014E8 RID: 5352
		private const float AverageSiegeDurationAsDays = 8.02f;

		// Token: 0x040014E9 RID: 5353
		private IDisbandPartyCampaignBehavior _disbandPartyCampaignBehavior;

		// Token: 0x040014EA RID: 5354
		private readonly HashSet<Settlement> _checkedNeighbors = new HashSet<Settlement>();
	}
}
