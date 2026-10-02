using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DB RID: 475
	public abstract class VassalRewardsModel : MBGameModel<VassalRewardsModel>
	{
		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001F0C RID: 7948
		public abstract float InfluenceReward { get; }

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06001F0D RID: 7949
		public abstract int RelationRewardWithLeader { get; }

		// Token: 0x06001F0E RID: 7950
		public abstract TroopRoster GetTroopRewardsForJoiningKingdom(Kingdom kingdom);

		// Token: 0x06001F0F RID: 7951
		public abstract ItemRoster GetEquipmentRewardsForJoiningKingdom(Kingdom kingdom);
	}
}
