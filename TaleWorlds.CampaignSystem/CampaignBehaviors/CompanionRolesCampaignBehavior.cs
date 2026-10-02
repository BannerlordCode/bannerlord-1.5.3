using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003FE RID: 1022
	public class CompanionRolesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06003FA5 RID: 16293 RVA: 0x00114379 File Offset: 0x00112579
		private CompanionRolesCampaignBehavior CurrentBehavior
		{
			get
			{
				return Campaign.Current.GetCampaignBehavior<CompanionRolesCampaignBehavior>();
			}
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x00114388 File Offset: 0x00112588
		public override void RegisterEvents()
		{
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.HeroRelationChanged.AddNonSerializedListener(this, new Action<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero>(this.OnHeroRelationChanged));
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x001143DA File Offset: 0x001125DA
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<int>>("_alreadyUsedIconIdsForNewClans", ref this._alreadyUsedIconIdsForNewClans);
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x001143F0 File Offset: 0x001125F0
		private void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			if (((effectiveHero == Hero.MainHero && effectiveHeroGainedRelationWith.IsPlayerCompanion) || (effectiveHero.IsPlayerCompanion && effectiveHeroGainedRelationWith == Hero.MainHero)) && relationChange < 0 && effectiveHero.GetRelation(effectiveHeroGainedRelationWith) < -10)
			{
				KillCharacterAction.ApplyByRemove(effectiveHero.IsPlayerCompanion ? effectiveHero : effectiveHeroGainedRelationWith, false, true);
			}
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0011443F File Offset: 0x0011263F
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x00114448 File Offset: 0x00112648
		protected void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddPlayerLine("companion_rejoin_after_emprisonment_role", "hero_main_options", "companion_rejoin", "{=!}{COMPANION_REJOIN_LINE}", new ConversationSentence.OnConditionDelegate(this.companion_rejoin_after_emprisonment_role_on_condition), delegate
			{
				Campaign.Current.ConversationManager.ConversationEnd += this.companion_rejoin_after_emprisonment_role_on_consequence;
			}, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_rejoin", "companion_rejoin", "close_window", "{=ppi6eVos}As you wish.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_start_role", "hero_main_options", "companion_role_pretalk", "{=d4t6oUCn}About your position in the clan...", new ConversationSentence.OnConditionDelegate(this.companion_role_discuss_on_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_pretalk", "companion_role_pretalk", "companion_role", "{=!}{COMPANION_ROLE}", new ConversationSentence.OnConditionDelegate(this.companion_has_role_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_talk_fire", "companion_role", "companion_fire", "{=pRsCnGoo}I no longer have need of your services.", new ConversationSentence.OnConditionDelegate(this.companion_fire_condition), null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_talk_fire_2", "companion_role", "companion_assign_new_role", "{=2g18dlwo}I would like to assign you a new role.", new ConversationSentence.OnConditionDelegate(this.companion_assign_role_on_condition), null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_talk_fire_3", "companion_role", "too_many_roles", "{=2g18dlwo}I would like to assign you a new role.", new ConversationSentence.OnConditionDelegate(this.companion_assign_but_too_many_role_on_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_assign_new_role", "companion_assign_new_role", "companion_roles", "{=5ajobQiL}What role do you have in mind?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_talk_fire_3", "companion_role", "companion_okay", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_engineer", "companion_roles", "companion_okay", "{=E91oU7oi}I no longer need you as Engineer.", new ConversationSentence.OnConditionDelegate(this.companion_fire_engineer_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_engineer_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_surgeon", "companion_roles", "companion_okay", "{=Dga7sQOu}I no longer need you as Surgeon.", new ConversationSentence.OnConditionDelegate(this.companion_fire_surgeon_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_surgeon_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_quartermaster", "companion_roles", "companion_okay", "{=GjpJN2xE}I no longer need you as Quartermaster.", new ConversationSentence.OnConditionDelegate(this.companion_fire_quartermaster_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_quartermaster_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_scout", "companion_roles", "companion_okay", "{=EUQnsZFb}I no longer need you as Scout.", new ConversationSentence.OnConditionDelegate(this.companion_fire_scout_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_scout_on_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("companion_role_response", "companion_okay", "hero_main_options", "{=dzXaXKaC}Very well.", null, null, 1, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_engineer_2", "companion_roles", "give_companion_roles", "{=UuFPafDj}Engineer {CURRENTLY_HELD_ENGINEER}", new ConversationSentence.OnConditionDelegate(this.companion_becomes_engineer_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_becomes_engineer_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.companion_becomes_engineer_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("companion_becomes_surgeon_2", "companion_roles", "give_companion_roles", "{=6xZ8U3Yz}Surgeon {CURRENTLY_HELD_SURGEON}", new ConversationSentence.OnConditionDelegate(this.companion_becomes_surgeon_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_becomes_surgeon_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.companion_becomes_surgeon_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("companion_becomes_quartermaster_2", "companion_roles", "give_companion_roles", "{=B0VLXHHz}Quartermaster {CURRENTLY_HELD_QUARTERMASTER}", new ConversationSentence.OnConditionDelegate(this.companion_becomes_quartermaster_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_becomes_quartermaster_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.companion_becomes_quartermaster_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("companion_becomes_scout_2", "companion_roles", "give_companion_roles", "{=3aziL3Gs}Scout {CURRENTLY_HELD_SCOUT}", new ConversationSentence.OnConditionDelegate(this.companion_becomes_scout_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_becomes_scout_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.companion_becomes_scout_clickable_condition), null);
			campaignGameStarter.AddDialogLine("companion_role_response_2", "give_companion_roles", "hero_main_options", "{=5hhxQBTj}I would be honored.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("companion_have_too_many_roles", "too_many_roles", "too_many_roles_responses", "{=m3AsvplJ}I already have quite a few duties. Perhaps you could relieve me of one of them, so that I can take on this new responsibility?", null, null, 1, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_engineer_3", "too_many_roles_responses", "companion_okay_to_role_selection", "{=E91oU7oi}I no longer need you as Engineer.", new ConversationSentence.OnConditionDelegate(this.companion_fire_engineer_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_engineer_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_surgeon_3", "too_many_roles_responses", "companion_okay_to_role_selection", "{=Dga7sQOu}I no longer need you as Surgeon.", new ConversationSentence.OnConditionDelegate(this.companion_fire_surgeon_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_surgeon_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_quartermaster_3", "too_many_roles_responses", "companion_okay_to_role_selection", "{=GjpJN2xE}I no longer need you as Quartermaster.", new ConversationSentence.OnConditionDelegate(this.companion_fire_quartermaster_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_quartermaster_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_scout_3", "too_many_roles_responses", "companion_okay_to_role_selection", "{=EUQnsZFb}I no longer need you as Scout.", new ConversationSentence.OnConditionDelegate(this.companion_fire_scout_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_scout_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_becomes_engineer", "too_many_roles_responses", "hero_main_options", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_role_unassign_response", "companion_okay_to_role_selection", "companion_assign_new_role", "{=dzXaXKaC}Very well.", null, null, 1, null);
			campaignGameStarter.AddPlayerLine("companion_talk_return", "companion_roles", "companion_okay", "{=D33fIGQe}Never mind.", null, null, 1, null, null);
			campaignGameStarter.AddDialogLine("companion_start_mission", "hero_main_options", "companion_mission_pretalk", "{=4ry48jbg}I have a mission for you...", () => HeroHelper.IsCompanionInPlayerParty(Hero.OneToOneConversationHero), null, 100, null);
			campaignGameStarter.AddDialogLine("companion_pretalk_2", "companion_mission_pretalk", "companion_mission", "{=7EoBCTX0}What do you want me to do?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_mission_gather_troops", "companion_mission", "companion_recruit_troops", "{=MDik3Kfn}I want you to recruit some troops.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_mission_forage", "companion_mission", "companion_forage", "{=kAbebv72}I want you to go forage some food.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_mission_patrol", "companion_mission", "companion_patrol", "{=OMaM6ihN}I want you to patrol the area.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_mission_cancel", "companion_mission", "hero_main_options", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_forage_1", "companion_forage", "companion_forage_2", "{=o2g6Wi9K}As you wish. Will I take some troops with me?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_forage_2", "companion_forage_2", "companion_forage_troops", "{=lVbQCibL}Yes. Take these troops with you.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_forage_3", "companion_forage_2", "companion_forage_3", "{=3bOcF1Cw}I can't spare anyone now. You will need to go alone.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("companion_fire", "companion_fire", "companion_fire2", "{=bUzU50P8}What? Why? Did I do something wrong?[ib:closed]", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("companion_fire_age", "companion_fire2", "companion_fire3", "{=ywtuRAmP}Time has taken its toll on us all, friend. It's time that you retire.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_fire_no_fit", "companion_fire2", "companion_fire3", "{=1s3bHupn}You're not getting along with the rest of the company. It's better you go.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_fire_no_fit_2", "companion_fire2", "companion_fire3", "{=Q0xPr6CP}I cannot be sure of your loyalty any longer.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_fire_underperforming", "companion_fire2", "companion_fire3", "{=aCwCaWGC}Your skills are not what I need.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("companion_fire_cancel", "companion_fire2", "companion_fire_cancel", "{=8VlqJteC}I was just jesting. I need you more than ever. Now go back to your job.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_talk_done_on_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("companion_fire_cancel2", "companion_fire_cancel", "close_window", "{=vctta154}Well {PLAYER.NAME}, it is certainly good to see you still retain your sense of humor.[if:convo_nervous][ib:normal2]", null, null, 100, null);
			campaignGameStarter.AddDialogLine("companion_fire_farewell", "companion_fire3", "close_window", "{=!}{AGREE_TO_LEAVE}[ib:nervous2]", new ConversationSentence.OnConditionDelegate(this.companion_agrees_to_leave_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_fire_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_start", "hero_main_options", "turn_companion_to_lord_talk_answer", "{=B9uT9wa6}I wish to reward you for your services.", new ConversationSentence.OnConditionDelegate(this.turn_companion_to_lord_on_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_start_answer_2", "turn_companion_to_lord_talk_answer", "companion_leading_caravan", "{=IkH0pVhC}I would be honored, my {?PLAYER.GENDER}lady{?}lord{\\?}. But I can't take on any new responsibilities while leading this caravan. If you wish to relieve me of my duties, we can discuss this further.", new ConversationSentence.OnConditionDelegate(this.companion_is_leading_caravan_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_start_answer_player", "companion_leading_caravan", "lord_pretalk", "{=i7k0AXsO}I see. We will speak again when you are relieved from your duty.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_start_answer", "turn_companion_to_lord_talk_answer", "turn_companion_to_lord_talk", "{=TXO1ihiZ}Thank you, my {?PLAYER.GENDER}lady{?}lord{\\?}. I have often thought about that. If I had a fief, with revenues, and perhaps a title to go with it, I could marry well and pass my wealth down to my heirs, and of course raise troops to help defend the realm.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_has_fief", "turn_companion_to_lord_talk", "check_player_has_fief_to_grant", "{=KqazzTWV}Indeed. You have shed your blood for me, and you deserve a fief of your own..", null, new ConversationSentence.OnConsequenceDelegate(this.fief_grant_answer_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_has_no_fief", "check_player_has_fief_to_grant", "player_has_no_fief_to_grant", "{=Wx5ysDp1}My {?PLAYER.GENDER}lady{?}lord{\\?}, as much as I appreciate the gesture, I am not sure that you have a suitable estate to grant me.", new ConversationSentence.OnConditionDelegate(this.turn_companion_to_lord_no_fief_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_has_no_fief_player_answer", "player_has_no_fief_to_grant", "player_has_no_fief_to_grant_answer", "{=6uUzWz46}I see. Maybe we will speak again when I have one.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_has_no_fief_companion_answer", "player_has_no_fief_to_grant_answer", "hero_main_options", "{=PP3LzCKk}As you wish, my {?PLAYER.GENDER}lady{?}lord{\\?}.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_has_fief_answer", "check_player_has_fief_to_grant", "player_has_fief_list", "{=ArNB7aaL}Where exactly did you have in mind?[if:convo_happy]", null, null, 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("turn_companion_to_lord_has_fief_list", "player_has_fief_list", "player_selected_fief_to_grant", "{=3rHeoq6r}{SETTLEMENT_NAME}.", "{=sxc2D6NJ}I am thinking of a different location.", "check_player_has_fief_to_grant", new ConversationSentence.OnConditionDelegate(this.list_player_fief_on_condition), new ConversationSentence.OnConsequenceDelegate(this.list_player_fief_selected_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.list_player_fief_clickable_condition));
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_has_fief_list_cancel", "player_has_fief_list", "turn_companion_to_lord_fief_conclude", "{=UEbesbKZ}Actually, I have changed my mind.", null, new ConversationSentence.OnConsequenceDelegate(this.list_player_fief_cancel_on_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_fief_selected", "player_selected_fief_to_grant", "turn_companion_to_lord_fief_selected_answer", "{=Mt9abZzi}{SETTLEMENT_NAME}? This is a great honor, my {?PLAYER.GENDER}lady{?}lord{\\?}. I will protect it until the last drop of my blood.[ib:hip][if:convo_happy]", new ConversationSentence.OnConditionDelegate(this.fief_selected_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_fief_selected_confirm", "turn_companion_to_lord_fief_selected_answer", "turn_companion_to_lord_fief_selected_confirm_box", "{=TtlwXnVc}I am pleased to grant you the title of {CULTURE_SPECIFIC_TITLE} and the fiefdom of {SETTLEMENT_NAME}.. You richly deserve it.", null, null, 100, new ConversationSentence.OnClickableConditionDelegate(this.fief_selected_confirm_clickable_on_condition), null);
			campaignGameStarter.AddPlayerLine("turn_companion_to_lord_fief_selected_reject", "turn_companion_to_lord_fief_selected_answer", "turn_companion_to_lord_fief_conclude", "{=LDGMSQJJ}Very well. Let me think on this a bit longer", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_fief_selected_confirm_box", "turn_companion_to_lord_fief_selected_confirm_box", "turn_companion_to_lord_fief_conclude", "{=LOiZfCEy}My {?PLAYER.GENDER}lady{?}lord{\\?}, it would be an honor if you were to choose the name of my noble house.", null, new ConversationSentence.OnConsequenceDelegate(this.turn_companion_to_lord_consequence), 100, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_done_answer_thanks", "turn_companion_to_lord_fief_conclude", "close_window", "{=dpYhBgAC}Thank you my {?PLAYER.GENDER}lady{?}lord{\\?}. I will always remember this grand gesture.[ib:hip][if:convo_happy]", new ConversationSentence.OnConditionDelegate(this.companion_thanks_on_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_talk_done_on_consequence), 100, null);
			campaignGameStarter.AddDialogLine("turn_companion_to_lord_done_answer_rejected", "turn_companion_to_lord_fief_conclude", "hero_main_options", "{=SVEptNxR}It's only normal that you have second thoughts. I will be right by your side if you change your mind, my {?PLAYER.GENDER}lady{?}lord{\\?}.[ib:hip][if:convo_nervous]", null, new ConversationSentence.OnConsequenceDelegate(this.companion_talk_done_on_consequence), 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_start", "start", "rescue_companion_option_acknowledgement", "{=FVOfzPot}{SALUTATION}... Thank you for freeing me.", new ConversationSentence.OnConditionDelegate(this.companion_rescue_start_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("rescue_companion_option_acknowledgement", "rescue_companion_option_acknowledgement", "rescue_companion_preoptions", "{=YyNywO6Z}Think nothing of it. I'm glad you're safe.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("rescue_companion_preoptions", "rescue_companion_preoptions", "rescue_companion_options", "{=kaVMFgBs}What now?", new ConversationSentence.OnConditionDelegate(this.companion_rescue_start_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("rescue_companion_option_1", "rescue_companion_options", "rescue_companion_join_party", "{=drIfaTa7}Rejoin the others and let's be off.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_rescue_answer_options_join_party_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("rescue_companion_option_2", "rescue_companion_options", "rescue_companion_lead_party", "{=Y6Z8qNW9}I'll need you to lead a party.", null, null, 100, new ConversationSentence.OnClickableConditionDelegate(this.lead_a_party_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("rescue_companion_option_3", "rescue_companion_options", "rescue_companion_do_nothing", "{=dRKk0E1V}Unfortunately, I can't take you back right now.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("rescue_companion_lead_party_answer", "rescue_companion_lead_party", "close_window", "{=Q9Ltufg5}Tell me who to command.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_rescue_answer_options_lead_party_consequence), 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_join_party_answer", "rescue_companion_join_party", "close_window", "{=92mngWSd}All right. It's good to be back.", null, new ConversationSentence.OnConsequenceDelegate(this.end_rescue_companion), 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_do_nothing_answer", "rescue_companion_do_nothing", "close_window", "{=gT2O4YXc}I will go off on my own, then. I can stay busy. But I'll remember - I owe you one!", null, new ConversationSentence.OnConsequenceDelegate(this.end_rescue_companion), 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_lead_party_create_party_continue_0", "start", "party_screen_rescue_continue", "{=ppi6eVos}As you wish.", new ConversationSentence.OnConditionDelegate(this.party_screen_continue_conversation_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_lead_party_create_party_continue_1", "party_screen_rescue_continue", "rescue_companion_options", "{=ttWBYlxS}So, what shall I do?", new ConversationSentence.OnConditionDelegate(this.party_screen_opened_but_party_is_not_created_after_rescue_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("rescue_companion_lead_party_create_party_continue_2", "party_screen_rescue_continue", "close_window", "{=DiEKuVGF}We'll make ready to set out at once.", new ConversationSentence.OnConditionDelegate(this.party_screen_opened_and_party_is_created_after_rescue_condition), new ConversationSentence.OnConsequenceDelegate(this.end_rescue_companion), 100, null);
			campaignGameStarter.AddDialogLine("default_conversation_for_wrongly_created_heroes", "start", "close_window", "{=BaeqKlQ6}I am not allowed to talk with you.", null, null, 0, null);
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x00115097 File Offset: 0x00113297
		private bool companion_fire_condition()
		{
			return Hero.OneToOneConversationHero.IsPlayerCompanion && Settlement.CurrentSettlement == null && (Hero.OneToOneConversationHero.PartyBelongedTo == null || !Hero.OneToOneConversationHero.PartyBelongedTo.IsInNavalAutoTravel);
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x001150CE File Offset: 0x001132CE
		private bool turn_companion_to_lord_no_fief_on_condition()
		{
			return !Hero.MainHero.Clan.Settlements.Any<Settlement>((Settlement x) => x.IsTown || x.IsCastle);
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x00115108 File Offset: 0x00113308
		private bool turn_companion_to_lord_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			if (oneToOneConversationHero != null && oneToOneConversationHero.IsPlayerCompanion && Hero.MainHero.IsKingdomLeader)
			{
				MobileParty partyBelongedTo = oneToOneConversationHero.PartyBelongedTo;
				if (partyBelongedTo == null || !partyBelongedTo.IsCurrentlyAtSea)
				{
					this.CurrentBehavior._playerConfirmedTheAction = false;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x00115158 File Offset: 0x00113358
		private bool companion_is_leading_caravan_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			return oneToOneConversationHero != null && oneToOneConversationHero.IsPlayerCompanion && oneToOneConversationHero.PartyBelongedTo != null && oneToOneConversationHero.PartyBelongedTo.IsCaravan;
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x0011518B File Offset: 0x0011338B
		private void fief_grant_answer_consequence()
		{
			ConversationSentence.SetObjectsToRepeatOver(Hero.MainHero.Clan.Settlements.Where<Settlement>((Settlement x) => x.IsTown || x.IsCastle).ToList<Settlement>(), 5);
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x001151CC File Offset: 0x001133CC
		private bool list_player_fief_clickable_condition(out TextObject explanation)
		{
			Kingdom kingdom = Hero.MainHero.MapFaction as Kingdom;
			Settlement fief = ConversationSentence.CurrentProcessedRepeatObject as Settlement;
			if (fief.SiegeEvent != null)
			{
				explanation = new TextObject("{=arCGUuR5}The settlement is under siege.", null);
				return false;
			}
			if (fief.Town.IsOwnerUnassigned || kingdom.UnresolvedDecisions.Any<KingdomDecision>(delegate(KingdomDecision x)
			{
				SettlementClaimantDecision settlementClaimantDecision;
				SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision;
				return ((settlementClaimantDecision = x as SettlementClaimantDecision) != null && settlementClaimantDecision.Settlement == fief) || ((settlementClaimantPreliminaryDecision = x as SettlementClaimantPreliminaryDecision) != null && settlementClaimantPreliminaryDecision.Settlement == fief);
			}))
			{
				explanation = new TextObject("{=OiPqa3L8}This settlement's ownership will be decided through voting.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x0011525C File Offset: 0x0011345C
		private bool list_player_fief_on_condition()
		{
			Settlement settlement = ConversationSentence.CurrentProcessedRepeatObject as Settlement;
			if (settlement != null)
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("SETTLEMENT_NAME", settlement.Name);
			}
			return true;
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x0011528E File Offset: 0x0011348E
		private void list_player_fief_selected_on_consequence()
		{
			this._selectedFief = ConversationSentence.SelectedRepeatObject as Settlement;
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x001152A0 File Offset: 0x001134A0
		private void turn_companion_to_lord_consequence()
		{
			TextObject textObject = new TextObject("{=ntDH7J3H}This action costs {NEEDED_GOLD_TO_GRANT_FIEF}{GOLD_ICON} and {NEEDED_INFLUENCE_TO_GRANT_FIEF}{INFLUENCE_ICON}. You will also be granting {SETTLEMENT} to {COMPANION.NAME}.", null);
			textObject.SetTextVariable("NEEDED_GOLD_TO_GRANT_FIEF", 20000);
			textObject.SetTextVariable("NEEDED_INFLUENCE_TO_GRANT_FIEF", 500);
			textObject.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
			textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			textObject.SetCharacterProperties("COMPANION", Hero.OneToOneConversationHero.CharacterObject, false);
			textObject.SetTextVariable("SETTLEMENT", this.CurrentBehavior._selectedFief.Name);
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=awjomtnJ}Are you sure?", null).ToString(), textObject.ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), new Action(this.ConfirmTurningCompanionToLordConsequence), new Action(this.RejectTurningCompanionToLordConsequence), "", 0f, null, null, null), false, false);
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x00115398 File Offset: 0x00113598
		private void ConfirmTurningCompanionToLordConsequence()
		{
			this.CurrentBehavior._playerConfirmedTheAction = true;
			object obj = new TextObject("{=0oItvT2C}Choose {COMPANION.NAME}{.o} clan name: ", null);
			StringHelpers.SetCharacterProperties("COMPANION", Hero.OneToOneConversationHero.CharacterObject, null, false);
			InformationManager.ShowTextInquiry(new TextInquiryData(obj.ToString(), string.Empty, true, false, GameTexts.FindText("str_done", null).ToString(), null, new Action<string>(this.ClanNameSelectionIsDone), null, false, new Func<string, Tuple<bool, string>>(FactionHelper.IsClanNameApplicable), "", ""), false, false);
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x00115420 File Offset: 0x00113620
		private void RejectTurningCompanionToLordConsequence()
		{
			this.CurrentBehavior._playerConfirmedTheAction = false;
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x00115440 File Offset: 0x00113640
		private void ClanNameSelectionIsDone(string clanName)
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			RemoveCompanionAction.ApplyByByTurningToLord(Hero.MainHero.Clan, oneToOneConversationHero);
			oneToOneConversationHero.SetNewOccupation(Occupation.Lord);
			TextObject textObject = GameTexts.FindText("str_generic_clan_name", null);
			textObject.SetTextVariable("CLAN_NAME", new TextObject(clanName, null));
			int randomBannerIdForNewClan = this.GetRandomBannerIdForNewClan();
			Clan clan = Clan.CreateCompanionToLordClan(oneToOneConversationHero, this.CurrentBehavior._selectedFief, textObject, randomBannerIdForNewClan);
			if (oneToOneConversationHero.PartyBelongedTo == MobileParty.MainParty)
			{
				MobileParty.MainParty.MemberRoster.AddToCounts(oneToOneConversationHero.CharacterObject, -1, false, 0, 0, true, -1);
			}
			MobileParty mobileParty = oneToOneConversationHero.PartyBelongedTo;
			if (mobileParty == null)
			{
				mobileParty = LordPartyComponent.CreateLordParty(oneToOneConversationHero.CharacterObject.StringId, oneToOneConversationHero, MobileParty.MainParty.Position, 3f, this.CurrentBehavior._selectedFief, oneToOneConversationHero);
			}
			else
			{
				mobileParty.ActualClan = clan;
				mobileParty.Party.SetVisualAsDirty();
			}
			if (mobileParty.MemberRoster.TotalManCount < 33)
			{
				int num = 33 - mobileParty.MemberRoster.TotalManCount;
				int num2 = num / 2;
				int num3 = num - num2;
				mobileParty.MemberRoster.AddToCounts(clan.Culture.BasicTroop, num2, false, 0, 0, true, -1);
				mobileParty.MemberRoster.AddToCounts(clan.Culture.EliteBasicTroop, num3, false, 0, 0, true, -1);
			}
			this.AdjustCompanionsEquipment(oneToOneConversationHero);
			this.SpawnNewHeroesForNewCompanionClan(oneToOneConversationHero, clan, this.CurrentBehavior._selectedFief);
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, oneToOneConversationHero, 20000, false);
			GainKingdomInfluenceAction.ApplyForDefault(Hero.MainHero, -500f);
			ChangeRelationAction.ApplyPlayerRelation(oneToOneConversationHero, 50, true, true);
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x001155D8 File Offset: 0x001137D8
		private void AdjustCompanionsEquipment(Hero companionHero)
		{
			Equipment equipmentForCompanionWhenTurningToLord = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForCompanionWhenTurningToLord(companionHero, Equipment.EquipmentType.Civilian);
			Equipment equipmentForCompanionWhenTurningToLord2 = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForCompanionWhenTurningToLord(companionHero, Equipment.EquipmentType.Battle);
			Equipment equipment = new Equipment(Equipment.EquipmentType.Civilian);
			Equipment equipment2 = new Equipment(Equipment.EquipmentType.Battle);
			for (int i = 0; i < 12; i++)
			{
				if (equipmentForCompanionWhenTurningToLord2[i].Item != null && (companionHero.BattleEquipment[i].Item == null || companionHero.BattleEquipment[i].Item.Tier < equipmentForCompanionWhenTurningToLord2[i].Item.Tier))
				{
					equipment2[i] = equipmentForCompanionWhenTurningToLord2[i];
				}
				else
				{
					equipment2[i] = companionHero.BattleEquipment[i];
				}
				if (equipmentForCompanionWhenTurningToLord[i].Item != null && (companionHero.CivilianEquipment[i].Item == null || companionHero.CivilianEquipment[i].Item.Tier < equipmentForCompanionWhenTurningToLord[i].Item.Tier))
				{
					equipment[i] = equipmentForCompanionWhenTurningToLord[i];
				}
				else
				{
					equipment[i] = companionHero.CivilianEquipment[i];
				}
			}
			EquipmentHelper.AssignHeroEquipmentFromEquipment(companionHero, equipment);
			EquipmentHelper.AssignHeroEquipmentFromEquipment(companionHero, equipment2);
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x00115750 File Offset: 0x00113950
		private int GetRandomBannerIdForNewClan()
		{
			MBReadOnlyList<int> possibleClanBannerIconsIDs = Hero.MainHero.MapFaction.Culture.PossibleClanBannerIconsIDs;
			int num = possibleClanBannerIconsIDs.GetRandomElement<int>();
			if (this.CurrentBehavior._alreadyUsedIconIdsForNewClans.Contains(num))
			{
				int num2 = 0;
				do
				{
					num = possibleClanBannerIconsIDs.GetRandomElement<int>();
					num2++;
				}
				while (this.CurrentBehavior._alreadyUsedIconIdsForNewClans.Contains(num) && num2 < 20);
				bool flag = num2 != 20;
				if (!flag)
				{
					for (int i = 0; i < possibleClanBannerIconsIDs.Count; i++)
					{
						if (!this.CurrentBehavior._alreadyUsedIconIdsForNewClans.Contains(possibleClanBannerIconsIDs[i]))
						{
							num = possibleClanBannerIconsIDs[i];
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					num = possibleClanBannerIconsIDs.GetRandomElement<int>();
				}
			}
			if (!this.CurrentBehavior._alreadyUsedIconIdsForNewClans.Contains(num))
			{
				this.CurrentBehavior._alreadyUsedIconIdsForNewClans.Add(num);
			}
			return num;
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x0011582C File Offset: 0x00113A2C
		private void SpawnNewHeroesForNewCompanionClan(Hero companionHero, Clan clan, Settlement settlement)
		{
			MBReadOnlyList<CharacterObject> lordTemplates = companionHero.Culture.LordTemplates;
			List<Hero> list = new List<Hero>();
			list.Add(this.CreateNewHeroForNewCompanionClan(lordTemplates.GetRandomElement<CharacterObject>(), settlement, new Dictionary<SkillObject, int>
			{
				{
					DefaultSkills.Steward,
					MBRandom.RandomInt(100, 175)
				},
				{
					DefaultSkills.Leadership,
					MBRandom.RandomInt(125, 175)
				},
				{
					DefaultSkills.OneHanded,
					MBRandom.RandomInt(125, 175)
				},
				{
					DefaultSkills.Medicine,
					MBRandom.RandomInt(125, 175)
				}
			}));
			list.Add(this.CreateNewHeroForNewCompanionClan(lordTemplates.GetRandomElement<CharacterObject>(), settlement, new Dictionary<SkillObject, int>
			{
				{
					DefaultSkills.OneHanded,
					MBRandom.RandomInt(100, 175)
				},
				{
					DefaultSkills.Leadership,
					MBRandom.RandomInt(125, 175)
				},
				{
					DefaultSkills.Tactics,
					MBRandom.RandomInt(125, 175)
				},
				{
					DefaultSkills.Engineering,
					MBRandom.RandomInt(125, 175)
				}
			}));
			list.Add(companionHero);
			foreach (Hero hero in list)
			{
				hero.Clan = clan;
				hero.ChangeState(Hero.CharacterStates.Active);
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, Hero.MainHero, MBRandom.RandomInt(5, 10), false);
				if (hero != companionHero)
				{
					EnterSettlementAction.ApplyForCharacterOnly(hero, settlement);
				}
				foreach (Hero hero2 in list)
				{
					if (hero != hero2)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, hero2, MBRandom.RandomInt(5, 10), false);
					}
				}
			}
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x001159F0 File Offset: 0x00113BF0
		private Hero CreateNewHeroForNewCompanionClan(CharacterObject templateCharacter, Settlement settlement, Dictionary<SkillObject, int> startingSkills)
		{
			Hero hero = HeroCreator.CreateSpecialHero(templateCharacter, settlement, null, null, MBRandom.RandomInt(Campaign.Current.Models.AgeModel.HeroComesOfAge, 50));
			foreach (KeyValuePair<SkillObject, int> keyValuePair in startingSkills)
			{
				hero.HeroDeveloper.SetInitialSkillLevel(keyValuePair.Key, keyValuePair.Value);
			}
			return hero;
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x00115A78 File Offset: 0x00113C78
		private void list_player_fief_cancel_on_consequence()
		{
			this.CurrentBehavior._playerConfirmedTheAction = false;
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x00115A86 File Offset: 0x00113C86
		private bool fief_selected_on_condition()
		{
			MBTextManager.SetTextVariable("SETTLEMENT_NAME", this.CurrentBehavior._selectedFief.Name, false);
			return true;
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x00115AA4 File Offset: 0x00113CA4
		private bool companion_thanks_on_condition()
		{
			return this.CurrentBehavior._playerConfirmedTheAction;
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x00115AB4 File Offset: 0x00113CB4
		private bool fief_selected_confirm_clickable_on_condition(out TextObject explanation)
		{
			MBTextManager.SetTextVariable("CULTURE_SPECIFIC_TITLE", HeroHelper.GetTitleInIndefiniteCase(Hero.OneToOneConversationHero), false);
			MBTextManager.SetTextVariable("SETTLEMENT_NAME", this.CurrentBehavior._selectedFief.Name, false);
			bool flag = Hero.MainHero.Gold >= 20000;
			bool flag2 = Hero.MainHero.Clan.Influence >= 500f;
			MBTextManager.SetTextVariable("NEEDED_GOLD_TO_GRANT_FIEF", 20000);
			MBTextManager.SetTextVariable("NEEDED_INFLUENCE_TO_GRANT_FIEF", 500);
			MBTextManager.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">", false);
			MBTextManager.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">", false);
			if (flag && flag2)
			{
				explanation = new TextObject("{=PxQEwCha}You will pay {NEEDED_GOLD_TO_GRANT_FIEF}{GOLD_ICON}, {NEEDED_INFLUENCE_TO_GRANT_FIEF}{INFLUENCE_ICON}.", null);
				return true;
			}
			explanation = new TextObject("{=!}{GOLD_REQUIREMENT}{INFLUENCE_REQUIREMENT}", null);
			if (!flag)
			{
				TextObject textObject = new TextObject("{=yo2NvkQQ}You need {NEEDED_GOLD_TO_GRANT_FIEF}{GOLD_ICON}. ", null);
				explanation.SetTextVariable("GOLD_REQUIREMENT", textObject);
			}
			if (!flag2)
			{
				TextObject textObject2 = new TextObject("{=pDeFXZJd}You need {NEEDED_INFLUENCE_TO_GRANT_FIEF}{INFLUENCE_ICON}.", null);
				explanation.SetTextVariable("INFLUENCE_REQUIREMENT", textObject2);
			}
			return false;
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x00115BBA File Offset: 0x00113DBA
		private void companion_talk_done_on_consequence()
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x00115BCC File Offset: 0x00113DCC
		private void companion_fire_on_consequence()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			RemoveCompanionAction.ApplyByFire(oneToOneConversationHero.CompanionOf, oneToOneConversationHero);
			KillCharacterAction.ApplyByRemove(oneToOneConversationHero, false, true);
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x00115C00 File Offset: 0x00113E00
		private bool companion_rejoin_after_emprisonment_role_on_condition()
		{
			if (Hero.OneToOneConversationHero != null && !Hero.OneToOneConversationHero.IsPartyLeader && (Hero.OneToOneConversationHero.IsPlayerCompanion || Hero.OneToOneConversationHero.Clan == Clan.PlayerClan) && Hero.OneToOneConversationHero.PartyBelongedTo != MobileParty.MainParty && (Hero.OneToOneConversationHero.PartyBelongedTo == null || !Hero.OneToOneConversationHero.PartyBelongedTo.IsCaravan))
			{
				if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsTown && Hero.OneToOneConversationHero.GovernorOf == Settlement.CurrentSettlement.Town)
				{
					MBTextManager.SetTextVariable("COMPANION_REJOIN_LINE", "{=Z5zAok5G}I need to recall you to my party, and to stop governing this town.", false);
				}
				else
				{
					MBTextManager.SetTextVariable("COMPANION_REJOIN_LINE", "{=gR0ksbaQ}Get your things. I'd like you to rejoin the party.", false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x00115CBF File Offset: 0x00113EBF
		private void companion_rejoin_after_emprisonment_role_on_consequence()
		{
			AddHeroToPartyAction.Apply(Hero.OneToOneConversationHero, MobileParty.MainParty, true);
			Campaign.Current.ConversationManager.ConversationEnd -= this.companion_rejoin_after_emprisonment_role_on_consequence;
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x00115CEC File Offset: 0x00113EEC
		private void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (LocationComplex.Current != null)
			{
				LocationComplex.Current.RemoveCharacterIfExists(companion);
			}
			if (PlayerEncounter.LocationEncounter != null)
			{
				PlayerEncounter.LocationEncounter.RemoveAccompanyingCharacter(companion);
			}
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x00115D12 File Offset: 0x00113F12
		private bool companion_agrees_to_leave_on_condition()
		{
			MBTextManager.SetTextVariable("AGREE_TO_LEAVE", new TextObject("{=0geP718k}Well... I don't know what to say. Goodbye, then.", null), false);
			return true;
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x00115D2C File Offset: 0x00113F2C
		private bool companion_has_role_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			List<PartyRole> heroPartyRoles = MobileParty.MainParty.GetHeroPartyRoles(oneToOneConversationHero);
			if (heroPartyRoles.Count == 0)
			{
				MBTextManager.SetTextVariable("COMPANION_ROLE", new TextObject("{=k7ebznzr}Yes?", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("COMPANION_ROLE", new TextObject("{=n3bvfe8t}I am currently working as {COMPANION_JOB}.", null), false);
				if (heroPartyRoles.Count == 1)
				{
					MBTextManager.SetTextVariable("COMPANION_JOB", GameTexts.FindText("role", heroPartyRoles.First<PartyRole>().ToString()), false);
				}
				else
				{
					List<TextObject> list = new List<TextObject>();
					foreach (PartyRole partyRole in heroPartyRoles)
					{
						list.Add(GameTexts.FindText("role", partyRole.ToString()));
					}
					TextObject textObject = GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, true);
					MBTextManager.SetTextVariable("COMPANION_JOB", textObject, false);
				}
			}
			return true;
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x00115E30 File Offset: 0x00114030
		private bool companion_role_discuss_on_condition()
		{
			if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.Clan == Clan.PlayerClan)
			{
				MobileParty partyBelongedTo = Hero.OneToOneConversationHero.PartyBelongedTo;
				return partyBelongedTo != null && !partyBelongedTo.IsInNavalAutoTravel;
			}
			return false;
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x00115E64 File Offset: 0x00114064
		private bool companion_assign_role_on_condition()
		{
			return Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.Clan == Clan.PlayerClan && Hero.OneToOneConversationHero.PartyBelongedTo == MobileParty.MainParty && MobileParty.MainParty.GetHeroPartyRoles(Hero.OneToOneConversationHero).Count < Campaign.Current.Models.ClanMemberPartyRoleModel.MaximumPartyRoleAssignmentCount;
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x00115EC8 File Offset: 0x001140C8
		private bool companion_assign_but_too_many_role_on_condition()
		{
			return Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.Clan == Clan.PlayerClan && Hero.OneToOneConversationHero.PartyBelongedTo == MobileParty.MainParty && MobileParty.MainParty.GetHeroPartyRoles(Hero.OneToOneConversationHero).Count >= Campaign.Current.Models.ClanMemberPartyRoleModel.MaximumPartyRoleAssignmentCount;
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x00115F2D File Offset: 0x0011412D
		private bool party_role_assignment_clickable_condition(PartyRole role, out TextObject explanation)
		{
			bool flag = Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRoleInParty(role, Hero.OneToOneConversationHero, Hero.OneToOneConversationHero.PartyBelongedTo);
			if (!flag)
			{
				explanation = new TextObject("{=zcTOL3gI}Not eligible for the role.", null);
				return flag;
			}
			explanation = TextObject.GetEmpty();
			return flag;
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x00115F6B File Offset: 0x0011416B
		private bool companion_becomes_engineer_clickable_condition(out TextObject explanation)
		{
			return this.party_role_assignment_clickable_condition(PartyRole.Engineer, out explanation);
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x00115F78 File Offset: 0x00114178
		private bool companion_becomes_engineer_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			Hero roleHolder = oneToOneConversationHero.PartyBelongedTo.GetRoleHolder(PartyRole.Engineer);
			if (roleHolder != null)
			{
				TextObject textObject = new TextObject("{=QEp8t8u0}(Currently held by {COMPANION.LINK})", null);
				StringHelpers.SetCharacterProperties("COMPANION", roleHolder.CharacterObject, textObject, false);
				MBTextManager.SetTextVariable("CURRENTLY_HELD_ENGINEER", textObject, false);
			}
			else
			{
				MBTextManager.SetTextVariable("CURRENTLY_HELD_ENGINEER", "{=kNQMkh3j}(Currently unassigned)", false);
			}
			return roleHolder != oneToOneConversationHero;
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x00115FDF File Offset: 0x001141DF
		private void companion_becomes_engineer_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.SetPartyEngineer(Hero.OneToOneConversationHero);
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x00115FF5 File Offset: 0x001141F5
		private bool companion_becomes_surgeon_clickable_condition(out TextObject explanation)
		{
			return this.party_role_assignment_clickable_condition(PartyRole.Surgeon, out explanation);
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x00116000 File Offset: 0x00114200
		private bool companion_becomes_surgeon_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			Hero roleHolder = oneToOneConversationHero.PartyBelongedTo.GetRoleHolder(PartyRole.Surgeon);
			if (roleHolder != null)
			{
				TextObject textObject = new TextObject("{=QEp8t8u0}(Currently held by {COMPANION.LINK})", null);
				StringHelpers.SetCharacterProperties("COMPANION", roleHolder.CharacterObject, textObject, false);
				MBTextManager.SetTextVariable("CURRENTLY_HELD_SURGEON", textObject, false);
			}
			else
			{
				MBTextManager.SetTextVariable("CURRENTLY_HELD_SURGEON", "{=kNQMkh3j}(Currently unassigned)", false);
			}
			return roleHolder != oneToOneConversationHero && Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRoleInParty(PartyRole.Surgeon, oneToOneConversationHero, oneToOneConversationHero.PartyBelongedTo);
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x00116082 File Offset: 0x00114282
		private void companion_becomes_surgeon_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.SetPartySurgeon(Hero.OneToOneConversationHero);
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x00116098 File Offset: 0x00114298
		private bool companion_becomes_quartermaster_clickable_condition(out TextObject explanation)
		{
			return this.party_role_assignment_clickable_condition(PartyRole.Quartermaster, out explanation);
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x001160A4 File Offset: 0x001142A4
		private bool companion_becomes_quartermaster_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			Hero roleHolder = oneToOneConversationHero.PartyBelongedTo.GetRoleHolder(PartyRole.Quartermaster);
			if (roleHolder != null)
			{
				TextObject textObject = new TextObject("{=QEp8t8u0}(Currently held by {COMPANION.LINK})", null);
				StringHelpers.SetCharacterProperties("COMPANION", roleHolder.CharacterObject, textObject, false);
				MBTextManager.SetTextVariable("CURRENTLY_HELD_QUARTERMASTER", textObject, false);
			}
			else
			{
				MBTextManager.SetTextVariable("CURRENTLY_HELD_QUARTERMASTER", "{=kNQMkh3j}(Currently unassigned)", false);
			}
			Hero oneToOneConversationHero2 = Hero.OneToOneConversationHero;
			return roleHolder != oneToOneConversationHero && Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRoleInParty(PartyRole.Quartermaster, oneToOneConversationHero, oneToOneConversationHero.PartyBelongedTo);
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x0011612E File Offset: 0x0011432E
		private void companion_becomes_quartermaster_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.SetPartyQuartermaster(Hero.OneToOneConversationHero);
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x00116144 File Offset: 0x00114344
		private bool companion_becomes_scout_clickable_condition(out TextObject explanation)
		{
			return this.party_role_assignment_clickable_condition(PartyRole.Scout, out explanation);
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x00116150 File Offset: 0x00114350
		private bool companion_becomes_scout_on_condition()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			Hero roleHolder = oneToOneConversationHero.PartyBelongedTo.GetRoleHolder(PartyRole.Scout);
			if (roleHolder != null)
			{
				TextObject textObject = new TextObject("{=QEp8t8u0}(Currently held by {COMPANION.LINK})", null);
				StringHelpers.SetCharacterProperties("COMPANION", roleHolder.CharacterObject, textObject, false);
				MBTextManager.SetTextVariable("CURRENTLY_HELD_SCOUT", textObject, false);
			}
			else
			{
				MBTextManager.SetTextVariable("CURRENTLY_HELD_SCOUT", "{=kNQMkh3j}(Currently unassigned)", false);
			}
			return roleHolder != oneToOneConversationHero && Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRoleInParty(PartyRole.Scout, oneToOneConversationHero, oneToOneConversationHero.PartyBelongedTo);
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x001161D4 File Offset: 0x001143D4
		private void companion_becomes_scout_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.SetPartyScout(Hero.OneToOneConversationHero);
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x001161EA File Offset: 0x001143EA
		private bool CanFireHeroFromRole(PartyRole role, Hero hero)
		{
			return hero.PartyBelongedTo.GetRoleHolder(role) == hero && hero != hero.PartyBelongedTo.LeaderHero;
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0011620E File Offset: 0x0011440E
		private bool companion_fire_engineer_on_condition()
		{
			return this.CanFireHeroFromRole(PartyRole.Engineer, Hero.OneToOneConversationHero);
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x0011621C File Offset: 0x0011441C
		private bool companion_fire_surgeon_on_condition()
		{
			return this.CanFireHeroFromRole(PartyRole.Surgeon, Hero.OneToOneConversationHero);
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x0011622A File Offset: 0x0011442A
		private bool companion_fire_quartermaster_on_condition()
		{
			return this.CanFireHeroFromRole(PartyRole.Quartermaster, Hero.OneToOneConversationHero);
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x00116239 File Offset: 0x00114439
		private bool companion_fire_scout_on_condition()
		{
			return this.CanFireHeroFromRole(PartyRole.Scout, Hero.OneToOneConversationHero);
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x00116248 File Offset: 0x00114448
		private void companion_fire_engineer_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.RemovePartyRoleOfHero(Hero.OneToOneConversationHero, PartyRole.Engineer);
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x0011625F File Offset: 0x0011445F
		private void companion_fire_surgeon_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.RemovePartyRoleOfHero(Hero.OneToOneConversationHero, PartyRole.Surgeon);
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x00116276 File Offset: 0x00114476
		private void companion_fire_quartermaster_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.RemovePartyRoleOfHero(Hero.OneToOneConversationHero, PartyRole.Quartermaster);
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x0011628E File Offset: 0x0011448E
		private void companion_fire_scout_on_consequence()
		{
			Hero.OneToOneConversationHero.PartyBelongedTo.RemovePartyRoleOfHero(Hero.OneToOneConversationHero, PartyRole.Scout);
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x001162A8 File Offset: 0x001144A8
		private bool companion_rescue_start_condition()
		{
			if (Campaign.Current.CurrentConversationContext == ConversationContext.FreeOrCapturePrisonerHero)
			{
				Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
				if (((oneToOneConversationHero != null) ? oneToOneConversationHero.CompanionOf : null) == Clan.PlayerClan && CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Wanderer)
				{
					MBTextManager.SetTextVariable("SALUTATION", Campaign.Current.ConversationManager.FindMatchingTextOrNull("str_salutation", CharacterObject.OneToOneConversationCharacter), false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x0011630F File Offset: 0x0011450F
		private void companion_rescue_answer_options_join_party_consequence()
		{
			EndCaptivityAction.ApplyByReleasedAfterBattle(Hero.OneToOneConversationHero);
			Hero.OneToOneConversationHero.ChangeState(Hero.CharacterStates.Active);
			MobileParty.MainParty.AddElementToMemberRoster(CharacterObject.OneToOneConversationCharacter, 1, false);
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x00116338 File Offset: 0x00114538
		private bool lead_a_party_clickable_condition(out TextObject reason)
		{
			bool flag = Clan.PlayerClan.WarPartyLimit > Clan.PlayerClan.WarPartyComponents.Count;
			int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
			bool flag2 = Hero.MainHero.Gold > partyGoldLowerThreshold - Hero.OneToOneConversationHero.Gold;
			TextObject textObject = new TextObject("{=QH3pgsia}Creating the party will cost you {PARTY_COST}{GOLD_ICON}.", null).SetTextVariable("PARTY_COST", partyGoldLowerThreshold - Hero.OneToOneConversationHero.Gold).SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			reason = textObject;
			if (!flag)
			{
				reason = GameTexts.FindText("str_clan_doesnt_have_empty_party_slots", null);
			}
			else if (!flag2)
			{
				reason = new TextObject("{=xpCdwmlX}You don't have enough gold to make {HERO.NAME} a party leader.", null);
				reason.SetCharacterProperties("HERO", Hero.OneToOneConversationHero.CharacterObject, false);
			}
			return flag && flag2;
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x001163FD File Offset: 0x001145FD
		private void companion_rescue_answer_options_lead_party_consequence()
		{
			this.OpenPartyScreenForRescue();
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x00116405 File Offset: 0x00114605
		private void OpenPartyScreenForRescue()
		{
			PartyScreenHelper.OpenScreenAsCreateClanPartyForHero(Hero.OneToOneConversationHero, new PartyScreenClosedDelegate(this.PartyScreenClosed), new IsTroopTransferableDelegate(this.TroopTransferableDelegate));
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0011642C File Offset: 0x0011462C
		private void PartyScreenClosed(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool fromCancel)
		{
			if (!fromCancel)
			{
				CharacterObject character = leftMemberRoster.GetTroopRoster().FirstOrDefault<TroopRosterElement>(delegate(TroopRosterElement x)
				{
					Hero heroObject = x.Character.HeroObject;
					return heroObject != null && heroObject.IsPlayerCompanion;
				}).Character;
				EndCaptivityAction.ApplyByReleasedAfterBattle(character.HeroObject);
				character.HeroObject.ChangeState(Hero.CharacterStates.Active);
				MobileParty.MainParty.AddElementToMemberRoster(character, 1, false);
				this._partyCreatedAfterRescueForCompanion = true;
				int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
				if (character.HeroObject.Gold < partyGoldLowerThreshold)
				{
					GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, character.HeroObject, partyGoldLowerThreshold - character.HeroObject.Gold, false);
				}
				MobileParty mobileParty = MobilePartyHelper.CreateNewClanMobileParty(character.HeroObject, Clan.PlayerClan);
				foreach (TroopRosterElement troopRosterElement in leftMemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character != character)
					{
						mobileParty.MemberRoster.Add(troopRosterElement);
						rightOwnerParty.MemberRoster.AddToCounts(troopRosterElement.Character, -troopRosterElement.Number, false, -troopRosterElement.WoundedNumber, -troopRosterElement.Xp, true, -1);
					}
				}
				foreach (TroopRosterElement troopRosterElement2 in leftPrisonRoster.GetTroopRoster())
				{
					mobileParty.MemberRoster.Add(troopRosterElement2);
					rightOwnerParty.PrisonRoster.AddToCounts(troopRosterElement2.Character, -troopRosterElement2.Number, false, -troopRosterElement2.WoundedNumber, -troopRosterElement2.Xp, true, -1);
				}
			}
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x001165EC File Offset: 0x001147EC
		private bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return !character.IsHero;
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x001165F7 File Offset: 0x001147F7
		private bool party_screen_continue_conversation_condition()
		{
			if (Campaign.Current.CurrentConversationContext == ConversationContext.FreeOrCapturePrisonerHero)
			{
				Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
				if (((oneToOneConversationHero != null) ? oneToOneConversationHero.CompanionOf : null) == Clan.PlayerClan)
				{
					return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Wanderer;
				}
			}
			return false;
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x0011662E File Offset: 0x0011482E
		private bool party_screen_opened_but_party_is_not_created_after_rescue_condition()
		{
			return !this._partyCreatedAfterRescueForCompanion;
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x00116639 File Offset: 0x00114839
		private bool party_screen_opened_and_party_is_created_after_rescue_condition()
		{
			return this._partyCreatedAfterRescueForCompanion;
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x00116641 File Offset: 0x00114841
		private void end_rescue_companion()
		{
			this._partyCreatedAfterRescueForCompanion = false;
			if (Hero.OneToOneConversationHero.IsPrisoner)
			{
				EndCaptivityAction.ApplyByReleasedAfterBattle(Hero.OneToOneConversationHero);
			}
		}

		// Token: 0x04001367 RID: 4967
		private const int CompanionRelationLimit = -10;

		// Token: 0x04001368 RID: 4968
		private const int NeededGoldToGrantFief = 20000;

		// Token: 0x04001369 RID: 4969
		private const int NeededInfluenceToGrantFief = 500;

		// Token: 0x0400136A RID: 4970
		private const int RelationGainWhenCompanionToLordAction = 50;

		// Token: 0x0400136B RID: 4971
		private const int NewCreatedHeroForCompanionClanMaxAge = 50;

		// Token: 0x0400136C RID: 4972
		private const int NewHeroSkillUpperLimit = 175;

		// Token: 0x0400136D RID: 4973
		private const int NewHeroSkillLowerLimit = 125;

		// Token: 0x0400136E RID: 4974
		private const int CompanionBecomingVassalPartySizeTarget = 33;

		// Token: 0x0400136F RID: 4975
		private Settlement _selectedFief;

		// Token: 0x04001370 RID: 4976
		private bool _playerConfirmedTheAction;

		// Token: 0x04001371 RID: 4977
		private List<int> _alreadyUsedIconIdsForNewClans = new List<int>();

		// Token: 0x04001372 RID: 4978
		private bool _partyCreatedAfterRescueForCompanion;
	}
}
