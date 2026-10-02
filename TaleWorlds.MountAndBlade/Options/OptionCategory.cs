using System;
using System.Collections.Generic;
using TaleWorlds.Engine.Options;

namespace TaleWorlds.MountAndBlade.Options
{
	// Token: 0x020003A0 RID: 928
	public class OptionCategory
	{
		// Token: 0x06003566 RID: 13670 RVA: 0x000DD20F File Offset: 0x000DB40F
		public OptionCategory(IEnumerable<IOptionData> baseOptions, IEnumerable<OptionGroup> groups)
		{
			this.BaseOptions = baseOptions;
			this.Groups = groups;
		}

		// Token: 0x040016CC RID: 5836
		public readonly IEnumerable<IOptionData> BaseOptions;

		// Token: 0x040016CD RID: 5837
		public readonly IEnumerable<OptionGroup> Groups;
	}
}
