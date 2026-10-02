using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000464 RID: 1124
	public class SiegeAftermathCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600489B RID: 18587 RVA: 0x00169AD8 File Offset: 0x00167CD8
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnSiegeAftermathAppliedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, SiegeAftermathAction.SiegeAftermath, Clan, Dictionary<MobileParty, float>>(this.OnSiegeAftermathApplied));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnBuildingLevelChangedEvent.AddNonSerializedListener(this, new Action<Town, Building, int>(this.OnBuildingLevelChanged));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
		}

		// Token: 0x0600489C RID: 18588 RVA: 0x00169B58 File Offset: 0x00167D58
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x00169B64 File Offset: 0x00167D64
		private void AddGameMenus(CampaignGameStarter gameSystemInitializer)
		{
			gameSystemInitializer.AddGameMenu("menu_settlement_taken", "", new OnInitDelegate(this.menu_settlement_taken_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenu("menu_settlement_taken_player_leader", "{=!}{SETTLEMENT_TAKEN_TEXT}", new OnInitDelegate(this.menu_settlement_taken_player_leader_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_settlement_taken_player_leader", "menu_settlement_taken_devastate", "{=v0mZi3Zd}Devastate {DEVASTATE_INFLUENCE_COST_TEXT}", new GameMenuOption.OnConditionDelegate(this.menu_settlement_taken_devastate_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_devastate_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("menu_settlement_taken_player_leader", "menu_settlement_taken_pillage", "{=tZCLAkGZ}Pillage", new GameMenuOption.OnConditionDelegate(this.menu_settlement_taken_pillage_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_pillage_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("menu_settlement_taken_player_leader", "menu_settlement_taken_show_mercy", "{=EuwtMZGZ}Show Mercy {SHOW_MERCY_INFLUENCE_COST_TEXT}", new GameMenuOption.OnConditionDelegate(this.menu_settlement_taken_show_mercy_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_show_mercy_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_settlement_taken_player_army_member", "{=!}{LEADER_DECISION_TEXT}", new OnInitDelegate(this.menu_settlement_taken_player_army_member_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_settlement_taken_player_army_member", "menu_settlement_taken_continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_settlement_taken_player_participant", "{=!}{PLAYER_PARTICIPANT_TEXT}", new OnInitDelegate(this.menu_settlement_taken_player_participant_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_settlement_taken_player_participant", "menu_settlement_taken_continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("siege_aftermath_contextual_summary", "{=!}{START_OF_EXPLANATION}{newline} {newline}{CONTEXTUAL_SUMMARY_TEXT}{newline} {newline}{END_OF_EXPLANATION}", new OnInitDelegate(this.siege_aftermath_contextual_summary_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("siege_aftermath_contextual_summary", "menu_settlement_taken_continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_continue_on_consequence), false, -1, false, null);
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x00169D34 File Offset: 0x00167F34
		private MBReadOnlyList<ValueTuple<PartyBase, float>> GetLootPercentagesOfPartiesOnSideForSiegeAftermath(MapEvent mapEvent, BattleSideEnum side)
		{
			MBReadOnlyList<MapEventParty> mbreadOnlyList = mapEvent.PartiesOnSide(side);
			MBList<ValueTuple<PartyBase, float>> mblist = new MBList<ValueTuple<PartyBase, float>>(mbreadOnlyList.Count);
			int num = mapEvent.GetMapEventSide(side).CalculateTotalContribution();
			foreach (MapEventParty mapEventParty in mbreadOnlyList)
			{
				mblist.Add(new ValueTuple<PartyBase, float>(mapEventParty.Party, (float)((int)(100f * ((float)mapEventParty.ContributionToBattle / (float)num)))));
			}
			return mblist;
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x00169DC0 File Offset: 0x00167FC0
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			BattleSideEnum battleSideEnum = ((mapEvent.IsSallyOut || mapEvent.IsBlockadeSallyOut) ? BattleSideEnum.Defender : BattleSideEnum.Attacker);
			if ((mapEvent.IsSiegeAssault || mapEvent.IsSallyOut || mapEvent.IsBlockadeSallyOut) && mapEvent.WinningSide == battleSideEnum && mapEvent.MapEventSettlement != null)
			{
				this._siegeEventPartyContributions.Clear();
				foreach (ValueTuple<PartyBase, float> valueTuple in this.GetLootPercentagesOfPartiesOnSideForSiegeAftermath(mapEvent, battleSideEnum))
				{
					if (valueTuple.Item1.IsMobile && !this._siegeEventPartyContributions.ContainsKey(valueTuple.Item1.MobileParty))
					{
						this._siegeEventPartyContributions.Add(valueTuple.Item1.MobileParty, valueTuple.Item2);
					}
				}
				if (mapEvent.GetMapEventSide(battleSideEnum).IsMainPartyAmongParties())
				{
					this._playerEncounterAftermathDamagedBuildings.Clear();
					this._wasPlayerArmyMember = false;
					this._besiegerParty = mapEvent.GetMapEventSide(battleSideEnum).LeaderParty.MobileParty;
					this._prevSettlementOwnerClan = mapEvent.MapEventSettlement.OwnerClan;
					if (this._besiegerParty != MobileParty.MainParty)
					{
						if (this._besiegerParty.Army != null && this._besiegerParty.Army.Parties.Contains(MobileParty.MainParty))
						{
							this._wasPlayerArmyMember = true;
						}
						else if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
						{
							this._wasPlayerArmyMember = true;
						}
						this._playerEncounterAftermath = this.DetermineAISiegeAftermath(this._besiegerParty, mapEvent.MapEventSettlement);
						SiegeAftermathAction.ApplyAftermath(this._besiegerParty, mapEvent.MapEventSettlement, this._playerEncounterAftermath, this._prevSettlementOwnerClan, this._siegeEventPartyContributions);
						return;
					}
				}
				else
				{
					if (mapEvent.MapEventSettlement.SiegeEvent != null)
					{
						MobileParty leaderParty = mapEvent.MapEventSettlement.SiegeEvent.BesiegerCamp.LeaderParty;
						SiegeAftermathAction.SiegeAftermath siegeAftermath = this.DetermineAISiegeAftermath(leaderParty, mapEvent.MapEventSettlement);
						SiegeAftermathAction.ApplyAftermath(leaderParty, mapEvent.MapEventSettlement, siegeAftermath, mapEvent.MapEventSettlement.OwnerClan, this._siegeEventPartyContributions);
						return;
					}
					Debug.FailedAssert("Siege event is null in siege aftermath", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\SiegeAftermathCampaignBehavior.cs", "OnMapEventEnded", 145);
				}
			}
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x0016A000 File Offset: 0x00168200
		private void OnSiegeAftermathApplied(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			float siegeAftermathInfluenceCost = this.GetSiegeAftermathInfluenceCost(attackerParty, settlement, aftermathType);
			if (siegeAftermathInfluenceCost > 0f)
			{
				ChangeClanInfluenceAction.Apply(attackerParty.ActualClan, -siegeAftermathInfluenceCost);
			}
			this._settlementProsperityCache = settlement.Town.Prosperity;
			settlement.Town.Prosperity += this.GetSiegeAftermathProsperityPenalty(attackerParty, settlement, aftermathType);
			if (aftermathType != SiegeAftermathAction.SiegeAftermath.ShowMercy)
			{
				int siegeAftermathProjectsLoss = this.GetSiegeAftermathProjectsLoss(attackerParty, aftermathType);
				for (int i = 0; i < siegeAftermathProjectsLoss; i++)
				{
					settlement.Town.Buildings.GetRandomElementWithPredicate<Building>((Building x) => !x.BuildingType.IsDailyProject).LevelDown();
				}
				settlement.Town.Loyalty += this.GetSiegeAftermathLoyaltyPenalty(aftermathType);
				if (settlement.IsTown)
				{
					foreach (Hero hero in settlement.Notables)
					{
						hero.AddPower(hero.Power * this.GetSiegeAftermathNotablePowerModifierForAftermath(aftermathType));
					}
				}
				if (previousSettlementOwner.Leader == null)
				{
					Debug.Print(string.Format("{0}: {1} leader was null", previousSettlementOwner.StringId, previousSettlementOwner), 0, Debug.DebugColor.White, 17592186044416UL);
					Debug.FailedAssert(string.Format("{0}: {1} leader was null", previousSettlementOwner.StringId, previousSettlementOwner), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\SiegeAftermathCampaignBehavior.cs", "OnSiegeAftermathApplied", 187);
				}
				if (attackerParty.LeaderHero != null)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(attackerParty.LeaderHero, previousSettlementOwner.Leader, this.GetSiegeAftermathRelationPenaltyWithSettlementOwner(aftermathType), true);
				}
			}
			float num = (float)this.GetSiegeAftermathArmyGoldGain(attackerParty, settlement, aftermathType);
			foreach (KeyValuePair<MobileParty, float> keyValuePair in partyContributions)
			{
				MobileParty key = keyValuePair.Key;
				int siegeAftermathPartyGoldGain = this.GetSiegeAftermathPartyGoldGain(num, keyValuePair.Value);
				if (key.LeaderHero != null)
				{
					GiveGoldAction.ApplyForPartyToCharacter(null, key.LeaderHero, siegeAftermathPartyGoldGain, false);
				}
				else
				{
					GiveGoldAction.ApplyForCharacterToParty(null, key.Party, siegeAftermathPartyGoldGain, false);
				}
				key.RecentEventsMorale += this.GetSiegeAftermathPartyMoraleBonus(attackerParty, settlement, aftermathType);
				if (attackerParty == MobileParty.MainParty && key != attackerParty && key.LeaderHero != null && aftermathType != SiegeAftermathAction.SiegeAftermath.Pillage && attackerParty.MapFaction.Culture != settlement.Culture)
				{
					int siegeAftermathRelationChangeWithLord = this.GetSiegeAftermathRelationChangeWithLord(key.LeaderHero, aftermathType);
					if (siegeAftermathRelationChangeWithLord != 0)
					{
						ChangeRelationAction.ApplyPlayerRelation(key.LeaderHero, siegeAftermathRelationChangeWithLord, true, true);
					}
				}
			}
			if (attackerParty == MobileParty.MainParty && aftermathType != SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				TraitLevelingHelper.OnSiegeAftermathApplied(settlement, aftermathType, new TraitObject[] { DefaultTraits.Mercy });
			}
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x0016A2A8 File Offset: 0x001684A8
		private SiegeAftermathAction.SiegeAftermath DetermineSiegeAftermathOnEncounterLeaderDeath(MobileParty attackerParty, Settlement settlement)
		{
			IFaction mapFaction = attackerParty.MapFaction;
			if (((mapFaction != null) ? mapFaction.Culture : null) != settlement.Culture)
			{
				return SiegeAftermathAction.SiegeAftermath.Devastate;
			}
			return SiegeAftermathAction.SiegeAftermath.ShowMercy;
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x0016A2C7 File Offset: 0x001684C7
		private bool IsMobilePartyLeaderAliveForSiegeAftermath(MobileParty attackerParty)
		{
			return attackerParty.LeaderHero != null && attackerParty.LeaderHero.IsAlive && attackerParty.LeaderHero.DeathMark != KillCharacterAction.KillCharacterActionDetail.DiedInBattle && attackerParty.LeaderHero.DeathMark != KillCharacterAction.KillCharacterActionDetail.WoundedInBattle;
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x0016A300 File Offset: 0x00168500
		private SiegeAftermathAction.SiegeAftermath DetermineAISiegeAftermath(MobileParty attackerParty, Settlement settlement)
		{
			if (!this.IsMobilePartyLeaderAliveForSiegeAftermath(attackerParty))
			{
				return this.DetermineSiegeAftermathOnEncounterLeaderDeath(attackerParty, settlement);
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			float num = ((attackerParty.Army != null) ? attackerParty.Army.Morale : attackerParty.Morale);
			IFaction mapFaction = attackerParty.MapFaction;
			if (((mapFaction != null) ? mapFaction.Culture : null) != settlement.Culture)
			{
				Clan actualClan = attackerParty.ActualClan;
				float? num2 = ((actualClan != null) ? new float?(actualClan.Influence) : null);
				float num3 = 2f * this.GetSiegeAftermathInfluenceCost(attackerParty, settlement, SiegeAftermathAction.SiegeAftermath.ShowMercy);
				if (!((num2.GetValueOrDefault() > num3) & (num2 != null)) || num <= 60f)
				{
					goto IL_00A0;
				}
			}
			flag = true;
			IL_00A0:
			IFaction mapFaction2 = attackerParty.MapFaction;
			if (((mapFaction2 != null) ? mapFaction2.Culture : null) != settlement.Culture)
			{
				flag2 = true;
			}
			IFaction mapFaction3 = attackerParty.MapFaction;
			if (((mapFaction3 != null) ? mapFaction3.Culture : null) != settlement.Culture)
			{
				Clan actualClan2 = attackerParty.ActualClan;
				float? num2 = ((actualClan2 != null) ? new float?(actualClan2.Influence) : null);
				float num3 = 2f * this.GetSiegeAftermathInfluenceCost(attackerParty, settlement, SiegeAftermathAction.SiegeAftermath.Devastate);
				if (((num2.GetValueOrDefault() > num3) & (num2 != null)) && num < 90f)
				{
					flag3 = true;
				}
			}
			Hero leaderHero = attackerParty.LeaderHero;
			int num4 = ((leaderHero != null) ? leaderHero.GetTraitLevel(DefaultTraits.Mercy) : 0);
			float num5;
			float num6;
			float num7;
			if (num4 > 0)
			{
				num5 = 0.4f + 0.2f * (float)num4;
				num6 = 1f - num5;
				num7 = 0f;
			}
			else if (num4 < 0)
			{
				num7 = 0.4f + 0.2f * (float)MathF.Abs(num4);
				num6 = 1f - num7;
				num5 = 0f;
			}
			else
			{
				num5 = 0.2f;
				num6 = 0.6f;
				num7 = 0.2f;
			}
			if (!flag)
			{
				num6 += num5;
				num5 = 0f;
			}
			if (!flag3)
			{
				num6 += num7;
				num7 = 0f;
			}
			if (!flag2)
			{
				num5 += num6;
				num6 = 0f;
			}
			return MBRandom.ChooseWeighted<SiegeAftermathAction.SiegeAftermath>(new List<ValueTuple<SiegeAftermathAction.SiegeAftermath, float>>
			{
				new ValueTuple<SiegeAftermathAction.SiegeAftermath, float>(SiegeAftermathAction.SiegeAftermath.ShowMercy, num5),
				new ValueTuple<SiegeAftermathAction.SiegeAftermath, float>(SiegeAftermathAction.SiegeAftermath.Pillage, num6),
				new ValueTuple<SiegeAftermathAction.SiegeAftermath, float>(SiegeAftermathAction.SiegeAftermath.Devastate, num7)
			});
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x0016A538 File Offset: 0x00168738
		private void OnBuildingLevelChanged(Town town, Building building, int level)
		{
			if (town.Settlement == PlayerEncounter.EncounterSettlement && level < 0)
			{
				if (!this._playerEncounterAftermathDamagedBuildings.ContainsKey(building))
				{
					this._playerEncounterAftermathDamagedBuildings.Add(building, 0);
				}
				Dictionary<Building, int> playerEncounterAftermathDamagedBuildings = this._playerEncounterAftermathDamagedBuildings;
				playerEncounterAftermathDamagedBuildings[building] += -1;
			}
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x0016A58C File Offset: 0x0016878C
		private void HandlePlayerDeathDuringSiegeAftermath()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			SiegeAftermathAction.SiegeAftermath siegeAftermath = this.DetermineSiegeAftermathOnEncounterLeaderDeath(MobileParty.MainParty, currentSettlement);
			SiegeAftermathAction.ApplyAftermath(MobileParty.MainParty, currentSettlement, siegeAftermath, this._prevSettlementOwnerClan, this._siegeEventPartyContributions);
			GameMenu.SwitchToMenu("siege_aftermath_contextual_summary");
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x0016A5D0 File Offset: 0x001687D0
		private void menu_settlement_taken_on_init(MenuCallbackArgs args)
		{
			MobileParty besiegerParty = this._besiegerParty;
			if (besiegerParty == MobileParty.MainParty)
			{
				if (this.IsMobilePartyLeaderAliveForSiegeAftermath(besiegerParty))
				{
					GameMenu.SwitchToMenu("menu_settlement_taken_player_leader");
					return;
				}
				this.HandlePlayerDeathDuringSiegeAftermath();
				return;
			}
			else
			{
				if (this._wasPlayerArmyMember)
				{
					GameMenu.SwitchToMenu("menu_settlement_taken_player_army_member");
					return;
				}
				GameMenu.SwitchToMenu("menu_settlement_taken_player_participant");
				return;
			}
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x0016A624 File Offset: 0x00168824
		private void menu_settlement_taken_player_leader_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.GameMenu.GetText().SetTextVariable("SETTLEMENT_TAKEN_TEXT", new TextObject("{=QvyFYn1b}The defenders are routed, and it's clear that {TOWN_NAME} is yours. It's time for you to determine the fate of the {?IS_CITY}city{?}fortress{\\?}.", null));
			MBTextManager.SetTextVariable("TOWN_NAME", Settlement.CurrentSettlement.Name, false);
			MBTextManager.SetTextVariable("IS_CITY", Settlement.CurrentSettlement.IsTown ? 1 : 0);
			args.MenuContext.SetBackgroundMeshName("encounter_win");
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x0016A698 File Offset: 0x00168898
		private void menu_settlement_taken_player_army_member_on_init(MenuCallbackArgs args)
		{
			bool flag = this._besiegerParty.Army != null && this._besiegerParty.Army.Parties.Contains(MobileParty.MainParty);
			Settlement currentSettlement = this._besiegerParty.CurrentSettlement;
			TextObject textObject;
			if (this._playerEncounterAftermath == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				if (flag)
				{
					textObject = new TextObject("{=peHCARhM}{DEFAULT_TEXT}{ARMY_LEADER.LINK} has ordered that {SETTLEMENT} to be laid waste. {?ARMY_LEADER.GENDER}Her{?}His{\\?} troops sweep through the {?IS_CITY}city{?}fortress{\\?} taking whatever loot they like and setting fire to the rest.", null);
				}
				else if (this._wasPlayerArmyMember)
				{
					textObject = new TextObject("{=qeRRWMfU}{DEFAULT_TEXT}{ARMY_LEADER.LINK} fell during the fighting. {?ARMY_LEADER.GENDER}Her{?}His{\\?} vengeful troops sweep through the {?IS_CITY}city{?}fortress{\\?} taking whatever loot they like and setting fire to the rest.", null);
				}
				else
				{
					textObject = TextObject.GetEmpty();
				}
				textObject.SetTextVariable("IS_CITY", currentSettlement.IsTown ? 1 : 0);
			}
			else if (this._playerEncounterAftermath == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				if (flag)
				{
					textObject = new TextObject("{=BXw5MwX7}{DEFAULT_TEXT}{ARMY_LEADER.LINK} grants {?ARMY_LEADER.GENDER}her{?}his{\\?} men their customary right of pillage after a successful siege. {?ARMY_LEADER.GENDER}She{?}He{\\?} tells them they may take property but must spare the townsfolk's lives.", null);
				}
				else if (this._wasPlayerArmyMember)
				{
					Debug.FailedAssert("_wasPlayerArmyMember", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\SiegeAftermathCampaignBehavior.cs", "menu_settlement_taken_player_army_member_on_init", 435);
					textObject = new TextObject("{=99v8GTTe}{DEFAULT_TEXT}Before {?ARMY_LEADER.GENDER}she{?}he{\\?} fell, {ARMY_LEADER.LINK} granted {?ARMY_LEADER.GENDER}her{?}his{\\?} men their customary right of pillage after a successful siege. They may take property but must spare the townsfolk's lives.", null);
				}
				else
				{
					textObject = TextObject.GetEmpty();
				}
			}
			else
			{
				IFaction mapFaction = this._besiegerParty.MapFaction;
				if (((mapFaction != null) ? mapFaction.Culture : null) == this._besiegerParty.CurrentSettlement.Culture)
				{
					if (flag)
					{
						textObject = new TextObject("{=Wmq47pvL}{DEFAULT_TEXT}{ARMY_LEADER.LINK} had to show mercy to the people of {SETTLEMENT} who were originally descendants of {FACTION}.", null);
						textObject.SetTextVariable("FACTION", currentSettlement.Culture.GetName());
					}
					else if (this._wasPlayerArmyMember)
					{
						textObject = new TextObject("{=F5Xc0m5O}{DEFAULT_TEXT}{ARMY_LEADER.LINK} fell during the fighting. {?ARMY_LEADER.GENDER}Her{?}His{\\?} troops, reluctant to harm their {CULTURE_ADJ} kinfolk, forego their traditional right of pillage.", null);
						textObject.SetTextVariable("CULTURE_ADJ", FactionHelper.GetAdjectiveForFactionCulture(this._besiegerParty.MapFaction.Culture));
					}
					else
					{
						textObject = TextObject.GetEmpty();
					}
				}
				else if (flag)
				{
					textObject = new TextObject("{=Bp0ZQbfp}{DEFAULT_TEXT}{ARMY_LEADER.LINK} has decided to show mercy to the people of {SETTLEMENT}. You can hear disgruntled murmuring among the troops, who have been denied their customary right of pillage.", null);
				}
				else if (this._wasPlayerArmyMember)
				{
					Debug.FailedAssert("_wasPlayerArmyMember", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\SiegeAftermathCampaignBehavior.cs", "menu_settlement_taken_player_army_member_on_init", 470);
					textObject = new TextObject("{=ULtzLvXi}{DEFAULT_TEXT}Before {?ARMY_LEADER.GENDER}she{?}he{\\?} fell, {ARMY_LEADER.LINK} gave orders that mercy should be shown to the people of {SETTLEMENT}.", null);
				}
				else
				{
					textObject = TextObject.GetEmpty();
				}
			}
			TextObject text = args.MenuContext.GameMenu.GetText();
			TextObject textObject2 = new TextObject("{=hvQUqRSb}{SETTLEMENT} has been taken by an army of which you are a member. ", null);
			textObject2.SetTextVariable("SETTLEMENT", currentSettlement.GetName());
			textObject.SetTextVariable("DEFAULT_TEXT", textObject2);
			string text2 = "ARMY_LEADER";
			LordPartyComponent lordPartyComponent = this._besiegerParty.LordPartyComponent;
			CharacterObject characterObject;
			if (lordPartyComponent == null)
			{
				characterObject = null;
			}
			else
			{
				Hero owner = lordPartyComponent.Owner;
				characterObject = ((owner != null) ? owner.CharacterObject : null);
			}
			StringHelpers.SetCharacterProperties(text2, characterObject ?? CharacterObject.PlayerCharacter, textObject, false);
			textObject.SetTextVariable("SETTLEMENT", currentSettlement.GetName());
			text.SetTextVariable("LEADER_DECISION_TEXT", textObject);
			text.SetTextVariable("SETTLEMENT", currentSettlement.GetName());
			args.MenuContext.SetBackgroundMeshName("encounter_win");
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x0016A918 File Offset: 0x00168B18
		private void menu_settlement_taken_player_participant_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = this._besiegerParty.CurrentSettlement;
			TextObject textObject = new TextObject("{=C2KeQd0a}{ENCOUNTER_LEADER.LINK} thanks you for helping in the siege of {SETTLEMENT}. You were able to loot your fallen foes, but you do not participate in the sack of the {?IS_TOWN}town{?}castle{\\?} as you are not part of the army that took it.", null);
			string text = "ENCOUNTER_LEADER";
			LordPartyComponent lordPartyComponent = this._besiegerParty.LordPartyComponent;
			CharacterObject characterObject;
			if (lordPartyComponent == null)
			{
				characterObject = null;
			}
			else
			{
				Hero owner = lordPartyComponent.Owner;
				characterObject = ((owner != null) ? owner.CharacterObject : null);
			}
			StringHelpers.SetCharacterProperties(text, characterObject ?? CharacterObject.PlayerCharacter, textObject, false);
			textObject.SetTextVariable("SETTLEMENT", currentSettlement.GetName());
			textObject.SetTextVariable("IS_TOWN", currentSettlement.IsTown ? 1 : 0);
			args.MenuContext.GameMenu.GetText().SetTextVariable("PLAYER_PARTICIPANT_TEXT", textObject);
			args.MenuContext.SetBackgroundMeshName("encounter_win");
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x0016A9CC File Offset: 0x00168BCC
		private void menu_settlement_taken_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x0016A9D4 File Offset: 0x00168BD4
		private bool menu_settlement_taken_devastate_on_condition(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty attackerParty = this._besiegerParty;
			Army army = attackerParty.Army;
			MBList<MobileParty> mblist = ((army != null) ? army.Parties.WhereQ<MobileParty>((MobileParty t) => t != attackerParty && t.LeaderHero != null && t.LeaderHero.GetTraitLevel(DefaultTraits.Mercy) > 0).ToMBList<MobileParty>() : null) ?? new MBList<MobileParty>();
			int num = (int)this.GetSiegeAftermathInfluenceCost(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.Devastate);
			bool flag = (float)num > 0f;
			CultureObject culture = currentSettlement.Culture;
			IFaction mapFaction = Hero.MainHero.MapFaction;
			bool flag2 = culture == ((mapFaction != null) ? mapFaction.Culture : null);
			TextObject textObject = new TextObject("{=FPPb7ur6}({INFLUENCE_AMOUNT}{INFLUENCE_ICON})", null);
			textObject.SetTextVariable("INFLUENCE_AMOUNT", num);
			textObject.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
			MBTextManager.SetTextVariable("DEVASTATE_INFLUENCE_COST_TEXT", flag ? textObject : TextObject.GetEmpty(), false);
			TextObject textObject2 = new TextObject("{=0FxtNPvV}You cannot devastate a settlement that has your faction culture.{newline}", null);
			TextObject textObject3 = new TextObject("{=Q9RXyDBz}{newline} • {HERO.NAME} must use {INFLUENCE_AMOUNT} influence to convince {MERCIFUL_LORD_COUNT} merciful leaders of this action:{newline} {MERCIFUL_LORDS}", null);
			string text = "HERO";
			Hero leaderHero = attackerParty.LeaderHero;
			StringHelpers.SetCharacterProperties(text, (leaderHero != null) ? leaderHero.CharacterObject : null, textObject3, false);
			textObject3.SetTextVariable("INFLUENCE_AMOUNT", num);
			textObject3.SetTextVariable("MERCIFUL_LORD_COUNT", mblist.Count);
			List<TextObject> list = new List<TextObject>();
			foreach (MobileParty mobileParty in mblist)
			{
				list.Add(mobileParty.LeaderHero.Name);
			}
			textObject3.SetTextVariable("MERCIFUL_LORDS", GameTexts.GameTextHelper.MergeTextObjectsWithSymbol(list, new TextObject("{=!}{newline}", null), null));
			textObject3.SetTextVariable("INFLUENCE_AMOUNT", num);
			TextObject textObject4 = new TextObject("{=!}{CULTURE_CONDITION_TEXT}{STATIC_CONDITIONS_TEXT}{INFLUENCE_CONDITION_TEXT}", null);
			textObject4.SetTextVariable("CULTURE_CONDITION_TEXT", flag2 ? textObject2 : TextObject.GetEmpty());
			textObject4.SetTextVariable("STATIC_CONDITIONS_TEXT", this.GetSiegeAftermathConsequencesText(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.Devastate, true));
			textObject4.SetTextVariable("INFLUENCE_CONDITION_TEXT", flag ? textObject3 : TextObject.GetEmpty());
			args.IsEnabled = this.IsSiegeAftermathPossible(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.Devastate);
			args.Tooltip = textObject4;
			args.optionLeaveType = GameMenuOption.LeaveType.Devastate;
			return true;
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x0016AC18 File Offset: 0x00168E18
		private void menu_settlement_taken_devastate_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty besiegerParty = this._besiegerParty;
			this._playerEncounterAftermath = SiegeAftermathAction.SiegeAftermath.Devastate;
			SiegeAftermathAction.ApplyAftermath(besiegerParty, currentSettlement, this._playerEncounterAftermath, this._prevSettlementOwnerClan, this._siegeEventPartyContributions);
			GameMenu.SwitchToMenu("siege_aftermath_contextual_summary");
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x0016AC5A File Offset: 0x00168E5A
		private bool continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x0016AC68 File Offset: 0x00168E68
		private bool menu_settlement_taken_pillage_on_condition(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty besiegerParty = this._besiegerParty;
			CultureObject culture = currentSettlement.Culture;
			IFaction mapFaction = Hero.MainHero.MapFaction;
			bool flag = culture == ((mapFaction != null) ? mapFaction.Culture : null);
			TextObject textObject = new TextObject("{=!}{CULTURE_CONDITION_TEXT}{STATIC_CONDITIONS_TEXT}", null);
			TextObject textObject2 = new TextObject("{=uwmHjy7z}You cannot pillage a settlement that has your faction culture.{newline}", null);
			textObject.SetTextVariable("CULTURE_CONDITION_TEXT", flag ? textObject2 : TextObject.GetEmpty());
			textObject.SetTextVariable("STATIC_CONDITIONS_TEXT", this.GetSiegeAftermathConsequencesText(besiegerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.Pillage, true));
			args.IsEnabled = this.IsSiegeAftermathPossible(besiegerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.Pillage);
			args.Tooltip = textObject;
			args.optionLeaveType = GameMenuOption.LeaveType.Pillage;
			return true;
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x0016AD08 File Offset: 0x00168F08
		private void menu_settlement_taken_pillage_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty besiegerParty = this._besiegerParty;
			this._playerEncounterAftermath = SiegeAftermathAction.SiegeAftermath.Pillage;
			SiegeAftermathAction.ApplyAftermath(besiegerParty, currentSettlement, this._playerEncounterAftermath, this._prevSettlementOwnerClan, this._siegeEventPartyContributions);
			GameMenu.SwitchToMenu("siege_aftermath_contextual_summary");
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x0016AD4C File Offset: 0x00168F4C
		private bool menu_settlement_taken_show_mercy_on_condition(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty attackerParty = this._besiegerParty;
			Army army = attackerParty.Army;
			MBList<MobileParty> mblist = ((army != null) ? army.Parties.WhereQ<MobileParty>((MobileParty t) => t != attackerParty && t.LeaderHero != null && t.LeaderHero.GetTraitLevel(DefaultTraits.Mercy) < 0).ToMBList<MobileParty>() : null) ?? new MBList<MobileParty>();
			int num = (int)this.GetSiegeAftermathInfluenceCost(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.ShowMercy);
			CultureObject culture = currentSettlement.Culture;
			IFaction mapFaction = attackerParty.MapFaction;
			bool flag = culture == ((mapFaction != null) ? mapFaction.Culture : null);
			bool flag2 = (float)num > 0f;
			TextObject textObject = new TextObject("{=FPPb7ur6}({INFLUENCE_AMOUNT}{INFLUENCE_ICON})", null);
			textObject.SetTextVariable("INFLUENCE_AMOUNT", num);
			textObject.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
			MBTextManager.SetTextVariable("SHOW_MERCY_INFLUENCE_COST_TEXT", flag2 ? textObject : TextObject.GetEmpty(), false);
			TextObject textObject2 = new TextObject("{=aXFYyBEQ}Showing mercy to a settlement that shares your faction's culture requires no influence.{newline}", null);
			TextObject textObject3 = new TextObject("{=bn5fpYx3}{newline} • {HERO.NAME} must use {INFLUENCE_AMOUNT} influence to convince {CRUEL_LORD_COUNT} non-merciful leaders of this action:{newline} {CRUEL_LORDS}", null);
			string text = "HERO";
			Hero leaderHero = attackerParty.LeaderHero;
			StringHelpers.SetCharacterProperties(text, (leaderHero != null) ? leaderHero.CharacterObject : null, textObject3, false);
			textObject3.SetTextVariable("INFLUENCE_AMOUNT", num);
			textObject3.SetTextVariable("CRUEL_LORD_COUNT", mblist.Count);
			List<TextObject> list = new List<TextObject>();
			foreach (MobileParty mobileParty in mblist)
			{
				list.Add(mobileParty.LeaderHero.Name);
			}
			textObject3.SetTextVariable("CRUEL_LORDS", GameTexts.GameTextHelper.MergeTextObjectsWithSymbol(list, new TextObject("{=!}{newline}", null), null));
			TextObject textObject4 = new TextObject("{=!}{CULTURE_CONDITION_TEXT}{STATIC_CONDITIONS_TEXT}{INFLUENCE_CONDITION_TEXT}", null);
			textObject4.SetTextVariable("CULTURE_CONDITION_TEXT", flag ? textObject2 : TextObject.GetEmpty());
			textObject4.SetTextVariable("STATIC_CONDITIONS_TEXT", this.GetSiegeAftermathConsequencesText(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.ShowMercy, true));
			textObject4.SetTextVariable("INFLUENCE_CONDITION_TEXT", flag2 ? textObject3 : TextObject.GetEmpty());
			args.IsEnabled = this.IsSiegeAftermathPossible(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.ShowMercy);
			args.Tooltip = textObject4;
			args.optionLeaveType = GameMenuOption.LeaveType.ShowMercy;
			return true;
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x0016AF84 File Offset: 0x00169184
		private void menu_settlement_taken_show_mercy_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty besiegerParty = this._besiegerParty;
			this._playerEncounterAftermath = SiegeAftermathAction.SiegeAftermath.ShowMercy;
			SiegeAftermathAction.ApplyAftermath(besiegerParty, currentSettlement, this._playerEncounterAftermath, this._prevSettlementOwnerClan, this._siegeEventPartyContributions);
			GameMenu.SwitchToMenu("siege_aftermath_contextual_summary");
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x0016AFC8 File Offset: 0x001691C8
		private TextObject GetSiegeAftermathConsequencesText(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermath, bool isTooltip)
		{
			TextObject textObject = new TextObject("{=!}{PROSPERITY_TEXT}{TOWN_PROJECTS_TEXT}{LOYALTY_TEXT}{NOTABLE_POWER_TEXT}{PARTY_MORALE_TEXT}{ARMY_GOLD_TEXT}{PARTY_GOLD_TEXT}{TRAIT_CHANGE}{OWNER_RELATION_TEXT}", null);
			TextObject textObject2 = new TextObject("{=ERh2DVEa} • Prosperity Lost: {PROSPERITY_LOST_AMOUNT}", null);
			textObject2.SetTextVariable("PROSPERITY_LOST_AMOUNT", -1 * (int)this.GetSiegeAftermathProsperityPenalty(attackerParty, settlement, aftermath));
			textObject.SetTextVariable("PROSPERITY_TEXT", textObject2);
			TextObject textObject3 = new TextObject("{=HtHcEv7N}{newline} • Party Morale Change : {MORALE_CHANGE}", null);
			textObject3.SetTextVariable("MORALE_CHANGE", this.GetSiegeAftermathPartyMoraleBonus(attackerParty, settlement, aftermath), 2);
			textObject.SetTextVariable("PARTY_MORALE_TEXT", textObject3);
			if (aftermath != SiegeAftermathAction.SiegeAftermath.ShowMercy)
			{
				TextObject textObject4 = TextObject.GetEmpty();
				if (isTooltip)
				{
					textObject4 = new TextObject("{=tF1G5YLe}{newline} • Building Levels Reduced: {LEVELS_LOST}", null);
					textObject4.SetTextVariable("LEVELS_LOST", this.GetSiegeAftermathProjectsLoss(attackerParty, aftermath));
					textObject.SetTextVariable("TOWN_PROJECTS_TEXT", textObject4);
				}
				else
				{
					textObject4 = new TextObject("{=WDRTZ8se}{newline} • Town Projects Razed: {LEVELS_LOST}{PROJECTS_DESTROYED}", null);
					TextObject textObject5 = new TextObject("{=W1KJEvit}{newline}    Levels Lost: {BUILDINGS_LOST_LEVEL}", null);
					TextObject textObject6 = new TextObject("{=n1bQHmCk}{newline}    Projects Destroyed: {BUILDINGS_DESTROYED}", null);
					TextObject textObject7 = new TextObject("{=HDNedIxl}{newline}        {BUILDING_NAME}: {LEVEL_LOST}", null);
					TextObject textObject8 = new TextObject("{=jZmBbA5M}{newline}        {BUILDING_NAME}", null);
					List<KeyValuePair<Building, int>> list = new List<KeyValuePair<Building, int>>(this._playerEncounterAftermathDamagedBuildings.Where<KeyValuePair<Building, int>>((KeyValuePair<Building, int> t) => t.Key.CurrentLevel > 0));
					List<Building> list2 = new List<Building>(from t in this._playerEncounterAftermathDamagedBuildings
						where t.Key.CurrentLevel <= 0
						select t.Key);
					string text = "";
					foreach (KeyValuePair<Building, int> keyValuePair in list)
					{
						TextObject textObject9 = textObject7.CopyTextObject();
						textObject9.SetTextVariable("BUILDING_NAME", keyValuePair.Key.Name);
						textObject9.SetTextVariable("LEVEL_LOST", keyValuePair.Value);
						text += textObject9.ToString();
					}
					string text2 = "";
					foreach (Building building in list2)
					{
						TextObject textObject10 = textObject8.CopyTextObject();
						textObject10.SetTextVariable("BUILDING_NAME", building.Name);
						text2 += textObject10.ToString();
					}
					textObject5.SetTextVariable("BUILDINGS_LOST_LEVEL", text);
					textObject6.SetTextVariable("BUILDINGS_DESTROYED", text2);
					textObject4.SetTextVariable("LEVELS_LOST", (!list.IsEmpty<KeyValuePair<Building, int>>()) ? textObject5.ToString() : "");
					textObject4.SetTextVariable("PROJECTS_DESTROYED", (!list2.IsEmpty<Building>()) ? textObject6.ToString() : "");
					textObject.SetTextVariable("TOWN_PROJECTS_TEXT", (!list2.IsEmpty<Building>() || !list.IsEmpty<KeyValuePair<Building, int>>()) ? textObject4.ToString() : "");
				}
				TextObject textObject11 = new TextObject("{=EVxxKXmW}{newline} • Loyalty in {SETTLEMENT} : {LOYALTY_LOST_AMOUNT}", null);
				textObject11.SetTextVariable("LOYALTY_LOST_AMOUNT", this.GetSiegeAftermathLoyaltyPenalty(aftermath), 2);
				textObject11.SetTextVariable("SETTLEMENT", settlement.GetName());
				textObject.SetTextVariable("LOYALTY_TEXT", textObject11);
				if (settlement.Notables.Count > 0)
				{
					TextObject textObject12 = new TextObject("{=38dcXWzq}{newline} • Notable Powers: {NOTABLE_POWER_LOST_AMOUNT}%", null);
					textObject12.SetTextVariable("NOTABLE_POWER_LOST_AMOUNT", this.GetSiegeAftermathNotablePowerModifierForAftermath(aftermath) * 100f, 2);
					textObject.SetTextVariable("NOTABLE_POWER_TEXT", textObject12);
				}
				TextObject textObject13 = new TextObject("{=RO3Zv0K4}{newline} • Relation with Settlement Owner : {OWNER.LINK} : {RELATION_CHANGE}", null);
				textObject13.SetTextVariable("RELATION_CHANGE", this.GetSiegeAftermathRelationPenaltyWithSettlementOwner(aftermath));
				StringHelpers.SetCharacterProperties("OWNER", this._prevSettlementOwnerClan.Leader.CharacterObject, textObject13, false);
				textObject.SetTextVariable("OWNER_RELATION_TEXT", textObject13);
				int siegeAftermathArmyGoldGain = this.GetSiegeAftermathArmyGoldGain(attackerParty, settlement, aftermath);
				if (attackerParty.Army != null)
				{
					TextObject textObject14 = new TextObject("{=2wwAyZdL}{newline} • Army Gold Gain : {ARMY_GOLD_GAIN}", null);
					textObject14.SetTextVariable("ARMY_GOLD_GAIN", siegeAftermathArmyGoldGain);
					textObject.SetTextVariable("ARMY_GOLD_TEXT", textObject14);
				}
				else
				{
					textObject.SetTextVariable("ARMY_GOLD_TEXT", "");
				}
				TextObject textObject15 = new TextObject("{=RmW8Wf83}{newline} • Party Gold Gain : {PARTY_GOLD_GAIN}", null);
				float num;
				textObject15.SetTextVariable("PARTY_GOLD_GAIN", this.GetSiegeAftermathPartyGoldGain((float)siegeAftermathArmyGoldGain, this._siegeEventPartyContributions.TryGetValue(attackerParty, out num) ? num : 0f));
				textObject.SetTextVariable("PARTY_GOLD_TEXT", textObject15);
			}
			else
			{
				textObject.SetTextVariable("TOWN_PROJECTS_TEXT", "");
				textObject.SetTextVariable("LOYALTY_TEXT", "");
				textObject.SetTextVariable("NOTABLE_POWER_TEXT", "");
				textObject.SetTextVariable("OWNER_RELATION_TEXT", "");
				textObject.SetTextVariable("ARMY_GOLD_TEXT", "");
				textObject.SetTextVariable("PARTY_GOLD_TEXT", "");
			}
			TextObject textObject16 = new TextObject("{=wuQo5VtM}{newline} • Increase reputation for being {TRAIT}", null);
			if (aftermath == SiegeAftermathAction.SiegeAftermath.ShowMercy)
			{
				textObject16.SetTextVariable("TRAIT", HeroHelper.GetPersonalityTraitChangeName(DefaultTraits.Mercy, Hero.MainHero, true));
			}
			else if (aftermath == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				textObject16.SetTextVariable("TRAIT", HeroHelper.GetPersonalityTraitChangeName(DefaultTraits.Mercy, Hero.MainHero, false));
			}
			else
			{
				textObject16 = TextObject.GetEmpty();
			}
			textObject.SetTextVariable("TRAIT_CHANGE", textObject16);
			return textObject;
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x0016B514 File Offset: 0x00169714
		private void siege_aftermath_contextual_summary_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty besiegerParty = this._besiegerParty;
			if (this._playerEncounterAftermath == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				TextObject textObject = new TextObject("{=VFVqjZwY}Your troops sweep through the {?IS_CITY}city{?}fortress{\\?}, taking whatever loot they like and setting fire to the rest.", null);
				textObject.SetTextVariable("IS_CITY", Settlement.CurrentSettlement.IsTown ? 1 : 0);
				MBTextManager.SetTextVariable("START_OF_EXPLANATION", textObject, false);
			}
			else if (this._playerEncounterAftermath == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				MBTextManager.SetTextVariable("START_OF_EXPLANATION", new TextObject("{=oJUxWEwp}You grant your men their customary right of pillage after a successful siege. You tell them they may take property but must spare the townsfolk's lives.", null), false);
			}
			else
			{
				CultureObject culture = currentSettlement.Culture;
				IFaction mapFaction = besiegerParty.MapFaction;
				if (culture != ((mapFaction != null) ? mapFaction.Culture : null))
				{
					TextObject textObject2 = new TextObject("{=x2dvXNQ0}You have decided to show mercy to the people of {SETTLEMENT_NAME}.{newline}You can hear disgruntled murmuring among the troops, who have been denied their customary right of pillage.", null);
					textObject2.SetTextVariable("SETTLEMENT_NAME", currentSettlement.Name);
					MBTextManager.SetTextVariable("START_OF_EXPLANATION", textObject2, false);
				}
				else
				{
					TextObject textObject3 = new TextObject("{=bXN2fbcv}Your men treat the residents of {SETTLEMENT_NAME} as wayward subjects of the {SETTLEMENT_CULTURE_NAME} rather than foes, and treat them relatively well.", null);
					textObject3.SetTextVariable("SETTLEMENT_NAME", currentSettlement.Name);
					textObject3.SetTextVariable("SETTLEMENT_CULTURE_NAME", FactionHelper.GetFormalNameForFactionCulture(currentSettlement.Culture));
					MBTextManager.SetTextVariable("START_OF_EXPLANATION", textObject3, false);
				}
			}
			MBTextManager.SetTextVariable("CONTEXTUAL_SUMMARY_TEXT", this.GetSiegeAftermathConsequencesText(besiegerParty, currentSettlement, this._playerEncounterAftermath, false), false);
			TextObject textObject4 = new TextObject("{=I0ZG4tIj}{TOWN_NAME} has fallen to your troops. You may station a garrison here to defend it against enemies who may try to recapture it.", null);
			MBTextManager.SetTextVariable("TOWN_NAME", Settlement.CurrentSettlement.Name, false);
			MBTextManager.SetTextVariable("END_OF_EXPLANATION", textObject4, false);
			args.MenuContext.SetBackgroundMeshName("encounter_win");
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x0016B678 File Offset: 0x00169878
		private float GetSiegeAftermathProsperityPenalty(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			TroopRoster memberRoster = attackerParty.MemberRoster;
			int num = ((memberRoster != null) ? memberRoster.TotalHealthyCount : 0);
			if (attackerParty.Army != null)
			{
				num = attackerParty.Army.TotalHealthyMembers;
			}
			float num2 = -1f * ((MathF.Log((float)num * 0.04f + 2f, 2f) * 2.5f + 2.5f) * 0.01f * ((this._settlementProsperityCache < 0f) ? settlement.Town.Prosperity : this._settlementProsperityCache));
			float num3 = num2;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				num3 = 1.5f * num2;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.ShowMercy)
			{
				num3 = 0.5f * num2;
			}
			return num3;
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x0016B71C File Offset: 0x0016991C
		private int GetSiegeAftermathProjectsLoss(MobileParty attackerParty, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			TroopRoster memberRoster = attackerParty.MemberRoster;
			int num = ((memberRoster != null) ? memberRoster.TotalHealthyCount : 0);
			if (attackerParty.Army != null)
			{
				num = attackerParty.Army.TotalHealthyMembers;
			}
			int num2 = MathF.Floor(MathF.Log((float)num * 0.02f + 2f, 2f));
			int num3 = 0;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				num3 = MathF.Ceiling(1.5f * (float)num2);
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				num3 = num2;
			}
			return num3;
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x0016B78C File Offset: 0x0016998C
		private float GetSiegeAftermathLoyaltyPenalty(SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			float num = 0f;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				num = -30f;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				num = -15f;
			}
			return num;
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x0016B7B8 File Offset: 0x001699B8
		private float GetSiegeAftermathNotablePowerModifierForAftermath(SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			float num = 0f;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				num = -0.5f;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				num = -0.25f;
			}
			return num;
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x0016B7E4 File Offset: 0x001699E4
		private float GetSiegeAftermathPartyMoraleBonus(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			int num = 0;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				num = 20;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				num = 10;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.ShowMercy)
			{
				IFaction mapFaction = attackerParty.MapFaction;
				if (((mapFaction != null) ? mapFaction.Culture : null) != settlement.Culture)
				{
					num = -15;
				}
			}
			return (float)num;
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x0016B828 File Offset: 0x00169A28
		private int GetSiegeAftermathArmyGoldGain(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			float num = -1f * this.GetSiegeAftermathProsperityPenalty(attackerParty, settlement, aftermathType);
			float num2 = 0f;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate || aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				num2 = num * 15f;
			}
			return MathF.Floor(num2);
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x0016B860 File Offset: 0x00169A60
		private int GetSiegeAftermathPartyGoldGain(float totalArmyGoldGain, float partyContributionPercentage)
		{
			return MathF.Floor(totalArmyGoldGain * partyContributionPercentage / 100f);
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x0016B870 File Offset: 0x00169A70
		private int GetSiegeAftermathRelationChangeWithLord(Hero hero, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			if (hero.GetTraitLevel(DefaultTraits.Mercy) > 0)
			{
				return this.GetSiegeAftermathRelationChangeWithMercifulLord(aftermathType);
			}
			if (hero.GetTraitLevel(DefaultTraits.Mercy) < 0)
			{
				return -1 * this.GetSiegeAftermathRelationChangeWithMercifulLord(aftermathType);
			}
			return 0;
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x0016B8A4 File Offset: 0x00169AA4
		private int GetSiegeAftermathRelationChangeWithMercifulLord(SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			int num = 0;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				num = -10;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.ShowMercy)
			{
				num = 10;
			}
			return num;
		}

		// Token: 0x060048BD RID: 18621 RVA: 0x0016B8C4 File Offset: 0x00169AC4
		private int GetSiegeAftermathRelationPenaltyWithSettlementOwner(SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			int num = 0;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				num = -30;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				num = -15;
			}
			return num;
		}

		// Token: 0x060048BE RID: 18622 RVA: 0x0016B8E4 File Offset: 0x00169AE4
		private float GetSiegeAftermathInfluenceCost(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			float num = 0f;
			if (attackerParty.Army != null && aftermathType != SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				int num2 = attackerParty.Army.Parties.Count<MobileParty>((MobileParty t) => t != attackerParty && t.LeaderHero != null && t.LeaderHero.GetTraitLevel(DefaultTraits.Mercy) > 0);
				int num3 = attackerParty.Army.Parties.Count<MobileParty>((MobileParty t) => t != attackerParty && t.LeaderHero != null && t.LeaderHero.GetTraitLevel(DefaultTraits.Mercy) < 0);
				if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
				{
					num = (3000f + settlement.Town.Prosperity) / 400f * (float)num2;
				}
				else if (aftermathType == SiegeAftermathAction.SiegeAftermath.ShowMercy)
				{
					IFaction mapFaction = attackerParty.MapFaction;
					if (((mapFaction != null) ? mapFaction.Culture : null) != settlement.Culture)
					{
						num = (3000f + settlement.Town.Prosperity) / 400f * (float)num3;
					}
				}
			}
			return num;
		}

		// Token: 0x060048BF RID: 18623 RVA: 0x0016B9C0 File Offset: 0x00169BC0
		private bool IsSiegeAftermathPossible(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			float siegeAftermathInfluenceCost = this.GetSiegeAftermathInfluenceCost(attackerParty, settlement, aftermathType);
			Clan actualClan = attackerParty.ActualClan;
			bool flag = actualClan != null && actualClan.Influence >= siegeAftermathInfluenceCost;
			CultureObject culture = settlement.Culture;
			IFaction mapFaction = Hero.MainHero.MapFaction;
			bool flag2 = culture == ((mapFaction != null) ? mapFaction.Culture : null);
			bool flag3;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				flag3 = flag && !flag2;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				flag3 = !flag2;
			}
			else
			{
				flag3 = flag || flag2;
			}
			return flag3;
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x0016BA34 File Offset: 0x00169C34
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement.IsFortification && detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege && capturerHero != null && settlement.OwnerClan != null && settlement.OwnerClan != Clan.PlayerClan && !oldOwner.IsDead)
			{
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(oldOwner, capturerHero, -10, capturerHero == Hero.MainHero);
				if (capturerHero.MapFaction.Leader != capturerHero && settlement.OwnerClan.Leader != capturerHero.MapFaction.Leader)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(oldOwner, capturerHero.MapFaction.Leader, -6, capturerHero.MapFaction.Leader == Hero.MainHero);
				}
			}
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x0016BAE0 File Offset: 0x00169CE0
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<MobileParty>("_besiegerParty", ref this._besiegerParty);
			dataStore.SyncData<Clan>("_prevSettlementOwnerClan", ref this._prevSettlementOwnerClan);
			dataStore.SyncData<SiegeAftermathAction.SiegeAftermath>("_playerEncounterAftermath", ref this._playerEncounterAftermath);
			dataStore.SyncData<Dictionary<MobileParty, float>>("_siegeEventPartyContributions", ref this._siegeEventPartyContributions);
			dataStore.SyncData<bool>("_wasPlayerArmyMember", ref this._wasPlayerArmyMember);
			dataStore.SyncData<Dictionary<Building, int>>("_playerEncounterAftermathDamagedBuildings", ref this._playerEncounterAftermathDamagedBuildings);
		}

		// Token: 0x04001484 RID: 5252
		private MobileParty _besiegerParty;

		// Token: 0x04001485 RID: 5253
		private Clan _prevSettlementOwnerClan;

		// Token: 0x04001486 RID: 5254
		private SiegeAftermathAction.SiegeAftermath _playerEncounterAftermath;

		// Token: 0x04001487 RID: 5255
		private Dictionary<MobileParty, float> _siegeEventPartyContributions = new Dictionary<MobileParty, float>();

		// Token: 0x04001488 RID: 5256
		private Dictionary<Building, int> _playerEncounterAftermathDamagedBuildings = new Dictionary<Building, int>();

		// Token: 0x04001489 RID: 5257
		private bool _wasPlayerArmyMember;

		// Token: 0x0400148A RID: 5258
		private float _settlementProsperityCache = -1f;
	}
}
