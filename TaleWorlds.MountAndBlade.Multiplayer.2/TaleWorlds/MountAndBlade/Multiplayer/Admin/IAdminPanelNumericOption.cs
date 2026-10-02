using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x0200006F RID: 111
	public interface IAdminPanelNumericOption : IAdminPanelOption<int>, IAdminPanelOption
	{
		// Token: 0x06000369 RID: 873
		int? GetMinimumValue();

		// Token: 0x0600036A RID: 874
		int? GetMaximumValue();
	}
}
