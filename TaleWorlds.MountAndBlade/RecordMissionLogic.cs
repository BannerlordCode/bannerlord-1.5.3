using System;
using System.Diagnostics;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200029B RID: 667
	public class RecordMissionLogic : MissionLogic
	{
		// Token: 0x0600252C RID: 9516 RVA: 0x000871E2 File Offset: 0x000853E2
		public override void OnBehaviorInitialize()
		{
			base.Mission.Recorder.StartRecording();
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000871F4 File Offset: 0x000853F4
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._lastRecordedTime + 0.02f < base.Mission.CurrentTime)
			{
				this._lastRecordedTime = base.Mission.CurrentTime;
				base.Mission.Recorder.RecordCurrentState();
			}
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x00087244 File Offset: 0x00085444
		public override void OnEndMissionInternal()
		{
			base.OnEndMissionInternal();
			base.Mission.Recorder.BackupRecordToFile("Mission_record_" + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}_", DateTime.Now) + Process.GetCurrentProcess().Id, Game.Current.GameType.GetType().Name, base.Mission.SceneLevels);
			GameNetwork.ResetMissionData();
		}

		// Token: 0x04000E58 RID: 3672
		private float _lastRecordedTime = -1f;
	}
}
