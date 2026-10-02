using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Library.NewsManager;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000016 RID: 22
	public class LauncherNewsVM : ViewModel
	{
		// Token: 0x060000CD RID: 205 RVA: 0x00004D33 File Offset: 0x00002F33
		public LauncherNewsVM(NewsManager newsManager, bool isDefaultMultiplayer)
		{
			this._newsManager = newsManager;
			this.NewsItems = new MBBindingList<LauncherNewsItemVM>();
			this.GetNewsItems(isDefaultMultiplayer);
			this.IsDisabledOnMultiplayer = false;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004D5C File Offset: 0x00002F5C
		private async void GetNewsItems(bool isMultiplayer)
		{
			await this._newsManager.GetNewsItems(false);
			this.Refresh(isMultiplayer);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004DA0 File Offset: 0x00002FA0
		public void Refresh(bool isMultiplayer)
		{
			this.NewsItems.Clear();
			this.MainNews = new LauncherNewsItemVM(default(NewsItem), isMultiplayer);
			NewsItem.NewsTypes singleplayerMultiplayerEnum = (isMultiplayer ? NewsItem.NewsTypes.LauncherMultiplayer : NewsItem.NewsTypes.LauncherSingleplayer);
			List<NewsItem> list = new List<NewsItem>();
			Func<NewsType, bool> <>9__2;
			foreach (NewsItem newsItem in this._newsManager.NewsItems)
			{
				List<NewsType> feeds = newsItem.Feeds;
				bool flag;
				if (feeds == null)
				{
					flag = false;
				}
				else
				{
					Func<NewsType, bool> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = (NewsType f) => f.Type == singleplayerMultiplayerEnum);
					}
					flag = feeds.Any<NewsType>(func);
				}
				if (flag && !string.IsNullOrEmpty(newsItem.Title) && !string.IsNullOrEmpty(newsItem.NewsLink) && !string.IsNullOrEmpty(newsItem.ImageSourcePath))
				{
					list.Add(newsItem);
				}
			}
			Func<NewsType, bool> <>9__3;
			List<IGrouping<int, NewsItem>> list2 = (from i in list.GroupBy<NewsItem, int>(delegate(NewsItem i)
				{
					IEnumerable<NewsType> feeds2 = i.Feeds;
					Func<NewsType, bool> func2;
					if ((func2 = <>9__3) == null)
					{
						func2 = (<>9__3 = (NewsType t) => t.Type == singleplayerMultiplayerEnum);
					}
					return feeds2.FirstOrDefault<NewsType>(func2).Index;
				}).ToList<IGrouping<int, NewsItem>>()
				orderby i.Key
				select i).ToList<IGrouping<int, NewsItem>>();
			int num = 0;
			while (num < list2.Count && this.NewsItems.Count < 3)
			{
				NewsItem newsItem2 = list2[num].First<NewsItem>();
				NewsItem newsItem3 = (newsItem2.Equals(default(NewsItem)) ? default(NewsItem) : newsItem2);
				this.NewsItems.Add(new LauncherNewsItemVM(newsItem3, isMultiplayer));
				num++;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004F48 File Offset: 0x00003148
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00004F50 File Offset: 0x00003150
		[DataSourceProperty]
		public bool IsDisabledOnMultiplayer
		{
			get
			{
				return this._isDisabledOnMultiplayer;
			}
			set
			{
				if (value != this._isDisabledOnMultiplayer)
				{
					this._isDisabledOnMultiplayer = value;
					base.OnPropertyChangedWithValue(value, "IsDisabledOnMultiplayer");
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00004F6E File Offset: 0x0000316E
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00004F76 File Offset: 0x00003176
		[DataSourceProperty]
		public LauncherNewsItemVM MainNews
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
					base.OnPropertyChangedWithValue<LauncherNewsItemVM>(value, "MainNews");
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004F94 File Offset: 0x00003194
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00004F9C File Offset: 0x0000319C
		[DataSourceProperty]
		public MBBindingList<LauncherNewsItemVM> NewsItems
		{
			get
			{
				return this._newsItems;
			}
			set
			{
				if (value != this._newsItems)
				{
					this._newsItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<LauncherNewsItemVM>>(value, "NewsItems");
				}
			}
		}

		// Token: 0x04000067 RID: 103
		private readonly NewsManager _newsManager;

		// Token: 0x04000068 RID: 104
		private const int _numOfNewsItemsToShow = 3;

		// Token: 0x04000069 RID: 105
		private LauncherNewsItemVM _mainNews;

		// Token: 0x0400006A RID: 106
		private MBBindingList<LauncherNewsItemVM> _newsItems;

		// Token: 0x0400006B RID: 107
		private bool _isDisabledOnMultiplayer;
	}
}
