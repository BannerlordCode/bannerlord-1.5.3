using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Library.NewsManager;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x02000052 RID: 82
	public class MPNewsVM : ViewModel
	{
		// Token: 0x0600071F RID: 1823 RVA: 0x000168DC File Offset: 0x00014ADC
		public MPNewsVM(NewsManager newsManager)
		{
			this._newsManager = newsManager;
			this.ImportantNews = new MBBindingList<MPNewsItemVM>();
			this.GetNewsItems();
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x000168FC File Offset: 0x00014AFC
		private async void GetNewsItems()
		{
			if (this._newsManager == null)
			{
				Debug.FailedAssert("News manager is null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Home\\MPNewsVM.cs", "GetNewsItems", 27);
			}
			else
			{
				MBReadOnlyList<NewsItem> mbreadOnlyList = await this._newsManager.GetNewsItems(false);
				this._newsItemsCached = mbreadOnlyList;
				this.RefreshNews();
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00016938 File Offset: 0x00014B38
		private void RefreshNews()
		{
			this.MainNews = null;
			this.ImportantNews.Clear();
			this.HasValidNews = false;
			if (this._newsItemsCached == null)
			{
				Debug.FailedAssert("News items list is null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Home\\MPNewsVM.cs", "RefreshNews", 43);
				return;
			}
			List<IGrouping<int, NewsItem>> list = (from i in (from i in this._newsItemsCached.Where<NewsItem>((NewsItem n) => n.Feeds.Any<NewsType>((NewsType t) => t.Type == NewsItem.NewsTypes.MultiplayerLobby) && !string.IsNullOrEmpty(n.Title) && !string.IsNullOrEmpty(n.NewsLink) && !string.IsNullOrEmpty(n.ImageSourcePath)).ToList<NewsItem>()
					group i by i.Feeds.First<NewsType>((NewsType t) => t.Type == NewsItem.NewsTypes.MultiplayerLobby).Index).ToList<IGrouping<int, NewsItem>>()
				orderby i.Key
				select i).ToList<IGrouping<int, NewsItem>>();
			int num = 0;
			while (num < list.Count && this.ImportantNews.Count + 1 < 4)
			{
				NewsItem newsItem = list[num].First<NewsItem>();
				NewsItem newsItem2 = (newsItem.Equals(default(NewsItem)) ? default(NewsItem) : newsItem);
				if (num == 0)
				{
					this.MainNews = new MPNewsItemVM(newsItem2);
				}
				else
				{
					this.ImportantNews.Add(new MPNewsItemVM(newsItem2));
				}
				num++;
			}
			if (this.MainNews != null)
			{
				this.HasValidNews = true;
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00016A8A File Offset: 0x00014C8A
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._newsManager = null;
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00016A99 File Offset: 0x00014C99
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x00016AA1 File Offset: 0x00014CA1
		[DataSourceProperty]
		public bool HasValidNews
		{
			get
			{
				return this._hasValidNews;
			}
			set
			{
				if (value != this._hasValidNews)
				{
					this._hasValidNews = value;
					base.OnPropertyChangedWithValue(value, "HasValidNews");
				}
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x00016ABF File Offset: 0x00014CBF
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x00016AC7 File Offset: 0x00014CC7
		[DataSourceProperty]
		public MPNewsItemVM MainNews
		{
			get
			{
				return this._mainNews;
			}
			set
			{
				if (value != this._mainNews)
				{
					this._mainNews = value;
					base.OnPropertyChangedWithValue<MPNewsItemVM>(value, "MainNews");
				}
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00016AE5 File Offset: 0x00014CE5
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x00016AED File Offset: 0x00014CED
		[DataSourceProperty]
		public MBBindingList<MPNewsItemVM> ImportantNews
		{
			get
			{
				return this._importantNews;
			}
			set
			{
				if (value != this._importantNews)
				{
					this._importantNews = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPNewsItemVM>>(value, "ImportantNews");
				}
			}
		}

		// Token: 0x04000352 RID: 850
		private NewsManager _newsManager;

		// Token: 0x04000353 RID: 851
		private const int _numOfNewsItemsToShow = 4;

		// Token: 0x04000354 RID: 852
		private MBReadOnlyList<NewsItem> _newsItemsCached;

		// Token: 0x04000355 RID: 853
		private bool _hasValidNews;

		// Token: 0x04000356 RID: 854
		private MPNewsItemVM _mainNews;

		// Token: 0x04000357 RID: 855
		private MBBindingList<MPNewsItemVM> _importantNews;
	}
}
