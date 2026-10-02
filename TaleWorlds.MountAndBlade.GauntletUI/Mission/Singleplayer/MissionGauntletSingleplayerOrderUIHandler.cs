using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x0200003F RID: 63
	[OverrideView(typeof(MissionOrderUIHandler))]
	public class MissionGauntletSingleplayerOrderUIHandler : GauntletOrderUIHandler
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0001121F File Offset: 0x0000F41F
		public override bool IsValidForTick
		{
			get
			{
				return !base.MissionScreen.IsPhotoModeEnabled && !GameStateManager.Current.ActiveStateDisabledByUser && (!base.MissionScreen.IsRadialMenuActive || this._dataSource.IsToggleOrderShown);
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00011258 File Offset: 0x0000F458
		protected virtual MissionOrderVM CreateDataSource(OrderController orderController)
		{
			MissionOrderVM missionOrderVM = new MissionOrderVM(orderController, this.IsDeployment, false);
			missionOrderVM.SetCallbacks(new MissionOrderCallbacks
			{
				ToggleMissionInputs = new Action<bool>(base.ToggleScreenRotation),
				GetVisualOrderExecutionParameters = new MissionOrderCallbacks.GetOrderExecutionParametersDelegate(base.GetVisualOrderExecutionParameters),
				SetSuspendTroopPlacer = new MissionOrderCallbacks.ToggleOrderPositionVisibilityDelegate(this.SetSuspendTroopPlacer),
				OnActivateToggleOrder = new MissionOrderCallbacks.OnToggleActivateOrderStateDelegate(base.OnActivateToggleOrder),
				OnDeactivateToggleOrder = new MissionOrderCallbacks.OnToggleActivateOrderStateDelegate(base.OnDeactivateToggleOrder),
				OnTransferTroopsFinished = new MissionOrderCallbacks.OnTransferTroopsFinishedDelegate(this.OnTransferFinished),
				OnBeforeOrder = new MissionOrderCallbacks.OnBeforeOrderDelegate(base.OnBeforeOrder)
			});
			return missionOrderVM;
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00011308 File Offset: 0x0000F508
		public override bool IsDeployment
		{
			get
			{
				Mission mission = base.Mission;
				return mission != null && mission.Mode == MissionMode.Deployment;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0001131E File Offset: 0x0000F51E
		public override bool IsSiegeDeployment
		{
			get
			{
				return this.IsDeployment && this._siegeDeploymentHandler != null;
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00011333 File Offset: 0x0000F533
		public override void OnConversationBegin()
		{
			base.OnConversationBegin();
			MissionOrderVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.TryCloseToggleOrder(false);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0001134D File Offset: 0x0000F54D
		public MissionGauntletSingleplayerOrderUIHandler()
		{
			this.ViewOrderPriority = 14;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00011360 File Offset: 0x0000F560
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			GameKeyContext category = HotKeyManager.GetCategory("MissionOrderHotkeyCategory");
			GameKeyContext category2 = HotKeyManager.GetCategory("GenericPanelGameKeyCategory");
			base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(category);
			this._orderTroopPlacer = base.Mission.GetMissionBehavior<OrderTroopPlacer>();
			OrderTroopPlacer orderTroopPlacer = this._orderTroopPlacer;
			if (((orderTroopPlacer != null) ? orderTroopPlacer.OrderFlag : null) == null)
			{
				Debug.FailedAssert("Order troop placer's order flag is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\Mission\\Singleplayer\\MissionGauntletSingleplayerOrderUIHandler.cs", "OnMissionScreenInitialize", 70);
			}
			base.MissionScreen.OrderFlag = this._orderTroopPlacer.OrderFlag;
			Debug.Print("MissionScreen.OrderFlag has been set (SP)", 0, Debug.DebugColor.White, 17592186044416UL);
			base.MissionScreen.SetOrderFlagVisibility(false);
			this._siegeDeploymentHandler = base.Mission.GetMissionBehavior<SiegeDeploymentHandler>();
			this._formationTargetHandler = base.Mission.GetMissionBehavior<MissionFormationTargetSelectionHandler>();
			if (this._formationTargetHandler != null)
			{
				this._formationTargetHandler.OnFormationFocused += this.OnFormationFocused;
			}
			this._dataSource = this.CreateDataSource(base.Mission.PlayerTeam.PlayerOrderController);
			this._dataSource.SetCancelInputKey(category2.GetHotKey("ToggleEscapeMenu"));
			this._dataSource.TroopController.SetDoneInputKey(category2.GetHotKey("Confirm"));
			this._dataSource.TroopController.SetCancelInputKey(category2.GetHotKey("Exit"));
			this._dataSource.TroopController.SetResetInputKey(category2.GetHotKey("Reset"));
			this._dataSource.SetOrderIndexKey(0, category.GetGameKey(69));
			this._dataSource.SetOrderIndexKey(1, category.GetGameKey(70));
			this._dataSource.SetOrderIndexKey(2, category.GetGameKey(71));
			this._dataSource.SetOrderIndexKey(3, category.GetGameKey(72));
			this._dataSource.SetOrderIndexKey(4, category.GetGameKey(73));
			this._dataSource.SetOrderIndexKey(5, category.GetGameKey(74));
			this._dataSource.SetOrderIndexKey(6, category.GetGameKey(75));
			this._dataSource.SetOrderIndexKey(7, category.GetGameKey(76));
			this._dataSource.SetOrderIndexKey(8, category.GetGameKey(77));
			this._dataSource.SetReturnKey(category.GetGameKey(77));
			this._gauntletLayer = new GauntletLayer("MissionOrder", this.ViewOrderPriority, false);
			this._gauntletLayer.Input.RegisterHotKeyCategory(category2);
			string text;
			if (this.IsDeployment)
			{
				text = this._radialOrderMovieName;
			}
			else
			{
				text = ((BannerlordConfig.OrderType == 0) ? this._barOrderMovieName : this._radialOrderMovieName);
			}
			this._spriteCategory = UIResourceManager.LoadSpriteCategory("ui_order");
			this._movie = this._gauntletLayer.LoadMovie(text, this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			if (!this.IsDeployment && BannerlordConfig.HideBattleUI)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
			this._dataSource.InputRestrictions = this._gauntletLayer.InputRestrictions;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00011680 File Offset: 0x0000F880
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.OrderType)
			{
				if (!this.IsDeployment)
				{
					this._gauntletLayer.ReleaseMovie(this._movie);
					string text = ((BannerlordConfig.OrderType == 0) ? this._barOrderMovieName : this._radialOrderMovieName);
					this._movie = this._gauntletLayer.LoadMovie(text, this._dataSource);
					return;
				}
			}
			else if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.OrderLayoutType)
			{
				MissionOrderVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.OnOrderLayoutTypeChanged();
				return;
			}
			else if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.HideBattleUI)
			{
				if (!this.IsDeployment)
				{
					this._gauntletLayer.UIContext.ContextAlpha = (BannerlordConfig.HideBattleUI ? 0f : 1f);
					return;
				}
			}
			else if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.SlowDownOnOrder && !BannerlordConfig.SlowDownOnOrder && this._slowedDownMission)
			{
				base.Mission.RemoveTimeSpeedRequest(864);
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00011748 File Offset: 0x0000F948
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			if (this._formationTargetHandler != null)
			{
				this._formationTargetHandler.OnFormationFocused -= this.OnFormationFocused;
			}
			this._orderTroopPlacer = null;
			this._movie = null;
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._siegeDeploymentHandler = null;
			this._spriteCategory.Unload();
			this._formationTargetHandler = null;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x000117DA File Offset: 0x0000F9DA
		protected override void OnTransferFinished()
		{
			if (!this.IsDeployment)
			{
				this.SetLayerEnabled(false);
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x000117EB File Offset: 0x0000F9EB
		public void OnAutoDeploy()
		{
			this._dataSource.DeploymentController.ExecuteAutoDeploy();
			this.ClearFormationSelection();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00011803 File Offset: 0x0000FA03
		public void OnBeginMission(bool showSiegeMachineInquiry = false)
		{
			this._dataSource.DeploymentController.ExecuteBeginMission(showSiegeMachineInquiry);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00011818 File Offset: 0x0000FA18
		protected override void SetLayerEnabled(bool isEnabled)
		{
			if (isEnabled)
			{
				if (!base.MissionScreen.IsRadialMenuActive)
				{
					this._orderTroopPlacer.SuspendTroopPlacer = false;
					if (!this._slowedDownMission && BannerlordConfig.SlowDownOnOrder)
					{
						base.Mission.AddTimeSpeedRequest(new Mission.TimeSpeedRequest(0.25f, 864));
						this._slowedDownMission = true;
					}
					base.MissionScreen.SetOrderFlagVisibility(true);
					Game.Current.EventManager.TriggerEvent<MissionPlayerToggledOrderViewEvent>(new MissionPlayerToggledOrderViewEvent(true));
					return;
				}
			}
			else
			{
				this.SetSuspendTroopPlacer(true);
				if (this._slowedDownMission)
				{
					base.Mission.RemoveTimeSpeedRequest(864);
					this._slowedDownMission = false;
				}
				Game.Current.EventManager.TriggerEvent<MissionPlayerToggledOrderViewEvent>(new MissionPlayerToggledOrderViewEvent(false));
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000118D4 File Offset: 0x0000FAD4
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			this._dataSource.OnDeploymentFinished();
			this._dataSource.TryCloseToggleOrder(false);
			this.SetSuspendTroopPlacer(true);
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
			this._gauntletLayer.UIContext.ContextAlpha = (BannerlordConfig.HideBattleUI ? 0f : 1f);
			string text = ((BannerlordConfig.OrderType == 0) ? this._barOrderMovieName : this._radialOrderMovieName);
			if (text != this._radialOrderMovieName)
			{
				this._gauntletLayer.ReleaseMovie(this._movie);
				this._movie = this._gauntletLayer.LoadMovie(text, this._dataSource);
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0001198D File Offset: 0x0000FB8D
		public override void OnAfterDeploymentFinished()
		{
			base.OnAfterDeploymentFinished();
			this._dataSource.OnAfterDeploymentFinished();
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000119A0 File Offset: 0x0000FBA0
		public void ClearFormationSelection()
		{
			MissionOrderVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OrderController.ClearSelectedFormations();
			}
			MissionOrderVM dataSource2 = this._dataSource;
			if (dataSource2 == null)
			{
				return;
			}
			dataSource2.TryCloseToggleOrder(false);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x000119CA File Offset: 0x0000FBCA
		public void OnFiltersSet(List<MissionOrderVM.FormationConfiguration> filterData)
		{
			this._dataSource.OnFiltersSet(filterData);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x000119D8 File Offset: 0x0000FBD8
		private void OnFormationFocused(Formation focusedFormation)
		{
			this._focusedFormation = focusedFormation;
		}

		// Token: 0x0400017F RID: 383
		private const float _slowDownAmountWhileOrderIsOpen = 0.25f;

		// Token: 0x04000180 RID: 384
		private const int _missionTimeSpeedRequestID = 864;
	}
}
