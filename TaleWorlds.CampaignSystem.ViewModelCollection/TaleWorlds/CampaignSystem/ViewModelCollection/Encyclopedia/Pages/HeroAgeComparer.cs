using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D5 RID: 213
	public class HeroAgeComparer : IComparer<HeroVM>
	{
		// Token: 0x060013F6 RID: 5110 RVA: 0x0005066A File Offset: 0x0004E86A
		public HeroAgeComparer(bool isAscending)
		{
			this._isAscending = isAscending;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0005067C File Offset: 0x0004E87C
		int IComparer<HeroVM>.Compare(HeroVM x, HeroVM y)
		{
			int num = x.Hero.Age.CompareTo(y.Hero.Age) * (this._isAscending ? 1 : (-1));
			if (num == 0)
			{
				num = x.NameText.CompareTo(y.NameText);
			}
			return num;
		}

		// Token: 0x04000917 RID: 2327
		private readonly bool _isAscending;
	}
}
