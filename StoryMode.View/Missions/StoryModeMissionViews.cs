using System;
using System.Collections.Generic;
using SandBox.View;
using SandBox.View.Missions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Sound;

namespace StoryMode.View.Missions
{
	// Token: 0x02000007 RID: 7
	[ViewCreatorModule]
	public class StoryModeMissionViews
	{
		// Token: 0x0600001C RID: 28 RVA: 0x000027FC File Offset: 0x000009FC
		[ViewMethod("TrainingField")]
		public static MissionView[] OpenVillageMission(Mission mission)
		{
			return new List<MissionView>
			{
				new MissionCampaignView(),
				new MissionConversationCameraView(),
				SandBoxViewCreator.CreateMissionConversationView(mission),
				ViewCreator.CreateMissionSingleplayerEscapeMenu(CampaignOptions.IsIronmanMode),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				new MissionSingleplayerViewHandler(),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				ViewCreator.CreateMissionAgentLockVisualizerView(mission),
				ViewCreator.CreateMissionBoundaryCrossingView(),
				SandBoxViewCreator.CreateMissionBarterView(),
				ViewCreator.CreateMissionLeaveView(),
				ViewCreator.CreatePhotoModeView(),
				new MissionBoundaryWallView(),
				SandBoxViewCreator.CreateMissionNameMarkerUIHandler(mission),
				new MissionItemContourControllerView(),
				new MissionAgentContourControllerView(),
				StoryModeViewCreator.CreateTrainingFieldObjectiveView(mission),
				new MissionCampaignBattleSpectatorView(),
				ViewCreator.CreateSingleplayerMissionKillNotificationUIHandler()
			}.ToArray();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002914 File Offset: 0x00000B14
		[ViewMethod("SneakIntoTheVillaMission")]
		public static MissionView[] OpenSneakIntoTheVillaMission(Mission mission)
		{
			return new List<MissionView>
			{
				new MissionCampaignView(),
				new MissionConversationCameraView(),
				ViewCreator.CreateMissionSingleplayerEscapeMenu(CampaignOptions.IsIronmanMode),
				ViewCreator.CreateOptionsUIHandler(),
				SandBoxViewCreator.CreateMissionConversationView(mission),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				new MissionSingleplayerViewHandler(),
				new MusicStealthMissionView(),
				new StealthTutorialView(),
				SandBoxViewCreator.CreateMissionStealthFailCounter(null),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionAgentLockVisualizerView(mission),
				ViewCreator.CreateMissionBoundaryCrossingView(),
				new MissionBoundaryWallView(),
				SandBoxViewCreator.CreateMissionNameMarkerUIHandler(mission),
				ViewCreator.CreateSingleplayerMissionKillNotificationUIHandler(),
				ViewCreator.CreateMissionSpectatorControlView(mission),
				SandBoxViewCreator.CreateMissionAgentAlarmStateView(mission),
				new MissionItemContourControllerView(),
				new MissionAgentContourControllerView(),
				new MissionCampaignBattleSpectatorView(),
				ViewCreator.CreatePhotoModeView(),
				ViewCreator.CreateMissionLeaveView()
			}.ToArray();
		}
	}
}
