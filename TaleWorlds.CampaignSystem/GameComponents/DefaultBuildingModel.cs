using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000102 RID: 258
	public class DefaultBuildingModel : BuildingModel
	{
		// Token: 0x06001733 RID: 5939 RVA: 0x0006C518 File Offset: 0x0006A718
		public override bool CanAddBuildingTypeToTown(BuildingType buildingType, Town town)
		{
			if (buildingType == DefaultBuildingTypes.SettlementFortifications || buildingType == DefaultBuildingTypes.SettlementBarracks || buildingType == DefaultBuildingTypes.SettlementTrainingFields || buildingType == DefaultBuildingTypes.SettlementGuardHouse || buildingType == DefaultBuildingTypes.SettlementSiegeWorkshop || buildingType == DefaultBuildingTypes.SettlementTaxOffice || buildingType == DefaultBuildingTypes.SettlementMarketplace || buildingType == DefaultBuildingTypes.SettlementWarehouse || buildingType == DefaultBuildingTypes.SettlementMason || buildingType == DefaultBuildingTypes.SettlementWaterworks || buildingType == DefaultBuildingTypes.SettlementCourthouse || buildingType == DefaultBuildingTypes.SettlementRoadsAndPaths)
			{
				return town.IsTown;
			}
			if (buildingType == DefaultBuildingTypes.CastleFortifications || buildingType == DefaultBuildingTypes.CastleBarracks || buildingType == DefaultBuildingTypes.CastleTrainingFields || buildingType == DefaultBuildingTypes.CastleGuardHouse || buildingType == DefaultBuildingTypes.CastleSiegeWorkshop || buildingType == DefaultBuildingTypes.CastleCastallansOffice || buildingType == DefaultBuildingTypes.CastleGranary || buildingType == DefaultBuildingTypes.CastleCraftmansQuarters || buildingType == DefaultBuildingTypes.CastleFarmlands || buildingType == DefaultBuildingTypes.CastleMason || buildingType == DefaultBuildingTypes.CastleRoadsAndPaths)
			{
				return town.IsCastle;
			}
			if (buildingType == DefaultBuildingTypes.SettlementDailyHousing || buildingType == DefaultBuildingTypes.SettlementDailyTrainMilitia || buildingType == DefaultBuildingTypes.SettlementDailyFestivalAndGames || buildingType == DefaultBuildingTypes.SettlementDailyIrrigation)
			{
				return town.IsTown;
			}
			return (buildingType != DefaultBuildingTypes.CastleDailySlackenGarrison && buildingType != DefaultBuildingTypes.CastleDailyRaiseTroops && buildingType != DefaultBuildingTypes.CastleDailyDrills && buildingType != DefaultBuildingTypes.CastleDailyIrrigation) || town.IsCastle;
		}
	}
}
