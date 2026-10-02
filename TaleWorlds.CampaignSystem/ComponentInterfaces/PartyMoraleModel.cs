using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AD RID: 429
	public abstract class PartyMoraleModel : MBGameModel<PartyMoraleModel>
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06001D65 RID: 7525
		public abstract float HighMoraleValue { get; }

		// Token: 0x06001D66 RID: 7526
		public abstract int GetDailyStarvationMoralePenalty(PartyBase party);

		// Token: 0x06001D67 RID: 7527
		public abstract int GetDailyNoWageMoralePenalty(MobileParty party);

		// Token: 0x06001D68 RID: 7528
		public abstract float GetStandardBaseMorale(PartyBase party);

		// Token: 0x06001D69 RID: 7529
		public abstract float GetVictoryMoraleChange(PartyBase party);

		// Token: 0x06001D6A RID: 7530
		public abstract float GetDefeatMoraleChange(PartyBase party);

		// Token: 0x06001D6B RID: 7531
		public abstract ExplainedNumber GetEffectivePartyMorale(MobileParty party, bool includeDescription = false);
	}
}
