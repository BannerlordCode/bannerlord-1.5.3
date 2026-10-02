using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.MainAgentControlMode
{
	// Token: 0x020000FC RID: 252
	public class MainAgentControlModeParentWidget : Widget
	{
		// Token: 0x06000D5C RID: 3420 RVA: 0x00024B96 File Offset: 0x00022D96
		public MainAgentControlModeParentWidget(UIContext context)
			: base(context)
		{
			this._selectionItems = new List<ButtonWidget>();
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00024BB8 File Offset: 0x00022DB8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isSelectionItemsDirty && this._selectionItems != null && !string.IsNullOrEmpty(this.ChildItemId))
			{
				this.UnregisterChildEvents();
				this._selectionItems = this._controlModesList.FindChildrenWithId<ButtonWidget>(this.ChildItemId, true);
				this.RegisterChildEvents();
				this._isSelectionItemsDirty = false;
			}
			this.AnimationTick(dt);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00024C1B File Offset: 0x00022E1B
		private void OnControlModesListUpdated(Widget widget, string eventName, object[] args)
		{
			if (eventName == "ItemAdd" || eventName == "ItemRemove")
			{
				this._isSelectionItemsDirty = true;
			}
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00024C3E File Offset: 0x00022E3E
		private void OnControlItemUpdated(PropertyOwnerObject widget, string propertyName, bool value)
		{
			if (propertyName == "IsSelected" && !this.IsActive)
			{
				this.StartIndicatorAnimation();
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00024C5B File Offset: 0x00022E5B
		private void StartIndicatorAnimation()
		{
			this._animationTimer = 0f;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00024C68 File Offset: 0x00022E68
		private void AnimationTick(float dt)
		{
			if (this.SelectionIndicatorWidget == null)
			{
				return;
			}
			if (this._animationTimer < this.AnimationFirstStepDuration + this.AnimationSecondStepDuration)
			{
				float num = ((this._animationTimer < this.AnimationFirstStepDuration) ? (this._animationTimer / this.AnimationFirstStepDuration) : ((this._animationTimer - this.AnimationFirstStepDuration) / this.AnimationSecondStepDuration));
				num = Mathf.Clamp(num, 0f, 1f);
				float num2 = ((this._animationTimer < this.AnimationFirstStepDuration) ? 1f : 0f);
				float num3 = Mathf.Lerp((this._animationTimer < this.AnimationFirstStepDuration) ? 0f : 1f, num2, num);
				this.SelectionIndicatorWidget.SetGlobalAlphaRecursively(num3);
				this._animationTimer += dt;
				return;
			}
			if (this.SelectionIndicatorWidget.AlphaFactor > 0f)
			{
				this.SelectionIndicatorWidget.SetGlobalAlphaRecursively(0f);
			}
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00024D54 File Offset: 0x00022F54
		private void RegisterChildEvents()
		{
			for (int i = 0; i < this._selectionItems.Count; i++)
			{
				this._selectionItems[i].boolPropertyChanged += this.OnControlItemUpdated;
			}
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00024D94 File Offset: 0x00022F94
		private void UnregisterChildEvents()
		{
			for (int i = 0; i < this._selectionItems.Count; i++)
			{
				this._selectionItems[i].boolPropertyChanged -= this.OnControlItemUpdated;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00024DD4 File Offset: 0x00022FD4
		// (set) Token: 0x06000D65 RID: 3429 RVA: 0x00024DDC File Offset: 0x00022FDC
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
				}
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00024DFA File Offset: 0x00022FFA
		// (set) Token: 0x06000D67 RID: 3431 RVA: 0x00024E02 File Offset: 0x00023002
		public float AnimationFirstStepDuration
		{
			get
			{
				return this._animationFirstStepDuration;
			}
			set
			{
				if (value != this._animationFirstStepDuration)
				{
					this._animationFirstStepDuration = value;
					base.OnPropertyChanged(value, "AnimationFirstStepDuration");
				}
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x00024E20 File Offset: 0x00023020
		// (set) Token: 0x06000D69 RID: 3433 RVA: 0x00024E28 File Offset: 0x00023028
		public float AnimationSecondStepDuration
		{
			get
			{
				return this._animationSecondStepDuration;
			}
			set
			{
				if (value != this._animationSecondStepDuration)
				{
					this._animationSecondStepDuration = value;
					base.OnPropertyChanged(value, "AnimationSecondStepDuration");
				}
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00024E46 File Offset: 0x00023046
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x00024E4E File Offset: 0x0002304E
		public string ChildItemId
		{
			get
			{
				return this._childItemId;
			}
			set
			{
				if (value != this._childItemId)
				{
					this._childItemId = value;
					base.OnPropertyChanged<string>(value, "ChildItemId");
				}
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00024E71 File Offset: 0x00023071
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x00024E7C File Offset: 0x0002307C
		public ListPanel ControlModesList
		{
			get
			{
				return this._controlModesList;
			}
			set
			{
				if (value != this._controlModesList)
				{
					if (this._controlModesList != null)
					{
						this._controlModesList.EventFire -= this.OnControlModesListUpdated;
					}
					this._controlModesList = value;
					base.OnPropertyChanged<ListPanel>(value, "ControlModesList");
					if (this._controlModesList != null)
					{
						this._controlModesList.EventFire += this.OnControlModesListUpdated;
					}
					this._isSelectionItemsDirty = true;
				}
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00024EEA File Offset: 0x000230EA
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x00024EF2 File Offset: 0x000230F2
		public Widget SelectionIndicatorWidget
		{
			get
			{
				return this._selectionIndicatorWidget;
			}
			set
			{
				if (value != this._selectionIndicatorWidget)
				{
					this._selectionIndicatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "SelectionIndicatorWidget");
				}
			}
		}

		// Token: 0x04000610 RID: 1552
		private List<ButtonWidget> _selectionItems;

		// Token: 0x04000611 RID: 1553
		private float _animationTimer = float.MaxValue;

		// Token: 0x04000612 RID: 1554
		private bool _isSelectionItemsDirty;

		// Token: 0x04000613 RID: 1555
		private bool _isActive;

		// Token: 0x04000614 RID: 1556
		private float _animationFirstStepDuration;

		// Token: 0x04000615 RID: 1557
		private float _animationSecondStepDuration;

		// Token: 0x04000616 RID: 1558
		private string _childItemId;

		// Token: 0x04000617 RID: 1559
		private ListPanel _controlModesList;

		// Token: 0x04000618 RID: 1560
		private Widget _selectionIndicatorWidget;
	}
}
