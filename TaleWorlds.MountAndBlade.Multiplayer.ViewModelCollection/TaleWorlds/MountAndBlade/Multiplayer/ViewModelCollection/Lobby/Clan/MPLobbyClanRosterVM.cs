using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000073 RID: 115
	public class MPLobbyClanRosterVM : ViewModel
	{
		// Token: 0x06000B6A RID: 2922 RVA: 0x000224A8 File Offset: 0x000206A8
		public MPLobbyClanRosterVM()
		{
			this.MembersList = new MBBindingList<MPLobbyClanMemberItemVM>();
			this.MemberActionsList = new MBBindingList<StringPairItemWithActionVM>();
			this._memberComparer = new MPLobbyClanRosterVM.MemberComparer();
			this.RefreshValues();
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x000224D8 File Offset: 0x000206D8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RosterText = new TextObject("{=oyVeCtlg}Roster", null).ToString();
			this.NameText = new TextObject("{=PDdh1sBj}Name", null).ToString();
			this.BadgeText = new TextObject("{=4PrfimcK}Badge", null).ToString();
			this.StatusText = new TextObject("{=DXczLzml}Status", null).ToString();
			this.PromoteToClanOfficerHint = new HintViewModel(new TextObject("{=oeSrXaKt}You need to demote one of the officers", null), null);
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0002255C File Offset: 0x0002075C
		public void RefreshClanInformation(ClanHomeInfo info)
		{
			if (info == null || info.ClanInfo == null)
			{
				return;
			}
			this._isClanLeader = NetworkMain.GameClient.IsClanLeader;
			this._isClanOfficer = NetworkMain.GameClient.IsClanOfficer;
			this.MembersList.Clear();
			ClanPlayer[] players = info.ClanInfo.Players;
			for (int j = 0; j < players.Length; j++)
			{
				ClanPlayer member = players[j];
				if (!MultiplayerPlayerHelper.IsBlocked(member.PlayerId))
				{
					ClanPlayerInfo clanPlayerInfo = info.ClanPlayerInfos.First<ClanPlayerInfo>((ClanPlayerInfo i) => i.PlayerId.Equals(member.PlayerId));
					if (clanPlayerInfo != null)
					{
						bool flag = clanPlayerInfo.State == AnotherPlayerState.AtLobby || clanPlayerInfo.State == AnotherPlayerState.InMultiplayerGame || clanPlayerInfo.State == AnotherPlayerState.InParty;
						this.MembersList.Add(new MPLobbyClanMemberItemVM(member, flag, clanPlayerInfo.ActiveBadgeId, clanPlayerInfo.State, new Action<MPLobbyClanMemberItemVM>(this.ExecutePopulateActionsList)));
					}
				}
			}
			this.MembersList.Sort(this._memberComparer);
			this.IsPrivilegedMember = NetworkMain.GameClient.IsClanLeader || NetworkMain.GameClient.IsClanOfficer;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0002267C File Offset: 0x0002087C
		public void OnPlayerNameUpdated(string playerName)
		{
			for (int i = 0; i < this.MembersList.Count; i++)
			{
				MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = this.MembersList[i];
				if (mplobbyClanMemberItemVM.Id == NetworkMain.GameClient.PlayerID)
				{
					mplobbyClanMemberItemVM.UpdateNameAndAvatar(true);
				}
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x000226CC File Offset: 0x000208CC
		private void ExecutePopulateActionsList(MPLobbyClanMemberItemVM member)
		{
			this.MemberActionsList.Clear();
			if (NetworkMain.GameClient.PlayerID != member.Id)
			{
				if (this._isClanLeader)
				{
					this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecutePromoteToClanLeader), new TextObject("{=GRpGNYHW}Promote To Clan Leader", null).ToString(), "PromoteToClanLeader", member));
					if (NetworkMain.GameClient.IsPlayerClanOfficer(member.Id))
					{
						this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteDemoteFromClanOfficer), new TextObject("{=gowlLS2b}Demote From Clan Officer", null).ToString(), "DemoteFromClanOfficer", member));
					}
					else
					{
						StringPairItemWithActionVM stringPairItemWithActionVM = new StringPairItemWithActionVM(new Action<object>(this.ExecutePromoteToClanOfficer), new TextObject("{=BXI1ObU8}Promote To Clan Officer", null).ToString(), "PromoteToClanOfficer", member);
						if (NetworkMain.GameClient.PlayersInClan.Count<ClanPlayer>((ClanPlayer m) => m.Role == ClanPlayerRole.Officer) == Parameters.ClanOfficerCount)
						{
							stringPairItemWithActionVM.IsEnabled = false;
							stringPairItemWithActionVM.Hint = this.PromoteToClanOfficerHint;
						}
						this.MemberActionsList.Add(stringPairItemWithActionVM);
					}
				}
				if ((this._isClanOfficer || this._isClanLeader) && !NetworkMain.GameClient.IsPlayerClanLeader(member.Id) && (!this._isClanOfficer || !NetworkMain.GameClient.IsPlayerClanOfficer(member.Id)))
				{
					this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteKickFromClan), new TextObject("{=S8pZEPni}Kick From Clan", null).ToString(), "KickFromClan", member));
				}
				if (NetworkMain.GameClient.FriendInfos.All<FriendInfo>((FriendInfo f) => f.Id != member.Id))
				{
					this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteRequestFriendship), GameTexts.FindText("str_mp_scoreboard_context_request_friendship", null).ToString(), "RequestFriendship", member));
				}
				else
				{
					this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteTerminateFriendship), new TextObject("{=2YIVRuRa}Remove From Friends", null).ToString(), "TerminateFriendship", member));
				}
				if (NetworkMain.GameClient.SupportedFeatures.SupportsFeatures(Features.Party))
				{
					this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteInviteToParty), new TextObject("{=RzROgBkv}Invite To Party", null).ToString(), "InviteToParty", member));
				}
				MultiplayerPlayerContextMenuHelper.AddLobbyViewProfileOptions(member, this.MemberActionsList);
			}
			if (this.MemberActionsList.Count > 0)
			{
				this.IsMemberActionsActive = false;
				this.IsMemberActionsActive = true;
			}
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x000229A8 File Offset: 0x00020BA8
		private void ExecuteRequestFriendship(object memberObj)
		{
			MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = memberObj as MPLobbyClanMemberItemVM;
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(mplobbyClanMemberItemVM.Id);
			NetworkMain.GameClient.AddFriend(mplobbyClanMemberItemVM.Id, flag);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x000229EC File Offset: 0x00020BEC
		private void ExecuteTerminateFriendship(object memberObj)
		{
			MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = memberObj as MPLobbyClanMemberItemVM;
			NetworkMain.GameClient.RemoveFriend(mplobbyClanMemberItemVM.Id);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00022A10 File Offset: 0x00020C10
		private void ExecutePromoteToClanLeader(object memberObj)
		{
			MPLobbyClanMemberItemVM member = memberObj as MPLobbyClanMemberItemVM;
			GameTexts.SetVariable("MEMBER_NAME", member.Name);
			string text = new TextObject("{=GRpGNYHW}Promote To Clan Leader", null).ToString();
			string text2 = new TextObject("{=Z0TW2cub}Are you sure want to promote {MEMBER_NAME} as clan leader? You will lose your leadership.", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				this.PromoteToClanLeader(member.Id);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00022AB8 File Offset: 0x00020CB8
		private void ExecutePromoteToClanOfficer(object memberObj)
		{
			MPLobbyClanMemberItemVM member = memberObj as MPLobbyClanMemberItemVM;
			GameTexts.SetVariable("MEMBER_NAME", member.Name);
			string text = new TextObject("{=BXI1ObU8}Promote To Clan Officer", null).ToString();
			string text2 = new TextObject("{=MS4Ng2iw}Are you sure want to promote {MEMBER_NAME} as clan officer?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				this.PromoteToClanOfficer(member.Id);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00022B60 File Offset: 0x00020D60
		private void ExecuteDemoteFromClanOfficer(object memberObj)
		{
			MPLobbyClanMemberItemVM member = memberObj as MPLobbyClanMemberItemVM;
			GameTexts.SetVariable("MEMBER_NAME", member.Name);
			string text = new TextObject("{=gowlLS2b}Demote From Clan Officer", null).ToString();
			string text2 = new TextObject("{=pSb1P6ZA}Are you sure want to demote {MEMBER_NAME} from clan officers?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				this.DemoteFromClanOfficer(member.Id);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00022C08 File Offset: 0x00020E08
		private void ExecuteKickFromClan(object memberObj)
		{
			MPLobbyClanMemberItemVM member = memberObj as MPLobbyClanMemberItemVM;
			GameTexts.SetVariable("MEMBER_NAME", member.Name);
			string text = new TextObject("{=S8pZEPni}Kick From Clan", null).ToString();
			string text2 = new TextObject("{=L6eaNe2q}Are you sure want to kick {MEMBER_NAME} from clan?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				this.KickFromClan(member.Id);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00022CB0 File Offset: 0x00020EB0
		private void ExecuteInviteToParty(object memberObj)
		{
			MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = memberObj as MPLobbyClanMemberItemVM;
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(mplobbyClanMemberItemVM.Id);
			NetworkMain.GameClient.InviteToParty(mplobbyClanMemberItemVM.Id, flag);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00022CF3 File Offset: 0x00020EF3
		private void ExecuteViewProfile(object memberObj)
		{
			(memberObj as MPLobbyClanMemberItemVM).ExecuteShowProfile();
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00022D00 File Offset: 0x00020F00
		private async void PromoteToClanLeader(PlayerId playerId)
		{
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(playerId);
			await NetworkMain.GameClient.PromoteToClanLeader(playerId, flag);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00022D3C File Offset: 0x00020F3C
		private void PromoteToClanOfficer(PlayerId playerId)
		{
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(playerId);
			NetworkMain.GameClient.AssignAsClanOfficer(playerId, flag);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00022D6E File Offset: 0x00020F6E
		private void DemoteFromClanOfficer(PlayerId playerId)
		{
			NetworkMain.GameClient.RemoveClanOfficerRoleForPlayer(playerId);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00022D7B File Offset: 0x00020F7B
		private void KickFromClan(PlayerId playerId)
		{
			NetworkMain.GameClient.KickFromClan(playerId);
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00022D88 File Offset: 0x00020F88
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x00022D90 File Offset: 0x00020F90
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00022DAE File Offset: 0x00020FAE
		// (set) Token: 0x06000B7E RID: 2942 RVA: 0x00022DB6 File Offset: 0x00020FB6
		[DataSourceProperty]
		public bool IsMemberActionsActive
		{
			get
			{
				return this._isMemberActionsActive;
			}
			set
			{
				if (value != this._isMemberActionsActive)
				{
					this._isMemberActionsActive = value;
					base.OnPropertyChangedWithValue(value, "IsMemberActionsActive");
				}
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00022DD4 File Offset: 0x00020FD4
		// (set) Token: 0x06000B80 RID: 2944 RVA: 0x00022DDC File Offset: 0x00020FDC
		[DataSourceProperty]
		public bool IsPrivilegedMember
		{
			get
			{
				return this._isPrivilegedMember;
			}
			set
			{
				if (value != this._isPrivilegedMember)
				{
					this._isPrivilegedMember = value;
					base.OnPropertyChangedWithValue(value, "IsPrivilegedMember");
				}
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00022DFA File Offset: 0x00020FFA
		// (set) Token: 0x06000B82 RID: 2946 RVA: 0x00022E02 File Offset: 0x00021002
		[DataSourceProperty]
		public string RosterText
		{
			get
			{
				return this._rosterText;
			}
			set
			{
				if (value != this._rosterText)
				{
					this._rosterText = value;
					base.OnPropertyChangedWithValue<string>(value, "RosterText");
				}
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00022E25 File Offset: 0x00021025
		// (set) Token: 0x06000B84 RID: 2948 RVA: 0x00022E2D File Offset: 0x0002102D
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x00022E50 File Offset: 0x00021050
		// (set) Token: 0x06000B86 RID: 2950 RVA: 0x00022E58 File Offset: 0x00021058
		[DataSourceProperty]
		public string BadgeText
		{
			get
			{
				return this._badgeText;
			}
			set
			{
				if (value != this._badgeText)
				{
					this._badgeText = value;
					base.OnPropertyChangedWithValue<string>(value, "BadgeText");
				}
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x00022E7B File Offset: 0x0002107B
		// (set) Token: 0x06000B88 RID: 2952 RVA: 0x00022E83 File Offset: 0x00021083
		[DataSourceProperty]
		public string StatusText
		{
			get
			{
				return this._statusText;
			}
			set
			{
				if (value != this._statusText)
				{
					this._statusText = value;
					base.OnPropertyChangedWithValue<string>(value, "StatusText");
				}
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00022EA6 File Offset: 0x000210A6
		// (set) Token: 0x06000B8A RID: 2954 RVA: 0x00022EAE File Offset: 0x000210AE
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanMemberItemVM> MembersList
		{
			get
			{
				return this._membersList;
			}
			set
			{
				if (value != this._membersList)
				{
					this._membersList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClanMemberItemVM>>(value, "MembersList");
				}
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x00022ECC File Offset: 0x000210CC
		// (set) Token: 0x06000B8C RID: 2956 RVA: 0x00022ED4 File Offset: 0x000210D4
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> MemberActionsList
		{
			get
			{
				return this._memberActionsList;
			}
			set
			{
				if (value != this._memberActionsList)
				{
					this._memberActionsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "MemberActionsList");
				}
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x00022EF2 File Offset: 0x000210F2
		// (set) Token: 0x06000B8E RID: 2958 RVA: 0x00022EFA File Offset: 0x000210FA
		[DataSourceProperty]
		public HintViewModel PromoteToClanOfficerHint
		{
			get
			{
				return this._promoteToClanOfficerHint;
			}
			set
			{
				if (value != this._promoteToClanOfficerHint)
				{
					this._promoteToClanOfficerHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PromoteToClanOfficerHint");
				}
			}
		}

		// Token: 0x04000530 RID: 1328
		private bool _isClanLeader;

		// Token: 0x04000531 RID: 1329
		private bool _isClanOfficer;

		// Token: 0x04000532 RID: 1330
		private MPLobbyClanRosterVM.MemberComparer _memberComparer;

		// Token: 0x04000533 RID: 1331
		private bool _isSelected;

		// Token: 0x04000534 RID: 1332
		private bool _isMemberActionsActive;

		// Token: 0x04000535 RID: 1333
		private bool _isPrivilegedMember;

		// Token: 0x04000536 RID: 1334
		private string _rosterText;

		// Token: 0x04000537 RID: 1335
		private string _nameText;

		// Token: 0x04000538 RID: 1336
		private string _badgeText;

		// Token: 0x04000539 RID: 1337
		private string _statusText;

		// Token: 0x0400053A RID: 1338
		private MBBindingList<MPLobbyClanMemberItemVM> _membersList;

		// Token: 0x0400053B RID: 1339
		private MBBindingList<StringPairItemWithActionVM> _memberActionsList;

		// Token: 0x0400053C RID: 1340
		private HintViewModel _promoteToClanOfficerHint;

		// Token: 0x0200015C RID: 348
		private class MemberComparer : IComparer<MPLobbyClanMemberItemVM>
		{
			// Token: 0x0600130C RID: 4876 RVA: 0x0003D034 File Offset: 0x0003B234
			public int Compare(MPLobbyClanMemberItemVM x, MPLobbyClanMemberItemVM y)
			{
				if (y.Rank != x.Rank)
				{
					return y.Rank.CompareTo(x.Rank);
				}
				return y.IsOnline.CompareTo(x.IsOnline);
			}
		}
	}
}
