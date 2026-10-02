using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CB RID: 459
	public abstract class TradeAgreementModel : MBGameModel<TradeAgreementModel>
	{
		// Token: 0x06001E92 RID: 7826
		public abstract int GetProfitPerCaravanVisit(MobileParty mobileParty);

		// Token: 0x06001E93 RID: 7827
		public abstract CampaignTime GetTradeAgreementDurationInYears(Kingdom iniatatingKingdom, Kingdom otherKingdom);

		// Token: 0x06001E94 RID: 7828
		public abstract int GetMaximumTradeAgreementCount(Kingdom kingdom);

		// Token: 0x06001E95 RID: 7829
		public abstract int GetInfluenceCostOfProposingTradeAgreement(Clan clan);

		// Token: 0x06001E96 RID: 7830
		public abstract float GetScoreOfStartingTradeAgreement(Kingdom kingdom, Kingdom targetKingdom, Clan clan, out TextObject explanation, bool includeExplanation = false);

		// Token: 0x06001E97 RID: 7831
		public abstract bool CanMakeTradeAgreement(Kingdom kingdom, Kingdom other, bool checkOtherSideTradeSupport, out TextObject reason, bool includeReason = false);
	}
}
