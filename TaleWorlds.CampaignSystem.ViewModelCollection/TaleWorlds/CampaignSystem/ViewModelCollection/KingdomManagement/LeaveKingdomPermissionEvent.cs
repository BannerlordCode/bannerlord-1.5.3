using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement
{
	// Token: 0x0200006C RID: 108
	public class LeaveKingdomPermissionEvent : EventBase
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x000254F1 File Offset: 0x000236F1
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x000254F9 File Offset: 0x000236F9
		public Action<bool, TextObject> IsLeaveKingdomPossbile { get; private set; }

		// Token: 0x06000812 RID: 2066 RVA: 0x00025502 File Offset: 0x00023702
		public LeaveKingdomPermissionEvent(Action<bool, TextObject> isLeaveKingdomPossbile)
		{
			this.IsLeaveKingdomPossbile = isLeaveKingdomPossbile;
		}
	}
}
