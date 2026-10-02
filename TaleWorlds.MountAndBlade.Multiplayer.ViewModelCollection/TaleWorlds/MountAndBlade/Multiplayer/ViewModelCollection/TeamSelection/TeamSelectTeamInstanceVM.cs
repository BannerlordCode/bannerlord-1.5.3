using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.TeamSelection
{
	// Token: 0x0200001B RID: 27
	public class TeamSelectTeamInstanceVM : ViewModel
	{
		// Token: 0x06000188 RID: 392 RVA: 0x00006CA4 File Offset: 0x00004EA4
		public TeamSelectTeamInstanceVM(MissionScoreboardComponent missionScoreboardComponent, Team team, BasicCultureObject culture, Banner banner, Action<Team> onSelect, MultiplayerBattleColors.MultiplayerCultureColorInfo cultureColors)
		{
			this.Team = team;
			this._onSelect = onSelect;
			this._culture = culture;
			Mission mission = Mission.Current;
			this.IsSiege = mission != null && mission.HasMissionBehavior<MissionMultiplayerSiegeClient>();
			if (this.Team != null && this.Team.Side != BattleSideEnum.None)
			{
				this._missionScoreboardComponent = missionScoreboardComponent;
				this._missionScoreboardComponent.OnRoundPropertiesChanged += this.UpdateTeamScores;
				this._missionScoreboardSide = this._missionScoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == this.Team.Side);
				this.IsAttacker = this.Team.Side == BattleSideEnum.Attacker;
				this.UpdateTeamScores();
			}
			this.CultureId = ((culture == null) ? "" : culture.StringId);
			if (team == null)
			{
				this.IsDisabled = true;
			}
			this.Banner = new BannerImageIdentifierVM(banner, true);
			this.CultureColor1 = cultureColors.Color1;
			this.CultureColor2 = cultureColors.Color2;
			this._friends = new List<MPPlayerVM>();
			this.FriendAvatars = new MBBindingList<MPPlayerVM>();
			this.RefreshValues();
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00006DB8 File Offset: 0x00004FB8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DisplayedPrimary = ((this._culture == null) ? new TextObject("{=pSheKLB4}Spectator", null).ToString() : this._culture.Name.ToString());
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006DF0 File Offset: 0x00004FF0
		public override void OnFinalize()
		{
			if (this._missionScoreboardComponent != null)
			{
				this._missionScoreboardComponent.OnRoundPropertiesChanged -= this.UpdateTeamScores;
			}
			this._missionScoreboardComponent = null;
			this._missionScoreboardSide = null;
			base.OnFinalize();
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006E25 File Offset: 0x00005025
		private void UpdateTeamScores()
		{
			if (this._missionScoreboardSide != null)
			{
				this.Score = this._missionScoreboardSide.SideScore;
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006E40 File Offset: 0x00005040
		public void RefreshFriends(IEnumerable<MissionPeer> friends)
		{
			List<MissionPeer> list = friends.ToList<MissionPeer>();
			List<MPPlayerVM> list2 = new List<MPPlayerVM>();
			foreach (MPPlayerVM mpplayerVM in this._friends)
			{
				if (!list.Contains(mpplayerVM.Peer))
				{
					list2.Add(mpplayerVM);
				}
			}
			foreach (MPPlayerVM mpplayerVM2 in list2)
			{
				this._friends.Remove(mpplayerVM2);
			}
			List<MissionPeer> list3 = this._friends.Select<MPPlayerVM, MissionPeer>((MPPlayerVM x) => x.Peer).ToList<MissionPeer>();
			foreach (MissionPeer missionPeer in list)
			{
				if (!list3.Contains(missionPeer))
				{
					this._friends.Add(new MPPlayerVM(missionPeer));
				}
			}
			this.FriendAvatars.Clear();
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "RefreshFriends");
			for (int i = 0; i < this._friends.Count; i++)
			{
				if (i < 6)
				{
					this.FriendAvatars.Add(this._friends[i]);
				}
				else
				{
					mbstringBuilder.AppendLine<string>(this._friends[i].Peer.DisplayedName);
				}
			}
			int num = this._friends.Count - 6;
			if (num > 0)
			{
				this.HasExtraFriends = true;
				TextObject textObject = new TextObject("{=hbwp3g3k}+{FRIEND_COUNT} {newline} {?PLURAL}friends{?}friend{\\?}", null);
				textObject.SetTextVariable("FRIEND_COUNT", num);
				textObject.SetTextVariable("PLURAL", (num == 1) ? 0 : 1);
				this.FriendsExtraText = textObject.ToString();
				this.FriendsExtraHint = new HintViewModel(textObject, null);
				return;
			}
			mbstringBuilder.Release();
			this.HasExtraFriends = false;
			this.FriendsExtraText = "";
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007078 File Offset: 0x00005278
		public void SetIsDisabled(bool isCurrentTeam, bool disabledForBalance)
		{
			this.IsDisabled = isCurrentTeam || disabledForBalance;
			if (isCurrentTeam)
			{
				this.LockText = new TextObject("{=SoQcsslF}CURRENT TEAM", null).ToString();
				return;
			}
			if (disabledForBalance)
			{
				this.LockText = new TextObject("{=qe46yXVJ}LOCKED FOR BALANCE", null).ToString();
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000070B6 File Offset: 0x000052B6
		public void ExecuteSelectTeam()
		{
			if (this._onSelect != null)
			{
				this._onSelect(this.Team);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000070D1 File Offset: 0x000052D1
		// (set) Token: 0x06000190 RID: 400 RVA: 0x000070D9 File Offset: 0x000052D9
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (this._cultureId != value)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000070FC File Offset: 0x000052FC
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00007104 File Offset: 0x00005304
		[DataSourceProperty]
		public int Score
		{
			get
			{
				return this._score;
			}
			set
			{
				if (value != this._score)
				{
					this._score = value;
					base.OnPropertyChangedWithValue(value, "Score");
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00007122 File Offset: 0x00005322
		// (set) Token: 0x06000194 RID: 404 RVA: 0x0000712A File Offset: 0x0000532A
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (this._isDisabled != value)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00007148 File Offset: 0x00005348
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00007150 File Offset: 0x00005350
		[DataSourceProperty]
		public bool IsAttacker
		{
			get
			{
				return this._isAttacker;
			}
			set
			{
				if (this._isAttacker != value)
				{
					this._isAttacker = value;
					base.OnPropertyChangedWithValue(value, "IsAttacker");
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000716E File Offset: 0x0000536E
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00007176 File Offset: 0x00005376
		[DataSourceProperty]
		public bool IsSiege
		{
			get
			{
				return this._isSiege;
			}
			set
			{
				if (this._isSiege != value)
				{
					this._isSiege = value;
					base.OnPropertyChangedWithValue(value, "IsSiege");
				}
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00007194 File Offset: 0x00005394
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000719C File Offset: 0x0000539C
		[DataSourceProperty]
		public string DisplayedPrimary
		{
			get
			{
				return this._displayedPrimary;
			}
			set
			{
				this._displayedPrimary = value;
				base.OnPropertyChangedWithValue<string>(value, "DisplayedPrimary");
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600019B RID: 411 RVA: 0x000071B1 File Offset: 0x000053B1
		// (set) Token: 0x0600019C RID: 412 RVA: 0x000071B9 File Offset: 0x000053B9
		[DataSourceProperty]
		public string DisplayedSecondary
		{
			get
			{
				return this._displayedSecondary;
			}
			set
			{
				this._displayedSecondary = value;
				base.OnPropertyChangedWithValue<string>(value, "DisplayedSecondary");
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000071CE File Offset: 0x000053CE
		// (set) Token: 0x0600019E RID: 414 RVA: 0x000071D6 File Offset: 0x000053D6
		[DataSourceProperty]
		public string DisplayedSecondarySub
		{
			get
			{
				return this._displayedSecondarySub;
			}
			set
			{
				this._displayedSecondarySub = value;
				base.OnPropertyChangedWithValue<string>(value, "DisplayedSecondarySub");
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000071EB File Offset: 0x000053EB
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x000071F3 File Offset: 0x000053F3
		[DataSourceProperty]
		public string LockText
		{
			get
			{
				return this._lockText;
			}
			set
			{
				this._lockText = value;
				base.OnPropertyChangedWithValue<string>(value, "LockText");
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00007208 File Offset: 0x00005408
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00007210 File Offset: 0x00005410
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner && (value == null || this._banner == null || this._banner.Id != value.Id))
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000725C File Offset: 0x0000545C
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00007264 File Offset: 0x00005464
		[DataSourceProperty]
		public MBBindingList<MPPlayerVM> FriendAvatars
		{
			get
			{
				return this._friendAvatars;
			}
			set
			{
				if (this._friendAvatars != value)
				{
					this._friendAvatars = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPlayerVM>>(value, "FriendAvatars");
				}
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00007282 File Offset: 0x00005482
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x0000728A File Offset: 0x0000548A
		[DataSourceProperty]
		public bool HasExtraFriends
		{
			get
			{
				return this._hasExtraFriends;
			}
			set
			{
				if (this._hasExtraFriends != value)
				{
					this._hasExtraFriends = value;
					base.OnPropertyChangedWithValue(value, "HasExtraFriends");
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000072A8 File Offset: 0x000054A8
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x000072B0 File Offset: 0x000054B0
		[DataSourceProperty]
		public string FriendsExtraText
		{
			get
			{
				return this._friendsExtraText;
			}
			set
			{
				if (this._friendsExtraText != value)
				{
					this._friendsExtraText = value;
					base.OnPropertyChangedWithValue<string>(value, "FriendsExtraText");
				}
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000072D3 File Offset: 0x000054D3
		// (set) Token: 0x060001AA RID: 426 RVA: 0x000072DB File Offset: 0x000054DB
		[DataSourceProperty]
		public HintViewModel FriendsExtraHint
		{
			get
			{
				return this._friendsExtraHint;
			}
			set
			{
				if (this._friendsExtraHint != value)
				{
					this._friendsExtraHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FriendsExtraHint");
				}
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000072F9 File Offset: 0x000054F9
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00007301 File Offset: 0x00005501
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00007324 File Offset: 0x00005524
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000732C File Offset: 0x0000552C
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor2");
				}
			}
		}

		// Token: 0x040000CF RID: 207
		private const int MaxFriendAvatarCount = 6;

		// Token: 0x040000D0 RID: 208
		public readonly Team Team;

		// Token: 0x040000D1 RID: 209
		public readonly Action<Team> _onSelect;

		// Token: 0x040000D2 RID: 210
		private readonly List<MPPlayerVM> _friends;

		// Token: 0x040000D3 RID: 211
		private MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x040000D4 RID: 212
		private MissionScoreboardComponent.MissionScoreboardSide _missionScoreboardSide;

		// Token: 0x040000D5 RID: 213
		private readonly BasicCultureObject _culture;

		// Token: 0x040000D6 RID: 214
		private bool _isDisabled;

		// Token: 0x040000D7 RID: 215
		private string _displayedPrimary;

		// Token: 0x040000D8 RID: 216
		private string _displayedSecondary;

		// Token: 0x040000D9 RID: 217
		private string _displayedSecondarySub;

		// Token: 0x040000DA RID: 218
		private string _lockText;

		// Token: 0x040000DB RID: 219
		private string _cultureId;

		// Token: 0x040000DC RID: 220
		private int _score;

		// Token: 0x040000DD RID: 221
		private BannerImageIdentifierVM _banner;

		// Token: 0x040000DE RID: 222
		private MBBindingList<MPPlayerVM> _friendAvatars;

		// Token: 0x040000DF RID: 223
		private bool _hasExtraFriends;

		// Token: 0x040000E0 RID: 224
		private bool _isAttacker;

		// Token: 0x040000E1 RID: 225
		private bool _isSiege;

		// Token: 0x040000E2 RID: 226
		private string _friendsExtraText;

		// Token: 0x040000E3 RID: 227
		private HintViewModel _friendsExtraHint;

		// Token: 0x040000E4 RID: 228
		private Color _cultureColor1;

		// Token: 0x040000E5 RID: 229
		private Color _cultureColor2;
	}
}
