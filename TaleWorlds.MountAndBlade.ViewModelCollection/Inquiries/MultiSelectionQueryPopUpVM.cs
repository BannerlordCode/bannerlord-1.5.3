using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Inquiries
{
	// Token: 0x02000046 RID: 70
	public class MultiSelectionQueryPopUpVM : PopUpBaseVM
	{
		// Token: 0x060005E7 RID: 1511 RVA: 0x00015D18 File Offset: 0x00013F18
		public MultiSelectionQueryPopUpVM(Action closeQuery)
			: base(closeQuery)
		{
			this.InquiryElements = new MBBindingList<InquiryElementVM>();
			this.MaxSelectableOptionCount = 0;
			this.MinSelectableOptionCount = 0;
			this._selectedOptionCount = 0;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00015D44 File Offset: 0x00013F44
		public void SetData(MultiSelectionInquiryData data)
		{
			this._data = data;
			this.InquiryElements.Clear();
			foreach (InquiryElement inquiryElement in this._data.InquiryElements)
			{
				TextObject textObject = (string.IsNullOrEmpty(inquiryElement.Hint) ? TextObject.GetEmpty() : new TextObject("{=!}" + inquiryElement.Hint, null));
				InquiryElementVM inquiryElementVM = new InquiryElementVM(inquiryElement, textObject, new Action<InquiryElementVM, bool>(this.OnInquiryElementSelected));
				this.InquiryElements.Add(inquiryElementVM);
			}
			base.TitleText = this._data.TitleText;
			base.PopUpLabel = this._data.DescriptionText;
			this.MaxSelectableOptionCount = this._data.MaxSelectableOptionCount;
			this.MinSelectableOptionCount = this._data.MinSelectableOptionCount;
			base.ButtonOkLabel = this._data.AffirmativeText;
			base.ButtonCancelLabel = this._data.NegativeText;
			base.IsButtonOkShown = true;
			base.IsButtonCancelShown = this._data.IsExitShown;
			this.IsSearchAvailable = this._data.IsSeachAvailable;
			this.SearchPlaceholderText = new TextObject("{=tQOPRBFg}Search...", null).ToString();
			this.RefreshIsButtonOkEnabled();
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00015EA0 File Offset: 0x000140A0
		private void OnInquiryElementSelected(InquiryElementVM elementVM, bool isSelected)
		{
			if (isSelected)
			{
				this._selectedOptionCount++;
				if (this.MaxSelectableOptionCount != 1)
				{
					goto IL_005C;
				}
				using (IEnumerator<InquiryElementVM> enumerator = this.InquiryElements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InquiryElementVM inquiryElementVM = enumerator.Current;
						if (inquiryElementVM != elementVM)
						{
							inquiryElementVM.IsSelected = false;
						}
					}
					goto IL_005C;
				}
			}
			this._selectedOptionCount--;
			IL_005C:
			this.RefreshIsButtonOkEnabled();
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00015F20 File Offset: 0x00014120
		public override void ExecuteAffirmativeAction()
		{
			if (this._data.AffirmativeAction != null)
			{
				List<InquiryElement> list = new List<InquiryElement>();
				foreach (InquiryElementVM inquiryElementVM in this.InquiryElements)
				{
					if (inquiryElementVM.IsSelected)
					{
						list.Add(inquiryElementVM.InquiryElement);
					}
				}
				this._data.AffirmativeAction(list);
			}
			base.CloseQuery();
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00015FA4 File Offset: 0x000141A4
		public override void ExecuteNegativeAction()
		{
			Action<List<InquiryElement>> negativeAction = this._data.NegativeAction;
			if (negativeAction != null)
			{
				negativeAction(new List<InquiryElement>());
			}
			base.CloseQuery();
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00015FC7 File Offset: 0x000141C7
		public override void OnClearData()
		{
			base.OnClearData();
			this._data = null;
			this.MaxSelectableOptionCount = 0;
			this.MinSelectableOptionCount = 0;
			this._selectedOptionCount = 0;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00015FEB File Offset: 0x000141EB
		private void RefreshIsButtonOkEnabled()
		{
			base.IsButtonOkEnabled = (this.MaxSelectableOptionCount <= 0 || this._selectedOptionCount <= this.MaxSelectableOptionCount) && this._selectedOptionCount >= this.MinSelectableOptionCount;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00016020 File Offset: 0x00014220
		private void UpdateInquiryFilter(string searchText, bool isAppending)
		{
			string text = searchText.ToLower();
			for (int i = 0; i < this.InquiryElements.Count; i++)
			{
				InquiryElementVM inquiryElementVM = this.InquiryElements[i];
				if (!isAppending || !inquiryElementVM.IsFilteredOut)
				{
					inquiryElementVM.IsFilteredOut = !inquiryElementVM.Text.ToLower().Contains(text);
				}
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001607C File Offset: 0x0001427C
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x00016084 File Offset: 0x00014284
		[DataSourceProperty]
		public MBBindingList<InquiryElementVM> InquiryElements
		{
			get
			{
				return this._inquiryElements;
			}
			set
			{
				if (value != this._inquiryElements)
				{
					this._inquiryElements = value;
					base.OnPropertyChangedWithValue<MBBindingList<InquiryElementVM>>(value, "InquiryElements");
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x000160A2 File Offset: 0x000142A2
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x000160AA File Offset: 0x000142AA
		[DataSourceProperty]
		public int MaxSelectableOptionCount
		{
			get
			{
				return this._maxSelectableOptionCount;
			}
			set
			{
				if (value != this._maxSelectableOptionCount)
				{
					this._maxSelectableOptionCount = value;
					base.OnPropertyChangedWithValue(value, "MaxSelectableOptionCount");
				}
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x000160C8 File Offset: 0x000142C8
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x000160D0 File Offset: 0x000142D0
		[DataSourceProperty]
		public int MinSelectableOptionCount
		{
			get
			{
				return this._minSelectableOptionCount;
			}
			set
			{
				if (value != this._minSelectableOptionCount)
				{
					this._minSelectableOptionCount = value;
					base.OnPropertyChangedWithValue(value, "MinSelectableOptionCount");
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x000160EE File Offset: 0x000142EE
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x000160F6 File Offset: 0x000142F6
		[DataSourceProperty]
		public bool IsSearchAvailable
		{
			get
			{
				return this._isSearchAvailable;
			}
			set
			{
				if (value != this._isSearchAvailable)
				{
					this._isSearchAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsSearchAvailable");
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00016114 File Offset: 0x00014314
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x0001611C File Offset: 0x0001431C
		[DataSourceProperty]
		public string SearchText
		{
			get
			{
				return this._searchText;
			}
			set
			{
				if (value != this._searchText)
				{
					bool flag = value.IndexOf(this._searchText ?? "") >= 0;
					this._searchText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchText");
					this.UpdateInquiryFilter(this._searchText, flag);
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x00016173 File Offset: 0x00014373
		// (set) Token: 0x060005FA RID: 1530 RVA: 0x0001617B File Offset: 0x0001437B
		[DataSourceProperty]
		public string SearchPlaceholderText
		{
			get
			{
				return this._searchPlaceholderText;
			}
			set
			{
				if (value != this._searchPlaceholderText)
				{
					this._searchPlaceholderText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchPlaceholderText");
				}
			}
		}

		// Token: 0x040002A7 RID: 679
		private MultiSelectionInquiryData _data;

		// Token: 0x040002A8 RID: 680
		private int _selectedOptionCount;

		// Token: 0x040002A9 RID: 681
		private MBBindingList<InquiryElementVM> _inquiryElements;

		// Token: 0x040002AA RID: 682
		private int _maxSelectableOptionCount;

		// Token: 0x040002AB RID: 683
		private int _minSelectableOptionCount;

		// Token: 0x040002AC RID: 684
		private bool _isSearchAvailable;

		// Token: 0x040002AD RID: 685
		private string _searchText;

		// Token: 0x040002AE RID: 686
		private string _searchPlaceholderText;
	}
}
