using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F9 RID: 505
	public abstract class DifficultyModel : MBGameModel<DifficultyModel>
	{
		// Token: 0x06001FE4 RID: 8164
		public abstract float GetPlayerTroopsReceivedDamageMultiplier();

		// Token: 0x06001FE5 RID: 8165
		public abstract int GetPlayerRecruitSlotBonus();

		// Token: 0x06001FE6 RID: 8166
		public abstract float GetPlayerMapMovementSpeedBonusMultiplier();

		// Token: 0x06001FE7 RID: 8167
		public abstract float GetCombatAIDifficultyMultiplier();

		// Token: 0x06001FE8 RID: 8168
		public abstract float GetPersuasionBonusChance();

		// Token: 0x06001FE9 RID: 8169
		public abstract float GetClanMemberDeathChanceMultiplier();

		// Token: 0x06001FEA RID: 8170
		public abstract float GetStealthDifficultyMultiplier();

		// Token: 0x06001FEB RID: 8171
		public abstract float GetDisguiseDifficultyMultiplier();
	}
}
