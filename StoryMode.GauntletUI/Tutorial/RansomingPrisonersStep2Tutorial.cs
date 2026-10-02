using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000013 RID: 19
	[Tutorial("RansomingPrisonersStep2")]
	public class RansomingPrisonersStep2Tutorial : TutorialItemBase
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00002AA8 File Offset: 0x00000CA8
		public RansomingPrisonersStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "sell_all_prisoners";
			base.MouseRequired = true;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002AC9 File Offset: 0x00000CC9
		public override bool IsConditionsMetForCompletion()
		{
			return this._sellPrisonersOptionsSelected;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002AD1 File Offset: 0x00000CD1
		public override void OnGameMenuOptionSelected(GameMenuOption obj)
		{
			this._sellPrisonersOptionsSelected = obj.IdString == "sell_all_prisoners";
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002AE9 File Offset: 0x00000CE9
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002AEC File Offset: 0x00000CEC
		public override bool IsConditionsMetForActivation()
		{
			return !TutorialHelper.IsCharacterPopUpWindowOpen && TutorialHelper.CurrentContext == TutorialContexts.MapWindow && TutorialHelper.PlayerIsInNonEnemyTown && TutorialHelper.BackStreetMenuIsOpen && !Hero.MainHero.IsPrisoner && MobileParty.MainParty.PrisonRoster.TotalManCount > 0;
		}

		// Token: 0x04000016 RID: 22
		private bool _sellPrisonersOptionsSelected;
	}
}
