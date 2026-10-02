using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000049 RID: 73
	public struct PropertyChangedWithValueEventArgs
	{
		// Token: 0x06000263 RID: 611 RVA: 0x00007CDB File Offset: 0x00005EDB
		public PropertyChangedWithValueEventArgs(string propertyName, object value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00007CEB File Offset: 0x00005EEB
		public string PropertyName { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00007CF3 File Offset: 0x00005EF3
		public object Value { get; }
	}
}
