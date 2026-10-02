using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x0200017D RID: 381
	public class EncyclopediaFilterItem
	{
		// Token: 0x06001BFB RID: 7163 RVA: 0x000909D8 File Offset: 0x0008EBD8
		public EncyclopediaFilterItem(TextObject name, Predicate<object> predicate)
		{
			this.Name = name;
			this.Predicate = predicate;
			this.IsActive = false;
		}

		// Token: 0x0400094E RID: 2382
		public readonly TextObject Name;

		// Token: 0x0400094F RID: 2383
		public readonly Predicate<object> Predicate;

		// Token: 0x04000950 RID: 2384
		public bool IsActive;
	}
}
