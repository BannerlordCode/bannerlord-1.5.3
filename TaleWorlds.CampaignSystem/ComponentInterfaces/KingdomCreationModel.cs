using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AE RID: 430
	public abstract class KingdomCreationModel : MBGameModel<KingdomCreationModel>
	{
		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06001D6D RID: 7533
		public abstract int MinimumClanTierToCreateKingdom { get; }

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06001D6E RID: 7534
		public abstract int MinimumNumberOfSettlementsOwnedToCreateKingdom { get; }

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001D6F RID: 7535
		public abstract int MinimumTroopCountToCreateKingdom { get; }

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06001D70 RID: 7536
		public abstract int MaximumNumberOfInitialPolicies { get; }

		// Token: 0x06001D71 RID: 7537
		public abstract bool IsPlayerKingdomCreationPossible(out List<TextObject> explanations);

		// Token: 0x06001D72 RID: 7538
		public abstract bool IsPlayerKingdomAbdicationPossible(out List<TextObject> explanations);

		// Token: 0x06001D73 RID: 7539
		public abstract IEnumerable<CultureObject> GetAvailablePlayerKingdomCultures();
	}
}
