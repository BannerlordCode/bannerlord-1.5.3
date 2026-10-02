using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x02000108 RID: 264
	public class AutoClosePopupWidget : Widget
	{
		// Token: 0x06000E22 RID: 3618 RVA: 0x00026D1C File Offset: 0x00024F1C
		public AutoClosePopupWidget(UIContext context)
			: base(context)
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				AutoClosePopupClosingWidget autoClosePopupClosingWidget;
				if ((autoClosePopupClosingWidget = base.GetChild(i) as AutoClosePopupClosingWidget) != null)
				{
					this._closingWidgets.Add(autoClosePopupClosingWidget);
				}
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00026D68 File Offset: 0x00024F68
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible && base.EventManager.LatestMouseUpWidget != this.PopupParentWidget && base.EventManager.LatestMouseUpWidget != this._lastCheckedMouseUpWidget)
			{
				base.IsVisible = base.EventManager.LatestMouseUpWidget == this || base.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget);
				this.CheckClosingWidgetsAndUpdateVisibility();
				this._lastCheckedMouseUpWidget = (base.IsVisible ? base.EventManager.LatestMouseUpWidget : null);
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00026DF4 File Offset: 0x00024FF4
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			AutoClosePopupClosingWidget autoClosePopupClosingWidget;
			if ((autoClosePopupClosingWidget = child as AutoClosePopupClosingWidget) != null)
			{
				this._closingWidgets.Add(autoClosePopupClosingWidget);
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00026E20 File Offset: 0x00025020
		protected void CheckClosingWidgetsAndUpdateVisibility()
		{
			if (base.IsVisible)
			{
				using (List<AutoClosePopupClosingWidget>.Enumerator enumerator = this._closingWidgets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ShouldClosePopup())
						{
							base.IsVisible = false;
							break;
						}
					}
				}
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x00026E84 File Offset: 0x00025084
		// (set) Token: 0x06000E27 RID: 3623 RVA: 0x00026E8C File Offset: 0x0002508C
		[Editor(false)]
		public Widget PopupParentWidget
		{
			get
			{
				return this._popupParentWidget;
			}
			set
			{
				if (this._popupParentWidget != value)
				{
					this._popupParentWidget = value;
					base.OnPropertyChanged<Widget>(value, "PopupParentWidget");
				}
			}
		}

		// Token: 0x0400066D RID: 1645
		private List<AutoClosePopupClosingWidget> _closingWidgets = new List<AutoClosePopupClosingWidget>();

		// Token: 0x0400066E RID: 1646
		protected Widget _lastCheckedMouseUpWidget;

		// Token: 0x0400066F RID: 1647
		private Widget _popupParentWidget;
	}
}
