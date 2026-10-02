using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019F RID: 415
	public abstract class PartyTradeModel : MBGameModel<PartyTradeModel>
	{
		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06001D07 RID: 7431
		public abstract int CaravanTransactionHighestValueItemCount { get; }

		// Token: 0x06001D08 RID: 7432
		public abstract float GetTradePenaltyFactor(MobileParty party);
	}
}
