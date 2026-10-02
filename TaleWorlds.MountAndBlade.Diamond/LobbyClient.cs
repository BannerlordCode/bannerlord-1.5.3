using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Messages.FromClient.ToLobbyServer;
using Messages.FromLobbyServer.ToClient;
using TaleWorlds.Core;
using TaleWorlds.Diamond;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000127 RID: 295
	public class LobbyClient : Client
	{
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00008A54 File Offset: 0x00006C54
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x00008A5B File Offset: 0x00006C5B
		private static int FriendListCheckDelay
		{
			get
			{
				return LobbyClient._friendListCheckDelay;
			}
			set
			{
				if (value != LobbyClient._friendListCheckDelay)
				{
					LobbyClient._friendListCheckDelay = value;
				}
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00008A6B File Offset: 0x00006C6B
		// (set) Token: 0x060006B3 RID: 1715 RVA: 0x00008A73 File Offset: 0x00006C73
		public PlayerData PlayerData { get; private set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x00008A7C File Offset: 0x00006C7C
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x00008A84 File Offset: 0x00006C84
		public SupportedFeatures SupportedFeatures { get; private set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00008A8D File Offset: 0x00006C8D
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x00008A95 File Offset: 0x00006C95
		public ClanInfo ClanInfo { get; private set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00008A9E File Offset: 0x00006C9E
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00008AA6 File Offset: 0x00006CA6
		public ClanHomeInfo ClanHomeInfo { get; private set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00008AAF File Offset: 0x00006CAF
		public IReadOnlyList<string> OwnedCosmetics
		{
			get
			{
				return this._ownedCosmetics;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x00008AB7 File Offset: 0x00006CB7
		public IReadOnlyDictionary<string, List<string>> UsedCosmetics
		{
			get
			{
				return this._usedCosmetics;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00008ABF File Offset: 0x00006CBF
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x00008AC7 File Offset: 0x00006CC7
		public AvailableScenes AvailableScenes { get; private set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x00008AD0 File Offset: 0x00006CD0
		public PlayerId PlayerID
		{
			get
			{
				return this._playerId;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x00008AD8 File Offset: 0x00006CD8
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x00008AE0 File Offset: 0x00006CE0
		public bool IsRefreshingPlayerData { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00008AE9 File Offset: 0x00006CE9
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x00008AF4 File Offset: 0x00006CF4
		public LobbyClient.State CurrentState
		{
			get
			{
				return this._state;
			}
			private set
			{
				if (this._state != value)
				{
					LobbyClient.State state = this._state;
					this._state = value;
					ILobbyClientSessionHandler handler = this._handler;
					if (handler == null)
					{
						return;
					}
					handler.OnGameClientStateChange(state);
				}
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00008B2C File Offset: 0x00006D2C
		public override int AliveCheckTimeInMilliSeconds
		{
			get
			{
				switch (this.CurrentState)
				{
				case LobbyClient.State.Idle:
				case LobbyClient.State.Working:
				case LobbyClient.State.Connected:
				case LobbyClient.State.SessionRequested:
				case LobbyClient.State.AtLobby:
					return 6000;
				case LobbyClient.State.SearchingToRejoinBattle:
				case LobbyClient.State.RequestingToSearchBattle:
				case LobbyClient.State.RequestingToCancelSearchBattle:
				case LobbyClient.State.SearchingBattle:
				case LobbyClient.State.QuittingFromBattle:
				case LobbyClient.State.WaitingToRegisterCustomGame:
				case LobbyClient.State.HostingCustomGame:
				case LobbyClient.State.WaitingToJoinCustomGame:
					return 3500;
				case LobbyClient.State.AtBattle:
				case LobbyClient.State.InCustomGame:
					return 60000;
				}
				return 1000;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00008BA3 File Offset: 0x00006DA3
		public bool AtLobby
		{
			get
			{
				return this.CurrentState == LobbyClient.State.AtLobby;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00008BAE File Offset: 0x00006DAE
		public bool CanPerformLobbyActions
		{
			get
			{
				return this.CurrentState == LobbyClient.State.AtLobby || this.CurrentState == LobbyClient.State.RequestingToSearchBattle || this.CurrentState == LobbyClient.State.SearchingBattle || this.CurrentState == LobbyClient.State.WaitingToJoinCustomGame;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00008BD7 File Offset: 0x00006DD7
		public string Name
		{
			get
			{
				return this._userName;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00008BDF File Offset: 0x00006DDF
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x00008BE7 File Offset: 0x00006DE7
		public string LastBattleServerAddressForClient { get; private set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00008BF0 File Offset: 0x00006DF0
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x00008BF8 File Offset: 0x00006DF8
		public ushort LastBattleServerPortForClient { get; private set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00008C01 File Offset: 0x00006E01
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x00008C09 File Offset: 0x00006E09
		public bool LastBattleIsOfficial { get; private set; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00008C12 File Offset: 0x00006E12
		public bool Connected
		{
			get
			{
				return this.CurrentState != LobbyClient.State.Working && this.CurrentState > LobbyClient.State.Idle;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00008C28 File Offset: 0x00006E28
		public bool IsIdle
		{
			get
			{
				return this.CurrentState == LobbyClient.State.Idle;
			}
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00008C33 File Offset: 0x00006E33
		public void Logout(TextObject logOutReason)
		{
			base.BeginDisconnect();
			this._logOutReason = logOutReason;
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00008C42 File Offset: 0x00006E42
		public bool LoggedIn
		{
			get
			{
				return this.CurrentState != LobbyClient.State.Idle && this.CurrentState != LobbyClient.State.Working && this.CurrentState != LobbyClient.State.Connected && this.CurrentState != LobbyClient.State.SessionRequested;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00008C6C File Offset: 0x00006E6C
		public bool IsInGame
		{
			get
			{
				return this.CurrentState == LobbyClient.State.AtBattle || this.CurrentState == LobbyClient.State.HostingCustomGame || this.CurrentState == LobbyClient.State.InCustomGame;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00008C8E File Offset: 0x00006E8E
		public bool IsHostingCustomGame
		{
			get
			{
				return this._state == LobbyClient.State.HostingCustomGame;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00008C9A File Offset: 0x00006E9A
		public bool IsMatchmakingAvailable
		{
			get
			{
				ServerStatus serverStatus = this._serverStatus;
				return serverStatus != null && serverStatus.IsMatchmakingEnabled;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00008CAD File Offset: 0x00006EAD
		public bool IsAbleToSearchForGame
		{
			get
			{
				return this.IsMatchmakingAvailable && this._matchmakerBlockedTime <= DateTime.Now;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00008CC9 File Offset: 0x00006EC9
		public bool PartySystemAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x00008CCC File Offset: 0x00006ECC
		public bool IsCustomBattleAvailable
		{
			get
			{
				ServerStatus serverStatus = this._serverStatus;
				return serverStatus != null && serverStatus.IsCustomBattleEnabled;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00008CDF File Offset: 0x00006EDF
		public IReadOnlyList<ModuleInfoModel> LoadedUnofficialModules
		{
			get
			{
				return this._loadedUnofficialModules;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00008CE7 File Offset: 0x00006EE7
		public bool HasUnofficialModulesLoaded
		{
			get
			{
				return this.LoadedUnofficialModules.Count > 0;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00008CF7 File Offset: 0x00006EF7
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00008CFF File Offset: 0x00006EFF
		public bool HasUserGeneratedContentPrivilege { get; private set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00008D08 File Offset: 0x00006F08
		public bool IsPartyLeader
		{
			get
			{
				if (this.Connected)
				{
					object obj = true;
					PartyPlayerInLobbyClient partyPlayerInLobbyClient = this.PlayersInParty.Find((PartyPlayerInLobbyClient p) => p.PlayerId == this._playerId);
					return object.Equals(obj, (partyPlayerInLobbyClient != null) ? new bool?(partyPlayerInLobbyClient.IsPartyLeader) : null);
				}
				return false;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x00008D5F File Offset: 0x00006F5F
		public bool IsClanLeader
		{
			get
			{
				ClanPlayer clanPlayer = this.PlayersInClan.Find((ClanPlayer p) => p.PlayerId == this._playerId);
				return clanPlayer != null && clanPlayer.Role == ClanPlayerRole.Leader;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00008D86 File Offset: 0x00006F86
		public bool IsClanOfficer
		{
			get
			{
				ClanPlayer clanPlayer = this.PlayersInClan.Find((ClanPlayer p) => p.PlayerId == this._playerId);
				return clanPlayer != null && clanPlayer.Role == ClanPlayerRole.Officer;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x00008DAD File Offset: 0x00006FAD
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x00008DB5 File Offset: 0x00006FB5
		public bool IsEligibleToCreatePremadeGame { get; private set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x00008DBE File Offset: 0x00006FBE
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x00008DC6 File Offset: 0x00006FC6
		public CustomBattleId CustomBattleId { get; private set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00008DCF File Offset: 0x00006FCF
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x00008DD7 File Offset: 0x00006FD7
		public string CustomGameType { get; private set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00008DE0 File Offset: 0x00006FE0
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x00008DE8 File Offset: 0x00006FE8
		public string CustomGameScene { get; private set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00008DF1 File Offset: 0x00006FF1
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x00008DF9 File Offset: 0x00006FF9
		public AvailableCustomGames AvailableCustomGames { get; private set; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00008E02 File Offset: 0x00007002
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x00008E0A File Offset: 0x0000700A
		public PremadeGameList AvailablePremadeGames { get; private set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00008E13 File Offset: 0x00007013
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x00008E1B File Offset: 0x0000701B
		public List<PartyPlayerInLobbyClient> PlayersInParty { get; private set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00008E24 File Offset: 0x00007024
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x00008E2C File Offset: 0x0000702C
		public List<ClanPlayer> PlayersInClan { get; private set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x00008E35 File Offset: 0x00007035
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x00008E3D File Offset: 0x0000703D
		public List<ClanPlayerInfo> PlayerInfosInClan { get; private set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00008E46 File Offset: 0x00007046
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x00008E4E File Offset: 0x0000704E
		public FriendInfo[] FriendInfos { get; private set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00008E57 File Offset: 0x00007057
		public bool IsInParty
		{
			get
			{
				return this.Connected && this.PlayersInParty.Count > 0;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x00008E71 File Offset: 0x00007071
		public bool IsPartyFull
		{
			get
			{
				return this.PlayersInParty.Count == Parameters.MaxPlayerCountInParty;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00008E85 File Offset: 0x00007085
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x00008E8D File Offset: 0x0000708D
		public string CurrentMatchId { get; private set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00008E96 File Offset: 0x00007096
		public bool IsInClan
		{
			get
			{
				return this.PlayersInClan.Count > 0;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00008EA6 File Offset: 0x000070A6
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x00008EAE File Offset: 0x000070AE
		public bool IsPartyInvitationPopupActive { get; private set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00008EB7 File Offset: 0x000070B7
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x00008EBF File Offset: 0x000070BF
		public bool IsPartyJoinRequestPopupActive { get; private set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x00008EC8 File Offset: 0x000070C8
		public bool CanInvitePlayers
		{
			get
			{
				SupportedFeatures supportedFeatures = this.SupportedFeatures;
				return supportedFeatures != null && supportedFeatures.SupportsFeatures(Features.Party) && (!this.IsInParty || this.IsPartyLeader);
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x00008EF1 File Offset: 0x000070F1
		public bool CanSuggestPlayers
		{
			get
			{
				SupportedFeatures supportedFeatures = this.SupportedFeatures;
				return supportedFeatures != null && supportedFeatures.SupportsFeatures(Features.Party) && this.IsInParty && !this.IsPartyLeader;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x00008F1B File Offset: 0x0000711B
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x00008F23 File Offset: 0x00007123
		public Guid ClanID { get; private set; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x00008F2C File Offset: 0x0000712C
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x00008F34 File Offset: 0x00007134
		public List<PlayerId> FriendIDs { get; private set; }

		// Token: 0x06000701 RID: 1793 RVA: 0x00008F40 File Offset: 0x00007140
		public LobbyClient(DiamondClientApplication diamondClientApplication, IClientSessionFactory sessionProvider)
			: base(diamondClientApplication, sessionProvider, false)
		{
			this._serverStatusTimer = new Stopwatch();
			this._serverStatusTimer.Start();
			this._matchmakerBlockedTime = DateTime.MinValue;
			this._friendListTimer = new Stopwatch();
			this._friendListTimer.Start();
			this._recentPlayersTimer = new Stopwatch();
			this._recentPlayersTimer.Start();
			this.PlayersInParty = new List<PartyPlayerInLobbyClient>();
			this.PlayersInClan = new List<ClanPlayer>();
			this.PlayerInfosInClan = new List<ClanPlayerInfo>();
			this.FriendInfos = new FriendInfo[0];
			this.ClanID = Guid.Empty;
			this.FriendIDs = new List<PlayerId>();
			this.SupportedFeatures = new SupportedFeatures();
			this._ownedCosmetics = new List<string>();
			this._usedCosmetics = new Dictionary<string, List<string>>();
			this._cachedRankInfos = new TimedDictionaryCache<PlayerId, GameTypeRankInfo[]>(TimeSpan.FromSeconds(10.0));
			this._cachedPlayerStats = new TimedDictionaryCache<PlayerId, PlayerStatsBase[]>(TimeSpan.FromSeconds(10.0));
			this._cachedPlayerDatas = new TimedDictionaryCache<PlayerId, PlayerData>(TimeSpan.FromSeconds(10.0));
			this._cachedPlayerBannerlordIDs = new TimedDictionaryCache<PlayerId, string>(TimeSpan.FromSeconds(30.0));
			this._pendingPlayerRequests = new Dictionary<ValueTuple<LobbyClient.PendingRequest, PlayerId>, Task>();
			base.AddMessageHandler<FindGameAnswerMessage>(new ClientMessageHandler<FindGameAnswerMessage>(this.OnFindGameAnswerMessage));
			base.AddMessageHandler<JoinBattleMessage>(new ClientMessageHandler<JoinBattleMessage>(this.OnJoinBattleMessage));
			base.AddMessageHandler<BattleResultMessage>(new ClientMessageHandler<BattleResultMessage>(this.OnBattleResultMessage));
			base.AddMessageHandler<BattleServerLostMessage>(new ClientMessageHandler<BattleServerLostMessage>(this.OnBattleServerLostMessage));
			base.AddMessageHandler<BattleOverMessage>(new ClientMessageHandler<BattleOverMessage>(this.OnBattleOverMessage));
			base.AddMessageHandler<CancelBattleResponseMessage>(new ClientMessageHandler<CancelBattleResponseMessage>(this.OnCancelBattleResponseMessage));
			base.AddMessageHandler<RejoinRequestRejectedMessage>(new ClientMessageHandler<RejoinRequestRejectedMessage>(this.OnRejoinRequestRejectedMessage));
			base.AddMessageHandler<CancelFindGameMessage>(new ClientMessageHandler<CancelFindGameMessage>(this.OnCancelFindGameMessage));
			base.AddMessageHandler<RequestJoinPartyMessage>(new ClientMessageHandler<RequestJoinPartyMessage>(this.OnRequestJoinPartyMessage));
			base.AddMessageHandler<WhisperReceivedMessage>(new ClientMessageHandler<WhisperReceivedMessage>(this.OnWhisperMessageReceivedMessage));
			base.AddMessageHandler<ClanMessageReceivedMessage>(new ClientMessageHandler<ClanMessageReceivedMessage>(this.OnClanMessageReceivedMessage));
			base.AddMessageHandler<PartyMessageReceivedMessage>(new ClientMessageHandler<PartyMessageReceivedMessage>(this.OnPartyMessageReceivedMessage));
			base.AddMessageHandler<SystemMessage>(new ClientMessageHandler<SystemMessage>(this.OnSystemMessage));
			base.AddMessageHandler<InvitationToPartyMessage>(new ClientMessageHandler<InvitationToPartyMessage>(this.OnInvitationToPartyMessage));
			base.AddMessageHandler<PartyInvitationInvalidMessage>(new ClientMessageHandler<PartyInvitationInvalidMessage>(this.OnPartyInvitationInvalidMessage));
			base.AddMessageHandler<UpdatePlayerDataMessage>(new ClientMessageHandler<UpdatePlayerDataMessage>(this.OnUpdatePlayerDataMessage));
			base.AddMessageHandler<RecentPlayerStatusesMessage>(new ClientMessageHandler<RecentPlayerStatusesMessage>(this.OnRecentPlayerStatusesMessage));
			base.AddMessageHandler<PlayerQuitFromMatchmakerGameResult>(new ClientMessageHandler<PlayerQuitFromMatchmakerGameResult>(this.OnPlayerQuitFromMatchmakerGameResult));
			base.AddMessageHandler<PlayerRemovedFromMatchmakerGame>(new ClientMessageHandler<PlayerRemovedFromMatchmakerGame>(this.OnPlayerRemovedFromMatchmakerGameMessage));
			base.AddMessageHandler<EnterBattleWithPartyAnswer>(new ClientMessageHandler<EnterBattleWithPartyAnswer>(this.OnEnterBattleWithPartyAnswerMessage));
			base.AddMessageHandler<JoinCustomGameResultMessage>(new ClientMessageHandler<JoinCustomGameResultMessage>(this.OnJoinCustomGameResultMessage));
			base.AddMessageHandler<ClientWantsToConnectCustomGameMessage>(new ClientMessageHandler<ClientWantsToConnectCustomGameMessage>(this.OnClientWantsToConnectCustomGameMessage));
			base.AddMessageHandler<ClientQuitFromCustomGameMessage>(new ClientMessageHandler<ClientQuitFromCustomGameMessage>(this.OnClientQuitFromCustomGameMessage));
			base.AddMessageHandler<PlayerRemovedFromCustomGame>(new ClientMessageHandler<PlayerRemovedFromCustomGame>(this.OnPlayerRemovedFromCustomGame));
			base.AddMessageHandler<EnterCustomBattleWithPartyAnswer>(new ClientMessageHandler<EnterCustomBattleWithPartyAnswer>(this.OnEnterCustomBattleWithPartyAnswerMessage));
			base.AddMessageHandler<PlayerInvitedToPartyMessage>(new ClientMessageHandler<PlayerInvitedToPartyMessage>(this.OnPlayerInvitedToPartyMessage));
			base.AddMessageHandler<PlayersAddedToPartyMessage>(new ClientMessageHandler<PlayersAddedToPartyMessage>(this.OnPlayerAddedToPartyMessage));
			base.AddMessageHandler<PlayerRemovedFromPartyMessage>(new ClientMessageHandler<PlayerRemovedFromPartyMessage>(this.OnPlayerRemovedFromPartyMessage));
			base.AddMessageHandler<PlayerAssignedPartyLeaderMessage>(new ClientMessageHandler<PlayerAssignedPartyLeaderMessage>(this.OnPlayerAssignedPartyLeaderMessage));
			base.AddMessageHandler<PlayerSuggestedToPartyMessage>(new ClientMessageHandler<PlayerSuggestedToPartyMessage>(this.OnPlayerSuggestedToPartyMessage));
			base.AddMessageHandler<ServerStatusMessage>(new ClientMessageHandler<ServerStatusMessage>(this.OnServerStatusMessage));
			base.AddMessageHandler<MatchmakerDisabledMessage>(new ClientMessageHandler<MatchmakerDisabledMessage>(this.OnMatchmakerDisabledMessage));
			base.AddMessageHandler<FriendListMessage>(new ClientMessageHandler<FriendListMessage>(this.OnFriendListMessage));
			base.AddMessageHandler<AdminMessage>(new ClientMessageHandler<AdminMessage>(this.OnAdminMessage));
			base.AddMessageHandler<CreateClanAnswerMessage>(new ClientMessageHandler<CreateClanAnswerMessage>(this.OnCreateClanAnswerMessage));
			base.AddMessageHandler<ClanCreationRequestMessage>(new ClientMessageHandler<ClanCreationRequestMessage>(this.OnClanCreationRequestMessage));
			base.AddMessageHandler<ClanCreationRequestAnsweredMessage>(new ClientMessageHandler<ClanCreationRequestAnsweredMessage>(this.OnClanCreationRequestAnsweredMessage));
			base.AddMessageHandler<ClanCreationFailedMessage>(new ClientMessageHandler<ClanCreationFailedMessage>(this.OnClanCreationFailedMessage));
			base.AddMessageHandler<ClanCreationSuccessfulMessage>(new ClientMessageHandler<ClanCreationSuccessfulMessage>(this.OnClanCreationSuccessfulMessage));
			base.AddMessageHandler<ClanInfoChangedMessage>(new ClientMessageHandler<ClanInfoChangedMessage>(this.OnClanInfoChangedMessage));
			base.AddMessageHandler<InvitationToClanMessage>(new ClientMessageHandler<InvitationToClanMessage>(this.OnInvitationToClanMessage));
			base.AddMessageHandler<ClanDisbandedMessage>(new ClientMessageHandler<ClanDisbandedMessage>(this.OnClanDisbandedMessage));
			base.AddMessageHandler<KickedFromClanMessage>(new ClientMessageHandler<KickedFromClanMessage>(this.OnKickedFromClan));
			base.AddMessageHandler<PartyPlayerLeftClanMessage>(new ClientMessageHandler<PartyPlayerLeftClanMessage>(this.OnPartyPlayerLeftClan));
			base.AddMessageHandler<JoinPremadeGameAnswerMessage>(new ClientMessageHandler<JoinPremadeGameAnswerMessage>(this.OnJoinPremadeGameAnswerMessage));
			base.AddMessageHandler<PremadeGameEligibilityStatusMessage>(new ClientMessageHandler<PremadeGameEligibilityStatusMessage>(this.OnPremadeGameEligibilityStatusMessage));
			base.AddMessageHandler<CreatePremadeGameAnswerMessage>(new ClientMessageHandler<CreatePremadeGameAnswerMessage>(this.OnCreatePremadeGameAnswerMessage));
			base.AddMessageHandler<JoinPremadeGameRequestMessage>(new ClientMessageHandler<JoinPremadeGameRequestMessage>(this.OnJoinPremadeGameRequestMessage));
			base.AddMessageHandler<JoinPremadeGameRequestResultMessage>(new ClientMessageHandler<JoinPremadeGameRequestResultMessage>(this.OnJoinPremadeGameRequestResultMessage));
			base.AddMessageHandler<ClanGameCreationCancelledMessage>(new ClientMessageHandler<ClanGameCreationCancelledMessage>(this.OnClanGameCreationCancelledMessage));
			base.AddMessageHandler<SigilChangeAnswerMessage>(new ClientMessageHandler<SigilChangeAnswerMessage>(this.OnSigilChangeAnswerMessage));
			base.AddMessageHandler<LobbyNotificationsMessage>(new ClientMessageHandler<LobbyNotificationsMessage>(this.OnLobbyNotificationsMessage));
			base.AddMessageHandler<CustomBattleOverMessage>(new ClientMessageHandler<CustomBattleOverMessage>(this.OnCustomBattleOverMessage));
			base.AddMessageHandler<RejoinBattleRequestAnswerMessage>(new ClientMessageHandler<RejoinBattleRequestAnswerMessage>(this.OnRejoinBattleRequestAnswerMessage));
			base.AddMessageHandler<PendingBattleRejoinMessage>(new ClientMessageHandler<PendingBattleRejoinMessage>(this.OnPendingBattleRejoinMessage));
			base.AddMessageHandler<ShowAnnouncementMessage>(new ClientMessageHandler<ShowAnnouncementMessage>(this.OnShowAnnouncementMessage));
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00009468 File Offset: 0x00007668
		public void SetLoadedModules(string[] moduleIDs)
		{
			if (this._loadedUnofficialModules == null)
			{
				this._loadedUnofficialModules = new List<ModuleInfoModel>();
				try
				{
					using (List<ModuleInfo>.Enumerator enumerator = ModuleHelper.GetSortedModules(moduleIDs).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ModuleInfoModel moduleInfoModel;
							if (ModuleInfoModel.TryCreateForSession(enumerator.Current, out moduleInfoModel))
							{
								this._loadedUnofficialModules.Add(moduleInfoModel);
							}
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000094EC File Offset: 0x000076EC
		public async Task<AvailableCustomGames> GetCustomGameServerList()
		{
			this.AssertCanPerformLobbyActions();
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<CustomGameServerListResponse>(new RequestCustomGameServerListMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			CustomGameServerListResponse customGameServerListResponse = taskAwaiter.GetResult().Result as CustomGameServerListResponse;
			Debug.Print("Custom game server list received", 0, Debug.DebugColor.White, 17592186044416UL);
			AvailableCustomGames availableCustomGames;
			if (customGameServerListResponse != null)
			{
				this.AvailableCustomGames = customGameServerListResponse.AvailableCustomGames;
				ILobbyClientSessionHandler handler = this._handler;
				if (handler != null)
				{
					handler.OnCustomGameServerListReceived(this.AvailableCustomGames);
				}
				availableCustomGames = this.AvailableCustomGames;
			}
			else
			{
				availableCustomGames = null;
			}
			return availableCustomGames;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00009531 File Offset: 0x00007731
		public void QuitFromCustomGame()
		{
			base.SendMessage(new QuitFromCustomGameMessage());
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnQuitFromCustomGame();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00009555 File Offset: 0x00007755
		public void QuitFromMatchmakerGame()
		{
			if (this.CurrentState == LobbyClient.State.AtBattle)
			{
				this.CheckAndSendMessage(new QuitFromMatchmakerGameMessage());
				this.CurrentState = LobbyClient.State.QuittingFromBattle;
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnQuitFromMatchmakerGame();
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00009584 File Offset: 0x00007784
		public async Task<bool> RequestJoinCustomGame(CustomBattleId serverId, CustomGameJoinType joinType, string password)
		{
			this.CurrentState = LobbyClient.State.WaitingToJoinCustomGame;
			this.CustomBattleId = serverId;
			string text = ((!string.IsNullOrEmpty(password)) ? Common.CalculateMD5Hash(password) : null);
			base.SendMessage(new RequestJoinCustomGameMessage(serverId, joinType, text));
			while (this.CurrentState == LobbyClient.State.WaitingToJoinCustomGame)
			{
				await Task.Yield();
			}
			bool flag;
			if (this.CurrentState == LobbyClient.State.InCustomGame)
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x000095E4 File Offset: 0x000077E4
		public async Task<bool> RequestJoinPlayerParty(PlayerId targetPlayer, bool inviteRequest)
		{
			this.AssertCanPerformLobbyActions();
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<RequestJoinPlayerPartyMessageResult>(new RequestJoinPlayerPartyMessage(targetPlayer, inviteRequest)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			RequestJoinPlayerPartyMessageResult requestJoinPlayerPartyMessageResult = taskAwaiter.GetResult().Result as RequestJoinPlayerPartyMessageResult;
			bool flag;
			if (requestJoinPlayerPartyMessageResult != null)
			{
				flag = requestJoinPlayerPartyMessageResult.Success;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00009639 File Offset: 0x00007839
		public void CancelFindGame()
		{
			this.CurrentState = LobbyClient.State.RequestingToCancelSearchBattle;
			this.CheckAndSendMessage(new CancelBattleRequestMessage());
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0000964D File Offset: 0x0000784D
		public void FindGame()
		{
			this.CurrentState = LobbyClient.State.RequestingToSearchBattle;
			this.CheckAndSendMessage(new FindGameMessage());
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00009664 File Offset: 0x00007864
		public async Task<bool> FindCustomGame(string[] selectedCustomGameTypes, bool? hasCrossplayPrivilege, string region)
		{
			this.CurrentState = LobbyClient.State.WaitingToJoinCustomGame;
			int i = 0;
			while (i < LobbyClient.CheckForCustomGamesCount)
			{
				TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<CustomGameServerListResponse>(new RequestCustomGameServerListMessage()).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<CallResult> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<CallResult>);
				}
				CustomGameServerListResponse customGameServerListResponse = taskAwaiter.GetResult().Result as CustomGameServerListResponse;
				if (customGameServerListResponse != null && customGameServerListResponse.AvailableCustomGames.CustomGameServerInfos.Count > 0)
				{
					List<GameServerEntry> list = customGameServerListResponse.AvailableCustomGames.CustomGameServerInfos.OrderByDescending<GameServerEntry, int>((GameServerEntry c) => c.PlayerCount).ToList<GameServerEntry>();
					bool? flag = hasCrossplayPrivilege;
					bool flag2 = true;
					GameServerEntry.FilterGameServerEntriesBasedOnCrossplay(ref list, (flag.GetValueOrDefault() == flag2) & (flag != null));
					foreach (string text in selectedCustomGameTypes)
					{
						foreach (GameServerEntry gameServerEntry in list)
						{
							if (gameServerEntry.IsOfficial && gameServerEntry.GameType == text && gameServerEntry.Region == region && !gameServerEntry.PasswordProtected && gameServerEntry.MaxPlayerCount >= gameServerEntry.PlayerCount + this.PlayersInParty.Count)
							{
								base.SendMessage(new RequestJoinCustomGameMessage(gameServerEntry.Id, CustomGameJoinType.Player, null));
								while (this.CurrentState == LobbyClient.State.WaitingToJoinCustomGame)
								{
									await Task.Yield();
								}
								if (this.CurrentState == LobbyClient.State.InCustomGame)
								{
									return true;
								}
								return false;
							}
						}
						List<GameServerEntry>.Enumerator enumerator = default(List<GameServerEntry>.Enumerator);
					}
					await Task.Delay(LobbyClient.CheckForCustomGamesDelay);
				}
				int j = i++;
			}
			this.CurrentState = LobbyClient.State.AtLobby;
			return false;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x000096C4 File Offset: 0x000078C4
		public async Task<LobbyClientConnectResult> Connect(ILobbyClientSessionHandler lobbyClientSessionHandler, ILoginAccessProvider lobbyClientLoginAccessProvider, string overridenUserName, bool hasUserGeneratedContentPrivilege, PlatformInitParams initParams, Func<Task<bool>> preLoginTask)
		{
			base.AccessProvider = lobbyClientLoginAccessProvider;
			base.AccessProvider.Initialize(overridenUserName, initParams);
			this._handler = lobbyClientSessionHandler;
			this.CurrentState = LobbyClient.State.Working;
			this.HasUserGeneratedContentPrivilege = hasUserGeneratedContentPrivilege;
			base.BeginConnect();
			while (this.CurrentState == LobbyClient.State.Working)
			{
				await Task.Yield();
			}
			LobbyClientConnectResult lobbyClientConnectResult;
			if (this.CurrentState != LobbyClient.State.Connected)
			{
				lobbyClientConnectResult = new LobbyClientConnectResult(false, new TextObject("{=3cWg0cWt}Could not connect to server.", null));
			}
			else
			{
				AccessObjectResult accessObjectResult = AccessObjectResult.CreateFailed(new TextObject("{=gAeQdLU5}Failed to acquire access data from platform", null));
				Task getAccessObjectTask = Task.Run(delegate
				{
					accessObjectResult = this.AccessProvider.CreateAccessObject();
				});
				while (!getAccessObjectTask.IsCompleted)
				{
					await Task.Yield();
				}
				if (getAccessObjectTask.IsFaulted)
				{
					throw getAccessObjectTask.Exception ?? new Exception("Get access object task faulted without exception");
				}
				if (getAccessObjectTask.IsCanceled)
				{
					throw new Exception("Get access object task canceled");
				}
				if (!accessObjectResult.Success)
				{
					base.BeginDisconnect();
					lobbyClientConnectResult = new LobbyClientConnectResult(false, accessObjectResult.FailReason ?? new TextObject("{=JO37PkfW}Your platform service is not initialized.", null));
				}
				else
				{
					bool flag = preLoginTask != null;
					if (flag)
					{
						TaskAwaiter<bool> taskAwaiter = preLoginTask().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						flag = !taskAwaiter.GetResult();
					}
					if (flag)
					{
						base.BeginDisconnect();
						lobbyClientConnectResult = new LobbyClientConnectResult(false, new TextObject("{=63X8LERm}Couldn't receive login result from server.", null));
					}
					else
					{
						this._userName = base.AccessProvider.GetUserName();
						this._playerId = base.AccessProvider.GetPlayerId();
						this.CurrentState = LobbyClient.State.SessionRequested;
						string environmentVariable = Environment.GetEnvironmentVariable("Bannerlord.ConnectionPassword");
						LoginResult loginResult = await base.Login(new InitializeSession(this._playerId, this._userName, accessObjectResult.AccessObject, base.Application.ApplicationVersion, environmentVariable, this._loadedUnofficialModules.ToArray()));
						if (loginResult == null)
						{
							base.BeginDisconnect();
							lobbyClientConnectResult = new LobbyClientConnectResult(false, new TextObject("{=63X8LERm}Couldn't receive login result from server.", null));
						}
						else if (!loginResult.Successful)
						{
							base.BeginDisconnect();
							lobbyClientConnectResult = LobbyClientConnectResult.FromServerConnectResult(loginResult.ErrorCode, loginResult.ErrorParameters);
						}
						else
						{
							InitializeSessionResponse initializeSessionResponse = (InitializeSessionResponse)loginResult.LoginResultObject;
							this.PlayerData = initializeSessionResponse.PlayerData;
							this._serverStatus = initializeSessionResponse.ServerStatus;
							this.SupportedFeatures = initializeSessionResponse.SupportedFeatures;
							this.AvailableScenes = initializeSessionResponse.AvailableScenes;
							this._logOutReason = new TextObject("{=i4MNr0bo}Disconnected from the Lobby.", null);
							await PermaMuteList.LoadMutedPlayers(this.PlayerData.PlayerId);
							this._ownedCosmetics.Clear();
							this._usedCosmetics.Clear();
							ILobbyClientSessionHandler handler = this._handler;
							if (handler != null)
							{
								handler.OnPlayerDataReceived(this.PlayerData);
							}
							ILobbyClientSessionHandler handler2 = this._handler;
							if (handler2 != null)
							{
								handler2.OnServerStatusReceived(initializeSessionResponse.ServerStatus);
							}
							LobbyClient.FriendListCheckDelay = this._serverStatus.FriendListUpdatePeriod * 1000;
							if (initializeSessionResponse.HasPendingRejoin)
							{
								ILobbyClientSessionHandler handler3 = this._handler;
								if (handler3 != null)
								{
									handler3.OnPendingRejoin();
								}
							}
							this.CurrentState = LobbyClient.State.AtLobby;
							lobbyClientConnectResult = new LobbyClientConnectResult(true, null);
						}
					}
				}
			}
			return lobbyClientConnectResult;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0000973C File Offset: 0x0000793C
		public void KickPlayer(PlayerId id, bool banPlayer)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00009744 File Offset: 0x00007944
		public async Task<bool> ChangeRegion(string region)
		{
			bool flag;
			if (this.PlayerData != null && this.PlayerData.LastRegion == region)
			{
				flag = true;
			}
			else
			{
				PlayerData playerData = this.PlayerData;
				string previousRegion = ((playerData != null) ? playerData.LastRegion : null);
				if (this.CurrentState == LobbyClient.State.AtLobby && this.PlayerData != null)
				{
					this.PlayerData.LastRegion = region;
				}
				CallResult callResult = await base.CallFunction<FunctionResult>(new ChangeRegionMessage(region));
				if (!callResult.Success)
				{
					if (this.PlayerData != null && previousRegion != null)
					{
						this.PlayerData.LastRegion = previousRegion;
					}
					string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
					if (localizedFailureMessage != null)
					{
						ILobbyClientSessionHandler handler = this._handler;
						if (handler != null)
						{
							handler.OnSystemMessageReceived(localizedFailureMessage);
						}
					}
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00009794 File Offset: 0x00007994
		public async Task<bool> ChangeGameTypes(string[] gameTypes)
		{
			bool flag = this.PlayerData == null || this.PlayerData.LastGameTypes.Length != gameTypes.Length;
			if (!flag)
			{
				foreach (string text in gameTypes)
				{
					if (!this.PlayerData.LastGameTypes.Contains(text))
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2;
			if (!flag)
			{
				flag2 = true;
			}
			else
			{
				PlayerData playerData = this.PlayerData;
				string[] previousGameTypes = ((playerData != null) ? playerData.LastGameTypes : null);
				if (this.CurrentState == LobbyClient.State.AtLobby && this.PlayerData != null)
				{
					this.PlayerData.LastGameTypes = gameTypes;
				}
				CallResult callResult = await base.CallFunction<FunctionResult>(new ChangeGameTypesMessage(gameTypes));
				if (!callResult.Success)
				{
					if (this.PlayerData != null && previousGameTypes != null)
					{
						this.PlayerData.LastGameTypes = previousGameTypes;
					}
					string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
					if (localizedFailureMessage != null)
					{
						ILobbyClientSessionHandler handler = this._handler;
						if (handler != null)
						{
							handler.OnSystemMessageReceived(localizedFailureMessage);
						}
					}
					flag2 = false;
				}
				else
				{
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x000097E1 File Offset: 0x000079E1
		private void CheckAndSendMessage(Message message)
		{
			base.SendMessage(message);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x000097EA File Offset: 0x000079EA
		public override void OnConnected()
		{
			base.OnConnected();
			this.CurrentState = LobbyClient.State.Connected;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnConnected();
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00009809 File Offset: 0x00007A09
		public override void OnCantConnect()
		{
			base.OnCantConnect();
			this.CurrentState = LobbyClient.State.Idle;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnCantConnect();
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00009828 File Offset: 0x00007A28
		public override void OnDisconnected()
		{
			base.OnDisconnected();
			bool loggedIn = this.LoggedIn;
			this.CurrentState = LobbyClient.State.Idle;
			this.PlayerData = null;
			this.PlayersInParty.Clear();
			this.PlayersInClan.Clear();
			this.ClanHomeInfo = null;
			this._matchmakerBlockedTime = DateTime.MinValue;
			this.FriendInfos = new FriendInfo[0];
			PermaMuteList.SaveMutedPlayers();
			this._ownedCosmetics.Clear();
			this._usedCosmetics.Clear();
			ILobbyClientSessionHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnDisconnected(loggedIn ? this._logOutReason : null);
			}
			this.RemoveLobbyClientHandler();
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000098C2 File Offset: 0x00007AC2
		public void RemoveLobbyClientHandler()
		{
			this._handler = null;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000098CB File Offset: 0x00007ACB
		private void OnFindGameAnswerMessage(FindGameAnswerMessage message)
		{
			if (!message.Successful)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
			}
			else
			{
				this.CurrentState = LobbyClient.State.SearchingBattle;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnFindGameAnswer(message.Successful, message.SelectedAndEnabledGameTypes, false);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00009904 File Offset: 0x00007B04
		private void OnJoinBattleMessage(JoinBattleMessage message)
		{
			BattleServerInformationForClient battleServerInformation = message.BattleServerInformation;
			string text;
			if (base.Application.ProxyAddressMap.TryGetValue(battleServerInformation.ServerAddress, out text))
			{
				battleServerInformation.ServerAddress = text;
			}
			this.LastBattleServerAddressForClient = battleServerInformation.ServerAddress;
			this.LastBattleServerPortForClient = battleServerInformation.ServerPort;
			this.CurrentMatchId = battleServerInformation.MatchId;
			this.LastBattleIsOfficial = true;
			string text2 = "Successful matchmaker game join response\n";
			text2 = text2 + "Address: " + this.LastBattleServerAddressForClient + "\n";
			text2 = string.Concat(new object[] { text2, "Port: ", this.LastBattleServerPortForClient, "\n" });
			text2 = text2 + "Match Id: " + this.CurrentMatchId + "\n";
			Debug.Print(text2, 0, Debug.DebugColor.White, 17592186044416UL);
			ILobbyClientSessionHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnBattleServerInformationReceived(battleServerInformation);
			}
			this.CurrentState = LobbyClient.State.AtBattle;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000099F8 File Offset: 0x00007BF8
		private void OnBattleOverMessage(BattleOverMessage message)
		{
			if (this.CurrentState == LobbyClient.State.AtBattle || this.CurrentState == LobbyClient.State.QuittingFromBattle || this.CurrentState == LobbyClient.State.AtLobby)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnMatchmakerGameOver(message.OldExperience, message.NewExperience, message.EarnedBadges, message.GoldGained, message.OldInfo, message.NewInfo, message.BattleCancelReason);
			}
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00009A63 File Offset: 0x00007C63
		private void OnBattleResultMessage(BattleResultMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleResultReceived();
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00009A75 File Offset: 0x00007C75
		private void OnBattleServerLostMessage(BattleServerLostMessage message)
		{
			if (this.CurrentState == LobbyClient.State.AtBattle || this.CurrentState == LobbyClient.State.SearchingToRejoinBattle)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleServerLost();
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00009AA1 File Offset: 0x00007CA1
		private void OnCancelBattleResponseMessage(CancelBattleResponseMessage message)
		{
			if (message.Successful)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler != null)
				{
					handler.OnCancelJoiningBattle();
				}
				this.CurrentState = LobbyClient.State.AtLobby;
				return;
			}
			if (this.CurrentState == LobbyClient.State.RequestingToCancelSearchBattle)
			{
				this.CurrentState = LobbyClient.State.SearchingBattle;
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00009AD4 File Offset: 0x00007CD4
		private void OnRejoinRequestRejectedMessage(RejoinRequestRejectedMessage message)
		{
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRejoinRequestRejected();
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00009AED File Offset: 0x00007CED
		private void OnCancelFindGameMessage(CancelFindGameMessage message)
		{
			if (this.CurrentState == LobbyClient.State.SearchingBattle)
			{
				this.CancelFindGame();
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00009AFE File Offset: 0x00007CFE
		private void OnWhisperMessageReceivedMessage(WhisperReceivedMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnWhisperMessageReceived(message.FromPlayer, message.ToPlayer, message.Message);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00009B22 File Offset: 0x00007D22
		private void OnClanMessageReceivedMessage(ClanMessageReceivedMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanMessageReceived(message.PlayerName, message.Message);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00009B40 File Offset: 0x00007D40
		private void OnPartyMessageReceivedMessage(PartyMessageReceivedMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPartyMessageReceived(message.PlayerName, message.Message);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00009B5E File Offset: 0x00007D5E
		private void OnPlayerQuitFromMatchmakerGameResult(PlayerQuitFromMatchmakerGameResult message)
		{
			if (this.CurrentState == LobbyClient.State.QuittingFromBattle)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00009B74 File Offset: 0x00007D74
		private void OnEnterBattleWithPartyAnswerMessage(EnterBattleWithPartyAnswer message)
		{
			if (!message.Successful)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
				return;
			}
			if (this.CurrentState == LobbyClient.State.AtLobby || this.CurrentState == LobbyClient.State.RequestingToSearchBattle)
			{
				this.CurrentState = LobbyClient.State.SearchingBattle;
			}
			else if (this.CurrentState != LobbyClient.State.SearchingBattle)
			{
				LobbyClient.State currentState = this.CurrentState;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnEnterBattleWithPartyAnswer(message.SelectedAndEnabledGameTypes);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00009BD4 File Offset: 0x00007DD4
		private void OnJoinCustomGameResultMessage(JoinCustomGameResultMessage message)
		{
			if (!message.Success && message.Response == CustomGameJoinResponse.AlreadyRequestedWaitingForServerResponse)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnSystemMessageReceived(new TextObject("{=ivKntfNA}Already requested to join, waiting for server response", null).ToString());
				return;
			}
			else if (message.Success)
			{
				message.JoinGameData.GameServerProperties.CheckAndReplaceProxyAddress(base.Application.ProxyAddressMap);
				this.CurrentState = LobbyClient.State.InCustomGame;
				this.LastBattleServerAddressForClient = message.JoinGameData.GameServerProperties.Address;
				this.LastBattleServerPortForClient = (ushort)message.JoinGameData.GameServerProperties.Port;
				this.LastBattleIsOfficial = message.JoinGameData.GameServerProperties.IsOfficial;
				this.CurrentMatchId = message.MatchId;
				string text = "Successful custom game join response\n";
				text = text + "Server Name: " + message.JoinGameData.GameServerProperties.Name + "\n";
				text = text + "Host Name: " + message.JoinGameData.GameServerProperties.HostName + "\n";
				text = text + "Address: " + this.LastBattleServerAddressForClient + "\n";
				text = string.Concat(new object[] { text, "Port: ", this.LastBattleServerPortForClient, "\n" });
				text = text + "Match Id: " + this.CurrentMatchId + "\n";
				text = text + "Is Official: " + message.JoinGameData.GameServerProperties.IsOfficial.ToString() + "\n";
				Debug.Print(text, 0, Debug.DebugColor.White, 17592186044416UL);
				ILobbyClientSessionHandler handler2 = this._handler;
				if (handler2 == null)
				{
					return;
				}
				handler2.OnJoinCustomGameResponse(message.Success, message.JoinGameData, message.Response, message.JoinType);
				return;
			}
			else
			{
				this.CurrentState = LobbyClient.State.AtLobby;
				ILobbyClientSessionHandler handler3 = this._handler;
				if (handler3 == null)
				{
					return;
				}
				handler3.OnJoinCustomGameFailureResponse(message.Response);
				return;
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00009DB8 File Offset: 0x00007FB8
		private void OnClientWantsToConnectCustomGameMessage(ClientWantsToConnectCustomGameMessage message)
		{
			this.AssertCanPerformLobbyActions();
			List<PlayerJoinGameResponseDataFromHost> list = new List<PlayerJoinGameResponseDataFromHost>();
			PlayerJoinGameData[] playerJoinGameData = message.PlayerJoinGameData;
			for (int i = 0; i < playerJoinGameData.Length; i++)
			{
				if (playerJoinGameData[i] != null)
				{
					List<PlayerJoinGameData> list2 = new List<PlayerJoinGameData>();
					PlayerJoinGameData playerJoinGameData2 = playerJoinGameData[i];
					Guid? guid = playerJoinGameData2.PartyId;
					if (guid == null)
					{
						list2.Add(playerJoinGameData2);
					}
					else
					{
						for (int j = i; j < playerJoinGameData.Length; j++)
						{
							PlayerJoinGameData playerJoinGameData3 = playerJoinGameData[j];
							guid = playerJoinGameData2.PartyId;
							if (guid.Equals((playerJoinGameData3 != null) ? playerJoinGameData3.PartyId : null))
							{
								list2.Add(playerJoinGameData3);
								playerJoinGameData[j] = null;
							}
						}
					}
					if (this._handler != null)
					{
						PlayerJoinGameResponseDataFromHost[] array = this._handler.OnClientWantsToConnectCustomGame(list2.ToArray());
						list.AddRange(array);
					}
				}
			}
			this.ResponseCustomGameClientConnection(list.ToArray());
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00009EA3 File Offset: 0x000080A3
		private void OnClientQuitFromCustomGameMessage(ClientQuitFromCustomGameMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClientQuitFromCustomGame(message.PlayerId);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00009EBB File Offset: 0x000080BB
		private void OnEnterCustomBattleWithPartyAnswerMessage(EnterCustomBattleWithPartyAnswer message)
		{
			if (!message.Successful)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
				return;
			}
			if (this.CurrentState == LobbyClient.State.AtLobby)
			{
				this.CurrentState = LobbyClient.State.WaitingToJoinCustomGame;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnEnterCustomBattleWithPartyAnswer();
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00009EEE File Offset: 0x000080EE
		private void OnPlayerRemovedFromMatchmakerGameMessage(PlayerRemovedFromMatchmakerGame message)
		{
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRemovedFromMatchmakerGame(message.DisconnectType);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00009F0D File Offset: 0x0000810D
		private void OnPlayerRemovedFromCustomGame(PlayerRemovedFromCustomGame message)
		{
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRemovedFromCustomGame(message.DisconnectType);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00009F2C File Offset: 0x0000812C
		private void OnSystemMessage(SystemMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnSystemMessageReceived(message.GetDescription().ToString());
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00009F49 File Offset: 0x00008149
		private void OnAdminMessage(AdminMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnAdminMessageReceived(message.Message);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00009F61 File Offset: 0x00008161
		private void OnInvitationToPartyMessage(InvitationToPartyMessage message)
		{
			this.IsPartyInvitationPopupActive = true;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPartyInvitationReceived(message.InviterPlayerName, message.InviterPlayerId);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00009F86 File Offset: 0x00008186
		private void OnPartyInvitationInvalidMessage(PartyInvitationInvalidMessage message)
		{
			this.IsPartyInvitationPopupActive = false;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPartyInvitationInvalidated();
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00009F9F File Offset: 0x0000819F
		private void OnRequestJoinPartyMessage(RequestJoinPartyMessage message)
		{
			this.IsPartyJoinRequestPopupActive = true;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPartyJoinRequestReceived(message.PlayerId, message.ViaPlayerId, message.ViaPlayerName);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00009FCA File Offset: 0x000081CA
		private void OnPlayerInvitedToPartyMessage(PlayerInvitedToPartyMessage message)
		{
			this.PlayersInParty.Add(new PartyPlayerInLobbyClient(message.PlayerId, message.PlayerName, false));
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerInvitedToParty(message.PlayerId);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0000A000 File Offset: 0x00008200
		private void OnPlayerAddedToPartyMessage(PlayersAddedToPartyMessage message)
		{
			foreach (ValueTuple<PlayerId, string, bool> valueTuple in message.Players)
			{
				PlayerId playerId = valueTuple.Item1;
				string item = valueTuple.Item2;
				bool item2 = valueTuple.Item3;
				PartyPlayerInLobbyClient partyPlayerInLobbyClient = this.PlayersInParty.Find((PartyPlayerInLobbyClient p) => p.PlayerId == playerId);
				if (partyPlayerInLobbyClient != null)
				{
					partyPlayerInLobbyClient.SetAtParty();
				}
				else
				{
					partyPlayerInLobbyClient = new PartyPlayerInLobbyClient(playerId, item, item2);
					this.PlayersInParty.Add(partyPlayerInLobbyClient);
					partyPlayerInLobbyClient.SetAtParty();
				}
				if (playerId != this.PlayerID)
				{
					RecentPlayersManager.AddOrUpdatePlayerEntry(playerId, item, InteractionType.InPartyTogether, -1);
				}
			}
			foreach (ValueTuple<PlayerId, string> valueTuple2 in message.InvitedPlayers)
			{
				PlayerId item3 = valueTuple2.Item1;
				string item4 = valueTuple2.Item2;
				this.PlayersInParty.Add(new PartyPlayerInLobbyClient(item3, item4, false));
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayersAddedToParty(message.Players, message.InvitedPlayers);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0000A15C File Offset: 0x0000835C
		private void OnPlayerRemovedFromPartyMessage(PlayerRemovedFromPartyMessage message)
		{
			if (message.PlayerId == this._playerId)
			{
				this.PlayersInParty.Clear();
			}
			else
			{
				this.PlayersInParty.RemoveAll((PartyPlayerInLobbyClient partyPlayer) => partyPlayer.PlayerId == message.PlayerId);
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerRemovedFromParty(message.PlayerId, message.Reason);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0000A1DC File Offset: 0x000083DC
		private void OnPlayerAssignedPartyLeaderMessage(PlayerAssignedPartyLeaderMessage message)
		{
			PartyPlayerInLobbyClient partyPlayerInLobbyClient = this.PlayersInParty.FirstOrDefault<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient p) => p.IsPartyLeader);
			if (partyPlayerInLobbyClient != null)
			{
				partyPlayerInLobbyClient.SetMember();
			}
			PartyPlayerInLobbyClient partyPlayerInLobbyClient2 = this.PlayersInParty.FirstOrDefault<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient partyPlayer) => partyPlayer.PlayerId == message.PartyLeaderId);
			if (partyPlayerInLobbyClient2 != null)
			{
				partyPlayerInLobbyClient2.SetLeader();
			}
			else
			{
				this.KickPlayerFromParty(this.PlayerID);
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerAssignedPartyLeader(message.PartyLeaderId);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0000A275 File Offset: 0x00008475
		private void OnPlayerSuggestedToPartyMessage(PlayerSuggestedToPartyMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerSuggestedToParty(message.PlayerId, message.PlayerName, message.SuggestingPlayerId, message.SuggestingPlayerName);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0000A29F File Offset: 0x0000849F
		private void OnUpdatePlayerDataMessage(UpdatePlayerDataMessage updatePlayerDataMessage)
		{
			this.PlayerData = updatePlayerDataMessage.PlayerData;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerDataReceived(this.PlayerData);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0000A2C4 File Offset: 0x000084C4
		private void OnServerStatusMessage(ServerStatusMessage serverStatusMessage)
		{
			this._serverStatusTimer.Restart();
			this._serverStatus = serverStatusMessage.ServerStatus;
			if (!this.IsAbleToSearchForGame && this.CurrentState == LobbyClient.State.SearchingBattle)
			{
				this.CancelFindGame();
			}
			if (this._handler != null)
			{
				this._handler.OnServerStatusReceived(this._serverStatus);
				LobbyClient.FriendListCheckDelay = this._serverStatus.FriendListUpdatePeriod * 1000;
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0000A32E File Offset: 0x0000852E
		private void OnFriendListMessage(FriendListMessage friendListMessage)
		{
			this._friendListTimer.Restart();
			this.FriendInfos = friendListMessage.Friends;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnFriendListReceived(friendListMessage.Friends);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0000A360 File Offset: 0x00008560
		private void OnMatchmakerDisabledMessage(MatchmakerDisabledMessage matchmakerDisabledMessage)
		{
			if (matchmakerDisabledMessage.RemainingTime > 0)
			{
				this._matchmakerBlockedTime = DateTime.Now.AddSeconds((double)matchmakerDisabledMessage.RemainingTime);
				return;
			}
			this._matchmakerBlockedTime = DateTime.MinValue;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0000A39C File Offset: 0x0000859C
		private void OnClanCreationRequestMessage(ClanCreationRequestMessage clanCreationRequestMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInvitationReceived(clanCreationRequestMessage.ClanName, clanCreationRequestMessage.ClanTag, true);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0000A3BB File Offset: 0x000085BB
		private void OnClanCreationRequestAnsweredMessage(ClanCreationRequestAnsweredMessage clanCreationRequestAnsweredMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInvitationAnswered(clanCreationRequestAnsweredMessage.PlayerId, clanCreationRequestAnsweredMessage.ClanCreationAnswer);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0000A3D9 File Offset: 0x000085D9
		private void OnClanCreationSuccessfulMessage(ClanCreationSuccessfulMessage clanCreationSuccessfulMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanCreationSuccessful();
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0000A3EB File Offset: 0x000085EB
		private void OnClanCreationFailedMessage(ClanCreationFailedMessage clanCreationFailedMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanCreationFailed();
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0000A3FD File Offset: 0x000085FD
		private void OnCreateClanAnswerMessage(CreateClanAnswerMessage createClanAnswerMessage)
		{
			if (createClanAnswerMessage.Successful)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnClanCreationStarted();
			}
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0000A417 File Offset: 0x00008617
		public void SendWhisper(string playerName, string message)
		{
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0000A419 File Offset: 0x00008619
		private void OnRecentPlayerStatusesMessage(RecentPlayerStatusesMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRecentPlayerStatusesReceived(message.Friends);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0000A431 File Offset: 0x00008631
		public void FleeBattle()
		{
			this.CheckAndSendMessage(new RejoinBattleRequestMessage(false));
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0000A43F File Offset: 0x0000863F
		public void SendPartyMessage(string message)
		{
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0000A441 File Offset: 0x00008641
		private void OnClanInfoChangedMessage(ClanInfoChangedMessage clanInfoChangedMessage)
		{
			this.UpdateClanInfo(clanInfoChangedMessage.ClanHomeInfo);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0000A450 File Offset: 0x00008650
		protected override void OnTick()
		{
			if (this.LoggedIn && !this.IsInGame)
			{
				if (this._serverStatusTimer != null && this._serverStatusTimer.ElapsedMilliseconds > (long)LobbyClient.ServerStatusCheckDelay)
				{
					this._serverStatusTimer.Restart();
					this.CheckAndSendMessage(new GetServerStatusMessage());
				}
				if (this._friendListTimer != null && this._friendListTimer.ElapsedMilliseconds > (long)LobbyClient.FriendListCheckDelay)
				{
					this._friendListTimer.Restart();
					this.CheckAndSendMessage(new GetFriendListMessage());
				}
				if (this._recentPlayersTimer != null && this._recentPlayersTimer.ElapsedMilliseconds > (long)LobbyClient.RecentPlayersCheckDelay)
				{
					this._recentPlayersTimer.Restart();
					RecentPlayersManager.TrimPlayers();
					PlayerId[] recentPlayerIds = RecentPlayersManager.GetRecentPlayerIds();
					if (recentPlayerIds.Length != 0)
					{
						this.CheckAndSendMessage(new GetRecentPlayersStatusMessage(recentPlayerIds));
					}
				}
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0000A516 File Offset: 0x00008716
		private void OnInvitationToClanMessage(InvitationToClanMessage invitationToClanMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInvitationReceived(invitationToClanMessage.ClanName, invitationToClanMessage.ClanTag, false);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0000A535 File Offset: 0x00008735
		public void RejoinBattle()
		{
			this.CheckAndSendMessage(new RejoinBattleRequestMessage(true));
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0000A543 File Offset: 0x00008743
		private void OnJoinPremadeGameAnswerMessage(JoinPremadeGameAnswerMessage joinPremadeGameAnswerMessage)
		{
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0000A545 File Offset: 0x00008745
		public void OnBattleResultsSeen()
		{
			this.AssertCanPerformLobbyActions();
			this.CheckAndSendMessage(new BattleResultSeenMessage());
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0000A558 File Offset: 0x00008758
		private void OnCreatePremadeGameAnswerMessage(CreatePremadeGameAnswerMessage createPremadeGameAnswerMessage)
		{
			if (createPremadeGameAnswerMessage.Successful)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnPremadeGameCreated();
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0000A572 File Offset: 0x00008772
		private void OnJoinPremadeGameRequestMessage(JoinPremadeGameRequestMessage joinPremadeGameRequestMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnJoinPremadeGameRequested(joinPremadeGameRequestMessage.ClanName, joinPremadeGameRequestMessage.Sigil, joinPremadeGameRequestMessage.ChallengerPartyId, joinPremadeGameRequestMessage.ChallengerPlayers, joinPremadeGameRequestMessage.ChallengerPartyLeaderId, joinPremadeGameRequestMessage.PremadeGameType);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0000A5A8 File Offset: 0x000087A8
		private void OnJoinPremadeGameRequestResultMessage(JoinPremadeGameRequestResultMessage joinPremadeGameRequestResultMessage)
		{
			if (joinPremadeGameRequestResultMessage.Successful)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler != null)
				{
					handler.OnJoinPremadeGameRequestSuccessful();
				}
				this.CurrentState = LobbyClient.State.WaitingToJoinPremadeGame;
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0000A5CC File Offset: 0x000087CC
		private async void OnClanDisbandedMessage(ClanDisbandedMessage clanDisbandedMessage)
		{
			ClanHomeInfo clanHomeInfo = await this.GetClanHomeInfo();
			this.UpdateClanInfo(clanHomeInfo);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0000A605 File Offset: 0x00008805
		private void OnClanGameCreationCancelledMessage(ClanGameCreationCancelledMessage clanGameCreationCancelledMessage)
		{
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPremadeGameCreationCancelled();
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0000A61E File Offset: 0x0000881E
		private void OnPremadeGameEligibilityStatusMessage(PremadeGameEligibilityStatusMessage premadeGameEligibilityStatusMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnPremadeGameEligibilityStatusReceived(premadeGameEligibilityStatusMessage.EligibleGameTypes.Length != 0);
			}
			this.IsEligibleToCreatePremadeGame = premadeGameEligibilityStatusMessage.EligibleGameTypes.Length != 0;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0000A64C File Offset: 0x0000884C
		private async void OnKickedFromClan(KickedFromClanMessage kickedFromClanMessage)
		{
			ClanHomeInfo clanHomeInfo = await this.GetClanHomeInfo();
			this.UpdateClanInfo(clanHomeInfo);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0000A688 File Offset: 0x00008888
		private async void OnPartyPlayerLeftClan(PartyPlayerLeftClanMessage partyPlayerLeftClanMessage)
		{
			ClanHomeInfo clanHomeInfo = await this.GetClanHomeInfo();
			this.UpdateClanInfo(clanHomeInfo);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0000A6C1 File Offset: 0x000088C1
		private void OnCustomBattleOverMessage(CustomBattleOverMessage message)
		{
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMatchmakerGameOver(message.OldExperience, message.NewExperience, new List<string>(), message.GoldGain, null, null, BattleCancelReason.None);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0000A6F4 File Offset: 0x000088F4
		public async Task<bool> AcceptClanInvitation()
		{
			CallResult callResult = await base.CallFunction<FunctionResult>(new AcceptClanInvitationMessage());
			if (!callResult.Success)
			{
				string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
				if (localizedFailureMessage != null)
				{
					ILobbyClientSessionHandler handler = this._handler;
					if (handler != null)
					{
						handler.OnSystemMessageReceived(localizedFailureMessage);
					}
				}
			}
			return callResult.Success;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0000A739 File Offset: 0x00008939
		public void DeclineClanInvitation()
		{
			this.CheckAndSendMessage(new DeclineClanInvitationMessage());
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0000A746 File Offset: 0x00008946
		private void OnShowAnnouncementMessage(ShowAnnouncementMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnAnnouncementReceived(message.Announcement);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0000A760 File Offset: 0x00008960
		public void MarkNotificationAsRead(int notificationID)
		{
			UpdateNotificationsMessage updateNotificationsMessage = new UpdateNotificationsMessage(new int[] { notificationID });
			this.CheckAndSendMessage(updateNotificationsMessage);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0000A784 File Offset: 0x00008984
		private void OnRejoinBattleRequestAnswerMessage(RejoinBattleRequestAnswerMessage rejoinBattleRequestAnswerMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnRejoinBattleRequestAnswered(rejoinBattleRequestAnswerMessage.IsSuccessful);
			}
			if (rejoinBattleRequestAnswerMessage.IsSuccessful && rejoinBattleRequestAnswerMessage.IsRejoinAccepted)
			{
				this.CurrentState = LobbyClient.State.SearchingBattle;
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0000A7B4 File Offset: 0x000089B4
		public void AcceptClanCreationRequest()
		{
			this.CheckAndSendMessage(new AcceptClanCreationRequestMessage());
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0000A7C1 File Offset: 0x000089C1
		private void OnPendingBattleRejoinMessage(PendingBattleRejoinMessage pendingBattleRejoinMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPendingRejoin();
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0000A7D3 File Offset: 0x000089D3
		private void OnSigilChangeAnswerMessage(SigilChangeAnswerMessage message)
		{
			if (message.Successful)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnSigilChanged();
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0000A7ED File Offset: 0x000089ED
		public void DeclineClanCreationRequest()
		{
			this.CheckAndSendMessage(new DeclineClanCreationRequestMessage());
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0000A7FC File Offset: 0x000089FC
		public async Task<bool> PromoteToClanLeader(PlayerId playerId, bool dontUseNameForUnknownPlayer)
		{
			CallResult callResult = await base.CallFunction<FunctionResult>(new PromoteToClanLeaderMessage(playerId, dontUseNameForUnknownPlayer));
			if (!callResult.Success)
			{
				string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
				if (localizedFailureMessage != null)
				{
					ILobbyClientSessionHandler handler = this._handler;
					if (handler != null)
					{
						handler.OnSystemMessageReceived(localizedFailureMessage);
					}
				}
			}
			return callResult.Success;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0000A851 File Offset: 0x00008A51
		private void OnLobbyNotificationsMessage(LobbyNotificationsMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnNotificationsReceived(message.Notifications);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0000A869 File Offset: 0x00008A69
		public void KickFromClan(PlayerId playerId)
		{
			this.CheckAndSendMessage(new KickFromClanMessage(playerId));
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0000A878 File Offset: 0x00008A78
		public async Task<CheckClanParameterValidResult> ClanNameExists(string clanName)
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<CheckClanParameterValidResult>(new CheckClanNameValidMessage(clanName)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			return taskAwaiter.GetResult().Result as CheckClanParameterValidResult;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0000A8C8 File Offset: 0x00008AC8
		public async Task<CheckClanParameterValidResult> ClanTagExists(string clanTag)
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<CheckClanParameterValidResult>(new CheckClanTagValidMessage(clanTag)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			return taskAwaiter.GetResult().Result as CheckClanParameterValidResult;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0000A918 File Offset: 0x00008B18
		public async Task<ClanHomeInfo> GetClanHomeInfo()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetClanHomeInfoResult>(new GetClanHomeInfoMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetClanHomeInfoResult getClanHomeInfoResult = taskAwaiter.GetResult().Result as GetClanHomeInfoResult;
			ClanHomeInfo clanHomeInfo;
			if (getClanHomeInfoResult != null)
			{
				this.UpdateClanInfo(getClanHomeInfoResult.ClanHomeInfo);
				clanHomeInfo = getClanHomeInfoResult.ClanHomeInfo;
			}
			else
			{
				this.UpdateClanInfo(null);
				clanHomeInfo = null;
			}
			return clanHomeInfo;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0000A95D File Offset: 0x00008B5D
		public void AssignAsClanOfficer(PlayerId playerId, bool dontUseNameForUnknownPlayer)
		{
			this.CheckAndSendMessage(new AssignAsClanOfficerMessage(playerId, dontUseNameForUnknownPlayer));
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0000A96C File Offset: 0x00008B6C
		public void RemoveClanOfficerRoleForPlayer(PlayerId playerId)
		{
			this.CheckAndSendMessage(new RemoveClanOfficerRoleForPlayerMessage(playerId));
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0000A97C File Offset: 0x00008B7C
		private void UpdateClanInfo(ClanHomeInfo clanHomeInfo)
		{
			this.PlayersInClan.Clear();
			this.PlayerInfosInClan.Clear();
			this.ClanID = Guid.Empty;
			this.ClanInfo = null;
			this.ClanHomeInfo = clanHomeInfo;
			if (clanHomeInfo != null)
			{
				if (clanHomeInfo.IsInClan)
				{
					foreach (ClanPlayer clanPlayer in clanHomeInfo.ClanInfo.Players)
					{
						this.PlayersInClan.Add(clanPlayer);
					}
					foreach (ClanPlayerInfo clanPlayerInfo in clanHomeInfo.ClanPlayerInfos)
					{
						this.PlayerInfosInClan.Add(clanPlayerInfo);
					}
					this.ClanID = clanHomeInfo.ClanInfo.ClanId;
				}
				this.ClanInfo = clanHomeInfo.ClanInfo;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInfoChanged();
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0000AA44 File Offset: 0x00008C44
		public async Task<ClanLeaderboardInfo> GetClanLeaderboardInfo()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetClanLeaderboardResult>(new GetClanLeaderboardMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetClanLeaderboardResult getClanLeaderboardResult = taskAwaiter.GetResult().Result as GetClanLeaderboardResult;
			ClanLeaderboardInfo clanLeaderboardInfo;
			if (getClanLeaderboardResult != null)
			{
				clanLeaderboardInfo = getClanLeaderboardResult.ClanLeaderboardInfo;
			}
			else
			{
				clanLeaderboardInfo = null;
			}
			return clanLeaderboardInfo;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0000AA8C File Offset: 0x00008C8C
		public async Task<ClanInfo> GetPlayerClanInfo(PlayerId playerId)
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetPlayerClanInfoResult>(new GetPlayerClanInfo(playerId)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetPlayerClanInfoResult getPlayerClanInfoResult = taskAwaiter.GetResult().Result as GetPlayerClanInfoResult;
			ClanInfo clanInfo;
			if (((getPlayerClanInfoResult != null) ? getPlayerClanInfoResult.ClanInfo : null) != null)
			{
				clanInfo = getPlayerClanInfoResult.ClanInfo;
			}
			else
			{
				clanInfo = null;
			}
			return clanInfo;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0000AAD9 File Offset: 0x00008CD9
		public void SendClanMessage(string message)
		{
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0000AADC File Offset: 0x00008CDC
		public async Task<PremadeGameList> GetPremadeGameList()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetPremadeGameListResult>(new GetPremadeGameListMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetPremadeGameListResult getPremadeGameListResult = taskAwaiter.GetResult().Result as GetPremadeGameListResult;
			PremadeGameList premadeGameList;
			if (getPremadeGameListResult != null)
			{
				this.AvailablePremadeGames = getPremadeGameListResult.GameList;
				ILobbyClientSessionHandler handler = this._handler;
				if (handler != null)
				{
					handler.OnPremadeGameListReceived();
				}
				premadeGameList = getPremadeGameListResult.GameList;
			}
			else
			{
				premadeGameList = null;
			}
			return premadeGameList;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0000AB24 File Offset: 0x00008D24
		public async Task<AvailableScenes> GetAvailableScenes()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetAvailableScenesResult>(new GetAvailableScenesMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetAvailableScenesResult getAvailableScenesResult = taskAwaiter.GetResult().Result as GetAvailableScenesResult;
			AvailableScenes availableScenes;
			if (getAvailableScenesResult != null)
			{
				availableScenes = getAvailableScenesResult.AvailableScenes;
			}
			else
			{
				availableScenes = null;
			}
			return availableScenes;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0000AB6C File Offset: 0x00008D6C
		public async Task<PublishedLobbyNewsArticle[]> GetLobbyNews()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetPublishedLobbyNewsMessageResult>(new GetPublishedLobbyNewsMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetPublishedLobbyNewsMessageResult getPublishedLobbyNewsMessageResult = taskAwaiter.GetResult().Result as GetPublishedLobbyNewsMessageResult;
			PublishedLobbyNewsArticle[] array;
			if (getPublishedLobbyNewsMessageResult != null)
			{
				array = getPublishedLobbyNewsMessageResult.Content;
			}
			else
			{
				array = null;
			}
			return array;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0000ABB4 File Offset: 0x00008DB4
		public async Task<bool> SetClanInformationText(string informationText)
		{
			CallResult callResult = await base.CallFunction<FunctionResult>(new SetClanInformationMessage(informationText));
			if (!callResult.Success)
			{
				string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
				if (localizedFailureMessage != null)
				{
					ILobbyClientSessionHandler handler = this._handler;
					if (handler != null)
					{
						handler.OnSystemMessageReceived(localizedFailureMessage);
					}
				}
			}
			return callResult.Success;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0000AC04 File Offset: 0x00008E04
		public async Task<bool> AddClanAnnouncement(string announcement)
		{
			CallResult callResult = await base.CallFunction<FunctionResult>(new AddClanAnnouncementMessage(announcement));
			if (!callResult.Success)
			{
				string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
				if (localizedFailureMessage != null)
				{
					ILobbyClientSessionHandler handler = this._handler;
					if (handler != null)
					{
						handler.OnSystemMessageReceived(localizedFailureMessage);
					}
				}
			}
			return callResult.Success;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0000AC51 File Offset: 0x00008E51
		public void EditClanAnnouncement(int announcementId, string text)
		{
			this.CheckAndSendMessage(new EditClanAnnouncementMessage(announcementId, text));
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0000AC60 File Offset: 0x00008E60
		public void RemoveClanAnnouncement(int announcementId)
		{
			this.CheckAndSendMessage(new RemoveClanAnnouncementMessage(announcementId));
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0000AC70 File Offset: 0x00008E70
		public async Task<bool> ChangeClanFaction(string faction)
		{
			CallResult callResult = await base.CallFunction<FunctionResult>(new ChangeClanFactionMessage(faction));
			if (!callResult.Success)
			{
				string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
				if (localizedFailureMessage != null)
				{
					ILobbyClientSessionHandler handler = this._handler;
					if (handler != null)
					{
						handler.OnSystemMessageReceived(localizedFailureMessage);
					}
				}
			}
			return callResult.Success;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0000ACC0 File Offset: 0x00008EC0
		public async Task<bool> ChangeClanSigil(string sigil)
		{
			CallResult callResult = await base.CallFunction<FunctionResult>(new ChangeClanSigilMessage(sigil));
			if (!callResult.Success)
			{
				string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
				if (localizedFailureMessage != null)
				{
					ILobbyClientSessionHandler handler = this._handler;
					if (handler != null)
					{
						handler.OnSystemMessageReceived(localizedFailureMessage);
					}
				}
			}
			return callResult.Success;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0000AD0D File Offset: 0x00008F0D
		public void DestroyClan()
		{
			this.CheckAndSendMessage(new DestroyClanMessage());
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0000AD1A File Offset: 0x00008F1A
		public void InviteToClan(PlayerId invitedPlayerId, bool dontUseNameForUnknownPlayer)
		{
			this.CheckAndSendMessage(new InviteToClanMessage(invitedPlayerId, dontUseNameForUnknownPlayer));
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0000AD2C File Offset: 0x00008F2C
		public async void CreatePremadeGame(string name, string gameType, string mapName, string factionA, string factionB, string password, PremadeGameType premadeGameType, string spectatorPassword, int maxSpectatorCount)
		{
			this.CurrentState = LobbyClient.State.WaitingToCreatePremadeGame;
			string text = ((!string.IsNullOrEmpty(password)) ? Common.CalculateMD5Hash(password) : null);
			string text2 = ((!string.IsNullOrEmpty(spectatorPassword)) ? Common.CalculateMD5Hash(spectatorPassword) : null);
			CallResult callResult = await base.CallFunction<CreatePremadeGameMessageResult>(new CreatePremadeGameMessage(name, gameType, mapName, factionA, factionB, text, premadeGameType, text2, maxSpectatorCount));
			CreatePremadeGameMessageResult createPremadeGameMessageResult = callResult.Result as CreatePremadeGameMessageResult;
			if (!callResult.Success || createPremadeGameMessageResult == null || !createPremadeGameMessageResult.Successful)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0000ADB3 File Offset: 0x00008FB3
		public void CancelCreatingPremadeGame()
		{
			this.CheckAndSendMessage(new CancelCreatingPremadeGameMessage());
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0000ADC0 File Offset: 0x00008FC0
		public void RequestToJoinPremadeGame(Guid gameId, string password)
		{
			string text = ((!string.IsNullOrEmpty(password)) ? Common.CalculateMD5Hash(password) : null);
			this.CheckAndSendMessage(new RequestToJoinPremadeGameMessage(gameId, text));
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0000ADEC File Offset: 0x00008FEC
		public void AcceptJoinPremadeGameRequest(Guid partyId)
		{
			this.CheckAndSendMessage(new AcceptJoinPremadeGameRequestMessage(partyId));
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0000ADFA File Offset: 0x00008FFA
		public void DeclineJoinPremadeGameRequest(Guid partyId)
		{
			this.CheckAndSendMessage(new DeclineJoinPremadeGameRequestMessage(partyId));
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0000AE08 File Offset: 0x00009008
		public void InviteToParty(PlayerId playerId, bool dontUseNameForUnknownPlayer)
		{
			this.CheckAndSendMessage(new InviteToPartyMessage(playerId, dontUseNameForUnknownPlayer));
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0000AE17 File Offset: 0x00009017
		public void DisbandParty()
		{
			this.CheckAndSendMessage(new DisbandPartyMessage());
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0000AE24 File Offset: 0x00009024
		public void KickPlayerFromParty(PlayerId playerId)
		{
			this.CheckAndSendMessage(new KickPlayerFromPartyMessage(playerId));
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0000AE32 File Offset: 0x00009032
		public void OnPlayerNameUpdated(string name)
		{
			this._userName = name;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0000AE3B File Offset: 0x0000903B
		public void ToggleUseClanSigil(bool isUsed)
		{
			this.CheckAndSendMessage(new UpdateUsingClanSigil(isUsed));
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0000AE49 File Offset: 0x00009049
		public void PromotePlayerToPartyLeader(PlayerId playerId)
		{
			this.CheckAndSendMessage(new PromotePlayerToPartyLeaderMessage(playerId));
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0000AE58 File Offset: 0x00009058
		public async Task<bool> ChangeSigil(string sigilId)
		{
			CallResult callResult = await base.CallFunction<FunctionResult>(new ChangePlayerSigilMessage(sigilId));
			if (!callResult.Success)
			{
				string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
				if (localizedFailureMessage != null)
				{
					ILobbyClientSessionHandler handler = this._handler;
					if (handler != null)
					{
						handler.OnSystemMessageReceived(localizedFailureMessage);
					}
				}
			}
			return callResult.Success;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0000AEA8 File Offset: 0x000090A8
		public async Task<bool> InviteToPlatformSession(PlayerId playerId)
		{
			bool flag = false;
			if (this._handler != null)
			{
				flag = await this._handler.OnInviteToPlatformSession(playerId);
			}
			return flag;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0000AEF8 File Offset: 0x000090F8
		public async void EndCustomGame()
		{
			FunctionResult result = (await base.CallFunction<EndHostingCustomGameResult>(new EndHostingCustomGameMessage())).Result;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnCustomGameEnd();
			}
			this.CurrentState = LobbyClient.State.AtLobby;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0000AF34 File Offset: 0x00009134
		public async void RegisterCustomGame(string gameModule, string gameType, string serverName, int maxPlayerCount, string map, string uniqueMapId, string gamePassword, string adminPassword, string spectatorPassword, int port, int maxSpectatorCount, bool enableSpectators = false)
		{
			this.CustomGameType = gameType;
			this.CustomGameScene = map;
			this.CurrentState = LobbyClient.State.WaitingToRegisterCustomGame;
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<RegisterCustomGameResult>(new RegisterCustomGameMessage(gameModule, gameType, serverName, maxPlayerCount, map, uniqueMapId, gamePassword, adminPassword, spectatorPassword, port, maxSpectatorCount, enableSpectators)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			RegisterCustomGameResult registerCustomGameResult = taskAwaiter.GetResult().Result as RegisterCustomGameResult;
			Debug.Print("Register custom game server response received", 0, Debug.DebugColor.White, 17592186044416UL);
			if (registerCustomGameResult != null && registerCustomGameResult.Success)
			{
				this.CurrentState = LobbyClient.State.HostingCustomGame;
				ILobbyClientSessionHandler handler = this._handler;
				if (handler != null)
				{
					handler.OnRegisterCustomGameServerResponse();
				}
			}
			else
			{
				this.CurrentState = LobbyClient.State.AtLobby;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0000AFD6 File Offset: 0x000091D6
		public void UpdateCustomGameData(string newGameType, string newMap, int newCount)
		{
			base.SendMessage(new UpdateCustomGameData(newGameType, newMap, newCount));
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0000AFE6 File Offset: 0x000091E6
		public void ResponseCustomGameClientConnection(PlayerJoinGameResponseDataFromHost[] playerJoinData)
		{
			base.SendMessage(new ResponseCustomGameClientConnectionMessage(playerJoinData));
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0000AFF4 File Offset: 0x000091F4
		public void AcceptPartyInvitation()
		{
			this.IsPartyInvitationPopupActive = false;
			this.CheckAndSendMessage(new AcceptPartyInvitationMessage());
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0000B008 File Offset: 0x00009208
		public void DeclinePartyInvitation()
		{
			this.IsPartyInvitationPopupActive = false;
			this.CheckAndSendMessage(new DeclinePartyInvitationMessage());
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0000B01C File Offset: 0x0000921C
		public void AcceptPartyJoinRequest(PlayerId playerId)
		{
			this.IsPartyJoinRequestPopupActive = false;
			this.CheckAndSendMessage(new AcceptPartyJoinRequestMessage(playerId));
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0000B031 File Offset: 0x00009231
		public void DeclinePartyJoinRequest(PlayerId playerId, PartyJoinDeclineReason reason)
		{
			this.IsPartyJoinRequestPopupActive = false;
			this.CheckAndSendMessage(new DeclinePartyJoinRequestMessage(playerId, reason));
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0000B048 File Offset: 0x00009248
		public async Task<bool> UpdateCharacter(BodyProperties bodyProperties, bool isFemale)
		{
			this.AssertCanPerformLobbyActions();
			PlayerData playerData = this.PlayerData;
			BodyProperties? previousBodyProperties = ((playerData != null) ? new BodyProperties?(playerData.BodyProperties) : null);
			PlayerData playerData2 = this.PlayerData;
			bool? previousIsFemale = ((playerData2 != null) ? new bool?(playerData2.IsFemale) : null);
			if (this.CanPerformLobbyActions && this.PlayerData != null)
			{
				this.PlayerData.BodyProperties = bodyProperties;
				this.PlayerData.IsFemale = isFemale;
			}
			CallResult callResult = await base.CallFunction<FunctionResult>(new UpdateCharacterMessage(bodyProperties, isFemale));
			bool flag;
			if (!callResult.Success)
			{
				if (this.PlayerData != null && previousBodyProperties != null)
				{
					this.PlayerData.BodyProperties = previousBodyProperties.Value;
					this.PlayerData.IsFemale = previousIsFemale ?? false;
				}
				string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
				if (localizedFailureMessage != null)
				{
					ILobbyClientSessionHandler handler = this._handler;
					if (handler != null)
					{
						handler.OnSystemMessageReceived(localizedFailureMessage);
					}
				}
				flag = false;
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0000B0A0 File Offset: 0x000092A0
		public async Task<bool> UpdateShownBadgeId(string shownBadgeId)
		{
			this.AssertCanPerformLobbyActions();
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<UpdateShownBadgeIdMessageResult>(new UpdateShownBadgeIdMessage(shownBadgeId)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			UpdateShownBadgeIdMessageResult updateShownBadgeIdMessageResult = taskAwaiter.GetResult().Result as UpdateShownBadgeIdMessageResult;
			if (updateShownBadgeIdMessageResult != null && updateShownBadgeIdMessageResult.Successful)
			{
				this.PlayerData.ShownBadgeId = shownBadgeId;
			}
			return updateShownBadgeIdMessageResult != null && updateShownBadgeIdMessageResult.Successful;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0000B0F0 File Offset: 0x000092F0
		public async Task<AnotherPlayerData> GetAnotherPlayerState(PlayerId playerId)
		{
			this.AssertCanPerformLobbyActions();
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetAnotherPlayerStateMessageResult>(new GetAnotherPlayerStateMessage(playerId)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetAnotherPlayerStateMessageResult getAnotherPlayerStateMessageResult = taskAwaiter.GetResult().Result as GetAnotherPlayerStateMessageResult;
			AnotherPlayerData anotherPlayerData;
			if (getAnotherPlayerStateMessageResult != null)
			{
				anotherPlayerData = getAnotherPlayerStateMessageResult.AnotherPlayerData;
			}
			else
			{
				anotherPlayerData = new AnotherPlayerData(AnotherPlayerState.NoAnswer, 0);
			}
			return anotherPlayerData;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0000B140 File Offset: 0x00009340
		public async Task<PlayerData> GetAnotherPlayerData(PlayerId playerID)
		{
			this.AssertCanPerformLobbyActions();
			await this.WaitForPendingRequestCompletion(LobbyClient.PendingRequest.PlayerData, playerID);
			PlayerData playerData;
			PlayerData playerData2;
			if (this._cachedPlayerDatas.TryGetValue(playerID, out playerData))
			{
				playerData2 = playerData;
			}
			else
			{
				GetAnotherPlayerDataMessageResult getAnotherPlayerDataMessageResult = await this.CreatePendingRequest<GetAnotherPlayerDataMessageResult>(LobbyClient.PendingRequest.PlayerData, playerID, base.CallFunction<GetAnotherPlayerDataMessageResult>(new GetAnotherPlayerDataMessage(playerID)));
				if (((getAnotherPlayerDataMessageResult != null) ? getAnotherPlayerDataMessageResult.AnotherPlayerData : null) != null)
				{
					this._cachedPlayerDatas[playerID] = getAnotherPlayerDataMessageResult.AnotherPlayerData;
				}
				playerData2 = ((getAnotherPlayerDataMessageResult != null) ? getAnotherPlayerDataMessageResult.AnotherPlayerData : null);
			}
			return playerData2;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0000B190 File Offset: 0x00009390
		public async Task<MatchmakingQueueStats> GetPlayerCountInQueue()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetPlayerCountInQueueResult>(new GetPlayerCountInQueue()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetPlayerCountInQueueResult getPlayerCountInQueueResult = taskAwaiter.GetResult().Result as GetPlayerCountInQueueResult;
			MatchmakingQueueStats matchmakingQueueStats;
			if (getPlayerCountInQueueResult != null)
			{
				matchmakingQueueStats = getPlayerCountInQueueResult.MatchmakingQueueStats;
			}
			else
			{
				matchmakingQueueStats = MatchmakingQueueStats.Empty;
			}
			return matchmakingQueueStats;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0000B1D8 File Offset: 0x000093D8
		public async Task<List<ValueTuple<PlayerId, AnotherPlayerData>>> GetOtherPlayersState(List<PlayerId> players)
		{
			this.AssertCanPerformLobbyActions();
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetOtherPlayersStateMessageResult>(new GetOtherPlayersStateMessage(players)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetOtherPlayersStateMessageResult getOtherPlayersStateMessageResult = taskAwaiter.GetResult().Result as GetOtherPlayersStateMessageResult;
			return (getOtherPlayersStateMessageResult != null) ? getOtherPlayersStateMessageResult.States : null;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0000B228 File Offset: 0x00009428
		public async Task<MatchmakingWaitTimeStats> GetMatchmakingWaitTimes()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetAverageMatchmakingWaitTimesResult>(new GetAverageMatchmakingWaitTimesMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetAverageMatchmakingWaitTimesResult getAverageMatchmakingWaitTimesResult = taskAwaiter.GetResult().Result as GetAverageMatchmakingWaitTimesResult;
			MatchmakingWaitTimeStats matchmakingWaitTimeStats;
			if (getAverageMatchmakingWaitTimesResult != null)
			{
				matchmakingWaitTimeStats = getAverageMatchmakingWaitTimesResult.MatchmakingWaitTimeStats;
			}
			else
			{
				matchmakingWaitTimeStats = MatchmakingWaitTimeStats.Empty;
			}
			return matchmakingWaitTimeStats;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0000B270 File Offset: 0x00009470
		public async Task<Badge[]> GetPlayerBadges()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetPlayerBadgesMessageResult>(new GetPlayerBadgesMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetPlayerBadgesMessageResult getPlayerBadgesMessageResult = taskAwaiter.GetResult().Result as GetPlayerBadgesMessageResult;
			List<Badge> list = new List<Badge>();
			if (getPlayerBadgesMessageResult != null)
			{
				string[] badges = getPlayerBadgesMessageResult.Badges;
				for (int i = 0; i < badges.Length; i++)
				{
					Badge byId = BadgeManager.GetById(badges[i]);
					if (byId != null)
					{
						list.Add(byId);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0000B2B8 File Offset: 0x000094B8
		public async Task<PlayerStatsBase[]> GetPlayerStats(PlayerId playerID)
		{
			PlayerStatsBase[] array;
			PlayerStatsBase[] array2;
			if (this._cachedPlayerStats.TryGetValue(playerID, out array))
			{
				array2 = array;
			}
			else
			{
				TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetPlayerStatsMessageResult>(new GetPlayerStatsMessage(playerID)).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<CallResult> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<CallResult>);
				}
				GetPlayerStatsMessageResult getPlayerStatsMessageResult = taskAwaiter.GetResult().Result as GetPlayerStatsMessageResult;
				if (((getPlayerStatsMessageResult != null) ? getPlayerStatsMessageResult.PlayerStats : null) != null)
				{
					this._cachedPlayerStats[playerID] = getPlayerStatsMessageResult.PlayerStats;
				}
				array2 = ((getPlayerStatsMessageResult != null) ? getPlayerStatsMessageResult.PlayerStats : null);
			}
			return array2;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0000B308 File Offset: 0x00009508
		public async Task<GameTypeRankInfo[]> GetGameTypeRankInfo(PlayerId playerID)
		{
			await this.WaitForPendingRequestCompletion(LobbyClient.PendingRequest.RankInfo, playerID);
			GameTypeRankInfo[] array;
			GameTypeRankInfo[] array2;
			if (this._cachedRankInfos.TryGetValue(playerID, out array))
			{
				array2 = array;
			}
			else
			{
				GetPlayerGameTypeRankInfoMessageResult getPlayerGameTypeRankInfoMessageResult = await this.CreatePendingRequest<GetPlayerGameTypeRankInfoMessageResult>(LobbyClient.PendingRequest.RankInfo, playerID, base.CallFunction<GetPlayerGameTypeRankInfoMessageResult>(new GetPlayerGameTypeRankInfoMessage(playerID)));
				if (((getPlayerGameTypeRankInfoMessageResult != null) ? getPlayerGameTypeRankInfoMessageResult.GameTypeRankInfo : null) != null)
				{
					this._cachedRankInfos[playerID] = getPlayerGameTypeRankInfoMessageResult.GameTypeRankInfo;
				}
				array2 = ((getPlayerGameTypeRankInfoMessageResult != null) ? getPlayerGameTypeRankInfoMessageResult.GameTypeRankInfo : null);
			}
			return array2;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0000B358 File Offset: 0x00009558
		public async Task<int> GetRankedLeaderboardCount(string gameType)
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetRankedLeaderboardCountMessageResult>(new GetRankedLeaderboardCountMessage(gameType)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetRankedLeaderboardCountMessageResult getRankedLeaderboardCountMessageResult = taskAwaiter.GetResult().Result as GetRankedLeaderboardCountMessageResult;
			return (getRankedLeaderboardCountMessageResult != null) ? getRankedLeaderboardCountMessageResult.Count : 0;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0000B3A8 File Offset: 0x000095A8
		public async Task<PlayerLeaderboardData[]> GetRankedLeaderboard(string gameType, int startIndex, int count)
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetRankedLeaderboardMessageResult>(new GetRankedLeaderboardMessage(gameType, startIndex, count)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetRankedLeaderboardMessageResult getRankedLeaderboardMessageResult = taskAwaiter.GetResult().Result as GetRankedLeaderboardMessageResult;
			return (getRankedLeaderboardMessageResult != null) ? getRankedLeaderboardMessageResult.LeaderboardPlayers : null;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0000B405 File Offset: 0x00009605
		public void SendCreateClanMessage(string clanName, string clanTag, string clanFaction, string clanSigil)
		{
			this.AssertCanPerformLobbyActions();
			base.SendMessage(new CreateClanMessage(clanName, clanTag, clanFaction, clanSigil));
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0000B41D File Offset: 0x0000961D
		public void GetFriendList()
		{
			this.CheckAndSendMessage(new GetFriendListMessage());
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0000B42A File Offset: 0x0000962A
		public void AddFriend(PlayerId friendId, bool dontUseNameForUnknownPlayer)
		{
			this.CheckAndSendMessage(new AddFriendMessage(friendId, dontUseNameForUnknownPlayer));
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0000B439 File Offset: 0x00009639
		public void RemoveFriend(PlayerId friendId)
		{
			this.CheckAndSendMessage(new RemoveFriendMessage(friendId));
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0000B447 File Offset: 0x00009647
		public void RespondToFriendRequest(PlayerId playerId, bool dontUseNameForUnknownPlayer, bool isAccepted, bool isBlocked = false)
		{
			this.CheckAndSendMessage(new FriendRequestResponseMessage(playerId, dontUseNameForUnknownPlayer, isAccepted, isBlocked));
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0000B45C File Offset: 0x0000965C
		public void ReportPlayer(string gameId, PlayerId player, string playerName, PlayerReportType type, string message)
		{
			Guid guid;
			if (Guid.TryParse(gameId, out guid))
			{
				this.CheckAndSendMessage(new ReportPlayerMessage(guid, player, playerName, type, message));
				return;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnSystemMessageReceived(new TextObject("{=dnKQbXIZ}Could not report player: Game does not exist.", null).ToString());
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0000B4A8 File Offset: 0x000096A8
		public async Task<bool> ChangeUsername(string username)
		{
			bool flag;
			if ((this.PlayerData == null || this.PlayerData.Username != username) && username != null && username.Length >= Parameters.UsernameMinLength && username.Length <= Parameters.UsernameMaxLength && Common.IsAllLetters(username))
			{
				CallResult callResult = await base.CallFunction<FunctionResult>(new ChangeUsernameMessage(username));
				if (!callResult.Success)
				{
					string localizedFailureMessage = LobbyClient.GetLocalizedFailureMessage(callResult.SuccessfulReason);
					if (localizedFailureMessage != null)
					{
						ILobbyClientSessionHandler handler = this._handler;
						if (handler != null)
						{
							handler.OnSystemMessageReceived(localizedFailureMessage);
						}
					}
				}
				flag = callResult.Success;
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0000B4F8 File Offset: 0x000096F8
		public void AddFriendByUsernameAndId(string username, int userId, bool dontUseNameForUnknownPlayer)
		{
			if (username != null && username.Length >= Parameters.UsernameMinLength && username.Length <= Parameters.UsernameMaxLength && Common.IsAllLetters(username) && userId >= 0 && userId <= Parameters.UserIdMax)
			{
				this.CheckAndSendMessage(new AddFriendByUsernameAndIdMessage(username, userId, dontUseNameForUnknownPlayer));
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0000B544 File Offset: 0x00009744
		public async Task<bool> DoesPlayerWithUsernameAndIdExist(string username, int userId)
		{
			bool flag;
			if (username != null && username.Length >= Parameters.UsernameMinLength && username.Length <= Parameters.UsernameMaxLength && Common.IsAllLetters(username) && userId >= 0 && userId <= Parameters.UserIdMax)
			{
				TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetPlayerByUsernameAndIdMessageResult>(new GetPlayerByUsernameAndIdMessage(username, userId)).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<CallResult> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<CallResult>);
				}
				GetPlayerByUsernameAndIdMessageResult getPlayerByUsernameAndIdMessageResult = taskAwaiter.GetResult().Result as GetPlayerByUsernameAndIdMessageResult;
				flag = getPlayerByUsernameAndIdMessageResult != null && getPlayerByUsernameAndIdMessageResult.PlayerId.IsValid;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0000B59C File Offset: 0x0000979C
		public bool IsPlayerClanLeader(PlayerId playerID)
		{
			ClanPlayer clanPlayer = this.PlayersInClan.Find((ClanPlayer p) => p.PlayerId == playerID);
			return clanPlayer != null && clanPlayer.Role == ClanPlayerRole.Leader;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0000B5DC File Offset: 0x000097DC
		public bool IsPlayerClanOfficer(PlayerId playerID)
		{
			ClanPlayer clanPlayer = this.PlayersInClan.Find((ClanPlayer p) => p.PlayerId == playerID);
			return clanPlayer != null && clanPlayer.Role == ClanPlayerRole.Officer;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0000B61C File Offset: 0x0000981C
		public async Task<bool> UpdateUsedCosmeticItems([TupleElementNames(new string[] { "cosmeticId", "isEquipped" })] Dictionary<string, List<ValueTuple<string, bool>>> usedCosmetics)
		{
			List<CosmeticItemInfo> list = new List<CosmeticItemInfo>();
			foreach (string text in usedCosmetics.Keys)
			{
				foreach (ValueTuple<string, bool> valueTuple in usedCosmetics[text])
				{
					CosmeticItemInfo cosmeticItemInfo = new CosmeticItemInfo(text, valueTuple.Item1, valueTuple.Item2);
					list.Add(cosmeticItemInfo);
				}
			}
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<UpdateUsedCosmeticItemsMessageResult>(new UpdateUsedCosmeticItemsMessage(list)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			UpdateUsedCosmeticItemsMessageResult updateUsedCosmeticItemsMessageResult = taskAwaiter.GetResult().Result as UpdateUsedCosmeticItemsMessageResult;
			if (updateUsedCosmeticItemsMessageResult != null && updateUsedCosmeticItemsMessageResult.Successful)
			{
				foreach (KeyValuePair<string, List<ValueTuple<string, bool>>> keyValuePair in usedCosmetics)
				{
					if (!string.IsNullOrWhiteSpace(keyValuePair.Key))
					{
						List<string> list2;
						if (!this.UsedCosmetics.TryGetValue(keyValuePair.Key, out list2))
						{
							list2 = new List<string>();
							this._usedCosmetics.Add(keyValuePair.Key, list2);
						}
						foreach (ValueTuple<string, bool> valueTuple2 in keyValuePair.Value)
						{
							string item = valueTuple2.Item1;
							if (valueTuple2.Item2)
							{
								list2.Add(item);
							}
							else
							{
								list2.Remove(item);
							}
						}
					}
				}
			}
			return updateUsedCosmeticItemsMessageResult != null && updateUsedCosmeticItemsMessageResult.Successful;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0000B66C File Offset: 0x0000986C
		[return: TupleElementNames(new string[] { "isSuccessful", "finalGold" })]
		public async Task<ValueTuple<bool, int>> BuyCosmetic(string cosmeticId)
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<BuyCosmeticMessageResult>(new BuyCosmeticMessage(cosmeticId)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			BuyCosmeticMessageResult buyCosmeticMessageResult = taskAwaiter.GetResult().Result as BuyCosmeticMessageResult;
			if (buyCosmeticMessageResult != null && buyCosmeticMessageResult.Successful)
			{
				this._ownedCosmetics.Add(cosmeticId);
			}
			return new ValueTuple<bool, int>(buyCosmeticMessageResult != null && buyCosmeticMessageResult.Successful, (buyCosmeticMessageResult != null) ? buyCosmeticMessageResult.Gold : 0);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0000B6BC File Offset: 0x000098BC
		[return: TupleElementNames(new string[] { "isSuccessful", "ownedCosmetics", "usedCosmetics" })]
		public async Task<ValueTuple<bool, List<string>, Dictionary<string, List<string>>>> GetCosmeticsInfo()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetUserCosmeticsInfoMessageResult>(new GetUserCosmeticsInfoMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetUserCosmeticsInfoMessageResult getUserCosmeticsInfoMessageResult = taskAwaiter.GetResult().Result as GetUserCosmeticsInfoMessageResult;
			if (getUserCosmeticsInfoMessageResult != null)
			{
				this._usedCosmetics = getUserCosmeticsInfoMessageResult.UsedCosmetics ?? new Dictionary<string, List<string>>();
				this._ownedCosmetics = getUserCosmeticsInfoMessageResult.OwnedCosmetics ?? new List<string>();
			}
			return new ValueTuple<bool, List<string>, Dictionary<string, List<string>>>(getUserCosmeticsInfoMessageResult != null && getUserCosmeticsInfoMessageResult.Successful, (getUserCosmeticsInfoMessageResult != null) ? getUserCosmeticsInfoMessageResult.OwnedCosmetics : null, (getUserCosmeticsInfoMessageResult != null) ? getUserCosmeticsInfoMessageResult.UsedCosmetics : null);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0000B704 File Offset: 0x00009904
		public async Task<string> GetDedicatedCustomServerAuthToken()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetDedicatedCustomServerAuthTokenMessageResult>(new GetDedicatedCustomServerAuthTokenMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetDedicatedCustomServerAuthTokenMessageResult getDedicatedCustomServerAuthTokenMessageResult = taskAwaiter.GetResult().Result as GetDedicatedCustomServerAuthTokenMessageResult;
			return (getDedicatedCustomServerAuthTokenMessageResult != null) ? getDedicatedCustomServerAuthTokenMessageResult.AuthToken : null;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0000B74C File Offset: 0x0000994C
		public async Task<string> GetOfficialServerProviderName()
		{
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<GetOfficialServerProviderNameResult>(new GetOfficialServerProviderNameMessage()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			GetOfficialServerProviderNameResult getOfficialServerProviderNameResult = taskAwaiter.GetResult().Result as GetOfficialServerProviderNameResult;
			return ((getOfficialServerProviderNameResult != null) ? getOfficialServerProviderNameResult.Name : null) ?? string.Empty;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0000B794 File Offset: 0x00009994
		public async Task<string> GetPlayerBannerlordID(PlayerId playerId)
		{
			await this.WaitForPendingRequestCompletion(LobbyClient.PendingRequest.BannerlordID, playerId);
			string text;
			string text2;
			if (this._cachedPlayerBannerlordIDs.TryGetValue(playerId, out text))
			{
				text2 = text;
			}
			else
			{
				GetBannerlordIDMessageResult getBannerlordIDMessageResult = await this.CreatePendingRequest<GetBannerlordIDMessageResult>(LobbyClient.PendingRequest.BannerlordID, playerId, base.CallFunction<GetBannerlordIDMessageResult>(new GetBannerlordIDMessage(playerId)));
				if (getBannerlordIDMessageResult != null && getBannerlordIDMessageResult.BannerlordID != null)
				{
					this._cachedPlayerBannerlordIDs[playerId] = getBannerlordIDMessageResult.BannerlordID;
				}
				text2 = ((getBannerlordIDMessageResult != null) ? getBannerlordIDMessageResult.BannerlordID : null) ?? string.Empty;
			}
			return text2;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0000B7E4 File Offset: 0x000099E4
		public bool IsKnownPlayer(PlayerId playerID)
		{
			bool flag = playerID == this._playerId;
			bool flag2 = this.FriendIDs.Contains(playerID);
			bool flag3 = this.IsInParty && this.PlayersInParty.Any<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient p) => p.PlayerId.Equals(playerID));
			bool flag4 = this.IsInClan && this.PlayersInClan.Any<ClanPlayer>((ClanPlayer p) => p.PlayerId.Equals(playerID));
			return flag || flag2 || flag3 || flag4;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0000B870 File Offset: 0x00009A70
		public async Task<long> GetPingToServer(string IpAddress)
		{
			long num;
			try
			{
				using (Ping ping = new Ping())
				{
					PingReply pingReply = await ping.SendPingAsync(IpAddress, (int)TimeSpan.FromSeconds(15.0).TotalMilliseconds);
					num = ((pingReply.Status != IPStatus.Success) ? (-1L) : pingReply.RoundtripTime);
				}
			}
			catch (Exception)
			{
				num = -1L;
			}
			return num;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0000B8B5 File Offset: 0x00009AB5
		private void AssertCanPerformLobbyActions()
		{
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0000B8B8 File Offset: 0x00009AB8
		public async Task<bool> SendPSPlayerJoinedToPlayerSessionMessage(ulong inviterPlayerId)
		{
			PSPlayerJoinedToPlayerSessionMessage psplayerJoinedToPlayerSessionMessage = new PSPlayerJoinedToPlayerSessionMessage(inviterPlayerId);
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<PSPlayerJoinedToPlayerSessionMessageResult>(psplayerJoinedToPlayerSessionMessage).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			PSPlayerJoinedToPlayerSessionMessageResult psplayerJoinedToPlayerSessionMessageResult = taskAwaiter.GetResult().Result as PSPlayerJoinedToPlayerSessionMessageResult;
			return psplayerJoinedToPlayerSessionMessageResult != null && psplayerJoinedToPlayerSessionMessageResult.Successful;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0000B908 File Offset: 0x00009B08
		public async Task<bool> SendPlatformPlayerJoinedToPlayerSessionMessage(PlayerId inviterPlayerId)
		{
			PlatformPlayerJoinedToPlayerSessionMessage platformPlayerJoinedToPlayerSessionMessage = new PlatformPlayerJoinedToPlayerSessionMessage(inviterPlayerId);
			TaskAwaiter<CallResult> taskAwaiter = base.CallFunction<PSPlayerJoinedToPlayerSessionMessageResult>(platformPlayerJoinedToPlayerSessionMessage).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CallResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CallResult>);
			}
			PSPlayerJoinedToPlayerSessionMessageResult psplayerJoinedToPlayerSessionMessageResult = taskAwaiter.GetResult().Result as PSPlayerJoinedToPlayerSessionMessageResult;
			return psplayerJoinedToPlayerSessionMessageResult != null && psplayerJoinedToPlayerSessionMessageResult.Successful;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0000B958 File Offset: 0x00009B58
		private Task WaitForPendingRequestCompletion(LobbyClient.PendingRequest requestType, PlayerId playerId)
		{
			Task task;
			if (this._pendingPlayerRequests.TryGetValue(new ValueTuple<LobbyClient.PendingRequest, PlayerId>(requestType, playerId), out task))
			{
				return task;
			}
			return Task.CompletedTask;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0000B984 File Offset: 0x00009B84
		private static string GetLocalizedFailureMessage(string successfulReason)
		{
			if (string.IsNullOrEmpty(successfulReason))
			{
				return null;
			}
			ServerInfoMessage serverInfoMessage;
			if (Enum.TryParse<ServerInfoMessage>(successfulReason, out serverInfoMessage))
			{
				return new SystemMessage(serverInfoMessage, Array.Empty<string>()).GetDescription().ToString();
			}
			return null;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0000B9BC File Offset: 0x00009BBC
		private async Task<T> CreatePendingRequest<T>(LobbyClient.PendingRequest requestType, PlayerId playerId, Task<CallResult> requestTask) where T : FunctionResult
		{
			ValueTuple<LobbyClient.PendingRequest, PlayerId> key = new ValueTuple<LobbyClient.PendingRequest, PlayerId>(requestType, playerId);
			T t;
			try
			{
				this._pendingPlayerRequests[key] = requestTask;
				TaskAwaiter<CallResult> taskAwaiter = requestTask.GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<CallResult> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<CallResult>);
				}
				t = taskAwaiter.GetResult().Result as T;
			}
			finally
			{
				this._pendingPlayerRequests.Remove(key);
			}
			return t;
		}

		// Token: 0x040002FD RID: 765
		public const string TestRegionCode = "Test";

		// Token: 0x040002FE RID: 766
		private static readonly int ServerStatusCheckDelay = 120000;

		// Token: 0x040002FF RID: 767
		private static int _friendListCheckDelay;

		// Token: 0x04000300 RID: 768
		private static readonly int CheckForCustomGamesCount = 5;

		// Token: 0x04000301 RID: 769
		private static readonly int CheckForCustomGamesDelay = 5000;

		// Token: 0x04000302 RID: 770
		private ILobbyClientSessionHandler _handler;

		// Token: 0x04000303 RID: 771
		private readonly Stopwatch _serverStatusTimer;

		// Token: 0x04000304 RID: 772
		private readonly Stopwatch _friendListTimer;

		// Token: 0x04000305 RID: 773
		private readonly Stopwatch _recentPlayersTimer;

		// Token: 0x04000306 RID: 774
		private static readonly int RecentPlayersCheckDelay = 40000;

		// Token: 0x0400030B RID: 779
		private List<string> _ownedCosmetics;

		// Token: 0x0400030C RID: 780
		private Dictionary<string, List<string>> _usedCosmetics;

		// Token: 0x0400030F RID: 783
		private ServerStatus _serverStatus;

		// Token: 0x04000310 RID: 784
		private DateTime _matchmakerBlockedTime;

		// Token: 0x04000311 RID: 785
		private TextObject _logOutReason;

		// Token: 0x04000312 RID: 786
		private LobbyClient.State _state;

		// Token: 0x04000313 RID: 787
		private string _userName;

		// Token: 0x04000314 RID: 788
		private PlayerId _playerId;

		// Token: 0x04000318 RID: 792
		private List<ModuleInfoModel> _loadedUnofficialModules;

		// Token: 0x04000329 RID: 809
		private TimedDictionaryCache<PlayerId, GameTypeRankInfo[]> _cachedRankInfos;

		// Token: 0x0400032A RID: 810
		private TimedDictionaryCache<PlayerId, PlayerStatsBase[]> _cachedPlayerStats;

		// Token: 0x0400032B RID: 811
		private TimedDictionaryCache<PlayerId, PlayerData> _cachedPlayerDatas;

		// Token: 0x0400032C RID: 812
		private TimedDictionaryCache<PlayerId, string> _cachedPlayerBannerlordIDs;

		// Token: 0x0400032D RID: 813
		private Dictionary<ValueTuple<LobbyClient.PendingRequest, PlayerId>, Task> _pendingPlayerRequests;

		// Token: 0x02000198 RID: 408
		public enum State
		{
			// Token: 0x040005CA RID: 1482
			Idle,
			// Token: 0x040005CB RID: 1483
			Working,
			// Token: 0x040005CC RID: 1484
			Connected,
			// Token: 0x040005CD RID: 1485
			SessionRequested,
			// Token: 0x040005CE RID: 1486
			AtLobby,
			// Token: 0x040005CF RID: 1487
			SearchingToRejoinBattle,
			// Token: 0x040005D0 RID: 1488
			RequestingToSearchBattle,
			// Token: 0x040005D1 RID: 1489
			RequestingToCancelSearchBattle,
			// Token: 0x040005D2 RID: 1490
			SearchingBattle,
			// Token: 0x040005D3 RID: 1491
			AtBattle,
			// Token: 0x040005D4 RID: 1492
			QuittingFromBattle,
			// Token: 0x040005D5 RID: 1493
			WaitingToCreatePremadeGame,
			// Token: 0x040005D6 RID: 1494
			WaitingToJoinPremadeGame,
			// Token: 0x040005D7 RID: 1495
			WaitingToRegisterCustomGame,
			// Token: 0x040005D8 RID: 1496
			HostingCustomGame,
			// Token: 0x040005D9 RID: 1497
			WaitingToJoinCustomGame,
			// Token: 0x040005DA RID: 1498
			InCustomGame
		}

		// Token: 0x02000199 RID: 409
		private enum PendingRequest
		{
			// Token: 0x040005DC RID: 1500
			RankInfo,
			// Token: 0x040005DD RID: 1501
			PlayerData,
			// Token: 0x040005DE RID: 1502
			BannerlordID
		}
	}
}
