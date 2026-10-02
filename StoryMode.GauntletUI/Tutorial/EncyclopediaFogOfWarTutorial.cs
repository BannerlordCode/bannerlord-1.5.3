using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200003F RID: 63
	[Tutorial("EncyclopediaFogOfWarTutorial")]
	public class EncyclopediaFogOfWarTutorial : TutorialItemBase
	{
		// Token: 0x06000125 RID: 293 RVA: 0x00004333 File Offset: 0x00002533
		public EncyclopediaFogOfWarTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "";
			base.MouseRequired = false;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00004354 File Offset: 0x00002554
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			if (!this._registeredEvents && TutorialHelper.CurrentContext == TutorialContexts.EncyclopediaWindow)
			{
				Game.Current.EventManager.RegisterEvent<EncyclopediaPageChangedEvent>(new Action<EncyclopediaPageChangedEvent>(this.OnLimitedInformationPageOpened));
				this._registeredEvents = true;
			}
			else if (this._registeredEvents && TutorialHelper.CurrentContext != TutorialContexts.EncyclopediaWindow)
			{
				Game.Current.EventManager.UnregisterEvent<EncyclopediaPageChangedEvent>(new Action<EncyclopediaPageChangedEvent>(this.OnLimitedInformationPageOpened));
				this._registeredEvents = false;
			}
			return TutorialContexts.EncyclopediaWindow;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000043CB File Offset: 0x000025CB
		public override void OnTutorialContextChanged(TutorialContextChangedEvent evnt)
		{
			base.OnTutorialContextChanged(evnt);
			if (this._registeredEvents && evnt.NewContext != TutorialContexts.EncyclopediaWindow)
			{
				Game.Current.EventManager.UnregisterEvent<EncyclopediaPageChangedEvent>(new Action<EncyclopediaPageChangedEvent>(this.OnLimitedInformationPageOpened));
				this._registeredEvents = false;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004408 File Offset: 0x00002608
		public override bool IsConditionsMetForActivation()
		{
			if (!this._registeredEvents)
			{
				Game.Current.EventManager.RegisterEvent<EncyclopediaPageChangedEvent>(new Action<EncyclopediaPageChangedEvent>(this.OnLimitedInformationPageOpened));
				this._registeredEvents = true;
			}
			return this._isActive;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000443C File Offset: 0x0000263C
		public override bool IsConditionsMetForCompletion()
		{
			if (!this._lastActiveState && this._isActive)
			{
				this._activatedPage = TutorialHelper.CurrentEncyclopediaPage;
			}
			if (this._lastActiveState && this._isActive && this._activatedPage != TutorialHelper.CurrentEncyclopediaPage)
			{
				Game.Current.EventManager.UnregisterEvent<EncyclopediaPageChangedEvent>(new Action<EncyclopediaPageChangedEvent>(this.OnLimitedInformationPageOpened));
				return true;
			}
			this._lastActiveState = this._isActive;
			return false;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000044AB File Offset: 0x000026AB
		private void OnLimitedInformationPageOpened(EncyclopediaPageChangedEvent evnt)
		{
			if (evnt.NewPageHasHiddenInformation)
			{
				this._isActive = true;
			}
		}

		// Token: 0x0400004C RID: 76
		private EncyclopediaPages _activatedPage;

		// Token: 0x0400004D RID: 77
		private bool _registeredEvents;

		// Token: 0x0400004E RID: 78
		private bool _lastActiveState;

		// Token: 0x0400004F RID: 79
		private bool _isActive;
	}
}
