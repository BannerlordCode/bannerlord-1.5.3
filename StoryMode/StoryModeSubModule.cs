using System;
using StoryMode.GameComponents;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.Quests.PlayerClanQuests;
using StoryMode.Quests.SecondPhase;
using StoryMode.Quests.ThirdPhase;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace StoryMode
{
	// Token: 0x02000016 RID: 22
	public class StoryModeSubModule : MBSubModuleBase
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x000051D0 File Offset: 0x000033D0
		protected override void InitializeGameStarter(Game game, IGameStarter gameStarterObject)
		{
			CampaignStoryMode campaignStoryMode = game.GameType as CampaignStoryMode;
			if (campaignStoryMode != null)
			{
				CampaignGameStarter campaignGameStarter = (CampaignGameStarter)gameStarterObject;
				campaignStoryMode.AddCampaignEventReceiver(StoryModeEvents.Instance);
				this.AddGameMenus(campaignGameStarter);
				this.AddModels(campaignGameStarter);
				this.AddBehaviors(campaignGameStarter);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005213 File Offset: 0x00003413
		public override void OnGameEnd(Game game)
		{
			base.OnGameEnd(game);
			if (game.GameType is CampaignStoryMode && StoryModeManager.Current != null)
			{
				StoryModeManager.Current.Destroy();
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000523C File Offset: 0x0000343C
		private void AddGameMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenu("menu_story_mode_welcome", "{=GGfM1HKn}Welcome to MBII Bannerlord", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("menu_story_mode_welcome", "mno_continue", "{=str_continue}Continue...", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				return true;
			}, null, false, -1, false, null);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005298 File Offset: 0x00003498
		private void AddBehaviors(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddBehavior(new LordConversationsStoryModeBehavior());
			campaignGameStarter.AddBehavior(new MainStorylineCampaignBehavior());
			if (!StoryModeManager.Current.MainStoryLine.IsCompleted)
			{
				if (!StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted)
				{
					campaignGameStarter.AddBehavior(new TutorialPhaseCampaignBehavior());
				}
				if (!StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted)
				{
					campaignGameStarter.AddBehavior(new FirstPhaseCampaignBehavior());
				}
				if (!StoryModeManager.Current.MainStoryLine.IsSecondPhaseCompleted)
				{
					campaignGameStarter.AddBehavior(new SecondPhaseCampaignBehavior());
				}
				campaignGameStarter.AddBehavior(new ThirdPhaseCampaignBehavior());
			}
			campaignGameStarter.AddBehavior(new TrainingFieldCampaignBehavior());
			campaignGameStarter.AddBehavior(new StoryModeTutorialBoxCampaignBehavior());
			campaignGameStarter.AddBehavior(new StoryModeCharacterCreationCampaignBehavior());
			campaignGameStarter.AddBehavior(new StoryModeBanditSpawnCampaignBehavior());
			Debug.Print("campaignGameStarter.AddBehavior(AchievementsCampaignBehavior)", 0, Debug.DebugColor.White, 17592186044416UL);
			campaignGameStarter.AddBehavior(new AchievementsCampaignBehavior());
			campaignGameStarter.AddBehavior(new WeakenEmpireQuestBehavior());
			campaignGameStarter.AddBehavior(new AssembleEmpireQuestBehavior());
			campaignGameStarter.AddBehavior(new DefeatTheConspiracyQuestBehavior());
			campaignGameStarter.AddBehavior(new RescueFamilyQuestBehavior());
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000053AC File Offset: 0x000035AC
		private void AddModels(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddModel<BanditDensityModel>(new StoryModeBanditDensityModel());
			campaignGameStarter.AddModel<EncounterGameMenuModel>(new StoryModeEncounterGameMenuModel());
			campaignGameStarter.AddModel<BattleRewardModel>(new StoryModeBattleRewardModel());
			campaignGameStarter.AddModel<TargetScoreCalculatingModel>(new StoryModeTargetScoreCalculatingModel());
			campaignGameStarter.AddModel<PartyWageModel>(new StoryModePartyWageModel());
			campaignGameStarter.AddModel<KingdomDecisionPermissionModel>(new StoryModeKingdomDecisionPermissionModel());
			campaignGameStarter.AddModel<CombatXpModel>(new StoryModeCombatXpModel());
			campaignGameStarter.AddModel<GenericXpModel>(new StoryModeGenericXpModel());
			campaignGameStarter.AddModel<NotableSpawnModel>(new StoryModeNotableSpawnModel());
			campaignGameStarter.AddModel<HeroDeathProbabilityCalculationModel>(new StoryModeHeroDeathProbabilityCalculationModel());
			campaignGameStarter.AddModel<AgentDecideKilledOrUnconsciousModel>(new StoryModeAgentDecideKilledOrUnconsciousModel());
			campaignGameStarter.AddModel<PartySizeLimitModel>(new StoryModePartySizeLimitModel());
			campaignGameStarter.AddModel<BannerItemModel>(new StoryModeBannerItemModel());
			campaignGameStarter.AddModel<PrisonerRecruitmentCalculationModel>(new StoryModePrisonerRecruitmentCalculationModel());
			campaignGameStarter.AddModel<TroopSupplierProbabilityModel>(new StoryModeTroopSupplierProbabilityModel());
			campaignGameStarter.AddModel<CutsceneSelectionModel>(new StoryModeCutsceneSelectionModel());
			campaignGameStarter.AddModel<VoiceOverModel>(new StoryModeVoiceOverModel());
			campaignGameStarter.AddModel<IncidentModel>(new StoryModeIncidentModel());
		}
	}
}
