using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200024D RID: 589
	public class VideoPlaybackState : GameState
	{
		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002202 RID: 8706 RVA: 0x00077BEB File Offset: 0x00075DEB
		// (set) Token: 0x06002203 RID: 8707 RVA: 0x00077BF3 File Offset: 0x00075DF3
		public string VideoPath { get; private set; }

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002204 RID: 8708 RVA: 0x00077BFC File Offset: 0x00075DFC
		// (set) Token: 0x06002205 RID: 8709 RVA: 0x00077C04 File Offset: 0x00075E04
		public string AudioPath { get; private set; }

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002206 RID: 8710 RVA: 0x00077C0D File Offset: 0x00075E0D
		// (set) Token: 0x06002207 RID: 8711 RVA: 0x00077C15 File Offset: 0x00075E15
		public float FrameRate { get; private set; }

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002208 RID: 8712 RVA: 0x00077C1E File Offset: 0x00075E1E
		// (set) Token: 0x06002209 RID: 8713 RVA: 0x00077C26 File Offset: 0x00075E26
		public string SubtitleFileBasePath { get; private set; }

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x0600220A RID: 8714 RVA: 0x00077C2F File Offset: 0x00075E2F
		// (set) Token: 0x0600220B RID: 8715 RVA: 0x00077C37 File Offset: 0x00075E37
		public bool CanUserSkip { get; private set; }

		// Token: 0x0600220C RID: 8716 RVA: 0x00077C40 File Offset: 0x00075E40
		public void SetStartingParameters(string videoPath, string audioPath, string subtitleFileBasePath, float frameRate = 30f, bool canUserSkip = true)
		{
			this.VideoPath = videoPath;
			this.AudioPath = audioPath;
			this.FrameRate = frameRate;
			this.SubtitleFileBasePath = subtitleFileBasePath;
			this.CanUserSkip = canUserSkip;
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x00077C67 File Offset: 0x00075E67
		public void SetOnVideoFinisedDelegate(Action onVideoFinised)
		{
			this._onVideoFinised = onVideoFinised;
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x00077C70 File Offset: 0x00075E70
		public void OnVideoStarted()
		{
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.PauseMusicManagerSystem();
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x00077C86 File Offset: 0x00075E86
		public void OnVideoFinished()
		{
			MBMusicManager.Current.UnpauseMusicManagerSystem();
			Action onVideoFinised = this._onVideoFinised;
			if (onVideoFinised == null)
			{
				return;
			}
			onVideoFinised();
		}

		// Token: 0x04000D0D RID: 3341
		private Action _onVideoFinised;
	}
}
