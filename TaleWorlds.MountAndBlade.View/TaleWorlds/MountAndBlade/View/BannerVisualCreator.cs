using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000011 RID: 17
	public class BannerVisualCreator : IBannerVisualCreator
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00003F19 File Offset: 0x00002119
		IBannerVisual IBannerVisualCreator.CreateBannerVisual(Banner banner)
		{
			return new BannerVisual(banner);
		}
	}
}
