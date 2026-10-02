using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B2 RID: 434
	public abstract class DiplomacyModel : MBGameModel<DiplomacyModel>
	{
		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06001D8D RID: 7565
		public abstract int MaxRelationLimit { get; }

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06001D8E RID: 7566
		public abstract int MinRelationLimit { get; }

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06001D8F RID: 7567
		public abstract int MaxNeutralRelationLimit { get; }

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001D90 RID: 7568
		public abstract int MinNeutralRelationLimit { get; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001D91 RID: 7569
		public abstract int MinimumRelationWithConversationCharacterToJoinKingdom { get; }

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001D92 RID: 7570
		public abstract int GiftingTownRelationshipBonus { get; }

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001D93 RID: 7571
		public abstract int GiftingCastleRelationshipBonus { get; }

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06001D94 RID: 7572
		public abstract float WarDeclarationScorePenaltyAgainstTradePartners { get; }

		// Token: 0x06001D95 RID: 7573
		public abstract float GetStrengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom(Kingdom kingdomToJoin);

		// Token: 0x06001D96 RID: 7574
		public abstract int GetEffectiveRelationChange(Hero originalHero, Hero originalGainedRelationWith, int relationChange);

		// Token: 0x06001D97 RID: 7575
		public abstract int GetInfluenceAwardForSettlementCapturer(Settlement settlement);

		// Token: 0x06001D98 RID: 7576
		public abstract float GetHourlyInfluenceAwardForRaidingEnemyVillage(MobileParty mobileParty);

		// Token: 0x06001D99 RID: 7577
		public abstract float GetHourlyInfluenceAwardForBesiegingEnemyFortification(MobileParty mobileParty);

		// Token: 0x06001D9A RID: 7578
		public abstract float GetHourlyInfluenceAwardForBeingArmyMember(MobileParty mobileParty);

		// Token: 0x06001D9B RID: 7579
		public abstract float GetScoreOfClanToJoinKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x06001D9C RID: 7580
		public abstract float GetScoreOfClanToLeaveKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x06001D9D RID: 7581
		public abstract float GetScoreOfKingdomToGetClan(Kingdom kingdom, Clan clan);

		// Token: 0x06001D9E RID: 7582
		public abstract float GetScoreOfKingdomToSackClan(Kingdom kingdom, Clan clan);

		// Token: 0x06001D9F RID: 7583
		public abstract float GetScoreOfMercenaryToJoinKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x06001DA0 RID: 7584
		public abstract float GetScoreOfMercenaryToLeaveKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x06001DA1 RID: 7585
		public abstract float GetScoreOfKingdomToHireMercenary(Kingdom kingdom, Clan mercenaryClan);

		// Token: 0x06001DA2 RID: 7586
		public abstract float GetScoreOfKingdomToSackMercenary(Kingdom kingdom, Clan mercenaryClan);

		// Token: 0x06001DA3 RID: 7587
		public abstract float GetScoreOfDeclaringPeaceForClan(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace, Clan evaluatingClan, out TextObject reason, bool includeReason = false);

		// Token: 0x06001DA4 RID: 7588
		public abstract float GetScoreOfDeclaringPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace);

		// Token: 0x06001DA5 RID: 7589
		public abstract bool IsPeaceSuitable(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace);

		// Token: 0x06001DA6 RID: 7590
		public abstract float GetScoreOfDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar, Clan evaluatingClan, out TextObject reason, bool includeReason = false);

		// Token: 0x06001DA7 RID: 7591
		public abstract ExplainedNumber GetWarProgressScore(IFaction factionDeclaresWar, IFaction factionDeclaredWar, bool includeDescriptions = false);

		// Token: 0x06001DA8 RID: 7592
		public abstract float GetScoreOfLettingPartyGo(MobileParty party, MobileParty partyToLetGo);

		// Token: 0x06001DA9 RID: 7593
		public abstract float GetValueOfHeroForFaction(Hero examinedHero, IFaction targetFaction, bool forMarriage = false);

		// Token: 0x06001DAA RID: 7594
		public abstract int GetRelationCostOfExpellingClanFromKingdom();

		// Token: 0x06001DAB RID: 7595
		public abstract int GetInfluenceCostOfSupportingClan();

		// Token: 0x06001DAC RID: 7596
		public abstract int GetInfluenceCostOfExpellingClan(Clan proposingClan);

		// Token: 0x06001DAD RID: 7597
		public abstract int GetInfluenceCostOfProposingPeace(Clan proposingClan);

		// Token: 0x06001DAE RID: 7598
		public abstract int GetInfluenceCostOfProposingWar(Clan proposingClan);

		// Token: 0x06001DAF RID: 7599
		public abstract int GetInfluenceValueOfSupportingClan();

		// Token: 0x06001DB0 RID: 7600
		public abstract int GetRelationValueOfSupportingClan();

		// Token: 0x06001DB1 RID: 7601
		public abstract int GetInfluenceCostOfAnnexation(Clan proposingClan);

		// Token: 0x06001DB2 RID: 7602
		public abstract int GetInfluenceCostOfChangingLeaderOfArmy();

		// Token: 0x06001DB3 RID: 7603
		public abstract int GetInfluenceCostOfDisbandingArmy();

		// Token: 0x06001DB4 RID: 7604
		public abstract int GetRelationCostOfDisbandingArmy(bool isLeaderParty);

		// Token: 0x06001DB5 RID: 7605
		public abstract int GetInfluenceCostOfPolicyProposalAndDisavowal(Clan proposingClan);

		// Token: 0x06001DB6 RID: 7606
		public abstract int GetInfluenceCostOfAbandoningArmy();

		// Token: 0x06001DB7 RID: 7607
		public abstract int GetEffectiveRelation(Hero hero, Hero hero1);

		// Token: 0x06001DB8 RID: 7608
		public abstract int GetBaseRelation(Hero hero, Hero hero1);

		// Token: 0x06001DB9 RID: 7609
		public abstract void GetHeroesForEffectiveRelation(Hero hero1, Hero hero2, out Hero effectiveHero1, out Hero effectiveHero2);

		// Token: 0x06001DBA RID: 7610
		public abstract int GetRelationChangeAfterClanLeaderIsDead(Hero deadLeader, Hero relationHero);

		// Token: 0x06001DBB RID: 7611
		public abstract int GetRelationChangeAfterVotingInSettlementOwnerPreliminaryDecision(Hero supporter, bool hasHeroVotedAgainstOwner);

		// Token: 0x06001DBC RID: 7612
		public abstract float GetClanStrength(Clan clan);

		// Token: 0x06001DBD RID: 7613
		public abstract float GetHeroCommandingStrengthForClan(Hero hero);

		// Token: 0x06001DBE RID: 7614
		public abstract float GetHeroGoverningStrengthForClan(Hero hero);

		// Token: 0x06001DBF RID: 7615
		public abstract uint GetNotificationColor(ChatNotificationType notificationType);

		// Token: 0x06001DC0 RID: 7616
		public abstract int GetDailyTributeToPay(Clan factionToPay, Clan factionToReceive, out int tributeDurationInDays);

		// Token: 0x06001DC1 RID: 7617
		public abstract float GetDecisionMakingThreshold(IFaction consideringFaction);

		// Token: 0x06001DC2 RID: 7618
		public abstract float GetValueOfSettlementsForFaction(IFaction faction);

		// Token: 0x06001DC3 RID: 7619
		public abstract bool CanSettlementBeGifted(Settlement settlement);

		// Token: 0x06001DC4 RID: 7620
		public abstract bool IsClanEligibleToBecomeRuler(Clan clan);

		// Token: 0x06001DC5 RID: 7621
		public abstract IEnumerable<BarterGroup> GetBarterGroups();

		// Token: 0x06001DC6 RID: 7622
		public abstract int GetCharmExperienceFromRelationGain(Hero hero, float relationChange, ChangeRelationAction.ChangeRelationDetail detail);

		// Token: 0x06001DC7 RID: 7623
		public abstract float DenarsToInfluence();

		// Token: 0x06001DC8 RID: 7624
		public abstract DiplomacyModel.DiplomacyStance? GetShallowDiplomaticStance(IFaction faction1, IFaction faction2);

		// Token: 0x06001DC9 RID: 7625
		public abstract DiplomacyModel.DiplomacyStance GetDefaultDiplomaticStance(IFaction faction1, IFaction faction2);

		// Token: 0x06001DCA RID: 7626
		public abstract bool IsAtConstantWar(IFaction faction1, IFaction faction2);

		// Token: 0x0200062D RID: 1581
		public enum DiplomacyStance
		{
			// Token: 0x04001A2D RID: 6701
			Neutral,
			// Token: 0x04001A2E RID: 6702
			War
		}
	}
}
