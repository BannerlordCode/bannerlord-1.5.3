using System;
using SandBox.GauntletUI.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.GauntletUI.SceneNotification;

namespace SandBox.GauntletUI
{
	// Token: 0x02000013 RID: 19
	public class SandBoxGauntletUISubModule : MBSubModuleBase
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00007EF7 File Offset: 0x000060F7
		public SandBoxGauntletUISubModule()
		{
			this._conversationListener = new SandBoxGauntletUISubModule.SandBoxGameStateManagerListener();
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00007F0A File Offset: 0x0000610A
		public override void OnCampaignStart(Game game, object starterObject)
		{
			base.OnCampaignStart(game, starterObject);
			if (!this._gameStarted && game.GameType is Campaign)
			{
				this._gameStarted = true;
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00007F30 File Offset: 0x00006130
		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			base.OnGameStart(game, gameStarterObject);
			if (!this._gameStarted && game.GameType is Campaign)
			{
				this._gameStarted = true;
				SandBoxGauntletGameNotification.Initialize();
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007F5B File Offset: 0x0000615B
		public override void OnGameEnd(Game game)
		{
			base.OnGameEnd(game);
			if (this._gameStarted && game.GameType is Campaign)
			{
				this._gameStarted = false;
				GauntletGameNotification.Initialize();
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00007F85 File Offset: 0x00006185
		public override void BeginGameStart(Game game)
		{
			base.BeginGameStart(game);
			if (Campaign.Current != null)
			{
				Campaign.Current.VisualCreator.MapEventVisualCreator = new GauntletMapEventVisualCreator();
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00007FAC File Offset: 0x000061AC
		protected override void OnApplicationTick(float dt)
		{
			base.OnApplicationTick(dt);
			if (!this._initializedConversationHandler)
			{
				Game game = Game.Current;
				if (((game != null) ? game.GameStateManager : null) != null)
				{
					Game.Current.GameStateManager.RegisterListener(this._conversationListener);
					this._registeredGameStateManager = Game.Current.GameStateManager;
					this._initializedConversationHandler = true;
					goto IL_008C;
				}
			}
			if (this._initializedConversationHandler)
			{
				Game game2 = Game.Current;
				if (((game2 != null) ? game2.GameStateManager : null) == null)
				{
					this._registeredGameStateManager.UnregisterListener(this._conversationListener);
					this._initializedConversationHandler = false;
					this._registeredGameStateManager = null;
				}
			}
			IL_008C:
			if (!this._initialized && GauntletSceneNotification.Current != null)
			{
				if (!Utilities.CommandLineArgumentExists("VisualTests"))
				{
					GauntletSceneNotification.Current.RegisterContextProvider(new SandboxSceneNotificationContextProvider());
				}
				this._initialized = true;
			}
		}

		// Token: 0x0400005B RID: 91
		private bool _gameStarted;

		// Token: 0x0400005C RID: 92
		private bool _initialized;

		// Token: 0x0400005D RID: 93
		private GameStateManager _registeredGameStateManager;

		// Token: 0x0400005E RID: 94
		private bool _initializedConversationHandler;

		// Token: 0x0400005F RID: 95
		private SandBoxGauntletUISubModule.SandBoxGameStateManagerListener _conversationListener;

		// Token: 0x02000058 RID: 88
		private class SandBoxGameStateManagerListener : IGameStateManagerListener
		{
			// Token: 0x06000409 RID: 1033 RVA: 0x00018903 File Offset: 0x00016B03
			void IGameStateManagerListener.OnCleanStates()
			{
				this.UpdateCampaignMission();
			}

			// Token: 0x0600040A RID: 1034 RVA: 0x0001890B File Offset: 0x00016B0B
			void IGameStateManagerListener.OnCreateState(GameState gameState)
			{
			}

			// Token: 0x0600040B RID: 1035 RVA: 0x0001890D File Offset: 0x00016B0D
			void IGameStateManagerListener.OnPopState(GameState gameState)
			{
				this.UpdateCampaignMission();
				if (gameState is MissionState || gameState is MapState)
				{
					CampaignInformationManager.ClearAllDialogNotifications(false);
				}
			}

			// Token: 0x0600040C RID: 1036 RVA: 0x0001892B File Offset: 0x00016B2B
			void IGameStateManagerListener.OnPushState(GameState gameState, bool isTopGameState)
			{
				this.UpdateCampaignMission();
				if (gameState is MissionState || gameState is MapState)
				{
					CampaignInformationManager.ClearAllDialogNotifications(false);
				}
			}

			// Token: 0x0600040D RID: 1037 RVA: 0x00018949 File Offset: 0x00016B49
			void IGameStateManagerListener.OnSavedGameLoadFinished()
			{
			}

			// Token: 0x0600040E RID: 1038 RVA: 0x0001894B File Offset: 0x00016B4B
			private void UpdateCampaignMission()
			{
				ICampaignMission campaignMission = CampaignMission.Current;
				if (campaignMission == null)
				{
					return;
				}
				campaignMission.OnGameStateChanged();
			}
		}
	}
}
