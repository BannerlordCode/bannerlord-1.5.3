using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200005E RID: 94
	public static class CampaignMission
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x00028606 File Offset: 0x00026806
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x0002860D File Offset: 0x0002680D
		public static ICampaignMission Current { get; set; }

		// Token: 0x0600092E RID: 2350 RVA: 0x00028615 File Offset: 0x00026815
		public static IMission OpenBattleMission(string scene, bool usesTownDecalAtlas, string sceneLevels = "")
		{
			return Campaign.Current.CampaignMissionManager.OpenBattleMission(scene, usesTownDecalAtlas, sceneLevels);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00028629 File Offset: 0x00026829
		public static IMission OpenNavalRaidMission(TroopRoster attackerSideTroops, BattleSideEnum navalSide, List<Ship> allShips)
		{
			return Campaign.Current.CampaignMissionManager.OpenNavalRaidMission(attackerSideTroops, navalSide, allShips);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002863D File Offset: 0x0002683D
		public static IMission OpenAlleyFightMission(string scene, int upgradeLevel, Location location, TroopRoster playerSideTroops, TroopRoster rivalSideTroops)
		{
			return Campaign.Current.CampaignMissionManager.OpenAlleyFightMission(scene, upgradeLevel, location, playerSideTroops, rivalSideTroops);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00028654 File Offset: 0x00026854
		public static IMission OpenCombatMissionWithDialogue(string scene, CharacterObject characterToTalkTo, int upgradeLevel)
		{
			return Campaign.Current.CampaignMissionManager.OpenCombatMissionWithDialogue(scene, characterToTalkTo, upgradeLevel);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00028668 File Offset: 0x00026868
		public static IMission OpenBattleMissionWhileEnteringSettlement(string scene, int upgradeLevel, int numberOfMaxTroopToBeSpawnedForPlayer, int numberOfMaxTroopToBeSpawnedForOpponent)
		{
			return Campaign.Current.CampaignMissionManager.OpenBattleMissionWhileEnteringSettlement(scene, upgradeLevel, numberOfMaxTroopToBeSpawnedForPlayer, numberOfMaxTroopToBeSpawnedForOpponent);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002867D File Offset: 0x0002687D
		public static IMission OpenHideoutBattleMission(string scene, FlattenedTroopRoster playerTroops, bool isTutorial)
		{
			return Campaign.Current.CampaignMissionManager.OpenHideoutBattleMission(scene, playerTroops, isTutorial);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00028694 File Offset: 0x00026894
		public static IMission OpenSiegeMissionWithDeployment(string scene, float[] wallHitPointsPercentages, bool hasAnySiegeTower, List<MissionSiegeWeapon> siegeWeaponsOfAttackers, List<MissionSiegeWeapon> siegeWeaponsOfDefenders, bool isPlayerAttacker, int upgradeLevel = 0, bool isSallyOut = false, bool isReliefForceAttack = false)
		{
			return Campaign.Current.CampaignMissionManager.OpenSiegeMissionWithDeployment(scene, wallHitPointsPercentages, hasAnySiegeTower, siegeWeaponsOfAttackers, siegeWeaponsOfDefenders, isPlayerAttacker, upgradeLevel, isSallyOut, isReliefForceAttack);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x000286BE File Offset: 0x000268BE
		public static IMission OpenSiegeMissionNoDeployment(string scene, bool isSallyOut = false, bool isReliefForceAttack = false)
		{
			return Campaign.Current.CampaignMissionManager.OpenSiegeMissionNoDeployment(scene, isSallyOut, isReliefForceAttack);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x000286D2 File Offset: 0x000268D2
		public static IMission OpenSiegeLordsHallFightMission(string scene, FlattenedTroopRoster attackerPriorityList)
		{
			return Campaign.Current.CampaignMissionManager.OpenSiegeLordsHallFightMission(scene, attackerPriorityList);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x000286E5 File Offset: 0x000268E5
		public static IMission OpenBattleMission(MissionInitializerRecord rec)
		{
			return Campaign.Current.CampaignMissionManager.OpenBattleMission(rec);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x000286F7 File Offset: 0x000268F7
		public static IMission OpenNavalBattleMission(MissionInitializerRecord rec)
		{
			return Campaign.Current.CampaignMissionManager.OpenNavalBattleMission(rec);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00028709 File Offset: 0x00026909
		public static IMission OpenNavalSetPieceBattleMission(MissionInitializerRecord rec, MBList<IShipOrigin> playerShips, MBList<IShipOrigin> playerAllyShips, MBList<IShipOrigin> enemyShips)
		{
			return Campaign.Current.CampaignMissionManager.OpenNavalSetPieceBattleMission(rec, playerShips, playerAllyShips, enemyShips);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0002871E File Offset: 0x0002691E
		public static IMission OpenCaravanBattleMission(MissionInitializerRecord rec, bool isCaravan)
		{
			return Campaign.Current.CampaignMissionManager.OpenCaravanBattleMission(rec, isCaravan);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00028731 File Offset: 0x00026931
		public static IMission OpenTownCenterMission(string scene, Location location, CharacterObject talkToChar, int townUpgradeLevel, string playerSpawnTag)
		{
			return Campaign.Current.CampaignMissionManager.OpenTownCenterMission(scene, townUpgradeLevel, location, talkToChar, playerSpawnTag);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00028748 File Offset: 0x00026948
		public static IMission OpenCastleCourtyardMission(string scene, Location location, CharacterObject talkToChar, int castleUpgradeLevel)
		{
			return Campaign.Current.CampaignMissionManager.OpenCastleCourtyardMission(scene, castleUpgradeLevel, location, talkToChar);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002875D File Offset: 0x0002695D
		public static IMission OpenVillageMission(string scene, Location location, CharacterObject talkToChar)
		{
			return Campaign.Current.CampaignMissionManager.OpenVillageMission(scene, location, talkToChar);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00028771 File Offset: 0x00026971
		public static IMission OpenIndoorMission(string scene, int upgradeLevel, Location location, CharacterObject talkToChar)
		{
			return Campaign.Current.CampaignMissionManager.OpenIndoorMission(scene, upgradeLevel, location, talkToChar);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00028786 File Offset: 0x00026986
		public static IMission OpenPrisonBreakMission(string scene, Location location, CharacterObject prisonerCharacter)
		{
			return Campaign.Current.CampaignMissionManager.OpenPrisonBreakMission(scene, location, prisonerCharacter);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002879A File Offset: 0x0002699A
		public static IMission OpenArenaStartMission(string scene, Location location, CharacterObject talkToChar)
		{
			return Campaign.Current.CampaignMissionManager.OpenArenaStartMission(scene, location, talkToChar);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x000287AE File Offset: 0x000269AE
		public static IMission OpenArenaDuelMission(string scene, Location location, CharacterObject talkToChar, bool requireCivilianEquipment, bool spawnBothSidesWithHorse, Action<CharacterObject> onDuelEnd, float customAgentHealth)
		{
			return Campaign.Current.CampaignMissionManager.OpenArenaDuelMission(scene, location, talkToChar, requireCivilianEquipment, spawnBothSidesWithHorse, onDuelEnd, customAgentHealth);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000287C9 File Offset: 0x000269C9
		public static IMission OpenConversationMission(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData, string specialScene = "", string sceneLevels = "", bool isMultiAgentConversation = false)
		{
			return Campaign.Current.CampaignMissionManager.OpenConversationMission(playerCharacterData, conversationPartnerData, specialScene, sceneLevels, isMultiAgentConversation);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x000287E0 File Offset: 0x000269E0
		public static IMission OpenRetirementMission(string scene, Location location, CharacterObject talkToChar = null, string sceneLevels = null, string unconsciousMenuId = "")
		{
			return Campaign.Current.CampaignMissionManager.OpenRetirementMission(scene, location, talkToChar, sceneLevels, unconsciousMenuId);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x000287F7 File Offset: 0x000269F7
		public static IMission OpenHideoutAmbushMission(string sceneName, FlattenedTroopRoster playerTroops, Location location)
		{
			return Campaign.Current.CampaignMissionManager.OpenHideoutAmbushMission(sceneName, playerTroops, location);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002880B File Offset: 0x00026A0B
		public static IMission OpenDisguiseMission(string scene, bool willSetUpContact, string sceneLevels, Location fromLocation)
		{
			return Campaign.Current.CampaignMissionManager.OpenDisguiseMission(scene, willSetUpContact, sceneLevels, fromLocation);
		}

		// Token: 0x02000540 RID: 1344
		public interface ICampaignMissionManager
		{
			// Token: 0x06004F3F RID: 20287
			IMission OpenSiegeMissionWithDeployment(string scene, float[] wallHitPointsPercentages, bool hasAnySiegeTower, List<MissionSiegeWeapon> siegeWeaponsOfAttackers, List<MissionSiegeWeapon> siegeWeaponsOfDefenders, bool isPlayerAttacker, int upgradeLevel = 0, bool isSallyOut = false, bool isReliefForceAttack = false);

			// Token: 0x06004F40 RID: 20288
			IMission OpenSiegeMissionNoDeployment(string scene, bool isSallyOut = false, bool isReliefForceAttack = false);

			// Token: 0x06004F41 RID: 20289
			IMission OpenSiegeLordsHallFightMission(string scene, FlattenedTroopRoster attackerPriorityList);

			// Token: 0x06004F42 RID: 20290
			IMission OpenBattleMission(MissionInitializerRecord rec);

			// Token: 0x06004F43 RID: 20291
			IMission OpenCaravanBattleMission(MissionInitializerRecord rec, bool isCaravan);

			// Token: 0x06004F44 RID: 20292
			IMission OpenBattleMission(string scene, bool usesTownDecalAtlas, string sceneLevels);

			// Token: 0x06004F45 RID: 20293
			IMission OpenNavalRaidMission(TroopRoster navalRaidTroops, BattleSideEnum navalSide, List<Ship> allShips);

			// Token: 0x06004F46 RID: 20294
			IMission OpenNavalBattleMission(MissionInitializerRecord rec);

			// Token: 0x06004F47 RID: 20295
			IMission OpenNavalSetPieceBattleMission(MissionInitializerRecord rec, MBList<IShipOrigin> playerShips, MBList<IShipOrigin> playerAllyShips, MBList<IShipOrigin> enemyShips);

			// Token: 0x06004F48 RID: 20296
			IMission OpenHideoutBattleMission(string scene, FlattenedTroopRoster playerTroops, bool isTutorial);

			// Token: 0x06004F49 RID: 20297
			IMission OpenTownCenterMission(string scene, int townUpgradeLevel, Location location, CharacterObject talkToChar, string playerSpawnTag);

			// Token: 0x06004F4A RID: 20298
			IMission OpenCastleCourtyardMission(string scene, int castleUpgradeLevel, Location location, CharacterObject talkToChar);

			// Token: 0x06004F4B RID: 20299
			IMission OpenVillageMission(string scene, Location location, CharacterObject talkToChar);

			// Token: 0x06004F4C RID: 20300
			IMission OpenIndoorMission(string scene, int upgradeLevel, Location location, CharacterObject talkToChar);

			// Token: 0x06004F4D RID: 20301
			IMission OpenPrisonBreakMission(string scene, Location location, CharacterObject prisonerCharacter);

			// Token: 0x06004F4E RID: 20302
			IMission OpenArenaStartMission(string scene, Location location, CharacterObject talkToChar);

			// Token: 0x06004F4F RID: 20303
			IMission OpenArenaDuelMission(string scene, Location location, CharacterObject duelCharacter, bool requireCivilianEquipment, bool spawnBOthSidesWithHorse, Action<CharacterObject> onDuelEndAction, float customAgentHealth);

			// Token: 0x06004F50 RID: 20304
			IMission OpenConversationMission(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData, string specialScene = "", string sceneLevels = "", bool isMultiAgentConversation = false);

			// Token: 0x06004F51 RID: 20305
			IMission OpenMeetingMission(string scene, CharacterObject character);

			// Token: 0x06004F52 RID: 20306
			IMission OpenAlleyFightMission(string scene, int upgradeLevel, Location location, TroopRoster playerSideTroops, TroopRoster rivalSideTroops);

			// Token: 0x06004F53 RID: 20307
			IMission OpenCombatMissionWithDialogue(string scene, CharacterObject characterToTalkTo, int upgradeLevel);

			// Token: 0x06004F54 RID: 20308
			IMission OpenBattleMissionWhileEnteringSettlement(string scene, int upgradeLevel, int numberOfMaxTroopToBeSpawnedForPlayer, int numberOfMaxTroopToBeSpawnedForOpponent);

			// Token: 0x06004F55 RID: 20309
			IMission OpenRetirementMission(string scene, Location location, CharacterObject talkToChar = null, string sceneLevels = null, string unconsciousMenuId = "");

			// Token: 0x06004F56 RID: 20310
			IMission OpenHideoutAmbushMission(string sceneName, FlattenedTroopRoster playerTroops, Location location);

			// Token: 0x06004F57 RID: 20311
			IMission OpenDisguiseMission(string scene, bool willSetUpContact, string sceneLevels, Location fromLocation);
		}
	}
}
