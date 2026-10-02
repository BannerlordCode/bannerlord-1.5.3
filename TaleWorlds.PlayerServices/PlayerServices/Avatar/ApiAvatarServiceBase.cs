using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TaleWorlds.PlayerServices.Avatar
{
	// Token: 0x02000008 RID: 8
	public abstract class ApiAvatarServiceBase : IAvatarService
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002A0B File Offset: 0x00000C0B
		protected Dictionary<ulong, AvatarData> AvatarImageCache { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002A13 File Offset: 0x00000C13
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002A1B File Offset: 0x00000C1B
		[TupleElementNames(new string[] { "accountId", "avatarData" })]
		protected List<ValueTuple<ulong, AvatarData>> WaitingAccounts
		{
			[return: TupleElementNames(new string[] { "accountId", "avatarData" })]
			get;
			[param: TupleElementNames(new string[] { "accountId", "avatarData" })]
			set;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002A24 File Offset: 0x00000C24
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002A2C File Offset: 0x00000C2C
		[TupleElementNames(new string[] { "accountId", "avatarData" })]
		protected List<ValueTuple<ulong, AvatarData>> InProgressAccounts
		{
			[return: TupleElementNames(new string[] { "accountId", "avatarData" })]
			get;
			[param: TupleElementNames(new string[] { "accountId", "avatarData" })]
			set;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002A35 File Offset: 0x00000C35
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002A3D File Offset: 0x00000C3D
		protected Task FetchAvatarsTask { get; set; }

		// Token: 0x06000036 RID: 54 RVA: 0x00002A46 File Offset: 0x00000C46
		protected ApiAvatarServiceBase()
		{
			this.AvatarImageCache = new Dictionary<ulong, AvatarData>();
			this.WaitingAccounts = new List<ValueTuple<ulong, AvatarData>>();
			this.InProgressAccounts = new List<ValueTuple<ulong, AvatarData>>();
			this.FetchAvatarsTask = null;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A76 File Offset: 0x00000C76
		public void Tick(float dt)
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002A78 File Offset: 0x00000C78
		public AvatarData GetPlayerAvatar(PlayerId playerId)
		{
			ulong part = playerId.Part4;
			Dictionary<ulong, AvatarData> avatarImageCache = this.AvatarImageCache;
			AvatarData avatarData;
			lock (avatarImageCache)
			{
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
			}
			List<ValueTuple<ulong, AvatarData>> waitingAccounts = this.WaitingAccounts;
			lock (waitingAccounts)
			{
				this.WaitingAccounts.Add(new ValueTuple<ulong, AvatarData>(part, avatarData));
			}
			this.CheckWaitingAccounts();
			return avatarData;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002B50 File Offset: 0x00000D50
		private void CheckWaitingAccounts()
		{
			List<ValueTuple<ulong, AvatarData>> waitingAccounts = this.WaitingAccounts;
			lock (waitingAccounts)
			{
				if (this.FetchAvatarsTask == null || this.FetchAvatarsTask.IsCompleted)
				{
					Task fetchAvatarsTask = this.FetchAvatarsTask;
					if (fetchAvatarsTask != null && fetchAvatarsTask.IsFaulted)
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
					if (this.WaitingAccounts.Count > 0)
					{
						this.FetchAvatarsTask = this.FetchAvatars();
						Task.Run(async delegate
						{
							await this.FetchAvatarsTask;
							this.CheckWaitingAccounts();
						});
					}
				}
			}
		}

		// Token: 0x0600003A RID: 58
		protected abstract Task FetchAvatars();

		// Token: 0x0600003B RID: 59 RVA: 0x00002C5C File Offset: 0x00000E5C
		public void Initialize()
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002C60 File Offset: 0x00000E60
		public void ClearCache()
		{
			Dictionary<ulong, AvatarData> avatarImageCache = this.AvatarImageCache;
			lock (avatarImageCache)
			{
				this.AvatarImageCache.Clear();
			}
			List<ValueTuple<ulong, AvatarData>> waitingAccounts = this.WaitingAccounts;
			lock (waitingAccounts)
			{
				this.WaitingAccounts.Clear();
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002CDC File Offset: 0x00000EDC
		public bool IsInitialized()
		{
			return true;
		}
	}
}
