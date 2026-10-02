using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008C RID: 140
	public class JsonISerializableContract : JsonContainerContract
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001C98D File Offset: 0x0001AB8D
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x0001C995 File Offset: 0x0001AB95
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> ISerializableCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001C99E File Offset: 0x0001AB9E
		[NullableContext(1)]
		public JsonISerializableContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Serializable;
		}
	}
}
