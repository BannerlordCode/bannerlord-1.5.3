using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CD RID: 461
	public abstract class SettlementMilitiaModel : MBGameModel<SettlementMilitiaModel>
	{
		// Token: 0x06001E9E RID: 7838
		public abstract int MilitiaToSpawnAfterSiege(Town town);

		// Token: 0x06001E9F RID: 7839
		public abstract ExplainedNumber CalculateMilitiaChange(Settlement settlement, bool includeDescriptions = false);

		// Token: 0x06001EA0 RID: 7840
		public abstract ExplainedNumber CalculateVeteranMilitiaSpawnChance(Settlement settlement);

		// Token: 0x06001EA1 RID: 7841
		public abstract void CalculateMilitiaSpawnRate(Settlement settlement, out float meleeTroopRate, out float rangedTroopRate);
	}
}
