using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200003B RID: 59
	[Tutorial("EncyclopediaTrackTutorial")]
	public class EncyclopediaTrackTutorial : TutorialItemBase
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00003F7E File Offset: 0x0000217E
		public EncyclopediaTrackTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "EncyclopediaItemTrackButton";
			base.MouseRequired = false;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00003F9F File Offset: 0x0000219F
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.EncyclopediaWindow;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00003FA4 File Offset: 0x000021A4
		public override bool IsConditionsMetForActivation()
		{
			bool isActive = this._isActive;
			this._isActive = TutorialHelper.CurrentEncyclopediaPage == EncyclopediaPages.Settlement;
			if (!isActive && this._isActive)
			{
				Game.Current.EventManager.RegisterEvent<PlayerToggleTrackSettlementFromEncyclopediaEvent>(new Action<PlayerToggleTrackSettlementFromEncyclopediaEvent>(this.OnTrackToggledFromEncyclopedia));
			}
			else if (!this._isActive && isActive)
			{
				Game.Current.EventManager.UnregisterEvent<PlayerToggleTrackSettlementFromEncyclopediaEvent>(new Action<PlayerToggleTrackSettlementFromEncyclopediaEvent>(this.OnTrackToggledFromEncyclopedia));
			}
			return this._isActive;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004020 File Offset: 0x00002220
		public override bool IsConditionsMetForCompletion()
		{
			if (this._isActive)
			{
				bool flag = false;
				if (this._isActive)
				{
					if (TutorialHelper.CurrentContext != TutorialContexts.EncyclopediaWindow)
					{
						flag = true;
					}
					if (TutorialHelper.CurrentEncyclopediaPage != EncyclopediaPages.Hero && TutorialHelper.CurrentEncyclopediaPage != EncyclopediaPages.Settlement)
					{
						flag = true;
					}
					if (this._usedTrackFromEncyclopedia)
					{
						flag = true;
					}
				}
				if (flag)
				{
					Game.Current.EventManager.UnregisterEvent<PlayerToggleTrackSettlementFromEncyclopediaEvent>(new Action<PlayerToggleTrackSettlementFromEncyclopediaEvent>(this.OnTrackToggledFromEncyclopedia));
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004089 File Offset: 0x00002289
		private void OnTrackToggledFromEncyclopedia(PlayerToggleTrackSettlementFromEncyclopediaEvent callback)
		{
			this._usedTrackFromEncyclopedia = true;
		}

		// Token: 0x04000044 RID: 68
		private bool _isActive;

		// Token: 0x04000045 RID: 69
		private bool _usedTrackFromEncyclopedia;
	}
}
