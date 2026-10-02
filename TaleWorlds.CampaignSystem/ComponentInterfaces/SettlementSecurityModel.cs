using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D5 RID: 469
	public abstract class SettlementSecurityModel : MBGameModel<SettlementSecurityModel>
	{
		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001EDE RID: 7902
		public abstract int MaximumSecurityInSettlement { get; }

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001EDF RID: 7903
		public abstract int SecurityDriftMedium { get; }

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06001EE0 RID: 7904
		public abstract float MapEventSecurityEffectRadius { get; }

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06001EE1 RID: 7905
		public abstract float HideoutClearedSecurityEffectRadius { get; }

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06001EE2 RID: 7906
		public abstract int HideoutClearedSecurityGain { get; }

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06001EE3 RID: 7907
		public abstract int ThresholdForTaxCorruption { get; }

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06001EE4 RID: 7908
		public abstract int ThresholdForHigherTaxCorruption { get; }

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06001EE5 RID: 7909
		public abstract int ThresholdForTaxBoost { get; }

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06001EE6 RID: 7910
		public abstract int SettlementTaxBoostPercentage { get; }

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001EE7 RID: 7911
		public abstract int SettlementTaxPenaltyPercentage { get; }

		// Token: 0x06001EE8 RID: 7912
		public abstract float GetLootedNearbyPartySecurityEffect(Town town, float sumOfAttackedPartyStrengths);

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001EE9 RID: 7913
		public abstract int ThresholdForNotableRelationBonus { get; }

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06001EEA RID: 7914
		public abstract int ThresholdForNotableRelationPenalty { get; }

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06001EEB RID: 7915
		public abstract int DailyNotableRelationBonus { get; }

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06001EEC RID: 7916
		public abstract int DailyNotableRelationPenalty { get; }

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06001EED RID: 7917
		public abstract int DailyNotablePowerBonus { get; }

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06001EEE RID: 7918
		public abstract int DailyNotablePowerPenalty { get; }

		// Token: 0x06001EEF RID: 7919
		public abstract ExplainedNumber CalculateSecurityChange(Town town, bool includeDescriptions = false);

		// Token: 0x06001EF0 RID: 7920
		public abstract float GetNearbyBanditPartyDefeatedSecurityEffect(Town town, float sumOfAttackedPartyStrengths);

		// Token: 0x06001EF1 RID: 7921
		public abstract void CalculateGoldGainDueToHighSecurity(Town town, ref ExplainedNumber explainedNumber);

		// Token: 0x06001EF2 RID: 7922
		public abstract void CalculateGoldCutDueToLowSecurity(Town town, ref ExplainedNumber explainedNumber);
	}
}
