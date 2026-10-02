using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200001B RID: 27
	public interface IDrawObject
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000120 RID: 288
		bool IsValid { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000121 RID: 289
		// (set) Token: 0x06000122 RID: 290
		Rectangle2D Rectangle { get; set; }
	}
}
