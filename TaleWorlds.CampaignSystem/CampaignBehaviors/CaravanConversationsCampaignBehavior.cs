using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F7 RID: 1015
	public class CaravanConversationsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003DAF RID: 15791 RVA: 0x00102279 File Offset: 0x00100479
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00102292 File Offset: 0x00100492
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x00102294 File Offset: 0x00100494
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x001022A0 File Offset: 0x001004A0
		protected void AddDialogs(CampaignGameStarter starter)
		{
			starter.AddPlayerLine("caravan_create_conversation_1", "hero_main_options", "magistrate_form_a_caravan_cost", "{=!}{CARAVAN_BUY_INTENT_TEXT}", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_build_on_condition), null, 100, new ConversationSentence.OnClickableConditionDelegate(this.conversation_caravan_build_clickable_condition), null);
			starter.AddDialogLine("caravan_create_conversation_2", "magistrate_form_a_caravan_cost", "magistrate_form_a_caravan_player_answer", "{=!}{CARAVAN_FORMING_INFO_1}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_caravan_cost_on_condition), null, 100, null);
			starter.AddPlayerLine("caravan_create_conversation_3", "magistrate_form_a_caravan_player_answer", "lord_pretalk", "{=otVPaR6T}Actually I do not have a free companion right now.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_caravan_companion_condition), null, 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_4", "magistrate_form_a_caravan_player_answer", "lord_pretalk", "{=w6WFuDn0}I am sorry, I don't have that much money.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_caravan_gold_condition), null, 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_5", "magistrate_form_a_caravan_player_answer", "magistrate_form_a_caravan_accepted", "{=!}{FORM_CARAVAN_ACCEPT}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_small_caravan_accept_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_magistrate_form_a_small_caravan_accept_on_consequence), 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_6", "magistrate_form_a_caravan_player_answer", "magistrate_form_a_caravan_big", "{=!}{LARGE_CARAVAN_OFFER}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_large_caravan_accept_on_condition), null, 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_10", "magistrate_form_a_caravan_player_answer", "lord_pretalk", "{=2mJjDTAZ}That sounds expensive.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_caravan_reject_on_condition), null, 100, null, null);
			starter.AddDialogLine("caravan_create_conversation_7", "magistrate_form_a_caravan_big", "magistrate_form_a_caravan_big_player_answer", "{=DaBzJkIz}I can increase quality of troops, but cost will proportionally increase, too. It will cost {AMOUNT}{GOLD_ICON}.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_big_caravan_offer_condition), null, 100, null);
			starter.AddPlayerLine("caravan_create_conversation_8", "magistrate_form_a_caravan_big_player_answer", "magistrate_form_a_caravan_accepted", "{=!}{CREATE_LARGE_CARAVAN_TEXT}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_big_caravan_accept_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_magistrate_form_a_big_caravan_accept_on_consequence), 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_9", "magistrate_form_a_caravan_big_player_answer", "lord_pretalk", "{=w6WFuDn0}I am sorry, I don't have that much money.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_big_caravan_gold_condition), null, 100, null, null);
			starter.AddPlayerLine("caravan_create_conversation_10_2", "magistrate_form_a_caravan_big_player_answer", "lord_pretalk", "{=2mJjDTAZ}That sounds expensive.", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_big_caravan_reject_on_condition), null, 100, null, null);
			starter.AddDialogLine("caravan_create_conversation_11", "magistrate_form_a_caravan_accepted", "magistrate_form_a_caravan_accepted_choose_leader", "{=!}{CARAVAN_LEADER_CHOOSE_TEXT}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_caravan_accepted_choose_leader_on_condition), null, 100, null);
			starter.AddRepeatablePlayerLine("caravan_create_conversation_12", "magistrate_form_a_caravan_accepted_choose_leader", "magistrate_form_a_caravan_accepted_leader_is_chosen", "{=!}{HERO.NAME}", "{=UNFE1BeG}I am thinking of a different person", "magistrate_form_a_caravan_accepted", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_caravan_accepted_leader_is_chosen_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_magistrate_form_a_caravan_accept_on_consequence), 100, null);
			starter.AddPlayerLine("caravan_create_conversation_13", "magistrate_form_a_caravan_accepted_choose_leader", "lord_pretalk", "{=PznWhAdU}Actually, never mind.", null, null, 100, null, null);
			starter.AddDialogLine("caravan_create_conversation_14", "magistrate_form_a_caravan_accepted_leader_is_chosen", "close_window", "{=!}{CARAVAN_NOTABLE_FINAL_TALK}", new ConversationSentence.OnConditionDelegate(this.conversation_magistrate_form_a_caravan_final_conversation_on_condition), null, 100, null);
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x00102568 File Offset: 0x00100768
		private bool conversation_caravan_build_on_condition()
		{
			bool flag = Hero.OneToOneConversationHero != null && (Hero.OneToOneConversationHero.IsMerchant || Hero.OneToOneConversationHero.IsArtisan);
			if (flag)
			{
				if (this.ShouldCreateConvoy())
				{
					MBTextManager.SetTextVariable("CARAVAN_BUY_INTENT_TEXT", "{=7l4I06Hi}I wish to form a trade convoy in this town.", false);
					return flag;
				}
				MBTextManager.SetTextVariable("CARAVAN_BUY_INTENT_TEXT", "{=tuz8ZNT6}I wish to form a caravan in this town.", false);
			}
			return flag;
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x001025C4 File Offset: 0x001007C4
		private bool conversation_caravan_build_clickable_condition(out TextObject explanation)
		{
			if (Campaign.Current.IsMainHeroDisguised)
			{
				explanation = new TextObject("{=jcEoUPCB}You are in disguise.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x001025E5 File Offset: 0x001007E5
		private bool conversation_magistrate_form_a_caravan_cost_on_condition()
		{
			if (this.ShouldCreateConvoy())
			{
				MBTextManager.SetTextVariable("CARAVAN_FORMING_INFO_1", "{=OvtzH0b3}Well.. There are many goods around the town that can bring good money if you trade them. A trade convoy you formed will do this for you. You need to pay at least {AMOUNT}{GOLD_ICON} to hire guards to form a trade convoy and you need one companion to lead the convoy guards.", false);
			}
			else
			{
				MBTextManager.SetTextVariable("CARAVAN_FORMING_INFO_1", "{=cZptYTYd}Well.. There are many goods around the town that can bring good money if you trade them. A caravan you formed will do this for you. You need to pay at least {AMOUNT}{GOLD_ICON} to hire caravan guards to form a caravan and you need one companion to lead the caravan guards.", false);
			}
			MBTextManager.SetTextVariable("AMOUNT", this.GetSmallCaravanGoldCost());
			return true;
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x00102622 File Offset: 0x00100822
		private bool conversation_magistrate_form_caravan_companion_condition()
		{
			return this.FindSuitableCompanionsToLeadCaravan().Count == 0;
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00102632 File Offset: 0x00100832
		private bool conversation_magistrate_form_caravan_gold_condition()
		{
			return this.FindSuitableCompanionsToLeadCaravan().Count > 0 && Hero.MainHero.Gold < this.GetSmallCaravanGoldCost();
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x00102658 File Offset: 0x00100858
		private bool conversation_magistrate_form_a_small_caravan_accept_on_condition()
		{
			if (this.ShouldCreateConvoy())
			{
				MBTextManager.SetTextVariable("FORM_CARAVAN_ACCEPT", new TextObject("{=JNZwJaJ9}I accept these conditions and I am ready to pay {AMOUNT}{GOLD_ICON} to create a trade convoy.", null), false);
				MBTextManager.SetTextVariable("LARGE_CARAVAN_OFFER", new TextObject("{=V8bxlnSl}Is there a way to form a trade convoy that includes better troops?", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("FORM_CARAVAN_ACCEPT", new TextObject("{=zOp48Fsg}I accept these conditions and I am ready to pay {AMOUNT}{GOLD_ICON} to create a caravan.", null), false);
				MBTextManager.SetTextVariable("LARGE_CARAVAN_OFFER", new TextObject("{=4mhOs9Fb}Is there a way to form a caravan that includes better troops?", null), false);
			}
			MBTextManager.SetTextVariable("AMOUNT", this.GetSmallCaravanGoldCost());
			return this.FindSuitableCompanionsToLeadCaravan().Count > 0 && Hero.MainHero.Gold >= this.GetSmallCaravanGoldCost();
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x001026FC File Offset: 0x001008FC
		private bool conversation_magistrate_form_a_large_caravan_accept_on_condition()
		{
			if (this.ShouldCreateConvoy())
			{
				MBTextManager.SetTextVariable("LARGE_CARAVAN_OFFER", new TextObject("{=V8bxlnSl}Is there a way to form a trade convoy that includes better troops?", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("LARGE_CARAVAN_OFFER", new TextObject("{=4mhOs9Fb}Is there a way to form a caravan that includes better troops?", null), false);
			}
			return this.conversation_magistrate_form_a_small_caravan_accept_on_condition();
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x0010273A File Offset: 0x0010093A
		private void conversation_magistrate_form_a_small_caravan_accept_on_consequence()
		{
			this._selectedCaravanType = 0;
			this.conversation_magistrate_form_a_caravan_accepted_on_consequence();
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x00102749 File Offset: 0x00100949
		private void conversation_magistrate_form_a_caravan_accepted_on_consequence()
		{
			ConversationSentence.SetObjectsToRepeatOver(this.FindSuitableCompanionsToLeadCaravan(), 5);
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x00102757 File Offset: 0x00100957
		private bool conversation_magistrate_form_a_caravan_reject_on_condition()
		{
			return Hero.MainHero.Gold >= this.GetSmallCaravanGoldCost();
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x0010276E File Offset: 0x0010096E
		private bool conversation_magistrate_form_a_big_caravan_offer_condition()
		{
			MBTextManager.SetTextVariable("AMOUNT", this.GetLargeCaravanGoldCost());
			return true;
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x00102784 File Offset: 0x00100984
		private bool conversation_magistrate_form_a_big_caravan_accept_on_condition()
		{
			TextObject textObject = TextObject.GetEmpty();
			if (this.ShouldCreateConvoy())
			{
				textObject = new TextObject("{=XxQzR39f}Okay then lets go with better troops, I am ready to pay {AMOUNT}{GOLD_ICON} to create a trade convoy.", null);
			}
			else
			{
				textObject = new TextObject("{=AuMLELpp}Okay then lets go with better troops, I am ready to pay {AMOUNT}{GOLD_ICON} to create a caravan.", null);
			}
			MBTextManager.SetTextVariable("AMOUNT", this.GetLargeCaravanGoldCost());
			MBTextManager.SetTextVariable("CREATE_LARGE_CARAVAN_TEXT", textObject, false);
			return this.FindSuitableCompanionsToLeadCaravan().Count > 0 && Hero.MainHero.Gold >= this.GetLargeCaravanGoldCost();
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x001027FA File Offset: 0x001009FA
		private bool conversation_magistrate_form_a_big_caravan_gold_condition()
		{
			return Hero.MainHero.Gold < this.GetLargeCaravanGoldCost();
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x0010280E File Offset: 0x00100A0E
		private void conversation_magistrate_form_a_big_caravan_accept_on_consequence()
		{
			this._selectedCaravanType = 1;
			this.conversation_magistrate_form_a_caravan_accepted_on_consequence();
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x0010281D File Offset: 0x00100A1D
		private bool conversation_magistrate_form_a_big_caravan_reject_on_condition()
		{
			return Hero.MainHero.Gold >= this.GetLargeCaravanGoldCost();
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x00102834 File Offset: 0x00100A34
		private bool conversation_magistrate_form_a_caravan_accepted_choose_leader_on_condition()
		{
			if (this.ShouldCreateConvoy())
			{
				MBTextManager.SetTextVariable("CARAVAN_LEADER_CHOOSE_TEXT", new TextObject("{=Ww7vJSb9}Whom do you want to lead the convoy?", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("CARAVAN_LEADER_CHOOSE_TEXT", new TextObject("{=aeCYFe1g}Whom do you want to lead the caravan?", null), false);
			}
			return true;
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x0010286D File Offset: 0x00100A6D
		private bool conversation_magistrate_form_a_caravan_final_conversation_on_condition()
		{
			if (this.ShouldCreateConvoy())
			{
				MBTextManager.SetTextVariable("CARAVAN_NOTABLE_FINAL_TALK", new TextObject("{=2WFPZrFf}Ok then. I will call my men to help you form a trade convoy. I hope it brings you a good profit.", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("CARAVAN_NOTABLE_FINAL_TALK", new TextObject("{=Z2Lq2QLq}Ok then. I will call my men to help you form a caravan. I hope it brings you a good profit.", null), false);
			}
			return true;
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x001028A8 File Offset: 0x00100AA8
		private bool conversation_magistrate_form_a_caravan_accepted_leader_is_chosen_on_condition()
		{
			CharacterObject characterObject = ConversationSentence.CurrentProcessedRepeatObject as CharacterObject;
			if (characterObject != null)
			{
				StringHelpers.SetRepeatableCharacterProperties("HERO", characterObject, false);
				return true;
			}
			return false;
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x001028D4 File Offset: 0x00100AD4
		private void conversation_magistrate_form_a_caravan_accept_on_consequence()
		{
			CharacterObject characterObject = ConversationSentence.SelectedRepeatObject as CharacterObject;
			this.FadeOutSelectedCaravanCompanionInMission(characterObject);
			LeaveSettlementAction.ApplyForCharacterOnly(characterObject.HeroObject);
			bool flag = this._selectedCaravanType == 1;
			PartyTemplateObject randomCaravanTemplate = CaravanHelper.GetRandomCaravanTemplate(Settlement.CurrentSettlement.Culture, flag, !this.ShouldCreateConvoy());
			CaravanPartyComponent.CreateCaravanParty(Hero.MainHero, Settlement.CurrentSettlement, randomCaravanTemplate, false, characterObject.HeroObject, null, flag);
			GiveGoldAction.ApplyForCharacterToSettlement(Hero.MainHero, Settlement.CurrentSettlement, (!flag) ? this.GetSmallCaravanGoldCost() : this.GetLargeCaravanGoldCost(), false);
			TextObject textObject;
			if (this.ShouldCreateConvoy())
			{
				textObject = new TextObject("{=c7VOPmSb}A new trade convoy is created for {HERO.NAME}.", null);
			}
			else
			{
				textObject = new TextObject("{=RmtTsqcx}A new caravan is created for {HERO.NAME}.", null);
			}
			StringHelpers.SetCharacterProperties("HERO", Hero.MainHero.CharacterObject, textObject, false);
			InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x001029A6 File Offset: 0x00100BA6
		private void FadeOutSelectedCaravanCompanionInMission(CharacterObject caravanLeader)
		{
			ICampaignMission campaignMission = CampaignMission.Current;
			if (campaignMission == null)
			{
				return;
			}
			campaignMission.FadeOutCharacter(caravanLeader);
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x001029B8 File Offset: 0x00100BB8
		private List<CharacterObject> FindSuitableCompanionsToLeadCaravan()
		{
			List<CharacterObject> list = new List<CharacterObject>();
			foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
			{
				Hero heroObject = troopRosterElement.Character.HeroObject;
				if (heroObject != null && heroObject != Hero.MainHero && heroObject.Clan == Clan.PlayerClan && heroObject.GovernorOf == null && heroObject.CanLeadParty())
				{
					list.Add(troopRosterElement.Character);
				}
			}
			return list;
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x00102A54 File Offset: 0x00100C54
		private bool ShouldCreateConvoy()
		{
			if (Settlement.CurrentSettlement == null)
			{
				Debug.FailedAssert("Current settlement is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CaravanConversationsCampaignBehavior.cs", "ShouldCreateConvoy", 302);
				return false;
			}
			return Settlement.CurrentSettlement.HasPort;
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x00102A82 File Offset: 0x00100C82
		private int GetLargeCaravanGoldCost()
		{
			if (!this.ShouldCreateConvoy())
			{
				return Campaign.Current.Models.CaravanModel.GetCaravanFormingCost(true, false);
			}
			return Campaign.Current.Models.CaravanModel.GetCaravanFormingCost(true, true);
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x00102AB9 File Offset: 0x00100CB9
		private int GetSmallCaravanGoldCost()
		{
			if (!this.ShouldCreateConvoy())
			{
				return Campaign.Current.Models.CaravanModel.GetCaravanFormingCost(false, false);
			}
			return Campaign.Current.Models.CaravanModel.GetCaravanFormingCost(false, true);
		}

		// Token: 0x04001326 RID: 4902
		private int _selectedCaravanType;
	}
}
