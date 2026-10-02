using System;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000C6 RID: 198
	public interface IReadOnlyPropertyOwner<T> where T : MBObjectBase
	{
		// Token: 0x06000AE0 RID: 2784
		int GetPropertyValue(T attribute);

		// Token: 0x06000AE1 RID: 2785
		bool HasProperty(T attribute);
	}
}
