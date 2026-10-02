using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000136 RID: 310
	public class DefaultNotablePowerModel : NotablePowerModel
	{
		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06001994 RID: 6548 RVA: 0x0007F169 File Offset: 0x0007D369
		public override int NotableDisappearPowerLimit
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x0007F170 File Offset: 0x0007D370
		public override ExplainedNumber CalculateDailyPowerChangeForHero(Hero hero, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			if (!hero.IsActive)
			{
				return explainedNumber;
			}
			if (hero.Power > (float)this.RegularNotableMaxPowerLevel)
			{
				this.CalculateDailyPowerChangeForInfluentialNotables(hero, ref explainedNumber);
			}
			this.CalculateDailyPowerChangePerPropertyOwned(hero, ref explainedNumber);
			if (hero.Issue != null)
			{
				this.CalculatePowerChangeFromIssues(hero, ref explainedNumber);
			}
			if (hero.IsArtisan)
			{
				explainedNumber.Add(-0.1f, this._propertyEffect, null);
			}
			if (hero.IsGangLeader)
			{
				explainedNumber.Add(-0.4f, this._propertyEffect, null);
			}
			if (hero.IsRuralNotable)
			{
				explainedNumber.Add(0.1f, this._propertyEffect, null);
			}
			if (hero.IsHeadman)
			{
				explainedNumber.Add(0.1f, this._propertyEffect, null);
			}
			if (hero.IsMerchant)
			{
				explainedNumber.Add(0.2f, this._propertyEffect, null);
			}
			if (hero.CurrentSettlement != null)
			{
				if (hero.CurrentSettlement.IsVillage && hero.CurrentSettlement.Village.Bound.IsCastle)
				{
					explainedNumber.Add(0.1f, this._propertyEffect, null);
				}
				if (hero.SupporterOf == hero.CurrentSettlement.OwnerClan)
				{
					this.CalculateDailyPowerChangeForAffiliationWithRulerClan(ref explainedNumber);
				}
			}
			return explainedNumber;
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06001996 RID: 6550 RVA: 0x0007F2A9 File Offset: 0x0007D4A9
		public override int RegularNotableMaxPowerLevel
		{
			get
			{
				return this.NotablePowerRanks[1].MinPowerValue;
			}
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0007F2BC File Offset: 0x0007D4BC
		private void CalculateDailyPowerChangePerPropertyOwned(Hero hero, ref ExplainedNumber explainedNumber)
		{
			int count = hero.OwnedAlleys.Count;
			explainedNumber.Add(0.1f * (float)count, this._propertyEffect, null);
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0007F2EA File Offset: 0x0007D4EA
		private void CalculateDailyPowerChangeForAffiliationWithRulerClan(ref ExplainedNumber explainedNumber)
		{
			explainedNumber.Add(0.2f, this._rulerClanEffect, null);
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0007F300 File Offset: 0x0007D500
		private void CalculateDailyPowerChangeForInfluentialNotables(Hero hero, ref ExplainedNumber explainedNumber)
		{
			float num = -1f * ((hero.Power - (float)this.RegularNotableMaxPowerLevel) / 500f);
			explainedNumber.Add(num, this._currentRankEffect, null);
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0007F336 File Offset: 0x0007D536
		private void CalculatePowerChangeFromIssues(Hero hero, ref ExplainedNumber explainedNumber)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectOfHero(DefaultIssueEffects.IssueOwnerPower, hero, ref explainedNumber);
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0007F353 File Offset: 0x0007D553
		public override TextObject GetPowerRankName(Hero hero)
		{
			return this.GetPowerRank(hero).Name;
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0007F361 File Offset: 0x0007D561
		public override float GetInfluenceBonusToClan(Hero hero)
		{
			return this.GetPowerRank(hero).InfluenceBonus;
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0007F370 File Offset: 0x0007D570
		private DefaultNotablePowerModel.NotablePowerRank GetPowerRank(Hero hero)
		{
			int num = 0;
			for (int i = 0; i < this.NotablePowerRanks.Length; i++)
			{
				if (hero.Power > (float)this.NotablePowerRanks[i].MinPowerValue)
				{
					num = i;
				}
			}
			return this.NotablePowerRanks[num];
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0007F3BC File Offset: 0x0007D5BC
		public override int GetInitialPower(Hero hero)
		{
			int num = 0;
			float randomFloat = MBRandom.RandomFloat;
			num += ((randomFloat < 0.2f) ? MBRandom.RandomInt((int)((float)(this.NotablePowerRanks[0].MinPowerValue + this.NotablePowerRanks[1].MinPowerValue) * 0.5f), this.NotablePowerRanks[1].MinPowerValue) : ((randomFloat < 0.8f) ? MBRandom.RandomInt(this.NotablePowerRanks[1].MinPowerValue, this.NotablePowerRanks[2].MinPowerValue) : MBRandom.RandomInt(this.NotablePowerRanks[2].MinPowerValue, (int)((float)this.NotablePowerRanks[2].MinPowerValue * 2f))));
			if ((hero.Occupation == Occupation.GangLeader || hero.Occupation == Occupation.Artisan || hero.Occupation == Occupation.RuralNotable || hero.Occupation == Occupation.Merchant || hero.Occupation == Occupation.Headman) && hero.HomeSettlement.IsVillage && hero.HomeSettlement.Village.Bound != null && hero.HomeSettlement.Village.Bound.IsCastle)
			{
				num += (int)(MBRandom.RandomFloat * 20f);
			}
			return num;
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0007F4F7 File Offset: 0x0007D6F7
		public override int GetInitialNotableSupporterCost(Hero hero)
		{
			return 20000 + 10000 * Clan.PlayerClan.SupporterNotables.Count;
		}

		// Token: 0x0400084A RID: 2122
		private DefaultNotablePowerModel.NotablePowerRank[] NotablePowerRanks = new DefaultNotablePowerModel.NotablePowerRank[]
		{
			new DefaultNotablePowerModel.NotablePowerRank(new TextObject("{=aTeuX4L0}Regular", null), 0, 0.05f),
			new DefaultNotablePowerModel.NotablePowerRank(new TextObject("{=nTETQEmy}Influential", null), 100, 0.1f),
			new DefaultNotablePowerModel.NotablePowerRank(new TextObject("{=UCpyo9hw}Powerful", null), 200, 0.15f)
		};

		// Token: 0x0400084B RID: 2123
		private TextObject _currentRankEffect = new TextObject("{=7j9uHxLM}Current Rank Effect", null);

		// Token: 0x0400084C RID: 2124
		private TextObject _militiaEffect = new TextObject("{=R1MaIgOb}Militia Effect", null);

		// Token: 0x0400084D RID: 2125
		private TextObject _rulerClanEffect = new TextObject("{=JE3RTqx5}Ruler Clan Effect", null);

		// Token: 0x0400084E RID: 2126
		private TextObject _propertyEffect = new TextObject("{=yDomN9L2}Property Effect", null);

		// Token: 0x020005C5 RID: 1477
		private struct NotablePowerRank
		{
			// Token: 0x06005177 RID: 20855 RVA: 0x001908B5 File Offset: 0x0018EAB5
			public NotablePowerRank(TextObject name, int minPowerValue, float influenceBonus)
			{
				this.Name = name;
				this.MinPowerValue = minPowerValue;
				this.InfluenceBonus = influenceBonus;
			}

			// Token: 0x04001906 RID: 6406
			public readonly TextObject Name;

			// Token: 0x04001907 RID: 6407
			public readonly int MinPowerValue;

			// Token: 0x04001908 RID: 6408
			public readonly float InfluenceBonus;
		}
	}
}
