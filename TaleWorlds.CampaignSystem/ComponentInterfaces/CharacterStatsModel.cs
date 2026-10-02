using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018E RID: 398
	public abstract class CharacterStatsModel : MBGameModel<CharacterStatsModel>
	{
		// Token: 0x06001C7F RID: 7295
		public abstract ExplainedNumber MaxHitpoints(CharacterObject character, bool includeDescriptions = false);

		// Token: 0x06001C80 RID: 7296
		public abstract int GetTier(CharacterObject character);

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06001C81 RID: 7297
		public abstract int MaxCharacterTier { get; }

		// Token: 0x06001C82 RID: 7298
		public abstract int WoundedHitPointLimit(Hero hero);
	}
}
