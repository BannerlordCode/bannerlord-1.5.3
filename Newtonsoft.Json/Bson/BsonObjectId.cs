using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000104 RID: 260
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonObjectId
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x000357E1 File Offset: 0x000339E1
		public byte[] Value { get; }

		// Token: 0x06000D60 RID: 3424 RVA: 0x000357E9 File Offset: 0x000339E9
		public BsonObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new ArgumentException("An ObjectId must be 12 bytes", "value");
			}
			this.Value = value;
		}
	}
}
