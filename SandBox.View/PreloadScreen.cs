using System;
using Sandbox.View.GameStates;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;
using TaleWorlds.ScreenSystem;

namespace SandBox.View
{
	// Token: 0x02000008 RID: 8
	[GameStateScreen(typeof(PreloadState))]
	public class PreloadScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002974 File Offset: 0x00000B74
		public PreloadScreen(PreloadState inventoryState)
		{
			this._state = inventoryState;
			this._delayCounter = 0;
			this._delayInFrames = Math.Max(0, this._state.LoadDelayInFrames);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000029A1 File Offset: 0x00000BA1
		protected override void OnInitialize()
		{
			base.OnInitialize();
			LoadingWindow.EnableGlobalLoadingWindow();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000029B0 File Offset: 0x00000BB0
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._delayCounter != this._delayInFrames)
			{
				this._delayCounter++;
				return;
			}
			SaveGameFileInfo saveFileWithName = MBSaveLoad.GetSaveFileWithName(this._state.SaveToLoad);
			if (saveFileWithName == null)
			{
				throw new MBException("Preload state called without a valid save name. Game will be stuck at this point.");
			}
			SandBoxSaveHelper.TryLoadSave(saveFileWithName, new Action<LoadResult>(this.StartGame), null);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A11 File Offset: 0x00000C11
		private void StartGame(LoadResult loadResult)
		{
			MBGameManager.StartNewGame(new SandBoxGameManager(loadResult));
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A1E File Offset: 0x00000C1E
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A20 File Offset: 0x00000C20
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002A22 File Offset: 0x00000C22
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002A24 File Offset: 0x00000C24
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x04000004 RID: 4
		private readonly PreloadState _state;

		// Token: 0x04000005 RID: 5
		private readonly int _delayInFrames;

		// Token: 0x04000006 RID: 6
		private int _delayCounter;
	}
}
