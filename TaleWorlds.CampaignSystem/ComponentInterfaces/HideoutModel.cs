using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BC RID: 444
	public abstract class HideoutModel : MBGameModel<HideoutModel>
	{
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06001E0C RID: 7692
		public abstract CampaignTime HideoutHiddenDuration { get; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06001E0D RID: 7693
		public abstract int CanAttackHideoutStartTime { get; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06001E0E RID: 7694
		public abstract int CanAttackHideoutEndTime { get; }

		// Token: 0x06001E0F RID: 7695
		public abstract float GetRogueryXpGainAsGhost();

		// Token: 0x06001E10 RID: 7696
		public abstract float GetRogueryXpGainOnHideoutMissionEnd(bool isSucceeded);

		// Token: 0x06001E11 RID: 7697
		public abstract float GetSendTroopsSuccessChance(Hideout hideout);
	}
}
