using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x02000054 RID: 84
	[GameStateScreen(typeof(GameLoadingState))]
	public class GameLoadingScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x00011FB6 File Offset: 0x000101B6
		public GameLoadingScreen(GameLoadingState gameLoadingState)
		{
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00011FBE File Offset: 0x000101BE
		protected override void OnActivate()
		{
			base.OnActivate();
			LoadingWindow.EnableGlobalLoadingWindow();
			Utilities.SetScreenTextRenderingState(false);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00011FD1 File Offset: 0x000101D1
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			Utilities.SetScreenTextRenderingState(true);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00011FE0 File Offset: 0x000101E0
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00011FE2 File Offset: 0x000101E2
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00011FE4 File Offset: 0x000101E4
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00011FE6 File Offset: 0x000101E6
		void IGameStateListener.OnFinalize()
		{
		}
	}
}
