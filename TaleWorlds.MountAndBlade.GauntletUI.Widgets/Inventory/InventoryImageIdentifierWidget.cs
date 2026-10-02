using System;
using TaleWorlds.GauntletUI;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000140 RID: 320
	public class InventoryImageIdentifierWidget : ImageIdentifierWidget
	{
		// Token: 0x060010B4 RID: 4276 RVA: 0x0002DDF3 File Offset: 0x0002BFF3
		public InventoryImageIdentifierWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0002DDFC File Offset: 0x0002BFFC
		public void SetRenderRequestedPreviousFrame(bool isRequested)
		{
			this._isRenderRequestedPreviousFrame = isRequested && base.IsRecursivelyVisible() && base.EventManager.AreaRectangle.IsCollide(in this.AreaRect);
		}
	}
}
