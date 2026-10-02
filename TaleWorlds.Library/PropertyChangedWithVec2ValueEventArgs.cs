using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000050 RID: 80
	public struct PropertyChangedWithVec2ValueEventArgs
	{
		// Token: 0x06000278 RID: 632 RVA: 0x00007DBB File Offset: 0x00005FBB
		public PropertyChangedWithVec2ValueEventArgs(string propertyName, Vec2 value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00007DCB File Offset: 0x00005FCB
		public string PropertyName { get; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00007DD3 File Offset: 0x00005FD3
		public Vec2 Value { get; }
	}
}
