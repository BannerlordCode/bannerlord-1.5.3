using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000B0 RID: 176
	public class TownManagementShopItemVM : ViewModel
	{
		// Token: 0x06001089 RID: 4233 RVA: 0x00043860 File Offset: 0x00041A60
		public TownManagementShopItemVM(Workshop workshop)
		{
			this._workshop = workshop;
			this.IsEmpty = this._workshop.WorkshopType == null;
			if (!this.IsEmpty)
			{
				this.ShopId = this._workshop.WorkshopType.StringId;
			}
			else
			{
				this.ShopId = "empty";
			}
			this.RefreshValues();
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x000438C0 File Offset: 0x00041AC0
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (!this.IsEmpty)
			{
				this.ShopName = this._workshop.WorkshopType.Name.ToString();
				return;
			}
			this.ShopName = GameTexts.FindText("str_empty", null).ToString();
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0004390D File Offset: 0x00041B0D
		public void ExecuteBeginHint()
		{
			if (this._workshop.WorkshopType != null)
			{
				InformationManager.ShowTooltip(typeof(Workshop), new object[] { this._workshop });
			}
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0004393A File Offset: 0x00041B3A
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x00043941 File Offset: 0x00041B41
		// (set) Token: 0x0600108E RID: 4238 RVA: 0x00043949 File Offset: 0x00041B49
		[DataSourceProperty]
		public bool IsEmpty
		{
			get
			{
				return this._isEmpty;
			}
			set
			{
				if (value != this._isEmpty)
				{
					this._isEmpty = value;
					base.OnPropertyChangedWithValue(value, "IsEmpty");
				}
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x0600108F RID: 4239 RVA: 0x00043967 File Offset: 0x00041B67
		// (set) Token: 0x06001090 RID: 4240 RVA: 0x0004396F File Offset: 0x00041B6F
		[DataSourceProperty]
		public string ShopName
		{
			get
			{
				return this._shopName;
			}
			set
			{
				if (value != this._shopName)
				{
					this._shopName = value;
					base.OnPropertyChangedWithValue<string>(value, "ShopName");
				}
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001091 RID: 4241 RVA: 0x00043992 File Offset: 0x00041B92
		// (set) Token: 0x06001092 RID: 4242 RVA: 0x0004399A File Offset: 0x00041B9A
		[DataSourceProperty]
		public string ShopId
		{
			get
			{
				return this._shopId;
			}
			set
			{
				if (value != this._shopId)
				{
					this._shopId = value;
					base.OnPropertyChangedWithValue<string>(value, "ShopId");
				}
			}
		}

		// Token: 0x04000785 RID: 1925
		private readonly Workshop _workshop;

		// Token: 0x04000786 RID: 1926
		private bool _isEmpty;

		// Token: 0x04000787 RID: 1927
		private string _shopName;

		// Token: 0x04000788 RID: 1928
		private string _shopId;
	}
}
