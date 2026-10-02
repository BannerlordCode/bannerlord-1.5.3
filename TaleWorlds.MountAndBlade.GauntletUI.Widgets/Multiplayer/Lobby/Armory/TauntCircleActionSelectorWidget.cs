using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000BD RID: 189
	public class TauntCircleActionSelectorWidget : CircleActionSelectorWidget
	{
		// Token: 0x060009FA RID: 2554 RVA: 0x0001C0CA File Offset: 0x0001A2CA
		public TauntCircleActionSelectorWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0001C0E4 File Offset: 0x0001A2E4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._currentSelectedIndex != -1)
			{
				Widget child = base.GetChild(this._currentSelectedIndex);
				object obj;
				if (child == null)
				{
					obj = null;
				}
				else
				{
					obj = child.Children.FirstOrDefault<Widget>((Widget c) => c is ButtonWidget);
				}
				ButtonWidget buttonWidget = obj as ButtonWidget;
				Widget widget = ((buttonWidget != null) ? buttonWidget.FindChild("InputKeyContainer", true) : null);
				if (widget != null && !widget.IsVisible)
				{
					base.EventManager.HoveredWidget = buttonWidget;
				}
			}
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0001C170 File Offset: 0x0001A370
		protected override void OnSelectedIndexChanged(int selectedIndex)
		{
			if (this._currentSelectedIndex == selectedIndex)
			{
				return;
			}
			this._currentSelectedIndex = selectedIndex;
			bool flag = false;
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				ButtonWidget buttonWidget = child.Children.FirstOrDefault<Widget>((Widget c) => c is ButtonWidget) as ButtonWidget;
				if (child.GamepadNavigationIndex != -1 && buttonWidget != null)
				{
					bool flag2 = buttonWidget.IsEnabled && this._currentSelectedIndex == i;
					child.DoNotAcceptNavigation = !flag2;
					if (flag2)
					{
						this.SetCurrentNavigationTarget(child);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this.SetCurrentNavigationTarget(this.FallbackNavigationWidget);
			}
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0001C222 File Offset: 0x0001A422
		private void SetCurrentNavigationTarget(Widget target)
		{
			if (this._tauntSlotNavigationTrialCount == -1)
			{
				this._currentNavigationTarget = target;
				this._tauntSlotNavigationTrialCount = 0;
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.NavigationUpdate), 1);
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0001C254 File Offset: 0x0001A454
		private void NavigationUpdate(float dt)
		{
			if (this._currentNavigationTarget != null)
			{
				if (GauntletGamepadNavigationManager.Instance.TryNavigateTo(this._currentNavigationTarget))
				{
					this._currentNavigationTarget = null;
					this._tauntSlotNavigationTrialCount = -1;
					return;
				}
				if (this._tauntSlotNavigationTrialCount < 5)
				{
					this._tauntSlotNavigationTrialCount++;
					base.EventManager.AddLateUpdateAction(this, new Action<float>(this.NavigationUpdate), 1);
					return;
				}
				this._tauntSlotNavigationTrialCount = -1;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x0001C2C2 File Offset: 0x0001A4C2
		// (set) Token: 0x06000A00 RID: 2560 RVA: 0x0001C2CA File Offset: 0x0001A4CA
		public Widget FallbackNavigationWidget
		{
			get
			{
				return this._fallbackNavigationWidget;
			}
			set
			{
				if (value != this._fallbackNavigationWidget)
				{
					this._fallbackNavigationWidget = value;
					base.OnPropertyChanged<Widget>(value, "FallbackNavigationWidget");
				}
			}
		}

		// Token: 0x0400047E RID: 1150
		private Widget _currentNavigationTarget;

		// Token: 0x0400047F RID: 1151
		private int _currentSelectedIndex = -1;

		// Token: 0x04000480 RID: 1152
		private int _tauntSlotNavigationTrialCount = -1;

		// Token: 0x04000481 RID: 1153
		private Widget _fallbackNavigationWidget;
	}
}
