using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000025 RID: 37
	public class SceneLevelItemVM : SelectorItemVM
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000A880 File Offset: 0x00008A80
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x0000A888 File Offset: 0x00008A88
		public int Level { get; private set; }

		// Token: 0x060001D6 RID: 470 RVA: 0x0000A891 File Offset: 0x00008A91
		public SceneLevelItemVM(int level)
			: base(level.ToString())
		{
			this.Level = level;
		}
	}
}
