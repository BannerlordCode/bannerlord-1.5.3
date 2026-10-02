using System;
using System.Linq;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.PlayerServices;
using TaleWorlds.PlayerServices.Avatar;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders.ImageIdentifiers
{
	// Token: 0x02000029 RID: 41
	public class PlayerAvatarImageTextureProvider : ImageIdentifierTextureProvider
	{
		// Token: 0x0600019C RID: 412 RVA: 0x00009708 File Offset: 0x00007908
		public PlayerAvatarImageTextureProvider()
		{
			this._timeSinceAvatarFail = 5f;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000971B File Offset: 0x0000791B
		public override void Tick(float dt)
		{
			this._timeSinceAvatarFail += dt;
			base.Tick(dt);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00009734 File Offset: 0x00007934
		protected override void OnCreateImageWithId(string id, string additionalArgs)
		{
			PlayerId playerId = PlayerId.FromString(id);
			int num;
			int num2;
			if (!string.IsNullOrEmpty(additionalArgs) && int.TryParse(additionalArgs, out num))
			{
				num2 = num;
			}
			else
			{
				NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.FirstOrDefault<NetworkCommunicator>((NetworkCommunicator np) => np.VirtualPlayer.Id == playerId);
				num2 = ((networkCommunicator != null) ? networkCommunicator.ForcedAvatarIndex : (-1));
			}
			AvatarDataResponse playerAvatar = AvatarServices.GetPlayerAvatar(playerId, num2);
			if (playerAvatar != null)
			{
				this._receivedAvatarData = playerAvatar.AvatarData;
				this._isUsingFallbackAvatar = playerAvatar.IsFallBack;
				if (this._isUsingFallbackAvatar)
				{
					this._timeSinceAvatarFail = 0f;
					return;
				}
			}
			else
			{
				this._timeSinceAvatarFail = 0f;
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000097D5 File Offset: 0x000079D5
		protected override bool GetCanForceCheckTexture()
		{
			return this.TextureNeedsRefresh();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000097E0 File Offset: 0x000079E0
		protected override void OnCheckTexture()
		{
			if (this._receivedAvatarData != null)
			{
				if (this._receivedAvatarData.Status == AvatarData.DataStatus.Ready)
				{
					AvatarData receivedAvatarData = this._receivedAvatarData;
					this._receivedAvatarData = null;
					this.OnAvatarLoaded(base.ImageId + "." + base.AdditionalArgs, receivedAvatarData);
					return;
				}
				if (this._receivedAvatarData.Status == AvatarData.DataStatus.Failed)
				{
					this._receivedAvatarData = null;
					base.OnTextureCreated(null);
					base.ForceRefreshTextures();
					this._timeSinceAvatarFail = 0f;
					return;
				}
			}
			else
			{
				if (this._isUsingFallbackAvatar && this._timeSinceAvatarFail > 1f)
				{
					base.CreateImageWithId(base.ImageId, base.AdditionalArgs);
					return;
				}
				if (this._timeSinceAvatarFail > 5f)
				{
					base.CreateImageWithId(base.ImageId, base.AdditionalArgs);
				}
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000098A4 File Offset: 0x00007AA4
		private void OnAvatarLoaded(string avatarID, AvatarData avatarData)
		{
			if (avatarData != null)
			{
				AvatarThumbnailCreationData avatarThumbnailCreationData = new AvatarThumbnailCreationData(avatarID, avatarData.Image, avatarData.Width, avatarData.Height, avatarData.Type);
				TextureCreationInfo textureCreationInfo = ThumbnailCacheManager.Current.CreateTexture(avatarThumbnailCreationData);
				base.OnTextureCreated(textureCreationInfo.Texture);
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000098EB File Offset: 0x00007AEB
		private bool TextureNeedsRefresh()
		{
			return this._receivedAvatarData != null || (this._isUsingFallbackAvatar && this._timeSinceAvatarFail > 1f) || this._timeSinceAvatarFail > 5f;
		}

		// Token: 0x040000D7 RID: 215
		private const float AvatarFallbackWaitTime = 1f;

		// Token: 0x040000D8 RID: 216
		private const float AvatarFailWaitTime = 5f;

		// Token: 0x040000D9 RID: 217
		private AvatarData _receivedAvatarData;

		// Token: 0x040000DA RID: 218
		private bool _isUsingFallbackAvatar;

		// Token: 0x040000DB RID: 219
		private float _timeSinceAvatarFail;
	}
}
