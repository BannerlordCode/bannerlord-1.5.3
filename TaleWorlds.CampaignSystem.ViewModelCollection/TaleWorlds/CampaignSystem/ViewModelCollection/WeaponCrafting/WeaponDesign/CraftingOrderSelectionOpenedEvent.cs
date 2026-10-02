using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x0200010E RID: 270
	public class CraftingOrderSelectionOpenedEvent : EventBase
	{
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060017D9 RID: 6105 RVA: 0x0005BA4C File Offset: 0x00059C4C
		// (set) Token: 0x060017DA RID: 6106 RVA: 0x0005BA54 File Offset: 0x00059C54
		public bool IsOpen { get; private set; }

		// Token: 0x060017DB RID: 6107 RVA: 0x0005BA5D File Offset: 0x00059C5D
		public CraftingOrderSelectionOpenedEvent(bool isOpen)
		{
			this.IsOpen = isOpen;
		}
	}
}
