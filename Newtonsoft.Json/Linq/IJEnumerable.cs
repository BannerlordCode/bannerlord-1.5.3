using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B6 RID: 182
	[NullableContext(1)]
	public interface IJEnumerable<[Nullable(0)] out T> : IEnumerable<T>, IEnumerable where T : JToken
	{
		// Token: 0x170001B9 RID: 441
		IJEnumerable<JToken> this[object key] { get; }
	}
}
