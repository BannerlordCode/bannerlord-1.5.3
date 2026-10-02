using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000107 RID: 263
	public class CraftingPieceListVM : ViewModel
	{
		// Token: 0x06001793 RID: 6035 RVA: 0x0005B1B7 File Offset: 0x000593B7
		public CraftingPieceListVM(MBBindingList<CraftingPieceVM> pieceList, CraftingPiece.PieceTypes pieceType, Action<CraftingPiece.PieceTypes, bool> onSelect)
		{
			this.Pieces = pieceList;
			this.PieceType = pieceType;
			this._onSelect = onSelect;
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0005B1D4 File Offset: 0x000593D4
		public void ExecuteSelect()
		{
			Action<CraftingPiece.PieceTypes, bool> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this.PieceType, true);
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0005B1ED File Offset: 0x000593ED
		public void Refresh()
		{
			this.HasNewlyUnlockedPieces = this.Pieces.Any<CraftingPieceVM>((CraftingPieceVM x) => x.IsNewlyUnlocked);
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x0005B21F File Offset: 0x0005941F
		// (set) Token: 0x06001797 RID: 6039 RVA: 0x0005B227 File Offset: 0x00059427
		[DataSourceProperty]
		public bool HasNewlyUnlockedPieces
		{
			get
			{
				return this._hasNewlyUnlockedPieces;
			}
			set
			{
				if (value != this._hasNewlyUnlockedPieces)
				{
					this._hasNewlyUnlockedPieces = value;
					base.OnPropertyChangedWithValue(value, "HasNewlyUnlockedPieces");
				}
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06001798 RID: 6040 RVA: 0x0005B245 File Offset: 0x00059445
		// (set) Token: 0x06001799 RID: 6041 RVA: 0x0005B24D File Offset: 0x0005944D
		[DataSourceProperty]
		public MBBindingList<CraftingPieceVM> Pieces
		{
			get
			{
				return this._pieces;
			}
			set
			{
				if (value != this._pieces)
				{
					this._pieces = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingPieceVM>>(value, "Pieces");
				}
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x0005B26B File Offset: 0x0005946B
		// (set) Token: 0x0600179B RID: 6043 RVA: 0x0005B273 File Offset: 0x00059473
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

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x0005B291 File Offset: 0x00059491
		// (set) Token: 0x0600179D RID: 6045 RVA: 0x0005B299 File Offset: 0x00059499
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

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x0600179E RID: 6046 RVA: 0x0005B2B7 File Offset: 0x000594B7
		// (set) Token: 0x0600179F RID: 6047 RVA: 0x0005B2BF File Offset: 0x000594BF
		[DataSourceProperty]
		public CraftingPieceVM SelectedPiece
		{
			get
			{
				return this._selectedPiece;
			}
			set
			{
				if (value != this._selectedPiece)
				{
					this._selectedPiece = value;
					base.OnPropertyChangedWithValue<CraftingPieceVM>(value, "SelectedPiece");
				}
			}
		}

		// Token: 0x04000AB9 RID: 2745
		public CraftingPiece.PieceTypes PieceType;

		// Token: 0x04000ABA RID: 2746
		private Action<CraftingPiece.PieceTypes, bool> _onSelect;

		// Token: 0x04000ABB RID: 2747
		private bool _hasNewlyUnlockedPieces;

		// Token: 0x04000ABC RID: 2748
		private MBBindingList<CraftingPieceVM> _pieces;

		// Token: 0x04000ABD RID: 2749
		private bool _isSelected;

		// Token: 0x04000ABE RID: 2750
		private bool _isEnabled;

		// Token: 0x04000ABF RID: 2751
		private CraftingPieceVM _selectedPiece;
	}
}
