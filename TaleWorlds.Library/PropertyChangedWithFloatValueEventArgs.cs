using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200004C RID: 76
	public struct PropertyChangedWithFloatValueEventArgs
	{
		// Token: 0x0600026C RID: 620 RVA: 0x00007D3B File Offset: 0x00005F3B
		public PropertyChangedWithFloatValueEventArgs(string propertyName, float value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00007D4B File Offset: 0x00005F4B
		public string PropertyName { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00007D53 File Offset: 0x00005F53
		public float Value { get; }
	}
}
