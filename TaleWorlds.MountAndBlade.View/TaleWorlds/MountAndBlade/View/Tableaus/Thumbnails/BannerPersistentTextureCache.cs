using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200003F RID: 63
	public class BannerPersistentTextureCache : ThumbnailCache<BannerTextureCreationData>
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000F0A9 File Offset: 0x0000D2A9
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000F0B0 File Offset: 0x0000D2B0
		public static BannerPersistentTextureCache Current { get; private set; }

		// Token: 0x06000239 RID: 569 RVA: 0x0000F0B8 File Offset: 0x0000D2B8
		public BannerPersistentTextureCache()
			: base(1000)
		{
			BannerPersistentTextureCache.Current = this;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000F0CB File Offset: 0x0000D2CB
		protected override void OnFinalize()
		{
			base.OnFinalize();
			BannerPersistentTextureCache.Current = null;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000F0DC File Offset: 0x0000D2DC
		protected override TextureCreationInfo OnCreateTexture(BannerTextureCreationData textureCreationData)
		{
			Action<Texture> setAction = textureCreationData.SetAction;
			Action cancelAction = textureCreationData.CancelAction;
			string renderId = textureCreationData.RenderId;
			Texture texture;
			if (((IThumbnailCache)this).GetValue(renderId, out texture))
			{
				if (this._renderCallbacks.ContainsKey(renderId))
				{
					this._renderCallbacks[renderId].SetActions.Add(setAction);
					this._renderCallbacks[renderId].CancelActions.Add(cancelAction);
				}
				else if (setAction != null)
				{
					setAction(texture);
				}
				return TextureCreationInfo.WithExistingTexture(texture);
			}
			Texture texture2 = BannerTextureCreator.CreateTexture(textureCreationData);
			((IThumbnailCache)this).Add(renderId, texture2);
			((IThumbnailCache)this).AddReference(renderId);
			if (!this._renderCallbacks.ContainsKey(renderId))
			{
				this._renderCallbacks.Add(renderId, RenderCallbackCollection.CreateEmpty());
			}
			this._renderCallbacks[renderId].SetActions.Add(setAction);
			this._renderCallbacks[renderId].CancelActions.Add(cancelAction);
			return TextureCreationInfo.WithNewTexture(texture2);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000F1D5 File Offset: 0x0000D3D5
		protected override bool OnReleaseTexture(BannerTextureCreationData thumbnailCreationData)
		{
			return true;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000F1D8 File Offset: 0x0000D3D8
		public void FlushCache()
		{
			((IThumbnailCache)this).Clear(true);
		}
	}
}
