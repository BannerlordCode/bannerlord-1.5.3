using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010D RID: 269
	internal class BsonBinary : BsonValue
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x000365B1 File Offset: 0x000347B1
		// (set) Token: 0x06000DA0 RID: 3488 RVA: 0x000365B9 File Offset: 0x000347B9
		public BsonBinaryType BinaryType { get; set; }

		// Token: 0x06000DA1 RID: 3489 RVA: 0x000365C2 File Offset: 0x000347C2
		public BsonBinary(byte[] value, BsonBinaryType binaryType)
			: base(value, BsonType.Binary)
		{
			this.BinaryType = binaryType;
		}
	}
}
