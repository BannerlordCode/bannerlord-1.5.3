using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x0200004E RID: 78
	public class MPAnnouncementsVM : ViewModel
	{
		// Token: 0x060006C2 RID: 1730 RVA: 0x00015D22 File Offset: 0x00013F22
		public MPAnnouncementsVM(float? announcementUpdateIntervalInSeconds)
		{
			this._updateTimer = null;
			this._announcementUpdateIntervalInSeconds = announcementUpdateIntervalInSeconds;
			this.AnnouncementList = new MBBindingList<MPAnnouncementItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00015D4E File Offset: 0x00013F4E
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=lQ0T2pbY}Events & Announcements", null).ToString();
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00015D6C File Offset: 0x00013F6C
		public void OnTick(float dt)
		{
			if (!NetworkMain.GameClient.AtLobby)
			{
				this._updateTimer = null;
				return;
			}
			if (this._announcementUpdateIntervalInSeconds != null)
			{
				float? num = this._announcementUpdateIntervalInSeconds;
				float num2 = 0f;
				if (((num.GetValueOrDefault() > num2) & (num != null)) && !this._isRefreshingAnnouncements)
				{
					if (this._updateTimer != null)
					{
						num = this._updateTimer;
						float? announcementUpdateIntervalInSeconds = this._announcementUpdateIntervalInSeconds;
						if (!((num.GetValueOrDefault() > announcementUpdateIntervalInSeconds.GetValueOrDefault()) & ((num != null) & (announcementUpdateIntervalInSeconds != null))))
						{
							this._updateTimer += dt;
							return;
						}
					}
					this.RefreshAnnouncements();
					this._updateTimer = new float?(0f);
					return;
				}
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00015E56 File Offset: 0x00014056
		private void RefreshAnnouncements()
		{
			this._isRefreshingAnnouncements = true;
			this.UpdateAnnouncements();
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00015E68 File Offset: 0x00014068
		public async void UpdateAnnouncements()
		{
			PublishedLobbyNewsArticle[] array = await NetworkMain.GameClient.GetLobbyNews();
			this.AnnouncementList.Clear();
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					MPAnnouncementItemVM mpannouncementItemVM = new MPAnnouncementItemVM(array[i]);
					this.AnnouncementList.Add(mpannouncementItemVM);
				}
			}
			this.HasValidAnnouncements = this.AnnouncementList.Count > 0 && ApplicationPlatform.IsPlatformWindows() && ApplicationPlatform.CurrentPlatform != Platform.GDKDesktop;
			this._isRefreshingAnnouncements = false;
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00015EA1 File Offset: 0x000140A1
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x00015EA9 File Offset: 0x000140A9
		[DataSourceProperty]
		public bool HasValidAnnouncements
		{
			get
			{
				return this._hasValidAnnouncements;
			}
			set
			{
				if (value != this._hasValidAnnouncements)
				{
					this._hasValidAnnouncements = value;
					base.OnPropertyChangedWithValue(value, "HasValidAnnouncements");
				}
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00015EC7 File Offset: 0x000140C7
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x00015ECF File Offset: 0x000140CF
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
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00015EF2 File Offset: 0x000140F2
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x00015EFA File Offset: 0x000140FA
		[DataSourceProperty]
		public MBBindingList<MPAnnouncementItemVM> AnnouncementList
		{
			get
			{
				return this._announcementList;
			}
			set
			{
				if (value != this._announcementList)
				{
					this._announcementList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPAnnouncementItemVM>>(value, "AnnouncementList");
				}
			}
		}

		// Token: 0x0400032D RID: 813
		private readonly float? _announcementUpdateIntervalInSeconds;

		// Token: 0x0400032E RID: 814
		private float? _updateTimer;

		// Token: 0x0400032F RID: 815
		private bool _isRefreshingAnnouncements;

		// Token: 0x04000330 RID: 816
		private bool _hasValidAnnouncements;

		// Token: 0x04000331 RID: 817
		private string _titleText;

		// Token: 0x04000332 RID: 818
		private MBBindingList<MPAnnouncementItemVM> _announcementList;
	}
}
