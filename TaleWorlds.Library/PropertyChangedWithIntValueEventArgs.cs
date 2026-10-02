using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200004B RID: 75
	public struct PropertyChangedWithIntValueEventArgs
	{
		// Token: 0x06000269 RID: 617 RVA: 0x00007D1B File Offset: 0x00005F1B
		public PropertyChangedWithIntValueEventArgs(string propertyName, int value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600026A RID: 618 RVA: 0x00007D2B File Offset: 0x00005F2B
		public string PropertyName { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00007D33 File Offset: 0x00005F33
		public int Value { get; }
	}
}
