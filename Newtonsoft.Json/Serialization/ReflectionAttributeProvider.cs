using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200009D RID: 157
	[NullableContext(1)]
	[Nullable(0)]
	public class ReflectionAttributeProvider : IAttributeProvider
	{
		// Token: 0x06000822 RID: 2082 RVA: 0x000235F9 File Offset: 0x000217F9
		public ReflectionAttributeProvider(object attributeProvider)
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			this._attributeProvider = attributeProvider;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00023613 File Offset: 0x00021813
		public IList<Attribute> GetAttributes(bool inherit)
		{
			return ReflectionUtils.GetAttributes(this._attributeProvider, null, inherit);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00023622 File Offset: 0x00021822
		public IList<Attribute> GetAttributes(Type attributeType, bool inherit)
		{
			return ReflectionUtils.GetAttributes(this._attributeProvider, attributeType, inherit);
		}

		// Token: 0x040002DF RID: 735
		private readonly object _attributeProvider;
	}
}
