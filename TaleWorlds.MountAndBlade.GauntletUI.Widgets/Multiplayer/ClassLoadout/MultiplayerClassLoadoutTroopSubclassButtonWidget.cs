using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000D1 RID: 209
	public class MultiplayerClassLoadoutTroopSubclassButtonWidget : ButtonWidget
	{
		// Token: 0x06000ADC RID: 2780 RVA: 0x0001E80D File Offset: 0x0001CA0D
		public MultiplayerClassLoadoutTroopSubclassButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0001E818 File Offset: 0x0001CA18
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.TroopType) || this._iconWidget == null)
			{
				return;
			}
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
			Sprite sprite2 = sprite;
			foreach (Style style in this.IconWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Sprite = sprite2;
				}
			}
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0001E8C8 File Offset: 0x0001CAC8
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			Widget parentWidget = base.ParentWidget;
			if (parentWidget == null)
			{
				return;
			}
			parentWidget.SetState(base.CurrentState);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0001E8E7 File Offset: 0x0001CAE7
		public override void SetState(string stateName)
		{
			base.SetState(stateName);
			if (this.PerksNavigationScopeTargeter != null)
			{
				this.PerksNavigationScopeTargeter.IsScopeEnabled = stateName == "Selected";
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0001E90E File Offset: 0x0001CB0E
		// (set) Token: 0x06000AE1 RID: 2785 RVA: 0x0001E916 File Offset: 0x0001CB16
		[DataSourceProperty]
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

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x0001E93F File Offset: 0x0001CB3F
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x0001E947 File Offset: 0x0001CB47
		[DataSourceProperty]
		public Brush IconBrush
		{
			get
			{
				return this._iconBrush;
			}
			set
			{
				if (value != this._iconBrush)
				{
					this._iconBrush = value;
					base.OnPropertyChanged<Brush>(value, "IconBrush");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x0001E96B File Offset: 0x0001CB6B
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x0001E973 File Offset: 0x0001CB73
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

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0001E997 File Offset: 0x0001CB97
		// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x0001E99F File Offset: 0x0001CB9F
		public NavigationScopeTargeter PerksNavigationScopeTargeter
		{
			get
			{
				return this._perksNavigationScopeTargeter;
			}
			set
			{
				if (value != this._perksNavigationScopeTargeter)
				{
					this._perksNavigationScopeTargeter = value;
					base.OnPropertyChanged<NavigationScopeTargeter>(value, "PerksNavigationScopeTargeter");
					if (this._perksNavigationScopeTargeter != null)
					{
						this._perksNavigationScopeTargeter.IsScopeEnabled = false;
					}
				}
			}
		}

		// Token: 0x040004F0 RID: 1264
		private string _troopType;

		// Token: 0x040004F1 RID: 1265
		private Brush _iconBrush;

		// Token: 0x040004F2 RID: 1266
		private BrushWidget _iconWidget;

		// Token: 0x040004F3 RID: 1267
		private NavigationScopeTargeter _perksNavigationScopeTargeter;
	}
}
