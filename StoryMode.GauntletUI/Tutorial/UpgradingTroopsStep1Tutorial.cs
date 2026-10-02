using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000004 RID: 4
	[Tutorial("UpgradingTroopsStep1")]
	public class UpgradingTroopsStep1Tutorial : TutorialItemBase
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000022B8 File Offset: 0x000004B8
		public UpgradingTroopsStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "party";
			base.MouseRequired = true;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022D9 File Offset: 0x000004D9
		public override bool IsConditionsMetForCompletion()
		{
			return this._partyScreenOpened || this._playerUpgradedTroop;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022EB File Offset: 0x000004EB
		public override void OnPlayerUpgradeTroop(CharacterObject arg1, CharacterObject arg2, int arg3)
		{
			this._playerUpgradedTroop = true;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022F4 File Offset: 0x000004F4
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._partyScreenOpened = obj.NewContext == TutorialContexts.PartyScreen;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002305 File Offset: 0x00000505
		public override bool IsConditionsMetForActivation()
		{
			return Hero.MainHero.Gold >= 100 && TutorialHelper.CurrentContext == TutorialContexts.MapWindow && !TutorialHelper.PlayerIsInAnySettlement && TutorialHelper.PlayerIsSafeOnMap && !TutorialHelper.AreTroopUpgradesDisabled && TutorialHelper.PlayerHasAnyUpgradeableTroop;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000233B File Offset: 0x0000053B
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x04000005 RID: 5
		private bool _partyScreenOpened;

		// Token: 0x04000006 RID: 6
		private bool _playerUpgradedTroop;
	}
}
