using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000DA RID: 218
	public class HeroRelationComparer : IComparer<HeroVM>
	{
		// Token: 0x060014B2 RID: 5298 RVA: 0x00052C2A File Offset: 0x00050E2A
		public HeroRelationComparer(Hero pageHero, bool isAscending, bool showLeadersFirst)
		{
			this._pageHero = pageHero;
			this._isAscending = isAscending;
			this._showLeadersFirst = showLeadersFirst;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00052C48 File Offset: 0x00050E48
		int IComparer<HeroVM>.Compare(HeroVM x, HeroVM y)
		{
			int num;
			if (this._showLeadersFirst)
			{
				num = y.IsKingdomLeader.CompareTo(x.IsKingdomLeader);
				if (num != 0)
				{
					return num;
				}
			}
			int relation = this._pageHero.GetRelation(x.Hero);
			int relation2 = this._pageHero.GetRelation(y.Hero);
			num = relation.CompareTo(relation2) * (this._isAscending ? 1 : (-1));
			if (num == 0)
			{
				num = x.NameText.CompareTo(y.NameText);
			}
			return num;
		}

		// Token: 0x0400096F RID: 2415
		private readonly Hero _pageHero;

		// Token: 0x04000970 RID: 2416
		private readonly bool _isAscending;

		// Token: 0x04000971 RID: 2417
		private readonly bool _showLeadersFirst;
	}
}
