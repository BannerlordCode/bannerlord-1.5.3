using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000073 RID: 115
	public class OrderSiegeDeploymentScreenWidget : Widget
	{
		// Token: 0x06000636 RID: 1590 RVA: 0x000126A5 File Offset: 0x000108A5
		public OrderSiegeDeploymentScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000126AE File Offset: 0x000108AE
		public void SetSelectedDeploymentItem(OrderSiegeDeploymentItemButtonWidget deploymentItem)
		{
			this._selectedDeploymentItem = deploymentItem;
			this.DeploymentListPanel.ParentWidget.IsVisible = this._selectedDeploymentItem != null;
			this.UpdatePosition();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000126D6 File Offset: 0x000108D6
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.UpdatePosition();
			this.HandleClickOutside();
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x000126EC File Offset: 0x000108EC
		private void HandleClickOutside()
		{
			if (this._selectedDeploymentItem == null)
			{
				return;
			}
			if (this._selectedDeploymentItem.IsPointInsideMeasuredArea(base.EventManager.MousePosition) || this.DeploymentListPanel.ParentWidget.IsPointInsideMeasuredArea(base.EventManager.MousePosition))
			{
				return;
			}
			InputKey[] clickKeys = base.Context.InputContext.GetClickKeys();
			for (int i = 0; i < clickKeys.Length; i++)
			{
				if (Input.IsKeyPressed(clickKeys[i]))
				{
					base.EventFired("SelectNone", Array.Empty<object>());
					return;
				}
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00012772 File Offset: 0x00010972
		private void UpdateEnabledState(bool isEnabled)
		{
			this.SetGlobalAlphaRecursively(isEnabled ? 1f : 0.5f);
			base.DoNotPassEventsToChildren = !isEnabled;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00012794 File Offset: 0x00010994
		private void UpdatePosition()
		{
			if (this._selectedDeploymentItem == null)
			{
				return;
			}
			this.DeploymentListPanel.MarginLeft = (this._selectedDeploymentItem.GlobalPosition.X + this._selectedDeploymentItem.Size.Y + 20f) / base._scaleToUse;
			this.DeploymentListPanel.MarginTop = (this._selectedDeploymentItem.GlobalPosition.Y + (this._selectedDeploymentItem.Size.Y / 2f - this.DeploymentListPanel.Size.Y / 2f)) / base._scaleToUse;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x00012833 File Offset: 0x00010A33
		// (set) Token: 0x0600063D RID: 1597 RVA: 0x0001283B File Offset: 0x00010A3B
		public bool IsSiegeDeploymentDisabled
		{
			get
			{
				return this._isSiegeDeploymentDisabled;
			}
			set
			{
				if (value != this._isSiegeDeploymentDisabled)
				{
					this._isSiegeDeploymentDisabled = value;
					base.OnPropertyChanged(value, "IsSiegeDeploymentDisabled");
					this.UpdateEnabledState(!value);
				}
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x00012863 File Offset: 0x00010A63
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x0001286B File Offset: 0x00010A6B
		public Widget DeploymentTargetsParent
		{
			get
			{
				return this._deploymentTargetsParent;
			}
			set
			{
				if (this._deploymentTargetsParent != value)
				{
					this._deploymentTargetsParent = value;
					base.OnPropertyChanged<Widget>(value, "DeploymentTargetsParent");
				}
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x00012889 File Offset: 0x00010A89
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x00012891 File Offset: 0x00010A91
		public ListPanel DeploymentListPanel
		{
			get
			{
				return this._deploymentListPanel;
			}
			set
			{
				if (this._deploymentListPanel != value)
				{
					this._deploymentListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "DeploymentListPanel");
				}
			}
		}

		// Token: 0x040002A8 RID: 680
		private OrderSiegeDeploymentItemButtonWidget _selectedDeploymentItem;

		// Token: 0x040002A9 RID: 681
		private bool _isSiegeDeploymentDisabled;

		// Token: 0x040002AA RID: 682
		private Widget _deploymentTargetsParent;

		// Token: 0x040002AB RID: 683
		private ListPanel _deploymentListPanel;
	}
}
