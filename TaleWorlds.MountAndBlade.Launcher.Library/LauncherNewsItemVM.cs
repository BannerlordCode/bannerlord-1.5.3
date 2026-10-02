using System;
using System.Diagnostics;
using TaleWorlds.Library;
using TaleWorlds.Library.NewsManager;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000015 RID: 21
	public class LauncherNewsItemVM : ViewModel
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x00004C30 File Offset: 0x00002E30
		public LauncherNewsItemVM(NewsItem item, bool isMultiplayer)
		{
			this.Category = item.Title;
			this.Title = item.Description;
			this.NewsImageUrl = item.ImageSourcePath;
			this._link = item.NewsLink + (isMultiplayer ? "?referrer=launchermp" : "?referrer=launchersp");
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004C8B File Offset: 0x00002E8B
		private void ExecuteOpenLink()
		{
			if (!string.IsNullOrEmpty(this._link))
			{
				Process.Start(new ProcessStartInfo(this._link)
				{
					UseShellExecute = true
				});
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004CB2 File Offset: 0x00002EB2
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00004CBA File Offset: 0x00002EBA
		[DataSourceProperty]
		public string NewsImageUrl
		{
			get
			{
				return this._newsImageUrl;
			}
			set
			{
				if (value != this._newsImageUrl)
				{
					this._newsImageUrl = value;
					base.OnPropertyChangedWithValue<string>(value, "NewsImageUrl");
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004CDD File Offset: 0x00002EDD
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00004CE5 File Offset: 0x00002EE5
		[DataSourceProperty]
		public string Category
		{
			get
			{
				return this._category;
			}
			set
			{
				if (value != this._category)
				{
					this._category = value;
					base.OnPropertyChangedWithValue<string>(value, "Category");
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00004D08 File Offset: 0x00002F08
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00004D10 File Offset: 0x00002F10
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

		// Token: 0x04000063 RID: 99
		private string _link;

		// Token: 0x04000064 RID: 100
		private string _newsImageUrl;

		// Token: 0x04000065 RID: 101
		private string _category;

		// Token: 0x04000066 RID: 102
		private string _title;
	}
}
