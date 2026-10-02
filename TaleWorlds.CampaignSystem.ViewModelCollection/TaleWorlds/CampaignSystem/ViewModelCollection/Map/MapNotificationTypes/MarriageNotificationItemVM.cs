using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200004D RID: 77
	public class MarriageNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x0002026C File Offset: 0x0001E46C
		// (set) Token: 0x06000637 RID: 1591 RVA: 0x00020274 File Offset: 0x0001E474
		public Hero Suitor { get; private set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0002027D File Offset: 0x0001E47D
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x00020285 File Offset: 0x0001E485
		public Hero Maiden { get; private set; }

		// Token: 0x0600063A RID: 1594 RVA: 0x00020290 File Offset: 0x0001E490
		public MarriageNotificationItemVM(MarriageMapNotification data)
			: base(data)
		{
			MarriageNotificationItemVM <>4__this = this;
			this.Suitor = data.Suitor;
			this.Maiden = data.Maiden;
			base.NotificationIdentifier = "marriage";
			this._onInspect = delegate
			{
				MBInformationManager.ShowSceneNotification(new MarriageSceneNotificationItem(data.Suitor, data.Maiden, data.CreationTime, SceneNotificationData.RelevantContextType.Any));
				<>4__this.ExecuteRemove();
			};
		}
	}
}
