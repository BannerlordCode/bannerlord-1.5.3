using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B4 RID: 436
	public abstract class KingdomDecisionPermissionModel : MBGameModel<KingdomDecisionPermissionModel>
	{
		// Token: 0x06001DCF RID: 7631
		public abstract bool IsPolicyDecisionAllowed(PolicyObject policy);

		// Token: 0x06001DD0 RID: 7632
		public abstract bool IsWarDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason);

		// Token: 0x06001DD1 RID: 7633
		public abstract bool IsPeaceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason);

		// Token: 0x06001DD2 RID: 7634
		public abstract bool IsStartAllianceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason);

		// Token: 0x06001DD3 RID: 7635
		public abstract bool IsAnnexationDecisionAllowed(Settlement annexedSettlement);

		// Token: 0x06001DD4 RID: 7636
		public abstract bool IsExpulsionDecisionAllowed(Clan expelledClan);

		// Token: 0x06001DD5 RID: 7637
		public abstract bool IsKingSelectionDecisionAllowed(Kingdom kingdom);
	}
}
