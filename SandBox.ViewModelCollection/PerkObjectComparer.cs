using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ViewModelCollection;

namespace SandBox.ViewModelCollection
{
	// Token: 0x02000008 RID: 8
	public class PerkObjectComparer : IComparer<PerkObject>
	{
		// Token: 0x06000050 RID: 80 RVA: 0x000040B0 File Offset: 0x000022B0
		public int Compare(PerkObject x, PerkObject y)
		{
			int skillObjectTypeSortIndex = CampaignUIHelper.GetSkillObjectTypeSortIndex(x.Skill);
			int num = CampaignUIHelper.GetSkillObjectTypeSortIndex(y.Skill).CompareTo(skillObjectTypeSortIndex);
			if (num != 0)
			{
				return num;
			}
			return this.ResolveEquality(x, y);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000040EC File Offset: 0x000022EC
		private int ResolveEquality(PerkObject x, PerkObject y)
		{
			return x.RequiredSkillValue.CompareTo(y.RequiredSkillValue);
		}
	}
}
