using System;

namespace psai.net
{
	// Token: 0x0200000C RID: 12
	public class AudioData
	{
		// Token: 0x0600012A RID: 298 RVA: 0x00006528 File Offset: 0x00004728
		public int GetFullLengthInMilliseconds()
		{
			return (int)((long)this.sampleCountTotal * 1000L / (long)this.sampleRateHz);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006541 File Offset: 0x00004741
		public int GetPreBeatZoneInMilliseconds()
		{
			return (int)((long)this.sampleCountPreBeat * 1000L / (long)this.sampleRateHz);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000655A File Offset: 0x0000475A
		public int GetPostBeatZoneInMilliseconds()
		{
			return (int)((long)this.sampleCountPostBeat * 1000L / (long)this.sampleRateHz);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006573 File Offset: 0x00004773
		public int GetSampleCountByMilliseconds(int milliSeconds)
		{
			return (int)((long)this.sampleRateHz * (long)milliSeconds / 1000L);
		}

		// Token: 0x04000076 RID: 118
		public string filePathRelativeToProjectDir;

		// Token: 0x04000077 RID: 119
		public string moduleId;

		// Token: 0x04000078 RID: 120
		public int sampleCountTotal;

		// Token: 0x04000079 RID: 121
		public int sampleCountPreBeat;

		// Token: 0x0400007A RID: 122
		public int sampleCountPostBeat;

		// Token: 0x0400007B RID: 123
		public int sampleRateHz;

		// Token: 0x0400007C RID: 124
		public float bpm;
	}
}
