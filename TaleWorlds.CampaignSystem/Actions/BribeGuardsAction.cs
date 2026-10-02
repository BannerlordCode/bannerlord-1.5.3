using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004BD RID: 1213
	public static class BribeGuardsAction
	{
		// Token: 0x06004D0A RID: 19722 RVA: 0x00185C1C File Offset: 0x00183E1C
		private static void ApplyInternal(Settlement settlement, int gold)
		{
			if (gold > 0)
			{
				if (MBRandom.RandomFloat < (float)gold / 1000f)
				{
					SkillLevelingManager.OnBribeGiven(gold);
				}
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, gold, false);
				settlement.BribePaid += gold;
			}
		}

		// Token: 0x06004D0B RID: 19723 RVA: 0x00185C52 File Offset: 0x00183E52
		public static void Apply(Settlement settlement, int gold)
		{
			BribeGuardsAction.ApplyInternal(settlement, gold);
		}
	}
}
