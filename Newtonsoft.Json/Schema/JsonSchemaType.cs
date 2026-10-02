using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AE RID: 174
	[Flags]
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public enum JsonSchemaType
	{
		// Token: 0x0400035E RID: 862
		None = 0,
		// Token: 0x0400035F RID: 863
		String = 1,
		// Token: 0x04000360 RID: 864
		Float = 2,
		// Token: 0x04000361 RID: 865
		Integer = 4,
		// Token: 0x04000362 RID: 866
		Boolean = 8,
		// Token: 0x04000363 RID: 867
		Object = 16,
		// Token: 0x04000364 RID: 868
		Array = 32,
		// Token: 0x04000365 RID: 869
		Null = 64,
		// Token: 0x04000366 RID: 870
		Any = 127
	}
}
