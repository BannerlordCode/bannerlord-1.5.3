using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000E6 RID: 230
	public class TavernEmployeesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00051AAB File Offset: 0x0004FCAB
		private float MaxTownDistanceAsDays
		{
			get
			{
				return Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All) * 3f / (Campaign.Current.EstimatedAverageVillagerPartySpeed * (float)CampaignTime.HoursInDay);
			}
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00051AD0 File Offset: 0x0004FCD0
		public override void RegisterEvents()
		{
			CampaignEvents.OnMissionStartedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionStarted));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00051B50 File Offset: 0x0004FD50
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Settlement>("_orderedDrinkThisDayInSettlement", ref this._orderedDrinkThisDayInSettlement);
			dataStore.SyncData<bool>("_orderedDrinkThisVisit", ref this._orderedDrinkThisVisit);
			dataStore.SyncData<bool>("_hasMetWithRansomBroker", ref this._hasMetWithRansomBroker);
			dataStore.SyncData<bool>("_hasBoughtTunToParty", ref this._hasBoughtTunToParty);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00051BA5 File Offset: 0x0004FDA5
		public void DailyTick()
		{
			this._orderedDrinkThisDayInSettlement = null;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00051BAE File Offset: 0x0004FDAE
		public void WeeklyTick()
		{
			this._hasBoughtTunToParty = false;
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00051BB7 File Offset: 0x0004FDB7
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
			this._inquiryVariationIndex = MBRandom.NondeterministicRandomInt % 6;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00051BD0 File Offset: 0x0004FDD0
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			if (settlement.IsTown && CampaignMission.Current != null)
			{
				Location location = CampaignMission.Current.Location;
				if (location != null && location.StringId == "tavern")
				{
					int num;
					if (unusedUsablePointCount.TryGetValue("spawnpoint_tavernkeeper", out num) && num > 0)
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateTavernkeeper), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
					}
					if (unusedUsablePointCount.TryGetValue("sp_tavern_wench", out num) && num > 0)
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateTavernWench), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
					}
					if (unusedUsablePointCount.TryGetValue("musician", out num) && num > 0)
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateMusician), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, num);
					}
					location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateRansomBroker), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
					return;
				}
				if (location != null && location.StringId == "center" && !Campaign.Current.IsNight)
				{
					int num2;
					if (unusedUsablePointCount.TryGetValue("spawnpoint_tavernkeeper", out num2))
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateTavernkeeper), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
					}
					if (unusedUsablePointCount.TryGetValue("sp_tavern_wench", out num2))
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateTavernWench), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
					}
					if (unusedUsablePointCount.TryGetValue("musician", out num2))
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateMusician), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, num2);
					}
				}
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00051D67 File Offset: 0x0004FF67
		public void OnMissionStarted(IMission mission)
		{
			this._orderedDrinkThisVisit = false;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00051D70 File Offset: 0x0004FF70
		private static LocationCharacter CreateTavernWench(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject tavernWench = culture.TavernWench;
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(tavernWench, out num, out num2, "");
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(tavernWench.Race, "_settlement");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(tavernWench, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "sp_tavern_wench", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_barmaid"), true, false, null, false, false, true, null, false)
			{
				PrefabNamesForBones = { 
				{
					agentData.AgentMonster.OffHandItemBoneIndex,
					"kitchen_pitcher_b_tavern"
				} }
			};
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00051E40 File Offset: 0x00050040
		private static LocationCharacter CreateTavernkeeper(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject tavernkeeper = culture.Tavernkeeper;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(tavernkeeper.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(tavernkeeper, out num, out num2, "");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(tavernkeeper, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "spawnpoint_tavernkeeper", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_tavern_keeper"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00051EF4 File Offset: 0x000500F4
		private static LocationCharacter CreateMusician(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject musician = culture.Musician;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(musician.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(musician, out num, out num2, "");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(musician, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "musician", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_musician"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00051FA8 File Offset: 0x000501A8
		private static LocationCharacter CreateRansomBroker(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject ransomBroker = culture.RansomBroker;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(ransomBroker.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(ransomBroker, out num, out num2, "");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(ransomBroker, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "npc_common", true, relation, null, true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00052040 File Offset: 0x00050240
		protected void AddDialogs(CampaignGameStarter cgs)
		{
			cgs.AddDialogLine("talk_common_to_tavernkeeper", "start", "tavernkeeper_talk", "{=QCuxL92I}Good day, {?PLAYER.GENDER}madam{?}sir{\\?}. How can I help you?", () => CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Tavernkeeper, null, 100, null);
			cgs.AddPlayerLine("tavernkeeper_talk_to_get_quest", "tavernkeeper_talk", "tavernkeeper_ask_quests", "{=A61ppTa6}Do you know of anyone who might have a task for someone like me?", null, null, 100, null, null);
			cgs.AddPlayerLine("tavernkeeper_get_clan_info_start", "tavernkeeper_talk", "tavernkeeper_offer_clan_info", "{=shXdvd5p}I'm looking for information about the owner of this town.", new ConversationSentence.OnConditionDelegate(this.player_ask_information_about_the_owner_of_the_town_condition), null, 100, null, null);
			cgs.AddDialogLine("tavernkeeper_get_clan_info_answer", "tavernkeeper_offer_clan_info", "player_offer_clan_info", "{=i96KTeph}I can sell you information about {OWNER_CLAN}, who are the owners of our town {SETTLEMENT} for {PRICE}{GOLD_ICON}.", new ConversationSentence.OnConditionDelegate(this.tavernkeeper_offer_clan_info_on_condition), null, 100, null);
			cgs.AddPlayerLine("tavernkeeper_get_clan_info_player_answer_1", "player_offer_clan_info", "tavernkeeper_pretalk", "{=VaxbQby7}That sounds like a great deal.", null, new ConversationSentence.OnConsequenceDelegate(this.player_accepts_clan_info_offer_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.player_accepts_clan_info_offer_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_get_clan_info_player_answer_2", "player_offer_clan_info", "tavernkeeper_pretalk", "{=CH7b5LaX}I have changed my mind.", null, null, 100, null, null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_start", "tavernkeeper_talk", "tavernkeeper_companion_info_tavernkeeper_answer", "{=JPiId1fb}I am looking for some people to hire with specific skills. Would you happen to know anyone looking for work in the towns nearby? ", new ConversationSentence.OnConditionDelegate(this.tavernkeeper_talk_companion_on_condition), null, 100, null, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_answer", "tavernkeeper_companion_info_tavernkeeper_answer", "tavernkeeper_list_companion_types", "{=ASVkuYHG}I know a few. What kind of a person are you looking for?", null, null, 100, null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_scout", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=joCSXAQQ}I would travel much faster if I had a good scout by my side.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Scout);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_engineer", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=NGcKLV88}A good engineer could help construct siege engines and new buildings in towns.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Engineer);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_surgeon", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=Y5ztM8zq}My men would feel safer with a good surgeon to aid them in their time of need.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Surgeon);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_quartermaster", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=88Fk9keT}I am sure I can do more with fewer supplies if I had a good quartermaster.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Quartermaster);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_caravan_leader", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=kePH44eg}I am planning to sponsor my own caravans, and someone who could run them would be perfect.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.CaravanLeader);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_leader", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=9z0Yz8za}I need a lieutenant to be my right hand and help me direct my troops in battle.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Leader);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_roguery", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=DMUsPekF}I need someone who knows a bit about the darker side of the world, who can handle villains and thieves.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Roguery);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_nevermind", "tavernkeeper_list_companion_types", "tavernkeeper_pretalk", "{=tdvnKIyS}Never mind.", null, null, 100, null, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_not_found_answer_1", "player_selected_companion_type", "tavernkeeper_talk_companion_end", "{=!}{CANNOT_THINK_OF_ANYONE_LINE}[rb:negative]", () => !this.FoundCompanion(), new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_tavernkeeper_end", "tavernkeeper_talk_companion_end", "start", "{=PDf52VCf}Thank you for your time anyway.", null, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_inside_1", "player_selected_companion_type", "player_companion_response", "{=QdEGu0CW}A {?INQUIRY_COMPANION.GENDER}woman{?}man{\\?} called {INQUIRY_COMPANION.LINK} has passed through here on his way to {COMPANION_SETTLEMENT}. You may catch up to {?INQUIRY_COMPANION.GENDER}her{?}him{\\?} if you hurry.[rb:positive]", () => this._inquiryCurrentCompanion.CurrentSettlement != Settlement.CurrentSettlement && this._inquiryVariationIndex % 3 == 0, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_inside_2", "player_selected_companion_type", "player_companion_response", "{=WSAS3XLC}There was someone here not long ago, went by the name of {INQUIRY_COMPANION.LINK}. {?INQUIRY_COMPANION.GENDER}She{?}He{\\?} left for {COMPANION_SETTLEMENT}. Perhaps you can find them there, or on the road.[rb:positive]", () => this._inquiryCurrentCompanion.CurrentSettlement != Settlement.CurrentSettlement && this._inquiryVariationIndex % 3 == 1, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_inside_3", "player_selected_companion_type", "player_companion_response", "{=ahydRFKe}There was someone who called {?INQUIRY_COMPANION.GENDER}herself{?}himself{\\?} {INQUIRY_COMPANION.LINK}. Sounds like the kind of person who might interest you, no? {?INQUIRY_COMPANION.GENDER}She{?}He{\\?} was headed for {COMPANION_SETTLEMENT}.[rb:positive]", () => this._inquiryCurrentCompanion.CurrentSettlement != Settlement.CurrentSettlement && this._inquiryVariationIndex % 3 == 2, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_outside_1", "player_selected_companion_type", "player_companion_response", "{=gfmU4wiM}A {?INQUIRY_COMPANION.GENDER}woman{?}man{\\?} named {INQUIRY_COMPANION.LINK} is staying at my tavern. You may want to talk with him.", () => this._inquiryVariationIndex % 3 == 0, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_outside_2", "player_selected_companion_type", "player_companion_response", "{=qSy4Ns1N}There's someone who might meet your needs who is in here having a drink right now. Goes by the name of {INQUIRY_COMPANION.LINK}.", () => this._inquiryVariationIndex % 3 == 1, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_outside_3", "player_selected_companion_type", "player_companion_response", "{=rLcRwkqK}You might want to look around here for a {?INQUIRY_COMPANION.GENDER}lady{?}fellow{\\?} named {INQUIRY_COMPANION.LINK}. I believe {?INQUIRY_COMPANION.GENDER}she{?}he{\\?} might possess that kind of skill.", () => this._inquiryVariationIndex % 3 == 2, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddPlayerLine("player_companion_response_to_tavernkeeper", "player_companion_response", "tavernkeeper_companion_info_tavernkeeper_answer", "{=SqrRaIU5}I would like to ask about someone with different expertise.", new ConversationSentence.OnConditionDelegate(this.FoundCompanion), null, 100, null, null);
			cgs.AddPlayerLine("player_companion_response_to_tavernkeeper_2", "player_companion_response", "player_selected_companion_type", "{=vx4ML2gX}Is there someone else other than {INQUIRY_COMPANION.LINK}?", new ConversationSentence.OnConditionDelegate(this.FoundCompanion), delegate
			{
				this.FindCompanionWithType(this._selectedCompanionType);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("player_companion_response_to_tavernkeeper_found", "player_companion_response", "start", "{=3FzBTb3w}Thank you for this information. It was {COMPANION_INQUIRY_COST}{GOLD_ICON} well spent.", new ConversationSentence.OnConditionDelegate(this.FoundCompanion), null, 100, null, null);
			cgs.AddPlayerLine("player_companion_response_to_tavernkeeper_not_found", "player_companion_response", "start", "{=PDf52VCf}Thank you for your time anyway.", () => !this.FoundCompanion(), null, 100, null, null);
			cgs.AddPlayerLine("tavernkeeper_talk_to_leave", "tavernkeeper_talk", "close_window", "{=fF2BdOy9}I don't need anything now.", null, delegate
			{
				this._previouslyRecommendedCompanions.Clear();
			}, 100, null, null);
			cgs.AddDialogLine("1972", "tavernkeeper_pretalk", "tavernkeeper_talk", "{=ds294zxi}Anything else?", null, null, 100, null);
			cgs.AddDialogLine("talk_common_to_tavernmaid", "start", "tavernmaid_talk", "{=ddYWbO8b}The usual?", new ConversationSentence.OnConditionDelegate(this.conversation_tavernmaid_offers_usual_on_condition), null, 100, null);
			cgs.AddDialogLine("talk_common_to_tavernmaid_2", "start", "tavernmaid_talk", "{=x7k87vj3}What can I bring you, {?PLAYER.GENDER}madam{?}sir{\\?}? Would you like to taste our local speciality, {DRINK} with {FOOD}?", new ConversationSentence.OnConditionDelegate(this.conversation_tavernmaid_offers_food_on_condition), null, 100, null);
			cgs.AddDialogLine("talk_common_to_tavernmaid_3", "start", "tavernmaid_talk", "{=Tn9g83ry}Enjoying your drink, {?PLAYER.GENDER}madam{?}sir{\\?}?", new ConversationSentence.OnConditionDelegate(this.conversation_tavernmaid_gossips_on_condition), null, 100, null);
			cgs.AddPlayerLine("tavernmaid_order_food", "tavernmaid_talk", "tavernmaid_order_acknowledge", "{=E57VFXqU}I'll have that.", new ConversationSentence.OnConditionDelegate(this.conversation_player_can_order_food_on_condition), null, 100, null, null);
			cgs.AddDialogLine("tavernmain_order_acknowledge", "tavernmaid_order_acknowledge", "close_window", "{=3wb2dCfz}It'll be right up then, {?PLAYER.GENDER}ma'am{?}sir{\\?}.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_tavernmaid_delivers_food_on_consequence), 100, null);
			cgs.AddPlayerLine("tavernmaid_ask_tun", "tavernmaid_talk", "tavern_drink_morale_to_party", "{=oAaKaXEy}I really like this meal. I'd like it served to all my men.", new ConversationSentence.OnConditionDelegate(this.conversation_tavernmaid_buy_tun_on_condition), null, 100, null, null);
			cgs.AddPlayerLine("tavernmaid_leave", "tavernmaid_talk", "close_window", "{=Piq3oYmG}I'm fine, thank you.", null, null, 100, null, null);
			cgs.AddDialogLine("tavernmaid_give_tun_gold", "tavern_drink_morale_to_party", "tun_give_gold", "{=bjNuUqTx}With pleasure, {?PLAYER.GENDER}madam{?}sir{\\?}. That will cost you {COST}{GOLD_ICON}.", new ConversationSentence.OnConditionDelegate(TavernEmployeesCampaignBehavior.calculate_tun_cost_on_condition), null, 100, null);
			cgs.AddPlayerLine("tavernmaid_give_tun_gold_2", "tun_give_gold", "tavernmaid_enjoy", "{=nAM821Fb}Here you are.", null, new ConversationSentence.OnConsequenceDelegate(this.can_buy_tun_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.can_buy_tun_on_clickable_condition), null);
			cgs.AddPlayerLine("tavernmaid_not_give_tun_gold", "tun_give_gold", "start", "{=2PEKd3Sz}Actually, I changed my mind. Cancel the order...", null, null, 100, null, null);
			cgs.AddDialogLine("tavernmaid_best_wishes", "tavernmaid_enjoy", "close_window", "{=ZGMfmNe0}Very generous of you, my {?PLAYER.GENDER}lady{?}lord{\\?}. Good health and fortune to all of you.", null, null, 100, null);
			cgs.AddDialogLine("talk_bard", "start", "talk_bard_player", "{=!}{LYRIC_SCRAP}", new ConversationSentence.OnConditionDelegate(this.conversation_talk_bard_on_condition), null, 100, null);
			cgs.AddPlayerLine("talk_bard_player_leave", "talk_bard_player", "close_window", "{=sbczu2VI}Play on, good man.", null, null, 100, null, null);
			cgs.AddDialogLine("tavernkeeper_quest_info", "tavernkeeper_ask_quests", "tavernkeeper_has_quest", "{=uUkCLZEo}Let's see... {ISSUE_GIVER_LIST}.", new ConversationSentence.OnConditionDelegate(TavernEmployeesCampaignBehavior.tavenkeeper_has_quest_on_condition), null, 100, null);
			cgs.AddPlayerLine("tavernkeeper_player_thanks", "tavernkeeper_has_quest", "tavernkeeper_pretalk", "{=eALf5d30}Thanks!", null, null, 100, null, null);
			cgs.AddDialogLine("tavernkeeper_turndown", "tavernkeeper_doesnot_have_quests", "tavernkeeper_talk", "{=py6Y46sa}No, I didn't hear any...", null, null, 100, null);
			this.AddRansomBrokerDialogs(cgs);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00052888 File Offset: 0x00050A88
		private bool player_ask_information_about_the_owner_of_the_town_condition()
		{
			return Settlement.CurrentSettlement.OwnerClan != Clan.PlayerClan;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x000528A0 File Offset: 0x00050AA0
		private void player_accepts_clan_info_offer_on_consequence()
		{
			foreach (Hero hero in Settlement.CurrentSettlement.OwnerClan.Heroes)
			{
				hero.IsKnownToPlayer = true;
			}
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, 500, false);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0005290C File Offset: 0x00050B0C
		private bool player_accepts_clan_info_offer_clickable_condition(out TextObject explanation)
		{
			bool flag = true;
			using (List<Hero>.Enumerator enumerator = Settlement.CurrentSettlement.OwnerClan.Heroes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsKnownToPlayer)
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				explanation = new TextObject("{=LBiZ9Rie}You already possess this information.", null);
				return false;
			}
			explanation = new TextObject("{=!}{INFORMATION_COST}{GOLD_ICON}.", null);
			MBTextManager.SetTextVariable("INFORMATION_COST", 500);
			if (Hero.MainHero.Gold < 500)
			{
				explanation = new TextObject("{=xVZVYNan}You don't have enough{GOLD_ICON}.", null);
				return false;
			}
			return true;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x000529BC File Offset: 0x00050BBC
		private bool tavernkeeper_offer_clan_info_on_condition()
		{
			MBTextManager.SetTextVariable("OWNER_CLAN", Settlement.CurrentSettlement.OwnerClan.EncyclopediaLinkWithName, false);
			MBTextManager.SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.EncyclopediaLinkWithName, false);
			MBTextManager.SetTextVariable("PRICE", 500);
			return true;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00052A08 File Offset: 0x00050C08
		private void SetCannotThinkOfAnyoneLine(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType type)
		{
			TextObject textObject;
			if (this._previouslyRecommendedCompanions.ContainsKey(type))
			{
				textObject = new TextObject("{=eeIX6loR}I can't think of anyone else.", null);
			}
			else
			{
				textObject = ((this._inquiryVariationIndex % 2 == 0) ? new TextObject("{=BYpoxUEB}I haven't heard of someone with such skills looking for work for a while now.", null) : new TextObject("{=SbYlYGFA}Sorry. No one like that has passed through here for a while.", null));
			}
			MBTextManager.SetTextVariable("CANNOT_THINK_OF_ANYONE_LINE", textObject, false);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00052A60 File Offset: 0x00050C60
		private void IncreaseVariationIndex()
		{
			this._inquiryVariationIndex++;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00052A70 File Offset: 0x00050C70
		private bool FoundCompanion()
		{
			return this._inquiryCurrentCompanion != null;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00052A7B File Offset: 0x00050C7B
		public void FindCompanionWithType(PartyRole role)
		{
			if (role == PartyRole.FirstMate)
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.FirstMate);
				return;
			}
			if (role == PartyRole.Navigator)
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Navigator);
				return;
			}
			Debug.FailedAssert("Wrong function call! This only should be called for First Mate and Navigator.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\CampaignBehaviors\\TavernEmployeesCampaignBehavior.cs", "FindCompanionWithType", 355);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00052AB0 File Offset: 0x00050CB0
		private void FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType companionType)
		{
			int num = 50;
			Hero hero = null;
			float num2 = this.MaxTownDistanceAsDays * Campaign.Current.EstimatedAverageVillagerPartySpeed * (float)CampaignTime.HoursInDay;
			foreach (Town town in Town.AllTowns)
			{
				if (town.Settlement.HeroesWithoutParty.Count > 0 && Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, Settlement.CurrentSettlement, false, false, (Settlement.CurrentSettlement.HasPort && town.Settlement.HasPort) ? MobileParty.NavigationType.All : MobileParty.NavigationType.Default) < num2)
				{
					foreach (Hero hero2 in town.Settlement.HeroesWithoutParty)
					{
						int num3 = 0;
						List<Hero> list;
						this._previouslyRecommendedCompanions.TryGetValue(companionType, out list);
						if (hero2.IsWanderer && (list == null || !list.Contains(hero2)))
						{
							switch (companionType)
							{
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Scout:
								num3 = hero2.GetSkillValue(Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(PartyRole.Scout));
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Engineer:
								num3 = hero2.GetSkillValue(Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(PartyRole.Engineer));
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Surgeon:
								num3 = hero2.GetSkillValue(Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(PartyRole.Surgeon));
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Quartermaster:
								num3 = hero2.GetSkillValue(Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(PartyRole.Quartermaster));
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.FirstMate:
								num3 = hero2.GetSkillValue(Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(PartyRole.FirstMate));
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Navigator:
								num3 = hero2.GetSkillValue(Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(PartyRole.Navigator));
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.CaravanLeader:
								num3 += hero2.GetSkillValue(DefaultSkills.Trade);
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Leader:
								num3 = hero2.GetSkillValue(DefaultSkills.Leadership);
								num3 += hero2.GetSkillValue(DefaultSkills.Tactics);
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Roguery:
								num3 = hero2.GetSkillValue(DefaultSkills.Roguery);
								break;
							}
						}
						if (num3 > num)
						{
							num = num3;
							hero = hero2;
						}
					}
				}
			}
			this._inquiryCurrentCompanion = hero;
			this._selectedCompanionType = companionType;
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, 2, false);
			if (this._inquiryCurrentCompanion != null)
			{
				StringHelpers.SetCharacterProperties("INQUIRY_COMPANION", this._inquiryCurrentCompanion.CharacterObject, null, false);
				MBTextManager.SetTextVariable("COMPANION_SETTLEMENT", this._inquiryCurrentCompanion.CurrentSettlement.EncyclopediaLinkWithName, false);
				this._inquiryCurrentCompanion.IsKnownToPlayer = true;
				if (this._previouslyRecommendedCompanions.ContainsKey(this._selectedCompanionType))
				{
					this._previouslyRecommendedCompanions[this._selectedCompanionType].Add(this._inquiryCurrentCompanion);
				}
				else
				{
					this._previouslyRecommendedCompanions.Add(this._selectedCompanionType, new List<Hero> { this._inquiryCurrentCompanion });
				}
			}
			this.SetCannotThinkOfAnyoneLine(companionType);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00052E1C File Offset: 0x0005101C
		private bool conversation_talk_bard_on_condition()
		{
			if (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Musician)
			{
				List<string> list = new List<string>();
				Settlement randomElementWithPredicate = Settlement.All.GetRandomElementWithPredicate<Settlement>((Settlement x) => x.IsTown && x.Culture == Settlement.CurrentSettlement.Culture && x != Settlement.CurrentSettlement);
				MBTextManager.SetTextVariable("RANDOM_TOWN", randomElementWithPredicate.Name, false);
				list.Add("{=3n1KRLpZ}'My love is far {newline} I know not where {newline} Perhaps the winds shall tell me'");
				list.Add("{=NQOQb0C9}'And many thousand bodies lay a-rotting in the sun {newline} But things like that must be you know for kingdoms to be won'");
				list.Add("{=bs8ayCGX}'A warrior brave you might surely be {newline} With your blade and your helm and your bold fiery steed {newline} But I'll give you a warning you'd be wise to heed {newline} Don't toy with the fishwives of {RANDOM_TOWN}'");
				list.Add("{=3n1KRLpZ}'My love is far {newline} I know not where {newline} Perhaps the winds shall tell me'");
				list.Add("{=YequZz6U}'Oh the maidens of {RANDOM_TOWN} are merry and fair {newline} Plotting their mischief with flowers in their hair {newline} Were I still a young man I sure would be there {newline} But now I'll take warmth over trouble'");
				list.Add("{=CM8Tr3lL}'Oh my pocket's been picked {newline} And my shirt's drenched with sick {newline} And my head feels like it's come a fit to bursting'");
				list.Add("{=DFkzQHRQ}'For all the silks of the Padishah {newline} For all the Emperor's gold {newline} For all the spice in the distance East...'");
				list.Add("{=2fbLBXtT}'O'er the whale-road she sped {newline} She were manned by the dead  {newline} And the clouds followed black in her wake'");
				string text = list[MBRandom.RandomInt(0, list.Count)];
				MBTextManager.SetTextVariable("LYRIC_SCRAP", new TextObject(text, null), false);
				return true;
			}
			return false;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00052EFB File Offset: 0x000510FB
		private static bool companion_type_select_clickable_condition(out TextObject explanation)
		{
			explanation = new TextObject("{=!}{COMPANION_INQUIRY_COST}{GOLD_ICON}.", null);
			MBTextManager.SetTextVariable("COMPANION_INQUIRY_COST", 2);
			if (Hero.MainHero.Gold < 2)
			{
				explanation = new TextObject("{=xVZVYNan}You don't have enough{GOLD_ICON}.", null);
				return false;
			}
			return true;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00052F34 File Offset: 0x00051134
		private void AddRansomBrokerDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("ransom_broker_start", "start", "ransom_broker_intro", "{=!}{RANSOM_BROKER_INTRO}", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_start_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_intro", "ransom_broker_intro", "ransom_broker_2", "{=TGYJUUn0}Go on.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("ransom_broker_intro_2_di", "ransom_broker_2", "ransom_broker_3", "{=MFDb5duu}Splendid! I suspect that you may, in your line of work, occasionally acquire a few captives. I could possibly take them off your hands. I'd pay you, of course.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_intro_2", "ransom_broker_3", "ransom_broker_4", "{=bPqwLopK}Mm. Are you a slaver?", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("ransom_broker_intro_3", "ransom_broker_4", "ransom_broker_5", "{=YCC0hPuC}Ah, no. Slavers are rare these days. It used to be that, when the Empire and its neighbors made war upon each other, the defeated were usually taken as slaves. That helped pay for the war, you see! But today, for better or for worse, that is not practical. What with the frontiers broken and raiders crossing this way and that, there are far too many opportunities for captives to escape. But that does not mean that war cannot be profitable! Indeed it still can!", null, null, 100, null);
			campaignGameStarter.AddDialogLine("ransom_broker_intro_4", "ransom_broker_5", "ransom_broker_6", "{=8bfzW0np}Many captives are still taken, and their families will pay to have them back. Men such as I criss-cross the Empire and the outer kingdoms, acquiring prisoners, and contacting their kin for a suitable ransom.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("ransom_broker_intro_5", "ransom_broker_6", "ransom_broker_info_talk", "{=rLlIVqmY}So, were you to acquire a few prisoners in one of your victorious affrays, and wish to relieve yourself of their care and feeding, I or one of my colleagues would be happy to pay for them as a sort of speculative investment.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_info_talk_player_1", "ransom_broker_info_talk", "ransom_broker_families", "{=QHoCsSZX}What if their families can't pay?", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_info_talk_player_2", "ransom_broker_info_talk", "ransom_broker_prices", "{=btA10FML}What can I get for a prisoner?", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_info_talk_player_3", "ransom_broker_info_talk", "ransom_broker_ransom_me", "{=nwJPPIvn}Would you be able to ransom me if I were taken?", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_info_talk_player_4", "ransom_broker_info_talk", "ransom_broker_pretalk", "{=TyultuzK}That's all I need to know. Thank you.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("ransom_broker_families", "ransom_broker_families", "ransom_broker_info_talk", "{=zxonBgY2}Oh, I suppose I could sell them to the republics of Geroia, to row their galleys, although even in Geroia they prefer free oarsmen these days... But it rarely comes to that. You'd be surprised what sorts of treasures a peasant can dig out of his cowshed or wheedle out of his cousins!", null, null, 100, null);
			campaignGameStarter.AddDialogLine("ransom_broker_prices", "ransom_broker_prices", "ransom_broker_info_talk", "{=PLbxHyPu}It varies. I fancy that I have a fine eye for assessing a ransom. There are a dozen little things about a man that will tell you whether he goes to bed hungry, or spices his meat with pepper and cloves from the east. The real money of course is in the aristocracy, and if you ever want to do my job you'll want to learn about every landowning family or tribal chief in Calradia, their estates, their offspring both lawful and bastard, and, of course, their credit with the merchants.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("ransom_broker_ransom_me", "ransom_broker_ransom_me", "ransom_broker_info_talk", "{=4tY23HWb}Of course. I'm welcome in every court in Calradia. There's not many who can say that! So always be sure to keep a pot of denars buried somewhere, and a loyal servant who can find it in a hurry.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("ransom_broker_start_has_met", "start", "ransom_broker_talk", "{=EIdFCZrx}Greetings. If you have any more business for me, please let me know.", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_start_has_met_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("ransom_broker_pretalk", "ransom_broker_pretalk", "ransom_broker_talk", "{=xYdOZblK}Anyway, if you have any more business for me, please let me know.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_talk_player_1", "ransom_broker_talk", "ransom_broker_sell_prisoners", "{=cAVxYAdw}Get out your purse. I have prisoners to sell.", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_open_party_screen_on_condition), null, 100, null, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_talk_player_2", "ransom_broker_talk", "ransom_broker_2", "{=Yac7bSU3}Tell me again about brokering ransoms.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_talk_player_4", "ransom_broker_talk", "ransom_broker_no_prisoners", "{=2aM84VZh}Nothing more for now.", null, null, 0, null, null);
			campaignGameStarter.AddDialogLine("ransom_broker_no_prisoners", "ransom_broker_no_prisoners", "close_window", "{=mEsaiLOR}Very well then. If you happen to have any more prisoners, you know where to find me.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("ransom_broker_sell_prisoners", "ransom_broker_sell_prisoners", "ransom_broker_sell_prisoners_3", "{=xFmYRCHs}Let me see what you have...", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_ransom_broker_sell_prisoners_on_consequence), 100, null);
			campaignGameStarter.AddDialogLine("ransom_broker_sell_prisoners_3", "ransom_broker_sell_prisoners_3", "ransom_broker_pretalk", "{=3BvfOe1y}Very well then. You catch some more and you want me to take them off of your hands, you know where to find me...", null, null, 100, null);
			campaignGameStarter.AddDialogLine("ransom_broker_sell_prisoners_2", "ransom_broker_sell_prisoners_2", "close_window", "{=fQaPv0Xl}I will be staying here for a few days. Let me know if you need my services.", null, null, 100, null);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00053255 File Offset: 0x00051455
		private bool conversation_tavernmaid_offers_usual_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernWench && !this._orderedDrinkThisVisit && this._orderedDrinkThisDayInSettlement == Settlement.CurrentSettlement;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0005327C File Offset: 0x0005147C
		private bool conversation_tavernmaid_offers_food_on_condition()
		{
			if (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernWench && this._orderedDrinkThisDayInSettlement != Settlement.CurrentSettlement)
			{
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "vlandia")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=07qBenIW}a flagon of ale", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=uJceH1Dv}a Sunor sausage", null), false);
					if (MobileParty.MainParty.CurrentSettlement.Position.Y < 150f)
					{
						MBTextManager.SetTextVariable("FOOD", new TextObject("{=07QlFlXK}a plate of herrings", null), false);
					}
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "empire")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=ybXgaKEv}a glass of local wine", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=IBhZGxxm}a plate of olives", null), false);
					if (MobileParty.MainParty.CurrentSettlement.Position.X < 300f)
					{
						MBTextManager.SetTextVariable("FOOD", new TextObject("{=d18Ul2Zl}a plate of sardines", null), false);
					}
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "khuzait")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=WZbrxhYm}a flask of kefir", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=0qc11kmz}a plate of mutton dumplings", null), false);
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "aserai")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=AULqrp7D}a glass of Calradian wine", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=GhPGpR90}a plate of dates", null), false);
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "sturgia")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=bZbFrIUr}a mug of kvass", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=LPBVTiV6}a strip of bacon", null), false);
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "battania")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=vEHaOSIT}a mug of sour beer", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=z4arML8E}a strip of dried venison", null), false);
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "nord")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=z47FuQ3f}a horn of barley beer", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=gcB54MUl}some buttered hardfish", null), false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00053529 File Offset: 0x00051729
		private void conversation_tavernmaid_delivers_food_on_consequence()
		{
			this._orderedDrinkThisDayInSettlement = Settlement.CurrentSettlement;
			this._orderedDrinkThisVisit = true;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0005353D File Offset: 0x0005173D
		private bool conversation_tavernmaid_gossips_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernWench && this._orderedDrinkThisDayInSettlement == Settlement.CurrentSettlement;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0005355C File Offset: 0x0005175C
		private bool conversation_player_can_order_food_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernWench && !this._orderedDrinkThisVisit;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00053578 File Offset: 0x00051778
		private static bool tavenkeeper_has_quest_on_condition()
		{
			List<IssueBase> list = IssueManager.GetIssuesInSettlement(Hero.MainHero.CurrentSettlement, true).ToList<IssueBase>();
			if (list.Count > 0)
			{
				list.Shuffle<IssueBase>();
				if (list.Count == 1)
				{
					MBTextManager.SetTextVariable("ISSUE_GIVER_LIST", "{=roTCX8S8}{ISSUE_GIVER_1.LINK} mentioned something about needing someone to do some work.", false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_1", list[0].IssueOwner.CharacterObject, null, false);
				}
				else if (list.Count == 2)
				{
					MBTextManager.SetTextVariable("ISSUE_GIVER_LIST", "{=79XElnsg}{ISSUE_GIVER_1.LINK} mentioned something about needing someone to do some work. And I think {ISSUE_GIVER_2.LINK} was looking for someone.", false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_1", list[0].IssueOwner.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_2", list[1].IssueOwner.CharacterObject, null, false);
				}
				else
				{
					MBTextManager.SetTextVariable("ISSUE_GIVER_LIST", "{=SIxE2LGn}{ISSUE_GIVER_1.LINK} mentioned something about needing someone to do some work. And I think {ISSUE_GIVER_2.LINK} and {ISSUE_GIVER_3.LINK} were looking for someone.", false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_1", list[0].IssueOwner.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_2", list[1].IssueOwner.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_3", list[2].IssueOwner.CharacterObject, null, false);
				}
				return true;
			}
			MBTextManager.SetTextVariable("ISSUE_GIVER_LIST", "{=RlP8aYVJ}Nobody is looking for help right now.", false);
			return true;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x000536BD File Offset: 0x000518BD
		private bool tavernkeeper_talk_companion_on_condition()
		{
			this._inquiryCurrentCompanion = null;
			return true;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x000536C7 File Offset: 0x000518C7
		private bool conversation_tavernmaid_buy_tun_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernWench && this._orderedDrinkThisDayInSettlement == Settlement.CurrentSettlement && !this._hasBoughtTunToParty && PartyBase.MainParty.MemberRoster.Count > 1;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00053700 File Offset: 0x00051900
		private bool can_buy_tun_on_clickable_condition(out TextObject explanation)
		{
			if (Hero.MainHero.Gold < TavernEmployeesCampaignBehavior.get_tun_price())
			{
				explanation = new TextObject("{=xVZVYNan}You don't have enough{GOLD_ICON}.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00053728 File Offset: 0x00051928
		private void can_buy_tun_on_consequence()
		{
			int tun_price = TavernEmployeesCampaignBehavior.get_tun_price();
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, tun_price, false);
			MobileParty.MainParty.RecentEventsMorale += 2f;
			this._hasBoughtTunToParty = true;
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00053768 File Offset: 0x00051968
		private static bool calculate_tun_cost_on_condition()
		{
			int tun_price = TavernEmployeesCampaignBehavior.get_tun_price();
			MBTextManager.SetTextVariable("COST", tun_price);
			return true;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00053787 File Offset: 0x00051987
		private static int get_tun_price()
		{
			return (int)(50f + (float)MobileParty.MainParty.MemberRoster.TotalHealthyCount * 0.2f);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x000537A8 File Offset: 0x000519A8
		private bool conversation_ransom_broker_start_on_condition()
		{
			if (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.RansomBroker && !this._hasMetWithRansomBroker)
			{
				this._hasMetWithRansomBroker = true;
				MBTextManager.SetTextVariable("RANSOM_BROKER_INTRO", "{=Y7tozytM}Hello, {?PLAYER.GENDER}madam{?}sir{\\?}. You have the bearing of a warrior. Do you have a minute? We may have interests in common.", false);
				if (Settlement.CurrentSettlement.OwnerClan == Hero.MainHero.Clan || Settlement.CurrentSettlement.MapFaction.Leader == Hero.MainHero)
				{
					MBTextManager.SetTextVariable("RANSOM_BROKER_INTRO", "{=6zxo4AU9}This is quite the honor, your {?PLAYER.GENDER}ladyship{?}lordship{\\?}. Do you have a minute? I may be able to do you a service.", false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00053821 File Offset: 0x00051A21
		private bool conversation_ransom_broker_open_party_screen_on_condition()
		{
			return MobileParty.MainParty.Party.NumberOfPrisoners > 0;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00053835 File Offset: 0x00051A35
		private bool conversation_ransom_broker_start_has_met_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.RansomBroker && this._hasMetWithRansomBroker;
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0005384D File Offset: 0x00051A4D
		private void conversation_ransom_broker_sell_prisoners_on_consequence()
		{
			PartyScreenHelper.OpenScreenAsRansom();
		}

		// Token: 0x040004D6 RID: 1238
		public const int TavernCompanionInquiryCost = 2;

		// Token: 0x040004D7 RID: 1239
		private const int MinimumTavernCompanionInquirySkillLevel = 50;

		// Token: 0x040004D8 RID: 1240
		private const int BaseTunPrice = 50;

		// Token: 0x040004D9 RID: 1241
		private const int AskForClanInfoPrice = 500;

		// Token: 0x040004DA RID: 1242
		private const float CompanionTownDistanceConsiderationMultiplier = 3f;

		// Token: 0x040004DB RID: 1243
		private Settlement _orderedDrinkThisDayInSettlement;

		// Token: 0x040004DC RID: 1244
		private bool _orderedDrinkThisVisit;

		// Token: 0x040004DD RID: 1245
		private bool _hasMetWithRansomBroker;

		// Token: 0x040004DE RID: 1246
		private bool _hasBoughtTunToParty;

		// Token: 0x040004DF RID: 1247
		private Hero _inquiryCurrentCompanion;

		// Token: 0x040004E0 RID: 1248
		private TavernEmployeesCampaignBehavior.TavernInquiryCompanionType _selectedCompanionType;

		// Token: 0x040004E1 RID: 1249
		private int _inquiryVariationIndex;

		// Token: 0x040004E2 RID: 1250
		private readonly Dictionary<TavernEmployeesCampaignBehavior.TavernInquiryCompanionType, List<Hero>> _previouslyRecommendedCompanions = new Dictionary<TavernEmployeesCampaignBehavior.TavernInquiryCompanionType, List<Hero>>();

		// Token: 0x02000216 RID: 534
		private enum TavernInquiryCompanionType
		{
			// Token: 0x04000972 RID: 2418
			Scout,
			// Token: 0x04000973 RID: 2419
			Engineer,
			// Token: 0x04000974 RID: 2420
			Surgeon,
			// Token: 0x04000975 RID: 2421
			Quartermaster,
			// Token: 0x04000976 RID: 2422
			FirstMate,
			// Token: 0x04000977 RID: 2423
			Navigator,
			// Token: 0x04000978 RID: 2424
			CaravanLeader,
			// Token: 0x04000979 RID: 2425
			Leader,
			// Token: 0x0400097A RID: 2426
			Roguery
		}
	}
}
