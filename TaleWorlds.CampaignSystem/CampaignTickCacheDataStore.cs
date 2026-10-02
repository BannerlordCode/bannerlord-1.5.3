using System;
using System.Collections.Generic;
using System.Threading;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BattleWreckages;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200007F RID: 127
	public class CampaignTickCacheDataStore
	{
		// Token: 0x060010D1 RID: 4305 RVA: 0x00050A0C File Offset: 0x0004EC0C
		internal CampaignTickCacheDataStore()
		{
			this._mobilePartyComparer = new CampaignTickCacheDataStore.MobilePartyComparer();
			this._parallelInitializeCachedPartyVariablesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelInitializeCachedPartyVariables);
			this._parallelCacheTargetPartyVariablesAtFrameStartPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelCacheTargetPartyVariablesAtFrameStart);
			this._parallelArrangePartyIndicesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelArrangePartyIndices);
			this._parallelTickMovingArmiesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelTickMovingArmies);
			this._parallelTickMovingPartiesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelTickMovingParties);
			this._parallelTickStationaryArmyLeaderPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelTickStationaryArmyLeaderParties);
			this._parallelTickStationaryPartiesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelTickStationaryParties);
			this._parallelCheckExitingSettlementsPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelCheckExitingSettlements);
			this._parallelTickTransitioningArmiesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelTickTransitioningArmyLeaders);
			this._parallelTickTransitioningPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelTickTransitioningParties);
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00050AF4 File Offset: 0x0004ECF4
		internal void ValidateMobilePartyTickDataCache(int currentTotalMobilePartyCount)
		{
			if (this._currentTotalMobilePartyCapacity <= currentTotalMobilePartyCount)
			{
				this.InitializeCacheArrays();
			}
			this._currentFrameMovingPartyCount = -1;
			this._currentFrameStationaryPartyCount = -1;
			this._currentFrameMovingArmyLeaderCount = -1;
			this._gridChangeCount = -1;
			this._exitingSettlementCount = -1;
			this._currentFrameStationaryArmyLeaderCount = -1;
			this._navigationTransitionedCount = -1;
			this._currentFrameTransitioningArmyLeaderCount = -1;
			this._currentFrameTransitioningCount = -1;
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00050B50 File Offset: 0x0004ED50
		private void InitializeCacheArrays()
		{
			int num = (int)((float)this._currentTotalMobilePartyCapacity * 2f);
			this._cacheData = new CampaignTickCacheDataStore.PartyTickCachePerParty[num];
			this._gridChangeMobilePartyList = new MobileParty[num];
			this._exitingSettlementMobilePartyList = new MobileParty[num];
			this._currentTotalMobilePartyCapacity = num;
			this._navigationTransitionedMobilePartyList = new MobileParty[num];
			this._movingPartyIndices = new int[num];
			this._stationaryPartyIndices = new int[num];
			this._transitioningArmyLeaderPartyIndices = new int[num];
			this._transitioningPartyIndices = new int[num];
			this._movingArmyLeaderPartyIndices = new int[num];
			this._stationaryArmyLeaderPartyIndices = new int[num];
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00050BEC File Offset: 0x0004EDEC
		internal void InitializeDataCache()
		{
			this._currentFrameMovingArmyLeaderCount = Campaign.Current.MobileParties.Count;
			this._currentTotalMobilePartyCapacity = Campaign.Current.MobileParties.Count;
			this._currentFrameStationaryPartyCount = Campaign.Current.MobileParties.Count;
			this.InitializeCacheArrays();
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00050C40 File Offset: 0x0004EE40
		private void ParallelTickTransitioningArmyLeaders(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				int num = this._transitioningArmyLeaderPartyIndices[i];
				MobileParty.CachedPartyVariables localVariables = this._cacheData[num].LocalVariables;
				Campaign.Current.MobileParties[num].FillCurrentTickMoveDataForMovingArmyLeader(ref localVariables, this._currentDt, this._currentRealDt);
				Campaign.Current.MobileParties[num].CommonTransitioningPartyTick(ref localVariables, ref this._navigationTransitionedCount, ref this._navigationTransitionedMobilePartyList, this._currentDt);
			}
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00050CC0 File Offset: 0x0004EEC0
		private void ParallelTickTransitioningParties(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				int num = this._transitioningPartyIndices[i];
				MobileParty.CachedPartyVariables localVariables = this._cacheData[num].LocalVariables;
				Campaign.Current.MobileParties[num].FillCurrentTickMoveDataForMovingMobileParty(ref localVariables, this._currentDt, this._currentRealDt);
				Campaign.Current.MobileParties[num].CommonTransitioningPartyTick(ref localVariables, ref this._navigationTransitionedCount, ref this._navigationTransitionedMobilePartyList, this._currentDt);
			}
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00050D40 File Offset: 0x0004EF40
		private void ParallelCheckExitingSettlements(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				Campaign.Current.MobileParties[i].CheckExitingSettlementParallel(ref this._exitingSettlementCount, ref this._exitingSettlementMobilePartyList, ref this._gridChangeCount, ref this._gridChangeMobilePartyList);
			}
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00050D88 File Offset: 0x0004EF88
		private void ParallelInitializeCachedPartyVariables(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				MobileParty mobileParty = Campaign.Current.MobileParties[i];
				this._cacheData[i].MobileParty = mobileParty;
				mobileParty.InitializeCachedPartyVariables(ref this._cacheData[i].LocalVariables);
			}
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00050DDC File Offset: 0x0004EFDC
		private void ParallelCacheTargetPartyVariablesAtFrameStart(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this._cacheData[i].MobileParty.CacheTargetPartyVariablesAtFrameStart(ref this._cacheData[i].LocalVariables);
			}
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00050E1C File Offset: 0x0004F01C
		private void ParallelArrangePartyIndices(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				MobileParty mobileParty = this._cacheData[i].MobileParty;
				MobileParty.CachedPartyVariables localVariables = this._cacheData[i].LocalVariables;
				if (mobileParty.IsActive)
				{
					if (localVariables.IsMoving)
					{
						if (localVariables.IsArmyLeader)
						{
							int num = Interlocked.Increment(ref this._currentFrameMovingArmyLeaderCount);
							this._movingArmyLeaderPartyIndices[num] = i;
						}
						else
						{
							int num2 = Interlocked.Increment(ref this._currentFrameMovingPartyCount);
							this._movingPartyIndices[num2] = i;
						}
					}
					else if (localVariables.IsArmyLeader)
					{
						if (localVariables.IsTransitionInProgress)
						{
							int num3 = Interlocked.Increment(ref this._currentFrameTransitioningArmyLeaderCount);
							this._transitioningArmyLeaderPartyIndices[num3] = i;
						}
						else
						{
							int num4 = Interlocked.Increment(ref this._currentFrameStationaryArmyLeaderCount);
							this._stationaryArmyLeaderPartyIndices[num4] = i;
						}
					}
					else if (localVariables.IsTransitionInProgress && !localVariables.IsAttachedArmyMember)
					{
						int num5 = Interlocked.Increment(ref this._currentFrameTransitioningCount);
						this._transitioningPartyIndices[num5] = i;
					}
					else
					{
						int num6 = Interlocked.Increment(ref this._currentFrameStationaryPartyCount);
						this._stationaryPartyIndices[num6] = i;
					}
				}
			}
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00050F30 File Offset: 0x0004F130
		private void ParallelTickMovingArmies(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				int num = this._movingArmyLeaderPartyIndices[i];
				CampaignTickCacheDataStore.PartyTickCachePerParty partyTickCachePerParty = this._cacheData[num];
				MobileParty mobileParty = partyTickCachePerParty.MobileParty;
				MobileParty.CachedPartyVariables localVariables = partyTickCachePerParty.LocalVariables;
				mobileParty.FillCurrentTickMoveDataForMovingArmyLeader(ref localVariables, this._currentDt, this._currentRealDt);
				mobileParty.TryToMoveThePartyWithCurrentTickMoveData(ref localVariables, ref this._gridChangeCount, ref this._gridChangeMobilePartyList);
				this._cacheData[num].LocalVariables = localVariables;
				mobileParty.ValidateSpeed();
			}
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00050FAC File Offset: 0x0004F1AC
		private void ParallelTickMovingParties(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				int num = this._movingPartyIndices[i];
				CampaignTickCacheDataStore.PartyTickCachePerParty partyTickCachePerParty = this._cacheData[num];
				MobileParty mobileParty = partyTickCachePerParty.MobileParty;
				MobileParty.CachedPartyVariables localVariables = partyTickCachePerParty.LocalVariables;
				mobileParty.FillCurrentTickMoveDataForMovingMobileParty(ref localVariables, this._currentDt, this._currentRealDt);
				mobileParty.TryToMoveThePartyWithCurrentTickMoveData(ref localVariables, ref this._gridChangeCount, ref this._gridChangeMobilePartyList);
				this._cacheData[num].LocalVariables = localVariables;
			}
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00051024 File Offset: 0x0004F224
		private void ParallelTickStationaryParties(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				int num = this._stationaryPartyIndices[i];
				CampaignTickCacheDataStore.PartyTickCachePerParty partyTickCachePerParty = this._cacheData[num];
				MobileParty mobileParty = partyTickCachePerParty.MobileParty;
				MobileParty.CachedPartyVariables localVariables = partyTickCachePerParty.LocalVariables;
				mobileParty.TickForStationaryMobileParty(ref localVariables, this._currentDt, this._currentRealDt);
				this._cacheData[num].LocalVariables = localVariables;
			}
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00051088 File Offset: 0x0004F288
		private void ParallelTickStationaryArmyLeaderParties(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				int num = this._stationaryArmyLeaderPartyIndices[i];
				CampaignTickCacheDataStore.PartyTickCachePerParty partyTickCachePerParty = this._cacheData[num];
				MobileParty mobileParty = partyTickCachePerParty.MobileParty;
				MobileParty.CachedPartyVariables localVariables = partyTickCachePerParty.LocalVariables;
				mobileParty.TickForStationaryMobileParty(ref localVariables, this._currentDt, this._currentRealDt);
				this._cacheData[num].LocalVariables = localVariables;
			}
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x000510EC File Offset: 0x0004F2EC
		internal void Tick()
		{
			TWParallel.For(0, Campaign.Current.MobileParties.Count, this._parallelCheckExitingSettlementsPredicate, 16);
			Array.Sort<MobileParty>(this._exitingSettlementMobilePartyList, 0, this._exitingSettlementCount + 1, this._mobilePartyComparer);
			for (int i = 0; i < this._exitingSettlementCount + 1; i++)
			{
				LeaveSettlementAction.ApplyForParty(this._exitingSettlementMobilePartyList[i]);
			}
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00051150 File Offset: 0x0004F350
		internal void RealTick(float dt, float realDt)
		{
			this._currentDt = dt;
			this._currentRealDt = realDt;
			this.ValidateMobilePartyTickDataCache(Campaign.Current.MobileParties.Count);
			int count = Campaign.Current.MobileParties.Count;
			TWParallel.For(0, count, this._parallelInitializeCachedPartyVariablesPredicate, 16);
			TWParallel.For(0, count, this._parallelCacheTargetPartyVariablesAtFrameStartPredicate, 16);
			TWParallel.For(0, count, this._parallelArrangePartyIndicesPredicate, 16);
			TWParallel.For(0, this._currentFrameMovingArmyLeaderCount + 1, this._parallelTickMovingArmiesPredicate, 16);
			TWParallel.For(0, this._currentFrameTransitioningArmyLeaderCount + 1, this._parallelTickTransitioningArmiesPredicate, 16);
			TWParallel.For(0, this._currentFrameMovingPartyCount + 1, this._parallelTickMovingPartiesPredicate, 16);
			TWParallel.For(0, this._currentFrameTransitioningCount + 1, this._parallelTickTransitioningPredicate, 16);
			TWParallel.For(0, this._currentFrameStationaryArmyLeaderCount + 1, this._parallelTickStationaryArmyLeaderPredicate, 16);
			TWParallel.For(0, this._currentFrameStationaryPartyCount + 1, this._parallelTickStationaryPartiesPredicate, 16);
			this.UpdateVisibilitiesAroundMainParty();
			Array.Sort<MobileParty>(this._gridChangeMobilePartyList, 0, this._gridChangeCount + 1, this._mobilePartyComparer);
			Campaign campaign = Campaign.Current;
			for (int i = 0; i < this._gridChangeCount + 1; i++)
			{
				campaign.MobilePartyLocator.UpdateLocator(this._gridChangeMobilePartyList[i]);
			}
			Array.Sort<MobileParty>(this._navigationTransitionedMobilePartyList, 0, this._navigationTransitionedCount + 1, this._mobilePartyComparer);
			for (int j = 0; j < this._navigationTransitionedCount + 1; j++)
			{
				this._navigationTransitionedMobilePartyList[j].FinishNavigationTransitionInternal();
			}
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x000512C8 File Offset: 0x0004F4C8
		private void UpdateVisibilitiesAroundMainParty()
		{
			if (MobileParty.MainParty.Position.IsValid())
			{
				if (MobileParty.MainParty.SiegeEvent != null && MobileParty.MainParty.SiegeEvent.BesiegedSettlement.HasPort)
				{
					this.UpdateVisibilitiesBasedOnPoint(new Vec2[]
					{
						MobileParty.MainParty.SiegeEvent.BesiegedSettlement.GatePosition.ToVec2(),
						MobileParty.MainParty.SiegeEvent.BesiegedSettlement.PortPosition.ToVec2(),
						MobileParty.MainParty.SiegeEvent.BesiegerCamp.LeaderParty.Position.ToVec2()
					});
				}
				else
				{
					MapEvent mapEvent = MobileParty.MainParty.MapEvent;
					if (mapEvent != null && mapEvent.IsRaid && MobileParty.MainParty.MapEvent.MapEventSettlement.HasPort)
					{
						this.UpdateVisibilitiesBasedOnPoint(new Vec2[]
						{
							MobileParty.MainParty.MapEvent.MapEventSettlement.GatePosition.ToVec2(),
							MobileParty.MainParty.MapEvent.MapEventSettlement.PortPosition.ToVec2()
						});
					}
					else
					{
						this.UpdateVisibilitiesBasedOnPoint(new Vec2[] { MobileParty.MainParty.Position.ToVec2() });
					}
				}
				foreach (BattleWreckage battleWreckage in Campaign.Current.Wreckages)
				{
					battleWreckage.UpdateVisibility();
				}
			}
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00051480 File Offset: 0x0004F680
		private void UpdateVisibilitiesBasedOnPoint(params Vec2[] points)
		{
			float seeingRange = MobileParty.MainParty.SeeingRange;
			Vec2 vec = Vec2.Zero;
			for (int i = 0; i < points.Length; i++)
			{
				vec += points[i];
			}
			vec /= (float)points.Length;
			float num = 0f;
			for (int j = 0; j < points.Length; j++)
			{
				float num2 = vec.Distance(points[j]);
				if (num2 > num)
				{
					num = num2;
				}
			}
			float num3 = num + seeingRange;
			LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(vec, num3);
			for (MobileParty mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData); mobileParty != null; mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData))
			{
				bool flag;
				bool flag2;
				bool flag3;
				Campaign.Current.Models.MapVisibilityModel.GetMobilePartyVisibilityAndInspectedState(mobileParty, points, seeingRange, out flag, out flag2, out flag3);
				mobileParty.IsVisible = flag;
				mobileParty.IsInspected = flag2;
				if (flag3)
				{
					this._currentFrameUpdatedPartyBases.Add(mobileParty.Party);
					this._previousFrameUpdatedPartyBases.Remove(mobileParty.Party);
				}
			}
			LocatableSearchData<Settlement> locatableSearchData2 = Settlement.StartFindingLocatablesAroundPosition(vec, num3);
			for (Settlement settlement = Settlement.FindNextLocatable(ref locatableSearchData2); settlement != null; settlement = Settlement.FindNextLocatable(ref locatableSearchData2))
			{
				bool flag4;
				bool flag5;
				Campaign.Current.Models.MapVisibilityModel.GetSettlementInspectedState(settlement, points, seeingRange, out flag4, out flag5);
				settlement.IsInspected = flag4;
				if (flag5)
				{
					this._currentFrameUpdatedPartyBases.Add(settlement.Party);
					this._previousFrameUpdatedPartyBases.Remove(settlement.Party);
				}
			}
			foreach (PartyBase partyBase in this._previousFrameUpdatedPartyBases)
			{
				if (partyBase.IsSettlement)
				{
					partyBase.Settlement.IsInspected = false;
				}
				else
				{
					partyBase.MobileParty.IsVisible = false;
					partyBase.MobileParty.IsInspected = false;
				}
			}
			List<PartyBase> previousFrameUpdatedPartyBases = this._previousFrameUpdatedPartyBases;
			this._previousFrameUpdatedPartyBases = this._currentFrameUpdatedPartyBases;
			this._currentFrameUpdatedPartyBases = previousFrameUpdatedPartyBases;
			this._currentFrameUpdatedPartyBases.Clear();
		}

		// Token: 0x04000498 RID: 1176
		private CampaignTickCacheDataStore.PartyTickCachePerParty[] _cacheData;

		// Token: 0x04000499 RID: 1177
		private MobileParty[] _gridChangeMobilePartyList;

		// Token: 0x0400049A RID: 1178
		private MobileParty[] _exitingSettlementMobilePartyList;

		// Token: 0x0400049B RID: 1179
		private MobileParty[] _navigationTransitionedMobilePartyList;

		// Token: 0x0400049C RID: 1180
		private List<PartyBase> _previousFrameUpdatedPartyBases = new List<PartyBase>();

		// Token: 0x0400049D RID: 1181
		private List<PartyBase> _currentFrameUpdatedPartyBases = new List<PartyBase>();

		// Token: 0x0400049E RID: 1182
		private int[] _movingPartyIndices;

		// Token: 0x0400049F RID: 1183
		private int _currentFrameMovingPartyCount;

		// Token: 0x040004A0 RID: 1184
		private int[] _stationaryPartyIndices;

		// Token: 0x040004A1 RID: 1185
		private int _currentFrameStationaryPartyCount;

		// Token: 0x040004A2 RID: 1186
		private int[] _transitioningArmyLeaderPartyIndices;

		// Token: 0x040004A3 RID: 1187
		private int _currentFrameTransitioningArmyLeaderCount;

		// Token: 0x040004A4 RID: 1188
		private int[] _transitioningPartyIndices;

		// Token: 0x040004A5 RID: 1189
		private int _currentFrameTransitioningCount;

		// Token: 0x040004A6 RID: 1190
		private int[] _movingArmyLeaderPartyIndices;

		// Token: 0x040004A7 RID: 1191
		private int _currentFrameMovingArmyLeaderCount;

		// Token: 0x040004A8 RID: 1192
		private int[] _stationaryArmyLeaderPartyIndices;

		// Token: 0x040004A9 RID: 1193
		private int _currentFrameStationaryArmyLeaderCount;

		// Token: 0x040004AA RID: 1194
		private int _currentTotalMobilePartyCapacity;

		// Token: 0x040004AB RID: 1195
		private int _gridChangeCount;

		// Token: 0x040004AC RID: 1196
		private int _exitingSettlementCount;

		// Token: 0x040004AD RID: 1197
		private int _navigationTransitionedCount;

		// Token: 0x040004AE RID: 1198
		private float _currentDt;

		// Token: 0x040004AF RID: 1199
		private float _currentRealDt;

		// Token: 0x040004B0 RID: 1200
		private readonly TWParallel.ParallelForAuxPredicate _parallelInitializeCachedPartyVariablesPredicate;

		// Token: 0x040004B1 RID: 1201
		private readonly TWParallel.ParallelForAuxPredicate _parallelCacheTargetPartyVariablesAtFrameStartPredicate;

		// Token: 0x040004B2 RID: 1202
		private readonly TWParallel.ParallelForAuxPredicate _parallelArrangePartyIndicesPredicate;

		// Token: 0x040004B3 RID: 1203
		private readonly TWParallel.ParallelForAuxPredicate _parallelTickMovingArmiesPredicate;

		// Token: 0x040004B4 RID: 1204
		private readonly TWParallel.ParallelForAuxPredicate _parallelTickTransitioningArmiesPredicate;

		// Token: 0x040004B5 RID: 1205
		private readonly TWParallel.ParallelForAuxPredicate _parallelTickTransitioningPredicate;

		// Token: 0x040004B6 RID: 1206
		private readonly TWParallel.ParallelForAuxPredicate _parallelTickMovingPartiesPredicate;

		// Token: 0x040004B7 RID: 1207
		private readonly TWParallel.ParallelForAuxPredicate _parallelTickStationaryPartiesPredicate;

		// Token: 0x040004B8 RID: 1208
		private readonly TWParallel.ParallelForAuxPredicate _parallelCheckExitingSettlementsPredicate;

		// Token: 0x040004B9 RID: 1209
		private readonly TWParallel.ParallelForAuxPredicate _parallelTickStationaryArmyLeaderPredicate;

		// Token: 0x040004BA RID: 1210
		private readonly CampaignTickCacheDataStore.MobilePartyComparer _mobilePartyComparer;

		// Token: 0x02000569 RID: 1385
		private struct PartyTickCachePerParty
		{
			// Token: 0x040017A5 RID: 6053
			internal MobileParty MobileParty;

			// Token: 0x040017A6 RID: 6054
			internal MobileParty.CachedPartyVariables LocalVariables;
		}

		// Token: 0x0200056A RID: 1386
		private class MobilePartyComparer : IComparer<MobileParty>
		{
			// Token: 0x06005004 RID: 20484 RVA: 0x0018E4CC File Offset: 0x0018C6CC
			public int Compare(MobileParty x, MobileParty y)
			{
				return x.Id.InternalValue.CompareTo(y.Id.InternalValue);
			}
		}
	}
}
