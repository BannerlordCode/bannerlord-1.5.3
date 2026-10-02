using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.Conversation.MissionLogics;
using SandBox.Missions;
using SandBox.Missions.AgentBehaviors;
using SandBox.Missions.MissionEvents;
using SandBox.Missions.MissionLogics;
using SandBox.Missions.MissionLogics.Arena;
using SandBox.Missions.MissionLogics.Hideout;
using SandBox.Missions.MissionLogics.Towns;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.TroopSuppliers;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;
using TaleWorlds.ObjectSystem;

namespace SandBox
{
	// Token: 0x02000021 RID: 33
	[MissionManager]
	public static class SandBoxMissions
	{
		// Token: 0x060000EA RID: 234 RVA: 0x000061A8 File Offset: 0x000043A8
		public static MissionInitializerRecord CreateSandBoxMissionInitializerRecord(string sceneName, string sceneLevels, bool doNotUseLoadingScreen, DecalAtlasGroup decalAtlasGroup)
		{
			CampaignVec2 campaignVec = MobileParty.MainParty.Position;
			if (MobileParty.MainParty.MapEvent != null)
			{
				campaignVec = MobileParty.MainParty.MapEvent.Position;
			}
			return new MissionInitializerRecord(sceneName)
			{
				DamageToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier(),
				DamageFromPlayerToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier(),
				PlayingInCampaignMode = (Campaign.Current.GameMode == CampaignGameMode.Campaign),
				AtmosphereOnCampaign = ((Campaign.Current.GameMode == CampaignGameMode.Campaign) ? Campaign.Current.Models.MapWeatherModel.GetAtmosphereModel(campaignVec) : AtmosphereInfo.GetInvalidAtmosphereInfo()),
				TerrainType = (int)((Campaign.Current.MapSceneWrapper != null) ? Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.CurrentNavigationFace) : ((TerrainType)0)),
				SceneLevels = sceneLevels,
				DoNotUseLoadingScreen = doNotUseLoadingScreen,
				DecalAtlasGroup = (int)decalAtlasGroup
			};
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000062A8 File Offset: 0x000044A8
		public static MissionInitializerRecord CreateSandBoxTrainingMissionInitializerRecord(string sceneName, string sceneLevels = "", bool doNotUseLoadingScreen = false)
		{
			return new MissionInitializerRecord(sceneName)
			{
				DamageToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier(),
				DamageFromPlayerToFriendsMultiplier = 1f,
				PlayingInCampaignMode = (Campaign.Current.GameMode == CampaignGameMode.Campaign),
				AtmosphereOnCampaign = ((Campaign.Current.GameMode == CampaignGameMode.Campaign) ? Campaign.Current.Models.MapWeatherModel.GetAtmosphereModel(MobileParty.MainParty.Position) : AtmosphereInfo.GetInvalidAtmosphereInfo()),
				TerrainType = (int)Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.CurrentNavigationFace),
				SceneLevels = sceneLevels,
				DoNotUseLoadingScreen = doNotUseLoadingScreen,
				DecalAtlasGroup = 3
			};
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000636C File Offset: 0x0000456C
		[MissionMethod]
		public static Mission OpenTownCenterMission(string scene, int townUpgradeLevel, Location location, CharacterObject talkToChar, string playerSpawnTag)
		{
			string civilianUpgradeLevelTag = Campaign.Current.Models.LocationModel.GetCivilianUpgradeLevelTag(townUpgradeLevel);
			return SandBoxMissions.OpenTownCenterMission(scene, civilianUpgradeLevelTag, location, talkToChar, playerSpawnTag);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000639C File Offset: 0x0000459C
		[MissionMethod]
		public static Mission OpenTownCenterMission(string scene, string sceneLevels, Location location, CharacterObject talkToChar, string playerSpawnTag)
		{
			return MissionState.OpenNew("TownCenter", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false, DecalAtlasGroup.Town), (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new MissionBasicTeamLogic(),
				new MissionSettlementPrepareLogic(),
				new TownCenterMissionController(),
				new MissionAgentLookHandler(),
				new SandBoxMissionHandler(),
				new WorkshopMissionHandler(SandBoxMissions.GetCurrentTown()),
				new BasicLeaveMissionLogic(),
				new LeaveMissionLogic("settlement_player_unconscious"),
				new BattleAgentLogic(),
				new MountAgentLogic(),
				new NotableSpawnPointHandler(),
				new MissionAgentPanicHandler(),
				new AgentHumanAILogic(),
				new MissionAlleyHandler(),
				new MissionCrimeHandler(),
				new MissionConversationLogic(talkToChar),
				new MissionAgentHandler(),
				new MissionLocationLogic(location, playerSpawnTag),
				new HeroSkillHandler(),
				new MissionFightHandler(),
				new MissionFacialAnimationHandler(),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new MissionBoundaryCrossingHandler(10f),
				new VisualTrackerMissionBehavior(),
				new EquipmentControllerLeaveLogic()
			}, true, true);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000063E8 File Offset: 0x000045E8
		[MissionMethod]
		public static Mission OpenTownCenterShadowATargetMission(string scene, string sceneLevels, Location location, CharacterObject talkToChar, string playerSpawnTag)
		{
			return MissionState.OpenNew("TownCenter", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false, DecalAtlasGroup.Town), (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new MissionBasicTeamLogic(),
				new MissionSettlementPrepareLogic(),
				new TownCenterMissionController(),
				new MissionAgentLookHandler(),
				new SandBoxMissionHandler(),
				new WorkshopMissionHandler(SandBoxMissions.GetCurrentTown()),
				new BasicLeaveMissionLogic(),
				new LeaveMissionLogic("settlement_player_unconscious"),
				new BattleAgentLogic(),
				new MountAgentLogic(),
				new NotableSpawnPointHandler(),
				new MissionAgentPanicHandler(),
				new AgentHumanAILogic(),
				new MissionAlleyHandler(),
				new MissionCrimeHandler(),
				new MissionConversationLogic(talkToChar),
				new MissionAgentHandler(),
				new HeroSkillHandler(),
				new MissionFightHandler(),
				new MissionFacialAnimationHandler(),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new MissionBoundaryCrossingHandler(10f),
				new VisualTrackerMissionBehavior(),
				new EquipmentControllerLeaveLogic()
			}, true, true);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006424 File Offset: 0x00004624
		[MissionMethod]
		public static Mission OpenCastleCourtyardMission(string scene, int castleUpgradeLevel, Location location, CharacterObject talkToChar)
		{
			string civilianUpgradeLevelTag = Campaign.Current.Models.LocationModel.GetCivilianUpgradeLevelTag(castleUpgradeLevel);
			return SandBoxMissions.OpenCastleCourtyardMission(scene, civilianUpgradeLevelTag, location, talkToChar);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006450 File Offset: 0x00004650
		[MissionMethod]
		public static Mission OpenCastleCourtyardMission(string scene, string sceneLevels, Location location, CharacterObject talkToChar)
		{
			return MissionState.OpenNew("TownCenter", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false, DecalAtlasGroup.Town), delegate(Mission mission)
			{
				List<MissionBehavior> list = new List<MissionBehavior>();
				list.Add(new MissionOptionsComponent());
				list.Add(new CampaignMissionComponent());
				list.Add(new MissionBasicTeamLogic());
				list.Add(new MissionSettlementPrepareLogic());
				list.Add(new TownCenterMissionController());
				list.Add(new MissionAgentLookHandler());
				list.Add(new SandBoxMissionHandler());
				list.Add(new BasicLeaveMissionLogic());
				list.Add(new LeaveMissionLogic("settlement_player_unconscious"));
				list.Add(new BattleAgentLogic());
				list.Add(new MountAgentLogic());
				Settlement currentTown = SandBoxMissions.GetCurrentTown();
				if (currentTown != null)
				{
					list.Add(new WorkshopMissionHandler(currentTown));
				}
				list.Add(new MissionAgentPanicHandler());
				list.Add(new AgentHumanAILogic());
				list.Add(new MissionConversationLogic(talkToChar));
				list.Add(new MissionAgentHandler());
				list.Add(new MissionLocationLogic(location, null));
				list.Add(new HeroSkillHandler());
				list.Add(new MissionFightHandler());
				list.Add(new MissionFacialAnimationHandler());
				list.Add(new MissionHardBorderPlacer());
				list.Add(new MissionBoundaryPlacer());
				list.Add(new EquipmentControllerLeaveLogic());
				list.Add(new MissionBoundaryCrossingHandler(10f));
				list.Add(new VisualTrackerMissionBehavior());
				return list.ToArray();
			}, true, true);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006494 File Offset: 0x00004694
		[MissionMethod]
		public static Mission OpenIndoorMission(string scene, int townUpgradeLevel, Location location, CharacterObject talkToChar)
		{
			string civilianUpgradeLevelTag = Campaign.Current.Models.LocationModel.GetCivilianUpgradeLevelTag(townUpgradeLevel);
			return SandBoxMissions.OpenIndoorMission(scene, location, talkToChar, civilianUpgradeLevelTag);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000064C0 File Offset: 0x000046C0
		[MissionMethod]
		public static Mission OpenIndoorMission(string scene, Location location, CharacterObject talkToChar = null, string sceneLevels = "")
		{
			List<Ship> mainPartyShips = new List<Ship>();
			List<Ship> townLordShips = new List<Ship>();
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsTown && location.StringId == "port")
			{
				mainPartyShips = MobileParty.MainParty.Ships.ToList<Ship>();
				foreach (MobileParty mobileParty in Settlement.CurrentSettlement.Parties)
				{
					townLordShips.AddRange(mobileParty.Ships);
				}
			}
			return MissionState.OpenNew("Indoor", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, true, DecalAtlasGroup.Town), (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new MissionBasicTeamLogic(),
				new BasicLeaveMissionLogic(),
				new LeaveMissionLogic("settlement_player_unconscious"),
				new SandBoxMissionHandler(),
				new MissionAgentLookHandler(),
				new MissionConversationLogic(talkToChar),
				new MissionAgentHandler(),
				new MissionLocationLogic(location, null),
				new HeroSkillHandler(),
				new MissionFightHandler(),
				new BattleAgentLogic(),
				new MountAgentLogic(),
				new AgentHumanAILogic(),
				new MissionCrimeHandler(),
				new MissionFacialAnimationHandler(),
				new LocationItemSpawnHandler(),
				new IndoorMissionController(),
				new VisualTrackerMissionBehavior(),
				new EquipmentControllerLeaveLogic(),
				new BattleSurgeonLogic(),
				new CivilianPortShipSpawnMissionLogic(mainPartyShips, townLordShips)
			}, true, true);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000065AC File Offset: 0x000047AC
		[MissionMethod]
		public static Mission OpenPrisonBreakMission(string scene, Location location, CharacterObject prisonerCharacter)
		{
			MissionInitializerRecord missionInitializerRecord = SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "prison_break", true, DecalAtlasGroup.Town);
			missionInitializerRecord.DisableCorpseFadeOut = true;
			Mission mission2 = MissionState.OpenNew("PrisonBreak", missionInitializerRecord, delegate(Mission mission)
			{
				List<MissionBehavior> list = new List<MissionBehavior>();
				list.Add(new MissionOptionsComponent());
				list.Add(new CampaignMissionComponent());
				list.Add(new MissionBasicTeamLogic());
				list.Add(new BasicLeaveMissionLogic());
				list.Add(new LeaveMissionLogic("settlement_player_unconscious"));
				list.Add(new SandBoxMissionHandler());
				list.Add(new BattleAgentLogic());
				list.Add(new MissionAgentLookHandler());
				list.Add(new MissionAgentHandler());
				list.Add(new StealthAreaMissionLogic());
				list.Add(new MissionLocationLogic(location, "sp_prison_break"));
				list.Add(new HeroSkillHandler());
				list.Add(new AgentHumanAILogic());
				list.Add(new MissionCrimeHandler());
				list.Add(new MissionFacialAnimationHandler());
				list.Add(new LocationItemSpawnHandler());
				list.Add(new MissionObjectiveLogic());
				list.Add(new PrisonBreakMissionController(prisonerCharacter));
				list.Add(new CorpseDraggingMissionLogic());
				list.Add(new StealthFailCounterMissionLogic());
				list.Add(new VisualTrackerMissionBehavior());
				list.Add(new EquipmentControllerLeaveLogic());
				list.Add(new BattleSurgeonLogic());
				list.Add(new StealthPatrolPointMissionLogic());
				bool isDevelopmentMode = Game.Current.IsDevelopmentMode;
				return list.ToArray();
			}, true, true);
			mission2.ForceNoFriendlyFire = true;
			return mission2;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00006604 File Offset: 0x00004804
		[MissionMethod]
		public static Mission OpenVillageMission(string scene, Location location, CharacterObject talkToChar = null, string sceneLevels = null)
		{
			string text = (string.IsNullOrEmpty(sceneLevels) ? "land_raid" : sceneLevels);
			return MissionState.OpenNew("Village", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, text, false, DecalAtlasGroup.Town), (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new MissionBasicTeamLogic(),
				new VillageMissionController(),
				new NotableSpawnPointHandler(),
				new BasicLeaveMissionLogic(),
				new LeaveMissionLogic("settlement_player_unconscious"),
				new MissionAgentLookHandler(),
				new SandBoxMissionHandler(),
				new MissionConversationLogic(talkToChar),
				new MissionFightHandler(),
				new MissionAgentHandler(),
				new MissionLocationLogic(location, null),
				new MissionAlleyHandler(),
				new HeroSkillHandler(),
				new MissionFacialAnimationHandler(),
				new MissionAgentPanicHandler(),
				new BattleAgentLogic(),
				new MountAgentLogic(),
				new AgentHumanAILogic(),
				new MissionCrimeHandler(),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new EquipmentControllerLeaveLogic(),
				new MissionBoundaryCrossingHandler(10f),
				new VisualTrackerMissionBehavior(),
				new BattleSurgeonLogic()
			}, true, true);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006658 File Offset: 0x00004858
		[MissionMethod]
		public static Mission OpenArenaStartMission(string scene, Location location, CharacterObject talkToChar = null, string sceneLevels = "")
		{
			return MissionState.OpenNew("ArenaPracticeFight", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false, DecalAtlasGroup.Town), (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new EquipmentControllerLeaveLogic(),
				new ArenaPracticeFightMissionController(),
				new BasicLeaveMissionLogic(),
				new MissionConversationLogic(talkToChar),
				new HeroSkillHandler(),
				new MissionFacialAnimationHandler(),
				new MissionAgentPanicHandler(),
				new AgentHumanAILogic(),
				new ArenaAgentStateDeciderLogic(),
				new VisualTrackerMissionBehavior(),
				new CampaignMissionComponent(),
				new MissionAgentHandler(),
				new MissionLocationLogic(location, null)
			}, true, true);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000669C File Offset: 0x0000489C
		[MissionMethod]
		public static Mission OpenRetirementMission(string scene, Location location, CharacterObject talkToChar = null, string sceneLevels = null, string unconsciousMenuId = "")
		{
			return MissionState.OpenNew("Retirement", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false, DecalAtlasGroup.Town), (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new MissionBasicTeamLogic(),
				new VillageMissionController(),
				new NotableSpawnPointHandler(),
				new BasicLeaveMissionLogic(),
				new MissionAgentLookHandler(),
				new MissionConversationLogic(talkToChar),
				new MissionFightHandler(),
				new MissionAgentHandler(),
				new MissionLocationLogic(location, null),
				new MissionAlleyHandler(),
				new HeroSkillHandler(),
				new MissionFacialAnimationHandler(),
				new MissionAgentPanicHandler(),
				new MountAgentLogic(),
				new AgentHumanAILogic(),
				new MissionCrimeHandler(),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new EquipmentControllerLeaveLogic(),
				new MissionBoundaryCrossingHandler(10f),
				new VisualTrackerMissionBehavior(),
				new BattleSurgeonLogic(),
				new RetirementMissionLogic(),
				new LeaveMissionLogic(unconsciousMenuId)
			}, true, true);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000066E8 File Offset: 0x000048E8
		[MissionMethod]
		public static Mission OpenArenaDuelMission(string scene, Location location, CharacterObject duelCharacter, bool requireCivilianEquipment, bool spawnBOthSidesWithHorse, Action<CharacterObject> onDuelEnd, float customAgentHealth, string sceneLevels = "")
		{
			return MissionState.OpenNew("ArenaDuelMission", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false, DecalAtlasGroup.Town), (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new ArenaDuelMissionController(duelCharacter, requireCivilianEquipment, spawnBOthSidesWithHorse, onDuelEnd, customAgentHealth),
				new MissionFacialAnimationHandler(),
				new MissionAgentPanicHandler(),
				new AgentHumanAILogic(),
				new ArenaAgentStateDeciderLogic(),
				new VisualTrackerMissionBehavior(),
				new CampaignMissionComponent(),
				new EquipmentControllerLeaveLogic(),
				new MissionAgentHandler(),
				new MissionLocationLogic(location, null)
			}, true, true);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000674C File Offset: 0x0000494C
		[MissionMethod]
		public static Mission OpenArenaDuelMission(string scene, Location location)
		{
			return MissionState.OpenNew("ArenaDuel", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "", false, DecalAtlasGroup.Town), (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new ArenaDuelMissionBehavior(),
				new BasicLeaveMissionLogic(),
				new MissionAgentHandler(),
				new MissionLocationLogic(location, null),
				new HeroSkillHandler(),
				new MissionFacialAnimationHandler(),
				new MissionAgentPanicHandler(),
				new AgentHumanAILogic(),
				new EquipmentControllerLeaveLogic(),
				new ArenaAgentStateDeciderLogic()
			}, true, true);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000678C File Offset: 0x0000498C
		[MissionMethod]
		public static Mission OpenBattleMission(MissionInitializerRecord rec)
		{
			bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
			bool isPlayerInArmy = MobileParty.MainParty.Army != null;
			List<string> heroesOnPlayerSideByPriority = HeroHelper.OrderHeroesOnPlayerSideByPriority(false, false);
			bool isPlayerAttacker = !MobileParty.MainParty.MapEvent.AttackerSide.Parties.Where<MapEventParty>((MapEventParty p) => p.Party == MobileParty.MainParty.Party).IsEmpty<MapEventParty>();
			Mission mission2 = MissionState.OpenNew("Battle", rec, delegate(Mission mission)
			{
				MissionBehavior[] array = new MissionBehavior[29];
				array[0] = SandBoxMissions.CreateCampaignMissionAgentSpawnLogic(Mission.BattleSizeType.Battle, null, null);
				array[1] = new BattlePowerCalculationLogic();
				array[2] = new BattleSpawnLogic("battle_set");
				array[3] = new SandBoxBattleMissionSpawnHandler();
				array[4] = new CampaignMissionComponent();
				array[5] = new BattleAgentLogic();
				array[6] = new MountAgentLogic();
				array[7] = new BannerBearerLogic();
				array[8] = new MissionOptionsComponent();
				array[9] = new BattleEndLogic();
				array[10] = new BattleReinforcementsSpawnController();
				array[11] = new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant);
				array[12] = new BattleObserverMissionLogic();
				array[13] = new AgentHumanAILogic();
				array[14] = new AgentVictoryLogic();
				array[15] = new BattleSurgeonLogic();
				array[16] = new MissionAgentPanicHandler();
				array[17] = new BattleMissionAgentInteractionLogic();
				array[18] = new AgentMoraleInteractionLogic();
				array[19] = new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, heroesOnPlayerSideByPriority);
				int num = 20;
				Hero leaderHero = MapEvent.PlayerMapEvent.AttackerSide.LeaderParty.LeaderHero;
				TextObject textObject = ((leaderHero != null) ? leaderHero.Name : null);
				Hero leaderHero2 = MapEvent.PlayerMapEvent.DefenderSide.LeaderParty.LeaderHero;
				array[num] = new SandboxGeneralsAndCaptainsAssignmentLogic(textObject, (leaderHero2 != null) ? leaderHero2.Name : null, null, null, true);
				array[21] = new EquipmentControllerLeaveLogic();
				array[22] = new MissionHardBorderPlacer();
				array[23] = new MissionBoundaryPlacer();
				array[24] = new MissionBoundaryCrossingHandler(10f);
				array[25] = new HighlightsController();
				array[26] = new BattleHighlightsController();
				array[27] = new BattleDeploymentMissionController(isPlayerAttacker);
				array[28] = new BattleDeploymentHandler(isPlayerAttacker);
				return array;
			}, true, true);
			mission2.SetPlayerCanTakeControlOfAnotherAgentWhenDead();
			return mission2;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000683C File Offset: 0x00004A3C
		[MissionMethod]
		public static Mission OpenCaravanBattleMission(MissionInitializerRecord rec, bool isCaravan)
		{
			bool isPlayerAttacker = !MobileParty.MainParty.MapEvent.AttackerSide.Parties.Where<MapEventParty>((MapEventParty p) => p.Party == MobileParty.MainParty.Party).IsEmpty<MapEventParty>();
			bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
			bool isPlayerInArmy = MobileParty.MainParty.Army != null;
			BattleSideEnum playerSide = (isPlayerAttacker ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
			PartyBase caravanCombatant = MapEvent.PlayerMapEvent.InvolvedParties.FirstOrDefaultQ<PartyBase>((PartyBase ip) => ip.IsMobile && ip.MobileParty.IsCaravan);
			if (caravanCombatant == null)
			{
				caravanCombatant = MapEvent.PlayerMapEvent.InvolvedParties.FirstOrDefaultQ<PartyBase>((PartyBase ip) => ip.IsMobile && ip.Owner != null && ip.Owner.IsMerchant);
			}
			if (caravanCombatant == null)
			{
				caravanCombatant = MapEvent.PlayerMapEvent.InvolvedParties.FirstOrDefaultQ<PartyBase>((PartyBase ip) => ip.IsMobile && ip.MobileParty.IsVillager);
			}
			bool isCaravanCulture = caravanCombatant.Culture.StringId == "aserai" || caravanCombatant.Culture.StringId == "khuzait";
			if (caravanCombatant.Side == BattleSideEnum.Attacker && caravanCombatant != MapEvent.PlayerMapEvent.AttackerSide.LeaderParty)
			{
				caravanCombatant = MapEvent.PlayerMapEvent.AttackerSide.LeaderParty;
			}
			else if (caravanCombatant.Side == BattleSideEnum.Defender && caravanCombatant != MapEvent.PlayerMapEvent.DefenderSide.LeaderParty)
			{
				caravanCombatant = MapEvent.PlayerMapEvent.DefenderSide.LeaderParty;
			}
			return MissionState.OpenNew("Battle", rec, delegate(Mission mission)
			{
				MissionBehavior[] array = new MissionBehavior[31];
				array[0] = new MissionOptionsComponent();
				array[1] = new CampaignMissionComponent();
				array[2] = new BattleEndLogic();
				array[3] = new BattleReinforcementsSpawnController();
				array[4] = new BannerBearerLogic();
				array[5] = new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant);
				array[6] = new BattleSpawnLogic("battle_set");
				array[7] = new AgentHumanAILogic();
				array[8] = SandBoxMissions.CreateCampaignMissionAgentSpawnLogic(Mission.BattleSizeType.Battle, null, null);
				array[9] = new BattlePowerCalculationLogic();
				array[10] = new SandBoxBattleMissionSpawnHandler();
				array[11] = new BattleObserverMissionLogic();
				array[12] = new BattleAgentLogic();
				array[13] = new MountAgentLogic();
				array[14] = new AgentVictoryLogic();
				array[15] = new MissionAgentPanicHandler();
				array[16] = new MissionHardBorderPlacer();
				array[17] = new MissionBoundaryPlacer();
				array[18] = new MissionBoundaryCrossingHandler(10f);
				array[19] = new BattleMissionAgentInteractionLogic();
				array[20] = new AgentMoraleInteractionLogic();
				array[21] = new HighlightsController();
				array[22] = new BattleHighlightsController();
				array[23] = new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, null);
				int num = 24;
				Hero leaderHero = MapEvent.PlayerMapEvent.AttackerSide.LeaderParty.LeaderHero;
				TextObject textObject = ((leaderHero != null) ? leaderHero.Name : null);
				Hero leaderHero2 = MapEvent.PlayerMapEvent.DefenderSide.LeaderParty.LeaderHero;
				array[num] = new SandboxGeneralsAndCaptainsAssignmentLogic(textObject, (leaderHero2 != null) ? leaderHero2.Name : null, null, null, true);
				array[25] = new EquipmentControllerLeaveLogic();
				array[26] = new MissionCaravanOrVillagerTacticsHandler();
				array[27] = new CaravanBattleMissionHandler(caravanCombatant, isCaravanCulture, isCaravan, playerSide);
				array[28] = new BattleDeploymentHandler(isPlayerAttacker);
				array[29] = new BattleDeploymentMissionController(isPlayerAttacker);
				array[30] = new BattleSurgeonLogic();
				return array;
			}, true, true);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006A50 File Offset: 0x00004C50
		[MissionMethod]
		public static Mission OpenAlleyFightMission(MissionInitializerRecord rec, Location location, TroopRoster playerSideTroops, TroopRoster rivalSideTroops)
		{
			return MissionState.OpenNew("AlleyFight", rec, delegate(Mission mission)
			{
				List<MissionBehavior> list = new List<MissionBehavior>();
				list.Add(new MissionOptionsComponent());
				list.Add(new BattleEndLogic());
				list.Add(new AgentHumanAILogic());
				list.Add(new BattlePowerCalculationLogic());
				list.Add(new CampaignMissionComponent());
				list.Add(new AlleyFightMissionHandler(playerSideTroops, rivalSideTroops));
				list.Add(new BattleObserverMissionLogic());
				list.Add(new AgentVictoryLogic());
				list.Add(new MissionHardBorderPlacer());
				list.Add(new MissionAgentHandler());
				list.Add(new MissionLocationLogic(location, null));
				list.Add(new MissionFightHandler());
				list.Add(new MissionBoundaryPlacer());
				list.Add(new MissionBoundaryCrossingHandler(10f));
				list.Add(new BattleMissionAgentInteractionLogic());
				list.Add(new HighlightsController());
				list.Add(new BattleHighlightsController());
				list.Add(new EquipmentControllerLeaveLogic());
				Settlement currentTown = SandBoxMissions.GetCurrentTown();
				if (currentTown != null)
				{
					list.Add(new WorkshopMissionHandler(currentTown));
				}
				return list.ToArray();
			}, true, true);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006A94 File Offset: 0x00004C94
		[MissionMethod]
		public static Mission OpenCombatMissionWithDialogue(MissionInitializerRecord rec, CharacterObject characterToTalkTo)
		{
			return MissionState.OpenNew("CombatWithDialogue", rec, delegate(Mission mission)
			{
				IMissionTroopSupplier[] array = new IMissionTroopSupplier[]
				{
					new PartyGroupTroopSupplier(PlayerEncounter.Battle, BattleSideEnum.Defender, null, null),
					new PartyGroupTroopSupplier(PlayerEncounter.Battle, BattleSideEnum.Attacker, null, null)
				};
				List<MissionBehavior> list = new List<MissionBehavior>();
				list.Add(new MissionOptionsComponent());
				list.Add(new CampaignMissionComponent());
				list.Add(new BattleEndLogic());
				list.Add(new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.NoTeamAI, false));
				list.Add(new BattleSpawnLogic("battle_set"));
				list.Add(new MissionAgentPanicHandler());
				list.Add(new AgentHumanAILogic());
				list.Add(new CombatMissionWithDialogueController(array, characterToTalkTo));
				list.Add(new MissionConversationLogic(null));
				list.Add(new BattleObserverMissionLogic());
				list.Add(new BattleAgentLogic());
				list.Add(new AgentVictoryLogic());
				list.Add(new MissionHardBorderPlacer());
				list.Add(new MissionBoundaryPlacer());
				list.Add(new MissionBoundaryCrossingHandler(10f));
				list.Add(new BattleMissionAgentInteractionLogic());
				list.Add(new HighlightsController());
				list.Add(new BattleHighlightsController());
				list.Add(new EquipmentControllerLeaveLogic());
				list.Add(new BattleSurgeonLogic());
				Settlement currentTown = SandBoxMissions.GetCurrentTown();
				if (currentTown != null)
				{
					list.Add(new WorkshopMissionHandler(currentTown));
				}
				return list.ToArray();
			}, true, true);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006AC8 File Offset: 0x00004CC8
		[MissionMethod]
		public static Mission OpenBattleMissionWhileEnteringSettlement(string scene, int upgradeLevel, int numberOfMaxTroopToBeSpawnedForPlayer, int numberOfMaxTroopToBeSpawnedForOpponent)
		{
			return MissionState.OpenNew("EnteringSettlementBattle", new MissionInitializerRecord(scene)
			{
				PlayingInCampaignMode = (Campaign.Current.GameMode == CampaignGameMode.Campaign),
				AtmosphereOnCampaign = ((Campaign.Current.GameMode == CampaignGameMode.Campaign) ? Campaign.Current.Models.MapWeatherModel.GetAtmosphereModel(MobileParty.MainParty.Position) : AtmosphereInfo.GetInvalidAtmosphereInfo()),
				DecalAtlasGroup = 3,
				SceneLevels = Campaign.Current.Models.LocationModel.GetCivilianUpgradeLevelTag(upgradeLevel)
			}, delegate(Mission mission)
			{
				IMissionTroopSupplier[] array = new IMissionTroopSupplier[]
				{
					new PartyGroupTroopSupplier(PlayerEncounter.Battle, BattleSideEnum.Defender, null, null),
					new PartyGroupTroopSupplier(PlayerEncounter.Battle, BattleSideEnum.Attacker, null, null)
				};
				List<MissionBehavior> list = new List<MissionBehavior>();
				list.Add(new MissionOptionsComponent());
				list.Add(new CampaignMissionComponent());
				list.Add(new BattleEndLogic());
				list.Add(new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.NoTeamAI, false));
				list.Add(new BattleSpawnLogic("battle_set"));
				list.Add(new MissionAgentPanicHandler());
				list.Add(new AgentHumanAILogic());
				list.Add(new BattleObserverMissionLogic());
				list.Add(new WhileEnteringSettlementBattleMissionController(array, numberOfMaxTroopToBeSpawnedForPlayer, numberOfMaxTroopToBeSpawnedForOpponent));
				list.Add(new MissionFightHandler());
				list.Add(new BattleAgentLogic());
				list.Add(new MountAgentLogic());
				list.Add(new AgentVictoryLogic());
				list.Add(new MissionHardBorderPlacer());
				list.Add(new MissionBoundaryPlacer());
				list.Add(new MissionBoundaryCrossingHandler(10f));
				list.Add(new BattleMissionAgentInteractionLogic());
				list.Add(new HighlightsController());
				list.Add(new BattleHighlightsController());
				list.Add(new EquipmentControllerLeaveLogic());
				list.Add(new BattleSurgeonLogic());
				Settlement currentTown = SandBoxMissions.GetCurrentTown();
				if (currentTown != null)
				{
					list.Add(new WorkshopMissionHandler(currentTown));
				}
				return list.ToArray();
			}, true, true);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00006B7B File Offset: 0x00004D7B
		[MissionMethod]
		public static Mission OpenBattleMission(string scene, bool usesTownDecalAtlas, string sceneLevels)
		{
			return SandBoxMissions.OpenBattleMission(SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false, usesTownDecalAtlas ? DecalAtlasGroup.Town : DecalAtlasGroup.Battle));
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006B91 File Offset: 0x00004D91
		[MissionMethod]
		public static Mission OpenAlleyFightMission(string scene, int upgradeLevel, Location location, TroopRoster playerSideTroops, TroopRoster rivalSideTroops)
		{
			return SandBoxMissions.OpenAlleyFightMission(SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, Campaign.Current.Models.LocationModel.GetCivilianUpgradeLevelTag(upgradeLevel), false, DecalAtlasGroup.Town), location, playerSideTroops, rivalSideTroops);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006BB9 File Offset: 0x00004DB9
		[MissionMethod]
		public static Mission OpenCombatMissionWithDialogue(string scene, CharacterObject characterToTalkTo, int upgradeLevel)
		{
			return SandBoxMissions.OpenCombatMissionWithDialogue(SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, Campaign.Current.Models.LocationModel.GetCivilianUpgradeLevelTag(upgradeLevel), false, DecalAtlasGroup.Town), characterToTalkTo);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006BE0 File Offset: 0x00004DE0
		[MissionMethod]
		public static Mission OpenHideoutBattleMission(string scene, FlattenedTroopRoster playerTroops, bool isTutorial)
		{
			List<MobileParty> list = new List<MobileParty>();
			foreach (MapEventParty mapEventParty in MapEvent.PlayerMapEvent.PartiesOnSide(BattleSideEnum.Defender))
			{
				if (mapEventParty.Party.IsMobile)
				{
					list.Add(mapEventParty.Party.MobileParty);
				}
			}
			string text = (isTutorial ? "level_1" : "level_2");
			int firstPhaseEnemySideTroopCount;
			FlattenedTroopRoster banditPriorityList = MapEventHelper.GetPriorityListForHideoutMission(list, out firstPhaseEnemySideTroopCount);
			FlattenedTroopRoster playerPriorityList = playerTroops ?? MobilePartyHelper.GetStrongestAndPriorTroops(MobileParty.MainParty, Campaign.Current.Models.BanditDensityModel.GetMaximumTroopCountForHideoutMission(MobileParty.MainParty, true), true).ToFlattenedRoster();
			int firstPhasePlayerSideTroopCount = playerPriorityList.Count<FlattenedTroopRosterElement>();
			MissionInitializerRecord missionInitializerRecord = SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, text, false, DecalAtlasGroup.Town);
			return MissionState.OpenNew("HideoutBattle", missionInitializerRecord, delegate(Mission mission)
			{
				IMissionTroopSupplier[] array = new IMissionTroopSupplier[]
				{
					new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, banditPriorityList, null),
					new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, playerPriorityList, null)
				};
				return new MissionBehavior[]
				{
					new MissionOptionsComponent(),
					new CampaignMissionComponent(),
					new BattleEndLogic(),
					new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.NoTeamAI, false),
					new AgentHumanAILogic(),
					new HideoutCinematicController(),
					new MissionConversationLogic(),
					new HideoutMissionController(array, PartyBase.MainParty.Side, firstPhaseEnemySideTroopCount, firstPhasePlayerSideTroopCount),
					new BattleObserverMissionLogic(),
					new BattleAgentLogic(),
					new MountAgentLogic(),
					new AgentVictoryLogic(),
					new MissionAgentPanicHandler(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new AgentMoraleInteractionLogic(),
					new HighlightsController(),
					new BattleHighlightsController(),
					new EquipmentControllerLeaveLogic(),
					new BattleSurgeonLogic(),
					new MissionObjectiveLogic()
				};
			}, true, true);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006CEC File Offset: 0x00004EEC
		[MissionMethod]
		public static Mission OpenHideoutAmbushMission(string sceneName, FlattenedTroopRoster playerTroops, Location location)
		{
			FlattenedTroopRoster priorAllyTroops = playerTroops ?? MobilePartyHelper.GetStrongestAndPriorTroops(MobileParty.MainParty, Campaign.Current.Models.BanditDensityModel.GetMaximumTroopCountForHideoutMission(MobileParty.MainParty, false), false).ToFlattenedRoster();
			priorAllyTroops.RemoveIf((FlattenedTroopRosterElement x) => x.Troop.IsPlayerCharacter);
			MissionInitializerRecord missionInitializerRecord = new MissionInitializerRecord(sceneName)
			{
				DamageToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier(),
				DamageFromPlayerToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier(),
				PlayingInCampaignMode = (Campaign.Current.GameMode == CampaignGameMode.Campaign),
				AtmosphereOnCampaign = ((Campaign.Current.GameMode == CampaignGameMode.Campaign) ? Campaign.Current.Models.MapWeatherModel.GetAtmosphereModel(MobileParty.MainParty.Position) : AtmosphereInfo.GetInvalidAtmosphereInfo()),
				TerrainType = (int)((Campaign.Current.MapSceneWrapper != null) ? Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.CurrentNavigationFace) : ((TerrainType)0)),
				SceneLevels = "level_1",
				DoNotUseLoadingScreen = false,
				DisableCorpseFadeOut = true,
				DecalAtlasGroup = 3
			};
			FlattenedTroopRoster banditPriorityList = new FlattenedTroopRoster(4);
			foreach (MapEventParty mapEventParty in MapEvent.PlayerMapEvent.PartiesOnSide(BattleSideEnum.Defender))
			{
				if (mapEventParty.Party.IsMobile)
				{
					banditPriorityList.Add(mapEventParty.Party.MemberRoster.GetTroopRoster());
				}
			}
			int playerTroopCount = priorAllyTroops.Count<FlattenedTroopRosterElement>();
			return MissionState.OpenNew("HideoutAmbushMission", missionInitializerRecord, delegate(Mission mission)
			{
				IMissionTroopSupplier[] array = new IMissionTroopSupplier[]
				{
					new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, banditPriorityList, null),
					new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, priorAllyTroops, null)
				};
				return new MissionBehavior[]
				{
					new MissionOptionsComponent(),
					new CampaignMissionComponent(),
					new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.NoTeamAI, false),
					new AgentHumanAILogic(),
					new StealthPatrolPointMissionLogic(),
					new HideoutAmbushMissionController(array, BattleSideEnum.Attacker, playerTroopCount),
					new BattleEndLogic(),
					new MountAgentLogic(),
					new HideoutAmbushBossFightCinematicController(),
					new MissionConversationLogic(),
					new BattleObserverMissionLogic(),
					new MissionAgentHandler(),
					new BattleAgentLogic(),
					new MissionLocationLogic(location, null),
					new AgentVictoryLogic(),
					new MissionAgentPanicHandler(),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new StealthFailCounterMissionLogic(),
					new HighlightsController(),
					new BattleHighlightsController(),
					new AgentMoraleInteractionLogic(),
					new EquipmentControllerLeaveLogic(),
					new BattleSurgeonLogic(),
					new MissionAIActivationDeactivationEventListenerLogic(),
					new CorpseDraggingMissionLogic(),
					new ShowQuickInformationEventListenerLogic(),
					new VisualTrackerMissionBehavior(),
					new StealthAreaMissionLogic(),
					new MissionObjectiveLogic()
				};
			}, true, true);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00006EEC File Offset: 0x000050EC
		[MissionMethod]
		public static Mission OpenSiegeMissionWithDeployment(string scene, float[] wallHitPointPercentages, bool hasAnySiegeTower, List<MissionSiegeWeapon> siegeWeaponsOfAttackers, List<MissionSiegeWeapon> siegeWeaponsOfDefenders, bool isPlayerAttacker, int sceneUpgradeLevel = 0, bool isSallyOut = false, bool isReliefForceAttack = false)
		{
			string text = Campaign.Current.Models.LocationModel.GetUpgradeLevelTag(sceneUpgradeLevel);
			text += " siege";
			bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
			bool isPlayerInArmy = MobileParty.MainParty.Army != null;
			List<string> heroesOnPlayerSideByPriority = HeroHelper.OrderHeroesOnPlayerSideByPriority(false, false);
			Mission mission2 = MissionState.OpenNew("SiegeMissionWithDeployment", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, text, false, DecalAtlasGroup.Town), delegate(Mission mission)
			{
				List<MissionBehavior> list = new List<MissionBehavior>();
				list.Add(new BattleSpawnLogic(isSallyOut ? "sally_out_set" : (isReliefForceAttack ? "relief_force_attack_set" : "battle_set")));
				list.Add(new MissionOptionsComponent());
				list.Add(new CampaignMissionComponent());
				BattleEndLogic battleEndLogic = new BattleEndLogic();
				if (MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker)
				{
					battleEndLogic.EnableEnemyDefenderPullBack(Campaign.Current.Models.SiegeLordsHallFightModel.DefenderTroopNumberForSuccessfulPullBack);
				}
				list.Add(battleEndLogic);
				list.Add(new BattleReinforcementsSpawnController());
				list.Add(new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), isSallyOut ? Mission.MissionTeamAITypeEnum.SallyOut : Mission.MissionTeamAITypeEnum.Siege, isPlayerSergeant));
				list.Add(new SiegeMissionPreparationHandler(isSallyOut, isReliefForceAttack, wallHitPointPercentages, hasAnySiegeTower));
				list.Add(new CampaignSiegeStateHandler());
				Settlement currentTown = SandBoxMissions.GetCurrentTown();
				if (currentTown != null)
				{
					list.Add(new WorkshopMissionHandler(currentTown));
				}
				Mission.BattleSizeType battleSizeType = Mission.BattleSizeType.Siege;
				if (isSallyOut)
				{
					battleSizeType = Mission.BattleSizeType.SallyOut;
					FlattenedTroopRoster priorityTroopsForSallyOutAmbush = Campaign.Current.Models.SiegeEventModel.GetPriorityTroopsForSallyOutAmbush();
					list.Add(new SandBoxSallyOutMissionController(true));
					list.Add(SandBoxMissions.CreateCampaignMissionAgentSpawnLogic(battleSizeType, priorityTroopsForSallyOutAmbush, null));
				}
				else
				{
					if (isReliefForceAttack)
					{
						list.Add(new SandBoxSallyOutMissionController(false));
					}
					else
					{
						list.Add(new SandBoxSiegeMissionSpawnHandler());
					}
					list.Add(SandBoxMissions.CreateCampaignMissionAgentSpawnLogic(battleSizeType, null, null));
				}
				list.Add(new BattlePowerCalculationLogic());
				list.Add(new BattleObserverMissionLogic());
				list.Add(new BattleAgentLogic());
				list.Add(new BattleSurgeonLogic());
				list.Add(new MountAgentLogic());
				list.Add(new BannerBearerLogic());
				list.Add(new AgentHumanAILogic());
				if (!isSallyOut)
				{
					list.Add(new AmmoSupplyLogic(new List<BattleSideEnum> { BattleSideEnum.Defender }));
				}
				list.Add(new AgentVictoryLogic());
				list.Add(new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, heroesOnPlayerSideByPriority));
				List<MissionBehavior> list2 = list;
				Hero leaderHero = MapEvent.PlayerMapEvent.AttackerSide.LeaderParty.LeaderHero;
				TextObject textObject = ((leaderHero != null) ? leaderHero.Name : null);
				Hero leaderHero2 = MapEvent.PlayerMapEvent.DefenderSide.LeaderParty.LeaderHero;
				list2.Add(new SandboxGeneralsAndCaptainsAssignmentLogic(textObject, (leaderHero2 != null) ? leaderHero2.Name : null, null, null, false));
				list.Add(new MissionAgentPanicHandler());
				list.Add(new MissionBoundaryPlacer());
				list.Add(new MissionBoundaryCrossingHandler(10f));
				list.Add(new AgentMoraleInteractionLogic());
				list.Add(new HighlightsController());
				list.Add(new BattleHighlightsController());
				list.Add(new EquipmentControllerLeaveLogic());
				if (isSallyOut)
				{
					list.Add(new MissionSiegeEnginesLogic(new List<MissionSiegeWeapon>(), siegeWeaponsOfAttackers));
				}
				else
				{
					list.Add(new MissionSiegeEnginesLogic(siegeWeaponsOfDefenders, siegeWeaponsOfAttackers));
				}
				list.Add(new SiegeDeploymentHandler(isPlayerAttacker));
				list.Add(new SiegeDeploymentMissionController(isPlayerAttacker));
				return list.ToArray();
			}, true, true);
			mission2.SetPlayerCanTakeControlOfAnotherAgentWhenDead();
			return mission2;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00006FB4 File Offset: 0x000051B4
		[MissionMethod]
		public static Mission OpenSiegeMissionNoDeployment(string scene, bool isSallyOut = false, bool isReliefForceAttack = false)
		{
			string text = Campaign.Current.Models.LocationModel.GetUpgradeLevelTag(3);
			text += " siege";
			bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
			bool isPlayerInArmy = MobileParty.MainParty.Army != null;
			List<string> heroesOnPlayerSideByPriority = HeroHelper.OrderHeroesOnPlayerSideByPriority(false, false);
			return MissionState.OpenNew("SiegeMissionNoDeployment", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, text, false, DecalAtlasGroup.Town), delegate(Mission mission)
			{
				List<MissionBehavior> list = new List<MissionBehavior>();
				list.Add(new MissionOptionsComponent());
				list.Add(new BattleSpawnLogic(isSallyOut ? "sally_out_set" : (isReliefForceAttack ? "relief_force_attack_set" : "battle_set")));
				list.Add(new CampaignMissionComponent());
				BattleEndLogic battleEndLogic = new BattleEndLogic();
				if (!isSallyOut && !isReliefForceAttack && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker)
				{
					battleEndLogic.EnableEnemyDefenderPullBack(Campaign.Current.Models.SiegeLordsHallFightModel.DefenderTroopNumberForSuccessfulPullBack);
				}
				list.Add(battleEndLogic);
				list.Add(new BattleReinforcementsSpawnController());
				list.Add(new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant));
				list.Add(new CampaignSiegeStateHandler());
				Mission.BattleSizeType battleSizeType = (isSallyOut ? Mission.BattleSizeType.SallyOut : Mission.BattleSizeType.Siege);
				list.Add(SandBoxMissions.CreateCampaignMissionAgentSpawnLogic(battleSizeType, null, null));
				list.Add(new BattlePowerCalculationLogic());
				list.Add(new SandBoxBattleMissionSpawnHandler());
				Settlement currentTown = SandBoxMissions.GetCurrentTown();
				if (currentTown != null)
				{
					list.Add(new WorkshopMissionHandler(currentTown));
				}
				list.Add(new BattleObserverMissionLogic());
				list.Add(new BattleAgentLogic());
				list.Add(new BattleSurgeonLogic());
				list.Add(new MountAgentLogic());
				list.Add(new AgentVictoryLogic());
				list.Add(new AmmoSupplyLogic(new List<BattleSideEnum> { BattleSideEnum.Defender }));
				list.Add(new MissionAgentPanicHandler());
				list.Add(new MissionHardBorderPlacer());
				list.Add(new MissionBoundaryPlacer());
				list.Add(new EquipmentControllerLeaveLogic());
				list.Add(new MissionBoundaryCrossingHandler(10f));
				list.Add(new AgentHumanAILogic());
				list.Add(new AgentMoraleInteractionLogic());
				list.Add(new HighlightsController());
				list.Add(new BattleHighlightsController());
				list.Add(new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, heroesOnPlayerSideByPriority));
				List<MissionBehavior> list2 = list;
				Hero leaderHero = MapEvent.PlayerMapEvent.AttackerSide.LeaderParty.LeaderHero;
				TextObject textObject = ((leaderHero != null) ? leaderHero.Name : null);
				Hero leaderHero2 = MapEvent.PlayerMapEvent.DefenderSide.LeaderParty.LeaderHero;
				list2.Add(new SandboxGeneralsAndCaptainsAssignmentLogic(textObject, (leaderHero2 != null) ? leaderHero2.Name : null, null, null, false));
				return list.ToArray();
			}, true, true);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00007050 File Offset: 0x00005250
		[MissionMethod]
		public static Mission OpenSiegeLordsHallFightMission(string scene, FlattenedTroopRoster attackerPriorityList)
		{
			int remainingDefenderArcherCount = Campaign.Current.Models.SiegeLordsHallFightModel.MaxDefenderArcherCount;
			FlattenedTroopRoster defenderPriorityList = Campaign.Current.Models.SiegeLordsHallFightModel.GetPriorityListForLordsHallFightMission(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, Campaign.Current.Models.SiegeLordsHallFightModel.MaxDefenderSideTroopCount);
			int attackerSideTroopCountMax = MathF.Min(Campaign.Current.Models.SiegeLordsHallFightModel.MaxAttackerSideTroopCount, attackerPriorityList.Troops.Count<CharacterObject>());
			int defenderSideTroopCountMax = MathF.Min(Campaign.Current.Models.SiegeLordsHallFightModel.MaxDefenderSideTroopCount, defenderPriorityList.Troops.Count<CharacterObject>());
			MissionInitializerRecord missionInitializerRecord = SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "siege", false, DecalAtlasGroup.Town);
			Func<UniqueTroopDescriptor, MapEventParty, bool> <>9__1;
			return MissionState.OpenNew("SiegeLordsHallFightMission", missionInitializerRecord, delegate(Mission mission)
			{
				IMissionTroopSupplier[] array = new IMissionTroopSupplier[2];
				IMissionTroopSupplier[] array2 = array;
				int num = 0;
				MapEvent playerMapEvent = MapEvent.PlayerMapEvent;
				BattleSideEnum battleSideEnum = BattleSideEnum.Defender;
				FlattenedTroopRoster defenderPriorityList2 = defenderPriorityList;
				Func<UniqueTroopDescriptor, MapEventParty, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = delegate(UniqueTroopDescriptor uniqueTroopDescriptor, MapEventParty mapEventParty)
					{
						bool flag = true;
						if (mapEventParty.GetTroop(uniqueTroopDescriptor).IsRanged)
						{
							if (remainingDefenderArcherCount > 0)
							{
								int remainingDefenderArcherCount2 = remainingDefenderArcherCount;
								remainingDefenderArcherCount = remainingDefenderArcherCount2 - 1;
							}
							else
							{
								flag = false;
							}
						}
						return flag;
					});
				}
				array2[num] = new PartyGroupTroopSupplier(playerMapEvent, battleSideEnum, defenderPriorityList2, func);
				array[1] = new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, attackerPriorityList, null);
				return new MissionBehavior[]
				{
					new MissionOptionsComponent(),
					new CampaignMissionComponent(),
					new BattleEndLogic(),
					new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.NoTeamAI, false),
					new CampaignSiegeStateHandler(),
					new AgentHumanAILogic(),
					new LordsHallFightMissionController(array, Campaign.Current.Models.SiegeLordsHallFightModel.AreaLostRatio, Campaign.Current.Models.SiegeLordsHallFightModel.AttackerDefenderTroopCountRatio, attackerSideTroopCountMax, defenderSideTroopCountMax, PartyBase.MainParty.Side),
					new BattleObserverMissionLogic(),
					new BattleAgentLogic(),
					new AgentVictoryLogic(),
					new AmmoSupplyLogic(new List<BattleSideEnum> { BattleSideEnum.Defender }),
					new MissionHardBorderPlacer(),
					new MissionBoundaryPlacer(),
					new MissionBoundaryCrossingHandler(10f),
					new EquipmentControllerLeaveLogic(),
					new BattleMissionAgentInteractionLogic(),
					new HighlightsController(),
					new BattleHighlightsController(),
					new BattleSurgeonLogic()
				};
			}, true, true);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00007140 File Offset: 0x00005340
		[MissionMethod]
		public static Mission OpenConversationMission(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData, string specialScene = "", string sceneLevels = "", bool isMultiAgentConversation = false)
		{
			PartyBase party = conversationPartnerData.Party;
			CampaignVec2 campaignVec = ((party != null) ? party.Position : PartyBase.MainParty.Position);
			string text = (specialScene.IsEmpty<char>() ? Campaign.Current.Models.SceneModel.GetConversationSceneForMapPosition(campaignVec) : specialScene);
			return MissionState.OpenNew("Conversation", SandBoxMissions.CreateSandBoxMissionInitializerRecord(text, sceneLevels, true, DecalAtlasGroup.Town), (Mission mission) => new MissionBehavior[]
			{
				new CampaignMissionComponent(),
				new MissionConversationLogic(),
				new MissionOptionsComponent(),
				new ConversationMissionLogic(playerCharacterData, conversationPartnerData, isMultiAgentConversation),
				new EquipmentControllerLeaveLogic()
			}, true, false);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000071CC File Offset: 0x000053CC
		[MissionMethod]
		public static Mission OpenMeetingMission(string scene, CharacterObject character)
		{
			Debug.FailedAssert("This mission was broken", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\SandBoxMissions.cs", "OpenMeetingMission", 1339);
			return MissionState.OpenNew("Conversation", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "", false, DecalAtlasGroup.Town), (Mission mission) => new MissionBehavior[]
			{
				new CampaignMissionComponent(),
				new MissionSettlementPrepareLogic(),
				new MissionOptionsComponent(),
				new MissionConversationLogic(),
				new EquipmentControllerLeaveLogic()
			}, true, false);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000722C File Offset: 0x0000542C
		private static Settlement GetCurrentTown()
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsTown)
			{
				return Settlement.CurrentSettlement;
			}
			if (MapEvent.PlayerMapEvent != null && MapEvent.PlayerMapEvent.MapEventSettlement != null && MapEvent.PlayerMapEvent.MapEventSettlement.IsTown)
			{
				return MapEvent.PlayerMapEvent.MapEventSettlement;
			}
			return null;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00007282 File Offset: 0x00005482
		private static DefaultBattleMissionAgentSpawnLogic CreateCampaignMissionAgentSpawnLogic(Mission.BattleSizeType battleSizeType, FlattenedTroopRoster priorTroopsForDefenders = null, FlattenedTroopRoster priorTroopsForAttackers = null)
		{
			return new DefaultBattleMissionAgentSpawnLogic(new IMissionTroopSupplier[]
			{
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, priorTroopsForDefenders, null),
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, priorTroopsForAttackers, null)
			}, PartyBase.MainParty.Side, battleSizeType);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000072BC File Offset: 0x000054BC
		[MissionMethod]
		public static Mission OpenDisguiseMission(string scene, bool willSetUpContact, Location fromLocation, string sceneLevels = null)
		{
			CharacterObject defaultContractorCharacter = MBObjectManager.Instance.GetObject<CharacterObject>("disguise_contractor_character");
			MissionInitializerRecord missionInitializerRecord = SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false, DecalAtlasGroup.Town);
			return MissionState.OpenNew("DisguiseMission", missionInitializerRecord, (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new MissionBasicTeamLogic(),
				new MissionSettlementPrepareLogic(),
				new BasicLeaveMissionLogic(),
				new BattleAgentLogic(),
				new MountAgentLogic(),
				new MissionAgentPanicHandler(),
				new AgentHumanAILogic(),
				new MissionCrimeHandler(),
				new MissionConversationLogic(),
				new HeroSkillHandler(),
				new MissionFightHandler(),
				new MissionFacialAnimationHandler(),
				new MissionHardBorderPlacer(),
				new CheckpointMissionLogic(),
				new NotableSpawnPointHandler(),
				new MissionBoundaryPlacer(),
				new MissionBoundaryCrossingHandler(10f),
				new VisualTrackerMissionBehavior(),
				new EquipmentControllerLeaveLogic(),
				new MissionAIActivationDeactivationEventListenerLogic(),
				new StealthPatrolPointMissionLogic(),
				new MissionAlleyHandler(),
				new MissionAgentHandler(),
				new DisguiseMissionLogic(defaultContractorCharacter, fromLocation, willSetUpContact),
				new ShowQuickInformationEventListenerLogic()
			}, true, true);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00007318 File Offset: 0x00005518
		[MissionMethod(UsableByEditor = true)]
		public static Mission OpenSimpleMountedPlayerMission(string scene, string sceneLevels)
		{
			return MissionState.OpenNew("SimpleMountedPlayer", new MissionInitializerRecord(scene)
			{
				PlayingInCampaignMode = (Campaign.Current.GameMode == CampaignGameMode.Campaign),
				AtmosphereOnCampaign = ((Campaign.Current.GameMode == CampaignGameMode.Campaign) ? Campaign.Current.Models.MapWeatherModel.GetAtmosphereModel(MobileParty.MainParty.Position) : AtmosphereInfo.GetInvalidAtmosphereInfo()),
				SceneLevels = sceneLevels
			}, (Mission missionController) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new SimpleMountedPlayerMissionController(),
				new EquipmentControllerLeaveLogic()
			}, true, true);
		}
	}
}
