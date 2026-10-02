using System;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000415 RID: 1045
	public class HeroDailyXpCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600429A RID: 17050 RVA: 0x00130110 File Offset: 0x0012E310
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(HeroDailyXpCampaignBehavior.DailyTickHero));
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x00130129 File Offset: 0x0012E329
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x0013012C File Offset: 0x0012E32C
		private static void DailyTickHero(Hero hero)
		{
			if (!HeroDailyXpCampaignBehavior.IsEligibleForDailyXp(hero))
			{
				return;
			}
			float num = (hero.IsWanderer ? 100f : 200f);
			foreach (SkillObject skillObject in Skills.All)
			{
				if (hero.HeroDeveloper.GetFocus(skillObject) > 0)
				{
					hero.HeroDeveloper.AddSkillXp(skillObject, num, true, false);
				}
			}
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x001301B4 File Offset: 0x0012E3B4
		private static bool IsEligibleForDailyXp(Hero hero)
		{
			if (hero != Hero.MainHero && hero.IsActive && !hero.IsChild && !hero.IsTemplate)
			{
				MobileParty partyBelongedTo = hero.PartyBelongedTo;
				if (((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) == null)
				{
					Settlement currentSettlement = hero.CurrentSettlement;
					return (currentSettlement == null || !currentSettlement.IsFortification || hero.GovernorOf == hero.CurrentSettlement.Town) && (hero.IsLord || hero.IsWanderer || hero.IsPlayerCompanion);
				}
			}
			return false;
		}

		// Token: 0x040013DE RID: 5086
		private const float XpPerFocusPointForLords = 200f;

		// Token: 0x040013DF RID: 5087
		private const float XpPerFocusPointForWanderers = 100f;
	}
}
