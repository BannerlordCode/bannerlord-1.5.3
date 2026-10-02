using System;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200016D RID: 365
	public class DefaultVillageTradeModel : VillageTradeModel
	{
		// Token: 0x06001BA4 RID: 7076 RVA: 0x0008F429 File Offset: 0x0008D629
		public override float TradeBoundDistanceLimitAsDays(MobileParty.NavigationType navigationType)
		{
			return Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(navigationType) * 3f / (Campaign.Current.EstimatedAverageVillagerPartySpeed * (float)CampaignTime.HoursInDay);
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0008F450 File Offset: 0x0008D650
		public override Settlement GetTradeBoundToAssignForVillage(Village village)
		{
			MobileParty.NavigationType navigationType = MobileParty.NavigationType.Default;
			Settlement settlement = SettlementHelper.FindNearestSettlementToSettlement(village.Settlement, navigationType, (Settlement x) => x.IsTown && x.Town.MapFaction == village.Settlement.MapFaction);
			float distanceLimit = Campaign.Current.Models.VillageTradeModel.TradeBoundDistanceLimitAsDays(navigationType) * Campaign.Current.EstimatedAverageVillagerPartySpeed * (float)CampaignTime.HoursInDay;
			if (settlement != null && Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, village.Settlement, false, false, navigationType) < distanceLimit)
			{
				return settlement;
			}
			Settlement settlement2 = SettlementHelper.FindNearestSettlementToSettlement(village.Settlement, navigationType, (Settlement x) => x.IsTown && x.Town.MapFaction != village.Settlement.MapFaction && !x.Town.MapFaction.IsAtWarWith(village.Settlement.MapFaction) && Campaign.Current.Models.MapDistanceModel.GetDistance(x, village.Settlement, false, false, navigationType) <= distanceLimit);
			if (settlement2 != null && Campaign.Current.Models.MapDistanceModel.GetDistance(settlement2, village.Settlement, false, false, navigationType) < distanceLimit)
			{
				return settlement2;
			}
			return null;
		}
	}
}
