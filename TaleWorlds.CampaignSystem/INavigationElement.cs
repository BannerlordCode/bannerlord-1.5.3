using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200009B RID: 155
	public interface INavigationElement
	{
		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001329 RID: 4905
		string StringId { get; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x0600132A RID: 4906
		NavigationPermissionItem Permission { get; }

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x0600132B RID: 4907
		bool IsLockingNavigation { get; }

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x0600132C RID: 4908
		bool IsActive { get; }

		// Token: 0x0600132D RID: 4909
		void OpenView();

		// Token: 0x0600132E RID: 4910
		void OpenView(params object[] parameters);

		// Token: 0x0600132F RID: 4911
		void GoToLink();

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001330 RID: 4912
		TextObject Tooltip { get; }

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001331 RID: 4913
		bool HasAlert { get; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001332 RID: 4914
		TextObject AlertTooltip { get; }
	}
}
