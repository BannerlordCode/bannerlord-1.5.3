using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000103 RID: 259
	public class DefaultBuildingScoreCalculationModel : BuildingScoreCalculationModel
	{
		// Token: 0x06001735 RID: 5941 RVA: 0x0006C642 File Offset: 0x0006A842
		public override Building GetNextDailyBuilding(Town town)
		{
			return town.Buildings.GetRandomElementWithPredicate<Building>((Building b) => b.BuildingType.IsDailyProject);
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x0006C670 File Offset: 0x0006A870
		public override Building GetNextBuilding(Town town)
		{
			return town.Buildings.WhereQ<Building>((Building x) => !x.BuildingType.IsDailyProject && x.CurrentLevel < 3 && !town.BuildingsInProgress.Contains(x)).GetRandomElementInefficiently<Building>();
		}
	}
}
