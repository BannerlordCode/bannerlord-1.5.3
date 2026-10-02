using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.Personal
{
	// Token: 0x0200008C RID: 140
	public class MPPersonalKillNotificationVM : ViewModel
	{
		// Token: 0x06000DA4 RID: 3492 RVA: 0x00029F4D File Offset: 0x0002814D
		public MPPersonalKillNotificationVM()
		{
			this.NotificationList = new MBBindingList<MPPersonalKillNotificationItemVM>();
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00029F60 File Offset: 0x00028160
		public void OnGoldChange(int changeAmount, GoldGainFlags goldGainType)
		{
			this.NotificationList.Add(new MPPersonalKillNotificationItemVM(changeAmount, goldGainType, new Action<MPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00029F80 File Offset: 0x00028180
		public void OnPersonalHit(int damageAmount, bool isFatal, bool isMountDamage, bool isFriendlyFire, bool isHeadshot, string killedAgentName)
		{
			this.NotificationList.Add(new MPPersonalKillNotificationItemVM(damageAmount, isFatal, isMountDamage, isFriendlyFire, isHeadshot, killedAgentName, new Action<MPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00029FB2 File Offset: 0x000281B2
		public void OnPersonalAssist(string killedAgentName)
		{
			this.NotificationList.Add(new MPPersonalKillNotificationItemVM(killedAgentName, new Action<MPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00029FD1 File Offset: 0x000281D1
		private void RemoveItem(MPPersonalKillNotificationItemVM item)
		{
			this.NotificationList.Remove(item);
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x00029FE0 File Offset: 0x000281E0
		// (set) Token: 0x06000DAA RID: 3498 RVA: 0x00029FE8 File Offset: 0x000281E8
		[DataSourceProperty]
		public MBBindingList<MPPersonalKillNotificationItemVM> NotificationList
		{
			get
			{
				return this._notificationList;
			}
			set
			{
				if (value != this._notificationList)
				{
					this._notificationList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPersonalKillNotificationItemVM>>(value, "NotificationList");
				}
			}
		}

		// Token: 0x04000637 RID: 1591
		private MBBindingList<MPPersonalKillNotificationItemVM> _notificationList;
	}
}
