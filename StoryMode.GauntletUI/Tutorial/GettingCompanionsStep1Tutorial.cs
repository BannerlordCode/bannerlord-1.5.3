using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200000F RID: 15
	[Tutorial("GettingCompanionsStep1")]
	public class GettingCompanionsStep1Tutorial : TutorialItemBase
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00002780 File Offset: 0x00000980
		public GettingCompanionsStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "town_backstreet";
			base.MouseRequired = true;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000027A1 File Offset: 0x000009A1
		public override bool IsConditionsMetForCompletion()
		{
			return this._wantedGameMenuOpened;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000027A9 File Offset: 0x000009A9
		public override void OnGameMenuOpened(MenuCallbackArgs obj)
		{
			base.OnGameMenuOpened(obj);
			this._wantedGameMenuOpened = obj.MenuContext.GameMenu.StringId == "town_backstreet";
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000027D4 File Offset: 0x000009D4
		public override bool IsConditionsMetForActivation()
		{
			LocationComplex locationComplex = LocationComplex.Current;
			Location location = ((locationComplex != null) ? locationComplex.GetLocationWithId("tavern") : null);
			if (!TutorialHelper.IsCharacterPopUpWindowOpen && TutorialHelper.PlayerIsInNonEnemyTown && TutorialHelper.TownMenuIsOpen && Clan.PlayerClan.Companions.Count == 0 && Clan.PlayerClan.CompanionLimit > 0)
			{
				bool? flag = TutorialHelper.IsThereAvailableCompanionInLocation(location);
				bool flag2 = true;
				if (((flag.GetValueOrDefault() == flag2) & (flag != null)) && Hero.MainHero.Gold > TutorialHelper.MinimumGoldForCompanion)
				{
					return TutorialHelper.CurrentContext == TutorialContexts.MapWindow;
				}
			}
			return false;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002863 File Offset: 0x00000A63
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x04000012 RID: 18
		private bool _wantedGameMenuOpened;
	}
}
