using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Siege
{
	// Token: 0x02000122 RID: 290
	public class MapSiegeScreenWidget : Widget
	{
		// Token: 0x06000F69 RID: 3945 RVA: 0x0002A98B File Offset: 0x00028B8B
		public MapSiegeScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0002A994 File Offset: 0x00028B94
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			Widget latestMouseUpWidget = base.EventManager.LatestMouseUpWidget;
			if (this._currentSelectedButton != null && latestMouseUpWidget != null && !(latestMouseUpWidget is MapSiegeMachineButtonWidget) && !this._currentSelectedButton.CheckIsMyChildRecursive(latestMouseUpWidget) && this.IsWidgetChildOfType<MapSiegeMachineButtonWidget>(latestMouseUpWidget) == null)
			{
				this.SetCurrentButton(null);
			}
			if (base.EventManager.LatestMouseUpWidget == null)
			{
				this.SetCurrentButton(null);
			}
			if (this.DeployableSiegeMachinesPopup != null)
			{
				this.DeployableSiegeMachinesPopup.IsVisible = this._currentSelectedButton != null;
			}
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0002AA18 File Offset: 0x00028C18
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._currentSelectedButton != null && this.DeployableSiegeMachinesPopup != null)
			{
				this.DeployableSiegeMachinesPopup.ScaledPositionXOffset = Mathf.Clamp(this._currentSelectedButton.GlobalPosition.X - this.DeployableSiegeMachinesPopup.Size.X / 2f + this._currentSelectedButton.Size.X / 2f, 0f, base.EventManager.PageSize.X - this.DeployableSiegeMachinesPopup.Size.X);
				this.DeployableSiegeMachinesPopup.ScaledPositionYOffset = Mathf.Clamp(this._currentSelectedButton.GlobalPosition.Y + this._currentSelectedButton.Size.Y + 10f * base._inverseScaleToUse, 0f, base.EventManager.PageSize.Y - this.DeployableSiegeMachinesPopup.Size.Y);
			}
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0002AB1A File Offset: 0x00028D1A
		public void SetCurrentButton(MapSiegeMachineButtonWidget button)
		{
			if (button == null)
			{
				this._currentSelectedButton = null;
				return;
			}
			if (this._currentSelectedButton == button || !button.IsDeploymentTarget)
			{
				this.SetCurrentButton(null);
				return;
			}
			this._currentSelectedButton = button;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0002AB47 File Offset: 0x00028D47
		protected override bool OnPreviewMousePressed()
		{
			this.SetCurrentButton(null);
			return false;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0002AB51 File Offset: 0x00028D51
		protected override bool OnPreviewDragEnd()
		{
			return false;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0002AB54 File Offset: 0x00028D54
		protected override bool OnPreviewDragBegin()
		{
			return false;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0002AB57 File Offset: 0x00028D57
		protected override bool OnPreviewDrop()
		{
			return false;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0002AB5A File Offset: 0x00028D5A
		protected override bool OnPreviewDragHover()
		{
			return false;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0002AB5D File Offset: 0x00028D5D
		protected override bool OnPreviewMouseMove()
		{
			return false;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0002AB60 File Offset: 0x00028D60
		protected override bool OnPreviewMouseReleased()
		{
			return false;
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0002AB63 File Offset: 0x00028D63
		protected override bool OnPreviewMouseScroll()
		{
			return false;
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0002AB66 File Offset: 0x00028D66
		protected override bool OnPreviewMouseAlternatePressed()
		{
			return false;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0002AB69 File Offset: 0x00028D69
		protected override bool OnPreviewMouseAlternateReleased()
		{
			return false;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0002AB6C File Offset: 0x00028D6C
		private T IsWidgetChildOfType<T>(Widget currentWidget) where T : Widget
		{
			while (currentWidget != null)
			{
				if (currentWidget is T)
				{
					return (T)((object)currentWidget);
				}
				currentWidget = currentWidget.ParentWidget;
			}
			return default(T);
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06000F78 RID: 3960 RVA: 0x0002AB9E File Offset: 0x00028D9E
		// (set) Token: 0x06000F79 RID: 3961 RVA: 0x0002ABA6 File Offset: 0x00028DA6
		[Editor(false)]
		public Widget DeployableSiegeMachinesPopup
		{
			get
			{
				return this._deployableSiegeMachinesPopup;
			}
			set
			{
				if (value != this._deployableSiegeMachinesPopup)
				{
					this._deployableSiegeMachinesPopup = value;
					base.OnPropertyChanged<Widget>(value, "DeployableSiegeMachinesPopup");
				}
			}
		}

		// Token: 0x0400070D RID: 1805
		private Widget _deployableSiegeMachinesPopup;

		// Token: 0x0400070E RID: 1806
		private MapSiegeMachineButtonWidget _currentSelectedButton;
	}
}
