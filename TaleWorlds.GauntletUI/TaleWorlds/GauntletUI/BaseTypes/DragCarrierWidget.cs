using System;
using TaleWorlds.GauntletUI.Layout;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000056 RID: 86
	public class DragCarrierWidget : Widget
	{
		// Token: 0x060005C4 RID: 1476 RVA: 0x000180A3 File Offset: 0x000162A3
		public DragCarrierWidget(UIContext context)
			: base(context)
		{
			base.LayoutImp = new DragCarrierLayout();
			base.DoNotAcceptEvents = true;
			base.DoNotPassEventsToChildren = true;
			base.IsDisabled = true;
		}
	}
}
