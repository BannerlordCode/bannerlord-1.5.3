using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000016 RID: 22
	[OverrideView(typeof(MultiplayerMissionOrderUIHandler))]
	public class MissionGauntletMultiplayerOrderUIHandler : GauntletOrderUIHandler
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000066DC File Offset: 0x000048DC
		public override bool IsDeployment
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000066DF File Offset: 0x000048DF
		public override bool IsSiegeDeployment
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000066E2 File Offset: 0x000048E2
		public override bool IsValidForTick
		{
			get
			{
				return this._shouldTick && (!base.MissionScreen.IsRadialMenuActive || this._dataSource.IsToggleOrderShown) && !GameStateManager.Current.ActiveStateDisabledByUser;
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006718 File Offset: 0x00004918
		public MissionGauntletMultiplayerOrderUIHandler()
		{
			this.ViewOrderPriority = 19;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006775 File Offset: 0x00004975
		public override bool IsReady()
		{
			return true;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006778 File Offset: 0x00004978
		public override void AfterStart()
		{
			base.AfterStart();
			int num;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.NumberOfBotsPerFormation, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out num);
			this._shouldTick = num > 0;
			this.RefreshFormationTargetingVisibilityConfig();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000067AF File Offset: 0x000049AF
		private void RefreshFormationTargetingVisibilityConfig()
		{
			this.RefreshFormationTargetingVisibilityGate();
			this.RefreshFormationMarkerDistanceConfig();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000067C0 File Offset: 0x000049C0
		private void RefreshFormationTargetingVisibilityGate()
		{
			int num;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.FormationTargetingVisibilityMode, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out num);
			int num2;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.FormationTargetingVisibilityThreshold, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out num2);
			int num3;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.FormationTargetingVisibilityAppliesAtCloseRange, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out num3);
			if (this._appliedVisibilityModeInt == num && this._appliedVisibilityThreshold == num2 && this._appliedVisibilityAppliesAtCloseRangeInt == num3)
			{
				return;
			}
			MissionFormationTargetSelectionHandler missionBehavior = base.Mission.GetMissionBehavior<MissionFormationTargetSelectionHandler>();
			if (missionBehavior == null)
			{
				return;
			}
			missionBehavior.SetVisibilityConfig(new MissionFormationTargetSelectionHandler.VisibilityConfig
			{
				Mode = (MultiplayerOptions.FormationTargetingVisibilityModes)num,
				Threshold = num2,
				AppliesAtCloseRange = (num3 != 0)
			});
			this._appliedVisibilityModeInt = num;
			this._appliedVisibilityThreshold = num2;
			this._appliedVisibilityAppliesAtCloseRangeInt = num3;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006878 File Offset: 0x00004A78
		private void RefreshFormationMarkerDistanceConfig()
		{
			int num;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.FormationMarkerFarDistanceCutoff, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out num);
			int num2;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.FormationMarkerFarAlphaTarget, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out num2);
			int num3;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.FormationMarkerAlwaysOnDistance, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out num3);
			if (this._appliedMarkerFarDistanceCutoff == num && this._appliedMarkerFarAlphaTarget == num2 && this._appliedMarkerAlwaysOnDistance == num3)
			{
				return;
			}
			MissionGauntletFormationMarker missionBehavior = base.Mission.GetMissionBehavior<MissionGauntletFormationMarker>();
			if (missionBehavior == null || !missionBehavior.IsViewCreated)
			{
				return;
			}
			float num4 = ((num >= 0) ? ((float)num) : (-1f));
			float num5 = ((num2 >= 0) ? ((float)num2 / 100f) : (-1f));
			float num6 = ((num3 >= 0) ? ((float)num3) : (-1f));
			missionBehavior.SetMarkerDistanceConfig(num4, num5, num6);
			this._appliedMarkerFarDistanceCutoff = num;
			this._appliedMarkerFarAlphaTarget = num2;
			this._appliedMarkerAlwaysOnDistance = num3;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000694C File Offset: 0x00004B4C
		public override void OnMissionScreenTick(float dt)
		{
			this.RefreshFormationTargetingVisibilityConfig();
			if (this.IsValidForTick)
			{
				if (!this._isInitialized)
				{
					Team team = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>().Team : null);
					if (team != null && (team == base.Mission.AttackerTeam || team == base.Mission.DefenderTeam))
					{
						this.InitializeInADisgustingManner();
					}
				}
				if (!this._isValid)
				{
					Team team2 = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>().Team : null);
					if (team2 != null && (team2 == base.Mission.AttackerTeam || team2 == base.Mission.DefenderTeam))
					{
						this.ValidateInADisgustingManner();
					}
					return;
				}
				if (this._shouldInitializeFormationInfo)
				{
					Team team3 = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>().Team : null);
					if (this._dataSource != null && team3 != null)
					{
						this._dataSource.AfterInitialize();
						this._shouldInitializeFormationInfo = false;
					}
				}
			}
			base.OnMissionScreenTick(dt);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006A40 File Offset: 0x00004C40
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("MissionOrderHotkeyCategory"));
			this._siegeDeploymentHandler = null;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			MissionMultiplayerGameModeBaseClient missionBehavior = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._roundComponent = ((missionBehavior != null) ? missionBehavior.RoundComponent : null);
			if (this._roundComponent != null)
			{
				this._roundComponent.OnRoundStarted += this.OnRoundStarted;
				this._roundComponent.OnPreparationEnded += this.OnPreparationEnded;
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006AEC File Offset: 0x00004CEC
		private void OnRoundStarted()
		{
			MissionOrderVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.AfterInitialize();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00006AFE File Offset: 0x00004CFE
		private void OnPreparationEnded()
		{
			this._shouldInitializeFormationInfo = true;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00006B08 File Offset: 0x00004D08
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.OrderType)
			{
				if (this._gauntletLayer != null && this._movie != null)
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
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006B80 File Offset: 0x00004D80
		public override void OnMissionScreenFinalize()
		{
			this.Clear();
			this._orderTroopPlacer = null;
			MissionPeer.OnTeamChanged -= this.TeamChange;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			if (this._formationTargetHandler != null)
			{
				this._formationTargetHandler.OnFormationFocused -= this.OnFormationFocused;
				this._formationTargetHandler = null;
			}
			if (this._roundComponent != null)
			{
				this._roundComponent.OnRoundStarted -= this.OnRoundStarted;
				this._roundComponent.OnPreparationEnded -= this.OnPreparationEnded;
			}
			base.OnMissionScreenFinalize();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006C2D File Offset: 0x00004E2D
		protected override void OnTransferFinished()
		{
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006C30 File Offset: 0x00004E30
		protected override void SetLayerEnabled(bool isEnabled)
		{
			if (isEnabled)
			{
				this._orderTroopPlacer.SuspendTroopPlacer = false;
				base.MissionScreen.SetOrderFlagVisibility(true);
				Game.Current.EventManager.TriggerEvent<MissionPlayerToggledOrderViewEvent>(new MissionPlayerToggledOrderViewEvent(true));
				return;
			}
			this._orderTroopPlacer.SuspendTroopPlacer = true;
			base.MissionScreen.SetOrderFlagVisibility(false);
			base.MissionScreen.UnregisterRadialMenuObject(this);
			Game.Current.EventManager.TriggerEvent<MissionPlayerToggledOrderViewEvent>(new MissionPlayerToggledOrderViewEvent(false));
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006CA8 File Offset: 0x00004EA8
		public void InitializeInADisgustingManner()
		{
			if (this._isInitialized)
			{
				Debug.Print("InitializeInADisgustingManner called while already initialized!", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert("InitializeInADisgustingManner called while already initialized!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.GauntletUI\\Mission\\MissionGauntletMultiplayerOrderUIHandler.cs", "InitializeInADisgustingManner", 274);
			}
			Debug.Print(string.Format("InitializeInADisgustingManner is called. IsValidForTick: {0}", this.IsValidForTick), 0, Debug.DebugColor.White, 17592186044416UL);
			base.AfterStart();
			this._orderTroopPlacer = base.Mission.GetMissionBehavior<OrderTroopPlacer>();
			OrderTroopPlacer orderTroopPlacer = this._orderTroopPlacer;
			if (((orderTroopPlacer != null) ? orderTroopPlacer.OrderFlag : null) == null)
			{
				Debug.FailedAssert("Order troop placer's order flag is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.GauntletUI\\Mission\\MissionGauntletMultiplayerOrderUIHandler.cs", "InitializeInADisgustingManner", 283);
			}
			base.MissionScreen.OrderFlag = this._orderTroopPlacer.OrderFlag;
			Debug.Print("MissionScreen.OrderFlag has been set (MP)", 0, Debug.DebugColor.White, 17592186044416UL);
			base.MissionScreen.SetOrderFlagVisibility(false);
			this._formationTargetHandler = base.Mission.GetMissionBehavior<MissionFormationTargetSelectionHandler>();
			if (this._formationTargetHandler != null)
			{
				this._formationTargetHandler.OnFormationFocused += this.OnFormationFocused;
			}
			MissionPeer.OnTeamChanged += this.TeamChange;
			this._isInitialized = true;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006DD6 File Offset: 0x00004FD6
		private void OnFormationFocused(Formation focusedFormation)
		{
			this._focusedFormation = focusedFormation;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006DE0 File Offset: 0x00004FE0
		public void ValidateInADisgustingManner()
		{
			this._dataSource = new MissionOrderVM(base.Mission.PlayerTeam.PlayerOrderController, false, true);
			this._dataSource.SetCallbacks(new MissionOrderCallbacks
			{
				ToggleMissionInputs = new Action<bool>(base.ToggleScreenRotation),
				GetVisualOrderExecutionParameters = new MissionOrderCallbacks.GetOrderExecutionParametersDelegate(base.GetVisualOrderExecutionParameters),
				SetSuspendTroopPlacer = new MissionOrderCallbacks.ToggleOrderPositionVisibilityDelegate(this.SetSuspendTroopPlacer),
				OnActivateToggleOrder = new MissionOrderCallbacks.OnToggleActivateOrderStateDelegate(base.OnActivateToggleOrder),
				OnDeactivateToggleOrder = new MissionOrderCallbacks.OnToggleActivateOrderStateDelegate(base.OnDeactivateToggleOrder),
				OnTransferTroopsFinished = new MissionOrderCallbacks.OnTransferTroopsFinishedDelegate(this.OnTransferFinished),
				OnBeforeOrder = new MissionOrderCallbacks.OnBeforeOrderDelegate(base.OnBeforeOrder)
			});
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("ToggleEscapeMenu"));
			this._dataSource.TroopController.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.TroopController.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.TroopController.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
			GameKeyContext category = HotKeyManager.GetCategory("MissionOrderHotkeyCategory");
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
			this._gauntletLayer = new GauntletLayer("MultiplayerOrder", this.ViewOrderPriority, false);
			this._spriteCategory = UIResourceManager.LoadSpriteCategory("ui_order");
			string text = ((BannerlordConfig.OrderType == 0) ? this._barOrderMovieName : this._radialOrderMovieName);
			this._movie = this._gauntletLayer.LoadMovie(text, this._dataSource);
			this._dataSource.InputRestrictions = this._gauntletLayer.InputRestrictions;
			base.MissionScreen.AddLayer(this._gauntletLayer);
			this._dataSource.AfterInitialize();
			this._isValid = true;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00007090 File Offset: 0x00005290
		private void Clear()
		{
			if (this._gauntletLayer != null)
			{
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
			}
			if (this._dataSource != null)
			{
				this._dataSource.OnFinalize();
			}
			this._gauntletLayer = null;
			this._dataSource = null;
			this._movie = null;
			if (this._isValid)
			{
				this._spriteCategory.Unload();
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000070F1 File Offset: 0x000052F1
		private void TeamChange(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (peer.IsMine)
			{
				this.Clear();
				this._isValid = false;
			}
		}

		// Token: 0x0400006A RID: 106
		private IRoundComponent _roundComponent;

		// Token: 0x0400006B RID: 107
		private bool _isValid;

		// Token: 0x0400006C RID: 108
		private bool _shouldTick;

		// Token: 0x0400006D RID: 109
		private bool _shouldInitializeFormationInfo;

		// Token: 0x0400006E RID: 110
		private const int NotAppliedYet = -2147483648;

		// Token: 0x0400006F RID: 111
		private int _appliedVisibilityModeInt = int.MinValue;

		// Token: 0x04000070 RID: 112
		private int _appliedVisibilityThreshold = int.MinValue;

		// Token: 0x04000071 RID: 113
		private int _appliedVisibilityAppliesAtCloseRangeInt = int.MinValue;

		// Token: 0x04000072 RID: 114
		private int _appliedMarkerFarDistanceCutoff = int.MinValue;

		// Token: 0x04000073 RID: 115
		private int _appliedMarkerFarAlphaTarget = int.MinValue;

		// Token: 0x04000074 RID: 116
		private int _appliedMarkerAlwaysOnDistance = int.MinValue;
	}
}
