using System;
using SandBox.Missions.MissionLogics.Hideout;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x0200001E RID: 30
	public class MissionHideoutCinematicView : MissionView
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x00009A6C File Offset: 0x00007C6C
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (!this._isInitialized)
			{
				this.InitializeView();
				return;
			}
			if (!Game.Current.GameStateManager.ActiveStateDisabledByUser && (this._currentState == HideoutCinematicController.HideoutCinematicState.Cinematic || this._nextState == HideoutCinematicController.HideoutCinematicState.Cinematic))
			{
				this.UpdateCamera(dt);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00009ABC File Offset: 0x00007CBC
		private void SetCameraFrame(Vec3 position, Vec3 direction, out MatrixFrame cameraFrame)
		{
			cameraFrame.origin = position;
			cameraFrame.rotation.s = Vec3.Side;
			cameraFrame.rotation.f = Vec3.Up;
			cameraFrame.rotation.u = -direction;
			cameraFrame.rotation.Orthonormalize();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00009B0C File Offset: 0x00007D0C
		private void SetupCamera()
		{
			this._camera = Camera.CreateCamera();
			Camera combatCamera = base.MissionScreen.CombatCamera;
			if (combatCamera != null)
			{
				this._camera.FillParametersFrom(combatCamera);
			}
			else
			{
				Debug.FailedAssert("Combat camera is null.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Missions\\MissionHideoutCinematicView.cs", "SetupCamera", 66);
			}
			Vec3 vec;
			this._cinematicLogicController.GetBossStandingEyePosition(out vec);
			Vec3 vec2;
			this._cinematicLogicController.GetPlayerStandingEyePosition(out vec2);
			Vec3 vec3 = (vec - vec2).NormalizedCopy();
			float num;
			float num2;
			float num3;
			this._cinematicLogicController.GetScenePrefabParameters(out num, out num2, out num3);
			float num4 = num + num2 + 1.5f * num3;
			this._cameraSpeed = num4 / MathF.Max(this._cinematicLogicController.CinematicDuration, 0.1f);
			this._cameraMoveDir = -vec3;
			this.SetCameraFrame(vec, vec3, out this._cameraFrame);
			Vec3 vec4 = this._cameraFrame.origin + this._cameraOffset.x * this._cameraFrame.rotation.s + this._cameraOffset.y * this._cameraFrame.rotation.f + this._cameraOffset.z * this._cameraFrame.rotation.u;
			Vec3 vec5 = (vec - vec4).NormalizedCopy();
			this.SetCameraFrame(vec4, vec5, out this._cameraFrame);
			this._camera.Frame = this._cameraFrame;
			base.MissionScreen.CustomCamera = this._camera;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00009CA4 File Offset: 0x00007EA4
		private void UpdateCamera(float dt)
		{
			Vec3 vec = this._cameraFrame.origin + this._cameraMoveDir * this._cameraSpeed * dt;
			Vec3 vec2;
			this._cinematicLogicController.GetBossStandingEyePosition(out vec2);
			Vec3 vec3 = (vec2 - vec).NormalizedCopy();
			this.SetCameraFrame(vec, vec3, out this._cameraFrame);
			this._camera.Frame = this._cameraFrame;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00009D15 File Offset: 0x00007F15
		private void ReleaseCamera()
		{
			base.MissionScreen.UpdateFreeCamera(base.MissionScreen.CustomCamera.Frame);
			base.MissionScreen.CustomCamera = null;
			this._camera.ReleaseCamera();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00009D49 File Offset: 0x00007F49
		private void OnCinematicStateChanged(HideoutCinematicController.HideoutCinematicState state)
		{
			if (this._isInitialized)
			{
				this._currentState = state;
				if (this._currentState == HideoutCinematicController.HideoutCinematicState.PreCinematic)
				{
					this.SetupCamera();
					return;
				}
				if (this._currentState == HideoutCinematicController.HideoutCinematicState.PostCinematic)
				{
					this.ReleaseCamera();
				}
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00009D79 File Offset: 0x00007F79
		private void OnCinematicTransition(HideoutCinematicController.HideoutCinematicState nextState, float duration)
		{
			if (this._isInitialized)
			{
				if (nextState == HideoutCinematicController.HideoutCinematicState.InitialFadeOut || nextState == HideoutCinematicController.HideoutCinematicState.PostCinematic)
				{
					ScreenFadeController.BeginFadeOut(duration);
				}
				else if (nextState == HideoutCinematicController.HideoutCinematicState.Cinematic || nextState == HideoutCinematicController.HideoutCinematicState.Completed)
				{
					ScreenFadeController.BeginFadeIn(duration);
				}
				this._nextState = nextState;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00009DA8 File Offset: 0x00007FA8
		private void InitializeView()
		{
			this._cinematicLogicController = base.Mission.GetMissionBehavior<HideoutCinematicController>();
			this._isInitialized = this._cinematicLogicController != null;
			if (this._cinematicLogicController != null)
			{
				this._cinematicLogicController.OnCinematicStateChanged += this.OnCinematicStateChanged;
				this._cinematicLogicController.OnCinematicTransition += this.OnCinematicTransition;
			}
		}

		// Token: 0x04000071 RID: 113
		private bool _isInitialized;

		// Token: 0x04000072 RID: 114
		private HideoutCinematicController _cinematicLogicController;

		// Token: 0x04000073 RID: 115
		private HideoutCinematicController.HideoutCinematicState _currentState;

		// Token: 0x04000074 RID: 116
		private HideoutCinematicController.HideoutCinematicState _nextState;

		// Token: 0x04000075 RID: 117
		private Camera _camera;

		// Token: 0x04000076 RID: 118
		private MatrixFrame _cameraFrame = MatrixFrame.Identity;

		// Token: 0x04000077 RID: 119
		private readonly Vec3 _cameraOffset = new Vec3(0.3f, 0.3f, 1.2f, -1f);

		// Token: 0x04000078 RID: 120
		private Vec3 _cameraMoveDir = Vec3.Forward;

		// Token: 0x04000079 RID: 121
		private float _cameraSpeed;
	}
}
