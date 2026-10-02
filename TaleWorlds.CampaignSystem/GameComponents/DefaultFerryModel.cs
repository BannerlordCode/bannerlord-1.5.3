using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011D RID: 285
	public class DefaultFerryModel : FerryModel
	{
		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001892 RID: 6290 RVA: 0x00076C38 File Offset: 0x00074E38
		public override int MaximumFerryCapacityForPassengers
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x00076C3B File Offset: 0x00074E3B
		public override ExplainedNumber GetFerryCost(Settlement departureVillage, bool includeReason = false)
		{
			return new ExplainedNumber(0f, includeReason, null);
		}
	}
}
