using System;
using Helpers;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.ViewModelCollection.Party;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000005 RID: 5
	[Tutorial("UpgradingTroopsStep2")]
	public class UpgradingTroopsStep2Tutorial : TutorialItemBase
	{
		// Token: 0x06000013 RID: 19 RVA: 0x0000233E File Offset: 0x0000053E
		public UpgradingTroopsStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Left;
			base.HighlightedVisualElementID = "UpgradePopupButton";
			base.MouseRequired = true;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000235F File Offset: 0x0000055F
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerUpgradedTroop || this._playerOpenedUpgradePopup;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002371 File Offset: 0x00000571
		public override void OnPlayerToggledUpgradePopup(PlayerToggledUpgradePopupEvent obj)
		{
			if (obj.IsOpened)
			{
				this._playerOpenedUpgradePopup = true;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002382 File Offset: 0x00000582
		public override void OnPlayerUpgradeTroop(CharacterObject arg1, CharacterObject arg2, int arg3)
		{
			this._playerUpgradedTroop = true;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000238B File Offset: 0x0000058B
		public override bool IsConditionsMetForActivation()
		{
			if (Hero.MainHero.Gold <= 100 || TutorialHelper.CurrentContext != TutorialContexts.PartyScreen)
			{
				return false;
			}
			PartyState activePartyState = PartyScreenHelper.GetActivePartyState();
			return (activePartyState == null || activePartyState.PartyScreenMode == PartyScreenHelper.PartyScreenMode.Normal) && !TutorialHelper.AreTroopUpgradesDisabled && TutorialHelper.PlayerHasAnyUpgradeableTroop;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000023C8 File Offset: 0x000005C8
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.PartyScreen;
		}

		// Token: 0x04000007 RID: 7
		private bool _playerUpgradedTroop;

		// Token: 0x04000008 RID: 8
		private bool _playerOpenedUpgradePopup;
	}
}
