using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x0200022A RID: 554
	internal interface ILocatable<T>
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002177 RID: 8567
		// (set) Token: 0x06002178 RID: 8568
		[CachedData]
		int LocatorNodeIndex { get; set; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002179 RID: 8569
		// (set) Token: 0x0600217A RID: 8570
		[CachedData]
		T NextLocatable { get; set; }

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x0600217B RID: 8571
		[CachedData]
		Vec2 GetPosition2D { get; }
	}
}
