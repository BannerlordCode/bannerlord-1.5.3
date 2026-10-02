using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x02000040 RID: 64
	[OverrideView(typeof(MissionSpectatorControlView))]
	public class MissionGauntletSpectatorControl : MissionView
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x000119E4 File Offset: 0x0000FBE4
		public override void EarlyStart()
		{
			base.EarlyStart();
			this.ViewOrderPriority = 14;
			this._dataSource = new MissionSpectatorControlVM(base.Mission);
			this._dataSource.SetPrevCharacterInputKey(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(10));
			this._dataSource.SetNextCharacterInputKey(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(9));
			this._dataSource.SetTakeControlInputKey(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(16));
			this._gauntletLayer = new GauntletLayer("MissionSpectatorControl", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("SpectatorControl", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.MissionScreen.OnSpectateAgentFocusIn += this._dataSource.OnSpectatedAgentFocusIn;
			base.MissionScreen.OnSpectateAgentFocusOut += this._dataSource.OnSpectatedAgentFocusOut;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00011ADC File Offset: 0x0000FCDC
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._dataSource != null)
			{
				Mission.SpectatorData spectatingData = base.MissionScreen.GetSpectatingData(base.MissionScreen.CombatCamera.Frame.origin);
				bool flag = spectatingData.CameraType == SpectatorCameraTypes.LockToMainPlayer || spectatingData.CameraType == SpectatorCameraTypes.LockToPosition;
				MissionSpectatorControlVM dataSource = this._dataSource;
				bool flag2;
				if ((!flag && base.Mission.Mode != MissionMode.Deployment) || (base.MissionScreen.IsCheatGhostMode && !base.Mission.IsOrderMenuOpen))
				{
					MissionMultiplayerGameModeBaseClient missionBehavior = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
					if ((missionBehavior == null || missionBehavior.IsRoundInProgress) && !base.MissionScreen.LockCameraMovement)
					{
						flag2 = base.MissionScreen.CustomCamera == null;
						goto IL_00B6;
					}
				}
				flag2 = false;
				IL_00B6:
				dataSource.IsEnabled = flag2;
				bool flag3 = base.Mission.PlayerTeam != null && base.Mission.MainAgent == null;
				this._dataSource.SetMainAgentStatus(flag3);
				this._dataSource.IsTakeControlRelevant = flag3 && base.Mission.CanPlayerTakeControlOfAnotherAgentWhenDead;
				this._dataSource.IsTakeControlEnabled = base.MissionScreen.LastFollowedAgent != null && base.Mission.CanTakeControlOfAgent(base.MissionScreen.LastFollowedAgent);
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00011C1C File Offset: 0x0000FE1C
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.OnSpectateAgentFocusIn -= this._dataSource.OnSpectatedAgentFocusIn;
			base.MissionScreen.OnSpectateAgentFocusOut -= this._dataSource.OnSpectatedAgentFocusOut;
			this._dataSource.OnFinalize();
		}

		// Token: 0x04000181 RID: 385
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000182 RID: 386
		private MissionSpectatorControlVM _dataSource;
	}
}
