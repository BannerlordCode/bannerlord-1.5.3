using System;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000E9 RID: 233
	public class ListTypeVM : ViewModel
	{
		// Token: 0x060015AD RID: 5549 RVA: 0x00055DFC File Offset: 0x00053FFC
		public ListTypeVM(EncyclopediaPage encyclopediaPage)
		{
			this.EncyclopediaPage = encyclopediaPage;
			this.ID = encyclopediaPage.GetIdentifierNames()[0];
			this.ImageID = encyclopediaPage.GetStringID();
			this.Order = encyclopediaPage.HomePageOrderIndex;
			this.RefreshValues();
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00055E37 File Offset: 0x00054037
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.EncyclopediaPage.GetName().ToString();
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00055E55 File Offset: 0x00054055
		public void Execute()
		{
			Campaign.Current.EncyclopediaManager.GoToLink("ListPage", this.ID);
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060015B0 RID: 5552 RVA: 0x00055E71 File Offset: 0x00054071
		// (set) Token: 0x060015B1 RID: 5553 RVA: 0x00055E79 File Offset: 0x00054079
		[DataSourceProperty]
		public string ID
		{
			get
			{
				return this._id;
			}
			set
			{
				if (value != this._id)
				{
					this._id = value;
					base.OnPropertyChangedWithValue<string>(value, "ID");
				}
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060015B2 RID: 5554 RVA: 0x00055E9C File Offset: 0x0005409C
		// (set) Token: 0x060015B3 RID: 5555 RVA: 0x00055EA4 File Offset: 0x000540A4
		[DataSourceProperty]
		public int Order
		{
			get
			{
				return this._order;
			}
			set
			{
				if (value != this._order)
				{
					this._order = value;
					base.OnPropertyChangedWithValue(value, "Order");
				}
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060015B4 RID: 5556 RVA: 0x00055EC2 File Offset: 0x000540C2
		// (set) Token: 0x060015B5 RID: 5557 RVA: 0x00055ECA File Offset: 0x000540CA
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

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x00055EED File Offset: 0x000540ED
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x00055EF5 File Offset: 0x000540F5
		[DataSourceProperty]
		public string ImageID
		{
			get
			{
				return this._imageId;
			}
			set
			{
				if (value != this._imageId)
				{
					this._imageId = value;
					base.OnPropertyChangedWithValue<string>(value, "ImageID");
				}
			}
		}

		// Token: 0x040009D1 RID: 2513
		public readonly EncyclopediaPage EncyclopediaPage;

		// Token: 0x040009D2 RID: 2514
		private string _name;

		// Token: 0x040009D3 RID: 2515
		private string _id;

		// Token: 0x040009D4 RID: 2516
		private string _imageId;

		// Token: 0x040009D5 RID: 2517
		private int _order;
	}
}
