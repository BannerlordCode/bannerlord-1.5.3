using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x0200004D RID: 77
	public class MPAnnouncementItemVM : ViewModel
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060006AD RID: 1709 RVA: 0x00015A10 File Offset: 0x00013C10
		// (remove) Token: 0x060006AE RID: 1710 RVA: 0x00015A44 File Offset: 0x00013C44
		public static event Action<MPAnnouncementItemVM> OnInspect;

		// Token: 0x060006AF RID: 1711 RVA: 0x00015A78 File Offset: 0x00013C78
		public MPAnnouncementItemVM(PublishedLobbyNewsArticle announcement)
		{
			this._announcement = announcement;
			this.IsPinned = announcement.Pinned;
			this.Type = announcement.Type;
			this.TypeName = this.GetAnnouncementTypeName(this.Type);
			this.Title = this.ParseMarkup(announcement.Title);
			this.Description = this.ParseMarkup(announcement.Description);
			this.UpdateDateText();
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00015AE6 File Offset: 0x00013CE6
		public void ExecuteInspect()
		{
			Action<MPAnnouncementItemVM> onInspect = MPAnnouncementItemVM.OnInspect;
			if (onInspect == null)
			{
				return;
			}
			onInspect(this);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00015AF8 File Offset: 0x00013CF8
		private string ParseMarkup(string markupText)
		{
			return markupText;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00015AFC File Offset: 0x00013CFC
		private void UpdateDateText()
		{
			this.IsSingleDate = string.IsNullOrEmpty(this._announcement.DateStart) || string.IsNullOrEmpty(this._announcement.DateEnd) || this._announcement.DateStart == this._announcement.DateEnd;
			string text = string.Empty;
			if (this.IsSingleDate)
			{
				string text2 = this._announcement.DateStart;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = this._announcement.DateEnd;
				}
				text = text2;
			}
			else
			{
				TextObject textObject = GameTexts.FindText("str_LEFT_dash_RIGHT", null);
				textObject.SetTextVariable("LEFT", this._announcement.DateStart);
				textObject.SetTextVariable("RIGHT", this._announcement.DateEnd);
				text = textObject.ToString();
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Replace('\\', '.');
				text = text.Replace('/', '.');
			}
			this.DateText = text;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00015BE7 File Offset: 0x00013DE7
		private string GetAnnouncementTypeName(int announcementType)
		{
			if (announcementType == 1)
			{
				return "Event";
			}
			if (announcementType != 2)
			{
				return string.Empty;
			}
			return "Announcement";
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x00015C04 File Offset: 0x00013E04
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x00015C0C File Offset: 0x00013E0C
		[DataSourceProperty]
		public bool IsSingleDate
		{
			get
			{
				return this._isSingleDate;
			}
			set
			{
				if (value != this._isSingleDate)
				{
					this._isSingleDate = value;
					base.OnPropertyChangedWithValue(value, "IsSingleDate");
				}
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00015C2A File Offset: 0x00013E2A
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x00015C32 File Offset: 0x00013E32
		[DataSourceProperty]
		public bool IsPinned
		{
			get
			{
				return this._isPinned;
			}
			set
			{
				if (value != this._isPinned)
				{
					this._isPinned = value;
					base.OnPropertyChangedWithValue(value, "IsPinned");
				}
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00015C50 File Offset: 0x00013E50
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00015C58 File Offset: 0x00013E58
		[DataSourceProperty]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00015C76 File Offset: 0x00013E76
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x00015C7E File Offset: 0x00013E7E
		[DataSourceProperty]
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
			set
			{
				if (value != this._typeName)
				{
					this._typeName = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeName");
				}
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00015CA1 File Offset: 0x00013EA1
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x00015CA9 File Offset: 0x00013EA9
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x00015CCC File Offset: 0x00013ECC
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x00015CD4 File Offset: 0x00013ED4
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x00015CF7 File Offset: 0x00013EF7
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x00015CFF File Offset: 0x00013EFF
		[DataSourceProperty]
		public string DateText
		{
			get
			{
				return this._dateText;
			}
			set
			{
				if (value != this._dateText)
				{
					this._dateText = value;
					base.OnPropertyChangedWithValue<string>(value, "DateText");
				}
			}
		}

		// Token: 0x04000325 RID: 805
		private readonly PublishedLobbyNewsArticle _announcement;

		// Token: 0x04000326 RID: 806
		private bool _isSingleDate;

		// Token: 0x04000327 RID: 807
		private bool _isPinned;

		// Token: 0x04000328 RID: 808
		private int _type;

		// Token: 0x04000329 RID: 809
		private string _typeName;

		// Token: 0x0400032A RID: 810
		private string _title;

		// Token: 0x0400032B RID: 811
		private string _description;

		// Token: 0x0400032C RID: 812
		private string _dateText;
	}
}
