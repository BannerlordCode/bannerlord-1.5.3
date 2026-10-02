using System;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000061 RID: 97
	public class OnlineImageTextureWidget : TextureWidget
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0001BE24 File Offset: 0x0001A024
		// (set) Token: 0x0600067E RID: 1662 RVA: 0x0001BE2C File Offset: 0x0001A02C
		public OnlineImageTextureWidget.ImageSizePolicies ImageSizePolicy { get; set; }

		// Token: 0x0600067F RID: 1663 RVA: 0x0001BE35 File Offset: 0x0001A035
		public OnlineImageTextureWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "OnlineImageTextureProvider";
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001BE49 File Offset: 0x0001A049
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.UpdateSizePolicy();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001BE58 File Offset: 0x0001A058
		private void UpdateSizePolicy()
		{
			Texture texture = base.Texture;
			bool flag = texture != null && texture.IsValid;
			if (this.ImageSizePolicy == OnlineImageTextureWidget.ImageSizePolicies.OriginalSize)
			{
				if (flag)
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
				if (this.ImageSizePolicy == OnlineImageTextureWidget.ImageSizePolicies.Stretch)
				{
					base.WidthSizePolicy = SizePolicy.StretchToParent;
					base.HeightSizePolicy = SizePolicy.StretchToParent;
					return;
				}
				if (this.ImageSizePolicy == OnlineImageTextureWidget.ImageSizePolicies.ScaleToBiggerDimension && flag)
				{
					base.WidthSizePolicy = SizePolicy.Fixed;
					base.HeightSizePolicy = SizePolicy.Fixed;
					float num;
					if (base.Texture.Width > base.Texture.Height)
					{
						num = base.ParentWidget.Size.Y / (float)base.Texture.Height;
						if (num * (float)base.Texture.Width < base.ParentWidget.Size.X)
						{
							num = base.ParentWidget.Size.X / (float)base.Texture.Width;
						}
					}
					else
					{
						num = base.ParentWidget.Size.X / (float)base.Texture.Width;
						if (num * (float)base.Texture.Height < base.ParentWidget.Size.Y)
						{
							num = base.ParentWidget.Size.Y / (float)base.Texture.Height;
						}
					}
					base.ScaledSuggestedWidth = num * (float)base.Texture.Width;
					base.ScaledSuggestedHeight = num * (float)base.Texture.Height;
				}
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001BFF3 File Offset: 0x0001A1F3
		// (set) Token: 0x06000683 RID: 1667 RVA: 0x0001BFFB File Offset: 0x0001A1FB
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

		// Token: 0x0400030B RID: 779
		private string _onlineImageSourceUrl;

		// Token: 0x02000096 RID: 150
		public enum ImageSizePolicies
		{
			// Token: 0x04000498 RID: 1176
			Stretch,
			// Token: 0x04000499 RID: 1177
			OriginalSize,
			// Token: 0x0400049A RID: 1178
			ScaleToBiggerDimension
		}
	}
}
