using System;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000060 RID: 96
	internal class SpriteFromTexture : Sprite
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0001BDF3 File Offset: 0x00019FF3
		public override Texture Texture
		{
			get
			{
				return this._texture;
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001BDFB File Offset: 0x00019FFB
		public override Vec2 GetMinUvs()
		{
			return Vec2.Zero;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001BE02 File Offset: 0x0001A002
		public override Vec2 GetMaxUvs()
		{
			return Vec2.One;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001BE09 File Offset: 0x0001A009
		public SpriteFromTexture(Texture texture, int width, int height)
			: base("Sprite", width, height, SpriteNinePatchParameters.Empty)
		{
			this._texture = texture;
		}

		// Token: 0x04000309 RID: 777
		private Texture _texture;
	}
}
