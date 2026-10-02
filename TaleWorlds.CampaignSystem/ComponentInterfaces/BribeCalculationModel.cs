using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A6 RID: 422
	public abstract class BribeCalculationModel : MBGameModel<BribeCalculationModel>
	{
		// Token: 0x06001D2E RID: 7470
		public abstract int GetBribeToEnterLordsHall(Settlement settlement);

		// Token: 0x06001D2F RID: 7471
		public abstract int GetBribeToEnterDungeon(Settlement settlement);

		// Token: 0x06001D30 RID: 7472
		public abstract bool IsBribeNotNeededToEnterKeep(Settlement settlement);

		// Token: 0x06001D31 RID: 7473
		public abstract bool IsBribeNotNeededToEnterDungeon(Settlement settlement);
	}
}
