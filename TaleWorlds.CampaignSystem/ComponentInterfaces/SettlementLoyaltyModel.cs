using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D4 RID: 468
	public abstract class SettlementLoyaltyModel : MBGameModel<SettlementLoyaltyModel>
	{
		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001EC5 RID: 7877
		public abstract int SettlementLoyaltyChangeDueToSecurityThreshold { get; }

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06001EC6 RID: 7878
		public abstract int MaximumLoyaltyInSettlement { get; }

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06001EC7 RID: 7879
		public abstract int LoyaltyDriftMedium { get; }

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06001EC8 RID: 7880
		public abstract float HighLoyaltyProsperityEffect { get; }

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06001EC9 RID: 7881
		public abstract int LowLoyaltyProsperityEffect { get; }

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06001ECA RID: 7882
		public abstract int MilitiaBoostPercentage { get; }

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06001ECB RID: 7883
		public abstract float HighSecurityLoyaltyEffect { get; }

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06001ECC RID: 7884
		public abstract float LowSecurityLoyaltyEffect { get; }

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06001ECD RID: 7885
		public abstract float SettlementOwnerDifferentCultureLoyaltyEffect { get; }

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06001ECE RID: 7886
		public abstract int ThresholdForTaxBoost { get; }

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06001ECF RID: 7887
		public abstract int RebellionStartLoyaltyThreshold { get; }

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06001ED0 RID: 7888
		public abstract int ThresholdForTaxCorruption { get; }

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06001ED1 RID: 7889
		public abstract int ThresholdForHigherTaxCorruption { get; }

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06001ED2 RID: 7890
		public abstract int ThresholdForProsperityBoost { get; }

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06001ED3 RID: 7891
		public abstract int ThresholdForProsperityPenalty { get; }

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06001ED4 RID: 7892
		public abstract int AdditionalStarvationPenaltyStartDay { get; }

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06001ED5 RID: 7893
		public abstract int AdditionalStarvationLoyaltyEffect { get; }

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06001ED6 RID: 7894
		public abstract int RebelliousStateStartLoyaltyThreshold { get; }

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06001ED7 RID: 7895
		public abstract int LoyaltyBoostAfterRebellionStartValue { get; }

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06001ED8 RID: 7896
		public abstract float ThresholdForNotableRelationBonus { get; }

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06001ED9 RID: 7897
		public abstract int DailyNotableRelationBonus { get; }

		// Token: 0x06001EDA RID: 7898
		public abstract ExplainedNumber CalculateLoyaltyChange(Town town, bool includeDescriptions = false);

		// Token: 0x06001EDB RID: 7899
		public abstract void CalculateGoldGainDueToHighLoyalty(Town town, ref ExplainedNumber explainedNumber);

		// Token: 0x06001EDC RID: 7900
		public abstract void CalculateGoldCutDueToLowLoyalty(Town town, ref ExplainedNumber explainedNumber);
	}
}
