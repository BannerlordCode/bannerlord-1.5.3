using System;
using TaleWorlds.GauntletUI;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000066 RID: 102
	public class PartyListPanel : NavigatableListPanel
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x00010875 File Offset: 0x0000EA75
		public PartyListPanel(UIContext context)
			: base(context)
		{
			base.ClearSelectedOnRemoval = true;
		}
	}
}
