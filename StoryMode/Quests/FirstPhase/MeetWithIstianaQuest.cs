using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.FirstPhase
{
	// Token: 0x02000037 RID: 55
	public class MeetWithIstianaQuest : StoryModeQuestBase
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00012BD4 File Offset: 0x00010DD4
		private TextObject _startQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=h9VP4ypW}Find and meet {HERO.LINK} to learn more about Neretzes' Banner. She is currently in {SETTLEMENT}.", null);
				StringHelpers.SetCharacterProperties("HERO", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("SETTLEMENT", StoryModeHeroes.ImperialMentor.CurrentSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00012C20 File Offset: 0x00010E20
		private TextObject _endQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=kTaYz2mo}You talked with {HERO.NAME}.", null);
				StringHelpers.SetCharacterProperties("HERO", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00012C54 File Offset: 0x00010E54
		public override TextObject Title
		{
			get
			{
				TextObject textObject = new TextObject("{=Y6SqyQwn}Meet with {HERO.NAME}", null);
				StringHelpers.SetCharacterProperties("HERO", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00012C85 File Offset: 0x00010E85
		public override bool IsRemainingTimeHidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00012C88 File Offset: 0x00010E88
		public MeetWithIstianaQuest(Settlement settlement)
			: base("meet_with_istiana_story_mode_quest", null, StoryModeManager.Current.MainStoryLine.FirstPhase.FirstPhaseEndTime)
		{
			this._metImperialMentor = false;
			this.SetDialogs();
			HeroHelper.SpawnHeroForTheFirstTime(StoryModeHeroes.ImperialMentor, settlement);
			base.AddTrackedObject(settlement);
			base.AddTrackedObject(StoryModeHeroes.ImperialMentor);
			base.AddLog(this._startQuestLog, false);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00012CED File Offset: 0x00010EED
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00012CF5 File Offset: 0x00010EF5
		protected override void HourlyTick()
		{
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00012CF8 File Offset: 0x00010EF8
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("lord_start", 110).NpcLine(new TextObject("{=5UHbg6D0}So. What brings you to me?", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor && !this._metImperialMentor)
				.PlayerLine(new TextObject("{=tfm5hcks}I believe I have a piece of the Dragon Banner of Calradios.", null), null, null, null)
				.NpcLine(new TextObject("{=0P4HqZiB}Is that true?[ib:normal][if:convo_shocked]", null), null, null, null, null)
				.NpcLine(new TextObject("{=ZDEcFXIm}You may have one piece of the banner, but it's of little use in itself. You'll have to find the other parts. But once you can bring together the pieces, you'll have something of tremendous value.[if:convo_undecided_open]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=t71lPdyb}How so?", null), null, null, null)
				.NpcLine(new TextObject("{=40oa2Nav}The banner of Calradios is part of a legend. They say it was carried by Calradios the Great as he led his small band of exiles into this land to make a new home for themselves. They say that, so long as it is carried by a true son of Calradios, he shall never be defeated in battle. Or a daughter, I imagine, although that has never come up.[ib:confident][if:convo_undecided_closed]", null), null, null, null, null)
				.NpcLine(new TextObject("{=xjduipCO}Of course our glorious armies have been defeated many times, but I guess those commanders and emperors were not 'true sons.' Clever little legend. A child could see through it, if she tried, but of course people never try to see through the noble lies that bind us together. Thank Heaven for that.[ib:closed][if:convo_pondering]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=FBp2ranI}So, can you help me find a buyer for it?", null), null, null, null)
				.NpcLine(new TextObject("{=WWcG7kPr}A 'buyer'? Think bigger than that. Let me just say that, if you can find the missing pieces, I am sure I can help you put it to good use.[ib:confident2][if:convo_calm_friendly]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=MnmblprY}So, where can I find the other pieces?", null), null, null, null)
				.NpcLine(new TextObject("{=jnOa3cbK}Before I reveal that information to you, I need to know more about your intentions. One could use the banner to restore the empire, but one could also use the banner to destroy it.[ib:closed][if:convo_angry_voice]", null), null, null, null, null)
				.NpcLine(new TextObject("{=DuUhVWaV}Let me tell you about myself... I was a confidant of the old emperor Neretzes. Officially I was not his spymaster, as I am a woman, but that was the role I played nonetheless. I liked Neretzes, and was very grateful for his trust, but he was not a good emperor. Too stubborn and principled. I probably should have poisoned him.[ib:demure2][if:convo_empathic_voice]", null), null, null, null, null)
				.NpcLine(new TextObject("{=bWdsH2Ls}This is what I learned from a lifetime in politics: There is nothing worse than disorder. Suffice to say that I know better than anyone about the lies and cruelty that kept the Empire alive. But all the murders I ever committed in 10 years of serving Neretzes do not amount to the death toll in a single hour when an army storms a town.[if:convo_snide_voice]", null), null, null, null, null)
				.NpcLine(new TextObject("{=GDNXavAl}There's nothing special about our Empire. [if:convo_calm_friendly]Any one of these petty kings and khans and sultans could probably get lucky and conquer Calradia and do as good a job ruling it as we did. But the point is - we already did it. Our greatest crimes are in the past. Let us not undo what has already been done.", null), null, null, null, null)
				.NpcLine(new TextObject("{=KXj8bsao}So... If you intend to use the banner to save the Empire, I'll tell you what I know. But if you want to go backward, not forward, then I will not help you.[ib:closed][if:convo_undecided_closed]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.ActivateAssembleTheBannerQuest))
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=IavuL9KI}Of course. I intend to use the banner to help save the Empire.", null), null, null, null)
				.NpcLine(new TextObject("{=JRQ7qRO6}Good. Then I will tell you what I know. I heard about one other piece.[ib:normal2][if:convo_calm_friendly]", null), null, null, null, null)
				.NpcLine(new TextObject("{=4WZ9zJbF}I do not know where the other pieces are, you may need to keep searching for them.[if:convo_confused_normal]", null), null, null, null, null)
				.NpcLine(new TextObject("{=kIDbW8fP}When you have recovered all pieces, return to me and I'll help you put them to use.[if:convo_calm_friendly]", null), null, null, null, null)
				.Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += base.CompleteQuestWithSuccess;
				})
				.CloseDialog()
				.PlayerOption(new TextObject("{=EitTbGvB}I am not sure. I haven't made up my mind about this.", null), null, null, null)
				.Consequence(delegate
				{
					this._metImperialMentor = true;
					Hero.OneToOneConversationHero.SetHasMet();
				})
				.NpcLine(new TextObject("{=TH6L7OXu}Then you can come back when you have made up your mind.[ib:demure][if:convo_snide_voice]", null), null, null, null, null)
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("lord_start", 110).NpcLine(new TextObject("{=oaSTbNwz}So have you made up your mind now?", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor && this._metImperialMentor)
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=Lwdkj0hG}Yes, I intend to use the banner to help save the Empire.", null), null, null, null)
				.NpcLine(new TextObject("{=JRQ7qRO6}Good. Then I will tell you what I know. I heard about one other piece.[ib:normal2][if:convo_calm_friendly]", null), null, null, null, null)
				.NpcLine(new TextObject("{=ijyROgb4}I do not know where the other pieces are, you may need to keep searching for them.[if:convo_thinking]", null), null, null, null, null)
				.NpcLine(new TextObject("{=kIDbW8fP}When you have recovered all pieces, return to me and I'll help you put them to use.[if:convo_calm_friendly]", null), null, null, null, null)
				.Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += base.CompleteQuestWithSuccess;
				})
				.CloseDialog()
				.PlayerOption(new TextObject("{=ibm9EEPa}No, I need more time to decide.", null), null, null, null)
				.NpcLine(new TextObject("{=PknruSY5}Then you can come back when you have made up your mind.[ib:demure][if:convo_nonchalant]", null), null, null, null, null)
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog(), this);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00013004 File Offset: 0x00011204
		private void ActivateAssembleTheBannerQuest()
		{
			if (!Campaign.Current.QuestManager.Quests.Any<QuestBase>((QuestBase q) => q is AssembleTheBannerQuest))
			{
				new AssembleTheBannerQuest().StartQuest();
			}
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00013050 File Offset: 0x00011250
		protected override void OnCompleteWithSuccess()
		{
			base.OnCompleteWithSuccess();
			base.AddLog(this._endQuestLog, false);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00013066 File Offset: 0x00011266
		internal static void AutoGeneratedStaticCollectObjectsMeetWithIstianaQuest(object o, List<object> collectedObjects)
		{
			((MeetWithIstianaQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00013074 File Offset: 0x00011274
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001307D File Offset: 0x0001127D
		internal static object AutoGeneratedGetMemberValue_metImperialMentor(object o)
		{
			return ((MeetWithIstianaQuest)o)._metImperialMentor;
		}

		// Token: 0x04000118 RID: 280
		[SaveableField(1)]
		private bool _metImperialMentor;
	}
}
