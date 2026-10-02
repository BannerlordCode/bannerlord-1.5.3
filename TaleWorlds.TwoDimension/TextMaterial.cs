using System;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000027 RID: 39
	public class TextMaterial : Material
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000073E4 File Offset: 0x000055E4
		// (set) Token: 0x0600019E RID: 414 RVA: 0x000073EC File Offset: 0x000055EC
		public Texture Texture { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000073F5 File Offset: 0x000055F5
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x000073FD File Offset: 0x000055FD
		public Color Color { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00007406 File Offset: 0x00005606
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x0000740E File Offset: 0x0000560E
		public float SmoothingConstant { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00007417 File Offset: 0x00005617
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x0000741F File Offset: 0x0000561F
		public bool Smooth { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00007428 File Offset: 0x00005628
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00007430 File Offset: 0x00005630
		public float ScaleFactor { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00007439 File Offset: 0x00005639
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00007441 File Offset: 0x00005641
		public Color GlowColor { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000744A File Offset: 0x0000564A
		// (set) Token: 0x060001AA RID: 426 RVA: 0x00007452 File Offset: 0x00005652
		public Color OutlineColor { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000745B File Offset: 0x0000565B
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00007463 File Offset: 0x00005663
		public float OutlineAmount { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000746C File Offset: 0x0000566C
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00007474 File Offset: 0x00005674
		public float GlowRadius { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000747D File Offset: 0x0000567D
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00007485 File Offset: 0x00005685
		public float Blur { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000748E File Offset: 0x0000568E
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00007496 File Offset: 0x00005696
		public float ShadowOffset { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000749F File Offset: 0x0000569F
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x000074A7 File Offset: 0x000056A7
		public float ShadowAngle { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x000074B0 File Offset: 0x000056B0
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x000074B8 File Offset: 0x000056B8
		public float ColorFactor { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x000074C1 File Offset: 0x000056C1
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x000074C9 File Offset: 0x000056C9
		public float AlphaFactor { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x000074D2 File Offset: 0x000056D2
		// (set) Token: 0x060001BA RID: 442 RVA: 0x000074DA File Offset: 0x000056DA
		public float HueFactor { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000074E3 File Offset: 0x000056E3
		// (set) Token: 0x060001BC RID: 444 RVA: 0x000074EB File Offset: 0x000056EB
		public float SaturationFactor { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000074F4 File Offset: 0x000056F4
		// (set) Token: 0x060001BE RID: 446 RVA: 0x000074FC File Offset: 0x000056FC
		public float ValueFactor { get; set; }

		// Token: 0x060001BF RID: 447 RVA: 0x00007505 File Offset: 0x00005705
		public TextMaterial()
			: this(null, 0)
		{
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000750F File Offset: 0x0000570F
		public TextMaterial(Texture texture)
			: this(texture, 0)
		{
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00007519 File Offset: 0x00005719
		public TextMaterial(Texture texture, int renderOrder)
			: this(texture, renderOrder, true)
		{
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00007524 File Offset: 0x00005724
		public TextMaterial(Texture texture, int renderOrder, bool blending)
			: base(blending, renderOrder)
		{
			this.Texture = texture;
			this.ScaleFactor = 1f;
			this.SmoothingConstant = 0.47f;
			this.Smooth = true;
			this.Color = new Color(1f, 1f, 1f, 1f);
			this.GlowColor = new Color(0f, 0f, 0f, 1f);
			this.OutlineColor = new Color(0f, 0f, 0f, 1f);
			this.OutlineAmount = 0f;
			this.GlowRadius = 0f;
			this.Blur = 0f;
			this.ShadowOffset = 0f;
			this.ShadowAngle = 0f;
			this.ColorFactor = 1f;
			this.AlphaFactor = 1f;
			this.HueFactor = 0f;
			this.SaturationFactor = 0f;
			this.ValueFactor = 0f;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00007628 File Offset: 0x00005828
		public void CopyFrom(TextMaterial sourceMaterial)
		{
			this.Texture = sourceMaterial.Texture;
			this.Color = sourceMaterial.Color;
			this.ScaleFactor = sourceMaterial.ScaleFactor;
			this.SmoothingConstant = sourceMaterial.SmoothingConstant;
			this.Smooth = sourceMaterial.Smooth;
			this.GlowColor = sourceMaterial.GlowColor;
			this.OutlineColor = sourceMaterial.OutlineColor;
			this.OutlineAmount = sourceMaterial.OutlineAmount;
			this.GlowRadius = sourceMaterial.GlowRadius;
			this.Blur = sourceMaterial.Blur;
			this.ShadowOffset = sourceMaterial.ShadowOffset;
			this.ShadowAngle = sourceMaterial.ShadowAngle;
			this.ColorFactor = sourceMaterial.ColorFactor;
			this.AlphaFactor = sourceMaterial.AlphaFactor;
			this.HueFactor = sourceMaterial.HueFactor;
			this.SaturationFactor = sourceMaterial.SaturationFactor;
			this.ValueFactor = sourceMaterial.ValueFactor;
		}
	}
}
