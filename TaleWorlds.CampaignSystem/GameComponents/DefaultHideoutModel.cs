using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000125 RID: 293
	public class DefaultHideoutModel : HideoutModel
	{
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x060018C2 RID: 6338 RVA: 0x000788D4 File Offset: 0x00076AD4
		public override CampaignTime HideoutHiddenDuration
		{
			get
			{
				return CampaignTime.Days(10f);
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x000788E0 File Offset: 0x00076AE0
		public override int CanAttackHideoutStartTime
		{
			get
			{
				return CampaignTime.SunSet;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060018C4 RID: 6340 RVA: 0x000788E7 File Offset: 0x00076AE7
		public override int CanAttackHideoutEndTime
		{
			get
			{
				return CampaignTime.SunRise;
			}
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x000788EE File Offset: 0x00076AEE
		public override float GetRogueryXpGainAsGhost()
		{
			return MBRandom.RandomFloatRanged(1000f, 1400f);
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x000788FF File Offset: 0x00076AFF
		public override float GetRogueryXpGainOnHideoutMissionEnd(bool isSucceeded)
		{
			return (float)(isSucceeded ? MBRandom.RandomInt(700, 1000) : MBRandom.RandomInt(225, 400));
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x00078928 File Offset: 0x00076B28
		public override float GetSendTroopsSuccessChance(Hideout hideout)
		{
			int skillValue = Hero.MainHero.GetSkillValue(DefaultSkills.Tactics);
			int skillValue2 = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
			return 0.3f + (float)(skillValue + skillValue2) / (325f + (float)skillValue + (float)skillValue2);
		}
	}
}
