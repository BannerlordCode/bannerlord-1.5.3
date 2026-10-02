using System;
using System.Collections.Generic;
using System.Numerics;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000055 RID: 85
	public abstract class Container : Widget
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00017C28 File Offset: 0x00015E28
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x00017C30 File Offset: 0x00015E30
		public ContainerItemDescription DefaultItemDescription { get; private set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060005AF RID: 1455
		// (set) Token: 0x060005B0 RID: 1456
		public abstract Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x060005B1 RID: 1457
		public abstract Vector2 GetDropGizmoPosition(Vector2 draggedWidgetPosition);

		// Token: 0x060005B2 RID: 1458
		public abstract int GetIndexForDrop(Vector2 draggedWidgetPosition);

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00017C39 File Offset: 0x00015E39
		// (set) Token: 0x060005B4 RID: 1460 RVA: 0x00017C44 File Offset: 0x00015E44
		public int IntValue
		{
			get
			{
				return this._intValue;
			}
			set
			{
				if (!this._currentlyChangingIntValue)
				{
					this._currentlyChangingIntValue = true;
					if (value != this._intValue && value < base.ChildCount)
					{
						this._intValue = value;
						this.UpdateSelected();
						foreach (Action<Widget> action in this.SelectEventHandlers)
						{
							action(this);
						}
						base.EventFired("SelectedItemChange", Array.Empty<object>());
						base.OnPropertyChanged(value, "IntValue");
					}
					this._currentlyChangingIntValue = false;
				}
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060005B5 RID: 1461
		public abstract bool IsDragHovering { get; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00017CE8 File Offset: 0x00015EE8
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x00017CF0 File Offset: 0x00015EF0
		public int DragHoverInsertionIndex
		{
			get
			{
				return this._dragHoverInsertionIndex;
			}
			set
			{
				if (this._dragHoverInsertionIndex != value)
				{
					this._dragHoverInsertionIndex = value;
					base.SetMeasureAndLayoutDirty();
				}
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00017D08 File Offset: 0x00015F08
		protected Container(UIContext context)
			: base(context)
		{
			this.DefaultItemDescription = new ContainerItemDescription();
			this._itemDescriptions = new List<ContainerItemDescription>();
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00017D68 File Offset: 0x00015F68
		private void UpdateSelected()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				ButtonWidget buttonWidget = base.GetChild(i) as ButtonWidget;
				if (buttonWidget != null)
				{
					bool flag = i == this.IntValue;
					buttonWidget.IsSelected = flag;
				}
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00017DA8 File Offset: 0x00015FA8
		protected internal override bool OnDrop()
		{
			if (base.AcceptDrop)
			{
				bool flag = true;
				if (this.AcceptDropHandler != null)
				{
					flag = this.AcceptDropHandler(this, base.EventManager.DraggedWidget);
				}
				if (flag)
				{
					Widget widget = base.EventManager.ReleaseDraggedWidget();
					int indexForDrop = this.GetIndexForDrop(base.EventManager.MousePosition * base.Context.CustomScale);
					if (!base.DropEventHandledManually)
					{
						widget.ParentWidget = this;
						widget.SetSiblingIndex(indexForDrop, false);
					}
					base.EventFired("Drop", new object[] { widget, indexForDrop });
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005BB RID: 1467
		public abstract void OnChildSelected(Widget widget);

		// Token: 0x060005BC RID: 1468 RVA: 0x00017E4C File Offset: 0x0001604C
		public ContainerItemDescription GetItemDescription(string id, int index)
		{
			bool flag = !string.IsNullOrEmpty(id);
			ContainerItemDescription containerItemDescription = null;
			ContainerItemDescription containerItemDescription2 = null;
			for (int i = 0; i < this._itemDescriptions.Count; i++)
			{
				ContainerItemDescription containerItemDescription3 = this._itemDescriptions[i];
				if (flag && containerItemDescription3.WidgetId == id)
				{
					containerItemDescription = containerItemDescription3;
				}
				if (index == containerItemDescription3.WidgetIndex)
				{
					containerItemDescription2 = containerItemDescription3;
				}
			}
			ContainerItemDescription containerItemDescription4;
			if ((containerItemDescription4 = containerItemDescription) == null)
			{
				containerItemDescription4 = containerItemDescription2 ?? this.DefaultItemDescription;
			}
			return containerItemDescription4;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00017EC0 File Offset: 0x000160C0
		protected override void OnChildAdded(Widget child)
		{
			foreach (Action<Widget, Widget> action in this.ItemAddEventHandlers)
			{
				action(this, child);
			}
			base.OnChildAdded(child);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00017F1C File Offset: 0x0001611C
		protected override void OnBeforeChildRemoved(Widget child)
		{
			foreach (Action<Widget, Widget> action in this.ItemRemoveEventHandlers)
			{
				action(this, child);
			}
			base.OnBeforeChildRemoved(child);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00017F78 File Offset: 0x00016178
		protected override void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			if (this.IntValue >= base.ChildCount)
			{
				if (this.ClearSelectedOnRemoval)
				{
					this.IntValue = -1;
				}
				else
				{
					this.IntValue = base.ChildCount - 1;
				}
			}
			else if (previousIndexOfChild >= 0 && this.IntValue >= 0)
			{
				if (this.IntValue == previousIndexOfChild && this.ClearSelectedOnRemoval)
				{
					this.IntValue = -1;
				}
				else if (previousIndexOfChild < this.IntValue)
				{
					int intValue = this.IntValue;
					this.IntValue = intValue - 1;
				}
			}
			foreach (Action<Widget> action in this.ItemAfterRemoveEventHandlers)
			{
				action(this);
			}
			base.OnAfterChildRemoved(child, previousIndexOfChild);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00018040 File Offset: 0x00016240
		public void AddItemDescription(ContainerItemDescription itemDescription)
		{
			this._itemDescriptions.Add(itemDescription);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00018050 File Offset: 0x00016250
		public ScrollablePanel FindParentPanel()
		{
			for (Widget widget = base.ParentWidget; widget != null; widget = widget.ParentWidget)
			{
				ScrollablePanel scrollablePanel;
				if ((scrollablePanel = widget as ScrollablePanel) != null)
				{
					return scrollablePanel;
				}
			}
			return null;
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0001807D File Offset: 0x0001627D
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x00018085 File Offset: 0x00016285
		[Editor(false)]
		public bool ClearSelectedOnRemoval
		{
			get
			{
				return this._clearSelectedOnRemoval;
			}
			set
			{
				if (this._clearSelectedOnRemoval != value)
				{
					this._clearSelectedOnRemoval = value;
					base.OnPropertyChanged(value, "ClearSelectedOnRemoval");
				}
			}
		}

		// Token: 0x040002B5 RID: 693
		public List<Action<Widget>> SelectEventHandlers = new List<Action<Widget>>();

		// Token: 0x040002B6 RID: 694
		public List<Action<Widget, Widget>> ItemAddEventHandlers = new List<Action<Widget, Widget>>();

		// Token: 0x040002B7 RID: 695
		public List<Action<Widget, Widget>> ItemRemoveEventHandlers = new List<Action<Widget, Widget>>();

		// Token: 0x040002B8 RID: 696
		public List<Action<Widget>> ItemAfterRemoveEventHandlers = new List<Action<Widget>>();

		// Token: 0x040002B9 RID: 697
		private int _intValue = -1;

		// Token: 0x040002BA RID: 698
		private bool _currentlyChangingIntValue;

		// Token: 0x040002BB RID: 699
		public bool ShowSelection;

		// Token: 0x040002BC RID: 700
		private int _dragHoverInsertionIndex;

		// Token: 0x040002BD RID: 701
		private List<ContainerItemDescription> _itemDescriptions;

		// Token: 0x040002BE RID: 702
		private bool _clearSelectedOnRemoval;
	}
}
