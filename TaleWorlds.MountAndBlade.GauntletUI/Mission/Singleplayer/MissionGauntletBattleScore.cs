using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x02000037 RID: 55
	[OverrideView(typeof(MissionBattleScoreUIHandler))]
	public class MissionGauntletBattleScore : MissionView
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000E7C1 File Offset: 0x0000C9C1
		public ScoreboardBaseVM DataSource
		{
			get
			{
				return this._dataSource;
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000E7C9 File Offset: 0x0000C9C9
		public MissionGauntletBattleScore(ScoreboardBaseVM scoreboardVM)
		{
			this._dataSource = scoreboardVM;
			this.ViewOrderPriority = 15;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000E7E0 File Offset: 0x0000C9E0
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			base.Mission.IsFriendlyMission = false;
			this._dataSource.Initialize(base.MissionScreen, base.Mission, null, new Action<bool>(this.ToggleScoreboard));
			this.CreateView();
			this._dataSource.SetShortcuts(new ScoreboardHotkeys
			{
				ShowMouseHotkey = HotKeyManager.GetCategory("ScoreboardHotKeyCategory").GetGameKey(35),
				ShowScoreboardHotkey = HotKeyManager.GetCategory("Generic").GetGameKey(4),
				DoneInputKey = HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"),
				FastForwardKey = HotKeyManager.GetCategory("ScoreboardHotKeyCategory").GetHotKey("ToggleFastForward")
			});
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000E8A4 File Offset: 0x0000CAA4
		private void CreateView()
		{
			this._gauntletLayer = new GauntletLayer("Scoreboard", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("SPScoreboard", this._dataSource);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("Generic"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("ScoreboardHotKeyCategory"));
			GameKeyContext category = HotKeyManager.GetCategory("ScoreboardHotKeyCategory");
			if (!base.MissionScreen.SceneLayer.Input.IsCategoryRegistered(category))
			{
				base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(category);
			}
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000E978 File Offset: 0x0000CB78
		public override void OnMissionScreenFinalize()
		{
			base.Mission.OnMainAgentChanged -= this.Mission_OnMainAgentChanged;
			base.MissionScreen.GetSpectatedCharacter = null;
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000E9D8 File Offset: 0x0000CBD8
		public override bool OnEscape()
		{
			if (this._dataSource.ShowScoreboard)
			{
				this.OnClose();
				return true;
			}
			return base.OnEscape();
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000E9F5 File Offset: 0x0000CBF5
		public override void EarlyStart()
		{
			base.EarlyStart();
			base.Mission.OnMainAgentChanged += this.Mission_OnMainAgentChanged;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000EA14 File Offset: 0x0000CC14
		private void Mission_OnMainAgentChanged(Agent oldAgent)
		{
			if (base.Mission.MainAgent == null)
			{
				this._dataSource.OnMainHeroDeath();
				return;
			}
			if (base.Mission.MainAgent.Character != Game.Current.PlayerTroop)
			{
				this._dataSource.OnTakenControlOfAnotherAgent();
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000EA64 File Offset: 0x0000CC64
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this._dataSource.Tick(dt);
			if (MissionGauntletBattleScore._forceScoreboardToggle || this._dataSource.IsOver || this._dataSource.IsMainCharacterDead || TaleWorlds.InputSystem.Input.IsGamepadActive)
			{
				bool flag = this.CanOpenScoreboard() && (base.Mission.InputManager.IsGameKeyPressed(4) || this._gauntletLayer.Input.IsGameKeyPressed(4));
				if (flag && !this._dataSource.ShowScoreboard)
				{
					IBattleEndLogic battleEndLogic = base.Mission.MissionBehaviors.FirstOrDefault<MissionBehavior>((MissionBehavior behavior) => behavior is IBattleEndLogic) as IBattleEndLogic;
					if (battleEndLogic != null)
					{
						battleEndLogic.SetNotificationDisabled(true);
					}
					this._toOpen = true;
				}
				if (flag && this._dataSource.ShowScoreboard)
				{
					IBattleEndLogic battleEndLogic2 = base.Mission.MissionBehaviors.FirstOrDefault<MissionBehavior>((MissionBehavior behavior) => behavior is IBattleEndLogic) as IBattleEndLogic;
					if (battleEndLogic2 != null)
					{
						battleEndLogic2.SetNotificationDisabled(false);
					}
					this.OnClose();
				}
			}
			else
			{
				bool flag2 = this.CanOpenScoreboard() && (base.Mission.InputManager.IsHotKeyDown("HoldShow") || this._gauntletLayer.Input.IsHotKeyDown("HoldShow"));
				if (flag2 && !this._dataSource.ShowScoreboard)
				{
					IBattleEndLogic battleEndLogic3 = base.Mission.MissionBehaviors.FirstOrDefault<MissionBehavior>((MissionBehavior behavior) => behavior is IBattleEndLogic) as IBattleEndLogic;
					if (battleEndLogic3 != null)
					{
						battleEndLogic3.SetNotificationDisabled(true);
					}
					this._toOpen = true;
				}
				if (!flag2 && this._dataSource.ShowScoreboard)
				{
					IBattleEndLogic battleEndLogic4 = base.Mission.MissionBehaviors.FirstOrDefault<MissionBehavior>((MissionBehavior behavior) => behavior is IBattleEndLogic) as IBattleEndLogic;
					if (battleEndLogic4 != null)
					{
						battleEndLogic4.SetNotificationDisabled(false);
					}
					this.OnClose();
				}
			}
			if (this._toOpen)
			{
				this.OnOpen();
			}
			if (this._dataSource.IsMainCharacterDead && !this._dataSource.IsOver && (base.Mission.InputManager.IsHotKeyReleased("ToggleFastForward") || this._gauntletLayer.Input.IsHotKeyReleased("ToggleFastForward")))
			{
				this._dataSource.IsFastForwarding = !this._dataSource.IsFastForwarding;
				this._dataSource.ExecuteFastForwardAction();
			}
			if (this._dataSource.IsOver && this._dataSource.ShowScoreboard && (base.Mission.InputManager.IsHotKeyPressed("Confirm") || this._gauntletLayer.Input.IsHotKeyPressed("Confirm")))
			{
				this.ExecuteQuitAction();
			}
			if (this._dataSource.ShowScoreboard && !base.DebugInput.IsControlDown() && base.DebugInput.IsHotKeyPressed("ShowHighlightsSummary"))
			{
				HighlightsController missionBehavior = base.Mission.GetMissionBehavior<HighlightsController>();
				if (missionBehavior != null)
				{
					missionBehavior.ShowSummary();
				}
			}
			bool flag3 = base.Mission.InputManager.IsGameKeyPressed(35) || this._gauntletLayer.Input.IsGameKeyPressed(35);
			if (this._dataSource.ShowScoreboard && !this._isMouseEnabled && flag3)
			{
				this.SetMouseState(true);
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000EDD6 File Offset: 0x0000CFD6
		private void ExecuteQuitAction()
		{
			this._dataSource.ExecuteQuitAction();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000EDE3 File Offset: 0x0000CFE3
		private bool CanOpenScoreboard()
		{
			return !base.MissionScreen.IsRadialMenuActive && !base.MissionScreen.IsPhotoModeEnabled && !base.Mission.IsOrderMenuOpen;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000EE0F File Offset: 0x0000D00F
		private void ToggleScoreboard(bool value)
		{
			if (value)
			{
				this._toOpen = true;
				return;
			}
			this.OnClose();
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000EE24 File Offset: 0x0000D024
		private void OnOpen()
		{
			this._toOpen = false;
			if (this._dataSource.ShowScoreboard || base.Mission.Mode == MissionMode.Deployment)
			{
				return;
			}
			base.MissionScreen.SetDisplayDialog(true);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
			this._dataSource.ShowScoreboard = true;
			base.MissionScreen.SetCameraLockState(true);
			if (this._dataSource.IsOver || this._dataSource.IsMainCharacterDead || ScreenManager.GetMouseVisibility())
			{
				this.SetMouseState(true);
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000EEB4 File Offset: 0x0000D0B4
		private void OnClose()
		{
			if (!this._dataSource.ShowScoreboard)
			{
				return;
			}
			base.MissionScreen.SetDisplayDialog(false);
			this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
			this._dataSource.ShowScoreboard = false;
			base.MissionScreen.SetCameraLockState(false);
			this.SetMouseState(false);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000EF0C File Offset: 0x0000D10C
		private void SetMouseState(bool isEnabled)
		{
			this._gauntletLayer.IsFocusLayer = isEnabled;
			if (isEnabled)
			{
				this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				ScreenManager.TrySetFocus(this._gauntletLayer);
			}
			else
			{
				ScreenManager.TryLoseFocus(this._gauntletLayer);
			}
			ScoreboardBaseVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.SetMouseState(isEnabled);
			}
			this._isMouseEnabled = isEnabled;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000EF6B File Offset: 0x0000D16B
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			ScoreboardBaseVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnDeploymentFinished();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000EF83 File Offset: 0x0000D183
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000EFD0 File Offset: 0x0000D1D0
		[CommandLineFunctionality.CommandLineArgumentFunction("force_toggle", "scoreboard")]
		public static string ForceScoreboardToggle(List<string> args)
		{
			int num;
			if (args.Count == 1 && int.TryParse(args[0], out num) && (num == 0 || num == 1))
			{
				MissionGauntletBattleScore._forceScoreboardToggle = num == 1;
				return "Force Scoreboard Toggle is: " + (MissionGauntletBattleScore._forceScoreboardToggle ? "ON" : "OFF");
			}
			return "Format is: scoreboard.force_toggle 0-1";
		}

		// Token: 0x04000147 RID: 327
		private ScoreboardBaseVM _dataSource;

		// Token: 0x04000148 RID: 328
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000149 RID: 329
		private bool _toOpen;

		// Token: 0x0400014A RID: 330
		private bool _isMouseEnabled;

		// Token: 0x0400014B RID: 331
		private static bool _forceScoreboardToggle;
	}
}
