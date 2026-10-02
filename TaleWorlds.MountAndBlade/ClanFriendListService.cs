using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F1 RID: 497
	public class ClanFriendListService : IFriendListService
	{
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x00062B4F File Offset: 0x00060D4F
		bool IFriendListService.InGameStatusFetchable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001CFB RID: 7419 RVA: 0x00062B52 File Offset: 0x00060D52
		bool IFriendListService.AllowsFriendOperations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x00062B55 File Offset: 0x00060D55
		bool IFriendListService.CanInvitePlayersToPlatformSession
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001CFD RID: 7421 RVA: 0x00062B58 File Offset: 0x00060D58
		bool IFriendListService.IncludeInAllFriends
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x00062B5B File Offset: 0x00060D5B
		public ClanFriendListService()
		{
			this._clanPlayerInfos = new Dictionary<PlayerId, ClanPlayerInfo>();
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x00062B6E File Offset: 0x00060D6E
		string IFriendListService.GetServiceCodeName()
		{
			return "ClanFriends";
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x00062B75 File Offset: 0x00060D75
		TextObject IFriendListService.GetServiceLocalizedName()
		{
			return new TextObject("{=j4F7tTzy}Clan", null);
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x00062B82 File Offset: 0x00060D82
		FriendListServiceType IFriendListService.GetFriendListServiceType()
		{
			return FriendListServiceType.Clan;
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x00062B85 File Offset: 0x00060D85
		IEnumerable<PlayerId> IFriendListService.GetAllFriends()
		{
			return this._clanPlayerInfos.Keys;
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06001D03 RID: 7427 RVA: 0x00062B94 File Offset: 0x00060D94
		// (remove) Token: 0x06001D04 RID: 7428 RVA: 0x00062BCC File Offset: 0x00060DCC
		public event Action<PlayerId> OnUserStatusChanged;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001D05 RID: 7429 RVA: 0x00062C04 File Offset: 0x00060E04
		// (remove) Token: 0x06001D06 RID: 7430 RVA: 0x00062C3C File Offset: 0x00060E3C
		public event Action<PlayerId> OnFriendRemoved;

		// Token: 0x06001D07 RID: 7431 RVA: 0x00062C74 File Offset: 0x00060E74
		async Task<bool> IFriendListService.GetUserOnlineStatus(PlayerId providedId)
		{
			bool flag = false;
			ClanPlayerInfo clanPlayerInfo;
			this._clanPlayerInfos.TryGetValue(providedId, out clanPlayerInfo);
			if (clanPlayerInfo != null)
			{
				flag = clanPlayerInfo.State == AnotherPlayerState.InMultiplayerGame || clanPlayerInfo.State == AnotherPlayerState.AtLobby || clanPlayerInfo.State == AnotherPlayerState.InParty;
			}
			return await Task.FromResult<bool>(flag);
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x00062CC4 File Offset: 0x00060EC4
		async Task<bool> IFriendListService.IsPlayingThisGame(PlayerId providedId)
		{
			return await ((IFriendListService)this).GetUserOnlineStatus(providedId);
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x00062D14 File Offset: 0x00060F14
		async Task<string> IFriendListService.GetUserName(PlayerId providedId)
		{
			ClanPlayerInfo clanPlayerInfo;
			this._clanPlayerInfos.TryGetValue(providedId, out clanPlayerInfo);
			return await Task.FromResult<string>((clanPlayerInfo != null) ? clanPlayerInfo.PlayerName : null);
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x00062D64 File Offset: 0x00060F64
		public async Task<PlayerId> GetUserWithName(string name)
		{
			ClanPlayerInfo clanPlayerInfo = this._clanPlayerInfos.Values.FirstOrDefaultQ<ClanPlayerInfo>((ClanPlayerInfo playerInfo) => playerInfo.PlayerName == name);
			return await Task.FromResult<PlayerId>((clanPlayerInfo != null) ? clanPlayerInfo.PlayerId : PlayerId.Empty);
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06001D0B RID: 7435 RVA: 0x00062DB4 File Offset: 0x00060FB4
		// (remove) Token: 0x06001D0C RID: 7436 RVA: 0x00062DEC File Offset: 0x00060FEC
		public event Action OnFriendListChanged;

		// Token: 0x06001D0D RID: 7437 RVA: 0x00062E21 File Offset: 0x00061021
		public IEnumerable<PlayerId> GetPendingRequests()
		{
			return null;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x00062E24 File Offset: 0x00061024
		public IEnumerable<PlayerId> GetReceivedRequests()
		{
			return null;
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x00062E28 File Offset: 0x00061028
		private void Dummy()
		{
			if (this.OnUserStatusChanged != null)
			{
				this.OnUserStatusChanged(default(PlayerId));
			}
			if (this.OnFriendRemoved != null)
			{
				this.OnFriendRemoved(default(PlayerId));
			}
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x00062E70 File Offset: 0x00061070
		public void OnClanInfoChanged(List<ClanPlayerInfo> playerInfosInClan)
		{
			this._clanPlayerInfos.Clear();
			if (playerInfosInClan != null)
			{
				foreach (ClanPlayerInfo clanPlayerInfo in playerInfosInClan)
				{
					this._clanPlayerInfos.Add(clanPlayerInfo.PlayerId, clanPlayerInfo);
				}
			}
			Action onFriendListChanged = this.OnFriendListChanged;
			if (onFriendListChanged == null)
			{
				return;
			}
			onFriendListChanged();
		}

		// Token: 0x040009DF RID: 2527
		public const string CodeName = "ClanFriends";

		// Token: 0x040009E0 RID: 2528
		private readonly Dictionary<PlayerId, ClanPlayerInfo> _clanPlayerInfos;
	}
}
