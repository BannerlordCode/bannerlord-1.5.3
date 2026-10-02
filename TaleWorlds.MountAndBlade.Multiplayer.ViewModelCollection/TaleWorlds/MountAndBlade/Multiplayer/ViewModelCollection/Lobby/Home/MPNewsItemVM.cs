using System;
using System.Diagnostics;
using TaleWorlds.Library;
using TaleWorlds.Library.NewsManager;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x02000051 RID: 81
	public class MPNewsItemVM : ViewModel
	{
		// Token: 0x06000717 RID: 1815 RVA: 0x000167CC File Offset: 0x000149CC
		public MPNewsItemVM(NewsItem item)
		{
			this.NewsImageUrl = item.ImageSourcePath;
			this.Category = item.Title;
			this.Title = item.Description;
			this._link = item.NewsLink + "?referrer=lobby";
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001681D File Offset: 0x00014A1D
		private void ExecuteOpenLink()
		{
			if (!string.IsNullOrEmpty(this._link) && !PlatformServices.Instance.ShowOverlayForWebPage(this._link).Result)
			{
				Process.Start(new ProcessStartInfo(this._link)
				{
					UseShellExecute = true
				});
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0001685B File Offset: 0x00014A5B
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x00016863 File Offset: 0x00014A63
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

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x00016886 File Offset: 0x00014A86
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x0001688E File Offset: 0x00014A8E
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

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x000168B1 File Offset: 0x00014AB1
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x000168B9 File Offset: 0x00014AB9
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

		// Token: 0x0400034E RID: 846
		private readonly string _link;

		// Token: 0x0400034F RID: 847
		private string _newsImageUrl;

		// Token: 0x04000350 RID: 848
		private string _category;

		// Token: 0x04000351 RID: 849
		private string _title;
	}
}
