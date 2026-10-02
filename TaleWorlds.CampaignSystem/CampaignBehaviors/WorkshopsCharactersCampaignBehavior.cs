using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000475 RID: 1141
	public class WorkshopsCharactersCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004A46 RID: 19014 RVA: 0x001759D0 File Offset: 0x00173BD0
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.ConversationEnded.AddNonSerializedListener(this, new Action<IEnumerable<CharacterObject>>(this.OnConversationEnd));
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x00175A22 File Offset: 0x00173C22
		private void OnConversationEnd(IEnumerable<CharacterObject> _)
		{
			this._currentWorkshop = null;
		}

		// Token: 0x06004A48 RID: 19016 RVA: 0x00175A2B File Offset: 0x00173C2B
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x00175A2D File Offset: 0x00173C2D
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddShopWorkerDialogs(campaignGameStarter);
			this.AddWorkshopOwnerDialogs(campaignGameStarter);
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x00175A40 File Offset: 0x00173C40
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("center");
			if (CampaignMission.Current.Location != null && CampaignMission.Current.Location == locationWithId && CampaignTime.Now.IsDayTime)
			{
				this.AddShopWorkersToTownCenter(unusedUsablePointCount);
			}
		}

		// Token: 0x06004A4B RID: 19019 RVA: 0x00175A94 File Offset: 0x00173C94
		private void AddShopWorkersToTownCenter(Dictionary<string, int> unusedUsablePointCount)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			CharacterObject shopWorker = settlement.Culture.ShopWorker;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(shopWorker.Race, "_settlement");
			Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("center");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(shopWorker, out num, out num2, "");
			foreach (Workshop workshop in settlement.Town.Workshops)
			{
				int num3;
				unusedUsablePointCount.TryGetValue(workshop.Tag, out num3);
				float num4 = (float)num3 * 0.33f;
				if (num4 > 0f)
				{
					int num5 = 0;
					while ((float)num5 < num4)
					{
						LocationCharacter locationCharacter = new LocationCharacter(new AgentData(new SimpleAgentOrigin(shopWorker, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), workshop.Tag, true, LocationCharacter.CharacterRelations.Neutral, null, true, false, null, false, false, true, null, false);
						locationWithId.AddCharacter(locationCharacter);
						num5++;
					}
				}
			}
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x00175BC0 File Offset: 0x00173DC0
		private Workshop FindCurrentWorkshop()
		{
			if (this._currentWorkshop == null)
			{
				WorkshopsCharactersCampaignBehavior.<>c__DisplayClass10_0 CS$<>8__locals1 = new WorkshopsCharactersCampaignBehavior.<>c__DisplayClass10_0();
				IAgent oneToOneConversationAgent = Campaign.Current.ConversationManager.OneToOneConversationAgent;
				WorkshopsCharactersCampaignBehavior.<>c__DisplayClass10_0 CS$<>8__locals2 = CS$<>8__locals1;
				LocationCharacter locationCharacter = Settlement.CurrentSettlement.LocationComplex.FindCharacter(oneToOneConversationAgent);
				CS$<>8__locals2.specialTag = ((locationCharacter != null) ? locationCharacter.SpecialTargetTag : null);
				if (!string.IsNullOrEmpty(CS$<>8__locals1.specialTag))
				{
					this._currentWorkshop = Settlement.CurrentSettlement.Town.Workshops.FirstOrDefault<Workshop>((Workshop x) => x.Tag == CS$<>8__locals1.specialTag);
				}
			}
			return this._currentWorkshop;
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x00175C48 File Offset: 0x00173E48
		private void AddWorkshopOwnerDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddPlayerLine("workshop_notable_owner_begin_1", "hero_main_options", "workshop_owner_notable_single_response", "{=ug5E8FCZ}I wish to buy your {.%}{WORKSHOP_NAME}{.%}.", new ConversationSentence.OnConditionDelegate(this.workshop_notable_owner_begin_single_on_condition), new ConversationSentence.OnConsequenceDelegate(this.workshop_notable_owner_begin_single_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.player_war_status_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("workshop_notable_owner_begin_2", "hero_main_options", "workshop_owner_notable_multiple_response", "{=LuLttpc5}I wish to buy one of your workshops.", new ConversationSentence.OnConditionDelegate(this.workshop_notable_owner_begin_multiple_on_condition), new ConversationSentence.OnConsequenceDelegate(this.workshop_notable_owner_answer_list_workshops_on_condition), 100, new ConversationSentence.OnClickableConditionDelegate(this.player_war_status_clickable_condition), null);
			campaignGameStarter.AddDialogLine("workshop_notable_owner_answer_1_single", "workshop_owner_notable_single_response", "workshop_notable_player_buy_options", "{=IdvcaDMu}I'm willing to sell. But it will cost you {COST} {GOLD_ICON}. Are you willing to pay?", new ConversationSentence.OnConditionDelegate(this.workshop_notable_owner_answer_single_workshop_cost_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("workshop_notable_player_buy_positive", "workshop_notable_player_buy_options", "workshop_notable_player_buy_positive_end", "{=kB65SzbF}Yes.", null, new ConversationSentence.OnConsequenceDelegate(this.workshop_notable_owner_player_buys_workshop_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.workshop_notable_owner_player_buys_workshop_on_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("workshop_notable_player_buy_negative", "workshop_notable_player_buy_options", "workshop_notable_player_buy_negative_end", "{=znDzVxVJ}No.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_notable_player_buy_negative_end", "workshop_notable_player_buy_negative_end", "hero_main_options", "{=Hj25CLlZ}As you wish. Let me know if you change your mind.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_notable_player_buy_positive_end", "workshop_notable_player_buy_positive_end", "hero_main_options", "{=ZtULAKGb}Well then, we have a deal. I will instruct my workers that they are now working for you. Good luck!", null, null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_notable_owner_answer_1_multiple", "workshop_owner_notable_multiple_response", "workshop_player_select_workshop", "{=j78hz3Qc}Hmm. That's possible. Which one?", null, null, 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("workshop_player_select_workshop", "workshop_player_select_workshop", "workshop_owner_notable_single_response", "{=!}{WORKSHOP_NAME}", "{=5z4hEq68}I am thinking of a different kind of workshop.", "workshop_owner_notable_multiple_response", new ConversationSentence.OnConditionDelegate(this.workshop_notable_owner_player_select_workshop_multiple_on_condition), new ConversationSentence.OnConsequenceDelegate(this.workshop_notable_owner_player_select_workshop_multiple_on_consequence), 100, null);
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x00175DFC File Offset: 0x00173FFC
		private bool workshop_notable_owner_begin_single_on_condition()
		{
			if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.IsNotable && Hero.OneToOneConversationHero.CurrentSettlement == Settlement.CurrentSettlement)
			{
				if (Hero.OneToOneConversationHero.OwnedWorkshops.Count<Workshop>((Workshop x) => !x.WorkshopType.IsHidden) == 1)
				{
					MBTextManager.SetTextVariable("WORKSHOP_NAME", Hero.OneToOneConversationHero.OwnedWorkshops.First<Workshop>((Workshop x) => !x.WorkshopType.IsHidden).Name, false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x00175EA5 File Offset: 0x001740A5
		private void workshop_notable_owner_begin_single_on_consequence()
		{
			this._lastSelectedWorkshop = Hero.OneToOneConversationHero.OwnedWorkshops.First<Workshop>((Workshop x) => !x.WorkshopType.IsHidden);
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x00175EDC File Offset: 0x001740DC
		private bool workshop_notable_owner_begin_multiple_on_condition()
		{
			if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.IsNotable && Hero.OneToOneConversationHero.CurrentSettlement == Settlement.CurrentSettlement)
			{
				return Hero.OneToOneConversationHero.OwnedWorkshops.Count<Workshop>((Workshop x) => !x.WorkshopType.IsHidden) > 1;
			}
			return false;
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x00175F40 File Offset: 0x00174140
		private bool workshop_notable_owner_answer_single_workshop_cost_on_condition()
		{
			if (this._lastSelectedWorkshop != null)
			{
				MBTextManager.SetTextVariable("COST", Campaign.Current.Models.WorkshopModel.GetCostForPlayer(this._lastSelectedWorkshop));
				return true;
			}
			return false;
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x00175F71 File Offset: 0x00174171
		private void workshop_notable_owner_answer_list_workshops_on_condition()
		{
			ConversationSentence.SetObjectsToRepeatOver(Hero.OneToOneConversationHero.OwnedWorkshops.Where<Workshop>((Workshop x) => !x.WorkshopType.IsHidden).ToList<Workshop>(), 5);
		}

		// Token: 0x06004A53 RID: 19027 RVA: 0x00175FAC File Offset: 0x001741AC
		private bool workshop_notable_owner_player_select_workshop_multiple_on_condition()
		{
			Workshop workshop = ConversationSentence.CurrentProcessedRepeatObject as Workshop;
			if (workshop != null)
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("WORKSHOP_NAME", workshop.Name);
				return true;
			}
			return false;
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x00175FE0 File Offset: 0x001741E0
		private void workshop_notable_owner_player_select_workshop_multiple_on_consequence()
		{
			this._lastSelectedWorkshop = ConversationSentence.SelectedRepeatObject as Workshop;
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x00175FF2 File Offset: 0x001741F2
		private void workshop_notable_owner_player_buys_workshop_on_consequence()
		{
			ChangeOwnerOfWorkshopAction.ApplyByPlayerBuying(this._lastSelectedWorkshop);
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x00175FFF File Offset: 0x001741FF
		private bool workshop_notable_owner_player_buys_workshop_on_clickable_condition(out TextObject explanation)
		{
			return this.can_player_buy_workshop_clickable_condition(this._lastSelectedWorkshop, out explanation);
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x00176010 File Offset: 0x00174210
		private bool can_player_buy_workshop_clickable_condition(Workshop workshop, out TextObject explanation)
		{
			bool flag = Hero.MainHero.Gold < Campaign.Current.Models.WorkshopModel.GetCostForPlayer(workshop);
			bool flag2 = Campaign.Current.Models.WorkshopModel.GetMaxWorkshopCountForClanTier(Clan.PlayerClan.Tier) <= Hero.MainHero.OwnedWorkshops.Count;
			bool flag3 = false;
			if (flag2)
			{
				explanation = new TextObject("{=Mzs39I2G}You have reached the maximum amount of workshops you can have.", null);
			}
			else if (flag)
			{
				explanation = new TextObject("{=B2jpmFh6}You don't have enough money to buy this workshop.", null);
			}
			else
			{
				explanation = null;
				flag3 = true;
			}
			return flag3;
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x0017609C File Offset: 0x0017429C
		private bool player_war_status_clickable_condition(out TextObject explanation)
		{
			if (Hero.MainHero.MapFaction.IsAtWarWith(Settlement.CurrentSettlement.MapFaction))
			{
				explanation = new TextObject("{=QkiqdcKa}You cannot own a workshop in an enemy town.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x001760CC File Offset: 0x001742CC
		private void AddShopWorkerDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("workshop_worker_talk_to_disguised_character", "start", "close_window", "{=qGIFpahv}Can't spare any coins for you. Sorry. May Heaven provide.", new ConversationSentence.OnConditionDelegate(this.conversation_disguise_start_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_npc_owner_begin", "start", "shopworker_npc_player", "{=!}{WORKSHOP_INTRO_LINE}", new ConversationSentence.OnConditionDelegate(this.workshop_npc_owner_begin_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_1", "start_2", "shopworker_npc_player", "{=XZgD99ol}Anything else I can do for you?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("workshop_2", "shopworker_npc_player", "player_ask_questions", "{=HbaziRMP}I have some questions.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("workshop_3", "shopworker_npc_player", "workshop_buy_result", "{=p3a44dQN}I would like to buy this workshop.", null, null, 100, new ConversationSentence.OnClickableConditionDelegate(this.workshop_buy_clickable_condition), null);
			campaignGameStarter.AddDialogLine("workshop_buy_result", "workshop_buy_result", "start_2", "{=icmr0jSa}This workshop belongs to {OWNER.LINK}. You need to talk to {?OWNER.GENDER}her{?}him{\\?}.", new ConversationSentence.OnConditionDelegate(this.workshop_12_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("workshop_4", "shopworker_npc_player", "workshop_end_dialog", "{=90YOVmcG}Good day to you.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_5", "workshop_end_dialog", "close_window", "{=QwAyt4aW}Have a nice day, {?PLAYER.GENDER}madam{?}sir{\\?}.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_6", "player_ask_questions", "player_ask_questions2", "{=!}{NPC_ANSWER}", new ConversationSentence.OnConditionDelegate(this.player_ask_questions_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_7", "player_ask_questions_return", "player_ask_questions2", "{=1psOym3y}Any other questions?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("workshop_8", "player_ask_questions2", "player_ask_questions3", "{=hmmoXy0E}Whose workshop is this?", new ConversationSentence.OnConditionDelegate(this.workshop_player_not_owner_on_condition), null, 100, null, null);
			campaignGameStarter.AddPlayerLine("workshop_9", "player_ask_questions2", "player_ask_questions4", "{=5siWBRk8}What do you produce here?", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("workshop_10", "player_ask_questions2", "player_ask_questions5", "{=v0HhVu4z}How do workshops work in general?", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("workshop_11", "player_ask_questions2", "start_2", "{=rXbL9mhQ}I want to talk about other things.", new ConversationSentence.OnConditionDelegate(this.workshop_player_not_owner_on_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_12", "player_ask_questions3", "player_ask_questions_return", "{=aE0kPqcT}This workshop belongs to {OWNER.LINK}, {?PLAYER.GENDER}madam{?}sir{\\?}.", new ConversationSentence.OnConditionDelegate(this.workshop_12_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_13", "player_ask_questions4", "player_ask_questions_return", "{=LXtebqEF}This a {.%}{WORKSHOP_TYPE}{.%}, {?PLAYER.GENDER}madam{?}sir{\\?}. {WORKSHOP_DESCRIPTION}", new ConversationSentence.OnConditionDelegate(this.workshop_13_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_14", "player_ask_questions5", "player_ask_questions_return", "{=QKsaPj6w}We take raw materials and produce goods and sell them at the local market. After paying the workers their wages we transfer the profits to the owner.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_player_owner_begin", "start", "player_options", "{=VO8qBWrv}Hey boss, come to check on the business?", new ConversationSentence.OnConditionDelegate(this.workshop_player_owner_begin_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_26", "shopkeeper_start", "player_options", "{=XZgD99ol}Anything else I can do for you?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("workshop_27", "player_options", "player_ask_questions", "{=uotflvH2}I would like to ask some questions.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("workshop_28", "player_options", "player_change_production", "{=b92969l9}I would like to change what you are producing here.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("workshop_29", "player_options", "player_sell_workshop", "{=z3BeN9ro}I would like to sell this workshop.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("workshop_30", "player_options", "player_workshop_end_dialog", "{=90YOVmcG}Good day to you.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_31", "player_workshop_end_dialog", "close_window", "{=NyxcGLxf}At your service.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_32", "player_workshop_questions", "player_workshop_questions_start", "{=Y4LhmAdi}Sure, boss. Go ahead.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_33", "player_workshop_questions_return", "player_workshop_questions_start", "{=1psOym3y}Any other questions?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("workshop_34", "player_ask_questions2", "shopkeeper_start", "{=rXbL9mhQ}I want to talk about other things.", new ConversationSentence.OnConditionDelegate(this.workshop_player_is_owner_on_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_35", "player_sell_workshop", "player_sell_price", "{=XQa48r6e}If you say so, boss. So, if I sell everything and transfer you the money we have on hand, you can get {PRICE}{GOLD_ICON}. You sure you want to sell?", new ConversationSentence.OnConditionDelegate(this.conversation_shopworker_sell_player_workshop_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("workshop_37", "player_sell_price", "player_sell_warehouse_done", "{=tc4DyxaL}Yes, I would like to sell.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("workshop_38", "player_sell_price", "player_sell_decline", "{=bi7ZhwNO}No, we are doing fine.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_41", "player_sell_warehouse_done", "player_sell_warehouse_done_2", "{=eemYX2d4}I will transfer the goods from your warehouse to your party immediately.", new ConversationSentence.OnConditionDelegate(this.conversation_shopworker_sell_player_and_workshop_warehouse_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("workshop_42", "player_sell_warehouse_done_2", "player_sell_warehouse_done", "{=eALf5d30}Thanks!", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_shopworker_player_sell_workshop_on_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_39", "player_sell_warehouse_done", "close_window", "{=EPb2BDuo}We had a good run, boss. Maybe we can work together again in the future. For now, it will take some time to pack things up.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_shopworker_player_sell_workshop_on_consequence), 100, null);
			campaignGameStarter.AddDialogLine("workshop_40", "player_sell_decline", "shopkeeper_start", "{=DBTOhVl0}I'm glad you reconsidered, boss. Maybe it's not my place to say, but I think you should never let go of a good investment.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("workshop_41_1", "player_change_production", "player_change_production_1", "{=9eLyeU7M}Sure, boss. It will cost us {COST}{GOLD_ICON} for new equipment. Shall we go ahead?", new ConversationSentence.OnConditionDelegate(this.conversation_player_workshop_change_production_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("workshop_42_2", "player_change_production_1", "player_change_production_2", "{=kB65SzbF}Yes.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_player_workshop_change_production_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.workshop_42_on_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("workshop_43", "player_change_production_1", "shopkeeper_start", "{=8OkPHu4f}No", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_44", "player_change_production_2", "player_change_production_2_1", "{=WAa4yaTo}What do you want us to make?", null, null, 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("workshop_45", "player_change_production_2_1", "player_change_production_3", "{=!}{BUILDING}", "{=5z4hEq68}I am thinking of a different kind of workshop.", "player_change_production_2", new ConversationSentence.OnConditionDelegate(this.conversation_player_workshop_player_decision_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_player_workshop_player_decision_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("workshop_cancel", "player_change_production_2_1", "shopkeeper_start", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_46", "player_change_production_3", "player_change_production_5", "{=8d7y1IHp}All right. Just to confirm - {.%}{.a} {WORKSHOP_TYPE}{.%} is what you want?", new ConversationSentence.OnConditionDelegate(this.workshop_46_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("workshop_50", "player_change_production_5", "player_change_production_end", "{=aeouhelq}Yes", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_47", "player_change_production_end", "close_window", "{=Imes09et}Okay, then. We'll take that silver and buy some tools and and hire some workers with the proper skills.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_player_workshop_player_changed_production_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("workshop_48", "player_change_production_5", "player_change_production_2", "{=EAb4hSDP}Let me think again.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("workshop_49", "player_change_production_5", "shopkeeper_start", "{=QHbcFrPX}On second thought, I don't want to change what we are producing. Go on like this.", null, null, 100, null, null);
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x0017678A File Offset: 0x0017498A
		private bool conversation_disguise_start_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.ShopWorker && Campaign.Current.IsMainHeroDisguised;
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x001767A6 File Offset: 0x001749A6
		private bool conversation_shopworker_sell_player_and_workshop_warehouse_on_condition()
		{
			return Hero.MainHero.OwnedWorkshops.Count<Workshop>((Workshop x) => x.Settlement == Settlement.CurrentSettlement) == 1;
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x001767DC File Offset: 0x001749DC
		private bool workshop_npc_owner_begin_on_condition()
		{
			if (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.ShopWorker)
			{
				Workshop workshop = this.FindCurrentWorkshop();
				if (workshop != null && workshop.Owner != Hero.MainHero && workshop.WorkshopType != null)
				{
					if (workshop.WorkshopType.StringId == "smithy" || workshop.WorkshopType.StringId == "silversmithy")
					{
						MBTextManager.SetTextVariable("WORKSHOP_INTRO_LINE", "{=KTgOXBPN}Hot around here, eh {?PLAYER.GENDER}madame{?}sir{\\?}? I need to get back to the forges in a minute, but how can I help you?", false);
					}
					else if (workshop.WorkshopType.StringId == "tannery")
					{
						MBTextManager.SetTextVariable("WORKSHOP_INTRO_LINE", "{=4pkfkSbe}Sorry about the smell around here, {?PLAYER.GENDER}madame{?}sir{\\?}. Tanning's like that, sorry to say. How can I help you?", false);
					}
					else if (workshop.WorkshopType.StringId == "pottery_shop")
					{
						MBTextManager.SetTextVariable("WORKSHOP_INTRO_LINE", "{=lUjCslGK}Hope you don't mind the smoke around here, {?PLAYER.GENDER}madame{?}sir{\\?}. Got to keep the kilns hot. How can I help you?", false);
					}
					else if (workshop.WorkshopType.StringId == "brewery")
					{
						MBTextManager.SetTextVariable("WORKSHOP_INTRO_LINE", "{=keLJvk37}I just have a minute, {?PLAYER.GENDER}madame{?}sir{\\?}. I need to check on a batch that's brewing. How can I help you?", false);
					}
					else if (workshop.WorkshopType.StringId == "wool_weavery" || workshop.WorkshopType.StringId == "linen_weavery" || workshop.WorkshopType.StringId == "velvet_weavery")
					{
						MBTextManager.SetTextVariable("WORKSHOP_INTRO_LINE", "{=1xyj3HW0}I just have a minute here, {?PLAYER.GENDER}madame{?}sir{\\?}. I need to get back to the loom. How can I help you?", false);
					}
					else if (workshop.WorkshopType.StringId == "olive_press" || workshop.WorkshopType.StringId == "wine_press")
					{
						MBTextManager.SetTextVariable("WORKSHOP_INTRO_LINE", "{=PRiOVcO9}Careful around here, {?PLAYER.GENDER}madame{?}sir{\\?}. Don't want to get your hands caught in one of the presses. How can I help you?", false);
					}
					else if (workshop.WorkshopType.StringId == "wood_WorkshopType")
					{
						MBTextManager.SetTextVariable("WORKSHOP_INTRO_LINE", "{=kMdBpDu1}Hope you don't mind all the sawdust around here, {?PLAYER.GENDER}madame{?}sir{\\?}. How can I help you?", false);
					}
					else
					{
						MBTextManager.SetTextVariable("WORKSHOP_INTRO_LINE", "{=TnZ9Mynm}Right... So, {?PLAYER.GENDER}madame{?}sir{\\?}, how can I help you?", false);
					}
					MBTextManager.SetTextVariable("WORKSHOP_TYPE", workshop.WorkshopType.Name, false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x001769D2 File Offset: 0x00174BD2
		private bool workshop_buy_clickable_condition(out TextObject explanation)
		{
			return this.player_war_status_clickable_condition(out explanation);
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x001769DC File Offset: 0x00174BDC
		private bool workshop_12_on_condition()
		{
			Workshop workshop = this.FindCurrentWorkshop();
			StringHelpers.SetCharacterProperties("OWNER", workshop.Owner.CharacterObject, null, false);
			return true;
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x00176A0C File Offset: 0x00174C0C
		private bool player_ask_questions_on_condition()
		{
			if (this.FindCurrentWorkshop().Owner == Hero.MainHero)
			{
				MBTextManager.SetTextVariable("NPC_ANSWER", new TextObject("{=Hw4mTZxm}Sure, boss. Ask me anything.", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("NPC_ANSWER", new TextObject("{=AbIUjOLZ}Sure. What do you want to know?", null), false);
			}
			return true;
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x00176A5C File Offset: 0x00174C5C
		private bool workshop_13_on_condition()
		{
			Workshop workshop = this.FindCurrentWorkshop();
			MBTextManager.SetTextVariable("WORKSHOP_TYPE", workshop.WorkshopType.Name, false);
			MBTextManager.SetTextVariable("WORKSHOP_DESCRIPTION", workshop.WorkshopType.Description, false);
			return true;
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x00176AA0 File Offset: 0x00174CA0
		private bool workshop_player_owner_begin_on_condition()
		{
			if (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.ShopWorker)
			{
				Workshop workshop = this.FindCurrentWorkshop();
				return workshop != null && workshop.Owner == Hero.MainHero;
			}
			return false;
		}

		// Token: 0x06004A62 RID: 19042 RVA: 0x00176AD8 File Offset: 0x00174CD8
		private bool workshop_player_not_owner_on_condition()
		{
			Workshop workshop = this.FindCurrentWorkshop();
			return workshop != null && workshop.Owner != Hero.MainHero;
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x00176B04 File Offset: 0x00174D04
		private bool workshop_player_is_owner_on_condition()
		{
			Workshop workshop = this.FindCurrentWorkshop();
			return workshop != null && workshop.Owner == Hero.MainHero;
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x00176B2C File Offset: 0x00174D2C
		private bool workshop_42_on_clickable_condition(out TextObject explanation)
		{
			Workshop workshop = this.FindCurrentWorkshop();
			if (Hero.MainHero.Gold < Campaign.Current.Models.WorkshopModel.GetConvertProductionCost(workshop.WorkshopType))
			{
				explanation = new TextObject("{=EASiM8NU}You haven't got enough denars to change production.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x00176B79 File Offset: 0x00174D79
		private bool workshop_46_on_condition()
		{
			MBTextManager.SetTextVariable("WORKSHOP_DESCRIPTION", this._lastSelectedWorkshopType.Description, false);
			MBTextManager.SetTextVariable("WORKSHOP_TYPE", this._lastSelectedWorkshopType.Name, false);
			return true;
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x00176BA8 File Offset: 0x00174DA8
		private bool conversation_shopworker_sell_player_workshop_on_condition()
		{
			Workshop workshop = this.FindCurrentWorkshop();
			int costForNotable = Campaign.Current.Models.WorkshopModel.GetCostForNotable(workshop);
			MBTextManager.SetTextVariable("PRICE", costForNotable);
			return true;
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x00176BE0 File Offset: 0x00174DE0
		private void conversation_shopworker_player_sell_workshop_on_consequence()
		{
			Workshop workshop = this.FindCurrentWorkshop();
			if (workshop.Owner == Hero.MainHero)
			{
				Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(workshop);
				ChangeOwnerOfWorkshopAction.ApplyByPlayerSelling(workshop, notableOwnerForWorkshop, workshop.WorkshopType);
			}
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x00176C24 File Offset: 0x00174E24
		private bool conversation_player_workshop_change_production_on_condition()
		{
			Workshop workshop = this.FindCurrentWorkshop();
			int convertProductionCost = Campaign.Current.Models.WorkshopModel.GetConvertProductionCost(workshop.WorkshopType);
			MBTextManager.SetTextVariable("COST", convertProductionCost);
			return true;
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x00176C60 File Offset: 0x00174E60
		private void conversation_player_workshop_change_production_on_consequence()
		{
			Workshop currentWorkshop = this.FindCurrentWorkshop();
			ConversationSentence.SetObjectsToRepeatOver(WorkshopType.All.Where<WorkshopType>((WorkshopType x) => x != currentWorkshop.WorkshopType && !x.IsHidden).ToList<WorkshopType>(), 5);
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x00176CA0 File Offset: 0x00174EA0
		private bool conversation_player_workshop_player_decision_on_condition()
		{
			WorkshopType workshopType = ConversationSentence.CurrentProcessedRepeatObject as WorkshopType;
			if (workshopType != null)
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("BUILDING", workshopType.Name);
				return true;
			}
			return false;
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x00176CD4 File Offset: 0x00174ED4
		private void conversation_player_workshop_player_decision_on_consequence()
		{
			this._lastSelectedWorkshopType = ConversationSentence.SelectedRepeatObject as WorkshopType;
		}

		// Token: 0x06004A6C RID: 19052 RVA: 0x00176CE8 File Offset: 0x00174EE8
		private void conversation_player_workshop_player_changed_production_on_consequence()
		{
			Workshop workshop = this.FindCurrentWorkshop();
			if (workshop.WorkshopType != this._lastSelectedWorkshopType)
			{
				ChangeProductionTypeOfWorkshopAction.Apply(workshop, this._lastSelectedWorkshopType, false);
			}
		}

		// Token: 0x040014D5 RID: 5333
		public const float WorkerSpawnPercentage = 0.33f;

		// Token: 0x040014D6 RID: 5334
		private WorkshopType _lastSelectedWorkshopType;

		// Token: 0x040014D7 RID: 5335
		private Workshop _lastSelectedWorkshop;

		// Token: 0x040014D8 RID: 5336
		private Workshop _currentWorkshop;
	}
}
