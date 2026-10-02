using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000036 RID: 54
	public class OrderOfBattleHeroAssignedToFormationEvent : EventBase
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00012724 File Offset: 0x00010924
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x0001272C File Offset: 0x0001092C
		public Agent AssignedHero { get; private set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00012735 File Offset: 0x00010935
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x0001273D File Offset: 0x0001093D
		public Formation AssignedFormation { get; private set; }

		// Token: 0x060004AA RID: 1194 RVA: 0x00012746 File Offset: 0x00010946
		public OrderOfBattleHeroAssignedToFormationEvent(Agent assignedHero, Formation assignedFormation)
		{
			this.AssignedHero = assignedHero;
			this.AssignedFormation = assignedFormation;
		}
	}
}
