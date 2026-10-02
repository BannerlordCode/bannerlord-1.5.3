using System;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000C7 RID: 199
	public interface IReadOnlyPropertyOwnerF<T> where T : MBObjectBase
	{
		// Token: 0x06000AE2 RID: 2786
		float GetPropertyValue(T attribute);

		// Token: 0x06000AE3 RID: 2787
		bool HasProperty(T attribute);
	}
}
