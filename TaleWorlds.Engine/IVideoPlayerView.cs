using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000034 RID: 52
	[ApplicationInterfaceBase]
	internal interface IVideoPlayerView
	{
		// Token: 0x0600054F RID: 1359
		[EngineMethod("create_video_player_view", false, null, false)]
		VideoPlayerView CreateVideoPlayerView();

		// Token: 0x06000550 RID: 1360
		[EngineMethod("play_video", false, null, false)]
		void PlayVideo(UIntPtr pointer, string videoFileName, string soundFileName, float framerate, bool looping);

		// Token: 0x06000551 RID: 1361
		[EngineMethod("stop_video", false, null, false)]
		void StopVideo(UIntPtr pointer);

		// Token: 0x06000552 RID: 1362
		[EngineMethod("is_video_finished", false, null, false)]
		bool IsVideoFinished(UIntPtr pointer);

		// Token: 0x06000553 RID: 1363
		[EngineMethod("finalize", false, null, false)]
		void Finalize(UIntPtr pointer);
	}
}
