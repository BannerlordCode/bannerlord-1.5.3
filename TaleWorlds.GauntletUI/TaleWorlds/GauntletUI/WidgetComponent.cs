using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000039 RID: 57
	public abstract class WidgetComponent
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000FE2D File Offset: 0x0000E02D
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x0000FE35 File Offset: 0x0000E035
		public Widget Target { get; private set; }

		// Token: 0x060003EA RID: 1002 RVA: 0x0000FE3E File Offset: 0x0000E03E
		protected WidgetComponent(Widget target)
		{
			this.Target = target;
		}
	}
}
