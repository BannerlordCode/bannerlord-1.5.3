using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013E RID: 318
	public class InventoryEquippedItemControlsBrushWidget : BrushWidget
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600109E RID: 4254 RVA: 0x0002D970 File Offset: 0x0002BB70
		// (remove) Token: 0x0600109F RID: 4255 RVA: 0x0002D9A8 File Offset: 0x0002BBA8
		public event Action OnHidePanel;

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x0002D9DD File Offset: 0x0002BBDD
		// (set) Token: 0x060010A1 RID: 4257 RVA: 0x0002D9E5 File Offset: 0x0002BBE5
		public NavigationForcedScopeCollectionTargeter ForcedScopeCollection { get; set; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x0002D9EE File Offset: 0x0002BBEE
		// (set) Token: 0x060010A3 RID: 4259 RVA: 0x0002D9F6 File Offset: 0x0002BBF6
		public NavigationScopeTargeter NavigationScope { get; set; }

		// Token: 0x060010A4 RID: 4260 RVA: 0x0002D9FF File Offset: 0x0002BBFF
		public InventoryEquippedItemControlsBrushWidget(UIContext context)
			: base(context)
		{
			base.AddState("LeftHidden");
			base.AddState("LeftVisible");
			base.AddState("RightHidden");
			base.AddState("RightVisible");
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0002DA34 File Offset: 0x0002BC34
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isScopeDirty && base.EventManager.Time - this._lastTransitionStartTime > base.VisualDefinition.TransitionDuration)
			{
				this.ForcedScopeCollection.IsCollectionDisabled = base.CurrentState == "RightHidden" || base.CurrentState == "LeftHidden";
				this.NavigationScope.IsScopeDisabled = this.ForcedScopeCollection.IsCollectionDisabled;
				this._isScopeDirty = false;
			}
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0002DABC File Offset: 0x0002BCBC
		public void ShowPanel()
		{
			if (this._panelVisible)
			{
				return;
			}
			if (this.ItemWidget.IsRightSide)
			{
				base.HorizontalAlignment = HorizontalAlignment.Right;
				base.Brush.HorizontalFlip = false;
				this.SetState("RightHidden");
				base.PositionXOffset = base.VisualDefinition.VisualStates["RightHidden"].PositionXOffset;
				this.SetState("RightVisible");
			}
			else
			{
				base.HorizontalAlignment = HorizontalAlignment.Left;
				base.Brush.HorizontalFlip = true;
				this.SetState("LeftHidden");
				base.PositionXOffset = base.VisualDefinition.VisualStates["LeftHidden"].PositionXOffset;
				this.SetState("LeftVisible");
			}
			base.IsVisible = true;
			this._panelVisible = true;
			this._isScopeDirty = true;
			this._lastTransitionStartTime = base.Context.EventManager.Time;
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0002DBA0 File Offset: 0x0002BDA0
		public void HidePanel()
		{
			if (!this._panelVisible)
			{
				return;
			}
			if (this.ItemWidget.IsRightSide)
			{
				this.SetState("RightHidden");
			}
			else
			{
				this.SetState("LeftHidden");
			}
			Action onHidePanel = this.OnHidePanel;
			if (onHidePanel != null)
			{
				onHidePanel();
			}
			this._panelVisible = false;
			this._isScopeDirty = true;
			this._lastTransitionStartTime = base.Context.EventManager.Time;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0002DC10 File Offset: 0x0002BE10
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._panelVisible && this.ItemWidget.IsSelected)
			{
				this.ShowPanel();
				return;
			}
			if (this._panelVisible && !this.ItemWidget.IsSelected)
			{
				this.HidePanel();
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x0002DC50 File Offset: 0x0002BE50
		// (set) Token: 0x060010AA RID: 4266 RVA: 0x0002DC58 File Offset: 0x0002BE58
		[Editor(false)]
		public InventoryItemButtonWidget ItemWidget
		{
			get
			{
				return this._itemWidget;
			}
			set
			{
				if (this._itemWidget != value)
				{
					this._itemWidget = value;
					base.OnPropertyChanged<InventoryItemButtonWidget>(value, "ItemWidget");
				}
			}
		}

		// Token: 0x0400078E RID: 1934
		private float _lastTransitionStartTime;

		// Token: 0x0400078F RID: 1935
		private bool _isScopeDirty;

		// Token: 0x04000790 RID: 1936
		private bool _panelVisible;

		// Token: 0x04000793 RID: 1939
		private InventoryItemButtonWidget _itemWidget;

		// Token: 0x020001CF RID: 463
		// (Invoke) Token: 0x060015AB RID: 5547
		public delegate void ButtonClickEventHandler(Widget itemWidget);
	}
}
