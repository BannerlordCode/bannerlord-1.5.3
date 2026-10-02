using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000039 RID: 57
	public static class CampaignCheats
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0001878D File Offset: 0x0001698D
		public static bool CheckCheatUsage(ref string ErrorType)
		{
			if (Campaign.Current == null)
			{
				ErrorType = "Campaign was not started.";
				return false;
			}
			if (!Game.Current.CheatMode)
			{
				ErrorType = "Cheat mode is disabled!";
				return false;
			}
			ErrorType = "";
			return true;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x000187BC File Offset: 0x000169BC
		public static bool CheckParameters(List<string> strings, int ParameterCount)
		{
			if (strings.Count == 0)
			{
				return ParameterCount == 0;
			}
			return strings.Count == ParameterCount;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x000187D4 File Offset: 0x000169D4
		public static bool CheckHelp(List<string> strings)
		{
			return strings.Count != 0 && strings[0].ToLower() == "help";
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000187F6 File Offset: 0x000169F6
		public static Settlement GetDefaultSettlement
		{
			get
			{
				if (Hero.MainHero.HomeSettlement == null)
				{
					return Town.AllTowns.GetRandomElement<Town>().Settlement;
				}
				return Hero.MainHero.HomeSettlement;
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001881E File Offset: 0x00016A1E
		private static bool IsValueAcceptable(float value)
		{
			return value <= 10000f;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0001882C File Offset: 0x00016A2C
		public static List<string> GetSeparatedNames(List<string> strings, bool removeEmptySpaces = false)
		{
			string text = "|";
			List<string> list = new List<string>();
			List<int> list2 = new List<int>(strings.Count);
			for (int i = 0; i < strings.Count; i++)
			{
				if (strings[i] == text)
				{
					list2.Add(i);
				}
			}
			list2.Add(strings.Count);
			int num = 0;
			for (int j = 0; j < list2.Count; j++)
			{
				int num2 = list2[j];
				string text2 = CampaignCheats.ConcatenateString(strings.GetRange(num, num2 - num));
				num = num2 + 1;
				list.Add(removeEmptySpaces ? text2.Replace(" ", "") : text2);
			}
			return list;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000188E4 File Offset: 0x00016AE4
		public static string ConcatenateString(List<string> strings)
		{
			if (strings == null || strings.IsEmpty<string>())
			{
				return string.Empty;
			}
			string text = strings[0];
			if (strings.Count > 1)
			{
				for (int i = 1; i < strings.Count; i++)
				{
					text = text + " " + strings[i];
				}
			}
			return text;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00018938 File Offset: 0x00016B38
		[CommandLineFunctionality.CommandLineArgumentFunction("export_hero", "campaign")]
		private static string ExportHero(List<string> strings)
		{
			string empty = string.Empty;
			if (!CampaignCheats.CheckCheatUsage(ref empty))
			{
				return empty;
			}
			string text = "Format is \"campaign.export_hero [filenamewithoutextension] | [nameofhero]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0];
			string text3 = separatedNames[1];
			PlatformFilePath saveFilePath = FileDriver.GetSaveFilePath(text2 + ".char");
			if (FileHelper.FileExists(saveFilePath))
			{
				return "File " + text2 + " already exists";
			}
			Hero hero;
			string text4;
			if (CampaignCheats.TryGetObject<Hero>(text3, out hero, out text4, null))
			{
				CharacterData.ExportCharacter(hero, saveFilePath.FileFullPath);
				return "Success";
			}
			return text4 + "\n" + text;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000189F8 File Offset: 0x00016BF8
		[CommandLineFunctionality.CommandLineArgumentFunction("import_main_hero", "campaign")]
		public static string ImportMainHero(List<string> strings)
		{
			string empty = string.Empty;
			if (!CampaignCheats.CheckCheatUsage(ref empty))
			{
				return empty;
			}
			string text = "Format is \"campaign.import_main_hero [filenamewithoutextension]\".";
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return text;
			}
			string text3;
			try
			{
				string text2 = CampaignCheats.ConcatenateString(strings);
				PlatformFilePath saveFilePath = FileDriver.GetSaveFilePath(text2 + ".char");
				if (!FileHelper.FileExists(saveFilePath))
				{
					text3 = "File " + text2 + " doesn't exists";
				}
				else if (FileHelper.GetFileContent(saveFilePath) == null)
				{
					text3 = "Can't read file: " + text2 + ". It's possible that it's corrupted.";
				}
				else
				{
					CharacterData.ImportCharacter(Hero.MainHero, saveFilePath.FileFullPath);
					text3 = "Main hero was imported successfully.";
				}
			}
			catch
			{
				text3 = "An error occurred";
			}
			return text3;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00018AB0 File Offset: 0x00016CB0
		[CommandLineFunctionality.CommandLineArgumentFunction("export_main_hero", "campaign")]
		public static string ExportMainHero(List<string> strings)
		{
			string empty = string.Empty;
			if (!CampaignCheats.CheckCheatUsage(ref empty))
			{
				return empty;
			}
			string text = "Format is \"campaign.export_main_hero [filenamewithoutextension]\".";
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return text;
			}
			string text2;
			try
			{
				PlatformFilePath saveFilePath = FileDriver.GetSaveFilePath(CampaignCheats.ConcatenateString(strings) + ".char");
				if (FileHelper.FileExists(saveFilePath))
				{
					text2 = "File already exists";
				}
				else
				{
					CharacterData.ExportCharacter(Hero.MainHero, saveFilePath.FileFullPath);
					text2 = "Main hero is exported to " + saveFilePath.FileFullPath;
				}
			}
			catch
			{
				text2 = "An error occurred";
			}
			return text2;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00018B48 File Offset: 0x00016D48
		[CommandLineFunctionality.CommandLineArgumentFunction("set_hero_crafting_stamina", "campaign")]
		public static string SetCraftingStamina(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_hero_crafting_stamina [HeroName] | [Stamina]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			ICraftingCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return "Can not found ICrafting Campaign Behavior!\n" + text;
			}
			int num = 0;
			if (!int.TryParse(separatedNames[1], out num) || num < 0 || num > 100)
			{
				return string.Concat(new object[] { "Please enter a valid number between 0-100 number is: ", num, "\n", text });
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			Hero hero;
			string text2;
			if (CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text2, (Hero x) => x.IsAlive && (x.Occupation == Occupation.Lord || x.Occupation == Occupation.Wanderer)))
			{
				int num2 = (int)((float)(campaignBehavior.GetMaxHeroCraftingStamina(hero) * num) / 100f);
				campaignBehavior.SetHeroCraftingStamina(hero, num2);
				return "Success";
			}
			return text2 + "\n" + text;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00018C6C File Offset: 0x00016E6C
		[CommandLineFunctionality.CommandLineArgumentFunction("set_hero_culture", "campaign")]
		public static string SetHeroCulture(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_hero_culture [HeroName] | [CultureName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			CultureObject cultureObject;
			string text2;
			if (!CampaignCheats.TryGetObject<CultureObject>(separatedNames[1], out cultureObject, out text2, null))
			{
				return text2 + "\n" + text;
			}
			Hero hero;
			string text3;
			if (!CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text3, (Hero x) => x.Occupation == Occupation.Lord || x.Occupation == Occupation.Wanderer))
			{
				return text3 + "\n" + text;
			}
			if (hero.Culture == cultureObject)
			{
				return string.Format("Hero culture is already {0}", cultureObject.Name);
			}
			hero.Culture = cultureObject;
			return "Success";
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00018D48 File Offset: 0x00016F48
		[CommandLineFunctionality.CommandLineArgumentFunction("set_clan_culture", "campaign")]
		public static string SetClanCulture(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_clan_culture [ClanName] | [CultureName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			CultureObject cultureObject;
			string text2;
			if (!CampaignCheats.TryGetObject<CultureObject>(separatedNames[1], out cultureObject, out text2, null))
			{
				return text2 + "\n" + text;
			}
			Clan clan;
			if (!CampaignCheats.TryGetObject<Clan>(separatedNames[0], out clan, out text2, null))
			{
				return text2 + "\n" + text;
			}
			if (clan.Culture == cultureObject)
			{
				return string.Format("Clan culture is already {0}", cultureObject.Name);
			}
			clan.Culture = cultureObject;
			return "Success";
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00018E04 File Offset: 0x00017004
		[CommandLineFunctionality.CommandLineArgumentFunction("add_skill_xp_to_hero", "campaign")]
		public static string AddSkillXpToHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			Hero hero = Hero.MainHero;
			int num = 100;
			string text = "Format is \"campaign.add_skill_xp_to_hero [HeroName] | [SkillName] | [PositiveNumber]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			if (!CampaignCheats.CheckParameters(strings, 0))
			{
				List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
				string text3;
				if (separatedNames.Count == 1)
				{
					string text2 = "";
					if (int.TryParse(separatedNames[0], out num))
					{
						if (num <= 0)
						{
							return "Please enter a positive number\n" + text;
						}
						foreach (SkillObject skillObject in Skills.All)
						{
							hero.HeroDeveloper.AddSkillXp(skillObject, (float)num, true, true);
							int num2 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject) * (float)num);
							text2 += string.Format("{0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill.\n", new object[] { num, num2, hero.Name, skillObject.Name });
						}
						return text2;
					}
					else
					{
						CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text3, null);
						num = 100;
						if (hero == null)
						{
							hero = Hero.MainHero;
							string text4 = separatedNames[0].ToLower();
							foreach (SkillObject skillObject2 in Skills.All)
							{
								if (skillObject2.Name.ToString().Replace(" ", "").ToLower()
									.Equals(text4, StringComparison.InvariantCultureIgnoreCase) || skillObject2.StringId.Replace(" ", "").ToLower() == text4)
								{
									if (hero.GetSkillValue(skillObject2) < 300)
									{
										hero.HeroDeveloper.AddSkillXp(skillObject2, (float)num, true, true);
										int num2 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject2) * (float)num);
										return string.Format("Input {0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill. ", new object[] { num, num2, hero.Name, skillObject2.Name });
									}
									return string.Format("{0} value for {1} is already at max.. ", skillObject2, hero);
								}
							}
							return text;
						}
						foreach (SkillObject skillObject3 in Skills.All)
						{
							hero.HeroDeveloper.AddSkillXp(skillObject3, (float)num, true, true);
							int num2 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject3) * (float)num);
							text2 += string.Format("{0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill.\n", new object[] { num, num2, hero.Name, skillObject3.Name });
						}
						return text2;
					}
				}
				else
				{
					if (separatedNames.Count == 2)
					{
						string text5;
						CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text5, null);
						if (hero != null)
						{
							if (int.TryParse(separatedNames[1], out num))
							{
								if (num <= 0)
								{
									return "Please enter a positive number\n" + text;
								}
								using (List<SkillObject>.Enumerator enumerator = Skills.All.GetEnumerator())
								{
									if (!enumerator.MoveNext())
									{
										goto IL_070C;
									}
									SkillObject skillObject4 = enumerator.Current;
									if (hero.GetSkillValue(skillObject4) < 300)
									{
										hero.HeroDeveloper.AddSkillXp(skillObject4, (float)num, true, true);
										int num3 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject4) * (float)num);
										return string.Format("Input {0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill. ", new object[] { num, num3, hero.Name, skillObject4.Name });
									}
									return string.Format("{0} value for {1} is already at max.. ", skillObject4, hero);
								}
							}
							num = 100;
							string text6 = separatedNames[1];
							foreach (SkillObject skillObject5 in Skills.All)
							{
								if (skillObject5.Name.ToString().Replace(" ", "").Equals(text6.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase) || skillObject5.StringId == text6.Replace(" ", ""))
								{
									if (hero.GetSkillValue(skillObject5) < 300)
									{
										hero.HeroDeveloper.AddSkillXp(skillObject5, (float)num, true, true);
										int num4 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject5) * (float)num);
										return string.Format("Input {0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill. ", new object[] { num, num4, hero.Name, skillObject5.Name });
									}
									return string.Format("{0} value for {1} is already at max.. ", skillObject5, hero);
								}
							}
							return "Skill not found.\n" + text;
						}
						hero = Hero.MainHero;
						if (!int.TryParse(separatedNames[1], out num))
						{
							return text;
						}
						if (num <= 0)
						{
							return "Please enter a positive number\n" + text;
						}
						string text7 = separatedNames[0];
						foreach (SkillObject skillObject6 in Skills.All)
						{
							if (skillObject6.Name.ToString().Replace(" ", "").Equals(text7.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase) || skillObject6.StringId == text7.Replace(" ", ""))
							{
								if (hero.GetSkillValue(skillObject6) < 300)
								{
									hero.HeroDeveloper.AddSkillXp(skillObject6, (float)num, true, true);
									int num5 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject6) * (float)num);
									return string.Format("Input {0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill. ", new object[] { num, num5, hero.Name, skillObject6.Name });
								}
								return string.Format("{0} value for {1} is already at max.. ", skillObject6, hero);
							}
						}
						return "Skill not found.\n" + text;
					}
					IL_070C:
					if (separatedNames.Count != 3)
					{
						return text;
					}
					if (!int.TryParse(separatedNames[2], out num) || num < 0)
					{
						return "Please enter a positive number\n" + text;
					}
					string text8;
					CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text8, null);
					if (hero == null)
					{
						return text8 + "\n" + text;
					}
					string text9 = separatedNames[1];
					foreach (SkillObject skillObject7 in Skills.All)
					{
						if (skillObject7.Name.ToString().Replace(" ", "").Equals(text9.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase) || skillObject7.StringId == text9.Replace(" ", ""))
						{
							if (hero.GetSkillValue(skillObject7) < 300)
							{
								hero.HeroDeveloper.AddSkillXp(skillObject7, (float)num, true, true);
								int num6 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject7) * (float)num);
								return string.Format("Input {0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill. ", new object[] { num, num6, hero.Name, skillObject7.Name });
							}
							return string.Format("{0} value for {1} is already at max.. ", skillObject7, hero);
						}
					}
					return "Skill not found.\n" + text;
				}
				return text3;
			}
			if (hero != null)
			{
				string text10 = "";
				foreach (SkillObject skillObject8 in Skills.All)
				{
					hero.HeroDeveloper.AddSkillXp(skillObject8, (float)num, true, true);
					int num7 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject8) * (float)num);
					text10 += string.Format("{0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill.\n", new object[] { num, num7, hero.Name, skillObject8.Name });
				}
				return text10;
			}
			return "Wrong Input.\n" + text;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000196F4 File Offset: 0x000178F4
		[CommandLineFunctionality.CommandLineArgumentFunction("print_player_traits", "campaign")]
		public static string PrintPlayerTrait(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.print_player_traits\".";
			}
			string text = "";
			foreach (TraitObject traitObject in TraitObject.All)
			{
				text = string.Concat(new object[]
				{
					text,
					traitObject.Name.ToString(),
					" Trait Level:  ",
					Hero.MainHero.GetTraitLevel(traitObject),
					" Trait Xp: ",
					Campaign.Current.PlayerTraitDeveloper.GetPropertyValue(traitObject),
					"\n"
				});
			}
			return text;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000197C8 File Offset: 0x000179C8
		[CommandLineFunctionality.CommandLineArgumentFunction("show_settlements", "campaign")]
		public static string ShowSettlements(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			foreach (Settlement settlement in Settlement.All)
			{
				if (!settlement.IsHideout)
				{
					settlement.IsVisible = true;
					settlement.IsInspected = true;
				}
			}
			return "Success";
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00019840 File Offset: 0x00017A40
		[CommandLineFunctionality.CommandLineArgumentFunction("set_skills_of_hero", "campaign")]
		public static string SetSkillsOfGivenHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_skills_of_hero [HeroName] | [Level]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			int num = -1;
			if (!int.TryParse(separatedNames[1], out num))
			{
				return "Level must be a number\n" + text;
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			Hero hero;
			string text2;
			if (!CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text2, (Hero x) => x.IsAlive && (x.Occupation == Occupation.Lord || x.Occupation == Occupation.Wanderer)))
			{
				return text2 + "\n" + text;
			}
			if (num > 0 && num <= 300)
			{
				hero.CharacterObject.Level = 0;
				hero.HeroDeveloper.ClearHero();
				int num2 = MathF.Min(num / 25 + 1, 10);
				int maxFocusPerSkill = Campaign.Current.Models.CharacterDevelopmentModel.MaxFocusPerSkill;
				foreach (SkillObject skillObject in Skills.All)
				{
					if (hero.HeroDeveloper.GetFocus(skillObject) + num2 > maxFocusPerSkill)
					{
						num2 = maxFocusPerSkill;
					}
					hero.HeroDeveloper.AddFocus(skillObject, num2, false);
					hero.HeroDeveloper.SetInitialSkillLevel(skillObject, num);
				}
				if (hero.Clan == Clan.PlayerClan)
				{
					for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
					{
						ItemObject itemObject = hero.BattleEquipment[equipmentIndex].Item;
						if (itemObject != null && itemObject.Difficulty > num)
						{
							MobileParty partyBelongedTo = hero.PartyBelongedTo;
							if (partyBelongedTo != null)
							{
								partyBelongedTo.ItemRoster.AddToCounts(hero.BattleEquipment[equipmentIndex], 1);
							}
							hero.BattleEquipment[equipmentIndex].Clear();
						}
						itemObject = hero.CivilianEquipment[equipmentIndex].Item;
						if (itemObject != null && itemObject.Difficulty > num)
						{
							MobileParty partyBelongedTo2 = hero.PartyBelongedTo;
							if (partyBelongedTo2 != null)
							{
								partyBelongedTo2.ItemRoster.AddToCounts(hero.CivilianEquipment[equipmentIndex], 1);
							}
							hero.CivilianEquipment[equipmentIndex].Clear();
						}
					}
				}
				hero.HeroDeveloper.UnspentFocusPoints = 0;
				return string.Format("{0}'s skills are set to level {1}.", hero.Name, num);
			}
			return string.Format("Level must be between 0 - {0}.", 300);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00019AE8 File Offset: 0x00017CE8
		[CommandLineFunctionality.CommandLineArgumentFunction("hide_settlements", "campaign")]
		public static string HideSettlements(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			foreach (Settlement settlement in Settlement.All)
			{
				if (!settlement.IsHideout && !(settlement.SettlementComponent is RetirementSettlementComponent))
				{
					settlement.IsVisible = false;
					settlement.IsInspected = false;
				}
			}
			return "Success";
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00019B70 File Offset: 0x00017D70
		[CommandLineFunctionality.CommandLineArgumentFunction("set_skill_player", "campaign")]
		public static string SetSkillMainHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_skill_player [SkillName] | [LevelValue]\".";
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
			if (separatedNames.Count != 2 || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			SkillObject skillObject;
			string text2;
			if (!CampaignCheats.TryGetObject<SkillObject>(separatedNames[0], out skillObject, out text2, null))
			{
				return text2 + "\n" + text;
			}
			int num;
			if (!int.TryParse(separatedNames[1], out num))
			{
				return "Please enter a number\n" + text;
			}
			if (num <= 0 || num > 300)
			{
				return string.Format("Level must be between 0 - {0}.", 300);
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			Hero.MainHero.HeroDeveloper.SetInitialSkillLevel(skillObject, num);
			Hero.MainHero.HeroDeveloper.InitializeSkillXp(skillObject);
			return "Success";
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00019C48 File Offset: 0x00017E48
		[CommandLineFunctionality.CommandLineArgumentFunction("set_skill_of_all_companions", "campaign")]
		public static string SetSkillCompanion(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_skill_of_all_companions [SkillName] | [LevelValue]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			if (Clan.PlayerClan.Companions.Count == 0)
			{
				return "There is no companion in player clan";
			}
			SkillObject skillObject;
			string text2;
			if (!CampaignCheats.TryGetObject<SkillObject>(separatedNames[0], out skillObject, out text2, null))
			{
				return text2 + "\n" + text;
			}
			int num = 1;
			if (!int.TryParse(separatedNames[1], out num))
			{
				return "Please enter a number\n" + text;
			}
			if (num <= 0 || num > 300)
			{
				return string.Format("Level must be between 0 - {0}.", 300);
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			foreach (HeroDeveloper heroDeveloper in Clan.PlayerClan.Companions.Select<Hero, HeroDeveloper>((Hero x) => x.HeroDeveloper))
			{
				heroDeveloper.SetInitialSkillLevel(skillObject, num);
				heroDeveloper.InitializeSkillXp(skillObject);
			}
			return "Success";
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00019DA4 File Offset: 0x00017FA4
		[CommandLineFunctionality.CommandLineArgumentFunction("set_all_companion_skills", "campaign")]
		public static string SetAllSkillsOfAllCompanions(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_all_companion_skills [LevelValue]\".";
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			foreach (SkillObject skillObject in Skills.All)
			{
				int num = 1;
				if (strings.Count == 0 || !int.TryParse(strings[0], out num))
				{
					return "Please enter a number\n" + text;
				}
				if (num <= 0 || num > 300)
				{
					return string.Format("Level must be between 0 - {0}.", 300);
				}
				if (!CampaignCheats.IsValueAcceptable((float)num))
				{
					return "The value is too much";
				}
				foreach (Hero hero in Clan.PlayerClan.Companions)
				{
					hero.HeroDeveloper.SetInitialSkillLevel(skillObject, num);
					hero.HeroDeveloper.InitializeSkillXp(skillObject);
				}
			}
			return "Success";
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00019EE4 File Offset: 0x000180E4
		[CommandLineFunctionality.CommandLineArgumentFunction("set_all_heroes_skills", "campaign")]
		public static string SetAllHeroSkills(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_all_heroes_skills [LevelValue]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num;
			if (strings.Count == 0 || !int.TryParse(strings[0], out num))
			{
				return "Please enter a positive number\n" + text;
			}
			foreach (Hero hero in Hero.AllAliveHeroes.Where<Hero>((Hero x) => x.IsActive && x.PartyBelongedTo != null).ToList<Hero>())
			{
				foreach (SkillObject skillObject in Skills.All)
				{
					if (num <= 0 || num > 300)
					{
						return string.Format("Level must be between 0 - {0}.", 300);
					}
					if (!CampaignCheats.IsValueAcceptable((float)num))
					{
						return "The value is too much";
					}
					hero.HeroDeveloper.SetInitialSkillLevel(skillObject, num);
					hero.HeroDeveloper.InitializeSkillXp(skillObject);
				}
			}
			return "Success";
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001A044 File Offset: 0x00018244
		[CommandLineFunctionality.CommandLineArgumentFunction("set_loyalty_of_settlement", "campaign")]
		public static string SetLoyaltyOfSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_loyalty_of_settlement [SettlementName] | [loyalty]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			int num;
			if (!int.TryParse(separatedNames[1], out num))
			{
				return "Please enter a positive number\n" + text;
			}
			if (num > 100 || num < 0)
			{
				return "Loyalty has to be in the range of 0 to 100";
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			string text2 = separatedNames[0];
			Settlement settlement;
			string text3;
			if (!CampaignCheats.TryGetObject<Settlement>(text2, out settlement, out text3, null))
			{
				return string.Concat(new string[] { text3, ": ", text2, "\n", text });
			}
			if (settlement.IsVillage)
			{
				return "Settlement must be castle or town";
			}
			settlement.Town.Loyalty = (float)num;
			return "Success";
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001A134 File Offset: 0x00018334
		[CommandLineFunctionality.CommandLineArgumentFunction("set_prosperity_of_settlement", "campaign")]
		public static string SetProsperityOfSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_prosperity_of_settlement [SettlementName/SettlementID] | [Value]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0];
			Settlement settlement;
			string text3;
			if (!CampaignCheats.TryGetObject<Settlement>(text2, out settlement, out text3, null))
			{
				return string.Concat(new string[] { text3, ": ", text2, "\n", text });
			}
			if (settlement.IsVillage)
			{
				return "Settlement must be castle or town";
			}
			float num;
			if (!float.TryParse(separatedNames[1], out num) || num < 0f)
			{
				return "Please enter a positive number\n" + text;
			}
			if (!CampaignCheats.IsValueAcceptable(num))
			{
				return "The value is too much";
			}
			settlement.Town.Prosperity = num;
			return "Success";
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0001A224 File Offset: 0x00018424
		[CommandLineFunctionality.CommandLineArgumentFunction("set_militia_of_settlement", "campaign")]
		public static string SetMilitiaOfSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_militia_of_settlement [SettlementName/SettlementID] | [Value]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0];
			Settlement settlement;
			string text3;
			if (!CampaignCheats.TryGetObject<Settlement>(text2, out settlement, out text3, null))
			{
				return string.Concat(new string[] { text3, ": ", text2, "\n", text });
			}
			float num;
			if (!float.TryParse(separatedNames[1], out num))
			{
				return "Please enter a number\n" + text;
			}
			if (!CampaignCheats.IsValueAcceptable(num))
			{
				return "The value is too much";
			}
			settlement.Militia = num;
			return "Success";
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001A2F8 File Offset: 0x000184F8
		[CommandLineFunctionality.CommandLineArgumentFunction("set_security_of_settlement", "campaign")]
		public static string SetSecurityOfSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_security_of_settlement [SettlementName/SettlementID] | [Value]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0];
			Settlement settlement;
			string text3;
			if (!CampaignCheats.TryGetObject<Settlement>(text2, out settlement, out text3, null))
			{
				return string.Concat(new string[] { text3, ": ", text2, "\n", text });
			}
			if (settlement.IsVillage)
			{
				return "Settlement must be castle or town";
			}
			float num;
			if (!float.TryParse(separatedNames[1], out num))
			{
				return "Please enter a number\n" + text;
			}
			if (!CampaignCheats.IsValueAcceptable(num))
			{
				return "The value is too much";
			}
			settlement.Town.Security = num;
			return "Success";
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001A3E0 File Offset: 0x000185E0
		[CommandLineFunctionality.CommandLineArgumentFunction("set_food_of_settlement", "campaign")]
		public static string SetFoodOfSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_food_of_settlement [SettlementName/SettlementID] | [Value]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0];
			Settlement settlement;
			string text3;
			if (!CampaignCheats.TryGetObject<Settlement>(text2, out settlement, out text3, null))
			{
				return string.Concat(new string[] { text3, ": ", text2, "\n", text });
			}
			if (settlement.IsVillage)
			{
				return "Settlement must be castle or town";
			}
			float num;
			if (!float.TryParse(separatedNames[1], out num))
			{
				return "Please enter a number\n" + text;
			}
			if (!CampaignCheats.IsValueAcceptable(num))
			{
				return "The value is too much";
			}
			settlement.Town.FoodStocks = num;
			return "Success";
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001A4C8 File Offset: 0x000186C8
		[CommandLineFunctionality.CommandLineArgumentFunction("set_hearth_of_settlement", "campaign")]
		public static string SetHearthOfSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_hearth_of_settlement [SettlementName/SettlementID] | [Value]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0];
			Settlement settlement;
			string text3;
			if (!CampaignCheats.TryGetObject<Settlement>(text2, out settlement, out text3, (Settlement x) => x.IsVillage))
			{
				return string.Concat(new string[] { text3, ": ", text2, "\n", text });
			}
			if (settlement.Village == null)
			{
				return "Settlement doesn't have hearth variable.";
			}
			float num;
			if (!float.TryParse(separatedNames[1], out num))
			{
				return "Please enter a number\n" + text;
			}
			if (!CampaignCheats.IsValueAcceptable(num))
			{
				return "The value is too much";
			}
			settlement.Village.Hearth = num;
			return "Success";
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001A5CC File Offset: 0x000187CC
		[CommandLineFunctionality.CommandLineArgumentFunction("show_relation", "campaign")]
		public static string ShowHeroRelation(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = (Game.Current.IsDevelopmentMode ? "Format is \"campaign.show_relation [HeroName] | [HeroName] \".\n" : "Format is \"campaign.show_relation [HeroName] | [HeroName] \".\n");
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0];
			string text3 = separatedNames[1];
			Hero hero;
			string text4;
			if (!CampaignCheats.TryGetObject<Hero>(text2, out hero, out text4, null))
			{
				return text4;
			}
			Hero hero2;
			string text5;
			if (!CampaignCheats.TryGetObject<Hero>(text3, out hero2, out text5, null))
			{
				return text5;
			}
			return string.Format("{0} relation to {1}: {2}", hero.Name, hero2.Name, hero.GetRelation(hero2));
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001A68C File Offset: 0x0001888C
		[CommandLineFunctionality.CommandLineArgumentFunction("add_hero_relation", "campaign")]
		public static string AddHeroRelation(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			bool isDevelopmentMode = Game.Current.IsDevelopmentMode;
			string text = (isDevelopmentMode ? "Format is \"campaign.add_hero_relation [HeroName]/All | [OtherHeroName(optional)] | [Value] \".\n" : "Format is \"campaign.add_hero_relation [HeroName] | [OtherHeroName(optional)] | [Value] \".\n");
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			Hero mainHero = Hero.MainHero;
			string text2 = string.Empty;
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count == 3)
			{
				string text3;
				if (!CampaignCheats.TryGetObject<Hero>(separatedNames[1], out mainHero, out text3, null))
				{
					return text3;
				}
				text2 = separatedNames[2];
			}
			else
			{
				if (separatedNames.Count != 2)
				{
					return text;
				}
				text2 = separatedNames[1];
			}
			int num;
			if (!int.TryParse(text2, out num))
			{
				return "Please enter a number\n" + text;
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			string text4 = separatedNames[0];
			Hero hero;
			string text5;
			CampaignCheats.TryGetObject<Hero>(text4, out hero, out text5, null);
			if (hero == mainHero)
			{
				return "Can not add relation to same heroes.";
			}
			if (hero != null)
			{
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, mainHero, num, true);
				return "Success";
			}
			if (string.Equals(text4, "all", StringComparison.OrdinalIgnoreCase) && isDevelopmentMode)
			{
				foreach (Hero hero2 in Hero.AllAliveHeroes)
				{
					if (!hero2.IsHumanPlayerCharacter && hero2 != mainHero)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero2, mainHero, num, true);
					}
				}
				return "Success";
			}
			return text5 + "\n" + text;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001A818 File Offset: 0x00018A18
		[CommandLineFunctionality.CommandLineArgumentFunction("print_player_party_position", "campaign")]
		public static string PrintMainPartyPosition(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.print_player_party_position\".";
			}
			return MobileParty.MainParty.Position.X + " " + MobileParty.MainParty.Position.Y;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001A888 File Offset: 0x00018A88
		[CommandLineFunctionality.CommandLineArgumentFunction("add_crafting_materials", "campaign")]
		public static string AddCraftingMaterials(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.add_crafting_materials\".";
			}
			for (int i = 0; i < 9; i++)
			{
				PartyBase.MainParty.ItemRoster.AddToCounts(Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem((CraftingMaterials)i), 100);
			}
			return "100 pieces for each crafting material is added to the player inventory.";
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001A8F8 File Offset: 0x00018AF8
		[CommandLineFunctionality.CommandLineArgumentFunction("heal_player_party", "campaign")]
		public static string HealMainParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.heal_player_party\".";
			}
			if (MobileParty.MainParty.MapEvent == null)
			{
				for (int i = 0; i < PartyBase.MainParty.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = PartyBase.MainParty.MemberRoster.GetElementCopyAtIndex(i);
					if (elementCopyAtIndex.Character.IsHero)
					{
						elementCopyAtIndex.Character.HeroObject.Heal(elementCopyAtIndex.Character.HeroObject.MaxHitPoints, false);
					}
					else
					{
						MobileParty.MainParty.Party.AddToMemberRosterElementAtIndex(i, 0, -PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(i));
					}
				}
				return "Success";
			}
			return "Party shouldn't be in a map event.";
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001A9C8 File Offset: 0x00018BC8
		[CommandLineFunctionality.CommandLineArgumentFunction("declare_war", "campaign")]
		public static string DeclareWar(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is campaign.declare_war [Faction1] | [Faction2]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0];
			string text3 = separatedNames[1];
			Kingdom kingdom;
			string text4;
			CampaignCheats.TryGetObject<Kingdom>(separatedNames[0], out kingdom, out text4, null);
			Kingdom kingdom2;
			CampaignCheats.TryGetObject<Kingdom>(separatedNames[1], out kingdom2, out text4, null);
			IFaction faction;
			if (kingdom != null)
			{
				faction = kingdom;
			}
			else
			{
				Clan clan;
				string text5;
				if (!CampaignCheats.TryGetObject<Clan>(separatedNames[0], out clan, out text5, null))
				{
					return string.Concat(new string[] { text5, ": ", text2, "\n", text });
				}
				faction = clan;
			}
			IFaction faction2;
			if (kingdom2 != null)
			{
				faction2 = kingdom2;
			}
			else
			{
				Clan clan2;
				string text6;
				if (!CampaignCheats.TryGetObject<Clan>(separatedNames[1], out clan2, out text6, null))
				{
					return string.Concat(new string[] { text6, ": ", text3, "\n", text });
				}
				faction2 = clan2;
			}
			if (faction == faction2 || faction.MapFaction == faction2.MapFaction)
			{
				return "Can't declare between same factions";
			}
			if (!faction.IsMapFaction)
			{
				return faction.Name + " is bound to a kingdom.";
			}
			if (!faction2.IsMapFaction)
			{
				return faction.Name + " is bound to a kingdom.";
			}
			if (faction.IsAtWarWith(faction2))
			{
				return "The factions already at war";
			}
			if (faction.IsEliminated)
			{
				return "Faction 1 is eliminated";
			}
			if (faction2.IsEliminated)
			{
				return "Faction 2 is eliminated";
			}
			DeclareWarAction.ApplyByDefault(faction, faction2);
			return string.Concat(new object[] { "War declared between ", faction.Name, " and ", faction2.Name });
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001ABA4 File Offset: 0x00018DA4
		[CommandLineFunctionality.CommandLineArgumentFunction("add_item_to_player_party", "campaign")]
		public static string AddItemToPlayerParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_item_to_player_party [ItemId] | [ModifierId] | [Amount]\"\n If amount is not entered only 1 item will be given.\n Modifier name is optional.";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(separatedNames[0]);
			if (@object == null)
			{
				return "Item is not found\n" + text;
			}
			int num = 1;
			if (separatedNames.Count == 1)
			{
				PartyBase.MainParty.ItemRoster.AddToCounts(@object, num);
				return @object.Name + " has been given to the main party.";
			}
			ItemModifier object2 = Game.Current.ObjectManager.GetObject<ItemModifier>(separatedNames[1]);
			if (object2 != null)
			{
				EquipmentElement equipmentElement = new EquipmentElement(@object, object2, null, false);
				if (separatedNames.Count > 2 && int.TryParse(separatedNames[2], out num) && num >= 1)
				{
					if (!CampaignCheats.IsValueAcceptable((float)num))
					{
						return "The value is too much";
					}
					MobileParty.MainParty.ItemRoster.AddToCounts(equipmentElement, num);
				}
				else
				{
					MobileParty.MainParty.ItemRoster.AddToCounts(equipmentElement, 1);
				}
				return equipmentElement.GetModifiedItemName() + " has been given to the main party.";
			}
			if (!int.TryParse(separatedNames[1], out num) || num < 1)
			{
				return "Second parameter is invalid.\n" + text;
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			MobileParty.MainParty.ItemRoster.AddToCounts(@object, num);
			return @object.Name + " has been given to the main party.";
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001AD1C File Offset: 0x00018F1C
		[CommandLineFunctionality.CommandLineArgumentFunction("declare_peace", "campaign")]
		public static string DeclarePeace(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "campaign.declare_peace [Faction1] | [Faction2]";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0];
			string text3 = separatedNames[1];
			Kingdom kingdom;
			string text4;
			CampaignCheats.TryGetObject<Kingdom>(separatedNames[0], out kingdom, out text4, null);
			Kingdom kingdom2;
			CampaignCheats.TryGetObject<Kingdom>(separatedNames[1], out kingdom2, out text4, null);
			IFaction faction;
			if (kingdom != null)
			{
				faction = kingdom;
			}
			else
			{
				Clan clan;
				string text5;
				if (!CampaignCheats.TryGetObject<Clan>(separatedNames[0], out clan, out text5, null))
				{
					return string.Concat(new string[] { text5, ": ", text2, "\n", text });
				}
				faction = clan;
			}
			IFaction faction2;
			if (kingdom2 != null)
			{
				faction2 = kingdom2;
			}
			else
			{
				Clan clan2;
				string text6;
				if (!CampaignCheats.TryGetObject<Clan>(separatedNames[1], out clan2, out text6, null))
				{
					return string.Concat(new string[] { text6, ": ", text3, "\n", text });
				}
				faction2 = clan2;
			}
			if (faction == faction2 || faction.MapFaction == faction2.MapFaction)
			{
				return "Can't declare between same factions";
			}
			if (!faction.IsMapFaction)
			{
				return faction.Name + " is bound to a kingdom.";
			}
			if (!faction2.IsMapFaction)
			{
				return faction.Name + " is bound to a kingdom.";
			}
			if (FactionManager.IsAtConstantWarAgainstFaction(faction, faction2))
			{
				return "There is constant war between factions, peace can't be declared";
			}
			if (!faction.IsAtWarWith(faction2))
			{
				return "The factions not at war";
			}
			MakePeaceAction.Apply(faction, faction2);
			return string.Concat(new object[] { "Peace declared between ", faction.Name, " and ", faction2.Name });
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001AEE8 File Offset: 0x000190E8
		[CommandLineFunctionality.CommandLineArgumentFunction("add_influence", "campaign")]
		public static string AddInfluence(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.add_influence [Number]\". If Number is not entered, 100 influence will be added.";
			}
			int num = 100;
			bool flag = false;
			if (!CampaignCheats.CheckParameters(strings, 0))
			{
				flag = int.TryParse(strings[0], out num);
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			if (flag || CampaignCheats.CheckParameters(strings, 0))
			{
				float num2 = MBMath.ClampFloat((float)num, -200f, float.MaxValue);
				ChangeClanInfluenceAction.Apply(Clan.PlayerClan, num2);
				return string.Format("The influence of player is changed by {0} to {1} ", num2, Clan.PlayerClan.Influence);
			}
			return "Please enter a positive number\nFormat is \"campaign.add_influence [Number]\".";
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0001AF90 File Offset: 0x00019190
		[CommandLineFunctionality.CommandLineArgumentFunction("add_renown_to_clan", "campaign")]
		public static string AddRenown(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_renown [ClanName] | [PositiveNumber]\". \n If number is not specified, 100 will be added. \n If clan name is not specified, player clan will get the renown.";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 100;
			string text2 = "";
			Hero hero = Hero.MainHero;
			bool flag = false;
			if (CampaignCheats.CheckParameters(strings, 1))
			{
				if (!int.TryParse(strings[0], out num))
				{
					num = 100;
					text2 = CampaignCheats.ConcatenateString(strings);
					hero = CampaignCheats.GetClanLeader(text2);
					flag = true;
				}
			}
			else if (!CampaignCheats.CheckParameters(strings, 0))
			{
				List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
				if (separatedNames.Count == 2 && !int.TryParse(separatedNames[1], out num))
				{
					return "Please enter a positive number\n" + text;
				}
				text2 = separatedNames[0];
				hero = CampaignCheats.GetClanLeader(text2);
				flag = true;
			}
			if (hero != null)
			{
				if (!CampaignCheats.IsValueAcceptable((float)num))
				{
					return "The value is too much";
				}
				if (num > 0)
				{
					GainRenownAction.Apply(hero, (float)num, false);
					return string.Format("Added {0} renown to ", num) + hero.Clan.Name;
				}
				return "Please enter a positive number\n" + text;
			}
			else
			{
				if (flag)
				{
					return "Clan: " + text2 + " not found.\n" + text;
				}
				return "Wrong Input.\n" + text;
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001B0BC File Offset: 0x000192BC
		[CommandLineFunctionality.CommandLineArgumentFunction("add_gold_to_hero", "campaign")]
		public static string AddGoldToHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_gold_to_hero [HeroName] | [PositiveNumber]\".\n If number is not specified, 1000 will be added. \n If hero name is not specified, player's gold will change.";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1000;
			Hero mainHero = Hero.MainHero;
			string empty = string.Empty;
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				GiveGoldAction.ApplyBetweenCharacters(null, mainHero, num, true);
				return "Success";
			}
			if (CampaignCheats.CheckParameters(strings, 1) && !int.TryParse(strings[0], out num))
			{
				num = 1000;
				CampaignCheats.TryGetObject<Hero>(CampaignCheats.ConcatenateString(strings), out mainHero, out empty, null);
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count == 2)
			{
				if (!int.TryParse(separatedNames[1], out num))
				{
					return "Please enter a number\n" + text;
				}
				CampaignCheats.TryGetObject<Hero>(separatedNames[0], out mainHero, out empty, null);
			}
			if (separatedNames.Count == 1 && !int.TryParse(separatedNames[0], out num))
			{
				CampaignCheats.TryGetObject<Hero>(separatedNames[0], out mainHero, out empty, null);
			}
			if (mainHero == null)
			{
				return empty + "\n" + text;
			}
			if (mainHero.Gold + num < 0 || mainHero.Gold + num > 100000000)
			{
				return "Hero's gold must be between 0-100000000.";
			}
			GiveGoldAction.ApplyBetweenCharacters(null, mainHero, num, true);
			return string.Format("{0}'s denars changed by {1}.", mainHero.Name, num);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001B208 File Offset: 0x00019408
		[CommandLineFunctionality.CommandLineArgumentFunction("add_building_level", "campaign")]
		public static string AddDevelopment(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_building_level [SettlementName] | [Building]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			Settlement settlement;
			string text2;
			if (!CampaignCheats.TryGetObject<Settlement>(separatedNames[0], out settlement, out text2, null) || !settlement.IsFortification)
			{
				return text2 + "\n" + text;
			}
			if (settlement.IsUnderSiege || settlement.IsUnderRaid || settlement.Party.MapEvent != null)
			{
				return "Requested settlement is not suitable right now to take this action";
			}
			string text3 = separatedNames[1];
			List<Building> settlementBuildings = settlement.Town.Buildings.ToList<Building>();
			BuildingType buildingType;
			if (!CampaignCheats.TryGetObject<BuildingType>(text3, out buildingType, out text2, (BuildingType x) => settlementBuildings.ContainsQ<Building>((Building y) => y.BuildingType == x)))
			{
				return text2 + "\n" + text;
			}
			Building building = settlementBuildings.First<Building>((Building x) => x.BuildingType == buildingType);
			if (building.CurrentLevel < 3)
			{
				Building building2 = building;
				int currentLevel = building2.CurrentLevel;
				building2.CurrentLevel = currentLevel + 1;
				CampaignEventDispatcher.Instance.OnBuildingLevelChanged(settlement.Town, building, 1);
				return string.Concat(new object[]
				{
					building.BuildingType.Name,
					" level increased to ",
					building.CurrentLevel,
					" at ",
					settlement.Name
				});
			}
			return building.BuildingType.Name + " is already at max level!";
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0001B3AC File Offset: 0x000195AC
		[CommandLineFunctionality.CommandLineArgumentFunction("activate_all_policies_for_player_kingdom", "campaign")]
		public static string ActivateAllPolicies(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.activate_all_policies_for_player_kingdom";
			}
			if (Clan.PlayerClan.Kingdom != null)
			{
				Kingdom kingdom = Clan.PlayerClan.Kingdom;
				foreach (PolicyObject policyObject in PolicyObject.All)
				{
					if (!kingdom.ActivePolicies.Contains(policyObject))
					{
						kingdom.AddPolicy(policyObject);
					}
				}
				return "All policies are now active for player kingdom.";
			}
			return "Player is not in a kingdom.";
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001B458 File Offset: 0x00019658
		[CommandLineFunctionality.CommandLineArgumentFunction("set_player_trait", "campaign")]
		public static string SetPlayerReputationTrait(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_player_trait [Trait] | [Number]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			int num;
			if (!int.TryParse(separatedNames[1], out num))
			{
				return "Please enter a number\n" + text;
			}
			TraitObject traitObject;
			string text2;
			if (!CampaignCheats.TryGetObject<TraitObject>(separatedNames[0], out traitObject, out text2, null))
			{
				return text2 + "\n" + text;
			}
			if (num >= traitObject.MinValue && num <= traitObject.MaxValue)
			{
				Hero.MainHero.SetTraitLevel(traitObject, num);
				TraitLevelingHelper.UpdateTraitXPAccordingToTraitLevels();
				return string.Format("Set {0} to {1}.", traitObject.Name, num);
			}
			return string.Format("Number must be between {0} and {1}.", traitObject.MinValue, traitObject.MaxValue);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0001B544 File Offset: 0x00019744
		[CommandLineFunctionality.CommandLineArgumentFunction("give_settlement_to_player", "campaign")]
		public static string GiveSettlementToPlayer(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.give_settlement_to_player [SettlementName/SettlementId]\nWrite \"campaign.give_settlement_to_player help\" to list available settlements.\nWrite \"campaign.give_settlement_to_player Calradia\" to give all settlements to player.";
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			if (text2.ToLower() == "help")
			{
				string text3 = "";
				text3 += "\n";
				text3 += "Available settlements";
				text3 += "\n";
				text3 += "==============================";
				text3 += "\n";
				foreach (Settlement settlement in MBObjectManager.Instance.GetObjectTypeList<Settlement>())
				{
					text3 = string.Concat(new object[] { text3, "Id: ", settlement.StringId, " Name: ", settlement.Name, "\n" });
				}
				return text3;
			}
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				return "Player cannot take ownership of a settlement during mercenary service.";
			}
			MBReadOnlyList<Settlement> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<Settlement>();
			if (text2.ToLower().Replace(" ", "") == "calradia")
			{
				foreach (Settlement settlement2 in objectTypeList)
				{
					if (settlement2.IsCastle || settlement2.IsTown)
					{
						ChangeOwnerOfSettlementAction.ApplyByDefault(Hero.MainHero, settlement2);
					}
				}
				return "You own all of Calradia now!";
			}
			Settlement settlement3;
			string text4;
			if (!CampaignCheats.TryGetObject<Settlement>(text2, out settlement3, out text4, null))
			{
				return text4 + "\n" + text;
			}
			if (settlement3.IsVillage)
			{
				return "Settlement must be castle or town.";
			}
			ChangeOwnerOfSettlementAction.ApplyByDefault(Hero.MainHero, settlement3);
			return settlement3.Name + " has been given to the player.";
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0001B744 File Offset: 0x00019944
		[CommandLineFunctionality.CommandLineArgumentFunction("give_settlement_to_kingdom", "campaign")]
		public static string GiveSettlementToKingdom(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.give_settlement_to_kingdom [SettlementName] | [KingdomName]";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			Settlement bound;
			string text2;
			if (!CampaignCheats.TryGetObject<Settlement>(separatedNames[0], out bound, out text2, null))
			{
				return text2 + "\n" + text;
			}
			if (bound.IsVillage)
			{
				bound = bound.Village.Bound;
			}
			Kingdom kingdom;
			if (!CampaignCheats.TryGetObject<Kingdom>(separatedNames[1], out kingdom, out text2, null))
			{
				return text2 + "\n" + text;
			}
			if (bound.MapFaction == kingdom)
			{
				return "Kingdom already owns the settlement.";
			}
			if (bound.IsVillage)
			{
				return "Settlement must be castle or town.";
			}
			ChangeOwnerOfSettlementAction.ApplyByDefault(kingdom.Leader, bound);
			return bound.Name + string.Format(" has been given to {0}.", kingdom.Leader.Name);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001B83C File Offset: 0x00019A3C
		[CommandLineFunctionality.CommandLineArgumentFunction("add_power_to_notable", "campaign")]
		public static string AddPowerToNotable(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is campaign.add_power_to_notable [HeroName] | [Number]";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			int num;
			if (!int.TryParse(separatedNames[1], out num))
			{
				return "Please enter a positive number\n" + text;
			}
			if (num <= 0)
			{
				return "Please enter a positive number\n" + text;
			}
			Hero hero;
			string text2;
			if (!CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text2, null))
			{
				return text2 + "\n" + text;
			}
			if (!hero.IsNotable)
			{
				return "Hero is not a notable.";
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			hero.AddPower((float)num);
			return string.Format("{0} power is {1}", hero.Name, hero.Power);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0001B920 File Offset: 0x00019B20
		[CommandLineFunctionality.CommandLineArgumentFunction("rule_your_faction", "campaign")]
		public static string LeadYourFaction(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.rule_your_faction\".";
			}
			if (Hero.MainHero.MapFaction.Leader != Hero.MainHero)
			{
				if (Hero.MainHero.MapFaction.IsKingdomFaction)
				{
					ChangeRulingClanAction.Apply(Hero.MainHero.MapFaction as Kingdom, Clan.PlayerClan);
				}
				else
				{
					(Hero.MainHero.MapFaction as Clan).SetLeader(Hero.MainHero);
				}
				return "Success";
			}
			return "Function execution failed.";
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0001B9B4 File Offset: 0x00019BB4
		[CommandLineFunctionality.CommandLineArgumentFunction("print_heroes_suitable_for_marriage", "campaign")]
		public static string PrintHeroesSuitableForMarriage(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"print_heroes_suitable_for_marriage\".";
			}
			List<Hero> list = new List<Hero>();
			List<Hero> list2 = new List<Hero>();
			foreach (Kingdom kingdom in Kingdom.All)
			{
				foreach (Hero hero in kingdom.AliveLords)
				{
					if (hero.CanMarry())
					{
						if (hero.IsFemale)
						{
							list.Add(hero);
						}
						else
						{
							list2.Add(hero);
						}
					}
				}
			}
			string text = "Maidens:\n";
			string text2 = "Suitors:\n";
			foreach (Hero hero2 in list)
			{
				TextObject textObject = ((hero2.PartyBelongedTo == null) ? TextObject.GetEmpty() : hero2.PartyBelongedTo.Name);
				text = string.Concat(new object[] { text, "Name: ", hero2.Name, " --- Clan: ", hero2.Clan, " --- Party:", textObject, "\n" });
			}
			foreach (Hero hero3 in list2)
			{
				TextObject textObject2 = ((hero3.PartyBelongedTo == null) ? TextObject.GetEmpty() : hero3.PartyBelongedTo.Name);
				text2 = string.Concat(new object[] { text2, "Name: ", hero3.Name, " --- Clan: ", hero3.Clan, " --- Party:", textObject2, "\n" });
			}
			return text + "\n" + text2;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001BBE4 File Offset: 0x00019DE4
		[CommandLineFunctionality.CommandLineArgumentFunction("marry_player_with_hero", "campaign")]
		public static string MarryPlayerWithHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.marry_player_with_hero [HeroName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			if (!Campaign.Current.Models.MarriageModel.IsSuitableForMarriage(Hero.MainHero))
			{
				return "Main hero is not suitable for marriage";
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			Hero hero;
			string text3;
			if (!CampaignCheats.TryGetObject<Hero>(text2, out hero, out text3, null))
			{
				return string.Concat(new string[]
				{
					text3,
					": ",
					text2.ToLower(),
					"\n",
					text
				});
			}
			MarriageModel marriageModel = Campaign.Current.Models.MarriageModel;
			if (marriageModel.IsCoupleSuitableForMarriage(Hero.MainHero, hero))
			{
				MarriageAction.Apply(Hero.MainHero, hero, true);
				return "Success";
			}
			if (!marriageModel.IsSuitableForMarriage(hero))
			{
				return string.Format("Hero: {0} is not suitable for marriage.", hero.Name);
			}
			if (!marriageModel.IsClanSuitableForMarriage(hero.Clan))
			{
				return string.Format("{0}'s clan is not suitable for marriage.", hero.Name);
			}
			if (!marriageModel.IsClanSuitableForMarriage(Hero.MainHero.Clan))
			{
				return "Main hero's clan is not suitable for marriage.";
			}
			Clan clan = Hero.MainHero.Clan;
			if (((clan != null) ? clan.Leader : null) == Hero.MainHero)
			{
				Clan clan2 = hero.Clan;
				if (((clan2 != null) ? clan2.Leader : null) == hero)
				{
					return "Clan leaders are not suitable for marriage.";
				}
			}
			if (!hero.IsFemale)
			{
				return "Hero is not female.";
			}
			DefaultMarriageModel defaultMarriageModel = new DefaultMarriageModel();
			if ((bool)typeof(DefaultMarriageModel).GetMethod("AreHeroesRelated", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(defaultMarriageModel, new object[]
			{
				Hero.MainHero,
				hero,
				1
			}))
			{
				return "Heroes are related.";
			}
			Hero courtedHeroInOtherClan = Romance.GetCourtedHeroInOtherClan(Hero.MainHero, hero);
			if (courtedHeroInOtherClan != null && courtedHeroInOtherClan != hero)
			{
				return string.Format("{0} has courted {1}.", courtedHeroInOtherClan.Name, Hero.MainHero.Name);
			}
			Hero courtedHeroInOtherClan2 = Romance.GetCourtedHeroInOtherClan(hero, Hero.MainHero);
			if (courtedHeroInOtherClan2 != null && courtedHeroInOtherClan2 != Hero.MainHero)
			{
				return string.Format("{0} has courted {1}.", courtedHeroInOtherClan2.Name, hero.Name);
			}
			return string.Concat(new object[]
			{
				"Marriage is not suitable between ",
				Hero.MainHero.Name,
				" and ",
				hero.Name,
				"\n"
			});
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001BE3C File Offset: 0x0001A03C
		[CommandLineFunctionality.CommandLineArgumentFunction("marry_hero_to_hero", "campaign")]
		public static string MarryHeroWithHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.marry_hero_to_hero [HeroName] | [HeroName]\".";
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2 || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			Hero hero;
			string text2;
			CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text2, null);
			Hero hero2;
			string text3;
			CampaignCheats.TryGetObject<Hero>(separatedNames[1], out hero2, out text3, null);
			if (hero != null && hero2 != null)
			{
				if (Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(hero, hero2))
				{
					MarriageAction.Apply(hero, hero2, true);
					return "Success";
				}
				return "They are not suitable for marriage";
			}
			else
			{
				if (hero == null)
				{
					return text2 + "\nCan't find a hero with name: " + separatedNames[0];
				}
				return text3 + "\nCan't find a hero with name: " + separatedNames[1];
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001BF04 File Offset: 0x0001A104
		[CommandLineFunctionality.CommandLineArgumentFunction("is_hero_suitable_for_marriage_with_player", "campaign")]
		public static string IsHeroSuitableForMarriageWithPlayer(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.is_hero_suitable_for_marriage_with_player [HeroName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			Hero hero;
			string text3;
			if (!CampaignCheats.TryGetObject<Hero>(text2, out hero, out text3, null))
			{
				return string.Concat(new string[]
				{
					text3,
					": ",
					text2.ToLower(),
					"\n",
					text
				});
			}
			if (Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(Hero.MainHero, hero))
			{
				return string.Concat(new object[]
				{
					"Marriage is suitable between ",
					Hero.MainHero.Name,
					" and ",
					hero.Name,
					"\n"
				});
			}
			return string.Concat(new object[]
			{
				"Marriage is not suitable between ",
				Hero.MainHero.Name,
				" and ",
				hero.Name,
				"\n"
			});
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001C018 File Offset: 0x0001A218
		[CommandLineFunctionality.CommandLineArgumentFunction("create_player_kingdom", "campaign")]
		public static string CreatePlayerKingdom(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings) || !CampaignCheats.CheckParameters(strings, 0))
			{
				return "Format is \"campaign.create_player_kingdom\".";
			}
			Campaign.Current.KingdomManager.CreateKingdom(Clan.PlayerClan.Name, Clan.PlayerClan.InformalName, Clan.PlayerClan.Culture, Clan.PlayerClan, Clan.PlayerClan.Name, null, null, Clan.PlayerClan.Name, null, "new_kingdom", null, null, null, null, null);
			return "Success";
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0001C0C8 File Offset: 0x0001A2C8
		[CommandLineFunctionality.CommandLineArgumentFunction("create_clan", "campaign")]
		public static string CreateRandomClan(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.create_clan [KingdomName]\".";
			if (CampaignCheats.CheckHelp(strings) || !CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			Kingdom kingdom;
			string text2;
			CampaignCheats.TryGetObject<Kingdom>(CampaignCheats.GetSeparatedNames(strings, false)[0], out kingdom, out text2, null);
			if (kingdom == null)
			{
				return text2 + "\n" + text;
			}
			CultureObject culture = kingdom.Culture;
			Settlement settlement;
			if ((settlement = kingdom.Settlements.FirstOrDefault<Settlement>((Settlement x) => x.IsTown)) == null)
			{
				settlement = kingdom.Settlements.GetRandomElement<Settlement>() ?? Settlement.All.FirstOrDefault<Settlement>((Settlement x) => x.IsTown && x.Culture == culture);
			}
			Settlement settlement2 = settlement;
			TextObject textObject = NameGenerator.Current.GenerateClanName(culture, settlement2);
			Clan clan = Clan.CreateClan("test_clan_" + Clan.All.Count);
			clan.ChangeClanName(textObject, textObject);
			clan.Culture = Kingdom.All.GetRandomElement<Kingdom>().Culture;
			clan.Banner = Banner.CreateRandomClanBanner(-1);
			clan.SetInitialHomeSettlement(settlement2);
			CharacterObject characterObject = culture.LordTemplates.FirstOrDefault<CharacterObject>((CharacterObject x) => x.Occupation == Occupation.Lord);
			if (characterObject == null)
			{
				return "Can't find a proper lord template.\n" + text;
			}
			Settlement settlement3 = kingdom.Settlements.GetRandomElement<Settlement>() ?? Settlement.All.FirstOrDefault<Settlement>((Settlement x) => x.IsTown && x.Culture == culture);
			Hero hero = HeroCreator.CreateSpecialHero(characterObject, settlement3, clan, null, MBRandom.RandomInt(18, 36));
			hero.HeroDeveloper.InitializeHeroDeveloper(true);
			hero.ChangeState(Hero.CharacterStates.Active);
			clan.SetLeader(hero);
			ChangeKingdomAction.ApplyByJoinToKingdom(clan, kingdom, default(CampaignTime), false);
			EnterSettlementAction.ApplyForCharacterOnly(hero, settlement2);
			GiveGoldAction.ApplyBetweenCharacters(null, hero, 15000, false);
			CampaignEventDispatcher.Instance.OnClanCreated(clan, false);
			return string.Format("{0} is added to {1}. Its leader is: {2}", clan.Name, kingdom.Name, hero.Name);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0001C2EC File Offset: 0x0001A4EC
		[CommandLineFunctionality.CommandLineArgumentFunction("lead_kingdom", "campaign")]
		public static string LeadKingdom(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (Hero.MainHero.IsKingdomLeader)
			{
				return "You are already leading your faction";
			}
			if (Clan.PlayerClan.Kingdom == null)
			{
				return "You are not in a kingdom";
			}
			ChangeRulingClanAction.Apply(Clan.PlayerClan.Kingdom, Clan.PlayerClan);
			return "OK";
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001C348 File Offset: 0x0001A548
		[CommandLineFunctionality.CommandLineArgumentFunction("print_criminal_ratings", "campaign")]
		public static string PrintCriminalRatingWithAllKingdoms(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Criminal Ratings\n";
			foreach (Kingdom kingdom in Kingdom.All)
			{
				text = string.Concat(new object[] { text, kingdom.Name, ": ", kingdom.MainHeroCrimeRating, "\n" });
			}
			return text;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001C3E4 File Offset: 0x0001A5E4
		[CommandLineFunctionality.CommandLineArgumentFunction("join_kingdom", "campaign")]
		public static string JoinKingdom(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.join_kingdom[KingdomName / FirstTwoCharactersOfKingdomName]\".\nWrite \"campaign.join_kingdom help\" to list available Kingdoms.";
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings).Replace(" ", "");
			if (text2.ToLower() == "help")
			{
				string text3 = "";
				text3 += "\n";
				text3 += "Format is \"campaign.join_kingdom [KingdomName/FirstTwoCharacterOfKingdomName]\".";
				text3 += "\n";
				text3 += "Available Kingdoms";
				text3 += "\n";
				foreach (Kingdom kingdom in Kingdom.All)
				{
					text3 = text3 + "Kingdom name " + kingdom.Name.ToString() + "\n";
				}
				return text3;
			}
			if (Hero.MainHero.IsKingdomLeader)
			{
				return "Cannot join a kingdom while leading a kingdom!";
			}
			Kingdom kingdom2;
			string text4;
			if (CampaignCheats.TryGetObject<Kingdom>(text2, out kingdom2, out text4, null))
			{
				ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, kingdom2, default(CampaignTime), true);
				return "Success";
			}
			return text4;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0001C530 File Offset: 0x0001A730
		[CommandLineFunctionality.CommandLineArgumentFunction("join_kingdom_as_mercenary", "campaign")]
		public static string JoinKingdomAsMercenary(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.join_kingdom_as_mercenary[KingdomName / FirstTwoCharactersOfKingdomName]\".\nWrite \"campaign.join_kingdom_as_mercenary help\" to list available Kingdoms.";
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings).Replace(" ", "");
			if (text2.ToLower() == "help")
			{
				string text3 = "";
				text3 += "\n";
				text3 += "Format is \"campaign.join_kingdom_as_mercenary [KingdomName/FirstTwoCharacterOfKingdomName]\".";
				text3 += "\n";
				text3 += "Available Kingdoms";
				text3 += "\n";
				foreach (Kingdom kingdom in Kingdom.All)
				{
					text3 = text3 + "Kingdom name " + kingdom.Name.ToString() + "\n";
				}
				return text3;
			}
			Kingdom kingdom2;
			string text4;
			if (CampaignCheats.TryGetObject<Kingdom>(text2, out kingdom2, out text4, null))
			{
				ChangeKingdomAction.ApplyByJoinFactionAsMercenary(Hero.MainHero.Clan, kingdom2, default(CampaignTime), 50, true);
				return "Success";
			}
			return string.Concat(new string[] { text4, ": ", text2, "\n", text });
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0001C694 File Offset: 0x0001A894
		[CommandLineFunctionality.CommandLineArgumentFunction("make_trade_agreement", "campaign")]
		public static string MakeTradeAgreement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is campaign.make_trade_agreement [Kingdom1] | [Kingdom2]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0];
			string text3 = separatedNames[1];
			Kingdom kingdom;
			string text4;
			CampaignCheats.TryGetObject<Kingdom>(separatedNames[0], out kingdom, out text4, null);
			Kingdom kingdom2;
			CampaignCheats.TryGetObject<Kingdom>(separatedNames[1], out kingdom2, out text4, null);
			if (kingdom == null || kingdom2 == null)
			{
				return "Cant find one of the kingdoms";
			}
			if (kingdom == kingdom2)
			{
				return "Can't declare between same factions";
			}
			TextObject textObject;
			if (!Campaign.Current.Models.TradeAgreementModel.CanMakeTradeAgreement(kingdom, kingdom2, true, out textObject, true))
			{
				return textObject.ToString();
			}
			if (kingdom.IsEliminated)
			{
				return "kingdom1 is eliminated";
			}
			if (kingdom2.IsEliminated)
			{
				return "kingdom2 is eliminated";
			}
			ITradeAgreementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
			if (campaignBehavior != null)
			{
				campaignBehavior.MakeTradeAgreement(kingdom, kingdom2, Campaign.Current.Models.TradeAgreementModel.GetTradeAgreementDurationInYears(kingdom, kingdom2));
			}
			return string.Concat(new object[] { "Trade agreement signed between ", kingdom.Name, " and ", kingdom2.Name });
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001C7CC File Offset: 0x0001A9CC
		[CommandLineFunctionality.CommandLineArgumentFunction("print_criminal_ratings", "campaign")]
		public static string PrintCriminalRatings(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.print_criminal_ratings";
			}
			string text = "";
			bool flag = true;
			foreach (Kingdom kingdom in Kingdom.All)
			{
				if (kingdom.MainHeroCrimeRating > 0f)
				{
					text = string.Concat(new object[] { text, kingdom.Name, "   criminal rating: ", kingdom.MainHeroCrimeRating, "\n" });
					flag = false;
				}
			}
			text += "-----------\n";
			foreach (Clan clan in Clan.NonBanditFactions)
			{
				if (clan.MainHeroCrimeRating > 0f)
				{
					text = string.Concat(new object[] { text, clan.Name, "   criminal rating: ", clan.MainHeroCrimeRating, "\n" });
					flag = false;
				}
			}
			if (flag)
			{
				return "You don't have any criminal rating.";
			}
			return text;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001C928 File Offset: 0x0001AB28
		[CommandLineFunctionality.CommandLineArgumentFunction("add_player_age", "campaign")]
		public static string SetMainHeroAge(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_player_age [PositiveNumber]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			float num = Hero.MainHero.Age;
			int num2 = -1;
			if (!int.TryParse(strings[0], out num2) || num2 < 0)
			{
				return "Please enter a positive number\n" + text;
			}
			num += (float)num2;
			if (num > (float)Campaign.Current.Models.AgeModel.MaxAge)
			{
				return string.Format("Age must be between {0} - {1}", Campaign.Current.Models.AgeModel.HeroComesOfAge, Campaign.Current.Models.AgeModel.MaxAge);
			}
			Hero.MainHero.SetBirthDay(HeroHelper.GetRandomBirthDayForAge(num));
			return "Success";
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001C9FC File Offset: 0x0001ABFC
		[CommandLineFunctionality.CommandLineArgumentFunction("set_main_party_attackable", "campaign")]
		public static string SetMainPartyAttackable(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is campaign.set_main_party_attackable [1/0]\".";
			if (CampaignCheats.CheckHelp(strings) || !CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			if (strings[0] == "0" || strings[0] == "1")
			{
				bool flag = strings[0] == "1";
				if (flag)
				{
					MobileParty.MainParty.IgnoreByOtherPartiesTill(CampaignTime.Now);
				}
				else
				{
					MobileParty.MainParty.IgnoreByOtherPartiesTill(CampaignTime.Never);
					foreach (MobileParty mobileParty in MobileParty.All)
					{
						if (mobileParty.TargetParty == MobileParty.MainParty && mobileParty.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction))
						{
							mobileParty.SetMoveModeHold();
						}
					}
				}
				return "Main party is" + (flag ? " " : " NOT ") + "attackable.";
			}
			return "Wrong input.\n" + text;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001CB24 File Offset: 0x0001AD24
		[CommandLineFunctionality.CommandLineArgumentFunction("add_morale_to_party", "campaign")]
		public static string AddMoraleToParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_morale_to_party [HeroName] | [Number]\".";
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2 || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 0;
			if (!int.TryParse(separatedNames[1], out num))
			{
				num = 100;
			}
			string text2 = separatedNames[0];
			if (text2.ToLower().Equals("all"))
			{
				foreach (MobileParty mobileParty in MobileParty.AllLordParties)
				{
					if (!mobileParty.IsMainParty)
					{
						float num2 = MBMath.ClampFloat((float)num, -10000f, 10000f);
						mobileParty.RecentEventsMorale += num2;
					}
				}
				return "All lords parties morale is changed";
			}
			Hero hero;
			string text3;
			if (!CampaignCheats.TryGetObject<Hero>(text2, out hero, out text3, (Hero x) => x.PartyBelongedTo != null))
			{
				return text3 + "\n" + text;
			}
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			if (partyBelongedTo != null)
			{
				float num3 = MBMath.ClampFloat((float)num, -10000f, 10000f);
				partyBelongedTo.RecentEventsMorale += num3;
				return string.Format("The base morale of {0}'s party changed by {1}.", hero.Name, num3);
			}
			return "Hero: " + text2 + " does not belonged to any party.\n" + text;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001CC98 File Offset: 0x0001AE98
		[CommandLineFunctionality.CommandLineArgumentFunction("boost_cohesion_of_army", "campaign")]
		public static string BoostCohesionOfArmy(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"boost_cohesion_of_army [ArmyLeaderName]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			Hero mainHero = Hero.MainHero;
			Army army = mainHero.PartyBelongedTo.Army;
			if (!CampaignCheats.CheckParameters(strings, 0))
			{
				string text2 = CampaignCheats.ConcatenateString(strings.GetRange(0, strings.Count));
				string text3;
				CampaignCheats.TryGetObject<Hero>(text2, out mainHero, out text3, null);
				if (mainHero == null)
				{
					return text3 + ".\n" + text;
				}
				if (mainHero.PartyBelongedTo == null)
				{
					return "Hero: " + text2 + " does not belong to any army.";
				}
				army = mainHero.PartyBelongedTo.Army;
				if (army == null)
				{
					return "Hero: " + text2 + " does not belong to any army.";
				}
			}
			if (army != null)
			{
				army.Cohesion = 100f;
				return string.Format("{0}'s army cohesion is boosted.", mainHero.Name);
			}
			return "Wrong input.\n" + text;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001CD7C File Offset: 0x0001AF7C
		[CommandLineFunctionality.CommandLineArgumentFunction("add_focus_points_to_hero", "campaign")]
		public static string AddFocusPointCheat(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_focus_points_to_hero [HeroName] | [PositiveNumber]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1;
			Hero hero;
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				Hero.MainHero.HeroDeveloper.UnspentFocusPoints = MBMath.ClampInt(Hero.MainHero.HeroDeveloper.UnspentFocusPoints + 1, 0, int.MaxValue);
				hero = Hero.MainHero;
				return string.Format("{0} focus points added to the {1}. ", num, hero.Name);
			}
			int num2 = 0;
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			bool flag2;
			if (separatedNames.Count == 1)
			{
				bool flag = int.TryParse(separatedNames[0], out num2);
				if (num2 <= 0 && flag)
				{
					return "Please enter a positive number\n" + text;
				}
				Hero.MainHero.HeroDeveloper.UnspentFocusPoints = MBMath.ClampInt(Hero.MainHero.HeroDeveloper.UnspentFocusPoints + num2, 0, 10000);
				hero = Hero.MainHero;
				flag2 = true;
				num = num2;
			}
			else
			{
				if (separatedNames.Count != 2)
				{
					return text;
				}
				if (int.TryParse(separatedNames[1], out num2))
				{
					string text2;
					if (!CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text2, null))
					{
						return text2;
					}
					hero.HeroDeveloper.UnspentFocusPoints = MBMath.ClampInt(hero.HeroDeveloper.UnspentFocusPoints + num2, 0, 10000);
					flag2 = true;
					num = num2;
				}
				else
				{
					string text3;
					if (!CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text3, null))
					{
						return text3;
					}
					hero.HeroDeveloper.UnspentFocusPoints = MBMath.ClampInt(hero.HeroDeveloper.UnspentFocusPoints + 1, 0, 10000);
					flag2 = true;
				}
			}
			if (flag2)
			{
				return string.Format("{0} focus points added to the {1}. ", num, hero.Name);
			}
			return "Check parameters \n" + text;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001CF40 File Offset: 0x0001B140
		[CommandLineFunctionality.CommandLineArgumentFunction("add_attribute_points_to_hero", "campaign")]
		public static string AddAttributePointsCheat(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_attribute_points_to_hero [HeroName] | [PositiveNumber]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1;
			Hero hero;
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				Hero.MainHero.HeroDeveloper.UnspentAttributePoints = MBMath.ClampInt(Hero.MainHero.HeroDeveloper.UnspentAttributePoints + 1, 0, 10000);
				hero = Hero.MainHero;
				return string.Format("{0} attribute points added to the {1}. ", num, hero.Name);
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			bool flag2;
			if (separatedNames.Count == 1)
			{
				int num2;
				bool flag = int.TryParse(separatedNames[0], out num2);
				if (num2 <= 0 || !flag)
				{
					return "Please enter a positive number\n" + text;
				}
				Hero.MainHero.HeroDeveloper.UnspentAttributePoints = MBMath.ClampInt(Hero.MainHero.HeroDeveloper.UnspentAttributePoints + num2, 0, 10000);
				hero = Hero.MainHero;
				flag2 = true;
				num = num2;
			}
			else
			{
				if (separatedNames.Count != 2)
				{
					return text;
				}
				int num3;
				if (int.TryParse(separatedNames[1], out num3))
				{
					string text2;
					if (!CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text2, null))
					{
						return text2;
					}
					hero.HeroDeveloper.UnspentAttributePoints = MBMath.ClampInt(hero.HeroDeveloper.UnspentAttributePoints + num3, 0, 10000);
					flag2 = true;
					num = num3;
				}
				else
				{
					string text3;
					if (!CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text3, null))
					{
						return text3;
					}
					hero.HeroDeveloper.UnspentAttributePoints = MBMath.ClampInt(hero.HeroDeveloper.UnspentAttributePoints + 1, 0, 10000);
					flag2 = true;
				}
			}
			if (flag2)
			{
				return string.Format("{0} attribute points added to the {1}. ", num, hero.Name);
			}
			return "Check parameters \n" + text;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001D0FC File Offset: 0x0001B2FC
		[CommandLineFunctionality.CommandLineArgumentFunction("print_tournaments", "campaign")]
		public static string PrintSettlementsWithTournament(List<string> strings)
		{
			string text = "";
			if (!CampaignCheats.CheckCheatUsage(ref text))
			{
				return text;
			}
			if (!Campaign.Current.IsDay)
			{
				return "Cant print tournaments. Wait day light.";
			}
			string text2 = "";
			foreach (Town town in Town.AllTowns)
			{
				if (Campaign.Current.TournamentManager.GetTournamentGame(town) != null)
				{
					text2 = text2 + town.Name + "\n";
				}
			}
			return text2;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001D198 File Offset: 0x0001B398
		public static string ConvertListToMultiLine(List<string> strings)
		{
			string text = "";
			foreach (string text2 in strings)
			{
				text = text + text2 + "\n";
			}
			return text;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001D1F4 File Offset: 0x0001B3F4
		[CommandLineFunctionality.CommandLineArgumentFunction("print_all_issues", "campaign")]
		public static string PrintAllIssues(List<string> strings)
		{
			string text = "";
			if (!CampaignCheats.CheckCheatUsage(ref text))
			{
				return text;
			}
			string text2 = "Total issue count : " + Campaign.Current.IssueManager.Issues.Count + "\n";
			int num = 0;
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
			{
				text2 = string.Concat(new object[]
				{
					text2,
					++num,
					") ",
					keyValuePair.Value.Title,
					", ",
					keyValuePair.Key,
					": ",
					keyValuePair.Value.IssueSettlement,
					"\n"
				});
			}
			return text2;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001D2F0 File Offset: 0x0001B4F0
		[CommandLineFunctionality.CommandLineArgumentFunction("give_workshop_to_player", "campaign")]
		public static string GiveWorkshopToPlayer(List<string> strings)
		{
			string text = "Format is \"campaign.give_workshop_to_player [SettlementName] | [workshop_name]\".";
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType) || CampaignCheats.CheckHelp(strings) || !CampaignCheats.CheckParameters(separatedNames, 2))
			{
				return text;
			}
			Settlement settlement;
			string text2;
			if (!CampaignCheats.TryGetObject<Settlement>(separatedNames[0], out settlement, out text2, null))
			{
				return text2;
			}
			if (!settlement.IsTown)
			{
				return "Settlement should be town\n";
			}
			WorkshopType workshopType;
			if (CampaignCheats.TryGetObject<WorkshopType>(separatedNames[1], out workshopType, out text2, null))
			{
				foreach (Workshop workshop in settlement.Town.Workshops)
				{
					if (workshop.WorkshopType == workshopType && workshop.Owner != Hero.MainHero)
					{
						int costForPlayer = Campaign.Current.Models.WorkshopModel.GetCostForPlayer(workshop);
						GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, costForPlayer, false);
						ChangeOwnerOfWorkshopAction.ApplyByPlayerBuying(workshop);
						return string.Format("Gave {0} to {1}", workshop.WorkshopType.Name, Hero.MainHero.Name);
					}
				}
				return "There is no suitable workshop to give player in requested settlement";
			}
			return text2;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001D400 File Offset: 0x0001B600
		[CommandLineFunctionality.CommandLineArgumentFunction("conceive_child", "campaign")]
		public static string MakePregnant(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (Hero.MainHero.Spouse == null)
			{
				if (!Game.Current.IsDevelopmentMode)
				{
					return "You need to be married to have a child, use \"campaign.marry_player_with_hero [HeroName]\" cheat to marry.";
				}
				Hero hero = Hero.AllAliveHeroes.FirstOrDefault<Hero>((Hero t) => t != Hero.MainHero && Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(Hero.MainHero, t));
				if (hero == null)
				{
					return "error";
				}
				MarriageAction.Apply(Hero.MainHero, hero, true);
				if (Hero.MainHero.IsFemale ? (!Hero.MainHero.IsPregnant) : (!Hero.MainHero.Spouse.IsPregnant))
				{
					MakePregnantAction.Apply(Hero.MainHero.IsFemale ? Hero.MainHero : Hero.MainHero.Spouse);
					return "Success";
				}
				return "You are expecting a child already.";
			}
			else
			{
				if (Hero.MainHero.IsFemale ? (!Hero.MainHero.IsPregnant) : (!Hero.MainHero.Spouse.IsPregnant))
				{
					MakePregnantAction.Apply(Hero.MainHero.IsFemale ? Hero.MainHero : Hero.MainHero.Spouse);
					return "Success";
				}
				return "You are expecting a child already.";
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001D540 File Offset: 0x0001B740
		public static Hero GenerateChild(Hero hero, bool isFemale, CultureObject culture)
		{
			if (hero.Spouse == null)
			{
				List<Hero> list = Hero.AllAliveHeroes.ToList<Hero>();
				list.Shuffle<Hero>();
				Hero hero2 = list.FirstOrDefault<Hero>((Hero t) => t != hero && Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(hero, t));
				if (hero2 != null)
				{
					MarriageAction.Apply(hero, hero2, true);
					if (hero.IsFemale ? (!hero.IsPregnant) : (!hero.Spouse.IsPregnant))
					{
						MakePregnantAction.Apply(hero.IsFemale ? hero : hero.Spouse);
					}
				}
			}
			Hero hero3 = (hero.IsFemale ? hero : hero.Spouse);
			Hero spouse = hero3.Spouse;
			Hero hero4 = HeroCreator.DeliverOffSpring(hero3, spouse, isFemale);
			hero4.Culture = culture;
			hero3.IsPregnant = false;
			return hero4;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001D634 File Offset: 0x0001B834
		[CommandLineFunctionality.CommandLineArgumentFunction("add_prisoner_to_party", "campaign")]
		public static string AddPrisonerToParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_prisoner_to_party [PrisonerName] | [CapturerName]\".";
			if (CampaignCheats.CheckHelp(strings) || CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0].Trim();
			string text3 = separatedNames[1].Trim();
			Hero hero;
			string text4;
			CampaignCheats.TryGetObject<Hero>(text2, out hero, out text4, null);
			Hero hero2;
			string text5;
			CampaignCheats.TryGetObject<Hero>(text3, out hero2, out text5, null);
			if (hero == null)
			{
				return text4 + "\n" + text;
			}
			if (hero2 == null)
			{
				return text5 + "\n" + text;
			}
			if (!hero2.IsActive || hero2.PartyBelongedTo == null)
			{
				return "Capturer hero is not active!";
			}
			if (!hero.IsActive || hero.IsHumanPlayerCharacter || (hero.Occupation != Occupation.Lord && hero.Occupation != Occupation.Wanderer))
			{
				return "Hero can't be taken as a prisoner!";
			}
			if (!FactionManager.IsAtWarAgainstFaction(hero.MapFaction, hero2.MapFaction))
			{
				return "Factions are not at war!";
			}
			if (hero.PartyBelongedTo != null)
			{
				if (hero.PartyBelongedTo.MapEvent != null)
				{
					return "prisoners party shouldn't be in a map event.";
				}
				if (hero.PartyBelongedTo.LeaderHero == hero)
				{
					DestroyPartyAction.Apply(null, hero.PartyBelongedTo);
				}
				else
				{
					hero.PartyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
				}
			}
			if (hero.IsPrisoner)
			{
				EndCaptivityAction.ApplyByEscape(hero, null, true);
			}
			if (hero.CurrentSettlement != null)
			{
				LeaveSettlementAction.ApplyForCharacterOnly(hero);
			}
			if (hero2.IsHumanPlayerCharacter)
			{
				hero.SetHasMet();
			}
			TakePrisonerAction.Apply(hero2.PartyBelongedTo.Party, hero);
			return "Success";
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001D7D8 File Offset: 0x0001B9D8
		[CommandLineFunctionality.CommandLineArgumentFunction("clear_settlement_defense", "campaign")]
		public static string ClearSettlementDefense(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.clear_settlement_defense [SettlementName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			Settlement settlement;
			string text2;
			if (CampaignCheats.TryGetObject<Settlement>(CampaignCheats.ConcatenateString(strings.GetRange(0, strings.Count)), out settlement, out text2, null))
			{
				settlement.Militia = 0f;
				MobileParty mobileParty = (settlement.IsFortification ? settlement.Town.GarrisonParty : null);
				if (mobileParty != null)
				{
					DestroyPartyAction.Apply(null, mobileParty);
				}
				return "Success";
			}
			return text2 + "\n" + text;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001D86C File Offset: 0x0001BA6C
		[CommandLineFunctionality.CommandLineArgumentFunction("add_xp_to_player_party_prisoners", "campaign")]
		public static string AddPrisonersXp(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_xp_to_player_party_prisoners [Amount]\".";
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1;
			if (!int.TryParse(strings[0], out num) || num < 1)
			{
				return "Please enter a positive number\n" + text;
			}
			for (int i = 0; i < MobileParty.MainParty.PrisonRoster.Count; i++)
			{
				MobileParty.MainParty.PrisonRoster.SetElementXp(i, MobileParty.MainParty.PrisonRoster.GetElementXp(i) + num);
				InformationManager.DisplayMessage(new InformationMessage(string.Concat(new object[]
				{
					"[DEBUG] ",
					num,
					" xp given to ",
					MobileParty.MainParty.PrisonRoster.GetElementCopyAtIndex(i).Character.Name
				})));
			}
			return "Success";
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001D958 File Offset: 0x0001BB58
		[CommandLineFunctionality.CommandLineArgumentFunction("set_hero_trait", "campaign")]
		public static string SetHeroTrait(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_hero_trait [HeroName] | [Trait]  | [Value]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
			if (separatedNames.Count < 3)
			{
				return text;
			}
			int num;
			if (!int.TryParse(separatedNames[2], out num))
			{
				return "Please enter a number\n" + text;
			}
			Hero hero;
			string text2;
			if (!CampaignCheats.TryGetObject<Hero>(separatedNames[0], out hero, out text2, null))
			{
				return text2 + "\n" + text;
			}
			int num2;
			if (!int.TryParse(separatedNames[2], out num2))
			{
				return "Trait not found.\n" + text;
			}
			TraitObject traitObject;
			string text3;
			if (!CampaignCheats.TryGetObject<TraitObject>(separatedNames[1], out traitObject, out text3, null))
			{
				return text3 + "\n" + text;
			}
			int traitLevel = hero.GetTraitLevel(traitObject);
			if (num2 >= traitObject.MinValue && num2 <= traitObject.MaxValue)
			{
				hero.SetTraitLevel(traitObject, num2);
				if (hero == Hero.MainHero)
				{
					TraitLevelingHelper.UpdateTraitXPAccordingToTraitLevels();
					CampaignEventDispatcher.Instance.OnPlayerTraitChanged(traitObject, traitLevel);
				}
				TraitLevelingHelper.UpdateTraitXPAccordingToTraitLevels();
				CampaignEventDispatcher.Instance.OnPlayerTraitChanged(traitObject, traitLevel);
				return string.Format("{0} 's {1} trait has been set to {2}.", separatedNames[0], traitObject.Name, num2);
			}
			return string.Format("Number must be between {0} and {1}.", traitObject.MinValue, traitObject.MaxValue);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001DAB8 File Offset: 0x0001BCB8
		[CommandLineFunctionality.CommandLineArgumentFunction("remove_militias_from_settlement", "campaign")]
		public static string RemoveMilitiasFromSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.remove_militias_from_settlement [SettlementName]\".";
			}
			string text = CampaignCheats.ConcatenateString(strings);
			Settlement settlement;
			string text2;
			if (!CampaignCheats.TryGetObject<Settlement>(text, out settlement, out text2, null))
			{
				return text2 + ": " + text;
			}
			if (settlement.Party.MapEvent != null)
			{
				return "Settlement, " + text + " is in a MapEvent, try later to remove them";
			}
			List<MobileParty> list = new List<MobileParty>();
			IEnumerable<MobileParty> all = MobileParty.All;
			Func<MobileParty, bool> <>9__0;
			Func<MobileParty, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (MobileParty x) => x.IsMilitia && x.CurrentSettlement == settlement);
			}
			foreach (MobileParty mobileParty in all.Where<MobileParty>(func))
			{
				if (mobileParty.MapEvent != null)
				{
					return "Militia in " + text + " are in a MapEvent, try later to remove them";
				}
				list.Add(mobileParty);
			}
			foreach (MobileParty mobileParty2 in list)
			{
				mobileParty2.RemoveParty();
			}
			return "Success";
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001DC14 File Offset: 0x0001BE14
		[CommandLineFunctionality.CommandLineArgumentFunction("cancel_quest", "campaign")]
		public static string CancelQuestCheat(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.cancel_quest [quest name]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
			if (separatedNames.Count != 1)
			{
				return "Given parameters is not correct";
			}
			string text2 = separatedNames[0].ToLower();
			if (text2.IsEmpty<char>())
			{
				return text;
			}
			QuestBase questBase = null;
			int num = 0;
			foreach (QuestBase questBase2 in Campaign.Current.QuestManager.Quests)
			{
				if (text2.Equals(questBase2.Title.ToString().Replace(" ", "").ToLower(), StringComparison.OrdinalIgnoreCase))
				{
					num++;
					if (num == 1)
					{
						questBase = questBase2;
					}
				}
			}
			if (questBase == null)
			{
				return "Quest not found.\n" + text;
			}
			if (num > 1)
			{
				return "There are more than one quest with the name: " + text2;
			}
			if (questBase.IsSpecialQuest)
			{
				return "Quest can not be special quest";
			}
			questBase.CompleteQuestWithCancel(new TextObject("{=!}Quest is canceled by cheat.", null));
			return "Success";
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001DD40 File Offset: 0x0001BF40
		[CommandLineFunctionality.CommandLineArgumentFunction("kick_companion", "campaign")]
		public static string KickCompanionFromParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			bool isDevelopmentMode = Game.Current.IsDevelopmentMode;
			string text = (isDevelopmentMode ? "Format is \"campaign.kick_companion [CompanionName] or [all](kicks all companions) or [noargument](kicks first companion if any) \"." : "Format is \"campaign.kick_companion [CompanionName] or [noargument](kicks first companion if any) \".");
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			IEnumerable<TroopRosterElement> enumerable = from h in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				where h.Character != null && h.Character.IsHero && h.Character.HeroObject.IsWanderer
				select h;
			if (enumerable.IsEmpty<TroopRosterElement>())
			{
				return "There are no companions in your party.";
			}
			string text2 = CampaignCheats.ConcatenateString(strings).Replace(" ", "");
			if (strings.IsEmpty<string>())
			{
				RemoveCompanionAction.ApplyByFire(Clan.PlayerClan, enumerable.First<TroopRosterElement>().Character.HeroObject);
				return "Success";
			}
			if (isDevelopmentMode && string.Equals(text2, "all", StringComparison.OrdinalIgnoreCase))
			{
				foreach (TroopRosterElement troopRosterElement in enumerable)
				{
					RemoveCompanionAction.ApplyByFire(Clan.PlayerClan, troopRosterElement.Character.HeroObject);
				}
				return "Success";
			}
			Hero hero;
			string text3;
			if (CampaignCheats.TryGetObject<Hero>(text2, out hero, out text3, null))
			{
				RemoveCompanionAction.ApplyByFire(Clan.PlayerClan, hero);
				return "Success";
			}
			return text3 + "\n" + text;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001DE98 File Offset: 0x0001C098
		[CommandLineFunctionality.CommandLineArgumentFunction("add_xp_to_player_party_troops", "campaign")]
		public static string AddTroopsXp(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_xp_to_player_party_troops [Amount]\".";
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1;
			if (!int.TryParse(strings[0], out num) || num < 1)
			{
				return "Please enter a positive number\n" + text;
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			for (int i = 0; i < MobileParty.MainParty.MemberRoster.Count; i++)
			{
				MobileParty.MainParty.MemberRoster.SetElementXp(i, MobileParty.MainParty.MemberRoster.GetElementXp(i) + num);
				InformationManager.DisplayMessage(new InformationMessage(string.Concat(new object[]
				{
					"[DEBUG] ",
					num,
					" xp given to ",
					MobileParty.MainParty.MemberRoster.GetElementCopyAtIndex(i).Character.Name
				})));
			}
			return "Success";
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001DF90 File Offset: 0x0001C190
		[CommandLineFunctionality.CommandLineArgumentFunction("print_gameplay_statistics", "campaign")]
		public static string PrintGameplayStatistics(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"statistics.print_gameplay_statistics\".";
			}
			IStatisticsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IStatisticsCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return "Can not find IStatistics Campaign Behavior!";
			}
			string text = "";
			text += "---------------------------GENERAL---------------------------\n";
			text = string.Concat(new object[]
			{
				text,
				"Played Time in Campaign Time(Days): ",
				campaignBehavior.GetTotalTimePlayed().ToDays,
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Played Time in Real Time: ",
				campaignBehavior.GetTotalTimePlayedInSeconds(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of children born: ",
				campaignBehavior.GetNumberOfChildrenBorn(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total influence earned: ",
				campaignBehavior.GetTotalInfluenceEarned(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of issues solved: ",
				campaignBehavior.GetNumberOfIssuesSolved(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of tournaments won: ",
				campaignBehavior.GetNumberOfTournamentWins(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Best tournament rank: ",
				campaignBehavior.GetHighestTournamentRank(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of prisoners recruited: ",
				campaignBehavior.GetNumberOfPrisonersRecruited(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of troops recruited: ",
				campaignBehavior.GetNumberOfTroopsRecruited(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of enemy clans defected: ",
				campaignBehavior.GetNumberOfClansDefected(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total crime rating gained: ",
				campaignBehavior.GetTotalCrimeRatingGained(),
				"\n"
			});
			text += "---------------------------BATTLE---------------------------\n";
			text = string.Concat(new object[]
			{
				text,
				"Battles Won / Lost: ",
				campaignBehavior.GetNumberOfBattlesWon(),
				" / ",
				campaignBehavior.GetNumberOfBattlesLost(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Largest battle won as the leader: ",
				campaignBehavior.GetLargestBattleWonAsLeader(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Largest army formed by the player: ",
				campaignBehavior.GetLargestArmyFormedByPlayer(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of enemy clans destroyed: ",
				campaignBehavior.GetNumberOfEnemyClansDestroyed(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Heroes killed in battle: ",
				campaignBehavior.GetNumberOfHeroesKilledInBattle(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Troops killed or knocked out in person: ",
				campaignBehavior.GetNumberOfTroopsKnockedOrKilledByPlayer(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Troops killed or knocked out by player party: ",
				campaignBehavior.GetNumberOfTroopsKnockedOrKilledAsParty(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of hero prisoners taken: ",
				campaignBehavior.GetNumberOfHeroPrisonersTaken(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of troop prisoners taken: ",
				campaignBehavior.GetNumberOfTroopPrisonersTaken(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of captured towns: ",
				campaignBehavior.GetNumberOfTownsCaptured(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of captured castles: ",
				campaignBehavior.GetNumberOfCastlesCaptured(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of cleared hideouts: ",
				campaignBehavior.GetNumberOfHideoutsCleared(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of raided villages: ",
				campaignBehavior.GetNumberOfVillagesRaided(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of days spent as prisoner: ",
				campaignBehavior.GetTimeSpentAsPrisoner().ToDays,
				"\n"
			});
			text += "---------------------------FINANCES---------------------------\n";
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned: ",
				campaignBehavior.GetTotalDenarsEarned(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned from caravans: ",
				campaignBehavior.GetDenarsEarnedFromCaravans(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned from workshops: ",
				campaignBehavior.GetDenarsEarnedFromWorkshops(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned from ransoms: ",
				campaignBehavior.GetDenarsEarnedFromRansoms(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned from taxes: ",
				campaignBehavior.GetDenarsEarnedFromTaxes(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned from tributes: ",
				campaignBehavior.GetDenarsEarnedFromTributes(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars paid in tributes: ",
				campaignBehavior.GetDenarsPaidAsTributes(),
				"\n"
			});
			text += "---------------------------CRAFTING---------------------------\n";
			text = string.Concat(new object[]
			{
				text,
				"Number of weapons crafted: ",
				campaignBehavior.GetNumberOfWeaponsCrafted(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Most expensive weapon crafted: ",
				campaignBehavior.GetMostExpensiveItemCrafted().Item1,
				" - ",
				campaignBehavior.GetMostExpensiveItemCrafted().Item2,
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Numbere of crafting parts unlocked: ",
				campaignBehavior.GetNumberOfCraftingPartsUnlocked(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of crafting orders completed: ",
				campaignBehavior.GetNumberOfCraftingOrdersCompleted(),
				"\n"
			});
			text += "---------------------------COMPANIONS---------------------------\n";
			text = string.Concat(new object[]
			{
				text,
				"Number of hired companions: ",
				campaignBehavior.GetNumberOfCompanionsHired(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Companion with most issues solved: ",
				campaignBehavior.GetCompanionWithMostIssuesSolved().Item1,
				" - ",
				campaignBehavior.GetCompanionWithMostIssuesSolved().Item2,
				"\n"
			});
			return string.Concat(new object[]
			{
				text,
				"Companion with most kills: ",
				campaignBehavior.GetCompanionWithMostKills().Item1,
				" - ",
				campaignBehavior.GetCompanionWithMostKills().Item2,
				"\n"
			});
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001E790 File Offset: 0x0001C990
		[CommandLineFunctionality.CommandLineArgumentFunction("set_parties_visible", "campaign")]
		public static string SetAllArmiesAndPartiesVisible(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.set_parties_visible [1/0]\".";
			}
			Campaign.Current.TrueSight = strings[0] == "1";
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				mobileParty.IsVisible = Campaign.Current.TrueSight;
			}
			return "Success";
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001E834 File Offset: 0x0001CA34
		[CommandLineFunctionality.CommandLineArgumentFunction("print_strength_of_lord_parties", "campaign")]
		public static string PrintStrengthOfLordParties(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MobileParty mobileParty in MobileParty.AllLordParties)
			{
				stringBuilder.AppendLine(mobileParty.Name + " strength: " + mobileParty.Party.CalculateCurrentStrength());
			}
			stringBuilder.AppendLine("Success");
			return stringBuilder.ToString();
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001E8D0 File Offset: 0x0001CAD0
		[CommandLineFunctionality.CommandLineArgumentFunction("toggle_information_restrictions", "campaign")]
		public static string ToggleInformationRestrictions(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.toggle_information_restrictions\".";
			}
			DefaultInformationRestrictionModel defaultInformationRestrictionModel = Campaign.Current.Models.InformationRestrictionModel as DefaultInformationRestrictionModel;
			if (defaultInformationRestrictionModel == null)
			{
				return "DefaultInformationRestrictionModel is missing.";
			}
			defaultInformationRestrictionModel.IsDisabledByCheat = !defaultInformationRestrictionModel.IsDisabledByCheat;
			return "Information restrictions are " + (defaultInformationRestrictionModel.IsDisabledByCheat ? "disabled" : "enabled") + ".";
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001E950 File Offset: 0x0001CB50
		[CommandLineFunctionality.CommandLineArgumentFunction("print_strength_of_factions", "campaign")]
		public static string PrintStrengthOfFactions(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Clan clan in Clan.All)
			{
				stringBuilder.AppendLine(clan.Name + " strength: " + clan.CurrentTotalStrength);
			}
			stringBuilder.AppendLine("Success");
			return stringBuilder.ToString();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0001E9E8 File Offset: 0x0001CBE8
		[CommandLineFunctionality.CommandLineArgumentFunction("add_supporters_for_main_hero", "campaign")]
		public static string AddSupportersForMainHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Usage : campaign.add_supporters_for_main_hero [Number]";
			if (CampaignCheats.CheckHelp(strings) || !CampaignCheats.CheckParameters(strings, 1))
			{
				return "" + "Usage : campaign.add_supporters_for_main_hero [Number]" + "\n";
			}
			string text2 = "";
			int num;
			if (int.TryParse(strings[0], out num) && num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					Hero randomElementWithPredicate = Hero.AllAliveHeroes.GetRandomElementWithPredicate<Hero>((Hero x) => !x.IsChild && x.SupporterOf != Clan.PlayerClan && x.IsNotable);
					if (randomElementWithPredicate != null)
					{
						randomElementWithPredicate.SupporterOf = Clan.PlayerClan;
						text2 = text2 + "supporter added: " + randomElementWithPredicate.Name.ToString() + "\n";
					}
				}
				return text2 + "\nSuccess";
			}
			return "Please enter a positive number\n" + text;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001EAD0 File Offset: 0x0001CCD0
		[CommandLineFunctionality.CommandLineArgumentFunction("set_campaign_speed_multiplier", "campaign")]
		public static string SetCampaignSpeed(List<string> strings)
		{
			string text = "Format is \"campaign.set_campaign_speed_multiplier  [positive speedUp multiplier]\".";
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			float num = (Game.Current.IsDevelopmentMode ? 30f : 15f);
			float num2;
			if (!float.TryParse(strings[0], out num2) || num2 <= 0f)
			{
				return text;
			}
			if (num2 <= num)
			{
				Campaign.Current.SpeedUpMultiplier = num2;
				return "Success";
			}
			Campaign.Current.SpeedUpMultiplier = num;
			return "Campaign speed is set to " + num + ". which is the maximum value for speed up multiplier!";
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001EB74 File Offset: 0x0001CD74
		[CommandLineFunctionality.CommandLineArgumentFunction("show_hideouts", "campaign")]
		public static string ShowHideouts(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (Game.Current.IsDevelopmentMode)
			{
				int num;
				if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings) || !int.TryParse(strings[0], out num) || (num != 1 && num != 2))
				{
					return "Format is \"campaign.show_hideouts [1/2]\n 1: Show infested hideouts\n2: Show all hideouts\".";
				}
				foreach (Settlement settlement in Settlement.All)
				{
					if (settlement.IsHideout && (num != 1 || settlement.Hideout.IsInfested))
					{
						settlement.Hideout.Owner.Settlement.IsVisible = true;
					}
				}
				return ((num == 1) ? "Infested" : "All") + " hideouts is visible now.";
			}
			else
			{
				if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
				{
					return "Format is \"campaign.show_hideouts";
				}
				foreach (Settlement settlement2 in Settlement.All)
				{
					if (settlement2.IsHideout && !settlement2.Hideout.IsInfested)
					{
						settlement2.Hideout.Owner.Settlement.IsVisible = true;
					}
				}
				return "Success";
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001ECDC File Offset: 0x0001CEDC
		[CommandLineFunctionality.CommandLineArgumentFunction("hide_hideouts", "campaign")]
		public static string HideHideouts(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsHideout)
				{
					settlement.Hideout.Owner.Settlement.IsVisible = false;
				}
			}
			return "All hideouts should be invisible now.";
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001ED5C File Offset: 0x0001CF5C
		[CommandLineFunctionality.CommandLineArgumentFunction("unlock_all_crafting_pieces", "campaign")]
		public static string UnlockCraftingPieces(List<string> strings)
		{
			string text = "";
			if (!CampaignCheats.CheckCheatUsage(ref text))
			{
				return text;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.unlock_all_crafting_pieces\".";
			}
			CraftingCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<CraftingCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return "Can not find Crafting Campaign Behavior!";
			}
			Type typeFromHandle = typeof(CraftingCampaignBehavior);
			FieldInfo field = typeFromHandle.GetField("_openedPartsDictionary", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo field2 = typeFromHandle.GetField("_openNewPartXpDictionary", BindingFlags.Instance | BindingFlags.NonPublic);
			Dictionary<CraftingTemplate, List<CraftingPiece>> dictionary = (Dictionary<CraftingTemplate, List<CraftingPiece>>)field.GetValue(campaignBehavior);
			Dictionary<CraftingTemplate, float> dictionary2 = (Dictionary<CraftingTemplate, float>)field2.GetValue(campaignBehavior);
			MethodInfo method = typeFromHandle.GetMethod("OpenPart", BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (CraftingTemplate craftingTemplate in CraftingTemplate.All)
			{
				if (!dictionary.ContainsKey(craftingTemplate))
				{
					dictionary.Add(craftingTemplate, new List<CraftingPiece>());
				}
				if (!dictionary2.ContainsKey(craftingTemplate))
				{
					dictionary2.Add(craftingTemplate, 0f);
				}
				foreach (CraftingPiece craftingPiece in craftingTemplate.Pieces)
				{
					object[] array = new object[] { craftingPiece, craftingTemplate, false };
					method.Invoke(campaignBehavior, array);
				}
			}
			field.SetValue(campaignBehavior, dictionary);
			field2.SetValue(campaignBehavior, dictionary2);
			return "Success";
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001EEE8 File Offset: 0x0001D0E8
		[CommandLineFunctionality.CommandLineArgumentFunction("rebellion_enabled", "campaign")]
		public static string SetRebellionEnabled(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is campaign.rebellion_enabled [1/0]\".";
			if (CampaignCheats.CheckHelp(strings) || !CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			if (!(strings[0] == "0") && !(strings[0] == "1"))
			{
				return "Wrong input.\n" + text;
			}
			RebellionsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<RebellionsCampaignBehavior>();
			if (campaignBehavior != null)
			{
				FieldInfo field = typeof(RebellionsCampaignBehavior).GetField("_rebellionEnabled", BindingFlags.Instance | BindingFlags.NonPublic);
				field.SetValue(campaignBehavior, strings[0] == "1");
				return "Rebellion is" + (((bool)field.GetValue(campaignBehavior)) ? " enabled" : " disabled");
			}
			return "Rebellions Campaign behavior not found.";
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001EFC0 File Offset: 0x0001D1C0
		[CommandLineFunctionality.CommandLineArgumentFunction("add_troops", "campaign")]
		public static string AddTroopsToParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return "Write \"campaign.add_troops help\" for help";
			}
			string text = "Usage : \"campaign.add_troops [TroopId] | [Number] | [PartyName]\". Party name is optional.";
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
			if (CampaignCheats.CheckHelp(strings) || separatedNames.Count < 2)
			{
				string text2 = "";
				text2 += text;
				text2 += "\n";
				text2 += "\n";
				text2 += "Available troops";
				text2 += "\n";
				text2 += "==============================";
				text2 += "\n";
				foreach (CharacterObject characterObject in MBObjectManager.Instance.GetObjectTypeList<CharacterObject>())
				{
					if (characterObject.Occupation == Occupation.Soldier || characterObject.Occupation == Occupation.Gangster)
					{
						text2 = string.Concat(new object[] { text2, "Id: ", characterObject.StringId, " Name: ", characterObject.Name, "\n" });
					}
				}
				return text2;
			}
			CharacterObject characterObject2;
			string text3;
			CampaignCheats.TryGetObject<CharacterObject>(separatedNames[0], out characterObject2, out text3, null);
			if (characterObject2 == null)
			{
				return text3 + "\n" + text;
			}
			if (characterObject2.Occupation != Occupation.Soldier && characterObject2.Occupation != Occupation.Gangster)
			{
				return "Troop occupation should be Soldier or Gangster to add party";
			}
			int num;
			if (!int.TryParse(separatedNames[1], out num) || num < 1)
			{
				return "Please enter a positive number\n" + text;
			}
			MobileParty mobileParty = PartyBase.MainParty.MobileParty;
			if (separatedNames.Count == 3)
			{
				string text4;
				CampaignCheats.TryGetObject<MobileParty>(separatedNames[2], out mobileParty, out text4, null);
				if (mobileParty == null)
				{
					return "Given party with the parameter: " + separatedNames[2] + "  not found";
				}
			}
			if (mobileParty.MapEvent != null)
			{
				return "Party shouldn't be in a map event.";
			}
			if (!CampaignCheats.IsValueAcceptable((float)num))
			{
				return "The value is too much";
			}
			typeof(DefaultPartySizeLimitModel).GetField("_addAdditionalPartySizeAsCheat", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, true);
			mobileParty.AddElementToMemberRoster(characterObject2, num, false);
			return string.Concat(new object[]
			{
				mobileParty.Name.ToString(),
				" gained ",
				num,
				" of ",
				characterObject2.Name,
				"."
			});
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001F240 File Offset: 0x0001D440
		public static bool TryGetObject<T>(string requestedId, out T obj, out string errorMessage, Func<T, bool> predicate = null) where T : MBObjectBase
		{
			Func<string, string, CampaignCheats.CheatTextControl, bool> func = delegate(string requestedIdLocal, string idToCompare, CampaignCheats.CheatTextControl cheatTextFlag)
			{
				if (cheatTextFlag.HasAnyFlag(CampaignCheats.CheatTextControl.RemoveEmptySpace))
				{
					requestedIdLocal = requestedIdLocal.Replace(" ", "");
					idToCompare = idToCompare.Replace(" ", "");
				}
				if (cheatTextFlag.HasAnyFlag(CampaignCheats.CheatTextControl.IgnoreCase))
				{
					requestedIdLocal = requestedIdLocal.ToLower();
					idToCompare = idToCompare.ToLower();
				}
				return idToCompare.Equals(requestedIdLocal) || (cheatTextFlag.HasAnyFlag(CampaignCheats.CheatTextControl.ContainId) && idToCompare.Contains(requestedIdLocal));
			};
			obj = default(T);
			errorMessage = string.Empty;
			if (string.IsNullOrEmpty(requestedId))
			{
				errorMessage = "Requested Id can't be empty";
				return false;
			}
			MBReadOnlyList<T> mbreadOnlyList = Campaign.Current.CampaignObjectManager.FindAll<T>((T x) => predicate == null || predicate(x));
			MBList<T> mblist = new MBList<T>();
			if (mbreadOnlyList == null || mbreadOnlyList.Count == 0)
			{
				mbreadOnlyList = MBObjectManager.Instance.GetObjects<T>((T x) => predicate == null || predicate(x));
			}
			if (mbreadOnlyList == null || mbreadOnlyList.Count == 0)
			{
				if (typeof(T) == typeof(PerkObject))
				{
					mbreadOnlyList = PerkObject.All as MBReadOnlyList<T>;
				}
				else if (typeof(T) == typeof(IssueBase))
				{
					mbreadOnlyList = Campaign.Current.IssueManager.Issues.Select<KeyValuePair<Hero, IssueBase>, IssueBase>((KeyValuePair<Hero, IssueBase> x) => x.Value).DistinctBy<IssueBase, string>((IssueBase x) => x.GetType().ToString()).ToMBList<IssueBase>() as MBReadOnlyList<T>;
				}
			}
			if (mbreadOnlyList != null && mbreadOnlyList.Any<T>())
			{
				for (int i = 0; i <= 7; i++)
				{
					for (int j = 0; j < mbreadOnlyList.Count; j++)
					{
						if (func(requestedId, mbreadOnlyList[j].StringId, (CampaignCheats.CheatTextControl)i))
						{
							obj = mbreadOnlyList[j];
							return true;
						}
					}
					for (int k = 0; k < mbreadOnlyList.Count; k++)
					{
						if (func(requestedId, mbreadOnlyList[k].GetName().ToString(), (CampaignCheats.CheatTextControl)i) && !mblist.ContainsQ(mbreadOnlyList[k]))
						{
							mblist.Add(mbreadOnlyList[k]);
						}
					}
				}
			}
			if (mblist.Count == 1)
			{
				obj = mblist[0];
				return true;
			}
			if (mblist.Count == 0)
			{
				errorMessage = "Requested object could not found, check parameters";
			}
			else
			{
				errorMessage = "There is ambiguity with requested id, check parameters";
				errorMessage += "\nEnter id to select the object you want";
				for (int l = 0; l < mblist.Count; l++)
				{
					T t = mblist[l];
					errorMessage += string.Format("\nObject {0}: {1}  Id:  {2}", l + 1, t.GetName(), t.StringId);
				}
			}
			return false;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001F4E4 File Offset: 0x0001D6E4
		private static Hero GetClanLeader(string clanName)
		{
			Clan clan;
			string text;
			if (!CampaignCheats.TryGetObject<Clan>(clanName, out clan, out text, null))
			{
				return null;
			}
			if (clan == null)
			{
				return null;
			}
			return clan.Leader;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001F50C File Offset: 0x0001D70C
		private static ItemModifier GetItemModifier(string itemModifierName)
		{
			ItemModifier itemModifier;
			string text;
			if (CampaignCheats.TryGetObject<ItemModifier>(itemModifierName, out itemModifier, out text, null))
			{
				return itemModifier;
			}
			return null;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001F529 File Offset: 0x0001D729
		public static bool IsPartySuitableToUseCheat(PartyBase party, bool ignoreMapEvents = false)
		{
			return (party.MapEvent == null || ignoreMapEvents) && party.SiegeEvent == null && party.IsActive && (party.LeaderHero == null || party.LeaderHero.IsActive);
		}

		// Token: 0x040000D5 RID: 213
		public const string Help = "help";

		// Token: 0x040000D6 RID: 214
		public const string EnterNumber = "Please enter a number";

		// Token: 0x040000D7 RID: 215
		public const string EnterPositiveNumber = "Please enter a positive number";

		// Token: 0x040000D8 RID: 216
		public const string CampaignNotStarted = "Campaign was not started.";

		// Token: 0x040000D9 RID: 217
		public const string CheatModeDisabled = "Cheat mode is disabled!";

		// Token: 0x040000DA RID: 218
		private const string AmbiguityFoundErrorMessage = "There is ambiguity with requested id, check parameters";

		// Token: 0x040000DB RID: 219
		private const string NotFoundErrorMessage = "Requested object could not found, check parameters";

		// Token: 0x040000DC RID: 220
		private const string EmptyIdRequestedMessage = "Requested Id can't be empty";

		// Token: 0x040000DD RID: 221
		public const int MaxSkillValue = 300;

		// Token: 0x040000DE RID: 222
		public const string OK = "Success";

		// Token: 0x040000DF RID: 223
		public const string CheatNameSeparator = "|";

		// Token: 0x040000E0 RID: 224
		public static string ErrorType = "";

		// Token: 0x040000E1 RID: 225
		private const float _maxAmountPlayerCanGive = 10000f;

		// Token: 0x040000E2 RID: 226
		private const string _maxAmountWarning = "The value is too much";

		// Token: 0x02000527 RID: 1319
		[Flags]
		private enum CheatTextControl
		{
			// Token: 0x040016A3 RID: 5795
			None = 0,
			// Token: 0x040016A4 RID: 5796
			IgnoreCase = 1,
			// Token: 0x040016A5 RID: 5797
			ContainId = 2,
			// Token: 0x040016A6 RID: 5798
			RemoveEmptySpace = 4,
			// Token: 0x040016A7 RID: 5799
			All = 7
		}
	}
}
