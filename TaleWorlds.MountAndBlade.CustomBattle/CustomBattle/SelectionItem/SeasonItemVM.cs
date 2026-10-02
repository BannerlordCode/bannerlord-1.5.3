using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000026 RID: 38
	public class SeasonItemVM : SelectorItemVM
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000A8A7 File Offset: 0x00008AA7
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000A8AF File Offset: 0x00008AAF
		public string SeasonId { get; private set; }

		// Token: 0x060001D9 RID: 473 RVA: 0x0000A8B8 File Offset: 0x00008AB8
		public SeasonItemVM(string seasonName, string seasonId)
			: base(seasonName)
		{
			this.SeasonId = seasonId;
		}
	}
}
