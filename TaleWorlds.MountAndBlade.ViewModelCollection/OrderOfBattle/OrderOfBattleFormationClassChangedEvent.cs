using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000037 RID: 55
	public class OrderOfBattleFormationClassChangedEvent : EventBase
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001275C File Offset: 0x0001095C
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x00012764 File Offset: 0x00010964
		public Formation Formation { get; private set; }

		// Token: 0x060004AD RID: 1197 RVA: 0x0001276D File Offset: 0x0001096D
		public OrderOfBattleFormationClassChangedEvent(Formation formation)
		{
			this.Formation = formation;
		}
	}
}
