using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001EC RID: 492
	public class BasicTimer
	{
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x00062534 File Offset: 0x00060734
		public float ElapsedTime
		{
			get
			{
				return MBCommon.GetApplicationTime() - this._startTime;
			}
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x00062542 File Offset: 0x00060742
		public BasicTimer()
		{
			this._startTime = MBCommon.GetApplicationTime();
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x00062555 File Offset: 0x00060755
		public void Reset()
		{
			this._startTime = MBCommon.GetApplicationTime();
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x00062562 File Offset: 0x00060762
		public void Set(float newElapsedTime)
		{
			this._startTime = MBCommon.GetApplicationTime() - newElapsedTime;
		}

		// Token: 0x040009AC RID: 2476
		private float _startTime;
	}
}
