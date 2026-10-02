using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders.ImageIdentifiers
{
	// Token: 0x02000027 RID: 39
	public abstract class ImageIdentifierTextureProvider : TextureProvider, IDisposable
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000093D4 File Offset: 0x000075D4
		// (set) Token: 0x06000181 RID: 385 RVA: 0x000093DC File Offset: 0x000075DC
		protected ThumbnailCreationData ThumbnailCreationData { get; set; }

		// Token: 0x06000182 RID: 386 RVA: 0x000093E5 File Offset: 0x000075E5
		public ImageIdentifierTextureProvider()
		{
			this._textureRequiresRefreshing = true;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000093F4 File Offset: 0x000075F4
		~ImageIdentifierTextureProvider()
		{
			this.OnDisposed();
		}

		// Token: 0x06000184 RID: 388
		protected abstract void OnCreateImageWithId(string id, string additionalArgs);

		// Token: 0x06000185 RID: 389 RVA: 0x00009420 File Offset: 0x00007620
		public override void Tick(float dt)
		{
			base.Tick(dt);
			this.CheckTexture();
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00009430 File Offset: 0x00007630
		private void ReleaseCache()
		{
			if (this.ThumbnailCreationData == null)
			{
				return;
			}
			if (!this.ThumbnailCreationData.IsProcessed)
			{
				Debug.FailedAssert("Created thumbnail data but trying to release it before its processed", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\TextureProviders\\ImageIdentifiers\\ImageIdentifierTextureProvider.cs", "ReleaseCache", 50);
				return;
			}
			ThumbnailCacheManager thumbnailCacheManager = ThumbnailCacheManager.Current;
			if (thumbnailCacheManager != null)
			{
				thumbnailCacheManager.DestroyTexture(this.ThumbnailCreationData);
			}
			this.ThumbnailCreationData = null;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00009488 File Offset: 0x00007688
		public override void Clear(bool clearNextFrame)
		{
			base.Clear(clearNextFrame);
			this._providedTexture = null;
			this._textureRequiresRefreshing = true;
			this.ReleaseCache();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000094A5 File Offset: 0x000076A5
		protected virtual bool GetCanForceCheckTexture()
		{
			return false;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000094A8 File Offset: 0x000076A8
		protected virtual void OnCheckTexture()
		{
			this.CreateImageWithId(this.ImageId, this.AdditionalArgs);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000094BC File Offset: 0x000076BC
		protected override TaleWorlds.TwoDimension.Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name)
		{
			return this._providedTexture;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000094C4 File Offset: 0x000076C4
		protected void ForceRefreshTextures()
		{
			this._textureRequiresRefreshing = true;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000094D0 File Offset: 0x000076D0
		private void CheckTexture()
		{
			if (this._textureRequiresRefreshing || this.GetCanForceCheckTexture())
			{
				this._texture = null;
				this.ReleaseCache();
				this.OnCheckTexture();
				this._textureRequiresRefreshing = false;
			}
			if (this._handleNewlyCreatedTexture)
			{
				TaleWorlds.Engine.Texture texture = null;
				TaleWorlds.TwoDimension.Texture providedTexture = this._providedTexture;
				EngineTexture engineTexture;
				if ((engineTexture = ((providedTexture != null) ? providedTexture.PlatformTexture : null) as EngineTexture) != null)
				{
					texture = engineTexture.Texture;
				}
				if (this._texture != texture)
				{
					if (this._texture != null)
					{
						EngineTexture engineTexture2 = new EngineTexture(this._texture);
						this._providedTexture = new TaleWorlds.TwoDimension.Texture(engineTexture2);
					}
					else
					{
						this._providedTexture = null;
					}
				}
				this._handleNewlyCreatedTexture = false;
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00009577 File Offset: 0x00007777
		public void CreateImageWithId(string id, string additionalArgs)
		{
			this.OnCreateImageWithId(id, additionalArgs ?? string.Empty);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000958A File Offset: 0x0000778A
		protected void OnTextureCreated(TaleWorlds.Engine.Texture texture)
		{
			this._texture = texture;
			this._textureRequiresRefreshing = false;
			this._handleNewlyCreatedTexture = true;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000095A1 File Offset: 0x000077A1
		protected void OnTextureCreationCancelled()
		{
			this._texture = null;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000095AA File Offset: 0x000077AA
		private void OnDisposed()
		{
			this.ReleaseCache();
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000095B2 File Offset: 0x000077B2
		// (set) Token: 0x06000192 RID: 402 RVA: 0x000095BA File Offset: 0x000077BA
		public bool IsReleased
		{
			get
			{
				return this._isReleased;
			}
			set
			{
				if (this._isReleased != value)
				{
					this._isReleased = value;
					if (this._isReleased)
					{
						this.ReleaseCache();
						this._isReleased = false;
					}
				}
				this._textureRequiresRefreshing = true;
				this._handleNewlyCreatedTexture = true;
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000095EF File Offset: 0x000077EF
		void IDisposable.Dispose()
		{
			this.OnDisposed();
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000194 RID: 404 RVA: 0x000095F7 File Offset: 0x000077F7
		// (set) Token: 0x06000195 RID: 405 RVA: 0x000095FF File Offset: 0x000077FF
		public bool IsBig
		{
			get
			{
				return this._isBig;
			}
			set
			{
				if (this._isBig != value)
				{
					this._isBig = value;
					this._textureRequiresRefreshing = true;
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00009618 File Offset: 0x00007818
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00009620 File Offset: 0x00007820
		public string ImageId
		{
			get
			{
				return this._imageId;
			}
			set
			{
				if (this._imageId != value)
				{
					this._imageId = value;
					this._textureRequiresRefreshing = true;
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000963E File Offset: 0x0000783E
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00009646 File Offset: 0x00007846
		public string AdditionalArgs
		{
			get
			{
				return this._additionalArgs;
			}
			set
			{
				if (this._additionalArgs != value)
				{
					this._additionalArgs = value;
					this._textureRequiresRefreshing = true;
				}
			}
		}

		// Token: 0x040000CF RID: 207
		private bool _textureRequiresRefreshing;

		// Token: 0x040000D0 RID: 208
		private bool _handleNewlyCreatedTexture;

		// Token: 0x040000D1 RID: 209
		private TaleWorlds.Engine.Texture _texture;

		// Token: 0x040000D2 RID: 210
		private TaleWorlds.TwoDimension.Texture _providedTexture;

		// Token: 0x040000D3 RID: 211
		private string _imageId;

		// Token: 0x040000D4 RID: 212
		private string _additionalArgs;

		// Token: 0x040000D5 RID: 213
		private bool _isBig;

		// Token: 0x040000D6 RID: 214
		private bool _isReleased;
	}
}
