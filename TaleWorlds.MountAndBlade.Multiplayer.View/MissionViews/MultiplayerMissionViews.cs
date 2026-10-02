using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews
{
	// Token: 0x02000018 RID: 24
	[ViewCreatorModule]
	public class MultiplayerMissionViews
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002378 File Offset: 0x00000578
		[ViewMethod("MultiplayerTeamDeathmatch")]
		public static MissionView[] OpenTeamDeathmatchMission(Mission mission)
		{
			return new List<MissionView>
			{
				MultiplayerViewCreator.CreateMissionServerStatusUIHandler(),
				MultiplayerViewCreator.CreateMissionMultiplayerPreloadView(mission),
				MultiplayerViewCreator.CreateMultiplayerTeamSelectUIHandler(),
				MultiplayerViewCreator.CreateMissionKillNotificationUIHandler(),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				MultiplayerViewCreator.CreateMissionMultiplayerEscapeMenu("TeamDeathmatch"),
				MultiplayerViewCreator.CreateMissionScoreBoardUIHandler(mission, false),
				MultiplayerViewCreator.CreateMultiplayerEndOfRoundUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerEndOfBattleUIHandler(),
				MultiplayerViewCreator.CreateLobbyEquipmentUIHandler(),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				MultiplayerViewCreator.CreatePollProgressUIHandler(),
				MultiplayerViewCreator.CreateMissionFlagMarkerUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerMissionHUDExtensionUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerMissionDeathCardUIHandler(null),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				MultiplayerViewCreator.CreateMultiplayerAdminPanelUIHandler(),
				ViewCreator.CreateMissionBoundaryCrossingView(),
				new MissionBoundaryWallView(),
				new MissionItemContourControllerView(),
				new MissionAgentContourControllerView(),
				new SpectatorCameraView(),
				new MultiplayerSpectatorSilhouetteView()
			}.ToArray();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000024BC File Offset: 0x000006BC
		[ViewMethod("MultiplayerDuel")]
		public static MissionView[] OpenDuelMission(Mission mission)
		{
			return new List<MissionView>
			{
				MultiplayerViewCreator.CreateMissionServerStatusUIHandler(),
				MultiplayerViewCreator.CreateMissionMultiplayerPreloadView(mission),
				MultiplayerViewCreator.CreateMultiplayerCultureSelectUIHandler(),
				MultiplayerViewCreator.CreateMissionKillNotificationUIHandler(),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				MultiplayerViewCreator.CreateMissionMultiplayerEscapeMenu("Duel"),
				MultiplayerViewCreator.CreateMultiplayerEndOfBattleUIHandler(),
				MultiplayerViewCreator.CreateMissionScoreBoardUIHandler(mission, true),
				MultiplayerViewCreator.CreateLobbyEquipmentUIHandler(),
				MultiplayerViewCreator.CreateMissionMultiplayerDuelUI(),
				MultiplayerViewCreator.CreatePollProgressUIHandler(),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				MultiplayerViewCreator.CreateMultiplayerAdminPanelUIHandler(),
				ViewCreator.CreateMissionBoundaryCrossingView(),
				new MissionBoundaryWallView(),
				new MissionItemContourControllerView(),
				new MissionAgentContourControllerView(),
				new SpectatorCameraView(),
				new MultiplayerSpectatorSilhouetteView()
			}.ToArray();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000025D4 File Offset: 0x000007D4
		[ViewMethod("MultiplayerSiege")]
		public static MissionView[] OpenSiegeMission(Mission mission)
		{
			return new List<MissionView>
			{
				MultiplayerViewCreator.CreateMissionServerStatusUIHandler(),
				MultiplayerViewCreator.CreateMissionMultiplayerPreloadView(mission),
				MultiplayerViewCreator.CreateMissionKillNotificationUIHandler(),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				MultiplayerViewCreator.CreateMissionMultiplayerEscapeMenu("Siege"),
				MultiplayerViewCreator.CreateMultiplayerEndOfBattleUIHandler(),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				MultiplayerViewCreator.CreateMultiplayerTeamSelectUIHandler(),
				MultiplayerViewCreator.CreateMissionScoreBoardUIHandler(mission, false),
				MultiplayerViewCreator.CreateMultiplayerEndOfRoundUIHandler(),
				MultiplayerViewCreator.CreateLobbyEquipmentUIHandler(),
				MultiplayerViewCreator.CreatePollProgressUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerMissionHUDExtensionUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerMissionDeathCardUIHandler(null),
				new MissionItemContourControllerView(),
				new MissionAgentContourControllerView(),
				MultiplayerViewCreator.CreateMissionFlagMarkerUIHandler(),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				MultiplayerViewCreator.CreateMultiplayerAdminPanelUIHandler(),
				ViewCreator.CreateMissionBoundaryCrossingView(),
				new MissionBoundaryWallView(),
				new SpectatorCameraView(),
				new MultiplayerSpectatorSilhouetteView()
			}.ToArray();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002718 File Offset: 0x00000918
		[ViewMethod("MultiplayerBattle")]
		public static MissionView[] OpenBattle(Mission mission)
		{
			return new List<MissionView>
			{
				MultiplayerViewCreator.CreateLobbyEquipmentUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerFactionBanVoteUIHandler(),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				MultiplayerViewCreator.CreateMissionMultiplayerPreloadView(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				MultiplayerViewCreator.CreateMissionMultiplayerEscapeMenu("Battle"),
				MultiplayerViewCreator.CreateMultiplayerMissionOrderUIHandler(mission),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				ViewCreator.CreateOrderTroopPlacerView(null),
				MultiplayerViewCreator.CreateMultiplayerTeamSelectUIHandler(),
				MultiplayerViewCreator.CreateMissionScoreBoardUIHandler(mission, false),
				MultiplayerViewCreator.CreateMultiplayerEndOfRoundUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerEndOfBattleUIHandler(),
				MultiplayerViewCreator.CreatePollProgressUIHandler(),
				new MissionItemContourControllerView(),
				new MissionAgentContourControllerView(),
				MultiplayerViewCreator.CreateMissionKillNotificationUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerMissionHUDExtensionUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerMissionDeathCardUIHandler(null),
				MultiplayerViewCreator.CreateMissionFlagMarkerUIHandler(),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				MultiplayerViewCreator.CreateMultiplayerAdminPanelUIHandler(),
				ViewCreator.CreateMissionBoundaryCrossingView(),
				new MissionBoundaryWallView(),
				new SpectatorCameraView(),
				new MultiplayerSpectatorSilhouetteView()
			}.ToArray();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002874 File Offset: 0x00000A74
		[ViewMethod("MultiplayerCaptain")]
		public static MissionView[] OpenCaptain(Mission mission)
		{
			return new List<MissionView>
			{
				MultiplayerViewCreator.CreateLobbyEquipmentUIHandler(),
				MultiplayerViewCreator.CreateMissionServerStatusUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerFactionBanVoteUIHandler(),
				MultiplayerViewCreator.CreateMissionMultiplayerPreloadView(mission),
				MultiplayerViewCreator.CreateMissionKillNotificationUIHandler(),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				MultiplayerViewCreator.CreateMissionMultiplayerEscapeMenu("Captain"),
				MultiplayerViewCreator.CreateMultiplayerMissionOrderUIHandler(mission),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				ViewCreator.CreateOrderTroopPlacerView(null),
				new MissionFormationTargetSelectionHandler(),
				ViewCreator.CreateMissionFormationMarkerUIHandler(mission),
				MultiplayerViewCreator.CreateMultiplayerTeamSelectUIHandler(),
				MultiplayerViewCreator.CreateMissionScoreBoardUIHandler(mission, false),
				MultiplayerViewCreator.CreateMultiplayerEndOfRoundUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerEndOfBattleUIHandler(),
				MultiplayerViewCreator.CreatePollProgressUIHandler(),
				new MissionItemContourControllerView(),
				new MissionAgentContourControllerView(),
				MultiplayerViewCreator.CreateMultiplayerMissionHUDExtensionUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerMissionDeathCardUIHandler(null),
				MultiplayerViewCreator.CreateMissionFlagMarkerUIHandler(),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				MultiplayerViewCreator.CreateMultiplayerAdminPanelUIHandler(),
				ViewCreator.CreateMissionBoundaryCrossingView(),
				new MissionBoundaryWallView(),
				new SpectatorCameraView(),
				new MultiplayerSpectatorSilhouetteView()
			}.ToArray();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000029F4 File Offset: 0x00000BF4
		[ViewMethod("MultiplayerSkirmish")]
		public static MissionView[] OpenSkirmish(Mission mission)
		{
			return new List<MissionView>
			{
				MultiplayerViewCreator.CreateLobbyEquipmentUIHandler(),
				MultiplayerViewCreator.CreateMissionServerStatusUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerFactionBanVoteUIHandler(),
				MultiplayerViewCreator.CreateMissionKillNotificationUIHandler(),
				ViewCreator.CreateMissionAgentStatusUIHandler(mission),
				MultiplayerViewCreator.CreateMissionMultiplayerPreloadView(mission),
				ViewCreator.CreateMissionMainAgentEquipmentController(mission),
				ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
				MultiplayerViewCreator.CreateMissionMultiplayerEscapeMenu("Skirmish"),
				MultiplayerViewCreator.CreateMultiplayerMissionOrderUIHandler(mission),
				ViewCreator.CreateMissionAgentLabelUIHandler(mission),
				ViewCreator.CreateOrderTroopPlacerView(null),
				MultiplayerViewCreator.CreateMultiplayerTeamSelectUIHandler(),
				MultiplayerViewCreator.CreateMissionScoreBoardUIHandler(mission, false),
				MultiplayerViewCreator.CreateMultiplayerEndOfRoundUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerEndOfBattleUIHandler(),
				MultiplayerViewCreator.CreatePollProgressUIHandler(),
				new MissionItemContourControllerView(),
				new MissionAgentContourControllerView(),
				MultiplayerViewCreator.CreateMultiplayerMissionHUDExtensionUIHandler(),
				MultiplayerViewCreator.CreateMultiplayerMissionDeathCardUIHandler(null),
				MultiplayerViewCreator.CreateMultiplayerMissionVoiceChatUIHandler(),
				MultiplayerViewCreator.CreateMissionFlagMarkerUIHandler(),
				ViewCreator.CreateOptionsUIHandler(),
				ViewCreator.CreateMissionMainAgentEquipDropView(mission),
				MultiplayerViewCreator.CreateMultiplayerAdminPanelUIHandler(),
				ViewCreator.CreateMissionBoundaryCrossingView(),
				new MissionBoundaryWallView(),
				new SpectatorCameraView(),
				new MultiplayerSpectatorSilhouetteView()
			}.ToArray();
		}
	}
}
