using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x0200009C RID: 156
	public class InventoryFilterChangedEvent : EventBase
	{
		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0003E1E1 File Offset: 0x0003C3E1
		// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x0003E1E9 File Offset: 0x0003C3E9
		public SPInventoryVM.Filters NewFilter { get; private set; }

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0003E1F2 File Offset: 0x0003C3F2
		public InventoryFilterChangedEvent(SPInventoryVM.Filters newFilter)
		{
			this.NewFilter = newFilter;
		}
	}
}
