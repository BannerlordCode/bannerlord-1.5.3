using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010B RID: 267
	internal class BsonBoolean : BsonValue
	{
		// Token: 0x06000D99 RID: 3481 RVA: 0x00036560 File Offset: 0x00034760
		private BsonBoolean(bool value)
			: base(value, BsonType.Boolean)
		{
		}

		// Token: 0x0400043F RID: 1087
		public static readonly BsonBoolean False = new BsonBoolean(false);

		// Token: 0x04000440 RID: 1088
		public static readonly BsonBoolean True = new BsonBoolean(true);
	}
}
