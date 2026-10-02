using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000008 RID: 8
	[Tutorial("GetSuppliesTutorialStep2")]
	public class BuyingFoodStep2Tutorial : TutorialItemBase
	{
		// Token: 0x06000023 RID: 35 RVA: 0x000024AF File Offset: 0x000006AF
		public BuyingFoodStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "InventoryMicsFilter";
			base.MouseRequired = true;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000024D0 File Offset: 0x000006D0
		public override bool IsConditionsMetForCompletion()
		{
			return this._filterChangedToMisc;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000024D8 File Offset: 0x000006D8
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.BuyingFoodBaseConditions && TutorialHelper.CurrentContext == TutorialContexts.InventoryScreen;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000024EB File Offset: 0x000006EB
		public override void OnInventoryFilterChanged(InventoryFilterChangedEvent obj)
		{
			this._filterChangedToMisc = obj.NewFilter == SPInventoryVM.Filters.Miscellaneous;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000024FC File Offset: 0x000006FC
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.InventoryScreen;
		}

		// Token: 0x0400000B RID: 11
		private bool _filterChangedToMisc;
	}
}
