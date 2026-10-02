using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000090 RID: 144
	public class MultiplayerTroopTypeIconWidget : Widget
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x000171A8 File Offset: 0x000153A8
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x000171B0 File Offset: 0x000153B0
		public float ScaleFactor { get; set; } = 1f;

		// Token: 0x060007F1 RID: 2033 RVA: 0x000171B9 File Offset: 0x000153B9
		public MultiplayerTroopTypeIconWidget(UIContext context)
			: base(context)
		{
			this.BackgroundWidget = this;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x000171D4 File Offset: 0x000153D4
		private void UpdateIcon()
		{
			if (this.BackgroundWidget == null || this.ForegroundWidget == null || string.IsNullOrEmpty(this.IconSpriteType))
			{
				return;
			}
			string text = "MPHud\\TroopIcons\\" + this.IconSpriteType;
			string text2 = text + "_Outline";
			this.ForegroundWidget.Sprite = base.Context.SpriteData.GetSprite(text);
			this.BackgroundWidget.Sprite = base.Context.SpriteData.GetSprite(text2);
			if (this.BackgroundWidget.Sprite != null)
			{
				float num = (float)this.BackgroundWidget.Sprite.Width;
				this.BackgroundWidget.SuggestedWidth = num * this.ScaleFactor;
				this.ForegroundWidget.SuggestedWidth = num * this.ScaleFactor;
				float num2 = (float)this.BackgroundWidget.Sprite.Height;
				this.BackgroundWidget.SuggestedHeight = num2 * this.ScaleFactor;
				this.ForegroundWidget.SuggestedHeight = num2 * this.ScaleFactor;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x000172D1 File Offset: 0x000154D1
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x000172D9 File Offset: 0x000154D9
		[DataSourceProperty]
		public Widget BackgroundWidget
		{
			get
			{
				return this._backgroundWidget;
			}
			set
			{
				if (this._backgroundWidget != value)
				{
					this._backgroundWidget = value;
					base.OnPropertyChanged<Widget>(value, "BackgroundWidget");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x000172FD File Offset: 0x000154FD
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x00017305 File Offset: 0x00015505
		[DataSourceProperty]
		public Widget ForegroundWidget
		{
			get
			{
				return this._foregroundWidget;
			}
			set
			{
				if (this._foregroundWidget != value)
				{
					this._foregroundWidget = value;
					base.OnPropertyChanged<Widget>(value, "ForegroundWidget");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00017329 File Offset: 0x00015529
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x00017331 File Offset: 0x00015531
		[DataSourceProperty]
		public string IconSpriteType
		{
			get
			{
				return this._iconSpriteType;
			}
			set
			{
				if (this._iconSpriteType != value)
				{
					this._iconSpriteType = value;
					base.OnPropertyChanged<string>(value, "IconSpriteType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x0400037D RID: 893
		private Widget _backgroundWidget;

		// Token: 0x0400037E RID: 894
		private Widget _foregroundWidget;

		// Token: 0x0400037F RID: 895
		private string _iconSpriteType;
	}
}
