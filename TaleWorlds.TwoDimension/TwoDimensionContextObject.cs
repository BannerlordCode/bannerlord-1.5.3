using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000037 RID: 55
	public class TwoDimensionContextObject
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000287 RID: 647 RVA: 0x000097CC File Offset: 0x000079CC
		// (set) Token: 0x06000288 RID: 648 RVA: 0x000097D4 File Offset: 0x000079D4
		public TwoDimensionContext Context { get; private set; }

		// Token: 0x06000289 RID: 649 RVA: 0x000097DD File Offset: 0x000079DD
		protected TwoDimensionContextObject(TwoDimensionContext context)
		{
			this.Context = context;
		}
	}
}
