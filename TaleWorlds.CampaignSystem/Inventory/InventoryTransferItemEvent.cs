using System;
using TaleWorlds.Core;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000E1 RID: 225
	public class InventoryTransferItemEvent : EventBase
	{
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001592 RID: 5522 RVA: 0x00064429 File Offset: 0x00062629
		// (set) Token: 0x06001593 RID: 5523 RVA: 0x00064431 File Offset: 0x00062631
		public ItemObject Item { get; private set; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001594 RID: 5524 RVA: 0x0006443A File Offset: 0x0006263A
		// (set) Token: 0x06001595 RID: 5525 RVA: 0x00064442 File Offset: 0x00062642
		public bool IsBuyForPlayer { get; private set; }

		// Token: 0x06001596 RID: 5526 RVA: 0x0006444B File Offset: 0x0006264B
		public InventoryTransferItemEvent(ItemObject item, bool isBuyForPlayer)
		{
			this.Item = item;
			this.IsBuyForPlayer = isBuyForPlayer;
		}
	}
}
