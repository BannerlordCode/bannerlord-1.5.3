using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MBHelpers
{
	// Token: 0x020000E1 RID: 225
	public static class BannerHelper
	{
		// Token: 0x0600092E RID: 2350 RVA: 0x0000F9AE File Offset: 0x0000DBAE
		public static void AddBannerBonusForBanner(BannerEffect bannerEffect, BannerComponent bannerComponent, ref FactoredNumber bonuses)
		{
			if (bannerComponent != null && bannerComponent.BannerEffect == bannerEffect)
			{
				BannerHelper.AddBannerEffectToStat(ref bonuses, bannerEffect.IncrementType, bannerComponent.GetBannerEffectBonus());
			}
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0000F9CE File Offset: 0x0000DBCE
		private static void AddBannerEffectToStat(ref FactoredNumber stat, EffectIncrementType effectIncrementType, float number)
		{
			if (effectIncrementType == EffectIncrementType.Add)
			{
				stat.Add(number);
				return;
			}
			if (effectIncrementType == EffectIncrementType.AddFactor)
			{
				stat.AddFactor(number);
			}
		}
	}
}
