using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000017 RID: 23
	public struct BannerIconData
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004BA8 File Offset: 0x00002DA8
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00004BB0 File Offset: 0x00002DB0
		public string MaterialName { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004BB9 File Offset: 0x00002DB9
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004BC1 File Offset: 0x00002DC1
		public int TextureIndex { get; private set; }

		// Token: 0x060000F9 RID: 249 RVA: 0x00004BCA File Offset: 0x00002DCA
		public BannerIconData(string materialName, int textureIndex)
		{
			this.MaterialName = materialName;
			this.TextureIndex = textureIndex;
		}
	}
}
