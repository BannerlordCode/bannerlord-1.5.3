using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200003E RID: 62
	[Tutorial("EncyclopediaSortTutorial")]
	public class EncyclopediaSortTutorial : TutorialItemBase
	{
		// Token: 0x06000120 RID: 288 RVA: 0x0000424F File Offset: 0x0000244F
		public EncyclopediaSortTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "EncyclopediaSortButton";
			base.MouseRequired = false;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00004270 File Offset: 0x00002470
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.EncyclopediaWindow;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004274 File Offset: 0x00002474
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
				Game.Current.EventManager.RegisterEvent<OnEncyclopediaListSortedEvent>(new Action<OnEncyclopediaListSortedEvent>(this.OnSortClicked));
			}
			else if (!this._isActive && isActive)
			{
				Game.Current.EventManager.UnregisterEvent<OnEncyclopediaListSortedEvent>(new Action<OnEncyclopediaListSortedEvent>(this.OnSortClicked));
			}
			return this._isActive;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000042FA File Offset: 0x000024FA
		private void OnSortClicked(OnEncyclopediaListSortedEvent evnt)
		{
			this._isSortClicked = true;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004303 File Offset: 0x00002503
		public override bool IsConditionsMetForCompletion()
		{
			if (this._isActive && this._isSortClicked)
			{
				Game.Current.EventManager.UnregisterEvent<OnEncyclopediaListSortedEvent>(new Action<OnEncyclopediaListSortedEvent>(this.OnSortClicked));
				return true;
			}
			return false;
		}

		// Token: 0x0400004A RID: 74
		private bool _isActive;

		// Token: 0x0400004B RID: 75
		private bool _isSortClicked;
	}
}
