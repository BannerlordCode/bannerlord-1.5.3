using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000034 RID: 52
	public class Texture
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000961A File Offset: 0x0000781A
		// (set) Token: 0x06000260 RID: 608 RVA: 0x00009622 File Offset: 0x00007822
		public ITexture PlatformTexture { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000962B File Offset: 0x0000782B
		public bool IsValid
		{
			get
			{
				return this.PlatformTexture.IsValid;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00009638 File Offset: 0x00007838
		public int Width
		{
			get
			{
				return this.PlatformTexture.Width;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00009645 File Offset: 0x00007845
		public int Height
		{
			get
			{
				return this.PlatformTexture.Height;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009652 File Offset: 0x00007852
		public Texture(ITexture platformTexture)
		{
			this.PlatformTexture = platformTexture;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009661 File Offset: 0x00007861
		public bool IsLoaded()
		{
			return this.PlatformTexture.IsLoaded();
		}
	}
}
