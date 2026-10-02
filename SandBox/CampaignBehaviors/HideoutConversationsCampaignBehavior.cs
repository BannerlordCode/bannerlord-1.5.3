using System;
using SandBox.Missions.MissionLogics.Hideout;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000DF RID: 223
	public class HideoutConversationsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x0004E80F File Offset: 0x0004CA0F
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0004E828 File Offset: 0x0004CA28
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0004E82A File Offset: 0x0004CA2A
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0004E834 File Offset: 0x0004CA34
		private void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("bandit_hideout_start_defender", "start", "bandit_hideout_defender", "{=nYCXzAYH}You! You've cut quite a swathe through my men there, damn you. How about we settle this, one-on-one?", new ConversationSentence.OnConditionDelegate(this.bandit_hideout_start_defender_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("bandit_hideout_start_defender_1", "bandit_hideout_defender", "close_window", "{=dzXaXKaC}Very well.", null, new ConversationSentence.OnConsequenceDelegate(this.bandit_hideout_start_duel_fight_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("bandit_hideout_start_defender_2", "bandit_hideout_defender", "close_window", "{=ukRZd2AA}I don't fight duels with brigands.", null, new ConversationSentence.OnConsequenceDelegate(this.bandit_hideout_continue_battle_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.bandit_hideout_continue_battle_on_clickable_condition), null);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0004E8D0 File Offset: 0x0004CAD0
		private bool bandit_hideout_start_defender_on_condition()
		{
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			if (encounteredParty != null)
			{
				IFaction mapFaction = encounteredParty.MapFaction;
				bool? flag = ((mapFaction != null) ? new bool?(mapFaction.IsBanditFaction) : null);
				bool flag2 = true;
				if (((flag.GetValueOrDefault() == flag2) & (flag != null)) && encounteredParty != null)
				{
					Settlement settlement = encounteredParty.Settlement;
					flag = ((settlement != null) ? new bool?(settlement.IsHideout) : null);
					flag2 = true;
					if ((flag.GetValueOrDefault() == flag2) & (flag != null))
					{
						Mission mission = Mission.Current;
						if (((mission != null) ? mission.GetMissionBehavior<HideoutMissionController>() : null) == null)
						{
							Mission mission2 = Mission.Current;
							return ((mission2 != null) ? mission2.GetMissionBehavior<HideoutAmbushMissionController>() : null) != null;
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0004E984 File Offset: 0x0004CB84
		private void bandit_hideout_start_duel_fight_on_consequence()
		{
			if (Mission.Current.GetMissionBehavior<HideoutMissionController>() != null)
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutMissionController.StartBossFightDuelMode;
				return;
			}
			if (Mission.Current.GetMissionBehavior<HideoutAmbushMissionController>() != null)
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutAmbushMissionController.StartBossFightDuelMode;
			}
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0004E9E0 File Offset: 0x0004CBE0
		private void bandit_hideout_continue_battle_on_consequence()
		{
			if (Mission.Current.GetMissionBehavior<HideoutMissionController>() != null)
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutMissionController.StartBossFightBattleMode;
				return;
			}
			if (Mission.Current.GetMissionBehavior<HideoutAmbushMissionController>() != null)
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutAmbushMissionController.StartBossFightBattleMode;
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0004EA3C File Offset: 0x0004CC3C
		private bool bandit_hideout_continue_battle_on_clickable_condition(out TextObject explanation)
		{
			bool flag = false;
			foreach (Agent agent in Mission.Current.PlayerTeam.ActiveAgents)
			{
				if (!agent.IsMount && agent.Character != CharacterObject.PlayerCharacter)
				{
					flag = true;
					break;
				}
			}
			explanation = TextObject.GetEmpty();
			if (!flag)
			{
				explanation = new TextObject("{=F9HxO1iS}You don't have any men.", null);
			}
			return flag;
		}
	}
}
