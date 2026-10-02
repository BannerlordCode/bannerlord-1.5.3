using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000453 RID: 1107
	public class PlayerTownVisitCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004716 RID: 18198 RVA: 0x0015BEDC File Offset: 0x0015A0DC
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.AddGameMenus));
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x0015BF2E File Offset: 0x0015A12E
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<CampaignTime>("_lastTimeRelationGivenPathfinder", ref this._lastTimeRelationGivenPathfinder);
			dataStore.SyncData<CampaignTime>("_lastTimeRelationGivenWaterDiviner", ref this._lastTimeRelationGivenWaterDiviner);
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x0015BF54 File Offset: 0x0015A154
		protected void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenu("town", "{=!}{SETTLEMENT_INFO}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_keep", "{=!}{GO_TO_KEEP_TEXT}", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_go_to_keep_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_go_to_keep_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_arena", "{=CfDlOdTH}Go to the arena", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_go_to_arena_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town_arena");
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_backstreet", "{=l9sFJawW}Go to the tavern district", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_go_to_tavern_district_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town_backstreet");
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "manage_production", "{=dgf6q4qB}Manage town", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_manage_town_on_condition), null, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "manage_production_cheat", "{=zZ3GqbzC}Manage town (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_manage_town_cheat_on_condition), null, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "recruit_volunteers", "{=E31IJyqs}Recruit troops", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_recruit_troops_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_recruit_volunteers_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "trade", "{=GmcgoiGy}Trade", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_trade_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_market_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_smithy", "{=McHsHbH8}Enter smithy", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_craft_item_on_condition), delegate(MenuCallbackArgs x)
			{
				CraftingHelper.OpenCrafting(CraftingTemplate.All[0], null);
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_streets", "{=R5ObSaUN}Take a walk around the town center", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_streets_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_streets_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_wait", "{=zEoHYEUS}Wait here for some time", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_wait_here_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town_wait_menus");
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_return_to_army", "{=SK43eB6y}Return to Army", new GameMenuOption.OnConditionDelegate(this.game_menu_return_to_army_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_return_to_army_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town", "town_leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_settlement_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_keep", "{=!}{SETTLEMENT_INFO}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_keep_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "town_lords_hall_go_to_dungeon", "{=etjMHPjQ}Go to dungeon", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_go_dungeon_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_go_dungeon_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "leave_troops_to_garrison", "{=7J9KNFTz}Donate troops to garrison", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_leave_troops_garrison_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_leave_troops_garrison_on_consequece), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "manage_garrison", "{=QazTA60M}Manage garrison", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_manage_garrison_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_manage_garrison_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "open_stash", "{=xl4K9ecB}Open stash", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_open_stash_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_open_stash_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "town_lords_hall", "{=dv2ZNazN}Go to the lord's hall", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_go_to_lords_hall_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_lordshall_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "town_lords_hall_cheat", "{=!}Go to the lord's hall (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_lords_hall_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_lordshall_cheat_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep", "town_castle_back", "{=qWAmxyYz}Back to town center", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_keep_dungeon", "{=!}{PRISONER_INTRODUCTION}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_keep_dungeon_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_dungeon", "town_prison_leave_prisoners", "{=kmsNUfbA}Donate prisoners", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_leave_prisoners_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_leave_prisoners_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_dungeon", "town_prison_manage_prisoners", "{=VXkL5Ysd}Manage prisoners", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_manage_prisoners_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_manage_prisoners_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_dungeon", "town_prison", "{=UnQFawna}Enter the dungeon", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_enter_the_dungeon_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_dungeon_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_dungeon", "town_prison_cheat", "{=KBxajw4c}Enter the dungeon (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_dungeon_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_dungeon_cheat_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_dungeon", "town_keep_dungeon_back", "{=E1OwmQFb}Back", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town_keep");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_keep_bribe", "{=yyz111nn}The guards say that they can't just let anyone in.", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_keep_bribe_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_bribe", "town_keep_bribe_pay", "{=fxEka7Bm}Pay a {AMOUNT}{GOLD_ICON} bribe to enter the keep", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_bribe_pay_bribe_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_bribe_pay_bribe_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_keep_bribe", "town_keep_bribe_back", "{=E1OwmQFb}Back", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_enemy_town_keep", "{=!}{SCOUT_KEEP_TEXT}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_enemy_keep_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_enemy_town_keep", "settlement_go_back_to_center", "{=qWAmxyYz}Back to town center", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("castle_enemy_keep", "{=!}{SCOUT_KEEP_TEXT}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_enemy_keep_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_enemy_keep", "settlement_go_back_to_center", "{=qWAmxyYz}Back to castle", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("castle_outside");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_backstreet", "{=Zwy8JybD}You are in the backstreets. The local tavern seems to be attracting its usual crowd.", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_backstreet_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_backstreet", "town_tavern", "{=qcl3YTPh}Visit the tavern", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.visit_the_tavern_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_tavern_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_backstreet", "sell_all_prisoners", "{=xZIBKK0v}Ransom your prisoners ({RANSOM_AMOUNT}{GOLD_ICON})", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.SellPrisonersCondition), delegate(MenuCallbackArgs x)
			{
				PlayerTownVisitCampaignBehavior.SellAllTransferablePrisoners();
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_backstreet", "sell_some_prisoners", "{=Q8VN9UCq}Choose the prisoners to be ransomed", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.SellPrisonerOneStackCondition), delegate(MenuCallbackArgs x)
			{
				PlayerTownVisitCampaignBehavior.ChooseRansomPrisoners();
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_backstreet", "town_backstreet_back", "{=qWAmxyYz}Back to town center", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_arena", "{=5id9mGrc}You are near the arena. {ADDITIONAL_STRING}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_arena_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_arena", "town_enter_arena", "{=YQ3vm6er}Enter the arena", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_enter_the_arena_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_town_arena_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_arena", "town_arena_back", "{=qWAmxyYz}Back to town center", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("settlement_player_unconscious", "{=S5OEsjwg}You slip into unconsciousness. After a little while some of the friendlier locals manage to bring you around. A little confused but without any serious injuries, you resolve to be more careful next time.", null, GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("settlement_player_unconscious", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.settlement_player_unconscious_continue_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddWaitGameMenu("settlement_wait", "{=8AbHxCM8}{CAPTIVITY_TEXT}{newline}Waiting in captivity...", new OnInitDelegate(PlayerTownVisitCampaignBehavior.settlement_wait_on_init), new OnConditionDelegate(PlayerTownVisitCampaignBehavior.wait_menu_prisoner_wait_on_condition), null, new OnTickDelegate(PlayerTownVisitCampaignBehavior.wait_menu_settlement_wait_on_tick), GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddWaitGameMenu("town_wait_menus", "{=ydbVysqv}You are waiting in {CURRENT_SETTLEMENT}.", new OnInitDelegate(this.game_menu_settlement_wait_on_init), new OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_wait_on_condition), null, delegate(MenuCallbackArgs args, CampaignTime dt)
			{
				this.SwitchToMenuIfThereIsAnInterrupt(args.MenuContext.GameMenu.StringId);
			}, GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.SettlementWithBoth, 0f, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_wait_menus", "wait_leave", "{=UqDNAZqM}Stop waiting", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs args)
			{
				PlayerEncounter.Current.IsPlayerWaiting = false;
				this.SwitchToMenuIfThereIsAnInterrupt(args.MenuContext.GameMenu.StringId);
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("castle", "{=!}{SETTLEMENT_INFO}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "castle_prison", "{=esSm5V6t}Go to the dungeon", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_the_dungeon_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_keep_dungeon_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "castle_prison_cheat", "{=pa7oiQb1}Go to the dungeon (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_dungeon_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_dungeon_cheat_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "manage_garrison", "{=QazTA60M}Manage garrison", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_manage_garrison_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_manage_garrison_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "manage_production", "{=Ll1EJHXF}Manage castle", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_manage_castle_on_condition), null, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "open_stash", "{=xl4K9ecB}Open stash", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_open_stash_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_keep_open_stash_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "leave_troops_to_garrison", "{=7J9KNFTz}Donate troops to garrison", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_leave_troops_garrison_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_leave_troops_garrison_on_consequece), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "take_a_walk_around_the_castle", "{=R92XzKXE}Take a walk around the castle", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_take_a_walk_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_take_a_walk_around_the_castle_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "castle_lords_hall", "{=dv2ZNazN}Go to the lord's hall", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_lords_hall_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_lordshall_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "castle_lords_hall_cheat", "{=dl6YxNTT}Go to the lord's hall (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_lords_hall_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_lordshall_cheat_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "town_wait", "{=zEoHYEUS}Wait here for some time", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_wait_here_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("town_wait_menus");
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "disembark", "{=kVqzLm5o}Disembark", new GameMenuOption.OnConditionDelegate(this.game_menu_castle_disembark_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_disembark_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "leave_set_sail", "{=fbCbFqyj}Set sail", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_set_sail_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_set_sail_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "castle_return_to_army", "{=SK43eB6y}Return to Army", new GameMenuOption.OnConditionDelegate(this.game_menu_return_to_army_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_return_to_army_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_settlement_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("castle_dungeon", "{=!}{PRISONER_INTRODUCTION}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.town_keep_dungeon_on_init), GameMenu.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_dungeon", "town_prison_leave_prisoners", "{=kmsNUfbA}Donate prisoners", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_leave_prisoners_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_leave_prisoners_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_dungeon", "town_prison_manage_prisoners", "{=VXkL5Ysd}Manage prisoners", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_manage_prisoners_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_manage_prisoners_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_dungeon", "town_prison", "{=UnQFawna}Enter the dungeon", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_enter_the_dungeon_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_dungeon_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_dungeon", "town_prison_cheat", "{=KBxajw4c}Enter the dungeon (Cheat)", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_castle_go_to_dungeon_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_dungeon_cheat_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("castle_dungeon", "town_keep_dungeon_back", "{=E1OwmQFb}Back", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), delegate(MenuCallbackArgs x)
			{
				GameMenu.SwitchToMenu("castle");
			}, true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("village", "{=!}{SETTLEMENT_INFO}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "recruit_volunteers", "{=E31IJyqs}Recruit troops", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_recruit_volunteers_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_recruit_volunteers_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "trade", "{=VN4ctHIU}Buy products", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_buy_good_on_condition), null, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "village_center", "{=U4azeSib}Take a walk through the lands", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_village_center_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_village_center_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "village_wait", "{=zEoHYEUS}Wait here for some time", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_wait_here_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_wait_village_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "village_return_to_army", "{=SK43eB6y}Return to Army", new GameMenuOption.OnConditionDelegate(this.game_menu_return_to_army_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_return_to_army_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "disembark", "{=kVqzLm5o}Disembark", new GameMenuOption.OnConditionDelegate(this.game_menu_village_disembark_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_disembark_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "leave_set_sail", "{=fbCbFqyj}Set sail", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_set_sail_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_set_sail_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(PlayerTownVisitCampaignBehavior.game_menu_settlement_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village_looted", "disembark", "{=kVqzLm5o}Disembark", new GameMenuOption.OnConditionDelegate(this.game_menu_village_disembark_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_disembark_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village_looted", "leave_set_sail", "{=fbCbFqyj}Set sail", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_set_sail_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_set_sail_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddWaitGameMenu("village_wait_menus", "{=lsBuV9W7}You are waiting in the village.", new OnInitDelegate(this.game_menu_settlement_wait_on_init), new OnConditionDelegate(PlayerTownVisitCampaignBehavior.game_menu_village_wait_on_condition), null, delegate(MenuCallbackArgs args, CampaignTime dt)
			{
				this.SwitchToMenuIfThereIsAnInterrupt(args.MenuContext.GameMenu.StringId);
			}, GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.SettlementWithBoth, 0f, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village_wait_menus", "wait_leave", "{=UqDNAZqM}Stop waiting", new GameMenuOption.OnConditionDelegate(PlayerTownVisitCampaignBehavior.back_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_stop_waiting_at_village_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddWaitGameMenu("prisoner_wait", "{=!}{CAPTIVITY_TEXT}", new OnInitDelegate(PlayerTownVisitCampaignBehavior.wait_menu_prisoner_wait_on_init), new OnConditionDelegate(PlayerTownVisitCampaignBehavior.wait_menu_prisoner_wait_on_condition), null, new OnTickDelegate(PlayerTownVisitCampaignBehavior.wait_menu_prisoner_wait_on_tick), GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameMenu.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x0015CF58 File Offset: 0x0015B158
		private bool game_menu_village_disembark_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			if (Settlement.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction) && Settlement.CurrentSettlement.Village.VillageState != Village.VillageStates.Looted)
			{
				args.Tooltip = new TextObject("{=FRbbKXXY}You cannot disembark to a hostile village.", null);
				args.IsEnabled = false;
			}
			return MobileParty.MainParty.IsCurrentlyAtSea;
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x0015CFBC File Offset: 0x0015B1BC
		private bool game_menu_castle_disembark_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return MobileParty.MainParty.IsCurrentlyAtSea;
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x0015CFD0 File Offset: 0x0015B1D0
		private void game_menu_settlement_wait_on_init(MenuCallbackArgs args)
		{
			string text = (PlayerEncounter.EncounterSettlement.IsVillage ? "village" : (PlayerEncounter.EncounterSettlement.IsTown ? "town" : (PlayerEncounter.EncounterSettlement.IsCastle ? "castle" : null)));
			if (text != null)
			{
				PlayerTownVisitCampaignBehavior.UpdateMenuLocations(text);
			}
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.IsPlayerWaiting = true;
			}
			MobileParty.MainParty.SetMoveModeHold();
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x0015D040 File Offset: 0x0015B240
		private static void OpenMissionWithSettingPreviousLocation(string previousLocationId, string missionLocationId)
		{
			Campaign.Current.GameMenuManager.NextLocation = LocationComplex.Current.GetLocationWithId(missionLocationId);
			Campaign.Current.GameMenuManager.PreviousLocation = LocationComplex.Current.GetLocationWithId(previousLocationId);
			PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(Campaign.Current.GameMenuManager.NextLocation, null, null, null);
			Campaign.Current.GameMenuManager.NextLocation = null;
			Campaign.Current.GameMenuManager.PreviousLocation = null;
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x0015D0BE File Offset: 0x0015B2BE
		private void game_menu_stop_waiting_at_village_on_consequence(MenuCallbackArgs args)
		{
			EnterSettlementAction.ApplyForParty(MobileParty.MainParty, MobileParty.MainParty.LastVisitedSettlement);
			GameMenu.SwitchToMenu("village");
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x0015D0DE File Offset: 0x0015B2DE
		private static bool continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x0015D0EC File Offset: 0x0015B2EC
		private static bool game_menu_castle_go_to_the_dungeon_on_condition(MenuCallbackArgs args)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterDungeon(Settlement.CurrentSettlement, out accessDetails);
			if (Settlement.CurrentSettlement.BribePaid < Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterDungeon(Settlement.CurrentSettlement) && accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && !FactionManager.IsAtWarAgainstFaction(Settlement.CurrentSettlement.MapFaction, Hero.MainHero.MapFaction))
			{
				args.Tooltip = new TextObject("{=aPoS8wOW}You have limited access to Dungeon.", null);
				args.IsEnabled = false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "prison").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			return true;
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x0015D1C0 File Offset: 0x0015B3C0
		private static bool game_menu_castle_enter_the_dungeon_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "prison").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			return true;
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x0015D210 File Offset: 0x0015B410
		private static bool game_menu_castle_go_to_dungeon_cheat_on_condition(MenuCallbackArgs args)
		{
			return Game.Current.IsDevelopmentMode;
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x0015D21C File Offset: 0x0015B41C
		private static bool game_menu_castle_leave_prisoners_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DonatePrisoners;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement.IsFortification)
			{
				if (currentSettlement.Party != null && currentSettlement.Party.PrisonerSizeLimit <= currentSettlement.Party.NumberOfPrisoners)
				{
					args.IsEnabled = false;
					args.Tooltip = GameTexts.FindText("str_dungeon_size_limit_exceeded", null);
					args.Tooltip.SetTextVariable("TROOP_NUMBER", currentSettlement.Party.NumberOfPrisoners);
				}
				return currentSettlement.OwnerClan != Clan.PlayerClan && currentSettlement.MapFaction == Hero.MainHero.MapFaction;
			}
			return false;
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x0015D2B8 File Offset: 0x0015B4B8
		private static bool game_menu_castle_manage_prisoners_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.ManagePrisoners;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			return currentSettlement.OwnerClan == Clan.PlayerClan && currentSettlement.MapFaction == Hero.MainHero.MapFaction && currentSettlement.IsFortification;
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x0015D2FA File Offset: 0x0015B4FA
		private static void game_menu_castle_leave_prisoners_on_consequence(MenuCallbackArgs args)
		{
			PartyScreenHelper.OpenScreenAsDonatePrisoners();
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x0015D301 File Offset: 0x0015B501
		private static void game_menu_castle_manage_prisoners_on_consequence(MenuCallbackArgs args)
		{
			PartyScreenHelper.OpenScreenAsManagePrisoners();
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x0015D308 File Offset: 0x0015B508
		private static bool game_menu_town_go_to_keep_on_condition(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=XZFQ1Jf6}Go to the keep", null);
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(Settlement.CurrentSettlement, out accessDetails);
			if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess)
			{
				args.IsEnabled = false;
				PlayerTownVisitCampaignBehavior.SetLordsHallAccessLimitationReasonText(args, accessDetails);
			}
			else if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Disguise)
			{
				textObject = new TextObject("{=1GPa9aTQ}Scout the keep", null);
				args.Tooltip = new TextObject("{=ubOtRU3u}You have limited access to keep while in disguise.", null);
			}
			MBTextManager.SetTextVariable("GO_TO_KEEP_TEXT", textObject, false);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall" || x == "prison").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x0015D3D4 File Offset: 0x0015B5D4
		private static bool game_menu_go_to_tavern_district_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroAccessLocation(Settlement.CurrentSettlement, "tavern", out flag2, out textObject);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "tavern").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x0015D450 File Offset: 0x0015B650
		private static bool game_menu_trade_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.Trade, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Trade;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x0015D490 File Offset: 0x0015B690
		private static bool game_menu_town_recruit_troops_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.RecruitTroops, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Recruit;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x0015D4D0 File Offset: 0x0015B6D0
		private static bool game_menu_wait_here_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.WaitInSettlement, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x0015D50D File Offset: 0x0015B70D
		private void game_menu_wait_village_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village_wait_menus");
			LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x0015D523 File Offset: 0x0015B723
		private bool game_menu_return_to_army_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty;
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x0015D554 File Offset: 0x0015B754
		private void game_menu_return_to_army_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("army_wait_at_settlement");
			if (MobileParty.MainParty.CurrentSettlement.IsVillage)
			{
				PlayerEncounter.LeaveSettlement();
				PlayerEncounter.Finish(true);
			}
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x0015D57C File Offset: 0x0015B77C
		private static bool game_menu_castle_take_a_walk_on_condition(MenuCallbackArgs args)
		{
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "center").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return true;
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x0015D5CC File Offset: 0x0015B7CC
		private static void game_menu_town_go_to_keep_on_consequence(MenuCallbackArgs args)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(Settlement.CurrentSettlement, out accessDetails);
			SettlementAccessModel.AccessLevel accessLevel = accessDetails.AccessLevel;
			int num = (int)accessLevel;
			if (num != 1)
			{
				if (num == 2)
				{
					GameMenu.SwitchToMenu("town_keep");
					return;
				}
			}
			else
			{
				if (accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Bribe)
				{
					GameMenu.SwitchToMenu("town_keep_bribe");
					return;
				}
				if (accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Disguise)
				{
					GameMenu.SwitchToMenu("town_enemy_town_keep");
					return;
				}
			}
			Debug.FailedAssert("invalid LimitedAccessSolution or AccessLevel for town keep", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\PlayerTownVisitCampaignBehavior.cs", "game_menu_town_go_to_keep_on_consequence", 488);
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x0015D654 File Offset: 0x0015B854
		private static bool game_menu_go_dungeon_on_condition(MenuCallbackArgs args)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterDungeon(Settlement.CurrentSettlement, out accessDetails);
			if (Settlement.CurrentSettlement.BribePaid < Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterDungeon(Settlement.CurrentSettlement) && accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess)
			{
				args.Tooltip = new TextObject("{=aPoS8wOW}You have limited access to Dungeon.", null);
				args.IsEnabled = false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "prison").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			return true;
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x0015D70A File Offset: 0x0015B90A
		private static void game_menu_go_dungeon_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("town_keep_dungeon");
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x0015D716 File Offset: 0x0015B916
		private static bool back_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x0015D724 File Offset: 0x0015B924
		private static bool visit_the_tavern_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "tavern").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			return true;
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x0015D774 File Offset: 0x0015B974
		private static bool game_menu_town_go_to_arena_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroAccessLocation(Settlement.CurrentSettlement, "arena", out flag2, out textObject);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "arena").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x0015D7F0 File Offset: 0x0015B9F0
		private static bool game_menu_town_enter_the_arena_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.WalkAroundTheArena, out flag2, out textObject);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "arena").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x0015D868 File Offset: 0x0015BA68
		private static bool game_menu_craft_item_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.Craft, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Craft;
			ICraftingCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			CraftingCampaignBehavior.CraftingOrderSlots craftingOrderSlots;
			if (Settlement.CurrentSettlement.IsTown && campaignBehavior != null && campaignBehavior.CraftingOrders != null && campaignBehavior.CraftingOrders.TryGetValue(Settlement.CurrentSettlement.Town, out craftingOrderSlots) && craftingOrderSlots.CustomOrders.Count > 0)
			{
				args.OptionQuestData |= GameMenuOption.IssueQuestFlags.ActiveIssue;
			}
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x0015D8FC File Offset: 0x0015BAFC
		public static void wait_menu_prisoner_wait_on_init(MenuCallbackArgs args)
		{
			TextObject text = args.MenuContext.GameMenu.GetText();
			int captiveTimeInDays = PlayerCaptivity.CaptiveTimeInDays;
			TextObject textObject;
			if (captiveTimeInDays == 0)
			{
				textObject = GameTexts.FindText("str_prisoner_of_party_menu_text", null);
			}
			else
			{
				textObject = GameTexts.FindText("str_prisoner_of_party_for_days_menu_text", null);
				textObject.SetTextVariable("NUMBER_OF_DAYS", captiveTimeInDays);
				textObject.SetTextVariable("PLURAL", (captiveTimeInDays > 1) ? 1 : 0);
			}
			textObject.SetTextVariable("PARTY_NAME", (Hero.MainHero.PartyBelongedToAsPrisoner != null) ? Hero.MainHero.PartyBelongedToAsPrisoner.Name : TextObject.GetEmpty());
			text.SetTextVariable("CAPTIVITY_TEXT", textObject);
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x0015D997 File Offset: 0x0015BB97
		[GameMenuInitializationHandler("settlement_wait")]
		public static void wait_menu_prisoner_settlement_wait_ui_on_init(MenuCallbackArgs args)
		{
			if (Hero.MainHero.IsFemale)
			{
				args.MenuContext.SetBackgroundMeshName("wait_prisoner_female");
				return;
			}
			args.MenuContext.SetBackgroundMeshName("wait_prisoner_male");
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x0015D9C6 File Offset: 0x0015BBC6
		public static bool wait_menu_prisoner_wait_on_condition(MenuCallbackArgs args)
		{
			return true;
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x0015D9CC File Offset: 0x0015BBCC
		public static void wait_menu_prisoner_wait_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			int captiveTimeInDays = PlayerCaptivity.CaptiveTimeInDays;
			if (captiveTimeInDays == 0)
			{
				return;
			}
			TextObject text = args.MenuContext.GameMenu.GetText();
			TextObject textObject = GameTexts.FindText("str_prisoner_of_party_for_days_menu_text", null);
			textObject.SetTextVariable("NUMBER_OF_DAYS", captiveTimeInDays);
			textObject.SetTextVariable("PLURAL", (captiveTimeInDays > 1) ? 1 : 0);
			textObject.SetTextVariable("PARTY_NAME", PlayerCaptivity.CaptorParty.Name);
			text.SetTextVariable("CAPTIVITY_TEXT", textObject);
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x0015DA44 File Offset: 0x0015BC44
		public static void wait_menu_settlement_wait_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			int captiveTimeInDays = PlayerCaptivity.CaptiveTimeInDays;
			if (captiveTimeInDays == 0)
			{
				return;
			}
			TextObject textObject = (Hero.MainHero.IsPrisoner ? Hero.MainHero.PartyBelongedToAsPrisoner.Settlement.Name : Settlement.CurrentSettlement.Name);
			TextObject text = args.MenuContext.GameMenu.GetText();
			TextObject textObject2 = GameTexts.FindText("str_prisoner_of_settlement_for_days_menu_text", null);
			textObject2.SetTextVariable("NUMBER_OF_DAYS", captiveTimeInDays);
			textObject2.SetTextVariable("PLURAL", (captiveTimeInDays > 1) ? 1 : 0);
			textObject2.SetTextVariable("SETTLEMENT_NAME", textObject);
			text.SetTextVariable("CAPTIVITY_TEXT", textObject2);
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x0015DAE0 File Offset: 0x0015BCE0
		private static bool SellPrisonersCondition(MenuCallbackArgs args)
		{
			if (PartyBase.MainParty.PrisonRoster.Count > 0)
			{
				int ransomValueOfAllTransferablePrisoners = PlayerTownVisitCampaignBehavior.GetRansomValueOfAllTransferablePrisoners();
				if (ransomValueOfAllTransferablePrisoners > 0)
				{
					MBTextManager.SetTextVariable("RANSOM_AMOUNT", ransomValueOfAllTransferablePrisoners);
					args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x0015DB1F File Offset: 0x0015BD1F
		private static bool SellPrisonerOneStackCondition(MenuCallbackArgs args)
		{
			if (PartyBase.MainParty.PrisonRoster.Count > 0)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
				return true;
			}
			return false;
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x0015DB40 File Offset: 0x0015BD40
		private static int GetRansomValueOfAllTransferablePrisoners()
		{
			int num = 0;
			List<string> list = Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetPartyPrisonerLocks().ToList<string>();
			foreach (TroopRosterElement troopRosterElement in PartyBase.MainParty.PrisonRoster.GetTroopRoster())
			{
				if (!list.Contains(troopRosterElement.Character.StringId))
				{
					num += Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(troopRosterElement.Character, Hero.MainHero) * troopRosterElement.Number;
				}
			}
			return num;
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x0015DBEC File Offset: 0x0015BDEC
		private static void ChooseRansomPrisoners()
		{
			GameMenu.SwitchToMenu("town_backstreet");
			PartyScreenHelper.OpenScreenAsRansom();
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x0015DBFD File Offset: 0x0015BDFD
		private static void SellAllTransferablePrisoners()
		{
			SellPrisonersAction.ApplyForSelectedPrisoners(PartyBase.MainParty, null, MobilePartyHelper.GetPlayerPrisonersPlayerCanSell());
			GameMenu.SwitchToMenu("town_backstreet");
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0015DC1C File Offset: 0x0015BE1C
		private static bool game_menu_castle_go_to_lords_hall_on_condition(MenuCallbackArgs args)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(Settlement.CurrentSettlement, out accessDetails);
			if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess)
			{
				args.IsEnabled = false;
				PlayerTownVisitCampaignBehavior.SetLordsHallAccessLimitationReasonText(args, accessDetails);
			}
			else if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Bribe && Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(Settlement.CurrentSettlement) > Hero.MainHero.Gold)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=d0kbtGYn}You don't have enough gold.", null);
			}
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return true;
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x0015DCF0 File Offset: 0x0015BEF0
		private static void SetLordsHallAccessLimitationReasonText(MenuCallbackArgs args, SettlementAccessModel.AccessDetails accessDetails)
		{
			SettlementAccessModel.AccessLimitationReason accessLimitationReason = accessDetails.AccessLimitationReason;
			if (accessLimitationReason == SettlementAccessModel.AccessLimitationReason.HostileFaction)
			{
				args.Tooltip = new TextObject("{=h9i9VXLd}You cannot enter an enemy lord's hall.", null);
				return;
			}
			if (accessLimitationReason != SettlementAccessModel.AccessLimitationReason.LocationEmpty)
			{
				Debug.FailedAssert(string.Format("{0} is not a valid no access reason for lord's hall", accessDetails.AccessLimitationReason), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\PlayerTownVisitCampaignBehavior.cs", "SetLordsHallAccessLimitationReasonText", 737);
				return;
			}
			args.Tooltip = new TextObject("{=cojKmfSk}There is no one inside.", null);
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x0015DD5C File Offset: 0x0015BF5C
		private static bool game_menu_town_keep_go_to_lords_hall_on_condition(MenuCallbackArgs args)
		{
			if (FactionManager.IsAtWarAgainstFaction(Settlement.CurrentSettlement.MapFaction, Hero.MainHero.MapFaction))
			{
				args.Tooltip = new TextObject("{=h9i9VXLd}You cannot enter an enemy lord's hall.", null);
				args.IsEnabled = false;
			}
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return true;
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x0015DDE0 File Offset: 0x0015BFE0
		private static bool game_menu_town_keep_bribe_pay_bribe_on_condition(MenuCallbackArgs args)
		{
			int bribeToEnterLordsHall = Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(Settlement.CurrentSettlement);
			MBTextManager.SetTextVariable("AMOUNT", bribeToEnterLordsHall);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			if (Hero.MainHero.Gold < bribeToEnterLordsHall)
			{
				args.Tooltip = new TextObject("{=d0kbtGYn}You don't have enough gold.", null);
				args.IsEnabled = false;
			}
			return bribeToEnterLordsHall > 0;
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x0015DE7D File Offset: 0x0015C07D
		private static bool game_menu_castle_go_to_lords_hall_cheat_on_condition(MenuCallbackArgs args)
		{
			return Game.Current.IsDevelopmentMode;
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x0015DE89 File Offset: 0x0015C089
		private static void game_menu_castle_take_a_walk_around_the_castle_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("center"), null, null, null);
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x0015DEB0 File Offset: 0x0015C0B0
		private static void game_menu_town_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, false);
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			MobileParty garrisonParty = Settlement.CurrentSettlement.Town.GarrisonParty;
			if (garrisonParty != null && garrisonParty.MemberRoster.Count <= 0)
			{
				MobileParty garrisonParty2 = Settlement.CurrentSettlement.Town.GarrisonParty;
				if (garrisonParty2 != null && garrisonParty2.PrisonRoster.Count <= 0)
				{
					DestroyPartyAction.Apply(null, Settlement.CurrentSettlement.Town.GarrisonParty);
				}
			}
			args.MenuTitle = new TextObject("{=mVKcvY2U}Town Center", null);
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x0015DF64 File Offset: 0x0015C164
		private static void UpdateMenuLocations(string menuID)
		{
			Campaign.Current.GameMenuManager.MenuLocations.Clear();
			Settlement settlement = ((Settlement.CurrentSettlement == null) ? MobileParty.MainParty.CurrentSettlement : Settlement.CurrentSettlement);
			uint num = <PrivateImplementationDetails>.ComputeStringHash(menuID);
			if (num <= 1556184416U)
			{
				if (num <= 983285761U)
				{
					if (num != 672971794U)
					{
						if (num != 864577349U)
						{
							if (num != 983285761U)
							{
								goto IL_03FF;
							}
							if (!(menuID == "castle_enter_bribe"))
							{
								goto IL_03FF;
							}
							return;
						}
						else
						{
							if (!(menuID == "town"))
							{
								goto IL_03FF;
							}
							Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("center"));
							Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("arena"));
							Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("house_1"));
							Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("house_2"));
							Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("house_3"));
							return;
						}
					}
					else if (!(menuID == "castle_enemy_keep"))
					{
						goto IL_03FF;
					}
				}
				else if (num != 1192893027U)
				{
					if (num != 1470125011U)
					{
						if (num != 1556184416U)
						{
							goto IL_03FF;
						}
						if (!(menuID == "castle_dungeon"))
						{
							goto IL_03FF;
						}
					}
					else
					{
						if (!(menuID == "town_keep"))
						{
							goto IL_03FF;
						}
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("lordshall"));
						return;
					}
				}
				else
				{
					if (!(menuID == "village"))
					{
						goto IL_03FF;
					}
					Campaign.Current.GameMenuManager.MenuLocations.AddRange(settlement.LocationComplex.GetListOfLocations());
					return;
				}
			}
			else if (num <= 2596447321U)
			{
				if (num != 1579208614U)
				{
					if (num != 1924483413U)
					{
						if (num != 2596447321U)
						{
							goto IL_03FF;
						}
						if (!(menuID == "castle"))
						{
							goto IL_03FF;
						}
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("center"));
						Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("lordshall"));
						return;
					}
					else if (!(menuID == "town_enemy_town_keep"))
					{
						goto IL_03FF;
					}
				}
				else
				{
					if (!(menuID == "town_backstreet"))
					{
						goto IL_03FF;
					}
					Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("tavern"));
					return;
				}
			}
			else if (num <= 4029725827U)
			{
				if (num != 2781132822U)
				{
					if (num != 4029725827U)
					{
						goto IL_03FF;
					}
					if (!(menuID == "town_center"))
					{
						goto IL_03FF;
					}
					Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("center"));
					Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("arena"));
					return;
				}
				else if (!(menuID == "town_keep_dungeon"))
				{
					goto IL_03FF;
				}
			}
			else if (num != 4147432362U)
			{
				if (num != 4246693001U)
				{
					goto IL_03FF;
				}
				if (!(menuID == "town_arena"))
				{
					goto IL_03FF;
				}
				Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("arena"));
				return;
			}
			else
			{
				if (!(menuID == "town_keep_bribe"))
				{
					goto IL_03FF;
				}
				return;
			}
			Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("prison"));
			return;
			IL_03FF:
			Debug.FailedAssert("Could not get the associated locations for Game Menu: " + menuID, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\PlayerTownVisitCampaignBehavior.cs", "UpdateMenuLocations", 877);
			Campaign.Current.GameMenuManager.MenuLocations.AddRange(settlement.LocationComplex.GetListOfLocations());
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x0015E3AE File Offset: 0x0015C5AE
		private static void town_keep_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, true);
			args.MenuTitle = new TextObject("{=723ig40Q}Keep", null);
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x0015E3EC File Offset: 0x0015C5EC
		private static void town_enemy_keep_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, true);
			TextObject text = args.MenuContext.GameMenu.GetText();
			MobileParty garrisonParty = Settlement.CurrentSettlement.Town.GarrisonParty;
			bool flag = (garrisonParty != null && garrisonParty.PrisonRoster.TotalHeroes > 0) || Settlement.CurrentSettlement.Party.PrisonRoster.TotalHeroes > 0;
			text.SetTextVariable("SCOUT_KEEP_TEXT", flag ? "{=Tfb9LNAr}You have observed the comings and goings of the guards at the keep. You think you've identified a guard who might be approached and offered a bribe." : "{=qGUrhBpI}After spending time observing the keep and eavesdropping on the guards, you conclude that there are no prisoners here who you can liberate.");
			args.MenuTitle = new TextObject("{=723ig40Q}Keep", null);
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x0015E4A0 File Offset: 0x0015C6A0
		private static void town_keep_dungeon_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			args.MenuTitle = new TextObject("{=x04UGQDn}Dungeon", null);
			TextObject textObject;
			if (Settlement.CurrentSettlement.SettlementComponent.GetPrisonerHeroes().Count == 0)
			{
				textObject = new TextObject("{=O4flV28Q}There are no prisoners here.", null);
			}
			else
			{
				int count = Settlement.CurrentSettlement.SettlementComponent.GetPrisonerHeroes().Count;
				textObject = new TextObject("{=gAc8SWDt}There {?(PRISONER_COUNT > 1)}are {PRISONER_COUNT} prisoners{?}is 1 prisoner{\\?} here.", null);
				textObject.SetTextVariable("PRISONER_COUNT", count);
			}
			MBTextManager.SetTextVariable("PRISONER_INTRODUCTION", textObject, false);
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x0015E53C File Offset: 0x0015C73C
		private static void town_keep_bribe_on_init(MenuCallbackArgs args)
		{
			args.MenuTitle = new TextObject("{=723ig40Q}Keep", null);
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(Settlement.CurrentSettlement) == 0)
			{
				GameMenu.ActivateGameMenu("town_keep");
			}
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x0015E594 File Offset: 0x0015C794
		private static void town_backstreet_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			string text = Settlement.CurrentSettlement.Culture.StringId + "_tavern";
			args.MenuContext.SetBackgroundMeshName(text);
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			args.MenuTitle = new TextObject("{=a0MVffcN}Backstreet", null);
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x0015E5F8 File Offset: 0x0015C7F8
		private static void town_arena_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsTown && Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town) != null && Campaign.Current.IsDay)
			{
				TextObject textObject = GameTexts.FindText("str_town_new_tournament_text", null);
				MBTextManager.SetTextVariable("ADDITIONAL_STRING", textObject, false);
			}
			else
			{
				TextObject textObject2 = GameTexts.FindText("str_town_empty_arena_text", null);
				MBTextManager.SetTextVariable("ADDITIONAL_STRING", textObject2, false);
			}
			if (MenuHelper.CheckAndOpenNextLocation(args))
			{
				return;
			}
			args.MenuTitle = new TextObject("{=mMU3H6HZ}Arena", null);
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x0015E6A0 File Offset: 0x0015C8A0
		public static bool game_menu_town_manage_town_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Manage;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			bool flag;
			TextObject textObject;
			return MenuHelper.SetOptionProperties(args, Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(currentSettlement, SettlementAccessModel.SettlementAction.ManageTown, out flag, out textObject), flag, textObject);
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x0015E6DD File Offset: 0x0015C8DD
		public static bool game_menu_town_manage_town_cheat_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Manage;
			return GameManagerBase.Current.IsDevelopmentMode && Settlement.CurrentSettlement.IsTown && Settlement.CurrentSettlement.OwnerClan.Leader != Hero.MainHero;
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x0015E71C File Offset: 0x0015C91C
		private static bool game_menu_town_keep_open_stash_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.OpenStash;
			return Settlement.CurrentSettlement.OwnerClan == Clan.PlayerClan && !Settlement.CurrentSettlement.Town.IsOwnerUnassigned;
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x0015E74B File Offset: 0x0015C94B
		private static void game_menu_town_keep_open_stash_on_consequence(MenuCallbackArgs args)
		{
			InventoryScreenHelper.OpenScreenAsStash(Settlement.CurrentSettlement.Stash);
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x0015E75C File Offset: 0x0015C95C
		private static bool game_menu_manage_garrison_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.ManageGarrison;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			return currentSettlement.OwnerClan == Clan.PlayerClan && currentSettlement.MapFaction == Hero.MainHero.MapFaction && currentSettlement.IsFortification;
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x0015E79E File Offset: 0x0015C99E
		private static bool game_menu_manage_castle_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Manage;
			return Settlement.CurrentSettlement.OwnerClan == Clan.PlayerClan && Settlement.CurrentSettlement.IsCastle;
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x0015E7C8 File Offset: 0x0015C9C8
		private static void game_menu_manage_garrison_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
			if (currentSettlement.Town.GarrisonParty == null)
			{
				currentSettlement.AddGarrisonParty();
			}
			PartyScreenHelper.OpenScreenAsManageTroops(currentSettlement.Town.GarrisonParty);
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x0015E804 File Offset: 0x0015CA04
		private static bool game_menu_leave_troops_garrison_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DonateTroops;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			return currentSettlement.OwnerClan != Clan.PlayerClan && currentSettlement.MapFaction == Hero.MainHero.MapFaction && currentSettlement.IsFortification && (currentSettlement.Town.GarrisonParty == null || currentSettlement.Town.GarrisonParty.Party.PartySizeLimit > currentSettlement.Town.GarrisonParty.Party.NumberOfAllMembers);
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x0015E883 File Offset: 0x0015CA83
		private static void game_menu_leave_troops_garrison_on_consequece(MenuCallbackArgs args)
		{
			PartyScreenHelper.OpenScreenAsDonateGarrisonWithCurrentSettlement();
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x0015E88C File Offset: 0x0015CA8C
		private static bool game_menu_town_town_streets_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroAccessLocation(Settlement.CurrentSettlement, "center", out flag2, out textObject);
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "center").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x0015E907 File Offset: 0x0015CB07
		private static void game_menu_town_town_streets_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("center"), null, null, null);
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x0015E92C File Offset: 0x0015CB2C
		private static void game_menu_town_lordshall_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "lordshall");
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x0015E943 File Offset: 0x0015CB43
		private static void game_menu_castle_lordshall_on_consequence(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "lordshall");
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x0015E954 File Offset: 0x0015CB54
		private static void game_menu_town_keep_bribe_pay_bribe_on_consequence(MenuCallbackArgs args)
		{
			int bribeToEnterLordsHall = Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(Settlement.CurrentSettlement);
			BribeGuardsAction.Apply(Settlement.CurrentSettlement, bribeToEnterLordsHall);
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			GameMenu.ActivateGameMenu("town_keep");
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x0015E996 File Offset: 0x0015CB96
		private static void game_menu_lordshall_cheat_on_consequence(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "lordshall");
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x0015E9A7 File Offset: 0x0015CBA7
		private static void game_menu_dungeon_cheat_on_consequence(MenuCallbackArgs ARGS)
		{
			GameMenu.SwitchToMenu("castle_dungeon");
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x0015E9B3 File Offset: 0x0015CBB3
		private static void game_menu_town_dungeon_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "prison");
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x0015E9CA File Offset: 0x0015CBCA
		private static void game_menu_castle_dungeon_on_consequence(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "prison");
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x0015E9DB File Offset: 0x0015CBDB
		private static void game_menu_keep_dungeon_on_consequence(MenuCallbackArgs args)
		{
			if (FactionManager.IsAtWarAgainstFaction(Settlement.CurrentSettlement.MapFaction, Hero.MainHero.MapFaction))
			{
				GameMenu.SwitchToMenu("castle_enemy_keep");
				return;
			}
			GameMenu.SwitchToMenu("castle_dungeon");
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x0015EA0D File Offset: 0x0015CC0D
		private static void game_menu_town_town_tavern_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "tavern");
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x0015EA24 File Offset: 0x0015CC24
		private static void game_menu_town_town_arena_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			PlayerTownVisitCampaignBehavior.OpenMissionWithSettingPreviousLocation("center", "arena");
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x0015EA3B File Offset: 0x0015CC3B
		private static void game_menu_town_town_market_on_consequence(MenuCallbackArgs args)
		{
			LocationEncounter locationEncounter = PlayerEncounter.LocationEncounter;
			InventoryScreenHelper.OpenScreenAsTrade(Settlement.CurrentSettlement.ItemRoster, Settlement.CurrentSettlement.Town, InventoryScreenHelper.InventoryCategoryType.None, null);
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x0015EA5E File Offset: 0x0015CC5E
		private static bool game_menu_town_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return !MobileParty.MainParty.IsCurrentlyAtSea && (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty);
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x0015EA9C File Offset: 0x0015CC9C
		private static bool game_menu_set_sail_leave_on_condition(MenuCallbackArgs args)
		{
			if (!MobileParty.MainParty.IsCurrentlyAtSea)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.SetSail;
				if (Settlement.CurrentSettlement.HasPort)
				{
					if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
					{
						args.Tooltip = new TextObject("{=NrM7h508}You cannot set sail while you're a member of an army.", null);
						args.IsEnabled = false;
					}
					else if (!MobileParty.MainParty.HasNavalNavigationCapability)
					{
						args.Tooltip = new TextObject("{=Yu10hbHI}You don't own any ships!", null);
						args.IsEnabled = false;
					}
					else if (Settlement.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction) && Settlement.CurrentSettlement.Village.VillageState != Village.VillageStates.Looted)
					{
						args.Tooltip = new TextObject("{=Y9EaZjJm}You cannot set sail from a hostile village.", null);
						args.IsEnabled = false;
					}
					else if (!MobileParty.MainParty.Anchor.IsAtSettlement(Settlement.CurrentSettlement))
					{
						args.Tooltip = new TextObject("{=LmTqrE8x}Your fleet is not docked at this settlement.", null);
						args.IsEnabled = false;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x0015EBAC File Offset: 0x0015CDAC
		private static void game_menu_settlement_leave_on_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.Position = (MobileParty.MainParty.IsCurrentlyAtSea ? MobileParty.MainParty.CurrentSettlement.PortPosition : MobileParty.MainParty.CurrentSettlement.GatePosition);
			if (MobileParty.MainParty.Army != null)
			{
				foreach (MobileParty mobileParty in MobileParty.MainParty.AttachedParties)
				{
					mobileParty.Position = MobileParty.MainParty.Position;
				}
			}
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.SetMoveModeHold();
			Campaign.Current.SaveHandler.SignalAutoSave();
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x0015EC70 File Offset: 0x0015CE70
		private void game_menu_set_sail_leave_on_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.SetSailAtPosition(Settlement.CurrentSettlement.PortPosition);
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.SetMoveModeHold();
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x0015EC96 File Offset: 0x0015CE96
		private void game_menu_disembark_on_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.DisembarkToPosition(Settlement.CurrentSettlement.GatePosition);
			MobileParty.MainParty.Anchor.Settlement = Settlement.CurrentSettlement;
			PlayerTownVisitCampaignBehavior.game_menu_settlement_leave_on_consequence(args);
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x0015ECC8 File Offset: 0x0015CEC8
		private static void settlement_wait_on_init(MenuCallbackArgs args)
		{
			TextObject text = args.MenuContext.GameMenu.GetText();
			TextObject textObject = (Hero.MainHero.IsPrisoner ? Hero.MainHero.PartyBelongedToAsPrisoner.Settlement.Name : Settlement.CurrentSettlement.Name);
			int captiveTimeInDays = PlayerCaptivity.CaptiveTimeInDays;
			TextObject textObject2;
			if (captiveTimeInDays == 0)
			{
				textObject2 = GameTexts.FindText("str_prisoner_of_settlement_menu_text", null);
			}
			else
			{
				textObject2 = GameTexts.FindText("str_prisoner_of_settlement_for_days_menu_text", null);
				textObject2.SetTextVariable("NUMBER_OF_DAYS", captiveTimeInDays);
				textObject2.SetTextVariable("PLURAL", (captiveTimeInDays > 1) ? 1 : 0);
			}
			textObject2.SetTextVariable("SETTLEMENT_NAME", textObject);
			text.SetTextVariable("CAPTIVITY_TEXT", textObject2);
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x0015ED70 File Offset: 0x0015CF70
		private static void game_menu_village_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, false);
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			SettlementAccessModel settlementAccessModel = Campaign.Current.Models.SettlementAccessModel;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			SettlementAccessModel.AccessDetails accessDetails;
			settlementAccessModel.CanMainHeroEnterSettlement(currentSettlement, out accessDetails);
			if (currentSettlement != null && accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess && accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.VillageIsLooted)
			{
				GameMenu.SwitchToMenu("village_looted");
			}
			args.MenuTitle = new TextObject("{=Ua6CNLBZ}Village", null);
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x0015EDEC File Offset: 0x0015CFEC
		private static void game_menu_village_drop_off_on_init(MenuCallbackArgs args)
		{
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, false);
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterSettlement(Settlement.CurrentSettlement, out accessDetails);
			if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess && accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.VillageIsLooted)
			{
				GameMenu.SwitchToMenu("village_looted");
			}
			args.MenuTitle = new TextObject("{=Ua6CNLBZ}Village", null);
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x0015EE4C File Offset: 0x0015D04C
		private static void game_menu_castle_on_init(MenuCallbackArgs args)
		{
			MobileParty garrisonParty = Settlement.CurrentSettlement.Town.GarrisonParty;
			if (garrisonParty != null && garrisonParty.MemberRoster.Count <= 0)
			{
				DestroyPartyAction.Apply(null, Settlement.CurrentSettlement.Town.GarrisonParty);
			}
			PlayerTownVisitCampaignBehavior.SetIntroductionText(Settlement.CurrentSettlement, true);
			PlayerTownVisitCampaignBehavior.UpdateMenuLocations(args.MenuContext.GameMenu.StringId);
			if (Campaign.Current.GameMenuManager.NextLocation != null)
			{
				PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(Campaign.Current.GameMenuManager.NextLocation, Campaign.Current.GameMenuManager.PreviousLocation, null, null);
				Campaign.Current.GameMenuManager.NextLocation = null;
				Campaign.Current.GameMenuManager.PreviousLocation = null;
			}
			args.MenuTitle = new TextObject("{=sVXa3zFx}Castle", null);
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x0015EF24 File Offset: 0x0015D124
		private static bool game_menu_village_village_center_on_condition(MenuCallbackArgs args)
		{
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.GetListOfLocations().ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return Settlement.CurrentSettlement.Village.VillageState == Village.VillageStates.Normal;
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x0015EF66 File Offset: 0x0015D166
		private static void game_menu_village_village_center_on_consequence(MenuCallbackArgs args)
		{
			(PlayerEncounter.LocationEncounter as VillageEncounter).CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("village_center"), null, null, null);
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x0015EF8C File Offset: 0x0015D18C
		private static bool game_menu_village_buy_good_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.Trade, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Trade;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x0015EFC9 File Offset: 0x0015D1C9
		private static void game_menu_recruit_volunteers_on_consequence(MenuCallbackArgs args)
		{
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x0015EFCC File Offset: 0x0015D1CC
		private static bool game_menu_recruit_volunteers_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.RecruitTroops, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Recruit;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x0015F009 File Offset: 0x0015D209
		private static bool game_menu_village_wait_on_condition(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return village.VillageState == Village.VillageStates.Normal;
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x0015F025 File Offset: 0x0015D225
		private static bool game_menu_town_wait_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			MBTextManager.SetTextVariable("CURRENT_SETTLEMENT", Settlement.CurrentSettlement.EncyclopediaLinkWithName, false);
			return true;
		}

		// Token: 0x06004775 RID: 18293 RVA: 0x0015F048 File Offset: 0x0015D248
		public static void settlement_player_unconscious_continue_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
			GameMenu.SwitchToMenu(currentSettlement.IsVillage ? "village" : (currentSettlement.IsCastle ? "castle" : "town"));
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x0015F088 File Offset: 0x0015D288
		private static void SetIntroductionText(Settlement settlement, bool fromKeep)
		{
			TextObject textObject;
			if (settlement.IsTown && !fromKeep)
			{
				if (settlement.OwnerClan == Clan.PlayerClan)
				{
					textObject = new TextObject("{=kXVHwjoV}You have arrived at your fief of {SETTLEMENT_LINK}. {PROSPERITY_INFO} {MORALE_INFO}", null);
				}
				else
				{
					textObject = new TextObject("{=UWzQsHA2}{SETTLEMENT_LINK} is governed by {LORD.LINK}, {FACTION_OFFICIAL} of the {FACTION_TERM}. {PROSPERITY_INFO} {MORALE_INFO}", null);
				}
			}
			else if (settlement.IsTown && fromKeep)
			{
				if (settlement.OwnerClan == Clan.PlayerClan)
				{
					textObject = new TextObject("{=u0Dc5g4Z}You are in your town of {SETTLEMENT_LINK}. {KEEP_INFO}", null);
				}
				else
				{
					textObject = new TextObject("{=q3wD0rbq}{SETTLEMENT_LINK} is governed by {LORD.LINK}, {FACTION_OFFICIAL} of the {FACTION_TERM}. {KEEP_INFO}", null);
				}
			}
			else if (settlement.IsCastle)
			{
				if (settlement.OwnerClan == Clan.PlayerClan)
				{
					textObject = new TextObject("{=dA8RGoQ1}You have arrived at {SETTLEMENT_LINK}. {KEEP_INFO}", null);
				}
				else
				{
					textObject = new TextObject("{=4pmvrnmN}The castle of {SETTLEMENT_LINK} is owned by {LORD.LINK}, {FACTION_OFFICIAL} of the {FACTION_TERM}. {KEEP_INFO}", null);
				}
			}
			else if (settlement.IsVillage)
			{
				if (settlement.OwnerClan == Clan.PlayerClan)
				{
					textObject = new TextObject("{=M5iR1e5h}You have arrived at your fief of {SETTLEMENT_LINK}. {PROSPERITY_INFO}", null);
				}
				else
				{
					textObject = new TextObject("{=RVDojUOM}The lands around {SETTLEMENT_LINK} are owned mostly by {LORD.LINK}, {FACTION_OFFICIAL} of the {FACTION_TERM}. {PROSPERITY_INFO}", null);
				}
			}
			else
			{
				Debug.FailedAssert("Couldn't set settlementIntro!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\PlayerTownVisitCampaignBehavior.cs", "SetIntroductionText", 1477);
				textObject = TextObject.GetEmpty();
			}
			settlement.OwnerClan.Leader.SetPropertiesToTextObject(textObject, "LORD");
			string text = settlement.OwnerClan.Leader.MapFaction.Culture.StringId;
			if (settlement.OwnerClan.Leader.IsFemale)
			{
				text += "_f";
			}
			if (settlement.OwnerClan.Leader == Hero.MainHero && !Hero.MainHero.MapFaction.IsKingdomFaction)
			{
				textObject.SetTextVariable("FACTION_TERM", Hero.MainHero.Clan.EncyclopediaLinkWithName);
				textObject.SetTextVariable("FACTION_OFFICIAL", new TextObject("{=hb30yQPN}leader", null));
			}
			else
			{
				textObject.SetTextVariable("FACTION_TERM", settlement.MapFaction.EncyclopediaLinkWithName);
				if (settlement.OwnerClan.MapFaction.IsKingdomFaction && settlement.OwnerClan.Leader == settlement.OwnerClan.Leader.MapFaction.Leader)
				{
					textObject.SetTextVariable("FACTION_OFFICIAL", GameTexts.FindText("str_faction_ruler", text));
				}
				else
				{
					textObject.SetTextVariable("FACTION_OFFICIAL", GameTexts.FindText("str_faction_official", text));
				}
			}
			textObject.SetTextVariable("SETTLEMENT_LINK", settlement.EncyclopediaLinkWithName);
			settlement.SetPropertiesToTextObject(textObject, "SETTLEMENT_OBJECT");
			string text2 = settlement.SettlementComponent.GetProsperityLevel().ToString();
			if ((settlement.IsTown && settlement.Town.InRebelliousState) || (settlement.IsVillage && settlement.Village.Bound.Town.InRebelliousState))
			{
				textObject.SetTextVariable("PROSPERITY_INFO", GameTexts.FindText("str_settlement_rebellion", null));
				textObject.SetTextVariable("MORALE_INFO", "");
			}
			else if (settlement.IsTown)
			{
				textObject.SetTextVariable("PROSPERITY_INFO", GameTexts.FindText("str_town_long_prosperity_1", text2));
				textObject.SetTextVariable("MORALE_INFO", PlayerTownVisitCampaignBehavior.SetTownMoraleText(settlement));
			}
			else if (settlement.IsVillage)
			{
				textObject.SetTextVariable("PROSPERITY_INFO", GameTexts.FindText("str_village_long_prosperity", text2));
			}
			textObject.SetTextVariable("KEEP_INFO", "");
			if (fromKeep && LocationComplex.Current != null)
			{
				if (!LocationComplex.Current.GetLocationWithId("lordshall").GetCharacterList().Any<LocationCharacter>((LocationCharacter x) => x.Character.IsHero))
				{
					textObject.SetTextVariable("KEEP_INFO", "{=OgkSLkFi}There is nobody in the lord's hall.");
				}
			}
			MBTextManager.SetTextVariable("SETTLEMENT_INFO", textObject, false);
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x0015F404 File Offset: 0x0015D604
		private static TextObject SetTownMoraleText(Settlement settlement)
		{
			SettlementComponent.ProsperityLevel prosperityLevel = settlement.SettlementComponent.GetProsperityLevel();
			string text;
			if (settlement.Town.Loyalty < 25f)
			{
				if (prosperityLevel <= SettlementComponent.ProsperityLevel.Low)
				{
					text = "str_settlement_morale_rebellious_adversity";
				}
				else if (prosperityLevel <= SettlementComponent.ProsperityLevel.Mid)
				{
					text = "str_settlement_morale_rebellious_average";
				}
				else
				{
					text = "str_settlement_morale_rebellious_prosperity";
				}
			}
			else if (settlement.Town.Loyalty < 65f)
			{
				if (prosperityLevel <= SettlementComponent.ProsperityLevel.Low)
				{
				}
				if (prosperityLevel <= SettlementComponent.ProsperityLevel.Mid)
				{
					text = "str_settlement_morale_medium_average";
				}
				else
				{
					text = "str_settlement_morale_medium_prosperity";
				}
			}
			else if (prosperityLevel <= SettlementComponent.ProsperityLevel.Low)
			{
				text = "str_settlement_morale_high_adversity";
			}
			else if (prosperityLevel <= SettlementComponent.ProsperityLevel.Mid)
			{
				text = "str_settlement_morale_high_average";
			}
			else
			{
				text = "str_settlement_morale_high_prosperity";
			}
			return GameTexts.FindText(text, null);
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x0015F4AA File Offset: 0x0015D6AA
		[GameMenuInitializationHandler("town_guard")]
		[GameMenuInitializationHandler("menu_tournament_withdraw_verify")]
		[GameMenuInitializationHandler("menu_tournament_bet_confirm")]
		[GameMenuInitializationHandler("town_castle_not_enough_bribe")]
		[GameMenuInitializationHandler("settlement_player_unconscious")]
		[GameMenuInitializationHandler("castle")]
		[GameMenuInitializationHandler("town_castle_nobody_inside")]
		[GameMenuInitializationHandler("encounter_interrupted")]
		[GameMenuInitializationHandler("encounter_interrupted_siege_preparations")]
		[GameMenuInitializationHandler("castle_dungeon")]
		[GameMenuInitializationHandler("castle_enemy_keep")]
		[GameMenuInitializationHandler("encounter_interrupted_raid_started")]
		[GameMenuInitializationHandler("settlement_player_unconscious_when_disguise")]
		public static void game_menu_town_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0015F4C8 File Offset: 0x0015D6C8
		[GameMenuInitializationHandler("town_arena")]
		public static void game_menu_town_menu_arena_on_init(MenuCallbackArgs args)
		{
			string text = Settlement.CurrentSettlement.Culture.StringId + "_arena";
			args.MenuContext.SetBackgroundMeshName(text);
			args.MenuContext.SetPanelSound("event:/ui/panels/panel_settlement_enter_arena");
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/arena");
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0015F51C File Offset: 0x0015D71C
		[GameMenuInitializationHandler("village_hostile_action")]
		[GameMenuInitializationHandler("force_volunteers_village")]
		[GameMenuInitializationHandler("force_supplies_village")]
		[GameMenuInitializationHandler("village_take_food_confirm")]
		public static void game_menu_village_menu_on_init(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			args.MenuContext.SetBackgroundMeshName(village.WaitMeshName);
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x0015F548 File Offset: 0x0015D748
		[GameMenuInitializationHandler("town_keep")]
		public static void game_menu_town_menu_keep_on_init(MenuCallbackArgs args)
		{
			string text = Settlement.CurrentSettlement.Culture.StringId + "_keep";
			args.MenuContext.SetBackgroundMeshName(text);
			args.MenuContext.SetPanelSound("event:/ui/panels/settlement_keep");
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/keep");
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x0015F59B File Offset: 0x0015D79B
		[GameMenuEventHandler("town", "manage_production", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_ui_town_manage_town_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTownManagement();
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x0015F5A8 File Offset: 0x0015D7A8
		[GameMenuEventHandler("town_keep", "manage_production", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_ui_town_castle_manage_town_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTownManagement();
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x0015F5B5 File Offset: 0x0015D7B5
		[GameMenuEventHandler("village", "trade", GameMenuEventHandler.EventType.OnConsequence)]
		private static void game_menu_ui_village_buy_good_on_consequence(MenuCallbackArgs args)
		{
			InventoryScreenHelper.OpenScreenAsTrade(Settlement.CurrentSettlement.ItemRoster, Settlement.CurrentSettlement.Village, InventoryScreenHelper.InventoryCategoryType.None, null);
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x0015F5D2 File Offset: 0x0015D7D2
		[GameMenuEventHandler("village", "manage_production", GameMenuEventHandler.EventType.OnConsequence)]
		private static void game_menu_ui_village_manage_village_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTownManagement();
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0015F5DF File Offset: 0x0015D7DF
		[GameMenuEventHandler("village", "recruit_volunteers", GameMenuEventHandler.EventType.OnConsequence)]
		[GameMenuEventHandler("town_backstreet", "recruit_volunteers", GameMenuEventHandler.EventType.OnConsequence)]
		[GameMenuEventHandler("town", "recruit_volunteers", GameMenuEventHandler.EventType.OnConsequence)]
		private static void game_menu_ui_recruit_volunteers_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenRecruitVolunteers();
			args.MenuContext.SetPanelSound("event:/ui/panels/panel_settlement_enter_recruit");
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x0015F5FC File Offset: 0x0015D7FC
		[GameMenuInitializationHandler("prisoner_wait")]
		private static void wait_menu_ui_prisoner_wait_on_init(MenuCallbackArgs args)
		{
			PartyBase partyBelongedToAsPrisoner = Hero.MainHero.PartyBelongedToAsPrisoner;
			bool flag;
			if (partyBelongedToAsPrisoner == null)
			{
				flag = false;
			}
			else
			{
				MobileParty mobileParty = partyBelongedToAsPrisoner.MobileParty;
				bool? flag2 = ((mobileParty != null) ? new bool?(mobileParty.IsCurrentlyAtSea) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				if (Hero.MainHero.IsFemale)
				{
					args.MenuContext.SetBackgroundMeshName("wait_captive_at_sea_female");
					return;
				}
				args.MenuContext.SetBackgroundMeshName("wait_captive_at_sea_male");
				return;
			}
			else
			{
				if (Hero.MainHero.IsFemale)
				{
					args.MenuContext.SetBackgroundMeshName("wait_captive_female");
					return;
				}
				args.MenuContext.SetBackgroundMeshName("wait_captive_male");
				return;
			}
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x0015F6AA File Offset: 0x0015D8AA
		[GameMenuInitializationHandler("town_backstreet")]
		public static void game_menu_town_menu_backstreet_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/tavern");
			args.MenuContext.SetPanelSound("event:/ui/panels/panel_settlement_enter_tavern");
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0015F6CC File Offset: 0x0015D8CC
		[GameMenuInitializationHandler("town_enemy_town_keep")]
		[GameMenuInitializationHandler("castle_enemy_keep")]
		[GameMenuInitializationHandler("town_keep_dungeon")]
		[GameMenuInitializationHandler("town_keep_bribe")]
		public static void game_menu_town_menu_keep_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/keep");
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0015F6F8 File Offset: 0x0015D8F8
		[GameMenuInitializationHandler("town_wait_menus")]
		[GameMenuInitializationHandler("town_wait")]
		public static void game_menu_town_menu_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/city");
			args.MenuContext.SetPanelSound("event:/ui/panels/panel_settlement_enter_wait");
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0015F734 File Offset: 0x0015D934
		[GameMenuInitializationHandler("town")]
		public static void game_menu_town_menu_enter_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetPanelSound("event:/ui/panels/settlement_city");
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/city");
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x0015F770 File Offset: 0x0015D970
		[GameMenuInitializationHandler("village_wait_menus")]
		public static void game_menu_village_menu_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/village");
			Village village = Settlement.CurrentSettlement.Village;
			args.MenuContext.SetBackgroundMeshName(village.WaitMeshName);
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0015F7AC File Offset: 0x0015D9AC
		[GameMenuInitializationHandler("village")]
		[GameMenuInitializationHandler("village_raid_diplomatically_ended")]
		public static void game_menu_village__enter_menu_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetPanelSound("event:/ui/panels/settlement_village");
			args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/village");
			Village village = Settlement.CurrentSettlement.Village;
			args.MenuContext.SetBackgroundMeshName(village.WaitMeshName);
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0015F7F5 File Offset: 0x0015D9F5
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (party != null && party.IsMainParty)
			{
				settlement.HasVisited = true;
				if (MBRandom.RandomFloat > 0.5f && (settlement.IsVillage || settlement.IsTown))
				{
					this.CheckPerkAndGiveRelation(party, settlement);
				}
			}
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x0015F830 File Offset: 0x0015DA30
		private void CheckPerkAndGiveRelation(MobileParty party, Settlement settlement)
		{
			bool isVillage = settlement.IsVillage;
			Hero hero = null;
			Hero hero2 = null;
			bool flag = (isVillage ? party.HasPerk(DefaultPerks.Scouting.WaterDiviner, out hero, true) : party.HasPerk(DefaultPerks.Scouting.Pathfinder, out hero2, true));
			bool flag2 = (isVillage ? (this._lastTimeRelationGivenWaterDiviner.Equals(CampaignTime.Zero) || this._lastTimeRelationGivenWaterDiviner.ElapsedDaysUntilNow >= 1f) : (this._lastTimeRelationGivenPathfinder.Equals(CampaignTime.Zero) || this._lastTimeRelationGivenPathfinder.ElapsedDaysUntilNow >= 1f));
			if (flag && flag2)
			{
				Hero randomElement = settlement.Notables.GetRandomElement<Hero>();
				if (randomElement != null)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(party.ActualClan.Leader, randomElement, 1, true);
				}
				if (isVillage)
				{
					this._lastTimeRelationGivenWaterDiviner = CampaignTime.Now;
					return;
				}
				this._lastTimeRelationGivenPathfinder = CampaignTime.Now;
			}
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0015F90A File Offset: 0x0015DB0A
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (party != null && party.IsMainParty)
			{
				Campaign.Current.IsMainHeroDisguised = false;
			}
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0015F924 File Offset: 0x0015DB24
		private void SwitchToMenuIfThereIsAnInterrupt(string currentMenuId)
		{
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (genericStateMenu != currentMenuId)
			{
				if (!string.IsNullOrEmpty(genericStateMenu))
				{
					GameMenu.SwitchToMenu(genericStateMenu);
					return;
				}
				GameMenu.ExitToLast();
			}
		}

		// Token: 0x04001450 RID: 5200
		private CampaignTime _lastTimeRelationGivenPathfinder;

		// Token: 0x04001451 RID: 5201
		private CampaignTime _lastTimeRelationGivenWaterDiviner;
	}
}
