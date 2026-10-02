using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000022 RID: 34
	public class MultiplayerGameManager : MBGameManager
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x00007DA3 File Offset: 0x00005FA3
		public MultiplayerGameManager()
		{
			MBMusicManager mbmusicManager = MBMusicManager.Current;
			if (mbmusicManager == null)
			{
				return;
			}
			mbmusicManager.PauseMusicManagerSystem();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00007DBC File Offset: 0x00005FBC
		protected override void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
		{
			nextStep = GameManagerLoadingSteps.None;
			switch (gameManagerLoadingStep)
			{
			case GameManagerLoadingSteps.PreInitializeZerothStep:
				nextStep = GameManagerLoadingSteps.FirstInitializeFirstStep;
				return;
			case GameManagerLoadingSteps.FirstInitializeFirstStep:
				MBGameManager.LoadModuleData(false);
				MBDebug.Print("Game creating...", 0, Debug.DebugColor.White, 17592186044416UL);
				MBGlobals.InitializeReferences();
				Game.CreateGame(new MultiplayerGame(), this).DoLoading();
				nextStep = GameManagerLoadingSteps.WaitSecondStep;
				return;
			case GameManagerLoadingSteps.WaitSecondStep:
				MBGameManager.StartNewGame();
				nextStep = GameManagerLoadingSteps.SecondInitializeThirdState;
				return;
			case GameManagerLoadingSteps.SecondInitializeThirdState:
				nextStep = (Game.Current.DoLoading() ? GameManagerLoadingSteps.PostInitializeFourthState : GameManagerLoadingSteps.SecondInitializeThirdState);
				return;
			case GameManagerLoadingSteps.PostInitializeFourthState:
			{
				bool flag = true;
				foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
				{
					flag = flag && mbsubModuleBase.DoLoading(Game.Current);
				}
				nextStep = (flag ? GameManagerLoadingSteps.FinishLoadingFifthStep : GameManagerLoadingSteps.PostInitializeFourthState);
				return;
			}
			case GameManagerLoadingSteps.FinishLoadingFifthStep:
				nextStep = GameManagerLoadingSteps.None;
				return;
			default:
				return;
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007EA8 File Offset: 0x000060A8
		public override void OnLoadFinished()
		{
			base.OnLoadFinished();
			MBGlobals.InitializeReferences();
			GameState gameState;
			if (GameNetwork.IsDedicatedServer)
			{
				DedicatedServerType dedicatedServerType = Module.CurrentModule.StartupInfo.DedicatedServerType;
				gameState = Game.Current.GameStateManager.CreateState<UnspecifiedDedicatedServerState>();
				Utilities.SetFrameLimiterWithSleep(true);
			}
			else
			{
				gameState = Game.Current.GameStateManager.CreateState<LobbyState>();
			}
			Game.Current.GameStateManager.CleanAndPushState(gameState, 0);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00007F12 File Offset: 0x00006112
		public override void OnAfterCampaignStart(Game game)
		{
			if (GameNetwork.IsDedicatedServer)
			{
				MultiplayerMain.InitializeAsDedicatedServer(new GameNetworkHandler());
				return;
			}
			MultiplayerMain.Initialize(new GameNetworkHandler());
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00007F30 File Offset: 0x00006130
		public override void OnNewCampaignStart(Game game, object starterObject)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.OnMultiplayerGameStart(game, starterObject);
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00007F88 File Offset: 0x00006188
		public override void OnSessionInvitationAccepted(SessionInvitationType sessionInvitationType)
		{
			if (sessionInvitationType == SessionInvitationType.Multiplayer)
			{
				return;
			}
			base.OnSessionInvitationAccepted(sessionInvitationType);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00007F96 File Offset: 0x00006196
		public override void OnPlatformRequestedMultiplayer()
		{
		}
	}
}
