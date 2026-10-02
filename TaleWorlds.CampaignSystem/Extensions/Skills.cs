using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000174 RID: 372
	public static class Skills
	{
		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001BDA RID: 7130 RVA: 0x000904DC File Offset: 0x0008E6DC
		public static MBReadOnlyList<SkillObject> All
		{
			get
			{
				return Campaign.Current.AllSkills;
			}
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000904E8 File Offset: 0x0008E6E8
		public static SkillObject GetSkill(int i)
		{
			return Skills.All[i];
		}
	}
}
