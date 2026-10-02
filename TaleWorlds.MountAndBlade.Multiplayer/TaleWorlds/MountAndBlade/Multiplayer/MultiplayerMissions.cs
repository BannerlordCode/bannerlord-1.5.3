using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Multiplayer.Missions;
using TaleWorlds.MountAndBlade.Source.Missions;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000063 RID: 99
	[MissionManager]
	public static class MultiplayerMissions
	{
		// Token: 0x060002F2 RID: 754 RVA: 0x0000D895 File Offset: 0x0000BA95
		[MissionMethod]
		public static void OpenTeamDeathmatchMission(string scene)
		{
			MissionState.OpenNew("MultiplayerTeamDeathmatch", new MissionInitializerRecord(scene), delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MissionMultiplayerTeamDeathmatch(),
						new MissionMultiplayerTeamDeathmatchClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new TeamDeathmatchSpawnFrameBehavior(), new TeamDeathmatchSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MultiplayerTeamSelectComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new TDMScoreboardData()),
						new MissionAgentPanicHandler(),
						new AgentHumanAILogic(),
						new EquipmentControllerLeaveLogic(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MissionMultiplayerTeamDeathmatchClient(),
					new MultiplayerAchievementComponent(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MultiplayerTeamSelectComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new TDMScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MissionRecentPlayersComponent(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000D8C9 File Offset: 0x0000BAC9
		[MissionMethod]
		public static void OpenDuelMission(string scene)
		{
			MissionState.OpenNew("MultiplayerDuel", new MissionInitializerRecord(scene), delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MissionMultiplayerDuel(),
						new MissionMultiplayerGameModeDuelClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new DuelSpawnFrameBehavior(), new DuelSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new DuelScoreboardData()),
						new MissionAgentPanicHandler(),
						new AgentHumanAILogic(),
						new EquipmentControllerLeaveLogic(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MissionMultiplayerGameModeDuelClient(),
					new MultiplayerAchievementComponent(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new DuelScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MissionRecentPlayersComponent(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000D900 File Offset: 0x0000BB00
		[MissionMethod]
		public static void OpenSiegeMission(string scene)
		{
			MissionState.OpenNew("MultiplayerSiege", new MissionInitializerRecord(scene)
			{
				SceneUpgradeLevel = 3,
				SceneLevels = ""
			}, delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MissionMultiplayerSiege(),
						new MultiplayerWarmupComponent(),
						new MissionMultiplayerSiegeClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new SiegeSpawnFrameBehavior(), new SiegeSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MultiplayerTeamSelectComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new SiegeScoreboardData()),
						new MissionAgentPanicHandler(),
						new AgentHumanAILogic(),
						new EquipmentControllerLeaveLogic(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MultiplayerWarmupComponent(),
					new MissionMultiplayerSiegeClient(),
					new MultiplayerAchievementComponent(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MultiplayerTeamSelectComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new SiegeScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MissionRecentPlayersComponent(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000D956 File Offset: 0x0000BB56
		[MissionMethod]
		public static void OpenBattleMission(string scene)
		{
			MissionState.OpenNew("MultiplayerBattle", new MissionInitializerRecord(scene), delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MultiplayerRoundController(),
						new MissionMultiplayerFlagDomination(MultiplayerGameType.Battle),
						new MultiplayerWarmupComponent(),
						new MissionMultiplayerGameModeFlagDominationClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new FlagDominationSpawnFrameBehavior(), new FlagDominationSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MultiplayerTeamSelectComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new AgentVictoryLogic(),
						new AgentHumanAILogic(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new BattleScoreboardData()),
						new EquipmentControllerLeaveLogic(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MultiplayerRoundComponent(),
					new MultiplayerWarmupComponent(),
					new MissionMultiplayerGameModeFlagDominationClient(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MultiplayerTeamSelectComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new BattleScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000D98A File Offset: 0x0000BB8A
		[MissionMethod]
		public static void OpenCaptainMission(string scene)
		{
			MissionState.OpenNew("MultiplayerCaptain", new MissionInitializerRecord(scene), delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MissionMultiplayerFlagDomination(MultiplayerGameType.Captain),
						new MultiplayerRoundController(),
						new MultiplayerWarmupComponent(),
						new MissionMultiplayerGameModeFlagDominationClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new FlagDominationSpawnFrameBehavior(), new FlagDominationSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MultiplayerTeamSelectComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new AgentVictoryLogic(),
						new AgentHumanAILogic(),
						new MissionAgentPanicHandler(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new CaptainScoreboardData()),
						new EquipmentControllerLeaveLogic(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MultiplayerAchievementComponent(),
					new MultiplayerWarmupComponent(),
					new MissionMultiplayerGameModeFlagDominationClient(),
					new MultiplayerRoundComponent(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MultiplayerTeamSelectComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new CaptainScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MissionRecentPlayersComponent(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000D9BE File Offset: 0x0000BBBE
		[MissionMethod]
		public static void OpenSkirmishMission(string scene)
		{
			MissionState.OpenNew("MultiplayerSkirmish", new MissionInitializerRecord(scene), delegate(Mission missionController)
			{
				if (GameNetwork.IsServer)
				{
					return new MissionBehavior[]
					{
						MissionLobbyComponent.CreateBehavior(),
						new MissionMultiplayerFlagDomination(MultiplayerGameType.Skirmish),
						new MultiplayerRoundController(),
						new MultiplayerWarmupComponent(),
						new MissionMultiplayerGameModeFlagDominationClient(),
						new MultiplayerTimerComponent(),
						new MultiplayerBattleMissionAgentInteractionLogic(),
						new MultiplayerMissionAgentVisualSpawnComponent(),
						new ConsoleMatchStartEndHandler(),
						new SpawnComponent(new FlagDominationSpawnFrameBehavior(), new FlagDominationSpawningBehavior()),
						new MissionLobbyEquipmentNetworkComponent(),
						new MultiplayerTeamSelectComponent(),
						new MissionHardBorderPlacer(),
						new MissionBoundaryPlacer(),
						new AgentVictoryLogic(),
						new MissionAgentPanicHandler(),
						new AgentHumanAILogic(),
						new MissionBoundaryCrossingHandler(10f),
						new MultiplayerPollComponent(),
						new MultiplayerAdminComponent(),
						new MultiplayerGameNotificationsComponent(),
						new MissionOptionsComponent(),
						new MissionScoreboardComponent(new SkirmishScoreboardData()),
						new EquipmentControllerLeaveLogic(),
						new VoiceChatHandler(),
						new MultiplayerPreloadHelper()
					};
				}
				return new MissionBehavior[]
				{
					MissionLobbyComponent.CreateBehavior(),
					new MultiplayerAchievementComponent(),
					new MultiplayerWarmupComponent(),
					new MissionMultiplayerGameModeFlagDominationClient(),
					new MultiplayerRoundComponent(),
					new MultiplayerTimerComponent(),
					new MultiplayerBattleMissionAgentInteractionLogic(),
					new MultiplayerMissionAgentVisualSpawnComponent(),
					new ConsoleMatchStartEndHandler(),
					new MissionLobbyEquipmentNetworkComponent(),
					new MultiplayerTeamSelectComponent(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new MultiplayerPollComponent(),
					new MultiplayerAdminComponent(),
					new MultiplayerGameNotificationsComponent(),
					new MissionOptionsComponent(),
					new MissionScoreboardComponent(new SkirmishScoreboardData()),
					MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
					new EquipmentControllerLeaveLogic(),
					new MissionRecentPlayersComponent(),
					new VoiceChatHandler(),
					new MultiplayerPreloadHelper()
				};
			}, true, true);
		}
	}
}
