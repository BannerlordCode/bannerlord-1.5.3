using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000019 RID: 25
	public struct BannerColor
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000540C File Offset: 0x0000360C
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00005414 File Offset: 0x00003614
		public uint Color { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000113 RID: 275 RVA: 0x0000541D File Offset: 0x0000361D
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00005425 File Offset: 0x00003625
		public bool PlayerCanChooseForSigil { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000542E File Offset: 0x0000362E
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00005436 File Offset: 0x00003636
		public bool PlayerCanChooseForBackground { get; private set; }

		// Token: 0x06000117 RID: 279 RVA: 0x0000543F File Offset: 0x0000363F
		public BannerColor(uint color, bool playerCanChooseForSigil, bool playerCanChooseForBackground)
		{
			this.Color = color;
			this.PlayerCanChooseForSigil = playerCanChooseForSigil;
			this.PlayerCanChooseForBackground = playerCanChooseForBackground;
		}
	}
}
