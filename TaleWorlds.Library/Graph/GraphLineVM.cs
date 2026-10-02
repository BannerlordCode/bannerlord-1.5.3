using System;

namespace TaleWorlds.Library.Graph
{
	// Token: 0x020000B6 RID: 182
	public class GraphLineVM : ViewModel
	{
		// Token: 0x060006CD RID: 1741 RVA: 0x00017263 File Offset: 0x00015463
		public GraphLineVM(string ID, string name)
		{
			this.Points = new MBBindingList<GraphLinePointVM>();
			this.Name = name;
			this.ID = ID;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00017284 File Offset: 0x00015484
		// (set) Token: 0x060006CF RID: 1743 RVA: 0x0001728C File Offset: 0x0001548C
		[DataSourceProperty]
		public MBBindingList<GraphLinePointVM> Points
		{
			get
			{
				return this._points;
			}
			set
			{
				if (value != this._points)
				{
					this._points = value;
					base.OnPropertyChangedWithValue<MBBindingList<GraphLinePointVM>>(value, "Points");
				}
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x000172AA File Offset: 0x000154AA
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x000172B2 File Offset: 0x000154B2
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

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x000172D5 File Offset: 0x000154D5
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x000172DD File Offset: 0x000154DD
		[DataSourceProperty]
		public string ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if (value != this._ID)
				{
					this._ID = value;
					base.OnPropertyChangedWithValue<string>(value, "ID");
				}
			}
		}

		// Token: 0x04000216 RID: 534
		private MBBindingList<GraphLinePointVM> _points;

		// Token: 0x04000217 RID: 535
		private string _name;

		// Token: 0x04000218 RID: 536
		private string _ID;
	}
}
