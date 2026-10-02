using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A8 RID: 680
	public class MissionTimer
	{
		// Token: 0x060025CA RID: 9674 RVA: 0x0008978D File Offset: 0x0008798D
		private MissionTimer()
		{
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x00089795 File Offset: 0x00087995
		public MissionTimer(float duration)
		{
			this._startTime = MissionTime.Now;
			this._duration = duration;
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000897AF File Offset: 0x000879AF
		public MissionTime GetStartTime()
		{
			return this._startTime;
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000897B7 File Offset: 0x000879B7
		public float GetTimerDuration()
		{
			return this._duration;
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000897C0 File Offset: 0x000879C0
		public float GetRemainingTimeInSeconds(bool synched = false)
		{
			if (this._duration < 0f)
			{
				return 0f;
			}
			float num = this._duration - this._startTime.ElapsedSeconds;
			if (synched && GameNetwork.IsClientOrReplay)
			{
				num -= Mission.Current.MissionTimeTracker.GetLastSyncDifference();
			}
			if (num <= 0f)
			{
				return 0f;
			}
			return num;
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x0008981E File Offset: 0x00087A1E
		public bool Check(bool reset = false)
		{
			bool flag = this.GetRemainingTimeInSeconds(false) <= 0f;
			if (flag && reset)
			{
				this._startTime = MissionTime.Now;
			}
			return flag;
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x00089841 File Offset: 0x00087A41
		public void Reset()
		{
			this._startTime = MissionTime.Now;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x0008984E File Offset: 0x00087A4E
		public void Set(float timeInSeconds)
		{
			this._startTime = new MissionTime(Mission.Current.MissionTimeTracker.NumberOfTicks + (long)(timeInSeconds * 10000000f));
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00089873 File Offset: 0x00087A73
		public void SetDuration(float duration)
		{
			this._duration = duration;
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x0008987C File Offset: 0x00087A7C
		public static MissionTimer CreateSynchedTimerClient(float startTimeInSeconds, float duration)
		{
			return new MissionTimer
			{
				_startTime = new MissionTime((long)(startTimeInSeconds * 10000000f)),
				_duration = duration
			};
		}

		// Token: 0x04000E96 RID: 3734
		private MissionTime _startTime;

		// Token: 0x04000E97 RID: 3735
		private float _duration;
	}
}
