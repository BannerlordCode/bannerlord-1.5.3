using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame
{
	// Token: 0x0200005F RID: 95
	public class MPCustomGameFiltersVM : ViewModel
	{
		// Token: 0x060008C9 RID: 2249 RVA: 0x0001C0FC File Offset: 0x0001A2FC
		public MPCustomGameFiltersVM()
		{
			this.SearchText = string.Empty;
			MBBindingList<MPCustomGameFilterItemVM> mbbindingList = new MBBindingList<MPCustomGameFilterItemVM>();
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.IsOfficial, new TextObject("{=Tlc2buKG}Is Official", null), (GameServerEntry x) => x.IsOfficial, new Action(this.OnAnyFilterChange)));
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.HasPlayers, new TextObject("{=aB4Md0if}Has players", null), (GameServerEntry x) => x.PlayerCount > 0, new Action(this.OnAnyFilterChange)));
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.HasPasswordProtection, new TextObject("{=v6J8ILV3}No password", null), (GameServerEntry x) => !x.PasswordProtected, new Action(this.OnAnyFilterChange)));
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.NotFull, new TextObject("{=W4DLzPSb}Server not full", null), (GameServerEntry x) => x.MaxPlayerCount - x.PlayerCount > 0, new Action(this.OnAnyFilterChange)));
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.ModuleCompatible, new TextObject("{=CNR4cZwZ}Modules compatible", null), new Func<GameServerEntry, bool>(this.FilterByCompatibleModules), new Action(this.OnAnyFilterChange)));
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.Favorite, new TextObject("{=BDdVhfuJ}Favorite", null), new Func<GameServerEntry, bool>(this.FilterByFavorites), new Action(this.OnAnyFilterChange)));
			this.Items = mbbindingList;
			this.RefreshValues();
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001C294 File Offset: 0x0001A494
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=OwqFpPwa}Filters", null).ToString();
			this.SearchInitialText = new TextObject("{=NLKmdNbt}Search", null).ToString();
			this.Items.ApplyActionOnAllItems(delegate(MPCustomGameFilterItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0001C300 File Offset: 0x0001A500
		public List<GameServerEntry> GetFilteredServerList(IEnumerable<GameServerEntry> unfilteredList)
		{
			List<GameServerEntry> list = unfilteredList.ToList<GameServerEntry>();
			IEnumerable<MPCustomGameFilterItemVM> enabledFilterItems = this.Items.Where<MPCustomGameFilterItemVM>((MPCustomGameFilterItemVM filterItem) => filterItem.IsSelected);
			if (enabledFilterItems.Any<MPCustomGameFilterItemVM>())
			{
				list.RemoveAll((GameServerEntry s) => enabledFilterItems.Any<MPCustomGameFilterItemVM>((MPCustomGameFilterItemVM fi) => !fi.GetIsApplicaple(s)));
			}
			if (!string.IsNullOrEmpty(this.SearchText))
			{
				list = list.Where<GameServerEntry>((GameServerEntry i) => i.ServerName.IndexOf(this.SearchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList<GameServerEntry>();
			}
			return list;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001C397 File Offset: 0x0001A597
		private bool FilterByCompatibleModules(GameServerEntry serverEntry)
		{
			return NetworkMain.GameClient.LoadedUnofficialModules.IsCompatibleWith(serverEntry.LoadedModules, serverEntry.AllowsOptionalModules);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0001C3B4 File Offset: 0x0001A5B4
		private bool FilterByFavorites(GameServerEntry serverEntry)
		{
			FavoriteServerData favoriteServerData;
			return MultiplayerLocalDataManager.Instance.FavoriteServers.TryGetServerData(serverEntry, out favoriteServerData);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0001C3D3 File Offset: 0x0001A5D3
		private void OnAnyFilterChange()
		{
			Action onFiltersApplied = this.OnFiltersApplied;
			if (onFiltersApplied == null)
			{
				return;
			}
			onFiltersApplied();
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0001C3E5 File Offset: 0x0001A5E5
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x0001C3ED File Offset: 0x0001A5ED
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

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0001C410 File Offset: 0x0001A610
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x0001C418 File Offset: 0x0001A618
		[DataSourceProperty]
		public string SearchInitialText
		{
			get
			{
				return this._searchInitialText;
			}
			set
			{
				if (value != this._searchInitialText)
				{
					this._searchInitialText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchInitialText");
				}
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0001C43B File Offset: 0x0001A63B
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0001C443 File Offset: 0x0001A643
		[DataSourceProperty]
		public string SearchText
		{
			get
			{
				return this._searchText;
			}
			set
			{
				if (value != this._searchText)
				{
					this._searchText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchText");
					this.OnAnyFilterChange();
				}
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0001C46C File Offset: 0x0001A66C
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0001C474 File Offset: 0x0001A674
		[DataSourceProperty]
		public MBBindingList<MPCustomGameFilterItemVM> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				if (value != this._items)
				{
					this._items = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPCustomGameFilterItemVM>>(value, "Items");
				}
			}
		}

		// Token: 0x04000412 RID: 1042
		public Action OnFiltersApplied;

		// Token: 0x04000413 RID: 1043
		private string _titleText;

		// Token: 0x04000414 RID: 1044
		private string _searchInitialText;

		// Token: 0x04000415 RID: 1045
		private string _searchText;

		// Token: 0x04000416 RID: 1046
		private MBBindingList<MPCustomGameFilterItemVM> _items;

		// Token: 0x0200012D RID: 301
		public enum CustomGameFilterType
		{
			// Token: 0x040009A6 RID: 2470
			Name,
			// Token: 0x040009A7 RID: 2471
			NotFull,
			// Token: 0x040009A8 RID: 2472
			HasPlayers,
			// Token: 0x040009A9 RID: 2473
			HasPasswordProtection,
			// Token: 0x040009AA RID: 2474
			IsOfficial,
			// Token: 0x040009AB RID: 2475
			ModuleCompatible,
			// Token: 0x040009AC RID: 2476
			Favorite
		}
	}
}
