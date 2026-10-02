using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004B9 RID: 1209
	public static class AdoptHeroAction
	{
		// Token: 0x06004CFB RID: 19707 RVA: 0x00185079 File Offset: 0x00183279
		private static void ApplyInternal(Hero adoptedHero)
		{
			if (Hero.MainHero.IsFemale)
			{
				adoptedHero.Mother = Hero.MainHero;
			}
			else
			{
				adoptedHero.Father = Hero.MainHero;
			}
			adoptedHero.Clan = Clan.PlayerClan;
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x001850AA File Offset: 0x001832AA
		public static void Apply(Hero adoptedHero)
		{
			AdoptHeroAction.ApplyInternal(adoptedHero);
		}
	}
}
