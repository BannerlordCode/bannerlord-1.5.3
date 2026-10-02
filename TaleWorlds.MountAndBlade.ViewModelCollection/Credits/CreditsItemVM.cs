using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Credits
{
	// Token: 0x02000082 RID: 130
	public class CreditsItemVM : ViewModel
	{
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0002692C File Offset: 0x00024B2C
		// (set) Token: 0x06000AD0 RID: 2768 RVA: 0x00026934 File Offset: 0x00024B34
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x00026957 File Offset: 0x00024B57
		// (set) Token: 0x06000AD2 RID: 2770 RVA: 0x0002695F File Offset: 0x00024B5F
		[DataSourceProperty]
		public string Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue<string>(value, "Type");
				}
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x00026982 File Offset: 0x00024B82
		// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x0002698A File Offset: 0x00024B8A
		[DataSourceProperty]
		public MBBindingList<CreditsItemVM> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				if (value != this._items)
				{
					this._items = value;
					base.OnPropertyChangedWithValue<MBBindingList<CreditsItemVM>>(value, "Items");
				}
			}
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000269A8 File Offset: 0x00024BA8
		public CreditsItemVM()
		{
			this._items = new MBBindingList<CreditsItemVM>();
			this.Type = "Entry";
			this.Text = "";
		}

		// Token: 0x04000500 RID: 1280
		private string _text;

		// Token: 0x04000501 RID: 1281
		private string _type;

		// Token: 0x04000502 RID: 1282
		private MBBindingList<CreditsItemVM> _items;
	}
}
