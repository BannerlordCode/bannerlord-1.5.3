using System;

namespace TaleWorlds.CampaignSystem.Map.DistanceCache
{
	// Token: 0x02000230 RID: 560
	public interface ISettlementDataHolder
	{
		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060021B8 RID: 8632
		CampaignVec2 GatePosition { get; }

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060021B9 RID: 8633
		CampaignVec2 PortPosition { get; }

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x060021BA RID: 8634
		string StringId { get; }

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x060021BB RID: 8635
		bool IsFortification { get; }

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x060021BC RID: 8636
		bool HasPort { get; }
	}
}
