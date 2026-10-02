using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000070 RID: 112
	public interface IAdminPanelMultiSelectionOption : IAdminPanelOption<IAdminPanelMultiSelectionItem>, IAdminPanelOption
	{
		// Token: 0x0600036B RID: 875
		MBReadOnlyList<IAdminPanelMultiSelectionItem> GetAvailableOptions();
	}
}
