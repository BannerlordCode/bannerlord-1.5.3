using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200003C RID: 60
	[Tutorial("EncyclopediaSearchTutorial")]
	public class EncyclopediaSearchTutorial : TutorialItemBase
	{
		// Token: 0x06000116 RID: 278 RVA: 0x00004092 File Offset: 0x00002292
		public EncyclopediaSearchTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "EncyclopediaSearchButton";
			base.MouseRequired = false;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000040B3 File Offset: 0x000022B3
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.EncyclopediaWindow;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000040B8 File Offset: 0x000022B8
		public override bool IsConditionsMetForActivation()
		{
			bool isActive = this._isActive;
			this._isActive = TutorialHelper.CurrentContext == TutorialContexts.EncyclopediaWindow;
			if (!isActive && this._isActive)
			{
				Game.Current.EventManager.RegisterEvent<OnEncyclopediaSearchActivatedEvent>(new Action<OnEncyclopediaSearchActivatedEvent>(this.OnEncyclopediaSearchBarUsed));
			}
			else if (!this._isActive && isActive)
			{
				Game.Current.EventManager.UnregisterEvent<OnEncyclopediaSearchActivatedEvent>(new Action<OnEncyclopediaSearchActivatedEvent>(this.OnEncyclopediaSearchBarUsed));
			}
			return this._isActive;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004131 File Offset: 0x00002331
		private void OnEncyclopediaSearchBarUsed(OnEncyclopediaSearchActivatedEvent evnt)
		{
			this._isSearchButtonPressed = true;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000413A File Offset: 0x0000233A
		public override bool IsConditionsMetForCompletion()
		{
			if (this._isActive && this._isSearchButtonPressed)
			{
				Game.Current.EventManager.UnregisterEvent<OnEncyclopediaSearchActivatedEvent>(new Action<OnEncyclopediaSearchActivatedEvent>(this.OnEncyclopediaSearchBarUsed));
				return true;
			}
			return false;
		}

		// Token: 0x04000046 RID: 70
		private bool _isActive;

		// Token: 0x04000047 RID: 71
		private bool _isSearchButtonPressed;
	}
}
