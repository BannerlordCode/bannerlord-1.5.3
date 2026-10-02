using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed.Personal
{
	// Token: 0x0200005E RID: 94
	public class SPPersonalKillNotificationVM : ViewModel
	{
		// Token: 0x06000778 RID: 1912 RVA: 0x0001A6DA File Offset: 0x000188DA
		public SPPersonalKillNotificationVM()
		{
			this.NotificationList = new MBBindingList<SPPersonalKillNotificationItemVM>();
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001A6F0 File Offset: 0x000188F0
		public void OnPersonalKill(int damageAmount, bool isMountDamage, bool isFriendlyFire, bool isHeadshot, string killedAgentName, bool isUnconscious)
		{
			this.NotificationList.Add(new SPPersonalKillNotificationItemVM(damageAmount, isMountDamage, isFriendlyFire, isHeadshot, killedAgentName, isUnconscious, new Action<SPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001A722 File Offset: 0x00018922
		public void OnPersonalHit(int damageAmount, bool isMountDamage, bool isFriendlyFire, string killedAgentName)
		{
			this.NotificationList.Add(new SPPersonalKillNotificationItemVM(damageAmount, isMountDamage, isFriendlyFire, killedAgentName, new Action<SPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001A745 File Offset: 0x00018945
		public void OnPersonalMessage(string message)
		{
			this.NotificationList.Add(new SPPersonalKillNotificationItemVM(message, new Action<SPPersonalKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001A764 File Offset: 0x00018964
		private void RemoveItem(SPPersonalKillNotificationItemVM item)
		{
			this.NotificationList.Remove(item);
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0001A773 File Offset: 0x00018973
		// (set) Token: 0x0600077E RID: 1918 RVA: 0x0001A77B File Offset: 0x0001897B
		[DataSourceProperty]
		public MBBindingList<SPPersonalKillNotificationItemVM> NotificationList
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
					base.OnPropertyChangedWithValue<MBBindingList<SPPersonalKillNotificationItemVM>>(value, "NotificationList");
				}
			}
		}

		// Token: 0x04000352 RID: 850
		private MBBindingList<SPPersonalKillNotificationItemVM> _notificationList;
	}
}
