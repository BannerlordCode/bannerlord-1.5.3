using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000045 RID: 69
	public class ToggleStateButtonWidget : ButtonWidget
	{
		// Token: 0x060003E3 RID: 995 RVA: 0x0000C534 File Offset: 0x0000A734
		public ToggleStateButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000C54C File Offset: 0x0000A74C
		protected override void HandleClick()
		{
			foreach (Action<Widget> action in this.ClickEventHandlers)
			{
				action(this);
			}
			bool isSelected = base.IsSelected;
			if (!base.IsSelected)
			{
				base.IsSelected = true;
			}
			else if (this.AllowSwitchOff)
			{
				base.IsSelected = false;
			}
			if (base.IsSelected && !isSelected && this.NotifyParentForSelection && base.ParentWidget is Container)
			{
				(base.ParentWidget as Container).OnChildSelected(this);
			}
			if (this.AllowSwitchOff && !base.IsSelected && this.NotifyParentForSelection && base.ParentWidget is Container)
			{
				(base.ParentWidget as Container).OnChildSelected(null);
			}
			base.EventFired("Click", Array.Empty<object>());
			if (base.Context.EventManager.Time - this._lastClickTime < 0.5f)
			{
				base.EventFired("DoubleClick", Array.Empty<object>());
				return;
			}
			this._lastClickTime = base.Context.EventManager.Time;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000C680 File Offset: 0x0000A880
		protected override void RefreshState()
		{
			base.RefreshState();
			if (base.UpdateChildrenStates)
			{
				this.UpdateChildrenStatesRecursively(this);
			}
			if (this._widgetToClose != null)
			{
				this._widgetToClose.IsVisible = base.IsSelected;
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000C6B0 File Offset: 0x0000A8B0
		private void UpdateChildrenStatesRecursively(Widget parent)
		{
			parent.SetState(base.CurrentState);
			if (parent.ChildCount > 0)
			{
				foreach (Widget widget in parent.Children)
				{
					this.UpdateChildrenStatesRecursively(widget);
				}
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000C718 File Offset: 0x0000A918
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x0000C720 File Offset: 0x0000A920
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
					if (this._widgetToClose != null)
					{
						this._widgetToClose.IsVisible = base.IsSelected;
					}
				}
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0000C757 File Offset: 0x0000A957
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x0000C75F File Offset: 0x0000A95F
		[Editor(false)]
		public bool AllowSwitchOff
		{
			get
			{
				return this._allowSwitchOff;
			}
			set
			{
				if (this._allowSwitchOff != value)
				{
					this._allowSwitchOff = value;
					base.OnPropertyChanged(value, "AllowSwitchOff");
				}
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x0000C77D File Offset: 0x0000A97D
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x0000C785 File Offset: 0x0000A985
		[Editor(false)]
		public bool NotifyParentForSelection
		{
			get
			{
				return this._notifyParentForSelection;
			}
			set
			{
				if (this._notifyParentForSelection != value)
				{
					this._notifyParentForSelection = value;
					base.OnPropertyChanged(value, "NotifyParentForSelection");
				}
			}
		}

		// Token: 0x0400019C RID: 412
		private Widget _widgetToClose;

		// Token: 0x0400019D RID: 413
		private bool _allowSwitchOff = true;

		// Token: 0x0400019E RID: 414
		private bool _notifyParentForSelection = true;
	}
}
