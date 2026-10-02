using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000036 RID: 54
	public class MPLobbyBadgeSelectionPopupVM : ViewModel
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00011865 File Offset: 0x0000FA65
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x0001186D File Offset: 0x0000FA6D
		public List<LobbyNotification> ActiveNotifications { get; private set; }

		// Token: 0x060004F4 RID: 1268 RVA: 0x00011876 File Offset: 0x0000FA76
		public MPLobbyBadgeSelectionPopupVM(Action onBadgeNotificationRead, Action onBadgeSelectionUpdated, Action<MPLobbyAchievementBadgeGroupVM> onBadgeProgressInfoRequested)
		{
			this._onBadgeNotificationRead = onBadgeNotificationRead;
			this._onBadgeSelectionUpdated = onBadgeSelectionUpdated;
			this._onBadgeProgressInfoRequested = onBadgeProgressInfoRequested;
			this.ActiveNotifications = new List<LobbyNotification>();
			this.Badges = new MBBindingList<MPLobbyBadgeItemVM>();
			this.AchivementBadgeGroups = new MBBindingList<MPLobbyAchievementBadgeGroupVM>();
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000118B4 File Offset: 0x0000FAB4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.BadgesText = new TextObject("{=nqYaiEo2}My Badges", null).ToString();
			this.SpecialBadgesText = new TextObject("{=yI9EV0II}Special Badges", null).ToString();
			this.AchievementBadgesText = new TextObject("{=n6yb5VCI}Achievement Badges", null).ToString();
			this.AchivementBadgeGroups.ApplyActionOnAllItems(delegate(MPLobbyAchievementBadgeGroupVM g)
			{
				g.RefreshValues();
			});
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00011949 File Offset: 0x0000FB49
		public void RefreshPlayerData(PlayerData playerData)
		{
			this.UpdateBadges(false);
			this.UpdateBadgeSelection();
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00011958 File Offset: 0x0000FB58
		public void RefreshKeyBindings(HotKey inspectProgressKey)
		{
			this._inspectProgressKey = inspectProgressKey;
			foreach (MPLobbyAchievementBadgeGroupVM mplobbyAchievementBadgeGroupVM in this.AchivementBadgeGroups)
			{
				mplobbyAchievementBadgeGroupVM.RefreshKeyBindings(inspectProgressKey);
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000119AC File Offset: 0x0000FBAC
		public async void UpdateBadges(bool shouldClear = false)
		{
			Badge[] array = await NetworkMain.GameClient.GetPlayerBadges();
			this._playerEarnedBadges = array;
			if (shouldClear)
			{
				this.Badges.Clear();
			}
			if (!this.Badges.Any<MPLobbyBadgeItemVM>((MPLobbyBadgeItemVM b) => b.Badge == null))
			{
				this.Badges.Add(new MPLobbyBadgeItemVM(null, new Action(this.UpdateBadgeSelection), (Badge b) => true, new Action<MPLobbyBadgeItemVM>(this.OnBadgeInspected)));
			}
			if (BadgeManager.Badges != null)
			{
				using (List<Badge>.Enumerator enumerator = BadgeManager.Badges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Badge badge = enumerator.Current;
						if ((badge.IsActive && !badge.IsVisibleOnlyWhenEarned) || this._playerEarnedBadges.Contains(badge))
						{
							if (badge.GroupId != null)
							{
								MPLobbyAchievementBadgeGroupVM mplobbyAchievementBadgeGroupVM = this.AchivementBadgeGroups.FirstOrDefault<MPLobbyAchievementBadgeGroupVM>((MPLobbyAchievementBadgeGroupVM g) => g.GroupID == badge.GroupId);
								if (mplobbyAchievementBadgeGroupVM == null)
								{
									mplobbyAchievementBadgeGroupVM = new MPLobbyAchievementBadgeGroupVM(badge.GroupId, this._onBadgeProgressInfoRequested);
									mplobbyAchievementBadgeGroupVM.RefreshKeyBindings(this._inspectProgressKey);
									this.AchivementBadgeGroups.Add(mplobbyAchievementBadgeGroupVM);
									mplobbyAchievementBadgeGroupVM.OnGroupBadgeAdded(new MPLobbyBadgeItemVM(badge, new Action(this.UpdateBadgeSelection), new Func<Badge, bool>(this.HasPlayerEarnedBadge), new Action<MPLobbyBadgeItemVM>(this.OnBadgeInspected)));
								}
								else
								{
									MPLobbyBadgeItemVM mplobbyBadgeItemVM = mplobbyAchievementBadgeGroupVM.Badges.FirstOrDefault<MPLobbyBadgeItemVM>((MPLobbyBadgeItemVM b) => b.Badge == badge);
									if (mplobbyBadgeItemVM == null)
									{
										mplobbyAchievementBadgeGroupVM.OnGroupBadgeAdded(new MPLobbyBadgeItemVM(badge, new Action(this.UpdateBadgeSelection), new Func<Badge, bool>(this.HasPlayerEarnedBadge), new Action<MPLobbyBadgeItemVM>(this.OnBadgeInspected)));
									}
									else
									{
										mplobbyBadgeItemVM.UpdateWith(badge);
									}
								}
							}
							else
							{
								MPLobbyBadgeItemVM mplobbyBadgeItemVM2 = this.Badges.SingleOrDefault<MPLobbyBadgeItemVM>((MPLobbyBadgeItemVM b) => b.Badge == badge);
								if (mplobbyBadgeItemVM2 == null)
								{
									this.Badges.Add(new MPLobbyBadgeItemVM(badge, new Action(this.UpdateBadgeSelection), new Func<Badge, bool>(this.HasPlayerEarnedBadge), new Action<MPLobbyBadgeItemVM>(this.OnBadgeInspected)));
								}
								else
								{
									mplobbyBadgeItemVM2.UpdateWith(badge);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x000119F0 File Offset: 0x0000FBF0
		public void UpdateBadgeSelection()
		{
			foreach (MPLobbyBadgeItemVM mplobbyBadgeItemVM in this.Badges)
			{
				mplobbyBadgeItemVM.UpdateIsSelected();
			}
			foreach (MPLobbyAchievementBadgeGroupVM mplobbyAchievementBadgeGroupVM in this.AchivementBadgeGroups)
			{
				mplobbyAchievementBadgeGroupVM.UpdateBadgeSelection();
			}
			Action onBadgeSelectionUpdated = this._onBadgeSelectionUpdated;
			if (onBadgeSelectionUpdated == null)
			{
				return;
			}
			onBadgeSelectionUpdated();
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00011A84 File Offset: 0x0000FC84
		private bool HasPlayerEarnedBadge(Badge badge)
		{
			Badge[] playerEarnedBadges = this._playerEarnedBadges;
			return playerEarnedBadges != null && playerEarnedBadges.Contains(badge);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00011A98 File Offset: 0x0000FC98
		public void OnNotificationReceived(LobbyNotification notification)
		{
			string badgeID = notification.Parameters["badge_id"];
			IEnumerable<MPLobbyBadgeItemVM> badges = this.Badges;
			Func<MPLobbyBadgeItemVM, bool> <>9__0;
			Func<MPLobbyBadgeItemVM, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (MPLobbyBadgeItemVM badge) => badge.BadgeId == badgeID);
			}
			foreach (MPLobbyBadgeItemVM mplobbyBadgeItemVM in badges.Where<MPLobbyBadgeItemVM>(func))
			{
				mplobbyBadgeItemVM.HasNotification = true;
			}
			this.ActiveNotifications.Add(notification);
			this.RefreshNotificationInfo();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00011B38 File Offset: 0x0000FD38
		public void OnBadgeInspected(MPLobbyBadgeItemVM badge)
		{
			this.InspectedBadge = badge;
			if (badge != null)
			{
				string badgeID = badge.BadgeId;
				foreach (LobbyNotification lobbyNotification in this.ActiveNotifications.Where<LobbyNotification>((LobbyNotification n) => n.Parameters["badge_id"] == badgeID))
				{
					NetworkMain.GameClient.MarkNotificationAsRead(lobbyNotification.Id);
				}
				this.ActiveNotifications.RemoveAll((LobbyNotification n) => n.Parameters["badge_id"] == badgeID);
				this.RefreshNotificationInfo();
				Action onBadgeNotificationRead = this._onBadgeNotificationRead;
				if (onBadgeNotificationRead == null)
				{
					return;
				}
				onBadgeNotificationRead();
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00011BEC File Offset: 0x0000FDEC
		private void RefreshNotificationInfo()
		{
			this.HasNotifications = this.ActiveNotifications.Count > 0;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00011C02 File Offset: 0x0000FE02
		public void Open()
		{
			this.IsEnabled = true;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00011C0B File Offset: 0x0000FE0B
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00011C14 File Offset: 0x0000FE14
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00011C2C File Offset: 0x0000FE2C
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x00011C3B File Offset: 0x0000FE3B
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x00011C43 File Offset: 0x0000FE43
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00011C60 File Offset: 0x0000FE60
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x00011C68 File Offset: 0x0000FE68
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00011C86 File Offset: 0x0000FE86
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00011C8E File Offset: 0x0000FE8E
		[DataSourceProperty]
		public bool HasNotifications
		{
			get
			{
				return this._hasNotifications;
			}
			set
			{
				if (value != this._hasNotifications)
				{
					this._hasNotifications = value;
					base.OnPropertyChangedWithValue(value, "HasNotifications");
				}
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00011CAC File Offset: 0x0000FEAC
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x00011CB4 File Offset: 0x0000FEB4
		[DataSourceProperty]
		public string CloseText
		{
			get
			{
				return this._closeText;
			}
			set
			{
				if (value != this._closeText)
				{
					this._closeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CloseText");
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00011CD7 File Offset: 0x0000FED7
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x00011CDF File Offset: 0x0000FEDF
		[DataSourceProperty]
		public string BadgesText
		{
			get
			{
				return this._badgesText;
			}
			set
			{
				if (value != this._badgesText)
				{
					this._badgesText = value;
					base.OnPropertyChangedWithValue<string>(value, "BadgesText");
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x00011D02 File Offset: 0x0000FF02
		// (set) Token: 0x0600050D RID: 1293 RVA: 0x00011D0A File Offset: 0x0000FF0A
		[DataSourceProperty]
		public string SpecialBadgesText
		{
			get
			{
				return this._specialBadgesText;
			}
			set
			{
				if (value != this._specialBadgesText)
				{
					this._specialBadgesText = value;
					base.OnPropertyChangedWithValue<string>(value, "SpecialBadgesText");
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00011D2D File Offset: 0x0000FF2D
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x00011D35 File Offset: 0x0000FF35
		[DataSourceProperty]
		public string AchievementBadgesText
		{
			get
			{
				return this._achievementBadgesText;
			}
			set
			{
				if (value != this._achievementBadgesText)
				{
					this._achievementBadgesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AchievementBadgesText");
				}
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x00011D58 File Offset: 0x0000FF58
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x00011D60 File Offset: 0x0000FF60
		[DataSourceProperty]
		public MBBindingList<MPLobbyBadgeItemVM> Badges
		{
			get
			{
				return this._badges;
			}
			set
			{
				if (value != this._badges)
				{
					this._badges = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyBadgeItemVM>>(value, "Badges");
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00011D7E File Offset: 0x0000FF7E
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x00011D86 File Offset: 0x0000FF86
		[DataSourceProperty]
		public MBBindingList<MPLobbyAchievementBadgeGroupVM> AchivementBadgeGroups
		{
			get
			{
				return this._achievementBadgeGroups;
			}
			set
			{
				if (value != this._achievementBadgeGroups)
				{
					this._achievementBadgeGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyAchievementBadgeGroupVM>>(value, "AchivementBadgeGroups");
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x00011DA4 File Offset: 0x0000FFA4
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x00011DAC File Offset: 0x0000FFAC
		[DataSourceProperty]
		public MPLobbyBadgeItemVM InspectedBadge
		{
			get
			{
				return this._inspectedBadge;
			}
			set
			{
				if (value != this._inspectedBadge)
				{
					this._inspectedBadge = value;
					base.OnPropertyChangedWithValue<MPLobbyBadgeItemVM>(value, "InspectedBadge");
				}
			}
		}

		// Token: 0x04000265 RID: 613
		private Badge[] _playerEarnedBadges;

		// Token: 0x04000267 RID: 615
		private Action _onBadgeNotificationRead;

		// Token: 0x04000268 RID: 616
		private Action<MPLobbyAchievementBadgeGroupVM> _onBadgeProgressInfoRequested;

		// Token: 0x04000269 RID: 617
		private Action _onBadgeSelectionUpdated;

		// Token: 0x0400026A RID: 618
		private HotKey _inspectProgressKey;

		// Token: 0x0400026B RID: 619
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400026C RID: 620
		private bool _isEnabled;

		// Token: 0x0400026D RID: 621
		private bool _hasNotifications;

		// Token: 0x0400026E RID: 622
		private string _closeText;

		// Token: 0x0400026F RID: 623
		private string _badgesText;

		// Token: 0x04000270 RID: 624
		private string _specialBadgesText;

		// Token: 0x04000271 RID: 625
		private string _achievementBadgesText;

		// Token: 0x04000272 RID: 626
		private MBBindingList<MPLobbyBadgeItemVM> _badges;

		// Token: 0x04000273 RID: 627
		private MBBindingList<MPLobbyAchievementBadgeGroupVM> _achievementBadgeGroups;

		// Token: 0x04000274 RID: 628
		private MPLobbyBadgeItemVM _inspectedBadge;
	}
}
