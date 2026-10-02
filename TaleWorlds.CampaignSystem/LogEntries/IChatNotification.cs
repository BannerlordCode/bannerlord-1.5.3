using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x02000369 RID: 873
	public interface IChatNotification
	{
		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06003404 RID: 13316
		bool IsVisibleNotification { get; }

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06003405 RID: 13317
		ChatNotificationType NotificationType { get; }

		// Token: 0x06003406 RID: 13318
		TextObject GetNotificationText();
	}
}
