using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed.Personal
{
	// Token: 0x0200005D RID: 93
	public class SPPersonalKillNotificationItemVM : ViewModel
	{
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x0001A4D5 File Offset: 0x000186D5
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x0001A4DD File Offset: 0x000186DD
		private SPPersonalKillNotificationItemVM.ItemTypes ItemTypeAsEnum
		{
			get
			{
				return this._itemTypeAsEnum;
			}
			set
			{
				this._itemType = (int)value;
				this._itemTypeAsEnum = value;
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001A4F0 File Offset: 0x000186F0
		public SPPersonalKillNotificationItemVM(int damageAmount, bool isMountDamage, bool isFriendlyFire, bool isHeadshot, string killedAgentName, bool isUnconscious, Action<SPPersonalKillNotificationItemVM> onRemoveItem)
		{
			this._onRemoveItem = onRemoveItem;
			this.Amount = damageAmount;
			if (isFriendlyFire)
			{
				this.ItemTypeAsEnum = SPPersonalKillNotificationItemVM.ItemTypes.FriendlyFireKill;
				this.Message = killedAgentName;
				return;
			}
			if (isMountDamage)
			{
				this.ItemTypeAsEnum = SPPersonalKillNotificationItemVM.ItemTypes.MountDamage;
				this.Message = GameTexts.FindText("str_damage_delivered_message", null).ToString();
				return;
			}
			this.ItemTypeAsEnum = (isUnconscious ? (isHeadshot ? SPPersonalKillNotificationItemVM.ItemTypes.MakeUnconsciousHeadshot : SPPersonalKillNotificationItemVM.ItemTypes.MakeUnconscious) : (isHeadshot ? SPPersonalKillNotificationItemVM.ItemTypes.NormalKillHeadshot : SPPersonalKillNotificationItemVM.ItemTypes.NormalKill));
			this.Message = killedAgentName;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001A56C File Offset: 0x0001876C
		public SPPersonalKillNotificationItemVM(int amount, bool isMountDamage, bool isFriendlyFire, string killedAgentName, Action<SPPersonalKillNotificationItemVM> onRemoveItem)
		{
			this._onRemoveItem = onRemoveItem;
			this.Amount = amount;
			if (isFriendlyFire)
			{
				this.ItemTypeAsEnum = SPPersonalKillNotificationItemVM.ItemTypes.FriendlyFireDamage;
				this.Message = killedAgentName;
				return;
			}
			if (isMountDamage)
			{
				this.ItemTypeAsEnum = SPPersonalKillNotificationItemVM.ItemTypes.MountDamage;
				this.Message = GameTexts.FindText("str_damage_delivered_message", null).ToString();
				return;
			}
			this.ItemTypeAsEnum = SPPersonalKillNotificationItemVM.ItemTypes.NormalDamage;
			this.Message = GameTexts.FindText("str_damage_delivered_message", null).ToString();
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001A5DF File Offset: 0x000187DF
		public SPPersonalKillNotificationItemVM(string victimAgentName, Action<SPPersonalKillNotificationItemVM> onRemoveItem)
		{
			this._onRemoveItem = onRemoveItem;
			this.Amount = -1;
			this.Message = victimAgentName;
			this.ItemTypeAsEnum = SPPersonalKillNotificationItemVM.ItemTypes.Message;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001A604 File Offset: 0x00018804
		public void ExecuteRemove()
		{
			this._onRemoveItem(this);
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0001A612 File Offset: 0x00018812
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x0001A61A File Offset: 0x0001881A
		[DataSourceProperty]
		public string VictimType
		{
			get
			{
				return this._victimType;
			}
			set
			{
				if (value != this._victimType)
				{
					this._victimType = value;
					base.OnPropertyChangedWithValue<string>(value, "VictimType");
				}
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x0001A63D File Offset: 0x0001883D
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x0001A645 File Offset: 0x00018845
		[DataSourceProperty]
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChangedWithValue<string>(value, "Message");
				}
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x0001A668 File Offset: 0x00018868
		// (set) Token: 0x06000773 RID: 1907 RVA: 0x0001A670 File Offset: 0x00018870
		[DataSourceProperty]
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChangedWithValue(value, "ItemType");
				}
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x0001A68E File Offset: 0x0001888E
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x0001A696 File Offset: 0x00018896
		[DataSourceProperty]
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				if (value != this._amount)
				{
					this._amount = value;
					base.OnPropertyChangedWithValue(value, "Amount");
				}
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x0001A6B4 File Offset: 0x000188B4
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x0001A6BC File Offset: 0x000188BC
		[DataSourceProperty]
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
			set
			{
				if (value != this._isPaused)
				{
					this._isPaused = value;
					base.OnPropertyChangedWithValue(value, "IsPaused");
				}
			}
		}

		// Token: 0x0400034B RID: 843
		private Action<SPPersonalKillNotificationItemVM> _onRemoveItem;

		// Token: 0x0400034C RID: 844
		private SPPersonalKillNotificationItemVM.ItemTypes _itemTypeAsEnum;

		// Token: 0x0400034D RID: 845
		private string _message;

		// Token: 0x0400034E RID: 846
		private string _victimType;

		// Token: 0x0400034F RID: 847
		private int _amount;

		// Token: 0x04000350 RID: 848
		private int _itemType;

		// Token: 0x04000351 RID: 849
		private bool _isPaused;

		// Token: 0x020000F0 RID: 240
		private enum ItemTypes
		{
			// Token: 0x04000683 RID: 1667
			NormalDamage,
			// Token: 0x04000684 RID: 1668
			FriendlyFireDamage,
			// Token: 0x04000685 RID: 1669
			FriendlyFireKill,
			// Token: 0x04000686 RID: 1670
			MountDamage,
			// Token: 0x04000687 RID: 1671
			NormalKill,
			// Token: 0x04000688 RID: 1672
			Assist,
			// Token: 0x04000689 RID: 1673
			MakeUnconscious,
			// Token: 0x0400068A RID: 1674
			NormalKillHeadshot,
			// Token: 0x0400068B RID: 1675
			MakeUnconsciousHeadshot,
			// Token: 0x0400068C RID: 1676
			Message
		}
	}
}
