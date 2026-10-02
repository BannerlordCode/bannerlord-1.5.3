using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000044 RID: 68
	public class BloodFeudClanMemberReleasedMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005FE RID: 1534 RVA: 0x0001F98A File Offset: 0x0001DB8A
		public BloodFeudClanMemberReleasedMapNotificationItemVM(BloodFeudClanMemberExecuteCancelledMapNotification data)
			: base(data)
		{
			base.NotificationIdentifier = "blood_feud_clan_member_released";
			this._onInspect = new Action(this.OnInspect);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001F9B0 File Offset: 0x0001DBB0
		private void OnInspect()
		{
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001F9B2 File Offset: 0x0001DBB2
		public override void OnFinalize()
		{
			base.OnFinalize();
		}
	}
}
