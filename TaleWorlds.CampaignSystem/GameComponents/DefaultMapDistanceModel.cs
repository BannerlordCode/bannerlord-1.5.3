using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200012E RID: 302
	public class DefaultMapDistanceModel : MapDistanceModel
	{
		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x00079875 File Offset: 0x00077A75
		public override int RegionSwitchCostFromLandToSea
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001902 RID: 6402 RVA: 0x00079878 File Offset: 0x00077A78
		public override int RegionSwitchCostFromSeaToLand
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001903 RID: 6403 RVA: 0x0007987B File Offset: 0x00077A7B
		public override float MaximumSpawnDistanceForCompanionsAfterDisband
		{
			get
			{
				return 150f;
			}
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x0007988A File Offset: 0x00077A8A
		public override void RegisterDistanceCache(MobileParty.NavigationType navigationCapability, MapDistanceModel.INavigationCache cacheToRegister)
		{
			this._navigationCache = cacheToRegister;
			cacheToRegister.FinalizeInitialization();
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x00079899 File Offset: 0x00077A99
		public override float GetMaximumDistanceBetweenTwoConnectedSettlements(MobileParty.NavigationType navigationCapabilities)
		{
			MapDistanceModel.INavigationCache navigationCache = this._navigationCache;
			if (navigationCache == null)
			{
				return 0f;
			}
			return navigationCache.MaximumDistanceBetweenTwoConnectedSettlements;
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x000798B0 File Offset: 0x00077AB0
		public override float GetLandRatioOfPathBetweenSettlements(Settlement fromSettlement, Settlement toSettlement, bool isFromPort, bool isTargetingPort)
		{
			if (this._navigationCache != null)
			{
				float num;
				this._navigationCache.GetSettlementToSettlementDistanceWithLandRatio(fromSettlement, false, toSettlement, false, out num);
				return num;
			}
			return 1f;
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x000798E0 File Offset: 0x00077AE0
		public override float GetDistance(Settlement fromSettlement, Settlement toSettlement, bool isFromPort = false, bool isTargetingPort = false, MobileParty.NavigationType navigationCapability = MobileParty.NavigationType.Default)
		{
			float num;
			return this.GetDistance(fromSettlement, toSettlement, isFromPort, isTargetingPort, MobileParty.NavigationType.Default, out num);
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x000798FC File Offset: 0x00077AFC
		public override float GetDistance(Settlement fromSettlement, Settlement toSettlement, bool isFromPort, bool isTargetingPort, MobileParty.NavigationType navigationCapability, out float landRatio)
		{
			float num = float.MaxValue;
			landRatio = 1f;
			if (fromSettlement != null && toSettlement != null)
			{
				if (fromSettlement != toSettlement)
				{
					return this._navigationCache.GetSettlementToSettlementDistanceWithLandRatio(fromSettlement, isFromPort, toSettlement, isTargetingPort, out landRatio);
				}
				num = 0f;
			}
			return num;
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x00079940 File Offset: 0x00077B40
		public override float GetDistance(MobileParty fromMobileParty, Settlement toSettlement, bool isTargetingPort, MobileParty.NavigationType customCapability, out float estimatedLandRatio)
		{
			float num = 100000000f;
			estimatedLandRatio = 1f;
			if (fromMobileParty.CurrentNavigationFace.FaceIndex == toSettlement.GatePosition.Face.FaceIndex)
			{
				if (Campaign.Current.Models.PartyNavigationModel.IsTerrainTypeValidForNavigationType(Campaign.Current.MapSceneWrapper.GetFaceTerrainType(fromMobileParty.Position.Face), MobileParty.NavigationType.Default))
				{
					num = fromMobileParty.Position.Distance(toSettlement.GatePosition);
				}
			}
			else if (fromMobileParty.IsCurrentlyAtSea)
			{
				num = 100000000f;
			}
			else
			{
				Settlement item = Campaign.Current.Models.MapDistanceModel.GetClosestEntranceToFace(fromMobileParty.CurrentNavigationFace, MobileParty.NavigationType.Default).Item1;
				if (item != null)
				{
					num = fromMobileParty.Position.Distance(toSettlement.GatePosition) - item.GatePosition.Distance(toSettlement.GatePosition) + Campaign.Current.Models.MapDistanceModel.GetDistance(item, toSettlement, false, false, MobileParty.NavigationType.Default);
				}
			}
			return MBMath.ClampFloat(num, 0f, float.MaxValue);
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x00079A54 File Offset: 0x00077C54
		public override float GetDistance(MobileParty fromMobileParty, MobileParty toMobileParty, MobileParty.NavigationType customCapability, out float landRatio)
		{
			float num;
			Campaign.Current.Models.MapDistanceModel.GetDistance(fromMobileParty, toMobileParty, customCapability, 100000000f, out num, out landRatio);
			return num;
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00079A84 File Offset: 0x00077C84
		public override bool GetDistance(MobileParty fromMobileParty, MobileParty toMobileParty, MobileParty.NavigationType customCapability, float maxDistance, out float distance, out float landRatio)
		{
			landRatio = 1f;
			distance = float.MaxValue;
			if (fromMobileParty.CurrentNavigationFace.FaceIndex == toMobileParty.CurrentNavigationFace.FaceIndex)
			{
				if (Campaign.Current.Models.PartyNavigationModel.IsTerrainTypeValidForNavigationType(Campaign.Current.MapSceneWrapper.GetFaceTerrainType(fromMobileParty.Position.Face), MobileParty.NavigationType.Default))
				{
					distance = fromMobileParty.Position.Distance(toMobileParty.Position);
				}
			}
			else if (fromMobileParty.IsCurrentlyAtSea || toMobileParty.IsCurrentlyAtSea)
			{
				distance = float.MaxValue;
			}
			else
			{
				distance = fromMobileParty.Position.Distance(toMobileParty.Position);
			}
			distance = MBMath.ClampFloat(distance, 0f, float.MaxValue);
			return distance <= maxDistance;
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00079B58 File Offset: 0x00077D58
		public override float GetDistance(in CampaignVec2 fromPoint, in CampaignVec2 toPoint, MobileParty.NavigationType customCapability, out float landRatio)
		{
			float num = float.MaxValue;
			landRatio = 1f;
			CampaignVec2 campaignVec = toPoint;
			PathFaceRecord face = campaignVec.Face;
			campaignVec = fromPoint;
			if (campaignVec.Face.FaceIndex == face.FaceIndex)
			{
				PartyNavigationModel partyNavigationModel = Campaign.Current.Models.PartyNavigationModel;
				IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
				campaignVec = fromPoint;
				if (partyNavigationModel.IsTerrainTypeValidForNavigationType(mapSceneWrapper.GetFaceTerrainType(campaignVec.Face), MobileParty.NavigationType.Default))
				{
					campaignVec = fromPoint;
					num = campaignVec.Distance(toPoint);
				}
			}
			else
			{
				MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
				campaignVec = fromPoint;
				ValueTuple<Settlement, bool> closestEntranceToFace = mapDistanceModel.GetClosestEntranceToFace(campaignVec.Face, MobileParty.NavigationType.Default);
				ref ValueTuple<Settlement, bool> closestEntranceToFace2 = mapDistanceModel.GetClosestEntranceToFace(face, MobileParty.NavigationType.Default);
				Settlement item = closestEntranceToFace.Item1;
				Settlement item2 = closestEntranceToFace2.Item1;
				if (item != null && item2 != null)
				{
					campaignVec = fromPoint;
					num = campaignVec.Distance(toPoint) - item.GatePosition.Distance(item2.GatePosition) + this.GetDistance(item, item2, false, false, MobileParty.NavigationType.Default);
				}
			}
			return MBMath.ClampFloat(num, 0f, float.MaxValue);
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00079C78 File Offset: 0x00077E78
		public override float GetDistance(MobileParty fromMobileParty, in CampaignVec2 toPoint, MobileParty.NavigationType customCapability, out float landRatio)
		{
			MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
			CampaignVec2 position = fromMobileParty.Position;
			return mapDistanceModel.GetDistance(in position, in toPoint, customCapability, out landRatio);
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00079CA8 File Offset: 0x00077EA8
		public override float GetDistance(Settlement fromSettlement, in CampaignVec2 toPoint, bool isFromPort, MobileParty.NavigationType customCapability)
		{
			float num = float.MaxValue;
			CampaignVec2 campaignVec = (isFromPort ? fromSettlement.PortPosition : fromSettlement.GatePosition);
			CampaignVec2 campaignVec2 = toPoint;
			PathFaceRecord face = campaignVec2.Face;
			PathFaceRecord face2 = campaignVec.Face;
			if (face2.FaceIndex == face.FaceIndex)
			{
				if (Campaign.Current.Models.PartyNavigationModel.IsTerrainTypeValidForNavigationType(Campaign.Current.MapSceneWrapper.GetFaceTerrainType(face2), MobileParty.NavigationType.Default))
				{
					num = campaignVec.Distance(toPoint);
				}
			}
			else
			{
				MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
				Settlement item = mapDistanceModel.GetClosestEntranceToFace(face, MobileParty.NavigationType.Default).Item1;
				if (item != null)
				{
					num = fromSettlement.GatePosition.Distance(toPoint) - fromSettlement.GatePosition.Distance(item.GatePosition) + mapDistanceModel.GetDistance(fromSettlement, item, false, false, MobileParty.NavigationType.Default);
				}
			}
			return MBMath.ClampFloat(num, 0f, 100000000f);
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x00079D9A File Offset: 0x00077F9A
		public override float GetPortToGateDistanceForSettlement(Settlement settlement)
		{
			return 100000000f;
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x00079DA1 File Offset: 0x00077FA1
		public override bool PathExistBetweenPoints(in CampaignVec2 fromPoint, in CampaignVec2 toPoint, MobileParty.NavigationType navigationType)
		{
			return fromPoint.IsOnLand && toPoint.IsOnLand;
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00079DB4 File Offset: 0x00077FB4
		public override ValueTuple<Settlement, bool> GetClosestEntranceToFace(PathFaceRecord face, MobileParty.NavigationType navigationCapabilities)
		{
			bool flag;
			return new ValueTuple<Settlement, bool>(this._navigationCache.GetClosestSettlementToFaceIndex(face.FaceIndex, out flag), flag);
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x00079DDA File Offset: 0x00077FDA
		public override MBReadOnlyList<Settlement> GetNeighborsOfFortification(Town town, MobileParty.NavigationType navigationCapabilities)
		{
			return this._navigationCache.GetNeighbors(town.Settlement);
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x00079DED File Offset: 0x00077FED
		public override float GetTransitionCostAdjustment(Settlement settlement1, bool isFromPort, Settlement settlement2, bool isTargetingPort, bool fromIsCurrentlyAtSea, bool toIsCurrentlyAtSea)
		{
			return 0f;
		}

		// Token: 0x0400082C RID: 2092
		private MapDistanceModel.INavigationCache _navigationCache;
	}
}
