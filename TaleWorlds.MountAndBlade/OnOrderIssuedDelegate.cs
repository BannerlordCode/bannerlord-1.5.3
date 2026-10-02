using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000154 RID: 340
	// (Invoke) Token: 0x06001191 RID: 4497
	public delegate void OnOrderIssuedDelegate(OrderType orderType, MBReadOnlyList<Formation> appliedFormations, OrderController orderController, params object[] delegateParams);
}
