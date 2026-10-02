using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008C RID: 140
	public class MultiplayerItemTabButtonWidget : ButtonWidget
	{
		// Token: 0x060007D3 RID: 2003 RVA: 0x00016E20 File Offset: 0x00015020
		public MultiplayerItemTabButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00016E2C File Offset: 0x0001502C
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.ItemType) || this._iconWidget == null)
			{
				return;
			}
			Sprite sprite = base.Context.SpriteData.GetSprite("StdAssets\\ItemIcons\\" + this.ItemType);
			this.IconWidget.Brush.DefaultLayer.Sprite = sprite;
			Sprite sprite2 = base.Context.SpriteData.GetSprite("StdAssets\\ItemIcons\\" + this.ItemType + "_selected");
			this.IconWidget.Brush.GetLayer("Selected").Sprite = sprite2;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00016EC7 File Offset: 0x000150C7
		protected override void RefreshState()
		{
			base.RefreshState();
			if (base.IsSelected && base.ParentWidget is Container)
			{
				(base.ParentWidget as Container).OnChildSelected(this);
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x00016EF5 File Offset: 0x000150F5
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x00016EFD File Offset: 0x000150FD
		[Editor(false)]
		public string ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChanged<string>(value, "ItemType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x00016F26 File Offset: 0x00015126
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x00016F2E File Offset: 0x0001512E
		[Editor(false)]
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

		// Token: 0x0400036C RID: 876
		private const string BaseSpritePath = "StdAssets\\ItemIcons\\";

		// Token: 0x0400036D RID: 877
		private string _itemType;

		// Token: 0x0400036E RID: 878
		private BrushWidget _iconWidget;
	}
}
