using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200009C RID: 156
	[EngineClass("rglVideo_player_view")]
	public sealed class VideoPlayerView : View
	{
		// Token: 0x06000DF4 RID: 3572 RVA: 0x0000FD06 File Offset: 0x0000DF06
		internal VideoPlayerView(UIntPtr meshPointer)
			: base(meshPointer)
		{
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0000FD0F File Offset: 0x0000DF0F
		public static VideoPlayerView CreateVideoPlayerView()
		{
			return EngineApplicationInterface.IVideoPlayerView.CreateVideoPlayerView();
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0000FD1B File Offset: 0x0000DF1B
		public void PlayVideo(string videoFileName, string soundFileName, float framerate, bool looping)
		{
			EngineApplicationInterface.IVideoPlayerView.PlayVideo(base.Pointer, videoFileName, soundFileName, framerate, looping);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0000FD32 File Offset: 0x0000DF32
		public void StopVideo()
		{
			EngineApplicationInterface.IVideoPlayerView.StopVideo(base.Pointer);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0000FD44 File Offset: 0x0000DF44
		public bool IsVideoFinished()
		{
			return EngineApplicationInterface.IVideoPlayerView.IsVideoFinished(base.Pointer);
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0000FD56 File Offset: 0x0000DF56
		public void FinalizePlayer()
		{
			EngineApplicationInterface.IVideoPlayerView.Finalize(base.Pointer);
		}
	}
}
