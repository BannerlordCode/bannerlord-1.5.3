using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions
{
	// Token: 0x0200007C RID: 124
	public class PlayerSelectedAKingdomDecisionOptionEvent : EventBase
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x0002B4AC File Offset: 0x000296AC
		// (set) Token: 0x060009DF RID: 2527 RVA: 0x0002B4B4 File Offset: 0x000296B4
		public DecisionOutcome Option { get; private set; }

		// Token: 0x060009E0 RID: 2528 RVA: 0x0002B4BD File Offset: 0x000296BD
		public PlayerSelectedAKingdomDecisionOptionEvent(DecisionOutcome option)
		{
			this.Option = option;
		}
	}
}
