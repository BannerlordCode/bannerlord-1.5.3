using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C9 RID: 713
	public class MultiplayerTimerComponent : MissionNetwork
	{
		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06002928 RID: 10536 RVA: 0x0009CA8C File Offset: 0x0009AC8C
		// (set) Token: 0x06002929 RID: 10537 RVA: 0x0009CA94 File Offset: 0x0009AC94
		public bool IsTimerRunning { get; private set; }

		// Token: 0x0600292A RID: 10538 RVA: 0x0009CA9D File Offset: 0x0009AC9D
		public void StartTimerAsServer(float duration)
		{
			this._missionTimer = new MissionTimer(duration);
			this.IsTimerRunning = true;
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x0009CAB2 File Offset: 0x0009ACB2
		public void StartTimerAsClient(float startTime, float duration)
		{
			this._missionTimer = MissionTimer.CreateSynchedTimerClient(startTime, duration);
			this.IsTimerRunning = true;
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x0009CAC8 File Offset: 0x0009ACC8
		public float GetRemainingTime(bool isSynched)
		{
			if (!this.IsTimerRunning)
			{
				return 0f;
			}
			float remainingTimeInSeconds = this._missionTimer.GetRemainingTimeInSeconds(isSynched);
			if (isSynched)
			{
				return MathF.Min(remainingTimeInSeconds, this._missionTimer.GetTimerDuration());
			}
			return remainingTimeInSeconds;
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x0009CB06 File Offset: 0x0009AD06
		public bool CheckIfTimerPassed()
		{
			return this.IsTimerRunning && this._missionTimer.Check(false);
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x0009CB1E File Offset: 0x0009AD1E
		public MissionTime GetCurrentTimerStartTime()
		{
			return this._missionTimer.GetStartTime();
		}

		// Token: 0x04000FC9 RID: 4041
		private MissionTimer _missionTimer;
	}
}
