using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.WalkMode;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x02000032 RID: 50
	[OverrideView(typeof(MissionMainAgentControlModeView))]
	public class MissionGauntletMainAgentControlModeView : MissionView
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000C59B File Offset: 0x0000A79B
		private float _slowDownAmountWhileRadialIsOpen
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000C5A2 File Offset: 0x0000A7A2
		private float _minOpenHoldTime
		{
			get
			{
				return 0.22f;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000C5A9 File Offset: 0x0000A7A9
		private bool IsDisplayingADialog
		{
			get
			{
				IMissionScreen missionScreenAsInterface = this._missionScreenAsInterface;
				return (missionScreenAsInterface != null && missionScreenAsInterface.GetDisplayDialog()) || base.MissionScreen.IsRadialMenuActive || base.Mission.IsOrderMenuOpen;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000C5D9 File Offset: 0x0000A7D9
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000C5E1 File Offset: 0x0000A7E1
		private bool HoldHandled
		{
			get
			{
				return this._holdHandled;
			}
			set
			{
				this._holdHandled = value;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000C5EA File Offset: 0x0000A7EA
		public MissionGauntletMainAgentControlModeView()
		{
			this._missionScreenAsInterface = base.MissionScreen;
			this.HoldHandled = false;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000C608 File Offset: 0x0000A808
		public override void EarlyStart()
		{
			base.EarlyStart();
			this._gauntletLayer = new GauntletLayer("MissionAgentControlMode", 3, false);
			this._dataSource = new MissionMainAgentWalkModeControllerVM();
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("CombatHotKeyCategory"));
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
			this._gauntletLayer.LoadMovie("MainAgentControlMode", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			this._mainAgentController = base.Mission.GetMissionBehavior<MissionMainAgentController>();
			base.Mission.OnMainAgentChanged += this.OnMainAgentChanged;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
		public override void AfterStart()
		{
			base.AfterStart();
			this.InitializeWalkModes();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000C6C4 File Offset: 0x0000A8C4
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.Mission.OnMainAgentChanged -= this.OnMainAgentChanged;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000C718 File Offset: 0x0000A918
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			Agent mainAgent = base.Mission.MainAgent;
			if (mainAgent == null || mainAgent.HasMount)
			{
				this._playerDismountTimer = 0f;
			}
			else if (this._playerDismountTimer < 2f)
			{
				this._playerDismountTimer += dt;
			}
			if (this.IsMainAgentAvailable() && (!base.MissionScreen.IsRadialMenuActive || this._dataSource.IsEnabled))
			{
				this.TickControls(dt);
				return;
			}
			if (this._dataSource.IsEnabled)
			{
				this.HandleClosingHold();
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000C7AC File Offset: 0x0000A9AC
		private void InitializeWalkModes()
		{
			GameKeyContext category = HotKeyManager.GetCategory("CombatHotKeyCategory");
			this._dataSource.AddWalkMode("walk", new TextObject("{=zmS2FpJH}Toggle Walk", null), () => base.Mission.MainAgent != null && base.Mission.MainAgent.WalkMode, delegate(bool value)
			{
				if (base.Mission.MainAgent != null)
				{
					if (value)
					{
						this._mainAgentController.AddOverrideControlsForFrame(MissionMainAgentController.OverrideMainAgentControlFlag.Walk);
						return;
					}
					this._mainAgentController.AddOverrideControlsForFrame(MissionMainAgentController.OverrideMainAgentControlFlag.Run);
				}
			}, () => true, category.GetHotKey("ControllerToggleWalk"), true);
			this._dataSource.AddWalkMode("crouch", new TextObject("{=0pd93SuK}Toggle Crouch", null), () => base.Mission.MainAgent != null && base.Mission.MainAgent.CrouchMode, delegate(bool value)
			{
				if (base.Mission.MainAgent != null)
				{
					if (value)
					{
						this._mainAgentController.AddOverrideControlsForFrame(MissionMainAgentController.OverrideMainAgentControlFlag.Crouch);
						return;
					}
					this._mainAgentController.AddOverrideControlsForFrame(MissionMainAgentController.OverrideMainAgentControlFlag.Stand);
				}
			}, () => base.Mission.MainAgent == null || base.Mission.MainAgent.IsCrouchingAllowed(), category.GetHotKey("ControllerToggleCrouch"), true);
			this._dataSource.LastUsedItem = this._dataSource.ControlModes.FirstOrDefault<WalkModeItemVM>((WalkModeItemVM w) => w.TypeId == "crouch");
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000C8A7 File Offset: 0x0000AAA7
		private void OnMainAgentChanged(Agent oldAgent)
		{
			if (base.Mission.MainAgent == null)
			{
				if (this.HoldHandled)
				{
					this.HoldHandled = false;
				}
				this._toggleHoldTime = 0f;
				this._dataSource.SetEnabled(false);
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000C8DC File Offset: 0x0000AADC
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent == Agent.Main)
			{
				this.HandleClosingHold();
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000C8EC File Offset: 0x0000AAEC
		private void TickControls(float dt)
		{
			if (base.MissionScreen.SceneLayer.Input.IsHotKeyDown("ControlModeToggle") && !base.MissionScreen.IsPhotoModeEnabled && !this.IsDisplayingADialog && base.Mission.Mode != MissionMode.Deployment && base.Mission.Mode != MissionMode.CutScene && !base.MissionScreen.IsRadialMenuActive)
			{
				if (this._toggleHoldTime > this._minOpenHoldTime && !this.HoldHandled)
				{
					this.HandleOpeningHold();
					this.HoldHandled = true;
				}
				this._toggleHoldTime += dt;
				this._prevKeyDown = true;
			}
			else if (this._prevKeyDown && !base.MissionScreen.SceneLayer.Input.IsHotKeyDown("ControlModeToggle"))
			{
				if (this._toggleHoldTime < this._minOpenHoldTime)
				{
					this.HandleQuickRelease();
				}
				else
				{
					this.HandleClosingHold();
				}
				this.HoldHandled = false;
				this._toggleHoldTime = 0f;
				this._prevKeyDown = false;
			}
			if (this._dataSource.IsEnabled)
			{
				for (int i = 0; i < this._dataSource.ControlModes.Count; i++)
				{
					WalkModeItemVM walkModeItemVM = this._dataSource.ControlModes[i];
					InputKeyItemVM toggleInputKey = walkModeItemVM.ToggleInputKey;
					if (((toggleInputKey.HotKey != null && base.Input.IsHotKeyReleased(toggleInputKey.HotKey.Id)) || (toggleInputKey.GameKey != null && base.Input.IsGameKeyReleased(toggleInputKey.GameKey.Id))) && !walkModeItemVM.IsDisabled)
					{
						walkModeItemVM.ToggleState();
						this.HandleClosingHold();
						return;
					}
				}
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000CA84 File Offset: 0x0000AC84
		private void HandleOpeningHold()
		{
			MissionMainAgentWalkModeControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.SetEnabled(true);
			}
			base.MissionScreen.RegisterRadialMenuObject<MissionGauntletMainAgentControlModeView>(this);
			if (!GameNetwork.IsMultiplayer && !this._isSlowDownApplied)
			{
				base.Mission.AddTimeSpeedRequest(new Mission.TimeSpeedRequest(this._slowDownAmountWhileRadialIsOpen, 813));
				this._isSlowDownApplied = true;
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000CAE0 File Offset: 0x0000ACE0
		private void HandleClosingHold()
		{
			MissionMainAgentWalkModeControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.SetEnabled(false);
			}
			base.MissionScreen.UnregisterRadialMenuObject(this);
			if (!GameNetwork.IsMultiplayer && this._isSlowDownApplied)
			{
				base.Mission.RemoveTimeSpeedRequest(813);
				this._isSlowDownApplied = false;
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000CB31 File Offset: 0x0000AD31
		private void HandleQuickRelease()
		{
			MissionMainAgentWalkModeControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				WalkModeItemVM lastUsedItem = dataSource.LastUsedItem;
				if (lastUsedItem != null)
				{
					lastUsedItem.ToggleState();
				}
			}
			MissionMainAgentWalkModeControllerVM dataSource2 = this._dataSource;
			if (dataSource2 != null)
			{
				dataSource2.SetEnabled(false);
			}
			base.MissionScreen.UnregisterRadialMenuObject(this);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000CB70 File Offset: 0x0000AD70
		private bool IsMainAgentAvailable()
		{
			Agent main = Agent.Main;
			return main != null && main.IsActive() && Agent.Main.MountAgent == null && this._playerDismountTimer >= 2f && !Agent.Main.IsUsingGameObject && !Agent.Main.IsInWater();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000CBC4 File Offset: 0x0000ADC4
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000CBE9 File Offset: 0x0000ADE9
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000110 RID: 272
		private const int _missionTimeSpeedRequestID = 813;

		// Token: 0x04000111 RID: 273
		private readonly IMissionScreen _missionScreenAsInterface;

		// Token: 0x04000112 RID: 274
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000113 RID: 275
		private MissionMainAgentWalkModeControllerVM _dataSource;

		// Token: 0x04000114 RID: 276
		private MissionMainAgentController _mainAgentController;

		// Token: 0x04000115 RID: 277
		private bool _isSlowDownApplied;

		// Token: 0x04000116 RID: 278
		private bool _holdHandled;

		// Token: 0x04000117 RID: 279
		private bool _prevKeyDown;

		// Token: 0x04000118 RID: 280
		private float _toggleHoldTime;

		// Token: 0x04000119 RID: 281
		private float _playerDismountTimer;
	}
}
