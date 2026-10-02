using System;
using System.Collections.Generic;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x0200000B RID: 11
	public class MPEscapeMenuVM : EscapeMenuVM
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00003B86 File Offset: 0x00001D86
		public MPEscapeMenuVM(IEnumerable<EscapeMenuItemVM> items, TextObject title = null)
			: base(items, title)
		{
		}
	}
}
