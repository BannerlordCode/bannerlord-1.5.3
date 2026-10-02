using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x0200006E RID: 110
	public interface IAdminPanelOption<T> : IAdminPanelOption
	{
		// Token: 0x06000367 RID: 871
		T GetValue();

		// Token: 0x06000368 RID: 872
		void SetValue(T value);
	}
}
