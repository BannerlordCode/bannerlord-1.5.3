using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map
{
	// Token: 0x02000036 RID: 54
	public class MapNotificationVM : ViewModel
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600056A RID: 1386 RVA: 0x0001D9C8 File Offset: 0x0001BBC8
		// (remove) Token: 0x0600056B RID: 1387 RVA: 0x0001DA00 File Offset: 0x0001BC00
		public event Action<MapNotificationItemBaseVM> ReceiveNewNotification;

		// Token: 0x0600056C RID: 1388 RVA: 0x0001DA38 File Offset: 0x0001BC38
		public MapNotificationVM(INavigationHandler navigationHandler, Action<CampaignVec2> fastMoveCameraToPosition)
		{
			this._navigationHandler = navigationHandler;
			this._fastMoveCameraToPosition = fastMoveCameraToPosition;
			MBInformationManager.OnAddMapNotice += this.AddMapNotification;
			this.NotificationItems = new MBBindingList<MapNotificationItemBaseVM>();
			this.PopulateTypeDictionary();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001DA86 File Offset: 0x0001BC86
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NotificationItems.ApplyActionOnAllItems(delegate(MapNotificationItemBaseVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001DAB8 File Offset: 0x0001BCB8
		private void PopulateTypeDictionary()
		{
			this._itemConstructors.Add(typeof(PeaceMapNotification), typeof(PeaceNotificationItemVM));
			this._itemConstructors.Add(typeof(SettlementRebellionMapNotification), typeof(RebellionNotificationItemVM));
			this._itemConstructors.Add(typeof(WarMapNotification), typeof(WarNotificationItemVM));
			this._itemConstructors.Add(typeof(ArmyDispersionMapNotification), typeof(ArmyDispersionItemVM));
			this._itemConstructors.Add(typeof(ChildBornMapNotification), typeof(NewBornNotificationItemVM));
			this._itemConstructors.Add(typeof(DeathMapNotification), typeof(DeathNotificationItemVM));
			this._itemConstructors.Add(typeof(MarriageMapNotification), typeof(MarriageNotificationItemVM));
			this._itemConstructors.Add(typeof(MarriageOfferMapNotification), typeof(MarriageOfferNotificationItemVM));
			this._itemConstructors.Add(typeof(MercenaryOfferMapNotification), typeof(MercenaryOfferMapNotificationItemVM));
			this._itemConstructors.Add(typeof(VassalOfferMapNotification), typeof(VassalOfferMapNotificationItemVM));
			this._itemConstructors.Add(typeof(ArmyCreationMapNotification), typeof(ArmyCreationNotificationItemVM));
			this._itemConstructors.Add(typeof(KingdomDecisionMapNotification), typeof(KingdomVoteNotificationItemVM));
			this._itemConstructors.Add(typeof(SettlementOwnerChangedMapNotification), typeof(SettlementOwnerChangedNotificationItemVM));
			this._itemConstructors.Add(typeof(SettlementUnderSiegeMapNotification), typeof(SettlementUnderSiegeMapNotificationItemVM));
			this._itemConstructors.Add(typeof(AlleyLeaderDiedMapNotification), typeof(AlleyLeaderDiedMapNotificationItemVM));
			this._itemConstructors.Add(typeof(AlleyUnderAttackMapNotification), typeof(AlleyUnderAttackMapNotificationItemVM));
			this._itemConstructors.Add(typeof(EducationMapNotification), typeof(EducationNotificationItemVM));
			this._itemConstructors.Add(typeof(TraitChangedMapNotification), typeof(TraitChangedNotificationItemVM));
			this._itemConstructors.Add(typeof(RansomOfferMapNotification), typeof(RansomNotificationItemVM));
			this._itemConstructors.Add(typeof(PeaceOfferMapNotification), typeof(PeaceOfferNotificationItemVM));
			this._itemConstructors.Add(typeof(PartyLeaderChangeNotification), typeof(PartyLeaderChangeNotificationVM));
			this._itemConstructors.Add(typeof(HeirComeOfAgeMapNotification), typeof(HeirComeOfAgeNotificationItemVM));
			this._itemConstructors.Add(typeof(KingdomDestroyedMapNotification), typeof(KingdomDestroyedNotificationItemVM));
			this._itemConstructors.Add(typeof(AllianceOfferMapNotification), typeof(AllianceOfferNotificationItemVM));
			this._itemConstructors.Add(typeof(AcceptCallToWarOfferMapNotification), typeof(AcceptCallToWarOfferNotificationItemVM));
			this._itemConstructors.Add(typeof(ProposeCallToWarOfferMapNotification), typeof(ProposeCallToWarOfferNotificationItemVM));
			this._itemConstructors.Add(typeof(TributeFinishedMapNotification), typeof(TributeFinishedMapNotificationVM));
			this._itemConstructors.Add(typeof(BloodFeudEndedMapNotification), typeof(BloodFeudEndedMapNotificationItemVM));
			this._itemConstructors.Add(typeof(BloodFeudClanMemberCapturedMapNotification), typeof(BloodFeudClanMemberCapturedMapNotificationItemVM));
			this._itemConstructors.Add(typeof(BloodFeudClanMemberGotExecutedMapNotification), typeof(BloodFeudClanMemberGotExecutedMapNotificationItemVM));
			this._itemConstructors.Add(typeof(BloodFeudClanMemberExecuteCancelledMapNotification), typeof(BloodFeudClanMemberReleasedMapNotificationItemVM));
			this._itemConstructors.Add(typeof(BloodFeudClanMemberExecutedLordMapNotification), typeof(BloodFeudClanMemberExecutedLordMapNotificationItemVM));
			this._itemConstructors.Add(typeof(BloodFeudStartedMapNotification), typeof(BloodFeudStartedMapNotificationItemVM));
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001DEC4 File Offset: 0x0001C0C4
		public void RegisterMapNotificationType(Type data, Type item)
		{
			this._itemConstructors[data] = item;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001DED3 File Offset: 0x0001C0D3
		public override void OnFinalize()
		{
			MBInformationManager.OnAddMapNotice -= this.AddMapNotification;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001DEE8 File Offset: 0x0001C0E8
		public void OnFrameTick(float dt)
		{
			for (int i = 0; i < this.NotificationItems.Count; i++)
			{
				this.NotificationItems[i].ManualRefreshRelevantStatus();
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001DF1C File Offset: 0x0001C11C
		public void OnMenuModeTick(float dt)
		{
			for (int i = 0; i < this.NotificationItems.Count; i++)
			{
				this.NotificationItems[i].ManualRefreshRelevantStatus();
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001DF50 File Offset: 0x0001C150
		private void RemoveNotificationItem(MapNotificationItemBaseVM item)
		{
			item.OnFinalize();
			this.NotificationItems.Remove(item);
			MBInformationManager.MapNoticeRemoved(item.Data);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001DF70 File Offset: 0x0001C170
		private void OnNotificationItemFocus(MapNotificationItemBaseVM item)
		{
			this.FocusedNotificationItem = item;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001DF7C File Offset: 0x0001C17C
		public void AddMapNotification(InformationData data)
		{
			MapNotificationItemBaseVM notificationFromData = this.GetNotificationFromData(data);
			if (notificationFromData != null)
			{
				this.NotificationItems.Add(notificationFromData);
				Action<MapNotificationItemBaseVM> receiveNewNotification = this.ReceiveNewNotification;
				if (receiveNewNotification == null)
				{
					return;
				}
				receiveNewNotification(notificationFromData);
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001DFB4 File Offset: 0x0001C1B4
		public void RemoveAllNotifications()
		{
			foreach (MapNotificationItemBaseVM mapNotificationItemBaseVM in this.NotificationItems.ToList<MapNotificationItemBaseVM>())
			{
				this.RemoveNotificationItem(mapNotificationItemBaseVM);
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001E00C File Offset: 0x0001C20C
		private MapNotificationItemBaseVM GetNotificationFromData(InformationData data)
		{
			Type type = data.GetType();
			MapNotificationItemBaseVM mapNotificationItemBaseVM = null;
			if (this._itemConstructors.ContainsKey(type))
			{
				mapNotificationItemBaseVM = (MapNotificationItemBaseVM)Activator.CreateInstance(this._itemConstructors[type], new object[] { data });
				if (mapNotificationItemBaseVM != null)
				{
					mapNotificationItemBaseVM.OnRemove = new Action<MapNotificationItemBaseVM>(this.RemoveNotificationItem);
					mapNotificationItemBaseVM.OnFocus = new Action<MapNotificationItemBaseVM>(this.OnNotificationItemFocus);
					mapNotificationItemBaseVM.SetNavigationHandler(this._navigationHandler);
					mapNotificationItemBaseVM.SetFastMoveCameraToPosition(this._fastMoveCameraToPosition);
					if (this.RemoveInputKey != null)
					{
						mapNotificationItemBaseVM.RemoveInputKey = this.RemoveInputKey;
					}
				}
			}
			return mapNotificationItemBaseVM;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001E0A5 File Offset: 0x0001C2A5
		public void SetRemoveInputKey(HotKey hotKey)
		{
			this.RemoveInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0001E0B4 File Offset: 0x0001C2B4
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x0001E0BC File Offset: 0x0001C2BC
		[DataSourceProperty]
		public InputKeyItemVM RemoveInputKey
		{
			get
			{
				return this._removeInputKey;
			}
			set
			{
				if (value != this._removeInputKey)
				{
					this._removeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RemoveInputKey");
					if (this._removeInputKey != null && this.NotificationItems != null)
					{
						for (int i = 0; i < this.NotificationItems.Count; i++)
						{
							this.NotificationItems[i].RemoveInputKey = this._removeInputKey;
						}
					}
				}
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0001E122 File Offset: 0x0001C322
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x0001E12A File Offset: 0x0001C32A
		[DataSourceProperty]
		public MapNotificationItemBaseVM FocusedNotificationItem
		{
			get
			{
				return this._focusedNotificationItem;
			}
			set
			{
				if (value != this._focusedNotificationItem)
				{
					this._focusedNotificationItem = value;
					base.OnPropertyChangedWithValue<MapNotificationItemBaseVM>(value, "FocusedNotificationItem");
				}
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0001E148 File Offset: 0x0001C348
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x0001E150 File Offset: 0x0001C350
		[DataSourceProperty]
		public MBBindingList<MapNotificationItemBaseVM> NotificationItems
		{
			get
			{
				return this._notificationItems;
			}
			set
			{
				if (value != this._notificationItems)
				{
					this._notificationItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapNotificationItemBaseVM>>(value, "NotificationItems");
				}
			}
		}

		// Token: 0x04000253 RID: 595
		private INavigationHandler _navigationHandler;

		// Token: 0x04000254 RID: 596
		private Action<CampaignVec2> _fastMoveCameraToPosition;

		// Token: 0x04000255 RID: 597
		private Dictionary<Type, Type> _itemConstructors = new Dictionary<Type, Type>();

		// Token: 0x04000256 RID: 598
		private InputKeyItemVM _removeInputKey;

		// Token: 0x04000257 RID: 599
		private MapNotificationItemBaseVM _focusedNotificationItem;

		// Token: 0x04000258 RID: 600
		private MBBindingList<MapNotificationItemBaseVM> _notificationItems;
	}
}
