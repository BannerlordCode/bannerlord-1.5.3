using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000061 RID: 97
	[Serializable]
	public struct ManagedArray
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x00008800 File Offset: 0x00006A00
		public ManagedArray(IntPtr array, int length)
		{
			this.Array = array;
			this.Length = length;
		}

		// Token: 0x04000122 RID: 290
		internal IntPtr Array;

		// Token: 0x04000123 RID: 291
		internal int Length;
	}
}
