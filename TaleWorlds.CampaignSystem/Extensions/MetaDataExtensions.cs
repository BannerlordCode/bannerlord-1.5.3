using System;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x0200017B RID: 379
	public static class MetaDataExtensions
	{
		// Token: 0x06001BE8 RID: 7144 RVA: 0x0009067C File Offset: 0x0008E87C
		public static string GetUniqueGameId(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("UniqueGameId", out text))
			{
				return "";
			}
			return text;
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x000906A4 File Offset: 0x0008E8A4
		public static int GetMainHeroLevel(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("MainHeroLevel", out text))
			{
				return 0;
			}
			return int.Parse(text);
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000906CC File Offset: 0x0008E8CC
		public static float GetMainPartyFood(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("MainPartyFood", out text))
			{
				return 0f;
			}
			return float.Parse(text);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x000906F8 File Offset: 0x0008E8F8
		public static int GetMainHeroGold(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("MainHeroGold", out text))
			{
				return 0;
			}
			return int.Parse(text);
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x00090720 File Offset: 0x0008E920
		public static float GetClanInfluence(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("ClanInfluence", out text))
			{
				return 0f;
			}
			return float.Parse(text);
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x0009074C File Offset: 0x0008E94C
		public static int GetClanFiefs(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("ClanFiefs", out text))
			{
				return 0;
			}
			return int.Parse(text);
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x00090774 File Offset: 0x0008E974
		public static int GetMainPartyShipCount(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("MainPartyShipCount", out text))
			{
				return 0;
			}
			return int.Parse(text);
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x0009079C File Offset: 0x0008E99C
		public static int GetMainPartyHealthyMemberCount(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("MainPartyHealthyMemberCount", out text))
			{
				return 0;
			}
			return int.Parse(text);
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x000907C4 File Offset: 0x0008E9C4
		public static int GetMainPartyPrisonerMemberCount(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("MainPartyPrisonerMemberCount", out text))
			{
				return 0;
			}
			return int.Parse(text);
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000907EC File Offset: 0x0008E9EC
		public static int GetMainPartyWoundedMemberCount(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("MainPartyWoundedMemberCount", out text))
			{
				return 0;
			}
			return int.Parse(text);
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x00090814 File Offset: 0x0008EA14
		public static string GetClanBannerCode(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("ClanBannerCode", out text))
			{
				return "";
			}
			return text;
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x0009083C File Offset: 0x0008EA3C
		public static string GetCharacterName(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("CharacterName", out text))
			{
				return "";
			}
			return text;
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x00090864 File Offset: 0x0008EA64
		public static string GetCharacterVisualCode(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("MainHeroVisual", out text))
			{
				return "";
			}
			return text;
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x0009088C File Offset: 0x0008EA8C
		public static double GetDayLong(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("DayLong", out text))
			{
				return 0.0;
			}
			return double.Parse(text);
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000908BC File Offset: 0x0008EABC
		public static bool GetIronmanMode(this MetaData metaData)
		{
			string text;
			int num;
			return metaData != null && metaData.TryGetValue("IronmanMode", out text) && int.TryParse(text, out num) && num == 1;
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000908EC File Offset: 0x0008EAEC
		public static int GetPlayerHealthPercentage(this MetaData metaData)
		{
			string text;
			int num;
			if (metaData == null || !metaData.TryGetValue("HealthPercentage", out text) || !int.TryParse(text, out num))
			{
				return 100;
			}
			return num;
		}
	}
}
