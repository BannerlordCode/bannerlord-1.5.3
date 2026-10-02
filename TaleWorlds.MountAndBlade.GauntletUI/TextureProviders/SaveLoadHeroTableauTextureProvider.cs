using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders
{
	// Token: 0x02000022 RID: 34
	public class SaveLoadHeroTableauTextureProvider : TextureProvider
	{
		// Token: 0x17000051 RID: 81
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00008EC9 File Offset: 0x000070C9
		public string HeroVisualCode
		{
			set
			{
				this._characterCode = value;
				this.DeserializeCharacterCode(this._characterCode);
			}
		}

		// Token: 0x17000052 RID: 82
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00008EDE File Offset: 0x000070DE
		public string BannerCode
		{
			set
			{
				this._tableau.SetBannerCode(value);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00008EEC File Offset: 0x000070EC
		public bool IsVersionCompatible
		{
			get
			{
				return this._tableau.IsVersionCompatible;
			}
		}

		// Token: 0x17000054 RID: 84
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00008EF9 File Offset: 0x000070F9
		public bool CurrentlyRotating
		{
			set
			{
				this._tableau.RotateCharacter(value);
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00008F07 File Offset: 0x00007107
		public SaveLoadHeroTableauTextureProvider()
		{
			this._tableau = new BasicCharacterTableau();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00008F1A File Offset: 0x0000711A
		public override void Tick(float dt)
		{
			this.CheckTexture();
			this._tableau.OnTick(dt);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008F2E File Offset: 0x0000712E
		public override void SetTargetSize(int width, int height)
		{
			base.SetTargetSize(width, height);
			this._tableau.SetTargetSize(width, height);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008F45 File Offset: 0x00007145
		private void DeserializeCharacterCode(string characterCode)
		{
			if (!string.IsNullOrEmpty(characterCode))
			{
				this._tableau.DeserializeCharacterCode(characterCode);
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008F5C File Offset: 0x0000715C
		private void CheckTexture()
		{
			if (this._texture != this._tableau.Texture)
			{
				this._texture = this._tableau.Texture;
				if (this._texture != null)
				{
					EngineTexture engineTexture = new EngineTexture(this._texture);
					this._providedTexture = new TaleWorlds.TwoDimension.Texture(engineTexture);
					return;
				}
				this._providedTexture = null;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008FC0 File Offset: 0x000071C0
		public override void Clear(bool clearNextFrame)
		{
			this._tableau.OnFinalize();
			base.Clear(clearNextFrame);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008FD4 File Offset: 0x000071D4
		protected override TaleWorlds.TwoDimension.Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name)
		{
			this.CheckTexture();
			return this._providedTexture;
		}

		// Token: 0x040000C5 RID: 197
		private string _characterCode;

		// Token: 0x040000C6 RID: 198
		private BasicCharacterTableau _tableau;

		// Token: 0x040000C7 RID: 199
		private TaleWorlds.Engine.Texture _texture;

		// Token: 0x040000C8 RID: 200
		private TaleWorlds.TwoDimension.Texture _providedTexture;
	}
}
