using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x0200002E RID: 46
	public class PlayerToggledUpgradePopupEvent : EventBase
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001BFF2 File Offset: 0x0001A1F2
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x0001BFFA File Offset: 0x0001A1FA
		public bool IsOpened { get; private set; }

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001C003 File Offset: 0x0001A203
		public PlayerToggledUpgradePopupEvent(bool isOpened)
		{
			this.IsOpened = isOpened;
		}
	}
}
