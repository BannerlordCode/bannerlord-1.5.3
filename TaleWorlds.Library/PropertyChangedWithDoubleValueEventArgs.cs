using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200004F RID: 79
	public struct PropertyChangedWithDoubleValueEventArgs
	{
		// Token: 0x06000275 RID: 629 RVA: 0x00007D9B File Offset: 0x00005F9B
		public PropertyChangedWithDoubleValueEventArgs(string propertyName, double value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00007DAB File Offset: 0x00005FAB
		public string PropertyName { get; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00007DB3 File Offset: 0x00005FB3
		public double Value { get; }
	}
}
