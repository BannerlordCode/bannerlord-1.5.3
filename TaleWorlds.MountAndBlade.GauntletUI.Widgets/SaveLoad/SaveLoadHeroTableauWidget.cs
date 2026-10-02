using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.SaveLoad
{
	// Token: 0x0200005B RID: 91
	public class SaveLoadHeroTableauWidget : TextureWidget
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0000F860 File Offset: 0x0000DA60
		public bool IsVersionCompatible
		{
			get
			{
				bool? textureProviderProperty = base.GetTextureProviderProperty<bool>("IsVersionCompatible");
				bool flag = true;
				return (textureProviderProperty.GetValueOrDefault() == flag) & (textureProviderProperty != null);
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0000F88D File Offset: 0x0000DA8D
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x0000F895 File Offset: 0x0000DA95
		[Editor(false)]
		public string HeroVisualCode
		{
			get
			{
				return this._heroVisualCode;
			}
			set
			{
				if (value != this._heroVisualCode)
				{
					this._heroVisualCode = value;
					base.OnPropertyChanged<string>(value, "HeroVisualCode");
					base.SetTextureProviderProperty("HeroVisualCode", value);
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0000F8C4 File Offset: 0x0000DAC4
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x0000F8CC File Offset: 0x0000DACC
		[Editor(false)]
		public string BannerCode
		{
			get
			{
				return this._bannerCode;
			}
			set
			{
				if (value != this._bannerCode)
				{
					this._bannerCode = value;
					base.OnPropertyChanged<string>(value, "BannerCode");
					base.SetTextureProviderProperty("BannerCode", value);
				}
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0000F8FB File Offset: 0x0000DAFB
		public SaveLoadHeroTableauWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "SaveLoadHeroTableauTextureProvider";
			this._isRenderRequestedPreviousFrame = true;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0000F916 File Offset: 0x0000DB16
		protected override void OnMousePressed()
		{
			base.SetTextureProviderProperty("CurrentlyRotating", true);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000F929 File Offset: 0x0000DB29
		protected override void OnMouseReleased(bool isFromInput)
		{
			base.SetTextureProviderProperty("CurrentlyRotating", false);
		}

		// Token: 0x04000221 RID: 545
		private string _heroVisualCode;

		// Token: 0x04000222 RID: 546
		private string _bannerCode;
	}
}
