using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Perks
{
	// Token: 0x02000099 RID: 153
	public class MultiplayerPerkItemToggleWidget : ToggleButtonWidget
	{
		// Token: 0x06000854 RID: 2132 RVA: 0x000182A8 File Offset: 0x000164A8
		public MultiplayerPerkItemToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000182B1 File Offset: 0x000164B1
		protected override void HandleClick()
		{
			base.HandleClick();
			MultiplayerPerkContainerPanelWidget containerPanel = this.ContainerPanel;
			if (containerPanel == null)
			{
				return;
			}
			containerPanel.PerkSelected(this._isSelectable ? this : null);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x000182D8 File Offset: 0x000164D8
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.IconType) || this._iconWidget == null)
			{
				return;
			}
			foreach (Style style in this.IconWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Sprite = base.Context.SpriteData.GetSprite("General\\Perks\\" + this.IconType);
				}
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x00018380 File Offset: 0x00016580
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x00018388 File Offset: 0x00016588
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChanged<string>(value, "IconType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x000183B1 File Offset: 0x000165B1
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x000183B9 File Offset: 0x000165B9
		[DataSourceProperty]
		public BrushWidget IconWidget
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
					base.OnPropertyChanged<BrushWidget>(value, "IconWidget");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x000183DD File Offset: 0x000165DD
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x000183E5 File Offset: 0x000165E5
		[DataSourceProperty]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (value != this._isSelectable)
				{
					this._isSelectable = value;
					base.OnPropertyChanged(value, "IsSelectable");
				}
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00018403 File Offset: 0x00016603
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x0001840B File Offset: 0x0001660B
		[DataSourceProperty]
		public MultiplayerPerkContainerPanelWidget ContainerPanel
		{
			get
			{
				return this._containerPanel;
			}
			set
			{
				if (value != this._containerPanel)
				{
					this._containerPanel = value;
					base.OnPropertyChanged<MultiplayerPerkContainerPanelWidget>(value, "ContainerPanel");
				}
			}
		}

		// Token: 0x040003B6 RID: 950
		private string _iconType;

		// Token: 0x040003B7 RID: 951
		private BrushWidget _iconWidget;

		// Token: 0x040003B8 RID: 952
		private bool _isSelectable;

		// Token: 0x040003B9 RID: 953
		private MultiplayerPerkContainerPanelWidget _containerPanel;
	}
}
