using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200004D RID: 77
	public struct PropertyChangedWithUIntValueEventArgs
	{
		// Token: 0x0600026F RID: 623 RVA: 0x00007D5B File Offset: 0x00005F5B
		public PropertyChangedWithUIntValueEventArgs(string propertyName, uint value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000270 RID: 624 RVA: 0x00007D6B File Offset: 0x00005F6B
		public string PropertyName { get; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00007D73 File Offset: 0x00005F73
		public uint Value { get; }
	}
}
