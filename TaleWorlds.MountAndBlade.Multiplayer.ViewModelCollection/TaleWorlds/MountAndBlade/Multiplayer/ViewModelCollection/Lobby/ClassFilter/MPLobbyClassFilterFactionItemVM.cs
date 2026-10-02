using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.ClassFilter
{
	// Token: 0x02000065 RID: 101
	public class MPLobbyClassFilterFactionItemVM : ViewModel
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0001E987 File Offset: 0x0001CB87
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x0001E98F File Offset: 0x0001CB8F
		public BasicCultureObject Culture { get; private set; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x0001E998 File Offset: 0x0001CB98
		// (set) Token: 0x060009D1 RID: 2513 RVA: 0x0001E9A0 File Offset: 0x0001CBA0
		public MPLobbyClassFilterClassItemVM SelectedClassItem { get; private set; }

		// Token: 0x060009D2 RID: 2514 RVA: 0x0001E9AC File Offset: 0x0001CBAC
		public MPLobbyClassFilterFactionItemVM(string cultureCode, bool isEnabled, Action<MPLobbyClassFilterFactionItemVM> onActiveChanged, Action<MPLobbyClassFilterClassItemVM> onClassSelect)
		{
			this._onActiveChanged = onActiveChanged;
			this._onClassSelect = onClassSelect;
			this.CultureCode = cultureCode;
			this.IsEnabled = isEnabled;
			this.Culture = MBObjectManager.Instance.GetObject<BasicCultureObject>(cultureCode);
			this.CreateClassGroupAndClasses(this.Culture);
			this.RefreshValues();
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0001EA00 File Offset: 0x0001CC00
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Hint = new HintViewModel(this.Culture.Name, null);
			this.ClassGroups.ApplyActionOnAllItems(delegate(MPLobbyClassFilterClassGroupItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0001EA54 File Offset: 0x0001CC54
		public override void OnFinalize()
		{
			this.Culture = null;
			this._classGroupDictionary.Clear();
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0001EA68 File Offset: 0x0001CC68
		private void CreateClassGroupAndClasses(BasicCultureObject culture)
		{
			this._classGroupDictionary = new Dictionary<string, MPLobbyClassFilterClassGroupItemVM>();
			this.ClassGroups = new MBBindingList<MPLobbyClassFilterClassGroupItemVM>();
			foreach (MultiplayerClassDivisions.MPHeroClassGroup mpheroClassGroup in MultiplayerClassDivisions.MultiplayerHeroClassGroups)
			{
				MPLobbyClassFilterClassGroupItemVM mplobbyClassFilterClassGroupItemVM = new MPLobbyClassFilterClassGroupItemVM(mpheroClassGroup);
				this.ClassGroups.Add(mplobbyClassFilterClassGroupItemVM);
				this._classGroupDictionary.Add(mpheroClassGroup.StringId, mplobbyClassFilterClassGroupItemVM);
			}
			foreach (MultiplayerClassDivisions.MPHeroClass mpheroClass in MultiplayerClassDivisions.GetMPHeroClasses(this.Culture))
			{
				this._classGroupDictionary[mpheroClass.ClassGroup.StringId].AddClass(culture, mpheroClass, new Action<MPLobbyClassFilterClassItemVM>(this.OnClassItemSelect));
			}
			for (int i = this.ClassGroups.Count - 1; i >= 0; i--)
			{
				if (this.ClassGroups[i].Classes.Count == 0)
				{
					this.ClassGroups.RemoveAt(i);
				}
			}
			MPLobbyClassFilterClassItemVM mplobbyClassFilterClassItemVM = this.ClassGroups[0].Classes[0];
			mplobbyClassFilterClassItemVM.IsSelected = true;
			this.SelectedClassItem = mplobbyClassFilterClassItemVM;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0001EBC4 File Offset: 0x0001CDC4
		private void OnClassItemSelect(MPLobbyClassFilterClassItemVM selectedClassItem)
		{
			foreach (MPLobbyClassFilterClassGroupItemVM mplobbyClassFilterClassGroupItemVM in this.ClassGroups)
			{
				foreach (MPLobbyClassFilterClassItemVM mplobbyClassFilterClassItemVM in mplobbyClassFilterClassGroupItemVM.Classes)
				{
					if (mplobbyClassFilterClassItemVM != selectedClassItem)
					{
						mplobbyClassFilterClassItemVM.IsSelected = false;
					}
				}
			}
			this.SelectedClassItem = selectedClassItem;
			if (this._onClassSelect != null)
			{
				this._onClassSelect(selectedClassItem);
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0001EC64 File Offset: 0x0001CE64
		private void IsActiveChanged()
		{
			if (this.IsActive && this._onActiveChanged != null)
			{
				this._onActiveChanged(this);
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0001EC82 File Offset: 0x0001CE82
		// (set) Token: 0x060009D9 RID: 2521 RVA: 0x0001EC8A File Offset: 0x0001CE8A
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
					this.IsActiveChanged();
				}
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x0001ECAE File Offset: 0x0001CEAE
		// (set) Token: 0x060009DB RID: 2523 RVA: 0x0001ECB6 File Offset: 0x0001CEB6
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x0001ECD4 File Offset: 0x0001CED4
		// (set) Token: 0x060009DD RID: 2525 RVA: 0x0001ECDC File Offset: 0x0001CEDC
		[DataSourceProperty]
		public string CultureCode
		{
			get
			{
				return this._cultureCode;
			}
			set
			{
				if (value != this._cultureCode)
				{
					this._cultureCode = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureCode");
				}
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x0001ECFF File Offset: 0x0001CEFF
		// (set) Token: 0x060009DF RID: 2527 RVA: 0x0001ED07 File Offset: 0x0001CF07
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0001ED25 File Offset: 0x0001CF25
		// (set) Token: 0x060009E1 RID: 2529 RVA: 0x0001ED2D File Offset: 0x0001CF2D
		[DataSourceProperty]
		public MBBindingList<MPLobbyClassFilterClassGroupItemVM> ClassGroups
		{
			get
			{
				return this._classGroups;
			}
			set
			{
				if (value != this._classGroups)
				{
					this._classGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClassFilterClassGroupItemVM>>(value, "ClassGroups");
				}
			}
		}

		// Token: 0x04000481 RID: 1153
		private Action<MPLobbyClassFilterFactionItemVM> _onActiveChanged;

		// Token: 0x04000482 RID: 1154
		private Action<MPLobbyClassFilterClassItemVM> _onClassSelect;

		// Token: 0x04000483 RID: 1155
		private Dictionary<string, MPLobbyClassFilterClassGroupItemVM> _classGroupDictionary;

		// Token: 0x04000486 RID: 1158
		private bool _isActive;

		// Token: 0x04000487 RID: 1159
		private bool _isEnabled;

		// Token: 0x04000488 RID: 1160
		private string _cultureCode;

		// Token: 0x04000489 RID: 1161
		private HintViewModel _hint;

		// Token: 0x0400048A RID: 1162
		private MBBindingList<MPLobbyClassFilterClassGroupItemVM> _classGroups;
	}
}
