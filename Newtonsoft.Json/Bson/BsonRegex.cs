using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010E RID: 270
	internal class BsonRegex : BsonToken
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x000365D3 File Offset: 0x000347D3
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x000365DB File Offset: 0x000347DB
		public BsonString Pattern { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x000365E4 File Offset: 0x000347E4
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x000365EC File Offset: 0x000347EC
		public BsonString Options { get; set; }

		// Token: 0x06000DA6 RID: 3494 RVA: 0x000365F5 File Offset: 0x000347F5
		public BsonRegex(string pattern, string options)
		{
			this.Pattern = new BsonString(pattern, false);
			this.Options = new BsonString(options, false);
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x00036617 File Offset: 0x00034817
		public override BsonType Type
		{
			get
			{
				return BsonType.Regex;
			}
		}
	}
}
