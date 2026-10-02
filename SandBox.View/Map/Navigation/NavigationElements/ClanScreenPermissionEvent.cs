using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x0200006E RID: 110
	public class ClanScreenPermissionEvent : EventBase
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00025A32 File Offset: 0x00023C32
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x00025A3A File Offset: 0x00023C3A
		public Action<bool, TextObject> IsClanScreenAvailable { get; private set; }

		// Token: 0x060004C2 RID: 1218 RVA: 0x00025A43 File Offset: 0x00023C43
		public ClanScreenPermissionEvent(Action<bool, TextObject> isClanScreenAvailable)
		{
			this.IsClanScreenAvailable = isClanScreenAvailable;
		}
	}
}
