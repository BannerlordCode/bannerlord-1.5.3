using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.General
{
	// Token: 0x0200008E RID: 142
	public class MPGeneralKillNotificationVM : ViewModel
	{
		// Token: 0x06000DC8 RID: 3528 RVA: 0x0002A6C0 File Offset: 0x000288C0
		public MPGeneralKillNotificationVM()
		{
			this.NotificationList = new MBBindingList<MPGeneralKillNotificationItemVM>();
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0002A6D3 File Offset: 0x000288D3
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, Agent assistedAgent, WeaponClass killWeaponClass = WeaponClass.Undefined)
		{
			this.NotificationList.Add(new MPGeneralKillNotificationItemVM(affectedAgent, affectorAgent, assistedAgent, new Action<MPGeneralKillNotificationItemVM>(this.RemoveItem), killWeaponClass));
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0002A6F6 File Offset: 0x000288F6
		private void RemoveItem(MPGeneralKillNotificationItemVM item)
		{
			this.NotificationList.Remove(item);
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x0002A705 File Offset: 0x00028905
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x0002A70D File Offset: 0x0002890D
		[DataSourceProperty]
		public MBBindingList<MPGeneralKillNotificationItemVM> NotificationList
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
					base.OnPropertyChangedWithValue<MBBindingList<MPGeneralKillNotificationItemVM>>(value, "NotificationList");
				}
			}
		}

		// Token: 0x04000644 RID: 1604
		private MBBindingList<MPGeneralKillNotificationItemVM> _notificationList;
	}
}
