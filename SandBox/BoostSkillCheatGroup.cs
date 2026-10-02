using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SandBox
{
	// Token: 0x02000010 RID: 16
	public class BoostSkillCheatGroup : GameplayCheatGroup
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00003904 File Offset: 0x00001B04
		public override IEnumerable<GameplayCheatBase> GetCheats()
		{
			foreach (SkillObject skillObject in Skills.All)
			{
				yield return new BoostSkillCheatGroup.BoostSkillCheeat(skillObject);
			}
			List<SkillObject>.Enumerator enumerator = default(List<SkillObject>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000390D File Offset: 0x00001B0D
		public override TextObject GetName()
		{
			return new TextObject("{=SFn4UFd4}Boost Skill", null);
		}

		// Token: 0x0200011E RID: 286
		public class BoostSkillCheeat : GameplayCheatItem
		{
			// Token: 0x06000DC9 RID: 3529 RVA: 0x00062DF7 File Offset: 0x00060FF7
			public BoostSkillCheeat(SkillObject skillToBoost)
			{
				this._skillToBoost = skillToBoost;
			}

			// Token: 0x06000DCA RID: 3530 RVA: 0x00062E08 File Offset: 0x00061008
			public override void ExecuteCheat()
			{
				int num = 50;
				if (Hero.MainHero.GetSkillValue(this._skillToBoost) + num > 330)
				{
					num = 330 - Hero.MainHero.GetSkillValue(this._skillToBoost);
				}
				Hero.MainHero.HeroDeveloper.ChangeSkillLevel(this._skillToBoost, num, false);
			}

			// Token: 0x06000DCB RID: 3531 RVA: 0x00062E5F File Offset: 0x0006105F
			public override TextObject GetName()
			{
				return this._skillToBoost.GetName();
			}

			// Token: 0x040005E4 RID: 1508
			private readonly SkillObject _skillToBoost;
		}
	}
}
