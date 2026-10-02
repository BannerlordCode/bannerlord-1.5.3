using System;
using System.Numerics;
using TaleWorlds.GauntletUI.Layout;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200005E RID: 94
	public class ListPanel : Container
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0001B89C File Offset: 0x00019A9C
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x0001B8A4 File Offset: 0x00019AA4
		public StackLayout StackLayout { get; private set; }

		// Token: 0x0600065E RID: 1630 RVA: 0x0001B8AD File Offset: 0x00019AAD
		public ListPanel(UIContext context)
			: base(context)
		{
			this.StackLayout = new StackLayout();
			base.LayoutImp = this.StackLayout;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001B8CD File Offset: 0x00019ACD
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.UpdateListPanel();
			if (this.ResetSelectedOnLosingFocus && !base.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget))
			{
				base.IntValue = -1;
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001B8FE File Offset: 0x00019AFE
		private void UpdateListPanel()
		{
			if (base.AcceptDrop && this.IsDragHovering)
			{
				base.DragHoverInsertionIndex = this.GetIndexForDrop(base.EventManager.MousePosition * base.Context.CustomScale);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001B937 File Offset: 0x00019B37
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x0001B93F File Offset: 0x00019B3F
		public override Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x06000663 RID: 1635 RVA: 0x0001B948 File Offset: 0x00019B48
		public override int GetIndexForDrop(Vector2 draggedWidgetPosition)
		{
			return this.StackLayout.GetIndexForDrop(this, draggedWidgetPosition);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001B957 File Offset: 0x00019B57
		public override Vector2 GetDropGizmoPosition(Vector2 draggedWidgetPosition)
		{
			return this.StackLayout.GetDropGizmoPosition(this, draggedWidgetPosition);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001B968 File Offset: 0x00019B68
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

		// Token: 0x06000666 RID: 1638 RVA: 0x0001B99B File Offset: 0x00019B9B
		protected internal override void OnDragHoverBegin()
		{
			this._dragHovering = true;
			base.SetMeasureAndLayoutDirty();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001B9AA File Offset: 0x00019BAA
		protected internal override void OnDragHoverEnd()
		{
			this._dragHovering = false;
			base.SetMeasureAndLayoutDirty();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001B9B9 File Offset: 0x00019BB9
		protected override bool OnPreviewDragHover()
		{
			return base.AcceptDrop;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0001B9C1 File Offset: 0x00019BC1
		public override bool IsDragHovering
		{
			get
			{
				return this._dragHovering;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x0001B9C9 File Offset: 0x00019BC9
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x0001B9D1 File Offset: 0x00019BD1
		[Editor(false)]
		public bool ResetSelectedOnLosingFocus
		{
			get
			{
				return this._resetSelectedOnLosingFocus;
			}
			set
			{
				if (this._resetSelectedOnLosingFocus != value)
				{
					this._resetSelectedOnLosingFocus = value;
					base.OnPropertyChanged(value, "ResetSelectedOnLosingFocus");
				}
			}
		}

		// Token: 0x040002FF RID: 767
		private bool _dragHovering;

		// Token: 0x04000300 RID: 768
		private bool _resetSelectedOnLosingFocus;
	}
}
