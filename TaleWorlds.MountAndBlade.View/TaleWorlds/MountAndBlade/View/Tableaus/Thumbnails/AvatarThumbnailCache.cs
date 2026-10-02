using System;
using TaleWorlds.Engine;
using TaleWorlds.PlayerServices.Avatar;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200003B RID: 59
	public class AvatarThumbnailCache : ThumbnailCache<AvatarThumbnailCreationData>
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000EDA3 File Offset: 0x0000CFA3
		// (set) Token: 0x0600021E RID: 542 RVA: 0x0000EDAA File Offset: 0x0000CFAA
		public static AvatarThumbnailCache Current { get; private set; }

		// Token: 0x0600021F RID: 543 RVA: 0x0000EDB2 File Offset: 0x0000CFB2
		public AvatarThumbnailCache(int capacity)
			: base(capacity)
		{
			AvatarThumbnailCache.Current = this;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000EDC1 File Offset: 0x0000CFC1
		protected override void OnFinalize()
		{
			base.OnFinalize();
			AvatarThumbnailCache.Current = null;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000EDD0 File Offset: 0x0000CFD0
		protected override TextureCreationInfo OnCreateTexture(AvatarThumbnailCreationData thumbnailCreationData)
		{
			Texture texture;
			((IThumbnailCache)this).GetValue(thumbnailCreationData.AvatarID, out texture);
			if (!(texture == null))
			{
				((IThumbnailCache)this).AddReference(thumbnailCreationData.AvatarID);
				return TextureCreationInfo.WithExistingTexture(texture);
			}
			if (thumbnailCreationData.AvatarBytes == null || thumbnailCreationData.AvatarBytes.Length == 0)
			{
				return TextureCreationInfo.WithNewTexture(null);
			}
			if (thumbnailCreationData.ImageType == AvatarData.ImageType.Image)
			{
				texture = Texture.CreateFromMemory(thumbnailCreationData.AvatarBytes);
				texture.Name = ThumbnailDebugUtility.CreateDebugIdFrom(thumbnailCreationData.AvatarID, "avatar", "byte_array");
			}
			else if (thumbnailCreationData.ImageType == AvatarData.ImageType.Raw)
			{
				texture = Texture.CreateFromByteArray(thumbnailCreationData.AvatarBytes, (int)thumbnailCreationData.Width, (int)thumbnailCreationData.Height);
				texture.Name = ThumbnailDebugUtility.CreateDebugIdFrom(thumbnailCreationData.AvatarID, "avatar", "raw_data");
			}
			((IThumbnailCache)this).Add(thumbnailCreationData.AvatarID, texture);
			((IThumbnailCache)this).AddReference(thumbnailCreationData.AvatarID);
			return TextureCreationInfo.WithNewTexture(texture);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000EEB1 File Offset: 0x0000D0B1
		protected override bool OnReleaseTexture(AvatarThumbnailCreationData thumbnailCreationData)
		{
			return true;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000EEB4 File Offset: 0x0000D0B4
		public void FlushCache()
		{
			((IThumbnailCache)this).Clear(true);
		}
	}
}
