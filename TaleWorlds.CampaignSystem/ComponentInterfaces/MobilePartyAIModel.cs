using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BB RID: 443
	public abstract class MobilePartyAIModel : MBGameModel<MobilePartyAIModel>
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001DFA RID: 7674
		public abstract float AiCheckInterval { get; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06001DFB RID: 7675
		public abstract float FleeToNearbyPartyRadius { get; }

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06001DFC RID: 7676
		public abstract float FleeToNearbySettlementRadius { get; }

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001DFD RID: 7677
		public abstract float HideoutPatrolDistanceAsDays { get; }

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06001DFE RID: 7678
		public abstract float FortificationPatrolDistanceAsDays { get; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06001DFF RID: 7679
		public abstract float FortificationPortPatrolDistanceAsDays { get; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001E00 RID: 7680
		public abstract float VillagePatrolDistanceAsDays { get; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001E01 RID: 7681
		public abstract float SettlementDefendingNearbyPartyCheckRadius { get; }

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001E02 RID: 7682
		public abstract float SettlementDefendingWaitingPositionRadius { get; }

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06001E03 RID: 7683
		public abstract float NeededFoodsInDaysThresholdForSiege { get; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06001E04 RID: 7684
		public abstract float NeededFoodsInDaysThresholdForRaid { get; }

		// Token: 0x06001E05 RID: 7685
		public abstract bool ShouldConsiderAvoiding(MobileParty party, MobileParty targetParty);

		// Token: 0x06001E06 RID: 7686
		public abstract bool ShouldConsiderAttacking(MobileParty party, MobileParty targetParty);

		// Token: 0x06001E07 RID: 7687
		public abstract float GetPatrolRadius(MobileParty mobileParty, CampaignVec2 patrolPoint);

		// Token: 0x06001E08 RID: 7688
		public abstract float GetSettlementNearbyThreatAndAllyCheckRadius(Settlement settlement, bool isPort);

		// Token: 0x06001E09 RID: 7689
		public abstract bool ShouldPartyCheckInitiativeBehavior(MobileParty mobileParty);

		// Token: 0x06001E0A RID: 7690
		public abstract void GetBestInitiativeBehavior(MobileParty mobileParty, out AiBehavior bestInitiativeBehavior, out MobileParty bestInitiativeTargetParty, out float bestInitiativeBehaviorScore, out Vec2 averageEnemyVec);
	}
}
