using System;

namespace System.Numerics.Hashing
{
	// Token: 0x02000012 RID: 18
	internal static class HashHelpers
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x0001E2FC File Offset: 0x0001C4FC
		public static int Combine(int h1, int h2)
		{
			uint num = (uint)((h1 << 5) | (int)((uint)h1 >> 27));
			return (int)((num + (uint)h1) ^ (uint)h2);
		}

		// Token: 0x04000072 RID: 114
		public static readonly int RandomSeed = Guid.NewGuid().GetHashCode();
	}
}
