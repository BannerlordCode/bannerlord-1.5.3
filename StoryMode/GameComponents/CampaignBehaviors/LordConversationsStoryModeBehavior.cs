using System;
using Helpers;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x0200004F RID: 79
	public class LordConversationsStoryModeBehavior : CampaignBehaviorBase
	{
		// Token: 0x060004E6 RID: 1254 RVA: 0x0001B8F8 File Offset: 0x00019AF8
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001B911 File Offset: 0x00019B11
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001B913 File Offset: 0x00019B13
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001B91C File Offset: 0x00019B1C
		private void AddDialogs(CampaignGameStarter starter)
		{
			starter.AddDialogLine("anti_imperial_mentor_introduction", "lord_introduction", "lord_start", "{=TB20aFsf}You probably are aware that I am {CONVERSATION_HERO.FIRSTNAME}. I am not sure why you have sought me out, but know that my old life, as imperial lap-dog, is over.", new ConversationSentence.OnConditionDelegate(this.conversation_anti_imperial_mentor_introduction_on_condition), null, 150, null);
			starter.AddDialogLine("imperial_mentor_introduction", "lord_introduction", "lord_start", "{=6aDiS9eP}I am {CONVERSATION_HERO.FIRSTNAME}. You probably already know that, though. Once I wielded great power, but now... Anyway, I am most curious what you might want with me.", new ConversationSentence.OnConditionDelegate(this.conversation_imperial_mentor_introduction_on_condition), null, 150, null);
			starter.AddDialogLine("start_default_for_mentors", "start", "lord_start", "{=!}{PLAYER.NAME}...", new ConversationSentence.OnConditionDelegate(this.start_default_for_mentors_on_condition), null, 150, null);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001B9B3 File Offset: 0x00019BB3
		private bool conversation_imperial_mentor_introduction_on_condition()
		{
			if (Campaign.Current.ConversationManager.CurrentConversationIsFirst && Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor)
			{
				StringHelpers.SetCharacterProperties("CONVERSATION_HERO", CharacterObject.OneToOneConversationCharacter, null, false);
				return true;
			}
			return false;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001B9E7 File Offset: 0x00019BE7
		private bool conversation_anti_imperial_mentor_introduction_on_condition()
		{
			if (Campaign.Current.ConversationManager.CurrentConversationIsFirst && Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor)
			{
				StringHelpers.SetCharacterProperties("CONVERSATION_HERO", CharacterObject.OneToOneConversationCharacter, null, false);
				return true;
			}
			return false;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001BA1B File Offset: 0x00019C1B
		private bool start_default_for_mentors_on_condition()
		{
			return Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.HasMet && (Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor || Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor);
		}
	}
}
