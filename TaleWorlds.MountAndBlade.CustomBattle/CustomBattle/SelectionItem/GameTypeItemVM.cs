using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000021 RID: 33
	public class GameTypeItemVM : SelectorItemVM
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000A70C File Offset: 0x0000890C
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x0000A714 File Offset: 0x00008914
		public string GameTypeStringId { get; private set; }

		// Token: 0x060001C3 RID: 451 RVA: 0x0000A71D File Offset: 0x0000891D
		public GameTypeItemVM(string gameTypeName, string gameType)
			: base(gameTypeName)
		{
			this.GameTypeStringId = gameType;
		}
	}
}
