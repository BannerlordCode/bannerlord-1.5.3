using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000046 RID: 70
	public class BloodFeudStartedMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x0001F9EA File Offset: 0x0001DBEA
		public BloodFeudStartedMapNotificationItemVM(BloodFeudStartedMapNotification data)
			: base(data)
		{
			base.NotificationIdentifier = "blood_feud_started";
			this._onInspect = new Action(this.OnInspect);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001FA10 File Offset: 0x0001DC10
		private void OnInspect()
		{
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001FA12 File Offset: 0x0001DC12
		public override void OnFinalize()
		{
			base.OnFinalize();
		}
	}
}
