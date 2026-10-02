using System;

namespace psai.net
{
	// Token: 0x02000017 RID: 23
	public class PlaybackChannel
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00009021 File Offset: 0x00007221
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00009029 File Offset: 0x00007229
		internal Segment Segment { get; private set; }

		// Token: 0x060001AD RID: 429 RVA: 0x00009032 File Offset: 0x00007232
		internal PlaybackChannel(Logik logik, bool isMainChannel)
		{
			this.m_audioPlaybackLayerChannel = new AudioPlaybackLayerChannelStandalone();
			this.m_logik = logik;
			this.m_isMainChannel = isMainChannel;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00009069 File Offset: 0x00007269
		internal void Release()
		{
			this.m_audioPlaybackLayerChannel.Release();
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00009076 File Offset: 0x00007276
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000907E File Offset: 0x0000727E
		internal bool Paused
		{
			get
			{
				return this.m_paused;
			}
			set
			{
				this.m_paused = value;
				this.m_audioPlaybackLayerChannel.SetPaused(value);
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009094 File Offset: 0x00007294
		internal ChannelState GetCurrentChannelState()
		{
			if (this.Segment == null || this.m_stoppedExplicitly)
			{
				return ChannelState.stopped;
			}
			float num = (float)this.GetCountdownToPlaybackInMilliseconds();
			if (this.m_playbackIsScheduled && num > 0f)
			{
				return ChannelState.load;
			}
			if (num * -1f > (float)this.Segment.audioData.GetFullLengthInMilliseconds())
			{
				return ChannelState.stopped;
			}
			return ChannelState.playing;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000090EA File Offset: 0x000072EA
		internal bool IsPlaying()
		{
			return this.GetCurrentChannelState() == ChannelState.playing;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000090F5 File Offset: 0x000072F5
		internal void LoadSegment(Segment snippet)
		{
			this.Segment = snippet;
			this.m_timeStampOfSnippetLoad = Logik.GetTimestampMillisElapsedSinceInitialisation();
			this.m_playbackIsScheduled = false;
			this.m_stoppedExplicitly = false;
			if (this.m_audioPlaybackLayerChannel != null)
			{
				this.m_audioPlaybackLayerChannel.LoadSegment(snippet);
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000912C File Offset: 0x0000732C
		internal bool CheckIfSegmentHadEnoughTimeToLoad()
		{
			return this.GetMillisecondsSinceSegmentLoad() >= Logik.s_audioLayerMaximumLatencyForBufferingSounds;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000913E File Offset: 0x0000733E
		internal int GetMillisecondsSinceSegmentLoad()
		{
			return Logik.GetTimestampMillisElapsedSinceInitialisation() - this.m_timeStampOfSnippetLoad;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000914C File Offset: 0x0000734C
		internal int GetMillisecondsUntilLoadingWillHaveFinished()
		{
			return Math.Max(0, Logik.s_audioLayerMaximumLatencyForBufferingSounds - this.GetMillisecondsSinceSegmentLoad());
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00009160 File Offset: 0x00007360
		internal void StopChannel()
		{
			this.m_stoppedExplicitly = true;
			if (this.m_audioPlaybackLayerChannel != null)
			{
				this.m_audioPlaybackLayerChannel.StopChannel();
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000917D File Offset: 0x0000737D
		internal void ReleaseSegment()
		{
			this.Segment = null;
			if (this.m_audioPlaybackLayerChannel != null)
			{
				this.m_audioPlaybackLayerChannel.ReleaseSegment();
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000919A File Offset: 0x0000739A
		internal int GetCountdownToPlaybackInMilliseconds()
		{
			return this.m_timeStampOfPlaybackStart - Logik.GetTimestampMillisElapsedSinceInitialisation();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000091A8 File Offset: 0x000073A8
		internal void ScheduleSegmentPlayback(Segment snippet, int delayInMilliseconds)
		{
			if (delayInMilliseconds < 0)
			{
				delayInMilliseconds = 0;
			}
			if (snippet != this.Segment)
			{
				this.LoadSegment(snippet);
			}
			this.m_stoppedExplicitly = false;
			this.m_playbackIsScheduled = true;
			this.m_timeStampOfPlaybackStart = Logik.GetTimestampMillisElapsedSinceInitialisation() + delayInMilliseconds;
			if (this.m_audioPlaybackLayerChannel != null)
			{
				this.m_audioPlaybackLayerChannel.ScheduleSegmentPlayback(snippet, delayInMilliseconds);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000091FD File Offset: 0x000073FD
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00009205 File Offset: 0x00007405
		internal float MasterVolume
		{
			get
			{
				return this.m_masterVolume;
			}
			set
			{
				if (value >= 0f && value <= 1f)
				{
					this.m_masterVolume = value;
					this.UpdateVolume();
				}
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00009224 File Offset: 0x00007424
		// (set) Token: 0x060001BE RID: 446 RVA: 0x0000922C File Offset: 0x0000742C
		internal float FadeOutVolume
		{
			get
			{
				return this.m_fadeOutVolume;
			}
			set
			{
				if (value >= 0f && value <= 1f)
				{
					this.m_fadeOutVolume = value;
					this.UpdateVolume();
				}
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000924C File Offset: 0x0000744C
		private void UpdateVolume()
		{
			if (this.m_audioPlaybackLayerChannel != null)
			{
				float num = this.MasterVolume * this.FadeOutVolume;
				this.m_audioPlaybackLayerChannel.SetVolume(num);
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000927C File Offset: 0x0000747C
		public void OnPlaybackHasStarted()
		{
			this.m_timeStampOfPlaybackStart = Logik.GetTimestampMillisElapsedSinceInitialisation();
			if (this.m_isMainChannel)
			{
				this.m_logik.SetSegmentEndApproachingAndReachedTimersAfterPlaybackHasStarted(0);
			}
		}

		// Token: 0x040000F2 RID: 242
		private Logik m_logik;

		// Token: 0x040000F3 RID: 243
		private int m_timeStampOfPlaybackStart;

		// Token: 0x040000F4 RID: 244
		private int m_timeStampOfSnippetLoad;

		// Token: 0x040000F5 RID: 245
		private bool m_playbackIsScheduled;

		// Token: 0x040000F6 RID: 246
		private bool m_stoppedExplicitly;

		// Token: 0x040000F7 RID: 247
		private IAudioPlaybackLayerChannel m_audioPlaybackLayerChannel;

		// Token: 0x040000F8 RID: 248
		private float m_masterVolume = 1f;

		// Token: 0x040000F9 RID: 249
		private float m_fadeOutVolume = 1f;

		// Token: 0x040000FA RID: 250
		private bool m_paused;

		// Token: 0x040000FB RID: 251
		private bool m_isMainChannel;
	}
}
