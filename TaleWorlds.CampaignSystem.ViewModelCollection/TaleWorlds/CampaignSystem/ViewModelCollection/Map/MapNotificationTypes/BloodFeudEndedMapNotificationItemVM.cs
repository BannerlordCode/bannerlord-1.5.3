using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000045 RID: 69
	public class BloodFeudEndedMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000601 RID: 1537 RVA: 0x0001F9BA File Offset: 0x0001DBBA
		public BloodFeudEndedMapNotificationItemVM(BloodFeudEndedMapNotification data)
			: base(data)
		{
			base.NotificationIdentifier = "blood_feud_ended";
			this._onInspect = new Action(this.OnInspect);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001F9E0 File Offset: 0x0001DBE0
		private void OnInspect()
		{
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001F9E2 File Offset: 0x0001DBE2
		public override void OnFinalize()
		{
			base.OnFinalize();
		}
	}
}
