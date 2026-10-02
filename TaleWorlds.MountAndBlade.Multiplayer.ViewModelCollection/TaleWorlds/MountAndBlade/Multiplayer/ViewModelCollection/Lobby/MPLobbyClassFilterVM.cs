using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.ClassFilter;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000025 RID: 37
	public class MPLobbyClassFilterVM : ViewModel
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000A33A File Offset: 0x0000853A
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000A342 File Offset: 0x00008542
		public MPLobbyClassFilterClassItemVM SelectedClassItem { get; private set; }

		// Token: 0x06000294 RID: 660 RVA: 0x0000A34C File Offset: 0x0000854C
		public MPLobbyClassFilterVM(Action<MPLobbyClassFilterClassItemVM, bool> onSelectionChange)
		{
			this._onSelectionChange = onSelectionChange;
			this.Factions = new MBBindingList<MPLobbyClassFilterFactionItemVM>();
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("empire", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("vlandia", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("battania", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("sturgia", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("khuzait", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("aserai", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.ActiveClassGroups = new MBBindingList<MPLobbyClassFilterClassGroupItemVM>();
			this.Factions[0].IsActive = true;
			this.RefreshValues();
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000A4A8 File Offset: 0x000086A8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=Q50X65NB}Classes", null).ToString();
			this.Factions.ApplyActionOnAllItems(delegate(MPLobbyClassFilterFactionItemVM x)
			{
				x.RefreshValues();
			});
			this.ActiveClassGroups.ApplyActionOnAllItems(delegate(MPLobbyClassFilterClassGroupItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000A525 File Offset: 0x00008725
		private void OnFactionFilterChanged(MPLobbyClassFilterFactionItemVM factionItemVm)
		{
			this.ActiveClassGroups = factionItemVm.ClassGroups;
			this.OnSelectionChange(factionItemVm.SelectedClassItem);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000A53F File Offset: 0x0000873F
		private void OnSelectionChange(MPLobbyClassFilterClassItemVM selectedItemVm)
		{
			this.SelectedClassItem = selectedItemVm;
			Action<MPLobbyClassFilterClassItemVM, bool> onSelectionChange = this._onSelectionChange;
			if (onSelectionChange == null)
			{
				return;
			}
			onSelectionChange(selectedItemVm, false);
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000A55A File Offset: 0x0000875A
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000A562 File Offset: 0x00008762
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

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000A585 File Offset: 0x00008785
		// (set) Token: 0x0600029B RID: 667 RVA: 0x0000A58D File Offset: 0x0000878D
		[DataSourceProperty]
		public MBBindingList<MPLobbyClassFilterFactionItemVM> Factions
		{
			get
			{
				return this._factions;
			}
			set
			{
				if (value != this._factions)
				{
					this._factions = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClassFilterFactionItemVM>>(value, "Factions");
				}
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000A5AB File Offset: 0x000087AB
		// (set) Token: 0x0600029D RID: 669 RVA: 0x0000A5B3 File Offset: 0x000087B3
		[DataSourceProperty]
		public MBBindingList<MPLobbyClassFilterClassGroupItemVM> ActiveClassGroups
		{
			get
			{
				return this._activeClassGroups;
			}
			set
			{
				if (value != this._activeClassGroups)
				{
					this._activeClassGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClassFilterClassGroupItemVM>>(value, "ActiveClassGroups");
				}
			}
		}

		// Token: 0x0400015A RID: 346
		private Action<MPLobbyClassFilterClassItemVM, bool> _onSelectionChange;

		// Token: 0x0400015C RID: 348
		private string _titleText;

		// Token: 0x0400015D RID: 349
		private MBBindingList<MPLobbyClassFilterFactionItemVM> _factions;

		// Token: 0x0400015E RID: 350
		private MBBindingList<MPLobbyClassFilterClassGroupItemVM> _activeClassGroups;
	}
}
