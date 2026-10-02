using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000040 RID: 64
	public class BannerTextureCreationData : BannerThumbnailCreationBaseData
	{
		// Token: 0x0600023E RID: 574 RVA: 0x0000F1E1 File Offset: 0x0000D3E1
		public BannerTextureCreationData(Banner banner, Action<Texture> setAction, Action cancelAction, BannerDebugInfo debugInfo, bool isTableauOrNineGrid, bool isLarge)
			: base(banner, setAction, cancelAction, debugInfo, isTableauOrNineGrid, isLarge)
		{
			base.RenderId = "Mesh_" + base.RenderId;
		}
	}
}
