using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map.DistanceCache
{
	// Token: 0x02000232 RID: 562
	public class SandBoxNavigationCache : NavigationCache<Settlement>, MapDistanceModel.INavigationCache
	{
		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x0009634B File Offset: 0x0009454B
		private IMapScene MapSceneWrapper
		{
			get
			{
				return Campaign.Current.MapSceneWrapper;
			}
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x00096358 File Offset: 0x00094558
		public SandBoxNavigationCache(MobileParty.NavigationType navigationType)
			: base(navigationType)
		{
			this._excludedFaceIds = Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(base._navigationType);
			this._regionSwitchCostTo0 = Campaign.Current.Models.MapDistanceModel.RegionSwitchCostFromLandToSea;
			this._regionSwitchCostTo1 = Campaign.Current.Models.MapDistanceModel.RegionSwitchCostFromSeaToLand;
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000963C0 File Offset: 0x000945C0
		protected override Settlement GetCacheElement(string settlementId)
		{
			return Settlement.Find(settlementId);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000963C8 File Offset: 0x000945C8
		protected override NavigationCacheElement<Settlement> GetCacheElement(Settlement settlement, bool isPortUsed)
		{
			return new NavigationCacheElement<Settlement>(settlement, isPortUsed);
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000963D4 File Offset: 0x000945D4
		float MapDistanceModel.INavigationCache.GetSettlementToSettlementDistanceWithLandRatio(Settlement settlement1, bool isAtSea1, Settlement settlement2, bool isAtSea2, out float landRatio)
		{
			NavigationCacheElement<Settlement> cacheElement = this.GetCacheElement(settlement1, isAtSea1);
			NavigationCacheElement<Settlement> cacheElement2 = this.GetCacheElement(settlement2, isAtSea2);
			return base.GetSettlementToSettlementDistanceWithLandRatio(cacheElement, cacheElement2, out landRatio);
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000963FE File Offset: 0x000945FE
		public override void GetSceneXmlCrcValues(out uint sceneXmlCrc, out uint sceneNavigationMeshCrc)
		{
			sceneXmlCrc = this.MapSceneWrapper.GetSceneXmlCrc();
			sceneNavigationMeshCrc = this.MapSceneWrapper.GetSceneNavigationMeshCrc();
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x0009641A File Offset: 0x0009461A
		protected override int GetNavMeshFaceCount()
		{
			return this.MapSceneWrapper.GetNumberOfNavigationMeshFaces();
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x00096427 File Offset: 0x00094627
		protected override Vec2 GetNavMeshFaceCenterPosition(int faceIndex)
		{
			return this.MapSceneWrapper.GetNavigationMeshCenterPosition(faceIndex);
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x00096435 File Offset: 0x00094635
		protected override PathFaceRecord GetFaceRecordAtIndex(int faceIndex)
		{
			return this.MapSceneWrapper.GetFaceAtIndex(faceIndex);
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x00096443 File Offset: 0x00094643
		protected override int GetRegionSwitchCostTo0()
		{
			return this._regionSwitchCostTo0;
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x0009644B File Offset: 0x0009464B
		protected override int GetRegionSwitchCostTo1()
		{
			return this._regionSwitchCostTo1;
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x00096453 File Offset: 0x00094653
		protected override int[] GetExcludedFaceIds()
		{
			return this._excludedFaceIds;
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x0009645C File Offset: 0x0009465C
		protected override float GetRealDistanceAndLandRatioBetweenSettlements(NavigationCacheElement<Settlement> settlement1, NavigationCacheElement<Settlement> settlement2, out float landRatio)
		{
			landRatio = 1f;
			float num = (float)Campaign.PathFindingMaxCostLimit;
			CampaignVec2 campaignVec = (settlement1.IsPortUsed ? settlement1.PortPosition : settlement1.GatePosition);
			CampaignVec2 campaignVec2 = (settlement2.IsPortUsed ? settlement2.PortPosition : settlement2.GatePosition);
			NavigationPath navigationPath = new NavigationPath();
			Campaign.Current.MapSceneWrapper.GetPathBetweenAIFaces(campaignVec.Face, campaignVec2.Face, campaignVec.ToVec2(), campaignVec2.ToVec2(), 0.3f, navigationPath, this.GetExcludedFaceIds(), 1f, this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1());
			float num2;
			Campaign.Current.MapSceneWrapper.GetPathDistanceBetweenAIFaces(campaignVec.Face, campaignVec2.Face, campaignVec.ToVec2(), campaignVec2.ToVec2(), 0.3f, num, out num2, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1());
			float num3;
			Campaign.Current.MapSceneWrapper.GetPathDistanceBetweenAIFaces(campaignVec2.Face, campaignVec.Face, campaignVec2.ToVec2(), campaignVec.ToVec2(), 0.3f, num, out num3, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1());
			float num4 = (num2 + num3) * 0.5f;
			if (num4 > 0f)
			{
				if (base._navigationType == MobileParty.NavigationType.Naval)
				{
					landRatio = 0f;
				}
				else if (base._navigationType == MobileParty.NavigationType.All)
				{
					landRatio = base.GetLandRatioOfPath(navigationPath, campaignVec.ToVec2());
				}
				bool flag;
				NavigationCacheElement<Settlement>.Sort(ref settlement1, ref settlement2, out flag);
				return num4;
			}
			return 0f;
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000965DC File Offset: 0x000947DC
		protected override void GetFaceRecordForPoint(Vec2 position, out bool isOnRegion1)
		{
			isOnRegion1 = true;
			IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
			CampaignVec2 campaignVec = new CampaignVec2(position, true);
			PathFaceRecord pathFaceRecord = mapSceneWrapper.GetFaceIndex(in campaignVec);
			if (!pathFaceRecord.IsValid())
			{
				isOnRegion1 = false;
				IMapScene mapSceneWrapper2 = Campaign.Current.MapSceneWrapper;
				campaignVec = new CampaignVec2(position, false);
				pathFaceRecord = mapSceneWrapper2.GetFaceIndex(in campaignVec);
			}
			if (!pathFaceRecord.IsValid())
			{
				Debug.Print(string.Format("{0} has no region data.", position), 0, Debug.DebugColor.Red, 17592186044416UL);
			}
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00096658 File Offset: 0x00094858
		protected override bool CheckBeingNeighbor(List<Settlement> settlementsToConsider, Settlement settlement1, Settlement settlement2, bool useGate1, bool useGate2, out float distance)
		{
			CampaignVec2 campaignVec = (useGate1 ? settlement1.GatePosition : settlement1.PortPosition);
			CampaignVec2 campaignVec2 = (useGate2 ? settlement2.GatePosition : settlement2.PortPosition);
			PathFaceRecord faceIndex = this.MapSceneWrapper.GetFaceIndex(in campaignVec);
			PathFaceRecord faceIndex2 = this.MapSceneWrapper.GetFaceIndex(in campaignVec2);
			if (!faceIndex.IsValid() || !faceIndex2.IsValid())
			{
				Debug.FailedAssert("Settlement navFace index should not be -1, check here", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Map\\DistanceCache\\SandboxNavigationCache.cs", "CheckBeingNeighbor", 193);
			}
			NavigationPath navigationPath = new NavigationPath();
			this.MapSceneWrapper.GetPathBetweenAIFaces(faceIndex, faceIndex2, campaignVec.ToVec2(), campaignVec2.ToVec2(), 0.3f, navigationPath, this.GetExcludedFaceIds(), 2f, this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1());
			bool flag = navigationPath.Size > 0 || faceIndex.FaceIndex == faceIndex2.FaceIndex;
			bool flag2 = useGate1;
			if (!this.MapSceneWrapper.GetPathDistanceBetweenAIFaces(faceIndex, faceIndex2, campaignVec.ToVec2(), campaignVec2.ToVec2(), 0.3f, Campaign.MapDiagonalSquared, out distance, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1()))
			{
				distance = Campaign.MapDiagonalSquared;
			}
			int num = 0;
			while (num < navigationPath.Size && flag)
			{
				Vec2 vec = navigationPath[num] - ((num == 0) ? campaignVec.ToVec2() : navigationPath[num - 1]);
				float num2 = vec.Length / 1f;
				vec.Normalize();
				int num3 = 0;
				while ((float)num3 < num2)
				{
					Vec2 vec2 = ((num == 0) ? campaignVec.ToVec2() : navigationPath[num - 1]) + vec * 1f * (float)num3;
					if (vec2 != campaignVec.ToVec2() && vec2 != campaignVec2.ToVec2())
					{
						CampaignVec2 campaignVec3 = new CampaignVec2(vec2, flag2);
						PathFaceRecord pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec3);
						if (pathFaceRecord.FaceIndex == -1)
						{
							flag2 = !flag2;
							campaignVec3 = new CampaignVec2(vec2, flag2);
							pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec3);
						}
						bool flag3;
						float realPathDistanceFromPositionToSettlement = this.GetRealPathDistanceFromPositionToSettlement(vec2, pathFaceRecord, distance, settlement1, out flag3);
						float realPathDistanceFromPositionToSettlement2 = this.GetRealPathDistanceFromPositionToSettlement(vec2, pathFaceRecord, distance, settlement2, out flag3);
						float num4 = ((realPathDistanceFromPositionToSettlement < realPathDistanceFromPositionToSettlement2) ? realPathDistanceFromPositionToSettlement : realPathDistanceFromPositionToSettlement2);
						if (pathFaceRecord.FaceIndex != -1)
						{
							Settlement closestSettlementToPosition = base.GetClosestSettlementToPosition(vec2, pathFaceRecord, this.GetExcludedFaceIds(), settlementsToConsider, this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1(), num4 * 0.8f, out flag3, false);
							if (closestSettlementToPosition != null && closestSettlementToPosition != settlement1 && closestSettlementToPosition != settlement2)
							{
								flag = false;
								break;
							}
						}
					}
					num3++;
				}
				num++;
			}
			return flag;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00096908 File Offset: 0x00094B08
		protected override float GetRealPathDistanceFromPositionToSettlement(Vec2 checkPosition, PathFaceRecord currentFaceRecord, float maxDistanceToLookForPathDetection, Settlement currentSettlementToLook, out bool isPort)
		{
			float num = float.MaxValue;
			isPort = false;
			switch (base._navigationType)
			{
			case MobileParty.NavigationType.Default:
			{
				CampaignVec2 campaignVec = currentSettlementToLook.GatePosition;
				PathFaceRecord pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec);
				float num2;
				if (this.MapSceneWrapper.GetPathDistanceBetweenAIFaces(currentFaceRecord, pathFaceRecord, checkPosition, currentSettlementToLook.GatePosition.ToVec2(), 0.3f, maxDistanceToLookForPathDetection, out num2, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1()))
				{
					num = num2;
				}
				break;
			}
			case MobileParty.NavigationType.Naval:
			{
				CampaignVec2 campaignVec = currentSettlementToLook.PortPosition;
				PathFaceRecord pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec);
				float num3;
				if (this.MapSceneWrapper.GetPathDistanceBetweenAIFaces(currentFaceRecord, pathFaceRecord, checkPosition, currentSettlementToLook.PortPosition.ToVec2(), 0.3f, maxDistanceToLookForPathDetection, out num3, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1()))
				{
					num = num3;
					isPort = true;
				}
				break;
			}
			case MobileParty.NavigationType.All:
			{
				CampaignVec2 campaignVec = currentSettlementToLook.GatePosition;
				PathFaceRecord pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec);
				float num4;
				if (this.MapSceneWrapper.GetPathDistanceBetweenAIFaces(currentFaceRecord, pathFaceRecord, checkPosition, currentSettlementToLook.GatePosition.ToVec2(), 0.3f, maxDistanceToLookForPathDetection, out num4, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1()))
				{
					num = num4;
				}
				if (currentSettlementToLook.HasPort)
				{
					campaignVec = currentSettlementToLook.PortPosition;
					pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec);
					float num5;
					if (this.MapSceneWrapper.GetPathDistanceBetweenAIFaces(currentFaceRecord, pathFaceRecord, checkPosition, currentSettlementToLook.PortPosition.ToVec2(), 0.3f, maxDistanceToLookForPathDetection, out num5, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1()) && num5 < num4)
					{
						num = num5;
						isPort = true;
					}
				}
				break;
			}
			}
			return num;
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x00096AB4 File Offset: 0x00094CB4
		protected override IEnumerable<Settlement> GetClosestSettlementsToPositionInCache(Vec2 checkPosition, List<Settlement> settlements)
		{
			if (base._navigationType == MobileParty.NavigationType.Naval)
			{
				return from x in settlements
					where x.HasPort
					orderby checkPosition.DistanceSquared(x.PortPosition.ToVec2())
					select x;
			}
			return settlements.OrderBy<Settlement, float>((Settlement x) => checkPosition.DistanceSquared(x.GatePosition.ToVec2()));
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x00096B20 File Offset: 0x00094D20
		protected override List<Settlement> GetAllRegisteredSettlements()
		{
			return Settlement.All.ToList<Settlement>();
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x00096B2C File Offset: 0x00094D2C
		public void FinalizeInitialization()
		{
			base.FinalizeCacheInitialization();
		}

		// Token: 0x040009DB RID: 2523
		private readonly int[] _excludedFaceIds;

		// Token: 0x040009DC RID: 2524
		private readonly int _regionSwitchCostTo0;

		// Token: 0x040009DD RID: 2525
		private readonly int _regionSwitchCostTo1;
	}
}
