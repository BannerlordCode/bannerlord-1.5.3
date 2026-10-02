using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox
{
	// Token: 0x0200001F RID: 31
	public class CampaignMissionManager : CampaignMission.ICampaignMissionManager
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00006070 File Offset: 0x00004270
		IMission CampaignMission.ICampaignMissionManager.OpenSiegeMissionWithDeployment(string scene, float[] wallHitPointsPercentages, bool hasAnySiegeTower, List<MissionSiegeWeapon> siegeWeaponsOfAttackers, List<MissionSiegeWeapon> siegeWeaponsOfDefenders, bool isPlayerAttacker, int upgradeLevel, bool isSallyOut, bool isReliefForceAttack)
		{
			return SandBoxMissions.OpenSiegeMissionWithDeployment(scene, wallHitPointsPercentages, hasAnySiegeTower, siegeWeaponsOfAttackers, siegeWeaponsOfDefenders, isPlayerAttacker, upgradeLevel, isSallyOut, isReliefForceAttack);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006091 File Offset: 0x00004291
		IMission CampaignMission.ICampaignMissionManager.OpenSiegeMissionNoDeployment(string scene, bool isSallyOut, bool isReliefForceAttack)
		{
			return SandBoxMissions.OpenSiegeMissionNoDeployment(scene, isSallyOut, isReliefForceAttack);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000609B File Offset: 0x0000429B
		IMission CampaignMission.ICampaignMissionManager.OpenSiegeLordsHallFightMission(string scene, FlattenedTroopRoster attackerPriorityList)
		{
			return SandBoxMissions.OpenSiegeLordsHallFightMission(scene, attackerPriorityList);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000060A4 File Offset: 0x000042A4
		IMission CampaignMission.ICampaignMissionManager.OpenBattleMission(MissionInitializerRecord rec)
		{
			return SandBoxMissions.OpenBattleMission(rec);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000060AC File Offset: 0x000042AC
		IMission CampaignMission.ICampaignMissionManager.OpenCaravanBattleMission(MissionInitializerRecord rec, bool isCaravan)
		{
			return SandBoxMissions.OpenCaravanBattleMission(rec, isCaravan);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000060B5 File Offset: 0x000042B5
		IMission CampaignMission.ICampaignMissionManager.OpenBattleMission(string scene, bool usesTownDecalAtlas, string sceneLevels)
		{
			return SandBoxMissions.OpenBattleMission(scene, usesTownDecalAtlas, sceneLevels);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000060BF File Offset: 0x000042BF
		IMission CampaignMission.ICampaignMissionManager.OpenAlleyFightMission(string scene, int upgradeLevel, Location location, TroopRoster playerSideTroops, TroopRoster rivalSideTroops)
		{
			return SandBoxMissions.OpenAlleyFightMission(scene, upgradeLevel, location, playerSideTroops, rivalSideTroops);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000060CD File Offset: 0x000042CD
		IMission CampaignMission.ICampaignMissionManager.OpenCombatMissionWithDialogue(string scene, CharacterObject characterToTalkTo, int upgradeLevel)
		{
			return SandBoxMissions.OpenCombatMissionWithDialogue(scene, characterToTalkTo, upgradeLevel);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000060D7 File Offset: 0x000042D7
		IMission CampaignMission.ICampaignMissionManager.OpenBattleMissionWhileEnteringSettlement(string scene, int upgradeLevel, int numberOfMaxTroopToBeSpawnedForPlayer, int numberOfMaxTroopToBeSpawnedForOpponent)
		{
			return SandBoxMissions.OpenBattleMissionWhileEnteringSettlement(scene, upgradeLevel, numberOfMaxTroopToBeSpawnedForPlayer, numberOfMaxTroopToBeSpawnedForOpponent);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000060E3 File Offset: 0x000042E3
		IMission CampaignMission.ICampaignMissionManager.OpenHideoutBattleMission(string scene, FlattenedTroopRoster playerTroops, bool isTutorial)
		{
			return SandBoxMissions.OpenHideoutBattleMission(scene, playerTroops, isTutorial);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000060ED File Offset: 0x000042ED
		IMission CampaignMission.ICampaignMissionManager.OpenTownCenterMission(string scene, int townUpgradeLevel, Location location, CharacterObject talkToChar, string playerSpawnTag)
		{
			return SandBoxMissions.OpenTownCenterMission(scene, townUpgradeLevel, location, talkToChar, playerSpawnTag);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000060FB File Offset: 0x000042FB
		IMission CampaignMission.ICampaignMissionManager.OpenCastleCourtyardMission(string scene, int castleUpgradeLevel, Location location, CharacterObject talkToChar)
		{
			return SandBoxMissions.OpenCastleCourtyardMission(scene, castleUpgradeLevel, location, talkToChar);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006107 File Offset: 0x00004307
		IMission CampaignMission.ICampaignMissionManager.OpenVillageMission(string scene, Location location, CharacterObject talkToChar)
		{
			return SandBoxMissions.OpenVillageMission(scene, location, talkToChar, null);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006112 File Offset: 0x00004312
		IMission CampaignMission.ICampaignMissionManager.OpenIndoorMission(string scene, int upgradeLevel, Location location, CharacterObject talkToChar)
		{
			return SandBoxMissions.OpenIndoorMission(scene, upgradeLevel, location, talkToChar);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000611E File Offset: 0x0000431E
		IMission CampaignMission.ICampaignMissionManager.OpenPrisonBreakMission(string scene, Location location, CharacterObject prisonerCharacter)
		{
			return SandBoxMissions.OpenPrisonBreakMission(scene, location, prisonerCharacter);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006128 File Offset: 0x00004328
		IMission CampaignMission.ICampaignMissionManager.OpenArenaStartMission(string scene, Location location, CharacterObject talkToChar)
		{
			return SandBoxMissions.OpenArenaStartMission(scene, location, talkToChar, "");
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006137 File Offset: 0x00004337
		public IMission OpenArenaDuelMission(string scene, Location location, CharacterObject duelCharacter, bool requireCivilianEquipment, bool spawnBOthSidesWithHorse, Action<CharacterObject> onDuelEndAction, float customAgentHealth)
		{
			return SandBoxMissions.OpenArenaDuelMission(scene, location, duelCharacter, requireCivilianEquipment, spawnBOthSidesWithHorse, onDuelEndAction, customAgentHealth, "");
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000614E File Offset: 0x0000434E
		IMission CampaignMission.ICampaignMissionManager.OpenConversationMission(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData, string specialScene, string sceneLevels, bool isMultiAgentConversation)
		{
			return SandBoxMissions.OpenConversationMission(playerCharacterData, conversationPartnerData, specialScene, sceneLevels, isMultiAgentConversation);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000615C File Offset: 0x0000435C
		IMission CampaignMission.ICampaignMissionManager.OpenMeetingMission(string scene, CharacterObject character)
		{
			return SandBoxMissions.OpenMeetingMission(scene, character);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006165 File Offset: 0x00004365
		IMission CampaignMission.ICampaignMissionManager.OpenRetirementMission(string scene, Location location, CharacterObject talkToChar, string sceneLevels, string unconsciousMenuId)
		{
			return SandBoxMissions.OpenRetirementMission(scene, location, talkToChar, sceneLevels, unconsciousMenuId);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006173 File Offset: 0x00004373
		IMission CampaignMission.ICampaignMissionManager.OpenHideoutAmbushMission(string sceneName, FlattenedTroopRoster playerTroops, Location location)
		{
			return SandBoxMissions.OpenHideoutAmbushMission(sceneName, playerTroops, location);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000617D File Offset: 0x0000437D
		public IMission OpenDisguiseMission(string scene, bool willSetUpContact, string sceneLevels, Location fromLocation)
		{
			return SandBoxMissions.OpenDisguiseMission(scene, willSetUpContact, fromLocation, sceneLevels);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006189 File Offset: 0x00004389
		public IMission OpenNavalRaidMission(TroopRoster navalRaidTroops, BattleSideEnum navalSide, List<Ship> allShips)
		{
			return null;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000618C File Offset: 0x0000438C
		public IMission OpenNavalBattleMission(MissionInitializerRecord rec)
		{
			return null;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000618F File Offset: 0x0000438F
		public IMission OpenNavalSetPieceBattleMission(MissionInitializerRecord rec, MBList<IShipOrigin> playerShips, MBList<IShipOrigin> playerAllyShips, MBList<IShipOrigin> enemyShips)
		{
			return null;
		}
	}
}
