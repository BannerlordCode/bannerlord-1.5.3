using System;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000393 RID: 915
	public class MBInitialScreenBase : ScreenBase, IGameStateListener
	{
		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060034B6 RID: 13494 RVA: 0x000DA1FA File Offset: 0x000D83FA
		// (set) Token: 0x060034B7 RID: 13495 RVA: 0x000DA202 File Offset: 0x000D8402
		private protected InitialState _state { protected get; private set; }

		// Token: 0x060034B8 RID: 13496 RVA: 0x000DA20B File Offset: 0x000D840B
		public MBInitialScreenBase(InitialState state)
		{
			this._state = state;
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x000DA225 File Offset: 0x000D8425
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000DA227 File Offset: 0x000D8427
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000DA229 File Offset: 0x000D8429
		void IGameStateListener.OnInitialize()
		{
			this._state.OnInitialMenuOptionInvoked += this.OnExecutedInitialStateOption;
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x000DA242 File Offset: 0x000D8442
		void IGameStateListener.OnFinalize()
		{
			this._state.OnInitialMenuOptionInvoked -= this.OnExecutedInitialStateOption;
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x000DA25B File Offset: 0x000D845B
		private void OnExecutedInitialStateOption(InitialStateOption target)
		{
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x000DA25D File Offset: 0x000D845D
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._camera = Camera.CreateCamera();
			Common.MemoryCleanupGC(false);
			if (Game.Current != null)
			{
				Game.Current.Destroy();
			}
			MBMusicManager.Initialize();
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x000DA28C File Offset: 0x000D848C
		protected override void OnFinalize()
		{
			this._camera = null;
			this._videoPlayerView.SetEnable(false);
			this._videoPlayerView.FinalizePlayer();
			base.OnFinalize();
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000DA2B4 File Offset: 0x000D84B4
		protected sealed override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._videoPlayerView == null)
			{
				Console.WriteLine("InitialScreen::OnFrameTick scene view null");
			}
			LoadingWindow.DisableGlobalLoadingWindow();
			if (Input.IsKeyDown(InputKey.LeftControl) && Input.IsKeyReleased(InputKey.E))
			{
				MBInitialScreenBase.OnEditModeEnterPress();
			}
			if (ScreenManager.TopScreen == this)
			{
				this.OnInitialScreenTick(dt);
			}
			Vec2 screenResolution = MBWindowManager.GetScreenResolution();
			if (this._screenResUsedForVideo != screenResolution)
			{
				this.RefreshVideoAspect(screenResolution);
				this._screenResUsedForVideo = screenResolution;
			}
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x000DA32D File Offset: 0x000D852D
		protected virtual void OnInitialScreenTick(float dt)
		{
			if (this._videoPlayerView == null || !this._isPlayingVideo)
			{
				this.RefreshScene();
			}
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x000DA34B File Offset: 0x000D854B
		protected override void OnActivate()
		{
			base.OnActivate();
			if (Utilities.renderingActive)
			{
				this.RefreshScene();
				Utilities.DisableGlobalLoadingWindow();
			}
			if (NativeConfig.DoLocalizationCheckAtStartup)
			{
				LocalizedTextManager.CheckValidity(new List<string>());
			}
			Module.CurrentModule.SetCanLoadModules(true);
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000DA384 File Offset: 0x000D8584
		private void RefreshScene()
		{
			this._isPlayingVideo = false;
			if (this._videoPlayerView != null)
			{
				this._videoPlayerView.StopVideo();
				this._videoPlayerView.FinalizePlayer();
				this._videoPlayerView = null;
			}
			this._videoPlayerView = VideoPlayerView.CreateVideoPlayerView();
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetActiveModules())
			{
				string text = Path.Combine(moduleInfo.FolderPath, "Videos", "initial_menu").ToString();
				if (Directory.Exists(text))
				{
					string text2 = "*_pc.ivf";
					foreach (string text3 in Directory.GetDirectories(text))
					{
						string[] files = Directory.GetFiles(text3, "*.ogg");
						bool flag = files.Length != 0;
						string[] files2 = Directory.GetFiles(text3, text2);
						bool flag2 = files2.Length != 0;
						if (flag && flag2)
						{
							list.Add(new KeyValuePair<string, string>(files2[0], files[0]));
						}
					}
				}
			}
			float num = 30f;
			string text4 = string.Empty;
			string text5 = string.Empty;
			if (list.Count > 0)
			{
				int count = list.Count;
				int num2 = new Random(DateTime.Now.Second).Next(count);
				text4 = list[num2].Key;
				text5 = list[num2].Value;
				this._videoPlayerView.PlayVideo(text4, text5, num, true);
				this._isPlayingVideo = true;
			}
			Vec2 screenResolution = MBWindowManager.GetScreenResolution();
			this.RefreshVideoAspect(screenResolution);
			Debug.Print(string.Format("Initial Screen: Video is playing after refresh: {0} {1}::{2}", this._isPlayingVideo, text4, text5), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x000DA554 File Offset: 0x000D8754
		private void RefreshVideoAspect(Vec2 screenRes)
		{
			float num = screenRes.x / screenRes.y;
			if (num > 1.7777778f)
			{
				float num2 = 1f - 1.7777778f / num;
				this._videoPlayerView.SetOffset(new Vec2(num2 * 0.5f, 0f));
				this._videoPlayerView.SetScale(new Vec2(1f - num2, 1f));
				return;
			}
			if (num < 1.7777778f)
			{
				float num3 = screenRes.y / screenRes.x;
				float num4 = 1f - 0.5625f / num3;
				this._videoPlayerView.SetOffset(new Vec2(0f, num4 * 0.5f));
				this._videoPlayerView.SetScale(new Vec2(1f, 1f - num4));
			}
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x000DA61A File Offset: 0x000D881A
		private void OnSceneEditorWindowOpen()
		{
			GameStateManager.Current.CleanAndPushState(GameStateManager.Current.CreateState<EditorState>(), 0);
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x000DA631 File Offset: 0x000D8831
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this._videoPlayerView.StopVideo();
			Module.CurrentModule.SetCanLoadModules(false);
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x000DA64F File Offset: 0x000D884F
		protected override void OnPause()
		{
			LoadingWindow.DisableGlobalLoadingWindow();
			this._videoPlayerView.StopVideo();
			base.OnPause();
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x000DA667 File Offset: 0x000D8867
		protected override void OnResume()
		{
			base.OnResume();
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x000DA66F File Offset: 0x000D886F
		public static void DoExitButtonAction()
		{
			MBAPI.IMBScreen.OnExitButtonClick();
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x000DA67B File Offset: 0x000D887B
		public bool StartedRendering()
		{
			return true;
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000DA67E File Offset: 0x000D887E
		public static void OnEditModeEnterPress()
		{
			MBAPI.IMBScreen.OnEditModeEnterPress();
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000DA68A File Offset: 0x000D888A
		public static void OnEditModeEnterRelease()
		{
			MBAPI.IMBScreen.OnEditModeEnterRelease();
		}

		// Token: 0x0400165D RID: 5725
		private Camera _camera;

		// Token: 0x0400165E RID: 5726
		protected VideoPlayerView _videoPlayerView;

		// Token: 0x0400165F RID: 5727
		private Vec2 _screenResUsedForVideo = Vec2.Zero;

		// Token: 0x04001661 RID: 5729
		private bool _isPlayingVideo;
	}
}
