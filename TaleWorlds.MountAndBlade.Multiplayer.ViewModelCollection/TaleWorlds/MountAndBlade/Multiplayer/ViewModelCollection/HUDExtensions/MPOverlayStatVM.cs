using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000099 RID: 153
	public class MPOverlayStatVM : ViewModel
	{
		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06000F4D RID: 3917 RVA: 0x0002F89C File Offset: 0x0002DA9C
		public string Id { get; }

		// Token: 0x06000F4E RID: 3918 RVA: 0x0002F8A4 File Offset: 0x0002DAA4
		public MPOverlayStatVM(string id, string header, string value)
		{
			this.Id = id;
			this.Header = header;
			this.Value = value;
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0002F8C1 File Offset: 0x0002DAC1
		public void Refresh(string value)
		{
			this.Value = value;
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0002F8CA File Offset: 0x0002DACA
		// (set) Token: 0x06000F51 RID: 3921 RVA: 0x0002F8D2 File Offset: 0x0002DAD2
		[DataSourceProperty]
		public string Header
		{
			get
			{
				return this._header;
			}
			set
			{
				if (value != this._header)
				{
					this._header = value;
					base.OnPropertyChangedWithValue<string>(value, "Header");
				}
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06000F52 RID: 3922 RVA: 0x0002F8F5 File Offset: 0x0002DAF5
		// (set) Token: 0x06000F53 RID: 3923 RVA: 0x0002F8FD File Offset: 0x0002DAFD
		[DataSourceProperty]
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue<string>(value, "Value");
				}
			}
		}

		// Token: 0x0400071E RID: 1822
		private string _header;

		// Token: 0x0400071F RID: 1823
		private string _value;
	}
}
