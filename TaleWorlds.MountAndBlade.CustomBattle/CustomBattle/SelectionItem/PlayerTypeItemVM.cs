using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000024 RID: 36
	public class PlayerTypeItemVM : SelectorItemVM
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000A85F File Offset: 0x00008A5F
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000A867 File Offset: 0x00008A67
		public CustomBattlePlayerType PlayerType { get; private set; }

		// Token: 0x060001D3 RID: 467 RVA: 0x0000A870 File Offset: 0x00008A70
		public PlayerTypeItemVM(string playerTypeName, CustomBattlePlayerType playerType)
			: base(playerTypeName)
		{
			this.PlayerType = playerType;
		}
	}
}
