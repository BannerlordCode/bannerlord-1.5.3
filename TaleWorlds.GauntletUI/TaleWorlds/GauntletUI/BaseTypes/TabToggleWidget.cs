using System;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200006A RID: 106
	public class TabToggleWidget : ButtonWidget
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0001F59B File Offset: 0x0001D79B
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x0001F5A3 File Offset: 0x0001D7A3
		public TabControl TabControlWidget { get; set; }

		// Token: 0x0600074A RID: 1866 RVA: 0x0001F5AC File Offset: 0x0001D7AC
		public TabToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001F5B5 File Offset: 0x0001D7B5
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this.TabControlWidget != null && !string.IsNullOrEmpty(this.TabName))
			{
				this.TabControlWidget.SetActiveTab(this.TabName);
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001F5E4 File Offset: 0x0001D7E4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			bool flag = false;
			if (this.TabControlWidget == null || string.IsNullOrEmpty(this.TabName))
			{
				flag = true;
			}
			else
			{
				Widget widget = this.TabControlWidget.FindChild(this.TabName);
				if (widget == null || widget.IsDisabled)
				{
					flag = true;
				}
			}
			base.IsDisabled = flag;
			base.IsSelected = this.DetermineIfIsSelected();
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001F648 File Offset: 0x0001D848
		private bool DetermineIfIsSelected()
		{
			TabControl tabControlWidget = this.TabControlWidget;
			return ((tabControlWidget != null) ? tabControlWidget.ActiveTab : null) != null && !string.IsNullOrEmpty(this.TabName) && this.TabControlWidget.ActiveTab.Id == this.TabName && base.IsVisible;
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x0001F69B File Offset: 0x0001D89B
		// (set) Token: 0x0600074F RID: 1871 RVA: 0x0001F6A3 File Offset: 0x0001D8A3
		[Editor(false)]
		public string TabName
		{
			get
			{
				return this._tabName;
			}
			set
			{
				if (this._tabName != value)
				{
					this._tabName = value;
					base.OnPropertyChanged<string>(value, "TabName");
				}
			}
		}

		// Token: 0x04000367 RID: 871
		private string _tabName;
	}
}
