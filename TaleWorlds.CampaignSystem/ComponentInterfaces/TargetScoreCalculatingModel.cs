using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B8 RID: 440
	public abstract class TargetScoreCalculatingModel : MBGameModel<TargetScoreCalculatingModel>
	{
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001DE5 RID: 7653
		public abstract float TravelingToAssignmentFactor { get; }

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001DE6 RID: 7654
		public abstract float BesiegingFactor { get; }

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06001DE7 RID: 7655
		public abstract float AssaultingTownFactor { get; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06001DE8 RID: 7656
		public abstract float RaidingFactor { get; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06001DE9 RID: 7657
		public abstract float DefendingFactor { get; }

		// Token: 0x06001DEA RID: 7658
		public abstract float GetDefensivePatrollingFactor(bool isNavalPatrolling);

		// Token: 0x06001DEB RID: 7659
		public abstract float GetOffensivePatrollingFactor(bool isNavalPatrolling);

		// Token: 0x06001DEC RID: 7660
		public abstract float GetTargetScoreForFaction(Settlement targetSettlement, Army.ArmyTypes missionType, MobileParty mobileParty, float ourStrength);

		// Token: 0x06001DED RID: 7661
		public abstract float CalculateDefensivePatrollingScoreForSettlement(Settlement settlement, bool isTargetingPort, MobileParty mobileParty);

		// Token: 0x06001DEE RID: 7662
		public abstract float CalculateOffensivePatrollingScoreForSettlement(Settlement settlement, bool isTargetingPort, MobileParty mobileParty);

		// Token: 0x06001DEF RID: 7663
		public abstract float CurrentObjectiveValue(MobileParty mobileParty);
	}
}
