using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000074 RID: 116
	public class DefaultNamingStrategy : NamingStrategy
	{
		// Token: 0x06000645 RID: 1605 RVA: 0x0001B0F7 File Offset: 0x000192F7
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return name;
		}
	}
}
