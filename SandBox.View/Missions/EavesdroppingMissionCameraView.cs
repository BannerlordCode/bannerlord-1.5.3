using System;
using SandBox.Missions;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x02000012 RID: 18
	public class EavesdroppingMissionCameraView : MissionView
	{
		// Token: 0x0600007A RID: 122 RVA: 0x00004948 File Offset: 0x00002B48
		protected virtual void SetPlayerMovementEnabled(bool isPlayerMovementEnabled)
		{
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000494A File Offset: 0x00002B4A
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._cameraSwitchState = EavesdroppingMissionCameraView.CameraSwitchState.None;
			this._eavesdroppingMissionLogic = base.Mission.GetMissionBehavior<EavesdroppingMissionLogic>();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000496C File Offset: 0x00002B6C
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._eavesdroppingMissionLogic != null)
			{
				switch (this._cameraSwitchState)
				{
				case EavesdroppingMissionCameraView.CameraSwitchState.None:
					if ((this._eavesdroppingMissionLogic.EavesdropStarted && base.MissionScreen.CustomCamera == null) || (!this._eavesdroppingMissionLogic.EavesdropStarted && base.MissionScreen.CustomCamera != null))
					{
						if (this._eavesdroppingMissionLogic.EavesdropStarted && base.MissionScreen.CustomCamera == null)
						{
							this.SetPlayerMovementEnabled(false);
						}
						this._cameraSwitchState = EavesdroppingMissionCameraView.CameraSwitchState.ReadyForFadeOut;
						return;
					}
					break;
				case EavesdroppingMissionCameraView.CameraSwitchState.ReadyForFadeOut:
					ScreenFadeController.BeginFadeOutAndIn(0.5f, 0.5f, 0.5f);
					this._cameraSwitchState = EavesdroppingMissionCameraView.CameraSwitchState.FadeOutAndInStarted;
					return;
				case EavesdroppingMissionCameraView.CameraSwitchState.FadeOutAndInStarted:
					if (ScreenFadeController.IsFadedOut)
					{
						base.MissionScreen.CustomCamera = ((base.MissionScreen.CustomCamera == null) ? this._eavesdroppingMissionLogic.CurrentEavesdroppingCamera : null);
						if (base.MissionScreen.CustomCamera == null)
						{
							this.SetPlayerMovementEnabled(true);
						}
						this._cameraSwitchState = EavesdroppingMissionCameraView.CameraSwitchState.WaitingForFadeInToEnd;
						return;
					}
					break;
				case EavesdroppingMissionCameraView.CameraSwitchState.WaitingForFadeInToEnd:
					if (!ScreenFadeController.IsFadeActive)
					{
						this._cameraSwitchState = EavesdroppingMissionCameraView.CameraSwitchState.None;
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x04000018 RID: 24
		private EavesdroppingMissionCameraView.CameraSwitchState _cameraSwitchState;

		// Token: 0x04000019 RID: 25
		private EavesdroppingMissionLogic _eavesdroppingMissionLogic;

		// Token: 0x0200008C RID: 140
		private enum CameraSwitchState
		{
			// Token: 0x040002BC RID: 700
			None,
			// Token: 0x040002BD RID: 701
			ReadyForFadeOut,
			// Token: 0x040002BE RID: 702
			FadeOutAndInStarted,
			// Token: 0x040002BF RID: 703
			WaitingForFadeInToEnd
		}
	}
}
