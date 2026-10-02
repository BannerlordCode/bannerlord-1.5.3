using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010C RID: 268
	internal class BsonString : BsonValue
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00036587 File Offset: 0x00034787
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x0003658F File Offset: 0x0003478F
		public int ByteCount { get; set; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00036598 File Offset: 0x00034798
		public bool IncludeLength { get; }

		// Token: 0x06000D9E RID: 3486 RVA: 0x000365A0 File Offset: 0x000347A0
		public BsonString(object value, bool includeLength)
			: base(value, BsonType.String)
		{
			this.IncludeLength = includeLength;
		}
	}
}
