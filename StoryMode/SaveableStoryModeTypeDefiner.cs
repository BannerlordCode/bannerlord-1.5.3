using System;
using System.Collections.Generic;
using StoryMode.Quests.FirstPhase;
using StoryMode.Quests.PlayerClanQuests;
using StoryMode.Quests.QuestTasks;
using StoryMode.Quests.SecondPhase;
using StoryMode.Quests.SecondPhase.ConspiracyQuests;
using StoryMode.Quests.TutorialPhase;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace StoryMode
{
	// Token: 0x0200000D RID: 13
	public class SaveableStoryModeTypeDefiner : SaveableTypeDefiner
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00002F2D File Offset: 0x0000112D
		public SaveableStoryModeTypeDefiner()
			: base(320000)
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002F3C File Offset: 0x0000113C
		protected override void DefineClassTypes()
		{
			base.AddClassDefinition(typeof(CampaignStoryMode), 1, null);
			base.AddClassDefinition(typeof(StoryModeManager), 2, null);
			base.AddClassDefinition(typeof(MainStoryLine), 3, null);
			base.AddClassDefinition(typeof(TrainingField), 4, null);
			base.AddClassDefinition(typeof(TrainingFieldEncounter), 5, null);
			base.AddClassDefinition(typeof(PurchaseItemTutorialQuestTask), 6, null);
			base.AddClassDefinition(typeof(RecruitTroopTutorialQuestTask), 7, null);
			base.AddClassDefinition(typeof(TutorialPhase), 8, null);
			base.AddClassDefinition(typeof(FirstPhase), 9, null);
			base.AddClassDefinition(typeof(SecondPhase), 10, null);
			base.AddClassDefinition(typeof(ThirdPhase), 11, null);
			base.AddClassDefinition(typeof(ConspiracyQuestBase), 12, null);
			base.AddClassDefinition(typeof(ConspiracyQuestMapNotification), 13, null);
			base.AddClassDefinition(typeof(ConspiracyBaseOfOperationsDiscoveredConspiracyQuest), 14, null);
			base.AddClassDefinition(typeof(DestroyRaidersConspiracyQuest), 15, null);
			base.AddClassDefinition(typeof(DisruptSupplyLinesConspiracyQuest), 17, null);
			base.AddClassDefinition(typeof(TravelToVillageTutorialQuest), 694001, null);
			base.AddClassDefinition(typeof(TalkToTheHeadmanTutorialQuest), 693001, null);
			base.AddClassDefinition(typeof(PurchaseGrainTutorialQuest), 691001, null);
			base.AddClassDefinition(typeof(RecruitTroopsTutorialQuest), 692001, null);
			base.AddClassDefinition(typeof(LocateAndRescueTravellerTutorialQuest), 688001, null);
			base.AddClassDefinition(typeof(FindHideoutTutorialQuest), 686001, null);
			base.AddClassDefinition(typeof(BannerInvestigationQuest), 684001, null);
			base.AddClassDefinition(typeof(AssembleTheBannerQuest), 683001, null);
			base.AddClassDefinition(typeof(MeetWithIstianaQuest), 690001, null);
			base.AddClassDefinition(typeof(MeetWithArzagosQuest), 689001, null);
			base.AddClassDefinition(typeof(IstianasBannerPieceQuest), 687001, null);
			base.AddClassDefinition(typeof(ArzagosBannerPieceQuest), 681001, null);
			base.AddClassDefinition(typeof(SupportKingdomQuest), 680001, null);
			base.AddClassDefinition(typeof(CreateKingdomQuest), 580001, null);
			base.AddClassDefinition(typeof(ConspiracyProgressQuest), 695001, null);
			base.AddClassDefinition(typeof(RebuildPlayerClanQuest), 3780001, null);
			base.AddClassDefinition(typeof(VillagersInNeed), 18, null);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000031E4 File Offset: 0x000013E4
		protected override void DefineStructTypes()
		{
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000031E8 File Offset: 0x000013E8
		protected override void DefineEnumTypes()
		{
			base.AddEnumDefinition(typeof(MainStoryLineSide), 2001, null);
			base.AddEnumDefinition(typeof(TutorialQuestPhase), 2002, null);
			base.AddEnumDefinition(typeof(FindHideoutTutorialQuest.HideoutBattleEndState), 686010, null);
			base.AddEnumDefinition(typeof(IstianasBannerPieceQuest.HideoutBattleEndState), 687010, null);
			base.AddEnumDefinition(typeof(ArzagosBannerPieceQuest.HideoutBattleEndState), 681010, null);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003263 File Offset: 0x00001463
		protected override void DefineInterfaceTypes()
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003265 File Offset: 0x00001465
		protected override void DefineRootClassTypes()
		{
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003267 File Offset: 0x00001467
		protected override void DefineGenericClassDefinitions()
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003269 File Offset: 0x00001469
		protected override void DefineGenericStructDefinitions()
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000326C File Offset: 0x0000146C
		protected override void DefineContainerDefinitions()
		{
			base.ConstructContainerDefinition(typeof(List<TrainingField>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, TrainingField>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, TrainingField>));
			base.ConstructContainerDefinition(typeof(Dictionary<int, CampaignTime>));
		}
	}
}
