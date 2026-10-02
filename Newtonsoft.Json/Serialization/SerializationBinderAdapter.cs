using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200009F RID: 159
	[NullableContext(1)]
	[Nullable(0)]
	internal class SerializationBinderAdapter : ISerializationBinder
	{
		// Token: 0x06000828 RID: 2088 RVA: 0x00023728 File Offset: 0x00021928
		public SerializationBinderAdapter(SerializationBinder serializationBinder)
		{
			this.SerializationBinder = serializationBinder;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00023737 File Offset: 0x00021937
		public Type BindToType([Nullable(2)] string assemblyName, string typeName)
		{
			return this.SerializationBinder.BindToType(assemblyName, typeName);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00023746 File Offset: 0x00021946
		[NullableContext(2)]
		public void BindToName([Nullable(1)] Type serializedType, out string assemblyName, out string typeName)
		{
			this.SerializationBinder.BindToName(serializedType, out assemblyName, out typeName);
		}

		// Token: 0x040002E1 RID: 737
		public readonly SerializationBinder SerializationBinder;
	}
}
