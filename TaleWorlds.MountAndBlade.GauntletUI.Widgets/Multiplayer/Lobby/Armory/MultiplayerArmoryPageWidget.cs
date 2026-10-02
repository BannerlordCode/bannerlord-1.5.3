using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B6 RID: 182
	public class MultiplayerArmoryPageWidget : Widget
	{
		// Token: 0x06000993 RID: 2451 RVA: 0x0001B139 File Offset: 0x00019339
		public MultiplayerArmoryPageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0001B144 File Offset: 0x00019344
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.IsTauntAssignmentActive && !Input.IsGamepadActive)
			{
				Widget latestMouseUpWidget = base.EventManager.LatestMouseUpWidget;
				Widget latestMouseDownWidget = base.EventManager.LatestMouseDownWidget;
				if (latestMouseUpWidget != null && latestMouseUpWidget == latestMouseDownWidget && !this.IsWidgetUsedForTauntSelection(latestMouseUpWidget))
				{
					base.EventFired("ReleaseTauntSelections", Array.Empty<object>());
				}
			}
			if (this.TauntSlotsContainer != null && this.TauntCircleActionSelector != null)
			{
				this.TauntCircleActionSelector.IsCircularInputEnabled = this.IsTauntControlsOpen && this.TauntSlotsContainer.IsPointInsideMeasuredArea(base.EventManager.MousePosition);
			}
			if (this._cosmeticPanelScrollTarget != null && this._cosmeticsScrollablePanel != null)
			{
				ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters(0f, 0f, 0f, 0f, -1f, 0.5f, 0.3f);
				this._cosmeticsScrollablePanel.ScrollToChild(this._cosmeticPanelScrollTarget, autoScrollParameters);
				this._cosmeticPanelScrollTarget = null;
			}
			this.UpdateTauntControlStates(dt);
			this.AnimateTauntAssignmentStates(dt);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0001B23C File Offset: 0x0001943C
		private bool IsWidgetUsedForTauntSelection(Widget widget)
		{
			CircleActionSelectorWidget tauntCircleActionSelector = this.TauntCircleActionSelector;
			MultiplayerLobbyArmoryCosmeticItemButtonWidget multiplayerLobbyArmoryCosmeticItemButtonWidget;
			return (tauntCircleActionSelector != null && tauntCircleActionSelector.CheckIsMyChildRecursive(widget)) || ((multiplayerLobbyArmoryCosmeticItemButtonWidget = widget as MultiplayerLobbyArmoryCosmeticItemButtonWidget) != null && multiplayerLobbyArmoryCosmeticItemButtonWidget.IsSelected);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0001B275 File Offset: 0x00019475
		private void RegisterForStateUpdate()
		{
			this._isTauntStateDirty = true;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0001B280 File Offset: 0x00019480
		private void UpdateTauntControlStates(float dt)
		{
			if (this._isTauntStateDirty)
			{
				string text = (this.IsTauntControlsOpen ? "TauntEnabled" : "Default");
				if (this.TauntCircleActionSelector != null)
				{
					this.TauntCircleActionSelector.AnimateDistanceFromCenterTo((float)(this.IsTauntControlsOpen ? this.TauntEnabledRadialDistance : this.TauntDisabledRadialDistance), this.TauntStateAnimationDuration);
					this.TauntCircleActionSelector.IsEnabled = this.IsTauntControlsOpen;
					this.TauntCircleActionSelector.SetGlobalAlphaRecursively(this.IsTauntControlsOpen ? 1f : 0.6f);
				}
				Widget tauntSlotsContainer = this.TauntSlotsContainer;
				if (tauntSlotsContainer != null)
				{
					tauntSlotsContainer.SetState(text);
				}
				Widget manageTauntsButton = this.ManageTauntsButton;
				if (manageTauntsButton != null)
				{
					manageTauntsButton.SetState(text);
				}
				Widget leftSideParent = this.LeftSideParent;
				if (leftSideParent != null)
				{
					leftSideParent.SetState(text);
				}
				Widget gameModesDropdownParent = this.GameModesDropdownParent;
				if (gameModesDropdownParent != null)
				{
					gameModesDropdownParent.SetState(text);
				}
				Widget heroPreviewParent = this.HeroPreviewParent;
				if (heroPreviewParent != null)
				{
					heroPreviewParent.SetState(text);
				}
				if (this.RightPanelTabControl != null && this.IsTauntControlsOpen)
				{
					this.RightPanelTabControl.SelectedIndex = 1;
				}
				this._isTauntStateDirty = false;
			}
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0001B38C File Offset: 0x0001958C
		private void OnTauntAssignmentStateChanged(bool isTauntAssignmentActive)
		{
			this._tauntAssignmentStateTimer = 0f;
			if (isTauntAssignmentActive && this.TauntCircleActionSelector != null)
			{
				if (this.TauntCircleActionSelector.GetFirstInChildrenRecursive(delegate(Widget c)
				{
					ButtonWidget buttonWidget = c as ButtonWidget;
					return buttonWidget != null && buttonWidget.IsSelected;
				}) != null)
				{
					if (this._cosmeticsScrollablePanel == null)
					{
						this._cosmeticsScrollablePanel = this.RightPanelTabControl.GetFirstInChildrenRecursive((Widget c) => c is ScrollablePanel) as ScrollablePanel;
					}
					if (this._cosmeticsScrollablePanel != null)
					{
						Widget firstInChildrenRecursive = this._cosmeticsScrollablePanel.GetFirstInChildrenRecursive(delegate(Widget c)
						{
							MultiplayerLobbyArmoryCosmeticItemButtonWidget multiplayerLobbyArmoryCosmeticItemButtonWidget;
							return (multiplayerLobbyArmoryCosmeticItemButtonWidget = c as MultiplayerLobbyArmoryCosmeticItemButtonWidget) != null && multiplayerLobbyArmoryCosmeticItemButtonWidget.IsSelectable;
						});
						if (firstInChildrenRecursive != null)
						{
							this._cosmeticPanelScrollTarget = firstInChildrenRecursive;
						}
					}
				}
			}
			if (Input.IsGamepadActive && isTauntAssignmentActive)
			{
				GauntletGamepadNavigationManager.Instance.TryNavigateTo(this.ManageTauntsButton);
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0001B478 File Offset: 0x00019678
		private void AnimateTauntAssignmentStates(float dt)
		{
			float num4;
			if (this._tauntAssignmentStateTimer < this.TauntStateAnimationDuration)
			{
				float num = this._tauntAssignmentStateTimer / this.TauntStateAnimationDuration;
				float num2 = (this.IsTauntAssignmentActive ? 0f : this.TauntAssignmentOverlayAlpha);
				float num3 = (this.IsTauntAssignmentActive ? this.TauntAssignmentOverlayAlpha : 0f);
				num4 = MathF.Lerp(num2, num3, num, 1E-05f);
				this._tauntAssignmentStateTimer += dt;
			}
			else
			{
				num4 = (this.IsTauntAssignmentActive ? this.TauntAssignmentOverlayAlpha : 0f);
			}
			if (this.TauntAssignmentOverlay != null)
			{
				this.TauntAssignmentOverlay.IsVisible = num4 != 0f;
				this.TauntAssignmentOverlay.SetGlobalAlphaRecursively(num4);
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0001B529 File Offset: 0x00019729
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x0001B531 File Offset: 0x00019731
		public bool IsTauntAssignmentActive
		{
			get
			{
				return this._isTauntAssignmentActive;
			}
			set
			{
				if (value != this._isTauntAssignmentActive)
				{
					this._isTauntAssignmentActive = value;
					base.OnPropertyChanged(value, "IsTauntAssignmentActive");
					this.OnTauntAssignmentStateChanged(value);
				}
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x0001B556 File Offset: 0x00019756
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x0001B55E File Offset: 0x0001975E
		public bool IsTauntControlsOpen
		{
			get
			{
				return this._isTauntControlsOpen;
			}
			set
			{
				if (value != this._isTauntControlsOpen)
				{
					this._isTauntControlsOpen = value;
					base.OnPropertyChanged(value, "IsTauntControlsOpen");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0001B582 File Offset: 0x00019782
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x0001B58A File Offset: 0x0001978A
		public int TauntEnabledRadialDistance
		{
			get
			{
				return this._tauntEnabledRadialDistance;
			}
			set
			{
				if (value != this._tauntEnabledRadialDistance)
				{
					this._tauntEnabledRadialDistance = value;
					base.OnPropertyChanged(value, "TauntEnabledRadialDistance");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x0001B5AE File Offset: 0x000197AE
		// (set) Token: 0x060009A1 RID: 2465 RVA: 0x0001B5B6 File Offset: 0x000197B6
		public int TauntDisabledRadialDistance
		{
			get
			{
				return this._tauntDisabledRadialDistance;
			}
			set
			{
				if (value != this._tauntDisabledRadialDistance)
				{
					this._tauntDisabledRadialDistance = value;
					base.OnPropertyChanged(value, "TauntDisabledRadialDistance");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0001B5DA File Offset: 0x000197DA
		// (set) Token: 0x060009A3 RID: 2467 RVA: 0x0001B5E2 File Offset: 0x000197E2
		public float TauntStateAnimationDuration
		{
			get
			{
				return this._tauntStateAnimationDuration;
			}
			set
			{
				if (value != this._tauntStateAnimationDuration)
				{
					this._tauntStateAnimationDuration = value;
					base.OnPropertyChanged(value, "TauntStateAnimationDuration");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0001B606 File Offset: 0x00019806
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x0001B60E File Offset: 0x0001980E
		public float TauntAssignmentOverlayAlpha
		{
			get
			{
				return this._tauntAssignmentOverlayAlpha;
			}
			set
			{
				if (value != this._tauntAssignmentOverlayAlpha)
				{
					this._tauntAssignmentOverlayAlpha = value;
					base.OnPropertyChanged(value, "TauntAssignmentOverlayAlpha");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0001B632 File Offset: 0x00019832
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x0001B63A File Offset: 0x0001983A
		public Widget LeftSideParent
		{
			get
			{
				return this._leftSideParent;
			}
			set
			{
				if (value != this._leftSideParent)
				{
					this._leftSideParent = value;
					base.OnPropertyChanged<Widget>(value, "LeftSideParent");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0001B65E File Offset: 0x0001985E
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x0001B666 File Offset: 0x00019866
		public Widget GameModesDropdownParent
		{
			get
			{
				return this._gameModesDropdownParent;
			}
			set
			{
				if (value != this._gameModesDropdownParent)
				{
					this._gameModesDropdownParent = value;
					base.OnPropertyChanged<Widget>(value, "GameModesDropdownParent");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x0001B68A File Offset: 0x0001988A
		// (set) Token: 0x060009AB RID: 2475 RVA: 0x0001B692 File Offset: 0x00019892
		public Widget HeroPreviewParent
		{
			get
			{
				return this._heroPreviewParent;
			}
			set
			{
				if (value != this._heroPreviewParent)
				{
					this._heroPreviewParent = value;
					base.OnPropertyChanged<Widget>(value, "HeroPreviewParent");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0001B6B6 File Offset: 0x000198B6
		// (set) Token: 0x060009AD RID: 2477 RVA: 0x0001B6BE File Offset: 0x000198BE
		public Widget TauntAssignmentOverlay
		{
			get
			{
				return this._tauntAssignmentOverlay;
			}
			set
			{
				if (value != this._tauntAssignmentOverlay)
				{
					this._tauntAssignmentOverlay = value;
					base.OnPropertyChanged<Widget>(value, "TauntAssignmentOverlay");
				}
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0001B6DC File Offset: 0x000198DC
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x0001B6E4 File Offset: 0x000198E4
		public Widget ManageTauntsButton
		{
			get
			{
				return this._manageTauntsButton;
			}
			set
			{
				if (value != this._manageTauntsButton)
				{
					this._manageTauntsButton = value;
					base.OnPropertyChanged<Widget>(value, "ManageTauntsButton");
				}
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0001B702 File Offset: 0x00019902
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x0001B70A File Offset: 0x0001990A
		public Widget TauntSlotsContainer
		{
			get
			{
				return this._tauntSlotsContainer;
			}
			set
			{
				if (value != this._tauntSlotsContainer)
				{
					this._tauntSlotsContainer = value;
					base.OnPropertyChanged<Widget>(value, "TauntSlotsContainer");
				}
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0001B728 File Offset: 0x00019928
		// (set) Token: 0x060009B3 RID: 2483 RVA: 0x0001B730 File Offset: 0x00019930
		public TabControl RightPanelTabControl
		{
			get
			{
				return this._rightPanelTabControl;
			}
			set
			{
				if (value != this._rightPanelTabControl)
				{
					this._rightPanelTabControl = value;
					base.OnPropertyChanged<TabControl>(value, "RightPanelTabControl");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0001B754 File Offset: 0x00019954
		// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0001B75C File Offset: 0x0001995C
		public CircleActionSelectorWidget TauntCircleActionSelector
		{
			get
			{
				return this._tauntCircleActionSelector;
			}
			set
			{
				if (value != this._tauntCircleActionSelector)
				{
					this._tauntCircleActionSelector = value;
					base.OnPropertyChanged<CircleActionSelectorWidget>(value, "TauntCircleActionSelector");
					if (this._tauntCircleActionSelector != null)
					{
						this._tauntCircleActionSelector.DistanceFromCenterModifier = (float)(this.IsTauntControlsOpen ? this.TauntEnabledRadialDistance : this.TauntDisabledRadialDistance);
					}
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x04000450 RID: 1104
		private bool _isTauntStateDirty;

		// Token: 0x04000451 RID: 1105
		private float _tauntAssignmentStateTimer;

		// Token: 0x04000452 RID: 1106
		private ScrollablePanel _cosmeticsScrollablePanel;

		// Token: 0x04000453 RID: 1107
		private Widget _cosmeticPanelScrollTarget;

		// Token: 0x04000454 RID: 1108
		private bool _isTauntAssignmentActive;

		// Token: 0x04000455 RID: 1109
		private bool _isTauntControlsOpen;

		// Token: 0x04000456 RID: 1110
		private int _tauntEnabledRadialDistance;

		// Token: 0x04000457 RID: 1111
		private int _tauntDisabledRadialDistance;

		// Token: 0x04000458 RID: 1112
		private float _tauntStateAnimationDuration;

		// Token: 0x04000459 RID: 1113
		private float _tauntAssignmentOverlayAlpha;

		// Token: 0x0400045A RID: 1114
		private Widget _leftSideParent;

		// Token: 0x0400045B RID: 1115
		private Widget _gameModesDropdownParent;

		// Token: 0x0400045C RID: 1116
		private Widget _heroPreviewParent;

		// Token: 0x0400045D RID: 1117
		private Widget _tauntAssignmentOverlay;

		// Token: 0x0400045E RID: 1118
		private Widget _manageTauntsButton;

		// Token: 0x0400045F RID: 1119
		private Widget _tauntSlotsContainer;

		// Token: 0x04000460 RID: 1120
		private TabControl _rightPanelTabControl;

		// Token: 0x04000461 RID: 1121
		private CircleActionSelectorWidget _tauntCircleActionSelector;
	}
}
