using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events
{
	// Token: 0x020000C7 RID: 199
	public class SettlementOverlayLeaveCharacterPermissionEvent : EventBase
	{
		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x0004E82B File Offset: 0x0004CA2B
		// (set) Token: 0x06001354 RID: 4948 RVA: 0x0004E833 File Offset: 0x0004CA33
		public Action<bool, TextObject> IsLeaveAvailable { get; private set; }

		// Token: 0x06001355 RID: 4949 RVA: 0x0004E83C File Offset: 0x0004CA3C
		public SettlementOverlayLeaveCharacterPermissionEvent(Action<bool, TextObject> isLeaveAvailable)
		{
			this.IsLeaveAvailable = isLeaveAvailable;
		}
	}
}
