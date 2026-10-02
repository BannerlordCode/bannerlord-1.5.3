using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement
{
	// Token: 0x0200006A RID: 106
	public abstract class KingdomItemVM : ViewModel
	{
		// Token: 0x060007AC RID: 1964 RVA: 0x000242F2 File Offset: 0x000224F2
		protected virtual void OnSelect()
		{
			this.IsSelected = true;
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x000242FB File Offset: 0x000224FB
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x00024303 File Offset: 0x00022503
		[DataSourceProperty]
		public bool IsNew
		{
			get
			{
				return this._isNew;
			}
			set
			{
				if (value != this._isNew)
				{
					this._isNew = value;
					base.OnPropertyChangedWithValue(value, "IsNew");
				}
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00024321 File Offset: 0x00022521
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x00024329 File Offset: 0x00022529
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

		// Token: 0x0400034B RID: 843
		private bool _isSelected;

		// Token: 0x0400034C RID: 844
		private bool _isNew;
	}
}
