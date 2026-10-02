using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200037B RID: 891
	public interface IOrderable
	{
		// Token: 0x06003320 RID: 13088
		OrderType GetOrder(BattleSideEnum side);
	}
}
