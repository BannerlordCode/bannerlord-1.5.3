using System;
using System.Collections.Generic;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x0200003B RID: 59
	[OverrideView(typeof(MissionOrderOfBattleUIHandler))]
	public class MissionGauntletOrderOfBattleUIHandler : MissionView
	{
		// Token: 0x060002AD RID: 685 RVA: 0x0000FB5C File Offset: 0x0000DD5C
		public MissionGauntletOrderOfBattleUIHandler(OrderOfBattleVM dataSource)
		{
			this._dataSource = dataSource;
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("OrderOfBattleHotKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetResetInputKey(HotKeyManager.GetCategory("OrderOfBattleHotKeyCategory").GetHotKey("AutoDeploy"));
			this.ViewOrderPriority = 13;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000FBBC File Offset: 0x0000DDBC
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._playerRoleMissionController = base.Mission.GetMissionBehavior<AssignPlayerRoleInTeamMissionController>();
			this._playerRoleMissionController.OnPlayerTurnToChooseFormationToLead += this.OnPlayerTurnToChooseFormationToLead;
			this._playerRoleMissionController.OnAllFormationsAssignedSergeants += this.OnAllFormationsAssignedSergeants;
			this._orderUIHandler = base.Mission.GetMissionBehavior<MissionGauntletSingleplayerOrderUIHandler>();
			this._orderTroopPlacer = base.Mission.GetMissionBehavior<OrderTroopPlacer>();
			OrderTroopPlacer orderTroopPlacer = this._orderTroopPlacer;
			orderTroopPlacer.OnUnitDeployed = (Action)Delegate.Combine(orderTroopPlacer.OnUnitDeployed, new Action(this.OnUnitDeployed));
			this._gauntletLayer = new GauntletLayer("MissionOrderOfBattle", this.ViewOrderPriority, false);
			this._movie = this._gauntletLayer.LoadMovie("OrderOfBattle", this._dataSource);
			this._orderOfBattleCategory = UIResourceManager.LoadSpriteCategory("ui_order_of_battle");
			base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("OrderOfBattleHotKeyCategory"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("OrderOfBattleHotKeyCategory"));
			base.MissionScreen.AddLayer(this._gauntletLayer);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000FCF6 File Offset: 0x0000DEF6
		public override bool IsReady()
		{
			return base.Mission.IsDeploymentFinished || this._orderOfBattleCategory.IsLoaded;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000FD12 File Offset: 0x0000DF12
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._isActive)
			{
				this._dataSource.Tick();
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000FD30 File Offset: 0x0000DF30
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._isActive)
			{
				this._wereHotkeysEnabledLastFrame = this._dataSource.AreHotkeysEnabled;
				this.HandleLayerFocus(out this._isAnyHeroSelected, out this._isClassSelectionEnabled);
				this._dataSource.AreHotkeysEnabled = !base.MissionScreen.IsRadialMenuActive && !base.Mission.IsOrderMenuOpen && TaleWorlds.InputSystem.Input.IsGamepadActive && !this._gauntletLayer.IsFocusLayer;
				this.TickInput();
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000FDB4 File Offset: 0x0000DFB4
		private void DestroyView()
		{
			if (this._gauntletLayer == null && this._dataSource == null)
			{
				return;
			}
			if (this._isActive)
			{
				ManagedOptions.SetConfig(ManagedOptions.ManagedOptionsType.OrderType, this._cachedOrderTypeSetting);
			}
			this._isActive = false;
			base.MissionScreen.SetDisplayDialog(false);
			this._dataSource.OnFinalize();
			this._dataSource = null;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._orderOfBattleCategory.Unload();
			this._playerRoleMissionController.OnPlayerTurnToChooseFormationToLead -= this.OnPlayerTurnToChooseFormationToLead;
			this._playerRoleMissionController.OnAllFormationsAssignedSergeants -= this.OnAllFormationsAssignedSergeants;
			OrderTroopPlacer orderTroopPlacer = this._orderTroopPlacer;
			orderTroopPlacer.OnUnitDeployed = (Action)Delegate.Remove(orderTroopPlacer.OnUnitDeployed, new Action(this.OnUnitDeployed));
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000FE88 File Offset: 0x0000E088
		private void TickInput()
		{
			if (base.MissionScreen.SceneLayer.Input.IsKeyDown(InputKey.RightMouseButton) || base.MissionScreen.SceneLayer.Input.IsKeyDown(InputKey.ControllerLTrigger))
			{
				this._gauntletLayer.InputRestrictions.SetMouseVisibility(false);
				this._dataSource.AreCameraControlsEnabled = true;
			}
			else
			{
				this._gauntletLayer.InputRestrictions.SetMouseVisibility(true);
				this._dataSource.AreCameraControlsEnabled = false;
			}
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				if (this._isClassSelectionEnabled)
				{
					UISoundsHelper.PlayUISound("event:/ui/oob/dropdown");
					this._dataSource.ExecuteDisableAllClassSelections();
				}
				else if (this._isAnyHeroSelected && this._dataSource.CanToggleHeroSelection)
				{
					UISoundsHelper.PlayUISound("event:/ui/oob/officer_pick");
					this._dataSource.ExecuteClearHeroSelection();
				}
			}
			if (base.MissionScreen.SceneLayer.Input.IsHotKeyPressed("AutoDeploy"))
			{
				this._isResetPressed = this._dataSource.AreHotkeysEnabled && this._wereHotkeysEnabledLastFrame;
			}
			if (base.MissionScreen.SceneLayer.Input.IsHotKeyPressed("Confirm"))
			{
				this._isReadyPressed = this._dataSource.AreHotkeysEnabled && this._wereHotkeysEnabledLastFrame;
			}
			if (!this._dataSource.AreHotkeysEnabled)
			{
				this._isResetPressed = false;
				this._isReadyPressed = false;
			}
			if (base.MissionScreen.SceneLayer.Input.IsHotKeyReleased("AutoDeploy") && this._dataSource.AreHotkeysEnabled && this._isResetPressed)
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.ExecuteAutoDeploy();
			}
			if (base.MissionScreen.SceneLayer.Input.IsHotKeyReleased("Confirm") && this._dataSource.AreHotkeysEnabled && this._dataSource.CanStartMission && this._isReadyPressed)
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.ExecuteBeginMission();
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00010090 File Offset: 0x0000E290
		private void HandleLayerFocus(out bool isAnyHeroSelected, out bool isClassSelectionEnabled)
		{
			isAnyHeroSelected = this._dataSource.HasSelectedHeroes;
			isClassSelectionEnabled = this._dataSource.IsAnyClassSelectionEnabled();
			bool flag = isAnyHeroSelected | isClassSelectionEnabled;
			if (this._gauntletLayer.IsFocusLayer && !flag)
			{
				base.MissionScreen.SetDisplayDialog(false);
				this._gauntletLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this._gauntletLayer);
				return;
			}
			if (!this._gauntletLayer.IsFocusLayer && flag)
			{
				base.MissionScreen.SetDisplayDialog(true);
				this._gauntletLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this._gauntletLayer);
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00010128 File Offset: 0x0000E328
		public override void OnMissionScreenFinalize()
		{
			this.DestroyView();
			base.OnMissionScreenFinalize();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00010138 File Offset: 0x0000E338
		public override bool OnEscape()
		{
			bool flag = false;
			if (this._isActive)
			{
				bool flag2 = false;
				if (this._orderUIHandler != null && this._orderUIHandler.IsOrderMenuActive)
				{
					flag2 = this._orderUIHandler.IsAnyOrderSetActive;
					flag = this._orderUIHandler.OnEscape();
				}
				if (!flag2)
				{
					flag = this._dataSource.OnEscape() || flag;
				}
			}
			return flag;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00010190 File Offset: 0x0000E390
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000101B5 File Offset: 0x0000E3B5
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000101DA File Offset: 0x0000E3DA
		public override bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return !this._isActive;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000101E8 File Offset: 0x0000E3E8
		private void OnPlayerTurnToChooseFormationToLead(Dictionary<int, Agent> lockedFormationIndicesAndSergeants, List<int> remainingFormationIndices)
		{
			if (base.Mission.PlayerTeam == null)
			{
				Debug.FailedAssert("Player team must be initialized before OOB", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\Mission\\Singleplayer\\MissionGauntletOrderOfBattleUIHandler.cs", "OnPlayerTurnToChooseFormationToLead", 284);
			}
			this._cachedOrderTypeSetting = ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.OrderType);
			ManagedOptions.SetConfig(ManagedOptions.ManagedOptionsType.OrderType, 1f);
			this._dataSource.Initialize(base.Mission, base.MissionScreen.CombatCamera, new Action<int>(this.SelectFormationAtIndex), new Action<int>(this.DeselectFormationAtIndex), new Action(this.ClearFormationSelection), new Action(this.OnAutoDeploy), new Action(this.OnBeginMission), lockedFormationIndicesAndSergeants);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._isActive = true;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000102A6 File Offset: 0x0000E4A6
		private void OnAllFormationsAssignedSergeants(Dictionary<int, Agent> formationsWithLooselyAssignedSergeants)
		{
			this._dataSource.OnAllFormationsAssignedSergeants(formationsWithLooselyAssignedSergeants);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x000102B4 File Offset: 0x0000E4B4
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			bool flag = MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle();
			this._dataSource.OnDeploymentFinalized(flag);
			this.DestroyView();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000102E9 File Offset: 0x0000E4E9
		private void SelectFormationAtIndex(int index)
		{
			MissionGauntletSingleplayerOrderUIHandler orderUIHandler = this._orderUIHandler;
			if (orderUIHandler == null)
			{
				return;
			}
			orderUIHandler.SelectFormationAtIndex(index);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000102FC File Offset: 0x0000E4FC
		private void DeselectFormationAtIndex(int index)
		{
			MissionGauntletSingleplayerOrderUIHandler orderUIHandler = this._orderUIHandler;
			if (orderUIHandler == null)
			{
				return;
			}
			orderUIHandler.DeselectFormationAtIndex(index);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0001030F File Offset: 0x0000E50F
		private void ClearFormationSelection()
		{
			MissionGauntletSingleplayerOrderUIHandler orderUIHandler = this._orderUIHandler;
			if (orderUIHandler == null)
			{
				return;
			}
			orderUIHandler.ClearFormationSelection();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00010321 File Offset: 0x0000E521
		private void OnAutoDeploy()
		{
			this._orderUIHandler.OnAutoDeploy();
			SiegeDeploymentVM siegeDeployment = this._dataSource.SiegeDeployment;
			if (siegeDeployment == null)
			{
				return;
			}
			siegeDeployment.AutoDeploySiegeMachines();
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00010343 File Offset: 0x0000E543
		private void OnBeginMission()
		{
			this._orderUIHandler.OnFiltersSet(this._dataSource.CurrentConfiguration);
			MissionGauntletSingleplayerOrderUIHandler orderUIHandler = this._orderUIHandler;
			SiegeDeploymentVM siegeDeployment = this._dataSource.SiegeDeployment;
			orderUIHandler.OnBeginMission(siegeDeployment != null && siegeDeployment.HasUndeployedSiegeMachines());
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0001037D File Offset: 0x0000E57D
		private void OnUnitDeployed()
		{
			this._dataSource.OnUnitDeployed();
		}

		// Token: 0x04000159 RID: 345
		private OrderOfBattleVM _dataSource;

		// Token: 0x0400015A RID: 346
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400015B RID: 347
		private GauntletMovieIdentifier _movie;

		// Token: 0x0400015C RID: 348
		private SpriteCategory _orderOfBattleCategory;

		// Token: 0x0400015D RID: 349
		private MissionGauntletSingleplayerOrderUIHandler _orderUIHandler;

		// Token: 0x0400015E RID: 350
		private AssignPlayerRoleInTeamMissionController _playerRoleMissionController;

		// Token: 0x0400015F RID: 351
		private OrderTroopPlacer _orderTroopPlacer;

		// Token: 0x04000160 RID: 352
		private bool _isActive;

		// Token: 0x04000161 RID: 353
		private bool _wereHotkeysEnabledLastFrame;

		// Token: 0x04000162 RID: 354
		private bool _isResetPressed;

		// Token: 0x04000163 RID: 355
		private bool _isReadyPressed;

		// Token: 0x04000164 RID: 356
		private bool _isAnyHeroSelected;

		// Token: 0x04000165 RID: 357
		private bool _isClassSelectionEnabled;

		// Token: 0x04000166 RID: 358
		private float _cachedOrderTypeSetting;
	}
}
