using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000120 RID: 288
	public class DefaultGenericXpModel : GenericXpModel
	{
		// Token: 0x060018A1 RID: 6305 RVA: 0x00076F7C File Offset: 0x0007517C
		public override float GetXpMultiplier(Hero hero)
		{
			float num = 1f;
			if (hero.IsPlayerCompanion && Hero.MainHero.GetPerkValue(DefaultPerks.Charm.NaturalLeader))
			{
				num += 0.2f;
			}
			return num;
		}
	}
}
