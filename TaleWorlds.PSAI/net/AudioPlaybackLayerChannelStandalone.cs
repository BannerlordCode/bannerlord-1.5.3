using System;
using System.IO;
using TaleWorlds.Engine;
using TaleWorlds.ModuleManager;

namespace psai.net
{
	// Token: 0x0200000D RID: 13
	public class AudioPlaybackLayerChannelStandalone : IAudioPlaybackLayerChannel
	{
		// Token: 0x0600012E RID: 302 RVA: 0x00006587 File Offset: 0x00004787
		public AudioPlaybackLayerChannelStandalone()
		{
			this.index = Music.GetFreeMusicChannelIndex();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000659C File Offset: 0x0000479C
		~AudioPlaybackLayerChannelStandalone()
		{
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000065C4 File Offset: 0x000047C4
		public void Release()
		{
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000065C6 File Offset: 0x000047C6
		internal void StopIfPlaying()
		{
			Music.StopMusic(this.index);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000065D4 File Offset: 0x000047D4
		public PsaiResult LoadSegment(Segment segment)
		{
			this._audioData = segment.audioData;
			string text = Path.Combine(ModuleHelper.GetModuleFullPath(this._audioData.moduleId) + "Music/", this._audioData.filePathRelativeToProjectDir);
			Music.LoadClip(this.index, text);
			return PsaiResult.OK;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006625 File Offset: 0x00004825
		public PsaiResult ReleaseSegment()
		{
			Music.UnloadClip(this.index);
			return PsaiResult.OK;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006633 File Offset: 0x00004833
		public PsaiResult ScheduleSegmentPlayback(Segment snippet, int delayMilliseconds)
		{
			Music.PlayDelayed(this.index, delayMilliseconds);
			return PsaiResult.OK;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006642 File Offset: 0x00004842
		public PsaiResult StopChannel()
		{
			this.StopIfPlaying();
			return PsaiResult.OK;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000664B File Offset: 0x0000484B
		public PsaiResult SetVolume(float volume)
		{
			Music.SetVolume(this.index, volume);
			return PsaiResult.OK;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000665A File Offset: 0x0000485A
		public PsaiResult SetPaused(bool paused)
		{
			if (paused)
			{
				Music.PauseMusic(this.index);
			}
			else
			{
				Music.PlayMusic(this.index);
			}
			return PsaiResult.OK;
		}

		// Token: 0x0400007D RID: 125
		private AudioData _audioData;

		// Token: 0x0400007E RID: 126
		private int index;
	}
}
