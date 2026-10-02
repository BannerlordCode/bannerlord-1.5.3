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
	// Token: 0x02000036 RID: 54
	public class MeetWithArzagosQuest : StoryModeQuestBase
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0001268C File Offset: 0x0001088C
		private TextObject _startQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=5K4wvz3w}Find and meet {HERO.LINK} to learn more about Neretzes' Banner. He is currently in {SETTLEMENT}.", null);
				StringHelpers.SetCharacterProperties("HERO", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("SETTLEMENT", StoryModeHeroes.AntiImperialMentor.CurrentSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000369 RID: 873 RVA: 0x000126D8 File Offset: 0x000108D8
		private TextObject _endQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=qMUfOtyk}You talked with {HERO.LINK}.", null);
				StringHelpers.SetCharacterProperties("HERO", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0001270C File Offset: 0x0001090C
		public override TextObject Title
		{
			get
			{
				TextObject textObject = new TextObject("{=Y6SqyQwn}Meet with {HERO.NAME}", null);
				StringHelpers.SetCharacterProperties("HERO", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0001273D File Offset: 0x0001093D
		public override bool IsRemainingTimeHidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00012740 File Offset: 0x00010940
		public MeetWithArzagosQuest(Settlement settlement)
			: base("meet_with_arzagos_story_mode_quest", null, StoryModeManager.Current.MainStoryLine.FirstPhase.FirstPhaseEndTime)
		{
			this._metAntiImperialMentor = false;
			this.SetDialogs();
			HeroHelper.SpawnHeroForTheFirstTime(StoryModeHeroes.AntiImperialMentor, settlement);
			base.AddTrackedObject(settlement);
			base.AddTrackedObject(StoryModeHeroes.AntiImperialMentor);
			base.AddLog(this._startQuestLog, false);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x000127A5 File Offset: 0x000109A5
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000127AD File Offset: 0x000109AD
		protected override void HourlyTick()
		{
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000127B0 File Offset: 0x000109B0
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("lord_start", 110).NpcLine(new TextObject("{=unOLk4PY}So. Who are you, and what brings you to me?", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor && !this._metAntiImperialMentor)
				.PlayerLine(new TextObject("{=tfm5hcks}I believe I have a piece of the Dragon Banner of Calradios.", null), null, null, null)
				.NpcLine(new TextObject("{=uvbCyLiR}Is that true? Well, that is interesting.[ib:normal][if:convo_astonished]", null), null, null, null, null)
				.NpcLine(new TextObject("{=pOuGX9j0}You may have one piece of the banner, but it's of little use in itself. You'll have to find the other parts. But once you can bring together the pieces, you'll have something of tremendous value.[if:convo_pondering]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=t71lPdyb}How so?", null), null, null, null)
				.NpcLine(new TextObject("{=SmVwMrUM}The banner of Calradios is part of a legend. It was said to be carried by Calradios the Great, who first led his people to this land, to conquer and despoil. The legend says that no army led by a true son of Calradios shall be defeated in battle.[ib:confident2][if:convo_thinking]", null), null, null, null, null)
				.NpcLine(new TextObject("{=cNwejsNl}Convenient legend, eh? Of course the Calradians have been defeated many times, but I guess those were not 'true sons.' Still, you could say it represents the strength and endurance of this empire.[ib:normal][if:convo_focused_happy]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=FBp2ranI}So, can you help me find a buyer for it?", null), null, null, null)
				.NpcLine(new TextObject("{=3G64Ej64}A buyer? I can help you do far more than that.[if:convo_astonished]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=MnmblprY}So, where can I find the other pieces?", null), null, null, null)
				.NpcLine(new TextObject("{=Fgta5mF6}Before I answer, you and I need to know more about each other. I don't know what you know about me.  I was a citizen of the Empire. I was a commander in the imperial armies. But I am not imperial.[ib:confident][if:convo_thinking]", null), null, null, null, null)
				.NpcLine(new TextObject("{=R5kLv5kg}I am what they call Palaic. Palaic is a language that is no longer spoken, except by a few old people. Even the word 'Palaic' is imperial. We are a people who have forgotten who we are.[if:convo_focused_voice]", null), null, null, null, null)
				.NpcLine(new TextObject("{=cfTiiEEM}The Empire has a genius for destruction - the destruction of languages, traditions, gods. It takes our fortresses, slaughters our men, and turns our children into its own children.[if:convo_focused_voice]", null), null, null, null, null)
				.NpcLine(new TextObject("{=qoA4UPly}Nothing can bring the Palaic people back. They are now imperial. But it is an insult to our name, to our gods, to our memory, that the state which destroyed our shrines and fortresses should last and thrive.[if:convo_empathic_voice]", null), null, null, null, null)
				.NpcLine(new TextObject("{=rMem50oz}I have vowed that this Empire shall not survive this civil war, if I can do anything to stop it. And believe me, if I had that banner, there is very much something I could do.[if:convo_angry_voice]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.ActivateAssembleTheBannerQuest))
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=tkXKef0Z}I too would see the empire destroyed.", null), null, null, null)
				.NpcLine(new TextObject("{=4RaspRbe}Good. Then I will tell you what I know. I heard about one other piece.[if:convo_calm_friendly]", null), null, null, null, null)
				.NpcLine(new TextObject("{=4WZ9zJbF}I do not know where the other pieces are, you may need to keep searching for them.[if:convo_confused_normal]", null), null, null, null, null)
				.NpcLine(new TextObject("{=kIDbW8fP}When you have recovered all pieces, return to me and I'll help you put them to use.[if:convo_calm_friendly]", null), null, null, null, null)
				.Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += base.CompleteQuestWithSuccess;
				})
				.CloseDialog()
				.PlayerOption(new TextObject("{=gdgbaMOP}I am not sure I share your views.", null), null, null, null)
				.Consequence(delegate
				{
					this._metAntiImperialMentor = true;
					Hero.OneToOneConversationHero.SetHasMet();
				})
				.NpcLine(new TextObject("{=7ULaG8aT}Then you can come back when you made your mind up.[ib:closed][if:convo_insulted]", null), null, null, null, null)
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("lord_start", 110).NpcLine(new TextObject("{=bHveKDUI}So have you made up your mind now?[ib:closed][if:convo_nonchalant]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == StoryModeHeroes.AntiImperialMentor && this._metAntiImperialMentor)
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=upyNhwZ9}Yes, I intend to use the banner to help destroy the empire.", null), null, null, null)
				.NpcLine(new TextObject("{=TEgoba7R}Good. Then I will tell you what I know. I heard about one other piece.[ib:confident2][if:convo_calm_friendly]", null), null, null, null, null)
				.NpcLine(new TextObject("{=ijyROgb4}I do not know where the other pieces are, you may need to keep searching for them.[if:convo_thinking]", null), null, null, null, null)
				.NpcLine(new TextObject("{=kIDbW8fP}When you have recovered all pieces, return to me and I'll help you put them to use.[if:convo_calm_friendly]", null), null, null, null, null)
				.Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += base.CompleteQuestWithSuccess;
				})
				.CloseDialog()
				.PlayerOption(new TextObject("{=ibm9EEPa}No, I need more time to decide.", null), null, null, null)
				.NpcLine(new TextObject("{=7ULaG8aT}Then you can come back when you made your mind up.[ib:closed][if:convo_insulted]", null), null, null, null, null)
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog(), this);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00012ABC File Offset: 0x00010CBC
		private void ActivateAssembleTheBannerQuest()
		{
			if (!Campaign.Current.QuestManager.Quests.Any<QuestBase>((QuestBase q) => q is AssembleTheBannerQuest))
			{
				new AssembleTheBannerQuest().StartQuest();
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00012B08 File Offset: 0x00010D08
		protected override void OnCompleteWithSuccess()
		{
			base.OnCompleteWithSuccess();
			base.AddLog(this._endQuestLog, false);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00012B1E File Offset: 0x00010D1E
		internal static void AutoGeneratedStaticCollectObjectsMeetWithArzagosQuest(object o, List<object> collectedObjects)
		{
			((MeetWithArzagosQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00012B2C File Offset: 0x00010D2C
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00012B35 File Offset: 0x00010D35
		internal static object AutoGeneratedGetMemberValue_metAntiImperialMentor(object o)
		{
			return ((MeetWithArzagosQuest)o)._metAntiImperialMentor;
		}

		// Token: 0x04000117 RID: 279
		[SaveableField(1)]
		private bool _metAntiImperialMentor;
	}
}
