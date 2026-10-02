using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000111 RID: 273
	public class CraftingWeaponResultPopupToggledEvent : EventBase
	{
		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x0600181F RID: 6175 RVA: 0x0005C41D File Offset: 0x0005A61D
		public bool IsOpen { get; }

		// Token: 0x06001820 RID: 6176 RVA: 0x0005C425 File Offset: 0x0005A625
		public CraftingWeaponResultPopupToggledEvent(bool isOpen)
		{
			this.IsOpen = isOpen;
		}
	}
}
