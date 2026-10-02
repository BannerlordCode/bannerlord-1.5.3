using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using StoryMode.Quests.TutorialPhase;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000007 RID: 7
	[Tutorial("GetSuppliesTutorialStep1")]
	public class BuyingFoodStep1Tutorial : TutorialItemBase
	{
		// Token: 0x0600001E RID: 30 RVA: 0x0000243D File Offset: 0x0000063D
		public BuyingFoodStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "storymode_tutorial_village_buy";
			base.MouseRequired = true;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000245E File Offset: 0x0000065E
		public override bool IsConditionsMetForCompletion()
		{
			return this._contextChangedToInventory;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002466 File Offset: 0x00000666
		public override bool IsConditionsMetForActivation()
		{
			return !TutorialHelper.IsCharacterPopUpWindowOpen && TutorialHelper.BuyingFoodBaseConditions && !Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(RecruitTroopsTutorialQuest)) && TutorialHelper.CurrentContext == TutorialContexts.MapWindow;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000249B File Offset: 0x0000069B
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._contextChangedToInventory = obj.NewContext == TutorialContexts.InventoryScreen;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000024AC File Offset: 0x000006AC
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x0400000A RID: 10
		private bool _contextChangedToInventory;
	}
}
