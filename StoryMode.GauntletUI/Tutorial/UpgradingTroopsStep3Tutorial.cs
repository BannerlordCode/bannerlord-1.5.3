using System;
using Helpers;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000006 RID: 6
	[Tutorial("UpgradingTroopsStep3")]
	public class UpgradingTroopsStep3Tutorial : TutorialItemBase
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000023CB File Offset: 0x000005CB
		public UpgradingTroopsStep3Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "UpgradeButton";
			base.MouseRequired = true;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000023EC File Offset: 0x000005EC
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerUpgradedTroop;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000023F4 File Offset: 0x000005F4
		public override void OnPlayerUpgradeTroop(CharacterObject arg1, CharacterObject arg2, int arg3)
		{
			this._playerUpgradedTroop = true;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023FD File Offset: 0x000005FD
		public override bool IsConditionsMetForActivation()
		{
			if (Hero.MainHero.Gold <= 100 || TutorialHelper.CurrentContext != TutorialContexts.PartyScreen)
			{
				return false;
			}
			PartyState activePartyState = PartyScreenHelper.GetActivePartyState();
			return (activePartyState == null || activePartyState.PartyScreenMode == PartyScreenHelper.PartyScreenMode.Normal) && !TutorialHelper.AreTroopUpgradesDisabled && TutorialHelper.PlayerHasAnyUpgradeableTroop;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000243A File Offset: 0x0000063A
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.PartyScreen;
		}

		// Token: 0x04000009 RID: 9
		private bool _playerUpgradedTroop;
	}
}
