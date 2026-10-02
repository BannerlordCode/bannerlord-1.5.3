using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000010 RID: 16
	public static class BannerExtensions
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00004108 File Offset: 0x00002308
		public static bool IsContentsSameWith(this Banner banner, Banner otherBanner)
		{
			if (banner == null && otherBanner == null)
			{
				return true;
			}
			if (banner == null || otherBanner == null)
			{
				return false;
			}
			if (banner.GetBannerDataListCount() != otherBanner.GetBannerDataListCount())
			{
				return false;
			}
			for (int i = 0; i < banner.GetBannerDataListCount(); i++)
			{
				object bannerDataAtIndex = banner.GetBannerDataAtIndex(i);
				BannerData bannerDataAtIndex2 = otherBanner.GetBannerDataAtIndex(i);
				if (!bannerDataAtIndex.Equals(bannerDataAtIndex2))
				{
					return false;
				}
			}
			return true;
		}
	}
}
