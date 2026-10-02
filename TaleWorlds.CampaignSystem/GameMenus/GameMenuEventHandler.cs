using System;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000EC RID: 236
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class GameMenuEventHandler : Attribute
	{
		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001605 RID: 5637 RVA: 0x000650F1 File Offset: 0x000632F1
		// (set) Token: 0x06001606 RID: 5638 RVA: 0x000650F9 File Offset: 0x000632F9
		public string MenuId { get; private set; }

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x00065102 File Offset: 0x00063302
		// (set) Token: 0x06001608 RID: 5640 RVA: 0x0006510A File Offset: 0x0006330A
		public string MenuOptionId { get; private set; }

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001609 RID: 5641 RVA: 0x00065113 File Offset: 0x00063313
		// (set) Token: 0x0600160A RID: 5642 RVA: 0x0006511B File Offset: 0x0006331B
		public GameMenuEventHandler.EventType Type { get; private set; }

		// Token: 0x0600160B RID: 5643 RVA: 0x00065124 File Offset: 0x00063324
		public GameMenuEventHandler(string menuId, string menuOptionId, GameMenuEventHandler.EventType type)
		{
			this.MenuId = menuId;
			this.MenuOptionId = menuOptionId;
			this.Type = type;
		}

		// Token: 0x02000591 RID: 1425
		public enum EventType
		{
			// Token: 0x04001830 RID: 6192
			OnCondition,
			// Token: 0x04001831 RID: 6193
			OnConsequence
		}
	}
}
