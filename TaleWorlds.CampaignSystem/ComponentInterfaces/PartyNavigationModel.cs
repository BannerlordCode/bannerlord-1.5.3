using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000197 RID: 407
	public abstract class PartyNavigationModel : MBGameModel<PartyNavigationModel>
	{
		// Token: 0x06001CD3 RID: 7379
		public abstract bool CanPlayerNavigateToPosition(CampaignVec2 vec2, out MobileParty.NavigationType navigationType);

		// Token: 0x06001CD4 RID: 7380
		public abstract float GetEmbarkDisembarkThresholdDistance();

		// Token: 0x06001CD5 RID: 7381
		public abstract bool IsTerrainTypeValidForNavigationType(TerrainType terrainType, MobileParty.NavigationType navigationType);

		// Token: 0x06001CD6 RID: 7382
		public abstract int[] GetInvalidTerrainTypesForNavigationType(MobileParty.NavigationType navigationType);

		// Token: 0x06001CD7 RID: 7383
		public abstract bool HasNavalNavigationCapability(MobileParty mobileParty);
	}
}
