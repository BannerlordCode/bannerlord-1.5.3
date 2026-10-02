using System;
using SandBox;
using SandBox.Conversation.MissionLogics;
using SandBox.Missions;
using SandBox.Missions.MissionEvents;
using SandBox.Missions.MissionLogics;
using Storymode.Missions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;

namespace StoryMode.Missions
{
	// Token: 0x02000039 RID: 57
	[MissionManager]
	public static class StoryModeMissions
	{
		// Token: 0x060003AE RID: 942 RVA: 0x00013B54 File Offset: 0x00011D54
		[MissionMethod]
		public static Mission OpenTrainingFieldMission(string scene, Location location, CharacterObject talkToChar = null, string sceneLevels = null)
		{
			return MissionState.OpenNew("TrainingField", SandBoxMissions.CreateSandBoxTrainingMissionInitializerRecord(scene, sceneLevels, false), (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new MissionBasicTeamLogic(),
				new TrainingFieldMissionController(),
				new BasicLeaveMissionLogic(),
				new LeaveMissionLogic("settlement_player_unconscious"),
				new MissionAgentLookHandler(),
				new SandBoxMissionHandler(),
				new MissionConversationLogic(talkToChar),
				new MissionFightHandler(),
				new MissionAgentHandler(),
				new MissionAlleyHandler(),
				new HeroSkillHandler(),
				new MissionFacialAnimationHandler(),
				new MissionAgentPanicHandler(),
				new BattleAgentLogic(),
				new AgentHumanAILogic(),
				new MissionCrimeHandler(),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new MissionBoundaryCrossingHandler(10f),
				new VisualTrackerMissionBehavior(),
				new EquipmentControllerLeaveLogic()
			}, true, true);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00013B90 File Offset: 0x00011D90
		[MissionMethod]
		public static Mission OpenSneakIntoTheVillaMission(string scene, CampaignTime overridenCt, string sceneLevels = null)
		{
			MissionInitializerRecord missionInitializerRecord = new MissionInitializerRecord(scene)
			{
				DamageToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier(),
				DamageFromPlayerToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier(),
				PlayingInCampaignMode = (Campaign.Current.GameMode == CampaignGameMode.Campaign),
				AtmosphereOnCampaign = ((Campaign.Current.GameMode == CampaignGameMode.Campaign) ? Campaign.Current.Models.MapWeatherModel.GetAtmosphereModel(MobileParty.MainParty.Position) : AtmosphereInfo.GetInvalidAtmosphereInfo()),
				TerrainType = (int)((Campaign.Current.MapSceneWrapper != null) ? Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.CurrentNavigationFace) : ((TerrainType)0)),
				SceneLevels = sceneLevels,
				DoNotUseLoadingScreen = false,
				DisableCorpseFadeOut = true,
				DecalAtlasGroup = 3
			};
			return MissionState.OpenNew("SneakIntoTheVillaMission", missionInitializerRecord, (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new AgentHumanAILogic(),
				new MissionBasicTeamLogic(),
				new StealthPatrolPointMissionLogic(),
				new MissionAgentHandler(),
				new SneakIntoTheVillaMissionController(),
				new MissionConversationLogic(),
				new BattleAgentLogic(),
				new MountAgentLogic(),
				new AgentVictoryLogic(),
				new MissionAgentPanicHandler(),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new MissionBoundaryCrossingHandler(10f),
				new HighlightsController(),
				new BattleHighlightsController(),
				new EquipmentControllerLeaveLogic(),
				new BattleSurgeonLogic(),
				new StealthFailCounterMissionLogic(),
				new MissionAIActivationDeactivationEventListenerLogic(),
				new CorpseDraggingMissionLogic(),
				new ShowQuickInformationEventListenerLogic()
			}, true, true);
		}
	}
}
