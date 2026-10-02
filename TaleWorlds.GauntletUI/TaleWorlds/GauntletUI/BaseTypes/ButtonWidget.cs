using System;
using System.Collections.Generic;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000053 RID: 83
	public class ButtonWidget : ImageWidget
	{
		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00017793 File Offset: 0x00015993
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x0001779B File Offset: 0x0001599B
		[Editor(false)]
		public ButtonType ButtonType
		{
			get
			{
				return this._buttonType;
			}
			set
			{
				if (this._buttonType != value)
				{
					this._buttonType = value;
					this.Refresh();
				}
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x000177B3 File Offset: 0x000159B3
		protected override bool OnPreviewMousePressed()
		{
			base.OnPreviewMousePressed();
			return true;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x000177C0 File Offset: 0x000159C0
		protected override void RefreshState()
		{
			base.RefreshState();
			if (!base.OverrideDefaultStateSwitchingEnabled)
			{
				if (base.IsDisabled)
				{
					this.SetState("Disabled");
				}
				else if (this.IsSelected && this.DominantSelectedState)
				{
					this.SetState("Selected");
				}
				else if (base.IsPressed)
				{
					this.SetState("Pressed");
				}
				else if (base.IsHovered)
				{
					this.SetState("Hovered");
				}
				else if (this.IsSelected && !this.DominantSelectedState)
				{
					this.SetState("Selected");
				}
				else
				{
					this.SetState("Default");
				}
			}
			if (base.UpdateChildrenStates)
			{
				for (int i = 0; i < base.ChildCount; i++)
				{
					Widget child = base.GetChild(i);
					if (!(child is ImageWidget) || !((ImageWidget)child).OverrideDefaultStateSwitchingEnabled)
					{
						child.SetState(base.CurrentState);
					}
				}
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x000178A4 File Offset: 0x00015AA4
		private void Refresh()
		{
			if (this.IsToggle)
			{
				this.ShowHideToggle();
			}
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x000178B4 File Offset: 0x00015AB4
		private void ShowHideToggle()
		{
			if (this.ToggleIndicator != null)
			{
				if (this._isSelected)
				{
					this.ToggleIndicator.Show();
					return;
				}
				this.ToggleIndicator.Hide();
			}
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x000178DD File Offset: 0x00015ADD
		public ButtonWidget(UIContext context)
			: base(context)
		{
			base.FrictionEnabled = true;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00017900 File Offset: 0x00015B00
		protected internal override void OnMousePressed()
		{
			if (this._clickState == ButtonWidget.ButtonClickState.None)
			{
				this._clickState = ButtonWidget.ButtonClickState.HandlingClick;
				base.IsPressed = true;
				if (!base.DoNotPassEventsToChildren)
				{
					for (int i = 0; i < base.ChildCount; i++)
					{
						Widget child = base.GetChild(i);
						if (child != null)
						{
							child.IsPressed = true;
						}
					}
				}
			}
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00017950 File Offset: 0x00015B50
		protected internal override void OnMouseReleased(bool isFromInput)
		{
			if (this._clickState == ButtonWidget.ButtonClickState.HandlingClick)
			{
				this._clickState = ButtonWidget.ButtonClickState.None;
				base.IsPressed = false;
				if (!base.DoNotPassEventsToChildren)
				{
					for (int i = 0; i < base.ChildCount; i++)
					{
						Widget child = base.GetChild(i);
						if (child != null)
						{
							child.IsPressed = false;
						}
					}
				}
				if (this.IsPointInsideMeasuredAreaAndCheckIfVisible() && isFromInput)
				{
					this.HandleClick();
				}
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000179AF File Offset: 0x00015BAF
		private bool IsPointInsideMeasuredAreaAndCheckIfVisible()
		{
			return base.IsPointInsideMeasuredArea(base.EventManager.MousePosition) && base.IsRecursivelyVisible();
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x000179D0 File Offset: 0x00015BD0
		protected internal override void OnMouseAlternatePressed()
		{
			if (this._clickState == ButtonWidget.ButtonClickState.None)
			{
				this._clickState = ButtonWidget.ButtonClickState.HandlingAlternateClick;
				base.IsPressed = true;
				if (!base.DoNotPassEventsToChildren)
				{
					for (int i = 0; i < base.ChildCount; i++)
					{
						Widget child = base.GetChild(i);
						if (child != null)
						{
							child.IsPressed = true;
						}
					}
				}
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00017A20 File Offset: 0x00015C20
		protected internal override void OnMouseAlternateReleased(bool isFromInput)
		{
			if (this._clickState == ButtonWidget.ButtonClickState.HandlingAlternateClick)
			{
				this._clickState = ButtonWidget.ButtonClickState.None;
				base.IsPressed = false;
				if (!base.DoNotPassEventsToChildren)
				{
					for (int i = 0; i < base.ChildCount; i++)
					{
						Widget child = base.GetChild(i);
						if (child != null)
						{
							child.IsPressed = false;
						}
					}
				}
				if (this.IsPointInsideMeasuredAreaAndCheckIfVisible() && isFromInput)
				{
					this.HandleAlternateClick();
				}
			}
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00017A80 File Offset: 0x00015C80
		protected virtual void HandleClick()
		{
			if (base.IsDisabled)
			{
				return;
			}
			foreach (Action<Widget> action in this.ClickEventHandlers)
			{
				action(this);
			}
			bool isSelected = this.IsSelected;
			if (this.IsToggle)
			{
				this.IsSelected = !this.IsSelected;
			}
			else if (this.IsRadio)
			{
				this.IsSelected = true;
				if (this.IsSelected && !isSelected && base.ParentWidget is Container)
				{
					(base.ParentWidget as Container).OnChildSelected(this);
				}
			}
			base.EventFired("Click", Array.Empty<object>());
			if (base.Context.EventManager.Time - this._lastClickTime < 0.5f)
			{
				base.EventFired("DoubleClick", Array.Empty<object>());
				return;
			}
			this._lastClickTime = base.Context.EventManager.Time;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00017B88 File Offset: 0x00015D88
		protected virtual void HandleAlternateClick()
		{
			base.EventFired("AlternateClick", Array.Empty<object>());
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00017B9A File Offset: 0x00015D9A
		public bool IsToggle
		{
			get
			{
				return this.ButtonType == ButtonType.Toggle;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00017BA5 File Offset: 0x00015DA5
		public bool IsRadio
		{
			get
			{
				return this.ButtonType == ButtonType.Radio;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00017BB0 File Offset: 0x00015DB0
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00017BB8 File Offset: 0x00015DB8
		[Editor(false)]
		public Widget ToggleIndicator
		{
			get
			{
				return this._toggleIndicator;
			}
			set
			{
				if (this._toggleIndicator != value)
				{
					this._toggleIndicator = value;
					this.Refresh();
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00017BD0 File Offset: 0x00015DD0
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x00017BD8 File Offset: 0x00015DD8
		[Editor(false)]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected != value)
				{
					this._isSelected = value;
					this.Refresh();
					this.RefreshState();
					base.OnPropertyChanged(value, "IsSelected");
				}
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00017C02 File Offset: 0x00015E02
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00017C0A File Offset: 0x00015E0A
		[Editor(false)]
		public bool DominantSelectedState
		{
			get
			{
				return this._dominantSelectedState;
			}
			set
			{
				if (this._dominantSelectedState != value)
				{
					this._dominantSelectedState = value;
					base.OnPropertyChanged(value, "DominantSelectedState");
				}
			}
		}

		// Token: 0x040002A8 RID: 680
		protected const float _maxDoubleClickDeltaTimeInSeconds = 0.5f;

		// Token: 0x040002A9 RID: 681
		protected float _lastClickTime;

		// Token: 0x040002AA RID: 682
		private ButtonWidget.ButtonClickState _clickState;

		// Token: 0x040002AB RID: 683
		private ButtonType _buttonType;

		// Token: 0x040002AC RID: 684
		public List<Action<Widget>> ClickEventHandlers = new List<Action<Widget>>();

		// Token: 0x040002AD RID: 685
		private Widget _toggleIndicator;

		// Token: 0x040002AE RID: 686
		private bool _isSelected;

		// Token: 0x040002AF RID: 687
		private bool _dominantSelectedState = true;

		// Token: 0x0200008F RID: 143
		private enum ButtonClickState
		{
			// Token: 0x0400047E RID: 1150
			None,
			// Token: 0x0400047F RID: 1151
			HandlingClick,
			// Token: 0x04000480 RID: 1152
			HandlingAlternateClick
		}
	}
}
