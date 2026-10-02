using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x0200012B RID: 299
	public class ClanFinanceIncomeItemBaseVM : ViewModel
	{
		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x00064FEC File Offset: 0x000631EC
		// (set) Token: 0x06001AD0 RID: 6864 RVA: 0x00064FF4 File Offset: 0x000631F4
		public IncomeTypes IncomeTypeAsEnum
		{
			get
			{
				return this._incomeTypeAsEnum;
			}
			protected set
			{
				if (value != this._incomeTypeAsEnum)
				{
					this._incomeTypeAsEnum = value;
					this.IncomeType = (int)value;
				}
			}
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0006500D File Offset: 0x0006320D
		protected ClanFinanceIncomeItemBaseVM(Action<ClanFinanceIncomeItemBaseVM> onSelection, Action onRefresh)
		{
			this._onSelection = onSelection;
			this._onRefresh = onRefresh;
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x0006502E File Offset: 0x0006322E
		protected virtual void PopulateStatsList()
		{
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x00065030 File Offset: 0x00063230
		protected virtual void PopulateActionList()
		{
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x00065032 File Offset: 0x00063232
		public void OnIncomeSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x00065040 File Offset: 0x00063240
		protected string DetermineIncomeText(int incomeAmount)
		{
			if (incomeAmount == 0)
			{
				return GameTexts.FindText("str_clan_finance_value_zero", null).ToString();
			}
			GameTexts.SetVariable("IS_POSITIVE", (this.Income > 0) ? 1 : 0);
			GameTexts.SetVariable("NUMBER", MathF.Abs(this.Income));
			return GameTexts.FindText("str_clan_finance_value", null).ToString();
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x0006509D File Offset: 0x0006329D
		// (set) Token: 0x06001AD7 RID: 6871 RVA: 0x000650A5 File Offset: 0x000632A5
		[DataSourceProperty]
		public MBBindingList<SelectableItemPropertyVM> ItemProperties
		{
			get
			{
				return this._itemProperties;
			}
			set
			{
				if (value != this._itemProperties)
				{
					this._itemProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<SelectableItemPropertyVM>>(value, "ItemProperties");
				}
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x000650C3 File Offset: 0x000632C3
		// (set) Token: 0x06001AD9 RID: 6873 RVA: 0x000650CB File Offset: 0x000632CB
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

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x000650EE File Offset: 0x000632EE
		// (set) Token: 0x06001ADB RID: 6875 RVA: 0x000650F6 File Offset: 0x000632F6
		[DataSourceProperty]
		public string Location
		{
			get
			{
				return this._location;
			}
			set
			{
				if (value != this._location)
				{
					this._location = value;
					base.OnPropertyChangedWithValue<string>(value, "Location");
				}
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06001ADC RID: 6876 RVA: 0x00065119 File Offset: 0x00063319
		// (set) Token: 0x06001ADD RID: 6877 RVA: 0x00065121 File Offset: 0x00063321
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

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x0006513F File Offset: 0x0006333F
		// (set) Token: 0x06001ADF RID: 6879 RVA: 0x00065147 File Offset: 0x00063347
		[DataSourceProperty]
		public string IncomeValueText
		{
			get
			{
				return this._incomeValueText;
			}
			set
			{
				if (value != this._incomeValueText)
				{
					this._incomeValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "IncomeValueText");
				}
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06001AE0 RID: 6880 RVA: 0x0006516A File Offset: 0x0006336A
		// (set) Token: 0x06001AE1 RID: 6881 RVA: 0x00065172 File Offset: 0x00063372
		[DataSourceProperty]
		public string ImageName
		{
			get
			{
				return this._imageName;
			}
			set
			{
				if (value != this._imageName)
				{
					this._imageName = value;
					base.OnPropertyChangedWithValue<string>(value, "ImageName");
				}
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x00065195 File Offset: 0x00063395
		// (set) Token: 0x06001AE3 RID: 6883 RVA: 0x0006519D File Offset: 0x0006339D
		[DataSourceProperty]
		public int Income
		{
			get
			{
				return this._income;
			}
			set
			{
				if (value != this._income)
				{
					this._income = value;
					base.OnPropertyChangedWithValue(value, "Income");
				}
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x000651BB File Offset: 0x000633BB
		// (set) Token: 0x06001AE5 RID: 6885 RVA: 0x000651C3 File Offset: 0x000633C3
		[DataSourceProperty]
		public ImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x000651E1 File Offset: 0x000633E1
		// (set) Token: 0x06001AE7 RID: 6887 RVA: 0x000651E9 File Offset: 0x000633E9
		[DataSourceProperty]
		public int IncomeType
		{
			get
			{
				return this._incomeType;
			}
			set
			{
				if (value != this._incomeType)
				{
					this._incomeType = value;
					base.OnPropertyChangedWithValue(value, "IncomeType");
				}
			}
		}

		// Token: 0x04000C70 RID: 3184
		protected Action _onRefresh;

		// Token: 0x04000C71 RID: 3185
		protected Action<ClanFinanceIncomeItemBaseVM> _onSelection;

		// Token: 0x04000C72 RID: 3186
		protected IncomeTypes _incomeTypeAsEnum;

		// Token: 0x04000C73 RID: 3187
		private int _incomeType;

		// Token: 0x04000C74 RID: 3188
		private string _name;

		// Token: 0x04000C75 RID: 3189
		private string _location;

		// Token: 0x04000C76 RID: 3190
		private string _incomeValueText;

		// Token: 0x04000C77 RID: 3191
		private string _imageName;

		// Token: 0x04000C78 RID: 3192
		private int _income;

		// Token: 0x04000C79 RID: 3193
		private bool _isSelected;

		// Token: 0x04000C7A RID: 3194
		private ImageIdentifierVM _visual;

		// Token: 0x04000C7B RID: 3195
		private MBBindingList<SelectableItemPropertyVM> _itemProperties = new MBBindingList<SelectableItemPropertyVM>();
	}
}
