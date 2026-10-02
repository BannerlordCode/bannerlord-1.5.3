using System;
using System.Collections.Generic;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000A8 RID: 168
	public interface ISelectionOptionData : IOptionData
	{
		// Token: 0x06000F72 RID: 3954
		int GetSelectableOptionsLimit();

		// Token: 0x06000F73 RID: 3955
		IEnumerable<SelectionData> GetSelectableOptionNames();
	}
}
