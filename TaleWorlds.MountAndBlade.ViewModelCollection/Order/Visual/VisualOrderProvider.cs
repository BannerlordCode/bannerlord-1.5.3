using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x0200002C RID: 44
	public abstract class VisualOrderProvider
	{
		// Token: 0x06000348 RID: 840
		public abstract bool IsAvailable();

		// Token: 0x06000349 RID: 841
		public abstract MBReadOnlyList<VisualOrderSet> GetOrders();
	}
}
