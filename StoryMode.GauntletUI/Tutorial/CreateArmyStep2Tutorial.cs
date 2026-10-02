using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200001F RID: 31
	[Tutorial("CreateArmyStep2")]
	public class CreateArmyStep2Tutorial : TutorialItemBase
	{
		// Token: 0x06000096 RID: 150 RVA: 0x000031FB File Offset: 0x000013FB
		public CreateArmyStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = "GatherArmyPartiesPanel";
			base.MouseRequired = true;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000321C File Offset: 0x0000141C
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerAddedPartyToArmy;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003224 File Offset: 0x00001424
		public override void OnPartyAddedToArmyByPlayer(PartyAddedToArmyByPlayerEvent obj)
		{
			this._playerAddedPartyToArmy = true;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000322D File Offset: 0x0000142D
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.ArmyManagement;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003231 File Offset: 0x00001431
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.CurrentContext == TutorialContexts.ArmyManagement && Campaign.Current.CurrentMenuContext == null && Clan.PlayerClan.Kingdom != null && MobileParty.MainParty.Army == null;
		}

		// Token: 0x04000027 RID: 39
		private bool _playerAddedPartyToArmy;
	}
}
