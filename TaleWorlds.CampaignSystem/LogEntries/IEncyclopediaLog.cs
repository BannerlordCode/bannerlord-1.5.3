using System;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x0200036A RID: 874
	public interface IEncyclopediaLog
	{
		// Token: 0x06003407 RID: 13319
		bool IsVisibleInEncyclopediaPageOf(MBObjectBase obj);

		// Token: 0x06003408 RID: 13320
		TextObject GetEncyclopediaText();

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06003409 RID: 13321
		CampaignTime GameTime { get; }
	}
}
