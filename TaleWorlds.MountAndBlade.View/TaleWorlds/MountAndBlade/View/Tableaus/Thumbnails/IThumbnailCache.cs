using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200004D RID: 77
	public interface IThumbnailCache
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600027E RID: 638
		int Count { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600027F RID: 639
		int RenderCallbackCount { get; }

		// Token: 0x06000280 RID: 640
		void Initialize(ThumbnailCreatorView thumnbailCreatorView);

		// Token: 0x06000281 RID: 641
		void Destroy();

		// Token: 0x06000282 RID: 642
		void Clear(bool releaseImmediately);

		// Token: 0x06000283 RID: 643
		bool GetValue(string key, out Texture texture);

		// Token: 0x06000284 RID: 644
		bool AddReference(string key);

		// Token: 0x06000285 RID: 645
		bool RemoveReference(string key);

		// Token: 0x06000286 RID: 646
		bool OnThumbnailRenderCompleted(string renderId, Texture renderTarget);

		// Token: 0x06000287 RID: 647
		void ClearUnusedCache();

		// Token: 0x06000288 RID: 648
		void Tick(float dt);

		// Token: 0x06000289 RID: 649
		void Add(string key, Texture value);

		// Token: 0x0600028A RID: 650
		void PrintToImgui();

		// Token: 0x0600028B RID: 651
		TextureCreationInfo CreateTexture(ThumbnailCreationData thumbnailCreationData);

		// Token: 0x0600028C RID: 652
		bool ReleaseTexture(ThumbnailCreationData thumbnailCreationData);
	}
}
