using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x02000107 RID: 263
	public class AutoClosePopupClosingWidget : Widget
	{
		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x00026C8F File Offset: 0x00024E8F
		// (set) Token: 0x06000E1B RID: 3611 RVA: 0x00026C97 File Offset: 0x00024E97
		public Widget Target { get; set; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x00026CA0 File Offset: 0x00024EA0
		// (set) Token: 0x06000E1D RID: 3613 RVA: 0x00026CA8 File Offset: 0x00024EA8
		public bool IncludeChildren { get; set; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x00026CB1 File Offset: 0x00024EB1
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x00026CB9 File Offset: 0x00024EB9
		public bool IncludeTarget { get; set; }

		// Token: 0x06000E20 RID: 3616 RVA: 0x00026CC2 File Offset: 0x00024EC2
		public AutoClosePopupClosingWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00026CCC File Offset: 0x00024ECC
		public bool ShouldClosePopup()
		{
			if (this.IncludeTarget && base.EventManager.LatestMouseUpWidget == this.Target)
			{
				return true;
			}
			if (this.IncludeChildren)
			{
				Widget target = this.Target;
				return target != null && target.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget);
			}
			return false;
		}
	}
}
