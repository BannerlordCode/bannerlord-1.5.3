using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003FA RID: 1018
	public class CharacterDevelopmentCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003F68 RID: 16232 RVA: 0x001107EC File Offset: 0x0010E9EC
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action<int>(this.OnCharacterCreationIsOver));
			CampaignEvents.OnHeroActivatedEvent.AddNonSerializedListener(this, new Action<Hero, Hero.CharacterStates>(this.OnHeroActivated));
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x0011083E File Offset: 0x0010EA3E
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x00110840 File Offset: 0x0010EA40
		private void DailyTickHero(Hero hero)
		{
			if (this.ShouldDevelopCharacterStats(hero))
			{
				hero.HeroDeveloper.DevelopCharacterStats();
			}
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x00110858 File Offset: 0x0010EA58
		private void OnCharacterCreationIsOver(int index)
		{
			if (index == 1 && CampaignOptions.AutoAllocateClanMemberPerks)
			{
				foreach (Hero hero in Campaign.Current.AliveHeroes)
				{
					if (!hero.IsChild && hero.Clan == Clan.PlayerClan && hero != Hero.MainHero)
					{
						hero.HeroDeveloper.DevelopCharacterStats();
					}
				}
			}
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x001108DC File Offset: 0x0010EADC
		private void OnHeroActivated(Hero hero, Hero.CharacterStates previousState)
		{
			if (this.ShouldDevelopCharacterStats(hero))
			{
				hero.HeroDeveloper.DevelopCharacterStats();
			}
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x001108F4 File Offset: 0x0010EAF4
		private bool ShouldDevelopCharacterStats(Hero hero)
		{
			if (!hero.IsChild && hero.IsAlive && (hero.Clan != Clan.PlayerClan || (hero != Hero.MainHero && CampaignOptions.AutoAllocateClanMemberPerks)))
			{
				MobileParty partyBelongedTo = hero.PartyBelongedTo;
				return ((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) == null;
			}
			return false;
		}
	}
}
