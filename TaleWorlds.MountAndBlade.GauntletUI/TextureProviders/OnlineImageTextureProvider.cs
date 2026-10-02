using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.TextureProviders
{
	// Token: 0x02000021 RID: 33
	public class OnlineImageTextureProvider : TextureProvider
	{
		// Token: 0x17000050 RID: 80
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00008D68 File Offset: 0x00006F68
		public string OnlineSourceUrl
		{
			set
			{
				this._onlineSourceUrl = value;
				this.RefreshOnlineImage();
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00008D77 File Offset: 0x00006F77
		public OnlineImageTextureProvider()
		{
			this._onlineImageCache = new Dictionary<string, PlatformFilePath>();
			this._onlineImageCacheFolderPath = new PlatformDirectoryPath(PlatformFileType.User, this.DataFolder);
			this.PopulateOnlineImageCache();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008DAD File Offset: 0x00006FAD
		public override void Tick(float dt)
		{
			base.Tick(dt);
			if (this._requiresRetry)
			{
				if (10 < this._retryCount)
				{
					this._requiresRetry = false;
					return;
				}
				this._retryCount++;
				this.RefreshOnlineImage();
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00008DE4 File Offset: 0x00006FE4
		private async void RefreshOnlineImage()
		{
			if (string.IsNullOrEmpty(this._onlineSourceUrl))
			{
				this._requiresRetry = false;
			}
			else if (this._retryCount < 10)
			{
				try
				{
					string guidOfRequestedURL = OnlineImageTextureProvider.ToGuid(this._onlineSourceUrl).ToString();
					if (!this._onlineImageCache.ContainsKey(guidOfRequestedURL))
					{
						PlatformFilePath pathOfTheDownloadedImage = new PlatformFilePath(this._onlineImageCacheFolderPath, guidOfRequestedURL + ".png");
						byte[] array = await HttpHelper.DownloadDataTaskAsync(this._onlineSourceUrl);
						if (array != null)
						{
							FileHelper.SaveFile(pathOfTheDownloadedImage, array);
							this._onlineImageCache.Add(guidOfRequestedURL, pathOfTheDownloadedImage);
						}
						pathOfTheDownloadedImage = default(PlatformFilePath);
					}
					PlatformFilePath platformFilePath;
					if (this._onlineImageCache.TryGetValue(guidOfRequestedURL, out platformFilePath))
					{
						TaleWorlds.Engine.Texture texture = TaleWorlds.Engine.Texture.CreateTextureFromPath(platformFilePath);
						if (texture == null)
						{
							this._onlineImageCache.Remove(guidOfRequestedURL);
							Debug.Print(string.Format("RETRYING TO DOWNLOAD: {0} | RETRY COUNT: {1}", this._onlineSourceUrl, this._retryCount), 0, Debug.DebugColor.Red, 17592186044416UL);
							this._requiresRetry = true;
						}
						else
						{
							this.OnTextureCreated(texture);
							this._requiresRetry = false;
						}
					}
					else
					{
						Debug.Print(string.Format("RETRYING TO DOWNLOAD: {0} | RETRY COUNT: {1}", this._onlineSourceUrl, this._retryCount), 0, Debug.DebugColor.Red, 17592186044416UL);
						this._requiresRetry = true;
					}
					guidOfRequestedURL = null;
				}
				catch (Exception ex)
				{
					Debug.FailedAssert("Error while trying to get image online: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\TextureProviders\\OnlineImageTextureProvider.cs", "RefreshOnlineImage", 115);
					Debug.Print(string.Format("RETRYING TO DOWNLOAD: {0} | RETRY COUNT: {1}", this._onlineSourceUrl, this._retryCount), 0, Debug.DebugColor.Red, 17592186044416UL);
					this._requiresRetry = true;
				}
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00008E1D File Offset: 0x0000701D
		protected override TaleWorlds.TwoDimension.Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name)
		{
			if (this._texture != null)
			{
				return new TaleWorlds.TwoDimension.Texture(new EngineTexture(this._texture));
			}
			return null;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00008E40 File Offset: 0x00007040
		private void PopulateOnlineImageCache()
		{
			foreach (PlatformFilePath platformFilePath in FileHelper.GetFiles(this._onlineImageCacheFolderPath, "*.png", SearchOption.AllDirectories))
			{
				string fileNameWithoutExtension = platformFilePath.GetFileNameWithoutExtension();
				this._onlineImageCache.Add(fileNameWithoutExtension, platformFilePath);
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00008E8C File Offset: 0x0000708C
		private static Guid ToGuid(string src)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(src);
			byte[] array = new SHA1CryptoServiceProvider().ComputeHash(bytes);
			Array.Resize<byte>(ref array, 16);
			return new Guid(array);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00008EC0 File Offset: 0x000070C0
		private void OnTextureCreated(TaleWorlds.Engine.Texture texture)
		{
			this._texture = texture;
		}

		// Token: 0x040000BD RID: 189
		private Dictionary<string, PlatformFilePath> _onlineImageCache;

		// Token: 0x040000BE RID: 190
		private readonly string DataFolder = "Online Images";

		// Token: 0x040000BF RID: 191
		private readonly PlatformDirectoryPath _onlineImageCacheFolderPath;

		// Token: 0x040000C0 RID: 192
		private TaleWorlds.Engine.Texture _texture;

		// Token: 0x040000C1 RID: 193
		private bool _requiresRetry;

		// Token: 0x040000C2 RID: 194
		private int _retryCount;

		// Token: 0x040000C3 RID: 195
		private const int _maxRetryCount = 10;

		// Token: 0x040000C4 RID: 196
		private string _onlineSourceUrl;
	}
}
