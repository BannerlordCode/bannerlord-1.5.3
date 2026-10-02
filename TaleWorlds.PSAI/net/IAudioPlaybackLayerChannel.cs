using System;

namespace psai.net
{
	// Token: 0x0200000E RID: 14
	internal interface IAudioPlaybackLayerChannel
	{
		// Token: 0x06000138 RID: 312
		PsaiResult LoadSegment(Segment segment);

		// Token: 0x06000139 RID: 313
		PsaiResult ReleaseSegment();

		// Token: 0x0600013A RID: 314
		PsaiResult ScheduleSegmentPlayback(Segment segment, int delayMilliseconds);

		// Token: 0x0600013B RID: 315
		PsaiResult StopChannel();

		// Token: 0x0600013C RID: 316
		PsaiResult SetVolume(float volume);

		// Token: 0x0600013D RID: 317
		PsaiResult SetPaused(bool paused);

		// Token: 0x0600013E RID: 318
		void Release();
	}
}
