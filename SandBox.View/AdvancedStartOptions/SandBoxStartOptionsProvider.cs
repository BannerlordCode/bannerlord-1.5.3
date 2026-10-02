using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SandBox.AdvancedStartOptions;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.View.AdvancedStartOptions
{
	// Token: 0x02000082 RID: 130
	[UsedImplicitly]
	public static class SandBoxStartOptionsProvider
	{
		// Token: 0x0600059C RID: 1436 RVA: 0x00029A34 File Offset: 0x00027C34
		[UsedImplicitly]
		[StartOptionsProvider]
		private static void AddStartOptions(AdvancedStartOptions options)
		{
			options.Add(new BooleanAdvancedStartOption("EnableFastMode", "general", null, false));
			options.Add(new UIntAdvancedStartOption("Seed", "general", 0U, uint.MaxValue, null, (uint)Environment.TickCount));
			options.Add(new ListAdvancedStartOption("Scenario", "worldscenarios", new List<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>>
			{
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("none", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("unitedempire", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("LastStand", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("twofactionwar", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("InvasionId", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("alternativecalradia", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem))
			}, null, "none"));
			options.Add(new ListAdvancedStartOption("UnitedEmpireUnifierKingdomId", "worldscenarios", SandBoxStartOptionsProvider.GetImperialCultureItems(), SandBoxStartOptionsProvider.MakeOptionHiddenCondition("UnitedEmpireUnifierKingdomId", (AdvancedStartOptions o) => SandBoxStartOptionsProvider.GetScenario(o) != "unitedempire"), ""));
			options.Add(new ListAdvancedStartOption("LastStandKingdomId", "worldscenarios", SandBoxStartOptionsProvider.GetCultureItems(null), SandBoxStartOptionsProvider.MakeOptionHiddenCondition("LastStandKingdomId", (AdvancedStartOptions o) => SandBoxStartOptionsProvider.GetScenario(o) != "LastStand"), ""));
			options.Add(new ListAdvancedStartOption("InvasionScenarioFactionId", "worldscenarios", SandBoxStartOptionsProvider.GetCultureItems(null), SandBoxStartOptionsProvider.MakeOptionHiddenCondition("InvasionScenarioFactionId", (AdvancedStartOptions o) => SandBoxStartOptionsProvider.GetScenario(o) != "InvasionId"), ""));
			options.Add(new ListAdvancedStartOption("TwoFactionWarFaction1Id", "worldscenarios", SandBoxStartOptionsProvider.GetCultureItems(new Func<AdvancedStartOptions, string, bool>(SandBoxStartOptionsProvider.GetFirstFactionIsDisabled)), SandBoxStartOptionsProvider.MakeOptionHiddenCondition("TwoFactionWarFaction1Id", (AdvancedStartOptions o) => SandBoxStartOptionsProvider.GetScenario(o) != "twofactionwar"), ""));
			options.Add(new ListAdvancedStartOption("TwoFactionWarFaction2Id", "worldscenarios", SandBoxStartOptionsProvider.GetCultureItems(new Func<AdvancedStartOptions, string, bool>(SandBoxStartOptionsProvider.GetSecondFactionIsDisabled)), SandBoxStartOptionsProvider.MakeOptionHiddenCondition("TwoFactionWarFaction2Id", (AdvancedStartOptions o) => SandBoxStartOptionsProvider.GetScenario(o) != "twofactionwar"), ""));
			string text = "AlternativeCalradiaVariantId";
			string text2 = "worldscenarios";
			List<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>> list = new List<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>>();
			list.Add(new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("alternativecalradiadefault", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)));
			list.Add(new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("alternativecalradiafractured", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)));
			list.Add(new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("alternativecalradiashattered", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)));
			options.Add(new ListAdvancedStartOption(text, text2, list, SandBoxStartOptionsProvider.MakeOptionHiddenCondition("AlternativeCalradiaVariantId", (AdvancedStartOptions o) => SandBoxStartOptionsProvider.GetScenario(o) != "alternativecalradia"), ""));
			options.Add(new ListAdvancedStartOption("StartType", "scenarios", new List<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>>
			{
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("default", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("king", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetKingStartCondition)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("vassal", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetVassalStartCondition)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("mercenary", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetMercenaryStartCondition)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("trader", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("outlaw", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem)),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("beggar", new ListAdvancedStartOption.ListItemCondition(SandBoxStartOptionsProvider.GetEnabledItem))
			}, null, "default"));
			options.Add(new ListAdvancedStartOption("KingdomId", "scenarios", SandBoxStartOptionsProvider.GetCultureItems(new Func<AdvancedStartOptions, string, bool>(SandBoxStartOptionsProvider.GetStartingKingdomIsDisabled)), new AdvancedStartOption.AdvancedStartOptionCondition(SandBoxStartOptionsProvider.GetStartingKingdomCondition), ""));
			options.Add(new BooleanAdvancedStartOption("RisenBanditsMultiplier", "globalmodifiers", null, false));
			options.Add(new BooleanAdvancedStartOption("HighRebellion", "globalmodifiers", null, false));
			options.Add(new BooleanAdvancedStartOption("RecruitmentRate", "globalmodifiers", null, false));
			options.Add(new BooleanAdvancedStartOption("IncreasedGlobalMovementSpeed", "globalmodifiers", null, false));
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00029ED2 File Offset: 0x000280D2
		private static AdvancedStartOption.AdvancedStartOptionCondition MakeOptionHiddenCondition(string optionId, Func<AdvancedStartOptions, bool> getIsHidden)
		{
			return (AdvancedStartOptions o) => getIsHidden(o);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00029EEB File Offset: 0x000280EB
		private static TextObject GetLockedReason(string itemId)
		{
			return Module.CurrentModule.GlobalTextManager.FindText("str_campaign_starting_options_locked_reason", itemId);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00029F04 File Offset: 0x00028104
		private static bool GetStartingKingdomCondition(AdvancedStartOptions options)
		{
			string startType = SandBoxStartOptionsProvider.GetStartType(options);
			return startType != "king" && startType != "vassal" && startType != "mercenary" && startType != "fleetadmiral";
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00029F4C File Offset: 0x0002814C
		private static bool GetEnabledItem(AdvancedStartOptions options, out TextObject disabledText)
		{
			disabledText = null;
			return false;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00029F52 File Offset: 0x00028152
		private static bool GetKingStartCondition(AdvancedStartOptions options, out TextObject disabledText)
		{
			disabledText = SandBoxStartOptionsProvider.GetKingPlaythroughLockedReason();
			return !BannerlordConfig.CompletedKingPlaythrough;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00029F63 File Offset: 0x00028163
		private static bool GetVassalStartCondition(AdvancedStartOptions options, out TextObject disabledText)
		{
			disabledText = SandBoxStartOptionsProvider.GetVassalPlaythroughLockedReason();
			return !BannerlordConfig.CompletedVassalPlaythrough;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00029F74 File Offset: 0x00028174
		private static bool GetMercenaryStartCondition(AdvancedStartOptions options, out TextObject disabledText)
		{
			disabledText = SandBoxStartOptionsProvider.GetMercenaryPlaythroughLockedReason();
			return !BannerlordConfig.CompletedMercenaryPlaythrough;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00029F85 File Offset: 0x00028185
		private static bool GetSecondFactionIsDisabled(AdvancedStartOptions o, string identifier)
		{
			return identifier == SandBoxStartOptionsProvider.GetTwoFactionWarFaction1Id(o);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00029F93 File Offset: 0x00028193
		private static bool GetFirstFactionIsDisabled(AdvancedStartOptions o, string identifier)
		{
			return identifier == SandBoxStartOptionsProvider.GetTwoFactionWarFaction2Id(o);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00029FA1 File Offset: 0x000281A1
		private static TextObject GetKingPlaythroughLockedReason()
		{
			return SandBoxStartOptionsProvider.GetLockedReason("king");
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00029FAD File Offset: 0x000281AD
		private static TextObject GetVassalPlaythroughLockedReason()
		{
			return SandBoxStartOptionsProvider.GetLockedReason("vassal");
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00029FB9 File Offset: 0x000281B9
		private static TextObject GetMercenaryPlaythroughLockedReason()
		{
			return SandBoxStartOptionsProvider.GetLockedReason("mercenary");
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00029FC5 File Offset: 0x000281C5
		private static ListAdvancedStartOption.ListItemCondition MakeCultureItem(Func<AdvancedStartOptions, string, bool> getIsDisabled, string identifier)
		{
			return delegate(AdvancedStartOptions o, out TextObject disabledText)
			{
				disabledText = null;
				Func<AdvancedStartOptions, string, bool> getIsDisabled2 = getIsDisabled;
				return getIsDisabled2 != null && getIsDisabled2(o, identifier);
			};
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00029FE8 File Offset: 0x000281E8
		private static List<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>> GetCultureItems(Func<AdvancedStartOptions, string, bool> getIsDisabled = null)
		{
			return new List<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>>
			{
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("sturgia", SandBoxStartOptionsProvider.MakeCultureItem(getIsDisabled, "sturgia")),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("vlandia", SandBoxStartOptionsProvider.MakeCultureItem(getIsDisabled, "vlandia")),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("battania", SandBoxStartOptionsProvider.MakeCultureItem(getIsDisabled, "battania")),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("empire", SandBoxStartOptionsProvider.MakeCultureItem(getIsDisabled, "empire")),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("empire_w", SandBoxStartOptionsProvider.MakeCultureItem(getIsDisabled, "empire_w")),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("empire_s", SandBoxStartOptionsProvider.MakeCultureItem(getIsDisabled, "empire_s")),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("khuzait", SandBoxStartOptionsProvider.MakeCultureItem(getIsDisabled, "khuzait")),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("aserai", SandBoxStartOptionsProvider.MakeCultureItem(getIsDisabled, "aserai"))
			};
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0002A0D4 File Offset: 0x000282D4
		private static bool GetStartingKingdomIsDisabled(AdvancedStartOptions o, string identifier)
		{
			string scenario = SandBoxStartOptionsProvider.GetScenario(o);
			bool flag = scenario == "twofactionwar";
			bool flag2 = scenario == "unitedempire";
			bool flag3 = identifier == "empire_s" || identifier == "empire" || identifier == "empire_w";
			return (flag && identifier != SandBoxStartOptionsProvider.GetTwoFactionWarFaction1Id(o) && identifier != SandBoxStartOptionsProvider.GetTwoFactionWarFaction2Id(o)) || (flag2 && identifier != SandBoxStartOptionsProvider.GetUnitedEmpireUnifierKingdomId(o) && flag3);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0002A15C File Offset: 0x0002835C
		private static List<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>> GetImperialCultureItems()
		{
			return new List<ValueTuple<string, ListAdvancedStartOption.ListItemCondition>>
			{
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("empire", SandBoxStartOptionsProvider.MakeCultureItem(null, "empire")),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("empire_w", SandBoxStartOptionsProvider.MakeCultureItem(null, "empire_w")),
				new ValueTuple<string, ListAdvancedStartOption.ListItemCondition>("empire_s", SandBoxStartOptionsProvider.MakeCultureItem(null, "empire_s"))
			};
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0002A1BF File Offset: 0x000283BF
		private static string GetTwoFactionWarFaction1Id(AdvancedStartOptions options)
		{
			return options.GetOption("TwoFactionWarFaction1Id").GetValue<string>();
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0002A1D1 File Offset: 0x000283D1
		private static string GetUnitedEmpireUnifierKingdomId(AdvancedStartOptions options)
		{
			return options.GetOption("UnitedEmpireUnifierKingdomId").GetValue<string>();
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0002A1E3 File Offset: 0x000283E3
		private static string GetTwoFactionWarFaction2Id(AdvancedStartOptions options)
		{
			return options.GetOption("TwoFactionWarFaction2Id").GetValue<string>();
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0002A1F5 File Offset: 0x000283F5
		private static string GetScenario(AdvancedStartOptions options)
		{
			return options.GetOption("Scenario").GetValue<string>();
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0002A207 File Offset: 0x00028407
		private static string GetStartType(AdvancedStartOptions options)
		{
			return options.GetOption("StartType").GetValue<string>();
		}

		// Token: 0x040002A3 RID: 675
		private const string KingdomSturgiaId = "sturgia";

		// Token: 0x040002A4 RID: 676
		private const string KingdomVlandiaId = "vlandia";

		// Token: 0x040002A5 RID: 677
		private const string KingdomBattaniaId = "battania";

		// Token: 0x040002A6 RID: 678
		private const string KingdomNorthernEmpireId = "empire";

		// Token: 0x040002A7 RID: 679
		private const string KingdomWesternEmpireId = "empire_w";

		// Token: 0x040002A8 RID: 680
		private const string KingdomSouthernEmpireId = "empire_s";

		// Token: 0x040002A9 RID: 681
		private const string KingdomKhuzaitId = "khuzait";

		// Token: 0x040002AA RID: 682
		private const string KingdomAseraiId = "aserai";
	}
}
