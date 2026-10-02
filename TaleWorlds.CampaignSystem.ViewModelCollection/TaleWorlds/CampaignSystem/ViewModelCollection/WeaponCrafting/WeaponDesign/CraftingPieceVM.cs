using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000108 RID: 264
	public class CraftingPieceVM : ViewModel
	{
		// Token: 0x060017A0 RID: 6048 RVA: 0x0005B2DD File Offset: 0x000594DD
		public CraftingPieceVM()
		{
			this.ImageIdentifier = new CraftingPieceImageIdentifierVM(null, string.Empty);
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x0005B300 File Offset: 0x00059500
		public CraftingPieceVM(Action<CraftingPieceVM> selectWeaponPart, Action<CraftingPiece> inspectCraftPiece, string templateId, WeaponDesignElement usableCraftingPiece, int pieceType, int index, bool isOpened)
		{
			this._selectWeaponPiece = selectWeaponPart;
			this._inspectCraftPiece = inspectCraftPiece;
			this.CraftingPiece = usableCraftingPiece;
			this.Tier = usableCraftingPiece.CraftingPiece.PieceTier;
			this.TierText = Common.ToRoman(this.Tier);
			this.ImageIdentifier = new CraftingPieceImageIdentifierVM(usableCraftingPiece.CraftingPiece, templateId);
			this.PieceType = pieceType;
			this.Index = index;
			this.PlayerHasPiece = isOpened;
			this.ItemAttributeIcons = new MBBindingList<CraftingItemFlagVM>();
			this.IsEmpty = string.IsNullOrEmpty(this.CraftingPiece.CraftingPiece.MeshName);
			this.RefreshFlagIcons();
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x0005B3AC File Offset: 0x000595AC
		public void RefreshFlagIcons()
		{
			this.ItemAttributeIcons.Clear();
			foreach (Tuple<string, TextObject> tuple in CampaignUIHelper.GetItemFlagDetails(this.CraftingPiece.CraftingPiece.AdditionalItemFlags))
			{
				this.ItemAttributeIcons.Add(new CraftingItemFlagVM(tuple.Item1, tuple.Item2, true));
			}
			foreach (ValueTuple<string, TextObject> valueTuple in CampaignUIHelper.GetWeaponFlagDetails(this.CraftingPiece.CraftingPiece.AdditionalWeaponFlags, null))
			{
				this.ItemAttributeIcons.Add(new CraftingItemFlagVM(valueTuple.Item1, valueTuple.Item2, true));
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x0005B498 File Offset: 0x00059698
		public void ExecuteOpenTooltip(bool isMouseMoving)
		{
			InformationManager.ShowTooltip(typeof(WeaponDesignElement), new object[] { this.CraftingPiece });
			if (isMouseMoving)
			{
				Action<CraftingPiece> inspectCraftPiece = this._inspectCraftPiece;
				if (inspectCraftPiece == null)
				{
					return;
				}
				inspectCraftPiece(this.CraftingPiece.CraftingPiece);
			}
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x0005B4D6 File Offset: 0x000596D6
		public void ExecuteCloseTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x0005B4DD File Offset: 0x000596DD
		public void ExecuteSelect()
		{
			this._selectWeaponPiece(this);
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060017A6 RID: 6054 RVA: 0x0005B4EB File Offset: 0x000596EB
		// (set) Token: 0x060017A7 RID: 6055 RVA: 0x0005B4F3 File Offset: 0x000596F3
		[DataSourceProperty]
		public bool IsFilteredOut
		{
			get
			{
				return this._isFilteredOut;
			}
			set
			{
				if (value != this._isFilteredOut)
				{
					this._isFilteredOut = value;
					base.OnPropertyChangedWithValue(value, "IsFilteredOut");
				}
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x0005B511 File Offset: 0x00059711
		// (set) Token: 0x060017A9 RID: 6057 RVA: 0x0005B519 File Offset: 0x00059719
		[DataSourceProperty]
		public MBBindingList<CraftingItemFlagVM> ItemAttributeIcons
		{
			get
			{
				return this._itemAttributeIcons;
			}
			set
			{
				if (value != this._itemAttributeIcons)
				{
					this._itemAttributeIcons = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingItemFlagVM>>(value, "ItemAttributeIcons");
				}
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x0005B537 File Offset: 0x00059737
		// (set) Token: 0x060017AB RID: 6059 RVA: 0x0005B53F File Offset: 0x0005973F
		[DataSourceProperty]
		public bool PlayerHasPiece
		{
			get
			{
				return this._playerHasPiece;
			}
			set
			{
				if (this._playerHasPiece != value)
				{
					this._playerHasPiece = value;
					base.OnPropertyChangedWithValue(value, "PlayerHasPiece");
				}
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x060017AC RID: 6060 RVA: 0x0005B55D File Offset: 0x0005975D
		// (set) Token: 0x060017AD RID: 6061 RVA: 0x0005B565 File Offset: 0x00059765
		[DataSourceProperty]
		public bool IsEmpty
		{
			get
			{
				return this._isEmpty;
			}
			set
			{
				if (this._isEmpty != value)
				{
					this._isEmpty = value;
					base.OnPropertyChangedWithValue(value, "IsEmpty");
				}
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x0005B583 File Offset: 0x00059783
		// (set) Token: 0x060017AF RID: 6063 RVA: 0x0005B58B File Offset: 0x0005978B
		[DataSourceProperty]
		public string TierText
		{
			get
			{
				return this._tierText;
			}
			set
			{
				if (this._tierText != value)
				{
					this._tierText = value;
					base.OnPropertyChangedWithValue<string>(value, "TierText");
				}
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x0005B5AE File Offset: 0x000597AE
		// (set) Token: 0x060017B1 RID: 6065 RVA: 0x0005B5B6 File Offset: 0x000597B6
		[DataSourceProperty]
		public int Tier
		{
			get
			{
				return this._tier;
			}
			set
			{
				if (this._tier != value)
				{
					this._tier = value;
					base.OnPropertyChangedWithValue(value, "Tier");
				}
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x060017B2 RID: 6066 RVA: 0x0005B5D4 File Offset: 0x000597D4
		// (set) Token: 0x060017B3 RID: 6067 RVA: 0x0005B5DC File Offset: 0x000597DC
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected != value)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x060017B4 RID: 6068 RVA: 0x0005B5FA File Offset: 0x000597FA
		// (set) Token: 0x060017B5 RID: 6069 RVA: 0x0005B602 File Offset: 0x00059802
		[DataSourceProperty]
		public CraftingPieceImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (this._imageIdentifier != value)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<CraftingPieceImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x060017B6 RID: 6070 RVA: 0x0005B620 File Offset: 0x00059820
		// (set) Token: 0x060017B7 RID: 6071 RVA: 0x0005B628 File Offset: 0x00059828
		[DataSourceProperty]
		public int PieceType
		{
			get
			{
				return this._pieceType;
			}
			set
			{
				if (this._pieceType != value)
				{
					this._pieceType = value;
					base.OnPropertyChangedWithValue(value, "PieceType");
				}
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x060017B8 RID: 6072 RVA: 0x0005B646 File Offset: 0x00059846
		// (set) Token: 0x060017B9 RID: 6073 RVA: 0x0005B64E File Offset: 0x0005984E
		[DataSourceProperty]
		public bool IsNewlyUnlocked
		{
			get
			{
				return this._isNewlyUnlocked;
			}
			set
			{
				if (value != this._isNewlyUnlocked)
				{
					this._isNewlyUnlocked = value;
					base.OnPropertyChangedWithValue(value, "IsNewlyUnlocked");
				}
			}
		}

		// Token: 0x04000AC0 RID: 2752
		public WeaponDesignElement CraftingPiece;

		// Token: 0x04000AC1 RID: 2753
		public int Index;

		// Token: 0x04000AC2 RID: 2754
		private readonly Action<CraftingPieceVM> _selectWeaponPiece;

		// Token: 0x04000AC3 RID: 2755
		private readonly Action<CraftingPiece> _inspectCraftPiece;

		// Token: 0x04000AC4 RID: 2756
		private bool _isFilteredOut;

		// Token: 0x04000AC5 RID: 2757
		public CraftingPieceImageIdentifierVM _imageIdentifier;

		// Token: 0x04000AC6 RID: 2758
		public int _pieceType = -1;

		// Token: 0x04000AC7 RID: 2759
		public int _tier;

		// Token: 0x04000AC8 RID: 2760
		public bool _isSelected;

		// Token: 0x04000AC9 RID: 2761
		public bool _playerHasPiece;

		// Token: 0x04000ACA RID: 2762
		private bool _isEmpty;

		// Token: 0x04000ACB RID: 2763
		public string _tierText;

		// Token: 0x04000ACC RID: 2764
		private MBBindingList<CraftingItemFlagVM> _itemAttributeIcons;

		// Token: 0x04000ACD RID: 2765
		private bool _isNewlyUnlocked;
	}
}
