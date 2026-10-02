using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events
{
	// Token: 0x020000C4 RID: 196
	public class PartyScreenCharacterTalkPermissionEvent : EventBase
	{
		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x0004E7B6 File Offset: 0x0004C9B6
		// (set) Token: 0x0600134B RID: 4939 RVA: 0x0004E7BE File Offset: 0x0004C9BE
		public Action<bool, TextObject> IsTalkAvailable { get; private set; }

		// Token: 0x0600134C RID: 4940 RVA: 0x0004E7C7 File Offset: 0x0004C9C7
		public PartyScreenCharacterTalkPermissionEvent(Hero heroToTalkTo, Action<bool, TextObject> isTalkAvailable)
		{
			this.HeroToTalkTo = heroToTalkTo;
			this.IsTalkAvailable = isTalkAvailable;
		}

		// Token: 0x040008BA RID: 2234
		public Hero HeroToTalkTo;
	}
}
