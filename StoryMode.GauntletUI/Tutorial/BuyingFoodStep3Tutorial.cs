using System;
using System.Collections.Generic;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000009 RID: 9
	[Tutorial("GetSuppliesTutorialStep3")]
	public class BuyingFoodStep3Tutorial : TutorialItemBase
	{
		// Token: 0x06000028 RID: 40 RVA: 0x000024FF File Offset: 0x000006FF
		public BuyingFoodStep3Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "TransferButtonOnlyFood";
			base.MouseRequired = true;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002520 File Offset: 0x00000720
		public override bool IsConditionsMetForCompletion()
		{
			return this._purchasedFoodCount >= 2;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000252E File Offset: 0x0000072E
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.BuyingFoodBaseConditions && TutorialHelper.CurrentContext == TutorialContexts.InventoryScreen;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002544 File Offset: 0x00000744
		public override void OnPlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
			for (int i = 0; i < purchasedItems.Count; i++)
			{
				ValueTuple<ItemRosterElement, int> valueTuple = purchasedItems[i];
				if (valueTuple.Item1.EquipmentElement.Item == DefaultItems.Grain)
				{
					this._purchasedFoodCount += valueTuple.Item1.Amount;
				}
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000259E File Offset: 0x0000079E
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.InventoryScreen;
		}

		// Token: 0x0400000C RID: 12
		private int _purchasedFoodCount;
	}
}
