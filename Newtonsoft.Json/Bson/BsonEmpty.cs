using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000109 RID: 265
	internal class BsonEmpty : BsonToken
	{
		// Token: 0x06000D93 RID: 3475 RVA: 0x0003650A File Offset: 0x0003470A
		private BsonEmpty(BsonType type)
		{
			this.Type = type;
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x00036519 File Offset: 0x00034719
		public override BsonType Type { get; }

		// Token: 0x0400043A RID: 1082
		public static readonly BsonToken Null = new BsonEmpty(BsonType.Null);

		// Token: 0x0400043B RID: 1083
		public static readonly BsonToken Undefined = new BsonEmpty(BsonType.Undefined);
	}
}
