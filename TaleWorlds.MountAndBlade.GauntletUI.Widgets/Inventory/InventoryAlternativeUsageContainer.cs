using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013B RID: 315
	public class InventoryAlternativeUsageContainer : Container
	{
		// Token: 0x06001082 RID: 4226 RVA: 0x0002D616 File Offset: 0x0002B816
		public InventoryAlternativeUsageContainer(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0002D63C File Offset: 0x0002B83C
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

		// Token: 0x06001084 RID: 4228 RVA: 0x0002D670 File Offset: 0x0002B870
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			foreach (Action<Widget, Widget> action in this.ItemAddEventHandlers)
			{
				action(this, child);
			}
			base.EventFired("ItemAdd", Array.Empty<object>());
			this.SetChildrenLayout();
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0002D6E0 File Offset: 0x0002B8E0
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			foreach (Action<Widget, Widget> action in this.ItemRemoveEventHandlers)
			{
				action(this, child);
			}
			base.EventFired("ItemRemove", Array.Empty<object>());
			this.SetChildrenLayout();
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0002D750 File Offset: 0x0002B950
		private void SetChildrenLayout()
		{
			if (base.ChildCount == 0)
			{
				return;
			}
			int num = MathF.Ceiling((float)base.ChildCount / (float)this.ColumnLimit);
			for (int i = 0; i < num; i++)
			{
				int num2 = MathF.Min(this.ColumnLimit, base.ChildCount - (num - 1) * this.ColumnLimit);
				int num3 = i * (int)this.CellHeight;
				for (int j = 0; j < num2; j++)
				{
					int num4 = (int)(((float)j - ((float)num2 - 1f) / 2f) * this.CellWidth);
					int num5 = i * this.ColumnLimit + j;
					Widget child = base.GetChild(num5);
					if (num4 > 0)
					{
						child.MarginLeft = (float)(num4 * 2);
					}
					else if (num4 < 0)
					{
						child.MarginRight = (float)(-(float)num4 * 2);
					}
					child.MarginTop = (float)num3;
				}
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x0002D825 File Offset: 0x0002BA25
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x0002D82D File Offset: 0x0002BA2D
		[Editor(false)]
		public int ColumnLimit
		{
			get
			{
				return this._columnLimit;
			}
			set
			{
				if (this._columnLimit != value)
				{
					this._columnLimit = value;
					base.OnPropertyChanged(value, "ColumnLimit");
				}
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x0002D84B File Offset: 0x0002BA4B
		// (set) Token: 0x0600108A RID: 4234 RVA: 0x0002D853 File Offset: 0x0002BA53
		[Editor(false)]
		public float CellWidth
		{
			get
			{
				return this._cellWidth;
			}
			set
			{
				if (this._cellWidth != value)
				{
					this._cellWidth = value;
					base.OnPropertyChanged(value, "CellWidth");
				}
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0002D871 File Offset: 0x0002BA71
		// (set) Token: 0x0600108C RID: 4236 RVA: 0x0002D879 File Offset: 0x0002BA79
		[Editor(false)]
		public float CellHeight
		{
			get
			{
				return this._cellHeight;
			}
			set
			{
				if (this._cellHeight != value)
				{
					this._cellHeight = value;
					base.OnPropertyChanged(value, "CellHeight");
				}
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x0002D897 File Offset: 0x0002BA97
		// (set) Token: 0x0600108E RID: 4238 RVA: 0x0002D89F File Offset: 0x0002BA9F
		public override Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x0600108F RID: 4239 RVA: 0x0002D8A8 File Offset: 0x0002BAA8
		public override Vector2 GetDropGizmoPosition(Vector2 draggedWidgetPosition)
		{
			return Vector2.Zero;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0002D8AF File Offset: 0x0002BAAF
		public override int GetIndexForDrop(Vector2 draggedWidgetPosition)
		{
			return -1;
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001091 RID: 4241 RVA: 0x0002D8B2 File Offset: 0x0002BAB2
		public override bool IsDragHovering { get; }

		// Token: 0x04000786 RID: 1926
		private int _columnLimit = 2;

		// Token: 0x04000787 RID: 1927
		private float _cellWidth = 100f;

		// Token: 0x04000788 RID: 1928
		private float _cellHeight = 100f;
	}
}
