using System;
using System.Collections.Generic;
using Helpers;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.TutorialPhase
{
	// Token: 0x02000022 RID: 34
	public class TalkToTheHeadmanTutorialQuest : StoryModeQuestBase
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00008D73 File Offset: 0x00006F73
		private TextObject _startQuestLog
		{
			get
			{
				return new TextObject("{=rinefpgo}You have arrived at the village. You can buy some food and hire some men to help hunt for the raiders. First go into the village and talk to the headman.", null);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00008D80 File Offset: 0x00006F80
		private TextObject _readyToGoLog
		{
			get
			{
				return new TextObject("{=KhL2ctsi}You're ready to leave now. Talk to the headman again. He had said he has a task for you.", null);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00008D8D File Offset: 0x00006F8D
		private TextObject _goBackToVillageMenuLog
		{
			get
			{
				return new TextObject("{=awgBkdXx}You should go back to the village menu and make your preparations to go after the raiders, then find out about the task that the headman has for you.", null);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00008D9C File Offset: 0x00006F9C
		public override TextObject Title
		{
			get
			{
				TextObject textObject = new TextObject("{=HqlXdzcv}Talk with Headman {HEADMAN.FIRSTNAME}", null);
				StringHelpers.SetCharacterProperties("HEADMAN", this._headman.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008DD0 File Offset: 0x00006FD0
		public TalkToTheHeadmanTutorialQuest(Hero headman)
			: base("talk_to_the_headman_tutorial_quest", null, CampaignTime.Never)
		{
			this._headman = headman;
			base.AddTrackedObject(this._headman);
			this.SetDialogs();
			base.InitializeQuestOnCreation();
			base.AddLog(this._startQuestLog, false);
			TutorialPhase.Instance.SetTutorialFocusSettlement(Settlement.CurrentSettlement);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008E2A File Offset: 0x0000702A
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008E32 File Offset: 0x00007032
		protected override void HourlyTick()
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008E34 File Offset: 0x00007034
		protected override void RegisterEvents()
		{
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			CampaignEvents.BeforeMissionOpenedEvent.AddNonSerializedListener(this, new Action(this.OnBeforeMissionOpened));
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00008E64 File Offset: 0x00007064
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=YEeb0B1V}I am {HEADMAN.FIRSTNAME}, headman of this village. What brings you here?[ib:normal][if:convo_shocked]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.headman_quest_conversation_start_on_condition))
				.PlayerLine(new TextObject("{=StLYbEQZ}We need help. Some raiders have taken our younger brother and sister captive. We think they may have passed this way.", null), null, null, null)
				.NpcLine(new TextObject("{=uNgu02FH}They got your people too? Sorry to hear that. Those bastards have done a bit of killing and looting in these parts as well.[ib:normal2][if:convo_dismayed]", null), null, null, null, null)
				.NpcLine(new TextObject("{=bNcGO33Q}We think they've gone north. I reckon there are a few folk around here who'll join you in going after them if you'll pay for their gear.[if:convo_thinking]", null), null, null, null, null)
				.NpcLine(new TextObject("{=5Mw4trfs}Once you've made your preparations, come and talk to me again. I may have a task for you if you are going after the raiders.[if:convo_pondering]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.headman_quest_conversation_end_on_consequence))
				.CloseDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000009).NpcLine(new TextObject("{=uhYXopnJ}Have you finished your preparations?[if:convo_undecided_open]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == this._headman && (!this._recruitTroopsQuest.IsFinalized || !this._purchaseGrainQuest.IsFinalized))
				.PlayerLine(new TextObject("{=elJCacQO}I am working on it.", null), null, null, null)
				.CloseDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=TIaVyqMx}Glad to see you found what you needed. Now, about that matter I mentioned earlier...[if:convo_grave]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.headman_quest_end_conversation_start_on_condition))
				.NpcLine(new TextObject("{=lnAhXvbo}There's this wandering doctor who comes through here from time to time. Name of Tacteos. Treats people for free... We're fond of him.[if:convo_thinking]", null), null, null, null, null)
				.NpcLine(new TextObject("{=xGdoz9Pn}Well, we last saw him a few days ago. He was carrying some sort of chest, which he was very mysterious about. He was on some sort of 'quest', he said, though wouldn't tell us more.[if:convo_pondering]", null), null, null, null, null)
				.NpcLine(new TextObject("{=WDylM3dx}He set off on the road just a few hours before the raiders came through here. Well, he's not really a worldly type, just the kind of fellow who'd stumble into a trap and let himself be captured. We're worried about him.[if:convo_dismayed]", null), null, null, null, null)
				.NpcLine(new TextObject("{=MREvo37b}If you can keep an eye out for him, this Tacteos, we'd be very grateful. Maybe, if he's alive and well, he'll tell you a little more about his 'quest.'[if:convo_normal]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.headman_quest_end_conversation_start_on_consequence))
				.CloseDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000009).NpcLine(new TextObject("{=gX0RzZoT}Let's just go speak to the headman.", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.headman_quest_conversation_talk_with_brother_on_condition))
				.CloseDialog(), this);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00009069 File Offset: 0x00007269
		private bool headman_quest_conversation_start_on_condition()
		{
			StringHelpers.SetCharacterProperties("HEADMAN", this._headman.CharacterObject, null, false);
			return Hero.OneToOneConversationHero == this._headman && !this._headman.HasMet;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000090A0 File Offset: 0x000072A0
		private bool headman_quest_conversation_talk_with_brother_on_condition()
		{
			StringHelpers.SetCharacterProperties("BROTHER", StoryModeHeroes.ElderBrother.CharacterObject, null, false);
			return Hero.OneToOneConversationHero == StoryModeHeroes.ElderBrother && !this._headman.HasMet;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000090D8 File Offset: 0x000072D8
		private void headman_quest_conversation_end_on_consequence()
		{
			this._headman.SetHasMet();
			this._headman.SetPersonalRelation(Hero.MainHero, 100);
			this._recruitTroopsQuest = new RecruitTroopsTutorialQuest(this._headman);
			this._recruitTroopsQuest.StartQuest();
			this._purchaseGrainQuest = new PurchaseGrainTutorialQuest(this._headman);
			this._purchaseGrainQuest.StartQuest();
			TutorialPhase.Instance.SetTutorialQuestPhase(TutorialQuestPhase.RecruitAndPurchaseStarted);
			base.AddLog(this._goBackToVillageMenuLog, false);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00009153 File Offset: 0x00007353
		private bool headman_quest_end_conversation_start_on_condition()
		{
			return Hero.OneToOneConversationHero == this._headman && this._recruitTroopsQuest.IsFinalized && this._purchaseGrainQuest.IsFinalized;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000917C File Offset: 0x0000737C
		private void headman_quest_end_conversation_start_on_consequence()
		{
			TutorialPhase.Instance.SetLockTutorialVillageEnter(false);
			base.CompleteQuestWithSuccess();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000918F File Offset: 0x0000738F
		protected override void OnCompleteWithSuccess()
		{
			TutorialPhase.Instance.RemoveTutorialFocusSettlement();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000919C File Offset: 0x0000739C
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (this._recruitTroopsQuest.IsFinalized && this._purchaseGrainQuest.IsFinalized)
			{
				TutorialPhase.Instance.SetLockTutorialVillageEnter(true);
				TextObject textObject = new TextObject("{=3YHL3wpM}{BROTHER.NAME}:", null);
				textObject.SetCharacterProperties("BROTHER", StoryModeHeroes.ElderBrother.CharacterObject, false);
				InformationManager.ShowInquiry(new InquiryData(textObject.ToString(), new TextObject("{=1xqmoDvS}We have finished our preparations. Let's talk to the headman again. He had said he may have a task for us. We could use his friendship.", null).ToString(), true, false, new TextObject("{=lmG7uRK2}Okay", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
				base.AddLog(this._readyToGoLog, false);
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00009244 File Offset: 0x00007444
		private void OnBeforeMissionOpened()
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.StringId == "village_ES3_2")
			{
				int hitPoints = StoryModeHeroes.ElderBrother.HitPoints;
				int num = 50;
				if (hitPoints < num)
				{
					int num2 = num - hitPoints;
					StoryModeHeroes.ElderBrother.Heal(num2, false);
				}
				LocationCharacter locationCharacterOfHero = LocationComplex.Current.GetLocationCharacterOfHero(StoryModeHeroes.ElderBrother);
				locationCharacterOfHero.CharacterRelation = LocationCharacter.CharacterRelations.Neutral;
				PlayerEncounter.LocationEncounter.AddAccompanyingCharacter(locationCharacterOfHero, true);
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000092B3 File Offset: 0x000074B3
		internal static void AutoGeneratedStaticCollectObjectsTalkToTheHeadmanTutorialQuest(object o, List<object> collectedObjects)
		{
			((TalkToTheHeadmanTutorialQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000092C1 File Offset: 0x000074C1
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._headman);
			collectedObjects.Add(this._recruitTroopsQuest);
			collectedObjects.Add(this._purchaseGrainQuest);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000092EE File Offset: 0x000074EE
		internal static object AutoGeneratedGetMemberValue_headman(object o)
		{
			return ((TalkToTheHeadmanTutorialQuest)o)._headman;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000092FB File Offset: 0x000074FB
		internal static object AutoGeneratedGetMemberValue_recruitTroopsQuest(object o)
		{
			return ((TalkToTheHeadmanTutorialQuest)o)._recruitTroopsQuest;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00009308 File Offset: 0x00007508
		internal static object AutoGeneratedGetMemberValue_purchaseGrainQuest(object o)
		{
			return ((TalkToTheHeadmanTutorialQuest)o)._purchaseGrainQuest;
		}

		// Token: 0x0400009D RID: 157
		[SaveableField(1)]
		private readonly Hero _headman;

		// Token: 0x0400009E RID: 158
		[SaveableField(2)]
		private RecruitTroopsTutorialQuest _recruitTroopsQuest;

		// Token: 0x0400009F RID: 159
		[SaveableField(3)]
		private PurchaseGrainTutorialQuest _purchaseGrainQuest;
	}
}
