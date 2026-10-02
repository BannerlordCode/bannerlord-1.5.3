using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F3 RID: 499
	public abstract class BuildingScoreCalculationModel : MBGameModel<BuildingScoreCalculationModel>
	{
		// Token: 0x06001FB3 RID: 8115
		public abstract Building GetNextBuilding(Town town);

		// Token: 0x06001FB4 RID: 8116
		public abstract Building GetNextDailyBuilding(Town town);
	}
}
