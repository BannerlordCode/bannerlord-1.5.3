using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x0200010D RID: 269
	public class CraftingOrderTabOpenedEvent : EventBase
	{
		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060017D6 RID: 6102 RVA: 0x0005BA2C File Offset: 0x00059C2C
		// (set) Token: 0x060017D7 RID: 6103 RVA: 0x0005BA34 File Offset: 0x00059C34
		public bool IsOpen { get; private set; }

		// Token: 0x060017D8 RID: 6104 RVA: 0x0005BA3D File Offset: 0x00059C3D
		public CraftingOrderTabOpenedEvent(bool isOpen)
		{
			this.IsOpen = isOpen;
		}
	}
}
