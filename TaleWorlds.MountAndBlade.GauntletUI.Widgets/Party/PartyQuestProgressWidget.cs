using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000068 RID: 104
	public class PartyQuestProgressWidget : Widget
	{
		// Token: 0x0600058E RID: 1422 RVA: 0x00010B06 File Offset: 0x0000ED06
		public PartyQuestProgressWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00010B10 File Offset: 0x0000ED10
		private void UpdateDividers()
		{
			if (this.DividerContainer == null || this.DividerBrush == null)
			{
				return;
			}
			int itemCount = this.ItemCount;
			if (this.DividerContainer.ChildCount > itemCount)
			{
				int num = this.DividerContainer.ChildCount - itemCount;
				for (int i = 0; i < num; i++)
				{
					this.DividerContainer.RemoveChild(this.DividerContainer.GetChild(i));
				}
			}
			else if (itemCount > this.DividerContainer.ChildCount)
			{
				int num2 = itemCount - this.DividerContainer.ChildCount;
				for (int j = 0; j < num2; j++)
				{
					this.DividerContainer.AddChild(this.CreateDivider());
				}
			}
			this.UpdateDividerPositions();
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00010BBC File Offset: 0x0000EDBC
		private Widget CreateDivider()
		{
			Widget widget = new Widget(base.Context);
			widget.WidthSizePolicy = SizePolicy.StretchToParent;
			widget.HeightSizePolicy = SizePolicy.StretchToParent;
			BrushWidget brushWidget = new BrushWidget(base.Context);
			brushWidget.WidthSizePolicy = SizePolicy.Fixed;
			brushWidget.HeightSizePolicy = SizePolicy.Fixed;
			brushWidget.Brush = this.DividerBrush;
			brushWidget.SuggestedWidth = (float)brushWidget.ReadOnlyBrush.Sprite.Width;
			brushWidget.SuggestedHeight = (float)brushWidget.ReadOnlyBrush.Sprite.Height;
			brushWidget.HorizontalAlignment = HorizontalAlignment.Right;
			brushWidget.VerticalAlignment = VerticalAlignment.Center;
			brushWidget.PositionXOffset = (float)brushWidget.ReadOnlyBrush.Sprite.Width * 0.5f;
			widget.AddChild(brushWidget);
			return widget;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00010C68 File Offset: 0x0000EE68
		private void UpdateDividerPositions()
		{
			int childCount = this.DividerContainer.ChildCount;
			float num = this.DividerContainer.Size.X / (float)(childCount + 1);
			for (int i = 0; i < childCount; i++)
			{
				Widget child = this.DividerContainer.GetChild(i);
				child.PositionXOffset = (float)i * num - child.Size.X / 2f;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x00010CCC File Offset: 0x0000EECC
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x00010CD4 File Offset: 0x0000EED4
		[Editor(false)]
		public int ItemCount
		{
			get
			{
				return this._itemCount;
			}
			set
			{
				if (this._itemCount != value)
				{
					this._itemCount = value;
					base.OnPropertyChanged(value, "ItemCount");
					this.UpdateDividers();
				}
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00010CF8 File Offset: 0x0000EEF8
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x00010D00 File Offset: 0x0000EF00
		[Editor(false)]
		public ListPanel DividerContainer
		{
			get
			{
				return this._dividerContainer;
			}
			set
			{
				if (this._dividerContainer != value)
				{
					this._dividerContainer = value;
					base.OnPropertyChanged<ListPanel>(value, "DividerContainer");
				}
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00010D1E File Offset: 0x0000EF1E
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x00010D26 File Offset: 0x0000EF26
		[Editor(false)]
		public Brush DividerBrush
		{
			get
			{
				return this._dividerBrush;
			}
			set
			{
				if (this._dividerBrush != value)
				{
					this._dividerBrush = value;
					base.OnPropertyChanged<Brush>(value, "DividerBrush");
				}
			}
		}

		// Token: 0x0400025F RID: 607
		private int _itemCount;

		// Token: 0x04000260 RID: 608
		private ListPanel _dividerContainer;

		// Token: 0x04000261 RID: 609
		private Brush _dividerBrush;
	}
}
