using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010F RID: 271
	internal class BsonProperty
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x0003661B File Offset: 0x0003481B
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x00036623 File Offset: 0x00034823
		public BsonString Name { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0003662C File Offset: 0x0003482C
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x00036634 File Offset: 0x00034834
		public BsonToken Value { get; set; }
	}
}
