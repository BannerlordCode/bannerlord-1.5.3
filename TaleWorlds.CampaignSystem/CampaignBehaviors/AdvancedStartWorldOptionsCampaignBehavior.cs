using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E8 RID: 1000
	public class AdvancedStartWorldOptionsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06003C44 RID: 15428 RVA: 0x000F4DC4 File Offset: 0x000F2FC4
		private static Kingdom PlayerSelectedKingdomInStartOptions
		{
			get
			{
				string playerSelectedKingdomId = Campaign.Current.AdvancedStartData.GetKingdomId();
				if (string.IsNullOrEmpty(playerSelectedKingdomId))
				{
					return null;
				}
				return Campaign.Current.Kingdoms.FirstOrDefaultQ<Kingdom>((Kingdom k) => k.StringId.Equals(playerSelectedKingdomId));
			}
		}

		// Token: 0x06003C45 RID: 15429 RVA: 0x000F4E16 File Offset: 0x000F3016
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x000F4E2F File Offset: 0x000F302F
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
			AdvancedStartWorldOptionsCampaignBehavior.ApplyWorldScenarios();
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x000F4E36 File Offset: 0x000F3036
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x000F4E38 File Offset: 0x000F3038
		public static void ApplyWorldScenarios()
		{
			string scenario = Campaign.Current.AdvancedStartData.GetScenario();
			if (scenario == "LastStand")
			{
				AdvancedStartWorldOptionsCampaignBehavior.OnLastStandScenarioSelected(AdvancedStartWorldOptionsCampaignBehavior.GetRandomForScenario(AdvancedStartWorldOptionsCampaignBehavior.WorldScenarioSeed.LastStand));
				return;
			}
			if (scenario == "twofactionwar")
			{
				AdvancedStartWorldOptionsCampaignBehavior.OnTwoFactionWarScenarioSelected(AdvancedStartWorldOptionsCampaignBehavior.GetRandomForScenario(AdvancedStartWorldOptionsCampaignBehavior.WorldScenarioSeed.TwoFactionWar));
				return;
			}
			if (scenario == "unitedempire")
			{
				AdvancedStartWorldOptionsCampaignBehavior.OnUnitedEmpireScenarioSelected(AdvancedStartWorldOptionsCampaignBehavior.GetRandomForScenario(AdvancedStartWorldOptionsCampaignBehavior.WorldScenarioSeed.UnitedEmpire));
				return;
			}
			if (scenario == "InvasionId")
			{
				AdvancedStartWorldOptionsCampaignBehavior.OnInvasionScenarioSelected(AdvancedStartWorldOptionsCampaignBehavior.GetRandomForScenario(AdvancedStartWorldOptionsCampaignBehavior.WorldScenarioSeed.Invasion));
				return;
			}
			if (scenario == "alternativecalradia")
			{
				AdvancedStartWorldOptionsCampaignBehavior.OnAlternativeCalradiaScenarioSelected(AdvancedStartWorldOptionsCampaignBehavior.GetRandomForScenario(AdvancedStartWorldOptionsCampaignBehavior.WorldScenarioSeed.AlternativeCalradia));
				return;
			}
			if (!(scenario == "nordinvasion"))
			{
				return;
			}
			AdvancedStartWorldOptionsCampaignBehavior.OnNordInvasionScenarioSelected(AdvancedStartWorldOptionsCampaignBehavior.GetRandomForScenario(AdvancedStartWorldOptionsCampaignBehavior.WorldScenarioSeed.NordInvasion));
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x000F4F03 File Offset: 0x000F3103
		private static void GiveSettlementToClan(Clan clan, Settlement settlement)
		{
			if (settlement == null || settlement.OwnerClan == clan)
			{
				return;
			}
			settlement.Town.IsOwnerUnassigned = false;
			settlement.Town.Governor = null;
			settlement.Town.OwnerClan = clan;
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x000F4F38 File Offset: 0x000F3138
		private static Clan GetRandomNonPlayerClanOrRulingClan(Kingdom kingdom, MBFastRandom random)
		{
			List<Clan> list = new List<Clan>();
			foreach (Clan clan in kingdom.Clans)
			{
				if (clan != Clan.PlayerClan)
				{
					list.Add(clan);
				}
			}
			if (list.Count <= 0)
			{
				return kingdom.RulingClan;
			}
			return AdvancedStartWorldOptionsCampaignBehavior.GetRandomElementInternal<Clan>(list, random);
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x000F4FB0 File Offset: 0x000F31B0
		private static T GetRandomElementInternal<T>(IReadOnlyList<T> list, MBFastRandom random)
		{
			if (list.Count == 0)
			{
				return default(T);
			}
			return list[random.Next(list.Count)];
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x000F4FE4 File Offset: 0x000F31E4
		private static T GetRandomElementWithPredicateInternal<T>(IReadOnlyList<T> list, Func<T, bool> predicate, MBFastRandom random)
		{
			List<T> list2 = new List<T>();
			foreach (T t in list)
			{
				if (predicate(t))
				{
					list2.Add(t);
				}
			}
			if (list2.Count <= 0)
			{
				return default(T);
			}
			return list2[random.Next(list2.Count)];
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x000F5060 File Offset: 0x000F3260
		private static MBFastRandom GetRandomForScenario(AdvancedStartWorldOptionsCampaignBehavior.WorldScenarioSeed scenarioSeed)
		{
			return new MBFastRandom((uint)((ulong)Campaign.Current.Options.Seed ^ (ulong)((long)scenarioSeed)));
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x000F507C File Offset: 0x000F327C
		private static List<Settlement> GetFortificationNeighbors(Settlement settlement)
		{
			List<Settlement> list = new List<Settlement>();
			if (((settlement != null) ? settlement.Town : null) == null)
			{
				return list;
			}
			foreach (Settlement settlement2 in settlement.Town.GetNeighborFortifications(MobileParty.NavigationType.All))
			{
				if (settlement2 != null && settlement2.IsFortification)
				{
					list.Add(settlement2);
				}
			}
			return list;
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x000F50F8 File Offset: 0x000F32F8
		private static void ChangeKingdomInternal(Clan clan, Kingdom oldKingdom, Kingdom newKingdom)
		{
			FactionHelper.AdjustFactionStancesForClanJoiningKingdom(clan, newKingdom);
			if (oldKingdom != null)
			{
				clan.ClanLeaveKingdom(true);
			}
			clan.Kingdom = newKingdom;
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x000F5114 File Offset: 0x000F3314
		private static void HandleKingdomCleanup(MBFastRandom scenarioRandom)
		{
			string scenario = Campaign.Current.AdvancedStartData.GetScenario();
			foreach (Kingdom kingdom in Kingdom.All.ToList<Kingdom>())
			{
				if (!kingdom.IsEliminated && kingdom.Clans.Count == 0 && (kingdom != AdvancedStartWorldOptionsCampaignBehavior.PlayerSelectedKingdomInStartOptions || (scenario == "unitedempire" && kingdom.Culture.StringId == "empire")))
				{
					kingdom.DeactivateKingdom();
					Campaign.Current.FactionManager.RemoveFactionsFromCampaignWars(kingdom);
				}
				if (!kingdom.IsEliminated && (kingdom.RulingClan == null || kingdom.RulingClan.MapFaction != kingdom))
				{
					kingdom.RulingClan = AdvancedStartWorldOptionsCampaignBehavior.GetRandomElementInternal<Clan>(kingdom.Clans, scenarioRandom);
				}
			}
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x000F5204 File Offset: 0x000F3404
		private static void OnLastStandScenarioSelected(MBFastRandom scenarioRandom)
		{
			string lastStandKingdomId = Campaign.Current.AdvancedStartData.GetLastStandKingdom();
			Kingdom lastStandKingdom = Kingdom.All.Find((Kingdom t) => t.StringId == lastStandKingdomId);
			Settlement randomElementWithPredicateInternal = AdvancedStartWorldOptionsCampaignBehavior.GetRandomElementWithPredicateInternal<Settlement>((from s in lastStandKingdom.Fiefs
				where s.IsTown
				select s into t
				select t.Settlement).ToList<Settlement>(), (Settlement x) => x.IsTown, scenarioRandom);
			AdvancedStartWorldOptionsCampaignBehavior.GiveSettlementToClan(lastStandKingdom.RulingClan, randomElementWithPredicateInternal);
			Dictionary<Kingdom, int> dictionary = new Dictionary<Kingdom, int>();
			foreach (Town town in lastStandKingdom.Fiefs)
			{
				using (List<Settlement>.Enumerator enumerator2 = town.GetNeighborFortifications(MobileParty.NavigationType.All).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Kingdom kingdom;
						if ((kingdom = enumerator2.Current.MapFaction as Kingdom) != null && kingdom != lastStandKingdom)
						{
							int num;
							if (!dictionary.TryGetValue(kingdom, out num))
							{
								num = 0;
							}
							dictionary[kingdom] = num + 1;
						}
					}
				}
			}
			Kingdom key = dictionary.MaxBy<KeyValuePair<Kingdom, int>, int>((KeyValuePair<Kingdom, int> x) => x.Value).Key;
			foreach (Clan clan in lastStandKingdom.Clans.ToList<Clan>())
			{
				if (clan != lastStandKingdom.RulingClan && scenarioRandom.NextFloat() < 0.7f)
				{
					AdvancedStartWorldOptionsCampaignBehavior.ChangeKingdomInternal(clan, lastStandKingdom, key);
				}
			}
			foreach (Settlement settlement in lastStandKingdom.Fiefs.Select<Town, Settlement>((Town s) => s.Settlement).ToList<Settlement>())
			{
				if (settlement != randomElementWithPredicateInternal)
				{
					AdvancedStartWorldOptionsCampaignBehavior.GiveSettlementToClan(AdvancedStartWorldOptionsCampaignBehavior.GetRandomNonPlayerClanOrRulingClan(key, scenarioRandom), settlement);
				}
			}
			IEnumerable<Kingdom> all = Kingdom.All;
			Func<Kingdom, bool> <>9__6;
			Func<Kingdom, bool> func;
			if ((func = <>9__6) == null)
			{
				func = (<>9__6 = (Kingdom x) => lastStandKingdom.IsAtWarWith(x));
			}
			foreach (Kingdom kingdom2 in all.Where<Kingdom>(func).ToList<Kingdom>())
			{
				FactionManager.SetNeutral(lastStandKingdom, kingdom2);
			}
			FactionManager.DeclareWar(lastStandKingdom, key);
			AdvancedStartWorldOptionsCampaignBehavior.HandleKingdomCleanup(scenarioRandom);
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x000F5534 File Offset: 0x000F3734
		private static void OnTwoFactionWarScenarioSelected(MBFastRandom scenarioRandom)
		{
			string kingdom1Id = Campaign.Current.AdvancedStartData.GetTwoFactionWarFaction1Id();
			string kingdom2Id = Campaign.Current.AdvancedStartData.GetTwoFactionWarFaction2Id();
			Kingdom kingdom = Kingdom.All.Find((Kingdom t) => t.StringId == kingdom1Id);
			Kingdom kingdom2 = Kingdom.All.Find((Kingdom t) => t.StringId == kingdom2Id);
			Kingdom kingdom3 = ((scenarioRandom.NextFloat() < 0.5f) ? kingdom : kingdom2);
			Kingdom kingdom4 = ((kingdom3 == kingdom) ? kingdom2 : kingdom);
			int num = AdvancedStartWorldOptionsCampaignBehavior.RoundRandomizedInternal((float)Settlement.All.CountQ<Settlement>((Settlement x) => x.IsFortification) / 2f, scenarioRandom);
			AdvancedStartWorldOptionsCampaignBehavior.GrowKingdomByInvasion(kingdom3, (float)num, scenarioRandom, true);
			List<Kingdom> list = Kingdom.All.ToList<Kingdom>();
			list.Remove(kingdom);
			list.Remove(kingdom2);
			AdvancedStartWorldOptionsCampaignBehavior.ShuffleInternal<Kingdom>(list, scenarioRandom);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				AdvancedStartWorldOptionsCampaignBehavior.DestroyKingdomByDefection(list[i], scenarioRandom);
			}
			AdvancedStartWorldOptionsCampaignBehavior.GrowKingdomByInvasion(kingdom4, (float)num, scenarioRandom, true);
			FactionManager.DeclareWar(kingdom, kingdom2);
			AdvancedStartWorldOptionsCampaignBehavior.HandleKingdomCleanup(scenarioRandom);
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x000F5660 File Offset: 0x000F3860
		private static void OnUnitedEmpireScenarioSelected(MBFastRandom scenarioRandom)
		{
			string unifierKingdomId = Campaign.Current.AdvancedStartData.GetUnitedEmpireUnifierKingdomId();
			Kingdom unifierKingdom = Kingdom.All.Find((Kingdom t) => t.StringId == unifierKingdomId);
			MBList<Kingdom> mblist = Kingdom.All.WhereQ<Kingdom>((Kingdom x) => unifierKingdom.IsAtWarWith(x)).ToMBList<Kingdom>();
			Kingdom kingdom = Kingdom.All.Find((Kingdom t) => t.StringId == "empire");
			Kingdom kingdom2 = Kingdom.All.Find((Kingdom t) => t.StringId == "empire_w");
			Kingdom kingdom3 = Kingdom.All.Find((Kingdom t) => t.StringId == "empire_s");
			List<Kingdom> list = new List<Kingdom> { kingdom, kingdom2, kingdom3 };
			TextObject textObject = new TextObject("{=AIh1Ik4A}Calradian Empire", null);
			Kingdom kingdom4 = Campaign.Current.KingdomManager.CreateKingdom(textObject, FactionHelper.GetInformalNameForFactionCulture(unifierKingdom.Culture), unifierKingdom.Culture, unifierKingdom.RulingClan, textObject, null, null, textObject, unifierKingdom.EncyclopediaRulerTitle, "calradian_empire", new Banner(unifierKingdom.Banner), new uint?(unifierKingdom.Color), new uint?(unifierKingdom.Color2), new uint?(unifierKingdom.PrimaryBannerColor), new uint?(unifierKingdom.SecondaryBannerColor));
			foreach (PolicyObject policyObject in unifierKingdom.ActivePolicies.ToList<PolicyObject>())
			{
				kingdom4.AddPolicy(policyObject);
			}
			foreach (Kingdom kingdom5 in list)
			{
				foreach (Clan clan in kingdom5.Clans.ToList<Clan>())
				{
					AdvancedStartWorldOptionsCampaignBehavior.ChangeKingdomInternal(clan, clan.Kingdom, kingdom4);
				}
			}
			AdvancedStartWorldOptionsCampaignBehavior.HandleKingdomCleanup(scenarioRandom);
			foreach (Kingdom kingdom6 in mblist)
			{
				if (!kingdom4.IsAtWarWith(kingdom6) && !kingdom6.IsEliminated)
				{
					FactionManager.DeclareWar(kingdom4, kingdom6);
				}
			}
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x000F593C File Offset: 0x000F3B3C
		private static void OnInvasionScenarioSelected(MBFastRandom scenarioRandom)
		{
			string invaderFactionId = Campaign.Current.AdvancedStartData.GetInvasionScenarioFactionId();
			Kingdom kingdom = Kingdom.All.Find((Kingdom t) => t.StringId == invaderFactionId);
			int num = AdvancedStartWorldOptionsCampaignBehavior.RoundRandomizedInternal(scenarioRandom.NextFloatRanged((float)Town.AllFiefs.Count<Town>() * 0.6f, (float)Town.AllFiefs.Count<Town>() * 0.7f), scenarioRandom);
			AdvancedStartWorldOptionsCampaignBehavior.GrowKingdomByInvasion(kingdom, (float)num, scenarioRandom, false);
			foreach (Town town in kingdom.Fiefs)
			{
				foreach (Settlement settlement in AdvancedStartWorldOptionsCampaignBehavior.GetFortificationNeighbors(town.Settlement))
				{
					if (settlement.MapFaction != kingdom.MapFaction && !kingdom.IsAtWarWith(settlement.MapFaction))
					{
						FactionManager.DeclareWar(kingdom, settlement.MapFaction);
					}
				}
			}
			AdvancedStartWorldOptionsCampaignBehavior.HandleKingdomCleanup(scenarioRandom);
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x000F5A64 File Offset: 0x000F3C64
		private static void OnNordInvasionScenarioSelected(MBFastRandom scenarioRandom)
		{
			Kingdom kingdom = Kingdom.All.Find((Kingdom t) => t.StringId == "nord");
			int num = kingdom.Fiefs.Count + scenarioRandom.Next(10, 14);
			AdvancedStartWorldOptionsCampaignBehavior.GrowKingdomByInvasion(kingdom, (float)num, scenarioRandom, false);
			foreach (Town town in kingdom.Fiefs)
			{
				foreach (Settlement settlement in AdvancedStartWorldOptionsCampaignBehavior.GetFortificationNeighbors(town.Settlement))
				{
					if (settlement.MapFaction != kingdom.MapFaction && !kingdom.IsAtWarWith(settlement.MapFaction))
					{
						FactionManager.DeclareWar(kingdom, settlement.MapFaction);
					}
				}
			}
			AdvancedStartWorldOptionsCampaignBehavior.HandleKingdomCleanup(scenarioRandom);
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x000F5B6C File Offset: 0x000F3D6C
		private static void OnAlternativeCalradiaScenarioSelected(MBFastRandom scenarioRandom)
		{
			ValueTuple<int, int> alternativeCalradiaDestroyRange = AdvancedStartWorldOptionsCampaignBehavior.GetAlternativeCalradiaDestroyRange(Campaign.Current.AdvancedStartData.GetAlternativeCalradiaVariant());
			int num = scenarioRandom.Next(alternativeCalradiaDestroyRange.Item1, alternativeCalradiaDestroyRange.Item2 + 1);
			List<Kingdom> list = Kingdom.All.ToList<Kingdom>();
			AdvancedStartWorldOptionsCampaignBehavior.ShuffleInternal<Kingdom>(list, scenarioRandom);
			if (AdvancedStartWorldOptionsCampaignBehavior.PlayerSelectedKingdomInStartOptions != null)
			{
				list.Remove(AdvancedStartWorldOptionsCampaignBehavior.PlayerSelectedKingdomInStartOptions);
			}
			for (int i = 0; i < num; i++)
			{
				Kingdom kingdom = list[0];
				list.RemoveAt(0);
				AdvancedStartWorldOptionsCampaignBehavior.DestroyKingdomByDefection(kingdom, scenarioRandom);
			}
			if (AdvancedStartWorldOptionsCampaignBehavior.PlayerSelectedKingdomInStartOptions != null)
			{
				list.Add(AdvancedStartWorldOptionsCampaignBehavior.PlayerSelectedKingdomInStartOptions);
			}
			AdvancedStartWorldOptionsCampaignBehavior.ShuffleInternal<Kingdom>(list, scenarioRandom);
			foreach (Kingdom kingdom2 in list)
			{
				float num2 = scenarioRandom.NextFloatRanged(0.2f, 0.4f);
				int num3 = kingdom2.Fiefs.Count * AdvancedStartWorldOptionsCampaignBehavior.RoundRandomizedInternal(1f + num2, scenarioRandom);
				AdvancedStartWorldOptionsCampaignBehavior.GrowKingdomByInvasion(kingdom2, (float)num3, scenarioRandom, true);
			}
			AdvancedStartWorldOptionsCampaignBehavior.HandleKingdomCleanup(scenarioRandom);
			AdvancedStartWorldOptionsCampaignBehavior.UpdateWarAndPeaceInTheWorld(scenarioRandom);
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x000F5C84 File Offset: 0x000F3E84
		private static void UpdateWarAndPeaceInTheWorld(MBFastRandom scenarioRandom)
		{
			MBReadOnlyList<Kingdom> all = Kingdom.All;
			for (int i = 0; i < all.Count; i++)
			{
				for (int j = i + 1; j < all.Count; j++)
				{
					Kingdom kingdom = all[i];
					Kingdom kingdom2 = all[j];
					bool flag = AdvancedStartWorldOptionsCampaignBehavior.AreKingdomsNeighbors(kingdom, kingdom2) && scenarioRandom.NextFloat() < 0.2f;
					bool flag2 = kingdom.IsAtWarWith(kingdom2);
					if (flag && !flag2)
					{
						FactionManager.DeclareWar(kingdom, kingdom2);
					}
					else if (!flag && flag2)
					{
						MakePeaceAction.Apply(kingdom, kingdom2);
					}
				}
			}
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x000F5D18 File Offset: 0x000F3F18
		private static bool AreKingdomsNeighbors(Kingdom kingdom1, Kingdom kingdom2)
		{
			foreach (Town town in kingdom1.Fiefs)
			{
				using (List<Settlement>.Enumerator enumerator2 = AdvancedStartWorldOptionsCampaignBehavior.GetFortificationNeighbors(town.Settlement).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.MapFaction == kingdom2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x000F5DB0 File Offset: 0x000F3FB0
		[return: TupleElementNames(new string[] { "Min", "Max" })]
		private static ValueTuple<int, int> GetAlternativeCalradiaDestroyRange(string variant)
		{
			if (variant == "alternativecalradiafractured")
			{
				return new ValueTuple<int, int>(2, 4);
			}
			if (variant == "alternativecalradiashattered")
			{
				return new ValueTuple<int, int>(3, 5);
			}
			return new ValueTuple<int, int>(0, 2);
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x000F5DE4 File Offset: 0x000F3FE4
		private static void DestroyKingdomByDefection(Kingdom kingdomToDestroy, MBFastRandom scenarioRandom)
		{
			List<Kingdom> list = Kingdom.All.Where<Kingdom>((Kingdom k) => !k.IsEliminated && k != kingdomToDestroy && AdvancedStartWorldOptionsCampaignBehavior.AreKingdomsNeighbors(k, kingdomToDestroy)).ToList<Kingdom>();
			bool flag = true;
			while (flag)
			{
				flag = false;
				int num = 0;
				Clan clan = null;
				Kingdom kingdom = null;
				foreach (Clan clan2 in kingdomToDestroy.Clans.ToList<Clan>())
				{
					if (clan2.Kingdom == kingdomToDestroy)
					{
						Dictionary<Kingdom, int> dictionary = new Dictionary<Kingdom, int>();
						foreach (Town town in clan2.Fiefs)
						{
							foreach (Settlement settlement in AdvancedStartWorldOptionsCampaignBehavior.GetFortificationNeighbors(town.Settlement))
							{
								Kingdom kingdom2 = (Kingdom)settlement.MapFaction;
								if (kingdom2 != kingdomToDestroy)
								{
									int num2;
									if (!dictionary.TryGetValue(kingdom2, out num2))
									{
										num2 = 0;
									}
									dictionary[kingdom2] = num2 + 1;
								}
							}
						}
						if (dictionary.Count != 0)
						{
							KeyValuePair<Kingdom, int> keyValuePair = dictionary.MaxBy<KeyValuePair<Kingdom, int>, int>((KeyValuePair<Kingdom, int> x) => x.Value);
							if (keyValuePair.Value > num)
							{
								num = keyValuePair.Value;
								clan = clan2;
								kingdom = keyValuePair.Key;
							}
						}
					}
				}
				if (clan != null)
				{
					AdvancedStartWorldOptionsCampaignBehavior.ChangeKingdomInternal(clan, kingdomToDestroy, kingdom);
					flag = true;
				}
			}
			while (kingdomToDestroy.Clans.Count > 0)
			{
				if (list.Count == 0)
				{
					Debug.FailedAssert("No neighbor kingdom found to receive remaining landlocked clans in Alternative Calradia, check this case", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\AdvancedStartWorldOptionsCampaignBehavior.cs", "DestroyKingdomByDefection", 591);
					break;
				}
				Kingdom randomElementInternal = AdvancedStartWorldOptionsCampaignBehavior.GetRandomElementInternal<Kingdom>(list, scenarioRandom);
				AdvancedStartWorldOptionsCampaignBehavior.ChangeKingdomInternal(kingdomToDestroy.Clans[0], kingdomToDestroy, randomElementInternal);
			}
			kingdomToDestroy.DeactivateKingdom();
			Campaign.Current.FactionManager.RemoveFactionsFromCampaignWars(kingdomToDestroy);
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x000F6054 File Offset: 0x000F4254
		private static void GrowKingdomByInvasion(Kingdom invaderKingdom, float targetFiefCount, MBFastRandom scenarioRandom, bool protectAllInvadedKingdomsFromElimination)
		{
			Func<Settlement, bool> <>9__0;
			while ((float)invaderKingdom.Fiefs.Count < targetFiefCount)
			{
				List<Settlement> list = new List<Settlement>();
				foreach (Town town in invaderKingdom.Fiefs)
				{
					foreach (Settlement settlement in AdvancedStartWorldOptionsCampaignBehavior.GetFortificationNeighbors(town.Settlement))
					{
						if (settlement.MapFaction != invaderKingdom)
						{
							Kingdom kingdom = (Kingdom)settlement.MapFaction;
							if ((!protectAllInvadedKingdomsFromElimination && kingdom != AdvancedStartWorldOptionsCampaignBehavior.PlayerSelectedKingdomInStartOptions) || settlement.OwnerClan != kingdom.RulingClan || settlement.OwnerClan.Fiefs.Count != 1)
							{
								list.Add(settlement);
							}
						}
					}
				}
				if (list.Count == 0)
				{
					break;
				}
				Settlement randomElementInternal = AdvancedStartWorldOptionsCampaignBehavior.GetRandomElementInternal<Settlement>(list, scenarioRandom);
				Kingdom kingdom2 = randomElementInternal.OwnerClan.Kingdom;
				if ((protectAllInvadedKingdomsFromElimination || kingdom2 == AdvancedStartWorldOptionsCampaignBehavior.PlayerSelectedKingdomInStartOptions) && randomElementInternal.OwnerClan == kingdom2.RulingClan)
				{
					List<Settlement> fortificationNeighbors = AdvancedStartWorldOptionsCampaignBehavior.GetFortificationNeighbors(randomElementInternal);
					Func<Settlement, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (Settlement f) => f.MapFaction == invaderKingdom);
					}
					AdvancedStartWorldOptionsCampaignBehavior.GiveSettlementToClan(AdvancedStartWorldOptionsCampaignBehavior.GetRandomElementInternal<Settlement>(fortificationNeighbors.WhereQ<Settlement>(func).ToList<Settlement>(), scenarioRandom).OwnerClan, randomElementInternal);
				}
				else
				{
					AdvancedStartWorldOptionsCampaignBehavior.ChangeKingdomInternal(randomElementInternal.OwnerClan, kingdom2, invaderKingdom);
					if (kingdom2 != null && kingdom2.Clans.Count == 0)
					{
						kingdom2.DeactivateKingdom();
						Campaign.Current.FactionManager.RemoveFactionsFromCampaignWars(kingdom2);
					}
				}
			}
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x000F6234 File Offset: 0x000F4434
		private static int RoundRandomizedInternal(float value, MBFastRandom scenarioRandom)
		{
			int num = MathF.Floor(value);
			float num2 = value - (float)num;
			if (scenarioRandom.NextFloat() < num2)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x000F625C File Offset: 0x000F445C
		private static void ShuffleInternal<T>(List<T> list, MBFastRandom scenarioRandom)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				int num = scenarioRandom.Next(list.Count);
				T t = list[i];
				list[i] = list[num];
				list[num] = t;
			}
		}

		// Token: 0x020007D4 RID: 2004
		private enum WorldScenarioSeed
		{
			// Token: 0x04002040 RID: 8256
			LastStand = 522134860,
			// Token: 0x04002041 RID: 8257
			TwoFactionWar = 335872418,
			// Token: 0x04002042 RID: 8258
			UnitedEmpire = 918273645,
			// Token: 0x04002043 RID: 8259
			Invasion = 1517171341,
			// Token: 0x04002044 RID: 8260
			AlternativeCalradia = 726028561,
			// Token: 0x04002045 RID: 8261
			NordInvasion = 647125983
		}
	}
}
