using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000101 RID: 257
	public class DefaultBuildingEffectModel : BuildingEffectModel
	{
		// Token: 0x06001731 RID: 5937 RVA: 0x0006C3E8 File Offset: 0x0006A5E8
		public override ExplainedNumber GetBuildingEffect(Building building, BuildingEffectEnum effect)
		{
			float baseBuildingEffectAmount = building.BuildingType.GetBaseBuildingEffectAmount(effect, building.CurrentLevel);
			ExplainedNumber explainedNumber = new ExplainedNumber(baseBuildingEffectAmount, false, null);
			if (effect == BuildingEffectEnum.DenarByBoundVillageHeartPerDay)
			{
				float num = 0f;
				foreach (Village village in building.Town.Villages)
				{
					num += village.Hearth;
				}
				explainedNumber = new ExplainedNumber(num * baseBuildingEffectAmount, false, null);
			}
			if (effect == BuildingEffectEnum.FoodStock && (building.BuildingType == DefaultBuildingTypes.CastleGranary || building.BuildingType == DefaultBuildingTypes.SettlementWarehouse))
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Engineering.Battlements, building.Town, false, ref explainedNumber);
			}
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.Contractors, building.Town, false, ref explainedNumber);
			if (building.BuildingType.IsDailyProject)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.MasterOfPlanning, building.Town, false, ref explainedNumber);
			}
			if (building.BuildingType == DefaultBuildingTypes.SettlementMarketplace || building.BuildingType == DefaultBuildingTypes.SettlementDailyFestivalAndGames)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Charm.PublicSpeaker, building.Town, false, ref explainedNumber);
			}
			return explainedNumber;
		}
	}
}
