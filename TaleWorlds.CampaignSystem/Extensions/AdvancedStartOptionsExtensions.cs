using System;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000172 RID: 370
	public static class AdvancedStartOptionsExtensions
	{
		// Token: 0x06001BC6 RID: 7110 RVA: 0x0009034C File Offset: 0x0008E54C
		public static TextObject GetSelectedScenarioName(this AdvancedStartOptionsData options)
		{
			if (options.GetOption("Scenario") != null)
			{
				return options.GetDisplayName("Scenario");
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x0009036C File Offset: 0x0008E56C
		public static string GetStartType(this AdvancedStartOptionsData options)
		{
			if (options.HasValue("StartType"))
			{
				return options.GetValue<string>("StartType");
			}
			return string.Empty;
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x0009038C File Offset: 0x0008E58C
		public static string GetKingdomId(this AdvancedStartOptionsData options)
		{
			if (options.HasValue("KingdomId"))
			{
				return options.GetValue<string>("KingdomId");
			}
			return string.Empty;
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000903AC File Offset: 0x0008E5AC
		public static bool IsFastModeEnabled(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<bool>("EnableFastMode");
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000903B9 File Offset: 0x0008E5B9
		public static string GetScenario(this AdvancedStartOptionsData options)
		{
			if (options.HasValue("Scenario"))
			{
				return options.GetValue<string>("Scenario");
			}
			return string.Empty;
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000903D9 File Offset: 0x0008E5D9
		public static string GetLastStandKingdom(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<string>("LastStandKingdomId");
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000903E6 File Offset: 0x0008E5E6
		public static string GetUnitedEmpireUnifierKingdomId(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<string>("UnitedEmpireUnifierKingdomId");
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000903F3 File Offset: 0x0008E5F3
		public static string GetTwoFactionWarFaction1Id(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<string>("TwoFactionWarFaction1Id");
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x00090400 File Offset: 0x0008E600
		public static string GetTwoFactionWarFaction2Id(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<string>("TwoFactionWarFaction2Id");
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0009040D File Offset: 0x0008E60D
		public static string GetInvasionScenarioFactionId(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<string>("InvasionScenarioFactionId");
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0009041A File Offset: 0x0008E61A
		public static bool IsRisenBanditsEnabled(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<bool>("RisenBanditsMultiplier");
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x00090427 File Offset: 0x0008E627
		public static bool IsHighRebellionEnabled(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<bool>("HighRebellion");
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x00090434 File Offset: 0x0008E634
		public static bool TryGetSeed(this AdvancedStartOptionsData options, out uint seed)
		{
			seed = 0U;
			bool flag = false;
			if (options.HasValue("Seed"))
			{
				flag = true;
				seed = options.GetValue<uint>("Seed") ^ 2654435761U;
			}
			return flag;
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x00090469 File Offset: 0x0008E669
		public static string GetAlternativeCalradiaVariant(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<string>("AlternativeCalradiaVariantId");
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x00090476 File Offset: 0x0008E676
		public static bool IsRecruitmentRateModifierEnabled(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<bool>("RecruitmentRate");
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x00090483 File Offset: 0x0008E683
		public static bool IsIncreasedGlobalMovementSpeedEnabled(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<bool>("IncreasedGlobalMovementSpeed");
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x00090490 File Offset: 0x0008E690
		public static bool IsPersonalShipEnabled(this AdvancedStartOptionsData options)
		{
			return options.TryGetValue<bool>("PersonalShip");
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x000904A0 File Offset: 0x0008E6A0
		private static T TryGetValue<T>(this AdvancedStartOptionsData options, string key)
		{
			if (options.HasValue(key))
			{
				return options.GetValue<T>(key);
			}
			return default(T);
		}

		// Token: 0x0400094B RID: 2379
		private const uint Prime = 2654435761U;
	}
}
