using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000049 RID: 73
	public class HeirComeOfAgeNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600060C RID: 1548 RVA: 0x0001FC94 File Offset: 0x0001DE94
		public HeirComeOfAgeNotificationItemVM(HeirComeOfAgeMapNotification data)
			: base(data)
		{
			HeirComeOfAgeNotificationItemVM <>4__this = this;
			base.NotificationIdentifier = "comeofage";
			this._onInspect = delegate
			{
				<>4__this.OnInspect(data);
			};
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001FCE0 File Offset: 0x0001DEE0
		private void OnInspect(HeirComeOfAgeMapNotification data)
		{
			SceneNotificationData sceneNotificationData;
			if (data.ComeOfAgeHero.IsFemale)
			{
				sceneNotificationData = new HeirComingOfAgeFemaleSceneNotificationItem(data.MentorHero, data.ComeOfAgeHero, data.CreationTime);
			}
			else
			{
				sceneNotificationData = new HeirComingOfAgeSceneNotificationItem(data.MentorHero, data.ComeOfAgeHero, data.CreationTime);
			}
			MBInformationManager.ShowSceneNotification(sceneNotificationData);
			base.ExecuteRemove();
		}
	}
}
