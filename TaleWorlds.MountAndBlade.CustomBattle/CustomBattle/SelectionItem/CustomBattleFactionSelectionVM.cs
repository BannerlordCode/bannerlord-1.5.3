using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x0200001E RID: 30
	public class CustomBattleFactionSelectionVM : ViewModel
	{
		// Token: 0x060001AA RID: 426 RVA: 0x0000A3F8 File Offset: 0x000085F8
		public CustomBattleFactionSelectionVM(Action<BasicCultureObject> onSelectionChanged)
		{
			this._onSelectionChanged = onSelectionChanged;
			this.Factions = new MBBindingList<FactionItemVM>();
			foreach (BasicCultureObject basicCultureObject in CustomBattleData.Factions)
			{
				this.Factions.Add(new FactionItemVM(basicCultureObject, new Action<FactionItemVM>(this.OnFactionSelected)));
			}
			this.SelectFaction(0);
			this.RefreshValues();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000A480 File Offset: 0x00008680
		public override void RefreshValues()
		{
			base.RefreshValues();
			FactionItemVM selectedItem = this.SelectedItem;
			this.SelectedFactionName = ((selectedItem != null) ? selectedItem.Faction.Name.ToString() : null);
			this.Factions.ApplyActionOnAllItems(delegate(FactionItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000A4DF File Offset: 0x000086DF
		public void SelectFaction(int index)
		{
			if (index >= 0 && index < this.Factions.Count)
			{
				this.SelectedItem = this.Factions[index];
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000A50C File Offset: 0x0000870C
		public void ExecuteRandomize()
		{
			int num = MBRandom.RandomInt(this.Factions.Count);
			this.SelectFaction(num);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000A531 File Offset: 0x00008731
		private void OnFactionSelected(FactionItemVM faction)
		{
			this.SelectedItem = faction;
			this._onSelectionChanged(faction.Faction);
			this.SelectedFactionName = this.SelectedItem.Faction.Name.ToString();
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000A566 File Offset: 0x00008766
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000A56E File Offset: 0x0000876E
		[DataSourceProperty]
		public MBBindingList<FactionItemVM> Factions
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
					base.OnPropertyChangedWithValue<MBBindingList<FactionItemVM>>(value, "Factions");
				}
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000A58C File Offset: 0x0000878C
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x0000A594 File Offset: 0x00008794
		[DataSourceProperty]
		public string SelectedFactionName
		{
			get
			{
				return this._selectedFactionName;
			}
			set
			{
				if (value != this._selectedFactionName)
				{
					this._selectedFactionName = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedFactionName");
				}
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000A5B7 File Offset: 0x000087B7
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x0000A5C0 File Offset: 0x000087C0
		[DataSourceProperty]
		public FactionItemVM SelectedItem
		{
			get
			{
				return this._selectedItem;
			}
			set
			{
				if (value != this._selectedItem)
				{
					if (this._selectedItem != null)
					{
						this._selectedItem.IsSelected = false;
					}
					this._selectedItem = value;
					base.OnPropertyChangedWithValue<FactionItemVM>(value, "SelectedItem");
					if (this._selectedItem != null)
					{
						this._selectedItem.IsSelected = true;
					}
				}
			}
		}

		// Token: 0x04000104 RID: 260
		private Action<BasicCultureObject> _onSelectionChanged;

		// Token: 0x04000105 RID: 261
		private MBBindingList<FactionItemVM> _factions;

		// Token: 0x04000106 RID: 262
		private string _selectedFactionName;

		// Token: 0x04000107 RID: 263
		private FactionItemVM _selectedItem;
	}
}
