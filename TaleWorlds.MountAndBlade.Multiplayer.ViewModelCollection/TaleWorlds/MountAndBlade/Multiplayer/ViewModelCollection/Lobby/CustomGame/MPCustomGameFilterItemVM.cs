using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame
{
	// Token: 0x0200005E RID: 94
	public class MPCustomGameFilterItemVM : ViewModel
	{
		// Token: 0x060008C1 RID: 2241 RVA: 0x0001BF65 File Offset: 0x0001A165
		public MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType filterType, TextObject description, Func<GameServerEntry, bool> getFilterApplicaple, Action onSelectionChange)
		{
			this._filterType = filterType;
			this._descriptionObj = description;
			this.GetIsApplicaple = getFilterApplicaple;
			this._onSelectionChange = onSelectionChange;
			this.SetInitialState();
			this.RefreshValues();
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0001BF96 File Offset: 0x0001A196
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Description = this._descriptionObj.ToString();
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001BFB0 File Offset: 0x0001A1B0
		private void SetInitialState()
		{
			switch (this._filterType)
			{
			case MPCustomGameFiltersVM.CustomGameFilterType.NotFull:
				this.IsSelected = BannerlordConfig.HideFullServers;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.HasPlayers:
				this.IsSelected = BannerlordConfig.HideEmptyServers;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.HasPasswordProtection:
				this.IsSelected = BannerlordConfig.HidePasswordProtectedServers;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.IsOfficial:
				this.IsSelected = BannerlordConfig.HideUnofficialServers;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.ModuleCompatible:
				this.IsSelected = BannerlordConfig.HideModuleIncompatibleServers;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.Favorite:
				this.IsSelected = BannerlordConfig.ShowOnlyFavoriteServers;
				return;
			default:
				return;
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001C02C File Offset: 0x0001A22C
		private void OnToggled()
		{
			this._onSelectionChange();
			switch (this._filterType)
			{
			case MPCustomGameFiltersVM.CustomGameFilterType.NotFull:
				BannerlordConfig.HideFullServers = this.IsSelected;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.HasPlayers:
				BannerlordConfig.HideEmptyServers = this.IsSelected;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.HasPasswordProtection:
				BannerlordConfig.HidePasswordProtectedServers = this.IsSelected;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.IsOfficial:
				BannerlordConfig.HideUnofficialServers = this.IsSelected;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.ModuleCompatible:
				BannerlordConfig.HideModuleIncompatibleServers = this.IsSelected;
				return;
			default:
				return;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0001C0A3 File Offset: 0x0001A2A3
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x0001C0AB File Offset: 0x0001A2AB
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
					base.OnPropertyChangedWithValue(value, "IsSelected");
					this.OnToggled();
				}
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0001C0CF File Offset: 0x0001A2CF
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x0001C0D7 File Offset: 0x0001A2D7
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

		// Token: 0x0400040C RID: 1036
		public readonly Func<GameServerEntry, bool> GetIsApplicaple;

		// Token: 0x0400040D RID: 1037
		public readonly Action _onSelectionChange;

		// Token: 0x0400040E RID: 1038
		private readonly TextObject _descriptionObj;

		// Token: 0x0400040F RID: 1039
		private MPCustomGameFiltersVM.CustomGameFilterType _filterType;

		// Token: 0x04000410 RID: 1040
		private bool _isSelected;

		// Token: 0x04000411 RID: 1041
		private string _description;
	}
}
