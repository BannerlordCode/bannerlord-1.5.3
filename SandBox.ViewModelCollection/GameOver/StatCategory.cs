using System;
using System.Collections.Generic;

namespace SandBox.ViewModelCollection.GameOver
{
	// Token: 0x0200005B RID: 91
	public class StatCategory
	{
		// Token: 0x060005AE RID: 1454 RVA: 0x0001564A File Offset: 0x0001384A
		public StatCategory(string id, IEnumerable<StatItem> items)
		{
			this.ID = id;
			this.Items = items;
		}

		// Token: 0x040002D6 RID: 726
		public readonly IEnumerable<StatItem> Items;

		// Token: 0x040002D7 RID: 727
		public readonly string ID;
	}
}
