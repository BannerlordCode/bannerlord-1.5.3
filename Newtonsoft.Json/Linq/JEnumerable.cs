using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BA RID: 186
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct JEnumerable<[Nullable(0)] T> : IJEnumerable<T>, IEnumerable<T>, IEnumerable, IEquatable<JEnumerable<T>> where T : JToken
	{
		// Token: 0x06000A13 RID: 2579 RVA: 0x0002916C File Offset: 0x0002736C
		public JEnumerable(IEnumerable<T> enumerable)
		{
			ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			this._enumerable = enumerable;
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00029180 File Offset: 0x00027380
		public IEnumerator<T> GetEnumerator()
		{
			return (this._enumerable ?? JEnumerable<T>.Empty).GetEnumerator();
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0002919B File Offset: 0x0002739B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x170001D8 RID: 472
		public IJEnumerable<JToken> this[object key]
		{
			get
			{
				if (this._enumerable == null)
				{
					return JEnumerable<JToken>.Empty;
				}
				return new JEnumerable<JToken>(this._enumerable.Values<T, JToken>(key));
			}
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x000291CE File Offset: 0x000273CE
		public bool Equals([Nullable(new byte[] { 0, 1 })] JEnumerable<T> other)
		{
			return object.Equals(this._enumerable, other._enumerable);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x000291E4 File Offset: 0x000273E4
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is JEnumerable<T>)
			{
				JEnumerable<T> jenumerable = (JEnumerable<T>)obj;
				return this.Equals(jenumerable);
			}
			return false;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00029209 File Offset: 0x00027409
		public override int GetHashCode()
		{
			if (this._enumerable == null)
			{
				return 0;
			}
			return this._enumerable.GetHashCode();
		}

		// Token: 0x0400037D RID: 893
		[Nullable(new byte[] { 0, 1 })]
		public static readonly JEnumerable<T> Empty = new JEnumerable<T>(Enumerable.Empty<T>());

		// Token: 0x0400037E RID: 894
		private readonly IEnumerable<T> _enumerable;
	}
}
