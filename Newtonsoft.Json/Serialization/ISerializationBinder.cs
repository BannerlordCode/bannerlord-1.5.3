using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007E RID: 126
	[NullableContext(1)]
	public interface ISerializationBinder
	{
		// Token: 0x0600066F RID: 1647
		Type BindToType([Nullable(2)] string assemblyName, string typeName);

		// Token: 0x06000670 RID: 1648
		[NullableContext(2)]
		void BindToName([Nullable(1)] Type serializedType, out string assemblyName, out string typeName);
	}
}
