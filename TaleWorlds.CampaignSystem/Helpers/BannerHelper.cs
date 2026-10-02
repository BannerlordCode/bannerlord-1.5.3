using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x0200001E RID: 30
	public static class BannerHelper
	{
		// Token: 0x0600010C RID: 268 RVA: 0x0000DC14 File Offset: 0x0000BE14
		public static ItemObject GetRandomBannerItemForHero(Hero hero)
		{
			return Campaign.Current.Models.BannerItemModel.GetPossibleRewardBannerItemsForHero(hero).GetRandomElementInefficiently<ItemObject>();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000DC30 File Offset: 0x0000BE30
		public static void AddBannerBonusForBanner(BannerEffect bannerEffect, BannerComponent bannerComponent, ref ExplainedNumber bonuses)
		{
			if (bannerComponent != null && bannerComponent.BannerEffect == bannerEffect)
			{
				BannerHelper.AddBannerEffectToStat(ref bonuses, bannerEffect.IncrementType, bannerComponent.GetBannerEffectBonus(), bannerEffect.Name);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000DC56 File Offset: 0x0000BE56
		private static void AddBannerEffectToStat(ref ExplainedNumber stat, EffectIncrementType effectIncrementType, float number, TextObject effectName)
		{
			if (effectIncrementType == EffectIncrementType.Add)
			{
				stat.Add(number, effectName, null);
				return;
			}
			if (effectIncrementType == EffectIncrementType.AddFactor)
			{
				stat.AddFactor(number, effectName);
			}
		}
	}
}
