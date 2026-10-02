using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000221 RID: 545
	public class EditorGameManager : MBGameManager
	{
		// Token: 0x06001FCD RID: 8141 RVA: 0x0006E244 File Offset: 0x0006C444
		protected override void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
		{
			nextStep = GameManagerLoadingSteps.None;
			switch (gameManagerLoadingStep)
			{
			case GameManagerLoadingSteps.PreInitializeZerothStep:
				MBGameManager.LoadModuleData(false);
				MBGlobals.InitializeReferences();
				Game.CreateGame(new EditorGame(), this).DoLoading();
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

		// Token: 0x06001FCE RID: 8142 RVA: 0x0006E318 File Offset: 0x0006C518
		public override void OnAfterCampaignStart(Game game)
		{
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0006E31A File Offset: 0x0006C51A
		public override void OnLoadFinished()
		{
			base.OnLoadFinished();
		}
	}
}
