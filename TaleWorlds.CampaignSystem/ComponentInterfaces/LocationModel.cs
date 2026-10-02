using System;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000193 RID: 403
	public abstract class LocationModel : MBGameModel<LocationModel>
	{
		// Token: 0x06001CBF RID: 7359
		public abstract int GetSettlementUpgradeLevel(LocationEncounter locationEncounter);

		// Token: 0x06001CC0 RID: 7360
		public abstract string GetCivilianSceneLevel(Settlement settlement);

		// Token: 0x06001CC1 RID: 7361
		public abstract string GetCivilianUpgradeLevelTag(int upgradeLevel);

		// Token: 0x06001CC2 RID: 7362
		public abstract string GetUpgradeLevelTag(int upgradeLevel);
	}
}
