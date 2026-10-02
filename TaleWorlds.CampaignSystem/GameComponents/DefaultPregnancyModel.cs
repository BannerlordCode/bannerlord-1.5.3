using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000148 RID: 328
	public class DefaultPregnancyModel : PregnancyModel
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x0008429C File Offset: 0x0008249C
		public override float PregnancyDurationInDays
		{
			get
			{
				return (float)((Campaign.Current.Options.AccelerationMode == GameAccelerationMode.Fast) ? 18 : 36);
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06001A34 RID: 6708 RVA: 0x000842B7 File Offset: 0x000824B7
		public override float MaternalMortalityProbabilityInLabor
		{
			get
			{
				return 0.015f;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x000842BE File Offset: 0x000824BE
		public override float StillbirthProbability
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001A36 RID: 6710 RVA: 0x000842C5 File Offset: 0x000824C5
		public override float DeliveringFemaleOffspringProbability
		{
			get
			{
				return 0.51f;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x000842CC File Offset: 0x000824CC
		public override float DeliveringTwinsProbability
		{
			get
			{
				return 0.03f;
			}
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x000842D3 File Offset: 0x000824D3
		private bool IsHeroAgeSuitableForPregnancy(Hero hero)
		{
			return hero.Age >= 18f && hero.Age <= 45f;
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x000842F4 File Offset: 0x000824F4
		public override float GetDailyChanceOfPregnancyForHero(Hero hero)
		{
			int num = hero.Children.Count + 1;
			float num2 = (float)(4 + 4 * hero.Clan.Tier);
			int count = hero.Clan.AliveLords.Count;
			float num3 = ((hero != Hero.MainHero && hero.Spouse != Hero.MainHero) ? Math.Min(1f, (2f * num2 - (float)count) / num2) : 1f);
			float num4 = (1.2f - (hero.Age - 18f) * 0.04f) / (float)(num * num) * 0.12f * num3;
			float num5 = ((hero.Spouse != null && this.IsHeroAgeSuitableForPregnancy(hero)) ? num4 : 0f);
			ExplainedNumber explainedNumber = new ExplainedNumber(num5, false, null);
			bool perkValue = hero.GetPerkValue(DefaultPerks.Charm.Virile);
			bool perkValue2 = hero.Spouse.GetPerkValue(DefaultPerks.Charm.Virile);
			if (perkValue || perkValue2)
			{
				explainedNumber.AddFactor(DefaultPerks.Charm.Virile.PrimaryBonus, DefaultPerks.Charm.Virile.Name);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x040008AF RID: 2223
		private const int MinPregnancyAge = 18;

		// Token: 0x040008B0 RID: 2224
		private const int MaxPregnancyAge = 45;
	}
}
