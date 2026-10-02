using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000083 RID: 131
	public class ReplayMissionView : MissionView
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x00025B13 File Offset: 0x00023D13
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._resetTime = 0f;
			this._replayMissionLogic = base.Mission.GetMissionBehavior<ReplayMissionLogic>();
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00025B38 File Offset: 0x00023D38
		public override void OnPreMissionTick(float dt)
		{
			base.OnPreMissionTick(dt);
			base.Mission.Recorder.ProcessRecordUntilTime(base.Mission.CurrentTime - this._resetTime);
			if (base.Mission.CurrentState == Mission.State.Continuing && base.Mission.Recorder.IsEndOfRecord())
			{
				if (MBEditor._isEditorMissionOn)
				{
					MBEditor.LeaveEditMissionMode();
					return;
				}
				base.Mission.EndMission();
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00025BA6 File Offset: 0x00023DA6
		public void OverrideInput(bool isOverridden)
		{
			this._isInputOverridden = isOverridden;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00025BB0 File Offset: 0x00023DB0
		public void ResetReplay()
		{
			this._resetTime = base.Mission.CurrentTime;
			base.Mission.ResetMission();
			base.Mission.Teams.Clear();
			base.Mission.Recorder.RestartRecord();
			MBCommon.UnPauseGameEngine();
			base.Mission.Scene.TimeSpeed = 1f;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00025C14 File Offset: 0x00023E14
		public void Rewind(float time)
		{
			this._resetTime = MathF.Min(this._resetTime + time, base.Mission.CurrentTime);
			base.Mission.ResetMission();
			base.Mission.Teams.Clear();
			base.Mission.Recorder.RestartRecord();
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00025C6A File Offset: 0x00023E6A
		public void FastForward(float time)
		{
			this._resetTime -= time;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00025C7A File Offset: 0x00023E7A
		public void Pause()
		{
			if (!MBCommon.IsPaused && base.Mission.Scene.TimeSpeed.ApproximatelyEqualsTo(1f, 1E-05f))
			{
				MBCommon.PauseGameEngine();
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00025CAC File Offset: 0x00023EAC
		public void Resume()
		{
			if (MBCommon.IsPaused || !base.Mission.Scene.TimeSpeed.ApproximatelyEqualsTo(1f, 1E-05f))
			{
				MBCommon.UnPauseGameEngine();
				base.Mission.Scene.TimeSpeed = 1f;
			}
		}

		// Token: 0x040002DE RID: 734
		private float _resetTime;

		// Token: 0x040002DF RID: 735
		private bool _isInputOverridden;

		// Token: 0x040002E0 RID: 736
		private ReplayMissionLogic _replayMissionLogic;
	}
}
