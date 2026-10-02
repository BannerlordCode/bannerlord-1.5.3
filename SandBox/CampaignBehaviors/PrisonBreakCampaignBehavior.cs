using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000E0 RID: 224
	public class PrisonBreakCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000A6E RID: 2670 RVA: 0x0004EACC File Offset: 0x0004CCCC
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.CanHeroDieEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.CanHeroDie));
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0004EB20 File Offset: 0x0004CD20
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> availableSpawnPoints)
		{
			if (this._launchingPrisonBreakMission)
			{
				this._launchingPrisonBreakMission = false;
				int num = 8;
				Location locationWithId = LocationComplex.Current.GetLocationWithId("prison");
				locationWithId.RemoveAllCharacters();
				locationWithId.AddCharacter(this.CreatePrisonBreakPrisoner());
				for (int i = 0; i < num; i++)
				{
					LocationCharacter locationCharacter = this.CreatePrisonBreakGuard();
					locationWithId.AddCharacter(locationCharacter);
				}
			}
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0004EB7C File Offset: 0x0004CD7C
		private LocationCharacter CreatePrisonBreakPrisoner()
		{
			AgentData agentData = new AgentData(new SimpleAgentOrigin(this._prisonerHero.CharacterObject, -1, null, default(UniqueTroopDescriptor))).Age((int)this._prisonerHero.CharacterObject.Age).NoHorses(true);
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddCompanionBehaviors), "sp_prison_break_prisoner", true, LocationCharacter.CharacterRelations.Friendly, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_guard"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0004EC08 File Offset: 0x0004CE08
		public LocationCharacter CreatePrisonBreakGuard()
		{
			AgentData prisonGuardAgentData = this.GetPrisonGuardAgentData();
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation((CharacterObject)prisonGuardAgentData.AgentCharacter, out num, out num2, "");
			prisonGuardAgentData.Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(prisonGuardAgentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddStealthAgentBehaviors), "stealth_agent", true, LocationCharacter.CharacterRelations.Enemy, ActionSetCode.GenerateActionSetNameWithSuffix(prisonGuardAgentData.AgentMonster, prisonGuardAgentData.AgentIsFemale, "_guard"), false, false, null, false, false, true, null, false);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0004EC94 File Offset: 0x0004CE94
		private AgentData GetPrisonGuardAgentData()
		{
			List<CharacterObject> list = CharacterHelper.GetTroopTree(Settlement.CurrentSettlement.Owner.Culture.BasicTroop.Culture.BasicTroop, 2f, 3f).ToListQ<CharacterObject>();
			object obj;
			if (!list.AnyQ<CharacterObject>((CharacterObject x) => !x.IsRanged))
			{
				obj = list.GetRandomElementInefficiently<CharacterObject>();
			}
			else
			{
				obj = list.Where<CharacterObject>((CharacterObject x) => !x.IsRanged).GetRandomElementInefficiently<CharacterObject>();
			}
			object obj2 = obj;
			Equipment equipment = obj2.Equipment.Clone(true);
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>("battania_mace_1_t2"), null, null, false));
			return new AgentData(new SimpleAgentOrigin(obj2, -1, null, default(UniqueTroopDescriptor))).Equipment(equipment);
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0004ED78 File Offset: 0x0004CF78
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Hero>("_prisonerHero", ref this._prisonerHero);
			dataStore.SyncData<Dictionary<Settlement, CampaignTime>>("_coolDownData", ref this._coolDownData);
			dataStore.SyncData<string>("_previousMenuId", ref this._previousMenuId);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0004EDB0 File Offset: 0x0004CFB0
		private void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail detail, ref bool result)
		{
			if (detail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle && hero == Hero.MainHero && this._prisonerHero != null && CampaignMission.Current != null)
			{
				Location location = CampaignMission.Current.Location;
				Settlement currentSettlement = Settlement.CurrentSettlement;
				object obj;
				if (currentSettlement == null)
				{
					obj = null;
				}
				else
				{
					LocationComplex locationComplex = currentSettlement.LocationComplex;
					obj = ((locationComplex != null) ? locationComplex.GetLocationWithId("prison") : null);
				}
				if (location == obj)
				{
					result = false;
				}
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0004EE09 File Offset: 0x0004D009
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0004EE1C File Offset: 0x0004D01C
		private void AddGameMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenuOption("town_keep_dungeon", "town_prison_break", "{=lc0YIqby}Stage a prison break", new GameMenuOption.OnConditionDelegate(this.game_menu_stage_prison_break_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_prison_break_from_dungeon_on_consequence), false, 2, false, null);
			campaignGameStarter.AddGameMenuOption("castle_dungeon", "town_prison_break", "{=lc0YIqby}Stage a prison break", new GameMenuOption.OnConditionDelegate(this.game_menu_stage_prison_break_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_prison_break_from_castle_dungeon_on_consequence), false, 2, false, null);
			campaignGameStarter.AddGameMenuOption("town_enemy_town_keep", "town_prison_break", "{=lc0YIqby}Stage a prison break", new GameMenuOption.OnConditionDelegate(this.game_menu_stage_prison_break_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_prison_break_from_enemy_keep_on_consequence), false, 0, false, null);
			campaignGameStarter.AddGameMenuOption("castle_enemy_keep", "town_prison_break", "{=lc0YIqby}Stage a prison break", new GameMenuOption.OnConditionDelegate(this.game_menu_stage_prison_break_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_prison_break_from_castle_enemy_keep_on_consequence), false, 0, false, null);
			campaignGameStarter.AddGameMenu("start_prison_break", "{=aZaujaHb}The guard accepts your offer. He is ready to help you break {PRISONER.NAME} out, if you're willing to pay.", new OnInitDelegate(this.start_prison_break_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("start_prison_break", "start", "{=N6UeziT8}Start ({COST}{GOLD_ICON})", new GameMenuOption.OnConditionDelegate(this.game_menu_castle_prison_break_on_condition), delegate(MenuCallbackArgs args)
			{
				this.OpenPrisonBreakMission();
			}, false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("start_prison_break", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_cancel_prison_break), true, -1, false, null);
			campaignGameStarter.AddGameMenu("prison_break_cool_down", "{=cGSXFJ3N}Because of a recent breakout attempt in this settlement it is on high alert. The guard won't even be seen talking to you.", null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("prison_break_cool_down", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_cancel_prison_break), true, -1, false, null);
			campaignGameStarter.AddGameMenu("settlement_prison_break_success", "{=TazumJGN}You emerge into the streets. No one is yet aware of what happened in the dungeons, and you hustle {PRISONER.NAME} towards the gates.{newline}You may now leave the {?SETTLEMENT_TYPE}settlement{?}castle{\\?}.", new OnInitDelegate(this.settlement_prison_break_success_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("settlement_prison_break_success", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.game_menu_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.settlement_prison_break_success_continue_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenu("settlement_prison_break_fail_player_unconscious", "{=svuD2vBo}You were knocked unconscious while trying to break {PRISONER.NAME} out of the dungeon.{newline}The guards caught you both and threw you in a cell.", new OnInitDelegate(this.settlement_prison_break_fail_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("settlement_prison_break_fail_player_unconscious", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.game_menu_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.settlement_prison_break_fail_player_unconscious_continue_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenu("settlement_prison_break_fail_prisoner_unconscious", "{=eKy1II3h}You made your way out but {PRISONER.NAME} was badly wounded during the escape. You had no choice but to leave {?PRISONER.GENDER}her{?}him{\\?} behind as you disappeared into the back streets and sneaked out the gate.{INFORMATION_IF_PRISONER_DEAD}", new OnInitDelegate(this.settlement_prison_break_fail_prisoner_injured_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("settlement_prison_break_fail_prisoner_unconscious", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.game_menu_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.settlement_prison_break_fail_prisoner_unconscious_continue_on_consequence), false, -1, false, null);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0004F0A4 File Offset: 0x0004D2A4
		private void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("prison_break_start_1", "start", "prison_break_end_already_met", "{=5RDF3aZN}{SALUTATION}... You came for me!", new ConversationSentence.OnConditionDelegate(this.prison_break_end_with_success_clan_member), null, 120, null);
			campaignGameStarter.AddDialogLine("prison_break_start_2", "start", "prison_break_end_already_met", "{=PRadDFN5}{SALUTATION}... Well, I hadn't expected this, but I'm very grateful.", new ConversationSentence.OnConditionDelegate(this.prison_break_end_with_success_player_already_met), null, 120, null);
			campaignGameStarter.AddDialogLine("prison_break_start_3", "start", "prison_break_end_meet", "{=zbPRul7h}Well.. I don't know you, but I'm very grateful.", new ConversationSentence.OnConditionDelegate(this.prison_break_end_with_success_other_on_condition), null, 120, null);
			campaignGameStarter.AddPlayerLine("prison_break_player_ask", "prison_break_end_already_met", "prison_break_next_move", "{=qFoMsPIf}I'm glad we made it out safe. What will you do now?", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("prison_break_player_meet", "prison_break_end_meet", "prison_break_next_move", "{=nMn63bV1}I am {PLAYER.NAME}. All I ask is that you remember that name, and what I did.{newline}Tell me, what will you do now?", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("prison_break_next_companion", "prison_break_next_move", "prison_break_next_move_player_companion", "{=aoJHP3Ud}I'm ready to rejoin you. I'm in your debt.", () => this._prisonerHero.CompanionOf == Clan.PlayerClan, null, 100, null);
			campaignGameStarter.AddDialogLine("prison_break_next_commander", "prison_break_next_move", "prison_break_next_move_player", "{=xADZi2bK}I'll go and find my men. I will remember your help...", () => this._prisonerHero.IsCommander, null, 100, null);
			campaignGameStarter.AddDialogLine("prison_break_next_noble", "prison_break_next_move", "prison_break_next_move_player", "{=W2vV5jzj}I'll go back to my family. I will remember your help...", () => this._prisonerHero.IsLord, null, 100, null);
			campaignGameStarter.AddDialogLine("prison_break_next_notable", "prison_break_next_move", "prison_break_next_move_player", "{=efdCZPw4}I'll go back to my work. I will remember your help...", () => this._prisonerHero.IsNotable, null, 100, null);
			campaignGameStarter.AddDialogLine("prison_break_next_other", "prison_break_next_move", "prison_break_next_move_player_other", "{=TWZ4abt5}I'll keep wandering about, as I've done before. I can make a living. No need to worry.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("prison_break_end_dialog_3", "prison_break_next_move_player_companion", "close_window", "{=ncvB4XRL}You could join me.", null, new ConversationSentence.OnConsequenceDelegate(this.prison_break_end_with_success_companion), 100, null, null);
			campaignGameStarter.AddPlayerLine("prison_break_end_dialog_1", "prison_break_next_move_player", "close_window", "{=rlAec9CM}Very well. Keep safe.", null, new ConversationSentence.OnConsequenceDelegate(this.prison_break_end_with_success_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("prison_break_end_dialog_2", "prison_break_next_move_player_other", "close_window", "{=dzXaXKaC}Very well.", null, new ConversationSentence.OnConsequenceDelegate(this.prison_break_end_with_success_on_consequence), 100, null, null);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0004F2C4 File Offset: 0x0004D4C4
		[GameMenuInitializationHandler("start_prison_break")]
		[GameMenuInitializationHandler("prison_break_cool_down")]
		[GameMenuInitializationHandler("settlement_prison_break_success")]
		[GameMenuInitializationHandler("settlement_prison_break_fail_player_unconscious")]
		[GameMenuInitializationHandler("settlement_prison_break_fail_prisoner_unconscious")]
		public static void game_menu_prison_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0004F2E0 File Offset: 0x0004D4E0
		private bool prison_break_end_with_success_clan_member()
		{
			bool flag = this._prisonerHero != null && this._prisonerHero.CharacterObject == CharacterObject.OneToOneConversationCharacter && (this._prisonerHero.CompanionOf == Clan.PlayerClan || this._prisonerHero.Clan == Clan.PlayerClan);
			if (flag)
			{
				MBTextManager.SetTextVariable("SALUTATION", Campaign.Current.ConversationManager.FindMatchingTextOrNull("str_salutation", CharacterObject.OneToOneConversationCharacter), false);
			}
			return flag;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0004F358 File Offset: 0x0004D558
		private bool prison_break_end_with_success_player_already_met()
		{
			bool flag = this._prisonerHero != null && this._prisonerHero.CharacterObject == CharacterObject.OneToOneConversationCharacter && this._prisonerHero.HasMet;
			if (flag)
			{
				MBTextManager.SetTextVariable("SALUTATION", Campaign.Current.ConversationManager.FindMatchingTextOrNull("str_salutation", CharacterObject.OneToOneConversationCharacter), false);
			}
			return flag;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0004F3B4 File Offset: 0x0004D5B4
		private bool prison_break_end_with_success_other_on_condition()
		{
			return this._prisonerHero != null && this._prisonerHero.CharacterObject == CharacterObject.OneToOneConversationCharacter;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0004F3D2 File Offset: 0x0004D5D2
		private void PrisonBreakEndedInternal()
		{
			ChangeRelationAction.ApplyPlayerRelation(this._prisonerHero, Campaign.Current.Models.PrisonBreakModel.GetRelationRewardOnPrisonBreak(this._prisonerHero), true, true);
			SkillLevelingManager.OnPrisonBreakEnd(this._prisonerHero, true);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0004F407 File Offset: 0x0004D607
		private void prison_break_end_with_success_on_consequence()
		{
			this.PrisonBreakEndedInternal();
			EndCaptivityAction.ApplyByEscape(this._prisonerHero, Hero.MainHero, true);
			this._prisonerHero = null;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0004F427 File Offset: 0x0004D627
		private void prison_break_end_with_success_companion()
		{
			this.PrisonBreakEndedInternal();
			EndCaptivityAction.ApplyByEscape(this._prisonerHero, Hero.MainHero, true);
			this._prisonerHero.ChangeState(Hero.CharacterStates.Active);
			AddHeroToPartyAction.Apply(this._prisonerHero, MobileParty.MainParty, true);
			this._prisonerHero = null;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0004F464 File Offset: 0x0004D664
		private bool game_menu_castle_prison_break_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			this._bribeCost = Campaign.Current.Models.PrisonBreakModel.GetPrisonBreakStartCost(this._prisonerHero);
			MBTextManager.SetTextVariable("COST", this._bribeCost);
			return true;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0004F4A0 File Offset: 0x0004D6A0
		private void AddCoolDownForPrisonBreak(Settlement settlement)
		{
			CampaignTime campaignTime = CampaignTime.DaysFromNow(7f);
			if (this._coolDownData.ContainsKey(settlement))
			{
				this._coolDownData[settlement] = campaignTime;
				return;
			}
			this._coolDownData.Add(settlement, campaignTime);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0004F4E4 File Offset: 0x0004D6E4
		private bool CanPlayerStartPrisonBreak(Settlement settlement)
		{
			bool flag = true;
			CampaignTime campaignTime;
			if (this._coolDownData.TryGetValue(settlement, out campaignTime))
			{
				flag = campaignTime.IsPast;
				if (flag)
				{
					this._coolDownData.Remove(settlement);
				}
			}
			return flag;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0004F51C File Offset: 0x0004D71C
		private bool game_menu_stage_prison_break_on_condition(MenuCallbackArgs args)
		{
			bool flag = false;
			if (Campaign.Current.Models.PrisonBreakModel.CanPlayerStagePrisonBreak(Settlement.CurrentSettlement))
			{
				args.optionLeaveType = GameMenuOption.LeaveType.StagePrisonBreak;
				if (Hero.MainHero.IsWounded)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=yNMrF2QF}You are wounded", null);
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0004F575 File Offset: 0x0004D775
		private void game_menu_castle_prison_break_from_dungeon_on_consequence(MenuCallbackArgs args)
		{
			this._previousMenuId = "town_keep_dungeon";
			this.game_menu_castle_prison_break_on_consequence(args);
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0004F589 File Offset: 0x0004D789
		private void game_menu_castle_prison_break_from_castle_dungeon_on_consequence(MenuCallbackArgs args)
		{
			this._previousMenuId = "castle_dungeon";
			this.game_menu_castle_prison_break_on_consequence(args);
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0004F59D File Offset: 0x0004D79D
		private void game_menu_castle_prison_break_from_enemy_keep_on_consequence(MenuCallbackArgs args)
		{
			this._previousMenuId = "town_enemy_town_keep";
			this.game_menu_castle_prison_break_on_consequence(args);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0004F5B1 File Offset: 0x0004D7B1
		private void game_menu_castle_prison_break_from_castle_enemy_keep_on_consequence(MenuCallbackArgs args)
		{
			this._previousMenuId = "castle_enemy_keep";
			this.game_menu_castle_prison_break_on_consequence(args);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0004F5C8 File Offset: 0x0004D7C8
		private void game_menu_castle_prison_break_on_consequence(MenuCallbackArgs args)
		{
			if (this.CanPlayerStartPrisonBreak(Settlement.CurrentSettlement))
			{
				FlattenedTroopRoster flattenedTroopRoster = Settlement.CurrentSettlement.Party.PrisonRoster.ToFlattenedRoster();
				if (Settlement.CurrentSettlement.Town.GarrisonParty != null)
				{
					flattenedTroopRoster.Add(Settlement.CurrentSettlement.Town.GarrisonParty.PrisonRoster.GetTroopRoster());
				}
				flattenedTroopRoster.RemoveIf((FlattenedTroopRosterElement x) => !x.Troop.IsHero);
				List<InquiryElement> list = new List<InquiryElement>();
				foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in flattenedTroopRoster)
				{
					TextObject textObject;
					TextObject textObject2;
					bool flag;
					if (FactionManager.IsAtWarAgainstFaction(Clan.PlayerClan.MapFaction, flattenedTroopRosterElement.Troop.HeroObject.MapFaction))
					{
						textObject = new TextObject("{=!}{HERO.NAME}", null);
						StringHelpers.SetCharacterProperties("HERO", flattenedTroopRosterElement.Troop, textObject, false);
						textObject2 = new TextObject("{=VM1SGrla}{HERO.NAME} is your enemy.", null);
						textObject2.SetCharacterProperties("HERO", flattenedTroopRosterElement.Troop, false);
						flag = true;
					}
					else
					{
						int prisonBreakStartCost = Campaign.Current.Models.PrisonBreakModel.GetPrisonBreakStartCost(flattenedTroopRosterElement.Troop.HeroObject);
						flag = Hero.MainHero.Gold < prisonBreakStartCost;
						textObject = new TextObject("{=!}{HERO.NAME}", null);
						StringHelpers.SetCharacterProperties("HERO", flattenedTroopRosterElement.Troop, textObject, false);
						textObject2 = new TextObject("{=I4SjNT6Y}This will cost you {BRIBE_COST}{GOLD_ICON}.{?ENOUGH_GOLD}{?} You don't have enough money.{\\?}", null);
						textObject2.SetTextVariable("BRIBE_COST", prisonBreakStartCost);
						textObject2.SetTextVariable("ENOUGH_GOLD", flag ? 0 : 1);
					}
					list.Add(new InquiryElement(flattenedTroopRosterElement.Troop, textObject.ToString(), new CharacterImageIdentifier(CharacterCode.CreateFrom(flattenedTroopRosterElement.Troop)), !flag, textObject2.ToString()));
				}
				MBInformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=oQjsShmH}PRISONERS", null).ToString(), new TextObject("{=abpzOR0D}Choose a prisoner to break out", null).ToString(), list, true, 1, 1, GameTexts.FindText("str_done", null).ToString(), string.Empty, new Action<List<InquiryElement>>(this.StartPrisonBreak), null, "", false), false, false);
				return;
			}
			GameMenu.SwitchToMenu("prison_break_cool_down");
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0004F830 File Offset: 0x0004DA30
		private void StartPrisonBreak(List<InquiryElement> prisonerList)
		{
			if (prisonerList.Count > 0)
			{
				this._prisonerHero = ((CharacterObject)prisonerList[0].Identifier).HeroObject;
				GameMenu.SwitchToMenu("start_prison_break");
				return;
			}
			this._prisonerHero = null;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0004F86C File Offset: 0x0004DA6C
		private void OpenPrisonBreakMission()
		{
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, this._bribeCost, false);
			this.AddCoolDownForPrisonBreak(Settlement.CurrentSettlement);
			this._launchingPrisonBreakMission = true;
			Location locationWithId = LocationComplex.Current.GetLocationWithId("prison");
			CampaignMission.OpenPrisonBreakMission(locationWithId.GetSceneName(Settlement.CurrentSettlement.Town.GetWallLevel()), locationWithId, this._prisonerHero.CharacterObject);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0004F8D4 File Offset: 0x0004DAD4
		private bool game_menu_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0004F8DF File Offset: 0x0004DADF
		private bool game_menu_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0004F8EA File Offset: 0x0004DAEA
		private void game_menu_cancel_prison_break(MenuCallbackArgs args)
		{
			this._prisonerHero = null;
			GameMenu.SwitchToMenu(this._previousMenuId);
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0004F8FE File Offset: 0x0004DAFE
		private void start_prison_break_on_init(MenuCallbackArgs args)
		{
			StringHelpers.SetCharacterProperties("PRISONER", this._prisonerHero.CharacterObject, null, false);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0004F918 File Offset: 0x0004DB18
		private void settlement_prison_break_success_on_init(MenuCallbackArgs args)
		{
			StringHelpers.SetCharacterProperties("PRISONER", this._prisonerHero.CharacterObject, null, false);
			MBTextManager.SetTextVariable("SETTLEMENT_TYPE", Settlement.CurrentSettlement.IsTown ? 1 : 0);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0004F94C File Offset: 0x0004DB4C
		private void settlement_prison_break_success_continue_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
			CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(this._prisonerHero.CharacterObject, null, false, false, false, false, false, false));
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0004F994 File Offset: 0x0004DB94
		private void settlement_prison_break_fail_prisoner_injured_on_init(MenuCallbackArgs args)
		{
			if (this._prisonerHero.IsDead)
			{
				TextObject textObject = new TextObject("{=GkwOyJn9}{newline}You later learn that {?PRISONER.GENDER}she{?}he{\\?} died from {?PRISONER.GENDER}her{?}his{\\?} injuries.", null);
				StringHelpers.SetCharacterProperties("PRISONER", this._prisonerHero.CharacterObject, textObject, false);
				MBTextManager.SetTextVariable("INFORMATION_IF_PRISONER_DEAD", textObject, false);
			}
			StringHelpers.SetCharacterProperties("PRISONER", this._prisonerHero.CharacterObject, null, false);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0004F9F6 File Offset: 0x0004DBF6
		private void settlement_prison_break_fail_on_init(MenuCallbackArgs args)
		{
			StringHelpers.SetCharacterProperties("PRISONER", this._prisonerHero.CharacterObject, null, false);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0004FA10 File Offset: 0x0004DC10
		private void settlement_prison_break_fail_player_unconscious_continue_on_consequence(MenuCallbackArgs args)
		{
			SkillLevelingManager.OnPrisonBreakEnd(this._prisonerHero, false);
			Settlement currentSettlement = Settlement.CurrentSettlement;
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
			TakePrisonerAction.Apply(currentSettlement.Party, Hero.MainHero);
			this._prisonerHero = null;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0004FA44 File Offset: 0x0004DC44
		private void settlement_prison_break_fail_prisoner_unconscious_continue_on_consequence(MenuCallbackArgs args)
		{
			SkillLevelingManager.OnPrisonBreakEnd(this._prisonerHero, false);
			this._prisonerHero = null;
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
		}

		// Token: 0x040004A3 RID: 1187
		private const int CoolDownInDays = 7;

		// Token: 0x040004A4 RID: 1188
		private const int PrisonBreakDialogPriority = 120;

		// Token: 0x040004A5 RID: 1189
		private const string DefaultPrisonGuardWeaponId = "battania_mace_1_t2";

		// Token: 0x040004A6 RID: 1190
		private Dictionary<Settlement, CampaignTime> _coolDownData = new Dictionary<Settlement, CampaignTime>();

		// Token: 0x040004A7 RID: 1191
		private Hero _prisonerHero;

		// Token: 0x040004A8 RID: 1192
		private bool _launchingPrisonBreakMission;

		// Token: 0x040004A9 RID: 1193
		private int _bribeCost;

		// Token: 0x040004AA RID: 1194
		private string _previousMenuId;
	}
}
