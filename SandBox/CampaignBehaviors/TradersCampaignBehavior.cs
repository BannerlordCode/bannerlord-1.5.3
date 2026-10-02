using System;
using Helpers;
using SandBox.Missions.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000E8 RID: 232
	public class TradersCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000B5A RID: 2906 RVA: 0x00053E36 File Offset: 0x00052036
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00053E4F File Offset: 0x0005204F
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00053E51 File Offset: 0x00052051
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00053E5C File Offset: 0x0005205C
		protected void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("weaponsmith_talk_start_normal", "start", "weaponsmith_talk_player", "{=!}{TRADER_GREETING}", new ConversationSentence.OnConditionDelegate(this.conversation_weaponsmith_talk_start_normal_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_start_to_player_in_disguise", "start", "close_window", "{=1auLEn9y}Look, my good {?PLAYER.GENDER}woman{?}man{\\?}, these are hard times for sure, but I need you to move along. You'll scare away my customers.", new ConversationSentence.OnConditionDelegate(this.conversation_weaponsmith_talk_start_to_player_in_disguise_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_initial", "weaponsmith_begin", "weaponsmith_talk_player", "{=jxw54Ijt}Okay, is there anything more I can help with?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("weaponsmith_talk_player_1", "weaponsmith_talk_player", "merchant_response_1", "{=ExltvaKo}Let me see what you have for sale...", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("weaponsmith_talk_player_request_craft", "weaponsmith_talk_player", "merchant_response_crafting", "{=w1vzpCNi}I need you to craft a weapon for me", new ConversationSentence.OnConditionDelegate(this.conversation_open_crafting_on_condition), null, 100, null, null);
			campaignGameStarter.AddPlayerLine("weaponsmith_talk_player_3", "weaponsmith_talk_player", "merchant_response_3", "{=8hNYr2VX}I was just passing by.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_merchant_response_1", "merchant_response_1", "player_merchant_talk_close", "{=K5mG9nDv}With pleasure.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_merchant_response_2", "merchant_response_2", "player_merchant_talk_2", "{=5bRQ0gt7}How many men do you need for it? For each men I want 100{GOLD_ICON}.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_merchant_response_craft", "merchant_response_crafting", "player_merchant_craft_talk_close", "{=lF5HkBDy}As you wish.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_merchant_craft_opened", "player_merchant_craft_talk_close", "close_window", "{=TD8Jxn7U}Have a nice day my {?PLAYER.GENDER}lady{?}lord{\\?}.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_weaponsmith_craft_on_consequence), 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_merchant_response_3", "merchant_response_3", "close_window", "{=FpNWdIaT}Yes, of course. Just ask me if there is anything you need.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_end", "player_merchant_talk_close", "close_window", "{=Yh0danUf}Thank you and good day my {?PLAYER.GENDER}lady{?}lord{\\?}.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_weaponsmith_talk_player_on_consequence), 100, null);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00054023 File Offset: 0x00052223
		private bool conversation_open_crafting_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter != null && CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Blacksmith;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0005403C File Offset: 0x0005223C
		private bool conversation_weaponsmith_talk_start_normal_on_condition()
		{
			if (!this.IsTrader())
			{
				return false;
			}
			if (!Campaign.Current.IsMainHeroDisguised)
			{
				MBTextManager.SetTextVariable("TRADER_GREETING", "{=7IxFrati}Greetings my {?PLAYER.GENDER}lady{?}lord{\\?}, how may I help you?", false);
				return true;
			}
			if (Mission.Current.GetMissionBehavior<DisguiseMissionLogic>().ContactAlreadySetCommonCondition() && Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.SmugglerConnections))
			{
				MBTextManager.SetTextVariable("TRADER_GREETING", "{=bqg2gS7i}Ah, a friend of a friend. How may I help you?", false);
				return true;
			}
			return false;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x000540A6 File Offset: 0x000522A6
		private bool conversation_weaponsmith_talk_start_to_player_in_disguise_on_condition()
		{
			return this.IsTrader() && !this.conversation_weaponsmith_talk_start_normal_on_condition();
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x000540BB File Offset: 0x000522BB
		private bool IsTrader()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Weaponsmith || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Armorer || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.HorseTrader || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.GoodsTrader;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x000540F8 File Offset: 0x000522F8
		private void conversation_weaponsmith_talk_player_on_consequence()
		{
			InventoryScreenHelper.InventoryCategoryType inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.None;
			Occupation occupation = CharacterObject.OneToOneConversationCharacter.Occupation;
			if (occupation != Occupation.GoodsTrader)
			{
				switch (occupation)
				{
				case Occupation.Weaponsmith:
					inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.Weapon;
					break;
				case Occupation.Armorer:
					inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.Armors;
					break;
				case Occupation.HorseTrader:
					inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.HorseCategory;
					break;
				default:
					if (occupation == Occupation.Blacksmith)
					{
						inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.Weapon;
					}
					break;
				}
			}
			else
			{
				inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.Goods;
			}
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (Mission.Current != null)
			{
				InventoryScreenHelper.OpenScreenAsTrade(currentSettlement.ItemRoster, currentSettlement.Town, inventoryCategoryType, new Action(this.OnInventoryScreenDone));
				return;
			}
			InventoryScreenHelper.OpenScreenAsTrade(currentSettlement.ItemRoster, currentSettlement.Town, inventoryCategoryType, null);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00054183 File Offset: 0x00052383
		private void conversation_weaponsmith_craft_on_consequence()
		{
			CraftingHelper.OpenCrafting(CraftingTemplate.All[0], null);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00054198 File Offset: 0x00052398
		private void OnInventoryScreenDone()
		{
			foreach (Agent agent in Mission.Current.Agents)
			{
				CharacterObject characterObject = (CharacterObject)agent.Character;
				if (agent.IsHuman && characterObject != null && characterObject.IsHero && characterObject.HeroObject.PartyBelongedTo == MobileParty.MainParty && (!agent.IsMainAgent || !Campaign.Current.IsMainHeroDisguised))
				{
					agent.UpdateSpawnEquipmentAndRefreshVisuals(Mission.Current.DoesMissionRequireCivilianEquipment ? characterObject.FirstCivilianEquipment : characterObject.FirstBattleEquipment);
				}
			}
		}
	}
}
