using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CB RID: 203
	public class ClassLoadoutAlternativeUsageItemTabButtonWidget : ButtonWidget
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x0001E05C File Offset: 0x0001C25C
		public ClassLoadoutAlternativeUsageItemTabButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0001E068 File Offset: 0x0001C268
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.UsageType) || this._iconWidget == null)
			{
				return;
			}
			Sprite sprite = base.Context.SpriteData.GetSprite("MPClassLoadout\\UsageIcons\\" + this.UsageType);
			foreach (Style style in this.IconWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Sprite = sprite;
				}
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0001E114 File Offset: 0x0001C314
		protected override void RefreshState()
		{
			base.RefreshState();
			if (base.IsSelected && base.ParentWidget is Container)
			{
				(base.ParentWidget as Container).OnChildSelected(this);
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0001E142 File Offset: 0x0001C342
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x0001E14A File Offset: 0x0001C34A
		public string UsageType
		{
			get
			{
				return this._usageType;
			}
			set
			{
				if (value != this._usageType)
				{
					this._usageType = value;
					base.OnPropertyChanged<string>(value, "UsageType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0001E173 File Offset: 0x0001C373
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x0001E17B File Offset: 0x0001C37B
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

		// Token: 0x040004E1 RID: 1249
		private string _usageType;

		// Token: 0x040004E2 RID: 1250
		private BrushWidget _iconWidget;
	}
}
