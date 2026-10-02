using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Barter
{
	// Token: 0x0200015F RID: 351
	public class BarterItemVM : EncyclopediaLinkVM
	{
		// Token: 0x060021A2 RID: 8610 RVA: 0x000783E4 File Offset: 0x000765E4
		public BarterItemVM(Barterable barterable, BarterItemVM.BarterTransferEventDelegate OnTransfer, Action onAmountChange, bool isFixed = false)
		{
			this.Barterable = barterable;
			base.ActiveLink = barterable.GetEncyclopediaLink();
			this._onTransfer = OnTransfer;
			this._onAmountChange = onAmountChange;
			this._isFixed = isFixed;
			this.IsItemTransferrable = !isFixed;
			this.BarterableType = this.Barterable.StringID;
			ImageIdentifier visualIdentifier = this.Barterable.GetVisualIdentifier();
			this.HasVisualIdentifier = visualIdentifier != null;
			if (visualIdentifier != null)
			{
				this.VisualIdentifier = new GenericImageIdentifierVM(visualIdentifier);
			}
			else
			{
				this.VisualIdentifier = null;
				FiefBarterable fiefBarterable;
				if ((fiefBarterable = this.Barterable as FiefBarterable) != null)
				{
					this.FiefFileName = fiefBarterable.TargetSettlement.SettlementComponent.BackgroundMeshName;
				}
			}
			this.TotalItemCount = this.Barterable.MaxAmount;
			this.CurrentOfferedAmount = 1;
			this.IsMultiple = this.TotalItemCount > 1;
			this.IsOffered = this.Barterable.IsOffered;
			this.RefreshValues();
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000784EA File Offset: 0x000766EA
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ItemLbl = this.Barterable.Name.ToString();
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x00078508 File Offset: 0x00076708
		public void RefreshCompabilityWithItem(BarterItemVM item, bool isItemGotOffered)
		{
			if (isItemGotOffered && !item.Barterable.IsCompatible(this.Barterable))
			{
				this._incompatibleItems.Add(item.Barterable);
			}
			else if (!isItemGotOffered && this._incompatibleItems.Contains(item.Barterable))
			{
				this._incompatibleItems.Remove(item.Barterable);
			}
			this.IsItemTransferrable = this._incompatibleItems.Count <= 0;
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x00078580 File Offset: 0x00076780
		public void ExecuteAddOffered()
		{
			int num = (BarterItemVM.IsEntireStackModifierActive ? this.TotalItemCount : (this.CurrentOfferedAmount + (BarterItemVM.IsFiveStackModifierActive ? 5 : 1)));
			this.CurrentOfferedAmount = ((num < this.TotalItemCount) ? num : this.TotalItemCount);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000785C8 File Offset: 0x000767C8
		public void ExecuteRemoveOffered()
		{
			int num = (BarterItemVM.IsEntireStackModifierActive ? 1 : (this.CurrentOfferedAmount - (BarterItemVM.IsFiveStackModifierActive ? 5 : 1)));
			this.CurrentOfferedAmount = ((num > 1) ? num : 1);
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x00078600 File Offset: 0x00076800
		public void ExecuteAction()
		{
			if (this.IsItemTransferrable)
			{
				this._onTransfer(this, false);
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x060021A8 RID: 8616 RVA: 0x00078617 File Offset: 0x00076817
		// (set) Token: 0x060021A9 RID: 8617 RVA: 0x0007861F File Offset: 0x0007681F
		[DataSourceProperty]
		public int TotalItemCount
		{
			get
			{
				return this._totalItemCount;
			}
			set
			{
				if (this._totalItemCount != value)
				{
					this._totalItemCount = value;
					base.OnPropertyChangedWithValue(value, "TotalItemCount");
					this.TotalItemCountText = CampaignUIHelper.GetAbbreviatedValueTextFromValue(value);
				}
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x060021AA RID: 8618 RVA: 0x00078649 File Offset: 0x00076849
		// (set) Token: 0x060021AB RID: 8619 RVA: 0x00078651 File Offset: 0x00076851
		[DataSourceProperty]
		public string TotalItemCountText
		{
			get
			{
				return this._totalItemCountText;
			}
			set
			{
				if (this._totalItemCountText != value)
				{
					this._totalItemCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalItemCountText");
				}
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x060021AC RID: 8620 RVA: 0x00078674 File Offset: 0x00076874
		// (set) Token: 0x060021AD RID: 8621 RVA: 0x0007867C File Offset: 0x0007687C
		[DataSourceProperty]
		public int CurrentOfferedAmount
		{
			get
			{
				return this._currentOfferedAmount;
			}
			set
			{
				if (this._currentOfferedAmount != value)
				{
					this.Barterable.CurrentAmount = value;
					Action onAmountChange = this._onAmountChange;
					if (onAmountChange != null)
					{
						onAmountChange();
					}
					this._currentOfferedAmount = value;
					base.OnPropertyChangedWithValue(value, "CurrentOfferedAmount");
					this.CurrentOfferedAmountText = CampaignUIHelper.GetAbbreviatedValueTextFromValue(value);
				}
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x000786CE File Offset: 0x000768CE
		// (set) Token: 0x060021AF RID: 8623 RVA: 0x000786D6 File Offset: 0x000768D6
		[DataSourceProperty]
		public string CurrentOfferedAmountText
		{
			get
			{
				return this._currentOfferedAmountText;
			}
			set
			{
				if (this._currentOfferedAmountText != value)
				{
					this._currentOfferedAmountText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentOfferedAmountText");
				}
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x060021B0 RID: 8624 RVA: 0x000786F9 File Offset: 0x000768F9
		// (set) Token: 0x060021B1 RID: 8625 RVA: 0x00078701 File Offset: 0x00076901
		[DataSourceProperty]
		public string BarterableType
		{
			get
			{
				return this._barterableType;
			}
			set
			{
				if (this._barterableType != value)
				{
					this._barterableType = value;
					base.OnPropertyChangedWithValue<string>(value, "BarterableType");
				}
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x060021B2 RID: 8626 RVA: 0x00078724 File Offset: 0x00076924
		// (set) Token: 0x060021B3 RID: 8627 RVA: 0x0007872C File Offset: 0x0007692C
		[DataSourceProperty]
		public bool HasVisualIdentifier
		{
			get
			{
				return this._hasVisualIdentifier;
			}
			set
			{
				if (this._hasVisualIdentifier != value)
				{
					this._hasVisualIdentifier = value;
					base.OnPropertyChangedWithValue(value, "HasVisualIdentifier");
				}
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x0007874A File Offset: 0x0007694A
		// (set) Token: 0x060021B5 RID: 8629 RVA: 0x00078752 File Offset: 0x00076952
		[DataSourceProperty]
		public bool IsMultiple
		{
			get
			{
				return this._isMultiple;
			}
			set
			{
				if (this._isMultiple != value)
				{
					this._isMultiple = value;
					base.OnPropertyChangedWithValue(value, "IsMultiple");
				}
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x060021B6 RID: 8630 RVA: 0x00078770 File Offset: 0x00076970
		// (set) Token: 0x060021B7 RID: 8631 RVA: 0x00078778 File Offset: 0x00076978
		[DataSourceProperty]
		public bool IsSelectorActive
		{
			get
			{
				return this._isSelectorActive;
			}
			set
			{
				if (this._isSelectorActive != value)
				{
					this._isSelectorActive = value;
					base.OnPropertyChangedWithValue(value, "IsSelectorActive");
				}
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x060021B8 RID: 8632 RVA: 0x00078796 File Offset: 0x00076996
		// (set) Token: 0x060021B9 RID: 8633 RVA: 0x0007879E File Offset: 0x0007699E
		[DataSourceProperty]
		public ImageIdentifierVM VisualIdentifier
		{
			get
			{
				return this._visualIdentifier;
			}
			set
			{
				if (this._visualIdentifier != value)
				{
					this._visualIdentifier = value;
					base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "VisualIdentifier");
				}
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060021BA RID: 8634 RVA: 0x000787BC File Offset: 0x000769BC
		// (set) Token: 0x060021BB RID: 8635 RVA: 0x000787C4 File Offset: 0x000769C4
		[DataSourceProperty]
		public string ItemLbl
		{
			get
			{
				return this._itemLbl;
			}
			set
			{
				this._itemLbl = value;
				base.OnPropertyChangedWithValue<string>(value, "ItemLbl");
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x060021BC RID: 8636 RVA: 0x000787D9 File Offset: 0x000769D9
		// (set) Token: 0x060021BD RID: 8637 RVA: 0x000787E1 File Offset: 0x000769E1
		[DataSourceProperty]
		public string FiefFileName
		{
			get
			{
				return this._fiefFileName;
			}
			set
			{
				this._fiefFileName = value;
				base.OnPropertyChangedWithValue<string>(value, "FiefFileName");
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x060021BE RID: 8638 RVA: 0x000787F6 File Offset: 0x000769F6
		// (set) Token: 0x060021BF RID: 8639 RVA: 0x000787FE File Offset: 0x000769FE
		[DataSourceProperty]
		public bool IsItemTransferrable
		{
			get
			{
				return this._isItemTransferrable;
			}
			set
			{
				if (this._isFixed)
				{
					value = false;
				}
				if (this._isItemTransferrable != value)
				{
					this._isItemTransferrable = value;
					base.OnPropertyChangedWithValue(value, "IsItemTransferrable");
				}
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x060021C0 RID: 8640 RVA: 0x00078827 File Offset: 0x00076A27
		// (set) Token: 0x060021C1 RID: 8641 RVA: 0x0007882F File Offset: 0x00076A2F
		[DataSourceProperty]
		public bool IsOffered
		{
			get
			{
				return this._isOffered;
			}
			set
			{
				if (value != this._isOffered)
				{
					this._isOffered = value;
					base.OnPropertyChangedWithValue(value, "IsOffered");
				}
			}
		}

		// Token: 0x04000F5B RID: 3931
		public static bool IsEntireStackModifierActive;

		// Token: 0x04000F5C RID: 3932
		public static bool IsFiveStackModifierActive;

		// Token: 0x04000F5D RID: 3933
		private readonly BarterItemVM.BarterTransferEventDelegate _onTransfer;

		// Token: 0x04000F5E RID: 3934
		private readonly Action _onAmountChange;

		// Token: 0x04000F5F RID: 3935
		private bool _isFixed;

		// Token: 0x04000F60 RID: 3936
		private List<Barterable> _incompatibleItems = new List<Barterable>();

		// Token: 0x04000F61 RID: 3937
		public Barterable Barterable;

		// Token: 0x04000F62 RID: 3938
		public bool _isOffered;

		// Token: 0x04000F63 RID: 3939
		private bool _isItemTransferrable = true;

		// Token: 0x04000F64 RID: 3940
		private string _itemLbl;

		// Token: 0x04000F65 RID: 3941
		private string _fiefFileName;

		// Token: 0x04000F66 RID: 3942
		private string _barterableType = "NULL";

		// Token: 0x04000F67 RID: 3943
		private string _currentOfferedAmountText;

		// Token: 0x04000F68 RID: 3944
		private ImageIdentifierVM _visualIdentifier;

		// Token: 0x04000F69 RID: 3945
		private bool _isSelectorActive;

		// Token: 0x04000F6A RID: 3946
		private bool _hasVisualIdentifier;

		// Token: 0x04000F6B RID: 3947
		private bool _isMultiple;

		// Token: 0x04000F6C RID: 3948
		private int _totalItemCount;

		// Token: 0x04000F6D RID: 3949
		private string _totalItemCountText;

		// Token: 0x04000F6E RID: 3950
		private int _currentOfferedAmount;

		// Token: 0x020002EF RID: 751
		// (Invoke) Token: 0x0600286C RID: 10348
		public delegate void BarterTransferEventDelegate(BarterItemVM itemVM, bool transferAll);
	}
}
