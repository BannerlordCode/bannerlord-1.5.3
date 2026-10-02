using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed.General
{
	// Token: 0x02000060 RID: 96
	public class SPGeneralKillNotificationVM : ViewModel
	{
		// Token: 0x06000797 RID: 1943 RVA: 0x0001AB69 File Offset: 0x00018D69
		public SPGeneralKillNotificationVM()
		{
			this.NotificationList = new MBBindingList<SPGeneralKillNotificationItemVM>();
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001AB7C File Offset: 0x00018D7C
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, bool isHeadshot, bool isSuicide, bool isDrowning)
		{
			this.NotificationList.Add(new SPGeneralKillNotificationItemVM(affectedAgent, affectorAgent, isHeadshot, isSuicide, isDrowning, new Action<SPGeneralKillNotificationItemVM>(this.RemoveItem)));
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001ABA1 File Offset: 0x00018DA1
		private void RemoveItem(SPGeneralKillNotificationItemVM item)
		{
			this.NotificationList.Remove(item);
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0001ABB0 File Offset: 0x00018DB0
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x0001ABB8 File Offset: 0x00018DB8
		[DataSourceProperty]
		public MBBindingList<SPGeneralKillNotificationItemVM> NotificationList
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
					base.OnPropertyChangedWithValue<MBBindingList<SPGeneralKillNotificationItemVM>>(value, "NotificationList");
				}
			}
		}

		// Token: 0x04000363 RID: 867
		private MBBindingList<SPGeneralKillNotificationItemVM> _notificationList;
	}
}
