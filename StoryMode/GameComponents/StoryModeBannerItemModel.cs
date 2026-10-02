using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace StoryMode.GameComponents
{
	// Token: 0x0200003D RID: 61
	public class StoryModeBannerItemModel : BannerItemModel
	{
		// Token: 0x0600041C RID: 1052 RVA: 0x00018EC7 File Offset: 0x000170C7
		public override IEnumerable<ItemObject> GetPossibleRewardBannerItems()
		{
			if (!StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted)
			{
				return new List<ItemObject>();
			}
			return base.BaseModel.GetPossibleRewardBannerItems().WhereQ<ItemObject>((ItemObject i) => !this.IsItemDragonBanner(i));
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00018F01 File Offset: 0x00017101
		public override bool CanBannerBeUpdated(ItemObject item)
		{
			return !this.IsItemDragonBanner(item) && base.BaseModel.CanBannerBeUpdated(item);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00018F1C File Offset: 0x0001711C
		private bool IsItemDragonBanner(ItemObject item)
		{
			return item.StringId == "dragon_banner" || item.StringId == "dragon_banner_center" || item.StringId == "dragon_banner_dragonhead" || item.StringId == "dragon_banner_handle";
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00018F71 File Offset: 0x00017171
		public override IEnumerable<ItemObject> GetPossibleRewardBannerItemsForHero(Hero hero)
		{
			return base.BaseModel.GetPossibleRewardBannerItemsForHero(hero).WhereQ<ItemObject>((ItemObject b) => !this.IsItemDragonBanner(b));
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00018F90 File Offset: 0x00017190
		public override int GetBannerItemLevelForHero(Hero hero)
		{
			return base.BaseModel.GetBannerItemLevelForHero(hero);
		}
	}
}
