using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000080 RID: 128
	[NullableContext(1)]
	public interface IValueProvider
	{
		// Token: 0x06000673 RID: 1651
		void SetValue(object target, [Nullable(2)] object value);

		// Token: 0x06000674 RID: 1652
		[return: Nullable(2)]
		object GetValue(object target);
	}
}
