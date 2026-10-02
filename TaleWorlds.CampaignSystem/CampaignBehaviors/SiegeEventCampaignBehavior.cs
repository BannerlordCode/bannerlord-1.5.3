using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000466 RID: 1126
	public class SiegeEventCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x060048D0 RID: 18640 RVA: 0x0016BEC4 File Offset: 0x0016A0C4
		private TextObject _currentSiegeDescription
		{
			get
			{
				if (PlayerSiege.PlayerSiegeEvent == null)
				{
					return TextObject.GetEmpty();
				}
				TextObject textObject = ((PlayerSiege.PlayerSide == BattleSideEnum.Attacker) ? this._attackerSummaryText : this._defenderSummaryText);
				Settlement settlement = PlayerEncounter.EncounterSettlement ?? PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
				textObject.SetTextVariable("SETTLEMENT", settlement.Name);
				Hero leaderOfSiegeEvent = Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(PlayerSiege.PlayerSiegeEvent, PlayerSiege.PlayerSide);
				if (leaderOfSiegeEvent == Hero.MainHero)
				{
					TextObject textObject2 = ((PlayerSiege.PlayerSide == BattleSideEnum.Attacker) ? new TextObject("{=0DpoSNky}You are commanding the besiegers.", null) : new TextObject("{=W0FR7yy0}You are commanding the defenders.", null));
					textObject.SetTextVariable("FURTHER_EXPLANATION", textObject2);
				}
				else if (leaderOfSiegeEvent != null)
				{
					TextObject textObject3 = ((PlayerSiege.PlayerSide == BattleSideEnum.Attacker) ? new TextObject("{=d2spYiHG}{LEADER.NAME} is commanding the besiegers.", null) : new TextObject("{=Ja8dMYHi}{LEADER.NAME} is commanding the defenders.", null));
					StringHelpers.SetCharacterProperties("LEADER", leaderOfSiegeEvent.CharacterObject, textObject3, false);
					textObject.SetTextVariable("FURTHER_EXPLANATION", textObject3);
				}
				else
				{
					textObject.SetTextVariable("FURTHER_EXPLANATION", TextObject.GetEmpty());
				}
				return textObject;
			}
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x0016BFD0 File Offset: 0x0016A1D0
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
			CampaignEvents.SiegeEngineBuiltEvent.AddNonSerializedListener(this, new Action<SiegeEvent, BattleSideEnum, SiegeEngineType>(this.OnSiegeEngineBuilt));
			CampaignEvents.OnSiegeEngineDestroyedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType>(this.OnSiegeEngineDestroyed));
			CampaignEvents.OnSiegeBombardmentHitEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, SiegeBombardTargets>(this.OnSiegeEngineHit));
			CampaignEvents.OnSiegeBombardmentWallHitEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, bool>(this.OnSiegeBombardmentWallHit));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceDeclared));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x0016C098 File Offset: 0x0016A298
		private void OnPeaceDeclared(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			if (Campaign.Current.CurrentMenuContext != null && Game.Current.GameStateManager.ActiveState != null && Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu() == "menu_siege_strategies")
			{
				Campaign.Current.CurrentMenuContext.Refresh();
			}
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x0016C0F1 File Offset: 0x0016A2F1
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (settlement.SiegeEvent != null && party == MobileParty.MainParty)
			{
				this.SetDefaultTactics(settlement.SiegeEvent, BattleSideEnum.Defender);
			}
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x0016C110 File Offset: 0x0016A310
		private void OnSiegeBombardmentWallHit(MobileParty party, Settlement settlement, BattleSideEnum battleSide, SiegeEngineType siegeEngine, bool isWallCracked)
		{
			if (isWallCracked && party != null)
			{
				SkillLevelingManager.OnWallBreached(party);
			}
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x0016C11F File Offset: 0x0016A31F
		private void OnSiegeEngineHit(MobileParty party, Settlement settlement, BattleSideEnum side, SiegeEngineType engine, SiegeBombardTargets target)
		{
			if (target == SiegeBombardTargets.RangedEngines)
			{
				this.BombardHitEngineCasualties(settlement.SiegeEvent.GetSiegeEventSide(side), engine);
			}
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x0016C13C File Offset: 0x0016A33C
		private void OnSiegeEngineDestroyed(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum lostSide, SiegeEngineType siegeEngine)
		{
			SiegeEventModel siegeEventModel = Campaign.Current.Models.SiegeEventModel;
			SiegeEvent siegeEvent = besiegedSettlement.SiegeEvent;
			MobileParty effectiveSiegePartyForSide = siegeEventModel.GetEffectiveSiegePartyForSide(siegeEvent, lostSide);
			MobileParty effectiveSiegePartyForSide2 = siegeEventModel.GetEffectiveSiegePartyForSide(siegeEvent, lostSide.GetOppositeSide());
			if (effectiveSiegePartyForSide2 != null)
			{
				SkillLevelingManager.OnSiegeEngineDestroyed(effectiveSiegePartyForSide2, siegeEngine);
			}
			float casualtyChance = Campaign.Current.Models.SiegeEventModel.GetCasualtyChance(effectiveSiegePartyForSide, siegeEvent, lostSide);
			if (MBRandom.RandomFloat <= casualtyChance)
			{
				ISiegeEventSide siegeEventSide = siegeEvent.GetSiegeEventSide(lostSide);
				int num = siegeEventModel.GetSiegeEngineDestructionCasualties(siegeEvent, siegeEventSide.BattleSide, siegeEngine);
				BattleSideEnum oppositeSide = siegeEventSide.BattleSide.GetOppositeSide();
				if (effectiveSiegePartyForSide2 != null)
				{
					Hero hero = null;
					if (oppositeSide == BattleSideEnum.Attacker && effectiveSiegePartyForSide2.HasPerk(DefaultPerks.Tactics.PickThemOfTheWalls, out hero, false) && MBRandom.RandomFloat < DefaultPerks.Tactics.PickThemOfTheWalls.PrimaryBonus)
					{
						num *= 2;
					}
				}
				if (oppositeSide == BattleSideEnum.Defender)
				{
					Hero governor = siegeEvent.BesiegedSettlement.Town.Governor;
					if (governor != null && governor.GetPerkValue(DefaultPerks.Tactics.PickThemOfTheWalls) && MBRandom.RandomFloat < DefaultPerks.Tactics.PickThemOfTheWalls.SecondaryBonus)
					{
						num *= 2;
					}
				}
				num = this.ApplyValorCasualtyReduction(num, effectiveSiegePartyForSide);
				this.KillRandomTroopsOfEnemy(siegeEventSide, num);
			}
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x0016C258 File Offset: 0x0016A458
		private void OnSiegeEngineBuilt(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngineType)
		{
			MobileParty effectiveSiegePartyForSide = Campaign.Current.Models.SiegeEventModel.GetEffectiveSiegePartyForSide(siegeEvent, side);
			if (effectiveSiegePartyForSide != null)
			{
				SkillLevelingManager.OnSiegeEngineBuilt(effectiveSiegePartyForSide, siegeEngineType);
				Hero hero = null;
				if (effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Apprenticeship, out hero, false))
				{
					for (int i = 0; i < effectiveSiegePartyForSide.MemberRoster.Count; i++)
					{
						CharacterObject characterAtIndex = effectiveSiegePartyForSide.MemberRoster.GetCharacterAtIndex(i);
						if (!characterAtIndex.IsHero)
						{
							int num = effectiveSiegePartyForSide.MemberRoster.GetElementNumber(i) * (int)DefaultPerks.Engineering.Apprenticeship.PrimaryBonus;
							effectiveSiegePartyForSide.MemberRoster.AddXpToTroop(characterAtIndex, num);
						}
					}
				}
			}
		}

		// Token: 0x060048D8 RID: 18648 RVA: 0x0016C2EC File Offset: 0x0016A4EC
		private int KillRandomTroopsOfEnemy(ISiegeEventSide siegeEventSide, int count)
		{
			IEnumerable<PartyBase> involvedPartiesForEventType = siegeEventSide.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege);
			int num = involvedPartiesForEventType.Sum<PartyBase>((PartyBase p) => p.NumberOfRegularMembers);
			if (num == 0 || count == 0)
			{
				return 0;
			}
			int num2 = 0;
			int num3 = MBRandom.RandomInt(involvedPartiesForEventType.Count<PartyBase>() - 1);
			for (int i = 0; i < involvedPartiesForEventType.Count<PartyBase>(); i++)
			{
				PartyBase partyBase = involvedPartiesForEventType.ElementAt<PartyBase>((i + num3) % involvedPartiesForEventType.Count<PartyBase>());
				float siegeBombardmentHitSurgeryChance = Campaign.Current.Models.PartyHealingModel.GetSiegeBombardmentHitSurgeryChance(partyBase);
				float num4 = (float)partyBase.NumberOfRegularMembers / (float)num;
				float randomFloat = MBRandom.RandomFloat;
				int num5 = MathF.Min(MBRandom.RoundRandomized((float)(count - num2) * (num4 + randomFloat)), count);
				int num6 = partyBase.MemberRoster.TotalRegulars - partyBase.MemberRoster.TotalWoundedRegulars;
				if (num5 > num6)
				{
					num5 = num6;
				}
				if (num5 > 0)
				{
					int num7 = MathF.Round((float)num5 * siegeBombardmentHitSurgeryChance);
					num2 += num5;
					num5 -= num7;
					siegeEventSide.OnTroopsKilledOnSide(num5);
					partyBase.MemberRoster.RemoveNumberOfNonHeroTroopsRandomly(num5);
					if (num7 > 0)
					{
						partyBase.MemberRoster.WoundNumberOfNonHeroTroopsRandomly(num7);
					}
				}
				if (num2 >= count)
				{
					break;
				}
			}
			return num2;
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x0016C424 File Offset: 0x0016A624
		private int ApplyValorCasualtyReduction(int casualties, MobileParty leaderParty)
		{
			Hero hero;
			if (leaderParty == null)
			{
				hero = null;
			}
			else
			{
				Army army = leaderParty.Army;
				if (army == null)
				{
					hero = null;
				}
				else
				{
					MobileParty leaderParty2 = army.LeaderParty;
					hero = ((leaderParty2 != null) ? leaderParty2.LeaderHero : null);
				}
			}
			Hero hero2 = hero ?? ((leaderParty != null) ? leaderParty.LeaderHero : null);
			if (hero2 != null)
			{
				float traitEffectBonus = TraitEffectHelper.GetTraitEffectBonus(hero2, DefaultPersonalityTraitEffects.ValorSiegeCasualtyEffect);
				if (traitEffectBonus != 0f)
				{
					casualties = MBRandom.RoundRandomized((float)casualties * (1f + traitEffectBonus));
				}
			}
			return casualties;
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x0016C490 File Offset: 0x0016A690
		private void BombardHitEngineCasualties(ISiegeEventSide siegeEventSide, SiegeEngineType attackerEngineType)
		{
			SiegeEvent siegeEvent = siegeEventSide.SiegeEvent;
			Settlement besiegedSettlement = siegeEvent.BesiegedSettlement;
			BesiegerCamp besiegerCamp = siegeEvent.BesiegerCamp;
			MobileParty effectiveSiegePartyForSide = Campaign.Current.Models.SiegeEventModel.GetEffectiveSiegePartyForSide(siegeEvent, siegeEventSide.BattleSide);
			ISiegeEventSide siegeEventSide2 = siegeEvent.GetSiegeEventSide(siegeEventSide.BattleSide.GetOppositeSide());
			float siegeEngineHitChance = Campaign.Current.Models.SiegeEventModel.GetSiegeEngineHitChance(attackerEngineType, siegeEventSide.BattleSide, SiegeBombardTargets.People, besiegedSettlement.Town);
			if (MBRandom.RandomFloat < siegeEngineHitChance)
			{
				int num = Campaign.Current.Models.SiegeEventModel.GetColleteralDamageCasualties(attackerEngineType, effectiveSiegePartyForSide);
				MobileParty effectiveSiegePartyForSide2 = Campaign.Current.Models.SiegeEventModel.GetEffectiveSiegePartyForSide(siegeEvent, siegeEventSide2.BattleSide);
				num = this.ApplyValorCasualtyReduction(num, effectiveSiegePartyForSide2);
				if (this.KillRandomTroopsOfEnemy(siegeEventSide2, num) > 0)
				{
					CampaignEventDispatcher.Instance.OnSiegeBombardmentHit(besiegerCamp.LeaderParty, besiegedSettlement, siegeEventSide.BattleSide, attackerEngineType, SiegeBombardTargets.People);
				}
			}
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x0016C576 File Offset: 0x0016A776
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x0016C578 File Offset: 0x0016A778
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x0016C581 File Offset: 0x0016A781
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			this.SetDefaultTactics(siegeEvent, BattleSideEnum.Attacker);
			this.SetDefaultTactics(siegeEvent, BattleSideEnum.Defender);
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x0016C594 File Offset: 0x0016A794
		protected void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddWaitGameMenu("menu_siege_strategies", "{=!}{CURRENT_STRATEGY}", new OnInitDelegate(this.game_menu_siege_strategies_on_init), null, null, new OnTickDelegate(this.game_menu_siege_strategies_on_tick), GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.Encounter, 0f, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_lead_assault", "{=mjOcwUSA}Lead an assault", new GameMenuOption.OnConditionDelegate(SiegeEventCampaignBehavior.game_menu_siege_strategies_lead_assault_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_siege_strategies_lead_assault_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_order_troops", "{=TtGJqRI5}Send troops", new GameMenuOption.OnConditionDelegate(SiegeEventCampaignBehavior.game_menu_siege_strategies_order_assault_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_order_an_assault_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_request_parley", "{=2xVbLS5r}Request a parley", new GameMenuOption.OnConditionDelegate(SiegeEventCampaignBehavior.menu_defender_side_request_audience_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_defender_side_request_audience_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.menu_siege_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_siege_leave_on_consequence), true, 10, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_leave_army", "{=hSdJ0UUv}Leave Army", new GameMenuOption.OnConditionDelegate(this.menu_siege_strategies_passive_wait_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_siege_strategies_passive_wait_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("menu_siege_strategies_break_siege", "{=!}{SIEGE_LEAVE_TEXT}", new OnInitDelegate(this.menu_break_siege_on_init), GameMenu.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies_break_siege", "menu_siege_strategies_break_siege_return", "{=25ifdWOy}Return to siege", new GameMenuOption.OnConditionDelegate(this.return_siege_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("menu_siege_strategies");
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies_break_siege", "menu_siege_strategies_break_siege_go_on", "{=TGYJUUn0}Go on.", new GameMenuOption.OnConditionDelegate(this.leave_siege_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_end_besieging_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("menu_siege_safe_passage_accepted", "Besiegers have agreed to allow safe passage for your party.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_safe_passage_accepted", "menu_siege_safe_passage_accepted_leave", "Leave", new GameMenuOption.OnConditionDelegate(this.leave_siege_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_siege_leave_on_consequence), true, -1, false, null);
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x0016C7A4 File Offset: 0x0016A9A4
		private void game_menu_siege_strategies_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (!(genericStateMenu != "menu_siege_strategies"))
			{
				args.MenuContext.GameMenu.GetText().SetTextVariable("CURRENT_STRATEGY", this._currentSiegeDescription);
				Campaign.Current.GameMenuManager.RefreshMenuOptionConditions(args.MenuContext);
				return;
			}
			if (!string.IsNullOrEmpty(genericStateMenu))
			{
				GameMenu.SwitchToMenu(genericStateMenu);
				return;
			}
			GameMenu.ExitToLast();
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x0016C81E File Offset: 0x0016AA1E
		private void game_menu_siege_strategies_on_init(MenuCallbackArgs args)
		{
			MBTextManager.SetTextVariable("CURRENT_STRATEGY", this._currentSiegeDescription, false);
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x0016C831 File Offset: 0x0016AA31
		private static void menu_siege_strategies_lead_assault_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.IsActive)
			{
				PlayerEncounter.LeaveEncounter = false;
			}
			else
			{
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty, PlayerSiege.PlayerSiegeEvent.BesiegedSettlement);
			}
			GameMenu.SwitchToMenu("assault_town");
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x0016C860 File Offset: 0x0016AA60
		private static void menu_order_an_assault_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.IsActive)
			{
				PlayerEncounter.LeaveEncounter = false;
			}
			else
			{
				PlayerEncounter.Start();
				PlayerEncounter.Current.SetupFields(PartyBase.MainParty, PlayerSiege.PlayerSiegeEvent.BesiegedSettlement.Party);
			}
			GameMenu.SwitchToMenu("assault_town_order_attack");
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x0016C8A0 File Offset: 0x0016AAA0
		private bool menu_siege_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty) && ((PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Defender && !MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerSiege.PlayerSiegeEvent.BesiegerCamp.MapFaction)) || (PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Attacker));
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x0016C91C File Offset: 0x0016AB1C
		private bool menu_siege_strategies_passive_wait_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty;
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x0016C96C File Offset: 0x0016AB6C
		private void menu_break_siege_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.SiegeEvent.BesiegerCamp.LeaderParty == MobileParty.MainParty)
			{
				MBTextManager.SetTextVariable("SIEGE_LEAVE_TEXT", this._removeSiegeCompletelyText, false);
			}
			else
			{
				MBTextManager.SetTextVariable("SIEGE_LEAVE_TEXT", this._leaveSiegeText, false);
			}
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x0016C9C3 File Offset: 0x0016ABC3
		private bool return_siege_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x0016C9CE File Offset: 0x0016ABCE
		private bool leave_siege_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x0016C9D9 File Offset: 0x0016ABD9
		private void menu_siege_strategies_passive_wait_leave_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				PlayerSiege.FinalizePlayerSiege();
			}
			MobileParty.MainParty.BesiegerCamp = null;
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x0016CA04 File Offset: 0x0016AC04
		private static bool game_menu_siege_strategies_order_assault_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.OrderTroopsToAttack;
			if (MobileParty.MainParty.BesiegedSettlement == null)
			{
				return false;
			}
			if (Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(MobileParty.MainParty.BesiegedSettlement.SiegeEvent, PlayerSiege.PlayerSide) == Hero.MainHero)
			{
				PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
				Settlement settlement = ((encounteredParty != null) ? encounteredParty.Settlement : null) ?? PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
				if (PlayerSiege.PlayerSide == BattleSideEnum.Attacker && !settlement.SiegeEvent.BesiegerCamp.IsPreparationComplete)
				{
					args.IsEnabled = false;
					args.Tooltip = SiegeEventCampaignBehavior._waitSiegeEquipmentsText;
				}
				else
				{
					bool flag = MobileParty.MainParty.MemberRoster.GetTroopRoster().Any<TroopRosterElement>(delegate(TroopRosterElement x)
					{
						if (x.Character.IsHero)
						{
							return x.Character != CharacterObject.PlayerCharacter && !x.Character.HeroObject.IsWounded;
						}
						return x.Number > x.WoundedNumber;
					});
					if (!flag && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
					{
						foreach (MobileParty mobileParty in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
						{
							flag = mobileParty.MemberRoster.GetTroopRoster().Any<TroopRosterElement>(delegate(TroopRosterElement x)
							{
								if (x.Character.IsHero)
								{
									return x.Character != CharacterObject.PlayerCharacter && !x.Character.HeroObject.IsWounded;
								}
								return x.Number > x.WoundedNumber;
							});
							if (flag)
							{
								break;
							}
						}
					}
					args.Tooltip = TooltipHelper.GetSendTroopsPowerContextTooltipForSiege();
					if (!flag)
					{
						args.IsEnabled = false;
						args.Tooltip = new TextObject("{=ao9bhAhf}You are not leading any troops", null);
					}
					else if (!MobilePartyHelper.CanPartyAttackWithCurrentMorale(MobileParty.MainParty))
					{
						args.Tooltip = new TextObject("{=xnRtINwH}Your men lack the courage to continue the battle without you. (Low Morale)", null);
						args.IsEnabled = false;
					}
				}
			}
			else
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=1Hd19nq5}You are not in command of this siege.", null);
			}
			return true;
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x0016CBF0 File Offset: 0x0016ADF0
		private static bool game_menu_siege_strategies_lead_assault_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.LeadAssault;
			if (MobileParty.MainParty.BesiegedSettlement == null)
			{
				return false;
			}
			if (Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(MobileParty.MainParty.BesiegedSettlement.SiegeEvent, PlayerSiege.PlayerSide) == Hero.MainHero)
			{
				PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
				Settlement settlement = ((encounteredParty != null) ? encounteredParty.Settlement : null) ?? PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
				if (PlayerSiege.PlayerSide == BattleSideEnum.Attacker && !settlement.SiegeEvent.BesiegerCamp.IsPreparationComplete)
				{
					args.IsEnabled = false;
					args.Tooltip = SiegeEventCampaignBehavior._waitSiegeEquipmentsText;
				}
				else if (Hero.MainHero.IsWounded)
				{
					args.IsEnabled = false;
					args.Tooltip = SiegeEventCampaignBehavior._woundedAssaultText;
				}
			}
			else
			{
				args.IsEnabled = false;
				args.Tooltip = SiegeEventCampaignBehavior._noCommandText;
			}
			return true;
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x0016CCC3 File Offset: 0x0016AEC3
		private static void LeaveSiege()
		{
			MobileParty.MainParty.BesiegerCamp = null;
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			GameMenu.ExitToLast();
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x0016CCE4 File Offset: 0x0016AEE4
		private static void menu_siege_leave_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.BesiegerCamp == null)
			{
				if (PlayerEncounter.Current != null && MobileParty.MainParty.CurrentSettlement != null)
				{
					if (MobileParty.MainParty.Army != null)
					{
						MobileParty.MainParty.Army = null;
					}
					PlayerSiege.FinalizePlayerSiege();
					PlayerEncounter.LeaveSettlement();
					PlayerEncounter.Finish(true);
					return;
				}
				GameMenu.ExitToLast();
				return;
			}
			else
			{
				if (MobileParty.MainParty.BesiegerCamp.LeaderParty == MobileParty.MainParty)
				{
					GameMenu.SwitchToMenu("menu_siege_strategies_break_siege");
					return;
				}
				SiegeEventCampaignBehavior.LeaveSiege();
				return;
			}
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x0016CD64 File Offset: 0x0016AF64
		private static void menu_end_besieging_on_consequence(MenuCallbackArgs args)
		{
			SiegeEventCampaignBehavior.LeaveSiege();
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x0016CD6C File Offset: 0x0016AF6C
		private static bool menu_defender_side_request_audience_on_condition(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSide != BattleSideEnum.Defender)
			{
				return false;
			}
			if (PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Defender && !MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerSiege.PlayerSiegeEvent.BesiegerCamp.MapFaction))
			{
				return false;
			}
			Settlement settlement = Settlement.CurrentSettlement ?? PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
			if (settlement.SiegeEvent != null)
			{
				if (!settlement.SiegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege).Any<PartyBase>((PartyBase party) => party.LeaderHero != null && party.MobileParty.IsLordParty))
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=rO704KOG}There is no one with the authority to talk to you.", null);
				}
			}
			if (PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent != null)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=1UO0yMBr}You can not parley during an ongoing battle.", null);
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			return true;
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x0016CE5B File Offset: 0x0016B05B
		private static void menu_defender_side_request_audience_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("request_meeting_with_besiegers");
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x0016CE67 File Offset: 0x0016B067
		private void SetTactic(SiegeEvent siegeEvent, BattleSideEnum side, SiegeStrategy strategy)
		{
			siegeEvent.GetSiegeEventSide(side).SetSiegeStrategy(strategy);
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x0016CE78 File Offset: 0x0016B078
		private void SetDefaultTactics(SiegeEvent siegeEvent, BattleSideEnum side)
		{
			Hero leaderOfSiegeEvent = Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(siegeEvent, side);
			SiegeStrategy siegeStrategy = null;
			if (leaderOfSiegeEvent == Hero.MainHero)
			{
				siegeStrategy = DefaultSiegeStrategies.Custom;
			}
			else
			{
				IEnumerable<SiegeStrategy> enumerable = ((side == BattleSideEnum.Attacker) ? DefaultSiegeStrategies.AllAttackerStrategies : DefaultSiegeStrategies.AllDefenderStrategies);
				float num = float.MinValue;
				foreach (SiegeStrategy siegeStrategy2 in enumerable)
				{
					float num2 = Campaign.Current.Models.SiegeEventModel.GetSiegeStrategyScore(siegeEvent, side, siegeStrategy2) * (0.5f + 0.5f * MBRandom.RandomFloat);
					if (num2 > num)
					{
						num = num2;
						siegeStrategy = siegeStrategy2;
					}
				}
			}
			this.SetTactic(siegeEvent, side, siegeStrategy);
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x0016CF34 File Offset: 0x0016B134
		[GameMenuInitializationHandler("menu_siege_strategies")]
		private static void game_menu_siege_strategies_background_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("wait_besieging");
		}

		// Token: 0x0400148D RID: 5261
		private readonly TextObject _attackerSummaryText = new TextObject("{=sbmWGPYG}You are besieging {SETTLEMENT}. {FURTHER_EXPLANATION}", null);

		// Token: 0x0400148E RID: 5262
		private readonly TextObject _defenderSummaryText = new TextObject("{=l5YipTe3}{SETTLEMENT} is under siege. {FURTHER_EXPLANATION}", null);

		// Token: 0x0400148F RID: 5263
		private readonly TextObject _removeSiegeCompletelyText = new TextObject("{=5ZDCnrDQ}This will end the siege. You cannot take your siege engines with you, and they will be destroyed.", null);

		// Token: 0x04001490 RID: 5264
		private readonly TextObject _leaveSiegeText = new TextObject("{=176K8dcb}You will end the siege if you leave. Are you sure?", null);

		// Token: 0x04001491 RID: 5265
		private static readonly TextObject _waitSiegeEquipmentsText = new TextObject("{=bCuxzp1N}You need to wait for the siege equipment to be prepared.", null);

		// Token: 0x04001492 RID: 5266
		private static readonly TextObject _woundedAssaultText = new TextObject("{=gzYuWR28}You are wounded, and in no condition to lead an assault.", null);

		// Token: 0x04001493 RID: 5267
		private static readonly TextObject _noCommandText = new TextObject("{=1Hd19nq5}You are not in command of this siege.", null);
	}
}
