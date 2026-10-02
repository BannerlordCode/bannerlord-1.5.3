using System;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000072 RID: 114
	public class MPLobbyClanOverviewVM : ViewModel
	{
		// Token: 0x06000B1D RID: 2845 RVA: 0x00021C94 File Offset: 0x0001FE94
		public MPLobbyClanOverviewVM(Action openInviteClanMemberPopup)
		{
			this._openInviteClanMemberPopup = openInviteClanMemberPopup;
			this.AnnouncementsList = new MBBindingList<MPLobbyClanAnnouncementVM>();
			this.ChangeSigilPopup = new MPLobbyClanChangeSigilPopupVM();
			this.ChangeFactionPopup = new MPLobbyClanChangeFactionPopupVM();
			this.SendAnnouncementPopup = new MPLobbyClanSendPostPopupVM(MPLobbyClanSendPostPopupVM.PostPopupMode.Announcement);
			this.SetClanInformationPopup = new MPLobbyClanSendPostPopupVM(MPLobbyClanSendPostPopupVM.PostPopupMode.Information);
			this.AreActionButtonsEnabled = true;
			this.RefreshValues();
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00021CF4 File Offset: 0x0001FEF4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ChangeSigilText = new TextObject("{=7R0i82Nw}Change Sigil", null).ToString();
			this.ChangeFactionText = new TextObject("{=aGGq9lJT}Change Culture", null).ToString();
			this.LeaveText = new TextObject("{=3sRdGQou}Leave", null).ToString();
			this.DisbandText = new TextObject("{=xXSFaGW8}Disband", null).ToString();
			this.InformationText = new TextObject("{=SyklU5aP}Information", null).ToString();
			this.AnnouncementsText = new TextObject("{=JY2pBVHQ}Announcements", null).ToString();
			this.NoAnnouncementsText = new TextObject("{=0af2iQvw}Clan doesn't have any announcements", null).ToString();
			this.NoDescriptionText = new TextObject("{=NwiYsUwm}Clan doesn't have a description", null).ToString();
			this.TitleText = new TextObject("{=r223yChR}Overview", null).ToString();
			this.CantLeaveHint = new HintViewModel(new TextObject("{=76HlhP7r}You have to give leadership to another member to leave", null), null);
			this.InviteMembersHint = new HintViewModel(new TextObject("{=tSMckUw3}Invite Members", null), null);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00021DFC File Offset: 0x0001FFFC
		public async Task RefreshClanInformation(ClanHomeInfo info)
		{
			if (info == null || info.ClanInfo == null)
			{
				this.CloseAllPopups();
			}
			else
			{
				ClanInfo clanInfo = info.ClanInfo;
				GameTexts.SetVariable("STR", clanInfo.Tag);
				string clanTagInBrackets = new TextObject("{=uTXYEAOg}[{STR}]", null).ToString();
				string text = await PlatformServices.FilterString(clanInfo.Name, new TextObject("{=wNUcqcJP}Clan Name", null).ToString());
				GameTexts.SetVariable("STR1", text);
				GameTexts.SetVariable("STR2", clanTagInBrackets);
				this.NameText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
				GameTexts.SetVariable("LEFT", new TextObject("{=lBn2pSBL}Members", null).ToString());
				GameTexts.SetVariable("RIGHT", clanInfo.Players.Length);
				this.MembersText = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
				this.SigilImage = new BannerImageIdentifierVM(new Banner(clanInfo.Sigil), true);
				this.FactionCultureID = clanInfo.Faction;
				BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(this.FactionCultureID);
				this.CultureColor1 = Color.FromUint((@object != null) ? @object.Color : 0U);
				this.CultureColor2 = Color.FromUint((@object != null) ? @object.Color2 : 0U);
				if (NetworkMain.GameClient != null)
				{
					this.IsLeader = NetworkMain.GameClient.IsClanLeader;
					this.IsPrivilegedMember = this.IsLeader || NetworkMain.GameClient.IsClanOfficer;
				}
				else
				{
					Debug.FailedAssert("Game client is destroyed while updating clan home info", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Clan\\MPLobbyClanOverviewVM.cs", "RefreshClanInformation", 89);
					Debug.Print("Game client is destroyed while updating clan home info", 0, Debug.DebugColor.White, 17592186044416UL);
					this.IsLeader = false;
					this.IsPrivilegedMember = false;
				}
				this.FactionBanner = new BannerImageIdentifierVM(@object.Banner, true);
				this.ClanDescriptionText = clanInfo.InformationText;
				this.DoesHaveDescription = true;
				if (string.IsNullOrEmpty(clanInfo.InformationText))
				{
					this.DoesHaveDescription = false;
				}
				this.AnnouncementsList.Clear();
				ClanAnnouncement[] announcements = clanInfo.Announcements;
				foreach (ClanAnnouncement clanAnnouncement in announcements)
				{
					this.AnnouncementsList.Add(new MPLobbyClanAnnouncementVM(clanAnnouncement.AuthorId, clanAnnouncement.Announcement, clanAnnouncement.CreationTime, clanAnnouncement.Id, this.IsPrivilegedMember));
				}
				this.DoesHaveAnnouncements = true;
				if (announcements.IsEmpty<ClanAnnouncement>())
				{
					this.DoesHaveAnnouncements = false;
				}
				clanInfo = null;
				clanTagInBrackets = null;
			}
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00021E4C File Offset: 0x0002004C
		private void ExecuteDisbandClan()
		{
			string text = new TextObject("{=oFWcihyW}Disband Clan", null).ToString();
			string text2 = new TextObject("{=vW1VgmaP}Are you sure want to disband your clan?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.DisbandClan), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00021EC3 File Offset: 0x000200C3
		private void DisbandClan()
		{
			NetworkMain.GameClient.DestroyClan();
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00021ED0 File Offset: 0x000200D0
		private void ExecuteLeaveClan()
		{
			string text = new TextObject("{=4ZE6i9nW}Leave Clan", null).ToString();
			string text2 = new TextObject("{=67hsZZor}Are you sure want to leave your clan?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.LeaveClan), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00021F47 File Offset: 0x00020147
		private void LeaveClan()
		{
			NetworkMain.GameClient.KickFromClan(NetworkMain.GameClient.PlayerID);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00021F5D File Offset: 0x0002015D
		private void ExecuteOpenChangeSigilPopup()
		{
			this.ChangeSigilPopup.ExecuteOpenPopup();
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00021F6A File Offset: 0x0002016A
		private void ExecuteCloseChangeSigilPopup()
		{
			this.ChangeSigilPopup.ExecuteClosePopup();
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00021F77 File Offset: 0x00020177
		private void ExecuteOpenChangeFactionPopup()
		{
			this.ChangeFactionPopup.ExecuteOpenPopup();
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00021F84 File Offset: 0x00020184
		private void ExecuteCloseChangeFactionPopup()
		{
			this.ChangeFactionPopup.ExecuteClosePopup();
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00021F91 File Offset: 0x00020191
		private void ExecuteOpenSendAnnouncementPopup()
		{
			this.SendAnnouncementPopup.ExecuteOpenPopup();
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00021F9E File Offset: 0x0002019E
		private void ExecuteCloseSendAnnouncementPopup()
		{
			this.SendAnnouncementPopup.ExecuteClosePopup();
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00021FAB File Offset: 0x000201AB
		private void ExecuteOpenSetClanInformationPopup()
		{
			this.SetClanInformationPopup.ExecuteOpenPopup();
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00021FB8 File Offset: 0x000201B8
		private void ExecuteCloseSetClanInformationPopup()
		{
			this.SetClanInformationPopup.ExecuteClosePopup();
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00021FC5 File Offset: 0x000201C5
		private void ExecuteOpenInviteClanMemberPopup()
		{
			Action openInviteClanMemberPopup = this._openInviteClanMemberPopup;
			if (openInviteClanMemberPopup == null)
			{
				return;
			}
			openInviteClanMemberPopup();
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00021FD7 File Offset: 0x000201D7
		private void CloseAllPopups()
		{
			this.ChangeSigilPopup.ExecuteClosePopup();
			this.ChangeFactionPopup.ExecuteClosePopup();
			this.SendAnnouncementPopup.ExecuteClosePopup();
			this.SetClanInformationPopup.ExecuteClosePopup();
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x00022005 File Offset: 0x00020205
		// (set) Token: 0x06000B2F RID: 2863 RVA: 0x0002200D File Offset: 0x0002020D
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
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0002202A File Offset: 0x0002022A
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x00022032 File Offset: 0x00020232
		[DataSourceProperty]
		public bool IsLeader
		{
			get
			{
				return this._isLeader;
			}
			set
			{
				if (value != this._isLeader)
				{
					this._isLeader = value;
					base.OnPropertyChanged("IsLeader");
				}
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x0002204F File Offset: 0x0002024F
		// (set) Token: 0x06000B33 RID: 2867 RVA: 0x00022057 File Offset: 0x00020257
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
					base.OnPropertyChanged("IsPrivilegedMember");
				}
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x00022074 File Offset: 0x00020274
		// (set) Token: 0x06000B35 RID: 2869 RVA: 0x0002207C File Offset: 0x0002027C
		[DataSourceProperty]
		public bool AreActionButtonsEnabled
		{
			get
			{
				return this._areActionButtonsEnabled;
			}
			set
			{
				if (value != this._areActionButtonsEnabled)
				{
					this._areActionButtonsEnabled = value;
					base.OnPropertyChanged("AreActionButtonsEnabled");
				}
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00022099 File Offset: 0x00020299
		// (set) Token: 0x06000B37 RID: 2871 RVA: 0x000220A1 File Offset: 0x000202A1
		[DataSourceProperty]
		public bool DoesHaveDescription
		{
			get
			{
				return this._doesHaveDescription;
			}
			set
			{
				if (value != this._doesHaveDescription)
				{
					this._doesHaveDescription = value;
					base.OnPropertyChanged("DoesHaveDescription");
				}
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x000220BE File Offset: 0x000202BE
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x000220C6 File Offset: 0x000202C6
		[DataSourceProperty]
		public bool DoesHaveAnnouncements
		{
			get
			{
				return this._doesHaveAnnouncements;
			}
			set
			{
				if (value != this._doesHaveAnnouncements)
				{
					this._doesHaveAnnouncements = value;
					base.OnPropertyChanged("DoesHaveAnnouncements");
				}
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x000220E3 File Offset: 0x000202E3
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x000220EB File Offset: 0x000202EB
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
					base.OnPropertyChanged("NameText");
				}
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0002210D File Offset: 0x0002030D
		// (set) Token: 0x06000B3D RID: 2877 RVA: 0x00022115 File Offset: 0x00020315
		[DataSourceProperty]
		public string MembersText
		{
			get
			{
				return this._membersText;
			}
			set
			{
				if (value != this._membersText)
				{
					this._membersText = value;
					base.OnPropertyChanged("MembersText");
				}
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00022137 File Offset: 0x00020337
		// (set) Token: 0x06000B3F RID: 2879 RVA: 0x0002213F File Offset: 0x0002033F
		[DataSourceProperty]
		public string ChangeSigilText
		{
			get
			{
				return this._changeSigilText;
			}
			set
			{
				if (value != this._changeSigilText)
				{
					this._changeSigilText = value;
					base.OnPropertyChanged("ChangeSigilText");
				}
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00022161 File Offset: 0x00020361
		// (set) Token: 0x06000B41 RID: 2881 RVA: 0x00022169 File Offset: 0x00020369
		[DataSourceProperty]
		public string ChangeFactionText
		{
			get
			{
				return this._changeFactionText;
			}
			set
			{
				if (value != this._changeFactionText)
				{
					this._changeFactionText = value;
					base.OnPropertyChanged("ChangeFactionText");
				}
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0002218B File Offset: 0x0002038B
		// (set) Token: 0x06000B43 RID: 2883 RVA: 0x00022193 File Offset: 0x00020393
		[DataSourceProperty]
		public string LeaveText
		{
			get
			{
				return this._leaveText;
			}
			set
			{
				if (value != this._leaveText)
				{
					this._leaveText = value;
					base.OnPropertyChanged("LeaveText");
				}
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x000221B5 File Offset: 0x000203B5
		// (set) Token: 0x06000B45 RID: 2885 RVA: 0x000221BD File Offset: 0x000203BD
		[DataSourceProperty]
		public string DisbandText
		{
			get
			{
				return this._disbandText;
			}
			set
			{
				if (value != this._disbandText)
				{
					this._disbandText = value;
					base.OnPropertyChanged("DisbandText");
				}
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000B46 RID: 2886 RVA: 0x000221DF File Offset: 0x000203DF
		// (set) Token: 0x06000B47 RID: 2887 RVA: 0x000221E7 File Offset: 0x000203E7
		[DataSourceProperty]
		public string FactionCultureID
		{
			get
			{
				return this._factionCultureID;
			}
			set
			{
				if (value != this._factionCultureID)
				{
					this._factionCultureID = value;
					base.OnPropertyChanged("FactionCultureID");
				}
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x00022209 File Offset: 0x00020409
		// (set) Token: 0x06000B49 RID: 2889 RVA: 0x00022211 File Offset: 0x00020411
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

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00022234 File Offset: 0x00020434
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x0002223C File Offset: 0x0002043C
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

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0002225F File Offset: 0x0002045F
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x00022267 File Offset: 0x00020467
		[DataSourceProperty]
		public string InformationText
		{
			get
			{
				return this._informationText;
			}
			set
			{
				if (value != this._informationText)
				{
					this._informationText = value;
					base.OnPropertyChanged("InformationText");
				}
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00022289 File Offset: 0x00020489
		// (set) Token: 0x06000B4F RID: 2895 RVA: 0x00022291 File Offset: 0x00020491
		[DataSourceProperty]
		public string AnnouncementsText
		{
			get
			{
				return this._announcementsText;
			}
			set
			{
				if (value != this._announcementsText)
				{
					this._announcementsText = value;
					base.OnPropertyChanged("AnnouncementsText");
				}
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x000222B3 File Offset: 0x000204B3
		// (set) Token: 0x06000B51 RID: 2897 RVA: 0x000222BB File Offset: 0x000204BB
		[DataSourceProperty]
		public string ClanDescriptionText
		{
			get
			{
				return this._clanDescriptionText;
			}
			set
			{
				if (value != this._clanDescriptionText)
				{
					this._clanDescriptionText = value;
					base.OnPropertyChanged("ClanDescriptionText");
				}
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x000222DD File Offset: 0x000204DD
		// (set) Token: 0x06000B53 RID: 2899 RVA: 0x000222E5 File Offset: 0x000204E5
		[DataSourceProperty]
		public string NoDescriptionText
		{
			get
			{
				return this._noDescriptionText;
			}
			set
			{
				if (value != this._noDescriptionText)
				{
					this._noDescriptionText = value;
					base.OnPropertyChanged("NoDescriptionText");
				}
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00022307 File Offset: 0x00020507
		// (set) Token: 0x06000B55 RID: 2901 RVA: 0x0002230F File Offset: 0x0002050F
		[DataSourceProperty]
		public string NoAnnouncementsText
		{
			get
			{
				return this._noAnnouncementsText;
			}
			set
			{
				if (value != this._noAnnouncementsText)
				{
					this._noAnnouncementsText = value;
					base.OnPropertyChanged("NoAnnouncementsText");
				}
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x00022331 File Offset: 0x00020531
		// (set) Token: 0x06000B57 RID: 2903 RVA: 0x00022339 File Offset: 0x00020539
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

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0002235B File Offset: 0x0002055B
		// (set) Token: 0x06000B59 RID: 2905 RVA: 0x00022363 File Offset: 0x00020563
		[DataSourceProperty]
		public BannerImageIdentifierVM SigilImage
		{
			get
			{
				return this._sigilImage;
			}
			set
			{
				if (value != this._sigilImage)
				{
					this._sigilImage = value;
					base.OnPropertyChanged("SigilImage");
				}
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x00022380 File Offset: 0x00020580
		// (set) Token: 0x06000B5B RID: 2907 RVA: 0x00022388 File Offset: 0x00020588
		[DataSourceProperty]
		public BannerImageIdentifierVM FactionBanner
		{
			get
			{
				return this._factionBanner;
			}
			set
			{
				if (value != this._factionBanner)
				{
					this._factionBanner = value;
					base.OnPropertyChanged("FactionBanner");
				}
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x000223A5 File Offset: 0x000205A5
		// (set) Token: 0x06000B5D RID: 2909 RVA: 0x000223AD File Offset: 0x000205AD
		[DataSourceProperty]
		public MPLobbyClanChangeSigilPopupVM ChangeSigilPopup
		{
			get
			{
				return this._changeSigilPopup;
			}
			set
			{
				if (value != this._changeSigilPopup)
				{
					this._changeSigilPopup = value;
					base.OnPropertyChanged("ChangeSigilPopup");
				}
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x000223CA File Offset: 0x000205CA
		// (set) Token: 0x06000B5F RID: 2911 RVA: 0x000223D2 File Offset: 0x000205D2
		[DataSourceProperty]
		public MPLobbyClanChangeFactionPopupVM ChangeFactionPopup
		{
			get
			{
				return this._changeFactionPopup;
			}
			set
			{
				if (value != this._changeFactionPopup)
				{
					this._changeFactionPopup = value;
					base.OnPropertyChanged("ChangeFactionPopup");
				}
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x000223EF File Offset: 0x000205EF
		// (set) Token: 0x06000B61 RID: 2913 RVA: 0x000223F7 File Offset: 0x000205F7
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanAnnouncementVM> AnnouncementsList
		{
			get
			{
				return this._announcementsList;
			}
			set
			{
				if (value != this._announcementsList)
				{
					this._announcementsList = value;
					base.OnPropertyChanged("AnnouncementsList");
				}
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x00022414 File Offset: 0x00020614
		// (set) Token: 0x06000B63 RID: 2915 RVA: 0x0002241C File Offset: 0x0002061C
		[DataSourceProperty]
		public MPLobbyClanSendPostPopupVM SendAnnouncementPopup
		{
			get
			{
				return this._sendAnnouncementPopup;
			}
			set
			{
				if (value != this._sendAnnouncementPopup)
				{
					this._sendAnnouncementPopup = value;
					base.OnPropertyChanged("SendAnnouncementPopup");
				}
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00022439 File Offset: 0x00020639
		// (set) Token: 0x06000B65 RID: 2917 RVA: 0x00022441 File Offset: 0x00020641
		[DataSourceProperty]
		public MPLobbyClanSendPostPopupVM SetClanInformationPopup
		{
			get
			{
				return this._setClanInformationPopup;
			}
			set
			{
				if (value != this._setClanInformationPopup)
				{
					this._setClanInformationPopup = value;
					base.OnPropertyChanged("SetClanInformationPopup");
				}
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0002245E File Offset: 0x0002065E
		// (set) Token: 0x06000B67 RID: 2919 RVA: 0x00022466 File Offset: 0x00020666
		[DataSourceProperty]
		public HintViewModel CantLeaveHint
		{
			get
			{
				return this._cantLeaveHint;
			}
			set
			{
				if (value != this._cantLeaveHint)
				{
					this._cantLeaveHint = value;
					base.OnPropertyChanged("CantLeaveHint");
				}
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x00022483 File Offset: 0x00020683
		// (set) Token: 0x06000B69 RID: 2921 RVA: 0x0002248B File Offset: 0x0002068B
		[DataSourceProperty]
		public HintViewModel InviteMembersHint
		{
			get
			{
				return this._inviteMembersHint;
			}
			set
			{
				if (value != this._inviteMembersHint)
				{
					this._inviteMembersHint = value;
					base.OnPropertyChanged("InviteMembersHint");
				}
			}
		}

		// Token: 0x04000511 RID: 1297
		private readonly Action _openInviteClanMemberPopup;

		// Token: 0x04000512 RID: 1298
		private bool _isSelected;

		// Token: 0x04000513 RID: 1299
		private bool _isLeader;

		// Token: 0x04000514 RID: 1300
		private bool _isPrivilegedMember;

		// Token: 0x04000515 RID: 1301
		private bool _areActionButtonsEnabled;

		// Token: 0x04000516 RID: 1302
		private bool _doesHaveDescription;

		// Token: 0x04000517 RID: 1303
		private bool _doesHaveAnnouncements;

		// Token: 0x04000518 RID: 1304
		private string _nameText;

		// Token: 0x04000519 RID: 1305
		private string _membersText;

		// Token: 0x0400051A RID: 1306
		private string _changeSigilText;

		// Token: 0x0400051B RID: 1307
		private string _changeFactionText;

		// Token: 0x0400051C RID: 1308
		private string _leaveText;

		// Token: 0x0400051D RID: 1309
		private string _disbandText;

		// Token: 0x0400051E RID: 1310
		private string _factionCultureID;

		// Token: 0x0400051F RID: 1311
		private string _informationText;

		// Token: 0x04000520 RID: 1312
		private string _announcementsText;

		// Token: 0x04000521 RID: 1313
		private string _clanDescriptionText;

		// Token: 0x04000522 RID: 1314
		private string _noDescriptionText;

		// Token: 0x04000523 RID: 1315
		private string _noAnnouncementsText;

		// Token: 0x04000524 RID: 1316
		private string _titleText;

		// Token: 0x04000525 RID: 1317
		private Color _cultureColor1;

		// Token: 0x04000526 RID: 1318
		private Color _cultureColor2;

		// Token: 0x04000527 RID: 1319
		private BannerImageIdentifierVM _sigilImage;

		// Token: 0x04000528 RID: 1320
		private BannerImageIdentifierVM _factionBanner;

		// Token: 0x04000529 RID: 1321
		private MPLobbyClanChangeSigilPopupVM _changeSigilPopup;

		// Token: 0x0400052A RID: 1322
		private MPLobbyClanChangeFactionPopupVM _changeFactionPopup;

		// Token: 0x0400052B RID: 1323
		private MBBindingList<MPLobbyClanAnnouncementVM> _announcementsList;

		// Token: 0x0400052C RID: 1324
		private MPLobbyClanSendPostPopupVM _sendAnnouncementPopup;

		// Token: 0x0400052D RID: 1325
		private MPLobbyClanSendPostPopupVM _setClanInformationPopup;

		// Token: 0x0400052E RID: 1326
		private HintViewModel _cantLeaveHint;

		// Token: 0x0400052F RID: 1327
		private HintViewModel _inviteMembersHint;
	}
}
