using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200005B RID: 91
	public class TributeFinishedMapNotificationVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000675 RID: 1653 RVA: 0x0002118C File Offset: 0x0001F38C
		public TributeFinishedMapNotificationVM(TributeFinishedMapNotification data)
			: base(data)
		{
			TributeFinishedMapNotificationVM <>4__this = this;
			base.NotificationIdentifier = "ransom";
			this._onInspect = delegate
			{
				<>4__this.OnInspect(data.RelatedFaction);
			};
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x000211D6 File Offset: 0x0001F3D6
		private void OnInspect(IFaction relatedFaction)
		{
			INavigationHandler navigationHandler = base.NavigationHandler;
			if (navigationHandler != null)
			{
				navigationHandler.OpenKingdom(relatedFaction);
			}
			base.ExecuteRemove();
		}
	}
}
