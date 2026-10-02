using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000071 RID: 113
	public class MPLobbyClanMemberItemVM : MPLobbyPlayerBaseVM
	{
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00021978 File Offset: 0x0001FB78
		// (set) Token: 0x06000B09 RID: 2825 RVA: 0x00021980 File Offset: 0x0001FB80
		public PlayerId Id { get; private set; }

		// Token: 0x06000B0A RID: 2826 RVA: 0x00021989 File Offset: 0x0001FB89
		public MPLobbyClanMemberItemVM(PlayerId playerId)
			: base(playerId, "", null, null)
		{
			this.Id = playerId;
			this.RefreshValues();
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x000219A8 File Offset: 0x0001FBA8
		public MPLobbyClanMemberItemVM(ClanPlayer member, bool isOnline, string selectedBadgeID, AnotherPlayerState state, Action<MPLobbyClanMemberItemVM> executeActivate = null)
			: base(member.PlayerId, "", null, null)
		{
			this._member = member;
			this.Id = this._member.PlayerId;
			this.IsOnline = isOnline;
			this._executeActivate = executeActivate;
			base.SelectedBadgeID = selectedBadgeID;
			if (isOnline)
			{
				base.StateText = GameTexts.FindText("str_multiplayer_lobby_state", state.ToString()).ToString();
			}
			this.IsClanLeader = this._member.Role == ClanPlayerRole.Leader;
			this.Rank = (int)this._member.Role;
			this.RankHint = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00021A54 File Offset: 0x0001FC54
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NotEligibleInfo = "";
			ClanPlayer member = this._member;
			if (member != null && member.Role == ClanPlayerRole.Leader)
			{
				this.RankHint.HintText = new TextObject("{=SrfYbg3x}Leader", null);
				return;
			}
			ClanPlayer member2 = this._member;
			if (member2 != null && member2.Role == ClanPlayerRole.Officer)
			{
				this.RankHint.HintText = new TextObject("{=ZYF2t1VI}Officer", null);
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00021AD0 File Offset: 0x0001FCD0
		public void SetNotEligibleInfo(PlayerNotEligibleError notEligibleError)
		{
			string text = "";
			if (notEligibleError == PlayerNotEligibleError.AlreadyInClan)
			{
				text = new TextObject("{=zEMWM4h3}Already In a Clan", null).ToString();
			}
			else if (notEligibleError == PlayerNotEligibleError.NotAtLobby)
			{
				text = new TextObject("{=hPbppi6E}Not At The Lobby", null).ToString();
			}
			else if (notEligibleError == PlayerNotEligibleError.DoesNotSupportFeature)
			{
				text = new TextObject("{=MsokbMx2}Does not support Clan feature", null).ToString();
			}
			if (string.IsNullOrEmpty(this.NotEligibleInfo))
			{
				this.NotEligibleInfo = text;
				return;
			}
			GameTexts.SetVariable("LEFT", this.NotEligibleInfo);
			GameTexts.SetVariable("RIGHT", text);
			this.NotEligibleInfo = GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString();
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00021B6B File Offset: 0x0001FD6B
		private void ExecuteSelection()
		{
			Action<MPLobbyClanMemberItemVM> executeActivate = this._executeActivate;
			if (executeActivate == null)
			{
				return;
			}
			executeActivate(this);
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x00021B7E File Offset: 0x0001FD7E
		// (set) Token: 0x06000B10 RID: 2832 RVA: 0x00021B86 File Offset: 0x0001FD86
		[DataSourceProperty]
		public bool IsOnline
		{
			get
			{
				return this._isOnline;
			}
			set
			{
				if (value != this._isOnline)
				{
					this._isOnline = value;
					base.OnPropertyChangedWithValue(value, "IsOnline");
				}
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x00021BA4 File Offset: 0x0001FDA4
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x00021BAC File Offset: 0x0001FDAC
		[DataSourceProperty]
		public bool IsClanLeader
		{
			get
			{
				return this._isClanLeader;
			}
			set
			{
				if (value != this._isClanLeader)
				{
					this._isClanLeader = value;
					base.OnPropertyChangedWithValue(value, "IsClanLeader");
				}
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00021BCA File Offset: 0x0001FDCA
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x00021BD2 File Offset: 0x0001FDD2
		[DataSourceProperty]
		public string NotEligibleInfo
		{
			get
			{
				return this._notEligibleInfo;
			}
			set
			{
				if (value != this._notEligibleInfo)
				{
					this._notEligibleInfo = value;
					base.OnPropertyChangedWithValue<string>(value, "NotEligibleInfo");
				}
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00021BF5 File Offset: 0x0001FDF5
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x00021BFD File Offset: 0x0001FDFD
		[DataSourceProperty]
		public string InviteAcceptInfo
		{
			get
			{
				return this._inviteAcceptInfo;
			}
			set
			{
				if (value != this._inviteAcceptInfo)
				{
					this._inviteAcceptInfo = value;
					base.OnPropertyChangedWithValue<string>(value, "InviteAcceptInfo");
				}
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00021C20 File Offset: 0x0001FE20
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x00021C28 File Offset: 0x0001FE28
		[DataSourceProperty]
		public int Rank
		{
			get
			{
				return this._rank;
			}
			set
			{
				if (value != this._rank)
				{
					this._rank = value;
					base.OnPropertyChangedWithValue(value, "Rank");
				}
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00021C46 File Offset: 0x0001FE46
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x00021C4E File Offset: 0x0001FE4E
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> UserActionsList
		{
			get
			{
				return this._userActionsList;
			}
			set
			{
				if (value != this._userActionsList)
				{
					this._userActionsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "UserActionsList");
				}
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00021C6C File Offset: 0x0001FE6C
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x00021C74 File Offset: 0x0001FE74
		[DataSourceProperty]
		public HintViewModel RankHint
		{
			get
			{
				return this._rankHint;
			}
			set
			{
				if (value != this._rankHint)
				{
					this._rankHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RankHint");
				}
			}
		}

		// Token: 0x04000507 RID: 1287
		private ClanPlayer _member;

		// Token: 0x04000509 RID: 1289
		private Action<MPLobbyClanMemberItemVM> _executeActivate;

		// Token: 0x0400050A RID: 1290
		private bool _isOnline;

		// Token: 0x0400050B RID: 1291
		private bool _isClanLeader;

		// Token: 0x0400050C RID: 1292
		private string _notEligibleInfo;

		// Token: 0x0400050D RID: 1293
		private string _inviteAcceptInfo;

		// Token: 0x0400050E RID: 1294
		private int _rank;

		// Token: 0x0400050F RID: 1295
		private MBBindingList<StringPairItemWithActionVM> _userActionsList;

		// Token: 0x04000510 RID: 1296
		private HintViewModel _rankHint;
	}
}
