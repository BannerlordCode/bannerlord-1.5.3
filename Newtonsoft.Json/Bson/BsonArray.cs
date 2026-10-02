using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000108 RID: 264
	internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
	{
		// Token: 0x06000D8E RID: 3470 RVA: 0x000364C5 File Offset: 0x000346C5
		public void Add(BsonToken token)
		{
			this._children.Add(token);
			token.Parent = this;
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x000364DA File Offset: 0x000346DA
		public override BsonType Type
		{
			get
			{
				return BsonType.Array;
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x000364DD File Offset: 0x000346DD
		public IEnumerator<BsonToken> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x000364EF File Offset: 0x000346EF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000439 RID: 1081
		private readonly List<BsonToken> _children = new List<BsonToken>();
	}
}
