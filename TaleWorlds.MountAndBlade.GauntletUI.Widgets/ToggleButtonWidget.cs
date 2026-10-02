using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000044 RID: 68
	public class ToggleButtonWidget : ButtonWidget
	{
		// Token: 0x060003DD RID: 989 RVA: 0x0000C48C File Offset: 0x0000A68C
		public ToggleButtonWidget(UIContext context)
			: base(context)
		{
			this.ClickEventHandlers.Add(new Action<Widget>(this.OnClick));
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000C4AD File Offset: 0x0000A6AD
		protected virtual void OnClick(Widget widget)
		{
			if (this._widgetToClose != null)
			{
				this.IsTargetVisible = !this._widgetToClose.IsVisible;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0000C4CB File Offset: 0x0000A6CB
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0000C4DE File Offset: 0x0000A6DE
		public bool IsTargetVisible
		{
			get
			{
				Widget widgetToClose = this._widgetToClose;
				return widgetToClose != null && widgetToClose.IsVisible;
			}
			set
			{
				if (this._widgetToClose != null && this._widgetToClose.IsVisible != value)
				{
					this._widgetToClose.IsVisible = value;
					base.OnPropertyChanged(value, "IsTargetVisible");
				}
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0000C50E File Offset: 0x0000A70E
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x0000C516 File Offset: 0x0000A716
		[Editor(false)]
		public Widget WidgetToClose
		{
			get
			{
				return this._widgetToClose;
			}
			set
			{
				if (this._widgetToClose != value)
				{
					this._widgetToClose = value;
					base.OnPropertyChanged<Widget>(value, "WidgetToClose");
				}
			}
		}

		// Token: 0x0400019B RID: 411
		private Widget _widgetToClose;
	}
}
