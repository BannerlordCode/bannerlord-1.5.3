using System;
using Helpers;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200000A RID: 10
	[Tutorial("ChoosingPerkUpgradesStep1")]
	public class ChoosingPerkUpgradesStep1Tutorial : TutorialItemBase
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000025A1 File Offset: 0x000007A1
		public ChoosingPerkUpgradesStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "character_developer";
			base.MouseRequired = true;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000025C2 File Offset: 0x000007C2
		public override bool IsConditionsMetForCompletion()
		{
			return this._contextChangedToCharacterScreen;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000025CA File Offset: 0x000007CA
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025CD File Offset: 0x000007CD
		public override bool IsConditionsMetForActivation()
		{
			return (TutorialHelper.PlayerIsInAnySettlement || TutorialHelper.PlayerIsSafeOnMap) && PerkHelper.AvailablePerkCountOfHero(Hero.MainHero) > 1 && TutorialHelper.CurrentContext == TutorialContexts.MapWindow;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000025F4 File Offset: 0x000007F4
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._contextChangedToCharacterScreen = obj.NewContext == TutorialContexts.CharacterScreen;
		}

		// Token: 0x0400000D RID: 13
		private bool _contextChangedToCharacterScreen;
	}
}
