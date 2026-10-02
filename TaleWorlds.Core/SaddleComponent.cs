using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000C8 RID: 200
	public class SaddleComponent : ItemComponent
	{
		// Token: 0x06000AE4 RID: 2788 RVA: 0x00023224 File Offset: 0x00021424
		public SaddleComponent(SaddleComponent saddleComponent)
		{
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002322C File Offset: 0x0002142C
		public override ItemComponent GetCopy()
		{
			return new SaddleComponent(this);
		}
	}
}
