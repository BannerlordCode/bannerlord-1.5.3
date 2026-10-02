using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.Launcher.Library.CustomWidgets
{
	// Token: 0x02000022 RID: 34
	public class LauncherHintTriggerWidget : Widget
	{
		// Token: 0x06000157 RID: 343 RVA: 0x0000633D File Offset: 0x0000453D
		public LauncherHintTriggerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006346 File Offset: 0x00004546
		protected override void OnConnectedToRoot()
		{
			base.ParentWidget.EventFire += this.ParentWidgetEventFired;
			base.OnConnectedToRoot();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006365 File Offset: 0x00004565
		protected override void OnDisconnectedFromRoot()
		{
			base.ParentWidget.EventFire -= this.ParentWidgetEventFired;
			base.OnDisconnectedFromRoot();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006384 File Offset: 0x00004584
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
				}
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000063C1 File Offset: 0x000045C1
		protected override bool OnPreviewMousePressed()
		{
			return false;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000063C4 File Offset: 0x000045C4
		protected override bool OnPreviewDragBegin()
		{
			return false;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000063C7 File Offset: 0x000045C7
		protected override bool OnPreviewDrop()
		{
			return false;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000063CA File Offset: 0x000045CA
		protected override bool OnPreviewMouseScroll()
		{
			return false;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000063CD File Offset: 0x000045CD
		protected override bool OnPreviewMouseReleased()
		{
			return false;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000063D0 File Offset: 0x000045D0
		protected override bool OnPreviewMouseMove()
		{
			return true;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000063D3 File Offset: 0x000045D3
		protected override bool OnPreviewDragHover()
		{
			return false;
		}
	}
}
