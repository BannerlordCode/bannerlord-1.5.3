using System;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000ED RID: 237
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class GameMenuInitializationHandler : Attribute
	{
		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x00065141 File Offset: 0x00063341
		// (set) Token: 0x0600160D RID: 5645 RVA: 0x00065149 File Offset: 0x00063349
		public string MenuId { get; private set; }

		// Token: 0x0600160E RID: 5646 RVA: 0x00065152 File Offset: 0x00063352
		public GameMenuInitializationHandler(string menuId)
		{
			this.MenuId = menuId;
		}
	}
}
