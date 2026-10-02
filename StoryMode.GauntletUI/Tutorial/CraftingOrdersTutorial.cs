using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Tutorial;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000044 RID: 68
	[Tutorial("CraftingOrdersTutorial")]
	public class CraftingOrdersTutorial : TutorialItemBase
	{
		// Token: 0x06000142 RID: 322 RVA: 0x00004647 File Offset: 0x00002847
		public CraftingOrdersTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Top;
			base.HighlightedVisualElementID = "CraftingOrdersButton";
			base.MouseRequired = false;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00004668 File Offset: 0x00002868
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.CraftingScreen;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000466C File Offset: 0x0000286C
		public override void OnCraftingWeaponClassSelectionOpened(CraftingWeaponClassSelectionOpenedEvent obj)
		{
			this._craftingCategorySelectionOpened = obj.IsOpen;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000467C File Offset: 0x0000287C
		public override void OnCraftingOrderTabOpened(CraftingOrderTabOpenedEvent obj)
		{
			this._craftingOrderTabOpened = obj.IsOpen;
			if (this._craftingOrderTabOpened)
			{
				base.HighlightedVisualElementID = "OrderSelectionButton";
			}
			else
			{
				base.HighlightedVisualElementID = "CraftingOrdersButton";
			}
			Game game = Game.Current;
			if (game == null)
			{
				return;
			}
			game.EventManager.TriggerEvent<TutorialNotificationElementChangeEvent>(new TutorialNotificationElementChangeEvent(base.HighlightedVisualElementID));
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000046D4 File Offset: 0x000028D4
		public override void OnCraftingOrderSelectionOpened(CraftingOrderSelectionOpenedEvent obj)
		{
			this._craftingOrderSelectionOpened = obj.IsOpen;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000046E2 File Offset: 0x000028E2
		public override void OnCraftingOnWeaponResultPopupOpened(CraftingWeaponResultPopupToggledEvent obj)
		{
			this._craftingOrderResultOpened = obj.IsOpen;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000046F0 File Offset: 0x000028F0
		public override bool IsConditionsMetForActivation()
		{
			return !this._craftingCategorySelectionOpened && !this._craftingOrderResultOpened && TutorialHelper.IsCurrentTownHaveDoableCraftingOrder;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00004709 File Offset: 0x00002909
		public override bool IsConditionsMetForCompletion()
		{
			return this._craftingOrderSelectionOpened;
		}

		// Token: 0x04000057 RID: 87
		private bool _craftingCategorySelectionOpened;

		// Token: 0x04000058 RID: 88
		private bool _craftingOrderSelectionOpened;

		// Token: 0x04000059 RID: 89
		private bool _craftingOrderResultOpened;

		// Token: 0x0400005A RID: 90
		private bool _craftingOrderTabOpened;
	}
}
