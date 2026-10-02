using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000028 RID: 40
	public class ImageIdentifierWidget : TextureWidget
	{
		// Token: 0x0600020D RID: 525 RVA: 0x00007940 File Offset: 0x00005B40
		public ImageIdentifierWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "";
			this._calculateSizeFirstFrame = false;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000795C File Offset: 0x00005B5C
		protected override void OnContextActivated()
		{
			base.OnContextActivated();
			string imageId = this.ImageId;
			this.ImageId = string.Empty;
			this.ImageId = imageId;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00007988 File Offset: 0x00005B88
		protected override void OnContextDeactivated()
		{
			base.OnContextDeactivated();
			base.SetTextureProviderProperty("IsReleased", true);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000079A1 File Offset: 0x00005BA1
		private void RefreshVisibility()
		{
			if (this.HideWhenNull)
			{
				base.IsVisible = !string.IsNullOrEmpty(this.ImageId);
				return;
			}
			base.IsVisible = true;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000079C7 File Offset: 0x00005BC7
		public override void OnClearTextureProvider()
		{
			base.SetTextureProviderProperty("IsReleased", true);
			base.OnClearTextureProvider();
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000212 RID: 530 RVA: 0x000079E0 File Offset: 0x00005BE0
		// (set) Token: 0x06000213 RID: 531 RVA: 0x000079E8 File Offset: 0x00005BE8
		[Editor(false)]
		public string ImageId
		{
			get
			{
				return this._imageId;
			}
			set
			{
				if (this._imageId != value)
				{
					if (!string.IsNullOrEmpty(this._imageId))
					{
						base.SetTextureProviderProperty("IsReleased", true);
					}
					this._imageId = value;
					base.OnPropertyChanged<string>(value, "ImageId");
					base.SetTextureProviderProperty("ImageId", value);
					if (!string.IsNullOrEmpty(this._imageId))
					{
						base.SetTextureProviderProperty("IsReleased", false);
					}
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00007A64 File Offset: 0x00005C64
		// (set) Token: 0x06000215 RID: 533 RVA: 0x00007A6C File Offset: 0x00005C6C
		[Editor(false)]
		public string AdditionalArgs
		{
			get
			{
				return this._additionalArgs;
			}
			set
			{
				if (this._additionalArgs != value)
				{
					base.SetTextureProviderProperty("IsReleased", true);
					this._additionalArgs = value;
					base.OnPropertyChanged<string>(value, "AdditionalArgs");
					base.SetTextureProviderProperty("AdditionalArgs", value);
					if (!string.IsNullOrEmpty(this._additionalArgs))
					{
						base.SetTextureProviderProperty("IsReleased", false);
					}
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00007ADB File Offset: 0x00005CDB
		// (set) Token: 0x06000217 RID: 535 RVA: 0x00007AE4 File Offset: 0x00005CE4
		[Editor(false)]
		public bool IsBig
		{
			get
			{
				return this._isBig;
			}
			set
			{
				if (this._isBig != value)
				{
					base.SetTextureProviderProperty("IsReleased", true);
					base.SetTextureProviderProperty("IsReleased", false);
					this._isBig = value;
					base.OnPropertyChanged(value, "IsBig");
					base.SetTextureProviderProperty("IsBig", value);
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00007B46 File Offset: 0x00005D46
		// (set) Token: 0x06000219 RID: 537 RVA: 0x00007B4E File Offset: 0x00005D4E
		[Editor(false)]
		public bool HideWhenNull
		{
			get
			{
				return this._hideWhenNull;
			}
			set
			{
				if (this._hideWhenNull != value)
				{
					this._hideWhenNull = value;
					base.OnPropertyChanged(value, "HideWhenNull");
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x040000F5 RID: 245
		private string _imageId;

		// Token: 0x040000F6 RID: 246
		private string _additionalArgs;

		// Token: 0x040000F7 RID: 247
		private bool _isBig;

		// Token: 0x040000F8 RID: 248
		private bool _hideWhenNull;
	}
}
