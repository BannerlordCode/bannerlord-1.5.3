using System;

namespace System.Management
{
	// Token: 0x02000005 RID: 5
	public enum CimType
	{
		// Token: 0x0400000C RID: 12
		None,
		// Token: 0x0400000D RID: 13
		SInt16 = 2,
		// Token: 0x0400000E RID: 14
		SInt32,
		// Token: 0x0400000F RID: 15
		Real32,
		// Token: 0x04000010 RID: 16
		Real64,
		// Token: 0x04000011 RID: 17
		String = 8,
		// Token: 0x04000012 RID: 18
		Boolean = 11,
		// Token: 0x04000013 RID: 19
		Object = 13,
		// Token: 0x04000014 RID: 20
		SInt8 = 16,
		// Token: 0x04000015 RID: 21
		UInt8,
		// Token: 0x04000016 RID: 22
		UInt16,
		// Token: 0x04000017 RID: 23
		UInt32,
		// Token: 0x04000018 RID: 24
		SInt64,
		// Token: 0x04000019 RID: 25
		UInt64,
		// Token: 0x0400001A RID: 26
		DateTime = 101,
		// Token: 0x0400001B RID: 27
		Reference,
		// Token: 0x0400001C RID: 28
		Char16
	}
}
