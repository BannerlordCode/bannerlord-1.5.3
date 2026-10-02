using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000D6 RID: 214
	public class CommonTownsfolkCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000977 RID: 2423 RVA: 0x00044B99 File Offset: 0x00042D99
		private float GetSpawnRate(Settlement settlement)
		{
			return this.TimeOfDayPercentage() * this.GetProsperityMultiplier(settlement.SettlementComponent) * this.GetWeatherEffectMultiplier(settlement);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00044BB6 File Offset: 0x00042DB6
		private float GetConfigValue()
		{
			return BannerlordConfig.CivilianAgentCount;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00044BBD File Offset: 0x00042DBD
		private float GetProsperityMultiplier(SettlementComponent settlement)
		{
			return ((float)settlement.GetProsperityLevel() + 1f) / 3f;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00044BD4 File Offset: 0x00042DD4
		private float GetWeatherEffectMultiplier(Settlement settlement)
		{
			MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(settlement.Position.ToVec2());
			if (weatherEventInPosition == MapWeatherModel.WeatherEvent.HeavyRain)
			{
				return 0.15f;
			}
			if (weatherEventInPosition != MapWeatherModel.WeatherEvent.Blizzard)
			{
				return 1f;
			}
			return 0.4f;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00044C20 File Offset: 0x00042E20
		private float TimeOfDayPercentage()
		{
			int num = MathF.Ceiling((float)CampaignTime.HoursInDay * 0.625f);
			return 1f - MathF.Abs(CampaignTime.Now.CurrentHourInDay - (float)num) / (float)num;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00044C5D File Offset: 0x00042E5D
		public override void RegisterEvents()
		{
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00044C76 File Offset: 0x00042E76
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00044C78 File Offset: 0x00042E78
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			if (!settlement.IsCastle)
			{
				Location locationWithId = settlement.LocationComplex.GetLocationWithId("center");
				Location locationWithId2 = settlement.LocationComplex.GetLocationWithId("tavern");
				if (CampaignMission.Current.Location == locationWithId)
				{
					this.AddPeopleToTownCenter(settlement, unusedUsablePointCount, CampaignTime.Now.IsDayTime);
				}
				if (CampaignMission.Current.Location == locationWithId2)
				{
					this.AddPeopleToTownTavern(settlement, unusedUsablePointCount);
				}
			}
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00044CF4 File Offset: 0x00042EF4
		private void AddPeopleToTownTavern(Settlement settlement, Dictionary<string, int> unusedUsablePointCount)
		{
			Location locationWithId = settlement.LocationComplex.GetLocationWithId("tavern");
			int num;
			unusedUsablePointCount.TryGetValue("npc_common", out num);
			MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(settlement.Position.ToVec2());
			bool flag = weatherEventInPosition == MapWeatherModel.WeatherEvent.HeavyRain || weatherEventInPosition == MapWeatherModel.WeatherEvent.Blizzard;
			if (num > 0)
			{
				int num2 = (int)((float)num * (0.3f + (flag ? 0.2f : 0f)));
				if (num2 > 0)
				{
					locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateTownsManForTavern), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, num2);
				}
				int num3 = (int)((float)num * (0.1f + (flag ? 0.2f : 0f)));
				if (num3 > 0)
				{
					locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateTownsWomanForTavern), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, num3);
				}
			}
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00044DCC File Offset: 0x00042FCC
		private void AddPeopleToTownCenter(Settlement settlement, Dictionary<string, int> unusedUsablePointCount, bool isDayTime)
		{
			Location locationWithId = settlement.LocationComplex.GetLocationWithId("center");
			CultureObject culture = settlement.Culture;
			int num;
			unusedUsablePointCount.TryGetValue("npc_common", out num);
			int num2;
			unusedUsablePointCount.TryGetValue("npc_common_limited", out num2);
			float num3 = (float)(num + num2) * 0.65000004f;
			if (num3 != 0f)
			{
				float num4 = MBMath.ClampFloat(this.GetConfigValue() / num3, 0f, 1f);
				float num5 = this.GetSpawnRate(settlement) * num4;
				if (num > 0)
				{
					int num6 = (int)((float)num * 0.2f * num5);
					if (num6 > 0)
					{
						locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateTownsMan), culture, LocationCharacter.CharacterRelations.Neutral, num6);
					}
					int num7 = (int)((float)num * 0.15f * num5);
					if (num7 > 0)
					{
						locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateTownsWoman), culture, LocationCharacter.CharacterRelations.Neutral, num7);
					}
				}
				MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(settlement.Position.ToVec2());
				bool flag = weatherEventInPosition == MapWeatherModel.WeatherEvent.HeavyRain || weatherEventInPosition == MapWeatherModel.WeatherEvent.Blizzard;
				if (isDayTime && !flag)
				{
					if (num2 > 0)
					{
						int num8 = (int)((float)num2 * 0.15f * num5);
						if (num8 > 0)
						{
							locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateTownsManCarryingStuff), culture, LocationCharacter.CharacterRelations.Neutral, num8);
						}
						int num9 = (int)((float)num2 * 0.1f * num5);
						if (num9 > 0)
						{
							locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateTownsWomanCarryingStuff), culture, LocationCharacter.CharacterRelations.Neutral, num9);
						}
						int num10 = (int)((float)num2 * 0.05f * num5);
						if (num10 > 0)
						{
							locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateMaleChild), culture, LocationCharacter.CharacterRelations.Neutral, num10);
							locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateFemaleChild), culture, LocationCharacter.CharacterRelations.Neutral, num10);
							locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateMaleTeenager), culture, LocationCharacter.CharacterRelations.Neutral, num10);
							locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateFemaleTeenager), culture, LocationCharacter.CharacterRelations.Neutral, num10);
						}
					}
					int num11 = 0;
					if (unusedUsablePointCount.TryGetValue("spawnpoint_cleaner", out num11))
					{
						locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateBroomsWoman), culture, LocationCharacter.CharacterRelations.Neutral, num11);
					}
					if (unusedUsablePointCount.TryGetValue("npc_dancer", out num11))
					{
						locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateDancer), culture, LocationCharacter.CharacterRelations.Neutral, num11);
					}
					if (settlement.IsTown && unusedUsablePointCount.TryGetValue("npc_beggar", out num11))
					{
						locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateFemaleBeggar), culture, LocationCharacter.CharacterRelations.Neutral, (num11 == 1) ? 0 : (num11 / 2));
						locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(CommonTownsfolkCampaignBehavior.CreateMaleBeggar), culture, LocationCharacter.CharacterRelations.Neutral, (num11 == 1) ? 1 : (num11 / 2));
					}
				}
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0004504C File Offset: 0x0004324C
		public static string GetActionSetSuffixAndMonsterForItem(string itemId, int race, bool isFemale, out Monster monster)
		{
			monster = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(race, "_settlement");
			uint num = <PrivateImplementationDetails>.ComputeStringHash(itemId);
			if (num <= 2354022098U)
			{
				if (num <= 524654717U)
				{
					if (num != 330511441U)
					{
						if (num != 423989003U)
						{
							if (num != 524654717U)
							{
								goto IL_020B;
							}
							if (!(itemId == "_to_carry_bed_convolute_g"))
							{
								goto IL_020B;
							}
							return "_villager_carry_on_shoulder";
						}
						else
						{
							if (!(itemId == "_to_carry_bed_convolute_a"))
							{
								goto IL_020B;
							}
							return "_villager_carry_front";
						}
					}
					else
					{
						if (!(itemId == "_to_carry_bd_basket_a"))
						{
							goto IL_020B;
						}
						return "_villager_with_backpack";
					}
				}
				else if (num != 1406916035U)
				{
					if (num != 1726492488U)
					{
						if (num != 2354022098U)
						{
							goto IL_020B;
						}
						if (!(itemId == "_to_carry_kitchen_pot_c"))
						{
							goto IL_020B;
						}
						return "_villager_carry_right_hand";
					}
					else if (!(itemId == "_to_carry_foods_watermelon_a"))
					{
						goto IL_020B;
					}
				}
				else
				{
					if (!(itemId == "_to_carry_arm_kitchen_pot_c"))
					{
						goto IL_020B;
					}
					return "_villager_carry_right_arm";
				}
			}
			else if (num <= 3512086304U)
			{
				if (num != 2481184366U)
				{
					if (num != 3004030871U)
					{
						if (num != 3512086304U)
						{
							goto IL_020B;
						}
						if (!(itemId == "_to_carry_bd_fabric_c"))
						{
							goto IL_020B;
						}
					}
					else
					{
						if (!(itemId == "_to_carry_foods_basket_apple"))
						{
							goto IL_020B;
						}
						return "_villager_carry_over_head_v2";
					}
				}
				else
				{
					if (!(itemId == "_to_carry_kitchen_pitcher_a"))
					{
						goto IL_020B;
					}
					return "_villager_carry_over_head";
				}
			}
			else if (num <= 3737849652U)
			{
				if (num != 3710634116U)
				{
					if (num != 3737849652U)
					{
						goto IL_020B;
					}
					if (!(itemId == "_to_carry_merchandise_hides_b"))
					{
						goto IL_020B;
					}
					return "_villager_with_backpack";
				}
				else
				{
					if (!(itemId == "practice_spear_t1"))
					{
						goto IL_020B;
					}
					return "_villager_with_staff";
				}
			}
			else if (num != 4035495654U)
			{
				if (num != 4038602446U)
				{
					goto IL_020B;
				}
				if (!(itemId == "simple_sparth_axe_t2"))
				{
					goto IL_020B;
				}
				return "_villager_carry_axe";
			}
			else
			{
				if (!(itemId == "_to_carry_foods_pumpkin_a"))
				{
					goto IL_020B;
				}
				return "_villager_carry_front_v2";
			}
			return "_villager_carry_right_side";
			IL_020B:
			return "_villager_carry_right_hand";
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0004526C File Offset: 0x0004346C
		public static Tuple<string, Monster> GetRandomTownsManActionSetAndMonster(int race)
		{
			int num = MBRandom.RandomInt(3);
			Monster monster;
			if (num == 0)
			{
				monster = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(race, "_settlement");
				return new Tuple<string, Monster>(ActionSetCode.GenerateActionSetNameWithSuffix(monster, false, "_villager"), monster);
			}
			if (num != 1)
			{
				monster = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(race, "_settlement");
				return new Tuple<string, Monster>(ActionSetCode.GenerateActionSetNameWithSuffix(monster, false, "_villager_3"), monster);
			}
			monster = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(race, "_settlement_slow");
			return new Tuple<string, Monster>(ActionSetCode.GenerateActionSetNameWithSuffix(monster, false, "_villager_2"), monster);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x000452E8 File Offset: 0x000434E8
		public static Tuple<string, Monster> GetRandomTownsWomanActionSetAndMonster(int race)
		{
			Monster monster;
			if (MBRandom.RandomInt(4) == 0)
			{
				monster = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(race, "_settlement_fast");
				return new Tuple<string, Monster>(ActionSetCode.GenerateActionSetNameWithSuffix(monster, true, "_villager"), monster);
			}
			monster = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(race, "_settlement_slow");
			return new Tuple<string, Monster>(ActionSetCode.GenerateActionSetNameWithSuffix(monster, true, "_villager_2"), monster);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0004533C File Offset: 0x0004353C
		private static LocationCharacter CreateTownsMan(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townsman = culture.Townsman;
			Tuple<string, Monster> randomTownsManActionSetAndMonster = CommonTownsfolkCampaignBehavior.GetRandomTownsManActionSetAndMonster(townsman.Race);
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townsman, out num, out num2, "");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(townsman, -1, null, default(UniqueTroopDescriptor))).Monster(randomTownsManActionSetAndMonster.Item2).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "npc_common", false, relation, randomTownsManActionSetAndMonster.Item1, true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x000453D8 File Offset: 0x000435D8
		private static LocationCharacter CreateTownsManForTavern(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townsman = culture.Townsman;
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(townsman.Race, "_settlement_slow");
			string text;
			if (culture.StringId.ToLower() == "aserai" || culture.StringId.ToLower() == "khuzait")
			{
				text = ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, townsman.IsFemale, "_villager_in_aserai_tavern");
			}
			else
			{
				text = ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, townsman.IsFemale, "_villager_in_tavern");
			}
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townsman, out num, out num2, "TavernVisitor");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(townsman, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_common", true, relation, text, true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x000454C4 File Offset: 0x000436C4
		private static LocationCharacter CreateTownsWomanForTavern(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townswoman = culture.Townswoman;
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(townswoman.Race, "_settlement_slow");
			string text;
			if (culture.StringId.ToLower() == "aserai" || culture.StringId.ToLower() == "khuzait")
			{
				text = ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, townswoman.IsFemale, "_warrior_in_aserai_tavern");
			}
			else
			{
				text = ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, townswoman.IsFemale, "_warrior_in_tavern");
			}
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townswoman, out num, out num2, "TavernVisitor");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(townswoman, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_common", true, relation, text, true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x000455B0 File Offset: 0x000437B0
		private static LocationCharacter CreateTownsManCarryingStuff(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townsman = culture.Townsman;
			string randomStuff = SettlementHelper.GetRandomStuff(false);
			Monster monster;
			string actionSetSuffixAndMonsterForItem = CommonTownsfolkCampaignBehavior.GetActionSetSuffixAndMonsterForItem(randomStuff, townsman.Race, false, out monster);
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townsman, out num, out num2, "TownsfolkCarryingStuff");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(townsman, -1, null, default(UniqueTroopDescriptor))).Monster(monster).Age(MBRandom.RandomInt(num, num2));
			ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(randomStuff);
			LocationCharacter locationCharacter = new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_common_limited", false, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, townsman.IsFemale, actionSetSuffixAndMonsterForItem), true, false, @object, false, false, true, null, false);
			if (@object == null)
			{
				locationCharacter.PrefabNamesForBones.Add(agentData.AgentMonster.MainHandItemBoneIndex, randomStuff);
			}
			return locationCharacter;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0004569C File Offset: 0x0004389C
		private static LocationCharacter CreateTownsWoman(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townswoman = culture.Townswoman;
			Tuple<string, Monster> randomTownsWomanActionSetAndMonster = CommonTownsfolkCampaignBehavior.GetRandomTownsWomanActionSetAndMonster(townswoman.Race);
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townswoman, out num, out num2, "");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(townswoman, -1, null, default(UniqueTroopDescriptor))).Monster(randomTownsWomanActionSetAndMonster.Item2).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "npc_common", false, relation, randomTownsWomanActionSetAndMonster.Item1, true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00045738 File Offset: 0x00043938
		private static LocationCharacter CreateMaleChild(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townsmanChild = culture.TownsmanChild;
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(townsmanChild.Race, "_child");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townsmanChild, out num, out num2, "Child");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(townsmanChild, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "npc_common_limited", false, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, townsmanChild.IsFemale, "_child"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x000457EC File Offset: 0x000439EC
		private static LocationCharacter CreateFemaleChild(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townswomanChild = culture.TownswomanChild;
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(townswomanChild.Race, "_child");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townswomanChild, out num, out num2, "Child");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(townswomanChild, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "npc_common_limited", false, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, townswomanChild.IsFemale, "_child"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000458A0 File Offset: 0x00043AA0
		private static LocationCharacter CreateMaleTeenager(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townsmanTeenager = culture.TownsmanTeenager;
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(townsmanTeenager.Race, "_child");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townsmanTeenager, out num, out num2, "Teenager");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(townsmanTeenager, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "npc_common_limited", false, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, townsmanTeenager.IsFemale, "_villager"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00045954 File Offset: 0x00043B54
		private static LocationCharacter CreateFemaleTeenager(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townswomanTeenager = culture.TownswomanTeenager;
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(townswomanTeenager.Race, "_child");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townswomanTeenager, out num, out num2, "Teenager");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(townswomanTeenager, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "npc_common_limited", false, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, townswomanTeenager.IsFemale, "_villager"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00045A08 File Offset: 0x00043C08
		private static LocationCharacter CreateTownsWomanCarryingStuff(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townswoman = culture.Townswoman;
			string randomStuff = SettlementHelper.GetRandomStuff(true);
			Monster monster;
			string actionSetSuffixAndMonsterForItem = CommonTownsfolkCampaignBehavior.GetActionSetSuffixAndMonsterForItem(randomStuff, townswoman.Race, false, out monster);
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townswoman, out num, out num2, "TownsfolkCarryingStuff");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(townswoman, -1, null, default(UniqueTroopDescriptor))).Monster(monster).Age(MBRandom.RandomInt(num, num2));
			ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(randomStuff);
			LocationCharacter locationCharacter = new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_common_limited", false, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, townswoman.IsFemale, actionSetSuffixAndMonsterForItem), true, false, @object, false, false, true, null, false);
			if (@object == null)
			{
				locationCharacter.PrefabNamesForBones.Add(agentData.AgentMonster.MainHandItemBoneIndex, randomStuff);
			}
			return locationCharacter;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00045AF4 File Offset: 0x00043CF4
		public static LocationCharacter CreateBroomsWoman(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject townswoman = culture.Townswoman;
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(townswoman.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(townswoman, out num, out num2, "BroomsWoman");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(townswoman, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "spawnpoint_cleaner", false, relation, null, true, false, null, false, false, true, null, false);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00045B8C File Offset: 0x00043D8C
		private static LocationCharacter CreateDancer(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject femaleDancer = culture.FemaleDancer;
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(femaleDancer.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(femaleDancer, out num, out num2, "Dancer");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(femaleDancer, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_dancer", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_dancer"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00045C40 File Offset: 0x00043E40
		public static LocationCharacter CreateMaleBeggar(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject beggar = culture.Beggar;
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(beggar.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(beggar, out num, out num2, "Beggar");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(beggar, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_beggar", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_beggar"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00045CF4 File Offset: 0x00043EF4
		public static LocationCharacter CreateFemaleBeggar(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject femaleBeggar = culture.FemaleBeggar;
			Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(femaleBeggar.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(femaleBeggar, out num, out num2, "Beggar");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(femaleBeggar, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_beggar", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_beggar"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x04000480 RID: 1152
		public const float TownsmanSpawnPercentageMale = 0.2f;

		// Token: 0x04000481 RID: 1153
		public const float TownsmanSpawnPercentageFemale = 0.15f;

		// Token: 0x04000482 RID: 1154
		public const float TownsmanSpawnPercentageLimitedMale = 0.15f;

		// Token: 0x04000483 RID: 1155
		public const float TownsmanSpawnPercentageLimitedFemale = 0.1f;

		// Token: 0x04000484 RID: 1156
		public const float TownOtherPeopleSpawnPercentage = 0.05f;

		// Token: 0x04000485 RID: 1157
		public const float TownsmanSpawnPercentageTavernMale = 0.3f;

		// Token: 0x04000486 RID: 1158
		public const float TownsmanSpawnPercentageTavernFemale = 0.1f;

		// Token: 0x04000487 RID: 1159
		public const float BeggarSpawnPercentage = 0.33f;
	}
}
