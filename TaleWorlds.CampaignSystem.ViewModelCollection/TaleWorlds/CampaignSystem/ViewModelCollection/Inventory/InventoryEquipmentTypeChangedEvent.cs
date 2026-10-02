using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x0200009B RID: 155
	public class InventoryEquipmentTypeChangedEvent : EventBase
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x0003E1C1 File Offset: 0x0003C3C1
		// (set) Token: 0x06000EC5 RID: 3781 RVA: 0x0003E1C9 File Offset: 0x0003C3C9
		public bool IsCurrentlyWarSet { get; private set; }

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0003E1D2 File Offset: 0x0003C3D2
		public InventoryEquipmentTypeChangedEvent(bool isCurrentlyWarSet)
		{
			this.IsCurrentlyWarSet = isCurrentlyWarSet;
		}
	}
}
