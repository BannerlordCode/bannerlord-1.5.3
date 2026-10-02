using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events
{
	// Token: 0x020000C5 RID: 197
	public class SettlementOverlayTalkPermissionEvent : EventBase
	{
		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x0004E7DD File Offset: 0x0004C9DD
		// (set) Token: 0x0600134E RID: 4942 RVA: 0x0004E7E5 File Offset: 0x0004C9E5
		public Action<bool, TextObject> IsTalkAvailable { get; private set; }

		// Token: 0x0600134F RID: 4943 RVA: 0x0004E7EE File Offset: 0x0004C9EE
		public SettlementOverlayTalkPermissionEvent(Hero heroToTalkTo, Action<bool, TextObject> isTalkAvailable)
		{
			this.HeroToTalkTo = heroToTalkTo;
			this.IsTalkAvailable = isTalkAvailable;
		}

		// Token: 0x040008BC RID: 2236
		public Hero HeroToTalkTo;
	}
}
