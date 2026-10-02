using System;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.Library;

namespace TaleWorlds.PlayerServices.Avatar
{
	// Token: 0x0200000C RID: 12
	internal class ForcedAvatarService : IAvatarService
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002F63 File Offset: 0x00001163
		public int AvatarCount
		{
			get
			{
				return this._avatarImagesAsByteArrays.Count;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002F70 File Offset: 0x00001170
		public AvatarData GetPlayerAvatar(PlayerId playerId)
		{
			if (this._avatarImagesAsByteArrays.Count == 0)
			{
				return new AvatarData();
			}
			return this.GetForcedPlayerAvatar(AvatarServices.GetForcedAvatarIndexOfPlayer(playerId));
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002F91 File Offset: 0x00001191
		private AvatarData GetForcedPlayerAvatar(int forcedIndex)
		{
			return new AvatarData(this._avatarImagesAsByteArrays[forcedIndex]);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002FA4 File Offset: 0x000011A4
		public void Initialize()
		{
			if (this._isInitialized)
			{
				return;
			}
			this._avatarImagesAsByteArrays.Clear();
			if (Directory.Exists(this._resourceFolder))
			{
				foreach (string text in Directory.GetFiles(this._resourceFolder, "*.png"))
				{
					this._avatarImagesAsByteArrays.Add(File.ReadAllBytes(text));
				}
			}
			this._isInitialized = true;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000300D File Offset: 0x0000120D
		public bool IsInitialized()
		{
			return this._isInitialized;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003015 File Offset: 0x00001215
		public void ClearCache()
		{
			if (!this._isInitialized)
			{
				return;
			}
			this._avatarImagesAsByteArrays.Clear();
			this._isInitialized = false;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003032 File Offset: 0x00001232
		public void Tick(float dt)
		{
		}

		// Token: 0x04000028 RID: 40
		private readonly string _resourceFolder = BasePath.Name + "Modules/Native/MultiplayerForcedAvatars/";

		// Token: 0x04000029 RID: 41
		private readonly List<byte[]> _avatarImagesAsByteArrays = new List<byte[]>();

		// Token: 0x0400002A RID: 42
		private bool _isInitialized;
	}
}
