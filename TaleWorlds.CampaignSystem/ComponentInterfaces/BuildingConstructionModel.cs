using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E1 RID: 481
	public abstract class BuildingConstructionModel : MBGameModel<BuildingConstructionModel>
	{
		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06001F37 RID: 7991
		public abstract int TownBoostCost { get; }

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06001F38 RID: 7992
		public abstract int TownBoostBonus { get; }

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06001F39 RID: 7993
		public abstract int CastleBoostCost { get; }

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06001F3A RID: 7994
		public abstract int CastleBoostBonus { get; }

		// Token: 0x06001F3B RID: 7995
		public abstract ExplainedNumber CalculateDailyConstructionPower(Town town, bool includeDescriptions = false);

		// Token: 0x06001F3C RID: 7996
		public abstract int CalculateDailyConstructionPowerWithoutBoost(Town town);

		// Token: 0x06001F3D RID: 7997
		public abstract int GetBoostCost(Town town);

		// Token: 0x06001F3E RID: 7998
		public abstract int GetBoostAmount(Town town);
	}
}
