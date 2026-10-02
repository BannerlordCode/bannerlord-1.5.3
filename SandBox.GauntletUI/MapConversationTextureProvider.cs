using System;
using SandBox.View.Map;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI
{
	// Token: 0x02000011 RID: 17
	public class MapConversationTextureProvider : TextureProvider
	{
		// Token: 0x17000009 RID: 9
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00007B14 File Offset: 0x00005D14
		public object Data
		{
			set
			{
				this._mapConversationTableau.SetData(value);
			}
		}

		// Token: 0x1700000A RID: 10
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00007B22 File Offset: 0x00005D22
		public bool IsEnabled
		{
			set
			{
				this._mapConversationTableau.SetEnabled(value);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00007B30 File Offset: 0x00005D30
		public MapConversationTextureProvider()
		{
			this._mapConversationTableau = new MapConversationTableau();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00007B43 File Offset: 0x00005D43
		public override void Clear(bool clearNextFrame)
		{
			this._mapConversationTableau.OnFinalize(clearNextFrame);
			base.Clear(clearNextFrame);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00007B58 File Offset: 0x00005D58
		private void CheckTexture()
		{
			if (this._texture != this._mapConversationTableau.Texture)
			{
				this._texture = this._mapConversationTableau.Texture;
				if (this._texture != null)
				{
					EngineTexture engineTexture = new EngineTexture(this._texture);
					this._providedTexture = new TaleWorlds.TwoDimension.Texture(engineTexture);
					return;
				}
				this._providedTexture = null;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00007BBC File Offset: 0x00005DBC
		protected override TaleWorlds.TwoDimension.Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name)
		{
			this.CheckTexture();
			return this._providedTexture;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00007BCA File Offset: 0x00005DCA
		public override void SetTargetSize(int width, int height)
		{
			base.SetTargetSize(width, height);
			this._mapConversationTableau.SetTargetSize(width, height);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00007BE1 File Offset: 0x00005DE1
		public override void Tick(float dt)
		{
			base.Tick(dt);
			this.CheckTexture();
			this._mapConversationTableau.OnTick(dt);
		}

		// Token: 0x04000057 RID: 87
		private MapConversationTableau _mapConversationTableau;

		// Token: 0x04000058 RID: 88
		private TaleWorlds.Engine.Texture _texture;

		// Token: 0x04000059 RID: 89
		private TaleWorlds.TwoDimension.Texture _providedTexture;
	}
}
