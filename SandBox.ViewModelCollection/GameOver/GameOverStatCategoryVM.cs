using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.GameOver
{
	// Token: 0x02000058 RID: 88
	public class GameOverStatCategoryVM : ViewModel
	{
		// Token: 0x06000594 RID: 1428 RVA: 0x00015350 File Offset: 0x00013550
		public GameOverStatCategoryVM(StatCategory category, Action<GameOverStatCategoryVM> onSelect)
		{
			this._category = category;
			this._onSelect = onSelect;
			this.Items = new MBBindingList<GameOverStatItemVM>();
			this.ID = category.ID;
			this.RefreshValues();
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00015384 File Offset: 0x00013584
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Items.Clear();
			this.Name = GameTexts.FindText("str_game_over_stat_category", this._category.ID).ToString();
			foreach (StatItem statItem in this._category.Items)
			{
				this.Items.Add(new GameOverStatItemVM(statItem));
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00015414 File Offset: 0x00013614
		public void ExecuteSelectCategory()
		{
			Action<GameOverStatCategoryVM> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect.DynamicInvokeWithLog(new object[] { this });
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00015431 File Offset: 0x00013631
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x00015439 File Offset: 0x00013639
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0001545C File Offset: 0x0001365C
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x00015464 File Offset: 0x00013664
		[DataSourceProperty]
		public string ID
		{
			get
			{
				return this._id;
			}
			set
			{
				if (value != this._id)
				{
					this._id = value;
					base.OnPropertyChangedWithValue<string>(value, "ID");
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00015487 File Offset: 0x00013687
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x0001548F File Offset: 0x0001368F
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
				}
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x000154AD File Offset: 0x000136AD
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x000154B5 File Offset: 0x000136B5
		[DataSourceProperty]
		public MBBindingList<GameOverStatItemVM> Items
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
					base.OnPropertyChangedWithValue<MBBindingList<GameOverStatItemVM>>(value, "Items");
				}
			}
		}

		// Token: 0x040002CB RID: 715
		private readonly StatCategory _category;

		// Token: 0x040002CC RID: 716
		private readonly Action<GameOverStatCategoryVM> _onSelect;

		// Token: 0x040002CD RID: 717
		private string _name;

		// Token: 0x040002CE RID: 718
		private string _id;

		// Token: 0x040002CF RID: 719
		private bool _isSelected;

		// Token: 0x040002D0 RID: 720
		private MBBindingList<GameOverStatItemVM> _items;
	}
}
