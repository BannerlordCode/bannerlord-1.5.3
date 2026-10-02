using System;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x0200009D RID: 157
	public class InventoryItemInspectedEvent : EventBase
	{
		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x0003E201 File Offset: 0x0003C401
		// (set) Token: 0x06000ECB RID: 3787 RVA: 0x0003E209 File Offset: 0x0003C409
		public ItemRosterElement Item { get; private set; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x0003E212 File Offset: 0x0003C412
		// (set) Token: 0x06000ECD RID: 3789 RVA: 0x0003E21A File Offset: 0x0003C41A
		public InventoryLogic.InventorySide ItemSide { get; private set; }

		// Token: 0x06000ECE RID: 3790 RVA: 0x0003E223 File Offset: 0x0003C423
		public InventoryItemInspectedEvent(ItemRosterElement item, InventoryLogic.InventorySide itemSide)
		{
			this.ItemSide = itemSide;
			this.Item = item;
		}
	}
}
