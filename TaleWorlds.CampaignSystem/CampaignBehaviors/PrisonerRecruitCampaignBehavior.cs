using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000459 RID: 1113
	public class PrisonerRecruitCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060047C2 RID: 18370 RVA: 0x00160A12 File Offset: 0x0015EC12
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x00160A2B File Offset: 0x0015EC2B
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<CharacterObject, float>>("PrisonerTalkRecords", ref this.PrisonerTalkRecords);
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x00160A3F File Offset: 0x0015EC3F
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x00160A48 File Offset: 0x0015EC48
		protected void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("conversation_prisoner_chat_start", "start", "prisoner_recruit_start_player", "{=k7ebznzr}Yes?", new ConversationSentence.OnConditionDelegate(this.conversation_prisoner_chat_start_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("conversation_prisoner_chat_player", "prisoner_recruit_start_player", "prisoner_recruit_start_response", "{=ksZXyDJG}Don't do anything stupid, like trying to run away. I will be watching you.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("conversation_prisoner_chat_response", "prisoner_recruit_start_response", "close_window", "{=Oe1bTJp6}No, I swear I won't.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("conversation_prisoner_recruit_start_1", "start", "prisoner_recruit_start", "{=!}I'm going to take a chance on you, to give you a chance to walk free, if you like.", new ConversationSentence.OnConditionDelegate(this.conversation_prisoner_recruit_start_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("conversation_prisoner_recruit_start", "prisoner_recruit_start", "prisoner_recruit", "{=!}Are you willing to join us? To fight alongside us?", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("prisoner_recruit_1", "prisoner_recruit", "close_window", "{=!}Aye. I would do that.", new ConversationSentence.OnConditionDelegate(this.conversation_prisoner_recruit_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("prisoner_recruit_2", "prisoner_recruit", "close_window", "{=!}No. I'm no traitor.", new ConversationSentence.OnConditionDelegate(this.conversation_prisoner_recruit_no_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("prisoner_recruit_3", "prisoner_recruit", "close_window", "{=!}You heard me the first time. You know where to stick your offer.", null, null, 100, null);
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x00160B84 File Offset: 0x0015ED84
		private bool conversation_prisoner_chat_start_on_condition()
		{
			bool flag = (CharacterObject.OneToOneConversationCharacter.IsHero && (Hero.OneToOneConversationHero.PartyBelongedTo == null || !Hero.OneToOneConversationHero.PartyBelongedTo.IsActive)) || (CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.PrisonGuard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.Guard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.CaravanGuard && MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsMainParty);
			return MobileParty.MainParty.PrisonRoster.Contains(CharacterObject.OneToOneConversationCharacter) && flag;
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x00160C18 File Offset: 0x0015EE18
		private bool conversation_prisoner_recruit_start_on_condition()
		{
			bool flag = !CharacterObject.OneToOneConversationCharacter.IsHero && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.PrisonGuard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.Guard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.CaravanGuard && MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsMainParty;
			bool flag2 = MobileParty.MainParty.PrisonRoster.Contains(CharacterObject.OneToOneConversationCharacter);
			if (flag2 && !this.PrisonerTalkRecords.ContainsKey(CharacterObject.OneToOneConversationCharacter))
			{
				this.PrisonerTalkRecords.Add(CharacterObject.OneToOneConversationCharacter, -1f);
			}
			return flag2 && flag;
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x00160CB4 File Offset: 0x0015EEB4
		public bool conversation_prisoner_recruit_on_condition()
		{
			bool flag = false;
			float num;
			if (this.PrisonerTalkRecords.TryGetValue(CharacterObject.OneToOneConversationCharacter, out num) && (num < 0f || Campaign.CurrentTime - num >= 5f))
			{
				flag = MBRandom.RandomInt(MBMath.ClampInt(150 - CharacterObject.PlayerCharacter.GetSkillValue(DefaultSkills.Steward), 1, 150)) < 30;
				if (flag)
				{
					this.PrisonerTalkRecords.Remove(CharacterObject.OneToOneConversationCharacter);
					int num2 = MobileParty.MainParty.PrisonRoster.FindIndexOfTroop(CharacterObject.OneToOneConversationCharacter);
					if (num2 != -1)
					{
						TroopRosterElement elementCopyAtIndex = MobileParty.MainParty.PrisonRoster.GetElementCopyAtIndex(num2);
						MobileParty.MainParty.PrisonRoster.AddToCounts(elementCopyAtIndex.Character, -elementCopyAtIndex.Number, false, 0, 0, true, -1);
						MobileParty.MainParty.MemberRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, 0, 0, true, -1);
					}
				}
			}
			return flag;
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x00160DA4 File Offset: 0x0015EFA4
		public bool conversation_prisoner_recruit_no_on_condition()
		{
			bool flag = false;
			float num;
			if (this.PrisonerTalkRecords.TryGetValue(CharacterObject.OneToOneConversationCharacter, out num) && num < 0f)
			{
				this.PrisonerTalkRecords[CharacterObject.OneToOneConversationCharacter] = Campaign.CurrentTime;
				flag = true;
			}
			return flag;
		}

		// Token: 0x04001456 RID: 5206
		private Dictionary<CharacterObject, float> PrisonerTalkRecords = new Dictionary<CharacterObject, float>();
	}
}
