using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F9 RID: 249
	public class DefaultBannerItemModel : BannerItemModel
	{
		// Token: 0x060016EB RID: 5867 RVA: 0x0006A6B9 File Offset: 0x000688B9
		public override IEnumerable<ItemObject> GetPossibleRewardBannerItems()
		{
			return Items.All.WhereQ<ItemObject>((ItemObject i) => i.IsBannerItem && i.StringId != "campaign_banner_small");
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x0006A6E4 File Offset: 0x000688E4
		public override IEnumerable<ItemObject> GetPossibleRewardBannerItemsForHero(Hero hero)
		{
			IEnumerable<ItemObject> possibleRewardBannerItems = this.GetPossibleRewardBannerItems();
			int bannerItemLevelForHero = this.GetBannerItemLevelForHero(hero);
			List<ItemObject> list = new List<ItemObject>();
			foreach (ItemObject itemObject in possibleRewardBannerItems)
			{
				if ((itemObject.Culture == null || itemObject.Culture == hero.Culture) && (itemObject.ItemComponent as BannerComponent).BannerLevel == bannerItemLevelForHero)
				{
					list.Add(itemObject);
				}
			}
			return list;
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x0006A76C File Offset: 0x0006896C
		public override int GetBannerItemLevelForHero(Hero hero)
		{
			if (hero.Clan == null || hero.Clan.Leader != hero)
			{
				return 1;
			}
			if (hero.MapFaction.IsKingdomFaction && hero.Clan.Kingdom.RulingClan == hero.Clan)
			{
				return 3;
			}
			return 2;
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x0006A7B9 File Offset: 0x000689B9
		public override bool CanBannerBeUpdated(ItemObject item)
		{
			return true;
		}

		// Token: 0x040007A3 RID: 1955
		public const int BannerLevel1 = 1;

		// Token: 0x040007A4 RID: 1956
		public const int BannerLevel2 = 2;

		// Token: 0x040007A5 RID: 1957
		public const int BannerLevel3 = 3;

		// Token: 0x040007A6 RID: 1958
		private const string MapBannerId = "campaign_banner_small";
	}
}
