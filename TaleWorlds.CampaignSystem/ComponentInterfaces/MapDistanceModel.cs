using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019C RID: 412
	public abstract class MapDistanceModel : MBGameModel<MapDistanceModel>
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06001CE8 RID: 7400
		public abstract int RegionSwitchCostFromLandToSea { get; }

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06001CE9 RID: 7401
		public abstract int RegionSwitchCostFromSeaToLand { get; }

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06001CEA RID: 7402
		public abstract float MaximumSpawnDistanceForCompanionsAfterDisband { get; }

		// Token: 0x06001CEB RID: 7403
		public abstract float GetMaximumDistanceBetweenTwoConnectedSettlements(MobileParty.NavigationType navigationType);

		// Token: 0x06001CEC RID: 7404
		public abstract float GetLandRatioOfPathBetweenSettlements(Settlement fromSettlement, Settlement toSettlement, bool isFromPort, bool isTargetingPort);

		// Token: 0x06001CED RID: 7405
		public abstract float GetDistance(MobileParty fromMobileParty, Settlement toSettlement, bool isTargetingPort, MobileParty.NavigationType customCapability, out float estimatedLandRatio);

		// Token: 0x06001CEE RID: 7406
		public abstract float GetDistance(MobileParty fromMobileParty, MobileParty toMobileParty, MobileParty.NavigationType customCapability, out float landRatio);

		// Token: 0x06001CEF RID: 7407
		public abstract bool GetDistance(MobileParty fromMobileParty, MobileParty toMobileParty, MobileParty.NavigationType customCapability, float maxDistance, out float distance, out float landRatio);

		// Token: 0x06001CF0 RID: 7408
		public abstract float GetDistance(Settlement fromSettlement, Settlement toSettlement, bool isFromPort, bool isTargetingPort, MobileParty.NavigationType navigationCapability);

		// Token: 0x06001CF1 RID: 7409
		public abstract float GetDistance(Settlement fromSettlement, Settlement toSettlement, bool isFromPort, bool isTargetingPort, MobileParty.NavigationType navigationCapability, out float landRatio);

		// Token: 0x06001CF2 RID: 7410
		public abstract float GetDistance(MobileParty fromMobileParty, in CampaignVec2 toPoint, MobileParty.NavigationType navigationType, out float landRatio);

		// Token: 0x06001CF3 RID: 7411
		public abstract float GetDistance(in CampaignVec2 fromPoint, in CampaignVec2 toPoint, MobileParty.NavigationType navigationType, out float landRatio);

		// Token: 0x06001CF4 RID: 7412
		public abstract float GetDistance(Settlement fromSettlement, in CampaignVec2 toPoint, bool isFromPort, MobileParty.NavigationType navigationType);

		// Token: 0x06001CF5 RID: 7413
		public abstract float GetPortToGateDistanceForSettlement(Settlement settlement);

		// Token: 0x06001CF6 RID: 7414
		public abstract bool PathExistBetweenPoints(in CampaignVec2 fromPoint, in CampaignVec2 toPoint, MobileParty.NavigationType navigationType);

		// Token: 0x06001CF7 RID: 7415
		public abstract void RegisterDistanceCache(MobileParty.NavigationType navigationCapability, MapDistanceModel.INavigationCache cacheToRegister);

		// Token: 0x06001CF8 RID: 7416
		public abstract ValueTuple<Settlement, bool> GetClosestEntranceToFace(PathFaceRecord face, MobileParty.NavigationType navigationCapabilities);

		// Token: 0x06001CF9 RID: 7417
		public abstract MBReadOnlyList<Settlement> GetNeighborsOfFortification(Town town, MobileParty.NavigationType navigationCapabilities);

		// Token: 0x06001CFA RID: 7418
		public abstract float GetTransitionCostAdjustment(Settlement settlement1, bool isFromPort, Settlement settlement2, bool isTargetingPort, bool fromIsCurrentlyAtSea, bool toIsCurrentlyAtSea);

		// Token: 0x0400096B RID: 2411
		public const float PossibleMaximumMapBoundary = 100000000f;

		// Token: 0x0200062B RID: 1579
		public interface INavigationCache
		{
			// Token: 0x17000F9D RID: 3997
			// (get) Token: 0x06005321 RID: 21281
			float MaximumDistanceBetweenTwoConnectedSettlements { get; }

			// Token: 0x06005322 RID: 21282
			float GetSettlementToSettlementDistanceWithLandRatio(Settlement settlement1, bool isAtSea1, Settlement settlement2, bool isAtSea2, out float landRatio);

			// Token: 0x06005323 RID: 21283
			MBReadOnlyList<Settlement> GetNeighbors(Settlement settlement);

			// Token: 0x06005324 RID: 21284
			Settlement GetClosestSettlementToFaceIndex(int faceId, out bool isAtSea);

			// Token: 0x06005325 RID: 21285
			void FinalizeInitialization();
		}
	}
}
