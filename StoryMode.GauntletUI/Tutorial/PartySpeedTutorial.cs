using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200001B RID: 27
	[Tutorial("PartySpeed")]
	public class PartySpeedTutorial : TutorialItemBase
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00002F3E File Offset: 0x0000113E
		public PartySpeedTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "PartySpeedLabel";
			base.MouseRequired = true;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002F5F File Offset: 0x0000115F
		public override bool IsConditionsMetForCompletion()
		{
			return this._isPlayerInspectedPartySpeed;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002F67 File Offset: 0x00001167
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002F6A File Offset: 0x0000116A
		public override void OnPlayerInspectedPartySpeed(PlayerInspectedPartySpeedEvent obj)
		{
			if (this._isActivated)
			{
				this._isPlayerInspectedPartySpeed = true;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002F7C File Offset: 0x0000117C
		public override bool IsConditionsMetForActivation()
		{
			this._isActivated = TutorialHelper.CurrentContext == TutorialContexts.MapWindow && Campaign.Current.CurrentMenuContext == null && MobileParty.MainParty.PartyMoveMode != MoveModeType.Hold && MobileParty.MainParty.IsActive && MobileParty.MainParty.Speed < TutorialHelper.MaximumSpeedForPartyForSpeedTutorial && (float)MobileParty.MainParty.InventoryCapacity < MobileParty.MainParty.TotalWeightCarried;
			return this._isActivated;
		}

		// Token: 0x04000021 RID: 33
		private bool _isPlayerInspectedPartySpeed;

		// Token: 0x04000022 RID: 34
		private bool _isActivated;
	}
}
