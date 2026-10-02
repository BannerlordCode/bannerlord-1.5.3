using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000053 RID: 83
	public class MPLobbyFriendGroupVM : ViewModel
	{
		// Token: 0x06000729 RID: 1833 RVA: 0x00016B0B File Offset: 0x00014D0B
		public MPLobbyFriendGroupVM(MPLobbyFriendGroupVM.FriendGroupType groupType)
		{
			this.GroupType = groupType;
			this._friendOperationQueue = new List<MPLobbyFriendGroupVM.FriendOperation>();
			this.FriendList = new MBBindingList<MPLobbyFriendItemVM>();
			this.RefreshValues();
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00016B38 File Offset: 0x00014D38
		public override void RefreshValues()
		{
			base.RefreshValues();
			switch (this.GroupType)
			{
			case MPLobbyFriendGroupVM.FriendGroupType.InGame:
				this.Title = new TextObject("{=uUoSmCBS}In Bannerlord", null).ToString();
				break;
			case MPLobbyFriendGroupVM.FriendGroupType.Online:
				this.Title = new TextObject("{=V305MaOP}Online", null).ToString();
				break;
			case MPLobbyFriendGroupVM.FriendGroupType.Offline:
				this.Title = new TextObject("{=Zv1lg272}Offline", null).ToString();
				break;
			case MPLobbyFriendGroupVM.FriendGroupType.FriendRequests:
				this.Title = new TextObject("{=K8CGzQYL}Received Requests", null).ToString();
				break;
			case MPLobbyFriendGroupVM.FriendGroupType.PendingRequests:
				this.Title = new TextObject("{=QwbVdMLi}Sent Requests", null).ToString();
				break;
			}
			this.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00016C10 File Offset: 0x00014E10
		public void Tick()
		{
			List<MPLobbyFriendGroupVM.FriendOperation> friendOperationQueue = this._friendOperationQueue;
			lock (friendOperationQueue)
			{
				for (int i = 0; i < this._friendOperationQueue.Count; i++)
				{
					this.HandleFriendOperationAux(this._friendOperationQueue[i]);
				}
				this._friendOperationQueue.Clear();
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00016C80 File Offset: 0x00014E80
		private void HandleFriendOperationAux(MPLobbyFriendGroupVM.FriendOperation operation)
		{
			switch (operation.Type)
			{
			case MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Add:
				this.FriendList.Add(operation.Friend);
				operation.Friend.UpdateNameAndAvatar(false);
				return;
			case MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Remove:
				this.FriendList.Remove(operation.Friend);
				return;
			case MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Clear:
				this.FriendList.Clear();
				return;
			default:
				return;
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00016CE4 File Offset: 0x00014EE4
		private void EnqueueFriendOperation(MPLobbyFriendGroupVM.FriendOperation operation)
		{
			List<MPLobbyFriendGroupVM.FriendOperation> friendOperationQueue = this._friendOperationQueue;
			lock (friendOperationQueue)
			{
				this._friendOperationQueue.Add(operation);
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00016D2C File Offset: 0x00014F2C
		public void ClearFriends()
		{
			this.EnqueueFriendOperation(new MPLobbyFriendGroupVM.FriendOperation(MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Clear, null));
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00016D3B File Offset: 0x00014F3B
		public void AddFriend(MPLobbyFriendItemVM player)
		{
			if (player.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				this.EnqueueFriendOperation(new MPLobbyFriendGroupVM.FriendOperation(MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Add, player));
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00016D61 File Offset: 0x00014F61
		public void RemoveFriend(MPLobbyFriendItemVM player)
		{
			if (player.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				this.EnqueueFriendOperation(new MPLobbyFriendGroupVM.FriendOperation(MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Remove, player));
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00016D87 File Offset: 0x00014F87
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x00016D8F File Offset: 0x00014F8F
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00016DB2 File Offset: 0x00014FB2
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x00016DBA File Offset: 0x00014FBA
		[DataSourceProperty]
		public MPLobbyFriendGroupVM.FriendGroupType GroupType
		{
			get
			{
				return this._groupType;
			}
			set
			{
				if (value != this._groupType)
				{
					this._groupType = value;
					base.OnPropertyChanged("GroupType");
				}
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00016DD7 File Offset: 0x00014FD7
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00016DDF File Offset: 0x00014FDF
		[DataSourceProperty]
		public MBBindingList<MPLobbyFriendItemVM> FriendList
		{
			get
			{
				return this._friendList;
			}
			set
			{
				if (value != this._friendList)
				{
					this._friendList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyFriendItemVM>>(value, "FriendList");
				}
			}
		}

		// Token: 0x04000358 RID: 856
		private readonly List<MPLobbyFriendGroupVM.FriendOperation> _friendOperationQueue;

		// Token: 0x04000359 RID: 857
		private string _title;

		// Token: 0x0400035A RID: 858
		private MPLobbyFriendGroupVM.FriendGroupType _groupType;

		// Token: 0x0400035B RID: 859
		private MBBindingList<MPLobbyFriendItemVM> _friendList;

		// Token: 0x02000107 RID: 263
		private readonly struct FriendOperation
		{
			// Token: 0x06001235 RID: 4661 RVA: 0x000397D8 File Offset: 0x000379D8
			public FriendOperation(MPLobbyFriendGroupVM.FriendOperation.OperationTypes type, MPLobbyFriendItemVM friend)
			{
				this.Type = type;
				this.Friend = friend;
			}

			// Token: 0x0400091F RID: 2335
			public readonly MPLobbyFriendItemVM Friend;

			// Token: 0x04000920 RID: 2336
			public readonly MPLobbyFriendGroupVM.FriendOperation.OperationTypes Type;

			// Token: 0x020001A5 RID: 421
			public enum OperationTypes
			{
				// Token: 0x04000AEB RID: 2795
				Add,
				// Token: 0x04000AEC RID: 2796
				Remove,
				// Token: 0x04000AED RID: 2797
				Clear
			}
		}

		// Token: 0x02000108 RID: 264
		public enum FriendGroupType
		{
			// Token: 0x04000922 RID: 2338
			InGame,
			// Token: 0x04000923 RID: 2339
			Online,
			// Token: 0x04000924 RID: 2340
			Offline,
			// Token: 0x04000925 RID: 2341
			FriendRequests,
			// Token: 0x04000926 RID: 2342
			PendingRequests
		}
	}
}
