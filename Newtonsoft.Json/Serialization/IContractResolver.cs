using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007C RID: 124
	[NullableContext(1)]
	public interface IContractResolver
	{
		// Token: 0x0600066A RID: 1642
		JsonContract ResolveContract(Type type);
	}
}
