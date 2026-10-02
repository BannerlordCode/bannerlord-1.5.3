using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E4 RID: 228
	public class BannerImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000B93 RID: 2963 RVA: 0x000256DA File Offset: 0x000238DA
		public BannerImageIdentifier(Banner banner, bool nineGrid = false)
		{
			base.Id = ((banner != null) ? banner.BannerCode : "");
			base.AdditionalArgs = (nineGrid ? "ninegrid" : "");
			base.TextureProviderName = "BannerImageTextureProvider";
		}
	}
}
