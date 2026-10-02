using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000183 RID: 387
	public class EncyclopediaSortController
	{
		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001C29 RID: 7209 RVA: 0x0009114F File Offset: 0x0008F34F
		public TextObject Name { get; }

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001C2A RID: 7210 RVA: 0x00091157 File Offset: 0x0008F357
		public EncyclopediaListItemComparerBase Comparer { get; }

		// Token: 0x06001C2B RID: 7211 RVA: 0x0009115F File Offset: 0x0008F35F
		public EncyclopediaSortController(TextObject name, EncyclopediaListItemComparerBase comparer)
		{
			this.Name = name;
			this.Comparer = comparer;
		}
	}
}
