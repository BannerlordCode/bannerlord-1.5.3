using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001EB RID: 491
	public class BasicMissionTimer
	{
		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001CDF RID: 7391 RVA: 0x000624E3 File Offset: 0x000606E3
		public float ElapsedTime
		{
			get
			{
				return Mission.Current.CurrentTime - this._startTime;
			}
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000624F6 File Offset: 0x000606F6
		public BasicMissionTimer()
		{
			this._startTime = Mission.Current.CurrentTime;
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x0006250E File Offset: 0x0006070E
		public void Reset()
		{
			this._startTime = Mission.Current.CurrentTime;
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x00062520 File Offset: 0x00060720
		public void Set(float newElapsedTime)
		{
			this._startTime = Mission.Current.CurrentTime - newElapsedTime;
		}

		// Token: 0x040009AB RID: 2475
		private float _startTime;
	}
}
