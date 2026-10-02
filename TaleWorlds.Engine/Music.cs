using System;

namespace TaleWorlds.Engine
{
	// Token: 0x0200006F RID: 111
	public class Music
	{
		// Token: 0x06000A52 RID: 2642 RVA: 0x0000A6BD File Offset: 0x000088BD
		public static int GetFreeMusicChannelIndex()
		{
			return EngineApplicationInterface.IMusic.GetFreeMusicChannelIndex();
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0000A6C9 File Offset: 0x000088C9
		public static void LoadClip(int index, string pathToClip)
		{
			EngineApplicationInterface.IMusic.LoadClip(index, pathToClip);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0000A6D7 File Offset: 0x000088D7
		public static void UnloadClip(int index)
		{
			EngineApplicationInterface.IMusic.UnloadClip(index);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0000A6E4 File Offset: 0x000088E4
		public static bool IsClipLoaded(int index)
		{
			return EngineApplicationInterface.IMusic.IsClipLoaded(index);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0000A6F1 File Offset: 0x000088F1
		public static void PlayMusic(int index)
		{
			EngineApplicationInterface.IMusic.PlayMusic(index);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0000A6FE File Offset: 0x000088FE
		public static void PlayDelayed(int index, int deltaMilliseconds)
		{
			EngineApplicationInterface.IMusic.PlayDelayed(index, deltaMilliseconds);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0000A70C File Offset: 0x0000890C
		public static bool IsMusicPlaying(int index)
		{
			return EngineApplicationInterface.IMusic.IsMusicPlaying(index);
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0000A719 File Offset: 0x00008919
		public static void PauseMusic(int index)
		{
			EngineApplicationInterface.IMusic.PauseMusic(index);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0000A726 File Offset: 0x00008926
		public static void StopMusic(int index)
		{
			EngineApplicationInterface.IMusic.StopMusic(index);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0000A733 File Offset: 0x00008933
		public static void SetVolume(int index, float volume)
		{
			EngineApplicationInterface.IMusic.SetVolume(index, volume);
		}
	}
}
