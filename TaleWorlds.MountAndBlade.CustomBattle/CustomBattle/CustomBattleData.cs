using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
	// Token: 0x02000016 RID: 22
	public struct CustomBattleData
	{
		// Token: 0x0600010D RID: 269 RVA: 0x000086AA File Offset: 0x000068AA
		public static IEnumerable<SiegeEngineType> GetAllAttackerMeleeMachines()
		{
			yield return DefaultSiegeEngineTypes.Ram;
			yield return DefaultSiegeEngineTypes.SiegeTower;
			yield break;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000086B3 File Offset: 0x000068B3
		public static IEnumerable<SiegeEngineType> GetAllDefenderRangedMachines()
		{
			yield return DefaultSiegeEngineTypes.Ballista;
			yield return DefaultSiegeEngineTypes.FireBallista;
			yield return DefaultSiegeEngineTypes.Catapult;
			yield return DefaultSiegeEngineTypes.FireCatapult;
			yield break;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000086BC File Offset: 0x000068BC
		public static IEnumerable<SiegeEngineType> GetAllAttackerRangedMachines()
		{
			yield return DefaultSiegeEngineTypes.Ballista;
			yield return DefaultSiegeEngineTypes.FireBallista;
			yield return DefaultSiegeEngineTypes.Onager;
			yield return DefaultSiegeEngineTypes.FireOnager;
			yield return DefaultSiegeEngineTypes.Trebuchet;
			yield break;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000086C5 File Offset: 0x000068C5
		public static IEnumerable<Tuple<string, string>> GameTypes
		{
			get
			{
				yield return new Tuple<string, string>(GameTexts.FindText("str_battle", null).ToString(), "Battle");
				if (!Module.CurrentModule.IsOnlyCoreContentEnabled)
				{
					yield return new Tuple<string, string>(new TextObject("{=Ua6CNLBZ}Village", null).ToString(), "Village");
					yield return new Tuple<string, string>(GameTexts.FindText("str_siege", null).ToString(), "Siege");
				}
				yield break;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000086CE File Offset: 0x000068CE
		public static IEnumerable<Tuple<string, CustomBattlePlayerType>> PlayerTypes
		{
			get
			{
				yield return new Tuple<string, CustomBattlePlayerType>(GameTexts.FindText("str_team_commander", null).ToString(), CustomBattlePlayerType.Commander);
				if (!Module.CurrentModule.IsOnlyCoreContentEnabled)
				{
					yield return new Tuple<string, CustomBattlePlayerType>(new TextObject("{=g9VIbA9s}Sergeant", null).ToString(), CustomBattlePlayerType.Sergeant);
				}
				yield break;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000086D7 File Offset: 0x000068D7
		public static IEnumerable<Tuple<string, CustomBattlePlayerSide>> PlayerSides
		{
			get
			{
				yield return new Tuple<string, CustomBattlePlayerSide>(new TextObject("{=XEVFUaFj}Defender", null).ToString(), CustomBattlePlayerSide.Defender);
				yield return new Tuple<string, CustomBattlePlayerSide>(new TextObject("{=KASD0tnO}Attacker", null).ToString(), CustomBattlePlayerSide.Attacker);
				yield break;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000086E0 File Offset: 0x000068E0
		public static IEnumerable<BasicCharacterObject> Characters
		{
			get
			{
				yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_1");
				if (!Module.CurrentModule.IsOnlyCoreContentEnabled)
				{
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_2");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_3");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_4");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_5");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_6");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_7");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_8");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_9");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_10");
				}
				yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_11");
				if (!Module.CurrentModule.IsOnlyCoreContentEnabled)
				{
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_12");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_13");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_14");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_15");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_16");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_17");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_18");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_19");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_20");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_21");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_22");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_23");
					yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_24");
				}
				yield break;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000086E9 File Offset: 0x000068E9
		public static IEnumerable<BasicCultureObject> Factions
		{
			get
			{
				yield return Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire");
				if (!Module.CurrentModule.IsOnlyCoreContentEnabled)
				{
					yield return Game.Current.ObjectManager.GetObject<BasicCultureObject>("sturgia");
					yield return Game.Current.ObjectManager.GetObject<BasicCultureObject>("aserai");
					yield return Game.Current.ObjectManager.GetObject<BasicCultureObject>("vlandia");
					yield return Game.Current.ObjectManager.GetObject<BasicCultureObject>("battania");
					yield return Game.Current.ObjectManager.GetObject<BasicCultureObject>("khuzait");
					if (ModuleHelper.IsModuleActive("NavalDLC"))
					{
						yield return Game.Current.ObjectManager.GetObject<BasicCultureObject>("nord");
					}
				}
				yield break;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000086F2 File Offset: 0x000068F2
		public static IEnumerable<Tuple<string, CustomBattleTimeOfDay>> TimesOfDay
		{
			get
			{
				yield return new Tuple<string, CustomBattleTimeOfDay>(new TextObject("{=X3gcUz7C}Morning", null).ToString(), CustomBattleTimeOfDay.Morning);
				yield return new Tuple<string, CustomBattleTimeOfDay>(new TextObject("{=CTtjSwRb}Noon", null).ToString(), CustomBattleTimeOfDay.Noon);
				yield return new Tuple<string, CustomBattleTimeOfDay>(new TextObject("{=J2gvnexb}Afternoon", null).ToString(), CustomBattleTimeOfDay.Afternoon);
				yield return new Tuple<string, CustomBattleTimeOfDay>(new TextObject("{=gENb9SSW}Evening", null).ToString(), CustomBattleTimeOfDay.Evening);
				yield return new Tuple<string, CustomBattleTimeOfDay>(new TextObject("{=fAxjyMt5}Night", null).ToString(), CustomBattleTimeOfDay.Night);
				yield break;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000086FB File Offset: 0x000068FB
		public static IEnumerable<Tuple<string, string>> Seasons
		{
			get
			{
				yield return new Tuple<string, string>(new TextObject("{=f7vOVQb7}Summer", null).ToString(), "summer");
				yield return new Tuple<string, string>(new TextObject("{=cZzfNlxd}Fall", null).ToString(), "fall");
				yield return new Tuple<string, string>(new TextObject("{=nwqUFaU8}Winter", null).ToString(), "winter");
				yield return new Tuple<string, string>(new TextObject("{=nWbp3o3H}Spring", null).ToString(), "spring");
				yield break;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00008704 File Offset: 0x00006904
		public static IEnumerable<Tuple<string, int>> WallHitpoints
		{
			get
			{
				yield return new Tuple<string, int>(new TextObject("{=dsMeB3vi}Solid", null).ToString(), 0);
				yield return new Tuple<string, int>(new TextObject("{=Kvxo2jzJ}Single Breached", null).ToString(), 1);
				yield return new Tuple<string, int>(new TextObject("{=AiNXIt5N}Dual Breached", null).ToString(), 2);
				yield break;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000870D File Offset: 0x0000690D
		public static IEnumerable<int> SceneLevels
		{
			get
			{
				yield return 1;
				yield return 2;
				yield return 3;
				yield break;
			}
		}

		// Token: 0x04000099 RID: 153
		public const int NumberOfAttackerMeleeMachines = 3;

		// Token: 0x0400009A RID: 154
		public const int NumberOfAttackerRangedMachines = 4;

		// Token: 0x0400009B RID: 155
		public const int NumberOfDefenderRangedMachines = 4;

		// Token: 0x0400009C RID: 156
		public const string CoreContentDefaultSceneName = "battle_terrain_029";

		// Token: 0x0400009D RID: 157
		public string GameTypeStringId;

		// Token: 0x0400009E RID: 158
		public string SceneId;

		// Token: 0x0400009F RID: 159
		public string SeasonId;

		// Token: 0x040000A0 RID: 160
		public BasicCharacterObject PlayerCharacter;

		// Token: 0x040000A1 RID: 161
		public BasicCharacterObject PlayerSideGeneralCharacter;

		// Token: 0x040000A2 RID: 162
		public CustomBattleCombatant PlayerParty;

		// Token: 0x040000A3 RID: 163
		public CustomBattleCombatant EnemyParty;

		// Token: 0x040000A4 RID: 164
		public float TimeOfDay;

		// Token: 0x040000A5 RID: 165
		public bool IsPlayerGeneral;

		// Token: 0x040000A6 RID: 166
		public string SceneLevel;

		// Token: 0x040000A7 RID: 167
		public List<MissionSiegeWeapon> AttackerMachines;

		// Token: 0x040000A8 RID: 168
		public List<MissionSiegeWeapon> DefenderMachines;

		// Token: 0x040000A9 RID: 169
		public float[] WallHitpointPercentages;

		// Token: 0x040000AA RID: 170
		public bool HasAnySiegeTower;

		// Token: 0x040000AB RID: 171
		public bool IsPlayerAttacker;

		// Token: 0x040000AC RID: 172
		public bool IsReliefAttack;

		// Token: 0x040000AD RID: 173
		public bool IsSallyOut;

		// Token: 0x040000AE RID: 174
		public int SceneUpgradeLevel;
	}
}
