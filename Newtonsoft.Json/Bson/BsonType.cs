using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000110 RID: 272
	internal enum BsonType : sbyte
	{
		// Token: 0x04000449 RID: 1097
		Number = 1,
		// Token: 0x0400044A RID: 1098
		String,
		// Token: 0x0400044B RID: 1099
		Object,
		// Token: 0x0400044C RID: 1100
		Array,
		// Token: 0x0400044D RID: 1101
		Binary,
		// Token: 0x0400044E RID: 1102
		Undefined,
		// Token: 0x0400044F RID: 1103
		Oid,
		// Token: 0x04000450 RID: 1104
		Boolean,
		// Token: 0x04000451 RID: 1105
		Date,
		// Token: 0x04000452 RID: 1106
		Null,
		// Token: 0x04000453 RID: 1107
		Regex,
		// Token: 0x04000454 RID: 1108
		Reference,
		// Token: 0x04000455 RID: 1109
		Code,
		// Token: 0x04000456 RID: 1110
		Symbol,
		// Token: 0x04000457 RID: 1111
		CodeWScope,
		// Token: 0x04000458 RID: 1112
		Integer,
		// Token: 0x04000459 RID: 1113
		TimeStamp,
		// Token: 0x0400045A RID: 1114
		Long,
		// Token: 0x0400045B RID: 1115
		MinKey = -1,
		// Token: 0x0400045C RID: 1116
		MaxKey = 127
	}
}
