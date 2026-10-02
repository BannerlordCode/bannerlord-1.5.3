using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders
{
	// Token: 0x0200001E RID: 30
	public class BrightnessDemoTextureProvider : TextureProvider
	{
		// Token: 0x17000029 RID: 41
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00008884 File Offset: 0x00006A84
		public int DemoType
		{
			set
			{
				this._sceneTableau.SetDemoType(value);
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008892 File Offset: 0x00006A92
		public BrightnessDemoTextureProvider()
		{
			this._sceneTableau = new BrightnessDemoTableau();
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000088A8 File Offset: 0x00006AA8
		private void CheckTexture()
		{
			if (this._sceneTableau != null)
			{
				if (this._texture != this._sceneTableau.Texture)
				{
					this._texture = this._sceneTableau.Texture;
					if (this._texture != null)
					{
						this.wrappedTexture = new EngineTexture(this._texture);
						this._providedTexture = new TaleWorlds.TwoDimension.Texture(this.wrappedTexture);
						return;
					}
					this._providedTexture = null;
					return;
				}
			}
			else
			{
				this._providedTexture = null;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00008926 File Offset: 0x00006B26
		public override void Tick(float dt)
		{
			base.Tick(dt);
			this.CheckTexture();
			BrightnessDemoTableau sceneTableau = this._sceneTableau;
			if (sceneTableau == null)
			{
				return;
			}
			sceneTableau.OnTick(dt);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00008946 File Offset: 0x00006B46
		public override void Clear(bool clearNextFrame)
		{
			BrightnessDemoTableau sceneTableau = this._sceneTableau;
			if (sceneTableau != null)
			{
				sceneTableau.OnFinalize();
			}
			base.Clear(clearNextFrame);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00008960 File Offset: 0x00006B60
		public override void SetTargetSize(int width, int height)
		{
			base.SetTargetSize(width, height);
			this._sceneTableau.SetTargetSize(width, height);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00008977 File Offset: 0x00006B77
		protected override TaleWorlds.TwoDimension.Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name)
		{
			this.CheckTexture();
			return this._providedTexture;
		}

		// Token: 0x040000B2 RID: 178
		private BrightnessDemoTableau _sceneTableau;

		// Token: 0x040000B3 RID: 179
		private TaleWorlds.Engine.Texture _texture;

		// Token: 0x040000B4 RID: 180
		private TaleWorlds.TwoDimension.Texture _providedTexture;

		// Token: 0x040000B5 RID: 181
		private EngineTexture wrappedTexture;
	}
}
