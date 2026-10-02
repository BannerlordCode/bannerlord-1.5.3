using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes;

namespace StoryMode.ViewModelCollection.Map
{
	// Token: 0x02000005 RID: 5
	public class ConspiracyQuestMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002901 File Offset: 0x00000B01
		public QuestBase Quest { get; }

		// Token: 0x0600003B RID: 59 RVA: 0x0000290C File Offset: 0x00000B0C
		public ConspiracyQuestMapNotificationItemVM(ConspiracyQuestMapNotification data)
			: base(data)
		{
			ConspiracyQuestMapNotificationItemVM <>4__this = this;
			base.NotificationIdentifier = "conspiracyquest";
			this.Quest = data.ConspiracyQuest;
			this._onInspect = delegate
			{
				INavigationHandler navigationHandler = <>4__this.NavigationHandler;
				if (navigationHandler == null)
				{
					return;
				}
				navigationHandler.OpenQuests(data.ConspiracyQuest);
			};
		}
	}
}
