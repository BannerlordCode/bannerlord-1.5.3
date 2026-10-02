using System;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000181 RID: 385
	internal class EncyclopediaListItemNameComparer : EncyclopediaListItemComparerBase
	{
		// Token: 0x06001C1D RID: 7197 RVA: 0x000910C5 File Offset: 0x0008F2C5
		public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
		{
			return base.ResolveEquality(x, y);
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x000910CF File Offset: 0x0008F2CF
		public override string GetComparedValueText(EncyclopediaListItem item)
		{
			return "";
		}
	}
}
