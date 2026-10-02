using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x0200005B RID: 91
	public class VideoPlaybackScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x06000372 RID: 882 RVA: 0x00019F6A File Offset: 0x0001816A
		public VideoPlaybackScreen(VideoPlaybackState videoPlaybackState)
		{
			this._videoPlaybackState = videoPlaybackState;
			this._videoPlayerView = VideoPlayerView.CreateVideoPlayerView();
			this._videoPlayerView.SetRenderOrder(-10000);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00019F94 File Offset: 0x00018194
		protected sealed override void OnFrameTick(float dt)
		{
			this._totalElapsedTimeSinceVideoStart += dt;
			base.OnFrameTick(dt);
			if (this._videoPlayerView != null && this._videoPlaybackState != null)
			{
				if (this._videoPlaybackState.CanUserSkip && (Input.IsKeyReleased(InputKey.Escape) || Input.IsKeyReleased(InputKey.ControllerROption)))
				{
					this._videoPlayerView.StopVideo();
				}
				if (this._videoPlayerView.IsVideoFinished())
				{
					this._videoPlaybackState.OnVideoFinished();
					this._videoPlayerView.SetEnable(false);
					this._videoPlayerView.FinalizePlayer();
					this._videoPlayerView = null;
				}
				if (ScreenManager.TopScreen == this)
				{
					this.OnVideoPlaybackTick(dt);
				}
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001A03D File Offset: 0x0001823D
		protected virtual void OnVideoPlaybackTick(float dt)
		{
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001A040 File Offset: 0x00018240
		void IGameStateListener.OnInitialize()
		{
			this._videoPlayerView.PlayVideo(this._videoPlaybackState.VideoPath, this._videoPlaybackState.AudioPath, this._videoPlaybackState.FrameRate, false);
			this._videoPlaybackState.OnVideoStarted();
			LoadingWindow.DisableGlobalLoadingWindow();
			Utilities.DisableGlobalLoadingWindow();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0001A08F File Offset: 0x0001828F
		void IGameStateListener.OnFinalize()
		{
			VideoPlayerView videoPlayerView = this._videoPlayerView;
			if (videoPlayerView != null)
			{
				videoPlayerView.SetEnable(false);
			}
			VideoPlayerView videoPlayerView2 = this._videoPlayerView;
			if (videoPlayerView2 == null)
			{
				return;
			}
			videoPlayerView2.FinalizePlayer();
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0001A0B3 File Offset: 0x000182B3
		void IGameStateListener.OnActivate()
		{
			base.OnActivate();
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0001A0BB File Offset: 0x000182BB
		void IGameStateListener.OnDeactivate()
		{
			base.OnDeactivate();
		}

		// Token: 0x040001CF RID: 463
		protected VideoPlaybackState _videoPlaybackState;

		// Token: 0x040001D0 RID: 464
		protected VideoPlayerView _videoPlayerView;

		// Token: 0x040001D1 RID: 465
		protected float _totalElapsedTimeSinceVideoStart;
	}
}
