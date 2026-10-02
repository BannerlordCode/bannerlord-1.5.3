using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x0200010C RID: 268
	public class CraftingWeaponClassSelectionOpenedEvent : EventBase
	{
		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060017D3 RID: 6099 RVA: 0x0005BA0C File Offset: 0x00059C0C
		// (set) Token: 0x060017D4 RID: 6100 RVA: 0x0005BA14 File Offset: 0x00059C14
		public bool IsOpen { get; private set; }

		// Token: 0x060017D5 RID: 6101 RVA: 0x0005BA1D File Offset: 0x00059C1D
		public CraftingWeaponClassSelectionOpenedEvent(bool isOpen)
		{
			this.IsOpen = isOpen;
		}
	}
}
