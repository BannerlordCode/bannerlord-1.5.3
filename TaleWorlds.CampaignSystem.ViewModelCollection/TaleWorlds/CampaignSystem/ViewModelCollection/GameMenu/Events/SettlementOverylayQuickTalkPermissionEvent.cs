using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events
{
	// Token: 0x020000C6 RID: 198
	public class SettlementOverylayQuickTalkPermissionEvent : EventBase
	{
		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001350 RID: 4944 RVA: 0x0004E804 File Offset: 0x0004CA04
		// (set) Token: 0x06001351 RID: 4945 RVA: 0x0004E80C File Offset: 0x0004CA0C
		public Action<bool, TextObject> IsTalkAvailable { get; private set; }

		// Token: 0x06001352 RID: 4946 RVA: 0x0004E815 File Offset: 0x0004CA15
		public SettlementOverylayQuickTalkPermissionEvent(Hero heroToTalkTo, Action<bool, TextObject> isTalkAvailable)
		{
			this.HeroToTalkTo = heroToTalkTo;
			this.IsTalkAvailable = isTalkAvailable;
		}

		// Token: 0x040008BE RID: 2238
		public Hero HeroToTalkTo;
	}
}
