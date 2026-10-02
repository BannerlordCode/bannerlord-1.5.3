using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000059 RID: 89
	public interface IEntityComponent
	{
		// Token: 0x06000702 RID: 1794
		void OnInitialize();

		// Token: 0x06000703 RID: 1795
		void OnFinalize();
	}
}
