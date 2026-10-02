using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x02000145 RID: 325
	public class ClanRoleAssignedThroughClanScreenEvent : EventBase
	{
		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x00071A11 File Offset: 0x0006FC11
		// (set) Token: 0x06001F58 RID: 8024 RVA: 0x00071A19 File Offset: 0x0006FC19
		public PartyRole Role { get; private set; }

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06001F59 RID: 8025 RVA: 0x00071A22 File Offset: 0x0006FC22
		// (set) Token: 0x06001F5A RID: 8026 RVA: 0x00071A2A File Offset: 0x0006FC2A
		public Hero HeroObject { get; private set; }

		// Token: 0x06001F5B RID: 8027 RVA: 0x00071A33 File Offset: 0x0006FC33
		public ClanRoleAssignedThroughClanScreenEvent(PartyRole role, Hero heroObject)
		{
			this.Role = role;
			this.HeroObject = heroObject;
		}
	}
}
