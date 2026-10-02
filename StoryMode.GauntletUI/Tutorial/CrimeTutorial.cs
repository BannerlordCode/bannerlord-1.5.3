using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000046 RID: 70
	[Tutorial("CrimeTutorial")]
	public class CrimeTutorial : TutorialItemBase
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00004796 File Offset: 0x00002996
		public CrimeTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Top;
			base.HighlightedVisualElementID = "CrimeLabel";
			base.MouseRequired = false;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000047B7 File Offset: 0x000029B7
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000047BA File Offset: 0x000029BA
		public override void OnCrimeValueInspectedInSettlementOverlay(CrimeValueInspectedInSettlementOverlayEvent obj)
		{
			this._inspectedCrimeValueItem = true;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000047C3 File Offset: 0x000029C3
		public override bool IsConditionsMetForActivation()
		{
			if (TutorialHelper.TownMenuIsOpen)
			{
				IFaction mapFaction = Settlement.CurrentSettlement.MapFaction;
				return mapFaction != null && mapFaction.MainHeroCrimeRating > 0f;
			}
			return false;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000047EA File Offset: 0x000029EA
		public override bool IsConditionsMetForCompletion()
		{
			return this._inspectedCrimeValueItem;
		}

		// Token: 0x0400005C RID: 92
		private bool _inspectedCrimeValueItem;
	}
}
