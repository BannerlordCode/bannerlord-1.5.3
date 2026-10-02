using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200009A RID: 154
	public interface INavigationHandler
	{
		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001324 RID: 4900
		// (set) Token: 0x06001325 RID: 4901
		bool IsNavigationLocked { get; set; }

		// Token: 0x06001326 RID: 4902
		INavigationElement[] GetElements();

		// Token: 0x06001327 RID: 4903
		INavigationElement GetElement(string id);

		// Token: 0x06001328 RID: 4904
		bool IsAnyElementActive();
	}
}
