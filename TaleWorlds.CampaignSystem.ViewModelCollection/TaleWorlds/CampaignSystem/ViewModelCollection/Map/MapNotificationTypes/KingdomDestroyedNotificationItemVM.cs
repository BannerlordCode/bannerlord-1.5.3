using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200004A RID: 74
	public class KingdomDestroyedNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600060E RID: 1550 RVA: 0x0001FD38 File Offset: 0x0001DF38
		public KingdomDestroyedNotificationItemVM(KingdomDestroyedMapNotification data)
			: base(data)
		{
			KingdomDestroyedNotificationItemVM <>4__this = this;
			base.NotificationIdentifier = "kingdomdestroyed";
			this._onInspect = delegate
			{
				<>4__this.OnInspect(data);
			};
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001FD82 File Offset: 0x0001DF82
		private void OnInspect(KingdomDestroyedMapNotification data)
		{
			MBInformationManager.ShowSceneNotification(new KingdomDestroyedSceneNotificationItem(data.DestroyedKingdom, data.CreationTime));
			base.ExecuteRemove();
		}
	}
}
