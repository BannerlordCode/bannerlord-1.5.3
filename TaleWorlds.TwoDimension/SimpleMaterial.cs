using System;
using System.Numerics;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000026 RID: 38
	public class SimpleMaterial : Material
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000717D File Offset: 0x0000537D
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00007185 File Offset: 0x00005385
		public Texture Texture { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000718E File Offset: 0x0000538E
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00007196 File Offset: 0x00005396
		public Color Color { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000719F File Offset: 0x0000539F
		// (set) Token: 0x0600016E RID: 366 RVA: 0x000071A7 File Offset: 0x000053A7
		public float ColorFactor { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600016F RID: 367 RVA: 0x000071B0 File Offset: 0x000053B0
		// (set) Token: 0x06000170 RID: 368 RVA: 0x000071B8 File Offset: 0x000053B8
		public float AlphaFactor { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000171 RID: 369 RVA: 0x000071C1 File Offset: 0x000053C1
		// (set) Token: 0x06000172 RID: 370 RVA: 0x000071C9 File Offset: 0x000053C9
		public float HueFactor { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000071D2 File Offset: 0x000053D2
		// (set) Token: 0x06000174 RID: 372 RVA: 0x000071DA File Offset: 0x000053DA
		public float SaturationFactor { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000071E3 File Offset: 0x000053E3
		// (set) Token: 0x06000176 RID: 374 RVA: 0x000071EB File Offset: 0x000053EB
		public float ValueFactor { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000071F4 File Offset: 0x000053F4
		// (set) Token: 0x06000178 RID: 376 RVA: 0x000071FC File Offset: 0x000053FC
		public bool CircularMaskingEnabled { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00007205 File Offset: 0x00005405
		// (set) Token: 0x0600017A RID: 378 RVA: 0x0000720D File Offset: 0x0000540D
		public Vector2 CircularMaskingCenter { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00007216 File Offset: 0x00005416
		// (set) Token: 0x0600017C RID: 380 RVA: 0x0000721E File Offset: 0x0000541E
		public float CircularMaskingRadius { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00007227 File Offset: 0x00005427
		// (set) Token: 0x0600017E RID: 382 RVA: 0x0000722F File Offset: 0x0000542F
		public float CircularMaskingSmoothingRadius { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00007238 File Offset: 0x00005438
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00007240 File Offset: 0x00005440
		public SpriteNinePatchParameters NinePatchParameters { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00007249 File Offset: 0x00005449
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00007251 File Offset: 0x00005451
		public bool OverlayEnabled { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000725A File Offset: 0x0000545A
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00007262 File Offset: 0x00005462
		public Vector2 StartCoordinate { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000726B File Offset: 0x0000546B
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00007273 File Offset: 0x00005473
		public Vector2 Size { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000727C File Offset: 0x0000547C
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00007284 File Offset: 0x00005484
		public Texture OverlayTexture { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000728D File Offset: 0x0000548D
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00007295 File Offset: 0x00005495
		public bool UseOverlayAlphaAsMask { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000729E File Offset: 0x0000549E
		// (set) Token: 0x0600018C RID: 396 RVA: 0x000072A6 File Offset: 0x000054A6
		public float Scale { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000072AF File Offset: 0x000054AF
		// (set) Token: 0x0600018E RID: 398 RVA: 0x000072B7 File Offset: 0x000054B7
		public float OverlayTextureWidth { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000072C0 File Offset: 0x000054C0
		// (set) Token: 0x06000190 RID: 400 RVA: 0x000072C8 File Offset: 0x000054C8
		public float OverlayTextureHeight { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000072D1 File Offset: 0x000054D1
		// (set) Token: 0x06000192 RID: 402 RVA: 0x000072D9 File Offset: 0x000054D9
		public float OverlayXOffset { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000193 RID: 403 RVA: 0x000072E2 File Offset: 0x000054E2
		// (set) Token: 0x06000194 RID: 404 RVA: 0x000072EA File Offset: 0x000054EA
		public float OverlayYOffset { get; set; }

		// Token: 0x06000195 RID: 405 RVA: 0x000072F3 File Offset: 0x000054F3
		public SimpleMaterial()
			: this(null, 0)
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000072FD File Offset: 0x000054FD
		public SimpleMaterial(Texture texture)
			: this(texture, 0)
		{
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007307 File Offset: 0x00005507
		public SimpleMaterial(Texture texture, int renderOrder)
			: this(texture, renderOrder, true)
		{
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007312 File Offset: 0x00005512
		public SimpleMaterial(Texture texture, int renderOrder, bool blending)
			: base(blending, renderOrder)
		{
			this.Reset(texture);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007324 File Offset: 0x00005524
		public void Reset(Texture texture = null)
		{
			this.Texture = texture;
			this.NinePatchParameters = SpriteNinePatchParameters.Empty;
			this.ColorFactor = 1f;
			this.AlphaFactor = 1f;
			this.HueFactor = 0f;
			this.SaturationFactor = 0f;
			this.ValueFactor = 0f;
			this.Color = new Color(1f, 1f, 1f, 1f);
			this.CircularMaskingEnabled = false;
			this.OverlayEnabled = false;
			this.OverlayTextureWidth = 512f;
			this.OverlayTextureHeight = 512f;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000073BD File Offset: 0x000055BD
		public Vec2 GetCircularMaskingCenter()
		{
			return this.CircularMaskingCenter;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000073CA File Offset: 0x000055CA
		public Vec2 GetOverlayStartCoordinate()
		{
			return this.StartCoordinate;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000073D7 File Offset: 0x000055D7
		public Vec2 GetOverlaySize()
		{
			return this.Size;
		}
	}
}
