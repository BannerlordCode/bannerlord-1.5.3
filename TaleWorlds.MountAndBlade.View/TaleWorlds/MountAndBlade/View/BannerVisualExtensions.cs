using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000010 RID: 16
	public static class BannerVisualExtensions
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00003E7D File Offset: 0x0000207D
		public static Texture GetTableauTextureSmallForBannerEditor(this Banner banner, in BannerDebugInfo debugInfo, Action<Texture> setAction, out BannerEditorTextureCreationData textureCreationData)
		{
			textureCreationData = new BannerEditorTextureCreationData(banner, setAction, null, debugInfo, true, false);
			return ThumbnailCacheManager.Current.CreateTexture(textureCreationData).Texture;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003EA2 File Offset: 0x000020A2
		public static Texture GetTableauTextureLargeForBannerEditor(this Banner banner, in BannerDebugInfo debugInfo, Action<Texture> setAction, out BannerEditorTextureCreationData textureCreationData)
		{
			textureCreationData = new BannerEditorTextureCreationData(banner, setAction, null, debugInfo, true, true);
			return ThumbnailCacheManager.Current.CreateTexture(textureCreationData).Texture;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003EC7 File Offset: 0x000020C7
		public static Texture GetTableauTextureSmall(this Banner banner, in BannerDebugInfo debugInfo, Action<Texture> setAction)
		{
			return ((BannerVisual)banner.BannerVisual).GetTableauTextureSmall(in debugInfo, setAction, true);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003EDC File Offset: 0x000020DC
		public static Texture GetTableauTextureLarge(this Banner banner, in BannerDebugInfo debugInfo, Action<Texture> setAction)
		{
			return ((BannerVisual)banner.BannerVisual).GetTableauTextureLarge(in debugInfo, setAction, true);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003EF1 File Offset: 0x000020F1
		public static Texture GetTableauTextureLarge(this Banner banner, in BannerDebugInfo debugInfo, Action<Texture> setAction, out BannerTextureCreationData creationData)
		{
			return ((BannerVisual)banner.BannerVisual).GetTableauTextureLarge(in debugInfo, setAction, out creationData, true);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003F07 File Offset: 0x00002107
		public static MetaMesh ConvertToMultiMesh(this Banner banner)
		{
			return ((BannerVisual)banner.BannerVisual).ConvertToMultiMesh();
		}
	}
}
