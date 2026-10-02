using System;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000055 RID: 85
	public class QuestNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000661 RID: 1633 RVA: 0x00020DD0 File Offset: 0x0001EFD0
		public QuestNotificationItemVM(QuestBase quest, InformationData data, Action<QuestBase> onQuestNotificationInspect, Action<MapNotificationItemBaseVM> onRemove)
			: base(data)
		{
			this._quest = quest;
			this._onQuestNotificationInspect = onQuestNotificationInspect;
			this._onInspect = (this._onInspectAction = delegate
			{
				this._onQuestNotificationInspect(this._quest);
			});
			base.NotificationIdentifier = "quest";
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00020E18 File Offset: 0x0001F018
		public QuestNotificationItemVM(IssueBase issue, InformationData data, Action<IssueBase> onIssueNotificationInspect, Action<MapNotificationItemBaseVM> onRemove)
			: base(data)
		{
			this._issue = issue;
			this._onIssueNotificationInspect = onIssueNotificationInspect;
			this._onInspect = (this._onInspectAction = delegate
			{
				this._onIssueNotificationInspect(this._issue);
			});
			base.NotificationIdentifier = "quest";
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00020E60 File Offset: 0x0001F060
		public override void ManualRefreshRelevantStatus()
		{
			base.ManualRefreshRelevantStatus();
		}

		// Token: 0x040002AE RID: 686
		private QuestBase _quest;

		// Token: 0x040002AF RID: 687
		private IssueBase _issue;

		// Token: 0x040002B0 RID: 688
		private Action<QuestBase> _onQuestNotificationInspect;

		// Token: 0x040002B1 RID: 689
		private Action<IssueBase> _onIssueNotificationInspect;

		// Token: 0x040002B2 RID: 690
		protected Action _onInspectAction;
	}
}
