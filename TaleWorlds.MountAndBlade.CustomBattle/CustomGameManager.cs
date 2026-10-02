using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Multiplayer;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x02000011 RID: 17
	public class CustomGameManager : MBGameManager
	{
		// Token: 0x06000105 RID: 261 RVA: 0x0000858C File Offset: 0x0000678C
		protected override void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
		{
			nextStep = GameManagerLoadingSteps.None;
			switch (gameManagerLoadingStep)
			{
			case GameManagerLoadingSteps.PreInitializeZerothStep:
				MBGameManager.LoadModuleData(false);
				MBGlobals.InitializeReferences();
				Game.CreateGame(new CustomGame(), this).DoLoading();
				nextStep = GameManagerLoadingSteps.FirstInitializeFirstStep;
				return;
			case GameManagerLoadingSteps.FirstInitializeFirstStep:
			{
				bool flag = true;
				foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
				{
					flag = flag && mbsubModuleBase.DoLoading(Game.Current);
				}
				nextStep = (flag ? GameManagerLoadingSteps.WaitSecondStep : GameManagerLoadingSteps.FirstInitializeFirstStep);
				return;
			}
			case GameManagerLoadingSteps.WaitSecondStep:
				MBGameManager.StartNewGame();
				nextStep = GameManagerLoadingSteps.SecondInitializeThirdState;
				return;
			case GameManagerLoadingSteps.SecondInitializeThirdState:
				nextStep = (Game.Current.DoLoading() ? GameManagerLoadingSteps.PostInitializeFourthState : GameManagerLoadingSteps.SecondInitializeThirdState);
				return;
			case GameManagerLoadingSteps.PostInitializeFourthState:
				nextStep = GameManagerLoadingSteps.FinishLoadingFifthStep;
				return;
			case GameManagerLoadingSteps.FinishLoadingFifthStep:
				nextStep = GameManagerLoadingSteps.None;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00008660 File Offset: 0x00006860
		public override void OnAfterCampaignStart(Game game)
		{
			MultiplayerMain.Initialize(new GameNetworkHandler());
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000866C File Offset: 0x0000686C
		public override void OnLoadFinished()
		{
			base.OnLoadFinished();
			Game.Current.GameStateManager.CleanAndPushState(Game.Current.GameStateManager.CreateState<CustomBattleState>(), 0);
		}
	}
}
