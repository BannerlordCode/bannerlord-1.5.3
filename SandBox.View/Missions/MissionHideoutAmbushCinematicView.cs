using System;
using SandBox.Missions.MissionLogics.Hideout;
using SandBox.Objects.Cinematics;
using SandBox.Objects.Usables;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x0200001D RID: 29
	public class MissionHideoutAmbushCinematicView : MissionView
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x000097A0 File Offset: 0x000079A0
		protected virtual void SetPlayerMovementEnabled(bool isPlayerMovementEnabled)
		{
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000097A4 File Offset: 0x000079A4
		public override void AfterStart()
		{
			base.AfterStart();
			this._cameraEntity = base.Mission.Scene.FindEntityWithTag("hideout_ambush_cutscene_camera");
			this._arrowPath = base.Mission.Scene.FindEntityWithTag("hideout_ambush_cutscene_arrow_path");
			this._hideoutAmbushMissionController = base.Mission.GetMissionBehavior<HideoutAmbushMissionController>();
			Vec3 invalid = Vec3.Invalid;
			this._camera = Camera.CreateCamera();
			this._cameraEntity.GetCameraParamsFromCameraScript(this._camera, ref invalid);
			this._camera.SetFovVertical(this._camera.GetFovVertical(), Screen.AspectRatio, this._camera.Near, this._camera.Far);
			this._arrowPath.SetVisibilityExcludeParents(false);
			this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.None;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00009868 File Offset: 0x00007A68
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			switch (this._currentHideoutAmbushCinematicState)
			{
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.None:
			{
				HideoutAmbushMissionController hideoutAmbushMissionController = this._hideoutAmbushMissionController;
				if (hideoutAmbushMissionController != null && hideoutAmbushMissionController.IsReadyForCallTroopsCinematic)
				{
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.FirstFadeOut;
					this.SetPlayerMovementEnabled(false);
					return;
				}
				break;
			}
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.FirstFadeOut:
				ScreenFadeController.BeginFadeOutAndIn(0.5f, 0.5f, 0.5f);
				this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.ChangeToCustomCamera;
				return;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.ChangeToCustomCamera:
				if (ScreenFadeController.IsFadedOut)
				{
					base.MissionScreen.CustomCamera = this._camera;
					Agent.Main.AgentVisuals.SetVisible(false);
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.FirstFadeIn;
					return;
				}
				break;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.FirstFadeIn:
				if (!ScreenFadeController.IsFadeActive)
				{
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SendArrow;
					return;
				}
				break;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SendArrow:
				this._arrowPath.SetVisibilityExcludeParents(true);
				this._timer = new Timer(base.Mission.CurrentTime, 5f, true);
				this._arrowPath.GetFirstScriptOfType<CinematicBurningArrow>().StartMovement();
				this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.Wait;
				return;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.Wait:
				if (this._timer.Check(base.Mission.CurrentTime))
				{
					this._timer = null;
					this._arrowPath.SetVisibilityExcludeParents(false);
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SecondFadeOut;
					return;
				}
				break;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SecondFadeOut:
				ScreenFadeController.BeginFadeOutAndIn(0.5f, 0.5f, 0.5f);
				this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.ChangeBackToDefaultCamera;
				return;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.ChangeBackToDefaultCamera:
				if (ScreenFadeController.IsFadedOut)
				{
					base.MissionScreen.CustomCamera = null;
					Agent.Main.AgentVisuals.SetVisible(true);
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SecondFadeIn;
					return;
				}
				break;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.SecondFadeIn:
				if (!ScreenFadeController.IsFadeActive)
				{
					this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.Ending;
					return;
				}
				break;
			case MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.Ending:
				this.SetPlayerMovementEnabled(true);
				this._hideoutAmbushMissionController.OnAgentsShouldBeEnabled();
				this._currentHideoutAmbushCinematicState = MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState.Ended;
				break;
			default:
				return;
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00009A20 File Offset: 0x00007C20
		public override void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			base.OnObjectUsed(userAgent, usedObject);
			if (userAgent == Agent.Main && usedObject is StealthAreaUsePoint)
			{
				MissionAgentAlarmStateView missionBehavior = base.Mission.GetMissionBehavior<MissionAgentAlarmStateView>();
				if (missionBehavior != null && missionBehavior.IsReady())
				{
					missionBehavior.SuspendView();
				}
			}
		}

		// Token: 0x04000068 RID: 104
		private const string CameraTag = "hideout_ambush_cutscene_camera";

		// Token: 0x04000069 RID: 105
		private const string ArrowBarrelTag = "hideout_ambush_cutscene_arrow_barrel";

		// Token: 0x0400006A RID: 106
		private const string ArrowPathTag = "hideout_ambush_cutscene_arrow_path";

		// Token: 0x0400006B RID: 107
		private Camera _camera;

		// Token: 0x0400006C RID: 108
		private GameEntity _cameraEntity;

		// Token: 0x0400006D RID: 109
		private GameEntity _arrowPath;

		// Token: 0x0400006E RID: 110
		private HideoutAmbushMissionController _hideoutAmbushMissionController;

		// Token: 0x0400006F RID: 111
		private MissionHideoutAmbushCinematicView.HideoutAmbushCinematicState _currentHideoutAmbushCinematicState;

		// Token: 0x04000070 RID: 112
		private Timer _timer;

		// Token: 0x02000094 RID: 148
		private enum HideoutAmbushCinematicState
		{
			// Token: 0x040002E0 RID: 736
			None,
			// Token: 0x040002E1 RID: 737
			FirstFadeOut,
			// Token: 0x040002E2 RID: 738
			ChangeToCustomCamera,
			// Token: 0x040002E3 RID: 739
			FirstFadeIn,
			// Token: 0x040002E4 RID: 740
			SendArrow,
			// Token: 0x040002E5 RID: 741
			Wait,
			// Token: 0x040002E6 RID: 742
			SecondFadeOut,
			// Token: 0x040002E7 RID: 743
			ChangeBackToDefaultCamera,
			// Token: 0x040002E8 RID: 744
			SecondFadeIn,
			// Token: 0x040002E9 RID: 745
			Ending,
			// Token: 0x040002EA RID: 746
			Ended
		}
	}
}
