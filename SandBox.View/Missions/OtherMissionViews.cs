using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;

namespace SandBox.View.Missions
{
	// Token: 0x02000027 RID: 39
	[ViewCreatorModule]
	public class OtherMissionViews
	{
		// Token: 0x060000EE RID: 238 RVA: 0x0000AB6C File Offset: 0x00008D6C
		[ViewMethod("BattleChallenge")]
		public static MissionView[] OpenBattleChallengeMission(Mission mission)
		{
			return new List<MissionView>
			{
				ViewCreator.CreateMissionSingleplayerEscapeMenu(CampaignOptions.IsIronmanMode),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				new MissionMessageUIHandler(),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				ViewCreator.CreateMissionAgentLockVisualizerView(mission),
				new MissionSingleplayerViewHandler()
			}.ToArray();
		}
	}
}
