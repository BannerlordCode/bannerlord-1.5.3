using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.Launcher.Library.CustomWidgets
{
	// Token: 0x02000025 RID: 37
	public class LauncherOnlineImageTextureWidget : TextureWidget
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00006B3A File Offset: 0x00004D3A
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00006B42 File Offset: 0x00004D42
		public LauncherOnlineImageTextureWidget.ImageSizePolicies ImageSizePolicy { get; set; }

		// Token: 0x06000178 RID: 376 RVA: 0x00006B4B File Offset: 0x00004D4B
		public LauncherOnlineImageTextureWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "LauncherOnlineImageTextureProvider";
			base.WidthSizePolicy = SizePolicy.Fixed;
			base.HeightSizePolicy = SizePolicy.Fixed;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006B6D File Offset: 0x00004D6D
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdateSizePolicy();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006B7C File Offset: 0x00004D7C
		protected override void OnTextureUpdated()
		{
			base.OnTextureUpdated();
			this.SetGlobalAlphaRecursively(0f);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00006B90 File Offset: 0x00004D90
		private void UpdateSizePolicy()
		{
			if (base.Texture != null && base.ReadOnlyBrush.GlobalAlphaFactor < 1f)
			{
				float num = Mathf.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, 1f, 0.1f);
				this.SetGlobalAlphaRecursively(num);
			}
			else if (base.Texture == null)
			{
				this.SetGlobalAlphaRecursively(0f);
			}
			if (this.ImageSizePolicy == LauncherOnlineImageTextureWidget.ImageSizePolicies.OriginalSize)
			{
				if (base.Texture != null)
				{
					base.WidthSizePolicy = SizePolicy.Fixed;
					base.HeightSizePolicy = SizePolicy.Fixed;
					base.SuggestedWidth = (float)base.Texture.Width;
					base.SuggestedHeight = (float)base.Texture.Height;
					return;
				}
			}
			else
			{
				if (this.ImageSizePolicy == LauncherOnlineImageTextureWidget.ImageSizePolicies.Stretch)
				{
					base.WidthSizePolicy = SizePolicy.StretchToParent;
					base.HeightSizePolicy = SizePolicy.StretchToParent;
					return;
				}
				if (this.ImageSizePolicy == LauncherOnlineImageTextureWidget.ImageSizePolicies.ScaleToBiggerDimension && base.Texture != null)
				{
					base.WidthSizePolicy = SizePolicy.Fixed;
					base.HeightSizePolicy = SizePolicy.Fixed;
					float num2;
					if (base.Texture.Width > base.Texture.Height)
					{
						num2 = base.ParentWidget.Size.Y / (float)base.Texture.Height;
						if (num2 * (float)base.Texture.Width < base.ParentWidget.Size.X)
						{
							num2 = base.ParentWidget.Size.X / (float)base.Texture.Width;
						}
					}
					else
					{
						num2 = base.ParentWidget.Size.X / (float)base.Texture.Width;
						if (num2 * (float)base.Texture.Height < base.ParentWidget.Size.Y)
						{
							num2 = base.ParentWidget.Size.Y / (float)base.Texture.Height;
						}
					}
					base.SuggestedWidth = num2 * (float)base.Texture.Width * base._inverseScaleToUse;
					base.SuggestedHeight = num2 * (float)base.Texture.Height * base._inverseScaleToUse;
					base.ScaledSuggestedWidth = num2 * (float)base.Texture.Width;
					base.ScaledSuggestedHeight = num2 * (float)base.Texture.Height;
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00006DA9 File Offset: 0x00004FA9
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00006DB1 File Offset: 0x00004FB1
		[Editor(false)]
		public string OnlineImageSourceUrl
		{
			get
			{
				return this._onlineImageSourceUrl;
			}
			set
			{
				if (this._onlineImageSourceUrl != value)
				{
					this._onlineImageSourceUrl = value;
					base.OnPropertyChanged<string>(value, "OnlineImageSourceUrl");
					base.SetTextureProviderProperty("OnlineSourceUrl", value);
					this.RefreshState();
				}
			}
		}

		// Token: 0x040000B6 RID: 182
		private string _onlineImageSourceUrl;

		// Token: 0x02000047 RID: 71
		public enum ImageSizePolicies
		{
			// Token: 0x04000106 RID: 262
			Stretch,
			// Token: 0x04000107 RID: 263
			OriginalSize,
			// Token: 0x04000108 RID: 264
			ScaleToBiggerDimension
		}
	}
}
