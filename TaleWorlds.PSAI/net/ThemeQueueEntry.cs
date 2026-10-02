using System;

namespace psai.net
{
	// Token: 0x02000026 RID: 38
	internal class ThemeQueueEntry : ICloneable
	{
		// Token: 0x06000243 RID: 579 RVA: 0x0000A668 File Offset: 0x00008868
		internal ThemeQueueEntry()
		{
			this.playmode = PsaiPlayMode.regular;
			this.themeId = -1;
			this.startIntensity = 1f;
			this.restTimeMillis = 0;
			this.holdIntensity = false;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000A697 File Offset: 0x00008897
		public object Clone()
		{
			return (ThemeQueueEntry)base.MemberwiseClone();
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000A6A4 File Offset: 0x000088A4
		public override string ToString()
		{
			return string.Format("playmode:{0}  themeId:{1}  startIntensity:{2}  restTimeMillis:{3}  holdIntensity:{4}  musicDuration:{5}", new object[] { this.playmode, this.themeId, this.startIntensity, this.restTimeMillis, this.holdIntensity, this.musicDuration });
		}

		// Token: 0x04000153 RID: 339
		internal PsaiPlayMode playmode;

		// Token: 0x04000154 RID: 340
		internal int themeId;

		// Token: 0x04000155 RID: 341
		internal float startIntensity;

		// Token: 0x04000156 RID: 342
		internal int restTimeMillis;

		// Token: 0x04000157 RID: 343
		internal bool holdIntensity;

		// Token: 0x04000158 RID: 344
		internal int musicDuration;
	}
}
