using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200041A RID: 1050
	public interface IAllianceCampaignBehavior
	{
		// Token: 0x06004311 RID: 17169
		void OnAllianceOfferedToPlayerKingdom(Kingdom proposerKingdom);

		// Token: 0x06004312 RID: 17170
		void OnAllianceOfferedToPlayer(Kingdom proposerKingdom);

		// Token: 0x06004313 RID: 17171
		void OnCallToWarAgreementProposedToPlayerKingdom(Kingdom proposerKingdom, Kingdom kingdomToCallToWarAgainst);

		// Token: 0x06004314 RID: 17172
		void OnCallToWarAgreementProposedByPlayerKingdom(Kingdom proposedKingdom, Kingdom kingdomToCallToWarAgainst);

		// Token: 0x06004315 RID: 17173
		void OnCallToWarAgreementProposedToPlayer(Kingdom proposerKingdom, Kingdom kingdomToCallToWarAgainst);

		// Token: 0x06004316 RID: 17174
		void OnCallToWarAgreementProposedByPlayer(Kingdom proposedKingdom, Kingdom kingdomToCallToWarAgainst);

		// Token: 0x06004317 RID: 17175
		bool IsAllyWithKingdom(Kingdom kingdom1, Kingdom kingdom2);

		// Token: 0x06004318 RID: 17176
		void StartAlliance(Kingdom proposerKingdom, Kingdom receiverKingdom);

		// Token: 0x06004319 RID: 17177
		void EndAlliance(Kingdom kingdom1, Kingdom kingdom2);

		// Token: 0x0600431A RID: 17178
		bool HasCalledToWar(Kingdom callingKingdom, Kingdom calledKingdom);

		// Token: 0x0600431B RID: 17179
		bool IsAtWarByCallToWarAgreement(Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst, out Kingdom callingKingdom);

		// Token: 0x0600431C RID: 17180
		void StartCallToWarAgreement(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst, int callToWarCost, bool isPlayerPaying = false);

		// Token: 0x0600431D RID: 17181
		void EndCallToWarAgreement(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst);

		// Token: 0x0600431E RID: 17182
		List<Kingdom> GetKingdomsToCallToWarAgainst(Kingdom callingKingdom, Kingdom calledKingdom);

		// Token: 0x0600431F RID: 17183
		CampaignTime GetAllianceEndDate(Kingdom kingdom1, Kingdom kingdom2);

		// Token: 0x06004320 RID: 17184
		void DenyCallToWarAgreement(Kingdom callingKingdom, Kingdom calledKingdom);
	}
}
