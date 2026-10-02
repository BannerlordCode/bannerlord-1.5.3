using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B1 RID: 433
	public abstract class AllianceModel : MBGameModel<AllianceModel>
	{
		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06001D7D RID: 7549
		public abstract CampaignTime MaxDurationOfAlliance { get; }

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06001D7E RID: 7550
		public abstract CampaignTime MaxDurationOfWarParticipation { get; }

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001D7F RID: 7551
		public abstract int MaxNumberOfAlliances { get; }

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001D80 RID: 7552
		public abstract CampaignTime DurationForOffers { get; }

		// Token: 0x06001D81 RID: 7553
		public abstract int GetCallToWarCost(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst);

		// Token: 0x06001D82 RID: 7554
		public abstract ExplainedNumber GetScoreOfStartingAlliance(Kingdom kingdomDeclaresAlliance, Kingdom kingdomDeclaredAlliance, out TextObject explanation, bool includeDescription = false);

		// Token: 0x06001D83 RID: 7555
		public abstract float GetSupportScoreOfStartingAllianceForClan(Kingdom kingdomDeclaresAlliance, Kingdom kingdomDeclaredAlliance, Clan evaluatingClan, out TextObject explanation, bool includeDescription = false);

		// Token: 0x06001D84 RID: 7556
		public abstract float GetScoreOfCallingToWar(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst, IFaction evaluatingFaction, out TextObject reason);

		// Token: 0x06001D85 RID: 7557
		public abstract float GetScoreOfJoiningWar(Kingdom offeringKingdom, Kingdom kingdomToOfferToJoinWarWith, Kingdom kingdomToOfferToJoinWarAgainst, IFaction evaluatingFaction, out TextObject reason);

		// Token: 0x06001D86 RID: 7558
		public abstract int GetInfluenceCostOfProposingStartingAlliance(Clan proposingClan);

		// Token: 0x06001D87 RID: 7559
		public abstract int GetInfluenceCostOfCallingToWar(Clan proposingClan);

		// Token: 0x06001D88 RID: 7560
		public abstract bool CanMakeAlliance(Kingdom kingdom, Kingdom targetKingdom, IFaction evaluatingFaction, out TextObject reason, bool includeReason = false);

		// Token: 0x06001D89 RID: 7561
		public abstract float GetAllianceFactorForDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar);

		// Token: 0x06001D8A RID: 7562
		public abstract float GetAllianceFactorForDeclaringPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace);

		// Token: 0x06001D8B RID: 7563
		public abstract Clan GetProposerClanForAllianceDecision(Kingdom proposerKingdom, Kingdom proposedKingdom);
	}
}
