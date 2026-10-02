using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200003F RID: 63
	[ApplicationInterfaceBase]
	internal interface IMusic
	{
		// Token: 0x0600065A RID: 1626
		[EngineMethod("get_free_music_channel_index", false, null, false)]
		int GetFreeMusicChannelIndex();

		// Token: 0x0600065B RID: 1627
		[EngineMethod("load_clip", false, null, false)]
		void LoadClip(int index, string pathToClip);

		// Token: 0x0600065C RID: 1628
		[EngineMethod("unload_clip", false, null, false)]
		void UnloadClip(int index);

		// Token: 0x0600065D RID: 1629
		[EngineMethod("is_clip_loaded", false, null, false)]
		bool IsClipLoaded(int index);

		// Token: 0x0600065E RID: 1630
		[EngineMethod("play_music", false, null, false)]
		void PlayMusic(int index);

		// Token: 0x0600065F RID: 1631
		[EngineMethod("play_delayed", false, null, false)]
		void PlayDelayed(int index, int delayMilliseconds);

		// Token: 0x06000660 RID: 1632
		[EngineMethod("is_music_playing", false, null, false)]
		bool IsMusicPlaying(int index);

		// Token: 0x06000661 RID: 1633
		[EngineMethod("pause_music", false, null, false)]
		void PauseMusic(int index);

		// Token: 0x06000662 RID: 1634
		[EngineMethod("stop_music", false, null, false)]
		void StopMusic(int index);

		// Token: 0x06000663 RID: 1635
		[EngineMethod("set_volume", false, null, false)]
		void SetVolume(int index, float volume);
	}
}
