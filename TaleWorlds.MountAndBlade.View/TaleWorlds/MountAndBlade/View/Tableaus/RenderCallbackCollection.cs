using System;
using System.Collections.Generic;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000037 RID: 55
	public struct RenderCallbackCollection
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000E556 File Offset: 0x0000C756
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x0000E55E File Offset: 0x0000C75E
		public List<Action<Texture>> SetActions { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000E567 File Offset: 0x0000C767
		// (set) Token: 0x060001FA RID: 506 RVA: 0x0000E56F File Offset: 0x0000C76F
		public List<Action> CancelActions { get; private set; }

		// Token: 0x060001FB RID: 507 RVA: 0x0000E578 File Offset: 0x0000C778
		public static RenderCallbackCollection CreateEmpty()
		{
			return new RenderCallbackCollection
			{
				SetActions = new List<Action<Texture>>(),
				CancelActions = new List<Action>()
			};
		}
	}
}
