using System;
using System.Collections.Generic;
using TaleWorlds.Engine.Options;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Options
{
	// Token: 0x020003A1 RID: 929
	public class OptionGroup
	{
		// Token: 0x06003567 RID: 13671 RVA: 0x000DD225 File Offset: 0x000DB425
		public OptionGroup(TextObject groupName, IEnumerable<IOptionData> options)
		{
			this.GroupName = groupName;
			this.Options = options;
		}

		// Token: 0x040016CE RID: 5838
		public readonly TextObject GroupName;

		// Token: 0x040016CF RID: 5839
		public readonly IEnumerable<IOptionData> Options;
	}
}
