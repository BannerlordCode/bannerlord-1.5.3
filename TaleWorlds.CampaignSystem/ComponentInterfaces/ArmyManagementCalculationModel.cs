using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BF RID: 447
	public abstract class ArmyManagementCalculationModel : MBGameModel<ArmyManagementCalculationModel>
	{
		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06001E23 RID: 7715
		public abstract float AIMobilePartySizeRatioToCallToArmy { get; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06001E24 RID: 7716
		public abstract float PlayerMobilePartySizeRatioToCallToArmy { get; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06001E25 RID: 7717
		public abstract float MinimumNeededFoodInDaysToCallToArmy { get; }

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06001E26 RID: 7718
		public abstract float MaximumDistanceToCallToArmy { get; }

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06001E27 RID: 7719
		public abstract int InfluenceValuePerGold { get; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06001E28 RID: 7720
		public abstract int AverageCallToArmyCost { get; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06001E29 RID: 7721
		public abstract int CohesionThresholdForDispersion { get; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001E2A RID: 7722
		public abstract float MaximumWaitTime { get; }

		// Token: 0x06001E2B RID: 7723
		public abstract bool CanPlayerCreateArmy(out TextObject disabledReason);

		// Token: 0x06001E2C RID: 7724
		public abstract int CalculatePartyInfluenceCost(MobileParty armyLeaderParty, MobileParty party);

		// Token: 0x06001E2D RID: 7725
		public abstract float DailyBeingAtArmyInfluenceAward(MobileParty armyMemberParty);

		// Token: 0x06001E2E RID: 7726
		public abstract bool CanLordCreateArmy(MobileParty leaderParty, out MBList<MobileParty> possibleArmyMembers);

		// Token: 0x06001E2F RID: 7727
		public abstract int CalculateTotalInfluenceCost(Army army, float percentage);

		// Token: 0x06001E30 RID: 7728
		public abstract float GetPartySizeScore(MobileParty party);

		// Token: 0x06001E31 RID: 7729
		public abstract bool CheckPartyEligibility(MobileParty party, out TextObject explanation);

		// Token: 0x06001E32 RID: 7730
		public abstract int GetPartyRelation(Hero hero);

		// Token: 0x06001E33 RID: 7731
		public abstract ExplainedNumber CalculateDailyCohesionChange(Army army, bool includeDescriptions = false);

		// Token: 0x06001E34 RID: 7732
		public abstract int CalculateNewCohesion(Army army, PartyBase newParty, int calculatedCohesion, int sign);

		// Token: 0x06001E35 RID: 7733
		public abstract int GetCohesionBoostInfluenceCost(Army army, int percentageToBoost = 100);
	}
}
