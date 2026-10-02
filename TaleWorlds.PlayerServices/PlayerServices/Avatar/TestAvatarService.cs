using System;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.Library;

namespace TaleWorlds.PlayerServices.Avatar
{
	// Token: 0x02000010 RID: 16
	public class TestAvatarService : IAvatarService
	{
		// Token: 0x06000079 RID: 121 RVA: 0x000031F5 File Offset: 0x000013F5
		public TestAvatarService()
		{
			this._avatarImageCache = new Dictionary<ulong, AvatarData>();
			this._avatarImagesAsByteArrays = new List<byte[]>();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003228 File Offset: 0x00001428
		public void ClearCache()
		{
			if (!this._isInitialized)
			{
				return;
			}
			this._avatarImageCache.Clear();
			this._avatarImagesAsByteArrays.Clear();
			this._isInitialized = false;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003250 File Offset: 0x00001450
		public AvatarData GetPlayerAvatar(PlayerId playerId)
		{
			if (this._avatarImagesAsByteArrays.Count == 0)
			{
				return new AvatarData();
			}
			int num = (int)((uint)playerId.Id2 % (uint)this._avatarImagesAsByteArrays.Count);
			return new AvatarData(this._avatarImagesAsByteArrays[num]);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003298 File Offset: 0x00001498
		public void Initialize()
		{
			if (this._isInitialized)
			{
				return;
			}
			if (Directory.Exists(this._resourceFolder))
			{
				foreach (string text in Directory.GetFiles(this._resourceFolder, "*.jpg"))
				{
					this._avatarImagesAsByteArrays.Add(File.ReadAllBytes(text));
				}
			}
			this._isInitialized = true;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000032F6 File Offset: 0x000014F6
		public bool IsInitialized()
		{
			return this._isInitialized;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000032FE File Offset: 0x000014FE
		public void Tick(float dt)
		{
		}

		// Token: 0x04000032 RID: 50
		private readonly Dictionary<ulong, AvatarData> _avatarImageCache;

		// Token: 0x04000033 RID: 51
		private readonly string _resourceFolder = BasePath.Name + "Modules/Native/MultiplayerTestAvatars/";

		// Token: 0x04000034 RID: 52
		private readonly List<byte[]> _avatarImagesAsByteArrays;

		// Token: 0x04000035 RID: 53
		private bool _isInitialized;
	}
}
