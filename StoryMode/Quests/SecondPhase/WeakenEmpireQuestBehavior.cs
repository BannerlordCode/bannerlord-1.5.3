using System;
using System.Collections.Generic;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.SecondPhase
{
	// Token: 0x02000029 RID: 41
	public class WeakenEmpireQuestBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000213 RID: 531 RVA: 0x0000BD5D File Offset: 0x00009F5D
		public override void RegisterEvents()
		{
			StoryModeEvents.OnMainStoryLineSideChosenEvent.AddNonSerializedListener(this, new Action<MainStoryLineSide>(this.OnMainStoryLineSideChosen));
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000BD76 File Offset: 0x00009F76
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000BD78 File Offset: 0x00009F78
		private void OnMainStoryLineSideChosen(MainStoryLineSide side)
		{
			if (side == MainStoryLineSide.CreateAntiImperialKingdom || side == MainStoryLineSide.SupportAntiImperialKingdom)
			{
				new WeakenEmpireQuestBehavior.WeakenEmpireQuest(StoryModeHeroes.AntiImperialMentor).StartQuest();
			}
		}

		// Token: 0x0200006E RID: 110
		public class WeakenEmpireQuestBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060005FF RID: 1535 RVA: 0x00021507 File Offset: 0x0001F707
			public WeakenEmpireQuestBehaviorTypeDefiner()
				: base(1005000)
			{
			}

			// Token: 0x06000600 RID: 1536 RVA: 0x00021514 File Offset: 0x0001F714
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(WeakenEmpireQuestBehavior.WeakenEmpireQuest), 1, null);
			}
		}

		// Token: 0x0200006F RID: 111
		public class WeakenEmpireQuest : StoryModeQuestBase
		{
			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x06000601 RID: 1537 RVA: 0x00021528 File Offset: 0x0001F728
			private TextObject _startQuestLog
			{
				get
				{
					TextObject textObject = new TextObject("{=0wQlpbtL}In order for the Empire to go into its final decline, there should be fewer than {NUMBER} imperial-owned settlements. If this happens, another kingdom can become the dominant power in Calradia.", null);
					textObject.SetTextVariable("NUMBER", 4);
					return textObject;
				}
			}

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x06000602 RID: 1538 RVA: 0x00021542 File Offset: 0x0001F742
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=iR4QCTxv}Weaken Empire", null);
				}
			}

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x06000603 RID: 1539 RVA: 0x0002154F File Offset: 0x0001F74F
			private TextObject _questCanceledLogText
			{
				get
				{
					return new TextObject("{=tVlZTOst}You have chosen a different path.", null);
				}
			}

			// Token: 0x06000604 RID: 1540 RVA: 0x0002155C File Offset: 0x0001F75C
			public WeakenEmpireQuest(Hero questGiver)
				: base("weaken_empire_quest", questGiver, CampaignTime.Never)
			{
				this._weakenedEmpire = false;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
				base.AddLog(this._startQuestLog, false);
			}

			// Token: 0x06000605 RID: 1541 RVA: 0x00021590 File Offset: 0x0001F790
			protected override void SetDialogs()
			{
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=VeY3PQFL}You chose to defeat the Empire.", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.CloseDialog();
			}

			// Token: 0x06000606 RID: 1542 RVA: 0x000215CE File Offset: 0x0001F7CE
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x06000607 RID: 1543 RVA: 0x000215D6 File Offset: 0x0001F7D6
			protected override void RegisterEvents()
			{
				StoryModeEvents.OnConspiracyActivatedEvent.AddNonSerializedListener(this, new Action(this.OnConspiracyActivated));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			}

			// Token: 0x06000608 RID: 1544 RVA: 0x00021606 File Offset: 0x0001F806
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (clan == Clan.PlayerClan && oldKingdom == StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom)
				{
					base.CompleteQuestWithCancel(this._questCanceledLogText);
					StoryModeManager.Current.MainStoryLine.CancelSecondAndThirdPhase();
				}
			}

			// Token: 0x06000609 RID: 1545 RVA: 0x0002163D File Offset: 0x0001F83D
			protected override void HourlyTick()
			{
				if (this.QuestConditionsHold())
				{
					this.SuccessComplete();
				}
			}

			// Token: 0x0600060A RID: 1546 RVA: 0x0002164D File Offset: 0x0001F84D
			private void OnConspiracyActivated()
			{
				if (!this._weakenedEmpire)
				{
					base.CompleteQuestWithFail(new TextObject("{=JVkPkbdg}You could not weaken the Empire.", null));
				}
			}

			// Token: 0x0600060B RID: 1547 RVA: 0x00021668 File Offset: 0x0001F868
			private bool QuestConditionsHold()
			{
				return StoryModeData.NorthernEmpireKingdom.Towns.Count + StoryModeData.WesternEmpireKingdom.Towns.Count + StoryModeData.SouthernEmpireKingdom.Towns.Count < 4;
			}

			// Token: 0x0600060C RID: 1548 RVA: 0x0002169C File Offset: 0x0001F89C
			private void SuccessComplete()
			{
				base.AddLog(new TextObject("{=wO19nK2y}You have weakened the Empire.", null), false);
				base.CompleteQuestWithSuccess();
				this._weakenedEmpire = true;
				SecondPhase.Instance.ActivateConspiracy();
			}

			// Token: 0x0600060D RID: 1549 RVA: 0x000216C8 File Offset: 0x0001F8C8
			internal static void AutoGeneratedStaticCollectObjectsWeakenEmpireQuest(object o, List<object> collectedObjects)
			{
				((WeakenEmpireQuestBehavior.WeakenEmpireQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600060E RID: 1550 RVA: 0x000216D6 File Offset: 0x0001F8D6
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0400022C RID: 556
			private const int EmpireDefeatSettlementCount = 4;

			// Token: 0x0400022D RID: 557
			private bool _weakenedEmpire;
		}
	}
}
