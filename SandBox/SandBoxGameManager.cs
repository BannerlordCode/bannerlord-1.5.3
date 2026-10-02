using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;

namespace SandBox
{
	// Token: 0x02000023 RID: 35
	public class SandBoxGameManager : MBGameManager
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000073EE File Offset: 0x000055EE
		// (set) Token: 0x06000110 RID: 272 RVA: 0x000073F6 File Offset: 0x000055F6
		public bool LoadingSavedGame { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000073FF File Offset: 0x000055FF
		public MetaData MetaData
		{
			get
			{
				LoadResult loadedGameResult = this._loadedGameResult;
				if (loadedGameResult == null)
				{
					return null;
				}
				return loadedGameResult.MetaData;
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007412 File Offset: 0x00005612
		public SandBoxGameManager(SandBoxGameManager.CampaignCreatorDelegate campaignCreator)
		{
			this.LoadingSavedGame = false;
			this._campaignCreator = campaignCreator;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00007428 File Offset: 0x00005628
		public SandBoxGameManager(LoadResult loadedGameResult)
		{
			this.LoadingSavedGame = true;
			this._loadedGameResult = loadedGameResult;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000743E File Offset: 0x0000563E
		public override void OnGameEnd(Game game)
		{
			MBDebug.SetErrorReportScene(null);
			base.OnGameEnd(game);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00007450 File Offset: 0x00005650
		protected override void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
		{
			nextStep = GameManagerLoadingSteps.None;
			switch (gameManagerLoadingStep)
			{
			case GameManagerLoadingSteps.PreInitializeZerothStep:
				nextStep = GameManagerLoadingSteps.FirstInitializeFirstStep;
				return;
			case GameManagerLoadingSteps.FirstInitializeFirstStep:
				MBGameManager.LoadModuleData(this.LoadingSavedGame);
				nextStep = GameManagerLoadingSteps.WaitSecondStep;
				return;
			case GameManagerLoadingSteps.WaitSecondStep:
				if (!this.LoadingSavedGame)
				{
					MBGameManager.StartNewGame();
				}
				nextStep = GameManagerLoadingSteps.SecondInitializeThirdState;
				return;
			case GameManagerLoadingSteps.SecondInitializeThirdState:
				MBGlobals.InitializeReferences();
				if (!this.LoadingSavedGame)
				{
					MBDebug.Print("Initializing new game begin...", 0, Debug.DebugColor.White, 17592186044416UL);
					Campaign campaign = this._campaignCreator();
					Game.CreateGame(campaign, this, campaign.Options.Seed);
					campaign.SetLoadingParameters(Campaign.GameLoadingType.NewCampaign);
					MBDebug.Print("Initializing new game end...", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				else
				{
					MBDebug.Print("Initializing saved game begin...", 0, Debug.DebugColor.White, 17592186044416UL);
					((Campaign)Game.LoadSaveGame(this._loadedGameResult, this).GameType).SetLoadingParameters(Campaign.GameLoadingType.SavedCampaign);
					this._loadedGameResult = null;
					Common.MemoryCleanupGC(false);
					MBDebug.Print("Initializing saved game end...", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				Game.Current.DoLoading();
				nextStep = GameManagerLoadingSteps.PostInitializeFourthState;
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
				nextStep = (Game.Current.DoLoading() ? GameManagerLoadingSteps.None : GameManagerLoadingSteps.FinishLoadingFifthStep);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000075DC File Offset: 0x000057DC
		public override void OnAfterCampaignStart(Game game)
		{
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000075E0 File Offset: 0x000057E0
		public override void OnLoadFinished()
		{
			if (!this.LoadingSavedGame)
			{
				MBDebug.Print("Switching to menu window...", 0, Debug.DebugColor.White, 17592186044416UL);
				if (!Game.Current.IsDevelopmentMode)
				{
					MBDebug.Print("OnLoadFinished Not DevelopmentMode", 0, Debug.DebugColor.White, 17592186044416UL);
					VideoPlaybackState videoPlaybackState = Game.Current.GameStateManager.CreateState<VideoPlaybackState>();
					string text = ModuleHelper.GetModuleFullPath("SandBox") + "Videos/CampaignIntro/";
					string text2 = text + "campaign_intro";
					string text3 = text + "campaign_intro.ivf";
					string text4 = text + "campaign_intro.ogg";
					videoPlaybackState.SetStartingParameters(text3, text4, text2, 30f, true);
					videoPlaybackState.SetOnVideoFinisedDelegate(new Action(this.LaunchSandboxCharacterCreation));
					Game.Current.GameStateManager.CleanAndPushState(videoPlaybackState, 0);
				}
				else
				{
					MBDebug.Print("OnLoadFinished DevelopmentMode", 0, Debug.DebugColor.White, 17592186044416UL);
					MBDebug.Print("Launching Sandbox Character Creation", 0, Debug.DebugColor.White, 17592186044416UL);
					this.LaunchSandboxCharacterCreation();
				}
			}
			else
			{
				MBDebug.Print("Loading Save Game", 0, Debug.DebugColor.White, 17592186044416UL);
				Game.Current.GameStateManager.OnSavedGameLoadFinished();
				Game.Current.GameStateManager.CleanAndPushState(Game.Current.GameStateManager.CreateState<MapState>(), 0);
				MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
				string text5 = ((mapState != null) ? mapState.GameMenuId : null);
				if (!string.IsNullOrEmpty(text5))
				{
					if (Campaign.Current.GameMenuManager.GetGameMenu(text5) != null)
					{
						PlayerEncounter playerEncounter = PlayerEncounter.Current;
						if (playerEncounter != null)
						{
							playerEncounter.OnLoad();
						}
						Campaign.Current.GameMenuManager.SetNextMenu(text5);
					}
					else
					{
						PlayerEncounter.Finish(true);
					}
				}
				PartyBase.MainParty.SetVisualAsDirty();
				Campaign.Current.CampaignInformationManager.OnGameLoaded();
				foreach (Settlement settlement in Settlement.All)
				{
					settlement.Party.SetLevelMaskIsDirty();
				}
				CampaignEventDispatcher.Instance.OnGameLoadFinished();
				if (mapState != null)
				{
					mapState.OnLoadingFinished();
				}
			}
			base.IsLoaded = true;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007814 File Offset: 0x00005A14
		private void LaunchSandboxCharacterCreation()
		{
			CharacterCreationState characterCreationState = Game.Current.GameStateManager.CreateState<CharacterCreationState>();
			Game.Current.GameStateManager.CleanAndPushState(characterCreationState, 0);
		}

		// Token: 0x04000053 RID: 83
		private LoadResult _loadedGameResult;

		// Token: 0x04000054 RID: 84
		private SandBoxGameManager.CampaignCreatorDelegate _campaignCreator;

		// Token: 0x0200013B RID: 315
		// (Invoke) Token: 0x06000E15 RID: 3605
		public delegate Campaign CampaignCreatorDelegate();
	}
}
