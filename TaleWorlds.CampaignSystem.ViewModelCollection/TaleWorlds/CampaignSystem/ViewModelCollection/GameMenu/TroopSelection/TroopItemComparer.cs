using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TroopSelection
{
	// Token: 0x020000A7 RID: 167
	public class TroopItemComparer : IComparer<TroopSelectionItemVM>
	{
		// Token: 0x06000FE8 RID: 4072 RVA: 0x00041E10 File Offset: 0x00040010
		public int Compare(TroopSelectionItemVM x, TroopSelectionItemVM y)
		{
			int num;
			if (y.Troop.Character.IsPlayerCharacter)
			{
				num = 1;
			}
			else if (y.Troop.Character.IsHero)
			{
				if (x.Troop.Character.IsPlayerCharacter)
				{
					num = -1;
				}
				else if (x.Troop.Character.IsHero)
				{
					num = y.Troop.Character.Level - x.Troop.Character.Level;
				}
				else
				{
					num = 1;
				}
			}
			else if (x.Troop.Character.IsPlayerCharacter || x.Troop.Character.IsHero)
			{
				num = -1;
			}
			else
			{
				num = y.Troop.Character.Level - x.Troop.Character.Level;
			}
			return num;
		}
	}
}
