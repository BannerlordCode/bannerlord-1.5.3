using System;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000094 RID: 148
	public class InventoryTradeVM : ViewModel
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000C60 RID: 3168 RVA: 0x00032FD8 File Offset: 0x000311D8
		// (remove) Token: 0x06000C61 RID: 3169 RVA: 0x0003300C File Offset: 0x0003120C
		public static event Action RemoveZeroCounts;

		// Token: 0x06000C62 RID: 3170 RVA: 0x00033040 File Offset: 0x00031240
		public InventoryTradeVM(InventoryLogic inventoryLogic, ItemRosterElement itemRoster, InventoryLogic.InventorySide side, Action<int, bool> onApplyTransaction)
		{
			this._inventoryLogic = inventoryLogic;
			this._referenceItemRoster = itemRoster;
			this._isPlayerItem = side == InventoryLogic.InventorySide.PlayerInventory;
			this._onApplyTransaction = onApplyTransaction;
			this.PieceLbl = this._pieceLblSingular;
			InventoryLogic inventoryLogic2 = this._inventoryLogic;
			this.IsTrading = inventoryLogic2 != null && inventoryLogic2.IsTrading;
			this.TakeHint = new HintViewModel(GameTexts.FindText("str_take", null), null);
			this.GiveHint = new HintViewModel(GameTexts.FindText("str_give", null), null);
			this.UpdateItemData(itemRoster, side, true);
			this.RefreshValues();
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x000330E4 File Offset: 0x000312E4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ThisStockLbl = GameTexts.FindText("str_inventory_this_stock", null).ToString();
			this.OtherStockLbl = GameTexts.FindText("str_inventory_total_stock", null).ToString();
			this.AveragePriceLbl = GameTexts.FindText("str_inventory_average_price", null).ToString();
			this._pieceLblSingular = GameTexts.FindText("str_inventory_piece", null).ToString();
			this._pieceLblPlural = GameTexts.FindText("str_inventory_pieces", null).ToString();
			this.ApplyExchangeHint = new HintViewModel(GameTexts.FindText("str_party_apply_exchange", null), null);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0003317C File Offset: 0x0003137C
		public void UpdateItemData(ItemRosterElement itemRoster, InventoryLogic.InventorySide side, bool forceUpdate = true)
		{
			if (side != InventoryLogic.InventorySide.OtherInventory && side != InventoryLogic.InventorySide.PlayerInventory)
			{
				return;
			}
			ItemRosterElement? itemRosterElement = new ItemRosterElement?(itemRoster);
			ItemRosterElement? itemRosterElement2 = null;
			if (side == InventoryLogic.InventorySide.PlayerInventory)
			{
				itemRosterElement2 = this.FindItemFromSide(itemRoster.EquipmentElement, InventoryLogic.InventorySide.OtherInventory);
			}
			else if (side == InventoryLogic.InventorySide.OtherInventory)
			{
				itemRosterElement2 = this.FindItemFromSide(itemRoster.EquipmentElement, InventoryLogic.InventorySide.PlayerInventory);
			}
			if (forceUpdate)
			{
				this.InitialThisStock = ((itemRosterElement != null) ? itemRosterElement.GetValueOrDefault().Amount : 0);
				this.InitialOtherStock = ((itemRosterElement2 != null) ? itemRosterElement2.GetValueOrDefault().Amount : 0);
				this.TotalStock = this.InitialThisStock + this.InitialOtherStock;
				this.ThisStock = this.InitialThisStock;
				this.OtherStock = this.InitialOtherStock;
				this.ThisStockUpdated();
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0003323E File Offset: 0x0003143E
		private ItemRosterElement? FindItemFromSide(EquipmentElement item, InventoryLogic.InventorySide side)
		{
			return this._inventoryLogic.FindItemFromSide(side, item);
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00033250 File Offset: 0x00031450
		private void ThisStockUpdated()
		{
			this.ExecuteApplyTransaction();
			this.OtherStock = this.TotalStock - this.ThisStock;
			this.IsThisStockIncreasable = this.OtherStock > 0;
			this.IsOtherStockIncreasable = this.OtherStock < this.TotalStock;
			this.UpdateProperties();
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x000332A0 File Offset: 0x000314A0
		private void UpdateProperties()
		{
			int num = this.ThisStock - this.InitialThisStock;
			bool flag = num >= 0;
			int num2 = (flag ? num : (-num));
			if (num2 == 0)
			{
				this.PieceChange = num2.ToString();
				this.PriceChange = "0";
				this.AveragePrice = "0";
				this.IsExchangeAvailable = false;
			}
			else
			{
				int num3;
				int itemTotalPrice = this._inventoryLogic.GetItemTotalPrice(this._referenceItemRoster, num2, out num3, flag);
				this.PieceChange = (flag ? "+" : "-") + num2;
				this.PriceChange = (flag ? "-" : "+") + itemTotalPrice * num2;
				this.AveragePrice = this.GetAveragePrice(itemTotalPrice, num3, flag);
				this.IsExchangeAvailable = true;
			}
			this.PieceLbl = ((num2 <= 1) ? this._pieceLblSingular : this._pieceLblPlural);
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00033384 File Offset: 0x00031584
		public string GetAveragePrice(int totalPrice, int lastPrice, bool isBuying)
		{
			InventoryLogic.InventorySide inventorySide = (isBuying ? InventoryLogic.InventorySide.OtherInventory : InventoryLogic.InventorySide.PlayerInventory);
			int costOfItemRosterElement = this._inventoryLogic.GetCostOfItemRosterElement(this._referenceItemRoster, inventorySide);
			if (costOfItemRosterElement == lastPrice)
			{
				return costOfItemRosterElement.ToString();
			}
			if (costOfItemRosterElement < lastPrice)
			{
				return costOfItemRosterElement + " - " + lastPrice;
			}
			return lastPrice + " - " + costOfItemRosterElement;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x000333E9 File Offset: 0x000315E9
		public void ExecuteIncreaseThisStock()
		{
			if (this.ThisStock < this.TotalStock)
			{
				this.ThisStock++;
			}
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00033407 File Offset: 0x00031607
		public void ExecuteIncreaseOtherStock()
		{
			if (this.ThisStock > 0)
			{
				this.ThisStock--;
			}
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00033420 File Offset: 0x00031620
		public void ExecuteReset()
		{
			this.ThisStock = this.InitialThisStock;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00033430 File Offset: 0x00031630
		public void ExecuteApplyTransaction()
		{
			int num = this.ThisStock - this.InitialThisStock;
			if (num == 0 || this._onApplyTransaction == null)
			{
				return;
			}
			bool flag = num >= 0;
			int num2 = (flag ? num : (-num));
			bool flag2 = (this._isPlayerItem ? flag : (!flag));
			this._onApplyTransaction(num2, flag2);
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00033485 File Offset: 0x00031685
		public void ExecuteRemoveZeroCounts()
		{
			Action removeZeroCounts = InventoryTradeVM.RemoveZeroCounts;
			if (removeZeroCounts == null)
			{
				return;
			}
			removeZeroCounts();
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x00033496 File Offset: 0x00031696
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x0003349E File Offset: 0x0003169E
		[DataSourceProperty]
		public string ThisStockLbl
		{
			get
			{
				return this._thisStockLbl;
			}
			set
			{
				if (value != this._thisStockLbl)
				{
					this._thisStockLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ThisStockLbl");
				}
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x000334C1 File Offset: 0x000316C1
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x000334C9 File Offset: 0x000316C9
		[DataSourceProperty]
		public string OtherStockLbl
		{
			get
			{
				return this._otherStockLbl;
			}
			set
			{
				if (value != this._otherStockLbl)
				{
					this._otherStockLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "OtherStockLbl");
				}
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x000334EC File Offset: 0x000316EC
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x000334F4 File Offset: 0x000316F4
		[DataSourceProperty]
		public string PieceLbl
		{
			get
			{
				return this._pieceLbl;
			}
			set
			{
				if (value != this._pieceLbl)
				{
					this._pieceLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "PieceLbl");
				}
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x00033517 File Offset: 0x00031717
		// (set) Token: 0x06000C75 RID: 3189 RVA: 0x0003351F File Offset: 0x0003171F
		[DataSourceProperty]
		public string AveragePriceLbl
		{
			get
			{
				return this._averagePriceLbl;
			}
			set
			{
				if (value != this._averagePriceLbl)
				{
					this._averagePriceLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "AveragePriceLbl");
				}
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00033542 File Offset: 0x00031742
		// (set) Token: 0x06000C77 RID: 3191 RVA: 0x0003354A File Offset: 0x0003174A
		[DataSourceProperty]
		public HintViewModel ApplyExchangeHint
		{
			get
			{
				return this._applyExchangeHint;
			}
			set
			{
				if (value != this._applyExchangeHint)
				{
					this._applyExchangeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ApplyExchangeHint");
				}
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00033568 File Offset: 0x00031768
		// (set) Token: 0x06000C79 RID: 3193 RVA: 0x00033570 File Offset: 0x00031770
		[DataSourceProperty]
		public bool IsExchangeAvailable
		{
			get
			{
				return this._isExchangeAvailable;
			}
			set
			{
				if (value != this._isExchangeAvailable)
				{
					this._isExchangeAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsExchangeAvailable");
				}
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x0003358E File Offset: 0x0003178E
		// (set) Token: 0x06000C7B RID: 3195 RVA: 0x00033596 File Offset: 0x00031796
		[DataSourceProperty]
		public string PriceChange
		{
			get
			{
				return this._priceChange;
			}
			set
			{
				if (value != this._priceChange)
				{
					this._priceChange = value;
					base.OnPropertyChangedWithValue<string>(value, "PriceChange");
				}
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x000335B9 File Offset: 0x000317B9
		// (set) Token: 0x06000C7D RID: 3197 RVA: 0x000335C1 File Offset: 0x000317C1
		[DataSourceProperty]
		public string PieceChange
		{
			get
			{
				return this._pieceChange;
			}
			set
			{
				if (value != this._pieceChange)
				{
					this._pieceChange = value;
					base.OnPropertyChangedWithValue<string>(value, "PieceChange");
				}
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x000335E4 File Offset: 0x000317E4
		// (set) Token: 0x06000C7F RID: 3199 RVA: 0x000335EC File Offset: 0x000317EC
		[DataSourceProperty]
		public string AveragePrice
		{
			get
			{
				return this._averagePrice;
			}
			set
			{
				if (value != this._averagePrice)
				{
					this._averagePrice = value;
					base.OnPropertyChangedWithValue<string>(value, "AveragePrice");
				}
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0003360F File Offset: 0x0003180F
		// (set) Token: 0x06000C81 RID: 3201 RVA: 0x00033617 File Offset: 0x00031817
		[DataSourceProperty]
		public int ThisStock
		{
			get
			{
				return this._thisStock;
			}
			set
			{
				if (value != this._thisStock)
				{
					this._thisStock = value;
					base.OnPropertyChangedWithValue(value, "ThisStock");
					this.ThisStockUpdated();
				}
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0003363B File Offset: 0x0003183B
		// (set) Token: 0x06000C83 RID: 3203 RVA: 0x00033643 File Offset: 0x00031843
		[DataSourceProperty]
		public int InitialThisStock
		{
			get
			{
				return this._initialThisStock;
			}
			set
			{
				if (value != this._initialThisStock)
				{
					this._initialThisStock = value;
					base.OnPropertyChangedWithValue(value, "InitialThisStock");
				}
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00033661 File Offset: 0x00031861
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x00033669 File Offset: 0x00031869
		[DataSourceProperty]
		public int OtherStock
		{
			get
			{
				return this._otherStock;
			}
			set
			{
				if (value != this._otherStock)
				{
					this._otherStock = value;
					base.OnPropertyChangedWithValue(value, "OtherStock");
				}
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x00033687 File Offset: 0x00031887
		// (set) Token: 0x06000C87 RID: 3207 RVA: 0x0003368F File Offset: 0x0003188F
		[DataSourceProperty]
		public int InitialOtherStock
		{
			get
			{
				return this._initialOtherStock;
			}
			set
			{
				if (value != this._initialOtherStock)
				{
					this._initialOtherStock = value;
					base.OnPropertyChangedWithValue(value, "InitialOtherStock");
				}
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x000336AD File Offset: 0x000318AD
		// (set) Token: 0x06000C89 RID: 3209 RVA: 0x000336B5 File Offset: 0x000318B5
		[DataSourceProperty]
		public int TotalStock
		{
			get
			{
				return this._totalStock;
			}
			set
			{
				if (value != this._totalStock)
				{
					this._totalStock = value;
					base.OnPropertyChangedWithValue(value, "TotalStock");
				}
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x000336D3 File Offset: 0x000318D3
		// (set) Token: 0x06000C8B RID: 3211 RVA: 0x000336DB File Offset: 0x000318DB
		[DataSourceProperty]
		public bool IsThisStockIncreasable
		{
			get
			{
				return this._isThisStockIncreasable;
			}
			set
			{
				if (value != this._isThisStockIncreasable)
				{
					this._isThisStockIncreasable = value;
					base.OnPropertyChangedWithValue(value, "IsThisStockIncreasable");
				}
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x000336F9 File Offset: 0x000318F9
		// (set) Token: 0x06000C8D RID: 3213 RVA: 0x00033701 File Offset: 0x00031901
		[DataSourceProperty]
		public bool IsOtherStockIncreasable
		{
			get
			{
				return this._isOtherStockIncreasable;
			}
			set
			{
				if (value != this._isOtherStockIncreasable)
				{
					this._isOtherStockIncreasable = value;
					base.OnPropertyChangedWithValue(value, "IsOtherStockIncreasable");
				}
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0003371F File Offset: 0x0003191F
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x00033727 File Offset: 0x00031927
		[DataSourceProperty]
		public bool IsTrading
		{
			get
			{
				return this._isTrading;
			}
			set
			{
				if (value != this._isTrading)
				{
					this._isTrading = value;
					base.OnPropertyChangedWithValue(value, "IsTrading");
				}
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00033745 File Offset: 0x00031945
		// (set) Token: 0x06000C91 RID: 3217 RVA: 0x0003374D File Offset: 0x0003194D
		[DataSourceProperty]
		public bool IsTradeable
		{
			get
			{
				return this._isTradeable;
			}
			set
			{
				if (value != this._isTradeable)
				{
					this._isTradeable = value;
					base.OnPropertyChangedWithValue(value, "IsTradeable");
				}
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x0003376B File Offset: 0x0003196B
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x00033773 File Offset: 0x00031973
		[DataSourceProperty]
		public HintViewModel TakeHint
		{
			get
			{
				return this._takeHint;
			}
			set
			{
				if (value != this._takeHint)
				{
					this._takeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "TakeHint");
				}
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00033791 File Offset: 0x00031991
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x00033799 File Offset: 0x00031999
		[DataSourceProperty]
		public HintViewModel GiveHint
		{
			get
			{
				return this._giveHint;
			}
			set
			{
				if (value != this._giveHint)
				{
					this._giveHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GiveHint");
				}
			}
		}

		// Token: 0x0400057F RID: 1407
		private InventoryLogic _inventoryLogic;

		// Token: 0x04000580 RID: 1408
		private ItemRosterElement _referenceItemRoster;

		// Token: 0x04000581 RID: 1409
		private Action<int, bool> _onApplyTransaction;

		// Token: 0x04000582 RID: 1410
		private string _pieceLblSingular;

		// Token: 0x04000583 RID: 1411
		private string _pieceLblPlural;

		// Token: 0x04000584 RID: 1412
		private bool _isPlayerItem;

		// Token: 0x04000585 RID: 1413
		private string _thisStockLbl;

		// Token: 0x04000586 RID: 1414
		private string _otherStockLbl;

		// Token: 0x04000587 RID: 1415
		private string _averagePriceLbl;

		// Token: 0x04000588 RID: 1416
		private string _pieceLbl;

		// Token: 0x04000589 RID: 1417
		private HintViewModel _applyExchangeHint;

		// Token: 0x0400058A RID: 1418
		private bool _isExchangeAvailable;

		// Token: 0x0400058B RID: 1419
		private string _averagePrice;

		// Token: 0x0400058C RID: 1420
		private string _pieceChange;

		// Token: 0x0400058D RID: 1421
		private string _priceChange;

		// Token: 0x0400058E RID: 1422
		private int _thisStock = -1;

		// Token: 0x0400058F RID: 1423
		private int _initialThisStock;

		// Token: 0x04000590 RID: 1424
		private int _otherStock = -1;

		// Token: 0x04000591 RID: 1425
		private int _initialOtherStock;

		// Token: 0x04000592 RID: 1426
		private int _totalStock;

		// Token: 0x04000593 RID: 1427
		private bool _isThisStockIncreasable;

		// Token: 0x04000594 RID: 1428
		private bool _isOtherStockIncreasable;

		// Token: 0x04000595 RID: 1429
		private bool _isTrading;

		// Token: 0x04000596 RID: 1430
		private bool _isTradeable;

		// Token: 0x04000597 RID: 1431
		private HintViewModel _takeHint;

		// Token: 0x04000598 RID: 1432
		private HintViewModel _giveHint;
	}
}
