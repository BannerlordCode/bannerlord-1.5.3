using System;
using System.Numerics;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000051 RID: 81
	public class BasicContainer : Container
	{
		// Token: 0x0600057B RID: 1403 RVA: 0x0001723E File Offset: 0x0001543E
		public BasicContainer(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00017247 File Offset: 0x00015447
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x0001724F File Offset: 0x0001544F
		public override Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x0600057E RID: 1406 RVA: 0x00017258 File Offset: 0x00015458
		public override Vector2 GetDropGizmoPosition(Vector2 draggedWidgetPosition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001725F File Offset: 0x0001545F
		public override int GetIndexForDrop(Vector2 draggedWidgetPosition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00017266 File Offset: 0x00015466
		public override bool IsDragHovering { get; }

		// Token: 0x06000581 RID: 1409 RVA: 0x00017270 File Offset: 0x00015470
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
	}
}
