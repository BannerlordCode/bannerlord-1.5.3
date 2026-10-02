using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200003D RID: 61
	[Tutorial("EncyclopediaFiltersTutorial")]
	public class EncyclopediaFiltersTutorial : TutorialItemBase
	{
		// Token: 0x0600011B RID: 283 RVA: 0x0000416A File Offset: 0x0000236A
		public EncyclopediaFiltersTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "EncyclopediaFiltersContainer";
			base.MouseRequired = false;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000418B File Offset: 0x0000238B
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.EncyclopediaWindow;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00004190 File Offset: 0x00002390
		public override bool IsConditionsMetForActivation()
		{
			bool isActive = this._isActive;
			EncyclopediaPages currentEncyclopediaPage = TutorialHelper.CurrentEncyclopediaPage;
			if (currentEncyclopediaPage - EncyclopediaPages.ListSettlements <= 6)
			{
				this._isActive = true;
			}
			else
			{
				this._isActive = false;
			}
			if (!isActive && this._isActive)
			{
				Game.Current.EventManager.RegisterEvent<OnEncyclopediaFilterActivatedEvent>(new Action<OnEncyclopediaFilterActivatedEvent>(this.OnFilterClicked));
			}
			else if (!this._isActive && isActive)
			{
				Game.Current.EventManager.UnregisterEvent<OnEncyclopediaFilterActivatedEvent>(new Action<OnEncyclopediaFilterActivatedEvent>(this.OnFilterClicked));
			}
			return this._isActive;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004216 File Offset: 0x00002416
		private void OnFilterClicked(OnEncyclopediaFilterActivatedEvent evnt)
		{
			this._isAnyFilterSelected = true;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000421F File Offset: 0x0000241F
		public override bool IsConditionsMetForCompletion()
		{
			if (this._isActive && this._isAnyFilterSelected)
			{
				Game.Current.EventManager.UnregisterEvent<OnEncyclopediaFilterActivatedEvent>(new Action<OnEncyclopediaFilterActivatedEvent>(this.OnFilterClicked));
				return true;
			}
			return false;
		}

		// Token: 0x04000048 RID: 72
		private bool _isActive;

		// Token: 0x04000049 RID: 73
		private bool _isAnyFilterSelected;
	}
}
