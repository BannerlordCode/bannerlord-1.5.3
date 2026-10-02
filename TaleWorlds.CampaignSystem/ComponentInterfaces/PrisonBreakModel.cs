using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FA RID: 506
	public abstract class PrisonBreakModel : MBGameModel<PrisonBreakModel>
	{
		// Token: 0x06001FED RID: 8173
		public abstract int GetNumberOfGuardsToSpawn(Settlement settlement);

		// Token: 0x06001FEE RID: 8174
		public abstract bool CanPlayerStagePrisonBreak(Settlement settlement);

		// Token: 0x06001FEF RID: 8175
		public abstract int GetPrisonBreakStartCost(Hero prisonerHero);

		// Token: 0x06001FF0 RID: 8176
		public abstract int GetRelationRewardOnPrisonBreak(Hero prisonerHero);

		// Token: 0x06001FF1 RID: 8177
		public abstract float GetRogueryRewardOnPrisonBreak(Hero prisonerHero, bool isSuccess);
	}
}
