using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x02000160 RID: 352
	public class EncyclopediaSearchBarBrushWidget : BrushWidget
	{
		// Token: 0x060012BB RID: 4795 RVA: 0x00033BBE File Offset: 0x00031DBE
		public EncyclopediaSearchBarBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00033BC8 File Offset: 0x00031DC8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			bool flag = base.EventManager.LatestMouseUpWidget == this || base.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget) || base.EventManager.LatestMouseDownWidget == this || base.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget);
			bool flag2 = this.SearchResultPanel.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget) || this.SearchResultPanel.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget);
			this.ShowResults = (flag || flag2) && this.SearchInputWidget.Text.Length >= this.MinCharAmountToShowResults;
			this.SearchResultPanel.IsVisible = this.ShowResults;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00033C8C File Offset: 0x00031E8C
		protected override void OnMousePressed()
		{
			base.OnMousePressed();
			base.EventFired("SearchBarClick", Array.Empty<object>());
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x00033CA4 File Offset: 0x00031EA4
		// (set) Token: 0x060012BF RID: 4799 RVA: 0x00033CAC File Offset: 0x00031EAC
		public bool ShowResults
		{
			get
			{
				return this._showChat;
			}
			set
			{
				if (value != this._showChat)
				{
					this._showChat = value;
					base.OnPropertyChanged(value, "ShowResults");
				}
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060012C0 RID: 4800 RVA: 0x00033CCA File Offset: 0x00031ECA
		// (set) Token: 0x060012C1 RID: 4801 RVA: 0x00033CD4 File Offset: 0x00031ED4
		public EditableTextWidget SearchInputWidget
		{
			get
			{
				return this._searchInputWidget;
			}
			set
			{
				if (value != this._searchInputWidget)
				{
					if (this._searchInputWidget != null)
					{
						this._searchInputWidget.EventFire -= this.OnSearchInputClick;
					}
					this._searchInputWidget = value;
					base.OnPropertyChanged<EditableTextWidget>(value, "SearchInputWidget");
					if (this._searchInputWidget != null)
					{
						this._searchInputWidget.EventFire += this.OnSearchInputClick;
					}
				}
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00033D3B File Offset: 0x00031F3B
		private void OnSearchInputClick(Widget widget, string eventName, object[] arguments)
		{
			if (eventName == "MouseDown")
			{
				base.EventFired("SearchBarClick", Array.Empty<object>());
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x00033D5A File Offset: 0x00031F5A
		// (set) Token: 0x060012C4 RID: 4804 RVA: 0x00033D62 File Offset: 0x00031F62
		public ScrollablePanel SearchResultPanel
		{
			get
			{
				return this._searchResultPanel;
			}
			set
			{
				if (value != this._searchResultPanel)
				{
					this._searchResultPanel = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "SearchResultPanel");
				}
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x00033D80 File Offset: 0x00031F80
		// (set) Token: 0x060012C6 RID: 4806 RVA: 0x00033D88 File Offset: 0x00031F88
		public int MinCharAmountToShowResults
		{
			get
			{
				return this._minCharAmountToShowResults;
			}
			set
			{
				if (value != this._minCharAmountToShowResults)
				{
					this._minCharAmountToShowResults = value;
					base.OnPropertyChanged(value, "MinCharAmountToShowResults");
				}
			}
		}

		// Token: 0x0400088B RID: 2187
		private bool _showChat;

		// Token: 0x0400088C RID: 2188
		private ScrollablePanel _searchResultPanel;

		// Token: 0x0400088D RID: 2189
		private EditableTextWidget _searchInputWidget;

		// Token: 0x0400088E RID: 2190
		private int _minCharAmountToShowResults;
	}
}
