using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010A RID: 266
	internal class BsonValue : BsonToken
	{
		// Token: 0x06000D96 RID: 3478 RVA: 0x0003653A File Offset: 0x0003473A
		public BsonValue(object value, BsonType type)
		{
			this._value = value;
			this._type = type;
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00036550 File Offset: 0x00034750
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x00036558 File Offset: 0x00034758
		public override BsonType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x0400043D RID: 1085
		private readonly object _value;

		// Token: 0x0400043E RID: 1086
		private readonly BsonType _type;
	}
}
