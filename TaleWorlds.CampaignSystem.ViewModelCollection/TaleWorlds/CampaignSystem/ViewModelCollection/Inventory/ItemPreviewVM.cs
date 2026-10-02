using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000098 RID: 152
	public class ItemPreviewVM : ViewModel
	{
		// Token: 0x06000CF4 RID: 3316 RVA: 0x00036FD9 File Offset: 0x000351D9
		public ItemPreviewVM(Action onClosed)
		{
			this._onClosed = onClosed;
			this.ItemTableau = new ItemCollectionElementViewModel();
			this.RefreshValues();
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00036FF9 File Offset: 0x000351F9
		public override void OnFinalize()
		{
			this.ItemTableau.OnFinalize();
			this.ItemTableau = null;
			base.OnFinalize();
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00037013 File Offset: 0x00035213
		public void Open(EquipmentElement item)
		{
			this.ItemTableau.FillFrom(item, Clan.PlayerClan.Banner);
			this.ItemName = item.Item.Name.ToString();
			this.IsSelected = true;
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00037049 File Offset: 0x00035249
		public void ExecuteClose()
		{
			this.Close();
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00037051 File Offset: 0x00035251
		public void Close()
		{
			this._onClosed();
			this.IsSelected = false;
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x00037065 File Offset: 0x00035265
		// (set) Token: 0x06000CFA RID: 3322 RVA: 0x0003706D File Offset: 0x0003526D
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
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x0003708A File Offset: 0x0003528A
		// (set) Token: 0x06000CFC RID: 3324 RVA: 0x00037092 File Offset: 0x00035292
		[DataSourceProperty]
		public string ItemName
		{
			get
			{
				return this._itemName;
			}
			set
			{
				if (value != this._itemName)
				{
					this._itemName = value;
					base.OnPropertyChanged("ItemName");
				}
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x000370B4 File Offset: 0x000352B4
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x000370BC File Offset: 0x000352BC
		[DataSourceProperty]
		public ItemCollectionElementViewModel ItemTableau
		{
			get
			{
				return this._itemTableau;
			}
			set
			{
				if (value != this._itemTableau)
				{
					this._itemTableau = value;
					base.OnPropertyChangedWithValue<ItemCollectionElementViewModel>(value, "ItemTableau");
				}
			}
		}

		// Token: 0x040005DF RID: 1503
		private Action _onClosed;

		// Token: 0x040005E0 RID: 1504
		private bool _isSelected;

		// Token: 0x040005E1 RID: 1505
		private string _itemName;

		// Token: 0x040005E2 RID: 1506
		private ItemCollectionElementViewModel _itemTableau;
	}
}
