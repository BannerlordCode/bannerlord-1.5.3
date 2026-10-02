using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000043 RID: 67
	[Tutorial("CraftingStep1Tutorial")]
	public class CraftingStep1Tutorial : TutorialItemBase
	{
		// Token: 0x0600013B RID: 315 RVA: 0x000045DB File Offset: 0x000027DB
		public CraftingStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Top;
			base.HighlightedVisualElementID = "FreeModeClassSelectionButton";
			base.MouseRequired = false;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000045FC File Offset: 0x000027FC
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.CraftingScreen;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004600 File Offset: 0x00002800
		public override void OnCraftingWeaponClassSelectionOpened(CraftingWeaponClassSelectionOpenedEvent obj)
		{
			this._craftingCategorySelectionOpened = obj.IsOpen;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000460E File Offset: 0x0000280E
		public override void OnCraftingOrderSelectionOpened(CraftingOrderSelectionOpenedEvent obj)
		{
			this._craftingOrderSelectionOpened = obj.IsOpen;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000461C File Offset: 0x0000281C
		public override void OnCraftingOnWeaponResultPopupOpened(CraftingWeaponResultPopupToggledEvent obj)
		{
			this._craftingOrderResultOpened = obj.IsOpen;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000462A File Offset: 0x0000282A
		public override bool IsConditionsMetForActivation()
		{
			return !this._craftingOrderSelectionOpened && !this._craftingOrderResultOpened;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000463F File Offset: 0x0000283F
		public override bool IsConditionsMetForCompletion()
		{
			return this._craftingCategorySelectionOpened;
		}

		// Token: 0x04000054 RID: 84
		private bool _craftingCategorySelectionOpened;

		// Token: 0x04000055 RID: 85
		private bool _craftingOrderSelectionOpened;

		// Token: 0x04000056 RID: 86
		private bool _craftingOrderResultOpened;
	}
}
