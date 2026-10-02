using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FB RID: 507
	public abstract class BattleCaptainModel : MBGameModel<BattleCaptainModel>
	{
		// Token: 0x06001FF3 RID: 8179
		public abstract float GetCaptainRatingForTroopUsages(Hero hero, TroopUsageFlags flag, BattleEnvironment battleEnvironment, out List<PerkObject> compatiblePerks);
	}
}
