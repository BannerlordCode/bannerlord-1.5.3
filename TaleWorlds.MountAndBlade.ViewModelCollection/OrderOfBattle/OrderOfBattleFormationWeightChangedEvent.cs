using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000038 RID: 56
	public class OrderOfBattleFormationWeightChangedEvent : EventBase
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0001277C File Offset: 0x0001097C
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x00012784 File Offset: 0x00010984
		public Formation Formation { get; private set; }

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001278D File Offset: 0x0001098D
		public OrderOfBattleFormationWeightChangedEvent(Formation formation)
		{
			this.Formation = formation;
		}
	}
}
