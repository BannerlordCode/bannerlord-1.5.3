using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000430 RID: 1072
	public interface ITradeRumorCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06004402 RID: 17410
		IEnumerable<TradeRumor> TradeRumors { get; }
	}
}
