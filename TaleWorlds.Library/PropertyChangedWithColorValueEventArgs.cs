using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200004E RID: 78
	public struct PropertyChangedWithColorValueEventArgs
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00007D7B File Offset: 0x00005F7B
		public PropertyChangedWithColorValueEventArgs(string propertyName, Color value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00007D8B File Offset: 0x00005F8B
		public string PropertyName { get; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000274 RID: 628 RVA: 0x00007D93 File Offset: 0x00005F93
		public Color Value { get; }
	}
}
