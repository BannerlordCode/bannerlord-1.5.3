using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders.ImageIdentifiers
{
	// Token: 0x02000024 RID: 36
	public class BannerImageTextureProvider : ImageIdentifierTextureProvider
	{
		// Token: 0x0600017A RID: 378 RVA: 0x00009120 File Offset: 0x00007320
		protected override void OnCreateImageWithId(string id, string additionalArgs)
		{
			if (string.IsNullOrEmpty(id))
			{
				base.OnTextureCreated(null);
				return;
			}
			Banner banner = new Banner(id);
			BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateWidget(base.SourceInfo ?? base.GetType().Name);
			if (additionalArgs == "ninegrid")
			{
				base.ThumbnailCreationData = new BannerThumbnailCreationData(banner, new Action<Texture>(base.OnTextureCreated), new Action(base.OnTextureCreationCancelled), bannerDebugInfo, true, true);
			}
			else
			{
				base.ThumbnailCreationData = new BannerThumbnailCreationData(banner, new Action<Texture>(base.OnTextureCreated), new Action(base.OnTextureCreationCancelled), bannerDebugInfo, false, false);
			}
			ThumbnailCacheManager.Current.CreateTexture(base.ThumbnailCreationData);
		}
	}
}
