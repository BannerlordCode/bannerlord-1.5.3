using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000472 RID: 1138
	public class VillageHostileActionCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060049C4 RID: 18884 RVA: 0x001719D8 File Offset: 0x0016FBD8
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<string, CampaignTime>>("_villageLastHostileActionTimeDictionary", ref this._villageLastHostileActionTimeDictionary);
			dataStore.SyncData<IFaction>("_raiderPartyMapFaction", ref this._raiderPartyMapFaction);
			dataStore.SyncData<Village>("_raidedVillage", ref this._raidedVillage);
		}

		// Token: 0x060049C5 RID: 18885 RVA: 0x00171A10 File Offset: 0x0016FC10
		public override void RegisterEvents()
		{
			CampaignEvents.OnAfterSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterSessionLaunched));
			CampaignEvents.ItemsLooted.AddNonSerializedListener(this, new Action<MobileParty, ItemRoster>(VillageHostileActionCampaignBehavior.OnItemsLooted));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.BeforeGameMenuOpenedEvent.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(VillageHostileActionCampaignBehavior.BeforeGameMenuOpened));
			CampaignEvents.OnGameEarlyLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(VillageHostileActionCampaignBehavior.OnGameEarlyLoaded));
		}

		// Token: 0x060049C6 RID: 18886 RVA: 0x00171A90 File Offset: 0x0016FC90
		private static void OnGameEarlyLoaded(CampaignGameStarter starter)
		{
			if (Campaign.Current.MapStateData != null)
			{
				string gameMenuId = Campaign.Current.MapStateData.GameMenuId;
				if (gameMenuId == "raid_village_no_resist_warn_player" || gameMenuId == "raid_village_resisted")
				{
					Campaign.Current.MapStateData.GameMenuId = "raid_village_resist";
				}
				else if (gameMenuId == "force_supplies_village_resist_warn_player")
				{
					Campaign.Current.MapStateData.GameMenuId = "force_supplies_resist";
				}
				if (gameMenuId == "force_troops_village_resist_warn_player")
				{
					Campaign.Current.MapStateData.GameMenuId = "force_volunteers_resist";
					return;
				}
				if (gameMenuId == "village_loot_no_resist" || gameMenuId == "village_take_food_confirm" || gameMenuId == "village_press_into_service_confirm" || gameMenuId == "menu_press_into_service_success" || gameMenuId == "menu_village_take_food_success")
				{
					if (PlayerEncounter.Current != null)
					{
						PlayerEncounter.Finish(true);
						return;
					}
					Campaign.Current.MapStateData.GameMenuId = null;
				}
			}
		}

		// Token: 0x060049C7 RID: 18887 RVA: 0x00171B90 File Offset: 0x0016FD90
		private static void BeforeGameMenuOpened(MenuCallbackArgs args)
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0)) && args.MenuContext.GameMenu.StringId == "raiding_village" && PlayerEncounter.Current == null)
			{
				GameMenu.ExitToLast();
			}
		}

		// Token: 0x060049C8 RID: 18888 RVA: 0x00171BE8 File Offset: 0x0016FDE8
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.IsRaid && mapEvent.IsPlayerMapEvent)
			{
				MobileParty mobileParty = mapEvent.GetMapEventSide(BattleSideEnum.Attacker).LeaderParty.MobileParty;
				this._raidedVillage = mapEvent.GetMapEventSide(BattleSideEnum.Defender).LeaderParty.Settlement.Village;
				this._raiderPartyMapFaction = mobileParty.MapFaction;
				if (!mapEvent.DiplomaticallyFinished)
				{
					if (this._raidedVillage.Settlement.IsRaided && mobileParty.Party == MobileParty.MainParty.Party)
					{
						GameMenu.ActivateGameMenu("village_player_raid_ended");
						return;
					}
					if (MobileParty.MainParty.IsActive && !Hero.MainHero.IsPrisoner && !mapEvent.EndedByRetreat)
					{
						GameMenu.ActivateGameMenu("village_raid_ended_leaded_by_someone_else");
					}
				}
			}
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x00171CA6 File Offset: 0x0016FEA6
		private void OnAfterSessionLaunched(CampaignGameStarter campaignGameSystemStarter)
		{
			this.AddGameMenus(campaignGameSystemStarter);
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x00171CB0 File Offset: 0x0016FEB0
		private void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenuOption("village", "hostile_action", "{=GM3tAYMr}Take a hostile action", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_action_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_action_on_consequence), false, 1, false, null);
			campaignGameSystemStarter.AddGameMenu("village_hostile_action", "{=YVNZaVCA}What action do you have in mind?", new OnInitDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_menu_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village_hostile_action", "raid_village", "{=CTi0ml5F}Raid the village", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_raid_village_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_action_raid_village_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village_hostile_action", "force_peasants_to_give_volunteers", "{=RL8z99Dt}Force notables to give you recruits", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_force_volunteers_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_action_force_volunteers_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village_hostile_action", "force_peasants_to_give_supplies", "{=eAzwpqE1}Force peasants to give you goods", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_take_food_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_action_take_food_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village_hostile_action", "forget_it", "{=sP9ohQTs}Forget it", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_back_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_action_forget_it_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddWaitGameMenu("raiding_village", "{=hWwr3mrC}You are raiding {VILLAGE_NAME}.", new OnInitDelegate(VillageHostileActionCampaignBehavior.village_raid_game_menu_init), new OnConditionDelegate(VillageHostileActionCampaignBehavior.wait_menu_start_raiding_on_condition), new OnConsequenceDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_on_consequence), new OnTickDelegate(VillageHostileActionCampaignBehavior.wait_menu_raiding_village_on_tick), GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, GameMenu.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("raiding_village", "raiding_village_end", "{=M7CcfbIx}End Raiding", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("raiding_village", "leave_army", "{=hSdJ0UUv}Leave Army", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_at_army_by_leaving_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_at_army_by_leaving_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("raiding_village", "abandon_army", "{=0vnegjxf}Abandon Army", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_at_army_by_abandoning_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_at_army_by_abandoning_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("raid_occupied", "{=!}{RAID_OCCUPPIED_TEXT}", new OnInitDelegate(VillageHostileActionCampaignBehavior.raid_occupied_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("raid_occupied", "raid_occuppied_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.raid_occupied_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.raid_occupied_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("raid_village_no_resist", "{=!}{RAID_NO_RESIST_DESC}", new OnInitDelegate(VillageHostileActionCampaignBehavior.game_menu_raid_no_resist_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("raid_village_no_resist", "raid_village_no_resist_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.game_menu_raid_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_raid_continue_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("raid_village_no_resist", "raid_village_no_resist_leave", "{=sP9ohQTs}Forget it", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_back_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_hostile_action_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("raid_village_resist", "{=!}{RAID_RESIST_DESC}", new OnInitDelegate(VillageHostileActionCampaignBehavior.game_menu_raid_resist_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("raid_village_resist", "raid_village_resist_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.game_menu_raid_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_raid_continue_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("raid_village_resist", "raid_village_resist_leave", "{=sP9ohQTs}Forget it", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_back_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_hostile_action_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("force_supplies_no_resist", "{=!}{FORCE_SUPPLIES_NO_RESIST_DESC}", new OnInitDelegate(VillageHostileActionCampaignBehavior.game_menu_force_supplies_no_resist_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("force_supplies_no_resist", "force_supplies_no_resist_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.game_menu_force_supplies_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_force_supplies_no_resist_continue_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("force_supplies_no_resist", "force_supplies_no_resist_leave", "{=sP9ohQTs}Forget it", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_back_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_hostile_action_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("force_supplies_resist", "{=!}{FORCE_SUPPLIES_DESC}", new OnInitDelegate(VillageHostileActionCampaignBehavior.game_menu_force_supplies_resist_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("force_supplies_resist", "force_supplies_resist_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.game_menu_force_supplies_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_force_supplies_resist_continue_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("force_supplies_resist", "force_supplies_resist_leave", "{=sP9ohQTs}Forget it", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_back_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_hostile_action_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("force_volunteers_no_resist", "{=!}{FORCE_TROOPS_NO_RESIST_DESC}", new OnInitDelegate(VillageHostileActionCampaignBehavior.game_menu_force_troops_no_resist_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("force_volunteers_no_resist", "force_volunteers_no_resist_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.game_menu_force_volunteers_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_force_volunteers_no_resist_continue_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("force_volunteers_no_resist", "force_volunteers_no_resist_leave", "{=sP9ohQTs}Forget it", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_back_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_hostile_action_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("force_volunteers_resist", "{=!}{FORCE_TROOPS_DESC}", new OnInitDelegate(VillageHostileActionCampaignBehavior.game_menu_force_troops_resist_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("force_volunteers_resist", "force_volunteers_resist_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.game_menu_force_volunteers_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_force_volunteers_resist_continue_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("force_volunteers_resist", "force_volunteers_resist_leave", "{=sP9ohQTs}Forget it", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_back_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_hostile_action_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("force_supplies_village", "{=EqFbNha8}The villagers grudgingly bring out what they have for you.", new OnInitDelegate(VillageHostileActionCampaignBehavior.force_supply_game_menu_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("force_supplies_village", "force_supplies_village_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.village_force_supplies_ended_successfully_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("force_volunteers_village", "{=BqkD4YWr}You manage to round up some men from the village who look like they might make decent recruits.", new OnInitDelegate(VillageHostileActionCampaignBehavior.force_troop_game_menu_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("force_volunteers_village", "force_supplies_village_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.village_force_volunteers_ended_successfully_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("village_looted", "{=NxcXfUxu}The village has been looted. A handful of souls scatter as you pass through the burnt-out houses.", new OnInitDelegate(VillageHostileActionCampaignBehavior.village_looted_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village_looted", "leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_back_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.village_looted_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("village_player_raid_ended", "{=m1rzHfxI}{VILLAGE_ENCOUNTER_RESULT}", new OnInitDelegate(VillageHostileActionCampaignBehavior.village_player_raid_ended_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village_player_raid_ended", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.village_player_raid_ended_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("village_raid_ended_leaded_by_someone_else", "{=m1rzHfxI}{VILLAGE_ENCOUNTER_RESULT}", new OnInitDelegate(this.village_raid_ended_leaded_by_someone_else_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village_raid_ended_leaded_by_someone_else", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.hostile_action_common_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.village_raid_ended_leaded_by_someone_else_on_consequence), true, -1, false, null);
		}

		// Token: 0x060049CB RID: 18891 RVA: 0x00172394 File Offset: 0x00170594
		private static void raid_occupied_on_init(MenuCallbackArgs args)
		{
			string encounterCultureBackgroundMesh = MenuHelper.GetEncounterCultureBackgroundMesh(PlayerEncounter.EncounteredParty.MapFaction.Culture);
			args.MenuContext.SetBackgroundMeshName(encounterCultureBackgroundMesh);
			TextObject textObject = new TextObject("{=kkdkQYVN}This village is being raided by {HERO.NAME}.", null);
			MobileParty mobileParty = null;
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.EncounterSettlementAux != null && PlayerEncounter.Current.EncounterSettlementAux.LastAttackerParty != null)
			{
				mobileParty = PlayerEncounter.Current.EncounterSettlementAux.LastAttackerParty;
			}
			else if (PlayerEncounter.EncounteredBattle != null && PlayerEncounter.EncounteredBattle.IsRaid)
			{
				mobileParty = PlayerEncounter.EncounteredBattle.AttackerSide.LeaderParty.MobileParty;
			}
			if (mobileParty == null)
			{
				mobileParty = PlayerEncounter.EncounteredParty.MobileParty;
			}
			if (mobileParty != null && mobileParty.LeaderHero != null)
			{
				textObject.SetCharacterProperties("HERO", mobileParty.LeaderHero.CharacterObject, false);
			}
			else
			{
				textObject.SetTextVariable("HERO", new TextObject("{=C0BEkIZq}hostile forces", null));
			}
			MBTextManager.SetTextVariable("RAID_OCCUPPIED_TEXT", textObject, false);
		}

		// Token: 0x060049CC RID: 18892 RVA: 0x00172483 File Offset: 0x00170683
		private static bool wait_menu_end_raiding_at_army_by_leaving_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty && MobileParty.MainParty.MapEvent == null;
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x001724BE File Offset: 0x001706BE
		private static bool raid_occupied_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x001724C9 File Offset: 0x001706C9
		private static void raid_occupied_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.SetMoveModeHold();
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x001724DC File Offset: 0x001706DC
		private static bool WillVillageResistHostileAction(Settlement settlement)
		{
			MilitiaPartyComponent militiaPartyComponent = settlement.MilitiaPartyComponent;
			MobileParty mobileParty = ((militiaPartyComponent != null) ? militiaPartyComponent.MobileParty : null);
			if (mobileParty == null || mobileParty.MemberRoster.TotalHealthyCount == 0)
			{
				return false;
			}
			float num = 0f;
			num += mobileParty.Party.CalculateCurrentStrength();
			if (mobileParty.IsLordParty)
			{
				return true;
			}
			foreach (MobileParty mobileParty2 in settlement.Parties)
			{
				if (mobileParty2 != mobileParty && mobileParty2.MapFaction == settlement.MapFaction && !mobileParty2.IsCaravan)
				{
					num += mobileParty2.Party.CalculateCurrentStrength();
					if (mobileParty2.IsLordParty)
					{
						return true;
					}
				}
			}
			float num2 = MobileParty.MainParty.Party.CalculateCurrentStrength();
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				foreach (MobileParty mobileParty3 in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
				{
					num2 += mobileParty3.Party.CalculateCurrentStrength();
				}
			}
			Clan ownerClan = settlement.OwnerClan;
			bool flag;
			if (ownerClan == null)
			{
				flag = null != null;
			}
			else
			{
				Hero leader = ownerClan.Leader;
				flag = ((leader != null) ? leader.PartyBelongedTo : null) != null;
			}
			float num3 = (flag ? settlement.OwnerClan.Leader.PartyBelongedTo.Party.RandomFloatWithSeed(1U, 0.05f, 0.15f) : 0.1f);
			return num2 * num3 <= num;
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x00172690 File Offset: 0x00170890
		private static void game_menu_village_hostile_menu_on_init(MenuCallbackArgs args)
		{
			PlayerEncounter.LeaveEncounter = false;
			if (Campaign.Current.GameMenuManager.NextLocation != null)
			{
				PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(Campaign.Current.GameMenuManager.NextLocation, Campaign.Current.GameMenuManager.PreviousLocation, null, null);
				Campaign.Current.GameMenuManager.NextLocation = null;
				Campaign.Current.GameMenuManager.PreviousLocation = null;
				return;
			}
			if (Settlement.CurrentSettlement.SettlementHitPoints <= 0f)
			{
				Debug.FailedAssert("This case should not be possible, check here", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\VillageHostileActionCampaignBehavior.cs", "game_menu_village_hostile_menu_on_init", 322);
			}
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x0017272C File Offset: 0x0017092C
		private static bool game_menu_village_hostile_action_on_condition(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				int minimumNumberOfMenForAttackingVillageViaScene = Campaign.Current.Models.EncounterModel.MinimumNumberOfMenForAttackingVillageViaScene;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumNumberOfMenForAttackingVillageViaScene)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=7b4WZVyU}You should at least have {NUMBER} healthy men in your party to take a hostile action.", null);
					args.Tooltip.SetTextVariable("NUMBER", minimumNumberOfMenForAttackingVillageViaScene);
				}
				else if (!ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().AnyQ<Ship>())
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=hd626n2z}You don't have any shallow draft ship.", null);
				}
				else if (Math.Min(MobileParty.MainParty.MemberRoster.TotalHealthyCount, ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().SumQ<Ship>((Ship x) => x.MainDeckCrewCapacity)) < minimumNumberOfMenForAttackingVillageViaScene)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=aeUaoEYs}Your shallow ship's crew capacity is too low for a hostile action. A minimum of {NUMBER} crew is required. Use a larger or additional vessel.", null);
					args.Tooltip.SetTextVariable("NUMBER", minimumNumberOfMenForAttackingVillageViaScene);
				}
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty) && village != null && Hero.MainHero.MapFaction != village.Owner.MapFaction && village.VillageState == Village.VillageStates.Normal;
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x00172889 File Offset: 0x00170A89
		private bool game_menu_village_hostile_action_raid_village_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			this.CheckVillageAttackableHonorably(args);
			return !DiplomacyHelper.IsSameFactionAndNotEliminated(Hero.MainHero.MapFaction, Settlement.CurrentSettlement.MapFaction);
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x001728B5 File Offset: 0x00170AB5
		private static void game_menu_village_hostile_action_raid_village_on_consequence(MenuCallbackArgs args)
		{
			if (VillageHostileActionCampaignBehavior.WillVillageResistHostileAction(Settlement.CurrentSettlement))
			{
				GameMenu.SwitchToMenu("raid_village_resist");
				return;
			}
			GameMenu.SwitchToMenu("raid_village_no_resist");
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x001728D8 File Offset: 0x00170AD8
		private static void game_menu_village_hostile_action_force_volunteers_on_consequence(MenuCallbackArgs args)
		{
			if (VillageHostileActionCampaignBehavior.WillVillageResistHostileAction(Settlement.CurrentSettlement))
			{
				GameMenu.SwitchToMenu("force_volunteers_resist");
				return;
			}
			GameMenu.SwitchToMenu("force_volunteers_no_resist");
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x001728FC File Offset: 0x00170AFC
		private bool game_menu_village_hostile_action_force_volunteers_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveTroops;
			this.CheckVillageAttackableHonorably(args);
			CampaignTime campaignTime;
			if (this._villageLastHostileActionTimeDictionary.TryGetValue(Settlement.CurrentSettlement.StringId, out campaignTime) && campaignTime.ElapsedDaysUntilNow <= 10f)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=q5OOjkXe}You have already taken hostile action against this village recently.", null);
			}
			else if (this._villageLastHostileActionTimeDictionary.ContainsKey(Settlement.CurrentSettlement.StringId))
			{
				this._villageLastHostileActionTimeDictionary.Remove(Settlement.CurrentSettlement.StringId);
			}
			else if (Settlement.CurrentSettlement.Village.Hearth <= 0f)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=wRo6hOka}The notables don't have any troops to give.", null);
			}
			return true;
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x001729BE File Offset: 0x00170BBE
		private static void game_menu_village_hostile_action_forget_it_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village");
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x001729CA File Offset: 0x00170BCA
		private static void game_menu_village_hostile_action_take_food_on_consequence(MenuCallbackArgs args)
		{
			if (VillageHostileActionCampaignBehavior.WillVillageResistHostileAction(Settlement.CurrentSettlement))
			{
				GameMenu.SwitchToMenu("force_supplies_resist");
				return;
			}
			GameMenu.SwitchToMenu("force_supplies_no_resist");
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x001729F0 File Offset: 0x00170BF0
		private bool game_menu_village_hostile_action_take_food_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveGoods;
			this.CheckVillageAttackableHonorably(args);
			if (this._villageLastHostileActionTimeDictionary.ContainsKey(Settlement.CurrentSettlement.StringId))
			{
				if (this._villageLastHostileActionTimeDictionary[Settlement.CurrentSettlement.StringId].ElapsedDaysUntilNow <= 10f)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=q5OOjkXe}You have already taken hostile action against this village recently.", null);
				}
				else
				{
					this._villageLastHostileActionTimeDictionary.Remove(Settlement.CurrentSettlement.StringId);
				}
			}
			return true;
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x00172A77 File Offset: 0x00170C77
		private static void game_menu_village_hostile_action_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village_hostile_action");
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x00172A84 File Offset: 0x00170C84
		private static void game_menu_raid_no_resist_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=m5Mxxqqg}The villagers, seeing the size of your force, watch helplessly as your troops prepare to sweep through the village. If you proceed you will gain loot and the village militia will disband, but the {KINGDOM} will regard your act as a crime.", null);
			if (PlayerEncounter.Current.EncounterSettlementAux.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				textObject = new TextObject("{=OjbWbnhT}The villagers, seeing the size of your force, watch helplessly as your troops prepare to sweep through the village. This will hurt your relations with the village.", null);
			}
			MBTextManager.SetTextVariable("RAID_NO_RESIST_DESC", textObject, false);
			MBTextManager.SetTextVariable("KINGDOM", PlayerEncounter.Current.EncounterSettlementAux.MapFaction.InformalName, false);
			VillageHostileActionCampaignBehavior.SetHostileActionWarnPlayerInitBackground(args);
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x00172AFC File Offset: 0x00170CFC
		private static void game_menu_force_supplies_no_resist_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=Z6JlB48N}The villagers grudgingly bring out what they have for you. You may take the supplies. The village militia will disband, but the {KINGDOM} will regard your act as a crime.", null);
			if (PlayerEncounter.Current.EncounterSettlementAux.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				textObject = new TextObject("{=avjZZbch}The villagers grudgingly bring out what they have for you. Taking their supplies will hurt your relations with the village.", null);
			}
			MBTextManager.SetTextVariable("FORCE_SUPPLIES_NO_RESIST_DESC", textObject, false);
			MBTextManager.SetTextVariable("KINGDOM", PlayerEncounter.Current.EncounterSettlementAux.MapFaction.InformalName, false);
			VillageHostileActionCampaignBehavior.SetHostileActionWarnPlayerInitBackground(args);
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x00172B74 File Offset: 0x00170D74
		private static void game_menu_force_troops_no_resist_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=aNfu8vPF}You spot some men from the village who look like they might make decent recruits. The village militia will not resist you and will disband if you proceed, but the {KINGDOM} will regard your act as a crime.", null);
			if (PlayerEncounter.Current.EncounterSettlementAux.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				textObject = new TextObject("{=15CPbtUh}You spot some men from the village who look like they might make decent recruits. You may force them to come with you, though this will hurt your relations with the village.", null);
			}
			MBTextManager.SetTextVariable("FORCE_TROOPS_NO_RESIST_DESC", textObject, false);
			MBTextManager.SetTextVariable("KINGDOM", PlayerEncounter.Current.EncounterSettlementAux.MapFaction.InformalName, false);
			VillageHostileActionCampaignBehavior.SetHostileActionWarnPlayerInitBackground(args);
		}

		// Token: 0x060049DD RID: 18909 RVA: 0x00172BEC File Offset: 0x00170DEC
		private static void game_menu_raid_resist_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=QBRtMnrM}The villagers take up arms and prepare to defend their homes. If you win you may loot the village, but once you give the order to attack you will be at war with the {KINGDOM}.", null);
			if (PlayerEncounter.Current.EncounterSettlementAux.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				textObject = new TextObject("{=kqum1fH5}The villagers take up arms and prepare to defend their homes. This will hurt your relations with the village.", null);
			}
			MBTextManager.SetTextVariable("RAID_RESIST_DESC", textObject, false);
			MBTextManager.SetTextVariable("KINGDOM", PlayerEncounter.Current.EncounterSettlementAux.MapFaction.InformalName, false);
			VillageHostileActionCampaignBehavior.SetHostileActionWarnPlayerInitBackground(args);
		}

		// Token: 0x060049DE RID: 18910 RVA: 0x00172C64 File Offset: 0x00170E64
		private static void game_menu_force_supplies_resist_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=eDZk4ajJ}The villagers refuse to hand over their goods and take up a defensive position. If you defeat them you may take their supplies, but once you give the order to attack you will be at war with the {KINGDOM}.", null);
			if (PlayerEncounter.Current.EncounterSettlementAux.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				textObject = new TextObject("{=EGKekOeV}The villagers refuse to hand over their goods and take up a defensive position. If you defeat them you may take their supplies but this will hurt your relations with the village.", null);
			}
			MBTextManager.SetTextVariable("FORCE_SUPPLIES_DESC", textObject, false);
			MBTextManager.SetTextVariable("KINGDOM", PlayerEncounter.Current.EncounterSettlementAux.MapFaction.InformalName, false);
			VillageHostileActionCampaignBehavior.SetHostileActionWarnPlayerInitBackground(args);
		}

		// Token: 0x060049DF RID: 18911 RVA: 0x00172CDC File Offset: 0x00170EDC
		private static void game_menu_force_troops_resist_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=rySjTkeH}The village notables refuse your demand for recruits, and stand ready to resist you along with their followers. If you defeat them you may round up some of the survivors and force them into your ranks, but once you give the order to attack you will be at war with the {KINGDOM}.", null);
			if (PlayerEncounter.Current.EncounterSettlementAux.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				textObject = new TextObject("{=QScLurk9}The village notables refuse your demand for recruits, and stand ready to resist you along with their followers. If you defeat them you may round up some of the survivors and force them into your ranks. However, this will hurt your relations with the village.", null);
			}
			MBTextManager.SetTextVariable("FORCE_TROOPS_DESC", textObject, false);
			MBTextManager.SetTextVariable("KINGDOM", PlayerEncounter.Current.EncounterSettlementAux.MapFaction.InformalName, false);
			VillageHostileActionCampaignBehavior.SetHostileActionWarnPlayerInitBackground(args);
		}

		// Token: 0x060049E0 RID: 18912 RVA: 0x00172D52 File Offset: 0x00170F52
		private static bool game_menu_raid_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Raid;
			return true;
		}

		// Token: 0x060049E1 RID: 18913 RVA: 0x00172D5D File Offset: 0x00170F5D
		private static void game_menu_raid_continue_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			PlayerEncounter.Current.ForceRaid = true;
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x00172D88 File Offset: 0x00170F88
		private static bool game_menu_force_supplies_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveGoods;
			return true;
		}

		// Token: 0x060049E3 RID: 18915 RVA: 0x00172D92 File Offset: 0x00170F92
		private void game_menu_force_supplies_no_resist_continue_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyMinorCoercionHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			this.village_force_supplies_ended_successfully_on_consequence(args);
		}

		// Token: 0x060049E4 RID: 18916 RVA: 0x00172DAF File Offset: 0x00170FAF
		private static void game_menu_force_supplies_resist_continue_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			PlayerEncounter.Current.ForceSupplies = true;
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x00172DDA File Offset: 0x00170FDA
		private static bool game_menu_force_volunteers_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveTroops;
			return true;
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x00172DE4 File Offset: 0x00170FE4
		private void game_menu_force_volunteers_no_resist_continue_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyMajorCoercionHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			this.village_force_volunteers_ended_successfully_on_consequence(args);
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x00172E01 File Offset: 0x00171001
		private static void game_menu_force_volunteers_resist_continue_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			PlayerEncounter.Current.ForceVolunteers = true;
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x060049E8 RID: 18920 RVA: 0x00172E2C File Offset: 0x0017102C
		private static void game_menu_hostile_action_leave_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village");
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x00172E38 File Offset: 0x00171038
		private static void village_raid_game_menu_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.EncounterSettlement != null)
			{
				MBTextManager.SetTextVariable("VILLAGE_NAME", PlayerEncounter.EncounterSettlement.Name, false);
				VillageHostileActionCampaignBehavior.UpdateWaitMenuProgress(args);
				return;
			}
			Debug.FailedAssert("Party is in raid but mapevent is empty!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\VillageHostileActionCampaignBehavior.cs", "village_raid_game_menu_init", 624);
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x00172E78 File Offset: 0x00171078
		private static bool wait_menu_start_raiding_on_condition(MenuCallbackArgs args)
		{
			MapEvent battle = PlayerEncounter.Battle;
			if (((battle != null) ? battle.MapEventSettlement : null) != null)
			{
				MBTextManager.SetTextVariable("SETTLEMENT_NAME", PlayerEncounter.Battle.MapEventSettlement.Name, false);
				return true;
			}
			Debug.FailedAssert("Party is in raid but mapevent is empty!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\VillageHostileActionCampaignBehavior.cs", "wait_menu_start_raiding_on_condition", 636);
			return false;
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x00172ECE File Offset: 0x001710CE
		private static void wait_menu_end_raiding_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Current.ForceRaid = false;
			PlayerEncounter.Finish(true);
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x00172EE1 File Offset: 0x001710E1
		private static void village_player_raid_ended_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x00172EE8 File Offset: 0x001710E8
		private static void wait_menu_end_raiding_at_army_by_leaving_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Current.ForceRaid = false;
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x00172F08 File Offset: 0x00171108
		private void village_raid_ended_leaded_by_someone_else_on_init(MenuCallbackArgs args)
		{
			if (this._raidedVillage == null)
			{
				VillageStateChangedLogEntry villageStateChangedLogEntry = Campaign.Current.LogEntryHistory.FindLastGameActionLog<VillageStateChangedLogEntry>((VillageStateChangedLogEntry entry) => entry.Village.Settlement == MobileParty.MainParty.LastVisitedSettlement);
				if (villageStateChangedLogEntry != null)
				{
					this._raidedVillage = villageStateChangedLogEntry.Village;
					this._raiderPartyMapFaction = villageStateChangedLogEntry.RaiderPartyMapFaction;
				}
			}
			if (this._raidedVillage != null)
			{
				if (MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.Army == null && MobileParty.MainParty.MapEvent.AttackerSide.LeaderParty != PartyBase.MainParty && MobileParty.MainParty.MapEvent.DefenderSide.LeaderParty != PartyBase.MainParty)
				{
					MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=3OW1QQNx}The raid was ended by the battle outside of the village.", null), false);
					return;
				}
				if (!this._raidedVillage.Settlement.SettlementHitPoints.ApproximatelyEqualsTo(0f, 1E-05f) && this._raiderPartyMapFaction != null && !this._raiderPartyMapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					if (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
					{
						MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=ZJOikvf4}You called off your raid on the village.", null), false);
						return;
					}
					MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=VYKc665f}The army leader called off the raid on the village.", null), false);
					return;
				}
				else if (MobileParty.MainParty.Army == null && this._raiderPartyMapFaction != null)
				{
					if (!this._raiderPartyMapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
					{
						MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=MEuuuOiF}The village was successfully raided with your help.", null), false);
						return;
					}
					MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=sHy7VHbw}The village was successfully saved with your help.", null), false);
					return;
				}
				else if (MobileParty.MainParty.Army != null && this._raidedVillage.Settlement.MapFaction != null)
				{
					if (this._raidedVillage.Settlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
					{
						if (MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
						{
							MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=jaiwriZc}The village was successfully raided by the army you are leading.", null), false);
							return;
						}
						MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=zzRJ7jqR}The village was successfully raided by the army you are following.", null), false);
						return;
					}
					else
					{
						if (MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
						{
							MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=XzDDwHbc}The village is saved by the army you are leading.", null), false);
							return;
						}
						MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=ibiQdZLf}The village is saved by the army you are following.", null), false);
						return;
					}
				}
			}
			else
			{
				MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=HkcYydHe}The raid has ended.", null), false);
			}
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x001731B0 File Offset: 0x001713B0
		private static bool wait_menu_end_raiding_at_army_by_abandoning_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			if (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty || MobileParty.MainParty.MapEvent == null)
			{
				return false;
			}
			args.Tooltip = GameTexts.FindText("str_abandon_army", null);
			args.Tooltip.SetTextVariable("INFLUENCE_COST", Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAbandoningArmy());
			return true;
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x00173234 File Offset: 0x00171434
		private static void wait_menu_end_raiding_at_army_by_abandoning_on_consequence(MenuCallbackArgs args)
		{
			Clan.PlayerClan.Influence -= (float)Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAbandoningArmy();
			PlayerEncounter.Current.ForceRaid = false;
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x00173283 File Offset: 0x00171483
		private static bool wait_menu_end_raiding_on_condition(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}
			return false;
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x001732B2 File Offset: 0x001714B2
		private static void wait_menu_raiding_village_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			MapEvent battle = PlayerEncounter.Battle;
			if (((battle != null) ? battle.MapEventSettlement : null) != null)
			{
				VillageHostileActionCampaignBehavior.UpdateWaitMenuProgress(args);
				return;
			}
			Debug.FailedAssert("Party is in raid but mapEvent is empty!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\VillageHostileActionCampaignBehavior.cs", "wait_menu_raiding_village_on_tick", 786);
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x001732E7 File Offset: 0x001714E7
		private static void force_supply_game_menu_init(MenuCallbackArgs args)
		{
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x001732E9 File Offset: 0x001714E9
		private static void village_raid_ended_leaded_by_someone_else_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				GameMenu.SwitchToMenu("army_wait");
				return;
			}
			GameMenu.ExitToLast();
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x0017331D File Offset: 0x0017151D
		private static void SetHostileActionWarnPlayerInitBackground(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x0017333C File Offset: 0x0017153C
		private void village_force_supplies_ended_successfully_on_consequence(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			GameMenu.SwitchToMenu("village");
			ItemRoster itemRoster = new ItemRoster();
			int num = MathF.Max((int)(Settlement.CurrentSettlement.Village.Hearth * 0.15f), 20);
			GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, num * Campaign.Current.Models.RaidModel.GoldRewardForEachLostHearth, false);
			for (int i = 0; i < Settlement.CurrentSettlement.Village.VillageType.Productions.Count; i++)
			{
				ValueTuple<ItemObject, float> valueTuple = Settlement.CurrentSettlement.Village.VillageType.Productions[i];
				ItemObject item = valueTuple.Item1;
				int num2 = (int)(valueTuple.Item2 / 60f * (float)num);
				if (num2 > 0)
				{
					itemRoster.AddToCounts(item, num2);
				}
			}
			if (!this._villageLastHostileActionTimeDictionary.ContainsKey(Settlement.CurrentSettlement.StringId))
			{
				this._villageLastHostileActionTimeDictionary.Add(Settlement.CurrentSettlement.StringId, CampaignTime.Now);
			}
			else
			{
				this._villageLastHostileActionTimeDictionary[Settlement.CurrentSettlement.StringId] = CampaignTime.Now;
			}
			Settlement.CurrentSettlement.SettlementHitPoints -= Settlement.CurrentSettlement.SettlementHitPoints * 0.8f;
			InventoryScreenHelper.OpenScreenAsLoot(new Dictionary<PartyBase, ItemRoster> { 
			{
				PartyBase.MainParty,
				itemRoster
			} });
			bool flag = MapEvent.PlayerMapEvent != null;
			SkillLevelingManager.OnForceSupplies(MobileParty.MainParty, itemRoster, flag);
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.ForceSupplies = false;
				PlayerEncounter.Current.FinalizeBattle();
			}
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x001734BD File Offset: 0x001716BD
		private static void force_troop_game_menu_init(MenuCallbackArgs args)
		{
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x001734C0 File Offset: 0x001716C0
		private void village_force_volunteers_ended_successfully_on_consequence(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			GameMenu.SwitchToMenu("village");
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			int num = (int)Math.Ceiling((double)(Settlement.CurrentSettlement.Village.Hearth / 30f));
			Hero hero = null;
			if (MobileParty.MainParty.HasPerk(DefaultPerks.Roguery.InBestLight, out hero, false))
			{
				num += Settlement.CurrentSettlement.Notables.Count;
			}
			troopRoster.AddToCounts(Settlement.CurrentSettlement.Culture.BasicTroop, num, false, 0, 0, true, -1);
			if (!this._villageLastHostileActionTimeDictionary.ContainsKey(Settlement.CurrentSettlement.StringId))
			{
				this._villageLastHostileActionTimeDictionary.Add(Settlement.CurrentSettlement.StringId, CampaignTime.Now);
			}
			else
			{
				this._villageLastHostileActionTimeDictionary[Settlement.CurrentSettlement.StringId] = CampaignTime.Now;
			}
			Settlement.CurrentSettlement.SettlementHitPoints -= Settlement.CurrentSettlement.SettlementHitPoints * 0.8f;
			Settlement.CurrentSettlement.Village.Hearth -= (float)(num / 2);
			PartyScreenHelper.OpenScreenAsLoot(troopRoster, TroopRoster.CreateDummyTroopRoster(), MobileParty.MainParty.CurrentSettlement.Name, troopRoster.TotalManCount, null);
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.ForceVolunteers = false;
				PlayerEncounter.Current.FinalizeBattle();
			}
			SkillLevelingManager.OnForceVolunteers(MobileParty.MainParty, Settlement.CurrentSettlement.Party);
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x0017361F File Offset: 0x0017181F
		private static void village_looted_init(MenuCallbackArgs args)
		{
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x00173621 File Offset: 0x00171821
		private static void UpdateWaitMenuProgress(MenuCallbackArgs args)
		{
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(1f - PlayerEncounter.Battle.MapEventSettlement.SettlementHitPoints);
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x00173648 File Offset: 0x00171848
		private static void village_looted_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.SetMoveModeHold();
			Campaign.Current.SaveHandler.SignalAutoSave();
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x00173670 File Offset: 0x00171870
		private static void village_player_raid_ended_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.LastVisitedSettlement == null)
			{
				MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=HkcYydHe}The raid has ended.", null), false);
				return;
			}
			if (!MobileParty.MainParty.LastVisitedSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", "{=aih1Y62W}You have saved the village.", false);
				return;
			}
			if (!MobileParty.MainParty.LastVisitedSettlement.SettlementHitPoints.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=ZJOikvf4}You called off your raid on the village.", null), false);
				return;
			}
			MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", "{=6snepBi5}You have successfully raided the village.", false);
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x00173718 File Offset: 0x00171918
		[GameMenuInitializationHandler("village_player_raid_ended")]
		private static void game_menu_village_raid_ended_menu_sound_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				args.MenuContext.SetBackgroundMeshName("wait_raiding_village_naval");
			}
			else
			{
				args.MenuContext.SetBackgroundMeshName("wait_raiding_village");
			}
			if (MobileParty.MainParty.LastVisitedSettlement != null && MobileParty.MainParty.LastVisitedSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/village_raided");
			}
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x0017378F File Offset: 0x0017198F
		[GameMenuInitializationHandler("village_looted")]
		[GameMenuInitializationHandler("village_raid_ended_leaded_by_someone_else")]
		[GameMenuInitializationHandler("raiding_village")]
		private static void game_menu_ui_village_hostile_raid_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				args.MenuContext.SetBackgroundMeshName("wait_raiding_village_naval");
				return;
			}
			args.MenuContext.SetBackgroundMeshName("wait_raiding_village");
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x001737C0 File Offset: 0x001719C0
		[GameMenuInitializationHandler("village_hostile_action")]
		[GameMenuInitializationHandler("force_volunteers_village")]
		[GameMenuInitializationHandler("force_supplies_village")]
		[GameMenuInitializationHandler("force_supplies_no_resist")]
		[GameMenuInitializationHandler("force_supplies_resist")]
		[GameMenuInitializationHandler("force_volunteers_no_resist")]
		[GameMenuInitializationHandler("force_volunteers_resist")]
		[GameMenuInitializationHandler("raid_village_no_resist")]
		[GameMenuInitializationHandler("raid_village_resist")]
		private static void game_menu_village_menu_on_init(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			args.MenuContext.SetBackgroundMeshName(village.WaitMeshName);
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x001737E9 File Offset: 0x001719E9
		private static bool hostile_action_common_back_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x001737F4 File Offset: 0x001719F4
		private static bool hostile_action_common_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x00173800 File Offset: 0x00171A00
		private void CheckVillageAttackableHonorably(MenuCallbackArgs args)
		{
			Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
			IFaction faction = ((currentSettlement != null) ? currentSettlement.MapFaction : null);
			this.CheckFactionAttackableHonorably(args, faction);
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x0017382C File Offset: 0x00171A2C
		private static void OnItemsLooted(MobileParty mobileParty, ItemRoster lootedItems)
		{
			SkillLevelingManager.OnRaid(mobileParty, lootedItems);
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x00173838 File Offset: 0x00171A38
		private void CheckFactionAttackableHonorably(MenuCallbackArgs args, IFaction faction)
		{
			if (faction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				args.IsEnabled = false;
				args.Tooltip = this.EnemyNotAttackableTooltip;
			}
		}

		// Token: 0x040014C9 RID: 5321
		private const int IntervalForHostileActionAsDay = 10;

		// Token: 0x040014CA RID: 5322
		private readonly TextObject EnemyNotAttackableTooltip = GameTexts.FindText("str_enemy_not_attackable_tooltip", null);

		// Token: 0x040014CB RID: 5323
		private Dictionary<string, CampaignTime> _villageLastHostileActionTimeDictionary = new Dictionary<string, CampaignTime>();

		// Token: 0x040014CC RID: 5324
		private IFaction _raiderPartyMapFaction;

		// Token: 0x040014CD RID: 5325
		private Village _raidedVillage;
	}
}
