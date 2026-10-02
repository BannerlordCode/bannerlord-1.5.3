using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DC RID: 476
	public abstract class ClanTierModel : MBGameModel<ClanTierModel>
	{
		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06001F11 RID: 7953
		public abstract int MinClanTier { get; }

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06001F12 RID: 7954
		public abstract int MaxClanTier { get; }

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06001F13 RID: 7955
		public abstract int MercenaryEligibleTier { get; }

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06001F14 RID: 7956
		public abstract int VassalEligibleTier { get; }

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06001F15 RID: 7957
		public abstract int BannerEligibleTier { get; }

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06001F16 RID: 7958
		public abstract int RebelClanStartingTier { get; }

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06001F17 RID: 7959
		public abstract int CompanionToLordClanStartingTier { get; }

		// Token: 0x06001F18 RID: 7960
		public abstract int CalculateInitialRenown(Clan clan);

		// Token: 0x06001F19 RID: 7961
		public abstract int CalculateInitialInfluence(Clan clan);

		// Token: 0x06001F1A RID: 7962
		public abstract int CalculateTier(Clan clan);

		// Token: 0x06001F1B RID: 7963
		public abstract ValueTuple<ExplainedNumber, bool> HasUpcomingTier(Clan clan, out TextObject extraExplanation, bool includeDescriptions = false);

		// Token: 0x06001F1C RID: 7964
		public abstract int GetRequiredRenownForTier(int tier);

		// Token: 0x06001F1D RID: 7965
		public abstract int GetPartyLimitForTier(Clan clan, int clanTierToCheck);

		// Token: 0x06001F1E RID: 7966
		public abstract int GetCompanionLimit(Clan clan);
	}
}
