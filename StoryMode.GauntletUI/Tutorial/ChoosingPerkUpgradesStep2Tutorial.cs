using System;
using Helpers;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200000B RID: 11
	[Tutorial("ChoosingPerkUpgradesStep2")]
	public class ChoosingPerkUpgradesStep2Tutorial : TutorialItemBase
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00002605 File Offset: 0x00000805
		public ChoosingPerkUpgradesStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = "AvailablePerks";
			base.MouseRequired = true;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002626 File Offset: 0x00000826
		public override bool IsConditionsMetForCompletion()
		{
			return this._perkPopupOpened;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000262E File Offset: 0x0000082E
		public override void OnPerkSelectionToggle(PerkSelectionToggleEvent obj)
		{
			this._perkPopupOpened = true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002637 File Offset: 0x00000837
		public override bool IsConditionsMetForActivation()
		{
			return (TutorialHelper.PlayerIsInAnySettlement || TutorialHelper.PlayerIsSafeOnMap) && PerkHelper.AvailablePerkCountOfHero(Hero.MainHero) > 1 && TutorialHelper.CurrentContext == TutorialContexts.CharacterScreen;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000265E File Offset: 0x0000085E
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.CharacterScreen;
		}

		// Token: 0x0400000E RID: 14
		private bool _perkPopupOpened;
	}
}
