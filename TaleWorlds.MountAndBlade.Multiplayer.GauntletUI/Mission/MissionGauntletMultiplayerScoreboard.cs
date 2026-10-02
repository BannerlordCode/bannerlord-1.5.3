using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000018 RID: 24
	[OverrideView(typeof(MissionScoreboardUIHandler))]
	public class MissionGauntletMultiplayerScoreboard : MissionView
	{
		// Token: 0x06000113 RID: 275 RVA: 0x000071F7 File Offset: 0x000053F7
		[UsedImplicitly]
		public MissionGauntletMultiplayerScoreboard(bool isSingleTeam)
		{
			this._isSingleTeam = isSingleTeam;
			this.ViewOrderPriority = 25;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00007210 File Offset: 0x00005410
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.InitializeLayer();
			base.Mission.IsFriendlyMission = false;
			GameKeyContext category = HotKeyManager.GetCategory("ScoreboardHotKeyCategory");
			if (!base.MissionScreen.SceneLayer.Input.IsCategoryRegistered(category))
			{
				base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(category);
			}
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._scoreboardStayDuration = MissionLobbyComponent.PostMatchWaitDuration / 2f;
			this._teamSelectComponent = base.Mission.GetMissionBehavior<MultiplayerTeamSelectComponent>();
			this.RegisterEvents();
			if (this._dataSource != null)
			{
				this._dataSource.IsActive = false;
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000072BB File Offset: 0x000054BB
		public override void OnRemoveBehavior()
		{
			this.UnregisterEvents();
			this.FinalizeLayer();
			base.OnRemoveBehavior();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000072CF File Offset: 0x000054CF
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this.UnregisterEvents();
			this.FinalizeLayer();
			base.OnMissionScreenFinalize();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000072EC File Offset: 0x000054EC
		private void RegisterEvents()
		{
			this._missionLobbyComponent.CurrentMultiplayerStateChanged += this.MissionLobbyComponentOnCurrentMultiplayerStateChanged;
			this._missionLobbyComponent.OnCultureSelectionRequested += this.OnCultureSelectionRequested;
			if (this._teamSelectComponent != null)
			{
				this._teamSelectComponent.OnSelectingTeam += this.OnSelectingTeam;
			}
			MissionPeer.OnTeamChanged += this.OnTeamChanged;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007358 File Offset: 0x00005558
		private void UnregisterEvents()
		{
			this._missionLobbyComponent.CurrentMultiplayerStateChanged -= this.MissionLobbyComponentOnCurrentMultiplayerStateChanged;
			this._missionLobbyComponent.OnCultureSelectionRequested -= this.OnCultureSelectionRequested;
			if (this._teamSelectComponent != null)
			{
				this._teamSelectComponent.OnSelectingTeam -= this.OnSelectingTeam;
			}
			MissionPeer.OnTeamChanged -= this.OnTeamChanged;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000073C4 File Offset: 0x000055C4
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._isMissionEnding)
			{
				if (this._scoreboardStayTimeElapsed >= this._scoreboardStayDuration)
				{
					this.ToggleScoreboard(false);
					return;
				}
				this._scoreboardStayTimeElapsed += dt;
			}
			this._dataSource.Tick(dt);
			if (TaleWorlds.InputSystem.Input.IsGamepadActive)
			{
				bool flag = base.MissionScreen.SceneLayer.Input.IsGameKeyPressed(4) || this._gauntletLayer.Input.IsGameKeyPressed(4);
				if (this._isMissionEnding)
				{
					this.ToggleScoreboard(true);
				}
				else if (flag && !base.MissionScreen.IsRadialMenuActive && !base.Mission.IsOrderMenuOpen)
				{
					this.ToggleScoreboard(!this._dataSource.IsActive);
				}
			}
			else
			{
				bool flag2 = base.MissionScreen.SceneLayer.Input.IsHotKeyDown("HoldShow") || this._gauntletLayer.Input.IsHotKeyDown("HoldShow");
				bool flag3 = this._isMissionEnding || (flag2 && !base.MissionScreen.IsRadialMenuActive && !base.Mission.IsOrderMenuOpen);
				this.ToggleScoreboard(flag3);
			}
			if (this._isActive && (base.MissionScreen.SceneLayer.Input.IsGameKeyPressed(35) || this._gauntletLayer.Input.IsGameKeyPressed(35)))
			{
				this._mouseRequstedWhileScoreboardActive = true;
			}
			bool flag4 = this._isMissionEnding || (this._isActive && this._mouseRequstedWhileScoreboardActive);
			this.SetMouseState(flag4);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000755C File Offset: 0x0000575C
		private void ToggleScoreboard(bool isActive)
		{
			if (this._isActive != isActive)
			{
				this._isActive = isActive;
				this._dataSource.IsActive = this._isActive;
				base.MissionScreen.SetCameraLockState(this._isActive);
				if (!this._isActive)
				{
					this._mouseRequstedWhileScoreboardActive = false;
				}
				Action<bool> onScoreboardToggled = this.OnScoreboardToggled;
				if (onScoreboardToggled == null)
				{
					return;
				}
				onScoreboardToggled(this._isActive);
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000075C0 File Offset: 0x000057C0
		private void SetMouseState(bool isMouseVisible)
		{
			if (this._isMouseVisible != isMouseVisible)
			{
				this._isMouseVisible = isMouseVisible;
				if (!this._isMouseVisible)
				{
					this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				}
				else
				{
					this._gauntletLayer.InputRestrictions.SetInputRestrictions(this._isMouseVisible, InputUsageMask.Mouse);
				}
				MissionScoreboardVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.SetMouseState(isMouseVisible);
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000761F File Offset: 0x0000581F
		private void MissionLobbyComponentOnCurrentMultiplayerStateChanged(MissionLobbyComponent.MultiplayerGameState newState)
		{
			this._isMissionEnding = newState == MissionLobbyComponent.MultiplayerGameState.Ending;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000762B File Offset: 0x0000582B
		private void OnTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (peer.IsMine)
			{
				this.FinalizeLayer();
				this.InitializeLayer();
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00007644 File Offset: 0x00005844
		private void FinalizeLayer()
		{
			if (this._dataSource != null)
			{
				this._dataSource.OnFinalize();
			}
			if (this._gauntletLayer != null)
			{
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
			}
			this._gauntletLayer = null;
			this._dataSource = null;
			this._isActive = false;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00007694 File Offset: 0x00005894
		private void InitializeLayer()
		{
			this._dataSource = new MissionScoreboardVM(this._isSingleTeam, base.Mission);
			this._gauntletLayer = new GauntletLayer("MultiplayerScoreboard", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerScoreboard", this._dataSource);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("Generic"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("ScoreboardHotKeyCategory"));
			base.MissionScreen.AddLayer(this._gauntletLayer);
			this._dataSource.IsActive = this._isActive;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000773C File Offset: 0x0000593C
		private void OnSelectingTeam(List<Team> disableTeams)
		{
			this.ToggleScoreboard(false);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00007745 File Offset: 0x00005945
		private void OnCultureSelectionRequested()
		{
			this.ToggleScoreboard(false);
		}

		// Token: 0x04000075 RID: 117
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000076 RID: 118
		private MissionScoreboardVM _dataSource;

		// Token: 0x04000077 RID: 119
		private bool _isSingleTeam;

		// Token: 0x04000078 RID: 120
		private bool _isActive;

		// Token: 0x04000079 RID: 121
		private bool _isMissionEnding;

		// Token: 0x0400007A RID: 122
		private bool _mouseRequstedWhileScoreboardActive;

		// Token: 0x0400007B RID: 123
		private bool _isMouseVisible;

		// Token: 0x0400007C RID: 124
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x0400007D RID: 125
		private MultiplayerTeamSelectComponent _teamSelectComponent;

		// Token: 0x0400007E RID: 126
		public Action<bool> OnScoreboardToggled;

		// Token: 0x0400007F RID: 127
		private float _scoreboardStayDuration;

		// Token: 0x04000080 RID: 128
		private float _scoreboardStayTimeElapsed;
	}
}
