using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000056 RID: 86
	public class MPLobbyFriendsVM : ViewModel
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x00017F10 File Offset: 0x00016110
		private PlayerId? _partyLeaderId
		{
			get
			{
				PartyPlayerInLobbyClient partyPlayerInLobbyClient = NetworkMain.GameClient.PlayersInParty.SingleOrDefault<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient p) => p.IsPartyLeader);
				if (partyPlayerInLobbyClient == null)
				{
					return null;
				}
				return new PlayerId?(partyPlayerInLobbyClient.PlayerId);
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00017F64 File Offset: 0x00016164
		public MPLobbyFriendsVM()
		{
			this.Player = new MPLobbyPartyPlayerVM(NetworkMain.GameClient.PlayerID, new Action<MPLobbyPartyPlayerVM>(this.ActivatePlayerActions));
			this.PartyFriends = new MBBindingList<MPLobbyPartyPlayerVM>();
			this.PlayerActions = new MBBindingList<StringPairItemWithActionVM>();
			this.FriendServices = new MBBindingList<MPLobbyFriendServiceVM>();
			IFriendListService[] friendListServices = PlatformServices.Instance.GetFriendListServices();
			for (int i = 0; i < friendListServices.Length; i++)
			{
				MPLobbyFriendServiceVM mplobbyFriendServiceVM = new MPLobbyFriendServiceVM(friendListServices[i], new Action<PlayerId>(this.OnFriendRequestAnswered), new Action<MPLobbyPlayerBaseVM>(this.ActivatePlayerActions));
				this.FriendServices.Add(mplobbyFriendServiceVM);
			}
			this._activeServiceIndex = 0;
			this.UpdateActiveService();
			this._activeNotifications = new List<LobbyNotification>();
			this.RefreshValues();
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00018024 File Offset: 0x00016224
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=abxndmIh}Social", null).ToString();
			this.InGameText = new TextObject("{=uUoSmCBS}In Bannerlord", null).ToString();
			this.OnlineText = new TextObject("{=V305MaOP}Online", null).ToString();
			this.OfflineText = new TextObject("{=Zv1lg272}Offline", null).ToString();
			this.FriendListHint = new HintViewModel(new TextObject("{=tjioq56N}Friend List", null), null);
			this.PartyFriends.ApplyActionOnAllItems(delegate(MPLobbyPartyPlayerVM x)
			{
				x.RefreshValues();
			});
			this.PlayerActions.ApplyActionOnAllItems(delegate(StringPairItemWithActionVM x)
			{
				x.RefreshValues();
			});
			this.FriendServices.ApplyActionOnAllItems(delegate(MPLobbyFriendServiceVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00018124 File Offset: 0x00016324
		public override void OnFinalize()
		{
			base.OnFinalize();
			foreach (MPLobbyFriendServiceVM mplobbyFriendServiceVM in this.FriendServices)
			{
				mplobbyFriendServiceVM.OnFinalize();
			}
			this.ToggleInputKey.OnFinalize();
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00018180 File Offset: 0x00016380
		public void OnStateActivate()
		{
			this.IsPartyAvailable = NetworkMain.GameClient.PartySystemAvailable;
			this.GetPartyData();
			foreach (MPLobbyFriendServiceVM mplobbyFriendServiceVM in this.FriendServices)
			{
				mplobbyFriendServiceVM.OnStateActivate();
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x000181E0 File Offset: 0x000163E0
		private void IsEnabledUpdated()
		{
			this.GetPartyData();
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x000181E8 File Offset: 0x000163E8
		private void GetPartyData()
		{
			this.PartyFriends.Clear();
			if (NetworkMain.GameClient.IsInParty)
			{
				foreach (PartyPlayerInLobbyClient partyPlayerInLobbyClient in NetworkMain.GameClient.PlayersInParty)
				{
					if (partyPlayerInLobbyClient.WaitingInvitation)
					{
						this.OnPlayerInvitedToParty(partyPlayerInLobbyClient.PlayerId);
					}
					else
					{
						this.OnPlayerAddedToParty(partyPlayerInLobbyClient.PlayerId);
					}
				}
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00018274 File Offset: 0x00016474
		public void OnTick(float dt)
		{
			int num = 0;
			foreach (MPLobbyFriendServiceVM mplobbyFriendServiceVM in this.FriendServices)
			{
				mplobbyFriendServiceVM.OnTick(dt);
				if (mplobbyFriendServiceVM.FriendListService.IncludeInAllFriends)
				{
					num += mplobbyFriendServiceVM.OnlineFriends.FriendList.Count;
					num += mplobbyFriendServiceVM.InGameFriends.FriendList.Count;
				}
			}
			this.TotalOnlineFriendCount = num;
			this.IsInParty = NetworkMain.GameClient.IsInParty;
			this.RemoveAnsweredNotifications(null);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0001831C File Offset: 0x0001651C
		public void OnPlayerInvitedToParty(PlayerId playerId)
		{
			if (playerId != NetworkMain.GameClient.PlayerData.PlayerId)
			{
				MPLobbyPartyPlayerVM mplobbyPartyPlayerVM = new MPLobbyPartyPlayerVM(playerId, new Action<MPLobbyPartyPlayerVM>(this.ActivatePlayerActions));
				mplobbyPartyPlayerVM.IsWaitingConfirmation = true;
				this.PartyFriends.Add(mplobbyPartyPlayerVM);
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00018368 File Offset: 0x00016568
		public void OnPlayerAddedToParty(PlayerId playerId)
		{
			if (playerId != NetworkMain.GameClient.PlayerData.PlayerId)
			{
				MPLobbyPartyPlayerVM mplobbyPartyPlayerVM = this.FindPartyFriend(playerId);
				if (mplobbyPartyPlayerVM == null)
				{
					mplobbyPartyPlayerVM = new MPLobbyPartyPlayerVM(playerId, new Action<MPLobbyPartyPlayerVM>(this.ActivatePlayerActions));
					this.PartyFriends.Add(mplobbyPartyPlayerVM);
				}
				else
				{
					mplobbyPartyPlayerVM.IsWaitingConfirmation = false;
				}
			}
			this.UpdateCanInviteOtherPlayersToParty();
			this.UpdatePartyLeader();
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x000183CC File Offset: 0x000165CC
		public void OnPlayerRemovedFromParty(PlayerId playerId)
		{
			if (playerId == NetworkMain.GameClient.PlayerData.PlayerId)
			{
				this.PartyFriends.Clear();
			}
			else
			{
				int num = -1;
				for (int i = 0; i < this.PartyFriends.Count; i++)
				{
					if (this.PartyFriends[i].ProvidedID == playerId)
					{
						num = i;
						break;
					}
				}
				if (this.PartyFriends.Count > 0 && num > -1 && num < this.PartyFriends.Count)
				{
					this.PartyFriends.RemoveAt(num);
				}
			}
			this.UpdateCanInviteOtherPlayersToParty();
			this.UpdatePartyLeader();
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001846C File Offset: 0x0001666C
		private MPLobbyPartyPlayerVM FindPartyFriend(PlayerId playerId)
		{
			foreach (MPLobbyPartyPlayerVM mplobbyPartyPlayerVM in this.PartyFriends)
			{
				if (mplobbyPartyPlayerVM.ProvidedID == playerId)
				{
					return mplobbyPartyPlayerVM;
				}
			}
			return null;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x000184C8 File Offset: 0x000166C8
		internal void OnPlayerAssignedPartyLeader()
		{
			this.UpdatePartyLeader();
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000184D0 File Offset: 0x000166D0
		internal void OnClanInfoChanged()
		{
			MPLobbyFriendServiceVM mplobbyFriendServiceVM = this.FriendServices.FirstOrDefault<MPLobbyFriendServiceVM>((MPLobbyFriendServiceVM f) => f.FriendListService.GetServiceCodeName() == "ClanFriends");
			if (NetworkMain.GameClient.IsInClan && mplobbyFriendServiceVM == null)
			{
				IFriendListService[] friendListServices = PlatformServices.Instance.GetFriendListServices();
				IFriendListService friendListService = friendListServices.FirstOrDefault<IFriendListService>((IFriendListService f) => f.GetServiceCodeName() == "ClanFriends");
				if (friendListService != null)
				{
					MPLobbyFriendServiceVM mplobbyFriendServiceVM2 = new MPLobbyFriendServiceVM(friendListService, new Action<PlayerId>(this.OnFriendRequestAnswered), new Action<MPLobbyPlayerBaseVM>(this.ActivatePlayerActions));
					this.FriendServices.Insert(friendListServices.Length - 2, mplobbyFriendServiceVM2);
					mplobbyFriendServiceVM2.ForceRefresh();
					return;
				}
			}
			else
			{
				if (NetworkMain.GameClient.IsInClan && mplobbyFriendServiceVM != null)
				{
					mplobbyFriendServiceVM.ForceRefresh();
					return;
				}
				if (!NetworkMain.GameClient.IsInClan && mplobbyFriendServiceVM != null)
				{
					for (int i = this.FriendServices.Count - 1; i >= 0; i--)
					{
						IFriendListService friendListService2 = this.FriendServices[i].FriendListService;
						if (!NetworkMain.GameClient.IsInClan && friendListService2.GetServiceCodeName() == "ClanFriends")
						{
							this.FriendServices[i].OnFinalize();
							this.FriendServices.RemoveAt(i);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001861C File Offset: 0x0001681C
		private void ActivatePlayerActions(MPLobbyPlayerBaseVM player)
		{
			this.PlayerActions.Clear();
			MPLobbyPartyPlayerVM mplobbyPartyPlayerVM;
			MPLobbyFriendItemVM mplobbyFriendItemVM;
			if ((mplobbyPartyPlayerVM = player as MPLobbyPartyPlayerVM) != null)
			{
				this.ActivatePartyPlayerActions(mplobbyPartyPlayerVM);
			}
			else if ((mplobbyFriendItemVM = player as MPLobbyFriendItemVM) != null)
			{
				this.ActivateFriendPlayerActions(mplobbyFriendItemVM);
			}
			this.IsPlayerActionsActive = false;
			this.IsPlayerActionsActive = this.PlayerActions.Count > 0;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00018674 File Offset: 0x00016874
		private void ExecuteSetPlayerAsLeader(object playerObj)
		{
			MPLobbyPlayerBaseVM mplobbyPlayerBaseVM = playerObj as MPLobbyPlayerBaseVM;
			NetworkMain.GameClient.PromotePlayerToPartyLeader(mplobbyPlayerBaseVM.ProvidedID);
			this.UpdatePartyLeader();
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x000186A0 File Offset: 0x000168A0
		private void ExecuteKickPlayerFromParty(object playerObj)
		{
			MPLobbyPlayerBaseVM mplobbyPlayerBaseVM = playerObj as MPLobbyPlayerBaseVM;
			if (NetworkMain.GameClient.IsInParty && NetworkMain.GameClient.IsPartyLeader)
			{
				NetworkMain.GameClient.KickPlayerFromParty(mplobbyPlayerBaseVM.ProvidedID);
			}
			this.UpdatePartyLeader();
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x000186E4 File Offset: 0x000168E4
		private void ExecuteLeaveParty(object playerObj)
		{
			MPLobbyPlayerBaseVM mplobbyPlayerBaseVM = playerObj as MPLobbyPlayerBaseVM;
			if (NetworkMain.GameClient.IsInParty && mplobbyPlayerBaseVM.ProvidedID == NetworkMain.GameClient.PlayerData.PlayerId)
			{
				NetworkMain.GameClient.KickPlayerFromParty(NetworkMain.GameClient.PlayerData.PlayerId);
			}
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0001873C File Offset: 0x0001693C
		private void ExecuteInviteFriend(PlayerId providedId)
		{
			bool dontUseNameForUnknownPlayer = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(providedId);
			PermissionResult <>9__1;
			PlatformServices.Instance.CheckPrivilege(Privilege.Communication, true, delegate(bool result)
			{
				if (result)
				{
					PlayerIdProvidedTypes providedType = providedId.ProvidedType;
					LobbyClient gameClient = NetworkMain.GameClient;
					PlayerIdProvidedTypes? playerIdProvidedTypes = ((gameClient != null) ? new PlayerIdProvidedTypes?(gameClient.PlayerID.ProvidedType) : null);
					if (!((providedType == playerIdProvidedTypes.GetValueOrDefault()) & (playerIdProvidedTypes != null)))
					{
						NetworkMain.GameClient.InviteToParty(providedId, dontUseNameForUnknownPlayer);
						return;
					}
					IPlatformServices instance = PlatformServices.Instance;
					Permission permission = Permission.PlayMultiplayer;
					PlayerId providedId2 = providedId;
					PermissionResult permissionResult2;
					if ((permissionResult2 = <>9__1) == null)
					{
						permissionResult2 = (<>9__1 = delegate(bool permissionResult)
						{
							if (permissionResult)
							{
								NetworkMain.GameClient.InviteToParty(providedId, dontUseNameForUnknownPlayer);
								return;
							}
							string text = new TextObject("{=ZwN6rzTC}No permission", null).ToString();
							string text2 = new TextObject("{=wlz3eQWp}No permission to invite player.", null).ToString();
							InformationManager.ShowInquiry(new InquiryData(text, text2, false, true, "", new TextObject("{=dismissnotification}Dismiss", null).ToString(), null, delegate
							{
								InformationManager.HideInquiry();
							}, "event:/ui/notification/quest_update", 0f, null, null, null), false, false);
						});
					}
					instance.CheckPermissionWithUser(permission, providedId2, permissionResult2);
				}
			});
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00018794 File Offset: 0x00016994
		private void ExecuteRequestFriendship(object playerObj)
		{
			MPLobbyPlayerBaseVM mplobbyPlayerBaseVM = playerObj as MPLobbyPlayerBaseVM;
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(mplobbyPlayerBaseVM.ProvidedID);
			NetworkMain.GameClient.AddFriend(mplobbyPlayerBaseVM.ProvidedID, flag);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000187D8 File Offset: 0x000169D8
		private void ExecuteTerminateFriendship(object memberObj)
		{
			MPLobbyPlayerBaseVM mplobbyPlayerBaseVM = memberObj as MPLobbyPlayerBaseVM;
			NetworkMain.GameClient.RemoveFriend(mplobbyPlayerBaseVM.ProvidedID);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x000187FC File Offset: 0x000169FC
		public void UpdateCanInviteOtherPlayersToParty()
		{
			foreach (MPLobbyFriendServiceVM mplobbyFriendServiceVM in this.FriendServices)
			{
				mplobbyFriendServiceVM.UpdateCanInviteOtherPlayersToParty();
			}
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00018848 File Offset: 0x00016A48
		public void UpdatePartyLeader()
		{
			this.Player.IsPartyLeader = NetworkMain.GameClient.IsInParty && this.Player.ProvidedID == this._partyLeaderId;
			foreach (MPLobbyPartyPlayerVM mplobbyPartyPlayerVM in this.PartyFriends)
			{
				mplobbyPartyPlayerVM.IsPartyLeader = mplobbyPartyPlayerVM.ProvidedID == this._partyLeaderId;
			}
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00018900 File Offset: 0x00016B00
		private static bool TryGetRequesterID(LobbyNotification notification, out PlayerId requesterID)
		{
			requesterID = default(PlayerId);
			string text;
			if (((notification != null) ? notification.Parameters : null) == null || !notification.Parameters.TryGetValue("friend_requester", out text) || string.IsNullOrEmpty(text))
			{
				return false;
			}
			bool flag;
			try
			{
				requesterID = PlayerId.FromString(text);
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00018968 File Offset: 0x00016B68
		private bool IsFriendRequestPending(PlayerId playerID)
		{
			FriendInfo[] friendInfos = NetworkMain.GameClient.FriendInfos;
			if (friendInfos == null)
			{
				return false;
			}
			foreach (FriendInfo friendInfo in friendInfos)
			{
				if (friendInfo.Id == playerID)
				{
					return friendInfo.Status == FriendStatus.Received;
				}
			}
			return false;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x000189B2 File Offset: 0x00016BB2
		private void RefreshNotificationCount()
		{
			this.NotificationCount = this._activeNotifications.Count;
			this.HasNotification = this._activeNotifications.Count > 0;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x000189DC File Offset: 0x00016BDC
		private void RemoveAnsweredNotifications(PlayerId? justAnsweredID = null)
		{
			bool flag = false;
			for (int i = this._activeNotifications.Count - 1; i >= 0; i--)
			{
				LobbyNotification lobbyNotification = this._activeNotifications[i];
				PlayerId playerId;
				if (!MPLobbyFriendsVM.TryGetRequesterID(lobbyNotification, out playerId) || !(playerId != justAnsweredID) || !this.IsFriendRequestPending(playerId))
				{
					NetworkMain.GameClient.MarkNotificationAsRead(lobbyNotification.Id);
					this._activeNotifications.RemoveAt(i);
					flag = true;
				}
			}
			if (flag)
			{
				this.RefreshNotificationCount();
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00018A6C File Offset: 0x00016C6C
		public void OnFriendRequestNotificationsReceived(List<LobbyNotification> notifications)
		{
			using (List<LobbyNotification>.Enumerator enumerator = notifications.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MPLobbyFriendsVM.<>c__DisplayClass31_0 CS$<>8__locals1 = new MPLobbyFriendsVM.<>c__DisplayClass31_0();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.notification = enumerator.Current;
					PlayerId notificationPlayerID;
					if (!MPLobbyFriendsVM.TryGetRequesterID(CS$<>8__locals1.notification, out notificationPlayerID))
					{
						NetworkMain.GameClient.MarkNotificationAsRead(CS$<>8__locals1.notification.Id);
					}
					else if (!this._activeNotifications.Exists(delegate(LobbyNotification n)
					{
						PlayerId playerId;
						return MPLobbyFriendsVM.TryGetRequesterID(n, out playerId) && playerId == notificationPlayerID;
					}))
					{
						PermissionResult <>9__2;
						PlatformServices.Instance.CheckPrivilege(Privilege.Communication, false, delegate(bool privilegeResult)
						{
							if (!privilegeResult)
							{
								CS$<>8__locals1.<>4__this.ProcessNotification(CS$<>8__locals1.notification, notificationPlayerID, false);
								return;
							}
							IPlatformServices instance = PlatformServices.Instance;
							Permission permission = Permission.CommunicateUsingText;
							PlayerId notificationPlayerID2 = notificationPlayerID;
							PermissionResult permissionResult2;
							if ((permissionResult2 = <>9__2) == null)
							{
								permissionResult2 = (<>9__2 = delegate(bool permissionResult)
								{
									CS$<>8__locals1.<>4__this.ProcessNotification(CS$<>8__locals1.notification, notificationPlayerID, permissionResult);
								});
							}
							instance.CheckPermissionWithUser(permission, notificationPlayerID2, permissionResult2);
						});
					}
				}
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00018B40 File Offset: 0x00016D40
		private void ProcessNotification(LobbyNotification notification, PlayerId notificationPlayerID, bool allowed)
		{
			if (!allowed)
			{
				NetworkMain.GameClient.MarkNotificationAsRead(notification.Id);
				return;
			}
			if (MultiplayerPlayerHelper.IsBlocked(notificationPlayerID))
			{
				bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(notificationPlayerID);
				NetworkMain.GameClient.RespondToFriendRequest(notificationPlayerID, flag, false, true);
				NetworkMain.GameClient.MarkNotificationAsRead(notification.Id);
				return;
			}
			if (!this.IsFriendRequestPending(notificationPlayerID))
			{
				NetworkMain.GameClient.MarkNotificationAsRead(notification.Id);
				return;
			}
			this._activeNotifications.Add(notification);
			this.RefreshNotificationCount();
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00018BCD File Offset: 0x00016DCD
		private void OnFriendRequestAnswered(PlayerId playerID)
		{
			this.RemoveAnsweredNotifications(new PlayerId?(playerID));
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00018BDC File Offset: 0x00016DDC
		public MBBindingList<MPLobbyPlayerBaseVM> GetAllFriends()
		{
			MBBindingList<MPLobbyPlayerBaseVM> mbbindingList = new MBBindingList<MPLobbyPlayerBaseVM>();
			foreach (MPLobbyFriendServiceVM mplobbyFriendServiceVM in this.FriendServices)
			{
				if (mplobbyFriendServiceVM.FriendListService.IncludeInAllFriends)
				{
					foreach (MPLobbyFriendItemVM mplobbyFriendItemVM in mplobbyFriendServiceVM.InGameFriends.FriendList)
					{
						mbbindingList.Add(mplobbyFriendItemVM);
					}
					foreach (MPLobbyFriendItemVM mplobbyFriendItemVM2 in mplobbyFriendServiceVM.OnlineFriends.FriendList)
					{
						mbbindingList.Add(mplobbyFriendItemVM2);
					}
				}
			}
			return mbbindingList;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00018CC4 File Offset: 0x00016EC4
		public void OnSupportedFeaturesRefreshed(SupportedFeatures supportedFeatures)
		{
			if (!supportedFeatures.SupportsFeatures(Features.BannerlordFriendList))
			{
				MPLobbyFriendServiceVM mplobbyFriendServiceVM = this.FriendServices.FirstOrDefault<MPLobbyFriendServiceVM>((MPLobbyFriendServiceVM fs) => fs.FriendListService.GetType() == typeof(BannerlordFriendListService));
				if (mplobbyFriendServiceVM != null)
				{
					mplobbyFriendServiceVM.OnFinalize();
				}
				this.FriendServices.Remove(mplobbyFriendServiceVM);
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00018D1C File Offset: 0x00016F1C
		public void OnFriendListUpdated(bool forceUpdate = false)
		{
			foreach (MPLobbyFriendServiceVM mplobbyFriendServiceVM in this.FriendServices)
			{
				mplobbyFriendServiceVM.OnFriendListUpdated(forceUpdate);
			}
			this.Player.UpdateNameAndAvatar(forceUpdate);
			foreach (MPLobbyPartyPlayerVM mplobbyPartyPlayerVM in this.PartyFriends)
			{
				mplobbyPartyPlayerVM.UpdateNameAndAvatar(forceUpdate);
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00018DB0 File Offset: 0x00016FB0
		public void SetToggleFriendListKey(HotKey hotkey)
		{
			this.ToggleInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00018DC0 File Offset: 0x00016FC0
		private void ActivatePartyPlayerActions(MPLobbyPartyPlayerVM player)
		{
			if (NetworkMain.GameClient.IsPartyLeader && player.ProvidedID != NetworkMain.GameClient.PlayerData.PlayerId)
			{
				PartyPlayerInLobbyClient partyPlayerInLobbyClient = NetworkMain.GameClient.PlayersInParty.SingleOrDefault<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient p) => p.PlayerId == player.ProvidedID);
				if (partyPlayerInLobbyClient != null && !partyPlayerInLobbyClient.WaitingInvitation && PlatformServices.InvitationServices == null)
				{
					this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteSetPlayerAsLeader), new TextObject("{=P7moPm3F}Set as party leader", null).ToString(), "PromoteToPartyLeader", player));
				}
				this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteKickPlayerFromParty), new TextObject("{=partykick}Kick", null).ToString(), "Kick", player));
			}
			if (player.ProvidedID == NetworkMain.GameClient.PlayerData.PlayerId)
			{
				if (NetworkMain.GameClient.IsInParty)
				{
					this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteLeaveParty), new TextObject("{=9w9JsBYP}Leave party", null).ToString(), "LeaveParty", player));
					return;
				}
			}
			else
			{
				bool flag = false;
				FriendInfo[] friendInfos = NetworkMain.GameClient.FriendInfos;
				for (int i = 0; i < friendInfos.Length; i++)
				{
					if (friendInfos[i].Id == player.ProvidedID)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteRequestFriendship), new TextObject("{=UwkpJq9N}Add As Friend", null).ToString(), "RequestFriendship", player));
				}
				else
				{
					this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteTerminateFriendship), new TextObject("{=2YIVRuRa}Remove From Friends", null).ToString(), "TerminateFriendship", player));
				}
				MultiplayerPlayerContextMenuHelper.AddLobbyViewProfileOptions(player, this.PlayerActions);
			}
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00018FD4 File Offset: 0x000171D4
		private void ActivateFriendPlayerActions(MPLobbyFriendItemVM player)
		{
			if (player.CanSpectate)
			{
				this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteSpectateMatch), new TextObject("{=V0zs1LfD}Watch Game", null).ToString(), "SpectateMatch", player));
			}
			MultiplayerPlayerContextMenuHelper.AddLobbyViewProfileOptions(player, this.PlayerActions);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00019027 File Offset: 0x00017227
		private void ExecuteSpectateMatch(object playerObj)
		{
			MPLobbyPlayerBaseVM mplobbyPlayerBaseVM = playerObj as MPLobbyPlayerBaseVM;
			if (mplobbyPlayerBaseVM == null)
			{
				return;
			}
			mplobbyPlayerBaseVM.ExecuteSpectateMatch();
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001903C File Offset: 0x0001723C
		private void ExecuteSwitchToNextService()
		{
			if (this.FriendServices.Count == 0)
			{
				Debug.FailedAssert("Friend service list is empty!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Friends\\MPLobbyFriendsVM.cs", "ExecuteSwitchToNextService", 636);
				return;
			}
			this._activeServiceIndex++;
			if (this._activeServiceIndex >= this.FriendServices.Count)
			{
				this._activeServiceIndex = 0;
			}
			this.UpdateActiveService();
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x000190A0 File Offset: 0x000172A0
		private void ExecuteSwitchToPreviousService()
		{
			if (this.FriendServices.Count == 0)
			{
				Debug.FailedAssert("Friend service list is empty!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Friends\\MPLobbyFriendsVM.cs", "ExecuteSwitchToPreviousService", 653);
				return;
			}
			this._activeServiceIndex--;
			if (this._activeServiceIndex < 0)
			{
				this._activeServiceIndex = this.FriendServices.Count - 1;
			}
			this.UpdateActiveService();
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00019104 File Offset: 0x00017304
		private void UpdateActiveService()
		{
			if (this._activeServiceIndex < 0 || this._activeServiceIndex >= this.FriendServices.Count)
			{
				Debug.FailedAssert(string.Format("Multiplayer service index is invalid: {0}", this._activeServiceIndex), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Friends\\MPLobbyFriendsVM.cs", "UpdateActiveService", 670);
				if (this.FriendServices.Count <= 0)
				{
					Debug.FailedAssert("Cancelling service update request.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Friends\\MPLobbyFriendsVM.cs", "UpdateActiveService", 678);
					return;
				}
				Debug.FailedAssert("Defaulting to first available service: " + this.FriendServices[0].ServiceName, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Friends\\MPLobbyFriendsVM.cs", "UpdateActiveService", 673);
				this._activeServiceIndex = 0;
			}
			this.ActiveService = this.FriendServices[this._activeServiceIndex];
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x000191D0 File Offset: 0x000173D0
		// (set) Token: 0x060007A0 RID: 1952 RVA: 0x000191D8 File Offset: 0x000173D8
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
					this.IsEnabledUpdated();
				}
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x000191FC File Offset: 0x000173FC
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x00019204 File Offset: 0x00017404
		[DataSourceProperty]
		public bool IsListEnabled
		{
			get
			{
				return this._isListEnabled;
			}
			set
			{
				if (value != this._isListEnabled)
				{
					this._isListEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsListEnabled");
				}
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00019222 File Offset: 0x00017422
		// (set) Token: 0x060007A4 RID: 1956 RVA: 0x0001922A File Offset: 0x0001742A
		[DataSourceProperty]
		public bool IsPlayerActionsActive
		{
			get
			{
				return this._isPlayerActionsActive;
			}
			set
			{
				if (value != this._isPlayerActionsActive)
				{
					this._isPlayerActionsActive = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerActionsActive");
				}
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x00019248 File Offset: 0x00017448
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x00019250 File Offset: 0x00017450
		[DataSourceProperty]
		public bool IsPartyAvailable
		{
			get
			{
				return this._isPartyAvailable;
			}
			set
			{
				if (value != this._isPartyAvailable)
				{
					this._isPartyAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsPartyAvailable");
				}
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0001926E File Offset: 0x0001746E
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x00019276 File Offset: 0x00017476
		[DataSourceProperty]
		public bool IsPartyFull
		{
			get
			{
				return this._isPartyFull;
			}
			set
			{
				if (value != this._isPartyFull)
				{
					this._isPartyFull = value;
					base.OnPropertyChangedWithValue(value, "IsPartyFull");
				}
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00019294 File Offset: 0x00017494
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x0001929C File Offset: 0x0001749C
		[DataSourceProperty]
		public bool IsInParty
		{
			get
			{
				return this._isInParty;
			}
			set
			{
				if (value != this._isInParty)
				{
					this._isInParty = value;
					base.OnPropertyChangedWithValue(value, "IsInParty");
				}
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x000192BA File Offset: 0x000174BA
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x000192C2 File Offset: 0x000174C2
		[DataSourceProperty]
		public MPLobbyPartyPlayerVM Player
		{
			get
			{
				return this._player;
			}
			set
			{
				if (value != this._player)
				{
					this._player = value;
					base.OnPropertyChangedWithValue<MPLobbyPartyPlayerVM>(value, "Player");
				}
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x000192E0 File Offset: 0x000174E0
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x000192E8 File Offset: 0x000174E8
		[DataSourceProperty]
		public MBBindingList<MPLobbyPartyPlayerVM> PartyFriends
		{
			get
			{
				return this._partyFriends;
			}
			set
			{
				if (value != this._partyFriends)
				{
					this._partyFriends = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyPartyPlayerVM>>(value, "PartyFriends");
				}
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00019306 File Offset: 0x00017506
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x0001930E File Offset: 0x0001750E
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> PlayerActions
		{
			get
			{
				return this._playerActions;
			}
			set
			{
				if (value != this._playerActions)
				{
					this._playerActions = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "PlayerActions");
				}
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0001932C File Offset: 0x0001752C
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x00019334 File Offset: 0x00017534
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00019357 File Offset: 0x00017557
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x0001935F File Offset: 0x0001755F
		[DataSourceProperty]
		public string InGameText
		{
			get
			{
				return this._inGameText;
			}
			set
			{
				if (value != this._inGameText)
				{
					this._inGameText = value;
					base.OnPropertyChangedWithValue<string>(value, "InGameText");
				}
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00019382 File Offset: 0x00017582
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x0001938A File Offset: 0x0001758A
		[DataSourceProperty]
		public string OnlineText
		{
			get
			{
				return this._onlineText;
			}
			set
			{
				if (value != this._onlineText)
				{
					this._onlineText = value;
					base.OnPropertyChangedWithValue<string>(value, "OnlineText");
				}
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x000193AD File Offset: 0x000175AD
		// (set) Token: 0x060007B8 RID: 1976 RVA: 0x000193B5 File Offset: 0x000175B5
		[DataSourceProperty]
		public string OfflineText
		{
			get
			{
				return this._offlineText;
			}
			set
			{
				if (value != this._offlineText)
				{
					this._offlineText = value;
					base.OnPropertyChangedWithValue<string>(value, "OfflineText");
				}
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x000193D8 File Offset: 0x000175D8
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x000193E0 File Offset: 0x000175E0
		[DataSourceProperty]
		public HintViewModel FriendListHint
		{
			get
			{
				return this._friendListHint;
			}
			set
			{
				if (value != this._friendListHint)
				{
					this._friendListHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FriendListHint");
				}
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x000193FE File Offset: 0x000175FE
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x00019406 File Offset: 0x00017606
		[DataSourceProperty]
		public MBBindingList<MPLobbyFriendServiceVM> FriendServices
		{
			get
			{
				return this._friendServices;
			}
			set
			{
				if (value != this._friendServices)
				{
					this._friendServices = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyFriendServiceVM>>(value, "FriendServices");
				}
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00019424 File Offset: 0x00017624
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x0001942C File Offset: 0x0001762C
		[DataSourceProperty]
		public MPLobbyFriendServiceVM ActiveService
		{
			get
			{
				return this._activeService;
			}
			set
			{
				if (value != this._activeService)
				{
					this._activeService = value;
					base.OnPropertyChangedWithValue<MPLobbyFriendServiceVM>(value, "ActiveService");
				}
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0001944A File Offset: 0x0001764A
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x00019452 File Offset: 0x00017652
		[DataSourceProperty]
		public int TotalOnlineFriendCount
		{
			get
			{
				return this._totalOnlineFriendCount;
			}
			set
			{
				if (value != this._totalOnlineFriendCount)
				{
					this._totalOnlineFriendCount = value;
					base.OnPropertyChangedWithValue(value, "TotalOnlineFriendCount");
				}
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00019470 File Offset: 0x00017670
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x00019478 File Offset: 0x00017678
		[DataSourceProperty]
		public int NotificationCount
		{
			get
			{
				return this._notificationCount;
			}
			set
			{
				if (value != this._notificationCount)
				{
					this._notificationCount = value;
					base.OnPropertyChangedWithValue(value, "NotificationCount");
					this.HasNotification = value > 0;
				}
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x000194A0 File Offset: 0x000176A0
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x000194A8 File Offset: 0x000176A8
		[DataSourceProperty]
		public bool HasNotification
		{
			get
			{
				return this._hasNotification;
			}
			set
			{
				if (value != this._hasNotification)
				{
					this._hasNotification = value;
					base.OnPropertyChangedWithValue(value, "HasNotification");
				}
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x000194C6 File Offset: 0x000176C6
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x000194CE File Offset: 0x000176CE
		[DataSourceProperty]
		public InputKeyItemVM ToggleInputKey
		{
			get
			{
				return this._toggleInputKey;
			}
			set
			{
				if (value != this._toggleInputKey)
				{
					this._toggleInputKey = value;
					base.OnPropertyChanged("ToggleInputKey");
				}
			}
		}

		// Token: 0x0400037A RID: 890
		private const string _inviteFailedSoundEvent = "event:/ui/notification/quest_update";

		// Token: 0x0400037B RID: 891
		private List<LobbyNotification> _activeNotifications;

		// Token: 0x0400037C RID: 892
		private int _activeServiceIndex;

		// Token: 0x0400037D RID: 893
		private bool _isEnabled;

		// Token: 0x0400037E RID: 894
		private bool _isListEnabled = true;

		// Token: 0x0400037F RID: 895
		private bool _isPartyAvailable;

		// Token: 0x04000380 RID: 896
		private bool _isPartyFull;

		// Token: 0x04000381 RID: 897
		private bool _isPlayerActionsActive;

		// Token: 0x04000382 RID: 898
		private bool _isInParty;

		// Token: 0x04000383 RID: 899
		private MPLobbyPartyPlayerVM _player;

		// Token: 0x04000384 RID: 900
		private MBBindingList<MPLobbyPartyPlayerVM> _partyFriends;

		// Token: 0x04000385 RID: 901
		private MBBindingList<StringPairItemWithActionVM> _playerActions;

		// Token: 0x04000386 RID: 902
		private string _titleText;

		// Token: 0x04000387 RID: 903
		private string _inGameText;

		// Token: 0x04000388 RID: 904
		private string _onlineText;

		// Token: 0x04000389 RID: 905
		private string _offlineText;

		// Token: 0x0400038A RID: 906
		private int _totalOnlineFriendCount;

		// Token: 0x0400038B RID: 907
		private int _notificationCount;

		// Token: 0x0400038C RID: 908
		private bool _hasNotification;

		// Token: 0x0400038D RID: 909
		private HintViewModel _friendListHint;

		// Token: 0x0400038E RID: 910
		private MBBindingList<MPLobbyFriendServiceVM> _friendServices;

		// Token: 0x0400038F RID: 911
		private MPLobbyFriendServiceVM _activeService;

		// Token: 0x04000390 RID: 912
		private InputKeyItemVM _toggleInputKey;
	}
}
