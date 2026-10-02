using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006C RID: 108
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct StructMultiKey<[Nullable(2)] T1, [Nullable(2)] T2> : IEquatable<StructMultiKey<T1, T2>>
	{
		// Token: 0x060005E1 RID: 1505 RVA: 0x00018EB9 File Offset: 0x000170B9
		public StructMultiKey(T1 v1, T2 v2)
		{
			this.Value1 = v1;
			this.Value2 = v2;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00018ECC File Offset: 0x000170CC
		public override int GetHashCode()
		{
			T1 value = this.Value1;
			int num = ((value != null) ? value.GetHashCode() : 0);
			T2 value2 = this.Value2;
			return num ^ ((value2 != null) ? value2.GetHashCode() : 0);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00018F24 File Offset: 0x00017124
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is StructMultiKey<T1, T2>)
			{
				StructMultiKey<T1, T2> structMultiKey = (StructMultiKey<T1, T2>)obj;
				return this.Equals(structMultiKey);
			}
			return false;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00018F4B File Offset: 0x0001714B
		public bool Equals([Nullable(new byte[] { 0, 1, 1 })] StructMultiKey<T1, T2> other)
		{
			return object.Equals(this.Value1, other.Value1) && object.Equals(this.Value2, other.Value2);
		}

		// Token: 0x04000225 RID: 549
		public readonly T1 Value1;

		// Token: 0x04000226 RID: 550
		public readonly T2 Value2;
	}
}
