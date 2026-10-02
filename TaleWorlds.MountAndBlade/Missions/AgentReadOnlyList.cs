using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Missions
{
	// Token: 0x020003EA RID: 1002
	public class AgentReadOnlyList : MBReadOnlyList<Agent>
	{
		// Token: 0x06003792 RID: 14226 RVA: 0x000E71D5 File Offset: 0x000E53D5
		public AgentReadOnlyList(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x000E71DE File Offset: 0x000E53DE
		public AgentReadOnlyList(IEnumerable<Agent> collection)
			: base(collection)
		{
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x000E71E7 File Offset: 0x000E53E7
		public AgentReadOnlyList(List<Agent> collection)
			: base(collection)
		{
		}
	}
}
