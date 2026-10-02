using System;
using SandBox.ViewModelCollection.GameOver;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI
{
	// Token: 0x0200000B RID: 11
	[GameStateScreen(typeof(GameOverState))]
	public class GauntletGameOverScreen : ScreenBase, IGameOverStateHandler, IGameStateListener
	{
		// Token: 0x0600006A RID: 106 RVA: 0x0000557A File Offset: 0x0000377A
		public GauntletGameOverScreen(GameOverState gameOverState)
		{
			this._gameOverState = gameOverState;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00005589 File Offset: 0x00003789
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this.CloseGameOverScreen();
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000055BC File Offset: 0x000037BC
		void IGameStateListener.OnActivate()
		{
			base.OnActivate();
			this._gameOverCategory = UIResourceManager.LoadSpriteCategory("ui_gameover");
			this._gauntletLayer = new GauntletLayer("GameOverScreen", 1, true);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
			base.AddLayer(this._gauntletLayer);
			this._dataSource = new GameOverVM(this._gameOverState.Reason, new Action(this.CloseGameOverScreen));
			this._dataSource.SetCloseInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._gauntletLayer.LoadMovie("GameOverScreen", this._dataSource);
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.GameOverScreen));
			switch (this._gameOverState.Reason)
			{
			case GameOverState.GameOverReason.Retirement:
				UISoundsHelper.PlayUISound("event:/ui/endgame/end_retirement");
				break;
			case GameOverState.GameOverReason.ClanDestroyed:
				UISoundsHelper.PlayUISound("event:/ui/endgame/end_clan_destroyed");
				break;
			case GameOverState.GameOverReason.Victory:
				UISoundsHelper.PlayUISound("event:/ui/endgame/end_victory");
				break;
			}
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000056F5 File Offset: 0x000038F5
		void IGameStateListener.OnDeactivate()
		{
			base.OnDeactivate();
			base.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.None));
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00005735 File Offset: 0x00003935
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005737 File Offset: 0x00003937
		void IGameStateListener.OnFinalize()
		{
			this._gameOverCategory.Unload();
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._gauntletLayer = null;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005760 File Offset: 0x00003960
		private void CloseGameOverScreen()
		{
			bool flag = false;
			if (flag || Game.Current.IsDevelopmentMode || this._gameOverState.Reason == GameOverState.GameOverReason.Victory)
			{
				Game.Current.GameStateManager.PopState(0);
				if ((flag || Game.Current.IsDevelopmentMode) && this._gameOverState.Reason == GameOverState.GameOverReason.Retirement)
				{
					PlayerEncounter.Finish(true);
					return;
				}
			}
			else
			{
				MBGameManager.EndGame();
			}
		}

		// Token: 0x04000038 RID: 56
		private SpriteCategory _gameOverCategory;

		// Token: 0x04000039 RID: 57
		private GameOverVM _dataSource;

		// Token: 0x0400003A RID: 58
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400003B RID: 59
		private readonly GameOverState _gameOverState;
	}
}
