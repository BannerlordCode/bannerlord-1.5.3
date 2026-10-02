using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Missions
{
	// Token: 0x020003E9 RID: 1001
	public class AgentList : AgentReadOnlyList
	{
		// Token: 0x0600378F RID: 14223 RVA: 0x000E71BA File Offset: 0x000E53BA
		public AgentList(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x000E71C3 File Offset: 0x000E53C3
		public AgentList(IEnumerable<Agent> collection)
			: base(collection)
		{
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x000E71CC File Offset: 0x000E53CC
		public AgentList(List<Agent> collection)
			: base(collection)
		{
		}
	}
}
