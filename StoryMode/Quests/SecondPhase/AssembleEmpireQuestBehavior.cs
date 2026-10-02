using System;
using System.Collections.Generic;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.SecondPhase
{
	// Token: 0x02000026 RID: 38
	public class AssembleEmpireQuestBehavior : CampaignBehaviorBase
	{
		// Token: 0x060001EF RID: 495 RVA: 0x0000B677 File Offset: 0x00009877
		public override void RegisterEvents()
		{
			StoryModeEvents.OnMainStoryLineSideChosenEvent.AddNonSerializedListener(this, new Action<MainStoryLineSide>(this.OnMainStoryLineSideChosen));
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000B690 File Offset: 0x00009890
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000B692 File Offset: 0x00009892
		private void OnMainStoryLineSideChosen(MainStoryLineSide side)
		{
			if (side == MainStoryLineSide.CreateImperialKingdom || side == MainStoryLineSide.SupportImperialKingdom)
			{
				new AssembleEmpireQuestBehavior.AssembleEmpireQuest(StoryModeHeroes.ImperialMentor).StartQuest();
			}
		}

		// Token: 0x02000069 RID: 105
		public class AssembleEmpireQuestBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060005E3 RID: 1507 RVA: 0x00021068 File Offset: 0x0001F268
			public AssembleEmpireQuestBehaviorTypeDefiner()
				: base(1002000)
			{
			}

			// Token: 0x060005E4 RID: 1508 RVA: 0x00021075 File Offset: 0x0001F275
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(AssembleEmpireQuestBehavior.AssembleEmpireQuest), 1, null);
			}
		}

		// Token: 0x0200006A RID: 106
		public class AssembleEmpireQuest : StoryModeQuestBase
		{
			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x060005E5 RID: 1509 RVA: 0x00021089 File Offset: 0x0001F289
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=ya8eMCpj}Unify the Empire", null);
				}
			}

			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00021096 File Offset: 0x0001F296
			private TextObject _questCanceledLogText
			{
				get
				{
					return new TextObject("{=tVlZTOst}You have chosen a different path.", null);
				}
			}

			// Token: 0x060005E7 RID: 1511 RVA: 0x000210A4 File Offset: 0x0001F2A4
			public AssembleEmpireQuest(Hero questGiver)
				: base("assemble_empire_quest", questGiver, CampaignTime.Never)
			{
				this._assembledEmpire = false;
				this.CacheSettlementCounts();
				this.SetDialogs();
				base.InitializeQuestOnCreation();
				this._numberOfCapturedSettlementsLog = base.AddDiscreteLog(new TextObject("{=3deb2lMd}To restore the Empire you should capture two thirds of settlements with imperial culture.", null), new TextObject("{=Dp6newHS}Conquered Settlements", null), this._ownedByPlayerImperialTowns, MathF.Ceiling((float)this._imperialCultureTowns * 0.66f), null, false);
			}

			// Token: 0x060005E8 RID: 1512 RVA: 0x00021117 File Offset: 0x0001F317
			protected override void SetDialogs()
			{
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=mxKhvbn7}You have decided to unify the Empire.", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.CloseDialog();
			}

			// Token: 0x060005E9 RID: 1513 RVA: 0x00021158 File Offset: 0x0001F358
			protected override void InitializeQuestOnGameLoad()
			{
				this.CacheSettlementCounts();
				this.SetDialogs();
				if (this._numberOfCapturedSettlementsLog == null)
				{
					this._numberOfCapturedSettlementsLog = base.AddDiscreteLog(new TextObject("{=3deb2lMd}To restore the Empire you should capture two thirds of settlements with imperial culture.", null), new TextObject("{=Dp6newHS}Conquered Settlements", null), this._ownedByPlayerImperialTowns, MathF.Ceiling((float)this._imperialCultureTowns * 0.66f), null, false);
				}
				this._numberOfCapturedSettlementsLog.UpdateCurrentProgress((int)MathF.Clamp((float)this._ownedByPlayerImperialTowns, 0f, (float)this._imperialCultureTowns));
			}

			// Token: 0x060005EA RID: 1514 RVA: 0x000211DC File Offset: 0x0001F3DC
			protected override void RegisterEvents()
			{
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
				StoryModeEvents.OnConspiracyActivatedEvent.AddNonSerializedListener(this, new Action(this.OnConspiracyActivated));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			}

			// Token: 0x060005EB RID: 1515 RVA: 0x0002122E File Offset: 0x0001F42E
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (clan == Clan.PlayerClan && oldKingdom == StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom)
				{
					base.CompleteQuestWithCancel(this._questCanceledLogText);
					StoryModeManager.Current.MainStoryLine.CancelSecondAndThirdPhase();
				}
			}

			// Token: 0x060005EC RID: 1516 RVA: 0x00021268 File Offset: 0x0001F468
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				if (settlement.IsTown && settlement.Culture.StringId == "empire")
				{
					if (settlement.OwnerClan.Kingdom == Clan.PlayerClan.Kingdom && oldOwner.Clan.Kingdom != Clan.PlayerClan.Kingdom)
					{
						this._ownedByPlayerImperialTowns++;
					}
					if (oldOwner.Clan.Kingdom == Clan.PlayerClan.Kingdom && newOwner.Clan.Kingdom != Clan.PlayerClan.Kingdom)
					{
						this._ownedByPlayerImperialTowns--;
					}
					this._numberOfCapturedSettlementsLog.UpdateCurrentProgress((int)MathF.Clamp((float)this._ownedByPlayerImperialTowns, 0f, (float)this._imperialCultureTowns));
				}
			}

			// Token: 0x060005ED RID: 1517 RVA: 0x00021338 File Offset: 0x0001F538
			protected override void HourlyTick()
			{
				if (this.QuestConditionsHold())
				{
					this.SuccessQuest();
				}
			}

			// Token: 0x060005EE RID: 1518 RVA: 0x00021348 File Offset: 0x0001F548
			private void OnConspiracyActivated()
			{
				if (!this._assembledEmpire)
				{
					base.CompleteQuestWithFail(new TextObject("{=80NOk1Ee}You could not unify the Empire.", null));
				}
			}

			// Token: 0x060005EF RID: 1519 RVA: 0x00021364 File Offset: 0x0001F564
			private void CacheSettlementCounts()
			{
				this._imperialCultureTowns = 0;
				this._ownedByPlayerImperialTowns = 0;
				foreach (Settlement settlement in Settlement.All)
				{
					if (settlement.IsTown && settlement.Culture.StringId == "empire")
					{
						this._imperialCultureTowns++;
						if (settlement.OwnerClan.Kingdom == Clan.PlayerClan.Kingdom)
						{
							this._ownedByPlayerImperialTowns++;
						}
					}
				}
			}

			// Token: 0x060005F0 RID: 1520 RVA: 0x00021410 File Offset: 0x0001F610
			private bool QuestConditionsHold()
			{
				return this._ownedByPlayerImperialTowns >= MathF.Ceiling((float)this._imperialCultureTowns * 0.66f);
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x0002142F File Offset: 0x0001F62F
			private void SuccessQuest()
			{
				base.AddLog(new TextObject("{=sJeYHMGG}You have unified the Empire.", null), false);
				base.CompleteQuestWithSuccess();
				this._assembledEmpire = true;
				SecondPhase.Instance.ActivateConspiracy();
			}

			// Token: 0x060005F2 RID: 1522 RVA: 0x0002145B File Offset: 0x0001F65B
			internal static void AutoGeneratedStaticCollectObjectsAssembleEmpireQuest(object o, List<object> collectedObjects)
			{
				((AssembleEmpireQuestBehavior.AssembleEmpireQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060005F3 RID: 1523 RVA: 0x00021469 File Offset: 0x0001F669
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._numberOfCapturedSettlementsLog);
			}

			// Token: 0x060005F4 RID: 1524 RVA: 0x0002147E File Offset: 0x0001F67E
			internal static object AutoGeneratedGetMemberValue_numberOfCapturedSettlementsLog(object o)
			{
				return ((AssembleEmpireQuestBehavior.AssembleEmpireQuest)o)._numberOfCapturedSettlementsLog;
			}

			// Token: 0x04000222 RID: 546
			private int _imperialCultureTowns;

			// Token: 0x04000223 RID: 547
			private int _ownedByPlayerImperialTowns;

			// Token: 0x04000224 RID: 548
			private bool _assembledEmpire;

			// Token: 0x04000225 RID: 549
			private const float _ratioOfSettlementToTake = 0.66f;

			// Token: 0x04000226 RID: 550
			[SaveableField(1)]
			private JournalLog _numberOfCapturedSettlementsLog;
		}
	}
}
