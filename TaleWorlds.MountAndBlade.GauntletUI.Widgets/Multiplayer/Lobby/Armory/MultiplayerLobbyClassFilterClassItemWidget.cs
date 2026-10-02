using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000BB RID: 187
	public class MultiplayerLobbyClassFilterClassItemWidget : ToggleStateButtonWidget
	{
		// Token: 0x060009E7 RID: 2535 RVA: 0x0001BE92 File Offset: 0x0001A092
		public MultiplayerLobbyClassFilterClassItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0001BE9B File Offset: 0x0001A09B
		private void SetFactionColor()
		{
			if (this.FactionColorWidget == null)
			{
				return;
			}
			this.FactionColorWidget.Color = this.CultureColor;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0001BEB8 File Offset: 0x0001A0B8
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.TroopType) || this._iconWidget == null)
			{
				return;
			}
			Widget iconWidget = this.IconWidget;
			Brush iconBrush = this.IconBrush;
			Sprite sprite;
			if (iconBrush == null)
			{
				sprite = null;
			}
			else
			{
				BrushLayer layer = iconBrush.GetLayer(this.TroopType);
				sprite = ((layer != null) ? layer.Sprite : null);
			}
			iconWidget.Sprite = sprite;
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0001BF0A File Offset: 0x0001A10A
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0001BF12 File Offset: 0x0001A112
		[Editor(false)]
		public string TroopType
		{
			get
			{
				return this._troopType;
			}
			set
			{
				if (value != this._troopType)
				{
					this._troopType = value;
					base.OnPropertyChanged<string>(value, "TroopType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0001BF3B File Offset: 0x0001A13B
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x0001BF43 File Offset: 0x0001A143
		[Editor(false)]
		public Color CultureColor
		{
			get
			{
				return this._cultureColor;
			}
			set
			{
				if (this._cultureColor != value)
				{
					this._cultureColor = value;
					base.OnPropertyChanged(value, "CultureColor");
					this.SetFactionColor();
				}
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x0001BF6C File Offset: 0x0001A16C
		// (set) Token: 0x060009EF RID: 2543 RVA: 0x0001BF74 File Offset: 0x0001A174
		[Editor(false)]
		public Brush IconBrush
		{
			get
			{
				return this._iconBrush;
			}
			set
			{
				if (this._iconBrush != value)
				{
					this._iconBrush = value;
					base.OnPropertyChanged<Brush>(value, "IconBrush");
				}
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0001BF92 File Offset: 0x0001A192
		// (set) Token: 0x060009F1 RID: 2545 RVA: 0x0001BF9A File Offset: 0x0001A19A
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

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0001BFBE File Offset: 0x0001A1BE
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0001BFC6 File Offset: 0x0001A1C6
		[Editor(false)]
		public Widget FactionColorWidget
		{
			get
			{
				return this._factionColorWidget;
			}
			set
			{
				if (this._factionColorWidget != value)
				{
					this._factionColorWidget = value;
					base.OnPropertyChanged<Widget>(value, "FactionColorWidget");
					this.SetFactionColor();
				}
			}
		}

		// Token: 0x04000477 RID: 1143
		private string _troopType;

		// Token: 0x04000478 RID: 1144
		private Color _cultureColor;

		// Token: 0x04000479 RID: 1145
		private Brush _iconBrush;

		// Token: 0x0400047A RID: 1146
		private Widget _iconWidget;

		// Token: 0x0400047B RID: 1147
		private Widget _factionColorWidget;
	}
}
