using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle;
using TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000024 RID: 36
	public static class ViewCreator
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x0000788C File Offset: 0x00005A8C
		public static ScreenBase CreateCreditsScreen()
		{
			return ViewCreatorManager.CreateScreenView<CreditsScreen>();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007893 File Offset: 0x00005A93
		public static ScreenBase CreateOptionsScreen(bool fromMainMenu)
		{
			return ViewCreatorManager.CreateScreenView<OptionsScreen>(new object[] { fromMainMenu });
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000078A9 File Offset: 0x00005AA9
		public static ScreenBase CreateMBFaceGeneratorScreen(BasicCharacterObject character, bool openedFromMultiplayer = false, IFaceGeneratorCustomFilter filter = null)
		{
			return ViewCreatorManager.CreateScreenView<FaceGeneratorScreen>(new object[] { character, openedFromMultiplayer, filter });
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000078C7 File Offset: 0x00005AC7
		public static MissionView CreateMissionAgentStatusUIHandler(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionAgentStatusUIHandler>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000078D8 File Offset: 0x00005AD8
		public static MissionView CreateMissionMainAgentEquipDropView(Mission mission)
		{
			return ViewCreatorManager.CreateMissionView<MissionMainAgentEquipDropView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000078E9 File Offset: 0x00005AE9
		public static MissionView CreateMissionSiegeEngineMarkerView(Mission mission)
		{
			return ViewCreatorManager.CreateMissionView<MissionSiegeEngineMarkerView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000078FA File Offset: 0x00005AFA
		public static MissionView CreateMissionMainAgentEquipmentController(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionMainAgentEquipmentControllerView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000790B File Offset: 0x00005B0B
		public static MissionView CreateMissionMainAgentCheerBarkControllerView(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionMainAgentCheerBarkControllerView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000791C File Offset: 0x00005B1C
		public static MissionView CreateMissionAgentLockVisualizerView(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionAgentLockVisualizerView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000792D File Offset: 0x00005B2D
		public static MissionView CreateOptionsUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionOptionsUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000793B File Offset: 0x00005B3B
		public static MissionView CreateSingleplayerMissionKillNotificationUIHandler()
		{
			return ViewCreatorManager.CreateMissionView<MissionSingleplayerKillNotificationUIHandler>(false, null, Array.Empty<object>());
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007949 File Offset: 0x00005B49
		public static MissionView CreateMissionAgentLabelUIHandler(Mission mission)
		{
			return ViewCreatorManager.CreateMissionView<MissionAgentLabelView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000795A File Offset: 0x00005B5A
		public static MissionView CreateMissionOrderUIHandler(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionOrderUIHandler>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000796B File Offset: 0x00005B6B
		public static MissionView CreateMissionOrderOfBattleUIHandler(Mission mission, OrderOfBattleVM dataSource)
		{
			return ViewCreatorManager.CreateMissionView<MissionOrderOfBattleUIHandler>(false, mission, new object[] { dataSource });
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000797E File Offset: 0x00005B7E
		public static MissionView CreateMissionSpectatorControlView(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionSpectatorControlView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000798F File Offset: 0x00005B8F
		public static MissionView CreateMissionBattleScoreUIHandler(Mission mission, ScoreboardBaseVM dataSource)
		{
			return ViewCreatorManager.CreateMissionView<MissionBattleScoreUIHandler>(false, mission, new object[] { dataSource });
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000079A2 File Offset: 0x00005BA2
		public static MissionView CreateMissionBoundaryCrossingView()
		{
			return ViewCreatorManager.CreateMissionView<MissionBoundaryCrossingView>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000079B0 File Offset: 0x00005BB0
		public static MissionView CreateMissionLeaveView()
		{
			return ViewCreatorManager.CreateMissionView<MissionLeaveView>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000079BE File Offset: 0x00005BBE
		public static MissionView CreatePhotoModeView()
		{
			return ViewCreatorManager.CreateMissionView<PhotoModeView>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000079CC File Offset: 0x00005BCC
		public static MissionView CreateMissionSingleplayerEscapeMenu(bool isIronmanMode)
		{
			return ViewCreatorManager.CreateMissionView<MissionSingleplayerEscapeMenu>(false, null, new object[] { isIronmanMode });
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000079E4 File Offset: 0x00005BE4
		public static MissionView CreateOrderTroopPlacerView(OrderController orderController)
		{
			return ViewCreatorManager.CreateMissionViewWithArgs<OrderTroopPlacer>(new object[] { orderController });
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000079F5 File Offset: 0x00005BF5
		public static MissionView CreateMissionFormationMarkerUIHandler(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionFormationMarkerUIHandler>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00007A06 File Offset: 0x00005C06
		public static MissionView CreateMissionReplayView(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<ReplayMissionView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00007A17 File Offset: 0x00005C17
		public static MissionView CreateMissionHintView(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionHintView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00007A28 File Offset: 0x00005C28
		public static MissionView CreateMissionObjectiveView(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionObjectiveView>(mission != null, mission, Array.Empty<object>());
		}
	}
}
