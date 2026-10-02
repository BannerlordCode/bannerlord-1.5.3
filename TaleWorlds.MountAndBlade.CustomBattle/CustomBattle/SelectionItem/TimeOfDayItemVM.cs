using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000027 RID: 39
	public class TimeOfDayItemVM : SelectorItemVM
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000A8C8 File Offset: 0x00008AC8
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000A8D0 File Offset: 0x00008AD0
		public int TimeOfDay { get; private set; }

		// Token: 0x060001DC RID: 476 RVA: 0x0000A8D9 File Offset: 0x00008AD9
		public TimeOfDayItemVM(string timeOfDayName, int timeOfDay)
			: base(timeOfDayName)
		{
			this.TimeOfDay = timeOfDay;
		}
	}
}
