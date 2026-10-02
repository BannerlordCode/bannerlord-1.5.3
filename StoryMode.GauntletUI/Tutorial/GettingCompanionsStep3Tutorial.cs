using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000011 RID: 17
	[Tutorial("GettingCompanionsStep3")]
	public class GettingCompanionsStep3Tutorial : TutorialItemBase
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00002942 File Offset: 0x00000B42
		public GettingCompanionsStep3Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "OverlayTalkButton";
			base.MouseRequired = true;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002963 File Offset: 0x00000B63
		public override bool IsConditionsMetForCompletion()
		{
			return this._startedTalkingWithCompanion;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000296B File Offset: 0x00000B6B
		public override void OnPlayerStartTalkFromMenuOverlay(Hero hero)
		{
			this._startedTalkingWithCompanion = hero.IsWanderer && !hero.IsPlayerCompanion;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002987 File Offset: 0x00000B87
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000298C File Offset: 0x00000B8C
		public override bool IsConditionsMetForActivation()
		{
			LocationComplex locationComplex = LocationComplex.Current;
			Location location = ((locationComplex != null) ? locationComplex.GetLocationWithId("tavern") : null);
			if (TutorialHelper.PlayerIsInNonEnemyTown && TutorialHelper.CurrentContext == TutorialContexts.MapWindow && TutorialHelper.BackStreetMenuIsOpen && TutorialHelper.IsCharacterPopUpWindowOpen && Clan.PlayerClan.Companions.Count == 0 && Clan.PlayerClan.CompanionLimit > 0)
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

		// Token: 0x04000014 RID: 20
		private bool _startedTalkingWithCompanion;
	}
}
