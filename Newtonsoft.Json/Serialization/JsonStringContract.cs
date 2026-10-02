using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000096 RID: 150
	public class JsonStringContract : JsonPrimitiveContract
	{
		// Token: 0x060007EF RID: 2031 RVA: 0x00022DD1 File Offset: 0x00020FD1
		[NullableContext(1)]
		public JsonStringContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.String;
		}
	}
}
