using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000070 RID: 112
	public class MPLobbyClanMatchmakingRequestPopupVM : ViewModel
	{
		// Token: 0x06000AEE RID: 2798 RVA: 0x0002169A File Offset: 0x0001F89A
		public MPLobbyClanMatchmakingRequestPopupVM()
		{
			this.ChallengerPartyPlayers = new MBBindingList<MPLobbyPlayerBaseVM>();
			this.RefreshValues();
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x000216B4 File Offset: 0x0001F8B4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=1pwQgr04}Matchmaking Request", null).ToString();
			this.WantsToJoinText = new TextObject("{=WHKG5Rbq}This team wants to join the match you created.", null).ToString();
			this.DoYouAcceptText = new TextObject("{=xkV9g4le}Do you accept them as your opponent?", null).ToString();
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0002170C File Offset: 0x0001F90C
		public void OpenWith(string clanName, string clanSigilCode, Guid partyId, PlayerId[] challengerPlayerIDs, PlayerId challengerPartyLeaderID, PremadeGameType premadeGameType)
		{
			this.ChallengerPartyPlayers.Clear();
			this.IsClanMatch = false;
			this.IsPracticeMatch = false;
			this._partyId = partyId;
			if (premadeGameType == PremadeGameType.Clan)
			{
				this.IsClanMatch = true;
				this.ClanName = clanName;
				this.ClanSigil = new BannerImageIdentifierVM(new Banner(clanSigilCode), true);
			}
			else if (premadeGameType == PremadeGameType.Practice)
			{
				this.IsPracticeMatch = true;
				this.ChallengerPartyLeader = new MPLobbyPlayerBaseVM(challengerPartyLeaderID, "", null, null);
				foreach (PlayerId playerId in challengerPlayerIDs)
				{
					this.ChallengerPartyPlayers.Add(new MPLobbyPlayerBaseVM(playerId, "", null, null));
				}
			}
			this.IsEnabled = true;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x000217B7 File Offset: 0x0001F9B7
		public void Close()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x000217C0 File Offset: 0x0001F9C0
		public void ExecuteAcceptMatchmaking()
		{
			NetworkMain.GameClient.AcceptJoinPremadeGameRequest(this._partyId);
			this.Close();
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x000217D8 File Offset: 0x0001F9D8
		public void ExecuteDeclineMatchmaking()
		{
			NetworkMain.GameClient.DeclineJoinPremadeGameRequest(this._partyId);
			this.Close();
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x000217F0 File Offset: 0x0001F9F0
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x000217F8 File Offset: 0x0001F9F8
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
					base.OnPropertyChanged("IsEnabled");
				}
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x00021815 File Offset: 0x0001FA15
		// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x0002181D File Offset: 0x0001FA1D
		[DataSourceProperty]
		public bool IsClanMatch
		{
			get
			{
				return this._isClanMatch;
			}
			set
			{
				if (value != this._isClanMatch)
				{
					this._isClanMatch = value;
					base.OnPropertyChanged("IsClanMatch");
				}
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x0002183A File Offset: 0x0001FA3A
		// (set) Token: 0x06000AF9 RID: 2809 RVA: 0x00021842 File Offset: 0x0001FA42
		[DataSourceProperty]
		public bool IsPracticeMatch
		{
			get
			{
				return this._isPracticeMatch;
			}
			set
			{
				if (value != this._isPracticeMatch)
				{
					this._isPracticeMatch = value;
					base.OnPropertyChanged("IsPracticeMatch");
				}
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x0002185F File Offset: 0x0001FA5F
		// (set) Token: 0x06000AFB RID: 2811 RVA: 0x00021867 File Offset: 0x0001FA67
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
					base.OnPropertyChanged("TitleText");
				}
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x00021889 File Offset: 0x0001FA89
		// (set) Token: 0x06000AFD RID: 2813 RVA: 0x00021891 File Offset: 0x0001FA91
		[DataSourceProperty]
		public string ClanName
		{
			get
			{
				return this._clanName;
			}
			set
			{
				if (value != this._clanName)
				{
					this._clanName = value;
					base.OnPropertyChanged("ClanName");
				}
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x000218B3 File Offset: 0x0001FAB3
		// (set) Token: 0x06000AFF RID: 2815 RVA: 0x000218BB File Offset: 0x0001FABB
		[DataSourceProperty]
		public string WantsToJoinText
		{
			get
			{
				return this._wantsToJoinText;
			}
			set
			{
				if (value != this._wantsToJoinText)
				{
					this._wantsToJoinText = value;
					base.OnPropertyChanged("WantsToJoinText");
				}
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x000218DD File Offset: 0x0001FADD
		// (set) Token: 0x06000B01 RID: 2817 RVA: 0x000218E5 File Offset: 0x0001FAE5
		[DataSourceProperty]
		public string DoYouAcceptText
		{
			get
			{
				return this._doYouAcceptText;
			}
			set
			{
				if (value != this._doYouAcceptText)
				{
					this._doYouAcceptText = value;
					base.OnPropertyChanged("DoYouAcceptText");
				}
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x00021907 File Offset: 0x0001FB07
		// (set) Token: 0x06000B03 RID: 2819 RVA: 0x0002190F File Offset: 0x0001FB0F
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanSigil
		{
			get
			{
				return this._clanSigil;
			}
			set
			{
				if (value != this._clanSigil)
				{
					this._clanSigil = value;
					base.OnPropertyChanged("ClanSigil");
				}
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x0002192C File Offset: 0x0001FB2C
		// (set) Token: 0x06000B05 RID: 2821 RVA: 0x00021934 File Offset: 0x0001FB34
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM ChallengerPartyLeader
		{
			get
			{
				return this._challengerPartyLeader;
			}
			set
			{
				if (value != this._challengerPartyLeader)
				{
					this._challengerPartyLeader = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerBaseVM>(value, "ChallengerPartyLeader");
				}
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00021952 File Offset: 0x0001FB52
		// (set) Token: 0x06000B07 RID: 2823 RVA: 0x0002195A File Offset: 0x0001FB5A
		[DataSourceProperty]
		public MBBindingList<MPLobbyPlayerBaseVM> ChallengerPartyPlayers
		{
			get
			{
				return this._challengerPartyPlayers;
			}
			set
			{
				if (value != this._challengerPartyPlayers)
				{
					this._challengerPartyPlayers = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyPlayerBaseVM>>(value, "ChallengerPartyPlayers");
				}
			}
		}

		// Token: 0x040004FC RID: 1276
		private Guid _partyId;

		// Token: 0x040004FD RID: 1277
		private bool _isEnabled;

		// Token: 0x040004FE RID: 1278
		private bool _isClanMatch;

		// Token: 0x040004FF RID: 1279
		private bool _isPracticeMatch;

		// Token: 0x04000500 RID: 1280
		private string _titleText;

		// Token: 0x04000501 RID: 1281
		private string _clanName;

		// Token: 0x04000502 RID: 1282
		private string _wantsToJoinText;

		// Token: 0x04000503 RID: 1283
		private string _doYouAcceptText;

		// Token: 0x04000504 RID: 1284
		private BannerImageIdentifierVM _clanSigil;

		// Token: 0x04000505 RID: 1285
		private MPLobbyPlayerBaseVM _challengerPartyLeader;

		// Token: 0x04000506 RID: 1286
		private MBBindingList<MPLobbyPlayerBaseVM> _challengerPartyPlayers;
	}
}
