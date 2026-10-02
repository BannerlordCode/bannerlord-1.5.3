using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008D RID: 141
	public class JsonLinqContract : JsonContract
	{
		// Token: 0x060006E9 RID: 1769 RVA: 0x0001C9AE File Offset: 0x0001ABAE
		[NullableContext(1)]
		public JsonLinqContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Linq;
		}
	}
}
