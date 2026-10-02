using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200003E RID: 62
	public class BannerEditorTextureCreationData : BannerThumbnailCreationBaseData
	{
		// Token: 0x06000236 RID: 566 RVA: 0x0000F098 File Offset: 0x0000D298
		public BannerEditorTextureCreationData(Banner banner, Action<Texture> setAction, Action cancelAction, BannerDebugInfo debugInfo, bool isTableauOrNineGrid, bool isLarge)
			: base(banner, setAction, cancelAction, debugInfo, isTableauOrNineGrid, isLarge)
		{
		}
	}
}
