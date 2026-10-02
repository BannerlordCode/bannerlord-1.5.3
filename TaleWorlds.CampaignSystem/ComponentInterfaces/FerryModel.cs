using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200020D RID: 525
	public abstract class FerryModel : MBGameModel<FerryModel>
	{
		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06002068 RID: 8296
		public abstract int MaximumFerryCapacityForPassengers { get; }

		// Token: 0x06002069 RID: 8297
		public abstract ExplainedNumber GetFerryCost(Settlement departureVillage, bool includeReason = false);
	}
}
