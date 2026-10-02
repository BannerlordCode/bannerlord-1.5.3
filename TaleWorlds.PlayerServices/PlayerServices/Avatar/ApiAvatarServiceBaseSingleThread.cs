using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TaleWorlds.PlayerServices.Avatar
{
	// Token: 0x02000009 RID: 9
	public abstract class ApiAvatarServiceBaseSingleThread : IAvatarService
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002D25 File Offset: 0x00000F25
		protected Dictionary<ulong, AvatarData> AvatarImageCache { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002D2D File Offset: 0x00000F2D
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002D35 File Offset: 0x00000F35
		[TupleElementNames(new string[] { "accountId", "avatarData" })]
		protected List<ValueTuple<ulong, AvatarData>> WaitingAccounts
		{
			[return: TupleElementNames(new string[] { "accountId", "avatarData" })]
			get;
			[param: TupleElementNames(new string[] { "accountId", "avatarData" })]
			set;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002D3E File Offset: 0x00000F3E
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002D46 File Offset: 0x00000F46
		[TupleElementNames(new string[] { "accountId", "avatarData" })]
		protected List<ValueTuple<ulong, AvatarData>> InProgressAccounts
		{
			[return: TupleElementNames(new string[] { "accountId", "avatarData" })]
			get;
			[param: TupleElementNames(new string[] { "accountId", "avatarData" })]
			set;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002D4F File Offset: 0x00000F4F
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002D57 File Offset: 0x00000F57
		protected Task FetchAvatarsTask { get; set; }

		// Token: 0x06000046 RID: 70 RVA: 0x00002D60 File Offset: 0x00000F60
		protected ApiAvatarServiceBaseSingleThread()
		{
			this.AvatarImageCache = new Dictionary<ulong, AvatarData>();
			this.WaitingAccounts = new List<ValueTuple<ulong, AvatarData>>();
			this.InProgressAccounts = new List<ValueTuple<ulong, AvatarData>>();
			this.FetchAvatarsTask = null;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002D90 File Offset: 0x00000F90
		public void Tick(float dt)
		{
			this.CheckWaitingAccounts();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002D98 File Offset: 0x00000F98
		public AvatarData GetPlayerAvatar(PlayerId playerId)
		{
			ulong part = playerId.Part4;
			AvatarData avatarData;
			if (this.AvatarImageCache.TryGetValue(part, out avatarData) && avatarData.Status != AvatarData.DataStatus.Failed)
			{
				return avatarData;
			}
			if (this.AvatarImageCache.Count > 300)
			{
				this.AvatarImageCache.Clear();
			}
			avatarData = new AvatarData();
			this.AvatarImageCache[part] = avatarData;
			this.WaitingAccounts.Add(new ValueTuple<ulong, AvatarData>(part, avatarData));
			return avatarData;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002E0C File Offset: 0x0000100C
		private async void CheckWaitingAccounts()
		{
			if (this.FetchAvatarsTask == null && this.WaitingAccounts.Count > 0)
			{
				this.FetchAvatarsTask = this.FetchAvatars();
				await this.FetchAvatarsTask;
				if (this.FetchAvatarsTask.IsFaulted)
				{
					this.FetchAvatarsTask = null;
					foreach (ValueTuple<ulong, AvatarData> valueTuple in this.InProgressAccounts)
					{
						AvatarData item = valueTuple.Item2;
						if (item.Status == AvatarData.DataStatus.NotReady)
						{
							item.SetFailed();
						}
					}
				}
				if (this.FetchAvatarsTask != null)
				{
					this.FetchAvatarsTask.Dispose();
					this.FetchAvatarsTask = null;
				}
				this.InProgressAccounts.Clear();
			}
		}

		// Token: 0x0600004A RID: 74
		protected abstract Task FetchAvatars();

		// Token: 0x0600004B RID: 75 RVA: 0x00002E45 File Offset: 0x00001045
		public void Initialize()
		{
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002E47 File Offset: 0x00001047
		public void ClearCache()
		{
			this.AvatarImageCache.Clear();
			this.WaitingAccounts.Clear();
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002E5F File Offset: 0x0000105F
		public bool IsInitialized()
		{
			return true;
		}
	}
}
