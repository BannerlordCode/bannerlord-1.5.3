using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000028 RID: 40
	public class WallHitpointItemVM : SelectorItemVM
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000A8E9 File Offset: 0x00008AE9
		// (set) Token: 0x060001DE RID: 478 RVA: 0x0000A8F1 File Offset: 0x00008AF1
		public string WallState { get; private set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000A8FA File Offset: 0x00008AFA
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x0000A902 File Offset: 0x00008B02
		public int BreachedWallCount { get; private set; }

		// Token: 0x060001E1 RID: 481 RVA: 0x0000A90B File Offset: 0x00008B0B
		public WallHitpointItemVM(string wallStateName, int breachedWallCount)
			: base(wallStateName)
		{
			this.WallState = wallStateName;
			this.BreachedWallCount = breachedWallCount;
		}
	}
}
