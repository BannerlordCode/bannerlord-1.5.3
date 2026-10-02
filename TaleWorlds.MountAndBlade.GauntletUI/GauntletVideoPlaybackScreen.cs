using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection.VideoPlayback;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x0200001A RID: 26
	[GameStateScreen(typeof(VideoPlaybackState))]
	public class GauntletVideoPlaybackScreen : VideoPlaybackScreen
	{
		// Token: 0x06000103 RID: 259 RVA: 0x0000838A File Offset: 0x0000658A
		public GauntletVideoPlaybackScreen(VideoPlaybackState videoPlaybackState)
			: base(videoPlaybackState)
		{
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00008394 File Offset: 0x00006594
		protected override void OnInitialize()
		{
			base.OnInitialize();
			string subtitleExtensionOfLanguage = LocalizedTextManager.GetSubtitleExtensionOfLanguage(BannerlordConfig.Language);
			List<SRTHelper.SubtitleItem> list = null;
			if (!string.IsNullOrEmpty(this._videoPlaybackState.SubtitleFileBasePath))
			{
				string text = this._videoPlaybackState.SubtitleFileBasePath + "_" + subtitleExtensionOfLanguage + ".srt";
				if (File.Exists(text))
				{
					list = SRTHelper.SrtParser.ParseStream(new FileStream(text, FileMode.Open, FileAccess.Read), Encoding.UTF8);
				}
				else
				{
					Debug.FailedAssert("No Subtitle file exists in path: " + text, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletVideoPlaybackScreen.cs", "OnInitialize", 41);
				}
			}
			this._layer = new GauntletLayer("VideoPlayback", 100002, false);
			this._dataSource = new VideoPlaybackVM();
			this._layer.LoadMovie("VideoPlayer", this._dataSource);
			this._dataSource.SetSubtitles(list);
			this._layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
			base.AddLayer(this._layer);
			InformationManager.HideAllMessages();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00008482 File Offset: 0x00006682
		protected override void OnVideoPlaybackTick(float dt)
		{
			base.OnVideoPlaybackTick(dt);
			this._dataSource.Tick(this._totalElapsedTimeSinceVideoStart);
		}

		// Token: 0x040000A2 RID: 162
		private GauntletLayer _layer;

		// Token: 0x040000A3 RID: 163
		private VideoPlaybackVM _dataSource;
	}
}
