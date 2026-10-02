using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace StoryMode
{
	// Token: 0x02000013 RID: 19
	public static class StoryModeData
	{
		// Token: 0x06000090 RID: 144 RVA: 0x000049B4 File Offset: 0x00002BB4
		public static bool IsKingdomImperial(Kingdom kingdomToCheck)
		{
			return kingdomToCheck != null && kingdomToCheck.Culture == StoryModeData.ImperialCulture;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000049C8 File Offset: 0x00002BC8
		public static CultureObject ImperialCulture
		{
			get
			{
				return StoryModeData.NorthernEmpireKingdom.Culture;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000092 RID: 146 RVA: 0x000049D4 File Offset: 0x00002BD4
		public static Kingdom NorthernEmpireKingdom
		{
			get
			{
				if (StoryModeData._northernEmpireKingdom != null)
				{
					return StoryModeData._northernEmpireKingdom;
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (kingdom.StringId == "empire")
					{
						StoryModeData._northernEmpireKingdom = kingdom;
						return kingdom;
					}
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\StoryModeData.cs", "NorthernEmpireKingdom", 74);
				return null;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004A64 File Offset: 0x00002C64
		public static Kingdom WesternEmpireKingdom
		{
			get
			{
				if (StoryModeData._westernEmpireKingdom != null)
				{
					return StoryModeData._westernEmpireKingdom;
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (kingdom.StringId == "empire_w")
					{
						StoryModeData._westernEmpireKingdom = kingdom;
						return kingdom;
					}
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\StoryModeData.cs", "WesternEmpireKingdom", 99);
				return null;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004AF4 File Offset: 0x00002CF4
		public static Kingdom SouthernEmpireKingdom
		{
			get
			{
				if (StoryModeData._southernEmpireKingdom != null)
				{
					return StoryModeData._southernEmpireKingdom;
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (kingdom.StringId == "empire_s")
					{
						StoryModeData._southernEmpireKingdom = kingdom;
						return kingdom;
					}
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\StoryModeData.cs", "SouthernEmpireKingdom", 124);
				return null;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004B84 File Offset: 0x00002D84
		public static Kingdom SturgiaKingdom
		{
			get
			{
				if (StoryModeData._sturgiaKingdom != null)
				{
					return StoryModeData._sturgiaKingdom;
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (kingdom.StringId == "sturgia")
					{
						StoryModeData._sturgiaKingdom = kingdom;
						return kingdom;
					}
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\StoryModeData.cs", "SturgiaKingdom", 149);
				return null;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004C14 File Offset: 0x00002E14
		public static Kingdom AseraiKingdom
		{
			get
			{
				if (StoryModeData._aseraiKingdom != null)
				{
					return StoryModeData._aseraiKingdom;
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (kingdom.StringId == "aserai")
					{
						StoryModeData._aseraiKingdom = kingdom;
						return kingdom;
					}
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\StoryModeData.cs", "AseraiKingdom", 174);
				return null;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004CA4 File Offset: 0x00002EA4
		public static Kingdom VlandiaKingdom
		{
			get
			{
				if (StoryModeData._vlandiaKingdom != null)
				{
					return StoryModeData._vlandiaKingdom;
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (kingdom.StringId == "vlandia")
					{
						StoryModeData._vlandiaKingdom = kingdom;
						return kingdom;
					}
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\StoryModeData.cs", "VlandiaKingdom", 200);
				return null;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004D34 File Offset: 0x00002F34
		public static Kingdom BattaniaKingdom
		{
			get
			{
				if (StoryModeData._battaniaKingdom != null)
				{
					return StoryModeData._battaniaKingdom;
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (kingdom.StringId == "battania")
					{
						StoryModeData._battaniaKingdom = kingdom;
						return kingdom;
					}
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\StoryModeData.cs", "BattaniaKingdom", 227);
				return null;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00004DC4 File Offset: 0x00002FC4
		public static Kingdom KhuzaitKingdom
		{
			get
			{
				if (StoryModeData._khuzaitKingdom != null)
				{
					return StoryModeData._khuzaitKingdom;
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (kingdom.StringId == "khuzait")
					{
						StoryModeData._khuzaitKingdom = kingdom;
						return kingdom;
					}
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\StoryModeData.cs", "KhuzaitKingdom", 253);
				return null;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004E54 File Offset: 0x00003054
		public static bool IsConspiracyTroop(CharacterObject troop)
		{
			return StoryModeData._conspiracyTroops.Contains(troop.StringId);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004E66 File Offset: 0x00003066
		public static void OnGameEnd()
		{
			StoryModeData._northernEmpireKingdom = null;
			StoryModeData._westernEmpireKingdom = null;
			StoryModeData._southernEmpireKingdom = null;
			StoryModeData._sturgiaKingdom = null;
			StoryModeData._aseraiKingdom = null;
			StoryModeData._vlandiaKingdom = null;
			StoryModeData._battaniaKingdom = null;
			StoryModeData._khuzaitKingdom = null;
		}

		// Token: 0x04000030 RID: 48
		private static HashSet<string> _conspiracyTroops = new HashSet<string>
		{
			"conspiracy_commander_antiempire", "conspiracy_wilder", "conspiracy_warmonger", "conspiracy_berserker", "conspiracy_hellion", "conspiracy_guardsman", "conspiracy_guardian", "conspiracy_raider", "conspiracy_battlerider", "conspiracy_trained_bowman",
			"conspiracy_longbowman", "conspiracy_kern", "conspiracy_horse_archer", "conspiracy_mounted_master_archer", "conspiracy_trained_spearman", "conspiracy_spearmaster", "conspiracy_knight_trainee", "conspiracy_knight", "conspiracy_fighter", "conspiracy_veteran_fighter",
			"conspiracy_noble_horseman", "conspiracy_mounted_fighter", "conspiracy_trained_crossbowman", "conspiracy_warworn_crossbowman", "conspiracy_trained_huntsman", "conspiracy_hunt_leader", "conspiracy_mounted_huntsman", "conspiracy_packmaster", "anti_imperial_conspiracy_boss", "imperial_conspiracy_boss"
		};

		// Token: 0x04000031 RID: 49
		public static CampaignTime StorylineQuestHideoutHiddenDuration = CampaignTime.Hours(12f);

		// Token: 0x04000032 RID: 50
		private static Kingdom _northernEmpireKingdom;

		// Token: 0x04000033 RID: 51
		private static Kingdom _westernEmpireKingdom;

		// Token: 0x04000034 RID: 52
		private static Kingdom _southernEmpireKingdom;

		// Token: 0x04000035 RID: 53
		private static Kingdom _sturgiaKingdom;

		// Token: 0x04000036 RID: 54
		private static Kingdom _aseraiKingdom;

		// Token: 0x04000037 RID: 55
		private static Kingdom _vlandiaKingdom;

		// Token: 0x04000038 RID: 56
		private static Kingdom _battaniaKingdom;

		// Token: 0x04000039 RID: 57
		private static Kingdom _khuzaitKingdom;
	}
}
