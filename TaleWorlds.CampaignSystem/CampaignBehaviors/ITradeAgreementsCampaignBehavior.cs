using System;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200042F RID: 1071
	public interface ITradeAgreementsCampaignBehavior
	{
		// Token: 0x060043FC RID: 17404
		void MakeTradeAgreement(Kingdom kingdom1, Kingdom kingdom2, CampaignTime duration);

		// Token: 0x060043FD RID: 17405
		bool HasTradeAgreement(Kingdom kingdom, Kingdom other, out TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement);

		// Token: 0x060043FE RID: 17406
		void EndTradeAgreement(Kingdom kingdom, Kingdom other);

		// Token: 0x060043FF RID: 17407
		void OnTradeAgreementOfferedToPlayer(Kingdom fromKingdom);

		// Token: 0x06004400 RID: 17408
		CampaignTime GetTradeAgreementEndDate(Kingdom kingdom, Kingdom other);

		// Token: 0x06004401 RID: 17409
		void OnTradeGoldDistributedInKingdom(Kingdom kingdom1, Kingdom kingdom2, Clan clan, int share);
	}
}
