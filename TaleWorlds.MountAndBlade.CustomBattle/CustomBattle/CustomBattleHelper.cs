using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
	// Token: 0x02000018 RID: 24
	public static class CustomBattleHelper
	{
		// Token: 0x0600011A RID: 282 RVA: 0x00008734 File Offset: 0x00006934
		public static int GetIndexFromGameTypeStringId(string gameTypeStringId)
		{
			if (gameTypeStringId == "Battle")
			{
				return 0;
			}
			if (gameTypeStringId == "Siege")
			{
				return 1;
			}
			if (!(gameTypeStringId == "Village"))
			{
				Debug.FailedAssert("Given gameTypeStringId: \"" + gameTypeStringId + "\" is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.CustomBattle\\CustomBattle\\CustomBattleHelper.cs", "GetIndexFromGameTypeStringId", 78);
				return -1;
			}
			return 2;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00008794 File Offset: 0x00006994
		public static void StartGame(CustomBattleData data)
		{
			Game.Current.PlayerTroop = data.PlayerCharacter;
			if (data.GameTypeStringId == "Siege")
			{
				BannerlordMissions.OpenSiegeMissionWithDeployment(data.SceneId, data.PlayerCharacter, data.PlayerParty, data.EnemyParty, data.IsPlayerGeneral, data.WallHitpointPercentages, data.HasAnySiegeTower, data.AttackerMachines, data.DefenderMachines, data.IsPlayerAttacker, data.SceneUpgradeLevel, data.SeasonId, data.IsSallyOut, data.IsReliefAttack, data.TimeOfDay);
				return;
			}
			BannerlordMissions.OpenCustomBattleMission(data.SceneId, data.PlayerCharacter, data.PlayerParty, data.EnemyParty, data.IsPlayerGeneral, data.PlayerSideGeneralCharacter, data.SceneLevel, data.SeasonId, data.TimeOfDay);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00008860 File Offset: 0x00006A60
		public static int[] GetTroopCounts(int armySize, CustomBattleCompositionData compositionData)
		{
			int[] array = new int[4];
			armySize--;
			array[1] = MathF.Round(compositionData.RangedPercentage * (float)armySize);
			array[2] = MathF.Round(compositionData.CavalryPercentage * (float)armySize);
			array[3] = MathF.Round(compositionData.RangedCavalryPercentage * (float)armySize);
			array[0] = armySize - array.Sum();
			return array;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000088B8 File Offset: 0x00006AB8
		public static float[] GetWallHitpointPercentages(int breachedWallCount)
		{
			float[] array = new float[2];
			if (breachedWallCount == 1)
			{
				int num = MBRandom.RandomInt(2);
				array[num] = 0f;
				array[1 - num] = 1f;
			}
			else if (breachedWallCount == 0)
			{
				array[0] = 1f;
				array[1] = 1f;
			}
			else
			{
				array[0] = 0f;
				array[1] = 0f;
			}
			return array;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008914 File Offset: 0x00006B14
		public static SiegeEngineType GetSiegeWeaponType(SiegeEngineType siegeWeaponType)
		{
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ladder)
			{
				return DefaultSiegeEngineTypes.Ladder;
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ballista)
			{
				return DefaultSiegeEngineTypes.Ballista;
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.FireBallista)
			{
				return DefaultSiegeEngineTypes.FireBallista;
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ram || siegeWeaponType == DefaultSiegeEngineTypes.ImprovedRam)
			{
				return DefaultSiegeEngineTypes.Ram;
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.SiegeTower)
			{
				return DefaultSiegeEngineTypes.SiegeTower;
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Onager || siegeWeaponType == DefaultSiegeEngineTypes.Catapult)
			{
				return DefaultSiegeEngineTypes.Onager;
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.FireOnager || siegeWeaponType == DefaultSiegeEngineTypes.FireCatapult)
			{
				return DefaultSiegeEngineTypes.FireOnager;
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Trebuchet || siegeWeaponType == DefaultSiegeEngineTypes.Bricole)
			{
				return DefaultSiegeEngineTypes.Trebuchet;
			}
			return siegeWeaponType;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000089B4 File Offset: 0x00006BB4
		public static CustomBattleData PrepareBattleData(BasicCharacterObject playerCharacter, BasicCharacterObject playerSideGeneralCharacter, CustomBattleCombatant playerParty, CustomBattleCombatant enemyParty, CustomBattlePlayerSide playerSide, CustomBattlePlayerType battlePlayerType, string gameTypeStringId, string scene, string season, float timeOfDay, List<MissionSiegeWeapon> attackerMachines, List<MissionSiegeWeapon> defenderMachines, float[] wallHitPointsPercentages, int sceneUpgradeLevel, bool isSallyOut, string forcedSceneLevel)
		{
			bool flag = playerSide == CustomBattlePlayerSide.Attacker;
			bool flag2 = battlePlayerType == CustomBattlePlayerType.Commander;
			CustomBattleData customBattleData = new CustomBattleData
			{
				GameTypeStringId = gameTypeStringId,
				SceneId = scene,
				PlayerCharacter = playerCharacter,
				PlayerParty = playerParty,
				EnemyParty = enemyParty,
				IsPlayerGeneral = flag2,
				PlayerSideGeneralCharacter = playerSideGeneralCharacter,
				SeasonId = season,
				SceneLevel = forcedSceneLevel,
				TimeOfDay = timeOfDay
			};
			if (customBattleData.GameTypeStringId == "Siege")
			{
				customBattleData.AttackerMachines = attackerMachines;
				customBattleData.DefenderMachines = defenderMachines;
				customBattleData.WallHitpointPercentages = wallHitPointsPercentages;
				customBattleData.HasAnySiegeTower = attackerMachines.Exists((MissionSiegeWeapon mm) => mm.Type == DefaultSiegeEngineTypes.SiegeTower);
				customBattleData.IsPlayerAttacker = flag;
				customBattleData.SceneUpgradeLevel = sceneUpgradeLevel;
				customBattleData.IsSallyOut = isSallyOut;
				customBattleData.IsReliefAttack = false;
			}
			return customBattleData;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008AA8 File Offset: 0x00006CA8
		public static CustomBattleCombatant[] GetCustomBattleParties(BasicCharacterObject playerCharacter, BasicCharacterObject playerSideGeneralCharacter, BasicCharacterObject enemyCharacter, BasicCultureObject playerFaction, int[] playerNumbers, List<BasicCharacterObject>[] playerTroopSelections, BasicCultureObject enemyFaction, int[] enemyNumbers, List<BasicCharacterObject>[] enemyTroopSelections, bool isPlayerAttacker)
		{
			Banner banner = new Banner(playerFaction.Banner, playerFaction.Color, playerFaction.Color2);
			Banner banner2 = new Banner(enemyFaction.Banner, enemyFaction.Color, enemyFaction.Color2);
			if (playerFaction.StringId == enemyFaction.StringId)
			{
				uint primaryColor = banner2.GetPrimaryColor();
				banner2.ChangePrimaryColor(banner2.GetFirstIconColor());
				banner2.ChangeIconColors(primaryColor);
			}
			CustomBattleCombatant[] array = new CustomBattleCombatant[]
			{
				new CustomBattleCombatant(new TextObject("{=sSJSTe5p}Player Party", null), playerFaction, banner, BattleEnvironment.Land),
				new CustomBattleCombatant(new TextObject("{=0xC75dN6}Enemy Party", null), enemyFaction, banner2, BattleEnvironment.Land)
			};
			array[0].Side = (isPlayerAttacker ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
			array[0].AddCharacter(playerCharacter, 1);
			if (playerSideGeneralCharacter != null)
			{
				array[0].AddCharacter(playerSideGeneralCharacter, 1);
				array[0].SetGeneral(playerSideGeneralCharacter);
			}
			else
			{
				array[0].SetGeneral(playerCharacter);
			}
			array[1].Side = array[0].Side.GetOppositeSide();
			array[1].AddCharacter(enemyCharacter, 1);
			for (int i = 0; i < array.Length; i++)
			{
				CustomBattleHelper.PopulateListsWithDefaults(ref array[i], (i == 0) ? playerNumbers : enemyNumbers, (i == 0) ? playerTroopSelections : enemyTroopSelections);
			}
			return array;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00008BD8 File Offset: 0x00006DD8
		private static void PopulateListsWithDefaults(ref CustomBattleCombatant customBattleParties, int[] numbers, List<BasicCharacterObject>[] troopList)
		{
			BasicCultureObject basicCulture = customBattleParties.BasicCulture;
			if (troopList == null)
			{
				troopList = new List<BasicCharacterObject>[]
				{
					new List<BasicCharacterObject>(),
					new List<BasicCharacterObject>(),
					new List<BasicCharacterObject>(),
					new List<BasicCharacterObject>()
				};
			}
			if (troopList[0].Count == 0)
			{
				troopList[0] = new List<BasicCharacterObject> { CustomBattleHelper.GetDefaultTroopOfFormationForFaction(basicCulture, FormationClass.Infantry) };
			}
			if (troopList[1].Count == 0)
			{
				troopList[1] = new List<BasicCharacterObject> { CustomBattleHelper.GetDefaultTroopOfFormationForFaction(basicCulture, FormationClass.Ranged) };
			}
			if (troopList[2].Count == 0)
			{
				troopList[2] = new List<BasicCharacterObject> { CustomBattleHelper.GetDefaultTroopOfFormationForFaction(basicCulture, FormationClass.Cavalry) };
			}
			if (troopList[3].Count == 0)
			{
				troopList[3] = new List<BasicCharacterObject> { CustomBattleHelper.GetDefaultTroopOfFormationForFaction(basicCulture, FormationClass.HorseArcher) };
			}
			if (troopList[3].Count != 0)
			{
				if (!troopList[3].All<BasicCharacterObject>((BasicCharacterObject troop) => troop == null))
				{
					goto IL_012C;
				}
			}
			numbers[2] += numbers[3] / 3;
			numbers[1] += numbers[3] / 3;
			numbers[0] += numbers[3] / 3;
			numbers[0] += numbers[3] - numbers[3] / 3 * 3;
			numbers[3] = 0;
			IL_012C:
			for (int i = 0; i < 4; i++)
			{
				int count = troopList[i].Count;
				int num = numbers[i];
				if (num > 0)
				{
					float num2 = (float)num / (float)count;
					float num3 = 0f;
					for (int j = 0; j < count; j++)
					{
						float num4 = num2 + num3;
						int num5 = MathF.Floor(num4);
						num3 = num4 - (float)num5;
						customBattleParties.AddCharacter(troopList[i][j], num5);
						numbers[i] -= num5;
						if (j == count - 1 && numbers[i] > 0)
						{
							customBattleParties.AddCharacter(troopList[i][j], numbers[i]);
							numbers[i] = 0;
						}
					}
				}
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008DAC File Offset: 0x00006FAC
		public static void AssertMissingTroopsForDebug()
		{
			foreach (BasicCultureObject basicCultureObject in CustomBattleData.Factions)
			{
				for (int i = 0; i < 4; i++)
				{
					CustomBattleHelper.GetDefaultTroopOfFormationForFaction(basicCultureObject, (FormationClass)i);
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008E08 File Offset: 0x00007008
		public static BasicCharacterObject GetDefaultTroopOfFormationForFaction(BasicCultureObject culture, FormationClass formation)
		{
			if (culture.StringId.ToLower() == "empire")
			{
				switch (formation)
				{
				case FormationClass.Infantry:
					return CustomBattleHelper.GetTroopFromId("imperial_veteran_infantryman");
				case FormationClass.Ranged:
					return CustomBattleHelper.GetTroopFromId("imperial_archer");
				case FormationClass.Cavalry:
					return CustomBattleHelper.GetTroopFromId("imperial_heavy_horseman");
				case FormationClass.HorseArcher:
					return CustomBattleHelper.GetTroopFromId("bucellarii");
				}
			}
			else if (culture.StringId.ToLower() == "sturgia")
			{
				switch (formation)
				{
				case FormationClass.Infantry:
					return CustomBattleHelper.GetTroopFromId("sturgian_spearman");
				case FormationClass.Ranged:
					return CustomBattleHelper.GetTroopFromId("sturgian_archer");
				case FormationClass.Cavalry:
					return CustomBattleHelper.GetTroopFromId("sturgian_hardened_brigand");
				}
			}
			else if (culture.StringId.ToLower() == "aserai")
			{
				switch (formation)
				{
				case FormationClass.Infantry:
					return CustomBattleHelper.GetTroopFromId("aserai_infantry");
				case FormationClass.Ranged:
					return CustomBattleHelper.GetTroopFromId("aserai_archer");
				case FormationClass.Cavalry:
					return CustomBattleHelper.GetTroopFromId("aserai_mameluke_cavalry");
				case FormationClass.HorseArcher:
					return CustomBattleHelper.GetTroopFromId("aserai_faris");
				}
			}
			else if (culture.StringId.ToLower() == "vlandia")
			{
				switch (formation)
				{
				case FormationClass.Infantry:
					return CustomBattleHelper.GetTroopFromId("vlandian_swordsman");
				case FormationClass.Ranged:
					return CustomBattleHelper.GetTroopFromId("vlandian_hardened_crossbowman");
				case FormationClass.Cavalry:
					return CustomBattleHelper.GetTroopFromId("vlandian_knight");
				}
			}
			else if (culture.StringId.ToLower() == "battania")
			{
				switch (formation)
				{
				case FormationClass.Infantry:
					return CustomBattleHelper.GetTroopFromId("battanian_picked_warrior");
				case FormationClass.Ranged:
					return CustomBattleHelper.GetTroopFromId("battanian_hero");
				case FormationClass.Cavalry:
					return CustomBattleHelper.GetTroopFromId("battanian_scout");
				}
			}
			else if (culture.StringId.ToLower() == "khuzait")
			{
				switch (formation)
				{
				case FormationClass.Infantry:
					return CustomBattleHelper.GetTroopFromId("khuzait_spear_infantry");
				case FormationClass.Ranged:
					return CustomBattleHelper.GetTroopFromId("khuzait_archer");
				case FormationClass.Cavalry:
					return CustomBattleHelper.GetTroopFromId("khuzait_lancer");
				case FormationClass.HorseArcher:
					return CustomBattleHelper.GetTroopFromId("khuzait_horse_archer");
				}
			}
			else if (culture.StringId.ToLower() == "nord")
			{
				if (formation == FormationClass.Infantry)
				{
					return CustomBattleHelper.GetTroopFromId("nord_spear_warrior");
				}
				if (formation == FormationClass.Ranged)
				{
					return CustomBattleHelper.GetTroopFromId("nord_marksman");
				}
			}
			return null;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00009050 File Offset: 0x00007250
		private static BasicCharacterObject GetTroopFromId(string troopId)
		{
			return MBObjectManager.Instance.GetObject<BasicCharacterObject>(troopId);
		}

		// Token: 0x040000B3 RID: 179
		public const string DefaultBattleGameTypeStringId = "Battle";

		// Token: 0x040000B4 RID: 180
		public const string DefaultSiegeGameTypeStringId = "Siege";

		// Token: 0x040000B5 RID: 181
		public const string DefaultVillageGameTypeStringId = "Village";

		// Token: 0x040000B6 RID: 182
		private const string EmpireInfantryTroop = "imperial_veteran_infantryman";

		// Token: 0x040000B7 RID: 183
		private const string EmpireRangedTroop = "imperial_archer";

		// Token: 0x040000B8 RID: 184
		private const string EmpireCavalryTroop = "imperial_heavy_horseman";

		// Token: 0x040000B9 RID: 185
		private const string EmpireHorseArcherTroop = "bucellarii";

		// Token: 0x040000BA RID: 186
		private const string SturgiaInfantryTroop = "sturgian_spearman";

		// Token: 0x040000BB RID: 187
		private const string SturgiaRangedTroop = "sturgian_archer";

		// Token: 0x040000BC RID: 188
		private const string SturgiaCavalryTroop = "sturgian_hardened_brigand";

		// Token: 0x040000BD RID: 189
		private const string AseraiInfantryTroop = "aserai_infantry";

		// Token: 0x040000BE RID: 190
		private const string AseraiRangedTroop = "aserai_archer";

		// Token: 0x040000BF RID: 191
		private const string AseraiCavalryTroop = "aserai_mameluke_cavalry";

		// Token: 0x040000C0 RID: 192
		private const string AseraiHorseArcherTroop = "aserai_faris";

		// Token: 0x040000C1 RID: 193
		private const string VlandiaInfantryTroop = "vlandian_swordsman";

		// Token: 0x040000C2 RID: 194
		private const string VlandiaRangedTroop = "vlandian_hardened_crossbowman";

		// Token: 0x040000C3 RID: 195
		private const string VlandiaCavalryTroop = "vlandian_knight";

		// Token: 0x040000C4 RID: 196
		private const string BattaniaInfantryTroop = "battanian_picked_warrior";

		// Token: 0x040000C5 RID: 197
		private const string BattaniaRangedTroop = "battanian_hero";

		// Token: 0x040000C6 RID: 198
		private const string BattaniaCavalryTroop = "battanian_scout";

		// Token: 0x040000C7 RID: 199
		private const string KhuzaitInfantryTroop = "khuzait_spear_infantry";

		// Token: 0x040000C8 RID: 200
		private const string KhuzaitRangedTroop = "khuzait_archer";

		// Token: 0x040000C9 RID: 201
		private const string KhuzaitCavalryTroop = "khuzait_lancer";

		// Token: 0x040000CA RID: 202
		private const string KhuzaitHorseArcherTroop = "khuzait_horse_archer";

		// Token: 0x040000CB RID: 203
		private const string NordInfantryTroop = "nord_spear_warrior";

		// Token: 0x040000CC RID: 204
		private const string NordRangedTroop = "nord_marksman";
	}
}
