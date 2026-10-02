using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;
using TaleWorlds.MountAndBlade.MissionSpawnHandlers;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000261 RID: 609
	[MissionManager]
	public static class BannerlordMissions
	{
		// Token: 0x060022CB RID: 8907 RVA: 0x0007ABC4 File Offset: 0x00078DC4
		private static Type GetSiegeWeaponType(SiegeEngineType siegeWeaponType)
		{
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ladder)
			{
				return typeof(SiegeLadder);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ballista)
			{
				return typeof(Ballista);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.FireBallista)
			{
				return typeof(FireBallista);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ram || siegeWeaponType == DefaultSiegeEngineTypes.ImprovedRam)
			{
				return typeof(BatteringRam);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.SiegeTower || siegeWeaponType == DefaultSiegeEngineTypes.HeavySiegeTower)
			{
				return typeof(SiegeTower);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Onager || siegeWeaponType == DefaultSiegeEngineTypes.Catapult)
			{
				return typeof(Mangonel);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.FireOnager || siegeWeaponType == DefaultSiegeEngineTypes.FireCatapult)
			{
				return typeof(FireMangonel);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Trebuchet || siegeWeaponType == DefaultSiegeEngineTypes.Bricole)
			{
				return typeof(Trebuchet);
			}
			return null;
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x0007AC94 File Offset: 0x00078E94
		private static Dictionary<Type, int> GetSiegeWeaponTypes(Dictionary<SiegeEngineType, int> values)
		{
			Dictionary<Type, int> dictionary = new Dictionary<Type, int>();
			foreach (KeyValuePair<SiegeEngineType, int> keyValuePair in values)
			{
				dictionary.Add(BannerlordMissions.GetSiegeWeaponType(keyValuePair.Key), keyValuePair.Value);
			}
			return dictionary;
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x0007ACFC File Offset: 0x00078EFC
		public static AtmosphereInfo CreateAtmosphereInfoForMission(string seasonId, int timeOfDay)
		{
			int num;
			new Dictionary<string, int>
			{
				{ "spring", 0 },
				{ "summer", 1 },
				{ "fall", 2 },
				{ "winter", 3 }
			}.TryGetValue(seasonId, out num);
			string text;
			new Dictionary<int, string>
			{
				{ 6, "TOD_06_00_SemiCloudy" },
				{ 12, "TOD_12_00_SemiCloudy" },
				{ 15, "TOD_04_00_SemiCloudy" },
				{ 18, "TOD_03_00_SemiCloudy" },
				{ 22, "TOD_01_00_SemiCloudy" }
			}.TryGetValue(timeOfDay, out text);
			return new AtmosphereInfo
			{
				AtmosphereName = text,
				TimeInfo = new TimeInformation
				{
					Season = num
				}
			};
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x0007ADC0 File Offset: 0x00078FC0
		[MissionMethod]
		public static Mission OpenCustomBattleMission(string scene, BasicCharacterObject playerCharacter, CustomBattleCombatant playerParty, CustomBattleCombatant enemyParty, bool isPlayerGeneral, BasicCharacterObject playerSideGeneralCharacter, string sceneLevels = "", string seasonString = "", float timeOfDay = 6f)
		{
			BattleSideEnum playerSide = playerParty.Side;
			bool isPlayerAttacker = playerSide == BattleSideEnum.Attacker;
			IMissionTroopSupplier[] troopSuppliers = new IMissionTroopSupplier[2];
			CustomBattleTroopSupplier customBattleTroopSupplier = new CustomBattleTroopSupplier(playerParty, true, isPlayerGeneral, false, null);
			troopSuppliers[(int)playerParty.Side] = customBattleTroopSupplier;
			CustomBattleTroopSupplier customBattleTroopSupplier2 = new CustomBattleTroopSupplier(enemyParty, false, false, false, null);
			troopSuppliers[(int)enemyParty.Side] = customBattleTroopSupplier2;
			bool isPlayerSergeant = !isPlayerGeneral;
			Mission mission = MissionState.OpenNew("CustomBattle", new MissionInitializerRecord(scene)
			{
				DoNotUseLoadingScreen = false,
				PlayingInCampaignMode = false,
				AtmosphereOnCampaign = BannerlordMissions.CreateAtmosphereInfoForMission(seasonString, (int)timeOfDay),
				SceneLevels = sceneLevels,
				DecalAtlasGroup = 2
			}, (Mission missionController) => new MissionBehavior[]
			{
				new DefaultBattleMissionAgentSpawnLogic(troopSuppliers, playerSide, Mission.BattleSizeType.Battle),
				new BattlePowerCalculationLogic(),
				new CustomBattleAgentLogic(),
				new BannerBearerLogic(),
				new CustomBattleMissionSpawnHandler((!isPlayerAttacker) ? playerParty : enemyParty, isPlayerAttacker ? playerParty : enemyParty),
				new MissionOptionsComponent(),
				new BattleEndLogic(),
				new BattleReinforcementsSpawnController(),
				new MissionCombatantsLogic(null, playerParty, (!isPlayerAttacker) ? playerParty : enemyParty, isPlayerAttacker ? playerParty : enemyParty, Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant),
				new BattleObserverMissionLogic(),
				new AgentHumanAILogic(),
				new AgentVictoryLogic(),
				new MissionAgentPanicHandler(),
				new BattleMissionAgentInteractionLogic(),
				new AgentMoraleInteractionLogic(),
				new AssignPlayerRoleInTeamMissionController(isPlayerGeneral, isPlayerSergeant, false, isPlayerSergeant ? Enumerable.Repeat<string>(playerCharacter.StringId, 1).ToList<string>() : new List<string>()),
				new GeneralsAndCaptainsAssignmentLogic((isPlayerAttacker & isPlayerGeneral) ? playerCharacter.GetName() : ((isPlayerAttacker & isPlayerSergeant) ? playerSideGeneralCharacter.GetName() : null), (!isPlayerAttacker & isPlayerGeneral) ? playerCharacter.GetName() : ((!isPlayerAttacker & isPlayerSergeant) ? playerSideGeneralCharacter.GetName() : null), null, null, true),
				new EquipmentControllerLeaveLogic(),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new MissionBoundaryCrossingHandler(10f),
				new HighlightsController(),
				new BattleHighlightsController(),
				new BattleDeploymentMissionController(isPlayerAttacker),
				new BattleDeploymentHandler(isPlayerAttacker),
				new MissionObjectiveLogic()
			}, true, true);
			mission.SetPlayerCanTakeControlOfAnotherAgentWhenDead();
			return mission;
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x0007AED8 File Offset: 0x000790D8
		[MissionMethod]
		public static Mission OpenSiegeMissionWithDeployment(string scene, BasicCharacterObject playerCharacter, CustomBattleCombatant playerParty, CustomBattleCombatant enemyParty, bool isPlayerGeneral, float[] wallHitPointPercentages, bool hasAnySiegeTower, List<MissionSiegeWeapon> siegeWeaponsOfAttackers, List<MissionSiegeWeapon> siegeWeaponsOfDefenders, bool isPlayerAttacker, int sceneUpgradeLevel = 0, string seasonString = "", bool isSallyOut = false, bool isReliefForceAttack = false, float timeOfDay = 6f)
		{
			string text = ((sceneUpgradeLevel == 1) ? "level_1" : ((sceneUpgradeLevel == 2) ? "level_2" : "level_3"));
			text += " siege";
			BattleSideEnum playerSide = playerParty.Side;
			IMissionTroopSupplier[] troopSuppliers = new IMissionTroopSupplier[2];
			CustomBattleTroopSupplier customBattleTroopSupplier = new CustomBattleTroopSupplier(playerParty, true, isPlayerGeneral, isSallyOut, null);
			troopSuppliers[(int)playerParty.Side] = customBattleTroopSupplier;
			CustomBattleTroopSupplier customBattleTroopSupplier2 = new CustomBattleTroopSupplier(enemyParty, false, false, isSallyOut, null);
			troopSuppliers[(int)enemyParty.Side] = customBattleTroopSupplier2;
			bool isPlayerSergeant = !isPlayerGeneral;
			Mission mission2 = MissionState.OpenNew("CustomSiegeBattle", new MissionInitializerRecord(scene)
			{
				PlayingInCampaignMode = false,
				AtmosphereOnCampaign = BannerlordMissions.CreateAtmosphereInfoForMission(seasonString, (int)timeOfDay),
				SceneLevels = text,
				DecalAtlasGroup = 2
			}, delegate(Mission mission)
			{
				List<MissionBehavior> list = new List<MissionBehavior>();
				list.Add(new BattleSpawnLogic(isSallyOut ? "sally_out_set" : (isReliefForceAttack ? "relief_force_attack_set" : "battle_set")));
				list.Add(new MissionOptionsComponent());
				list.Add(new BattleEndLogic());
				list.Add(new BattleReinforcementsSpawnController());
				list.Add(new MissionCombatantsLogic(null, playerParty, (!isPlayerAttacker) ? playerParty : enemyParty, isPlayerAttacker ? playerParty : enemyParty, (!isSallyOut) ? Mission.MissionTeamAITypeEnum.Siege : Mission.MissionTeamAITypeEnum.SallyOut, isPlayerSergeant));
				list.Add(new SiegeMissionPreparationHandler(isSallyOut, isReliefForceAttack, wallHitPointPercentages, hasAnySiegeTower));
				Mission.BattleSizeType battleSizeType = (isSallyOut ? Mission.BattleSizeType.SallyOut : Mission.BattleSizeType.Siege);
				list.Add(new DefaultBattleMissionAgentSpawnLogic(troopSuppliers, playerSide, battleSizeType));
				list.Add(new BattlePowerCalculationLogic());
				if (isSallyOut)
				{
					list.Add(new CustomSallyOutMissionController((!isPlayerAttacker) ? playerParty : enemyParty, isPlayerAttacker ? playerParty : enemyParty));
				}
				else if (isReliefForceAttack)
				{
					list.Add(new CustomSallyOutMissionController((!isPlayerAttacker) ? playerParty : enemyParty, isPlayerAttacker ? playerParty : enemyParty));
				}
				else
				{
					list.Add(new CustomSiegeMissionSpawnHandler((!isPlayerAttacker) ? playerParty : enemyParty, isPlayerAttacker ? playerParty : enemyParty, false));
				}
				list.Add(new BattleObserverMissionLogic());
				list.Add(new CustomBattleAgentLogic());
				list.Add(new BannerBearerLogic());
				list.Add(new AgentHumanAILogic());
				if (!isSallyOut)
				{
					list.Add(new AmmoSupplyLogic(new List<BattleSideEnum> { BattleSideEnum.Defender }));
				}
				list.Add(new AgentVictoryLogic());
				list.Add(new AssignPlayerRoleInTeamMissionController(isPlayerGeneral, isPlayerSergeant, false, null));
				list.Add(new GeneralsAndCaptainsAssignmentLogic((isPlayerAttacker & isPlayerGeneral) ? playerCharacter.GetName() : null, null, null, null, false));
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

		// Token: 0x060022D0 RID: 8912 RVA: 0x0007B03C File Offset: 0x0007923C
		[MissionMethod]
		public static Mission OpenCustomBattleLordsHallMission(string scene, BasicCharacterObject playerCharacter, CustomBattleCombatant playerParty, CustomBattleCombatant enemyParty, BasicCharacterObject playerSideGeneralCharacter, string sceneLevels = "", int sceneUpgradeLevel = 0, string seasonString = "")
		{
			int remainingDefenderArcherCount = MathF.Round(18.9f);
			BattleSideEnum playerSide = BattleSideEnum.Attacker;
			bool isPlayerAttacker = playerSide == BattleSideEnum.Attacker;
			IMissionTroopSupplier[] troopSuppliers = new IMissionTroopSupplier[2];
			CustomBattleTroopSupplier customBattleTroopSupplier = new CustomBattleTroopSupplier(playerParty, true, playerCharacter == playerSideGeneralCharacter, false, null);
			troopSuppliers[(int)playerParty.Side] = customBattleTroopSupplier;
			CustomBattleTroopSupplier customBattleTroopSupplier2 = new CustomBattleTroopSupplier(enemyParty, false, false, false, delegate(BasicCharacterObject basicCharacterObject)
			{
				bool flag = true;
				if (basicCharacterObject.IsRanged)
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
			troopSuppliers[(int)enemyParty.Side] = customBattleTroopSupplier2;
			return MissionState.OpenNew("CustomBattleLordsHall", new MissionInitializerRecord(scene)
			{
				DoNotUseLoadingScreen = false,
				PlayingInCampaignMode = false,
				SceneLevels = "siege",
				DecalAtlasGroup = 3
			}, (Mission missionController) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new BattleEndLogic(),
				new MissionCombatantsLogic(null, playerParty, (!isPlayerAttacker) ? playerParty : enemyParty, isPlayerAttacker ? playerParty : enemyParty, Mission.MissionTeamAITypeEnum.NoTeamAI, false),
				new BattleMissionStarterLogic(),
				new AgentHumanAILogic(),
				new LordsHallFightMissionController(troopSuppliers, 3f, 0.7f, 19, 27, playerSide),
				new BattleObserverMissionLogic(),
				new CustomBattleAgentLogic(),
				new AgentVictoryLogic(),
				new AmmoSupplyLogic(new List<BattleSideEnum> { BattleSideEnum.Defender }),
				new EquipmentControllerLeaveLogic(),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new MissionBoundaryCrossingHandler(10f),
				new BattleMissionAgentInteractionLogic(),
				new HighlightsController(),
				new BattleHighlightsController()
			}, true, true);
		}

		// Token: 0x04000D84 RID: 3460
		private const string Level1Tag = "level_1";

		// Token: 0x04000D85 RID: 3461
		private const string Level2Tag = "level_2";

		// Token: 0x04000D86 RID: 3462
		private const string Level3Tag = "level_3";

		// Token: 0x04000D87 RID: 3463
		private const string SiegeTag = "siege";

		// Token: 0x04000D88 RID: 3464
		private const string SallyOutTag = "sally";

		// Token: 0x0200054D RID: 1357
		private enum CustomBattleGameTypes
		{
			// Token: 0x04001E1A RID: 7706
			AttackerGeneral,
			// Token: 0x04001E1B RID: 7707
			DefenderGeneral,
			// Token: 0x04001E1C RID: 7708
			AttackerSergeant,
			// Token: 0x04001E1D RID: 7709
			DefenderSergeant
		}
	}
}
