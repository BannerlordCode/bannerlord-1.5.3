using System;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.Library;

namespace TaleWorlds.PlayerServices.Avatar
{
	// Token: 0x0200000D RID: 13
	public class GOGAvatarService : IAvatarService
	{
		// Token: 0x0600006B RID: 107 RVA: 0x0000305C File Offset: 0x0000125C
		public void Initialize()
		{
			if (this._isInitalized)
			{
				return;
			}
			foreach (string text in Directory.GetFiles(this._resourceFolder, "*.png"))
			{
				this._avatarImagesAsByteArrays.Add(File.ReadAllBytes(text));
			}
			this._isInitalized = true;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000030AD File Offset: 0x000012AD
		public void ClearCache()
		{
			if (!this._isInitalized)
			{
				return;
			}
			this._avatarImageCache.Clear();
			this._avatarImagesAsByteArrays.Clear();
			this._isInitalized = false;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000030D8 File Offset: 0x000012D8
		public AvatarData GetPlayerAvatar(PlayerId playerId)
		{
			int num = (int)((uint)playerId.Id2 % (uint)this._avatarImagesAsByteArrays.Count);
			return new AvatarData(this._avatarImagesAsByteArrays[num]);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000310B File Offset: 0x0000130B
		public bool IsInitialized()
		{
			return this._isInitalized;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003113 File Offset: 0x00001313
		public void Tick(float dt)
		{
		}

		// Token: 0x0400002B RID: 43
		private readonly Dictionary<ulong, AvatarData> _avatarImageCache = new Dictionary<ulong, AvatarData>();

		// Token: 0x0400002C RID: 44
		private readonly string _resourceFolder = BasePath.Name + "Modules/Native/MultiplayerForcedAvatars/";

		// Token: 0x0400002D RID: 45
		private readonly List<byte[]> _avatarImagesAsByteArrays = new List<byte[]>();

		// Token: 0x0400002E RID: 46
		private bool _isInitalized;
	}
}
