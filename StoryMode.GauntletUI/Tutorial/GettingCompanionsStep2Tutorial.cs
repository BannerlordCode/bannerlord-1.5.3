using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000010 RID: 16
	[Tutorial("GettingCompanionsStep2")]
	public class GettingCompanionsStep2Tutorial : TutorialItemBase
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00002866 File Offset: 0x00000A66
		public GettingCompanionsStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "ApplicapleCompanion";
			base.MouseRequired = true;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002887 File Offset: 0x00000A87
		public override bool IsConditionsMetForCompletion()
		{
			return this._wantedCharacterPopupOpened;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000288F File Offset: 0x00000A8F
		public override void OnCharacterPortraitPopUpOpened(CharacterObject obj)
		{
			this._wantedCharacterPopupOpened = obj != null && obj.IsHero && obj.HeroObject.IsWanderer;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000028B0 File Offset: 0x00000AB0
		public override bool IsConditionsMetForActivation()
		{
			LocationComplex locationComplex = LocationComplex.Current;
			Location location = ((locationComplex != null) ? locationComplex.GetLocationWithId("tavern") : null);
			if (!TutorialHelper.IsCharacterPopUpWindowOpen && TutorialHelper.CurrentContext == TutorialContexts.MapWindow && TutorialHelper.PlayerIsInNonEnemyTown && TutorialHelper.BackStreetMenuIsOpen && Clan.PlayerClan.Companions.Count == 0 && Clan.PlayerClan.CompanionLimit > 0)
			{
				bool? flag = TutorialHelper.IsThereAvailableCompanionInLocation(location);
				bool flag2 = true;
				if ((flag.GetValueOrDefault() == flag2) & (flag != null))
				{
					return Hero.MainHero.Gold > TutorialHelper.MinimumGoldForCompanion;
				}
			}
			return false;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000293F File Offset: 0x00000B3F
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x04000013 RID: 19
		private bool _wantedCharacterPopupOpened;
	}
}
