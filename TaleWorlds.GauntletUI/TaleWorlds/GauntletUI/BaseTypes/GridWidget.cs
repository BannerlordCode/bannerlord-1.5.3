using System;
using System.Numerics;
using TaleWorlds.GauntletUI.Layout;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200005A RID: 90
	public class GridWidget : Container
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0001AC13 File Offset: 0x00018E13
		// (set) Token: 0x06000627 RID: 1575 RVA: 0x0001AC1B File Offset: 0x00018E1B
		public GridLayout GridLayout { get; private set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0001AC24 File Offset: 0x00018E24
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x0001AC2C File Offset: 0x00018E2C
		[Editor(false)]
		public float DefaultCellWidth
		{
			get
			{
				return this._defaultCellWidth;
			}
			set
			{
				if (this._defaultCellWidth != value)
				{
					this._defaultCellWidth = value;
					base.OnPropertyChanged(value, "DefaultCellWidth");
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0001AC4A File Offset: 0x00018E4A
		public float DefaultScaledCellWidth
		{
			get
			{
				return this.DefaultCellWidth * base._scaleToUse;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x0001AC59 File Offset: 0x00018E59
		// (set) Token: 0x0600062C RID: 1580 RVA: 0x0001AC61 File Offset: 0x00018E61
		[Editor(false)]
		public float DefaultCellHeight
		{
			get
			{
				return this._defaultCellHeight;
			}
			set
			{
				if (this._defaultCellHeight != value)
				{
					this._defaultCellHeight = value;
					base.OnPropertyChanged(value, "DefaultCellHeight");
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x0001AC7F File Offset: 0x00018E7F
		public float DefaultScaledCellHeight
		{
			get
			{
				return this.DefaultCellHeight * base._scaleToUse;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x0001AC8E File Offset: 0x00018E8E
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x0001AC96 File Offset: 0x00018E96
		[Editor(false)]
		public int RowCount
		{
			get
			{
				return this._rowCount;
			}
			set
			{
				if (this._rowCount != value)
				{
					this._rowCount = value;
					base.OnPropertyChanged(value, "RowCount");
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0001ACB4 File Offset: 0x00018EB4
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x0001ACBC File Offset: 0x00018EBC
		[Editor(false)]
		public int ColumnCount
		{
			get
			{
				return this._columnCount;
			}
			set
			{
				if (this._columnCount != value)
				{
					this._columnCount = value;
					base.OnPropertyChanged(value, "ColumnCount");
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x0001ACDA File Offset: 0x00018EDA
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x0001ACE2 File Offset: 0x00018EE2
		[Editor(false)]
		public bool UseDynamicCellWidth
		{
			get
			{
				return this._useDynamicCellWidth;
			}
			set
			{
				if (this._useDynamicCellWidth != value)
				{
					this._useDynamicCellWidth = value;
					base.OnPropertyChanged(value, "UseDynamicCellWidth");
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0001AD00 File Offset: 0x00018F00
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x0001AD08 File Offset: 0x00018F08
		[Editor(false)]
		public bool UseDynamicCellHeight
		{
			get
			{
				return this._useDynamicCellHeight;
			}
			set
			{
				if (this._useDynamicCellHeight != value)
				{
					this._useDynamicCellHeight = value;
					base.OnPropertyChanged(value, "UseDynamicCellHeight");
				}
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x0001AD26 File Offset: 0x00018F26
		// (set) Token: 0x06000637 RID: 1591 RVA: 0x0001AD2E File Offset: 0x00018F2E
		public override Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0001AD37 File Offset: 0x00018F37
		public override bool IsDragHovering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001AD3A File Offset: 0x00018F3A
		public GridWidget(UIContext context)
			: base(context)
		{
			this.GridLayout = new GridLayout();
			base.LayoutImp = this.GridLayout;
			this.RowCount = -1;
			this.ColumnCount = -1;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001AD68 File Offset: 0x00018F68
		public override Vector2 GetDropGizmoPosition(Vector2 draggedWidgetPosition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001AD6F File Offset: 0x00018F6F
		public override int GetIndexForDrop(Vector2 draggedWidgetPosition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001AD78 File Offset: 0x00018F78
		public override void OnChildSelected(Widget widget)
		{
			int num = -1;
			for (int i = 0; i < base.ChildCount; i++)
			{
				if (widget == base.GetChild(i))
				{
					num = i;
				}
			}
			base.IntValue = num;
		}

		// Token: 0x040002EE RID: 750
		private float _defaultCellWidth;

		// Token: 0x040002EF RID: 751
		private float _defaultCellHeight;

		// Token: 0x040002F0 RID: 752
		private int _rowCount;

		// Token: 0x040002F1 RID: 753
		private int _columnCount;

		// Token: 0x040002F2 RID: 754
		private bool _useDynamicCellWidth;

		// Token: 0x040002F3 RID: 755
		private bool _useDynamicCellHeight;

		// Token: 0x040002F4 RID: 756
		public const int DefaultRowCount = 3;

		// Token: 0x040002F5 RID: 757
		public const int DefaultColumnCount = 3;
	}
}
