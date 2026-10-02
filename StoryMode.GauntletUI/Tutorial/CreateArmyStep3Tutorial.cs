using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000020 RID: 32
	[Tutorial("CreateArmyStep3")]
	public class CreateArmyStep3Tutorial : TutorialItemBase
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00003263 File Offset: 0x00001463
		public CreateArmyStep3Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = string.Empty;
			base.MouseRequired = true;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003284 File Offset: 0x00001484
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerClosedArmyManagement;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000328C File Offset: 0x0000148C
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._playerClosedArmyManagement = obj.NewContext != TutorialContexts.ArmyManagement;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000032A1 File Offset: 0x000014A1
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.ArmyManagement;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000032A5 File Offset: 0x000014A5
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.CurrentContext == TutorialContexts.ArmyManagement && Campaign.Current.CurrentMenuContext == null && Clan.PlayerClan.Kingdom != null && MobileParty.MainParty.Army == null;
		}

		// Token: 0x04000028 RID: 40
		private bool _playerClosedArmyManagement;
	}
}
