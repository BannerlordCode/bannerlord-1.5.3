using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013D RID: 317
	public class InventoryCenterPanelWidget : Widget
	{
		// Token: 0x06001096 RID: 4246 RVA: 0x0002D950 File Offset: 0x0002BB50
		public InventoryCenterPanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0002D959 File Offset: 0x0002BB59
		protected override bool OnPreviewDragBegin()
		{
			return false;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0002D95C File Offset: 0x0002BB5C
		protected override bool OnPreviewDrop()
		{
			return true;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0002D95F File Offset: 0x0002BB5F
		protected override bool OnPreviewDragHover()
		{
			return false;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0002D962 File Offset: 0x0002BB62
		protected override bool OnPreviewMouseMove()
		{
			return false;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0002D965 File Offset: 0x0002BB65
		protected override bool OnPreviewMousePressed()
		{
			return false;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0002D968 File Offset: 0x0002BB68
		protected override bool OnPreviewMouseReleased()
		{
			return false;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0002D96B File Offset: 0x0002BB6B
		protected override bool OnPreviewMouseScroll()
		{
			return false;
		}
	}
}
