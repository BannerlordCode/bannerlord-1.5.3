using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200009C RID: 156
	public struct NavigationPermissionItem
	{
		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001333 RID: 4915 RVA: 0x00057DCC File Offset: 0x00055FCC
		// (set) Token: 0x06001334 RID: 4916 RVA: 0x00057DD4 File Offset: 0x00055FD4
		public bool IsAuthorized { get; private set; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x00057DDD File Offset: 0x00055FDD
		// (set) Token: 0x06001336 RID: 4918 RVA: 0x00057DE5 File Offset: 0x00055FE5
		public TextObject ReasonString { get; private set; }

		// Token: 0x06001337 RID: 4919 RVA: 0x00057DEE File Offset: 0x00055FEE
		public NavigationPermissionItem(bool isAuthorized, TextObject reasonString)
		{
			this.IsAuthorized = isAuthorized;
			this.ReasonString = reasonString;
		}
	}
}
