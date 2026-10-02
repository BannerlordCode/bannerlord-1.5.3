using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000451 RID: 1105
	public class PlayerArmyWaitBehavior : CampaignBehaviorBase
	{
		// Token: 0x060046EE RID: 18158 RVA: 0x0015A564 File Offset: 0x00158764
		public PlayerArmyWaitBehavior()
		{
			this._leadingArmyDescriptionText = GameTexts.FindText("str_you_are_leading_army", null);
			this._armyDescriptionText = GameTexts.FindText("str_army_of_HERO", null);
			this._disbandingArmyDescriptionText = new TextObject("{=Yan3ZG1w}Disbanding Army!", null);
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x0015A5A0 File Offset: 0x001587A0
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.TickEvent.AddNonSerializedListener(this, new Action<float>(PlayerArmyWaitBehavior.OnTick));
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x0015A5F2 File Offset: 0x001587F2
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Army.ArmyDispersionReason>("_playerArmyDispersionReason", ref this._playerArmyDispersionReason);
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x0015A606 File Offset: 0x00158806
		private void OnSessionLaunched(CampaignGameStarter starter)
		{
			this.AddMenus(starter);
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x0015A610 File Offset: 0x00158810
		private void AddMenus(CampaignGameStarter starter)
		{
			starter.AddWaitGameMenu("army_wait", "{=0gwQGnm4}{ARMY_OWNER_TEXT} {ARMY_BEHAVIOR}", new OnInitDelegate(this.wait_menu_army_wait_on_init), new OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_wait_on_condition), null, new OnTickDelegate(this.ArmyWaitMenuTick), GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
			starter.AddGameMenuOption("army_wait", "leave_army", "{=hSdJ0UUv}Leave Army", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.wait_menu_army_leave_on_consequence), true, -1, false, null);
			starter.AddGameMenuOption("army_wait", "abandon_army", "{=0vnegjxf}Abandon Army", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_abandon_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.wait_menu_army_abandon_on_consequence), true, -1, false, null);
			starter.AddWaitGameMenu("army_wait_at_settlement", "{=0gwQGnm4}{ARMY_OWNER_TEXT} {ARMY_BEHAVIOR}", new OnInitDelegate(this.wait_menu_army_wait_at_settlement_on_init), new OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_wait_on_condition), null, new OnTickDelegate(PlayerArmyWaitBehavior.wait_menu_army_wait_at_settlement_on_tick), GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
			starter.AddGameMenuOption("army_wait_at_settlement", "enter_settlement", "{=!}{ENTER_SETTLEMENT}", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_enter_settlement_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.wait_menu_army_enter_settlement_on_consequence), false, -1, false, null);
			starter.AddGameMenuOption("army_wait_at_settlement", "leave_army", "{=hSdJ0UUv}Leave Army", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.wait_menu_army_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.wait_menu_army_leave_on_consequence), true, -1, false, null);
			starter.AddGameMenu("army_dispersed", "{=!}{ARMY_DISPERSE_REASON}", new OnInitDelegate(this.army_dispersed_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			starter.AddGameMenuOption("army_dispersed", "army_dispersed_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.army_dispersed_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.army_dispersed_continue_on_consequence), true, -1, false, null);
			starter.AddGameMenu("menu_player_kicked_out_from_army_navigation_incapability", "{=ayktBG98}Your party does not have seaworthy ships. Army leader kicked you out from the army.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			starter.AddGameMenuOption("menu_player_kicked_out_from_army_navigation_incapability", "menu_player_kicked_out_from_army_navigation_incapability_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(PlayerArmyWaitBehavior.army_dispersed_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerArmyWaitBehavior.player_kicked_out_from_army_consequence), false, -1, false, null);
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x0015A7F2 File Offset: 0x001589F2
		private void army_dispersed_menu_on_init(MenuCallbackArgs args)
		{
			MBTextManager.SetTextVariable("ARMY_DISPERSE_REASON", PlayerArmyWaitBehavior.GetArmyDispersionReason(this._playerArmyDispersionReason), false);
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x0015A80A File Offset: 0x00158A0A
		private static void player_kicked_out_from_army_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.Army = null;
			PlayerArmyWaitBehavior.army_dispersed_continue_on_consequence(args);
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x0015A820 File Offset: 0x00158A20
		private void ArmyWaitMenuTick(MenuCallbackArgs args, CampaignTime dt)
		{
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (genericStateMenu != "army_wait")
			{
				args.MenuContext.GameMenu.EndWait();
				if (!string.IsNullOrEmpty(genericStateMenu))
				{
					GameMenu.SwitchToMenu(genericStateMenu);
				}
				else
				{
					GameMenu.ExitToLast();
				}
			}
			else
			{
				this.RefreshArmyTexts(args);
			}
			if (MobileParty.MainParty.Army.LeaderParty.IsCurrentlyAtSea)
			{
				args.MenuContext.SetBackgroundMeshName("encounter_naval");
				return;
			}
			args.MenuContext.SetBackgroundMeshName(Hero.MainHero.MapFaction.Culture.EncounterBackgroundMesh);
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x0015A8C4 File Offset: 0x00158AC4
		private static TextObject GetArmyDispersionReason(Army.ArmyDispersionReason reason)
		{
			Army army = MobileParty.MainParty.Army;
			bool flag = army == null || army.LeaderParty == MobileParty.MainParty;
			bool flag2 = true;
			TextObject textObject;
			if (reason == Army.ArmyDispersionReason.NoActiveWar)
			{
				if (flag)
				{
					textObject = new TextObject("{=hrhDNRa0}Your army has disbanded. The kingdom is now at peace.", null);
				}
				else
				{
					textObject = new TextObject("{=tvAdOGzc}{ARMY_LEADER}'s army has disbanded. The kingdom is now at peace.", null);
				}
			}
			else if (reason == Army.ArmyDispersionReason.CohesionDepleted)
			{
				if (flag)
				{
					textObject = new TextObject("{=rJBgDaxe}Your army has disbanded due to lack of cohesion.", null);
				}
				else
				{
					textObject = new TextObject("{=5wwO7ozf}{ARMY_LEADER}'s army has disbanded due to a lack of cohesion.", null);
				}
			}
			else if (reason == Army.ArmyDispersionReason.FoodProblem)
			{
				if (flag)
				{
					textObject = new TextObject("{=jlU2MmaO}Your army has disbanded due to a lack of food.", null);
				}
				else
				{
					textObject = new TextObject("{=eVdUaG3x}{ARMY_LEADER}'s army has disbanded due to a lack of food.", null);
				}
			}
			else if (reason == Army.ArmyDispersionReason.NoShipToUse)
			{
				if (flag)
				{
					textObject = new TextObject("{=9ryGDgOX}Your fleet has disbanded, as you no longer have a flagship with which to lead it. ", null);
				}
				else
				{
					textObject = new TextObject("{=!}{ARMY_LEADER}'s fleet has disbanded, as {she/he} no longer has a flagship with which to lead it.", null);
				}
			}
			else
			{
				textObject = new TextObject("{=FXPvGTEa}Army you are in is dispersed.", null);
				flag2 = false;
			}
			if (!flag && flag2)
			{
				textObject.SetTextVariable("ARMY_LEADER", army.LeaderParty.LeaderHero.Name);
			}
			return textObject;
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x0015A9B8 File Offset: 0x00158BB8
		private void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
			if (isPlayersArmy)
			{
				Debug.Print(string.Format("Player army is dispersed due to:  {0}", reason), 0, Debug.DebugColor.White, 17592186044416UL);
				this._playerArmyDispersionReason = reason;
				if (Campaign.Current.CurrentMenuContext != null)
				{
					Campaign.Current.CurrentMenuContext.GameMenu.EndWait();
					GameMenu.SwitchToMenu("army_dispersed");
					return;
				}
				GameMenu.ActivateGameMenu("army_dispersed");
			}
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x0015AA28 File Offset: 0x00158C28
		private void wait_menu_army_wait_on_init(MenuCallbackArgs args)
		{
			Army army = MobileParty.MainParty.Army;
			bool flag;
			if (army == null)
			{
				flag = null != null;
			}
			else
			{
				MobileParty leaderParty = army.LeaderParty;
				flag = ((leaderParty != null) ? leaderParty.LeaderHero : null) != null;
			}
			if (flag)
			{
				this._armyDescriptionText.SetTextVariable("HERO", army.LeaderParty.LeaderHero.Name);
				args.MenuTitle = this._armyDescriptionText;
			}
			else
			{
				args.MenuTitle = this._disbandingArmyDescriptionText;
			}
			this.RefreshArmyTexts(args);
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x0015AA9C File Offset: 0x00158C9C
		private void wait_menu_army_wait_at_settlement_on_init(MenuCallbackArgs args)
		{
			if (!PlayerEncounter.InsideSettlement && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				PlayerEncounter.EnterSettlement();
			}
			this._armyDescriptionText.SetTextVariable("HERO", MobileParty.MainParty.Army.LeaderParty.LeaderHero.Name);
			args.MenuTitle = this._armyDescriptionText;
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.IsPlayerWaiting = true;
			}
			this.RefreshArmyTexts(args);
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x0015AB1C File Offset: 0x00158D1C
		private static void wait_menu_army_wait_at_settlement_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (genericStateMenu != "army_wait_at_settlement")
			{
				args.MenuContext.GameMenu.EndWait();
				if (!string.IsNullOrEmpty(genericStateMenu))
				{
					GameMenu.SwitchToMenu(genericStateMenu);
					return;
				}
				GameMenu.ExitToLast();
			}
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x0015AB70 File Offset: 0x00158D70
		private void RefreshArmyTexts(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army != null)
			{
				TextObject text = args.MenuContext.GameMenu.GetText();
				if (MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
				{
					TextObject textObject = GameTexts.FindText("str_you_are_following_army", null);
					textObject.SetTextVariable("ARMY_LEADER", MobileParty.MainParty.Army.LeaderParty.LeaderHero.Name);
					text.SetTextVariable("ARMY_OWNER_TEXT", textObject);
					text.SetTextVariable("ARMY_BEHAVIOR", MobileParty.MainParty.Army.GetLongTermBehaviorText(false));
					return;
				}
				text.SetTextVariable("ARMY_OWNER_TEXT", this._leadingArmyDescriptionText);
				text.SetTextVariable("ARMY_BEHAVIOR", "");
			}
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x0015AC31 File Offset: 0x00158E31
		private static bool wait_menu_army_wait_on_condition(MenuCallbackArgs args)
		{
			return true;
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x0015AC34 File Offset: 0x00158E34
		private static bool wait_menu_army_abandon_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			if (MobileParty.MainParty.Army == null || (MobileParty.MainParty.MapEvent == null && MobileParty.MainParty.BesiegedSettlement == null))
			{
				return false;
			}
			args.Tooltip = GameTexts.FindText("str_abandon_army", null);
			args.Tooltip.SetTextVariable("INFLUENCE_COST", Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAbandoningArmy());
			return true;
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x0015ACB0 File Offset: 0x00158EB0
		private static bool wait_menu_army_enter_settlement_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty && MobileParty.MainParty.MapEvent == null && MobileParty.MainParty.BesiegedSettlement == null)
			{
				Settlement settlement = null;
				if (MobileParty.MainParty.CurrentSettlement != null)
				{
					settlement = MobileParty.MainParty.CurrentSettlement;
				}
				else if (MobileParty.MainParty.Army.LeaderParty.LastVisitedSettlement != null && MobileParty.MainParty.Army.LeaderParty.LastVisitedSettlement.Position.Distance(MobileParty.MainParty.Army.LeaderParty.Position) < 1f)
				{
					settlement = MobileParty.MainParty.Army.LeaderParty.LastVisitedSettlement;
				}
				if (settlement != null)
				{
					if (settlement.IsTown)
					{
						MBTextManager.SetTextVariable("ENTER_SETTLEMENT", "{=bkoJ57h3}Enter the Town", false);
					}
					else if (settlement.IsCastle)
					{
						MBTextManager.SetTextVariable("ENTER_SETTLEMENT", "{=aa3kbW8j}Enter the Castle", false);
					}
					else if (settlement.IsVillage)
					{
						MBTextManager.SetTextVariable("ENTER_SETTLEMENT", "{=8UzRj1YW}Enter the Village", false);
					}
					else
					{
						MBTextManager.SetTextVariable("ENTER_SETTLEMENT", "{=eabR87ne}Enter the Settlement", false);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x0015ADF0 File Offset: 0x00158FF0
		private static void wait_menu_army_enter_settlement_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty && (MobileParty.MainParty.CurrentSettlement == null || PlayerEncounter.Current == null))
			{
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty, MobileParty.MainParty.Army.LeaderParty.LastVisitedSettlement);
			}
			Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
			if (currentSettlement.IsTown)
			{
				GameMenu.ActivateGameMenu("town");
				return;
			}
			if (currentSettlement.IsCastle)
			{
				GameMenu.ActivateGameMenu("castle");
				return;
			}
			GameMenu.ActivateGameMenu("village");
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x0015AE8B File Offset: 0x0015908B
		private static bool wait_menu_army_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.MapEvent == null && MobileParty.MainParty.BesiegedSettlement == null;
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x0015AEBC File Offset: 0x001590BC
		private static void wait_menu_army_leave_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(true);
			}
			else
			{
				GameMenu.ExitToLast();
			}
			if (Settlement.CurrentSettlement != null)
			{
				LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
				PartyBase.MainParty.SetVisualAsDirty();
			}
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x0015AEF8 File Offset: 0x001590F8
		private static void wait_menu_army_abandon_on_consequence(MenuCallbackArgs args)
		{
			ChangeClanInfluenceAction.Apply(Clan.PlayerClan, (float)(-(float)Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAbandoningArmy()));
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(true);
			}
			else
			{
				GameMenu.ExitToLast();
			}
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x0015AF44 File Offset: 0x00159144
		private static void OnTick(float dt)
		{
			if (MobileParty.MainParty.AttachedTo != null)
			{
				MenuContext currentMenuContext = Campaign.Current.CurrentMenuContext;
				string text;
				if (currentMenuContext == null)
				{
					text = null;
				}
				else
				{
					GameMenu gameMenu = currentMenuContext.GameMenu;
					text = ((gameMenu != null) ? gameMenu.StringId : null);
				}
				Settlement settlement;
				if (text == "army_wait" && (settlement = MobileParty.MainParty.AttachedTo.Army.AiBehaviorObject as Settlement) != null && settlement.SiegeEvent != null && Hero.MainHero.PartyBelongedTo.Army.LeaderParty.BesiegedSettlement == settlement)
				{
					PlayerSiege.StartPlayerSiege(BattleSideEnum.Attacker, false, settlement);
					PlayerSiege.StartSiegePreparation();
				}
			}
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x0015AFDC File Offset: 0x001591DC
		private static void army_dispersed_continue_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				GameMenu.ExitToLast();
				return;
			}
			if (MobileParty.MainParty.CurrentSettlement.IsVillage)
			{
				GameMenu.SwitchToMenu("village");
				return;
			}
			if (MobileParty.MainParty.CurrentSettlement.IsTown)
			{
				GameMenu.SwitchToMenu((MobileParty.MainParty.CurrentSettlement.SiegeEvent != null) ? "menu_siege_strategies" : "town");
				return;
			}
			if (MobileParty.MainParty.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu((MobileParty.MainParty.CurrentSettlement.SiegeEvent != null) ? "menu_siege_strategies" : "castle");
				return;
			}
			LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x0015B08C File Offset: 0x0015928C
		private static bool army_dispersed_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x0015B098 File Offset: 0x00159298
		[GameMenuInitializationHandler("army_wait")]
		private static void game_menu_army_wait_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army.LeaderParty.IsCurrentlyAtSea)
			{
				args.MenuContext.SetBackgroundMeshName("encounter_naval");
				return;
			}
			args.MenuContext.SetBackgroundMeshName(Hero.MainHero.MapFaction.Culture.EncounterBackgroundMesh);
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x0015B0EC File Offset: 0x001592EC
		[GameMenuInitializationHandler("army_wait_at_settlement")]
		private static void game_menu_army_wait_at_settlement_on_init(MenuCallbackArgs args)
		{
			Settlement settlement = ((Settlement.CurrentSettlement != null) ? Settlement.CurrentSettlement : ((MobileParty.MainParty.LastVisitedSettlement != null) ? MobileParty.MainParty.LastVisitedSettlement : MobileParty.MainParty.AttachedTo.LastVisitedSettlement));
			args.MenuContext.SetBackgroundMeshName(settlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x0015B145 File Offset: 0x00159345
		[GameMenuInitializationHandler("army_dispersed")]
		private static void game_menu_army_dispersed_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				args.MenuContext.SetBackgroundMeshName("encounter_naval");
				return;
			}
			args.MenuContext.SetBackgroundMeshName("wait_fallback");
		}

		// Token: 0x0400144B RID: 5195
		private readonly TextObject _leadingArmyDescriptionText;

		// Token: 0x0400144C RID: 5196
		private readonly TextObject _armyDescriptionText;

		// Token: 0x0400144D RID: 5197
		private readonly TextObject _disbandingArmyDescriptionText;

		// Token: 0x0400144E RID: 5198
		private Army.ArmyDispersionReason _playerArmyDispersionReason;
	}
}
