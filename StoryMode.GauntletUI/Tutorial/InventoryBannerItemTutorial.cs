using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000045 RID: 69
	[Tutorial("InventoryBannerItemTutorial")]
	public class InventoryBannerItemTutorial : TutorialItemBase
	{
		// Token: 0x0600014A RID: 330 RVA: 0x00004711 File Offset: 0x00002911
		public InventoryBannerItemTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Center;
			base.HighlightedVisualElementID = "InventoryOtherBannerItems";
			base.MouseRequired = false;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00004732 File Offset: 0x00002932
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.InventoryScreen;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00004738 File Offset: 0x00002938
		public override void OnInventoryItemInspected(InventoryItemInspectedEvent obj)
		{
			if (obj.Item.EquipmentElement.Item.IsBannerItem && obj.ItemSide == InventoryLogic.InventorySide.OtherInventory)
			{
				this._inspectedOtherBannerItem = true;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00004771 File Offset: 0x00002971
		public override bool IsConditionsMetForActivation()
		{
			return TutorialPhase.Instance.IsCompleted && TutorialHelper.CurrentContext == TutorialContexts.InventoryScreen && TutorialHelper.CurrentInventoryScreenIncludesBannerItem;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000478E File Offset: 0x0000298E
		public override bool IsConditionsMetForCompletion()
		{
			return this._inspectedOtherBannerItem;
		}

		// Token: 0x0400005B RID: 91
		private bool _inspectedOtherBannerItem;
	}
}
