using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.AdminMessage
{
	// Token: 0x020000D3 RID: 211
	public class MultiplayerAdminMessageItemWidget : Widget
	{
		// Token: 0x06000AEE RID: 2798 RVA: 0x0001EAB0 File Offset: 0x0001CCB0
		public MultiplayerAdminMessageItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0001EAB9 File Offset: 0x0001CCB9
		public void Remove()
		{
			base.EventFired("Remove", Array.Empty<object>());
		}
	}
}
