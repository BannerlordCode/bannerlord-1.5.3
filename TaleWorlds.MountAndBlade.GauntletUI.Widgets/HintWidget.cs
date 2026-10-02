using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000024 RID: 36
	public class HintWidget : Widget
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x000073FE File Offset: 0x000055FE
		public HintWidget(UIContext context)
			: base(context)
		{
			base.IsDisabled = true;
			base.DoNotAcceptEvents = true;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00007415 File Offset: 0x00005615
		protected override void OnConnectedToRoot()
		{
			base.ParentWidget.EventFire += this.ParentWidgetEventFired;
			base.OnConnectedToRoot();
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00007434 File Offset: 0x00005634
		protected override void OnDisconnectedFromRoot()
		{
			base.ParentWidget.EventFire -= this.ParentWidgetEventFired;
			base.OnDisconnectedFromRoot();
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007453 File Offset: 0x00005653
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			Debug.FailedAssert("HintWidget is not intended to be used as a parent widget!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\HintWidget.cs", "OnChildAdded", 34);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00007474 File Offset: 0x00005674
		private void ParentWidgetEventFired(Widget widget, string eventName, object[] args)
		{
			if (base.IsVisible)
			{
				if (eventName == "HoverBegin")
				{
					base.EventFired("HoverBegin", args);
					return;
				}
				if (eventName == "HoverEnd")
				{
					base.EventFired("HoverEnd", args);
					return;
				}
				if (eventName == "DragHoverBegin")
				{
					base.EventFired("DragHoverBegin", args);
					return;
				}
				if (eventName == "DragHoverEnd")
				{
					base.EventFired("DragHoverEnd", args);
				}
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000074F0 File Offset: 0x000056F0
		protected override bool OnPreviewMousePressed()
		{
			return false;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000074F3 File Offset: 0x000056F3
		protected override bool OnPreviewDragBegin()
		{
			return false;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000074F6 File Offset: 0x000056F6
		protected override bool OnPreviewDrop()
		{
			return false;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000074F9 File Offset: 0x000056F9
		protected override bool OnPreviewMouseScroll()
		{
			return false;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000074FC File Offset: 0x000056FC
		protected override bool OnPreviewMouseReleased()
		{
			return false;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000074FF File Offset: 0x000056FF
		protected override bool OnPreviewMouseMove()
		{
			return false;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007502 File Offset: 0x00005702
		protected override bool OnPreviewDragHover()
		{
			return false;
		}
	}
}
