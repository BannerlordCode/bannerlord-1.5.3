using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x02000081 RID: 129
	public class SettlementNameplateItemWidget : Widget
	{
		// Token: 0x06000740 RID: 1856 RVA: 0x000155D7 File Offset: 0x000137D7
		public SettlementNameplateItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x000155E0 File Offset: 0x000137E0
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x000155E8 File Offset: 0x000137E8
		public bool IsOverWidget { get; private set; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x000155F1 File Offset: 0x000137F1
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x000155F9 File Offset: 0x000137F9
		public int QuestType { get; set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00015602 File Offset: 0x00013802
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x0001560A File Offset: 0x0001380A
		public int IssueType { get; set; }

		// Token: 0x06000747 RID: 1863 RVA: 0x00015614 File Offset: 0x00013814
		public void ParallelUpdate(float dt)
		{
			Widget widgetToShow = this._widgetToShow;
			Widget parentWidget = base.ParentWidget;
			if (widgetToShow == null)
			{
				Debug.FailedAssert("widgetToShow is null during ParallelUpdate!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Nameplate\\SettlementNameplateItemWidget.cs", "ParallelUpdate", 24);
				return;
			}
			if (parentWidget != null && parentWidget.IsEnabled)
			{
				this.IsOverWidget = this.IsMouseOverWidget();
				if (this.IsOverWidget && !this._hoverBegan)
				{
					this._hoverBegan = true;
					widgetToShow.IsVisible = true;
				}
				else if (!this.IsOverWidget && this._hoverBegan)
				{
					this._hoverBegan = false;
					widgetToShow.IsVisible = false;
				}
				if (!this.IsOverWidget && widgetToShow.IsVisible)
				{
					widgetToShow.IsVisible = false;
					return;
				}
			}
			else
			{
				widgetToShow.IsVisible = false;
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000156C0 File Offset: 0x000138C0
		private bool IsMouseOverWidget()
		{
			if (!base.EventManager.GetIsHitThisFrame() || !base.EventManager.IsPointInsideUsableArea(base.EventManager.MousePosition))
			{
				return false;
			}
			Vector2 mousePosition = base.EventManager.MousePosition;
			return this.AreaRect.IsPointInside(in mousePosition);
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0001570D File Offset: 0x0001390D
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x00015715 File Offset: 0x00013915
		public Widget InspectedIconWidget
		{
			get
			{
				return this._inspectedIconWidget;
			}
			set
			{
				if (this._inspectedIconWidget != value)
				{
					this._inspectedIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "InspectedIconWidget");
				}
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x00015733 File Offset: 0x00013933
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x0001573B File Offset: 0x0001393B
		public Widget PortIconWidget
		{
			get
			{
				return this._portIconWidget;
			}
			set
			{
				if (this._portIconWidget != value)
				{
					this._portIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "PortIconWidget");
				}
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x00015759 File Offset: 0x00013959
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x00015761 File Offset: 0x00013961
		public GridWidget SettlementPartiesGridWidget
		{
			get
			{
				return this._settlementPartiesGridWidget;
			}
			set
			{
				if (this._settlementPartiesGridWidget != value)
				{
					this._settlementPartiesGridWidget = value;
					base.OnPropertyChanged<GridWidget>(value, "SettlementPartiesGridWidget");
				}
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001577F File Offset: 0x0001397F
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x00015787 File Offset: 0x00013987
		public MapEventVisualBrushWidget MapEventVisualWidget
		{
			get
			{
				return this._mapEventVisualWidget;
			}
			set
			{
				if (this._mapEventVisualWidget != value)
				{
					this._mapEventVisualWidget = value;
					base.OnPropertyChanged<MapEventVisualBrushWidget>(value, "MapEventVisualWidget");
				}
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x000157A5 File Offset: 0x000139A5
		// (set) Token: 0x06000752 RID: 1874 RVA: 0x000157AD File Offset: 0x000139AD
		[Editor(false)]
		public Widget WidgetToShow
		{
			get
			{
				return this._widgetToShow;
			}
			set
			{
				if (this._widgetToShow != value)
				{
					this._widgetToShow = value;
					base.OnPropertyChanged<Widget>(value, "WidgetToShow");
				}
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x000157CB File Offset: 0x000139CB
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x000157D3 File Offset: 0x000139D3
		public MaskedTextureWidget SettlementBannerWidget
		{
			get
			{
				return this._settlementBannerWidget;
			}
			set
			{
				if (this._settlementBannerWidget != value)
				{
					this._settlementBannerWidget = value;
					base.OnPropertyChanged<MaskedTextureWidget>(value, "SettlementBannerWidget");
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x000157F1 File Offset: 0x000139F1
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x000157F9 File Offset: 0x000139F9
		public TextWidget SettlementNameTextWidget
		{
			get
			{
				return this._settlementNameTextWidget;
			}
			set
			{
				if (this._settlementNameTextWidget != value)
				{
					this._settlementNameTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "SettlementNameTextWidget");
				}
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x00015817 File Offset: 0x00013A17
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x0001581F File Offset: 0x00013A1F
		public Widget ParleyIconWidget
		{
			get
			{
				return this._parleyIconWidget;
			}
			set
			{
				if (this._parleyIconWidget != value)
				{
					this._parleyIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "ParleyIconWidget");
				}
			}
		}

		// Token: 0x04000324 RID: 804
		private bool _hoverBegan;

		// Token: 0x04000328 RID: 808
		private Widget _inspectedIconWidget;

		// Token: 0x04000329 RID: 809
		private Widget _portIconWidget;

		// Token: 0x0400032A RID: 810
		private MapEventVisualBrushWidget _mapEventVisualWidget;

		// Token: 0x0400032B RID: 811
		private MaskedTextureWidget _settlementBannerWidget;

		// Token: 0x0400032C RID: 812
		private TextWidget _settlementNameTextWidget;

		// Token: 0x0400032D RID: 813
		private GridWidget _settlementPartiesGridWidget;

		// Token: 0x0400032E RID: 814
		private Widget _widgetToShow;

		// Token: 0x0400032F RID: 815
		private Widget _parleyIconWidget;
	}
}
