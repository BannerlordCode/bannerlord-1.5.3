using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000019 RID: 25
	public static class FactionHelper
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x0000B144 File Offset: 0x00009344
		public static float FindPotentialStrength(IFaction faction)
		{
			float num = 0f;
			if (faction.IsKingdomFaction)
			{
				Kingdom kingdom = (Kingdom)faction;
				using (List<Clan>.Enumerator enumerator = kingdom.Clans.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Clan clan = enumerator.Current;
						float num2 = (clan.IsUnderMercenaryService ? (((float)kingdom.Leader.Gold > 100000f) ? 0.3f : (0.3f - (1f - (float)kingdom.Leader.Gold / 100000f) * 0.3f)) : 1f);
						num += num2 * (float)clan.Tier * 100f;
					}
					goto IL_00C6;
				}
			}
			if (faction.IsClan)
			{
				num += (float)((Clan)faction).Tier * 100f;
			}
			IL_00C6:
			return num * 2f;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000B230 File Offset: 0x00009430
		public static IEnumerable<Kingdom> GetEnemyKingdoms(IFaction faction)
		{
			return faction.FactionsAtWarWith.Where<IFaction>((IFaction x) => x.IsKingdomFaction).Cast<Kingdom>();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000B264 File Offset: 0x00009464
		public static IEnumerable<StanceLink> GetStances(IFaction faction)
		{
			List<StanceLink> list = new List<StanceLink>();
			foreach (Kingdom kingdom in Kingdom.All)
			{
				if (kingdom != faction)
				{
					StanceLink stanceWith = faction.GetStanceWith(kingdom);
					if (stanceWith != null)
					{
						list.Add(stanceWith);
					}
				}
			}
			foreach (Clan clan in Clan.All)
			{
				if (clan != faction)
				{
					StanceLink stanceWith2 = faction.GetStanceWith(clan);
					if (stanceWith2 != null)
					{
						list.Add(stanceWith2);
					}
				}
			}
			return list;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000B324 File Offset: 0x00009524
		public static float GetPowerRatioToEnemies(Kingdom kingdom)
		{
			float currentTotalStrength = kingdom.CurrentTotalStrength;
			float totalEnemyKingdomPower = FactionHelper.GetTotalEnemyKingdomPower(kingdom);
			return currentTotalStrength / (totalEnemyKingdomPower + 0.0001f);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000B348 File Offset: 0x00009548
		private static List<TextObject> IsFactionNameApplicable(string name)
		{
			List<TextObject> list = new List<TextObject>();
			if (name == null)
			{
				Debug.FailedAssert("Calling IsFactionNameApplicable with null string!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "IsFactionNameApplicable", 5947);
				name = string.Empty;
			}
			if (name.Length > 50 || name.Length < 1)
			{
				TextObject textObject = GameTexts.FindText("str_faction_name_invalid_character_count", null).SetTextVariable("MIN", 1).SetTextVariable("MAX", 50);
				list.Add(textObject);
			}
			if (Common.TextContainsSpecialCharacters(name))
			{
				list.Add(GameTexts.FindText("str_faction_name_invalid_characters", null));
			}
			if (name.StartsWith(" ") || name.EndsWith(" "))
			{
				list.Add(new TextObject("{=LCOZZMta}Faction name cannot start or end with a white space", null));
			}
			if (name.Contains("  "))
			{
				list.Add(new TextObject("{=CtsdrQ9N}Faction name cannot contain consecutive white spaces", null));
			}
			return list;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000B428 File Offset: 0x00009628
		public static Tuple<bool, string> IsClanNameApplicable(string name)
		{
			string text = string.Empty;
			List<TextObject> list = FactionHelper.IsFactionNameApplicable(name);
			MBReadOnlyList<Clan> all = Clan.All;
			if (all != null && all.Any<Clan>((Clan x) => x != Clan.PlayerClan && string.Equals(x.Name.ToString(), name, StringComparison.InvariantCultureIgnoreCase)))
			{
				list.Add(GameTexts.FindText("str_clan_name_invalid_already_exist", null));
			}
			bool flag = list.Count == 0;
			if (list.Count == 1)
			{
				text = list[0].ToString();
			}
			else if (list.Count > 1)
			{
				TextObject textObject = list[0];
				for (int i = 1; i < list.Count; i++)
				{
					textObject = GameTexts.FindText("str_string_newline_newline_string", null).SetTextVariable("STR1", textObject.ToString()).SetTextVariable("STR2", list[i].ToString());
				}
				text = textObject.ToString();
			}
			return new Tuple<bool, string>(flag, text);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000B514 File Offset: 0x00009714
		public static Tuple<bool, string> IsKingdomNameApplicable(string name)
		{
			string text = string.Empty;
			List<TextObject> list = FactionHelper.IsFactionNameApplicable(name);
			MBReadOnlyList<Kingdom> all = Kingdom.All;
			if (all != null && all.Any<Kingdom>((Kingdom x) => x != Clan.PlayerClan.Kingdom && string.Equals(x.Name.ToString(), name, StringComparison.InvariantCultureIgnoreCase)))
			{
				list.Add(GameTexts.FindText("str_kingdom_name_invalid_already_exist", null));
			}
			bool flag = list.Count == 0;
			if (list.Count == 1)
			{
				text = list[0].ToString();
			}
			else if (list.Count > 1)
			{
				TextObject textObject = list[0];
				for (int i = 1; i < list.Count; i++)
				{
					textObject = GameTexts.FindText("str_string_newline_newline_string", null).SetTextVariable("STR1", textObject.ToString()).SetTextVariable("STR2", list[i].ToString());
				}
				text = textObject.ToString();
			}
			return new Tuple<bool, string>(flag, text);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000B600 File Offset: 0x00009800
		public static float GetPowerRatioToTributePayedKingdoms(Kingdom kingdom)
		{
			float currentTotalStrength = kingdom.CurrentTotalStrength;
			float totalTributePayedKingdomsPower = FactionHelper.GetTotalTributePayedKingdomsPower(kingdom);
			return currentTotalStrength / (totalTributePayedKingdomsPower + 0.0001f);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000B622 File Offset: 0x00009822
		public static bool CanClanBeGrantedFief(Clan clan)
		{
			return clan != Clan.PlayerClan && !clan.IsUnderMercenaryService;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000B638 File Offset: 0x00009838
		public static bool CanPlayerEnterFaction(bool asVassal = false)
		{
			float num = Campaign.Current.Settlements.Where<Settlement>((Settlement settlement) => (settlement.IsVillage || settlement.IsTown || settlement.IsCastle) && settlement.OwnerClan.Leader == Hero.MainHero).Sum<Settlement>((Settlement settlement) => settlement.GetSettlementValueForFaction(Hero.OneToOneConversationHero.MapFaction));
			float num2 = (asVassal ? 50f : 10f);
			float num3 = Clan.PlayerClan.Renown + (asVassal ? (num / 5000f) : 0f) + (asVassal ? ((float)Hero.MainHero.Gold / 10000f) : 0f) + MathF.Min(num2, Clan.PlayerClan.Renown) / num2 * 0.2f * Clan.PlayerClan.CurrentTotalStrength + Hero.OneToOneConversationHero.MapFaction.Leader.GetRelationWithPlayer() * 2f;
			if (!asVassal)
			{
				return num3 > 25f;
			}
			return num3 > 150f;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000B734 File Offset: 0x00009934
		public static float GetTotalEnemyKingdomPower(Kingdom kingdom)
		{
			float num = 0f;
			foreach (Kingdom kingdom2 in FactionHelper.GetEnemyKingdoms(kingdom))
			{
				num += kingdom2.CurrentTotalStrength;
			}
			return num;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000B78C File Offset: 0x0000998C
		public static float GetTotalTributePayedKingdomsPower(Kingdom kingdom)
		{
			float num = 0f;
			foreach (StanceLink stanceLink in FactionHelper.GetStances(kingdom))
			{
				IFaction faction = ((stanceLink.Faction1 == kingdom) ? stanceLink.Faction2 : stanceLink.Faction1);
				if (stanceLink.IsNeutral)
				{
					int dailyTributeToPay = stanceLink.GetDailyTributeToPay(kingdom);
					if (dailyTributeToPay < 0)
					{
						float num2 = MathF.Sqrt(MathF.Min(1f, (float)(-(float)dailyTributeToPay) / 4000f));
						num += num2 * faction.CurrentTotalStrength;
					}
				}
			}
			return num;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000B830 File Offset: 0x00009A30
		public static IEnumerable<Army> GetKingdomArmies(IFaction mapFaction)
		{
			if (!mapFaction.IsKingdomFaction)
			{
				return new List<Army>();
			}
			return ((Kingdom)mapFaction).Armies;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000B84B File Offset: 0x00009A4B
		public static float SettlementProsperityEffectOnGarrisonSizeConstant(Town town)
		{
			return 2.2f * (0.1f + 0.9f * MathF.Sqrt(MathF.Min(town.Prosperity, 5000f) / 5000f));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000B87C File Offset: 0x00009A7C
		public static float SettlementFoodPotentialEffectOnGarrisonSizeConstant(Settlement settlement)
		{
			int num = 0;
			if (settlement.IsFortification)
			{
				foreach (Village village in settlement.Town.Villages)
				{
					num += 5 * ((village.Hearth < 200f) ? 1 : ((village.Hearth < 600f) ? 2 : 3));
				}
			}
			return 0.5f + 0.5f * (float)MathF.Min(50, num) / 50f;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000B918 File Offset: 0x00009B18
		public static float OwnerClanEconomyEffectOnGarrisonSizeConstant(Clan clan)
		{
			if (clan == null || clan.Leader == null)
			{
				return 1f;
			}
			if ((float)clan.Leader.Gold > 160000f)
			{
				return 1.5f + 0.5f * MathF.Min(1f, ((float)clan.Leader.Gold - 160000f) / 160000f);
			}
			if ((float)clan.Leader.Gold > 80000f)
			{
				return 1f + 0.5f * MathF.Min(1f, ((float)clan.Leader.Gold - 80000f) / 80000f);
			}
			if ((float)clan.Leader.Gold < 40000f)
			{
				return 1f - 0.75f * (1f - (float)clan.Leader.Gold / 40000f);
			}
			return 1f;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000BA00 File Offset: 0x00009C00
		public static float FindIdealGarrisonStrengthPerWalledCenter(Kingdom kingdom, Clan clan = null)
		{
			if (kingdom == null && clan == null)
			{
				return 0f;
			}
			float num = 0f;
			int num2;
			if (kingdom == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = kingdom.Clans.Count<Clan>((Clan x) => !x.IsUnderMercenaryService);
			}
			int num3 = num2;
			List<Town> list = ((kingdom != null) ? kingdom.Fiefs : clan.Fiefs);
			float num4 = ((kingdom != null) ? ((kingdom.CurrentTotalStrength + (float)(num3 * 500)) / 2f) : clan.CurrentTotalStrength);
			float num5 = 0f;
			float num6 = 0f;
			foreach (Town town in list)
			{
				float num7 = FactionHelper.SettlementProsperityEffectOnGarrisonSizeConstant(town);
				float num8 = FactionHelper.SettlementFoodPotentialEffectOnGarrisonSizeConstant(town.Settlement);
				num7 *= num8;
				float num9 = FactionHelper.OwnerClanEconomyEffectOnGarrisonSizeConstant(town.OwnerClan);
				num6 += num7;
				num7 *= num9;
				num += num7 * 60f;
				num5 += num7;
			}
			float num10 = num4 * 0.5f / num5;
			float num11 = num / num6;
			return 5f + (num10 + num11) / 2f;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000BB30 File Offset: 0x00009D30
		public static void FinishAllRelatedHostileActionsOfNobleToFaction(Hero noble, IFaction faction)
		{
			if (noble.PartyBelongedTo != null && noble.PartyBelongedTo.MapEvent != null && ((noble.PartyBelongedTo.MapEvent.AttackerSide.LeaderParty == noble.PartyBelongedTo.Party && ((faction.IsKingdomFaction && noble.PartyBelongedTo.MapEvent.DefenderSide.LeaderParty.MapFaction == faction) || (!faction.IsKingdomFaction && noble.PartyBelongedTo.MapEvent.DefenderSide.LeaderParty.Owner != null && noble.PartyBelongedTo.MapEvent.DefenderSide.LeaderParty.Owner.Clan == faction))) || (noble.PartyBelongedTo.MapEvent.DefenderSide.LeaderParty == noble.PartyBelongedTo.Party && ((faction.IsKingdomFaction && noble.PartyBelongedTo.MapEvent.AttackerSide.LeaderParty.MapFaction == faction) || (!faction.IsKingdomFaction && noble.PartyBelongedTo.MapEvent.AttackerSide.LeaderParty.Owner != null && noble.PartyBelongedTo.MapEvent.AttackerSide.LeaderParty.Owner.Clan == faction)))))
			{
				noble.PartyBelongedTo.MapEvent.DiplomaticallyFinished = true;
				List<PartyBase> list = new List<PartyBase>();
				foreach (MapEventParty mapEventParty in noble.PartyBelongedTo.MapEvent.AttackerSide.Parties)
				{
					list.Add(mapEventParty.Party);
				}
				if (noble.PartyBelongedTo.MapEvent.MapEventSettlement != null)
				{
					foreach (WarPartyComponent warPartyComponent in noble.PartyBelongedTo.MapEvent.MapEventSettlement.MapFaction.WarPartyComponents)
					{
						MobileParty mobileParty = warPartyComponent.MobileParty;
						if (mobileParty.DefaultBehavior == AiBehavior.DefendSettlement && mobileParty.TargetSettlement == noble.PartyBelongedTo.MapEvent.MapEventSettlement && mobileParty.CurrentSettlement == null)
						{
							mobileParty.SetMoveModeHold();
						}
					}
				}
				noble.PartyBelongedTo.MapEvent.Update();
				foreach (PartyBase partyBase in list)
				{
					if (partyBase.IsMobile)
					{
						partyBase.MobileParty.SetMoveModeHold();
					}
				}
			}
			if (noble.PartyBelongedTo != null)
			{
				MobileParty partyBelongedTo = noble.PartyBelongedTo;
				if (partyBelongedTo.BesiegedSettlement != null && ((faction.IsKingdomFaction && partyBelongedTo.BesiegedSettlement.MapFaction == faction) || (!faction.IsKingdomFaction && partyBelongedTo.BesiegedSettlement.OwnerClan == faction)))
				{
					foreach (WarPartyComponent warPartyComponent2 in partyBelongedTo.BesiegedSettlement.MapFaction.WarPartyComponents)
					{
						MobileParty mobileParty2 = warPartyComponent2.MobileParty;
						if (mobileParty2.DefaultBehavior == AiBehavior.DefendSettlement && mobileParty2.TargetSettlement == partyBelongedTo.BesiegedSettlement && mobileParty2.CurrentSettlement == null)
						{
							mobileParty2.SetMoveModeHold();
						}
					}
					partyBelongedTo.BesiegerCamp = null;
					partyBelongedTo.SetMoveModeHold();
				}
				if ((partyBelongedTo.DefaultBehavior == AiBehavior.RaidSettlement || partyBelongedTo.DefaultBehavior == AiBehavior.BesiegeSettlement || partyBelongedTo.DefaultBehavior == AiBehavior.AssaultSettlement) && ((faction.IsKingdomFaction && partyBelongedTo.TargetSettlement.MapFaction == faction) || (!faction.IsKingdomFaction && partyBelongedTo.TargetSettlement.OwnerClan == faction)))
				{
					if (partyBelongedTo.Army != null)
					{
						partyBelongedTo.Army.FinishArmyObjective();
					}
					partyBelongedTo.SetMoveModeHold();
				}
				if (partyBelongedTo.ShortTermBehavior == AiBehavior.EngageParty && partyBelongedTo.ShortTermTargetParty != null && partyBelongedTo.ShortTermTargetParty.MapFaction == faction)
				{
					partyBelongedTo.SetMoveModeHold();
				}
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000BF54 File Offset: 0x0000A154
		public static void FinishAllRelatedHostileActionsOfFactionToFaction(IFaction faction1, IFaction faction2)
		{
			foreach (Hero hero in faction1.AliveLords)
			{
				FactionHelper.FinishAllRelatedHostileActionsOfNobleToFaction(hero, faction2);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000BFA8 File Offset: 0x0000A1A8
		public static void FinishAllRelatedHostileActions(Clan clan1, Clan clan2)
		{
			foreach (Hero hero in clan1.AliveLords)
			{
				FactionHelper.FinishAllRelatedHostileActionsOfNobleToFaction(hero, clan2);
			}
			foreach (Hero hero2 in clan2.AliveLords)
			{
				FactionHelper.FinishAllRelatedHostileActionsOfNobleToFaction(hero2, clan1);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000C03C File Offset: 0x0000A23C
		public static void FinishAllRelatedHostileActions(Kingdom kingdom1, Kingdom kingdom2)
		{
			foreach (Clan clan in kingdom1.Clans)
			{
				FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction(clan, kingdom2);
			}
			foreach (Clan clan2 in kingdom2.Clans)
			{
				FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction(clan2, kingdom1);
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000C0D0 File Offset: 0x0000A2D0
		public static void AdjustFactionStancesForClanJoiningKingdom(Clan joiningClan, Kingdom kingdomToJoin)
		{
			foreach (StanceLink stanceLink in FactionHelper.GetStances(joiningClan))
			{
				if (!Campaign.Current.Models.DiplomacyModel.IsAtConstantWar(stanceLink.Faction1, stanceLink.Faction2))
				{
					IFaction faction = ((stanceLink.Faction1 == joiningClan) ? stanceLink.Faction2 : stanceLink.Faction1);
					if (stanceLink.IsAtWar)
					{
						if (!kingdomToJoin.IsAtWarWith(faction))
						{
							MakePeaceAction.Apply(joiningClan, faction);
							FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction(joiningClan, faction);
							FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction(faction, joiningClan);
						}
					}
					else
					{
						stanceLink.ResetPeaceStats();
					}
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000C180 File Offset: 0x0000A380
		public static TextObject GetTermUsedByOtherFaction(IFaction faction, IFaction otherFaction, bool pejorative)
		{
			if (faction.IsMinorFaction || faction.IsEliminated)
			{
				TextObject textObject = new TextObject("{=n48jo6Qn}the {FACTION_NAME}", null);
				textObject.SetTextVariable("FACTION_NAME", faction.Name);
				return textObject;
			}
			if (otherFaction.Culture == faction.Culture)
			{
				TextObject textObject2 = ((!pejorative) ? new TextObject("{=WWFnlL3O}{FACTION_LIEGE}'s followers", null) : new TextObject("{=uujU2fSA}{FACTION_LIEGE}'s scum", null));
				textObject2.SetTextVariable("FACTION_LIEGE", (faction.Leader != null) ? faction.Leader.Name : TextObject.GetEmpty());
				return textObject2;
			}
			int num = 0;
			using (List<Kingdom>.Enumerator enumerator = Kingdom.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Culture == faction.Culture)
					{
						num++;
					}
				}
			}
			TextObject textObject3 = ((num == 1) ? new TextObject("{=bIWDtytH}the {ETHNIC_TERM}", null) : new TextObject("{=JrT9bBEK}{FACTION_LIEGE}'s {ETHNIC_TERM}", null));
			textObject3.SetTextVariable("ETHNIC_TERM", GameTexts.FindText("str_neutral_term_for_culture", faction.Culture.StringId));
			textObject3.SetTextVariable("FACTION_LIEGE", (faction.Leader != null) ? faction.Leader.Name : TextObject.GetEmpty());
			return textObject3;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000C2C0 File Offset: 0x0000A4C0
		public static TextObject GetFormalNameForFactionCulture(CultureObject factionCulture)
		{
			return GameTexts.FindText("str_faction_formal_name_for_culture", factionCulture.StringId);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000C2D2 File Offset: 0x0000A4D2
		public static TextObject GetInformalNameForFactionCulture(CultureObject factionCulture)
		{
			return GameTexts.FindText("str_faction_informal_name_for_culture", factionCulture.StringId);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000C2E4 File Offset: 0x0000A4E4
		public static TextObject GetAdjectiveForFactionCulture(CultureObject factionCulture)
		{
			return GameTexts.FindText("str_adjective_for_culture", factionCulture.StringId);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000C2F8 File Offset: 0x0000A4F8
		public static TextObject GenerateClanNameforPlayer()
		{
			CultureObject culture = CharacterObject.PlayerCharacter.Culture;
			TextObject textObject;
			if (culture.StringId == "vlandia")
			{
				textObject = new TextObject("{=Uk3qRuCS}dey Corvand", null);
			}
			else
			{
				textObject = NameGenerator.Current.GenerateClanName(culture, null);
			}
			return textObject;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000C340 File Offset: 0x0000A540
		public static float GetDistanceToClosestNonAllyFortificationOfFaction(IFaction faction)
		{
			float num = float.MaxValue;
			if (faction.FactionMidSettlement != null)
			{
				foreach (Town town in Town.AllFiefs)
				{
					Settlement settlement = town.Settlement;
					if (settlement.MapFaction != faction)
					{
						float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, faction.FactionMidSettlement, false, false, MobileParty.NavigationType.All);
						if (num > distance)
						{
							num = distance;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		public static Settlement GetMidSettlementOfFaction(IFaction faction)
		{
			Settlement settlement = null;
			if (faction.Settlements.Count == 0)
			{
				Clan clan;
				Kingdom kingdom;
				if ((clan = faction as Clan) != null)
				{
					settlement = clan.HomeSettlement;
				}
				else if ((kingdom = faction as Kingdom) != null)
				{
					settlement = kingdom.InitialHomeSettlement;
				}
			}
			else
			{
				float num = float.MaxValue;
				settlement = faction.Settlements[0];
				foreach (Settlement settlement2 in faction.Settlements)
				{
					float num2 = 0f;
					foreach (Settlement settlement3 in faction.Settlements)
					{
						if (settlement2 != settlement3)
						{
							float num3 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement2, settlement3, false, false, MobileParty.NavigationType.All);
							if (settlement3.IsVillage)
							{
								num3 *= 0.1f;
							}
							else if (settlement3.IsCastle)
							{
								num3 *= 0.25f;
							}
							num2 += num3;
						}
					}
					if (num > num2)
					{
						num = num2;
						settlement = settlement2;
					}
				}
			}
			return settlement;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000C510 File Offset: 0x0000A710
		public static List<IFaction> GetPossibleKingdomsToDeclareWar(Kingdom kingdom)
		{
			List<IFaction> list = new List<IFaction>();
			foreach (Kingdom kingdom2 in Kingdom.All)
			{
				if (kingdom2 != kingdom && !FactionManager.IsAtWarAgainstFaction(kingdom2, kingdom))
				{
					list.Add(kingdom2);
				}
			}
			return list;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000C578 File Offset: 0x0000A778
		public static List<IFaction> GetPossibleKingdomsToDeclarePeace(Kingdom kingdom)
		{
			List<IFaction> list = new List<IFaction>();
			foreach (Kingdom kingdom2 in Kingdom.All)
			{
				if (kingdom2 != kingdom && FactionManager.IsAtWarAgainstFaction(kingdom2, kingdom))
				{
					list.Add(kingdom2);
				}
			}
			return list;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000C5E0 File Offset: 0x0000A7E0
		public static IEnumerable<Clan> GetAllyMinorFactions(CharacterObject otherCharacter)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000C5E8 File Offset: 0x0000A7E8
		public static Clan ChooseHeirClanForFiefs(Clan oldClan)
		{
			Clan clan = null;
			if (oldClan.Kingdom != null)
			{
				if (!oldClan.Kingdom.IsEliminated && oldClan.Kingdom.RulingClan != oldClan)
				{
					clan = oldClan.Kingdom.RulingClan;
				}
				else
				{
					clan = oldClan.Kingdom.Clans.GetRandomElementWithPredicate<Clan>(delegate(Clan t)
					{
						if (t != oldClan && !t.IsEliminated && !t.IsMinorFaction && !t.AliveLords.IsEmpty<Hero>())
						{
							return t.AliveLords.Any<Hero>((Hero k) => !k.IsChild);
						}
						return false;
					});
				}
			}
			if (clan == null)
			{
				float num = float.MaxValue;
				IEnumerable<Clan> all = Clan.All;
				Func<Clan, bool> <>9__2;
				Func<Clan, bool> func;
				if ((func = <>9__2) == null)
				{
					func = (<>9__2 = delegate(Clan t)
					{
						if (t != oldClan && !t.IsEliminated && !t.IsMinorFaction && !t.AliveLords.IsEmpty<Hero>())
						{
							if (t.AliveLords.Any<Hero>((Hero k) => !k.IsChild))
							{
								return !t.IsBanditFaction;
							}
						}
						return false;
					});
				}
				foreach (Clan clan2 in all.Where<Clan>(func))
				{
					float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(clan2.FactionMidSettlement, oldClan.FactionMidSettlement, false, false, MobileParty.NavigationType.All);
					if (distance < num)
					{
						clan = clan2;
						num = distance;
					}
				}
				if (((clan != null) ? clan.Kingdom : null) != null && !clan.Kingdom.IsEliminated)
				{
					clan = clan.Kingdom.RulingClan;
				}
			}
			if (clan == null)
			{
				clan = Clan.PlayerClan;
			}
			return clan;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000C744 File Offset: 0x0000A944
		private static bool IsMainClanMemberAvailableForRelocate(Hero hero, out TextObject explanation)
		{
			if (hero.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				explanation = new TextObject("{=HAo6iIda}{HERO.NAME} is not eligible.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (hero.PartyBelongedTo != null)
			{
				if (hero.PartyBelongedTo.LeaderHero == hero)
				{
					explanation = new TextObject("{=kNW1qYSi}{HERO.NAME} is leading a party right now.", null);
					explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
					return false;
				}
				if (hero.PartyBelongedTo.MapEvent != null)
				{
					explanation = new TextObject("{=haY6IEw2}{HERO.NAME} is currently in a battle right now.", null);
					explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
					return false;
				}
			}
			if (hero.IsPrisoner)
			{
				explanation = new TextObject("{=hv1ARuaU}{HERO.NAME} is in prison right now.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (hero.IsReleased)
			{
				explanation = new TextObject("{=jGIw0Xku}{HERO.NAME} has just escaped from {?HERO.GENDER}her{?}his{\\?} captors and is currently recovering.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (hero.IsFugitive || hero.IsDisabled || !hero.CanBeGovernorOrHavePartyRole())
			{
				explanation = new TextObject("{=nMmYZ3xi}{HERO.NAME} is not available right now.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (hero.IsTraveling)
			{
				explanation = new TextObject("{=287Epvf0}{HERO.NAME} is traveling right now.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(hero))
			{
				explanation = new TextObject("{=se5704KH}{HERO.NAME} is solving an issue right now.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (Campaign.Current.GetCampaignBehavior<IAlleyCampaignBehavior>().IsHeroAlleyLeaderOfAnyPlayerAlley(hero))
			{
				explanation = new TextObject("{=WBcw6Z9W}{HERO.NAME} is leading an alley.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000C920 File Offset: 0x0000AB20
		public static bool CanPlayerOfferMercenaryService(Kingdom offerKingdom, out List<IFaction> playerWars, out List<IFaction> warsOfFactionToJoin)
		{
			playerWars = new List<IFaction>();
			warsOfFactionToJoin = new List<IFaction>();
			float strengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom = Campaign.Current.Models.DiplomacyModel.GetStrengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom(offerKingdom);
			foreach (Kingdom kingdom in Kingdom.All)
			{
				if (Clan.PlayerClan.MapFaction.IsAtWarWith(kingdom) && kingdom.CurrentTotalStrength > strengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom)
				{
					playerWars.Add(kingdom);
				}
			}
			foreach (Kingdom kingdom2 in Kingdom.All)
			{
				if (offerKingdom.IsAtWarWith(kingdom2))
				{
					warsOfFactionToJoin.Add(kingdom2);
				}
			}
			return Clan.PlayerClan.Kingdom == null && !Clan.PlayerClan.IsAtWarWith(offerKingdom) && Clan.PlayerClan.Tier >= Campaign.Current.Models.ClanTierModel.MercenaryEligibleTier && offerKingdom.Leader.GetRelationWithPlayer() >= (float)Campaign.Current.Models.DiplomacyModel.MinimumRelationWithConversationCharacterToJoinKingdom && warsOfFactionToJoin.Intersect<IFaction>(playerWars).Count<IFaction>() == playerWars.Count && Clan.PlayerClan.Settlements.IsEmpty<Settlement>();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000CA80 File Offset: 0x0000AC80
		public static bool CanPlayerOfferVassalage(Kingdom offerKingdom, out List<IFaction> playerWars, out List<IFaction> warsOfFactionToJoin)
		{
			playerWars = new List<IFaction>();
			warsOfFactionToJoin = new List<IFaction>();
			float strengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom = Campaign.Current.Models.DiplomacyModel.GetStrengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom(offerKingdom);
			foreach (Kingdom kingdom in Kingdom.All)
			{
				if (Clan.PlayerClan.MapFaction.IsAtWarWith(kingdom) && kingdom.CurrentTotalStrength > strengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom)
				{
					playerWars.Add(kingdom);
				}
			}
			foreach (Kingdom kingdom2 in Kingdom.All)
			{
				if (offerKingdom.IsAtWarWith(kingdom2))
				{
					warsOfFactionToJoin.Add(kingdom2);
				}
			}
			return (Clan.PlayerClan.Kingdom == null || Clan.PlayerClan.IsUnderMercenaryService) && !Clan.PlayerClan.IsAtWarWith(offerKingdom) && Clan.PlayerClan.Tier >= Campaign.Current.Models.ClanTierModel.VassalEligibleTier && !offerKingdom.IsEliminated && offerKingdom.Leader.GetRelationWithPlayer() >= (float)Campaign.Current.Models.DiplomacyModel.MinimumRelationWithConversationCharacterToJoinKingdom && warsOfFactionToJoin.Intersect<IFaction>(playerWars).Count<IFaction>() == playerWars.Count;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000CBE8 File Offset: 0x0000ADE8
		public static bool IsMainClanMemberAvailableForRecall(Hero hero, MobileParty targetParty, out TextObject explanation)
		{
			if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.IsMainParty)
			{
				explanation = new TextObject("{=uhOCqJwd}{HERO.NAME} is already in the main party.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (hero.CurrentSettlement != null && (hero.CurrentSettlement.IsUnderSiege || hero.CurrentSettlement.IsUnderRaid))
			{
				explanation = new TextObject("{=L9nn40qu}{HERO.NAME}{.o} location is under attack right now.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (Hero.MainHero.IsPrisoner)
			{
				explanation = new TextObject("{=jRslIaiU}You can't recall a clan member while you are a prisoner.", null);
				return false;
			}
			if (MobileParty.MainParty.MapEvent != null)
			{
				explanation = new TextObject("{=h0pBxG09}You can't recall a clan member while you are in a map event.", null);
				return false;
			}
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				explanation = new TextObject("{=3V2BTAfB}You cannot do this action when you are at sea.", null);
				return false;
			}
			return FactionHelper.IsMainClanMemberAvailableForRelocate(hero, out explanation);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000CCD0 File Offset: 0x0000AED0
		public static bool IsMainClanMemberAvailableForPartyLeaderChange(Hero hero, bool isSend, MobileParty targetParty, out TextObject explanation)
		{
			int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
			if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.IsMainParty && !isSend)
			{
				explanation = new TextObject("{=uhOCqJwd}{HERO.NAME} is already in the main party.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (targetParty != null)
			{
				if (targetParty.MemberRoster.Count == 1 && targetParty.LeaderHero != null)
				{
					explanation = new TextObject("{=pwuEqegC}Party leader is the only member of the party right now.", null);
					return false;
				}
				if (targetParty.MapEvent != null)
				{
					explanation = new TextObject("{=yC52EBCb}Target party is currently in a battle right now.", null);
					return false;
				}
				if (targetParty.Army != null)
				{
					explanation = new TextObject("{=2iRg3vpP}Target party is currently in an army right now.", null);
					return false;
				}
				if (targetParty.IsCurrentlyAtSea)
				{
					explanation = new TextObject("{=TbD2qPLy}Target party is currently sailing.", null);
					return false;
				}
			}
			if (hero.CurrentSettlement != null && (hero.CurrentSettlement.IsUnderSiege || hero.CurrentSettlement.IsUnderRaid))
			{
				explanation = new TextObject("{=L9nn40qu}{HERO.NAME}{.o} location is under attack right now.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (hero.GovernorOf != null)
			{
				explanation = new TextObject("{=bgVZcd1I}{HERO.NAME} is a governor.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.IsCurrentlyAtSea)
			{
				explanation = new TextObject("{=1ELK1UbN}{HERO.NAME} is currently sailing.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (!FactionHelper.IsMainClanMemberAvailableForRelocate(hero, out explanation))
			{
				return false;
			}
			if (partyGoldLowerThreshold - hero.Gold > Hero.MainHero.Gold)
			{
				explanation = new TextObject("{=xpCdwmlX}You don't have enough gold to make {HERO.NAME} a party leader.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			explanation = new TextObject("{=NAseSXPl}It would take {HOUR} {?HOUR > 1}hours{?}hour{\\?} for {HERO.NAME} to arrive at your party.", null);
			explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
			float num = ((targetParty != null) ? Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, targetParty.Party).ResultNumber : Campaign.Current.Models.DelayedTeleportationModel.MaximumDistanceForDelayAsDays);
			explanation.SetTextVariable("HOUR", (int)Math.Ceiling((double)num));
			return true;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000CEF4 File Offset: 0x0000B0F4
		public static bool IsMainClanMemberAvailableForSendingSettlement(Hero hero, Settlement targetSettlement, out TextObject explanation)
		{
			if (hero.PartyBelongedTo != null && (hero.PartyBelongedTo.IsCurrentlyAtSea || hero.PartyBelongedTo.IsInNavalAutoTravel))
			{
				explanation = new TextObject("{=1ELK1UbN}{HERO.NAME} is currently sailing.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (hero.CurrentSettlement != null && (hero.CurrentSettlement.IsUnderSiege || hero.CurrentSettlement.IsUnderRaid))
			{
				explanation = new TextObject("{=L9nn40qu}{HERO.NAME}{.o} location is under attack right now.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (targetSettlement.IsUnderRaid || targetSettlement.IsUnderSiege)
			{
				explanation = new TextObject("{=1tGP6vJn}Target settlement is under attack right now.", null);
				return false;
			}
			if (hero.GovernorOf != null)
			{
				explanation = new TextObject("{=bgVZcd1I}{HERO.NAME} is a governor.", null);
				explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
				return false;
			}
			if (!FactionHelper.IsMainClanMemberAvailableForRelocate(hero, out explanation))
			{
				return false;
			}
			explanation = new TextObject("{=NAseSXPl}It would take {HOUR} {?HOUR > 1}hours{?}hour{\\?} for {HERO.NAME} to arrive at your party.", null);
			explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
			float resultNumber = Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, targetSettlement.Party).ResultNumber;
			explanation.SetTextVariable("HOUR", (int)Math.Ceiling((double)resultNumber));
			return true;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000D038 File Offset: 0x0000B238
		public static bool IsMainClanMemberAvailableForSendingSettlementAsGovernor(Hero hero, Settlement settlementOfGovernor, out TextObject explanation)
		{
			if (hero.PartyBelongedToAsPrisoner != null)
			{
				explanation = new TextObject("{=knwId8DG}You cannot assign a prisoner as a governor of a settlement", null);
				return false;
			}
			if (hero == Hero.MainHero)
			{
				explanation = new TextObject("{=uoDuiBZR}You cannot assign yourself as a governor", null);
				return false;
			}
			if (hero.PartyBelongedTo != null)
			{
				if (hero.PartyBelongedTo.IsCurrentlyAtSea || hero.PartyBelongedTo.IsInNavalAutoTravel)
				{
					explanation = new TextObject("{=1ELK1UbN}{HERO.NAME} is currently sailing.", null);
					explanation.SetCharacterProperties("HERO", hero.CharacterObject, false);
					return false;
				}
				if (hero.PartyBelongedTo.LeaderHero == hero)
				{
					explanation = new TextObject("{=pWObBhj5}You cannot assign a party leader as a new governor of a settlement", null);
					return false;
				}
			}
			if (hero.IsFugitive)
			{
				explanation = new TextObject("{=KghY9qwl}You cannot assign a fugitive as the new governor of a settlement", null);
				return false;
			}
			if (hero.IsReleased)
			{
				explanation = new TextObject("{=mOFjZuSf}You cannot assign a newly released hero as the new governor of a settlement", null);
				return false;
			}
			if (settlementOfGovernor != null)
			{
				explanation = new TextObject("{=YbGu9rSH}This character is already the governor of {SETTLEMENT_NAME}", null);
				explanation.SetTextVariable("SETTLEMENT_NAME", settlementOfGovernor.Town.Name);
				return false;
			}
			if (!FactionHelper.IsMainClanMemberAvailableForRelocate(hero, out explanation))
			{
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000D13B File Offset: 0x0000B33B
		public static bool IsLooterFaction(IFaction faction)
		{
			return faction.IsBanditFaction && !faction.Culture.CanHaveSettlement && !faction.HasNavalNavigationCapability && faction.StringId != "deserters";
		}
	}
}
