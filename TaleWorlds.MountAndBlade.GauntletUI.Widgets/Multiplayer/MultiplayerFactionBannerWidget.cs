using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008A RID: 138
	public class MultiplayerFactionBannerWidget : Widget
	{
		// Token: 0x060007C1 RID: 1985 RVA: 0x00016BA0 File Offset: 0x00014DA0
		public MultiplayerFactionBannerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00016BB0 File Offset: 0x00014DB0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._firstFrame)
			{
				this.UpdateBanner();
				this.UpdateIcon();
				this._firstFrame = false;
			}
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00016BD4 File Offset: 0x00014DD4
		private void UpdateBanner()
		{
			if (this._bannerWidget == null)
			{
				return;
			}
			BrushWidget brushWidget;
			if ((brushWidget = this.BannerWidget as BrushWidget) != null)
			{
				using (Dictionary<string, Style>.ValueCollection.Enumerator enumerator = brushWidget.Brush.Styles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Style style = enumerator.Current;
						StyleLayer[] layers = style.GetLayers();
						for (int i = 0; i < layers.Length; i++)
						{
							layers[i].Color = this.CultureColor1;
						}
					}
					return;
				}
			}
			this.BannerWidget.Color = this.CultureColor1;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00016C70 File Offset: 0x00014E70
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.FactionCode) || this._iconWidget == null)
			{
				return;
			}
			this.IconWidget.Sprite = base.Context.SpriteData.GetSprite("StdAssets\\FactionIcons\\LargeIcons\\" + this.FactionCode);
			this.IconWidget.Color = this.CultureColor2;
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x00016CCF File Offset: 0x00014ECF
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x00016CD7 File Offset: 0x00014ED7
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChanged(value, "CultureColor1");
					this.UpdateBanner();
				}
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00016D00 File Offset: 0x00014F00
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x00016D08 File Offset: 0x00014F08
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChanged(value, "CultureColor2");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00016D31 File Offset: 0x00014F31
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x00016D39 File Offset: 0x00014F39
		[DataSourceProperty]
		public string FactionCode
		{
			get
			{
				return this._factionCode;
			}
			set
			{
				if (value != this._factionCode)
				{
					this._factionCode = value;
					base.OnPropertyChanged<string>(value, "FactionCode");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00016D62 File Offset: 0x00014F62
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x00016D6A File Offset: 0x00014F6A
		[DataSourceProperty]
		public Widget BannerWidget
		{
			get
			{
				return this._bannerWidget;
			}
			set
			{
				if (value != this._bannerWidget)
				{
					this._bannerWidget = value;
					base.OnPropertyChanged<Widget>(value, "BannerWidget");
					this.UpdateBanner();
				}
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00016D8E File Offset: 0x00014F8E
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x00016D96 File Offset: 0x00014F96
		[DataSourceProperty]
		public Widget IconWidget
		{
			get
			{
				return this._iconWidget;
			}
			set
			{
				if (value != this._iconWidget)
				{
					this._iconWidget = value;
					base.OnPropertyChanged<Widget>(value, "IconWidget");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x04000365 RID: 869
		private bool _firstFrame = true;

		// Token: 0x04000366 RID: 870
		private Color _cultureColor1;

		// Token: 0x04000367 RID: 871
		private Color _cultureColor2;

		// Token: 0x04000368 RID: 872
		private string _factionCode;

		// Token: 0x04000369 RID: 873
		private Widget _bannerWidget;

		// Token: 0x0400036A RID: 874
		private Widget _iconWidget;
	}
}
