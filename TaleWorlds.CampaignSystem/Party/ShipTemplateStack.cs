using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x02000318 RID: 792
	public struct ShipTemplateStack
	{
		// Token: 0x06002E3E RID: 11838 RVA: 0x000C1E59 File Offset: 0x000C0059
		public ShipTemplateStack(ShipHull shipHull, int minValue, int maxValue)
		{
			this.ShipHull = shipHull;
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}

		// Token: 0x04000D75 RID: 3445
		public ShipHull ShipHull;

		// Token: 0x04000D76 RID: 3446
		public int MinValue;

		// Token: 0x04000D77 RID: 3447
		public int MaxValue;
	}
}
