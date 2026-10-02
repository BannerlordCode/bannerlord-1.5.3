using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000142 RID: 322
	public class InventoryTupleExtensionControlsWidget : Widget
	{
		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x0002E3D2 File Offset: 0x0002C5D2
		// (set) Token: 0x060010C4 RID: 4292 RVA: 0x0002E3DA File Offset: 0x0002C5DA
		public Widget NavigationParent { get; set; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0002E3E3 File Offset: 0x0002C5E3
		// (set) Token: 0x060010C6 RID: 4294 RVA: 0x0002E3EB File Offset: 0x0002C5EB
		private GamepadNavigationScope _parentScope { get; set; }

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x0002E3F4 File Offset: 0x0002C5F4
		// (set) Token: 0x060010C8 RID: 4296 RVA: 0x0002E3FC File Offset: 0x0002C5FC
		private GamepadNavigationScope _extensionSliderScope { get; set; }

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x060010C9 RID: 4297 RVA: 0x0002E405 File Offset: 0x0002C605
		// (set) Token: 0x060010CA RID: 4298 RVA: 0x0002E40D File Offset: 0x0002C60D
		private GamepadNavigationScope _extensionIncreaseDecreaseScope { get; set; }

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x060010CB RID: 4299 RVA: 0x0002E416 File Offset: 0x0002C616
		// (set) Token: 0x060010CC RID: 4300 RVA: 0x0002E41E File Offset: 0x0002C61E
		private GamepadNavigationScope _extensionButtonsScope { get; set; }

		// Token: 0x060010CD RID: 4301 RVA: 0x0002E427 File Offset: 0x0002C627
		public InventoryTupleExtensionControlsWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0002E430 File Offset: 0x0002C630
		public void BuildNavigationData()
		{
			if (this._isNavigationActive)
			{
				return;
			}
			if (this.TransferSlider != null)
			{
				this._extensionSliderScope = new GamepadNavigationScope
				{
					ScopeID = "ExtensionSliderScope",
					ParentWidget = this.TransferSlider,
					IsEnabled = false,
					NavigateFromScopeEdges = true
				};
			}
			if (this.IncreaseDecreaseButtonsParent != null)
			{
				this._extensionIncreaseDecreaseScope = new GamepadNavigationScope
				{
					ScopeID = "ExtensionIncreaseDecreaseScope",
					ParentWidget = this.IncreaseDecreaseButtonsParent,
					IsEnabled = false,
					ScopeMovements = GamepadNavigationTypes.Horizontal,
					ExtendDiscoveryAreaTop = -40f,
					ExtendDiscoveryAreaBottom = -10f,
					ExtendDiscoveryAreaRight = -350f
				};
			}
			if (this.ButtonCarrier != null)
			{
				this._extensionButtonsScope = new GamepadNavigationScope
				{
					ScopeID = "ExtensionButtonsScope",
					ParentWidget = this.ButtonCarrier,
					IsEnabled = false,
					ScopeMovements = GamepadNavigationTypes.Horizontal
				};
			}
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0002E514 File Offset: 0x0002C714
		private void TransitionTick(float dt)
		{
			if (this._currentVisualStateAnimationState == VisualStateAnimationState.None)
			{
				if (!this._isNavigationActive)
				{
					this.AddGamepadNavigationControls();
					base.EventManager.AddLateUpdateAction(this, delegate(float _dt)
					{
						this.NavigateToBestChildScope();
					}, 1);
					return;
				}
			}
			else
			{
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.TransitionTick), 1);
			}
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0002E56C File Offset: 0x0002C76C
		private void AddGamepadNavigationControls()
		{
			if (this.ValidateParentScope() && !this._isNavigationActive)
			{
				if (this._extensionIncreaseDecreaseScope != null)
				{
					base.GamepadNavigationContext.AddNavigationScope(this._extensionIncreaseDecreaseScope, false);
				}
				if (this._extensionSliderScope != null)
				{
					base.GamepadNavigationContext.AddNavigationScope(this._extensionSliderScope, false);
				}
				if (this._extensionButtonsScope != null)
				{
					base.GamepadNavigationContext.AddNavigationScope(this._extensionButtonsScope, false);
				}
				this.SetEnabledAllScopes(true);
				if (this._extensionSliderScope != null)
				{
					this._extensionSliderScope.SetParentScope(this._parentScope);
				}
				if (this._extensionIncreaseDecreaseScope != null)
				{
					this._extensionIncreaseDecreaseScope.SetParentScope(this._parentScope);
				}
				if (this._extensionButtonsScope != null)
				{
					this._extensionButtonsScope.SetParentScope(this._parentScope);
				}
				base.DoNotAcceptNavigation = false;
				this._isNavigationActive = true;
			}
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0002E640 File Offset: 0x0002C840
		private void RemoveGamepadNavigationControls()
		{
			if (this.ValidateParentScope() && this._isNavigationActive)
			{
				this.SetEnabledAllScopes(false);
				if (this._extensionSliderScope != null)
				{
					this._extensionSliderScope.SetParentScope(null);
					base.GamepadNavigationContext.RemoveNavigationScope(this._extensionSliderScope);
					this._extensionSliderScope = null;
				}
				if (this._extensionIncreaseDecreaseScope != null)
				{
					this._extensionIncreaseDecreaseScope.SetParentScope(null);
					base.GamepadNavigationContext.RemoveNavigationScope(this._extensionIncreaseDecreaseScope);
					this._extensionIncreaseDecreaseScope = null;
				}
				if (this._extensionButtonsScope != null)
				{
					this._extensionButtonsScope.SetParentScope(null);
					base.GamepadNavigationContext.RemoveNavigationScope(this._extensionButtonsScope);
					this._extensionButtonsScope = null;
				}
				base.DoNotAcceptNavigation = true;
				this._isNavigationActive = false;
			}
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0002E6FC File Offset: 0x0002C8FC
		private void SetEnabledAllScopes(bool isEnabled)
		{
			if (this._extensionSliderScope != null)
			{
				this._extensionSliderScope.IsEnabled = isEnabled;
			}
			if (this._extensionIncreaseDecreaseScope != null)
			{
				this._extensionIncreaseDecreaseScope.IsEnabled = isEnabled;
			}
			if (this._extensionButtonsScope != null)
			{
				this._extensionButtonsScope.IsEnabled = isEnabled;
			}
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0002E73C File Offset: 0x0002C93C
		private void NavigateToBestChildScope()
		{
			if (this._parentScope.IsActiveScope)
			{
				GamepadNavigationScope[] array = new GamepadNavigationScope[] { this._extensionSliderScope, this._extensionButtonsScope, this._extensionIncreaseDecreaseScope };
				for (int i = 0; i < array.Length; i++)
				{
					if (GauntletGamepadNavigationManager.Instance.TryNavigateTo(array[i]))
					{
						return;
					}
				}
			}
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0002E796 File Offset: 0x0002C996
		private bool ValidateParentScope()
		{
			if (this._parentScope == null)
			{
				this._parentScope = this.GetParentScope();
			}
			return this._parentScope != null;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0002E7B8 File Offset: 0x0002C9B8
		private GamepadNavigationScope GetParentScope()
		{
			Widget navigationParent = this.NavigationParent;
			for (Widget widget = ((navigationParent != null) ? navigationParent.ParentWidget : null); widget != null; widget = widget.ParentWidget)
			{
				NavigationScopeTargeter navigationScopeTargeter;
				if ((navigationScopeTargeter = widget as NavigationScopeTargeter) != null)
				{
					return navigationScopeTargeter.NavigationScope;
				}
				NavigationScopeTargeter navigationScopeTargeter2 = widget.Children.FirstOrDefault<Widget>((Widget x) => x is NavigationScopeTargeter) as NavigationScopeTargeter;
				if (navigationScopeTargeter2 != null)
				{
					return navigationScopeTargeter2.NavigationScope;
				}
			}
			return null;
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x0002E830 File Offset: 0x0002CA30
		// (set) Token: 0x060010D7 RID: 4311 RVA: 0x0002E838 File Offset: 0x0002CA38
		public bool IsExtended
		{
			get
			{
				return this._isExtended;
			}
			set
			{
				if (value != this._isExtended)
				{
					this._isExtended = value;
					base.IsEnabled = this._isExtended;
					this.SetEnabledAllScopes(false);
					if (this._isExtended)
					{
						this.BuildNavigationData();
						base.EventManager.AddLateUpdateAction(this, new Action<float>(this.TransitionTick), 1);
						return;
					}
					this.RemoveGamepadNavigationControls();
				}
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x0002E896 File Offset: 0x0002CA96
		// (set) Token: 0x060010D9 RID: 4313 RVA: 0x0002E89E File Offset: 0x0002CA9E
		[Editor(false)]
		public Widget TransferSlider
		{
			get
			{
				return this._transferSlider;
			}
			set
			{
				if (this._transferSlider != value)
				{
					this._transferSlider = value;
					base.OnPropertyChanged<Widget>(value, "TransferSlider");
				}
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x0002E8BC File Offset: 0x0002CABC
		// (set) Token: 0x060010DB RID: 4315 RVA: 0x0002E8C4 File Offset: 0x0002CAC4
		[Editor(false)]
		public Widget IncreaseDecreaseButtonsParent
		{
			get
			{
				return this._increaseDecreaseButtonsParent;
			}
			set
			{
				if (this._increaseDecreaseButtonsParent != value)
				{
					this._increaseDecreaseButtonsParent = value;
					base.OnPropertyChanged<Widget>(value, "IncreaseDecreaseButtonsParent");
				}
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060010DC RID: 4316 RVA: 0x0002E8E2 File Offset: 0x0002CAE2
		// (set) Token: 0x060010DD RID: 4317 RVA: 0x0002E8EA File Offset: 0x0002CAEA
		[Editor(false)]
		public Widget ButtonCarrier
		{
			get
			{
				return this._buttonCarrier;
			}
			set
			{
				if (this._buttonCarrier != value)
				{
					this._buttonCarrier = value;
					base.OnPropertyChanged<Widget>(value, "ButtonCarrier");
				}
			}
		}

		// Token: 0x040007A0 RID: 1952
		private bool _isNavigationActive;

		// Token: 0x040007A1 RID: 1953
		private bool _isExtended;

		// Token: 0x040007A2 RID: 1954
		private Widget _transferSlider;

		// Token: 0x040007A3 RID: 1955
		private Widget _increaseDecreaseButtonsParent;

		// Token: 0x040007A4 RID: 1956
		private Widget _buttonCarrier;
	}
}
