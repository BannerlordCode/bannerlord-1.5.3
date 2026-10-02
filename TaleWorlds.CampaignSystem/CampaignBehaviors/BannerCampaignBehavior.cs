using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003EE RID: 1006
	public class BannerCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003CF5 RID: 15605 RVA: 0x000FB9DC File Offset: 0x000F9BDC
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.GiveBannersToHeroes));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnCollectLootsItemsEvent.AddNonSerializedListener(this, new Action<PartyBase, ItemRoster>(this.OnCollectLootItems));
			CampaignEvents.HeroComesOfAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroComesOfAge));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.OnClanCreatedEvent.AddNonSerializedListener(this, new Action<Clan, bool>(this.OnClanCreated));
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x000FBA8A File Offset: 0x000F9C8A
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Hero, CampaignTime>>("_heroNextBannerLootTime", ref this._heroNextBannerLootTime);
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x000FBA9E File Offset: 0x000F9C9E
		private void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.GiveBannersToHeroes();
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x000FBAA8 File Offset: 0x000F9CA8
		private void GiveBannersToHeroes()
		{
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (this.CanBannerBeGivenToHero(hero))
				{
					ItemObject randomBannerItemForHero = BannerHelper.GetRandomBannerItemForHero(hero);
					if (randomBannerItemForHero != null)
					{
						hero.BannerItem = new EquipmentElement(randomBannerItemForHero, null, null, false);
					}
				}
			}
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x000FBB18 File Offset: 0x000F9D18
		private void DailyTickHero(Hero hero)
		{
			if (hero.Clan != Clan.PlayerClan)
			{
				EquipmentElement bannerItem = hero.BannerItem;
				BannerItemModel bannerItemModel = Campaign.Current.Models.BannerItemModel;
				if (!bannerItem.IsInvalid() && bannerItemModel.CanBannerBeUpdated(bannerItem.Item) && MBRandom.RandomFloat < 0.1f)
				{
					int bannerLevel = ((BannerComponent)bannerItem.Item.ItemComponent).BannerLevel;
					int bannerItemLevelForHero = bannerItemModel.GetBannerItemLevelForHero(hero);
					if (bannerLevel != bannerItemLevelForHero)
					{
						ItemObject upgradeBannerForHero = this.GetUpgradeBannerForHero(hero, bannerItemLevelForHero);
						if (upgradeBannerForHero != null)
						{
							hero.BannerItem = new EquipmentElement(upgradeBannerForHero, null, null, false);
							return;
						}
					}
				}
				else if (bannerItem.IsInvalid() && this.CanBannerBeGivenToHero(hero) && MBRandom.RandomFloat < 0.25f && !hero.IsPrisoner)
				{
					ItemObject randomBannerItemForHero = BannerHelper.GetRandomBannerItemForHero(hero);
					if (randomBannerItemForHero != null)
					{
						hero.BannerItem = new EquipmentElement(randomBannerItemForHero, null, null, false);
					}
				}
			}
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x000FBBF0 File Offset: 0x000F9DF0
		private ItemObject GetUpgradeBannerForHero(Hero hero, int upgradeBannerLevel)
		{
			ItemObject item = hero.BannerItem.Item;
			foreach (ItemObject itemObject in Campaign.Current.Models.BannerItemModel.GetPossibleRewardBannerItems())
			{
				BannerComponent bannerComponent = (BannerComponent)itemObject.ItemComponent;
				if (itemObject.Culture == item.Culture && bannerComponent.BannerLevel == upgradeBannerLevel && bannerComponent.BannerEffect == ((BannerComponent)item.ItemComponent).BannerEffect)
				{
					return itemObject;
				}
			}
			return BannerHelper.GetRandomBannerItemForHero(hero);
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x000FBCA0 File Offset: 0x000F9EA0
		private void OnCollectLootItems(PartyBase winnerParty, ItemRoster gainedLoots)
		{
			if (winnerParty == PartyBase.MainParty)
			{
				MapEvent mapEvent = MobileParty.MainParty.MapEvent;
				ItemObject bannerRewardForWinningMapEvent = Campaign.Current.Models.BattleRewardModel.GetBannerRewardForWinningMapEvent(mapEvent);
				if (bannerRewardForWinningMapEvent != null)
				{
					gainedLoots.AddToCounts(bannerRewardForWinningMapEvent, 1);
				}
				Hero hero = null;
				MBReadOnlyList<MapEventParty> mbreadOnlyList = mapEvent.PartiesOnSide(mapEvent.DefeatedSide);
				if (mbreadOnlyList.Exists((MapEventParty x) => x.Party.IsMobile && x.Party.MobileParty.Army != null))
				{
					foreach (MapEventParty mapEventParty in mbreadOnlyList)
					{
						if (mapEventParty.Party.IsMobile && mapEventParty.Party.MobileParty.Army != null && !mapEventParty.Party.MobileParty.Army.ArmyOwner.BannerItem.IsInvalid() && this.CanBannerBeLootedFromHero(mapEventParty.Party.MobileParty.Army.ArmyOwner))
						{
							hero = mapEventParty.Party.MobileParty.Army.ArmyOwner;
							break;
						}
					}
				}
				if (hero == null)
				{
					MapEventParty randomElementWithPredicate = mbreadOnlyList.GetRandomElementWithPredicate<MapEventParty>((MapEventParty x) => x.Party.LeaderHero != null && !x.Party.LeaderHero.BannerItem.IsInvalid() && this.CanBannerBeLootedFromHero(x.Party.LeaderHero));
					hero = ((randomElementWithPredicate != null) ? randomElementWithPredicate.Party.LeaderHero : null);
				}
				if (hero != null)
				{
					float bannerLootChanceFromDefeatedHero = Campaign.Current.Models.BattleRewardModel.GetBannerLootChanceFromDefeatedHero(hero);
					if (MBRandom.RandomFloat <= bannerLootChanceFromDefeatedHero)
					{
						this.LogBannerLootForHero(hero, ((BannerComponent)hero.BannerItem.Item.ItemComponent).BannerLevel);
						gainedLoots.AddToCounts(hero.BannerItem.Item, 1);
						hero.BannerItem = new EquipmentElement(null, null, null, false);
					}
				}
			}
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x000FBE78 File Offset: 0x000FA078
		private void OnHeroComesOfAge(Hero hero)
		{
			if (this.CanBannerBeGivenToHero(hero))
			{
				ItemObject randomBannerItemForHero = BannerHelper.GetRandomBannerItemForHero(hero);
				if (randomBannerItemForHero != null)
				{
					hero.BannerItem = new EquipmentElement(randomBannerItemForHero, null, null, false);
				}
			}
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x000FBEA8 File Offset: 0x000FA0A8
		private void OnHeroCreated(Hero hero, bool isBornNaturally = false)
		{
			if (this.CanBannerBeGivenToHero(hero))
			{
				ItemObject randomBannerItemForHero = BannerHelper.GetRandomBannerItemForHero(hero);
				if (randomBannerItemForHero != null)
				{
					hero.BannerItem = new EquipmentElement(randomBannerItemForHero, null, null, false);
				}
			}
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x000FBED8 File Offset: 0x000FA0D8
		private void OnClanCreated(Clan clan, bool isCompanion)
		{
			if (isCompanion)
			{
				Hero leader = clan.Leader;
				if (leader.BannerItem.IsInvalid())
				{
					ItemObject randomBannerItemForHero = BannerHelper.GetRandomBannerItemForHero(leader);
					if (randomBannerItemForHero != null)
					{
						leader.BannerItem = new EquipmentElement(randomBannerItemForHero, null, null, false);
					}
				}
			}
		}

		// Token: 0x06003CFF RID: 15615 RVA: 0x000FBF18 File Offset: 0x000FA118
		private bool CanBannerBeLootedFromHero(Hero hero)
		{
			return !this._heroNextBannerLootTime.ContainsKey(hero) || this._heroNextBannerLootTime[hero].IsPast;
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x000FBF49 File Offset: 0x000FA149
		private int GetCooldownDays(int bannerLevel)
		{
			if (bannerLevel == 1)
			{
				return 4;
			}
			if (bannerLevel == 1)
			{
				return 8;
			}
			return 12;
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x000FBF5C File Offset: 0x000FA15C
		private void LogBannerLootForHero(Hero hero, int bannerLevel)
		{
			CampaignTime campaignTime = CampaignTime.DaysFromNow((float)this.GetCooldownDays(bannerLevel));
			if (!this._heroNextBannerLootTime.ContainsKey(hero))
			{
				this._heroNextBannerLootTime.Add(hero, campaignTime);
				return;
			}
			this._heroNextBannerLootTime[hero] = campaignTime;
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x000FBFA0 File Offset: 0x000FA1A0
		private bool CanBannerBeGivenToHero(Hero hero)
		{
			int heroComesOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
			return hero.Occupation == Occupation.Lord && hero.Age >= (float)heroComesOfAge && hero.BannerItem.IsInvalid() && hero.Clan != Clan.PlayerClan;
		}

		// Token: 0x040012AD RID: 4781
		private const int BannerLevel1CooldownDays = 4;

		// Token: 0x040012AE RID: 4782
		private const int BannerLevel2CooldownDays = 8;

		// Token: 0x040012AF RID: 4783
		private const int BannerLevel3CooldownDays = 12;

		// Token: 0x040012B0 RID: 4784
		private const float BannerItemUpdateChance = 0.1f;

		// Token: 0x040012B1 RID: 4785
		private const float GiveBannerItemChance = 0.25f;

		// Token: 0x040012B2 RID: 4786
		private Dictionary<Hero, CampaignTime> _heroNextBannerLootTime = new Dictionary<Hero, CampaignTime>();
	}
}
