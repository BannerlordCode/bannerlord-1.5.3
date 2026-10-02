using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.FirstPhase
{
	// Token: 0x02000032 RID: 50
	public class AssembleTheBannerQuest : StoryModeQuestBase
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000F892 File Offset: 0x0000DA92
		private TextObject _startQuestLog
		{
			get
			{
				return new TextObject("{=OS8YjyE5}You should collect all of the pieces of the Dragon Banner before deciding your path.", null);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000F8A0 File Offset: 0x0000DAA0
		private TextObject _allPiecesCollectedQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=eV8R0SKp}Now you can decide what to do with the {DRAGON_BANNER}.", null);
				textObject.SetTextVariable("DRAGON_BANNER", StoryModeManager.Current.MainStoryLine.DragonBanner.Name);
				StringHelpers.SetCharacterProperties("IMPERIAL_MENTOR", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				StringHelpers.SetCharacterProperties("ANTI_IMPERIAL_MENTOR", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000F908 File Offset: 0x0000DB08
		private TextObject _talkedWithImperialMentorButNotWithAntiImperialMentorQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=yNcBDr9j}You talked with {IMPERIAL_MENTOR.LINK}. Now, you may want to talk with {ANTI_IMPERIAL_MENTOR.LINK} and take {?ANTI_IMPERIAL_MENTOR.GENDER}her{?}his{\\?} opinions too. {?ANTI_IMPERIAL_MENTOR.GENDER}She{?}He{\\?} is currently in {SETTLEMENT_LINK}.", null);
				StringHelpers.SetCharacterProperties("IMPERIAL_MENTOR", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				StringHelpers.SetCharacterProperties("ANTI_IMPERIAL_MENTOR", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("SETTLEMENT_LINK", StoryModeHeroes.AntiImperialMentor.CurrentSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000F96C File Offset: 0x0000DB6C
		private TextObject _talkedWithImperialMentorQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=RwlDeE9t}You talked with {IMPERIAL_MENTOR.LINK} too. Now you should make a decision.", null);
				StringHelpers.SetCharacterProperties("IMPERIAL_MENTOR", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000F9A0 File Offset: 0x0000DBA0
		private TextObject _talkedWithAntiImperialMentorButNotWithImperialMentorQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=yub8ZSFP}You talked with {ANTI_IMPERIAL_MENTOR.LINK}. Now, you may want to talk with {IMPERIAL_MENTOR.LINK} and take {?IMPERIAL_MENTOR.GENDER}her{?}his{\\?} opinions too. {?IMPERIAL_MENTOR.GENDER}She{?}He{\\?} is currently in {SETTLEMENT_LINK}.", null);
				StringHelpers.SetCharacterProperties("ANTI_IMPERIAL_MENTOR", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				StringHelpers.SetCharacterProperties("IMPERIAL_MENTOR", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("SETTLEMENT_LINK", StoryModeHeroes.ImperialMentor.CurrentSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000FA04 File Offset: 0x0000DC04
		private TextObject _talkedWithAntiImperialMentorQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=rfkKxdxp}You talked with {ANTI_IMPERIAL_MENTOR.LINK} too. Now you should make a decision.", null);
				StringHelpers.SetCharacterProperties("ANTI_IMPERIAL_MENTOR", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000FA35 File Offset: 0x0000DC35
		private TextObject _endQuestLog
		{
			get
			{
				return new TextObject("{=eNJBjYG8}You successfully assembled the Dragon Banner of Calradios.", null);
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000FA42 File Offset: 0x0000DC42
		public override TextObject Title
		{
			get
			{
				return new TextObject("{=y84UnOQX}Assemble the Dragon Banner", null);
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000FA4F File Offset: 0x0000DC4F
		public override bool IsRemainingTimeHidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000FA52 File Offset: 0x0000DC52
		public AssembleTheBannerQuest()
			: base("assemble_the_banner_story_mode_quest", null, StoryModeManager.Current.MainStoryLine.FirstPhase.FirstPhaseEndTime)
		{
			this._talkedWithImperialMentor = false;
			this._talkedWithAntiImperialMentor = false;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000FA82 File Offset: 0x0000DC82
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000FA8A File Offset: 0x0000DC8A
		protected override void HourlyTick()
		{
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000FA8C File Offset: 0x0000DC8C
		protected override void RegisterEvents()
		{
			StoryModeEvents.OnBannerPieceCollectedEvent.AddNonSerializedListener(this, new Action(this.OnBannerPieceCollected));
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000FABC File Offset: 0x0000DCBC
		protected override void OnStartQuest()
		{
			this.SetDialogs();
			this._startLog = base.AddDiscreteLog(this._startQuestLog, new TextObject("{=xL3WGYsw}Collected Pieces", null), FirstPhase.Instance.CollectedBannerPieceCount, 3, null, false);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000FAEE File Offset: 0x0000DCEE
		protected override void OnCompleteWithSuccess()
		{
			base.AddLog(this._endQuestLog, false);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000FB00 File Offset: 0x0000DD00
		private void OnBannerPieceCollected()
		{
			this._startLog.UpdateCurrentProgress(FirstPhase.Instance.CollectedBannerPieceCount);
			if (FirstPhase.Instance.AllPiecesCollected)
			{
				base.AddLog(this._allPiecesCollectedQuestLog, false);
				base.AddTrackedObject(StoryModeHeroes.ImperialMentor.CurrentSettlement);
				base.AddTrackedObject(StoryModeHeroes.AntiImperialMentor.CurrentSettlement);
				base.AddTrackedObject(StoryModeHeroes.ImperialMentor);
				base.AddTrackedObject(StoryModeHeroes.AntiImperialMentor);
				FirstPhase firstPhase = StoryModeManager.Current.MainStoryLine.FirstPhase;
				if (firstPhase == null)
				{
					return;
				}
				firstPhase.MergeDragonBanner();
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000FB8C File Offset: 0x0000DD8C
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (quest is CreateKingdomQuest || quest is SupportKingdomQuest)
			{
				if (base.IsTracked(StoryModeHeroes.AntiImperialMentor.CurrentSettlement))
				{
					base.RemoveTrackedObject(StoryModeHeroes.AntiImperialMentor.CurrentSettlement);
				}
				if (base.IsTracked(StoryModeHeroes.ImperialMentor.CurrentSettlement))
				{
					base.RemoveTrackedObject(StoryModeHeroes.ImperialMentor.CurrentSettlement);
				}
				if (base.IsTracked(StoryModeHeroes.AntiImperialMentor))
				{
					base.RemoveTrackedObject(StoryModeHeroes.AntiImperialMentor);
				}
				if (base.IsTracked(StoryModeHeroes.ImperialMentor))
				{
					base.RemoveTrackedObject(StoryModeHeroes.ImperialMentor);
				}
				base.CompleteQuestWithSuccess();
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000FC23 File Offset: 0x0000DE23
		public override void OnFailed()
		{
			base.OnFailed();
			this.RemoveRemainingBannerPieces();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000FC31 File Offset: 0x0000DE31
		public override void OnCanceled()
		{
			base.OnCanceled();
			this.RemoveRemainingBannerPieces();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000FC3F File Offset: 0x0000DE3F
		protected override void OnTimedOut()
		{
			base.OnTimedOut();
			this.RemoveRemainingBannerPieces();
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000FC50 File Offset: 0x0000DE50
		private void RemoveRemainingBannerPieces()
		{
			ItemObject @object = Campaign.Current.ObjectManager.GetObject<ItemObject>("dragon_banner_center");
			ItemObject object2 = Campaign.Current.ObjectManager.GetObject<ItemObject>("dragon_banner_dragonhead");
			ItemObject object3 = Campaign.Current.ObjectManager.GetObject<ItemObject>("dragon_banner_handle");
			foreach (ItemRosterElement itemRosterElement in MobileParty.MainParty.ItemRoster)
			{
				if (itemRosterElement.EquipmentElement.Item == @object || itemRosterElement.EquipmentElement.Item == object2 || itemRosterElement.EquipmentElement.Item == object3)
				{
					MobileParty.MainParty.ItemRoster.Remove(itemRosterElement);
				}
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000FD24 File Offset: 0x0000DF24
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(this.GetImperialMentorEndQuestDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(this.GetAntiImperialMentorEndQuestDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("lord_start", 150).NpcLine(new TextObject("{=AHDQffXv}Have you assembled the banner?", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.AssembleBannerConditionDialogCondition))
				.PlayerLine(new TextObject("{=2h7IlBmv}Not yet, I'm working on it...", null), null, null, null)
				.Consequence(delegate
				{
					if (PlayerEncounter.Current != null)
					{
						PlayerEncounter.LeaveEncounter = true;
					}
				})
				.CloseDialog(), this);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000FDE0 File Offset: 0x0000DFE0
		private bool AssembleBannerConditionDialogCondition()
		{
			if ((Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor || Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor) && !FirstPhase.Instance.AllPiecesCollected)
			{
				if (Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor)
				{
					if (Campaign.Current.QuestManager.Quests.Any<QuestBase>((QuestBase q) => !q.IsFinalized && q is MeetWithIstianaQuest))
					{
						return false;
					}
				}
				if (Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor)
				{
					if (Campaign.Current.QuestManager.Quests.Any<QuestBase>((QuestBase q) => !q.IsFinalized && q is MeetWithArzagosQuest))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000FEA0 File Offset: 0x0000E0A0
		private DialogFlow GetAntiImperialMentorEndQuestDialog()
		{
			string text;
			return DialogFlow.CreateDialogFlow("hero_main_options", 150).BeginPlayerOptions(null, false).PlayerSpecialOption(new TextObject("{=r8ZLabb0}I have gathered all pieces of the Dragon Banner. What now?", null), null, null, null)
				.Condition(() => Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor && FirstPhase.Instance.AllPiecesCollected && !this._talkedWithAntiImperialMentor)
				.NpcLine(new TextObject("{=5j6qvGAF}Excellent work! When you unfurl this banner, and men see what they thought was lost, it will make a powerful impression.[ib:normal2][if:convo_astonished]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.GetAntiImperialQuests))
				.NpcLine(new TextObject("{=MOVWOyeh}Clearly you have been chosen by Heaven for a great purpose. I see the makings of a new legend here... Allow me to call you 'Bannerlord.'[ib:normal][if:convo_relaxed_happy]", null), null, null, null, null)
				.NpcLine(new TextObject("{=o791xRtb}Right then, to the business of bringing down this cursed Empire. As I see it, you have two options...[ib:confident2][if:convo_pondering]", null), null, null, null, null)
				.GetOutputToken(out text)
				.NpcLine(new TextObject("{=c6pDNXbb}You can create your own kingdom or support an existing one...[if:convo_normal]", null), null, null, null, null)
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=0pilmavQ}How can I create my own kingdom?", null), null, null, null)
				.NpcLine(new TextObject("{=frk7T3ue}It will not be easy, but I can explain in detail...", null), null, null, null, null)
				.NpcLine(new TextObject("{=yCzcfKNM}Firstly, your clan must be independent. You cannot be pledged to an existing realm.", null), null, null, null, null)
				.NpcLine(new TextObject("{=tJQ5oajd}Next, your clan must have won for itself considerable renown, or no one will follow you.", null), null, null, null, null)
				.NpcLine(new TextObject("{=MJd5agS2}I would recommend that you gather a fairly large army, as you may soon be at war with more powerful and established realms.", null), null, null, null, null)
				.NpcLine(new TextObject("{=6YhGGJ7a}Finally, you need a capital for your realm. It can be any settlement you own, so long as they do not speak the imperial tongue. I will not help you create another Empire.", null), null, null, null, null)
				.NpcLine(new TextObject("{=fprOWs1E}Now, when you are ready to declare your new kingdom, instruct the governor of your capital to have a proclamation read out throughout your lands.", null), null, null, null, null)
				.NpcLine(new TextObject("{=Q2obAF4E}So! You have much to do. I will await news of your success. Return to me when you wish to declare your ownership of the banner to the world.", null), null, null, null, null)
				.GotoDialogState(text)
				.PlayerOption(new TextObject("{=mtiaY2Pa}How can I support an existing kingdom?", null), null, null, null)
				.NpcLine(new TextObject("{=oKknZdXn}You should join the kingdom that you wish to support by talking to the leader. None will bring back the Palaic people, but the final victory of any one of those would be suitable vengeance.", null), null, null, null, null)
				.NpcLine(new TextObject("{=dPb2Vph3}My informants will tell me once you pledged your support...[ib:normal2][if:convo_nonchalant]", null), null, null, null, null)
				.GotoDialogState(text)
				.PlayerOption(new TextObject("{=6LQUuQhV}Thank you for your precious help.", null), null, null, null)
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00010060 File Offset: 0x0000E260
		private void GetAntiImperialQuests()
		{
			this._talkedWithAntiImperialMentor = true;
			if (!this._talkedWithImperialMentor)
			{
				base.AddLog(this._talkedWithAntiImperialMentorButNotWithImperialMentorQuestLog, false);
			}
			else
			{
				base.AddLog(this._talkedWithAntiImperialMentorQuestLog, false);
			}
			if (base.IsTracked(StoryModeHeroes.AntiImperialMentor.CurrentSettlement))
			{
				base.RemoveTrackedObject(StoryModeHeroes.AntiImperialMentor.CurrentSettlement);
			}
			new CreateKingdomQuest(StoryModeHeroes.AntiImperialMentor).StartQuest();
			new SupportKingdomQuest(StoryModeHeroes.AntiImperialMentor).StartQuest();
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000100DC File Offset: 0x0000E2DC
		private DialogFlow GetImperialMentorEndQuestDialog()
		{
			string text;
			return DialogFlow.CreateDialogFlow("hero_main_options", 150).BeginPlayerOptions(null, false).PlayerSpecialOption(new TextObject("{=r8ZLabb0}I have gathered all pieces of the Dragon Banner. What now?", null), null, null, null)
				.Condition(() => Hero.OneToOneConversationHero == StoryModeHeroes.ImperialMentor && FirstPhase.Instance.AllPiecesCollected && !this._talkedWithImperialMentor)
				.NpcLine(new TextObject("{=UjyZ7GFk}Impressive, most impressive. Well, things will get interesting now.[ib:normal2][if:convo_astonished]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.GetImperialQuests))
				.NpcLine(new TextObject("{=9E6faNBg}I will need to embroider a proper legend about you. Divine omens at your birth, that kind of thing. For now, we can call you 'Bannerlord,' who brings down the wrath of Heaven on the impudent barbarians.[ib:confident2][if:convo_relaxed_happy]", null), null, null, null, null)
				.NpcLine(new TextObject("{=CnXA7oyE}Now, there are two paths that lie ahead of you, my child!", null), null, null, null, null)
				.GetOutputToken(out text)
				.NpcLine(new TextObject("{=1GgTNRNl}You can make your own claim to the rulership of the Empire and try to win the civil war, or support an existing claimant...[if:convo_normal]", null), null, null, null, null)
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=Dgdopl1b}How can I create my own imperial kingdom?", null), null, null, null)
				.NpcLine(new TextObject("{=NdkqUnXb}To have a chance as an imperial contender, you must fullfil some conditions.[if:convo_empathic_voice]", null), null, null, null, null)
				.NpcLine(new TextObject("{=yCzcfKNM}Firstly, your clan must be independent. You cannot be pledged to an existing realm.", null), null, null, null, null)
				.NpcLine(new TextObject("{=LLJ0oB8i}Next, your clan's renown must have spread far and wide, or no one will take you seriously.", null), null, null, null, null)
				.NpcLine(new TextObject("{=3XbTo6O7}Also, of course, I recommend that you have as large an army as you can gather.", null), null, null, null, null)
				.NpcLine(new TextObject("{=Cl4xi6Be}Finally, you need a capital. Any settlement will do, so long as the inhabitants speak the imperial language.[if:convo_focused_voice]", null), null, null, null, null)
				.NpcLine(new TextObject("{=fprOWs1E}Now, when you are ready to declare your new kingdom, instruct the governor of your capital to have a proclamation read out throughout your lands.", null), null, null, null, null)
				.NpcLine(new TextObject("{=tkJD40hE}Well, that should keep you busy for a while. Come back when you are ready.", null), null, null, null, null)
				.GotoDialogState(text)
				.PlayerOption(new TextObject("{=tRzjuX0E}How can I support an existing imperial claimant?", null), null, null, null)
				.NpcLine(new TextObject("{=oL9BdThD}Choose one and pledge allegiance. When this civil war began, I was a bit torn... Rhagaea was the cleverest ruler, Garios probably the best fighter, and Lucon seemed to have the best grasp of our laws and traditions. But you can make up your own mind.", null), null, null, null, null)
				.NpcLine(new TextObject("{=eaxOH9mb}My little birds will tell me once you pledge your support...[if:convo_nonchalant]", null), null, null, null, null)
				.GotoDialogState(text)
				.PlayerOption(new TextObject("{=6LQUuQhV}Thank you for your precious help.", null), null, null, null)
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0001029C File Offset: 0x0000E49C
		private void GetImperialQuests()
		{
			this._talkedWithImperialMentor = true;
			if (!this._talkedWithAntiImperialMentor)
			{
				base.AddLog(this._talkedWithImperialMentorButNotWithAntiImperialMentorQuestLog, false);
			}
			else
			{
				base.AddLog(this._talkedWithImperialMentorQuestLog, false);
			}
			if (base.IsTracked(StoryModeHeroes.ImperialMentor.CurrentSettlement))
			{
				base.RemoveTrackedObject(StoryModeHeroes.ImperialMentor.CurrentSettlement);
			}
			new CreateKingdomQuest(StoryModeHeroes.ImperialMentor).StartQuest();
			new SupportKingdomQuest(StoryModeHeroes.ImperialMentor).StartQuest();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00010316 File Offset: 0x0000E516
		internal static void AutoGeneratedStaticCollectObjectsAssembleTheBannerQuest(object o, List<object> collectedObjects)
		{
			((AssembleTheBannerQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00010324 File Offset: 0x0000E524
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._startLog);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00010339 File Offset: 0x0000E539
		internal static object AutoGeneratedGetMemberValue_startLog(object o)
		{
			return ((AssembleTheBannerQuest)o)._startLog;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00010346 File Offset: 0x0000E546
		internal static object AutoGeneratedGetMemberValue_talkedWithImperialMentor(object o)
		{
			return ((AssembleTheBannerQuest)o)._talkedWithImperialMentor;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00010358 File Offset: 0x0000E558
		internal static object AutoGeneratedGetMemberValue_talkedWithAntiImperialMentor(object o)
		{
			return ((AssembleTheBannerQuest)o)._talkedWithAntiImperialMentor;
		}

		// Token: 0x040000EB RID: 235
		[SaveableField(1)]
		private JournalLog _startLog;

		// Token: 0x040000EC RID: 236
		[SaveableField(2)]
		private bool _talkedWithImperialMentor;

		// Token: 0x040000ED RID: 237
		[SaveableField(3)]
		private bool _talkedWithAntiImperialMentor;
	}
}
