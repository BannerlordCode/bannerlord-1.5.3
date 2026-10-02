using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.SecondPhase
{
	// Token: 0x02000027 RID: 39
	public class ConspiracyProgressQuest : StoryModeQuestBase
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000B6B3 File Offset: 0x000098B3
		private bool _isImperialSide
		{
			get
			{
				return StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000B6C4 File Offset: 0x000098C4
		private TextObject _startQuestLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=oX2aoilb}{MENTOR.NAME} knows of the rise of your {KINGDOM_NAME}. Rumors say {MENTOR.NAME} is planning to undo your progress. Be ready!", null);
				StringHelpers.SetCharacterProperties("MENTOR", this._isImperialSide ? StoryModeHeroes.AntiImperialMentor.CharacterObject : StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("KINGDOM_NAME", (Clan.PlayerClan.Kingdom != null) ? Clan.PlayerClan.Kingdom.Name : Clan.PlayerClan.Name);
				return textObject;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000B73C File Offset: 0x0000993C
		private TextObject _questCanceledLogText
		{
			get
			{
				return new TextObject("{=tVlZTOst}You have chosen a different path.", null);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000B74C File Offset: 0x0000994C
		public override TextObject Title
		{
			get
			{
				TextObject textObject;
				if (this._isImperialSide)
				{
					textObject = new TextObject("{=PJ5C3Dim}{ANTIIMPERIAL_MENTOR.NAME}'s Conspiracy", null);
					StringHelpers.SetCharacterProperties("ANTIIMPERIAL_MENTOR", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				}
				else
				{
					textObject = new TextObject("{=i3SSc0I4}{IMPERIAL_MENTOR.NAME}'s Plan", null);
					StringHelpers.SetCharacterProperties("IMPERIAL_MENTOR", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				}
				return textObject;
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000B7AA File Offset: 0x000099AA
		public ConspiracyProgressQuest()
			: base("conspiracy_quest_campaign_behavior", null, CampaignTime.Never)
		{
			SecondPhase.Instance.TriggerConspiracy();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000B7C7 File Offset: 0x000099C7
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000B7CF File Offset: 0x000099CF
		protected override void HourlyTick()
		{
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000B7D4 File Offset: 0x000099D4
		protected override void RegisterEvents()
		{
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			StoryModeEvents.OnConspiracyActivatedEvent.AddNonSerializedListener(this, new Action(this.OnConspiracyActivated));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000B826 File Offset: 0x00009A26
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan && oldKingdom == StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom)
			{
				base.CompleteQuestWithCancel(this._questCanceledLogText);
				StoryModeManager.Current.MainStoryLine.CancelSecondAndThirdPhase();
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000B85D File Offset: 0x00009A5D
		protected override void OnStartQuest()
		{
			this._startQuestLog = base.AddDiscreteLog(this._startQuestLogText, new TextObject("{=1LrHV647}Conspiracy Strength", null), (int)SecondPhase.Instance.ConspiracyStrength, 2000, null, false);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000B88E File Offset: 0x00009A8E
		protected override void SetDialogs()
		{
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000B890 File Offset: 0x00009A90
		protected override void OnFinalize()
		{
			base.OnFinalize();
			foreach (QuestBase questBase in Campaign.Current.QuestManager.Quests.ToList<QuestBase>())
			{
				if (typeof(ConspiracyQuestBase) == questBase.GetType().BaseType && questBase.IsOngoing)
				{
					questBase.CompleteQuestWithCancel(new TextObject("{=YJxCbbpd}Conspiracy is activated!", null));
				}
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000B928 File Offset: 0x00009B28
		protected override void DailyTick()
		{
			StoryModeManager.Current.MainStoryLine.SecondPhase.IncreaseConspiracyStrength();
			this._startQuestLog.UpdateCurrentProgress((int)StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyStrength);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000B960 File Offset: 0x00009B60
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (detail == QuestBase.QuestCompleteDetails.Success && typeof(ConspiracyQuestBase) == quest.GetType().BaseType)
			{
				this._startQuestLog.UpdateCurrentProgress((int)StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyStrength);
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000B9AD File Offset: 0x00009BAD
		private void OnConspiracyActivated()
		{
			base.CompleteQuestWithTimeOut(null);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000B9B6 File Offset: 0x00009BB6
		internal static void AutoGeneratedStaticCollectObjectsConspiracyProgressQuest(object o, List<object> collectedObjects)
		{
			((ConspiracyProgressQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000B9C4 File Offset: 0x00009BC4
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._startQuestLog);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000B9D9 File Offset: 0x00009BD9
		internal static object AutoGeneratedGetMemberValue_startQuestLog(object o)
		{
			return ((ConspiracyProgressQuest)o)._startQuestLog;
		}

		// Token: 0x040000B1 RID: 177
		[SaveableField(2)]
		private JournalLog _startQuestLog;
	}
}
