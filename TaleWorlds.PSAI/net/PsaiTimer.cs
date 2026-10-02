using System;

namespace psai.net
{
	// Token: 0x0200001D RID: 29
	internal class PsaiTimer
	{
		// Token: 0x06000208 RID: 520 RVA: 0x000096BE File Offset: 0x000078BE
		internal PsaiTimer()
		{
			this.m_isSet = false;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000096CD File Offset: 0x000078CD
		internal void SetTimer(int delayMillis, int remainingThresholdMilliseconds)
		{
			this.m_estimatedFireTime = Logik.GetTimestampMillisElapsedSinceInitialisation() + delayMillis;
			this.m_estimatedThresholdReachedTime = this.m_estimatedFireTime - remainingThresholdMilliseconds;
			this.m_isSet = true;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000096F1 File Offset: 0x000078F1
		internal bool IsSet()
		{
			return this.m_isSet;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000096F9 File Offset: 0x000078F9
		internal void Stop()
		{
			this.m_isSet = false;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00009704 File Offset: 0x00007904
		internal void SetPaused(bool setPaused)
		{
			if (this.m_isSet)
			{
				if (setPaused)
				{
					if (!this.m_isPaused)
					{
						this.m_isPaused = true;
						this.m_timerPausedTimestamp = Logik.GetTimestampMillisElapsedSinceInitialisation();
						return;
					}
				}
				else if (this.m_isPaused)
				{
					this.m_isPaused = false;
					int num = Logik.GetTimestampMillisElapsedSinceInitialisation() - this.m_timerPausedTimestamp;
					this.m_estimatedFireTime += num;
					this.m_estimatedThresholdReachedTime += num;
				}
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000976F File Offset: 0x0000796F
		internal int GetRemainingMillisToFireTime()
		{
			if (this.m_isSet && !this.m_isPaused)
			{
				return this.m_estimatedFireTime - Logik.GetTimestampMillisElapsedSinceInitialisation();
			}
			return 999999;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00009793 File Offset: 0x00007993
		internal int GetEstimatedFireTime()
		{
			if (this.m_isSet && !this.m_isPaused)
			{
				return this.m_estimatedFireTime;
			}
			return 999999;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000097B1 File Offset: 0x000079B1
		internal bool ThresholdHasBeenReached()
		{
			return this.m_isSet && Logik.GetTimestampMillisElapsedSinceInitialisation() >= this.m_estimatedThresholdReachedTime;
		}

		// Token: 0x0400011B RID: 283
		private bool m_isSet;

		// Token: 0x0400011C RID: 284
		private bool m_isPaused;

		// Token: 0x0400011D RID: 285
		private int m_estimatedThresholdReachedTime;

		// Token: 0x0400011E RID: 286
		private int m_estimatedFireTime;

		// Token: 0x0400011F RID: 287
		private int m_timerPausedTimestamp;
	}
}
