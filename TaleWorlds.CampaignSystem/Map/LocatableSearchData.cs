using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x0200022B RID: 555
	public struct LocatableSearchData<T>
	{
		// Token: 0x0600217C RID: 8572 RVA: 0x00094978 File Offset: 0x00092B78
		public LocatableSearchData(Vec2 position, float radius, int minX, int minY, int maxX, int maxY)
		{
			this.Position = position;
			this.RadiusSquared = radius * radius;
			this.MinY = minY;
			this.MaxXInclusive = maxX;
			this.MaxYInclusive = maxY;
			this.CurrentX = minX;
			this.CurrentY = minY - 1;
			this.CurrentLocatable = null;
		}

		// Token: 0x040009BE RID: 2494
		public readonly Vec2 Position;

		// Token: 0x040009BF RID: 2495
		public readonly float RadiusSquared;

		// Token: 0x040009C0 RID: 2496
		public readonly int MinY;

		// Token: 0x040009C1 RID: 2497
		public readonly int MaxXInclusive;

		// Token: 0x040009C2 RID: 2498
		public readonly int MaxYInclusive;

		// Token: 0x040009C3 RID: 2499
		public int CurrentX;

		// Token: 0x040009C4 RID: 2500
		public int CurrentY;

		// Token: 0x040009C5 RID: 2501
		internal ILocatable<T> CurrentLocatable;
	}
}
