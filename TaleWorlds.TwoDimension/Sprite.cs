using System;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200002E RID: 46
	public abstract class Sprite
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000204 RID: 516
		public abstract Texture Texture { get; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000084CA File Offset: 0x000066CA
		// (set) Token: 0x06000206 RID: 518 RVA: 0x000084D2 File Offset: 0x000066D2
		public string Name { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000207 RID: 519 RVA: 0x000084DB File Offset: 0x000066DB
		// (set) Token: 0x06000208 RID: 520 RVA: 0x000084E3 File Offset: 0x000066E3
		public int Width { get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000209 RID: 521 RVA: 0x000084EC File Offset: 0x000066EC
		// (set) Token: 0x0600020A RID: 522 RVA: 0x000084F4 File Offset: 0x000066F4
		public int Height { get; private set; }

		// Token: 0x0600020B RID: 523
		public abstract Vec2 GetMinUvs();

		// Token: 0x0600020C RID: 524
		public abstract Vec2 GetMaxUvs();

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600020D RID: 525 RVA: 0x000084FD File Offset: 0x000066FD
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00008505 File Offset: 0x00006705
		public SpriteNinePatchParameters NinePatchParameters { get; private set; }

		// Token: 0x0600020F RID: 527 RVA: 0x0000850E File Offset: 0x0000670E
		protected Sprite(string name, int width, int height, SpriteNinePatchParameters ninePatchParameters)
		{
			this.Name = name;
			this.Width = width;
			this.Height = height;
			this.NinePatchParameters = ninePatchParameters;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00008533 File Offset: 0x00006733
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				return base.ToString();
			}
			return this.Name;
		}
	}
}
