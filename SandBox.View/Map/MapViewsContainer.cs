using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaleWorlds.Core;

namespace SandBox.View.Map
{
	// Token: 0x0200005E RID: 94
	public class MapViewsContainer
	{
		// Token: 0x0600039C RID: 924 RVA: 0x0001CAAC File Offset: 0x0001ACAC
		public MapViewsContainer()
		{
			this.MapViews = new ObservableCollection<MapView>();
			this._mapViewsCopyCache = this.MapViews.ToList<MapView>();
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001CAD0 File Offset: 0x0001ACD0
		public void Add(MapView mapView)
		{
			this.MapViews.Add(mapView);
			this._isViewListDirty = true;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001CAE5 File Offset: 0x0001ACE5
		public void Remove(MapView mapView)
		{
			this.MapViews.Remove(mapView);
			this._isViewListDirty = true;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001CAFB File Offset: 0x0001ACFB
		public bool Contains(MapView mapView)
		{
			return this.MapViews.Contains(mapView);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0001CB0C File Offset: 0x0001AD0C
		public void Foreach(Action<MapView> action)
		{
			foreach (MapView mapView in this.GetMapViewsCopy())
			{
				if (!mapView.IsFinalized)
				{
					action(mapView);
				}
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0001CB68 File Offset: 0x0001AD68
		public void ForeachReverse(Action<MapView> action)
		{
			List<MapView> mapViewsCopy = this.GetMapViewsCopy();
			for (int i = mapViewsCopy.Count - 1; i >= 0; i--)
			{
				if (!mapViewsCopy[i].IsFinalized)
				{
					action(mapViewsCopy[i]);
				}
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001CBAC File Offset: 0x0001ADAC
		public MapView ReturnFirstElementWithCondition(Func<MapView, bool> condition)
		{
			foreach (MapView mapView in this.GetMapViewsCopy())
			{
				if (!mapView.IsFinalized && condition(mapView))
				{
					return mapView;
				}
			}
			return null;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001CC10 File Offset: 0x0001AE10
		public T GetMapViewWithType<T>() where T : MapView
		{
			foreach (MapView mapView in this.GetMapViewsCopy())
			{
				if (!mapView.IsFinalized && mapView is T)
				{
					return mapView as T;
				}
			}
			return default(T);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001CC88 File Offset: 0x0001AE88
		public TutorialContexts GetContextToChangeTo()
		{
			foreach (MapView mapView in this.GetMapViewsCopy())
			{
				if (!mapView.IsFinalized)
				{
					TutorialContexts tutorialContext = mapView.GetTutorialContext();
					if (tutorialContext != TutorialContexts.MapWindow)
					{
						return tutorialContext;
					}
				}
			}
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0001CCF0 File Offset: 0x0001AEF0
		public bool IsThereAnyViewIsEscaped()
		{
			return this.ReturnFirstElementWithCondition((MapView view) => view.IsEscaped()) != null;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001CD1C File Offset: 0x0001AF1C
		public bool IsOpeningEscapeMenuOnFocusChangeAllowedForAll()
		{
			bool flag = true;
			foreach (MapView mapView in this.GetMapViewsCopy())
			{
				if (!mapView.IsFinalized)
				{
					flag &= mapView.IsOpeningEscapeMenuOnFocusChangeAllowed();
				}
			}
			return flag;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001CD7C File Offset: 0x0001AF7C
		private List<MapView> GetMapViewsCopy()
		{
			if (this._isViewListDirty)
			{
				this._mapViewsCopyCache = this.MapViews.ToList<MapView>();
				this._isViewListDirty = false;
			}
			return this._mapViewsCopyCache;
		}

		// Token: 0x040001DB RID: 475
		public readonly ObservableCollection<MapView> MapViews;

		// Token: 0x040001DC RID: 476
		private List<MapView> _mapViewsCopyCache;

		// Token: 0x040001DD RID: 477
		private bool _isViewListDirty;
	}
}
