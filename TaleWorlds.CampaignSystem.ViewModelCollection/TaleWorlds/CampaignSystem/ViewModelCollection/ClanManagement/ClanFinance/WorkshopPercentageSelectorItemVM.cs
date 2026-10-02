using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance
{
	// Token: 0x0200013C RID: 316
	public class WorkshopPercentageSelectorItemVM : SelectorItemVM
	{
		// Token: 0x06001E24 RID: 7716 RVA: 0x0006CEF9 File Offset: 0x0006B0F9
		public WorkshopPercentageSelectorItemVM(string s, float percentage)
			: base(s)
		{
			this.Percentage = percentage;
		}

		// Token: 0x04000DC9 RID: 3529
		public readonly float Percentage;
	}
}
