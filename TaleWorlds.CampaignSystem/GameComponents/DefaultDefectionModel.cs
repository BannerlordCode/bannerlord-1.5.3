using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000114 RID: 276
	public class DefaultDefectionModel : DefectionModel
	{
		// Token: 0x060017FB RID: 6139 RVA: 0x00071798 File Offset: 0x0006F998
		public override bool CanHeroDefectToFaction(Hero hero, Kingdom kingdom)
		{
			return hero != null && hero.MapFaction != null && hero.MapFaction.IsKingdomFaction && hero.MapFaction != Hero.MainHero.MapFaction && hero.MapFaction.Leader != hero && hero.Clan != null && !hero.Clan.IsMinorFaction && !hero.Clan.IsUnderMercenaryService && hero.Clan.Kingdom != null && (kingdom.RulingClan != Clan.PlayerClan || !hero.Clan.HasBloodFeudWithPlayer) && !hero.IsPrisoner;
		}
	}
}
