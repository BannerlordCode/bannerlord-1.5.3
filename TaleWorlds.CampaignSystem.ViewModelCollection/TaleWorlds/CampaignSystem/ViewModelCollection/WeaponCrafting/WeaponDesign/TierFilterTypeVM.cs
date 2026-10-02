using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000109 RID: 265
	public class TierFilterTypeVM : ViewModel
	{
		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x060017BA RID: 6074 RVA: 0x0005B66C File Offset: 0x0005986C
		public WeaponDesignVM.CraftingPieceTierFilter FilterType { get; }

		// Token: 0x060017BB RID: 6075 RVA: 0x0005B674 File Offset: 0x00059874
		public TierFilterTypeVM(WeaponDesignVM.CraftingPieceTierFilter filterType, Action<WeaponDesignVM.CraftingPieceTierFilter> onSelect, string tierName)
		{
			this.FilterType = filterType;
			this._onSelect = onSelect;
			this.TierName = tierName;
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x0005B691 File Offset: 0x00059891
		public void ExecuteSelectTier()
		{
			Action<WeaponDesignVM.CraftingPieceTierFilter> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this.FilterType);
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x0005B6A9 File Offset: 0x000598A9
		// (set) Token: 0x060017BE RID: 6078 RVA: 0x0005B6B1 File Offset: 0x000598B1
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

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x0005B6CF File Offset: 0x000598CF
		// (set) Token: 0x060017C0 RID: 6080 RVA: 0x0005B6D7 File Offset: 0x000598D7
		[DataSourceProperty]
		public string TierName
		{
			get
			{
				return this._tierName;
			}
			set
			{
				if (value != this._tierName)
				{
					this._tierName = value;
					base.OnPropertyChangedWithValue<string>(value, "TierName");
				}
			}
		}

		// Token: 0x04000ACF RID: 2767
		private readonly Action<WeaponDesignVM.CraftingPieceTierFilter> _onSelect;

		// Token: 0x04000AD0 RID: 2768
		private bool _isSelected;

		// Token: 0x04000AD1 RID: 2769
		private string _tierName;
	}
}
