using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BD RID: 445
	public abstract class BanditDensityModel : MBGameModel<BanditDensityModel>
	{
		// Token: 0x06001E13 RID: 7699
		public abstract int GetMaxSupportedNumberOfLootersForClan(Clan clan);

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06001E14 RID: 7700
		public abstract int NumberOfMinimumBanditPartiesInAHideoutToInfestIt { get; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06001E15 RID: 7701
		public abstract int NumberOfMaximumBanditPartiesInEachHideout { get; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06001E16 RID: 7702
		public abstract int NumberOfMaximumBanditPartiesAroundEachHideout { get; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06001E17 RID: 7703
		public abstract int NumberOfMaximumHideoutsAtEachBanditFaction { get; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06001E18 RID: 7704
		public abstract int NumberOfInitialHideoutsAtEachBanditFaction { get; }

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06001E19 RID: 7705
		public abstract int NumberOfMinimumBanditTroopsInHideoutMission { get; }

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06001E1A RID: 7706
		public abstract int NumberOfMaximumTroopCountForFirstFightInHideout { get; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06001E1B RID: 7707
		public abstract int NumberOfMaximumTroopCountForBossFightInHideout { get; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06001E1C RID: 7708
		public abstract float SpawnPercentageForFirstFightInHideoutMission { get; }

		// Token: 0x06001E1D RID: 7709
		public abstract int GetMinimumTroopCountForHideoutMission(MobileParty party, bool isAssault);

		// Token: 0x06001E1E RID: 7710
		public abstract int GetMaximumTroopCountForHideoutMission(MobileParty party, bool isAssault);

		// Token: 0x06001E1F RID: 7711
		public abstract bool IsPositionInsideNavalSafeZone(CampaignVec2 position);
	}
}
