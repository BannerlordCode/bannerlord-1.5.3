using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement
{
	// Token: 0x02000068 RID: 104
	public abstract class KingdomCategoryVM : ViewModel
	{
		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x00023CD9 File Offset: 0x00021ED9
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x00023CE1 File Offset: 0x00021EE1
		[DataSourceProperty]
		public string CategoryNameText
		{
			get
			{
				return this._categoryNameText;
			}
			set
			{
				if (value != this._categoryNameText)
				{
					this._categoryNameText = value;
					base.OnPropertyChanged("NameText");
				}
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x00023D03 File Offset: 0x00021F03
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x00023D0B File Offset: 0x00021F0B
		[DataSourceProperty]
		public string NoItemSelectedText
		{
			get
			{
				return this._noItemSelectedText;
			}
			set
			{
				if (value != this._noItemSelectedText)
				{
					this._noItemSelectedText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoItemSelectedText");
				}
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x00023D2E File Offset: 0x00021F2E
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x00023D36 File Offset: 0x00021F36
		[DataSourceProperty]
		public bool IsAcceptableItemSelected
		{
			get
			{
				return this._isAcceptableItemSelected;
			}
			set
			{
				if (value != this._isAcceptableItemSelected)
				{
					this._isAcceptableItemSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAcceptableItemSelected");
				}
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x00023D54 File Offset: 0x00021F54
		// (set) Token: 0x0600077E RID: 1918 RVA: 0x00023D5C File Offset: 0x00021F5C
		[DataSourceProperty]
		public int NotificationCount
		{
			get
			{
				return this._notificationCount;
			}
			set
			{
				if (value != this._notificationCount)
				{
					this._notificationCount = value;
					base.OnPropertyChanged("NotificationCount");
				}
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00023D79 File Offset: 0x00021F79
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x00023D81 File Offset: 0x00021F81
		[DataSourceProperty]
		public bool Show
		{
			get
			{
				return this._show;
			}
			set
			{
				if (value != this._show)
				{
					this._show = value;
					base.OnPropertyChanged("Show");
				}
			}
		}

		// Token: 0x04000334 RID: 820
		private int _notificationCount;

		// Token: 0x04000335 RID: 821
		private string _categoryNameText;

		// Token: 0x04000336 RID: 822
		private string _noItemSelectedText;

		// Token: 0x04000337 RID: 823
		private bool _show;

		// Token: 0x04000338 RID: 824
		private bool _isAcceptableItemSelected;
	}
}
