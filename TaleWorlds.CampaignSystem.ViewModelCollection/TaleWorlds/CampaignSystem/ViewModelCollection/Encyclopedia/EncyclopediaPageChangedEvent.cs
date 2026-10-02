using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000CA RID: 202
	public class EncyclopediaPageChangedEvent : EventBase
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x0004E853 File Offset: 0x0004CA53
		// (set) Token: 0x06001358 RID: 4952 RVA: 0x0004E85B File Offset: 0x0004CA5B
		public EncyclopediaPages NewPage { get; private set; }

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001359 RID: 4953 RVA: 0x0004E864 File Offset: 0x0004CA64
		// (set) Token: 0x0600135A RID: 4954 RVA: 0x0004E86C File Offset: 0x0004CA6C
		public bool NewPageHasHiddenInformation { get; private set; }

		// Token: 0x0600135B RID: 4955 RVA: 0x0004E875 File Offset: 0x0004CA75
		public EncyclopediaPageChangedEvent(EncyclopediaPages newPage, bool hasHiddenInformation = false)
		{
			this.NewPage = newPage;
			this.NewPageHasHiddenInformation = hasHiddenInformation;
		}
	}
}
