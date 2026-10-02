using System;

namespace TaleWorlds.Library.Graph
{
	// Token: 0x020000B5 RID: 181
	public class GraphLinePointVM : ViewModel
	{
		// Token: 0x060006C8 RID: 1736 RVA: 0x00017201 File Offset: 0x00015401
		public GraphLinePointVM(float horizontalValue, float verticalValue)
		{
			this.HorizontalValue = horizontalValue;
			this.VerticalValue = verticalValue;
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00017217 File Offset: 0x00015417
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x0001721F File Offset: 0x0001541F
		[DataSourceProperty]
		public float HorizontalValue
		{
			get
			{
				return this._horizontalValue;
			}
			set
			{
				if (value != this._horizontalValue)
				{
					this._horizontalValue = value;
					base.OnPropertyChangedWithValue(value, "HorizontalValue");
				}
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x0001723D File Offset: 0x0001543D
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x00017245 File Offset: 0x00015445
		[DataSourceProperty]
		public float VerticalValue
		{
			get
			{
				return this._verticalValue;
			}
			set
			{
				if (value != this._verticalValue)
				{
					this._verticalValue = value;
					base.OnPropertyChangedWithValue(value, "VerticalValue");
				}
			}
		}

		// Token: 0x04000214 RID: 532
		private float _horizontalValue;

		// Token: 0x04000215 RID: 533
		private float _verticalValue;
	}
}
