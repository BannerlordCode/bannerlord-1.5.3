using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade.Missions.BattleScore;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle;
using TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard;

namespace TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews
{
	// Token: 0x0200001A RID: 26
	[ViewCreatorModule]
	public static class MultiplayerPracticeMissionViews
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002B78 File Offset: 0x00000D78
		[ViewMethod("MultiplayerPractice")]
		public static MissionView[] OpenMultiplayerPracticeMission(Mission mission)
		{
			List<MissionView> list = new List<MissionView>();
			list.Add(MultiplayerViewCreator.CreateMissionMultiplayerPracticeEscapeMenu());
			list.Add(ViewCreator.CreateMissionAgentLabelUIHandler(mission));
			list.Add(ViewCreator.CreateMissionBattleScoreUIHandler(mission, new CustomBattleScoreboardVM(new CustomBattleScoreContext(mission))));
			list.Add(ViewCreator.CreateOptionsUIHandler());
			list.Add(ViewCreator.CreateMissionMainAgentEquipDropView(mission));
			MissionView missionView = ViewCreator.CreateMissionOrderUIHandler(null);
			list.Add(missionView);
			list.Add(new OrderTroopPlacer(null));
			list.Add(ViewCreator.CreateMissionAgentStatusUIHandler(mission));
			list.Add(ViewCreator.CreateMissionMainAgentEquipmentController(mission));
			list.Add(ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission));
			list.Add(ViewCreator.CreateMissionAgentLockVisualizerView(mission));
			list.Add(new DeploymentMissionView());
			list.Add(ViewCreator.CreateMissionBoundaryCrossingView());
			list.Add(new MissionBoundaryWallView());
			list.Add(new MissionDeploymentBoundaryMarker("swallowtail_banner", 2f));
			list.Add(ViewCreator.CreateMissionFormationMarkerUIHandler(mission));
			list.Add(ViewCreator.CreateSingleplayerMissionKillNotificationUIHandler());
			list.Add(ViewCreator.CreateMissionSpectatorControlView(mission));
			list.Add(ViewCreator.CreatePhotoModeView());
			list.Add(new MissionItemContourControllerView());
			list.Add(new MissionAgentContourControllerView());
			list.Add(new MissionCustomBattlePreloadView());
			list.Add(ViewCreator.CreateMissionOrderOfBattleUIHandler(mission, new OrderOfBattleVM()));
			return list.ToArray();
		}
	}
}
