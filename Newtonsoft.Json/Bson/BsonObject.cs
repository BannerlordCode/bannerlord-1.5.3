using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000107 RID: 263
	internal class BsonObject : BsonToken, IEnumerable<BsonProperty>, IEnumerable
	{
		// Token: 0x06000D89 RID: 3465 RVA: 0x00036468 File Offset: 0x00034668
		public void Add(string name, BsonToken token)
		{
			this._children.Add(new BsonProperty
			{
				Name = new BsonString(name, false),
				Value = token
			});
			token.Parent = this;
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00036495 File Offset: 0x00034695
		public override BsonType Type
		{
			get
			{
				return BsonType.Object;
			}
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00036498 File Offset: 0x00034698
		public IEnumerator<BsonProperty> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000364AA File Offset: 0x000346AA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000438 RID: 1080
		private readonly List<BsonProperty> _children = new List<BsonProperty>();
	}
}
